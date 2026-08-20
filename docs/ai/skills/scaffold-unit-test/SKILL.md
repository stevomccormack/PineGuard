# Skill: Implement Unit Tests
**ID**: pineguard.skill.scaffold-unit-test
**Version**: 1.0

## 1. Context & Goal
Add **xUnit** tests for rules, must clauses, guard clauses, or utilities.

## 2. Inputs
- **Project**: The project being tested (e.g., `PineGuard.Core`).
- **Class**: The specific class/method to test.

## 3. Specifications (Source of Truth)
> [!IMPORTANT]
> Do NOT use ad-hoc patterns. Always refer to the canonical specs:
> - **Global Spec**: `docs/ai/specs/testing/unit-test.md`
>   (Defines coverage targets, folder structure, strict `TestData` file structures, and parameterization)
> - **Fixture Spec**: `docs/ai/specs/testing/fixture.md`
>   (Defines the `Expected` type hierarchy and the fixture partial conventions)
> - **Layer Spec**: `docs/ai/specs/{layer}/unit-test.md`
>   (e.g. `core`, `must-clauses`, `guard-clauses`, `fluent-validation`, `data-annotations`)
> - **Template Spec**: `docs/ai/meta/template-unit-test.md`
>   (Provides code-level examples of nested Operation Groups)
> - **Coverage Spec**: `docs/ai/specs/testing/coverage.md`

> [!IMPORTANT]
> Two non-negotiables, both enforced in CI:
> 1.  **`[Theory]` + `TheoryData` + `[MemberData]` only.** `[Fact]` and `[InlineData]` are disallowed.
> 2.  **Inherit the layer-specific base class**, not `BaseUnitTest` directly:
>     `BaseRuleUnitTest`, `BaseMustUnitTest`, `BaseGuardUnitTest`, `BaseFluentUnitTest`,
>     `BaseDataAnnotationUnitTest`.

## 4. Execution Steps

1.  **Locate Test Project & Folder**
    *   Mirror the source layout exactly (e.g., `src/PineGuard.Core` -> `tests/PineGuard.Core.UnitTests`).
    *   Place both `XxxTests.cs` and `XxxTestData.cs` side-by-side in the mirrored folder.

2.  **Create Test Data & Test Class**
    *   Follow the exact nested Operation Group pattern from `docs/ai/specs/testing/unit-test.md`.
    *   Do NOT hardcode Data arrays in tests.
    *   Do NOT use named arguments in test case records; use named tuples if there are multiple inputs.

3.  **Extract Shared Literals to Fixtures**
    *   Place shared literals in `tests/PineGuard.Testing/Fixtures/`.
    *   Fixture partials mirror the source Rules file: `XxxRules.Yyy.cs` -> `XxxRulesFixtures.Yyy.cs`.
    *   See `docs/ai/specs/testing/fixture.md` for the `Expected` types and nesting conventions.

4.  **Run Tests & Verify Coverage**
    *   Ensure all tests pass.
    *   Use the `Run-CodeCoverage.ps1` script to verify 100% line/branch coverage (per coverage spec).

## 5. Examples

| User says | Actions | Result |
|-----------|---------|--------|
| "Add tests for NetworkRules" | Create `NetworkRulesTests.cs` + `NetworkRulesTestData.cs` in mirrored folder | Theory-based tests with nested Operation Groups |
| "Test MustGeoLocationClauses" | Create test + test data files following MustClauses test patterns | Tests covering valid/invalid/null/edge cases |
| "Add tests for GuardJsonClauses" | Create test + test data, verify both success path and exception path | Guard tests with `Assert.Throws` for invalid inputs |

## 6. Definition of Done
- [ ] Tests pass (`dotnet test`).
- [ ] 100% line and branch coverage reached for the target.
- [ ] Code conforms identically to `docs/ai/specs/testing/unit-test.md` patterns.
- [ ] `pwsh ./tools/audit-cli/Run-All.ps1 -RuleId Rule50` passes (Theory-only + Tests/TestData pairing — this is a CI PR gate).

## 7. Success Criteria

| # | Criterion | Measure |
|---|-----------|---------|
| 1 | All tests pass | `dotnet test` exits 0 |
| 2 | 100% coverage | `Run-CodeCoverage.ps1 -Enforce100` passes for target assembly |
| 3 | Spec-compliant structure | Nested Operation Groups, `TheoryData` in separate `*TestData.cs` file |
| 4 | No hardcoded data in tests | All test cases defined in TestData classes, not inline |
| 5 | Mirrors source layout | Test file path mirrors `src/` folder structure exactly |

## 8. Reference Material
- `docs/ai/specs/testing/unit-test.md`
- `docs/ai/specs/testing/fixture.md`
- `docs/ai/specs/testing/coverage.md`
- [Reference exemplars](references/README.md)
