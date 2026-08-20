---
spec:
  id: pineguard.ai.safety
  title: "Destructive Operations Safety"
  version: 1
  parent:
    - spec.md
  dependencies:
    - dependencies.md
applies_to:
  - "**/*"
---

# Destructive Operations Safety

This specification governs every operation an AI agent may execute that could destroy, overwrite, or irrecoverably alter user work. It applies to all agents, all adapters, and all sessions — regardless of the model or tool being used.

**Why this exists**: Real incidents have occurred where AI agents destroyed uncommitted work via broad `git restore`, `git reset --hard`, recursive file deletion, and indiscriminate `git add .` staging. This spec exists to prevent recurrence.

**Scope**: All AI-initiated commands — git, filesystem (PowerShell, Bash, cmd), build tools, process management, and environment mutation. This spec is cross-cutting and takes precedence over convenience in every case.

**Non-negotiable principle**: When in doubt, stop and ask. The cost of pausing is seconds. The cost of lost work is hours.

---

## 1. Three-Tier Classification

Every command an AI agent may execute falls into exactly one tier.

### Tier 0 — NEVER

Absolute prohibition. These commands are **never permitted** regardless of stashing, user confirmation, turbo mode, or any other protocol. If an agent needs the effect of a Tier 0 command, it must explain what it would do and why, and let the user run it manually.

### Tier 1 — ASK FIRST

Sometimes necessary, but carries real risk. The agent must:

1. Explain what it intends to do and why.
2. Show the blast radius (what files or state will be affected).
3. Get explicit user confirmation before proceeding.
4. Follow the applicable safety protocol (stash, backup, etc.).

### Tier 2 — SAFE WITH PROTOCOL

Permitted without user confirmation, but the agent must follow the documented rules (scope, provenance, sequencing). Failure to follow the protocol demotes the operation to Tier 1.

### Classifying unlisted commands

If a command is not listed below, classify it by answering these questions in order:

1. Can it delete, overwrite, or discard data that cannot be regenerated? → **Tier 0** if broad, **Tier 1** if scoped.
2. Can it modify shared state visible outside this session (push, publish, send)? → **Tier 1**.
3. Is it read-only or purely additive within the repo? → **Tier 2**.

---

## 2. Git Operations

### 2.1 Tier 0 — NEVER

| Command | Why |
|---------|-----|
| `git restore .` | Wipes the entire working tree silently |
| `git restore --staged .` | Unstages everything — can cause data loss when combined with other commands |
| `git reset --hard` (any variant) | Discards all staged and unstaged changes irrecoverably |
| `git checkout -- .` | Mass-restores all files from HEAD, discarding all edits |
| `git checkout -- <broad-glob>` | Same as above but with a glob — equally destructive |
| `git clean -fd` | Deletes all untracked files and directories without confirmation |
| `git clean -fdx` / `git clean -fdX` | Deletes untracked AND ignored files (build artifacts, local configs, `.env`) |
| `git push --force` to `main` or `master` | Rewrites shared history — can destroy other contributors' work |
| `git branch -D main` / `git branch -D master` | Deletes protected branches |
| `git rebase` on `main` / `master` | History rewriting on shared branches |
| `git merge --abort` when user has manually resolved conflicts | Discards the user's conflict resolution work |

### 2.2 Tier 1 — ASK FIRST

| Command | When it may be needed |
|---------|----------------------|
| `git restore <specific-file>` | Reverting a single file the agent broke |
| `git restore --staged <specific-file>` | Unstaging a specific file |
| `git reset --soft HEAD~1` | Undoing the agent's own last commit (preserves changes) |
| `git reset --mixed HEAD~1` | Undoing commit + unstaging (preserves working tree) |
| `git stash drop` / `git stash clear` | Cleaning up stashes — may contain user's saved work |
| `git rebase` on feature branches | History rewriting — can lose commits if conflicts arise |
| `git cherry-pick` when conflicts are likely | May require user judgment to resolve |
| `git push --force-with-lease` on non-protected branches | Safer than `--force` but still rewrites remote history |
| `git clean -n` followed by selective `git clean` | Preview then act — the preview step is mandatory |

### 2.3 Tier 2 — SAFE WITH PROTOCOL

| Command | Protocol |
|---------|----------|
| `git status` / `git diff` / `git log` | Always safe (read-only) |
| `git stash push -u -m "..."` | Always safe — this IS the safety mechanism |
| `git add <specific-files>` | Only files the agent created or modified in this session |
| `git commit -m "..."` | Only after verifying staged files are exclusively the agent's work |
| `git fetch` / `git pull` | Safe — handle merge conflicts if they arise |

