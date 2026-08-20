#!/usr/bin/env bash
# Hook: pre-bash-destructive-guard.sh
# PreToolUse (Bash)
# Purpose: Intercept and block destructive git and filesystem operations.
#          Prompts the user to CONFIRM, STASH (for git reset), or CANCEL.
#
# BYPASS: Add  # FORCE-CONFIRMED  as a comment anywhere in the command.
#         Valid comment syntax in bash, PowerShell, and Python.
#
# Covered operations:
#   git  : reset --hard | push --force/-f/--force-with-lease | branch -D | clean -f
#   bash : rm -rf (and all recursive+force flag variants)
#   pwsh : Remove-Item -Recurse -Force (any flag order)
#   cmd  : rd /s  |  rmdir /s
#   py   : shutil.rmtree
#
# Input:  JSON on stdin  { tool_name, tool_input: { command } }
# Exit 0: allow
# Exit 2: block  (stderr message shown to Claude + user)

set -uo pipefail

INPUT=$(cat)

# ─── Parse command from JSON ──────────────────────────────────────────────────
# Python3 primary: handles multiline commands and proper JSON string decoding.
COMMAND=$(echo "$INPUT" | python3 -c "
import json, sys
try:
    data = json.load(sys.stdin)
    print(data.get('tool_input', {}).get('command', ''))
except Exception:
    pass
" 2>/dev/null)

# Fallback: sed (works for simple single-line JSON values)
if [ -z "$COMMAND" ]; then
    COMMAND=$(echo "$INPUT" | sed -n 's/.*"command"[[:space:]]*:[[:space:]]*"\([^"]*\)".*/\1/p' | head -1)
fi

[ -z "$COMMAND" ] && exit 0

# ─── Bypass: explicit confirmation marker ────────────────────────────────────
# When the user confirms, Claude re-runs the command with # FORCE-CONFIRMED.
if echo "$COMMAND" | grep -qF "# FORCE-CONFIRMED"; then
    exit 0
fi

# ─── Quick pre-filter ─────────────────────────────────────────────────────────
# Skip all checks early if no dangerous keywords are present.
if ! echo "$COMMAND" | grep -qiE \
    '(rm[[:space:]]|git[[:space:]]+(reset|push|branch|clean)|Remove-Item|(rd|rmdir)[[:space:]]|shutil\.rmtree)'; then
    exit 0
fi

# ─── Helper: print standard block message ────────────────────────────────────
REPO_DIR="${CLAUDE_PROJECT_DIR:-$(pwd)}"

print_options() {
    local show_stash="${1:-false}"
    echo "   ── Options ──────────────────────────────────────────────────────" >&2
    if [ "$show_stash" = "true" ]; then
    echo "   STASH   — run git stash first, then proceed (no data loss)"      >&2
    fi
    echo "   CONFIRM — explicitly authorize this destructive operation"        >&2
    echo "             (I will re-run the command with # FORCE-CONFIRMED)"     >&2
    echo "   CANCEL  — stop, do not proceed"                                   >&2
    echo "   ─────────────────────────────────────────────────────────────────" >&2
}

print_block() {
    local title="$1"
    local detail="$2"
    local show_stash="${3:-false}"
    echo "🚫  BLOCKED — $title" >&2
    echo "" >&2
    [ -n "$detail" ] && { echo "   $detail" >&2; echo "" >&2; }
    echo "   Command:" >&2
    echo "$COMMAND" | head -5 | sed 's/^/     /' >&2
    echo "" >&2
    print_options "$show_stash"
}

# ─── 1. rm with recursive + force flags ──────────────────────────────────────
# Covers: -rf  -fr  -rF  -fR  -Rf  -Fr  and  -r -f  -r --force  --recursive -f
if echo "$COMMAND" | grep -qE '(^|[[:space:]])rm[[:space:]]'; then
    HAS_RF=$(echo "$COMMAND" | grep -cE -- \
        '(-[a-zA-Z]*[rR][a-zA-Z]*[fF][a-zA-Z]*|-[a-zA-Z]*[fF][a-zA-Z]*[rR][a-zA-Z]*)' 2>/dev/null || true)
    HAS_R_SEPARATE=$(echo "$COMMAND" | grep -cE -- '(-r[[:space:]]|-R[[:space:]]|--recursive)' 2>/dev/null || true)
    HAS_F_SEPARATE=$(echo "$COMMAND" | grep -cE -- '(-f[[:space:]]|-F[[:space:]]|--force)' 2>/dev/null || true)

    if [ "$HAS_RF" -gt 0 ] || { [ "$HAS_R_SEPARATE" -gt 0 ] && [ "$HAS_F_SEPARATE" -gt 0 ]; }; then
        print_block "rm with recursive + force" \
            "Permanently deletes directories and all contents. Cannot be undone."
        exit 2
    fi
fi

# ─── 2. git reset --hard ──────────────────────────────────────────────────────
if echo "$COMMAND" | grep -qE '(^|[[:space:]])git[[:space:]]+reset[[:space:]]+--hard'; then
    GIT_STATUS=$(git -C "$REPO_DIR" status --porcelain 2>/dev/null || true)

    if [ -n "$GIT_STATUS" ]; then
        CHANGE_COUNT=$(echo "$GIT_STATUS" | grep -c "" 2>/dev/null || echo "?")
        echo "🚫  BLOCKED — git reset --hard with uncommitted changes" >&2
        echo "" >&2
        echo "   Uncommitted changes ($CHANGE_COUNT file(s)):" >&2
        echo "$GIT_STATUS" | head -15 | sed 's/^/     /' >&2
        if [ "$CHANGE_COUNT" -gt 15 ] 2>/dev/null; then
            echo "     ... ($((CHANGE_COUNT - 15)) more)" >&2
        fi
        echo "" >&2
        echo "   ── Options ──────────────────────────────────────────────────────" >&2
        echo "   STASH   — git stash first, then reset --hard (changes preserved)"  >&2
        echo "   CONFIRM — PERMANENTLY DISCARD all uncommitted changes listed above" >&2
        echo "             (I will re-run with # FORCE-CONFIRMED)"                  >&2
        echo "   CANCEL  — stop, do not proceed"                                    >&2
        echo "   ─────────────────────────────────────────────────────────────────" >&2
    else
        print_block "git reset --hard (clean working tree)" \
            "Moves HEAD and discards commits after the target — history is lost." \
            "false"
    fi
    exit 2
fi

# ─── 3. git push --force ──────────────────────────────────────────────────────
if echo "$COMMAND" | grep -qE '(^|[[:space:]])git[[:space:]]+push([[:space:]]|$)'; then
    if echo "$COMMAND" | grep -qE -- \
        '(--force|--force-with-lease|-f([[:space:]]|$)|-[a-zA-Z]*f[a-zA-Z]*(--| |$))'; then
        print_block "git push --force" \
            "Rewrites remote history — affects all collaborators and cannot be undone."
        exit 2
    fi
fi

# ─── 4. git branch -D ─────────────────────────────────────────────────────────
if echo "$COMMAND" | grep -qE '(^|[[:space:]])git[[:space:]]+branch([[:space:]]|$)'; then
    if echo "$COMMAND" | grep -qE -- '-[a-zA-Z]*D[a-zA-Z]*'; then
        print_block "git branch -D (force delete)" \
            "Force-deletes a branch — unmerged commits may become unreachable."
        exit 2
    fi
fi

# ─── 5. git clean -f ──────────────────────────────────────────────────────────
if echo "$COMMAND" | grep -qE '(^|[[:space:]])git[[:space:]]+clean([[:space:]]|$)'; then
    if echo "$COMMAND" | grep -qE -- '-[a-zA-Z]*f[a-zA-Z]*'; then
        print_block "git clean -f" \
            "Permanently deletes all untracked files — not recoverable from git."
        exit 2
    fi
fi

# ─── 6. PowerShell Remove-Item with -Recurse + -Force (any order) ────────────
if echo "$COMMAND" | grep -qiE 'Remove-Item'; then
    HAS_RECURSE=$(echo "$COMMAND" | grep -ciE -- '(-Recurse[^a-zA-Z]|-r[[:space:]])' 2>/dev/null || true)
    HAS_FORCE=$(  echo "$COMMAND" | grep -ciE -- '(-Force[^a-zA-Z]|-fo([[:space:]]|$))' 2>/dev/null || true)
    if [ "$HAS_RECURSE" -gt 0 ] && [ "$HAS_FORCE" -gt 0 ]; then
        print_block "PowerShell Remove-Item -Recurse -Force" \
            "Recursive force-delete via PowerShell — cannot be undone."
        exit 2
    fi
fi

# ─── 7. Windows rd /s or rmdir /s ─────────────────────────────────────────────
if echo "$COMMAND" | grep -qiE '(^|[[:space:]])(rd|rmdir)[[:space:]]+(/s|/S)'; then
    print_block "rd /s or rmdir /s (Windows recursive delete)" \
        "Recursively removes a directory and all contents — cannot be undone."
    exit 2
fi

# ─── 8. Python shutil.rmtree ──────────────────────────────────────────────────
if echo "$COMMAND" | grep -qE 'shutil\.rmtree'; then
    print_block "Python shutil.rmtree" \
        "Recursive directory removal via Python — cannot be undone."
    exit 2
fi

# ─── All clear ────────────────────────────────────────────────────────────────
exit 0
