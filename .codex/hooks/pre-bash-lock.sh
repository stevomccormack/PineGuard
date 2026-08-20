#!/usr/bin/env bash
# Hook: PreToolUse (Bash)
# Purpose: Acquire the shared "dotnet-ops" lock before running any dotnet,
#          coverage, or sonar command. Blocks the tool with a clear message
#          if another session (Desktop/VS/JetBrains/Terminal/subagent) holds it.
#
# Matched commands (case-insensitive):
#   dotnet test | dotnet build | dotnet publish
#   Run-CodeCoverage.ps1
#   Run-SonarScanner.ps1
#   sonar-scanner / sonarscanner
#
# Input:  JSON on stdin { tool_name, tool_input: { command } }
# Exit 0: allow the tool call (lock acquired or not a heavy command)
# Exit 2: block the tool call (lock held by another session)

set -uo pipefail

# Parse the command from stdin JSON without python (pure bash/sed)
INPUT=$(cat)
COMMAND=$(echo "$INPUT" | sed -n 's/.*"command" *: *"\([^"]*\)".*/\1/p' | head -1)

# Only gate on heavy / potentially conflicting commands
if ! echo "$COMMAND" | grep -qiE \
    '(dotnet[[:space:]]+(test|build|publish)|Run-CodeCoverage|Run-SonarScanner|sonar-scanner|sonarscanner)'; then
  exit 0
fi

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
# Fail open if coordination.sh is missing (first-time setup, etc.)
source "$SCRIPT_DIR/coordination.sh" 2>/dev/null || exit 0

# Store what command we're running in the lock metadata
export LOCK_COMMAND="${COMMAND:0:120}"

if acquire_lock "dotnet-ops" 300; then
  write_status "running" "${COMMAND:0:80}"
  exit 0
fi

# Lock is held — show who has it and how long remains
HOLDER=$(cat "$LOCK_DIR/dotnet-ops.lock/session"  2>/dev/null || echo "unknown")
CMD=$(cat    "$LOCK_DIR/dotnet-ops.lock/command"   2>/dev/null || echo "unknown")
EXPIRES=$(cat "$LOCK_DIR/dotnet-ops.lock/expires"  2>/dev/null || echo 0)
NOW=$(date +%s 2>/dev/null || echo 0)
REMAINING=$((EXPIRES - NOW))

echo "⚠  dotnet-ops lock held by session $HOLDER" >&2
echo "   Running: ${CMD:0:80}" >&2
if [ "$REMAINING" -gt 0 ]; then
  echo "   Expires in ~${REMAINING}s — retry after it completes." >&2
else
  echo "   Lock appears stale (past expiry). Run: bash .claude/hooks/coordination.sh lock-release" >&2
fi
echo "" >&2
echo "   To check all sessions: bash .claude/hooks/coordination.sh lock-status" >&2

exit 2
