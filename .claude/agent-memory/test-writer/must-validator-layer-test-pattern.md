---
name: must-validator-layer-test-pattern
description: How to test a sealed IMustValidator<T> implementation (e.g. XmlSchemaMustValidator) with BaseMustValidationUnitTest/MustValidationCase/MustValidationExpected, including the non-generic IMustValidator surface and lazily-thrown constructor dependencies.
metadata:
  type: feedback
---

Confirmed working end-to-end (2026-09-14, `PineGuard.Xml`'s `XmlSchemaMustValidatorTests`/`TestData`,
21/21 passing on net8.0+net10.0, zero build warnings, `dotnet format --verify-no-changes` clean).

`IMustValidator<T>` implementations are their own "layer" for testing purposes even though
`docs/ai/specs/testing/unit-test.md` §2.1's table doesn't list one: use
`BaseMustValidationUnitTest(output)` (not plain `BaseUnitTest`) and
`MustValidationCase<TValue>`/`MustValidationExpected` (both already `sealed`/generic in
`PineGuard.Testing`, in `PineGuard.Testing.UnitTests.MustClauses`) directly — no new project-local
`Case`/`Expected` pair needed, and no need to subclass `MustValidationCase<T>` (it's `sealed`,
subclassing throws CS0509 — just parameterize it with whatever `TValue` fits, including a tuple like
`(string? xml, XmlSchemaValidationOptions? options)` when the constructor takes extra config the
`Validate` method itself doesn't).

**Testing the non-generic `IMustValidator` surface (cast) without `Func`-wrapping hacks**: unlike a
true compile-time overload-resolution ambiguity ([[fact-to-theory-conversion]]), `IMustValidator`'s
`ValidatedType`/`Validate(object?)`/`ValidateAsync(object?, ct)`/`ValidateAsync(object?, mode, ct)`
are just distinctly-named interface members with default implementations — no receiver-type
ambiguity — so plain `[Theory]` methods per member work fine, each taking
`MustValidationCase<object?>` (for `Validate`/basic `ValidateAsync`) or
`MustValidationCase<(object? value, MustValidationMode? mode)>` (folding both async overloads into
one theory: `mode is { } m ? await ValidateAsync(value, m, ct) : await ValidateAsync(value, ct)`).
`ValidatedType` alone doesn't fit `MustValidationExpected` (expected is a `Type`, not a validation
outcome) — give it its own trivial single-case `sealed record Case(string Name, Type Expected) :
BaseCase(Name);` rather than forcing it into the validation-result shape.

