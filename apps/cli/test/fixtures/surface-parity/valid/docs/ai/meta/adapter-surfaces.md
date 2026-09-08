---
title: Fake Adapter Surfaces
type: meta
status: normative
---

# Fake Adapter Surfaces

Minimal fixture-only stand-in for the real `docs/ai/meta/adapter-surfaces.md`, shaped the
same way: root boot files (§1), full adapters (§2), and a parity policy with declared
exceptions (§4). Used by `test/rules/surface-parity.test.ts`'s `valid/` case — every agent
below is represented consistently across every surface, per the declared exceptions, so the
rule must produce zero findings.

## 1. Root boot files

| File            | Tool            | Notes                                 |
| --------------- | --------------- | ------------------------------------- |
| `FAKECLAUDE.md` | FakeClaude Tool | Boot file for the FakeClaude surface. |

## 2. Full adapters

| Surface         | Tool             | Command dir | Skills    | Other                      |
| --------------- | ---------------- | ----------- | --------- | -------------------------- |
| `.fakeclaude/`  | FakeClaude Tool  | `commands/` | `skills/` | `rules/`                   |
| `.fakepi/`      | FakePi Tool      | `prompts/`  | `skills/` | `AGENTS.md`, `extensions/` |
| `.fakecopilot/` | FakeCopilot Tool | `prompts/`  | `skills/` | `copilot-instructions.md`  |

## 3. Rules-only adapters

None in this fixture.

## 4. Parity policy

| Exception                                                                                                                             | Surfaces             | Rationale                                                                                                                      |
| ------------------------------------------------------------------------------------------------------------------------------------- | -------------------- | ------------------------------------------------------------------------------------------------------------------------------ |
| **Solo family** — `solo-task`                                                                                                         | FakeClaude Tool only | Solo-task is a fake Tier-0-style operation restricted to the FakeClaude surface.                                               |
| **FakeCopilot subset** — `.fakecopilot/prompts/` carries one representative per command family (alpha, gamma) rather than every agent | `.fakecopilot/`      | FakeCopilot prompt files are the least-used entry point; mirroring every agent multiplies the maintenance surface for no gain. |