### 2.4 Staging Rule — Always Add by Name

**`git add .` / `git add -A` / `git commit -a` are prohibited.** These are not classified in any tier — they are simply never used. The agent cannot distinguish its own files from the user's WIP when using broad staging commands.

Always use: `git add <file1> <file2> ...`

### 2.5 Stash-First Protocol

Before any Tier 1 git operation, follow this protocol:

**Step 1 — Preflight (always)**

```bash
git status
git diff
git diff --staged
```

Display the output so the user can see what exists.

**Step 2 — Create a stash (always)**

```bash
git stash push -u -m "pre-destructive: <reason>"
```

- `-u` is required — it captures untracked files.
- The message must start with `pre-destructive:` so stashes are findable.
- Log the stash hash and message in the output.

**Step 3 — Extra protection for ignored files (when applicable)**

If the operation may affect ignored files (`git clean -x`, etc.):

- Create an explicit backup (commit to a temporary WIP branch, or copy the directory outside the repo).
- `git stash -u` does NOT capture ignored files.

**Step 4 — Verify and proceed**

```bash
git status  # confirm everything is captured
```

Only then execute the Tier 1 command.

**Step 5 — Verify outcome**

```bash
git status  # confirm the operation completed as expected
```

---

## 3. Filesystem Operations

### 3.1 Tier 0 — NEVER

| PowerShell | Bash | cmd | Why |
|------------|------|-----|-----|
| `Remove-Item -Recurse` on dirs not created by agent | `rm -rf` / `rm -r` | `rd /s /q` | Recursive deletion of directories the agent does not own |
| `Remove-Item` with wildcards outside safe zones | `rm *.cs` / `rm *` | `del *.*` | Wildcard deletion is inherently unscoped |
| `Clear-Content` on source files | `> file.cs` / `truncate` | `type nul > file.cs` | Silently empties files |
| `Move-Item -Force` overwriting source files | `mv -f` over source | `move /y` | Overwrites without backup |
| Any operation targeting `.git/` | Any operation targeting `.git/` | — | Repository corruption |
| `Set-Content` / `Out-File` to source files without reading first | `>` redirect to source files | — | Silent overwrite of content the agent hasn't seen |

### 3.2 Tier 1 — ASK FIRST

| Operation | When it may be needed |
|-----------|----------------------|
| `Remove-Item` on specific files the agent did not create | Cleanup of obsolete files at user's request |
| `Move-Item` / `Rename-Item` on source files | Refactoring file structure |
| Overwriting an existing file with entirely new content | Even if the agent is "improving" it |
| Running cleanup scripts with `-Root` or `-Recursive` flags | These scripts may have broad scope |

### 3.3 Tier 2 — SAFE WITH PROTOCOL

| Operation | Protocol |
|-----------|----------|
| Creating new files in appropriate directories | Standard workflow |
| Deleting files the agent created in the current session | Must pass provenance check (see §7.1 and §7.4) |
| Writing to `artifacts/` and `logs/` | These are safe zones (`.gitignore`d, ephemeral) |
| Creating directories under `artifacts/` or `logs/` | Safe zones |

### 3.4 The Safe Zone Principle

`artifacts/` and `logs/` are `.gitignore`d and designed to be ephemeral. AI agents have broad permissions in these directories only. Everything outside safe zones requires specificity.

---

## 4. Build Tool Operations

### 4.1 Tier 1 — ASK FIRST (Destructive Build Commands)

| Command | Why |
|---------|-----|
| `dotnet clean` | Deletes `bin/` and `obj/` — forces full rebuild, can destroy local tool state |
| `npm prune` / `npm ci` | Deletes and reinstalls `node_modules/` |
| `nuget locals all -clear` | Wipes the global NuGet cache |
| `dotnet tool uninstall` | Removes global or local tools |
| Any package manager command that removes packages | May break the build until restored |

### 4.2 Tier 2 — SAFE WITH PROTOCOL (Constructive Build Commands)

| Command | Notes |
|---------|-------|
| `dotnet build` / `dotnet test` / `dotnet restore` | Constructive — always safe |
| `dotnet format` | Modifies files but in a controlled, reviewable way |
| `dotnet add package` / `npm install` | Additive operations |
| `pwsh -File ./tools/**/*.ps1` (vetted scripts) | See §8 for subagent rules |

