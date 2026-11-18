#!/usr/bin/env bash
# Hook: PreToolUse (Write)
# Purpose: Block creation of temp/output files in the project root.
#          Suggests structured subdirectory paths based on file type:
#            logs/YYYY-MM-DD/        — log/txt files
#            artifacts/tmp/          — temp scripts/helpers
#            artifacts/<tool>/report/— HTML/SARIF/JSON reports
#            artifacts/test-results/YYYY-MM-DD/ — .trx test results
#            artifacts/code-coverage/ — coverage XML
#
# Scope: only a file written directly into the project root. A file outside the project (a session
# scratchpad, the system temp directory) and a file in any subdirectory are not this hook's concern.
#
# FORCE override (see hook-lib.sh):
#   once:  bash .claude/hooks/coordination.sh force output-dirs "<reason>"
#
# Input: JSON from stdin with session_id, tool_name and tool_input
# Exit 0: Allow the write
# Exit 2: Block the write (stderr message shown to user)

set -uo pipefail

HOOK_NAME="output-dirs"

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
source "$SCRIPT_DIR/hook-lib.sh" 2>/dev/null || exit 0

hook_read_input
FILE_PATH=$(hook_field "tool_input.file_path")

# If we can't parse, allow (fail open)
if [ -z "$FILE_PATH" ]; then
  exit 0
fi

# Normalize path separators and drive-letter case for Windows
normalize() {
  local path
  path=$(printf '%s' "$1" | tr '\\' '/')
  path="${path%/}"
  case "$path" in
    [A-Za-z]:/*) path="$(printf '%s' "${path:0:1}" | tr '[:upper:]' '[:lower:]')${path:1}" ;;
  esac
  printf '%s' "$path"
}

FILE_PATH=$(normalize "$FILE_PATH")
ROOT=$(normalize "$HOOK_ROOT_DIR")
FILENAME=$(basename "$FILE_PATH")

# Relative paths are relative to the project root.
case "$FILE_PATH" in
  /*|[A-Za-z]:/*) ;;
  *) FILE_PATH="$ROOT/${FILE_PATH#./}" ;;
esac

# Outside the project: not ours to police.
case "$FILE_PATH" in
  "$ROOT"/*) REL_PATH="${FILE_PATH#"$ROOT"/}" ;;
  *) exit 0 ;;
esac

# In a subdirectory: allowed. Only the root itself is protected.
case "$REL_PATH" in
  */*) exit 0 ;;
esac

# Allow known root config/source files by extension or name
case "$FILENAME" in
  *.cs|*.csproj|*.sln|*.slnx|*.md|*.json|*.xml|*.yaml|*.yml|*.ps1|*.psm1|*.psd1|*.sh|*.props|*.targets|*.ruleset|*.DotSettings|*.config|*.toml)
    exit 0
    ;;
  .editorconfig|.gitignore|.gitattributes|.node-version|.nvmrc|.npmrc|.dockerignore|.globalconfig)
    exit 0
    ;;
  CLAUDE.md|AGENTS.md|LICENSE|LICENSE.md|README.md|Directory.Build.props|Directory.Packages.props|global.json)
    exit 0
    ;;
esac

if hook_force_requested "$HOOK_NAME" "" "" "$REL_PATH"; then
  exit 0
fi

# The file is in the project root and not a recognized source file.
# Block it and suggest a structured location based on file type
TODAY=$(date +%Y-%m-%d)

echo "BLOCKED: Files must not be created in the project root." >&2
echo "  File: $FILENAME" >&2

case "$FILENAME" in
  *.log|*.txt)
    echo "  Move to: logs/$TODAY/$FILENAME" >&2
    ;;
  *.trx)
    echo "  Move to: artifacts/test-results/$TODAY/$FILENAME" >&2
    ;;
  *.cobertura.xml|*.coverage|*.coveragexml)
    echo "  Move to: artifacts/code-coverage/coverlet/<scope>/testresults/<Project>/$FILENAME" >&2
    ;;
  *.html)
    echo "  Move to: artifacts/<tool>/report/$FILENAME" >&2
    ;;
  *.sarif)
    echo "  Move to: artifacts/<tool>/report/$FILENAME" >&2
    ;;
  *.py)
    echo "  Move to: \$TEMP (system temp) or artifacts/tmp/$FILENAME" >&2
    ;;
  *)
    echo "  Move to: artifacts/<category>/$FILENAME or logs/$TODAY/$FILENAME" >&2
    ;;
esac
echo "" >&2
hook_force_hint "$HOOK_NAME"

exit 2
