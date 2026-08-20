#!/usr/bin/env bash
# Hook: PreToolUse (Write)
# Purpose: Block creation of temp/output files in the project root.
#          Suggests structured subdirectory paths based on file type:
#            logs/YYYY-MM-DD/        — log/txt files
#            artifacts/tmp/          — temp scripts/helpers
#            artifacts/<tool>/html/  — HTML reports
#            artifacts/<tool>/report/— SARIF/JSON reports
#            artifacts/test-results/YYYY-MM-DD/ — .trx test results
#            artifacts/code-coverage/ — coverage XML
#
# Input: JSON from stdin with tool_name and tool_input
# Exit 0: Allow the write
# Exit 2: Block the write (stderr message shown to user)

set -uo pipefail

# Extract file_path from stdin JSON without python (pure bash/sed)
INPUT=$(cat)
FILE_PATH=$(echo "$INPUT" | sed -n 's/.*"file_path" *: *"\([^"]*\)".*/\1/p' | head -1)

# If we can't parse, allow (fail open)
if [ -z "$FILE_PATH" ]; then
  exit 0
fi

# Normalize path separators for Windows
FILE_PATH=$(echo "$FILE_PATH" | tr '\\' '/')

# Get just the filename and its parent directory
FILENAME=$(basename "$FILE_PATH")
# Get the relative path from project root (strip drive letter and project path)
REL_PATH=$(echo "$FILE_PATH" | sed -E 's|^.*/PineGuard/||' | sed -E 's|^[A-Za-z]:/[^/]*/||')

# Check if file is in the project root (no directory separator in relative path)
DIR_PART=$(dirname "$REL_PATH")

# Allow files in known source directories
case "$REL_PATH" in
  src/*|tests/*|docs/*|tools/*|.claude/*|.agent/*|.github/*|.vscode/*|artifacts/*|logs/*|diagnostics/*)
    exit 0
    ;;
esac

# Allow known root config/source files by extension
case "$FILENAME" in
  *.cs|*.csproj|*.sln|*.md|*.json|*.xml|*.yaml|*.yml|*.ps1|*.psm1|*.psd1|*.sh|*.editorconfig|*.gitignore|*.gitattributes|*.props|*.targets|*.ruleset|*.DotSettings)
    exit 0
    ;;
  # Allow specific known root files
  CLAUDE.md|AGENTS.md|LICENSE|LICENSE.md|README.md|Directory.Build.props|Directory.Packages.props|nuget.config|global.json|.cursorrules)
    exit 0
    ;;
esac

# If we got here, the file is in the project root and not a recognized source file
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
    echo "  Move to: artifacts/code-coverage/xplat/testresults/<Project>/$FILENAME" >&2
    ;;
  *.html)
    echo "  Move to: artifacts/<tool>/html/$FILENAME" >&2
    ;;
  *.sarif)
    echo "  Move to: artifacts/<tool>/report/$FILENAME" >&2
    ;;
  *.ps1|*.sh|*.py)
    echo "  Move to: \$TEMP (system temp) or artifacts/tmp/$FILENAME" >&2
    ;;
  *)
    echo "  Move to: artifacts/<category>/$FILENAME or logs/$TODAY/$FILENAME" >&2
    ;;
esac

exit 2
