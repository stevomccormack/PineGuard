#!/usr/bin/env bash
# Hook: Stop
# Purpose: Clean up this session's locks and status when Claude exits
#          so stale entries don't linger for other sessions to see.
#
# Input:  JSON on stdin (stop event details)
# Exit 0: always

set -uo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
source "$SCRIPT_DIR/coordination.sh" 2>/dev/null || exit 0

release_all_locks
clear_status

exit 0
