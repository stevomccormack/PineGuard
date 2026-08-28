---
name: must-codes-catalogue
description: How to wire a MustCodes error-code constant into the Must, FluentValidation and DataAnnotations layers — argument positions, the one-clause-one-code rule and its exceptions, and the reflection tests that will reject a bad catalogue entry
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
- **One-clause-one-code is not universal — verify, never assume.** `MustBitWiseClauses` is a known
  exception: every masked method emits `MustCodes.Bitwise.Mask.Invalid` on its mask-parse early
  returns and a *different* semantic code (`Bitwise.Bits.*`, `Bitwise.Equality.*`) on the real check.
  `MustPredicateClauses` is a second: `Satisfies`/`NotSatisfies` emit `Predicate.Callback.Null` on the
  `predicate is null` early return and `Predicate.Result.False`/`.True` on the real check.
  Grep the whole method body for `MustCodes\.` before picking one; if two distinct codes appear, the
  config/precondition one is the outlier and the semantic one is the clause's identity.
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

**Adapter layers carry the code too.** `FluentExtension.MustBe(check, message, code = null)` ends the
rule chain with `.WithErrorCode(code)`; the convention is `message, MustCodes.X.Y.Z);` appended on the
existing `message` line, plus `using PineGuard.Codes;`. Test side: `FluentExpected(bool IsValid,
string? Message, string? PropertyName, string? Code)` and `BaseFluentUnitTest.AssertResult` asserts
`Errors[0].ErrorCode` only when `Code` is non-null.

**One clause can hold two codes — pick the semantic one for the adapter.** The `MustString*Clauses`
parsing families (DateOnly/DateTimeOffset/TimeOnly/TimeSpan/Guid/Numbers…) emit a *secondary*
`MustCodes.Date.Format.Invalid` on the `TryParse` failure path and the method's *primary* semantic
code everywhere else (null early-return + the terminal `FromBool`). FluentValidation's `ErrorCode` is
fixed at rule-build time and cannot vary per invocation, so the adapter always gets the **primary**
code — the one in the terminal `FromBool`. Do not treat the parse-failure code as a candidate.
(The geo string clauses are the counter-example: parse failure reuses the same
`Geo.Latitude.Invalid`, so there is nothing to choose.)

**FluentValidation bridge.** `FluentExtension.MustBe(check, message, code)` takes the code as a
trailing **third positional** argument and ends the chain with `.WithErrorCode(code)`. Wiring a
`Fluent*Extensions` file means adding the wrapped clause's code to every `.MustBe(...)` plus
`using PineGuard.Codes;` (sorts before `PineGuard.Common`). House style keeps it on the `message`
line: `message, MustCodes.Text.Casing.NotCamel);`. The file is done when `grep -c MustBe` equals
`grep -c "MustCodes\."` and no bare `message);` line remains.
- The code is **fixed at rule-build time** — FluentValidation cannot vary `ErrorCode` per invocation.
  So `result.Errors[0].ErrorCode` is *always* the constant passed to `MustBe`, never the `Code` on the
  `MustResult` that actually failed.
- Which code, when a clause has two (bitwise mask, predicate-null, and every string-parsing clause
  whose parse branch returns `<Domain>.Format.Invalid`): wire the **semantic** one — the clause's
  identity, the one on the terminal `FromBool`. It also keeps a `string` overload aligned with its
  value-typed twin (`FluentStringTimeSpanExtensions` vs `FluentTimeSpanExtensions`).
- Follows from the two points above: a config-error or parse-error test case still asserts the
  *semantic* code, because the rule's `ErrorCode` never changes. That looks wrong but is correct —
  do not "fix" it by expecting `Bitwise.Mask.Invalid` / `Time.Format.Invalid` on those cases.
- Ternaries whose other branch just returns `MustResult<T>.Ok(...)` (`val is null ? …`,
  `val.HasValue ? …`) have exactly one fallible branch; the `Ok` short-circuit contributes no code.
- Test data: `FluentExpected(bool IsValid, string? Message, string? PropertyName, string? Code)` —
  `Code` is fourth, so pass it **named** (`Code: MustCodes.X.Y.Z`) or it lands on `PropertyName`.
  `AssertResult` asserts it only when non-null, so one representative invalid-case group per
  test-data file is enough — but a `Code:` on a switch arm that no invalid case reaches is a silent
  no-op. Prove the spot check runs by temporarily substituting a wrong-but-valid constant,
  confirming the test fails, then reverting.

**DataAnnotations bridge.** `ValidationAttributeBase(Type expectedType, string code, bool allowNull = true)`
takes the code as the **second positional** argument — *before* `allowNull`, which is the easy one to get
wrong on the attributes that pass `allowNull:` explicitly
(`ValidationAttributeBase(typeof(string), MustCodes.Text.Content.Blank, allowNull: false)`).
It surfaces as a public `string Code { get; }`
(validated non-blank at construction), so the attribute instance itself is the code carrier; PineGuard never
subclasses `ValidationResult`.
- The intermediate bases (`NumberAttributeBase`, `CollectionAttributeBase`, `ObjectAttributeBase`,
  `GenericDictionaryAttributeBase`) each take `string code` and forward it, so a derived attribute that
  previously had **no constructor at all** must gain the primary-constructor form
  `public sealed class FooAttribute() : SomeBase(MustCodes.X.Y.Z)`. Attributes that already had a primary
  constructor just gain the base argument: `FooAttribute(object v) : ObjectAttributeBase(MustCodes.X.Y.Z)`.
