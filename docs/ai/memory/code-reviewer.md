# Code Reviewer Memory

**Role:** `docs/ai/roles/reviewer.md`

This is the portable baseline. Adapter memories (e.g. `.claude/agent-memory/code-reviewer/`) extend it
with dated, per-file review logs; durable criteria belong here.

## Durable Patterns

- Review against the Brain first, not just against whether code compiles.
- Look for architectural drift before style drift.
- Flag duplication across Must, Guard, FluentValidation, and DataAnnotations.
- Prefer spec references and exact fixes over general comments.

## Recurring Drift Signals

- Missing `CallerArgumentExpression` on validated values.
- Guard logic that does more than call Must and throw.
- FluentValidation adapters using `.Must(...)` instead of `.MustBe(...)`.
- DataAnnotations that implement validation logic directly instead of adapting Must.
- DataAnnotations not inheriting `ValidationAttributeBase`.
- Nullable value type (`int?`) where Rules expect the non-nullable form.
- Test files that break the expected PineGuard test structure or fixture usage.

## Parsed-Result Drift Signals

- A MustClause passing the raw input as `result` instead of the parsed output of `Utility.TryXxx()` —
  the parsed/normalized value must flow through to `MustResult<T>.Result`.
- A MustClause calling `Rules.IsXxx()` when a `Utility.TryXxx()` exists — the Try method returns both
  the boolean and the parsed value in one call.
- Reference: `../specs/core/project.md` §4.1.

## Architectural Violations

- Logic in GuardClauses (should only call Must and throw).
- Logic in FluentValidation (should only call Must via the `MustBe` adapter).
- Logic in DataAnnotations (should only call Must via the `ValidateValue` override).
- User-facing messages in Core — Core is pure logic; messages belong in Must.
- Integrations calling Core directly instead of going through Must.
- IO in Core Rules/Utils.

## Fixture Architecture v2 Review Checklist

Spec: `../specs/testing/fixture.md` · Conventions: `../rules/fixture-conventions.md`

- Expected types per layer: `RuleExpected` (Core), `MustExpected` (Must), `GuardExpected` (Guard),
  `FluentExpected` (Fluent), `DataAnnotationExpected` (DataAnnotations).
- All implement `IExpectedResult { bool IsValid }`. `MustExpected`/`FluentExpected`/`DataAnnotationExpected`
  extend `ReturnExpected`; `GuardExpected` extends `ThrowExpected`.
- `MustExpected(bool IsValid, string? Message = null, string? ParamName = null)` and
  `FluentExpected(bool IsValid, string? Message = null, string? PropertyName = null)` — assert `IsValid`
  first, message details only when the case defines them. The case property is `Expected`, never
  `ExpectedReturn`/`ExpectedSuccess`.
- `RuleCase<T>` replaces the obsolete `IsCase<T>`/`HasCase<T>`; layer cases are `MustCase<T>`,
  `GuardCase<T>`, `FluentCase<T>`, `DataAnnotationCase`.
- Fixtures are `RuleScenario` arrays with named fields; edge-case values reference Rule/Utils constants
  and are never hardcoded.
- Every test is `[Theory]` + `TheoryData`/`[MemberData]`; `[Fact]` fails CI Rule50.
- Tests files are flat (`MethodName_BehavesAsExpected`); TestData files keep their Operation Groups.
- Single-line entries, zero explanatory comments, camelCase tuple elements matching the exact method
  parameter names, fixture partials mirroring the source Rules partials.

## Review Priorities

1. Architectural boundaries.
2. Message ownership.
3. Signature consistency.
4. Test structure and coverage expectations.
5. Formatting and naming.

## Canonical References

- `../agents/scan-qodana-all.md`
- `../agents/scan-sonar.md`
- `../agents/scan-roslyn-all.md`
- `../skills/scan-sonar/SKILL.md`
- `../skills/fix-sonar/SKILL.md`
- `../skills/scan-roslyn/SKILL.md`
- `../skills/fix-roslyn/SKILL.md`
- `../rules/fixture-conventions.md`