**`MustValidatorCast.To<T>(object?)`** (internal, `PineGuard.Core/MustClauses/MustValidatorCast.cs`)
returns `default!` for a `null` input when `default(T) is null` (true for `string`), so passing
`null` through the non-generic surface does NOT throw — it forwards `null` into the generic
`Validate(string value)` (bypassing the `notnull` constraint at runtime). A genuinely
wrong-runtime-type object (e.g. boxed `123`) throws `ArgumentException` with `ParamName: "value"`
(the cast's own parameter name, fixed at that internal call site — same "CallerArgumentExpression is
a non-issue here" reasoning as [[utils-layer-test-pattern]]). Test this with one
`ThrowsCase<object?>`/`Assert.Throws`/`ThrowsCaseAssert.Expected` case; don't bother re-testing it
through every non-generic member — it's shared interface-default code, not part of the class under
test's own coverage target.

**Lazy vs eager constructor validation — verify empirically, don't trust the plan prose**: a plan
document said "null schemas → ArgumentNullException" as if the constructor validated eagerly.
Reading `XmlSchemaMustValidator`'s actual source showed the primary-constructor body does nothing —
the null check only happens inside `XmlSchemaUtility.TryValidate` (via `ThrowHelper.ThrowIfNull`)
the first time `Validate`/`ValidateAsync` is called. Test what the code actually does: constructing
with `null!` schemas succeeds silently; the exception surfaces on the first `Validate(...)` call.
Split into `Constructor_BehavesAsExpected` (options=null uses `Default`, verified behaviourally by
asserting a warning-bearing document still validates) / `Constructor_ThrowsAsExpected` (constructs
successfully, then asserts the deferred throw on `Validate`) — same `_BehavesAsExpected`/
`_ThrowsAsExpected` split precedent as `FooRules.Parse` in `unit-test.md` §8.3. Always call out this
kind of plan-vs-implementation mismatch in the final report instead of silently "fixing" the test to
match the plan's wrong assumption.

**Don't over-generalize an invariant assertion across a whole dataset.** Tried adding a blanket
`Assert.False(string.IsNullOrEmpty(failure.Message))` inside a `foreach (var failure in
result.Failures)` loop shared by every case in a `Validate` theory — failed on the one case
(`UnknownNamespace` with no declared namespace, i.e. `xmlns` absent entirely) where the violation's
`Message` is legitimately `""` (the engine reports the offending namespace URI as the message, and an
absent namespace URI reports as empty string). Fix: scope the "message must be non-empty" check to
just the specific case name it's meaningful for (`if (tc.Name == nameof(Fixture.SpecificCase))
Assert.False(...)`), and keep only the truly-universal invariant (e.g. "every failure's `Value` is
always `null`") in the shared loop. When a task description says "assert X non-empty" for one named
scenario, don't promote it to a blanket per-failure assertion without checking every other case's
actual runtime output first (use a throwaway `[Fact]`-based spike test with `ITestOutputHelper` dumps
— see next paragraph).

**Re-verify ground truth after a parallel fixer agent lands source changes.** Mid-task, a coordinator
warned a fixer was editing `src/PineGuard.Xml` concurrently (adding the `ReportValidationWarnings`
flag so `TreatWarningsAsViolations` actually fires; making the constructor throw
`ArgumentNullException` eagerly instead of lazily on first `Validate`; and changing element-start
violations to carry the violating element's *own* path). Consequences observed empirically: the
constructor test changed from "construct successfully, then `Validate` throws" back to "construct
itself throws" (update the `Act` step only — the `TheoryData`/`ExpectedException` stayed identical,
since `ParamName: "schemas"` didn't change); and the lax-`xs:any` warning's `PropertyPath` shifted
from `Document/SplmtryData` to `Document/SplmtryData/Extra` (the leaf element itself is now on the
path) — every other path/kind/code already tested (`MsgIdTooLong`, `TwoViolations`,
`UnknownNamespaceDocument`, `NoNamespaceDocument`, `Malformed`) stayed unchanged. Re-run the same
spike-test ground-truth check after any "a fixer is touching this file" heads-up, rather than assuming
only the specifically-named behaviors changed — the src build itself will intermittently fail with
compile errors while the fixer is mid-edit (observed `CS1501`/`CS8121` on unrelated lines in the same
file); that's expected per the coordinator's own warning, not a signal to touch `src/` — just poll
(`dotnet build` in a bounded retry loop) until it's green again before trusting a "real" test failure.

**Getting runtime ground truth before writing `Expected` values**: for anything depending on the
.NET XML schema validation engine's own diagnostics (violation `Path`/`Message`/`Kind` per input
document), don't guess from the plan prose — build/run a scratch `[Fact]` test (or reuse one a
parallel agent already left, e.g. `ZZSpikeTests.cs`) with `output.WriteLine` dumps of every case
through the actual API (`XmlSchemaUtility.TryValidate` or equivalent), capture the real values, and
only then write deterministic `Expected` fixtures against paths/kinds/codes (never the free-text
schema-engine message itself, which differs across TFMs) — confirmed the plan's asserted
`UnknownNamespace` "Document" path was right, but also revealed the `NoNamespaceDocument` case
produces the identical `Path`/`Kind` (empty-string message) as `UnknownNamespaceDocument`, which the
plan prose didn't spell out.