- Reflection-dispatch attributes (`InvokeAndMap(nameof(MustXClauses.Y), …)`,
  `InvokeGenericMust("Y", value, ctx, args)`) hide the clause behind a method-name string — trace the string
  to the clause and read its code there; never infer the code from the attribute's own name. The negated
  pairs are the trap: `CamelCaseAttribute` carries `Text.Casing.NotCamel`, `NotCamelCaseAttribute` carries
  `Text.Casing.Camel`. Generally: the code names the **failure state**, not the assertion, so it always
  reads inverted beside the attribute name and that is correct — `TaskCompletedAttribute` →
  `Task.Status.NotCompleted`, `TaskFaultedAttribute` → `Task.Status.NotFaulted`, `TrueAttribute` →
  `Boolean.Value.False`.
- Two *different* attributes legitimately sharing one code is also normal when the clauses they adapt
  share it: `XmlStringAttribute` and `XmlDocumentStringAttribute` are both `Xml.Document.Invalid`,
  because `Must.Be.Xml` and `Must.Be.XmlDocument` both emit that constant. Not a bug — do not invent a
  distinct code to make them differ. Strict/non-strict bound pairs do this routinely: in
  `StringAttributes.cs`, `LongerThan`/`LongerThanOrEqual` are both `Text.Length.TooShort` and
  `ShorterThan`/`ShorterThanOrEqual` both `Text.Length.TooLong`.
- An intermediate base whose constructor has **no** `<param>` tags at all is fine, but adding `code` to one
  that documents its other parameters trips `CS1573` — and this repo builds docs warnings as errors.
- Test wiring has two shapes; detect which the file uses rather than introducing a third.
  Shared-infra files (`BaseDataAnnotationUnitTest` + `AssertResult`): put `Code:` as a **named** argument on
  `DataAnnotationExpected(bool IsValid, string? Message, string? MemberName, string? Code)` (fourth slot) in
  the invalid branch of the `*TestData.cs` projection, then switch that one call site to the three-arg
  `AssertResult(tc, result, attr.Code)`. Hand-rolled files (plain `Assert.Equal` on a `Verify` helper, no
  `DataAnnotationExpected`) instead take a direct `Assert.Equal(MustCodes.X.Y.Z, attribute.Code)` — the
  attribute's `Code` is independent of the `ValidationResult`, so the assertion can sit anywhere in the body.
  Those hand-rolled methods are usually expression-bodied (`=> Verify(new FooAttribute(), tc);`), which holds
  no handle on the instance; convert that one method to a block body with a local before asserting.
  `Verify<TAttribute>`'s `where TAttribute : ValidationAttribute` constraint does *not* block
  `attribute.Code` — the local is inferred as the concrete attribute type, so the base property resolves.
  `CollectionAttributesTests.cs` is the canonical example of this shape.
- One test file can straddle two source files — `StringAttributes.cs`'s four casing attributes are tested in
  `StringCasingAttributesTests.cs`, not `StringAttributesTests.cs`, and that file mixes both shapes' owners.
  Grep `new <Class>Attribute` across the whole test project instead of assuming the base name matches, and
  when two agents share a test file, edit only your own attributes' methods.
- The silent-no-op trap applies here too: a `Code:` on a `switch`/ternary arm no invalid case reaches never
  runs. Prove it the same way as Fluent — swap in a wrong-but-valid constant, confirm the test method fails,
  revert. Mutating every spot check at once and checking each expected test method appears in the failure list
  verifies the whole file in one run.
- An attribute whose `ValidateValue` `switch`es over several *overloads* of one clause (e.g.
  `InSqlDateTimeRange` dispatching on `DateTime` vs `DateTimeOffset`) is **not** the "more than one clause"
  ambiguity case — overloads of a semantic check share a single code, so grep all of them and confirm they
  agree rather than reaching for a `TODO(codes)`.
- As with Fluent, the code is fixed when the attribute instance is constructed, so it never varies with which
  branch of the clause failed.
- The **aspect** segment is not uniform inside a domain, so never pattern-match a sibling constant to guess it.
  `MustCodes.Character` alone splits three ways — `Charset.*` (letter/digit/ascii/hex), `Category.*`
  (whitespace/control) and `Casing.*` (upper/lower) — and only the clause body tells you which. Likewise the
  same clause backs several attributes at different arities (`Must.Be.PhoneNumberString` serves both
  `PhoneNumberAttribute` and `CustomPhoneNumberAttribute` → one shared `Phone.Number.Invalid`).
- A `String*Attributes.cs` family mirrors its value-typed twin code-for-code, because the string clauses do
  (`Must.Be.Between`/`BetweenTimeOnly` → `Time.Range.OutOfRange`; `Before`/`BeforeTimeOnly` →
  `Time.Order.NotBefore`). Diffing the two ordered code sequences is a cheap correctness check on a big file.
- For a 10+ attribute file, drive the edit from a scripted `class name → MustCodes constant` map that hard
  fails on any class it could not match and any mapped class it never replaced. Hand-editing dozens of
  near-identical `: ValidationAttributeBase(typeof(X))` lines silently skips one; the assertion does not.

**Verifying inside a shared worktree.** When several agents fix different files at once, a whole-project build
tells you nothing until the last one lands. Filter build output to your own filenames to clear yourself, and
read the error *code* mix to tell whose work is outstanding: leftover `CS7036` is another agent's unfixed
attribute, while `CS1573`/`CS1734` mean someone changed a base class's XML docs. `-p:TreatWarningsAsErrors=false`
temporarily separates real compile errors from doc-comment errors without editing a file you do not own. If the
test project still will not build, poll the build on a timer rather than patching around the blocker. Same for
the final `dotnet test`: expect project-wide failures, and confirm the failing *test classes* are all other
agents' rather than trusting the pass/fail total.

See [[MEMORY]] for the general clause-layer patterns.
