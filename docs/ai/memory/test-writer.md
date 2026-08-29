# Test Writer Memory

**Role:** `docs/ai/roles/verifier.md`

## Normative Rules

The structural rules for this role — `[Theory]` + `TheoryData`/`[MemberData]` only, `XxxTests.cs`/
`XxxTestData.cs` pairing, tuple naming, fixture partial mirroring, the 100% line/branch target —
are fully specified in `../specs/testing/unit-test.md`, `../specs/testing/fixture.md`, and
`../rules/fixture-conventions.md`. Read those; this file records observations, not rules.

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
- `../specs/testing/fixture.md`
- `../specs/testing/coverage.md`
- `../rules/fixture-conventions.md`
