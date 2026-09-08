---
name: scaffold-guard
description: Implement a new GuardClause (Guard.Against.X).
---
# Skill: Implement GuardClause

## Load First

Read these files before writing any code:
1. [spec.md](../../../docs/ai/specs/spec.md)
2. [project.md (GuardClauses)](../../../docs/ai/specs/guard-clauses/project.md)
3. [project.md (MustClauses)](../../../docs/ai/specs/must-clauses/project.md)
4. [coding-standard.md](../../../docs/ai/specs/coding-standard.md)
5. [scaffold-guard SKILL.md](../../../docs/ai/skills/scaffold-guard/SKILL.md)

## Execute

Follow the canonical recipe in [scaffold-guard SKILL.md](../../../docs/ai/skills/scaffold-guard/SKILL.md) exactly.

## Verify

- Calls `Must.Be.X` — NEVER duplicates logic
- Reuses Must message — NEVER invents new messages
- Throws via `GuardFailure.Throw(...)` on failure
- The Brain remains the source of truth.
