#!/bin/sh
set -eu

config_file="${HETS_CONFIG_FILE:-/opt/app-root/configmap/appsettings.json}"
if [ ! -r "$config_file" ]; then
  echo "Required application configuration is not readable: $config_file" >&2
  exit 1
fi
cp "$config_file" /app/appsettings.json
exec dotnet /app/HetsApi.dll
