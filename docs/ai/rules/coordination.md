> Inherits from: `docs/ai/rules/global.md`

# Coordination Rules

These rules prevent collisions when multiple Claude sessions run simultaneously
(Claude Desktop, VS, JetBrains, Terminal, or parallel Task subagents).

## Session Identity

Every Claude session has a unique ID (`$PPID` or `$CLAUDE_SESSION_ID`).
The coordination system lives in `.claude/run/` (gitignored, auto-created).

## Status Board

**At the start of any non-trivial task**, write your status so other sessions
know what you are doing:

```bash
bash "$CLAUDE_PROJECT_DIR/.claude/hooks/coordination.sh" status "agent-name" "brief description"
```

Example:
```bash
bash "$CLAUDE_PROJECT_DIR/.claude/hooks/coordination.sh" status "coverage-must-clauses" "fixing gaps in MustStringNumbersClauses"
```

**At completion or on error**, clear your status:

```bash
bash "$CLAUDE_PROJECT_DIR/.claude/hooks/coordination.sh" status-clear
```

## Build / Test Lock (Automatic)

The `dotnet test`, `dotnet build`, `Run-CodeCoverage`, and `Run-SonarScanner`
commands are **automatically locked** by hooks — you do not need to acquire
locks manually. The hook will:

- Acquire the `dotnet-ops` lock before your command runs
- Release it after your command completes
- **Block your command** (exit 2) if another session holds the lock

### When your command is blocked

If you see `⚠  dotnet-ops lock held by session ...`, the correct response is:

1. Wait ~30 seconds: `bash -c "sleep 30"`
2. Retry the original command
3. If blocked again after 3 retries, run `bash .claude/hooks/coordination.sh lock-status`
   and report the conflict to the user

Do NOT attempt to bypass the lock or forcibly delete `.claude/run/locks/`.

## Checking the Status Board

To see what all sessions are currently doing:

```bash
bash "$CLAUDE_PROJECT_DIR/.claude/hooks/coordination.sh" lock-status
```

## Messaging (Optional)

To send a message to another session (e.g., a parallel subagent):

```bash
# Get the target session's ID from lock-status first
bash "$CLAUDE_PROJECT_DIR/.claude/hooks/coordination.sh" send <session-id> "Subject" "Body text"
```

To check your inbox (messages expire after 120s):

```bash
bash "$CLAUDE_PROJECT_DIR/.claude/hooks/coordination.sh" messages
```

## File Editing Conflicts

Multiple sessions editing the **same file simultaneously** will cause data loss.
Before editing a file that another session might be touching:

1. Check the status board: `coordination.sh lock-status`
2. If another session is active on the same scope (e.g., both editing MustClauses),
   coordinate via messages or wait for the other session to complete

## Cleanup

If Claude exits unexpectedly and leaves stale locks:

```bash
bash "$CLAUDE_PROJECT_DIR/.claude/hooks/coordination.sh" lock-release
```
