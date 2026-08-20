---
spec:
  id: pineguard.ai.guard-clauses.project-spec
  title: "PineGuard.GuardClauses Project Spec"
  version: 1
  template:
    - ../../meta/template-project.md
  parent:
    - ../spec.md
  dependencies:
    - ../dependencies.md
applies_to:
  - "src/PineGuard.GuardClauses/**"
---

# PineGuard.GuardClauses Project Spec

This document is the **source-of-truth instruction set** for generating and maintaining **production GuardClauses code** in this repository.

It is written for:

- humans adding new Guard clause APIs, and
- AI sessions that must iterate quickly and deterministically.

Out of scope:

- Unit tests for GuardClauses (covered by the separate unit-test agent spec).

## Feature Implementation Checklist

See `docs/ai/specs/spec.md` §3 ("Feature Implementation Checklist (Master)").

## Related specs

- Unit tests addendum: `docs/ai/specs/guard-clauses/unit-test.md`
- Coverage addendum: `docs/ai/specs/guard-clauses/coverage.md`
- Naming collisions: `docs/ai/specs/language/naming-collisions.md`

---

## 1. What GuardClauses are (in PineGuard)

**GuardClauses** are fluent “validate/parse/normalize” helpers that:

- are accessed through `Guard.Against`,
- are implemented as extension methods on `IGuardClause`, and
- **throw exceptions on failure** (they do not return a result object).

Typical usage:

```csharp
Guard.Against.Null(value);
Guard.Against.NullOrWhiteSpace(name);

Guard.Against.NotEmail(email);
Guard.Against.NotAscii(ch);
Guard.Against.NotHasKey(headers, "Content-Type");
```

Semantics:

- A Guard clause either:
  - returns normally (success), or
  - throws (failure).
- Guard clauses may return a value when they parse/normalize/produce a typed output.

### 1.1 Relationship to MustClauses (analogous by design)

PineGuard layers in one direction — each layer calls only the one before it:

- **Core** (`Rules`/`Utils`) owns validation logic and parsing.
- **MustClauses** call Core and own the canonical, user-facing messages (`MustResult<T>`).
- **GuardClauses** call MustClauses and throw using `MustResult.Message`.
- **FluentValidation** adapts MustClauses into `IRuleBuilder` extensions.
- **DataAnnotations** adapts MustClauses into `ValidationAttribute`s.

Guard, Fluent and DataAnnotations are sibling adapters over Must — none calls another, and none reimplements Core logic.

Must and Guard are two fluent validation surfaces that intentionally share implementation:

- `Must.Be.*` returns a `MustResult<T>`.
- `Guard.Against.*` throws.

GuardClauses must be implemented by calling the corresponding MustClause:

- Evaluate the MustClause.
- If the MustClause failed, throw using the canonical message from the MustResult.
- If the MustClause succeeded, return normally (or return the Must result).

Rationale:

- Avoids logic duplication (Rules/Utils stay centralized behind Must).
- Keeps parsing/normalization identical.
- Ensures Guard and Must stay analogous over time.

Messaging contract (critical):

- GuardClauses do not own canonical messages.
- Canonical messages come from `MustResult.Message` so GuardClauses, FluentValidation, and DataAnnotations can stay DRY and consistent.

---

## 2. Core types (what exists today)

These foundational types live in Core and must not be reimplemented:

- `IGuardClause` (marker interface)
- `GuardClause` (sealed implementation)
- `Guard` (entry point)

Important: in PineGuard, the entry point is **`Guard.Against`** (not `Guard.Be`).

`Guard.Against` returns a singleton instance of `GuardClause`, typed as `IGuardClause`.

---

## 3. Where Guard clause implementations live

Guard clause implementations are extension methods on `IGuardClause` and belong in the GuardClauses project:

- `src/PineGuard.GuardClauses/**`
- Namespace: `PineGuard.GuardClauses`

Folder conventions (strict):

- Everything can live at `src/PineGuard.GuardClauses/` unless/until a domain folder is defined.

---

## 4. Non-negotiables

