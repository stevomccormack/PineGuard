#!/usr/bin/env bash
# coordination.sh — Agent coordination: locks, status, messaging
#
# USAGE (as CLI, called by agents):
#   coordination.sh status "message"        # write status for this session
#   coordination.sh status-clear            # clear status for this session
#   coordination.sh lock-status             # print all active locks + statuses
#   coordination.sh lock-release            # release all locks held by this session
#   coordination.sh lock-release --force    # FORCE: release every lock, whoever holds it (logged)
#   coordination.sh with-lock -- <command>  # run any command (a wrapper script, a gate) under the
#                                           # dotnet-ops lock; released when it exits, however it exits
#   coordination.sh force <hook|all> "<reason>"  # FORCE: one-use override for the next blocked call of
#                                           # a hook (destructive-guard, dotnet-lock, output-dirs, sln-edit)
#   coordination.sh prune                   # drop expired locks, tokens and day-old status entries
#   coordination.sh send <to> <subject> <body>  # send a message to another session
#   coordination.sh messages                # read + clear inbox for this session
#
# USAGE (as library, sourced by other hooks):
#   source coordination.sh
#   acquire_lock "dotnet-ops" 300   # returns 0=acquired 1=held
#   release_lock "dotnet-ops"
#   write_status "running" "dotnet test MustClauses"
#
# SESSION ID: the hook payload's session_id, else $CLAUDE_CODE_SESSION_ID, else $CLAUDE_SESSION_ID, else
# $PPID (see hook_session_id in hook-lib.sh). The first three name the same session from a hook and
# from a shell, so the hook that takes a lock and the hook or CLI call that releases it agree. $PPID
# alone does not: each hook invocation has its own parent, which used to leave every lock held until
# it expired.

set -uo pipefail

# ── Runtime directories ──────────────────────────────────────────────────────

COORD_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
source "$COORD_DIR/hook-lib.sh"

ROOT_DIR="$HOOK_ROOT_DIR"
RUN_DIR="$HOOK_RUN_DIR"
LOCK_DIR="$RUN_DIR/locks"
STATUS_DIR="$RUN_DIR/status"
MSG_DIR="$RUN_DIR/messages"

ensure_dirs() {
  mkdir -p "$LOCK_DIR" "$STATUS_DIR" "$MSG_DIR" 2>/dev/null || true
}

# ── Session identity ──────────────────────────────────────────────────────────

SESSION_ID="$(hook_session_id)"

# How long a lock lives if nothing releases it (a killed command, a restarted container). The normal
# path releases it the moment the command ends, so this only bounds the damage of a crash.
LOCK_TIMEOUT_SECONDS="${PINEGUARD_LOCK_TIMEOUT:-600}"

# ── Lock management ───────────────────────────────────────────────────────────

# acquire_lock <name> [timeout_seconds]
# Returns 0 if lock acquired, 1 if held by another session.
# Writes lock details to $LOCK_DIR/<name>.lock/
acquire_lock() {
  local name="$1"
  local timeout="${2:-$LOCK_TIMEOUT_SECONDS}"
  local lock_path="$LOCK_DIR/${name}.lock"

  ensure_dirs

  # Steal stale locks (past their expiry timestamp)
  if [ -d "$lock_path" ]; then
    local expires now
    expires=$(cat "$lock_path/expires" 2>/dev/null || echo 0)
    now=$(date +%s 2>/dev/null || echo 9999999999)
    if [ "$now" -gt "$expires" ]; then
      rm -rf "$lock_path" 2>/dev/null || true
    fi
  fi

  # Atomic acquire via mkdir (NTFS mkdir is atomic)
  if mkdir "$lock_path" 2>/dev/null; then
    local now
    now=$(date +%s 2>/dev/null || echo 0)
    printf '%s' "$SESSION_ID"             > "$lock_path/session"
    printf '%s' "$now"                    > "$lock_path/started"
    printf '%s' "$((now + timeout))"      > "$lock_path/expires"
    printf '%s' "${LOCK_COMMAND:-?}"      > "$lock_path/command"
    return 0
  fi

  return 1
}

