---
title: Fake Adapter Surfaces (invalid)
type: meta
status: normative
---

# Fake Adapter Surfaces

Minimal fixture-only stand-in for the real `docs/ai/meta/adapter-surfaces.md`. Used by
`test/rules/surface-parity.test.ts`'s `invalid/` case, which deliberately carries **two**
independent defects proving the rule's two behaviour fixes over the legacy PowerShell Rule12
(plan `docs/ai/plans/audit-cli-rebuild.md` §2.4, §8):

1. **A genuine parity gap** — `great-gamma` is present in `docs/ai/agents/` and on the
   FakeClaude and FakeCopilot surfaces, but missing from `.fakepi/prompts/` with no declared
   exception. This must still be caught (the rule isn't "fixed" into uselessness).
2. **A missing parity-policy section** — section "4. Parity policy" (present in the `valid/`
   fixture) is entirely absent below. The legacy rule silently treated a missing/malformed
   exceptions section as "zero exceptions" and carried on; this rule must fail loudly with a
   real finding instead.

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

Deliberately no "## 4. Parity policy" section here — see the note above.
