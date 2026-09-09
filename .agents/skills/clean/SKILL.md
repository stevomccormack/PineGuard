---
name: clean
description: Resolve a /clean-* command (clean-all, clean-artifact, clean-log) to its target agent and remove the matching generated output. Use whenever the user says "clean the artifacts", "clear the logs", "clean everything", or names one of the /clean-* family members directly. Nothing under version control is ever touched; deletion is destructive, so preview with -WhatIf if the scope is unclear.
argument-hint: "[Target]"
context: fork
allowed-tools: Read, Bash, Glob, Grep
metadata:
  author: stevomccormack
  version: 1.0.0
  category: routing
---
# Skill: Clean

## Step 0: Load the Intent Table (MANDATORY)
Read `docs/ai/commands/clean.md` — the authoritative intent table for the `/clean-*` command
family: `all` (both safe zones), `artifact` (`artifacts/` — coverage results, generated output,
analysis data), and `log` (`logs/` — testing and run logs).

## Step 1: Resolve the Row
Match the user's requested target to the matching row. If the user named a `/clean-*` command
directly, resolve it by name.

## Step 2: Execute the Resolved Agent
Read and follow `docs/ai/agents/clean-{target}.md` exactly as written. Each agent is Tier 2 only
because it is confined to its declared safe zone — read `docs/ai/specs/safety.md` §7.3 before
running, and use `-WhatIf` first if the scope is unclear.

## Worked Example
User: "clear the logs"
1. Read `docs/ai/commands/clean.md` → row `/clean-log` → `logs/` → `docs/ai/agents/clean-log.md`.
2. Read and execute `docs/ai/agents/clean-log.md`.

## References
- `docs/ai/commands/clean.md` (intent table)
- `docs/ai/specs/safety.md` §7.3 (Tier 2 safe-zone deletion)
