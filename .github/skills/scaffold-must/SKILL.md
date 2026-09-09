---
name: scaffold-must
description: Implement a new MustClause fluent validation method.
---
# Skill: Implement MustClause

## Load First

Read these files before writing any code:
1. [spec.md](../../../docs/ai/specs/spec.md)
2. [project.md (MustClauses)](../../../docs/ai/specs/must-clauses/project.md)
3. [project.md (Core)](../../../docs/ai/specs/core/project.md)
4. [coding-standard.md](../../../docs/ai/specs/coding-standard.md)
5. [scaffold-must SKILL.md](../../../docs/ai/skills/scaffold-must/SKILL.md)

## Execute

Follow the canonical recipe in [scaffold-must SKILL.md](../../../docs/ai/skills/scaffold-must/SKILL.md) exactly.

## Verify

- Returns `MustResult<T>` — never throws
- Owns canonical message with `{paramName}` placeholder
- Calls Core Rules/Utils for logic (no raw parsing in Must)
- Uses `Utility.TryXxx()` (not `Rules.IsXxx()`) when `result:` needs the parsed/normalized value (see docs/ai/specs/core/project.md §4.1)
- The Brain remains the source of truth.