---

## 5. Process Management

### 5.1 Tier 0 — NEVER

| Command | Why |
|---------|-----|
| `Stop-Process` / `kill` / `taskkill` on processes the agent did not start | May kill user's dev server, IDE, or other critical processes |
| Wildcard process killing (`Stop-Process -Name *`, `taskkill /IM *`) | Catastrophic — kills everything |
| Killing IDE processes (`devenv`, `rider`, `code`, `idea64`) | Destroys the user's editing session and unsaved work |

### 5.2 Tier 1 — ASK FIRST

| Command | When it may be needed |
|---------|----------------------|
| Killing a process the agent started that appears hung | e.g., `dotnet test` exceeding timeout |

### 5.3 Tier 2 — SAFE

| Command | Notes |
|---------|-------|
| Starting processes via documented scripts | Normal workflow |
| Starting `dotnet test`, `dotnet build`, `pwsh` invocations | Constructive operations |

---

## 6. Environment Mutation

### 6.1 Tier 0 — NEVER

| Operation | Why |
|-----------|-----|
| Modifying `PATH` (system or user) | Affects all processes on the machine |
| Writing to Windows Registry | Machine-level state change |
| `git config --global` | Affects all repositories on the machine |
| Modifying machine-level environment variables | Affects all processes |
| `dotnet tool install -g` / `npm install -g` without user request | Installs global tools the user didn't ask for |
| Modifying shell profiles (`.bashrc`, `.zshrc`, PowerShell `$PROFILE`) | Persistent state change across all sessions |
| Modifying IDE settings files outside the repo | VS Code user settings, Rider settings, etc. |

### 6.2 Tier 1 — ASK FIRST

| Operation | When it may be needed |
|-----------|----------------------|
| `dotnet tool install` (local, without `-g`) | Adding a tool the task requires |
| Modifying `.editorconfig` | Changing formatting rules |
| Modifying `Directory.Build.props` / `Directory.Packages.props` | Changing build-level configuration |
| Modifying NuGet sources | Adding or removing package feeds |

### 6.3 Tier 2 — SAFE

| Operation | Notes |
|-----------|-------|
| Setting process-scoped environment variables for one command | e.g., `DOTNET_CLI_TELEMETRY_OPTOUT=1 dotnet test` |
| Reading environment variables | Read-only, always safe |

---

## 7. Scope Containment

### 7.1 Session Provenance Principle

An agent may only delete or destructively modify files that meet **all** of the following:

1. The agent created or modified the file in the **current session**.
2. The file is not in a protected directory (see §7.2).
3. The file has not been committed to git.

AI agents do not have persistent memory across sessions. A file created by a previous session is indistinguishable from a file the user created. The agent must not assume ownership of files from prior sessions.

### 7.2 Protected Directories

These directories contain source-of-truth content. An agent must never delete files here — it may only modify file contents when explicitly instructed to implement a feature:

- `src/**` — production source code
- `tests/**` — test source code
- `docs/**` — documentation and AI specs
- `.github/**` — CI/CD configuration
- `.git/**` — repository internals
- `tools/**` — repo tooling scripts (agent may create files here but must not delete existing ones)
- Root-level files (`*.sln`, `*.md`, `*.json`, `.gitignore`, `.editorconfig`, etc.)

### 7.3 Safe Zones

These directories are `.gitignore`d and ephemeral. Agents have broad create/delete permissions here:

- `artifacts/**` — generated outputs, coverage reports, build artifacts
- `logs/**` — runtime and test logs

### 7.4 The "I Created It" Deletion Rule

This rule is valid **only within the current session** and **only for uncommitted files**. Before deleting, the agent must verify:

1. The file is in its mental provenance list (it created or generated the file this session).
2. The file is untracked by git (`git status` shows it as `??`).
3. The file is not in a protected directory that the user might have modified since creation.

If any check fails → **Tier 1** (ask the user).

---

## 8. Subagent and Child Process Safety

### 8.1 The Inheritance Problem

When an agent spawns a child process or delegates to a subagent, the safety rules are **not automatically inherited**:

- A PowerShell script may internally call `Remove-Item -Recurse`.
- A turbo-mode workflow may auto-approve commands that require confirmation.
- A downstream agent may not have read this spec.

### 8.2 Vetted Scripts Only

Agents should only invoke scripts under `tools/**` that have been reviewed and are known to be safe. The agent must not blindly execute scripts it has not inspected.

