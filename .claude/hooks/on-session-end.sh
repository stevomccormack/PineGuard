#!/usr/bin/env bash
# Hook: SessionEnd
# Purpose: Clean up this session's locks and status when the session ends
#          so stale entries don't linger for other sessions to see.
#
# Input:  JSON on stdin (session end details, including session_id)
# Exit 0: always

set -uo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
source "$SCRIPT_DIR/hook-lib.sh" 2>/dev/null || exit 0
hook_read_input
source "$SCRIPT_DIR/coordination.sh" 2>/dev/null || exit 0

release_all_locks
clear_status
prune_run_dir

exit 0
