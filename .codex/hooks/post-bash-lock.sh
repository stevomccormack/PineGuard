#!/usr/bin/env bash
# Hook: PostToolUse (Bash)
# Purpose: Release the "dotnet-ops" lock after a heavy command completes
#          and update status back to idle.
#
# Release is matched on the session id and on the command text recorded by pre-bash-lock.sh, so it
# works even on a surface that gives hooks no stable session id. A command started in the
# background returns at once; its lock is then released early, which is the lesser evil — use
# coordination.sh with-lock for anything long-running in the background.
#
# Input:  JSON on stdin { session_id, tool_name, tool_input, tool_response }
# Exit 0: always (PostToolUse hooks are advisory — never block)

set -uo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
source "$SCRIPT_DIR/hook-lib.sh" 2>/dev/null || exit 0

hook_read_input
COMMAND=$(hook_field "tool_input.command")

# Only act on heavy commands that may have acquired the lock
if ! printf '%s' "$COMMAND" | grep -qiE \
    '(dotnet[[:space:]]+(test|build|publish|pack|restore|format|clean|run)([[:space:]]|$)|Run-CodeCoverage|New-CoverageReport|Test-Coverage|Run-SonarScanner|sonar-scanner|sonarscanner)'; then
  exit 0
fi

# with-lock releases its own lock when the command exits.
if printf '%s' "$COMMAND" | grep -qE 'coordination\.sh[[:space:]]+with-lock'; then
  exit 0
fi

source "$SCRIPT_DIR/coordination.sh" 2>/dev/null || exit 0

export LOCK_COMMAND="${COMMAND:0:120}"
release_lock "dotnet-ops"
write_status "idle" ""

exit 0
