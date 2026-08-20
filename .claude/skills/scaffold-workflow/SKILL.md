---
name: scaffold-workflow
description: Author a new agent playbook in docs/ai/agents/ and cascade it to every adapter surface. Use whenever the user says "add a new agent", "create a workflow", "add a slash command", "scaffold an agent", or wants a new command wired across Claude, Antigravity, Pi and Copilot.
argument-hint: "[AgentName]"
context: fork
allowed-tools: Read, Write, Edit, Glob, Grep, Bash
metadata:
  author: stevomccormack
  version: 1.0.0
  category: meta
---
# Skill: Scaffold Agent Workflow

## Step 0: Load Specifications (MANDATORY)
Read these files:
1. `docs/ai/specs/protocol.md` (Brain/Adapter contract — Rule #1: adapters carry no logic)
2. `docs/ai/meta/adapter-surfaces.md` (the single inventory of surfaces and the cascade checklist)
3. `docs/ai/skills/scaffold-workflow/SKILL.md` (canonical recipe)

## Step 1: Follow the Recipe
Execute `docs/ai/skills/scaffold-workflow/SKILL.md` exactly as written.
Do NOT maintain a competing list of adapter surfaces — work the §5 cascade checklist of
`docs/ai/meta/adapter-surfaces.md` row by row.

## Step 2: Verify
- The playbook exists at `docs/ai/agents/<name>.md` with a business unit and a canonical role
- Every cascade row is either done or N/A under a declared policy exception
- Each adapter file is a pointer only — no restated steps