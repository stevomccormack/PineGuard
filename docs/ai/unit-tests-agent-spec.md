# PineGuard Unit Test Agent Spec

This document is the **source-of-truth instruction set** for generating and maintaining unit tests in this repository.

It is written for:

- humans contributing tests, and
- AI sessions that must iterate quickly and deterministically.

If you are doing coverage work, read these docs in order:

1. This file (how to write tests in this repo)
2. `docs/ai/code-coverage-agent-spec.md` (how to measure and enforce 100% line + branch)

---

## Non-negotiables

- Target is **100% line coverage** and **100% branch coverage** for the requested scope.
- All tests must pass.
- Tests must be deterministic and stable across machines.
- Every test case record must include a non-empty `Name` field/property (readability is non-negotiable).
- Make xUnit theory row output readable by either:
  - deriving case records from `PineGuard.Testing.UnitTests.BaseCase` (preferred), OR
  - implementing `public override string ToString() => Name;`.
- Do not add new test frameworks or infrastructure unless the repo already uses it.

---

## Repo conventions (must follow)

### Test framework

- xUnit.

### Base class

- Prefer inheriting from `PineGuard.Testing.UnitTests.BaseUnitTest`.
  - It forces `InvariantCulture` for determinism.
  - If you must test culture-specific behavior, use `UseCulture(...)` from the base class.
  - If you need xUnit output logging, accept `ITestOutputHelper` in the test constructor and pass it to `base(output)`.

### Folder and namespace mirroring

Mirror the source layout so navigation is obvious:

- `src/<Library>/<Subfolders>/<File>.cs`
- `tests/<Library>.UnitTests/<Subfolders>/<File>Tests.cs`

Follow existing patterns in the nearest test project.

---

## Work ordering (session workflow)

When doing incremental migrations across the repo:

- Progress alphabetically through root namespaces/projects.
- Within `PineGuard.Rules`, start with the deepest child namespaces first:
  - `PineGuard.Rules.Cldr.*`
  - `PineGuard.Rules.Iana.*`
  - `PineGuard.Rules.Iso.*`
  - `PineGuard.Rules.Owasp.*`
  - then other `PineGuard.Rules.*`

---

## File structure (strict)

For each unit under test:

1. A test data file: `XxxTestData.cs`
2. A test class file: `XxxTests.cs`

Naming:

- Test class: `public sealed class XxxTests : BaseUnitTest`
- Test data class: `public static class XxxTestData`

---

## Parameterized testing rules (strict)

TheoryData is mandatory for parameterized tests.

Allowed:

- `[Theory]`
- `[MemberData(nameof(XxxTestData.SomeDataset), MemberType = typeof(XxxTestData))]`
- `TheoryData<TCase>` where `TCase` is a record (preferred)
- `TheoryData<T1, T2, ...>` only for very small cases (e.g., 1-2 primitives)

Disallowed:

- `[InlineData]`
- `IEnumerable<object[]>` / `object[]` / ad-hoc arrays
- defining datasets inside the test class

Rationale: stable diffs, consistent style, easy expansion during coverage pushes.

---

## TestData design (how to reach 100% fast)

### Canonical TestData template (source-of-truth)

This repository uses a single, repeatable TestData pattern. Treat `RuleComparisonTestData` as the canonical example.

#### Structure (strict)

- One top-level file per unit: `XxxTestData.cs` containing `public static class XxxTestData`.
- Inside it, create **operation groups** as nested `public static class` types (e.g., `Constructor`, `Parse`, `IsBetween`, `Boundaries`).
- Each operation group must define exactly these datasets (no others):
  - `ValidCases` (`TheoryData<Case>`)
  - `EdgeCases` (`TheoryData<Case>`)
  - `InvalidCases` (`TheoryData<ThrowsCase>`) (throwing-only; may be empty)

Definitions:

- `ValidCases`: non-throw “normal” scenarios.
- `EdgeCases`: non-throw boundary / domain / “interesting” scenarios.
  - `EdgeCases` can include BOTH:
    - “valid edge” inputs (still succeed), and
    - “invalid-but-graceful” inputs (e.g., `TryParse(...)` returns `false` without throwing).
  - If the API returns `bool`, **expected `false` without throwing belongs in `EdgeCases`**, not `InvalidCases`.
  - The bucket name is about whether the behavior throws, not whether the input is conceptually “valid”.
