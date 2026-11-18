# Coordination Rules

> Inherits from: `docs/ai/rules/global.md` (read first)

These rules prevent collisions when multiple agent sessions run simultaneously
(Claude Desktop, VS, JetBrains, Terminal, Antigravity, Copilot, Codex, or parallel subagents).

## Universal Contract

Every surface follows this, with or without tooling support:

1. **Announce scope before starting.** State which project/layer you are about to build, test, or
   cover, so a parallel session can pick something else.
2. **Never run concurrent `dotnet build` / `dotnet test` / coverage / Sonar scans.** They contend for
   the same obj/bin outputs and produce corrupt or misleading results.
3. **Never edit a file another session has claimed.** Two sessions writing the same file loses work.
4. **Never clear or force-release another session's status or lock.** If you are blocked, wait, then
   report the conflict to the user.
5. **Clear your own status when you finish or fail.** A stale claim blocks everyone else.

Surfaces without automation satisfy this by stating scope in the response and by asking the user
before running a long dotnet operation that another session may already be running.

## Claude Code Implementation (hook-backed)

Claude Code enforces the contract above through `.claude/hooks/coordination.sh`. Every session has a
unique ID (`$PPID` or `$CLAUDE_SESSION_ID`); the store lives in `.claude/run/` (gitignored,
auto-created). Other surfaces have neither and must not be told to run these commands.

### Status board

At the start of any non-trivial task:

```bash
bash "$CLAUDE_PROJECT_DIR/.claude/hooks/coordination.sh" status "agent-name" "brief description"
```

Example:
```bash
bash "$CLAUDE_PROJECT_DIR/.claude/hooks/coordination.sh" status "coverage-must-clauses" "fixing gaps in MustStringNumbersClauses"
```

At completion or on error:

```bash
bash "$CLAUDE_PROJECT_DIR/.claude/hooks/coordination.sh" status-clear
```

To see what all sessions are doing:

```bash
bash "$CLAUDE_PROJECT_DIR/.claude/hooks/coordination.sh" lock-status
```

### Build / test lock (automatic)

The `dotnet test`, `dotnet build`, `Run-CodeCoverage`, and `Run-SonarScanner`
commands are **automatically locked** by hooks — you do not need to acquire
locks manually. The hook will:

- Acquire the `dotnet-ops` lock before your command runs
- Release it after your command completes
- **Block your command** (exit 2) if another session holds the lock

When you see `⚠  dotnet-ops lock held by session ...`:

1. Wait ~30 seconds: `bash -c "sleep 30"`
2. Retry the original command
3. If blocked again after 3 retries, run `bash .claude/hooks/coordination.sh lock-status`
   and report the conflict to the user

Do NOT attempt to bypass the lock or forcibly delete `.claude/run/locks/`.

### Messaging (optional)

To send a message to another session (e.g., a parallel subagent):

```bash
# Get the target session's ID from lock-status first
bash "$CLAUDE_PROJECT_DIR/.claude/hooks/coordination.sh" send <session-id> "Subject" "Body text"
```

To check your inbox (messages expire after 120s):

```bash
bash "$CLAUDE_PROJECT_DIR/.claude/hooks/coordination.sh" messages
```

### Cleanup

If a session exits unexpectedly and leaves its own stale locks:

```bash
bash "$CLAUDE_PROJECT_DIR/.claude/hooks/coordination.sh" lock-release
```

## File Editing Conflicts

Multiple sessions editing the **same file simultaneously** will cause data loss.
Before editing a file another session might be touching:

1. Check the status board (Claude Code: `coordination.sh lock-status`; elsewhere: ask the user).
2. If another session is active on the same scope (e.g., both editing MustClauses),
   coordinate or wait for that session to complete.

## Sessions in Separate Containers

Cloud sessions and scheduled runs each start in their own container with a fresh clone. The status
board and locks above live in `.claude/run/`, which is local to one machine, so these sessions cannot
see each other through it. For them the only shared state is the git remote, and the claim lives
there.

A unit of planned work (a pilot, a workstream, a wave item) has one working branch and one tracker
page with an **Active runs** table: unit, working branch, session, chunk, started (UTC), heartbeat
(UTC).

1. **Claim first.** Before any other work, fetch and read the table at the remote tip of the working
   branch. If the unit's row shows another session with a heartbeat under 3 hours old, report and
   stop. Do not pick a different unit unless your instructions authorise it.
2. **Push the claim.** Otherwise write your session, the chunk and the current UTC time (`date -u`)
   into the row, commit only that change and push it as your first action. If the push is rejected,
   another session claimed first: abandon the claim commit, report and stop. Never force-push or
   merge to win a claim.
3. **Heartbeat.** Refresh the timestamp with every push, and at least every 60 minutes.
4. **Check before every later push.** Fetch and confirm the row still holds your session and the
   remote tip is your own last push. If not, stop pushing, report what is unpushed and leave the
   branch alone.
5. **Clear on exit.** Clear your row in your final commit, including when you stop on a failure.
6. **Stale claims.** A heartbeat over 3 hours old is stale. Take the row over and record in the
   tracker's run log which session's claim you replaced.
7. **Shared files.** The tracker, `docs/ai/rules/*`, `.claude/*`, the solution file,
   `Directory.Packages.props` and the CI workflow change only in the claiming session's own commits.

The first tracker to carry the table is
[`docs/ai/plans/technical-readiness-to-1.0/tracker.md`](../plans/technical-readiness-to-1.0/tracker.md).
