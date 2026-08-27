---
name: must-codes-catalogue
description: How to wire a MustCodes error-code constant into a clause layer — argument positions, null-early-return rule, and the reflection tests that will reject a bad catalogue entry
metadata:
  type: project
---

`Must.Be.*` clauses carry a machine-readable `Code` (`<domain>.<aspect>.<condition>`) beside the
human message. The catalogue lives in `src/PineGuard.Core/Codes/` as one `MustCodes.<Domain>.cs`
partial per domain; the grammar, domain map and controlled condition vocabulary are in
`docs/ai/plans/new-surfaces-missing-validation-cases-00-program.md` §5.4.

**Why:** codes are public API frozen at first release, so they are curated like names, never derived
from method names. The plan's §5.4 is authoritative — read it before proposing any code.

**How to apply:**
- Argument position differs per factory and is easy to get backwards:
  `MustResult<T>.Fail(code, messageTemplate, paramName, value)` — code is **first**.
  `MustResult<T>.FromBool(ok, code, messageTemplate, paramName, value, result)` — code is **second**, after `ok`.
  Both have a legacy no-code overload, so a misplaced argument fails as a *type* error somewhere
  else in the call, not as "missing code".
- Every `Fail`/`FromBool` in a public clause needs a code, including config-param null early returns
  (`if (predicate is null)`). Those reuse the method's own semantic code — one clause, one code; the
  message is what differentiates. (Plan 01 §4.1.)
- Symmetric clause classes (`MustDictionaryClauses` / `MustReadOnlyDictionaryClauses`, and any other
  §12.2 semantic-parity pair) stay separate types but share one catalogue: matching methods get the
  identical constant. Diff the ordered `MustCodes.X.Y.Z` sequence of both files to prove it.
- `tests/PineGuard.Core.UnitTests/Codes/MustCodesTests.cs` reflects over the whole catalogue and will
  fail if a constant's value does not equal its kebab-cased identifier path, does not start with its
  declaring class's `Prefix`, does not match the three-segment grammar regex, or is **not globally
  unique across every domain**. Run it filtered
  (`dotnet test tests/PineGuard.Core.UnitTests/... --filter "FullyQualifiedName~MustCodesTests"`)
  after adding a domain file — it is fast and catches all of the above.
- Test-data wiring: `MustExpected(bool IsValid, string? Message, string? ParamName, string? Code)`;
  `BaseMustUnitTest.AssertResult` asserts `Code` only when non-null. Where `InvalidCases` is a
  `.ToMustCases(_ => new MustExpected(...))` projection, add `Code:` as a named argument inside that
  single factory — that is the only per-method granularity the projection offers.
- Audit Rule13 forbids code string literals outside `src/PineGuard.Core/Codes/`, so test data must
  reference the constant (`MustCodes.Dictionary.Keys.Missing`), never `"dictionary.keys.missing"`.

See [[MEMORY]] for the general clause-layer patterns.