- `InvalidCases`: **throwing** scenarios only.
  - Use `PineGuard.Testing.ExpectedException` to specify the expected exception.

Rule of thumb:

- If the test expects an exception → `InvalidCases`.
- If the test expects “failure” WITHOUT an exception (e.g., `false`, `null`, default out-param) → `EdgeCases`.

`InvalidCases` must always exist, even when empty:

```csharp
public static TheoryData<ThrowsCase> InvalidCases => [];
```

#### Case records (strict)

Each operation group ends with a `#region Cases` section that defines the public record types.

Placement (strict):

- Put the `#region Cases` at the **end of the operation group** (after `ValidCases`/`EdgeCases`/`InvalidCases`).
- Keep the case record(s) public, and keep them inside that region.

Rules:

- Prefer **one** record type named `Case` for `ValidCases` and `EdgeCases`.
- Define a separate throw-case record only when the data shape differs (typically because it needs additional inputs beyond `Name` + `ExpectedException`).
- Every record must include a `Name` field/property.
- Ensure xUnit output is readable by either:
  - deriving from `PineGuard.Testing.UnitTests.BaseCase` (preferred), OR
  - implementing `public override string ToString() => Name;`.

Example shapes:

- `public sealed record Case(string Name, /* inputs... */, /* expected... */) : BaseCase(Name);`
- `public sealed record InvalidCase(string Name, /* inputs... */, ExpectedException ExpectedException) : ThrowsCase(Name, ExpectedException);`

Preferred shared case record types (use when they fit; do not invent new shapes):

- `BaseCase` (Name + ToString)
- `ValueCase<TValue>` (Name + Value)
- `ReturnCase<TValue, TResult>` (Name + Value + ExpectedReturn)
- `ReturnWithOutCase<TValue, TResult, TOut>` (Name + Value + ExpectedReturn + ExpectedOutValue)
- `TryCase<TValue, TOut>` (for `TryXxx(value, out outValue)` patterns; `ExpectedReturn` is `bool`)
- `IsCase<TValue>` (for boolean predicate patterns; `ExpectedReturn` is `bool`)
- `HasCase<TValue>` (alias of `IsCase<TValue>`; use when semantics read better as “Has…”)
- `IThrowsCase` / `ThrowsCase` (standardizes throws-only `InvalidCases` via `ExpectedException`)

#### Contextual naming (strict)

When using the shared case base records, concrete case records MUST use domain/context-specific parameter names.
Do NOT propagate abstract parameter names like `Value`, `ExpectedReturn`, or `ExpectedOutValue` into concrete case records.

Rationale: the base classes are generic by design; case records should read like a sentence in their local domain.

Examples:

```csharp
// Bad: the dataset reads like plumbing.
public sealed record Case(string Name, string? Value)
  : ValueCase<string>(Name, Value);

// Good: the dataset reads like the domain.
public sealed record Case(string Name, string? emailAddress)
  : ValueCase<string>(Name, emailAddress);
```

```csharp
// For predicate APIs (Is/Has): use IsCase/HasCase and name the value contextually.
public sealed record Case(string Name, string? password)
  : IsCase<string>(Name, password, ExpectedReturn: true);
```

```csharp
// For TryXxx(value, out outValue) APIs: use TryCase and name both value and expected out-value contextually.
public sealed record Case(string Name, string? input, bool expectedSuccess, IPAddress? expectedIp)
  : TryCase<string, IPAddress>(Name, input, expectedSuccess, expectedIp);
```

#### Helper factories (optional)

Use private static helper factories for readability and consistency.

Required:

- Define and use a `V(...)` helper in each operation group for `ValidCases`/`EdgeCases` case construction.

Optional (but preferred when `InvalidCases` becomes non-empty):

- Define and use an `I(...)` helper for throw-cases.

Define helpers at the top of the operation group, e.g.:

- `private static Case V(...) => new(...);`
- `private static InvalidCase I(...) => new(...);`

Formatting (strict):

- `=> new(...)` must be a single line.
- Prefer positional arguments in `new(...)` (avoid named arguments unless necessary for clarity).

#### InvalidCases must be wired in tests (required)

