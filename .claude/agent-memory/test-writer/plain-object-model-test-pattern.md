---
name: plain-object-model-test-pattern
description: How to test plain object-model classes in MustClauses (MustFailure, MustValidationResult, MustValidationException) that sit outside the five layered stacks — same "(Other)" pattern as Utils, confirmed via MustCodesTests.cs precedent.
metadata:
  type: feedback
---

`MustFailure`, `MustValidationResult`, `MustValidationException` (object-level validation types in
`src/PineGuard.Core/MustClauses/`) are tested the same way as Utils/"Other" classes — see
[[utils-layer-test-pattern]] — not via `BaseMustUnitTest`/`MustCase<T>` (that stack is for
`Must.Be.*` single-value clauses only). Confirmed end-to-end (2026-08) writing
`MustFailureTests`/`MustValidationResultTests`/`MustValidationExceptionTests`: `BaseUnitTest` +
primary-ctor `(ITestOutputHelper output) : BaseUnitTest(output)`, ad-hoc `ReturnCase`/`ValueCase`/
`ThrowsCase<T>` records per Operation Group, `TheoryData` with `ValidCases`/`InvalidCases` ordering.

**Method naming confirmed**: descriptive names (`From_BuildsFailure_FromFailedResult`,
`Combine_Array_MergesEveryFailure`), NOT `_BehavesAsExpected`. Checked `MustCodesTests.cs` (the
memory-designated best "(Other)" precedent) — it also uses descriptive names
(`Constant_MatchesGrammarMirrorsIdentifierPathAndIsUnique`, `Prefix_MirrorsDomainTree`). The
`_BehavesAsExpected` suffix in `unit-test.md` §5.1 is normative only for the five
Rule/Must/Guard/Fluent/DA layer `Case<T>` stacks; plain `BaseUnitTest` classes are unconstrained by
it and should read naturally, one clause per behavior asserted.

**Overload-pair disambiguation (array vs `IEnumerable<T>`)**: when a static method has both a
`params T[]` overload and an `IEnumerable<T>` overload forwarding to it (e.g.
`MustValidationResult.From(params IMustResult[])` / `From(IEnumerable<IMustResult>)`), a local
variable's *static* declared type — not its runtime type — picks the overload. Test the array entry
point with a case field typed `T[]`; test the `IEnumerable<T>` entry point with a case field typed
`IEnumerable<T>` fed by a small `private static IEnumerable<T> EnumerateXxx(params T[] items) { foreach
(var i in items) yield return i; }` helper (§4.6 bottom-of-class helper) — a genuine lazy sequence,
not an array upcast, so the compiler can't silently pick the array overload. Since the array overload
is usually a one-line forward to the `IEnumerable` overload, exercising the array overload already
covers the `IEnumerable` overload's body for line coverage; the separate `IEnumerable` entry-point
group exists for API-surface completeness/explicitness, not because coverage requires it.

**`ThrowHelper.ThrowIfNull` paramName is call-site-fixed, not caller-expression-fixed**: same
`[CallerArgumentExpression]` note as [[utils-layer-test-pattern]] applies identically here — none of
these three types re-declare `[CallerArgumentExpression]` on their own public signatures, so
`ExpectedException.ParamName` is always the literal internal parameter name (`"result"`, `"failure"`,
`"additional"`, `"failures"`, `"results"`, `"prefix"`) regardless of the test's call-site expression
text. No local-variable-extraction dance needed (that's a Guard-layer-only concern).

**Discovered-and-fixed: record's default `ToString()` leaked a documented-as-PII-safe field**:
`MustFailure`'s XML doc says `Value` is "Never serialized by any adapter — a value that may hold a
secret must not reach a response body, a log line, or a localisation table through this property,"
but `MustFailure` was a plain `sealed record` with no `PrintMembers` override, so the
compiler-generated `ToString()` **included** `Value` (found by writing a test asserting
`Assert.Contains(sentinel, failure.ToString())` — it passed, proving the leak). Fixed in
`src/PineGuard.Core/MustClauses/MustFailure.cs` by adding a `private bool PrintMembers(StringBuilder
builder)` override that prints only `PropertyPath`/`Code`/`Message`; the test was updated to
`Value_RoundTrips_ButIsExcludedFromToString` asserting `Assert.DoesNotContain(sentinel, ...)` plus
that the three safe fields still appear. General lesson: when a doc comment makes a
security/PII claim about a record, verify the actual compiler-generated `ToString()`/equality
behavior before trusting the doc — records auto-include every positional property unless
`PrintMembers` is explicitly overridden; a failing assertion here is a real production bug to fix,
not just a doc-comment inaccuracy to report.
