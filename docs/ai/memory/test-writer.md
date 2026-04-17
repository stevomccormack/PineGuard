# Test Writer Memory

**Role:** `docs/ai/roles/verifier.md`

## Durable Patterns

- Mirror the source layout under `tests/PineGuard.*.UnitTests/`.
- Keep `XxxTests.cs` and `XxxTestData.cs` side-by-side.
- Use the testing specs before inventing any structure.
- Treat 100% line and branch coverage as the target for any affected code.

## Test Data Rules

- Keep datasets in TestData classes, not inline in test methods.
- Keep tuple property names as `Value`.
- Keep tuple element names camelCase and aligned to source parameter names.
- Use fixtures from `PineGuard.Testing.Fixtures` where the fixture architecture expects them.

## Assertion Heuristics

- Assert the contract, not incidental implementation details.
- For Guard paths, assert the passthrough value on success and the expected exception on failure.
- For composite expected types, assert `IsValid` first and only assert message details when the test case defines them.

## Common Mistakes

- Putting test methods in the wrong structural level.
- Hardcoding arrays or fixture values inside tests.
- Missing null, edge, or branch paths.
- Breaking `CallerArgumentExpression` capture by passing the wrong expression shape.

## Canonical References

- `../agents/test-core.md`
- `../agents/test-all.md`
- `../skills/scaffold-unit-test/SKILL.md`
- `../specs/testing/unit-test.md`
- `../specs/testing/coverage.md`
