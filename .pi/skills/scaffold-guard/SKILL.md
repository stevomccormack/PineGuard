---
name: scaffold-guard
description: Implement a new GuardClause (Guard.Against.X). Use when adding throw-on-failure guard methods. Do NOT use for Must clauses, Core rules, Fluent extensions, or DataAnnotations.
---

# Skill: Implement GuardClause

## Step 0: Load Specifications (MANDATORY — read before writing ANY code)
Read these files completely:
1. `docs/ai/specs/spec.md` (root invariants, cascading model, layer ordering)
2. `docs/ai/specs/guard-clauses/project.md` (GuardClauses project spec)
3. `docs/ai/specs/must-clauses/project.md` (MustClauses — your dependency)
4. `docs/ai/specs/coding-standard.md` (formatting rules)
5. `docs/ai/skills/scaffold-guard/SKILL.md` (canonical implementation recipe)

## Step 1: Follow the Recipe
Execute `docs/ai/skills/scaffold-guard/SKILL.md` exactly as written.
Do NOT improvise. Do NOT skip steps.

## Step 2: Verify
- Code compiles: `dotnet build src/PineGuard.GuardClauses/PineGuard.GuardClauses.csproj`
- Calls `Must.Be.X` — NEVER duplicates logic
- Reuses Must message — NEVER invents new messages
- Throws via `GuardFailure.Throw(...)` on failure
