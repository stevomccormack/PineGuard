#!/usr/bin/env bash
# Hook: PostToolUse (Bash)
# Purpose: Release the "dotnet-ops" lock after a heavy command completes
#          and update status back to idle.
#
# Input:  JSON on stdin { tool_name, tool_input, tool_response }
# Exit 0: always (PostToolUse hooks are advisory — never block)

set -uo pipefail

# Parse the command from stdin JSON without python (pure bash/sed)
INPUT=$(cat)
COMMAND=$(echo "$INPUT" | sed -n 's/.*"command" *: *"\([^"]*\)".*/\1/p' | head -1)

# Only act on heavy commands that may have acquired the lock
if ! echo "$COMMAND" | grep -qiE \
    '(dotnet[[:space:]]+(test|build|publish)|Run-CodeCoverage|Run-SonarScanner|sonar-scanner|sonarscanner)'; then
  exit 0
fi

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
source "$SCRIPT_DIR/coordination.sh" 2>/dev/null || exit 0

release_lock "dotnet-ops"
write_status "idle" ""

exit 0