If an operation group defines `InvalidCases` (it always should), the corresponding test class must include a dedicated throws-theory wired to that dataset.

Even when `InvalidCases => []` is empty today, the test method must exist so future invalid cases are automatically exercised.

Pattern:

```csharp
[Theory]
[MemberData(nameof(XxxTestData.SomeOperation.InvalidCases), MemberType = typeof(XxxTestData.SomeOperation))]
public void SomeOperation_ThrowsExpected(ThrowsCase testCase)
{
  var ex = Assert.Throws(testCase.ExpectedException.Type, () =>
  {
    // Act: call the API under test here.
  });

  ThrowsCaseAssert.AssertExpected(ex, testCase);
}
```

#### Dataset row formatting (strict)

- Every case row must be a single line:
  - `{ new Case("...", ...) },` or `{ new InvalidCase("...", ...) },`
- Keep `Name` concise and shorthand (e.g., `"0==0"`, `"min>max"`, `"invalid inclusion 123"`).

#### xUnit MemberData wiring (important)

When datasets live inside a nested operation group, `MemberType` **must** point at the type that contains the dataset member.

Example:

```csharp
[Theory]
[MemberData(nameof(RuleComparisonTestData.IsBetween.ValidCases), MemberType = typeof(RuleComparisonTestData.IsBetween))]
public void IsBetween_ReturnsExpected(RuleComparisonTestData.IsBetween.Case c) { /* ... */ }
```

### ExpectedException (guard/invalid tests)

Use `PineGuard.Testing.ExpectedException` (see `tests/PineGuard.Testing/ExpectedException.cs`) to define throw expectations.

It supports:

- exception `Type`
- optional `ParamName`
- optional `MessageContains` (only assert message content when necessary)

### Throws-only InvalidCases (shared abstraction)

For `InvalidCases` datasets (throws-only), standardize the exception expectation surface so assertion logic is reusable.

Definitions in `PineGuard.Testing.UnitTests`:

- `IThrowsCase` (preferred opt-in): exposes `ExpectedException ExpectedException { get; }`
  - Use this when your `InvalidCase` already inherits a different base record (single inheritance limitation).
- `ThrowsCase` (convenience base record): `Name + ExpectedException`
  - Use this when your `InvalidCase` does not already inherit another base record.

Shared assertion helper:

- `ThrowsCaseAssert.AssertExpected(Exception ex, ExpectedException expected)`
  - Does NOT depend on xUnit; it throws a descriptive exception on mismatch.
  - Standard pattern:

```csharp
var ex = Assert.Throws(testCase.ExpectedException.Type, () => /* Act */);
ThrowsCaseAssert.AssertExpected(ex, testCase.ExpectedException);
```

If your `InvalidCase` implements `IThrowsCase`, you may also use the overload:

```csharp
var ex = Assert.Throws(testCase.ExpectedException.Type, () => /* Act */);
ThrowsCaseAssert.AssertExpected(ex, testCase);
```

### Case counts (aim for rule coverage, not volume)

As a default starting point:

- `ValidCases`: 6-10 cases that cover unique normalization/valid combinations
- `InvalidCases`: 6-12 cases that cover unique guard/validation failure reasons
- `EdgeCases`: 3-6 cases (reserve for true boundaries and non-throw edge behaviors)

Increase counts when:

- Cobertura shows missing branches/conditions, or
- the code has multiple independent guard clauses and validation paths.

If a dataset requires single-use/stateful inputs (iterators, streams, enumerators), store **factories**:

- `Func<IEnumerable<T>?>`
- `Func<Stream?>`

### Verbosity control

To keep datasets readable as they grow:

- Use small factory helpers (e.g., `V(...)` / `I(...)`) inside the operation group.
- Keep expected fields minimal: assert only what matters for the behavior under test.

---

## AAA pattern (strict)

Every test must be readable as:

- Arrange
- Act
- Assert

Even when tiny, assertions must be explicit. Prefer verifying:

- primary outcome, and
- at least one invariant (e.g., out parameter defaulted on failure).

---

## Coverage-driven workflow (the loop)

This repo is optimized around a tight coverage loop.

Use this doc as the coverage source-of-truth:

- `docs/ai/code-coverage-agent-spec.md`

### Standard loop