- This is a **greenfield** library: do not preserve old APIs for compatibility.
- **Method Ordering Rule**: Negative methods must appear BEFORE Positive methods in the file.
  - Example: `Against.Null` (Negative) comes before `Against.NotNull` (Positive).
  - Rationale: Guard clauses are inherently defensive ("Guard Against Bad Thing").
  - This deliberately **inverts** `docs/ai/specs/must-clauses/project.md` ("Method ordering"), which is the canonical rule for the other layers. The inversion is intentional: ordering by the Must clause each guard invokes keeps Guard aligned with Must's file order.
  - Complement ordering convention (required): when Guard methods are implemented via Must complements, order Guard methods by the **Must clause they invoke** so they stay aligned with Must’s canonical ordering.
    - Example (complement pair):
      - `Must.Be.True(...)` appears before `Must.Be.False(...)`.
      - Therefore `Guard.Against.False(...)` (implemented via `Must.Be.True`) should appear before `Guard.Against.True(...)` (implemented via `Must.Be.False`).
- Do **not** add `[Obsolete]` attributes, compatibility wrappers, or forwarders.

  Note: the “no forwarders” rule forbids compatibility shims between old/new public APIs. It does not forbid the required domain facades described above, which exist to keep the public API surface stable while allowing internal organization.

- When renaming/refining APIs: change the public API directly and update all internal usages/tests/docs.
- Before deleting any functions, **double check** (see §5.6 Deletion checklist).
- All Guard clause methods must be **deterministic**, **allocation-conscious**, and safe to call repeatedly.
- Guard clause methods must throw for invalid input; they must not return a “success/fail result type”.
- Every exception message must be stable and user-readable by default.
- `paramName` must flow from `[CallerArgumentExpression]`.
- Prefer sourcing canonical messages from `MustResult.Message` (see §1.1), but see §5.5 for the permitted exception.
- GuardClauses must not post-process `result.Message` (including `{paramName}` replacement). That already happened inside Must.
- Validated value vs configuration/dependency parameters: see `docs/ai/specs/spec.md` (“Validated value vs configuration/dependency parameters”).
- **Coverage rule:** every public `Must.Be.*` clause must be representable via `Guard.Against.*`.
  - For each Must clause `X(...)`, add/maintain the Guard clause for its forbidden complement, typically named `NotX(...)` (or another Must-derived complement) and implemented via `Must.Be.X(...)`.
  - If Must defines both `X(...)` and `NotX(...)`, Guard must include **both** `NotX(...)` (implemented via `Must.Be.X`) and `X(...)` (implemented via `Must.Be.NotX`).

## Analyzer / ReSharper requirements (required)

See `docs/ai/specs/spec.md` (section “Analyzer / ReSharper requirements (global)”).

---

## Appendix A — Future considerations (non-binding)

This spec is intentionally strict today (Must vocabulary is canonical; Guard must not invent synonyms). Potential future language improvements (e.g., adopting `Missing*` / `NoMatching*` as curated bad-state names, richer complement naming, and additional `Non*` patterns) live in:

- `docs/ai/plans/future-language.md`

---

## 5. Naming & file conventions (strict)

### 5.1 Namespace

All GuardClauses live in:

```csharp
namespace PineGuard.GuardClauses;
```

### 5.2 File names and classes

- One file per domain: `GuardXxxClauses.cs`
- One public static class per file: `public static class GuardXxxClauses`

Required invariants:

- The file name **must** match the public class name exactly.
- GuardClauses class names must **never** use `*Extension` / `*Extensions` naming.

Required placement (strict):

- Place files under the appropriate domain folder if one is defined, otherwise at the project root.

### 5.3 Method naming

Guard method names are part of the public language of the library. In PineGuard, **MustClauses define the canonical vocabulary**.

The guiding rule is:

**Guard method names must be derived from (or identical to) the corresponding MustClause vocabulary. Do not invent new synonyms.**

Concretely:

- Prefer **exact Must names** when the Must name already represents a forbidden state (e.g. `Null`, `Empty`, `Default`).
- Prefer **`Not*` complements** when the forbidden state is the complement of a positive Must clause:
  - `Guard.Against.NotX(...)` is the complement of `Must.Be.X(...)`.
  - `Guard.Against.NotHasX(...)` / `NotHasAnyX(...)` are complements of `Must.Be.HasX(...)` / `HasAnyX(...)`.
