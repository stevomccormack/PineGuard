#!/usr/bin/env bash
# test-hooks.sh — behaviour tests for the hooks in the parent directory.
#
#   bash .claude/hooks/tests/test-hooks.sh
#
# Runs every hook against a throwaway git repository, so no lock, token or log of the real working
# tree is touched. Exit 0 when every case passes, 1 otherwise.

set -uo pipefail

HOOKS="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
WORK="$(mktemp -d)"
# Git Bash hands native programs Windows-spelled paths, so a POSIX $WORK comes back from python3
# re-spelled ("/tmp/x" -> "C:/Users/.../Temp/x") and no longer matches the root the hooks read from
# $CLAUDE_PROJECT_DIR. Spell it the Windows way up front, where cygpath can.
WORK="$(cygpath -m "$WORK" 2>/dev/null || printf '%s' "$WORK")"
trap 'cd /; find "$WORK" -mindepth 1 -delete 2>/dev/null; rmdir "$WORK" 2>/dev/null' EXIT

export CLAUDE_PROJECT_DIR="$WORK/repo"
export CLAUDE_CODE_SESSION_ID="session-A"
unset PINEGUARD_HOOK_FORCE ALLOW_SLN_EDIT CLAUDE_SESSION_ID 2>/dev/null || true
mkdir -p "$CLAUDE_PROJECT_DIR"
git -C "$CLAUDE_PROJECT_DIR" init -q
git -C "$CLAUDE_PROJECT_DIR" -c user.email=t@example.invalid -c user.name=t commit -q --allow-empty -m init

PASS=0
FAIL=0

# payload <tool-input-key> <value> [session]
payload() {
  python3 -c 'import json,sys; print(json.dumps({"session_id": sys.argv[3], "tool_name": "x", "tool_input": {sys.argv[1]: sys.argv[2]}}))' \
    "$1" "$2" "${3:-session-A}"
}

# expect <label> <expected-exit> <hook> <key> <value> [session]
expect() {
  local label="$1" want="$2" hook="$3" key="$4" value="$5" session="${6:-session-A}"
  local got
  payload "$key" "$value" "$session" | bash "$HOOKS/$hook" >/dev/null 2>&1
  got=$?
  if [ "$got" = "$want" ]; then
    PASS=$((PASS + 1))
  else
    FAIL=$((FAIL + 1))
    echo "FAIL [$hook] $label: expected exit $want, got $got"
    echo "      $key: ${value:0:160}"
  fi
}

block() { expect "$1" 2 pre-bash-destructive-guard.sh command "$2"; }
allow() { expect "$1" 0 pre-bash-destructive-guard.sh command "$2"; }

# ── destructive guard: must block ────────────────────────────────────────────
block "rm -rf"                    'rm -rf build'
block "rm -fr"                    'rm -fr build'
block "rm -r -f"                  'rm -r -f build'
block "rm --recursive --force"    'rm --recursive --force build'
block "rm -Rf after cd"           'cd /tmp && rm -Rf build'
block "sudo rm -rf"               'sudo rm -rf /var/x'
block "xargs rm -rf"              'ls | xargs rm -rf'
block "rm -rf in subshell"        'echo $(rm -rf build)'
block "rm -rf in bash -c"         'bash -c "rm -rf build"'
block "rm -rf in heredoc to bash" $'bash <<EOF\nrm -rf build\nEOF'
block "git reset --hard"          'git reset --hard HEAD~1'
block "git -C reset --hard"       'git -C sub reset --hard origin/main'
block "git push --force"          'git push --force origin main'
block "git push -f"               'git push -f origin main'
block "git push -uf"              'git push -uf origin topic'
block "git push force-with-lease" 'git push --force-with-lease origin topic'
block "git push +refspec"         'git push origin +topic'
block "git push --delete"         'git push origin --delete topic'
block "git push :refspec"         'git push origin :topic'
block "git push --mirror"         'git push --mirror origin'
block "git branch -D"             'git branch -D topic'
block "git clean -fd"             'git clean -fd'
block "git stash clear"           'git stash clear'
block "git stash drop"            'git stash drop stash@{0}'
block "git filter-branch"         'git filter-branch --tree-filter x HEAD'
block "git reflog expire"         'git reflog expire --expire=now --all'
block "git update-ref -d"         'git update-ref -d refs/heads/topic'
block "find -delete"              'find . -name "*.tmp" -delete'
block "find -exec rm"             'find . -name "*.tmp" -exec rm {} \;'
block "Remove-Item recurse force" 'pwsh -Command "Remove-Item -Recurse -Force build"'
block "Remove-Item force recurse" 'Remove-Item build -Force -Recurse'
block "rd /s"                     'rd /s /q build'
block "python rmtree -c"          'python3 -c "import shutil; shutil.rmtree(\"build\")"'
block "python rmtree heredoc"     $'python3 - <<EOF\nimport shutil\nshutil.rmtree("build")\nEOF'

