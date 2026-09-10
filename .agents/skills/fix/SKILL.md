---
name: fix
description: Resolve a /fix-coverage-*, /fix-test-*, /fix-roslyn-all, or /fix-sonar-{severity} command to its exact agent playbook — e.g. "fix the coverage gaps in Core", "fix the failing Guard tests", "fix-sonar-blocker", "fix all Roslyn warnings". Use whenever the user names one of these command-family members directly, or asks to close coverage gaps, repair failing tests, resolve Roslyn warnings, or resolve Sonar findings for a specific project or severity. None of these commands is auto-approved.
argument-hint: "[Axis] [Scope-or-Severity]"
context: fork
allowed-tools: Read, Write, Edit, Glob, Grep, Bash
metadata:
  author: stevomccormack
  version: 1.0.0
  category: routing
---
# Skill: Fix

## Step 0: Load the Intent Table (MANDATORY)
Read `docs/ai/commands/fix.md` — the authoritative intent table for the `/fix-*` command family. It
covers three axes:
- **Coverage gaps**, scoped by project: `/fix-coverage-{all|core|must|guard|fluent|annotation|testing}`
- **Test failures**, scoped by project: `/fix-test-{all|core|must|guard|fluent|annotation|testing}`
- **Static-analysis findings**: `/fix-roslyn-all` (no scope variants) and
  `/fix-sonar-{all|blocker|high|medium|low}` (severity-scoped, not project-scoped)

## Step 1: Resolve the Row
Match the user's requested axis + scope/severity to the matching row. If the user named a `/fix-*`
command directly, resolve it by name.

## Step 2: Run the Matching Read-Only Report First
Per `fix.md`: run the matching read-only command first (`docs/ai/commands/coverage.md`,
`docs/ai/commands/test.md`, or `docs/ai/commands/scan.md` — or the `coverage`, `test`, or `scan`
skill) so the repair loop starts from a current report, unless one already exists from this
session.

## Step 3: Execute the Resolved Agent
Read and follow that row's agent playbook (`docs/ai/agents/fix-coverage-{scope}.md`,
`docs/ai/agents/fix-test-{scope}.md`, `docs/ai/agents/fix-roslyn-all.md`, or
`docs/ai/agents/fix-sonar-{severity}.md`) exactly as written. Always fix the root cause —
suppressing a diagnostic is never an acceptable fix.

## Step 4: Confirm Before Writing
None of these commands is auto-approved — confirm the repair plan with the user before making code
changes, per `docs/ai/specs/safety.md`.

## Worked Example
User: "fix-sonar-blocker"
1. Read `docs/ai/commands/fix.md` → row `/fix-sonar-blocker` → `SonarQube blockers` →
   `docs/ai/agents/fix-sonar-blocker.md`.
2. Read and execute `docs/ai/agents/fix-sonar-blocker.md`.

## References
- `docs/ai/commands/fix.md` (intent table)
- `docs/ai/commands/coverage.md`, `docs/ai/commands/test.md`, `docs/ai/commands/scan.md` (source reports)
