---
name: rename-codex-skills
description: Strip Codex's source-command- prefix from the generated .agents/skills/ folders and commit them by following the canonical Brain workflow.
---
# Skill: Rename Codex Skills

## Load First

Read these files before running the migration:
1. [safety.md](../../../docs/ai/specs/safety.md)
2. [adapter-surfaces.md](../../../docs/ai/meta/adapter-surfaces.md)
3. [rename-codex-skills SKILL.md](../../../docs/ai/skills/rename-codex-skills/SKILL.md)

## Execute

Follow the canonical recipe in [rename-codex-skills SKILL.md](../../../docs/ai/skills/rename-codex-skills/SKILL.md) exactly.

## Verify

- No `source-command-*` folder remains except those reported as skipped or excluded.
- Every promoted `SKILL.md` has `name:` equal to its directory name.
- Exactly one `chore(agents)` commit, containing only `.agents/skills/<name>/` paths.
- The Brain remains the source of truth.
