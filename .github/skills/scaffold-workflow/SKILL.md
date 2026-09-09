---
name: scaffold-workflow
description: Author a new agent playbook in docs/ai/agents/ and cascade it to every adapter surface. Use whenever the user says "add a new agent", "create a workflow", "add a slash command", "scaffold an agent", or wants a new command wired across Claude, Antigravity, Copilot and OpenCode.
---
# Skill: Scaffold Agent Workflow

## Load First

Read these files before scaffolding a new workflow:
1. [protocol.md](../../../docs/ai/specs/protocol.md)
2. [adapter-surfaces.md](../../../docs/ai/meta/adapter-surfaces.md)
3. [scaffold-workflow SKILL.md](../../../docs/ai/skills/scaffold-workflow/SKILL.md)

## Execute

Follow the canonical recipe in [scaffold-workflow SKILL.md](../../../docs/ai/skills/scaffold-workflow/SKILL.md) exactly.

## Verify

- The playbook exists at `docs/ai/agents/<name>.md` with a business unit and a canonical role
- Every cascade row is either done or N/A under a declared policy exception
- Each adapter file is a pointer only — no restated steps
- The Brain remains the source of truth.
