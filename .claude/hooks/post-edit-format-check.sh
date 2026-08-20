#!/usr/bin/env bash
# Hook: PostToolUse (Write, Edit)
# Purpose: Remind about formatting after C# file edits, and flag a leading UTF-8
#          BOM on markdown files (a BOM before `---` stops YAML frontmatter from
#          parsing, silently disabling a skill, command, or rule).
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
  *.md)
    if [ -f "$FILE_PATH" ] && [ "$(head -c 3 "$FILE_PATH" | od -An -tx1 | tr -d ' \n')" = "efbbbf" ]; then
      echo "Warning: $FILE_PATH starts with a UTF-8 BOM. Re-save it as UTF-8 without BOM — a BOM before '---' stops the YAML frontmatter from parsing." >&2
    fi
    ;;
esac

exit 0
