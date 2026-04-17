---
name: implement-data-annotations
description: Implement a DataAnnotations ValidationAttribute adapting a MustClause. Use when adding [Attribute] validators. Do NOT use for Must clauses, Guard clauses, Core rules, or Fluent extensions.
---

# Skill: Implement DataAnnotations Attribute

## Step 0: Load Specifications (MANDATORY — read before writing ANY code)
Read these files completely:
1. `docs/ai/specs/spec.md` (root invariants, cascading model, layer ordering)
2. `docs/ai/specs/data-annotations/project.md` (DataAnnotations project spec)
3. `docs/ai/specs/must-clauses/project.md` (MustClauses — your dependency)
4. `docs/ai/specs/coding-standard.md` (formatting rules)
5. `docs/ai/skills/scaffold-annotation/SKILL.md` (canonical implementation recipe)

## Step 1: Follow the Recipe
Execute `docs/ai/skills/scaffold-annotation/SKILL.md` exactly as written.
Do NOT improvise. Do NOT skip steps.

## Step 2: Verify
- Code compiles: `dotnet build src/PineGuard.DataAnnotations/PineGuard.DataAnnotations.csproj`
- Inherits from `ValidationAttributeBase`
- Calls `Must.Be.X` with `paramName: null`
- No validation logic in the attribute (strict adaptation)
