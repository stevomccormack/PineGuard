#!/usr/bin/env bash
# Hook: Stop
# Purpose: Housekeeping at the end of a turn. Stop fires every time Claude finishes responding, not
#          when the session ends, and background work of the same session (a subagent mid-build) can
#          still be running. So this does NOT release the session's locks or clear its status — that
#          is on-session-end.sh. It only drops what has already expired.
#
# Input:  JSON on stdin (stop event details)
# Exit 0: always

set -uo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
source "$SCRIPT_DIR/hook-lib.sh" 2>/dev/null || exit 0
hook_read_input
source "$SCRIPT_DIR/coordination.sh" 2>/dev/null || exit 0

prune_run_dir

exit 0
