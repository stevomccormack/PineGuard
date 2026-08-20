# Unit Tests

> Inherits from: `docs/ai/rules/global.md` (read first)

Before writing or editing unit tests, also read:
- `docs/ai/specs/testing/unit-test.md` (unit test conventions, TestData structure)
- `docs/ai/specs/testing/fixture.md` (Fixture Architecture v2 — Expected/Case type hierarchy)
- `docs/ai/specs/testing/coverage.md` (100% line + branch target)
- `docs/ai/specs/testing/gold-standard.md` (per-project compliance status)
- `docs/ai/rules/fixture-conventions.md` (mandatory fixture and test-file conventions)
- The relevant project's `coverage.md` and `unit-test.md` under `docs/ai/specs/[project]/`

When implementing, follow:
- `docs/ai/skills/scaffold-unit-test/SKILL.md` (implementation recipe)

## Key Rules

- Every test is `[Theory]` + `TheoryData`/`[MemberData]`. `[Fact]` is prohibited.
- Every `XxxTests.cs` has a paired `XxxTestData.cs`.
- CI enforces both: `tools/audit-cli/Run-All.ps1 -RuleId Rule50`.

Follow the spec EXACTLY. Do not improvise patterns.
