---
name: scaffold-annotation
description: Implement a DataAnnotations ValidationAttribute adapting a MustClause.
---
# Skill: Implement DataAnnotations Attribute

## Load First

Read these files before writing any code:
1. [spec.md](../../../docs/ai/specs/spec.md)
2. [project.md (DataAnnotations)](../../../docs/ai/specs/data-annotations/project.md)
3. [project.md (MustClauses)](../../../docs/ai/specs/must-clauses/project.md)
4. [coding-standard.md](../../../docs/ai/specs/coding-standard.md)
5. [scaffold-annotation SKILL.md](../../../docs/ai/skills/scaffold-annotation/SKILL.md)

## Execute

Follow the canonical recipe in [scaffold-annotation SKILL.md](../../../docs/ai/skills/scaffold-annotation/SKILL.md) exactly.

## Verify

- Inherits from `ValidationAttributeBase`
- Calls `Must.Be.X` with `paramName: null`
- No validation logic in the attribute (strict adaptation)
- The Brain remains the source of truth.
