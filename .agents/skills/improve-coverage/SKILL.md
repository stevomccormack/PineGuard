---
name: improve-coverage
description: Analyze code coverage gaps and add tests to reach 100% line and branch coverage. Use whenever coverage is below target, the user says "coverage is at X%", "uncovered lines", "missing branches", "get X to 100%", or after running coverage and seeing gaps. Trigger on any request to improve, fix, or boost coverage.
argument-hint: "[ProjectName]"
context: fork
allowed-tools: Read, Write, Edit, Glob, Grep, Bash
metadata:
  author: stevomccormack
  version: 1.1.0
  category: testing
---
# Skill: Improve Code Coverage

## Step 0: Load Specifications (MANDATORY — read before writing ANY code)
Read these files completely:
1. `docs/ai/specs/spec.md` (root invariants)
2. `docs/ai/specs/testing/coverage.md` (coverage enforcement rules)
3. `docs/ai/specs/testing/unit-test.md` (test patterns and structure)
4. `docs/ai/specs/testing/fixture.md` (current Expected type hierarchy)
5. `docs/ai/skills/improve-coverage/SKILL.md` (canonical implementation recipe)

## Step 1: Follow the Recipe
Execute `docs/ai/skills/improve-coverage/SKILL.md` exactly as written.
Do NOT improvise. Do NOT use `[ExcludeFromCodeCoverage]` unless truly unreachable.

## Step 2: Iterative Loop
1. Run coverage (console + JSON output, no HTML): `pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/code-coverage/Run-CodeCoverage.ps1" -Engine xplat -Mode GenerateAndAnalyze -Scope [ProjectName] -Top 30 -Isolated -SkipHtml`
2. Analyze gaps from console output: 0-hit lines = uncovered, < 100% branch = partial branches
3. Add test cases to fill gaps (null inputs, edge values, both `true`/`false` branch paths)
4. Repeat until 100%

## Step 3: Verify
- Report shows 100% line AND branch coverage for the target
