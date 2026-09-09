---
name: format
description: Resolve a /format-* command (format-all, format-core, format-must, format-guard, format-fluent, format-annotation, format-testing) to its scoped agent and enforce .editorconfig via dotnet format over that project. Use whenever the user says "format Core", "run dotnet format on Guard", names one of the /format-* family members directly, or wants formatting enforced on one specific project rather than the whole solution. Formatting is whitespace-and-style only and is auto-approved on every surface.
argument-hint: "[Scope]"
context: fork
allowed-tools: Read, Glob, Grep, Bash
metadata:
  author: stevomccormack
  version: 1.0.0
  category: routing
---
# Skill: Format

## Step 0: Load the Intent Table (MANDATORY)
Read `docs/ai/commands/format.md` — the authoritative intent table for the `/format-*` command
family: `all`, `core`, `must`, `guard`, `fluent`, `annotation`, `testing`.

## Step 1: Resolve the Row
Match the user's requested scope to the matching row. If the user named a `/format-*` command
directly, resolve it by name.

## Step 2: Execute the Resolved Agent
Read and follow `docs/ai/agents/format-{scope}.md` (or `docs/ai/agents/format-all.md`) exactly as
written. Its canonical procedure is `docs/ai/skills/format-code/SKILL.md`, mirrored on this surface
as the `format-code` skill.

## Worked Example
User: "format-must"
1. Read `docs/ai/commands/format.md` → row `/format-must` → `MustClauses` →
   `docs/ai/agents/format-must.md`.
2. Read and execute `docs/ai/agents/format-must.md`.

## References
- `docs/ai/commands/format.md` (intent table)
- `docs/ai/workflows/format.md` (shared orchestration)
- `format-code` skill (canonical procedure)