# ── destructive guard: must allow (text that is never executed, or harmless) ──
allow "plain build"               'dotnet build PineGuard.slnx -c Release'
allow "rm single file"            'rm notes.txt'
allow "rm -f single file"         'rm -f notes.txt'
allow "git push"                  'git push origin feature/x'
allow "git push -u"               'git push -u origin feature/x'
allow "commit message with -f word" $'git commit -q -F - <<\'EOF\'\ntest(core): docs-conformance review fixes\n\n- rm -rf is mentioned here only as text\nEOF\ngit push origin feature/x'
allow "commit -m mentioning force" 'git commit -m "explain git push --force and rm -rf" && git push origin topic'
allow "script written by heredoc"  $'cat > gate.sh <<\'EOF\'\nrm -rf "$R"\ngit reset --hard\nEOF\nchmod +x gate.sh'
allow "echo of dangerous text"    'echo "do not run rm -rf / or git reset --hard"'
allow "grep for dangerous text"   'grep -rn "git push --force" docs'
allow "git branch -d"             'git branch -d merged-topic'
allow "git clean dry run"         'git clean -n -fd'
allow "git checkout branch"       'git checkout -B feature/x origin/feature/x'
allow "git restore --staged"      'git restore --staged file.cs'
allow "git stash push"            'git stash push -m wip'
allow "git log --oneline -f-ish"  'git log --oneline -5 --format=%s'
allow "find without delete"       'find . -name "*.cs" -newer x'
allow "Remove-Item single"        'Remove-Item notes.txt'
allow "rmdir empty"               'rmdir emptydir'

# ── destructive guard: dirty-only rules ──────────────────────────────────────
allow "checkout -- path, clean tree" 'git checkout -- src/a.cs'
allow "restore path, clean tree"     'git restore src/a.cs'
echo change > "$CLAUDE_PROJECT_DIR/dirty.txt"
block "checkout -- path, dirty tree" 'git checkout -- src/a.cs'
block "checkout ., dirty tree"       'git checkout .'
block "restore path, dirty tree"     'git restore src/a.cs'
rm -f "$CLAUDE_PROJECT_DIR/dirty.txt"

# ── FORCE: marker, one-use token, env ────────────────────────────────────────
allow "marker overrides"          'rm -rf build # FORCE-CONFIRMED'
bash "$HOOKS/coordination.sh" force destructive-guard "test" >/dev/null
allow "token overrides once"      'rm -rf build'
block "token is consumed"         'rm -rf build'
bash "$HOOKS/coordination.sh" force all "test" >/dev/null
allow "token 'all' overrides"     'git reset --hard'
PINEGUARD_HOOK_FORCE=destructive-guard allow "env overrides" 'git push --force origin x'
PINEGUARD_HOOK_FORCE=sln-edit          block "env for another hook does not" 'git push --force origin x'
if bash "$HOOKS/coordination.sh" force destructive-guard "" >/dev/null 2>&1; then
  FAIL=$((FAIL + 1)); echo "FAIL force without a reason was accepted"
else
  PASS=$((PASS + 1))
fi
if bash "$HOOKS/coordination.sh" force nonsense "x" >/dev/null 2>&1; then
  FAIL=$((FAIL + 1)); echo "FAIL force for an unknown hook was accepted"
else
  PASS=$((PASS + 1))
