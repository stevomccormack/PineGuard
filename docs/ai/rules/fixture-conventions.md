# Fixture Architecture v2 — Rules

> Inherits from: `docs/ai/rules/global.md` (read first)

Before writing or editing fixtures, TestData, or test classes, read the canonical spec:

- `docs/ai/specs/testing/fixture.md` — the Fixture Architecture v2 spec (Expected hierarchy,
  scenarios, case records, extensions, combinators, code conventions, Guard TestData patterns)
- `docs/ai/specs/testing/unit-test.md` — the global unit-test spec (§4 TestData pattern,
  §5 test classes and method naming)

## Key Rules

1. **Flat test classes** — no nested Operation Group classes in Tests files; one
   `MethodName_BehavesAsExpected` method per rule (`unit-test.md` §4.5/§5.1). TestData files
   keep their nested Op Groups.
2. **Fixture partials mirror source partials** — `XxxRules.Yyy.cs` → `XxxRulesFixtures.Yyy.cs`
   (`fixture.md` §10).
3. **Edge constants come from source classes** — never hardcode a boundary that exists as a
   `const`/`static readonly` (`fixture.md` §9).
4. **Zero magic strings** — `using F = ...Fixtures;` alias + `nameof(F.OpGroup.Field)` names
   (`fixture.md` §11.5).
5. **Structural comments only; single-line entries** (`fixture.md` §11.1–§11.2).
6. **Guard TestData**: extract `tc.Value` to a local before the guard call; inverted guards and
   tuple `NullValue` fields need explicit `GuardExpected` factories (`fixture.md` §12).

Follow the spec EXACTLY. Do not improvise patterns.
