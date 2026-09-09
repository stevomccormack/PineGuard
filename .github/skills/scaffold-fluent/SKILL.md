---
name: scaffold-fluent
description: Implement a FluentValidation extension method adapting a MustClause.
---
# Skill: Implement FluentValidation Extension

## Load First

Read these files before writing any code:
1. [spec.md](../../../docs/ai/specs/spec.md)
2. [project.md (FluentValidation)](../../../docs/ai/specs/fluent-validation/project.md)
3. [project.md (MustClauses)](../../../docs/ai/specs/must-clauses/project.md)
4. [coding-standard.md](../../../docs/ai/specs/coding-standard.md)
5. [scaffold-fluent SKILL.md](../../../docs/ai/skills/scaffold-fluent/SKILL.md)

## Execute

Follow the canonical recipe in [scaffold-fluent SKILL.md](../../../docs/ai/skills/scaffold-fluent/SKILL.md) exactly.

## Verify

- Uses `ruleBuilder.MustBe(...)` adapter — NOT `.Must(...)` directly
- Passes `paramName: null` to MustClause
- Returns `IRuleBuilderOptions<T, TProp>`
- The Brain remains the source of truth.
