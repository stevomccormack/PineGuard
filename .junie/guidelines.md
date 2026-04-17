# PineGuard — Junie Guidelines

> **This file is an Adapter.**
> It maps agent instructions to the canonical Brain in `docs/ai/`.
> Do not add logic here. Add logic to the Brain.
> Start at **docs/ai/README.md** for the full Brain index.

## Role Adoption

Before acting, adopt a persona from the Engineering Business Unit:
Read **docs/ai/business-units/engineering.md**

## Global Rules

Read `docs/ai/rules/global.md` for invariants that apply to all code in this repository.

Key invariants:
- Layer order: Core Utils → Core Rules → MustClauses → GuardClauses → Integrations
- Must owns canonical messages; Guard/Fluent/Data reuse them (never duplicate)
- Guard calls Must (never duplicate logic)
- Deterministic: No IO in Core Rules/Utils
- File-scoped namespaces, sorted usings, arrow functions for single-line expressions
- All output files → `artifacts/` or `logs/`, NEVER project root

## Coding Standards

Read `docs/ai/specs/coding-standard.md` for formatting, naming, and style rules.

## Testing Standards

Read `docs/ai/specs/testing/unit-test.md` for test patterns, coverage targets, and TestData structure.
- All tests: `[Theory]` + `TheoryData`, never `[Fact]`
- 100% line and branch coverage enforced
- Coverage tool: Coverlet xplat only (dotCover removed)

## Safety

Read `docs/ai/specs/safety.md` for Tier 0/1/2 command classification before executing shell commands.

## Per-Layer Context

When working in a specific layer, read the corresponding rule file:
- `src/PineGuard.Core/` → `docs/ai/rules/core.md`
- `src/PineGuard.MustClauses/` → `docs/ai/rules/must.md`
- `src/PineGuard.GuardClauses/` → `docs/ai/rules/guard.md`
- `src/PineGuard.FluentValidation/` → `docs/ai/rules/fluent.md`
- `src/PineGuard.DataAnnotations/` → `docs/ai/rules/annotation.md`
- `tests/` → `docs/ai/rules/testing.md`
- `tools/` → `docs/ai/rules/tools.md`

## Knowledge Base

- **Brain index**: `docs/ai/README.md`
- **Specs**: `docs/ai/specs/` (normative engineering rules)
- **Skills**: `docs/ai/skills/` (reusable implementation recipes)
- **Agents**: `docs/ai/agents/` (canonical playbooks)
- **Workflows**: `docs/ai/workflows/` (multi-step orchestration)