# release_lock <name>
# Releases only if the current session holds it, or if the lock was taken for exactly the command
# that has just finished (LOCK_COMMAND) — the second test keeps release working on a surface that
# gives hooks no stable session id.
release_lock() {
  local name="$1"
  local lock_path="$LOCK_DIR/${name}.lock"
  [ -d "$lock_path" ] || return 0
  local holder held_command
  holder=$(cat "$lock_path/session" 2>/dev/null || echo "")
  held_command=$(cat "$lock_path/command" 2>/dev/null || echo "")
  if [ "$holder" = "$SESSION_ID" ] || { [ -n "${LOCK_COMMAND:-}" ] && [ "$held_command" = "$LOCK_COMMAND" ]; }; then
    rm -rf "$lock_path" 2>/dev/null || true
  fi
}

# force_release_all_locks <reason>
# FORCE: releases every lock regardless of holder. For a lock left by a dead session.
force_release_all_locks() {
  ensure_dirs
  for lock_path in "$LOCK_DIR"/*.lock; do
    [ -d "$lock_path" ] || continue
    hook_force_log "dotnet-lock" "lock-release --force" \
      "$(basename "$lock_path" .lock) held by $(cat "$lock_path/session" 2>/dev/null || echo "?"): ${1:-no reason given}"
    rm -rf "$lock_path" 2>/dev/null || true
  done
}

# prune_run_dir
# Drops what nothing will ever clean up otherwise: expired locks, expired force tokens, and status
# entries more than a day old (sessions that ended without a Stop or SessionEnd event).
prune_run_dir() {
  ensure_dirs
  local now
  now=$(date +%s 2>/dev/null || echo 0)
  for lock_path in "$LOCK_DIR"/*.lock; do
    [ -d "$lock_path" ] || continue
    [ "$now" -gt "$(cat "$lock_path/expires" 2>/dev/null || echo 0)" ] && rm -rf "$lock_path" 2>/dev/null
  done
  for token in "$HOOK_FORCE_DIR"/*; do
    [ -f "$token" ] || continue
    [ "$now" -gt "$(sed -n '1p' "$token" 2>/dev/null || echo 0)" ] && rm -f "$token" 2>/dev/null
  done
  find "$STATUS_DIR" -name 'session-*.json' -mmin +1440 -exec rm -f {} + 2>/dev/null || true
}

# grant_force <hook|all> <reason>
grant_force() {
  local hook="$1" reason="${2:-}"
  case "$hook" in
    destructive-guard|dotnet-lock|output-dirs|sln-edit|all) ;;
    *) echo "Unknown hook '$hook'. Use: destructive-guard, dotnet-lock, output-dirs, sln-edit or all." >&2; return 1 ;;
  esac
  if [ -z "$reason" ]; then
    echo "A reason is required: coordination.sh force $hook \"<why this is intended>\"" >&2
    return 1
  fi
  mkdir -p "$HOOK_FORCE_DIR" 2>/dev/null || true
  local now
  now=$(date +%s 2>/dev/null || echo 0)
  printf '%s\n%s\n%s\n' "$((now + HOOK_FORCE_TTL_SECONDS))" "$SESSION_ID" "$reason" > "$HOOK_FORCE_DIR/$hook"
  hook_force_log "$hook" "token-granted" "$reason"
  echo "FORCE granted for '$hook': one use, valid for $((HOOK_FORCE_TTL_SECONDS / 60)) minutes. Reason: $reason"
}

# run_with_lock <command...>
# Holds dotnet-ops for the whole of an arbitrary command and always releases it.
run_with_lock() {
  local described="with-lock: $*"
  export LOCK_COMMAND="${described:0:120}"
  if ! acquire_lock "dotnet-ops"; then
    echo "dotnet-ops is held: $(lock_info "dotnet-ops")" >&2
    echo "Retry when it clears, or: coordination.sh lock-release --force (only if the holder is dead)." >&2
    return 75
  fi
  write_status "running" "$LOCK_COMMAND"
  trap 'release_lock "dotnet-ops"; write_status "idle" ""' EXIT INT TERM
  "$@"
}

# release_all_locks
# Releases every lock held by the current session.
release_all_locks() {
  ensure_dirs
  for lock_path in "$LOCK_DIR"/*.lock; do
    [ -d "$lock_path" ] || continue
    local holder
    holder=$(cat "$lock_path/session" 2>/dev/null || echo "")
    if [ "$holder" = "$SESSION_ID" ]; then
      rm -rf "$lock_path" 2>/dev/null || true
    fi
  done
}

# lock_info <name>
# Prints human-readable lock info (holder, command, remaining seconds).
lock_info() {
  local name="$1"
  local lock_path="$LOCK_DIR/${name}.lock"
  [ -d "$lock_path" ] || { echo "(no lock)"; return; }
  local holder cmd expires now remaining
  holder=$(cat "$lock_path/session"  2>/dev/null || echo "?")
  cmd=$(cat    "$lock_path/command"  2>/dev/null || echo "?")
  expires=$(cat "$lock_path/expires" 2>/dev/null || echo 0)
  now=$(date +%s 2>/dev/null || echo 0)
  remaining=$((expires - now))
  if [ "$remaining" -gt 0 ]; then
    echo "LOCKED by session=$holder | cmd=${cmd:0:80} | ~${remaining}s remaining"
  else
    echo "STALE  by session=$holder | cmd=${cmd:0:80} | expired"
  fi
}

# ── Status tracking ───────────────────────────────────────────────────────────

# write_status <action> [detail]
write_status() {
  local action="$1"
  local detail="${2:-}"
  ensure_dirs
  local now
  now=$(date -u '+%Y-%m-%dT%H:%M:%SZ' 2>/dev/null || date '+%Y-%m-%dT%H:%M:%SZ')
  local status_file="$STATUS_DIR/session-${SESSION_ID}.json"
  printf '{"session":"%s","action":"%s","detail":"%s","updated":"%s"}\n' \
    "$SESSION_ID" "$action" "${detail//\"/\'}" "$now" > "$status_file"
}

# clear_status
clear_status() {
  rm -f "$STATUS_DIR/session-${SESSION_ID}.json" 2>/dev/null || true
}

# print_status_board
# Prints all active sessions and locks to stdout.
print_status_board() {
  ensure_dirs
  local now
  now=$(date +%s 2>/dev/null || echo 0)

  echo "┌─ Sessions ────────────────────────────────────────────────────┐"
  local found=0
  for f in "$STATUS_DIR"/session-*.json; do
    [ -f "$f" ] || continue
    found=1
    local session action detail updated
    session=$(grep -o '"session":"[^"]*"' "$f" | cut -d'"' -f4)
    action=$(grep -o '"action":"[^"]*"'  "$f" | cut -d'"' -f4)
    detail=$(grep -o '"detail":"[^"]*"'  "$f" | cut -d'"' -f4)
    updated=$(grep -o '"updated":"[^"]*"' "$f" | cut -d'"' -f4)
    printf "│  [%s] %-16s %s\n" "$session" "$action" "${detail:0:50}"
  done
  [ "$found" -eq 0 ] && echo "│  (none)"
  echo "├─ Locks ───────────────────────────────────────────────────────┤"
  local lfound=0
  for lock_path in "$LOCK_DIR"/*.lock; do
    [ -d "$lock_path" ] || continue
    lfound=1
    local name holder cmd expires remaining
    name=$(basename "$lock_path" .lock)
    holder=$(cat "$lock_path/session"  2>/dev/null || echo "?")
    cmd=$(cat    "$lock_path/command"  2>/dev/null || echo "?")
    expires=$(cat "$lock_path/expires" 2>/dev/null || echo 0)
    remaining=$((expires - now))
    if [ "$remaining" -gt 0 ]; then
      printf "│  LOCKED  %-14s session=%-8s cmd=%s (~%ds)\n" \
        "$name" "$holder" "${cmd:0:30}" "$remaining"
    else
      printf "│  STALE   %-14s session=%-8s (expired)\n" "$name" "$holder"
    fi
  done
  [ "$lfound" -eq 0 ] && echo "│  (none)"
  echo "└───────────────────────────────────────────────────────────────┘"
}

# ── Messaging ─────────────────────────────────────────────────────────────────

# send_message <to_session> <subject> <body>
send_message() {
  local to="$1"
  local subject="$2"
  local body="$3"
  local ts
  ts=$(date +%s 2>/dev/null || echo 0)
  local inbox="$MSG_DIR/$to"
  mkdir -p "$inbox" 2>/dev/null || true
  local msg_file="$inbox/${ts}-from-${SESSION_ID}.msg"
  printf 'FROM: %s\nTO: %s\nSUBJECT: %s\nSENT: %s\nEXPIRES: %s\n\n%s\n' \
    "$SESSION_ID" "$to" "$subject" "$ts" "$((ts + 120))" "$body" > "$msg_file"
  echo "Message sent to session $to: $subject"
}

# check_messages
# Prints and clears all unexpired messages for the current session.
check_messages() {
  local inbox="$MSG_DIR/$SESSION_ID"
  [ -d "$inbox" ] || return 0
  local now
  now=$(date +%s 2>/dev/null || echo 0)
  local found=0
  for f in "$inbox"/*.msg; do
    [ -f "$f" ] || continue
    local expires
    expires=$(grep '^EXPIRES:' "$f" 2>/dev/null | awk '{print $2}')
    if [ "$now" -lt "${expires:-0}" ]; then
      found=1
      echo "── Message ──────────────────────────────"
      cat "$f"
      echo ""
    fi
    rm -f "$f" 2>/dev/null || true
  done
  [ "$found" -eq 0 ] && echo "(no messages)"
}

# ── CLI entry point ───────────────────────────────────────────────────────────

# Only run as CLI if this script is executed directly (not sourced)
if [[ "${BASH_SOURCE[0]}" == "${0}" ]]; then
  cmd="${1:-help}"
  case "$cmd" in
    status)
      write_status "${2:-idle}" "${3:-}"
      ;;
    status-clear)
      clear_status
      ;;
    lock-status)
      print_status_board
      ;;
    lock-release)
      if [ "${2:-}" = "--force" ]; then
        force_release_all_locks "${3:-}"
        echo "FORCE: released every lock (logged to .claude/run/force.log)"
      else
        release_all_locks
        echo "Released all locks for session $SESSION_ID"
      fi
      ;;
    with-lock)
      shift
      [ "${1:-}" = "--" ] && shift
      if [ "$#" -eq 0 ]; then
        echo "Usage: coordination.sh with-lock -- <command> [args...]" >&2
        exit 64
      fi
      run_with_lock "$@"
      exit $?
      ;;
    force)
      grant_force "${2:-}" "${3:-}" || exit 64
      ;;
    prune)
      prune_run_dir
      ;;
    send)
      send_message "${2:-}" "${3:-no subject}" "${4:-}"
      ;;
    messages)
      check_messages
      ;;
    help|*)
      echo "Usage: coordination.sh <command> [args]"
      echo "  status <action> [detail]   — write this session's status"
      echo "  status-clear               — clear this session's status"
      echo "  lock-status                — print all active locks + sessions"
      echo "  lock-release               — release all locks held by this session"
      echo "  lock-release --force [why] — FORCE: release every lock, whoever holds it"
      echo "  with-lock -- <command>     — run a command under the dotnet-ops lock"
      echo "  force <hook|all> <reason>  — FORCE: one-use override for a blocking hook"
      echo "  prune                      — drop expired locks, tokens and stale status"
      echo "  send <to> <subject> <body> — send a message to another session"
      echo "  messages                   — read + clear inbox"
      echo ""
      echo "Session ID: $SESSION_ID"
      echo "Run dir:    $RUN_DIR"
      ;;
  esac
fi
