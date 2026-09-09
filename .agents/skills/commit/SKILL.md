---
name: commit
description: Resolve a /commit-* command (commit-all, commit-core, commit-must, commit-guard, commit-fluent, commit-annotation, commit-testing, commit-doc, commit-agent, commit-tool, commit-solution) to its scoped agent and stage-and-commit that exact slice. Use whenever the user says "commit this", "commit the Core changes", "commit-core", "stage and commit X", names one of the /commit-* family members directly, or wants a conventional-commits commit scoped to one project/slice rather than the whole working tree. Do NOT use `git add -A` — only the resolved slice's files may be staged.
argument-hint: "[Slice]"
context: fork
allowed-tools: Read, Bash, Grep, Glob
metadata:
  author: stevomccormack
  version: 1.0.0
  category: routing
---
# Skill: Commit

## Step 0: Load the Intent Table (MANDATORY)
Read `docs/ai/commands/commit.md` — the authoritative intent table for the `/commit-*` command
family. It lists every member (`all`, `core`, `must`, `guard`, `fluent`, `annotation`, `testing`,
`doc`, `agent`, `tool`, `solution`) alongside the exact slice it stages and its target agent.

## Step 1: Resolve the Row
Match the user's requested slice (e.g. "Core", "the docs", "the tool scripts") to the matching row
in the intent table. If the user named a `/commit-*` command directly, resolve it by name instead.

## Step 2: Execute the Resolved Agent
Read and follow that row's agent playbook (`docs/ai/agents/commit-{slice}.md`) exactly as written.
Never substitute `git add -A` for the named slice — only the files belonging to that slice may be
staged.

## Step 3: Confirm Before Committing
Commits are never auto-approved: propose the conventional-commits message and the staged file list,
then wait for the user to confirm before running `git commit`.

## Worked Example
User: "commit the Core changes"
1. Read `docs/ai/commands/commit.md` → row `/commit-core` → `PineGuard.Core + its tests` →
   `docs/ai/agents/commit-core.md`.
2. Read and execute `docs/ai/agents/commit-core.md`.
3. Stage only `src/PineGuard.Core/**` and `tests/PineGuard.Core.UnitTests/**`, propose the
   conventional-commits message, and wait for confirmation.

## References
- `docs/ai/commands/commit.md` (intent table)
- `docs/ai/workflows/commit.md` (shared orchestration)
