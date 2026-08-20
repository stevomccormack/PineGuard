---
name: new-validation
description: Add a simple new validation rule across ALL layers (Core -> Must -> Guard -> Fluent -> DataAnnotations -> Tests). Use whenever the user says "add a new validation", "implement Must.Be.X", "add Guard.Against.X", or wants a validation rule based on an in-memory predicate (string format, numeric range, enum check, etc.).
argument-hint: "[DomainName] [ConditionName]"
context: fork
allowed-tools: Read, Write, Edit, Glob, Grep, Bash
metadata:
  author: stevomccormack
  version: 1.1.0
  category: implementation
---
# New Validation: Vertical Slice Implementation

## Step 0: Load Root Specifications (MANDATORY — read before writing ANY code)
Read these files completely before writing ANY code:
1. `docs/ai/specs/spec.md` (root invariants — especially "Feature Implementation Checklist")
2. `docs/ai/specs/dependencies.md` (layer dependency map)
3. `docs/ai/specs/coding-standard.md` (formatting rules)
4. `docs/ai/specs/orchestration.md` (process/logging)

## Step 1: Implement Vertical Slice
Follow `docs/ai/specs/spec.md` Section "Feature Implementation Checklist" EXACTLY.

For each layer, read the project-spec FIRST, then the canonical skill:

| Order | Layer | Spec to Read | Skill to Follow |
|---|---|---|---|
| 1 | Core Utils | `docs/ai/specs/core/project.md` | `docs/ai/skills/scaffold-rule/SKILL.md` |
| 2 | Core Rules | `docs/ai/specs/core/project.md` | `docs/ai/skills/scaffold-rule/SKILL.md` |
| 3 | MustClauses | `docs/ai/specs/must-clauses/project.md` | `docs/ai/skills/scaffold-must/SKILL.md` |
| 4 | GuardClauses | `docs/ai/specs/guard-clauses/project.md` | `docs/ai/skills/scaffold-guard/SKILL.md` |
| 5 | FluentValidation | `docs/ai/specs/fluent-validation/project.md` | `docs/ai/skills/scaffold-fluent/SKILL.md` |
| 6 | DataAnnotations | `docs/ai/specs/data-annotations/project.md` | `docs/ai/skills/scaffold-annotation/SKILL.md` |
| 7 | Unit Tests (all) | `docs/ai/specs/testing/unit-test.md` | `docs/ai/skills/scaffold-unit-test/SKILL.md` |

## Step 2: Build & Test
- Build entire solution: `dotnet build`
- Run all tests: `dotnet test`
- Verify 100% coverage for new code

## Step 3: Summary
Return a summary of all files created/modified, organized by layer.
