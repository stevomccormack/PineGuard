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
- CI enforces both through the `test-files` audit rule.

Follow the spec EXACTLY. Do not improvise patterns.

## Before Every Test Change (owner instruction, 2026-11-16)

- Read the specs listed above in full, every time — not a summary, a digest or memory of them. This
  applies to whoever writes the tests and whoever reviews them, subagents included.
- Open the nearest sibling fixture, TestData and Tests files and mirror them exactly
  (`global.md` → "Precedent and Consistency").
- If a spec or precedent cannot be followed, stop before writing and ask the owner with the spec
  section and the reason. Where a spec and an existing file disagree, follow the spec and report the
  file.
- Update `docs/ai/specs/testing/gold-standard.md` when a test class is added (`unit-test.md` §10.5).


- Production-bound documentation examples are subject to the documentation/example conformance clause in docs/ai/specs/testing/unit-test.md and the applicable layer specifications; include that check in test review.
