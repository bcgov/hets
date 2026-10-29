#!/bin/sh
set -eu

: "${REACT_APP_SSO_HOST:?REACT_APP_SSO_HOST is required}"
: "${REACT_APP_SSO_REALM:?REACT_APP_SSO_REALM is required}"
: "${REACT_APP_SSO_CLIENT:?REACT_APP_SSO_CLIENT is required}"
: "${HETS_DEPLOY_SUFFIX:?HETS_DEPLOY_SUFFIX is required}"

# Only a DNS-safe suffix may become part of the nginx upstream name.
case "$HETS_DEPLOY_SUFFIX" in
  *[!a-z0-9-]*) echo 'Invalid HETS_DEPLOY_SUFFIX' >&2; exit 1 ;;
esac

# JSON encoding keeps quotes, newlines and backslashes valid JavaScript.
jq -nr \
  --arg host "$REACT_APP_SSO_HOST" \
  --arg realm "$REACT_APP_SSO_REALM" \
  --arg client "$REACT_APP_SSO_CLIENT" \
  --arg api "${REACT_APP_API_HOST:-}" \
  --arg commit "${OPENSHIFT_BUILD_COMMIT:-}" \
  '{RUNTIME_REACT_APP_SSO_HOST: $host, RUNTIME_REACT_APP_SSO_REALM: $realm,
    RUNTIME_REACT_APP_SSO_CLIENT: $client, RUNTIME_REACT_APP_API_HOST: $api,
    RUNTIME_OPENSHIFT_BUILD_COMMIT: $commit}
   | to_entries[] | "window.\(.key)=\(.value | tojson);"' \
  > /usr/share/nginx/html/js/env.config.js

envsubst '${HETS_DEPLOY_SUFFIX}' < /opt/hets/nginx.conf.tmpl > /etc/nginx/nginx.conf
exec nginx -g 'daemon off;'