1. Add tests for the next lowest-covered target
2. Run tests (repo must be green)
3. Generate coverage
4. Analyze filtered totals + lowest-covered classes
5. Use Cobertura `condition-coverage="x/y"` to target exact missing branches
6. Repeat until enforcement passes

---

## Branch coverage tactics (the patterns that matter)

When branch coverage is not 100%, don’t guess. Target the exact missing condition side.

Common patterns that require multiple tests:

- Short-circuiting boolean (`||` / `&&`)

  - You usually need separate tests where each term independently triggers the branch.

- Ternaries

  - You need a test for both sides.

- Pattern matching/type checks

  - You need a negative case (e.g., `Equals(object)` with a non-matching object).

- Fallback logic
  - You need a test where the “default” path is taken but the lookup/mapping is missing.

---

## Determinism rules (do not violate)

- Avoid depending on machine state (installed time zones, current culture, current directory).
- Prefer internal “core” helpers that accept dependencies/flags so tests can drive edge paths deterministically.
- If code is OS-dependent (e.g., Windows-only branches), gate the test:

```csharp
if (!OperatingSystem.IsWindows())
    return;
```

---

## GeneratedRegex / generated code (coverage hygiene)

The coverage collector is configured by:

- `etc/powershell/code-coverage/coverlet.runsettings`

This repo excludes source-generated and compiler-generated code by attribute so coverage remains stable.

Implication for test generation:

- Do not write tests whose _only_ purpose is to “cover generator output”.
- Instead, cover the **behavioral contract** of the public methods that use `GeneratedRegex`.

If the team decides to include generated code in coverage, update both:

- `coverlet.runsettings` (collection)
- the analyzer’s include/exclude scope (enforcement)

---

## When to refactor production code (allowed, but disciplined)

If a branch is truly unreachable (or indistinguishable), it is acceptable to refactor production code to remove dead logic.

Rules:

- Keep changes minimal.
- Preserve behavior.
- Prefer internal helpers rather than new public APIs.
- Re-run coverage immediately to confirm the missing-branch count actually dropped.

---

## Troubleshooting (what usually goes wrong)

- “Why is branch coverage stuck at 50% (1/2)?”

  - You’re missing one side of a condition; consult Cobertura `condition-coverage` and craft the missing input.

- “Coverage files aren’t showing in search.”

  - Some tools exclude `etc/generated/**`; ensure your search includes ignored files.

- “Tests behave differently on different machines.”
  - Look for culture/OS/time-zone dependencies; use `BaseUnitTest` helpers and OS gating.

---

## Next-session AI prompt template

Use this to start a new session without re-explaining context:

- Target is 100% line + 100% branch for the requested scope.
- Tests are xUnit and should inherit `PineGuard.Testing.UnitTests.BaseUnitTest`.
- Parameterized tests must use `TheoryData` + `MemberData` from `XxxTestData`.
- Coverage workflow is in `docs/ai/code-coverage-agent-spec.md`.
- Use Cobertura `condition-coverage="x/y"` to target exact missing branches.
- Prefer minimal deterministic tests; refactor production code only to remove truly unreachable branches.

---

## References

- Code coverage agent spec: `docs/ai/code-coverage-agent-spec.md`
- Unit test agent spec: `docs/ai/unit-tests-agent-spec.md`
- Coverage scripts:
  - `etc/powershell/code-coverage/GenerateCodeCoverageReport.ps1`
  - `etc/powershell/code-coverage/AnalyzeCodeCoverage.ps1`
- Test base utilities:
  - `tests/PineGuard.Testing/UnitTests/BaseUnitTest.cs`
  - `tests/PineGuard.Testing/UnitTests/BaseTestCases.cs`
  - `tests/PineGuard.Testing/UnitTests/TryCase.cs`
  - `tests/PineGuard.Testing/UnitTests/IsCase.cs`
  - `tests/PineGuard.Testing/UnitTests/HasCase.cs`
  - `tests/PineGuard.Testing/UnitTests/IThrowsCase.cs`
  - `tests/PineGuard.Testing/UnitTests/ThrowsCase.cs`
  - `tests/PineGuard.Testing/UnitTests/ThrowsCaseAssert.cs`
  - `tests/PineGuard.Testing/ExpectedException.cs`
