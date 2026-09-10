---
name: scan
description: Resolve a /scan-roslyn-*, /scan-qodana-*, or /scan-sonar command to its exact tool-and-scope agent playbook — e.g. "scan Core with Roslyn", "run Qodana on GuardClauses", "scan-qodana-fluent", "/scan-sonar". Use whenever the user names one of these command-family members directly or asks for a scoped static-analysis run against a specific project. For a generic, scope-less "run Roslyn" or "run Sonar" check, use the scan-roslyn or scan-sonar skill directly instead.
argument-hint: "[Tool] [Scope]"
context: fork
allowed-tools: Read, Glob, Grep, Bash
metadata:
  author: stevomccormack
  version: 1.0.0
  category: routing
---
# Skill: Scan

## Step 0: Load the Intent Table (MANDATORY)
Read `docs/ai/commands/scan.md` — the authoritative intent table for the `/scan-roslyn-*`,
`/scan-qodana-*`, and `/scan-sonar` command family. Every row pairs a tool (Roslyn, Qodana, Sonar)
and scope (`all`, `core`, `must`, `guard`, `fluent`, `annotation`, `testing` — Sonar has no
per-scope variants, it always analyses the whole solution) with its target agent.

## Step 1: Resolve the Row
Match the user's requested tool + scope to the matching row. If the user named a `/scan-*` command
directly, resolve it by name.

## Step 2: Execute the Resolved Agent
Read and follow that row's agent playbook (`docs/ai/agents/scan-{tool}-{scope}.md`, or
`docs/ai/agents/scan-sonar.md` for Sonar) exactly as written. All scans are read-only and
auto-approved.

## Step 3: Route Fixes Separately
If the scan reports findings the user wants repaired, hand off to the `fix` skill (or
`docs/ai/commands/fix.md` directly) — this skill only runs the scan. Note there is no
`fix-qodana` family by design: Qodana findings largely overlap the Roslyn/Sonar rule sets, so
remediation goes through those commands.

## Worked Example
User: "run Qodana on GuardClauses"
1. Read `docs/ai/commands/scan.md` → row `/scan-qodana-guard` → `Qodana, GuardClauses` →
   `docs/ai/agents/scan-qodana-guard.md`.
2. Read and execute `docs/ai/agents/scan-qodana-guard.md`.

## References
- `docs/ai/commands/scan.md` (intent table)
- `docs/ai/commands/fix.md` (repairing what a scan reports)
