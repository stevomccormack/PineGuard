#!/usr/bin/env bash
# Hook: PreToolUse (Bash)
# Purpose: Take the shared "dotnet-ops" lock before any dotnet, coverage or sonar command, so two
#          sessions (Desktop/VS/JetBrains/Terminal/subagent) never build or test at the same time.
#          Blocks with a clear message when another session holds it.
#
# Matched commands (case-insensitive):
#   dotnet test | build | publish | pack | restore | format | clean | run
#   Run-CodeCoverage.ps1 / New-CoverageReport.ps1 / Test-Coverage.ps1
#   Run-SonarScanner.ps1 | sonar-scanner / sonarscanner
#
# A wrapper script that runs dotnet inside it is not seen here. Run it as
#   bash .claude/hooks/coordination.sh with-lock -- bash <script>
# so the lock covers the whole script.
#
# FORCE override (see hook-lib.sh): trailing comment  # FORCE-LOCK
#   or once:  bash .claude/hooks/coordination.sh force dotnet-lock "<reason>"
#   A lock left by a dead session:  bash .claude/hooks/coordination.sh lock-release --force "<reason>"
#
# Input:  JSON on stdin { session_id, tool_name, tool_input: { command } }
# Exit 0: allow the tool call (lock acquired, not a heavy command, or overridden)
# Exit 2: block the tool call (lock held by another session)

set -uo pipefail

HOOK_NAME="dotnet-lock"
FORCE_MARKER="FORCE-LOCK"

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
# Fail open if the shared scripts are missing (first-time setup, etc.)
source "$SCRIPT_DIR/hook-lib.sh" 2>/dev/null || exit 0

hook_read_input
COMMAND=$(hook_field "tool_input.command")

# Only gate on heavy / potentially conflicting commands
if ! printf '%s' "$COMMAND" | grep -qiE \
    '(dotnet[[:space:]]+(test|build|publish|pack|restore|format|clean|run)([[:space:]]|$)|Run-CodeCoverage|New-CoverageReport|Test-Coverage|Run-SonarScanner|sonar-scanner|sonarscanner)'; then
  exit 0
fi

# The command is already running under coordination.sh with-lock: the lock is held for it.
if printf '%s' "$COMMAND" | grep -qE 'coordination\.sh[[:space:]]+with-lock'; then
  exit 0
fi

source "$SCRIPT_DIR/coordination.sh" 2>/dev/null || exit 0

# Store what command we're running in the lock metadata
export LOCK_COMMAND="${COMMAND:0:120}"

if acquire_lock "dotnet-ops"; then
  write_status "running" "${COMMAND:0:80}"
  exit 0
fi

if hook_force_requested "$HOOK_NAME" "$FORCE_MARKER" "$COMMAND" "$COMMAND"; then
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
if [ "$HOLDER" = "$SESSION_ID" ]; then
  echo "   That is this session: another command of yours (a subagent, a background task) is still running." >&2
fi
echo "   Expires in ~${REMAINING}s if nothing releases it sooner — retry when it completes." >&2
echo "" >&2
echo "   To check all sessions: bash .claude/hooks/coordination.sh lock-status" >&2
echo "" >&2
hook_force_hint "$HOOK_NAME" "$FORCE_MARKER"
echo "   • holder is dead:  bash .claude/hooks/coordination.sh lock-release --force \"<reason>\"" >&2

exit 2
