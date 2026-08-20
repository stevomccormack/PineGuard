#!/usr/bin/env bash
# Hook: PostToolUse (Write, Edit)
# Purpose: Remind about spec compliance after TestData file edits.
#          Does NOT block — just logs reminders.
#
# Input: JSON from stdin with tool_name, tool_input, tool_result
# Exit 0: Always (PostToolUse hooks are advisory)

set -uo pipefail

# Extract file_path from stdin JSON without python (pure bash/sed)
INPUT=$(cat)
FILE_PATH=$(echo "$INPUT" | sed -n 's/.*"file_path" *: *"\([^"]*\)".*/\1/p' | head -1)

if [ -z "$FILE_PATH" ]; then
  exit 0
fi

case "$FILE_PATH" in
  *TestData.cs)
    echo "TestData spec check: Expected (not ExpectedReturn), camelCase tuples, single-line cases, single-line fixtures." >&2
    ;;
  *Tests.cs)
    echo "Tests spec check: BehavesAsExpected/ThrowsAsExpected naming, AAA comments, nested Op Groups." >&2
    ;;
esac

exit 0
