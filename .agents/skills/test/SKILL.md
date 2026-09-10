---
name: test
description: Resolve a /test-* command (test-all, test-core, test-must, test-guard, test-fluent, test-annotation, test-testing) to its scoped agent and run that project's test suite. Use whenever the user says "run the tests", "test Core", "run the Guard tests", names one of the /test-* family members directly, or wants xUnit run for one specific project rather than the whole solution. This skill only runs tests — for diagnosing and repairing failures, use the fix skill instead.
argument-hint: "[Scope]"
context: fork
allowed-tools: Read, Bash, Glob, Grep
metadata:
  author: stevomccormack
  version: 1.0.0
  category: routing
---
# Skill: Test

## Step 0: Load the Intent Table (MANDATORY)
Read `docs/ai/commands/test.md` — the authoritative intent table for the `/test-*` command family:
`all`, `core`, `must`, `guard`, `fluent`, `annotation`, `testing` (the last runs
`tests/PineGuard.Testing.UnitTests`, the suite for the shared test-infrastructure library itself).

## Step 1: Resolve the Row
Match the user's requested scope to the matching row. If the user named a `/test-*` command
directly, resolve it by name.

## Step 2: Execute the Resolved Agent
Read and follow `docs/ai/agents/test-{scope}.md` (or `docs/ai/agents/test-all.md`) exactly as
written. All test runs are read-only and auto-approved.

## Step 3: Route Failures Separately
If tests fail and the user wants them fixed, hand off to the `fix` skill (`/fix-test-{scope}`) —
this skill only runs the suite.

## Worked Example
User: "run the FluentValidation tests"
1. Read `docs/ai/commands/test.md` → row `/test-fluent` → `FluentValidation` →
   `docs/ai/agents/test-fluent.md`.
2. Read and execute `docs/ai/agents/test-fluent.md`.

## References
- `docs/ai/commands/test.md` (intent table)
- `docs/ai/workflows/test.md` (shared orchestration)
