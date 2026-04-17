---
name: scaffold-must
description: Implement a new MustClause fluent validation method. Use when adding Must.Be.X validation. Do NOT use for Core rules, Guard clauses, Fluent extensions, or DataAnnotations.
---

# Skill: Implement MustClause

## Step 0: Load Specifications (MANDATORY — read before writing ANY code)
Read these files completely:
1. `docs/ai/specs/spec.md` (root invariants, cascading model, layer ordering)
2. `docs/ai/specs/must-clauses/project.md` (MustClauses project spec)
3. `docs/ai/specs/core/project.md` (Core Rules/Utils — your dependency)
4. `docs/ai/specs/coding-standard.md` (formatting rules)
5. `docs/ai/skills/scaffold-must/SKILL.md` (canonical implementation recipe)

## Step 1: Follow the Recipe
Execute `docs/ai/skills/scaffold-must/SKILL.md` exactly as written.
Do NOT improvise. Do NOT skip steps.

## Step 2: Verify
- Code compiles: `dotnet build src/PineGuard.MustClauses/PineGuard.MustClauses.csproj`
- Returns `MustResult<T>` — never throws
- Owns canonical message with `{paramName}` placeholder
- Calls Core Rules/Utils for logic (no raw parsing in Must)
- Uses `Utility.TryXxx()` (not `Rules.IsXxx()`) when `result:` needs the parsed/normalized value
