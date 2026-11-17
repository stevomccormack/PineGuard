#!/usr/bin/env bash
# Hook: PostToolUse (Write, Edit)
# Purpose: Remind about spec compliance after fixture, TestData and Tests file edits.
#          Does NOT block — just logs reminders. The specs are the authority; this only points at them.
#
# Input: JSON from stdin with tool_name, tool_input, tool_result
# Exit 0: Always (PostToolUse hooks are advisory)

set -uo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
source "$SCRIPT_DIR/hook-lib.sh" 2>/dev/null || exit 0

hook_read_input
FILE_PATH=$(hook_field "tool_input.file_path")

if [ -z "$FILE_PATH" ]; then
  exit 0
fi

SPECS="Read in full before editing: docs/ai/rules/testing.md, docs/ai/specs/testing/unit-test.md, docs/ai/specs/testing/fixture.md and the layer's unit-test.md. Mirror the nearest sibling file exactly."

case "$FILE_PATH" in
  *Fixtures.cs|*Fixtures.*.cs)
    echo "Fixture spec check: one group per member; ValidScenarios/InvalidScenarios/AllScenarios (edge arrays only for boundary rules, fixture.md §2); camelCase tuple elements named after the parameters; single-line entries; structural comments only. $SPECS" >&2
    ;;
  *TestData.cs|*TestData.*.cs)
    echo "TestData spec check: dataset names and shape per the layer spec (unit-test.md §4.1); nameof(F.Group.Field) names, no magic strings; datasets before records; two-line records; single-line cases. Update docs/ai/specs/testing/gold-standard.md if a test class or group was added. $SPECS" >&2
    ;;
  *Tests.cs|*Tests.*.cs)
    echo "Tests spec check: flat test class (no nested classes); one Xxx_BehavesAsExpected per TestData group, in the same order; Arrange/Act/Assert markers; one member per Act. $SPECS" >&2
    ;;
esac

exit 0
