---
name: audit
description: Resolve a /audit-* command (audit-cli, audit-gap) to its target agent. Use whenever the user says "run the audit CLI", "check adapter parity", "audit-cli", "find coverage gaps and propose tests", "audit-gap", or names one of the /audit-* family members directly. Both commands are read-only and auto-approved.
argument-hint: "[Target]"
context: fork
allowed-tools: Read, Bash, Glob, Grep
metadata:
  author: stevomccormack
  version: 1.0.0
  category: routing
---
# Skill: Audit

## Step 0: Load the Intent Table (MANDATORY)
Read `docs/ai/commands/audit.md` — the authoritative intent table for the `/audit-*` command
family: `/audit-cli` (runs `pineguard audit`, the TypeScript CLI in `apps/cli`, including the
`test-files`/`doc-links`/`surface-parity` CI gate) and `/audit-gap` (analyses coverage gaps and
proposes the missing cases).

## Step 1: Resolve the Row
Match the user's requested intent to the matching row. If the user named a `/audit-*` command
directly, resolve it by name.

## Step 2: Execute the Resolved Agent
Read and follow `docs/ai/agents/audit-cli.md` or `docs/ai/agents/audit-gap.md` exactly as written.
Both are read-only and auto-approved.

## Worked Example
User: "check adapter parity across surfaces"
1. Read `docs/ai/commands/audit.md` → row `/audit-cli` → `docs/ai/agents/audit-cli.md`.
2. Read and execute `docs/ai/agents/audit-cli.md`.

## References
- `docs/ai/commands/audit.md` (intent table)
- `docs/ai/workflows/audit.md` (shared orchestration, used by `/audit-cli`)
