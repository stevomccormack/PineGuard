---
name: implement-fluent-validation
description: Implement a FluentValidation extension method adapting a MustClause. Use when adding IRuleBuilder extensions — whenever the user says "add a Fluent extension", "add FluentValidation support for Xxx", "implement IRuleBuilder.Xxx", or needs a FluentValidation integration for an existing Must clause. Do NOT use for Must clauses, Guard clauses, Core rules, or DataAnnotations.
argument-hint: "[MethodName]"
context: fork
allowed-tools: Read, Write, Edit, Glob, Grep, Bash
metadata:
  author: stevomccormack
  version: 1.1.0
  category: implementation
---
# Skill: Implement FluentValidation Extension

## Step 0: Load Specifications (MANDATORY — read before writing ANY code)
Read these files completely:
1. `docs/ai/specs/spec.md` (root invariants, cascading model, layer ordering)
2. `docs/ai/specs/fluent-validation/project.md` (FluentValidation project spec)
3. `docs/ai/specs/must-clauses/project.md` (MustClauses — your dependency)
4. `docs/ai/specs/coding-standard.md` (formatting rules)
5. `docs/ai/skills/scaffold-fluent/SKILL.md` (canonical implementation recipe)

## Step 1: Follow the Recipe
Execute `docs/ai/skills/scaffold-fluent/SKILL.md` exactly as written.
Do NOT improvise. Do NOT skip steps.

## Step 2: Verify
- Code compiles: `dotnet build src/PineGuard.FluentValidation/PineGuard.FluentValidation.csproj`
- Uses `ruleBuilder.MustBe(...)` adapter — NOT `.Must(...)` directly
- Passes `paramName: null` to MustClause
- Returns `IRuleBuilderOptions<T, TProp>`