- Do not introduce `Non*` or `Invalid*` naming. Standardize on `Not*` plus explicit semantic opposites (see §5.4.9 and the shared vocabulary map).

Shared vocabulary map (required):

- See `docs/ai/specs/language/vocabulary.md` for the human-readable guidance.
- See `docs/ai/specs/language/vocabulary.json` for the machine-readable map used by audit scripts.

Rule: use the **project-wide vocabulary map** so Guard, FluentValidation, and audit tooling stay aligned.

Hard rule:

- Do **not** introduce new naming vocabulary such as `Missing*`, `NoMatching*`, `ContainsForbidden*`, etc. If you need a complement name, use `Not*` derived from Must.

### 5.4 Naming conventions by “language” (how we handle ALL cases)

Guard naming must be consistent across all API families. Use the patterns below.

#### 5.4.1 “NotX” (complement set)

Use `NotX` when the forbidden set is “anything that is not X”.

Examples:

- `NotAscii` (forbid non-ASCII) — implemented via `Must.Be.Ascii`.
- `NotHexDigit` (forbid non-hex digits) — implemented via `Must.Be.HexDigit`.
- `NotPrintableAscii` (forbid non-printable ASCII) — implemented via `Must.Be.PrintableAscii`.

Implementation guidance:

- Prefer implementing `Guard.Against.NotX` via the _positive_ Must clause `Must.Be.X(...)` (see §5.5).

#### 5.4.2 “NotHasX / NotHasAnyX” (absence / predicate)

Use `NotHasX` / `NotHasAnyX` when the forbidden state is “does not contain / does not have” and Must provides (or implies) `HasX` / `HasAnyX`.

Examples:

- `NotHasKey(dictionary, key)` (complement of `Must.Be.HasKey`).
- `NotHasEmailAlias(email)` (complement of `Must.Be.HasEmailAlias`).
- `NotHasAnyKey(dictionary, predicate)` (complement of `Must.Be.HasAnyKey`).

Implementation guidance:

- Implement via the positive Must clause and document the complement mapping in a single-line comment:
  - `// Guard.Against.NotHasKey => Must.Be.HasKey (complement)`

#### 5.4.3 “InvalidX / MalformedX / UnparseableX” (format/parsing)

Use `NotX` as the default bad-state name for “this cannot be parsed / does not conform”, derived directly from the corresponding Must clause.

Examples:

- `NotEmail(value)` (implemented via `Must.Be.Email`)
- `NotStrictEmail(value)` (implemented via `Must.Be.StrictEmail`)
- `NotGuid(value)` (implemented via `Must.Be.Guid`)

#### 5.4.4 Inclusive/exclusive time and ranges (be explicit)

English antonyms are often **not** strict complements.

Prefer explicit names for inclusive variants:

- `Past` vs `Future`
- `PastOrPresent` and `FutureOrPresent`
- `Before` vs `After`
- `OnOrBefore` and `OnOrAfter`

Reasoning:

- “NotInPast” often means “present or future”, not “future”.
- “NotBefore” often means “on or after”, not “after”.

#### 5.4.5 Ranges: “OutOfRange / InRange” over “NotBetween / Between”

When the intent is numeric/time ranges, prefer:

- `OutOfRange(...)` (forbid out-of-range)
- `InRange(...)` only when the forbidden state is clearly “in range” (less common)

Reasoning:

- `NotBetween` tends to be read backwards.
- `OutOfRange` is the natural bad-state.

#### 5.4.6 Membership/contains: keep both when both are common

Both directions are common in real code:

- Forbid “contains disallowed X”
- Forbid “missing required X”

Naming guidance:

- Prefer `ContainsX` as a bad-state when it reads naturally (e.g., `ContainsControlChars`).
- Otherwise prefer Must-derived complements like `NotHasX` / `NotHasAnyX` when the intent is “must contain”.

#### 5.4.7 Temporal comparisons: Inclusion mapping (required)

Core time/date rules may expose boundary inclusion via `PineGuard.Common.Inclusion`.

Guard methods for *relative-to-now* temporal comparisons (`Past`/`PastOrPresent`/`Future`/`FutureOrPresent`) and for string-length comparisons (§5.4.8) must not accept an `Inclusion` parameter; they are split into explicit names per inclusion case.

