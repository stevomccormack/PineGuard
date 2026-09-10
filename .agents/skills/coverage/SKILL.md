---
name: coverage
description: Resolve a /coverage-* command (coverage-all, coverage-core, coverage-must, coverage-guard, coverage-fluent, coverage-annotation, coverage-testing) to its scoped agent and measure that project's line/branch coverage. Use whenever the user says "run coverage", "what's the coverage for Core", "check coverage on Guard", names one of the /coverage-* family members directly, or wants a coverage report for a specific project. This skill only measures coverage — for closing the gaps it reports, use the fix skill instead.
argument-hint: "[Scope]"
context: fork
allowed-tools: Read, Bash, Glob, Grep
metadata:
  author: stevomccormack
  version: 1.0.0
  category: routing
---
# Skill: Coverage

## Step 0: Load the Intent Table (MANDATORY)
Read `docs/ai/commands/coverage.md` — the authoritative intent table for the `/coverage-*` command
family: `all`, `core`, `must`, `guard`, `fluent`, `annotation`, `testing`. Every scope is a
first-class member of the `-Scope` ValidateSet in `tools/code-coverage/Run-CodeCoverage.ps1`.

## Step 1: Resolve the Row
Match the user's requested scope to the matching row. If the user named a `/coverage-*` command
directly, resolve it by name.

## Step 2: Execute the Resolved Agent
Read and follow `docs/ai/agents/coverage-{scope}.md` (or `docs/ai/agents/coverage-all.md`) exactly
as written. Coverage runs are read-only and auto-approved.

## Step 3: Route Gaps Separately
If coverage is below target and the user wants it closed, hand off to the `fix` skill
(`/fix-coverage-{scope}`) or the `improve-coverage` skill — this skill only measures.

## Worked Example
User: "what's the coverage on GuardClauses?"
1. Read `docs/ai/commands/coverage.md` → row `/coverage-guard` → `GuardClauses` →
   `docs/ai/agents/coverage-guard.md`.
2. Read and execute `docs/ai/agents/coverage-guard.md`.

## References
- `docs/ai/commands/coverage.md` (intent table)
- `docs/ai/workflows/coverage.md` (shared orchestration)
