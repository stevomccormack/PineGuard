---
name: scaffold-rule
description: Implement a new Core Rule or Util in PineGuard.Core. Use when adding low-level validation primitives or parsing helpers. Do NOT use for Must/Guard/Fluent/DataAnnotations layer code.
---

# Skill: Implement Core Rule/Util

## Step 0: Load Specifications (MANDATORY — read before writing ANY code)
Read these files completely:
1. `docs/ai/specs/spec.md` (root invariants, cascading model, layer ordering)
2. `docs/ai/specs/core/project.md` (Core project spec — Rules, Utils)
3. `docs/ai/specs/coding-standard.md` (formatting rules)
4. `docs/ai/skills/scaffold-rule/SKILL.md` (canonical implementation recipe)

## Step 1: Follow the Recipe
Execute `docs/ai/skills/scaffold-rule/SKILL.md` exactly as written.
Do NOT improvise. Do NOT skip steps.

## Step 2: Verify
- Code compiles: `dotnet build src/PineGuard.Core/PineGuard.Core.csproj`
- No user-facing messages in Core (pure logic only)
- No IO (File/Network) in Core Rules
- Null inputs handled explicitly (usually return `false`)
