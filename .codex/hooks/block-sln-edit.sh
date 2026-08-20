#!/usr/bin/env bash
# Hook: PreToolUse (Write, Edit)
# Purpose: Block edits to .sln/.slnx files to prevent accidental corruption.
#          Set ALLOW_SLN_EDIT=1 to override.
#
# Input: JSON from stdin with tool_name, tool_input
# Exit 0: Allow
# Exit 2: Block

set -uo pipefail

if [ "${ALLOW_SLN_EDIT:-0}" = "1" ]; then
  exit 0
fi

INPUT=$(cat)
FILE_PATH=$(echo "$INPUT" | sed -n 's/.*"file_path" *: *"\([^"]*\)".*/\1/p' | head -1)

if [ -z "$FILE_PATH" ]; then
  exit 0
fi

case "$FILE_PATH" in
  *.sln|*.slnx)
    echo "BLOCKED: Direct edits to solution files (*.sln/*.slnx) are not allowed." >&2
    echo "  Use 'dotnet sln' commands to modify the solution safely." >&2
    echo "  Override: set ALLOW_SLN_EDIT=1 if you know what you're doing." >&2
    exit 2
    ;;
esac

exit 0
