# Migration Checker Memory

**Role:** `docs/ai/roles/owner.md`

## Durable Patterns

- Trace every Core rule the whole way: Core -> Must -> Guard -> Fluent -> DataAnnotations -> Tests.
- A rule is only "done" when every layer that should adapt it does, and each has a test pair.
- Read `../specs/spec.md` and `../specs/dependencies.md` before any check — layer order and the
  dependency map decide which layers a given rule is expected to reach.
- Report gaps as the specific file that should exist, not as a vague "missing coverage".
- Do not implement the fix during a check; the check produces the gap list.

## Common Gap Types

- Core rule with a MustClause but no GuardClause, or a Guard with no Fluent/DataAnnotations adapter.
- A layer implementation with no paired `XxxTests.cs` / `XxxTestData.cs`.
- Only the `Not*` half of a Guard family implemented — the affirmative companions are missing.
- Core rule with no fixture under `tests/PineGuard.Testing/Fixtures/`.
- Fixture partial that does not mirror its source Rules partial (`XxxRules.Yyy.cs` -> `XxxRulesFixtures.Yyy.cs`).

## Drift Signals

- Signature mismatch between layers (parameter names, nullability, optional parameters).
- Message strings duplicated outside Must instead of reused from it.
- Parameter naming inconsistency that breaks `CallerArgumentExpression` capture.
- Partially added rules: a new Core rule landing with Must only is the usual shape of an abandoned slice.

## Canonical References

- `../agents/audit-gap.md`
- `../agents/scaffold-vertical-slice.md`
- `../specs/spec.md`
- `../specs/dependencies.md`
- `../rules/fixture-conventions.md`
