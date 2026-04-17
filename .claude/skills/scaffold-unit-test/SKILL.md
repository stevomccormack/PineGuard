---
name: implement-unit-tests
description: Implement xUnit tests for any PineGuard class. Use whenever the user says "add tests", "write tests", "test the X class", "add unit tests for X", or needs test coverage for rules, clauses, utilities, or any PineGuard layer. Trigger on any request to write or add test code.
argument-hint: "[TargetClass]"
context: fork
allowed-tools: Read, Write, Edit, Glob, Grep, Bash
metadata:
  author: stevomccormack
  version: 1.1.0
  category: testing
---
# Skill: Implement Unit Tests

## Step 0: Load Specifications (MANDATORY — read before writing ANY code)
Read these files completely:
1. `docs/ai/specs/spec.md` (root invariants)
2. `docs/ai/specs/testing/unit-test.md` (coverage targets, folder structure, TestData patterns)
3. `docs/ai/meta/template-unit-test.md` (code-level examples of nested Operation Groups)
4. `docs/ai/specs/testing/fixture-architecture.md` (current Expected type hierarchy: MustExpected, GuardExpected, FluentExpected, DataAnnotationExpected, RuleExpected)
5. `docs/ai/specs/testing/coverage.md` (coverage enforcement rules)
6. `docs/ai/skills/scaffold-unit-test/SKILL.md` (canonical implementation recipe)

Also read the project-specific test spec for the target project:
- Core: `docs/ai/specs/core/unit-test.md`
- MustClauses: `docs/ai/specs/must-clauses/unit-test.md`
- GuardClauses: `docs/ai/specs/guard-clauses/unit-test.md`
- FluentValidation: `docs/ai/specs/fluent-validation/unit-test.md`
- DataAnnotations: `docs/ai/specs/data-annotations/unit-test.md`

## Step 1: Follow the Recipe
Execute `docs/ai/skills/scaffold-unit-test/SKILL.md` exactly as written.
Do NOT improvise. Do NOT use ad-hoc patterns. Do NOT skip steps.

## Step 2: Verify
- All tests pass: `dotnet test`
- 100% line and branch coverage for the target
- Code conforms identically to `docs/ai/specs/testing/unit-test.md` patterns