fi
if [ "$(grep -c 'hook=destructive-guard' "$CLAUDE_PROJECT_DIR/.claude/run/force.log" 2>/dev/null)" -ge 4 ]; then
  PASS=$((PASS + 1))
else
  FAIL=$((FAIL + 1)); echo "FAIL overrides were not logged"
fi

# ── output directories ───────────────────────────────────────────────────────
out() { expect "$1" "$2" enforce-output-dirs.sh file_path "$3"; }
out "root .txt blocked"            2 "$CLAUDE_PROJECT_DIR/notes.txt"
out "root .log blocked"            2 "$CLAUDE_PROJECT_DIR/build.log"
out "relative root file blocked"   2 "scratch.txt"
out "root .md allowed"             0 "$CLAUDE_PROJECT_DIR/README.md"
out "root NuGet.config allowed"    0 "$CLAUDE_PROJECT_DIR/NuGet.config"
out "root .node-version allowed"   0 "$CLAUDE_PROJECT_DIR/.node-version"
out "known subdirectory allowed"   0 "$CLAUDE_PROJECT_DIR/src/A/B.cs"
out "new subdirectory allowed"     0 "$CLAUDE_PROJECT_DIR/benchmarks/results.txt"
out "outside project allowed"      0 "/tmp/claude-0/-home-user-PineGuard/scratchpad/msg.txt"
out "sibling with same prefix"     0 "$CLAUDE_PROJECT_DIR-other/notes.txt"
bash "$HOOKS/coordination.sh" force output-dirs "test" >/dev/null
out "token overrides once"         0 "$CLAUDE_PROJECT_DIR/notes.txt"
out "token is consumed"            2 "$CLAUDE_PROJECT_DIR/notes.txt"

# ── solution files ───────────────────────────────────────────────────────────
sln() { expect "$1" "$2" block-sln-edit.sh file_path "$3"; }
sln ".slnx blocked"                2 "$CLAUDE_PROJECT_DIR/PineGuard.slnx"
sln ".sln blocked"                 2 "$CLAUDE_PROJECT_DIR/Old.sln"
sln ".csproj allowed"              0 "$CLAUDE_PROJECT_DIR/src/A/A.csproj"
bash "$HOOKS/coordination.sh" force sln-edit "test" >/dev/null
sln "token overrides once"         0 "$CLAUDE_PROJECT_DIR/PineGuard.slnx"
sln "token is consumed"            2 "$CLAUDE_PROJECT_DIR/PineGuard.slnx"
ALLOW_SLN_EDIT=1 sln "legacy env still honoured" 0 "$CLAUDE_PROJECT_DIR/PineGuard.slnx"

# ── dotnet lock ──────────────────────────────────────────────────────────────
lock()   { expect "$1" "$2" pre-bash-lock.sh command "$3" "${4:-session-A}"; }
unlock() { payload command "$1" "${2:-session-A}" | bash "$HOOKS/post-bash-lock.sh" >/dev/null 2>&1; }
held()   { [ -d "$CLAUDE_PROJECT_DIR/.claude/run/locks/dotnet-ops.lock" ]; }
check()  { if "$@"; then PASS=$((PASS + 1)); else FAIL=$((FAIL + 1)); echo "FAIL check: $*"; fi; }
nothld() { ! held; }

lock "non-dotnet command passes"    0 'git status'
check nothld
lock "A takes the lock"             0 'dotnet build X.slnx'
check held
lock "B is blocked"                 2 'dotnet test X.slnx' session-B
lock "B with marker passes"         0 'dotnet test X.slnx # FORCE-LOCK' session-B
unlock 'dotnet test X.slnx' session-B
check held
unlock 'dotnet build X.slnx'
check nothld
lock "multi-line command is seen"   0 $'export PATH=x:$PATH\ncd repo && dotnet test "a b.csproj" -c Release'
check held
unlock $'export PATH=x:$PATH\ncd repo && dotnet test "a b.csproj" -c Release'
check nothld
lock "dotnet format is gated"       0 'dotnet format X.slnx --verify-no-changes'
lock "second session blocked"       2 'dotnet restore' session-B
CLAUDE_CODE_SESSION_ID=session-B bash "$HOOKS/coordination.sh" lock-release >/dev/null
check held
bash "$HOOKS/coordination.sh" lock-release >/dev/null
check nothld
lock "A takes the lock again"       0 'dotnet build X.slnx'
CLAUDE_CODE_SESSION_ID=session-B bash "$HOOKS/coordination.sh" lock-release --force "holder is dead" >/dev/null
check nothld

