#!/usr/bin/env bash

set -uo pipefail

max_attempts=3
attempt=1

while true; do
  log_file="$(mktemp)"

  set +e
  "$@" 2>&1 | tee "$log_file"
  command_status=${PIPESTATUS[0]}
  set -e

  if [ "$command_status" -eq 0 ]; then
    rm -f "$log_file"
    exit 0
  fi

  if [ "$attempt" -ge "$max_attempts" ] ||
    ! grep -Eqi \
      'i/o timeout|TLS handshake timeout|connection reset by peer|Unable to connect to the server|temporarily unavailable' \
      "$log_file"; then
    rm -f "$log_file"
    exit "$command_status"
  fi

  rm -f "$log_file"
  retry_delay=$((attempt * 10))
  echo "Transient OpenShift connection failure; retrying in ${retry_delay}s (attempt $((attempt + 1))/${max_attempts})."
  sleep "$retry_delay"
  attempt=$((attempt + 1))
done