### 8.3 No Laundry Pattern

The agent must not generate a script that contains Tier 0 commands and then execute that script. Wrapping a prohibited command in a script does not bypass the prohibition. The agent is responsible for the full call chain.

### 8.4 Turbo Mode Restrictions

The `// turbo` and `// turbo-all` annotations in workflow files apply **only to Tier 2 operations**:

- Tier 0 operations are **never** turbo-eligible.
- Tier 1 operations are **never** turbo-eligible.
- Turbo mode is valid for: `dotnet build`, `dotnet test`, read-only git commands, and other constructive operations.

---

## 9. Common Gotchas

Real scenarios where AI agents have caused damage or nearly caused damage. Each includes the correct action.

### 9.1 Relative path confusion

**Scenario**: Agent is in `src/PineGuard.Core/` and runs `Remove-Item ../PineGuard.MustClauses/SomeFile.cs`.
**Why it seems reasonable**: The agent thinks it is cleaning up a generated file.
**What actually happens**: Shell working directory may have reset between commands. The relative path resolves differently than expected.
**Correct action**: Always use absolute paths or repo-root-relative paths. Never rely on `cd` state persisting between commands.

### 9.2 Recursive glob matching too broadly

**Scenario**: Agent runs `Get-ChildItem -Recurse -Filter "*.generated.cs" | Remove-Item` from the repo root.
**Why it seems reasonable**: The agent wants to clean up generated files.
**What actually happens**: The filter matches hand-written files that happen to have "generated" in their name.
**Correct action**: Delete specific files by full path. Never use recursive glob + delete pipelines.

### 9.3 `git add .` after `dotnet format`

**Scenario**: Agent runs `dotnet format` which touches 50 files, then runs `git add .`.
**Why it seems reasonable**: The agent wants to commit the formatting changes.
**What actually happens**: `git add .` also stages the user's unrelated WIP changes. The commit bundles everything together.
**Correct action**: Use `git add <specific-files>` or `git diff --name-only` to identify only the formatted files.

### 9.4 `dotnet clean` before `dotnet test`

**Scenario**: Agent runs `dotnet clean` to "start fresh" before testing.
**Why it seems reasonable**: Ensures no stale build artifacts.
**What actually happens**: Deletes all `bin/` and `obj/` directories, forcing a full rebuild. Wastes time and can fail if NuGet sources are unavailable.
**Correct action**: Just run `dotnet build` then `dotnet test`. The build system handles incremental compilation. Only use `dotnet clean` if explicitly asked.

### 9.5 Killing all dotnet processes

**Scenario**: Agent observes `dotnet test` hanging and runs `Stop-Process -Name dotnet`.
**Why it seems reasonable**: The test run is stuck.
**What actually happens**: Kills ALL dotnet processes, including the user's running development server in another terminal.
**Correct action**: Ask the user. If permitted, kill only the specific process by PID, not by name.

### 9.6 Output redirect overwrites existing file

**Scenario**: Agent runs `dotnet test > results.txt` and the redirect overwrites an existing `results.txt`.
**Why it seems reasonable**: The agent wants to capture test output.
**What actually happens**: The existing file contained the user's baseline comparison data.
**Correct action**: Use timestamped filenames (`results-20260226.txt`) or append (`>>`) instead of overwrite (`>`).

### 9.7 Shell state does not persist between commands

**Scenario**: Agent runs `cd src/PineGuard.Core` in one command, then in the next command runs `Remove-Item ./Utils/SomeFile.cs`.
**Why it seems reasonable**: The agent thinks it is still in the Core directory.
**What actually happens**: Working directory has reset to the repo root. The command deletes a different file or fails.
**Correct action**: Use absolute paths. Never rely on `cd` persisting between separate shell invocations.

### 9.8 `git stash pop` creates merge conflicts

**Scenario**: Agent stashes user work, makes its own changes, then runs `git stash pop`.
**Why it seems reasonable**: Restoring the user's original state.
**What actually happens**: The stash conflicts with the agent's changes. The agent then runs `git checkout --theirs .` to "resolve" conflicts, which discards the user's stashed changes.
**Correct action**: Use `git stash apply` (not `pop`) so the stash is preserved. If conflicts arise, stop and ask the user to resolve them.

### 9.9 Cleanup scripts with broad flags

