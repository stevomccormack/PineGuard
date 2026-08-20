#!/usr/bin/env bash
# Hook: PostToolUse (Write, Edit)
# Purpose: Remind about formatting after C# file edits.
#          Does NOT block — just logs a reminder.
#
# Input: JSON from stdin with tool_name, tool_input, tool_result
# Exit 0: Always (PostToolUse hooks are advisory)

set -uo pipefail

# Extract file_path from stdin JSON without python (pure bash/sed)
INPUT=$(cat)
FILE_PATH=$(echo "$INPUT" | sed -n 's/.*"file_path" *: *"\([^"]*\)".*/\1/p' | head -1)

# If we can't parse, exit silently
if [ -z "$FILE_PATH" ]; then
  exit 0
fi

# Only care about C# source files
case "$FILE_PATH" in
  *.cs)
    echo "Reminder: Run 'dotnet format' to ensure editorconfig compliance after editing C# files." >&2
    ;;
esac

exit 0
