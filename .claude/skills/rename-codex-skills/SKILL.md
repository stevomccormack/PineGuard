---
name: rename-codex-skills
description: Strip the source-command- prefix that OpenAI Codex's session import stamps on .agents/skills/ folders, make each SKILL.md name match its directory, and commit only those folders. Use whenever the user says "Codex dumped source-command folders again", "remove the source-command prefix", "promote the Codex skills", "rename Codex skills", or git status shows .agents/skills/source-command-* entries. Do NOT use to author a new skill; use scaffold-workflow instead.
argument-hint: "[--remove-prefix \"source-command-\"] [-WhatIf]"
context: fork
allowed-tools: Read, Glob, Grep, Bash
metadata:
  author: stevomccormack
  version: 1.0.0
  category: maintenance
---
# Skill: Rename Codex Skills

## Step 0: Load Specifications (MANDATORY)
Read these files:
1. `docs/ai/specs/safety.md` (§2 git tiers: scoped staging, never `git add -A`, never unstage)
2. `docs/ai/meta/adapter-surfaces.md` (§2.1.1: what `.agents/skills/` may carry)
3. `docs/ai/skills/rename-codex-skills/SKILL.md` (canonical recipe)

## Step 1: Follow the Recipe
Execute `docs/ai/skills/rename-codex-skills/SKILL.md` exactly as written.

## Step 2: Verify
- No `source-command-*` folder remains except those reported as `skipped` or `excluded`
- Every promoted `SKILL.md` has `name:` equal to its directory name
- Exactly one `chore(agents)` commit, containing only `.agents/skills/<name>/` paths
- Skipped collisions are reported to the owner, never force-resolved