- Range, chronology and overlap clauses (`Between`/`NotBetween`, `Chronological`/`NotChronological`, `Overlapping`, `OutOfRange`, `InRange`) DO forward an `Inclusion inclusion = Inclusion.Inclusive|Exclusive` parameter to the corresponding Must clause, because the boundary set is caller-supplied rather than fixed.

Required vocabulary:

- Forbidden state: `Past` / `PastOrPresent` / `Future` / `FutureOrPresent`
- Forbidden state: `Before` / `OnOrBefore` / `After` / `OnOrAfter`

Implementation rule (non-negotiable): each Guard method must call the corresponding MustClause for the forbidden state's complement, then throw if that MustClause failed.

Examples (conceptual):

- `Guard.Against.Past(value)` calls `Must.Be.FutureOrPresent(value)`.
- `Guard.Against.PastOrPresent(value)` calls `Must.Be.Future(value)`.
- `Guard.Against.Future(value)` calls `Must.Be.PastOrPresent(value)`.
- `Guard.Against.FutureOrPresent(value)` calls `Must.Be.Past(value)`.

Note: this spec standardizes on `FutureOrPresent` (not `PresentOrFuture`).

#### 5.4.8 String length comparisons: OrEqual variants (required)

Core string length rules may expose inclusion via `PineGuard.Common.Inclusion`.

GuardClauses must not accept an `Inclusion` parameter for string length comparisons.

The generator must produce explicit `OrEqual` variants:

- Forbidden state: `ShorterThan(value, length)` and `ShorterThanOrEqual(value, length)`
- Forbidden state: `LongerThan(value, length)` and `LongerThanOrEqual(value, length)`

Guard implementations must use Must complements:

- `Guard.Against.ShorterThan` calls `Must.Be.LongerThanOrEqual`.
- `Guard.Against.ShorterThanOrEqual` calls `Must.Be.LongerThan`.
- `Guard.Against.LongerThan` calls `Must.Be.ShorterThanOrEqual`.
- `Guard.Against.LongerThanOrEqual` calls `Must.Be.ShorterThan`.

#### 5.4.9 Collections: distinct vs duplicates (language inversion) (required)

For collection item uniqueness, prefer explicit, natural vocabulary over awkward “not” complements.

Required vocabulary:

- Must (good/bad properties): `HasDistinctItems` and `HasDuplicateItems`
- Guard (forbidden states): `DuplicateItems` and `DistinctItems`

Implementation rule (non-negotiable): each Guard method must call the corresponding MustClause for the forbidden state's complement.

Required mappings:

- `Must.Be.HasDistinctItems(...)` maps to `Guard.Against.DuplicateItems(...)`
- `Must.Be.HasDuplicateItems(...)` maps to `Guard.Against.DistinctItems(...)`

Examples (conceptual):

- `Guard.Against.DuplicateItems(value)` calls `Must.Be.HasDistinctItems(value)`.
- `Guard.Against.DistinctItems(value)` calls `Must.Be.HasDuplicateItems(value)`.

Notes:

- Do not generate `NotHasDistinctItems` as a Must clause; prefer `HasDuplicateItems`.

#### 5.4.10 Tasks: forbidden states are state names

For tasks, name forbidden states directly:

- `Canceled(task)` (forbid canceled)
- `Faulted(task)` (forbid faulted)

`Not*` task names are permitted here, because `MustTaskClauses` exposes both directions (`Completed`/`NotCompleted`, `Canceled`/`NotCanceled`, `Faulted`/`NotFaulted`) and §4's coverage rule requires a Guard for each. `GuardTaskClauses` therefore carries the full six.

#### 5.4.11 URIs/schemes: do not confuse logical complements with alternate schemes

Do **not** treat `!Https` as `Http`.

Use explicit names:

- `NotHttpsUrl(value)` and `NotHttpUrl(value)`
- `NotHasScheme(value, scheme)`
- `RelativeUri(value)` / `AbsoluteUri(value)` (forbid the opposite)

### 5.5 Must/Rules mapping (how to implement without duplicating logic)

Some Must clauses naturally come in **directional/opposite pairs** (or have a clear complement available).

Examples (non-exhaustive):

