#!/usr/bin/env bash
# hook-lib.sh — shared helpers for every hook in this directory. Source it; do not run it.
#
#   source "$SCRIPT_DIR/hook-lib.sh" || exit 0      # every hook fails open if the library is missing
#
# What it provides
#   hook_read_input            read the hook's stdin JSON once into HOOK_INPUT
#   hook_field <a.b.c>         print one field of HOOK_INPUT (python3/python; sed fallback for flat keys)
#   hook_session_id            the id every hook and the coordination CLI agree on for this session
#   hook_force_requested <hook> [marker]
#                              0 if the owner or agent has asked to override <hook> for this call
#   hook_force_log <hook> <how> <target>
#                              append one line to the override log
#   hook_force_hint <hook> [marker]
#                              print "how to override" to stderr, for a block message
#
# FORCE OVERRIDE — every blocking hook honours all three, checked in this order:
#   1. Marker   a trailing shell comment in the Bash command itself, e.g.  # FORCE-CONFIRMED
#               (Bash hooks only; each hook documents its own marker).
#   2. Token    bash .claude/hooks/coordination.sh force <hook|all> "<reason>"
#               One use, expires after 10 minutes. Works for Write/Edit hooks, where no comment fits.
#   3. Env      PINEGUARD_HOOK_FORCE=<hook>[,<hook>...] or PINEGUARD_HOOK_FORCE=all
#               Standing override for the owner; set it in the shell or in settings "env".
# Hook names: destructive-guard, dotnet-lock, output-dirs, sln-edit.
# Every override is recorded in .claude/run/force.log.
#
# A pause is the point of a guard: it makes the agent look at what it is about to do. An override is
# for the case where it has looked and the operation is intended.

HOOK_ROOT_DIR="${CLAUDE_PROJECT_DIR:-$(git rev-parse --show-toplevel 2>/dev/null || pwd)}"
HOOK_RUN_DIR="$HOOK_ROOT_DIR/.claude/run"
HOOK_FORCE_DIR="$HOOK_RUN_DIR/force"
HOOK_FORCE_LOG="$HOOK_RUN_DIR/force.log"
HOOK_FORCE_TTL_SECONDS=600
HOOK_INPUT="${HOOK_INPUT:-}"

# ── Python discovery (python3 on Linux/macOS, python on Windows) ─────────────
HOOK_PYTHON=""
if command -v python3 >/dev/null 2>&1; then
  HOOK_PYTHON="python3"
elif command -v python >/dev/null 2>&1; then
  HOOK_PYTHON="python"
fi

hook_read_input() {
  if [ -z "$HOOK_INPUT" ] && [ ! -t 0 ]; then
    HOOK_INPUT=$(cat)
  fi
}

# hook_field <dotted.path>
# Decodes JSON properly, so multi-line and quoted commands survive. Prints nothing if absent.
hook_field() {
  local path="$1"
  [ -n "$HOOK_INPUT" ] || return 0
  if [ -n "$HOOK_PYTHON" ]; then
    printf '%s' "$HOOK_INPUT" | "$HOOK_PYTHON" -c '
import json, sys
try:
    node = json.load(sys.stdin)
    for key in sys.argv[1].split("."):
        node = node.get(key) if isinstance(node, dict) else None
    if isinstance(node, (str, int, float)) and not isinstance(node, bool):
        sys.stdout.write(str(node))
except Exception:
    pass
' "$path" 2>/dev/null
    return 0
  fi
  # Fallback without Python: last path segment as a flat key, first match, no escape decoding.
  local key="${path##*.}"
  printf '%s' "$HOOK_INPUT" | sed -n "s/.*\"$key\"[[:space:]]*:[[:space:]]*\"\([^\"]*\)\".*/\1/p" | head -1
}

# hook_session_id
# The hook payload carries session_id; shells started by the session carry CLAUDE_CODE_SESSION_ID. Both
# name the same session, so a lock taken by a PreToolUse hook is released by its PostToolUse hook and
# by the coordination CLI. PPID is the last resort and is NOT stable across hook invocations.
hook_session_id() {
  local id=""
  id=$(hook_field "session_id")
  [ -n "$id" ] || id="${CLAUDE_CODE_SESSION_ID:-}"
  [ -n "$id" ] || id="${CLAUDE_SESSION_ID:-}"
  [ -n "$id" ] || id="${PPID:-$$}"
  printf '%s' "${id:0:36}"
}

hook_force_log() {
  local hook="$1" how="$2" target="${3:-}"
  mkdir -p "$HOOK_RUN_DIR" 2>/dev/null || return 0
  local now
  now=$(date -u '+%Y-%m-%dT%H:%M:%SZ' 2>/dev/null || echo "?")
  target=$(printf '%s' "$target" | tr '\n\r\t' '   ')
  printf '%s session=%s hook=%s via=%s target=%s\n' \
    "$now" "$(hook_session_id)" "$hook" "$how" "${target:0:300}" >> "$HOOK_FORCE_LOG" 2>/dev/null || true
}

# _hook_force_token <name>  — consume a one-use token if present and unexpired.
_hook_force_token() {
  local token="$HOOK_FORCE_DIR/$1"
  [ -f "$token" ] || return 1
  local expires now
  expires=$(sed -n '1p' "$token" 2>/dev/null)
  now=$(date +%s 2>/dev/null || echo 0)
  rm -f "$token" 2>/dev/null || true
  [ -n "$expires" ] && [ "$now" -le "$expires" ] 2>/dev/null
}

# hook_force_requested <hook> [marker] [text-to-search-for-marker] [target-for-log]
hook_force_requested() {
  local hook="$1" marker="${2:-}" text="${3:-}" target="${4:-}"

  if [ -n "$marker" ] && [ -n "$text" ] && printf '%s' "$text" | grep -qF -- "# $marker"; then
    hook_force_log "$hook" "marker:$marker" "${target:-$text}"
    return 0
  fi

  if _hook_force_token "$hook"; then
    hook_force_log "$hook" "token:$hook" "$target"
    return 0
  fi
  if _hook_force_token "all"; then
    hook_force_log "$hook" "token:all" "$target"
    return 0
  fi

  local env_list=",${PINEGUARD_HOOK_FORCE:-},"
  case "$env_list" in
    *,all,*|*,"$hook",*)
      hook_force_log "$hook" "env:PINEGUARD_HOOK_FORCE" "$target"
      return 0
      ;;
  esac

  return 1
}

# hook_force_hint <hook> [marker]
hook_force_hint() {
  local hook="$1" marker="${2:-}"
  echo "   ── FORCE override (only when the operation is intended) ─────────" >&2
  if [ -n "$marker" ]; then
    echo "   • re-run the same command with a trailing comment:  # $marker" >&2
  fi
  echo "   • or, once:  bash .claude/hooks/coordination.sh force $hook \"<reason>\"" >&2
  echo "   • owner, standing:  PINEGUARD_HOOK_FORCE=$hook  (or =all)" >&2
  echo "   Overrides are logged to .claude/run/force.log." >&2
}