**Scenario**: Agent runs `Clean-Root.ps1 -Recursive -All` without checking what it does.
**Why it seems reasonable**: The agent was asked to clean up.
**What actually happens**: The script with those flags scans the entire repository and deletes files matching a default extension filter that may be too broad.
**Correct action**: Read the script first. Use the narrowest scope possible. Prefer `Clean-Artifacts.ps1` or `Clean-Logs.ps1` which target safe zones.

### 9.10 Package restore modifies lockfiles

**Scenario**: Agent runs `dotnet restore`. The lockfile (`packages.lock.json`) changes due to version drift.
**Why it seems reasonable**: Restore is needed before build.
**What actually happens**: The lockfile change gets bundled into the agent's commit, introducing unintended dependency updates.
**Correct action**: After restore, check `git diff` for lockfile changes. If lockfiles changed, either exclude them from the commit or flag the change to the user.

---

## 10. Recovery Guidance

### 10.1 Git Recovery

| Situation | Recovery command |
|-----------|----------------|
| Stash exists from pre-destructive protocol | `git stash list` → `git stash apply stash@{n}` (use `apply`, not `pop`) |
| File was in index but not committed | `git fsck --lost-found` may recover blobs |
| Accidental commit needs undoing | `git revert HEAD` (creates a new undo commit — do NOT use `reset --hard`) |
| Tracked file deleted | `git checkout HEAD -- <path>` restores from last commit |
| Accidental force push | `git reflog` on local machine — the old HEAD is usually still there |

### 10.2 Filesystem Recovery

| Situation | Recovery method |
|-----------|----------------|
| File was in `artifacts/` or `logs/` | Regenerable — re-run the build/test/coverage command |
| Source file deleted (was tracked) | `git checkout HEAD -- <path>` |
| Source file deleted (was untracked) | Check Recycle Bin (Windows) or `trash` (macOS/Linux) |
| File overwritten | `git diff` to see damage, `git checkout HEAD -- <path>` to restore from last commit |

### 10.3 Recovery Protocol (Step-by-Step)

1. **Stop immediately.** Do not run additional commands to "fix" the damage — this often makes it worse.
2. Run `git status` and `git stash list` to assess the situation.
3. Document what happened: what command was run, what the agent was trying to do.
4. If a stash exists from the pre-destructive protocol, apply it with `git stash apply`.
5. If no stash exists, use `git reflog` and `git fsck` to locate lost data.
6. If filesystem data was lost outside git, check OS-level recovery (Recycle Bin, File History, Time Machine).
7. Record any work that needs to be redone: update the relevant plan under `docs/ai/plans/`, and write the lesson to `.claude/agent-memory/` so the next session does not repeat it.

---

## 11. Quick Reference

Single scannable table for rapid lookup. **If you read nothing else, read this.**

| Command / Pattern | Tier | Action |
|-------------------|------|--------|
| `git restore .` | 0 | NEVER |
| `git restore <file>` | 1 | Ask first |
| `git reset --hard` | 0 | NEVER |
| `git reset --soft HEAD~1` | 1 | Ask first |
| `git checkout -- .` | 0 | NEVER |
| `git clean -fd` / `-fdx` | 0 | NEVER |
| `git push --force` to main | 0 | NEVER |
| `git push --force-with-lease` (feature branch) | 1 | Ask first |
| `git add .` / `git add -A` / `git commit -a` | — | Prohibited (use named files) |
| `git add <specific-file>` | 2 | Safe — agent's own files only |
| `git stash push -u -m "..."` | 2 | Always safe |
| `git stash drop` / `git stash clear` | 1 | Ask first |
| `git status` / `git diff` / `git log` | 2 | Always safe (read-only) |
| `Remove-Item -Recurse` (not agent-created) | 0 | NEVER |
| `Remove-Item` with wildcards outside safe zones | 0 | NEVER |
| `Remove-Item <specific-file>` (not agent-created) | 1 | Ask first |
| Delete agent-created file (current session) | 2 | Safe — verify provenance |
| Write to `artifacts/` or `logs/` | 2 | Safe zone |
| `dotnet clean` | 1 | Ask first |
| `dotnet build` / `dotnet test` | 2 | Always safe |
| `Stop-Process` (not agent-started) | 0 | NEVER |
| `git config --global` | 0 | NEVER |
| Modify PATH / registry / shell profile | 0 | NEVER |
| Modify `.editorconfig` / `Directory.Build.props` | 1 | Ask first |
| `// turbo` on Tier 0 or Tier 1 commands | — | Prohibited |

<!-- footer
last_verified: 2026-02-26
-->