- `Positive` vs `ZeroOrNegative`, `Negative` vs `ZeroOrPositive`
- `Even` vs `Odd`
- `GreaterThan` vs `LessThanOrEqual`
- `GreaterThanOrEqual` vs `LessThan`
- `LessThan` vs `GreaterThanOrEqual`
- `LessThanOrEqual` vs `GreaterThan`
- `Before` vs `AfterOrSame` (if such a Must exists)

Prefer implementing Guard clauses via Must clauses whenever practical:

- Compute the Must clause for the _desired_ (good) property.
- If it failed, throw using `message ?? result.Message`.
- If it succeeded, return normally (or return `result.Result`).

For bad-state naming, this often means implementing the Guard against a bad-state by calling the complement “good-state” Must clause.

Examples:

```csharp
// Forbid non-ASCII => enforce ASCII
var result = Must.Be.Ascii(value, paramName); // Guard.Against.NotAscii => Must.Be.Ascii (complement)
if (result.Failed)
  GuardFailure.Throw(message ?? result.Message, paramName, value, exceptionCreator);
return result.Result;
```

Inline mapping comment (required when using complements):

- Add a short end-of-line comment after the Must call describing the mapping.

#### 5.5.1 Permitted exception: when Must does not (yet) expose the complement

Sometimes the forbidden bad-state has no existing Must clause representing its complement: Must exposes the positive property but no `Not*` counterpart, so there is nothing to call for the Guard's success condition.

In that case you have two options:

1. Preferred: add an explicit Must clause with a canonical message (keeps message consistency across Guard/Must/validators).
2. Permitted (rare): implement the Guard directly against Rules and define a minimal local message template.

If you must define a local message template:

- It must use the standard `{paramName}` placeholder.
- It must be stable and user-readable.
- It must match the library’s message style (short, declarative: “must not be …”).

- The Guard method name should describe the **condition you are guarding against**.
- The implementation should call the **complement Must clause** (the condition that must be true for the Guard to _not_ throw).

This keeps call sites readable:

```csharp
// Enforce: value must be positive
Guard.Against.ZeroOrNegative(value);

// Enforce: value must be even
Guard.Against.Odd(value);

// Enforce: value must be > min
Guard.Against.LessThanOrEqual(value, min);
```

Implementation rule (strict):

- Still use `if (result.Failed) ...` and throw with `message ?? result.Message`.
- Add a short **inline comment** after the Must call explaining the complement mapping.

Example:

```csharp
var result = Must.Be.Positive(value, paramName); // Guard.Against.ZeroOrNegative => Must.Be.Positive (complement)
if (result.Failed)
  GuardFailure.Throw(message ?? result.Message, paramName, value, exceptionCreator);
```

Important: do NOT implement GuardClauses by “throwing on `result.Success`”.

Reason:

- `MustResult<T>.Ok(...)` sets `Message` to `string.Empty`.
- GuardClauses must throw stable, user-readable messages, and must not invent their own templates.

So, throwing on `result.Success` would typically throw with a blank message unless the caller always supplies `message`, which is not acceptable.

---

### 5.6 Deletion checklist (required before deleting/renaming)

Because we are greenfield, we can delete/rename aggressively — but we must do it safely.

Before deleting or renaming any public Guard clause method:

- Search the repo for usages (src + tests + docs).
- Confirm it is not referenced by any generator scripts/docs (including these AI specs).
- Confirm there is a clear replacement name that follows §5.4.
- Update all call sites and keep semantics identical unless an explicit behavior change is intended.
- Run `dotnet test -c Debug`.

If any step fails or results are ambiguous, stop and re-evaluate the naming/API shape.

---

## 6. Signature conventions (strict)

All Guard clause methods must be extension methods on `IGuardClause`.

Rules:

- Receiver parameter name must be `_`.
- The primary validated input parameter must be named `value`.
- The `paramName` parameter must be populated using `[CallerArgumentExpression(nameof(value))]`.

- Helper variables must be named `result` (not `must`).

### 6.0 Nullability

GuardClauses follow Rule07 (see `docs/ai/specs/tools/audit-cli/spec.md` and `tools/audit-cli/rules/Test-Rule07-Nullability.ps1`) — the hybrid nullability strategy: use nullable reference inputs for ergonomic call sites and correct exception typing, but treat null as invalid unless the method name explicitly encodes null as acceptable.

