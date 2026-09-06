---
title: Fake Adapter Surfaces (boundary)
type: meta
status: normative
---

# Fake Adapter Surfaces

Boundary fixture for `test/rules/surface-parity.test.ts`'s `boundary/` case: probes the
Copilot "one representative per family" policy's own decision boundary directly — a family
with **zero** representatives present on `.fakecopilot/prompts/` (`alpha`) and a family with
**two** (`gamma`), alongside every other surface staying fully parity-consistent so the only
findings produced are the two family-count boundary cases themselves.

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
