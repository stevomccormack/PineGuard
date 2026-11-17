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
4. **Never clear or force-release a live session's status or lock.** If you are blocked, wait, then
   report the conflict to the user. A lock whose holder is dead (the session crashed, the container
   restarted) may be force-released with a recorded reason; see [FORCE override](#force-override).
5. **Clear your own status when you finish or fail.** A stale claim blocks everyone else.

Surfaces without automation satisfy this by stating scope in the response and by asking the user
before running a long dotnet operation that another session may already be running.

## Claude Code Implementation (hook-backed)

Claude Code enforces the contract above through `.claude/hooks/coordination.sh`. Every session has a
unique ID: the hook payload's `session_id`, else `$CLAUDE_CODE_SESSION_ID`, else `$CLAUDE_SESSION_ID`,
else `$PPID`. The first three name the same session from a hook and from a shell, so a lock taken by
one is released by the other. The store lives in `.claude/run/` (gitignored, auto-created). Other surfaces have neither and must not be told to run these commands.

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

The `dotnet test`, `build`, `publish`, `pack`, `restore`, `format`, `clean` and `run` commands, the
coverage scripts (`Run-CodeCoverage`, `New-CoverageReport`, `Test-Coverage`) and `Run-SonarScanner`
are **automatically locked** by hooks — you do not need to acquire locks manually. The hook will:

- Acquire the `dotnet-ops` lock before your command runs
- Release it after your command completes
- **Block your command** (exit 2) if another session holds the lock

A lock that nothing releases (a killed command) expires after 10 minutes
(`PINEGUARD_LOCK_TIMEOUT`, in seconds).

The hooks only see the command line. A wrapper script that runs `dotnet` inside it is not locked
unless you run it under the lock yourself:

```bash
bash "$CLAUDE_PROJECT_DIR/.claude/hooks/coordination.sh" with-lock -- bash path/to/script.sh
```

`with-lock` holds `dotnet-ops` for the whole command and releases it when the command exits,
whatever its exit code.

When you see `⚠  dotnet-ops lock held by session ...`:

1. Wait ~30 seconds: `bash -c "sleep 30"`
2. Retry the original command
3. If blocked again after 3 retries, run `bash .claude/hooks/coordination.sh lock-status`
   and report the conflict to the user

Do NOT delete `.claude/run/locks/` by hand. If the holder is dead, use the FORCE override below.

### FORCE override

Every blocking hook stops first and can then be overridden. The stop is the point: it makes the
agent look at what it is about to do. An override is for the case where it has looked and the
operation is intended. Every override is recorded in `.claude/run/force.log`.

| Hook | Blocks | Marker (Bash commands) | Hook name for a token or the environment |
|---|---|---|---|
| `pre-bash-destructive-guard.sh` | destructive git and filesystem commands | `# FORCE-CONFIRMED` | `destructive-guard` |
| `pre-bash-lock.sh` | a dotnet command while another session holds the lock | `# FORCE-LOCK` | `dotnet-lock` |
| `enforce-output-dirs.sh` | a new file in the project root | — | `output-dirs` |
| `block-sln-edit.sh` | a direct edit of `*.sln` / `*.slnx` | — | `sln-edit` |

Three forms, in the order the hooks check them:

1. **Marker** — repeat the same Bash command with the marker as a trailing comment:
   `git clean -fd  # FORCE-CONFIRMED`.
2. **One-use token** — for Write and Edit, where no comment fits, or for any hook:

   ```bash
   bash "$CLAUDE_PROJECT_DIR/.claude/hooks/coordination.sh" force <hook|all> "<why this is intended>"
   ```

   The next blocked call of that hook passes; the token then expires (10 minutes at most). A reason
   is required.
3. **Standing, for the owner** — `PINEGUARD_HOOK_FORCE=<hook>[,<hook>...]` or `=all`, set in the shell
   or under `env` in the settings file.

To release a lock left by a dead session, whoever holds it:

```bash
bash "$CLAUDE_PROJECT_DIR/.claude/hooks/coordination.sh" lock-release --force "<why the holder is dead>"
```

An override does not lift the permission rules in `.claude/settings.json`: a command on the deny list
stays denied.

The hooks have their own tests: `bash .claude/hooks/tests/test-hooks.sh`.

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

A session's locks and status are released when it ends (`SessionEnd`). Expired locks, expired
override tokens and status entries more than a day old are pruned at the end of every turn.

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