Rules:

- **Null is invalid by default**.
  - If `value is null`, the corresponding MustClause should fail and GuardClauses should throw (via `GuardFailure`).
  - If null is valid, the method name must encode that intent (e.g., `NullOrXxx`) and the corresponding MustClause must succeed for null.

- **Value types (structs)**: Accept `T` (non-nullable).
  - Do not accept `T?` unless the clause is specifically about null (or an explicit "null means something" domain rule, which must be encoded in the method name).

- **Reference types**: Accept `string?` / `T?` for validated inputs.
  - This allows GuardClauses to throw `ArgumentNullException` when null is provided, and matches typical call-site usage.

- **Optional parameters** remain nullable:
  - `[CallerArgumentExpression(nameof(value))] string? paramName = null`
  - `string? message = null`
  - `Func<Exception>? exceptionCreator = null`

### 6.1 Optional custom message/exception

GuardClauses may optionally accept:

- `string? message = null` (caller override of the canonical message template), and
- `Func<Exception>? exceptionCreator = null` (caller override of the exception instance), and

GuardClauses should not expose an `exceptionReplacer` parameter in public APIs unless explicitly requested.
Central replacement is provided by `GuardExceptionPolicy`.

When provided:

- If `exceptionCreator` returns a non-null exception, that exception must be thrown.
- Otherwise, GuardClauses throw the appropriate BCL exception by default.

Replacement behavior:

GuardClauses also supports a global replacement policy via `GuardExceptionPolicy.ExceptionReplacer`.
`GuardFailure.Throw(...)` uses the effective Guard exception policy.
`GuardExceptionPolicy.ReplaceDefaultExceptions` controls whether the effective policy may replace the built-in default exceptions.
`GuardExceptionPolicy.BeginScope(...)` provides a scoped override on top of the global policy.
`GuardFailure.ThrowAndReplace(...)` remains the explicit per-call replacement path and takes precedence over scoped/global policy replacement.

See §11.5 for a copy/paste example combining a custom exception type and message.

---

## 7. Exception rules (strict)

### 7.1 Exception type selection

Current implementation rule (source-of-truth):

- The default thrown exception comes from `GuardFailure`:
  - `ArgumentNullException` when `value is null`
  - otherwise `ArgumentException`

Outlier / requires attention:

- Some clauses could be better represented by `ArgumentOutOfRangeException` (ranges/enums), but Must currently returns only a message.
- Do not invent exception-type heuristics in GuardClauses.
- If a specific exception type is required, use `exceptionCreator` (per call) or extend Must/GuardFailure intentionally in a separate change.

### 7.2 Messages

Every GuardClause failure message must be stable and user-readable.

Source-of-truth rule:

- The canonical message comes from `MustResult.Message`.
- GuardClauses may accept an optional `message` override and use it verbatim.

---

## 8. Mapping MustClauses to GuardClauses (required)

GuardClauses are a thin facade over MustClauses.

When implementing a GuardClause:

1. Call the corresponding MustClause in `src/PineGuard.MustClauses/**`.
2. If the MustClause failed, throw via `GuardFailure.Throw(message ?? result.Message, paramName, value, exceptionCreator)`.
3. If the MustClause succeeded:

- for validate-only: return normally.
- for parse/normalize: return `result.Result!`.

Rules:

- Do not duplicate parsing/validation logic in GuardClauses.
- Do not embed domain data/regex/constants in GuardClauses.

### 8.1 Preferred Must invocation style (strict)

Prefer calling Must clauses using `Must.Be.<Clause>(...)` extension-method notation.

Rationale:

- Most readable and consistent.
- Keeps Guard code “thin” and obviously delegated to Must.

Exception:

- If `Must.Be.<Clause>(...)` produces an ambiguity compile error (multiple extension methods with the same name/signature), use the disambiguation rule below.

### 8.2 Ambiguous Must extension methods (critical outlier)

Some Must clauses previously existed in multiple places with the same method name/signature,
which could make calls ambiguous depending on `using` directives.

Those duplicate surfaces have been removed; prefer calling `Must.Be.<Clause>(...)`.

Rule (strict):

