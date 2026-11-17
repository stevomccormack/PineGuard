#!/usr/bin/env bash
# Hook: PreToolUse (Write, Edit)
# Purpose: Block edits to .sln/.slnx files to prevent accidental corruption.
#
# FORCE override (see hook-lib.sh):
#   once:  bash .claude/hooks/coordination.sh force sln-edit "<reason>"
#   owner, standing:  PINEGUARD_HOOK_FORCE=sln-edit  (ALLOW_SLN_EDIT=1 is still honoured)
#
# Input: JSON from stdin with session_id, tool_name, tool_input
# Exit 0: Allow
# Exit 2: Block

set -uo pipefail

HOOK_NAME="sln-edit"

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
source "$SCRIPT_DIR/hook-lib.sh" 2>/dev/null || exit 0

hook_read_input
FILE_PATH=$(hook_field "tool_input.file_path")

if [ -z "$FILE_PATH" ]; then
  exit 0
fi

case "$FILE_PATH" in
  *.sln|*.slnx)
    if [ "${ALLOW_SLN_EDIT:-0}" = "1" ]; then
      hook_force_log "$HOOK_NAME" "env:ALLOW_SLN_EDIT" "$FILE_PATH"
      exit 0
    fi
    if hook_force_requested "$HOOK_NAME" "" "" "$FILE_PATH"; then
      exit 0
    fi
    echo "BLOCKED: Direct edits to solution files (*.sln/*.slnx) are not allowed." >&2
    echo "  Use 'dotnet sln' commands to modify the solution safely." >&2
    echo "" >&2
    hook_force_hint "$HOOK_NAME"
    exit 2
    ;;
esac

exit 0
