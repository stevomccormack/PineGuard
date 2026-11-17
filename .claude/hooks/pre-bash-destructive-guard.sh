#!/usr/bin/env bash
# Hook: pre-bash-destructive-guard.sh
# PreToolUse (Bash)
# Purpose: Stop a destructive git or filesystem command before it runs, say exactly what was found,
#          and let it through only when the caller has confirmed it is intended.
#
# FORCE override (see hook-lib.sh for all three forms):
#   re-run the same command with the trailing comment   # FORCE-CONFIRMED
#   or once:  bash .claude/hooks/coordination.sh force destructive-guard "<reason>"
#
# Covered operations (judged per command, by destructive-guard.py):
#   git  : reset --hard | push --force / -f / --force-with-lease / --mirror / +refspec
#          push --delete / :refspec | branch -D | clean -f | checkout -- <path> / . / -f
#          restore (working tree) | stash drop/clear | filter-branch | reflog expire/delete
#          update-ref -d | worktree remove --force
#   bash : rm -rf (every recursive+force spelling) | find -delete / -exec rm | shred | mkfs
#   pwsh : Remove-Item -Recurse -Force (any order, any abbreviation)
#   cmd  : rd /s | rmdir /s
#   py   : shutil.rmtree
#
# Not flagged: text that is never executed — commit messages, heredocs written to a file, arguments
# to echo/grep — and a clean-tree `git checkout -- <path>` / `git restore`, which has nothing to lose.
#
# Input:  JSON on stdin  { session_id, tool_name, tool_input: { command } }
# Exit 0: allow
# Exit 2: block  (stderr message shown to Claude + user)

set -uo pipefail

HOOK_NAME="destructive-guard"
FORCE_MARKER="FORCE-CONFIRMED"

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
# Fail open if the shared library is missing (first-time setup, partial checkout).
source "$SCRIPT_DIR/hook-lib.sh" 2>/dev/null || exit 0

hook_read_input
COMMAND=$(hook_field "tool_input.command")
[ -z "$COMMAND" ] && exit 0

# ─── Quick pre-filter ─────────────────────────────────────────────────────────
# Nearly every command has none of these words; skip the parser for them.
if ! printf '%s' "$COMMAND" | grep -qiE \
    '(rm|git|find|Remove-Item|ri|del|erase|rd|rmdir|rmtree|shred|mkfs)'; then
    exit 0
fi

# ─── Classify ─────────────────────────────────────────────────────────────────
FINDINGS=""
if [ -n "$HOOK_PYTHON" ] && [ -f "$SCRIPT_DIR/destructive-guard.py" ]; then
    FINDINGS=$(printf '%s' "$COMMAND" | "$HOOK_PYTHON" "$SCRIPT_DIR/destructive-guard.py" 2>/dev/null)