- Try `Must.Be.<Clause>(...)` first.
- If ambiguous, call the intended MustClause static class explicitly (or via a `using` alias) to force the correct target.

---

## 9. Parse once / enumerate once rules (required)

### 9.1 Avoid double-parse

If a GuardClause returns a typed output, it must parse exactly once.

Flow:

1. Parse once using a Core `TryParse`/`TryXxx`.
2. If parsing fails: throw.
3. If parsing succeeds: validate using typed rules (no string re-parse).
4. Return the parsed/normalized value.

### 9.2 Avoid double-enumeration

For `IEnumerable<T>` inputs where you return a collection or need stable inspection:

- Materialize once into an array.
- Validate using that materialized array.
- Return the materialized array when the GuardClause returns a collection.

---

## 10. Output expectations

When asked to add GuardClauses, produce:

- One or more new/updated `GuardXxxClauses.cs` files under `src/PineGuard.GuardClauses/**`.
- Code that compiles under all repo target frameworks — `netstandard2.1`, `net8.0` and `net10.0` (see `Directory.Build.props`) — with nullable enabled. Guard any BCL API not present on `netstandard2.1` behind the existing conditional-compilation pattern rather than assuming net8.0+.
- No unit tests (unless explicitly requested in a separate task).

---

## 11. Canonical examples (copy/paste templates)

### 11.1 Validate-only GuardClause

```csharp
public static void NotSomething(
  this IGuardClause _,
  string? value,
  [CallerArgumentExpression(nameof(value))] string? paramName = null,
  string? message = null,
  Func<Exception>? exceptionCreator = null)
{
  var result = Must.Be.Something(value, paramName); // (optional) note complement mapping when applicable
  if (result.Failed)
    GuardFailure.Throw(message ?? result.Message, paramName, value, exceptionCreator);
}
```

### 11.2 Parse/normalize GuardClause (returns a value)

```csharp
public static TParsed Something(
  this IGuardClause _,
  string? value,
  [CallerArgumentExpression(nameof(value))] string? paramName = null,
  string? message = null,
  Func<Exception>? exceptionCreator = null)
{
  var result = Must.Be.Something(value, paramName); // (optional) note complement mapping when applicable
  if (result.Failed)
    GuardFailure.Throw(message ?? result.Message, paramName, value, exceptionCreator);

  return result.Result!;
}
```

### 11.3 Central exception replacement (BusinessException)

```csharp
GuardExceptionPolicy.ExceptionReplacer = ex => new BusinessException(ex.Message, ex);
GuardExceptionPolicy.ReplaceDefaultExceptions = true;
```

### 11.4 Scoped exception replacement

```csharp
using var _ = GuardExceptionPolicy.BeginScope(options =>
{
  options.ExceptionReplacer = ex => new BusinessException(ex.Message, ex);
  options.ReplaceDefaultExceptions = true;
});
```

### 11.5 Example: custom exception type + message

```csharp
public static void NotValidEmail(
  this IGuardClause _,
  string? value,
  [CallerArgumentExpression(nameof(value))] string? paramName = null)
{
  var result = Must.Be.Email(value, paramName);
  if (result.Failed)
    GuardFailure.Throw(
      "Email format is invalid.",
      paramName,
      value,
      () => new BusinessException("Email format is invalid."));
}
```

---

## 12. PR-style checklist

- Code compiles.
- Public API naming matches GuardClauses style.
- Exception messages are stable and end with a period.
- `paramName` flows from `[CallerArgumentExpression(nameof(value))]`.
- Throws use the most appropriate exception type.

---

## 13. “Next session” prompt template

Use this to start a GuardClauses generation session without re-explaining context:

- Implement Guard clause extension methods in `src/PineGuard.GuardClauses/**`.
- Methods extend `PineGuard.GuardClauses.IGuardClause` and are invoked via `Guard.Against`.
- Methods throw on failure and return normally on success (optionally returning a typed parsed/normalized value).
- Messages come from the corresponding `MustResult.Message` (optional caller `message` override is allowed) and `paramName` flows from `[CallerArgumentExpression]`.
- Prefer delegating to the corresponding `PineGuard.MustClauses.Must.Be.*` clause so Guard stays a thin throwing facade over Must.
- Do not generate unit tests in this task.