# with-lock holds for the command and releases afterwards, including on failure
bash "$HOOKS/coordination.sh" with-lock -- bash -c "test -d '$CLAUDE_PROJECT_DIR/.claude/run/locks/dotnet-ops.lock'"
check test $? -eq 0
check nothld
bash "$HOOKS/coordination.sh" with-lock -- bash -c 'exit 7'
check test $? -eq 7
check nothld
lock "with-lock command is not double-locked" 0 'bash .claude/hooks/coordination.sh with-lock -- dotnet test X'
check nothld

# an expired lock is taken over; Stop prunes but does not release a live lock
lock "A takes the lock"             0 'dotnet build X.slnx'
echo 1 > "$CLAUDE_PROJECT_DIR/.claude/run/locks/dotnet-ops.lock/expires"
lock "B takes over an expired lock" 0 'dotnet build X.slnx' session-B
payload x y session-B | bash "$HOOKS/on-stop.sh" >/dev/null 2>&1
check held
payload x y session-B | bash "$HOOKS/on-session-end.sh" >/dev/null 2>&1
check nothld

# ── advisory hooks never block ───────────────────────────────────────────────
expect "format reminder"   0 post-edit-format-check.sh   file_path "$CLAUDE_PROJECT_DIR/src/A.cs"
expect "testdata reminder" 0 post-edit-testdata-check.sh file_path "$CLAUDE_PROJECT_DIR/tests/ATestData.cs"

# ── wiring: every configured hook command must be able to start ──────────────
# The command strings in .claude/settings.json and .codex/hooks.json run through a shell, so an
# unquoted $CLAUDE_PROJECT_DIR word-splits on any checkout path that contains a space. On this
# repository's own working copy ("D:\Steve McCormack\...") that made bash look for "D:\Steve"
# and every hook exited 127 — never run, never seen. The directory below has a space on purpose.
REPO_ROOT="$(cd "$HOOKS/../.." && pwd)"
WIRE_DIR="$WORK/wire dir"
WIRE_PAYLOAD="$WORK/wire-payload.json"
mkdir -p "$WIRE_DIR"
git -C "$WIRE_DIR" init -q
payload command 'echo hi' > "$WIRE_PAYLOAD"

for surface in .claude .codex; do
  [ -d "$REPO_ROOT/$surface/hooks" ] || continue
  mkdir -p "$WIRE_DIR/$surface"
  cp -R "$REPO_ROOT/$surface/hooks" "$WIRE_DIR/$surface/hooks"
done

for config in "$REPO_ROOT/.claude/settings.json" "$REPO_ROOT/.codex/hooks.json"; do
  [ -f "$config" ] || continue
  while IFS= read -r cmd; do
    [ -n "$cmd" ] || continue
    ( cd "$WIRE_DIR" && export CLAUDE_PROJECT_DIR="$WIRE_DIR" && eval "$cmd" )       < "$WIRE_PAYLOAD" >/dev/null 2>&1
    got=$?
    case "$got" in
      126|127)
        FAIL=$((FAIL + 1))
        echo "FAIL [wiring] $(basename "$config"): exit $got — the hook could not be started"
        echo "      $cmd"
        ;;
      *) PASS=$((PASS + 1)) ;;
    esac
  done <<EOF
$(python3 -c 'import json, sys
hooks = json.load(open(sys.argv[1], encoding="utf-8")).get("hooks", {})
for entries in hooks.values():
    for entry in entries:
        for hook in entry.get("hooks", []):
            if hook.get("type") == "command":
                print(hook["command"])' "$config")
EOF
done

echo "hooks: $PASS passed, $FAIL failed"
[ "$FAIL" -eq 0 ]
