#!/usr/bin/env bash
set -euo pipefail

component="${1:?Usage: container-smoke.sh api|client}"
case "$component" in api|client) ;; *) exit 1 ;; esac
docker info > /dev/null
test_directory="$(mktemp -d)"
prefix="hets-smoke-$(basename "$test_directory" | tr '[:upper:]' '[:lower:]' | tr -cd 'a-z0-9-')"
network="$prefix-network"
app="$prefix-app"
dependency="$prefix-dependency"
cleanup() {
  result=$?
  if [[ "$result" != 0 ]]; then docker logs "$app" 2>&1 || true; fi
  docker rm -fv "$app" "$dependency" > /dev/null 2>&1 || true
  docker network rm "$network" > /dev/null 2>&1 || true
  # Generated diagnostics contain only credentials for an ephemeral database.
  exit "$result"
}
trap cleanup EXIT
docker network create "$network" > /dev/null

if [[ "$component" == api ]]; then
  docker run -d --name "$dependency" --network "$network" \
    --network-alias hets-db-smoke \
    -e POSTGRES_PASSWORD=ephemeral-smoke-only -e POSTGRES_DB=hets \
    postgres:16-alpine > /dev/null
  for attempt in {1..60}; do
    if docker exec "$dependency" pg_isready -U postgres -d hets > /dev/null 2>&1; then break; fi
    sleep 1
  done
  docker exec "$dependency" pg_isready -U postgres -d hets
  jq '.Serilog.WriteTo = [{"Name":"Console"}]' Server/HetsApi/appsettings.json \
    > "$test_directory/appsettings.json"
  chmod 755 "$test_directory"
  chmod 644 "$test_directory/appsettings.json"
  docker run -d --name "$app" --network "$network" --user 1001230000:0 \
    -p 127.0.0.1::8080 \
    --mount "type=bind,src=$test_directory/appsettings.json,dst=/opt/app-root/configmap/appsettings.json,readonly" \
    --tmpfs /uploads:rw,mode=0777 --tmpfs /reports:rw,mode=0777 \
    -e 'ConnectionStrings__HETS=Host=hets-db-smoke;Database=hets;Username=postgres;Password=ephemeral-smoke-only' \
    -e JWT__Authority=https://example.invalid -e JWT__Audience=smoke \
    -e UploadPath=/uploads/ -e ReportsPath=/reports/ \
    hets-smoke/api:test > /dev/null
else
  docker run -d --name "$dependency" --network "$network" \
    --network-alias hets-api-smoke python:3.13-alpine \
    python -u -c 'from http.server import BaseHTTPRequestHandler, HTTPServer
class Handler(BaseHTTPRequestHandler):
 def do_GET(self):
  self.send_response(200); self.end_headers(); self.wfile.write(self.path.encode())
HTTPServer(("0.0.0.0", 8080), Handler).serve_forever()' > /dev/null
  docker run -d --name "$app" --network "$network" --user 1001230000:0 \
    -p 127.0.0.1::8080 \
    -e 'REACT_APP_SSO_HOST=https://sso.example.test/auth?x="quoted"' \
    -e REACT_APP_SSO_REALM=smoke -e REACT_APP_SSO_CLIENT=smoke \
    -e REACT_APP_API_HOST=hets.example.test -e HETS_DEPLOY_SUFFIX=-smoke \
    hets-smoke/client:test > /dev/null
fi

address="http://$(docker port "$app" 8080/tcp)"
healthy=false
for attempt in {1..90}; do
  if curl --fail --silent "$address/healthz" > /dev/null; then healthy=true; break; fi
  sleep 1
done
test "$healthy" = true
test "$(docker exec "$app" id -u)" = 1001230000

if [[ "$component" == client ]]; then
  curl --fail --silent "$address/owners/123" | grep -q '<div id="root">'
  curl --fail --silent "$address/js/env.config.js" --output "$test_directory/env.config.js"
  node --input-type=module - "$test_directory/env.config.js" <<'JS'
import { readFileSync } from 'node:fs';
import vm from 'node:vm';
import assert from 'node:assert/strict';
const context = { window: {} };
vm.runInNewContext(readFileSync(process.argv[2], 'utf8'), context);
assert.equal(context.window.RUNTIME_REACT_APP_SSO_HOST, 'https://sso.example.test/auth?x="quoted"');
assert.equal(context.window.RUNTIME_REACT_APP_SSO_REALM, 'smoke');
JS
  test "$(curl --fail --silent "$address/api/ping")" = /api/ping
  test "$(curl --fail --silent "$address/swagger/v1/swagger.json")" = /swagger/v1/swagger.json
fi
echo "$component: health and runtime checks passed with arbitrary UID 1001230000."