else
    # No Python: fall back to matching the whole string. Coarser, and it can flag text that is not
    # executed, but it never lets a destructive command through unseen.
    add() { FINDINGS="${FINDINGS}${1}"$'\t'"${2}"$'\t'"${3}"$'\t'"0"$'\n'; }
    if printf '%s' "$COMMAND" | grep -qE '(^|[[:space:];&|(])rm[[:space:]]' &&
       printf '%s' "$COMMAND" | grep -qE -- '(-[a-zA-Z]*[rR][a-zA-Z]*[fF]|-[a-zA-Z]*[fF][a-zA-Z]*[rR]|--recursive.*--force|--force.*--recursive|-[rR][[:space:]].*-[fF]([[:space:]]|$)|-[fF][[:space:]].*-[rR]([[:space:]]|$))'; then
        add "rm-recursive-force" "rm with recursive + force" "Permanently deletes directories and all their contents."
    fi
    printf '%s' "$COMMAND" | grep -qE 'git([[:space:]]+-[cC][[:space:]]+[^[:space:]]+)*[[:space:]]+reset[[:space:]].*--hard' &&
        add "git-reset-hard" "git reset --hard" "Moves HEAD and discards uncommitted changes."
    printf '%s' "$COMMAND" | grep -qE 'git[[:space:]]+push[[:space:]].*(--force|--mirror|[[:space:]]-[a-zA-Z]*f[a-zA-Z]*([[:space:]]|$)|[[:space:]]\+[^[:space:]])' &&
        add "git-push-force" "git push --force" "Rewrites remote history."
    printf '%s' "$COMMAND" | grep -qE 'git[[:space:]]+push[[:space:]].*(--delete|[[:space:]]:[^[:space:]])' &&
        add "git-push-delete" "git push that deletes a remote ref" "Deletes a branch or tag on the remote."
    printf '%s' "$COMMAND" | grep -qE 'git[[:space:]]+branch[[:space:]].*-[a-zA-Z]*D' &&
        add "git-branch-force-delete" "git branch -D (force delete)" "Unmerged commits may become unreachable."
    printf '%s' "$COMMAND" | grep -qE 'git[[:space:]]+clean[[:space:]].*-[a-zA-Z]*f' &&
        add "git-clean" "git clean -f" "Permanently deletes untracked files."
    printf '%s' "$COMMAND" | grep -qiE 'Remove-Item.*-Recurse.*-Force|Remove-Item.*-Force.*-Recurse' &&
        add "pwsh-remove-item" "PowerShell Remove-Item -Recurse -Force" "Recursive force-delete through PowerShell."
    printf '%s' "$COMMAND" | grep -qiE '(^|[[:space:]])(rd|rmdir)[[:space:]]+/s' &&
        add "cmd-rd-s" "rd /s or rmdir /s" "Recursively removes a directory."
    printf '%s' "$COMMAND" | grep -qE 'shutil\.rmtree' &&
        add "python-rmtree" "Python shutil.rmtree" "Recursive directory removal through Python."
fi

[ -z "$FINDINGS" ] && exit 0

# ─── Drop findings that cannot lose anything on a clean tree ─────────────────
GIT_STATUS=$(git -C "$HOOK_ROOT_DIR" status --porcelain 2>/dev/null || true)
ACTIVE=""
while IFS=$'\t' read -r rule title why dirty_only; do
    [ -z "$rule" ] && continue
    if [ "$dirty_only" = "1" ] && [ -z "$GIT_STATUS" ]; then
        continue
    fi
    ACTIVE="${ACTIVE}${rule}"$'\t'"${title}"$'\t'"${why}"$'\n'
done <<< "$FINDINGS"

[ -z "$ACTIVE" ] && exit 0

# ─── FORCE override ───────────────────────────────────────────────────────────
if hook_force_requested "$HOOK_NAME" "$FORCE_MARKER" "$COMMAND" "$COMMAND"; then
    exit 0
fi

# ─── Block ────────────────────────────────────────────────────────────────────
NEEDS_STASH="false"
while IFS=$'\t' read -r rule title why; do
    [ -z "$rule" ] && continue
    echo "🚫  BLOCKED — $title" >&2
    echo "   $why" >&2
    case "$rule" in
        git-reset-hard|git-checkout-discard|git-restore-discard|git-clean) NEEDS_STASH="true" ;;
    esac
done <<< "$ACTIVE"
echo "" >&2
echo "   Command:" >&2
printf '%s\n' "$COMMAND" | head -5 | sed 's/^/     /' >&2
echo "" >&2

if [ "$NEEDS_STASH" = "true" ] && [ -n "$GIT_STATUS" ]; then
    CHANGE_COUNT=$(printf '%s\n' "$GIT_STATUS" | grep -c "")
    echo "   Uncommitted changes that would be lost ($CHANGE_COUNT file(s)):" >&2
    printf '%s\n' "$GIT_STATUS" | head -15 | sed 's/^/     /' >&2
    [ "$CHANGE_COUNT" -gt 15 ] && echo "     ... ($((CHANGE_COUNT - 15)) more)" >&2
    echo "" >&2
    echo "   Safer: run 'git stash' first, then repeat the command." >&2
    echo "" >&2
fi

hook_force_hint "$HOOK_NAME" "$FORCE_MARKER"
exit 2
