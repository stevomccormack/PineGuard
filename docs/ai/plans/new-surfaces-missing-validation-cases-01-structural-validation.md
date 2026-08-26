<!-- metadata_header
type: plan
id: new-surfaces-01-structural-validation
version: 1.2
status: planned
last_updated: 2026-08-26
parent: new-surfaces-program
-->

# Plan 01 — Phase 1: Structural Validation

<!-- plan-nav -->
> [Parent](new-surfaces-missing-validation-cases.md) · [00 Program](new-surfaces-missing-validation-cases-00-program.md) · **01 Structural validation** · [02 Options](new-surfaces-missing-validation-cases-02-options.md) · [03 ASP.NET Core](new-surfaces-missing-validation-cases-03-aspnetcore.md) · [04 MediatR & bridges](new-surfaces-missing-validation-cases-04-mediatr-result-bridges.md) · [05 Rule batches](new-surfaces-missing-validation-cases-05-rule-batches.md) · [06 Analyzers](new-surfaces-missing-validation-cases-06-analyzers.md)
<!-- /plan-nav -->

> **Status**: Planned | **Depends on**: nothing (first phase) | **Unblocks**: every later phase
>
> **Worktrees**: split into four PRs so the rest of the program can start as early as possible (Plan 00 §10): **1a** `feature/structural-validation` = W0–W3 + W7/W8 (the keystone everything else waits for); **1b** `feature/structural-validation-fluent` = W4–W5; **1c** `feature/structural-validation-annotations` = W6; **1d** `feature/structural-validation-guard` = W6b. 1b, 1c and 1d branch from `main` after 1a merges and run in parallel with each other and with Phases 2–6.
>
> Read [Plan 00](new-surfaces-missing-validation-cases-00-program.md) first — §4 package conventions, §5 naming canon, §6 worktree protocol, §7 Definition of Done apply verbatim.

## 1. Business plan

### 1.1 The problem this phase solves

When a team evaluates PineGuard against FluentValidation for request validation, the first three questions are always the same: *"How do I compare two properties?"*, *"How do I make a rule conditional?"*, *"How do I validate each item in a list and get `Items[2].Name` back?"*. Today the honest answer to all three is "you can't inside PineGuard". The fourth question — *"Can my frontend key on a stable error code instead of parsing English?"* — has the same answer. These four gaps decide evaluations; no individual rule does.

Underneath all four is one missing concept: PineGuard validates **values** (`MustResult<T>`) and has no way to validate an **object** and hand back a structured result. Options binding, ASP.NET filters and mediator behaviors all need exactly that. This phase builds it once so Phases 2–4 are thin adapters.

### 1.2 Value

- **Competitive**: closes the four structural gaps from the parent plan Part 3 and `competitive-analysis.md` §6.2.
- **Platform**: `IMustValidator<T>` + `MustValidationResult` is the contract every later package consumes; error codes are baked in from day one because retrofitting codes into an aggregate type later is a breaking change.
- **Consumer**: an object validator that reads naturally next to the existing `Must.Be.*` vocabulary, with zero new adapter methods — every one of the ~550 clauses is usable inside a validator on day one because rules are plain lambdas that return `MustResult<T>`.

### 1.3 Success metrics

- Every public `Must.Be.*` clause carries a code and the new audit rule proves it mechanically.
- A `CreateOrderValidator` with a cross-property rule, a conditional rule, a nested validator and a `RuleForEach` fits in twenty lines and produces `Lines[2].Sku` paths.
- Existing public API: additive except the Guard exception-policy redesign (§3.2 lists the exact surface diff; Plan 00 §4.6 permits the deletions); all existing tests per TFM keep passing (record the baseline count in W0).
- Core, MustClauses, FluentValidation, DataAnnotations and Testing scopes stay at 100 %/100 %.

## 2. Functional plan

### 2.1 User stories

1. **Error codes.** As an API developer I get `result.Code == "email.address.invalid"` from `Must.Be.Email(x)` so my frontend can localise without parsing English, and can match `email.*` or `*.*.invalid` as a family.
2. **Object validation.** As a service developer I derive from `MustValidator<CreateOrder>` and declare rules per property; `Validate(order)` gives me a `MustValidationResult` with every failure, its property path, code and message.
3. **Cross-property.** As a validator author I write `RuleFor(x => x.EndDate, (order, end) => Must.Be.After(end, order.StartDate))` and the failure is attributed to `EndDate`.
4. **Conditional.** I write `RuleFor(x => x.Weight, w => Must.Be.Positive(w)).When(x => x.IsPhysical)` and the rule is skipped for digital orders. On a bare Must chain I write `Must.Be.Positive(weight).When(isPhysical)`.
5. **Collections.** I write `RuleForEach(x => x.Lines, line => Must.Be.NotNull(line))` and `RuleForEach(x => x.Lines, new OrderLineValidator())` and get `Lines[2]` / `Lines[2].Sku` paths.
6. **Composition.** I write `Must.Be.NotNull(id).AndThen(v => Must.Be.NotEmpty(v))` — the chain the `PineGuard.MustClauses` package README already advertises and the code does not yet have. Inside a validator the same chain is the presence-then-format idiom that keeps one property to one failure: `RuleFor(x => x.Email, e => Must.Be.NotNullOrWhiteSpace(e).AndThen(v => Must.Be.Email(v)))`.
7. **Boundary exception.** I write `MustValidationResult.From(Must.Be.Email(email).ToMustValidationResult("email"), Must.Be.Positive(qty).ToMustValidationResult("quantity")).ThrowIfFailed()` — naming each property path, because a bare `From(...)` would use `ParamName`, i.e. my local variable names, as the public property paths — and get a `MustValidationException` that carries the whole result — the marker Phase 3 maps to HTTP 400.
8. **Every layer carries the code.** FluentValidation failures expose it as `ErrorCode`; DataAnnotations attributes expose it as `Code`; Guard exceptions carry it in `Exception.Data["pineguard.code"]` (and the property path in `["pineguard.property-path"]`) so `GuardExceptionPolicy.Map` can build a domain exception from the `GuardFailure` and Phase 3 can handle it; DataAnnotations gains `[AfterProperty(nameof(StartDate))]`-style cross-property attributes; FluentValidation gains `After(x => x.StartDate)`-style cross-property overloads.

### 2.2 Canonical example (this must work verbatim at the end of the phase)

```csharp
using PineGuard.MustClauses;

public sealed record OrderLine(string? Sku, int Quantity);
public sealed record CreateOrder(string? Email, DateTime StartDate, DateTime EndDate, bool IsPhysical, decimal Weight, IReadOnlyList<OrderLine>? Lines);

public sealed class OrderLineValidator : MustValidator<OrderLine>
{
    public OrderLineValidator()
    {
        RuleFor(x => x.Sku, sku => Must.Be.NotNullOrWhiteSpace(sku));
        RuleFor(x => x.Quantity, qty => Must.Be.Positive(qty));
    }
}

public sealed class CreateOrderValidator : MustValidator<CreateOrder>
{
    public CreateOrderValidator()
    {
        RuleFor(x => x.Email, email => Must.Be.Email(email));
        RuleFor(x => x.EndDate, (order, end) => Must.Be.After(end, order.StartDate));
        RuleFor(x => x.Weight, weight => Must.Be.Positive(weight)).When(x => x.IsPhysical);
        RuleFor(x => x.Lines, lines => Must.Be.NotEmpty(lines));
        RuleForEach(x => x.Lines, new OrderLineValidator());
    }
}

var result = new CreateOrderValidator().Validate(order);
if (result.Failed)
    foreach (var failure in result.Failures)
        Console.WriteLine($"{failure.PropertyPath}: {failure.Message} [{failure.Code}]");
// Email: Email must be a valid email address. [email.address.invalid]
// EndDate: EndDate must be after the reference date. [date.order.not-after]
// Lines[1].Sku: Lines[1].Sku must not be null or whitespace. [text.content.blank]
```

(The exact message texts are whatever the clauses already say; the phase does not rewrite messages. `Must.Be.Positive` is in `MustNumberClauses`, which is `#if NET8_0_OR_GREATER` — this example targets net8+; on `netstandard2.1` substitute a non-numeric clause.)

### 2.3 Acceptance criteria

- [ ] `MustResult<T>` exposes `Code` and `MessageTemplate`; `Code` is `""` on success and never `""` on failure.
- [ ] `MustCodes` contains one `const string` per curated code (Plan 00 §5.4 grammar), every public clause passes its constant on every failure path, and identifier paths mirror values (composed `const`s, §4.1); the catalogue has been through the curation review; Rule13 verifies usage and `MustCodesTests` verifies shape, mirroring and uniqueness.
- [ ] `MustValidationResult`, `MustFailure`, `MustValidationException`, `IMustValidator`, `IMustValidator<T>`, `MustValidator<T>`, `MustPropertyRule<T,TProperty>`, `InlineMustValidator<T>`, `PropertyPathUtility` exist with the signatures in §4 and 100 % coverage.
- [ ] `MustResultExtension` has `AndThen`, `When`, `Unless`, `ToMustValidationResult`; `Combine` keeps every failure message (as today) but can carry only the first failure's `Code`/`ParamName` — a `MustResult<T>` has one slot for each; `ToMustValidationResult()` is the lossless aggregate and is what the adapters use.
- [ ] FluentValidation: every extension emits the clause's code as `ErrorCode`; four temporal families gain model-aware overloads.
- [ ] DataAnnotations: every attribute exposes `Code` (constructor parameter on `ValidationAttributeBase`, audited by Rule13 against the clause it calls); ten cross-property attributes exist.
- [ ] GuardClauses: `GuardExceptionPolicy` is `Map` / `BeginScope` / `Clear` / `HasMap` with the old members and `GuardExceptionPolicyOptions` deleted; `GuardFailure` is the data type; `ExceptionExtension` accessors and `Guard.Against.Invalid` exist; every guard throws through `GuardFailure.Throw(result, …)`; thrown exceptions — default, created or policy-replaced — carry `Data["pineguard.code"]` and `Data["pineguard.property-path"]`; `MustResult<T>.ThrowIfFailed()` / `ThrowNullIfFailed()` stamp the same keys.
- [ ] The `src/PineGuard.MustClauses/README.md` `AndThen` example is true; the root `README.md` gains *Object validation* and *Error codes* sections (with the §13.6 availability matrix from Plan 00).
- [ ] All Definition of Done items (Plan 00 §7).

### 2.4 Not in this phase

Async rules (`RuleForAsync`, `SatisfiesAsync`) — Phase 3. Clock injection — Phase 5. Fail-fast/short-circuit mode on validators — Phase 3 (added via default interface members so it is non-breaking). DataAnnotations object-graph walker — separate plan. A code-less `ValidationAttributeBase` constructor for consumer-written attributes (runtime capture or a `virtual Code`) — **deferred and demand-driven**: `code` is mandatory to begin with so every PineGuard attribute is defined correctly and Rule13 can hold it there; an optional overload is additive and non-breaking whenever a real consumer need shows what its semantics should be, whereas the reverse order would be a breaking change.

## 3. Technical plan — overview

### 3.1 Where things live

Everything foundational goes into `PineGuard.Core` beside `MustResult<T>` (the existing home of `Must`, `IMustClause`, `MustClause`, `MustResult<T>` — `docs/ai/specs/must-clauses/project.md` "Core types"), namespace `PineGuard.MustClauses`, one type per file:

```text
src/PineGuard.Core/MustClauses/
  IMustClause.cs                 (existing)
  Must.cs                        (existing)
  MustClause.cs                  (existing)
  MustResult.cs                  (existing — extended; MustResultExtension stays in this file)
  IMustResult.cs                 (+)
  (the code catalogue is NOT here — it is cross-layer vocabulary and lives beside Rules/, below)
  MustFailure.cs                 (+)
  MustValidationResult.cs        (+)
  MustValidationException.cs     (+)
  IMustValidator.cs              (+, non-generic)
  IMustValidatorOfT.cs           (+, generic — one type per file; `IMustValidatorOfT.cs` is a named exception to the file-name-equals-type-name rule, recorded in `docs/ai/specs/spec.md` §2.1 by W7, mirroring the BCL source convention)
  MustValidator.cs               (+)
  MustPropertyRule.cs            (+)
  InlineMustValidator.cs         (+)
  MustMessage.cs                 (+, internal — the {paramName} renderer shared by MustResult<T>, MustFailure and the validator)
  MustValidatorCast.cs           (+, internal — the object? → T cast behind the non-generic IMustValidator members)
  IMustRuleRunner.cs             (+, internal)
  Must<Shape>RuleRunner.cs       (+, internal — one sealed runner per RuleFor/RuleForEach shape: MustPropertyRuleRunner, MustCrossPropertyRuleRunner, MustNestedValidatorRuleRunner, MustCollectionRuleRunner, MustCollectionCrossPropertyRuleRunner, MustCollectionValidatorRuleRunner)
src/PineGuard.Core/Utils/
  PropertyPathUtility.cs           (+)
```

```text
src/PineGuard.Core/Codes/                          namespace PineGuard.Codes — sibling of Rules/, consumed by every call-site style
  MustCodes.cs                   (+, root of the partial catalogue — grammar doc only)
  MustCodes.<Domain>.cs          (+, one partial file per domain in Plan 00 §5.4 map order — MustCodes.Value.cs, MustCodes.Boolean.cs, MustCodes.Text.cs … MustCodes.Encoding.cs; 28 now, 32 after Phase 5; generated once, then hand-maintained)
```

`PineGuard.Core` gains no package references in this phase (Phase 5 Batch E adds the one exception, `Microsoft.Bcl.TimeProvider` — Plan 00 §4.3). `System.Linq.Expressions` and `ValueTask` are available on all three TFMs.

**Core Rules and Utils are unchanged by design.** Every Phase 1 type carries a message, a code or a path, which makes it Must-layer by the layering rule (`docs/ai/specs/spec.md` §5.1 — Rules are message-free predicates). The structural features need no new predicates: cross-property is `DateTimeRules.IsAfter` fed two properties, collection-element is any clause applied per item, conditional is control flow. The only Core-layer addition is `PropertyPathUtility`, a pure string helper, which lands in `PineGuard.Utils`. Core gains rules only in Phase 5, through vertical slices, with Rule02 (every Core rule consumed by a Must clause) and Rule14 (Core stays synchronous) guarding it.

### 3.2 Surface diff

| Type | Change |
|---|---|
| `MustResult<T>` | implements `IMustResult`; `+ Code`, `+ MessageTemplate`; `+ ThrowIfFailed<TException>(Func<IMustResult, TException>)` so a coded domain exception can be built on the single-value path; `implicit operator bool` becomes null-safe (`result?.Success ?? false`); factories gain a `code` parameter (old overloads removed at the end of W2 — this repository is greenfield, no shims: `docs/ai/specs/spec.md` §6.4) |
| `MustResultExtension` | `+ AndThen`, `+ When`, `+ Unless`, `+ ToMustValidationResult`; `Combine` keeps every message (as today) and carries the first failure's `Code`/`MessageTemplate` |
| `FluentExtension.MustBe` | `+ string? code = null` trailing parameter on all three overloads |
| `ValidationAttributeBase` | `+ string code` constructor parameter (second position; `allowNull` stays optional and named at call sites) and `+ Code` property; results remain the framework's own `ValidationResult` |
| `GuardExceptionPolicy` | members replaced: `+ Map(Func<GuardFailure, Exception>)`, `+ BeginScope(Func<GuardFailure, Exception>)` → `IDisposable`, `+ Clear()`, `+ HasMap`; deleted `ExceptionReplacer`, `ReplaceDefaultExceptions`, `BeginScope(Action<…>)` (§4.14.1; no shims) |
| `GuardExceptionPolicyOptions` | **deleted** |
| `GuardFailure` | **becomes a sealed data type** (`Code`, `Message`, `ParamName`, `Value`, `Exception`) keeping static `Throw(IMustResult result, string? message = null, Func<Exception>? exceptionCreator = null)` and the `CodeDataKey` / `PropertyPathDataKey` constants; string-based `Throw` and `ThrowAndReplace` deleted |
| `ExceptionExtension` (new) | `+ TryGetMustCode`, `HasMustCode`, `GetMustPropertyPath` (§4.14.2) |
| `Guard.Against.Invalid<T>(value, IMustValidator<T>)` (new) | §4.14.3 |
| `MustResult<T>.ThrowIfFailed()` / `ThrowNullIfFailed()` | thrown exceptions gain the same `Data` keys |
| `MustExpected`, `FluentExpected`, `DataAnnotationExpected`, `GuardExpected` (test) | `+ string? Code = null` optional positional parameter (last on the existing records) |

Everything above is additive except: the code-less `Fail`/`FromBool` overloads on `MustResult<T>`, and the Guard exception-policy members listed as deleted (Plan 00 §4.6 — no shims before the first release).

## 4. Technical plan — detail

### 4.1 Error codes

**Catalogue** — a `static partial class` split one file per domain, the same convention as `src/PineGuard.Core/Rules/StringRules.Bool.cs` and `tests/PineGuard.Testing/Fixtures/StringRulesFixtures.Casing.cs`: `+ src/PineGuard.Core/Codes/MustCodes.cs` is the root (grammar doc, no members) and `+ src/PineGuard.Core/Codes/MustCodes.Email.cs`, `MustCodes.Text.cs`, … each declare one nested domain class. The files live in their own top-level folder and namespace, **`PineGuard.Codes`**, a sibling of `PineGuard.Rules`: like the rules, the codes are cross-layer vocabulary consumed by every call-site style (Must returns them, Guard stamps them, Fluent emits them as `ErrorCode`, DataAnnotations attributes declare them), so they belong beside `Rules/`, not under any one style's folder. The **type** stays `MustCodes` because that is where a code is born — `PineGuard.Rules` are pure booleans; a Must clause is what pairs a rule with a message *and* a code, and Guard, Fluent and DataAnnotations all call Must — and because every other type in the program is Must-named for the same reason (`MustResult`, `MustFailure`, `MustValidator`, `AddMustValidation`). Namespace says who consumes; type says who defines; the namespace word recurring in the type is ordinary BCL practice (`System.Text.Json.JsonSerializer`). Rejected: `PineGuard.MustClauses.Codes` (nests everyone's vocabulary under one call-site style; a Guard user importing a `MustClauses.*` namespace is the wrong signal), `ErrorCodes` as the type (the one non-Must name in a Must family). (`PineGuard.MustCodes` as the namespace and `Codes` as the type were both rejected for the CA1724 namespace/type collision.) Consumers add `using PineGuard.Codes;` where they name a code (the Guard map, custom attributes, tests); the README's first code-bearing example shows it. **Dependency direction must stay acyclic**: `PineGuard.Codes` is a leaf consumed by `PineGuard.MustClauses` and every layer above it, exactly as `PineGuard.Rules` is; the `Codes/` files therefore contain `const string`s and doc comments **only** — no methods, no `using` of any PineGuard namespace — so the catalogue is a pure leaf that can never reach back up (Rule13 (g): no `using PineGuard` line under `src/PineGuard.Core/Codes/` — `Test-StructuralIntegrity.ps1`'s namespace check is informational only and runs in no gate). Shown merged here for brevity:

```csharp
namespace PineGuard.Codes;

/// <summary>The error-code catalogue: &lt;domain&gt;.&lt;aspect&gt;.&lt;condition&gt; (Plan 00 §5.4). The identifier path mirrors the code.</summary>
public static partial class MustCodes          // MustCodes.cs (root) + MustCodes.<Domain>.cs per domain
{
    // MustCodes.Email.cs — serves MustEmailClauses.cs
    public static class Email
    {
        public const string Prefix = "email";

        public static class Address
        {
            public const string Prefix = Email.Prefix + ".address";

            /// <summary><c>email.address.invalid</c></summary>
            public const string Invalid = Prefix + ".invalid";
            /// <summary><c>email.address.not-strict</c></summary>
            public const string NotStrict = Prefix + ".not-strict";
        }

        public static class Alias
        {
            public const string Prefix = Email.Prefix + ".alias";

            public const string Missing = Prefix + ".missing";
            public const string Present = Prefix + ".present";
        }
    }

    public static class Text
    {
        public const string Prefix = "text";

        public static class Length
        {
            public const string Prefix = Text.Prefix + ".length";

            public const string Mismatch = Prefix + ".mismatch";
            public const string OutOfRange = Prefix + ".out-of-range";
            // …
        }
    }
    // one nested class per domain, one per aspect, one const per condition — in catalogue order;
    // every level declares its own Prefix (the code of that node) and every value is composed from its parent's
}
```

**One file per domain, not per clause file.** A separate type per clause family (`MustEmailCodes`, `MustXmlCodes`) cannot be brought together under `MustCodes` — static classes neither inherit nor re-export — so the partial split is the idiom that gives both locality and a single entry point: `MustCodes.Email.cs` sits next to `MustEmailClauses.cs` in the tree, and the consumer still writes `MustCodes.Email.Address.Invalid`. The split is by **domain** because the map is many-to-one: the eight `MustString*Clauses` files and `MustCharClauses` are one domain (`text`), the six date/time clause files are `date`, `MustDictionaryClauses` + `MustReadOnlyDictionaryClauses` are `dictionary`, `MustNumberClauses` + `MustStringNumbers*Clauses` share `number` (one rule, one code, whatever the input type). Each domain file opens with a comment naming the clause files it serves; Rule13 checks the reverse — every constant a clause file uses comes from its mapped domain.

The identifier path mirrors the value one-to-one (`MustCodes.Email.Address.Invalid` ↔ `email.address.invalid`). **Every value is a composed `const`** — C# folds constant string concatenation at compile time, so `Prefix + ".invalid"` is still a compile-time constant and remains legal as an attribute argument (`ValidationAttributeBase(typeof(string), MustCodes.Email.Address.Invalid)`), a constant pattern (`MustCodes.Value.State.Null => …` in `GuardExceptionPolicy.Map`) and a `case` label — the three places that force `const` over `static readonly`. Composing makes mirroring structural: each segment is spelled exactly once, and `Prefix` at every level is the code of that node, which is what family matching wants (`code.StartsWith(MustCodes.Owasp.Prefix + '.', StringComparison.Ordinal)`). Two consequences: the full literal no longer appears in `MustCodes.cs`, so each leaf carries its folded value in its `<summary>` (`<c>email.address.invalid</c>`) to keep search working; and the mirroring/uniqueness checks move from Rule13 (a textual audit cannot fold constants) to a reflection test, `MustCodesTests` in Core (every `const string` in the tree **except those named `Prefix`** matches the three-segment grammar, equals its identifier path in kebab-case, is unique, and matches its `<summary>`; the `Prefix` constants are asserted separately — a domain `Prefix` equals the bare domain token, an aspect `Prefix` equals its parent's `Prefix + "." + kebab(aspect)`, and every leaf starts with its declaring class's `Prefix + "."`), while Rule13 keeps the textual checks — every clause passes a constant, and no code string literal appears outside `src/PineGuard.Core/Codes/` (Rule13 (c): every `"<domain>.` literal that matches a catalogue domain is flagged). Domain classes use the Plan 00 §5.4 map (`Text`, not `String`; `Value`, not `Object`; `Character`, not `Char`); the one domain that is itself a type name, `Boolean`, carries a single justified `[SuppressMessage("Naming", "CA1720", Justification = "Domain identifiers mirror the public code strings.")]` rather than a worse name. Type-variant clauses (numeric string parsers, the temporal quartet, both dictionary kinds) reference the same constant. Constants are plain strings, so nothing in the catalogue needs a TFM gate.

**Result type** — `src/PineGuard.Core/MustClauses/MustResult.cs`:

```csharp
public sealed class MustResult<T> : IMustResult
{
    public bool Success { get; }
    public bool Failed => !Success;
    public string Code { get; }              // "" on success
    public string Message { get; }           // formatted (unchanged semantics)
    public string MessageTemplate { get; }   // raw template with {paramName}; "" on success
    public string? ParamName { get; }
    public object? Value { get; }
    public T? Result { get; }
    object? IMustResult.Result => Result;

    public static MustResult<T> Ok(T? result, object? value = null, string? paramName = null);
    public static MustResult<T> Fail(string code, string messageTemplate, string? paramName, object? value);
    public static MustResult<T> FromBool(bool? ok, string code, string messageTemplate, string? paramName, object? value, T? result);
    public static MustResult<T> FromBool(bool? ok, string code, string messageTemplate, string? paramName, object? value);
    internal static MustResult<T> FailPreformatted(string code, string message, string messageTemplate, string? paramName, object? value);
    // implicit bool, Deconstruct (unchanged 5-way), ThrowIfFailed, ThrowIfFailed<TException>, ThrowNullIfFailed, OrThrow, OrThrow(T) — unchanged
}
```

`code` is the first parameter of `Fail`/`FromBool` ("fail with code X, saying Y"). `Fail` validates `code` is non-empty (`ArgumentException` — a configuration parameter, so throwing is permitted by `docs/ai/specs/spec.md` §5.4.2). Move the `{paramName}` substitution into an `internal static class MustMessage` (`Format(template, paramName)`) so `MustFailure` and the validator reuse it without touching the generic type.

**Clause bodies** — every clause in `src/PineGuard.MustClauses/` passes its catalogue constant on every failure path:

```csharp
public static MustResult<string> Email(this IMustClause _,
    string? value,
    [CallerArgumentExpression(nameof(value))] string? paramName = null)
{
    if (value is null)
        return MustResult<string>.Fail(MustCodes.Email.Address.Invalid, NullMessage, paramName, value);

    const string messageTemplate = "{paramName} must be a valid email address.";

    var ok = EmailRules.IsEmail(value);
    return MustResult<string>.FromBool(ok, MustCodes.Email.Address.Invalid, messageTemplate, paramName, value, result: value);
}
```

One clause, one code: the null early-return and any configuration-parameter misuse path (`nameof(predicate)`) use the same constant as the semantic failure; the message differentiates.

**Migration: generate the draft, curate the names, then rewrite by script.** A one-off generator in `tools/audit-cli/utils/` (`Run-Util03GenerateMustCodes.ps1`, PowerShell, `Rule10`-compliant header, all output under `artifacts/audit/`) runs in three passes:

1. **Draft** — reflect over the built `PineGuard.MustClauses` assembly (`src/PineGuard.MustClauses/bin/Release/net10.0/PineGuard.MustClauses.dll`) and emit `must-codes.map.json` under `artifacts/audit/`: one row per public clause method, `domain` pre-filled from the Plan 00 §5.4 map, `aspect`/`condition` left as a *proposed* kebab of the method name for the curator to replace, and a `sharesWith` slot for type-variant clauses.
2. **Curation checkpoint** — the implementer edits the map domain by domain against the Plan 00 §5.4 grammar, vocabulary and reading test, points type-variant clauses at one constant, and commits the map for the owner's review (`docs(must): propose the error-code catalogue`). This is the naming pass; it is reviewed like public API, and nothing is rewritten until it is signed off (budget: half a day across ~30 domains).
3. **Render + rewrite** — the generator renders `MustCodes.cs` (root) and one `MustCodes.<Domain>.cs` per domain from the map (aspect → condition nesting, map order, header comment listing the clause files served) and rewrites each clause file: every `MustResult<…>.Fail(` and `.FromBool(` call inside a public clause gains that clause's constant as its first argument (after `ok` for `FromBool`). Idempotent (skips calls already carrying a `MustCodes.` argument); prints what it could not rewrite (expect ~5: early returns inside nested lambdas, `MustNumberClauses` under `#if NET8_0_OR_GREATER`).

The map is a migration artifact: W4 (FluentValidation) reads it to pass the right constant per extension, then it is deleted — the catalogue and the call sites are the single source of truth thereafter, and new clauses (Phase 5) add their constant by hand.

**Audit** — new `Rule13 — Must Codes`, script `Test-Rule13-MustCodes.ps1` in `tools/audit-cli/rules/` registered in `tools/audit-cli/rules/Load-Catalog.ps1` (`-UsesConfiguration -UsesFailOnFindings`, output `artifacts/audit/Rule13-must-codes.txt`) and wired into `Run-AuditLibraryRules.ps1`; add a `.vscode/tasks.json` task `Audit: Must Codes (Rule13)`. Rule13 checks what a textual audit can see (source parsing, no build): (a) every `public static MustResult<…> X(` **or** `public static ValueTask<MustResult<…>> XAsync(` in `src/PineGuard.MustClauses/**` passes exactly one `MustCodes.<Domain>.<Aspect>.<Condition>` constant on every `Fail(`/`FromBool(` call; (b) every constant other than `Prefix` is referenced by at least one clause in `src/PineGuard.MustClauses/**`, one attribute in `src/PineGuard.DataAnnotations/**`, or one call site in `src/PineGuard.Core/**` or `src/PineGuard.AspNetCore/**` (`MustCodes.Value.State.Null` comes from `MustValidator<T>.Validate(null)`; `Value.Argument.Invalid` from the ASP.NET handler); (c) no code string literal (`"<domain>.` matching a catalogue domain) appears outside `src/PineGuard.Core/Codes/`; (d) every DataAnnotations attribute passes the constant of the clause it calls; (e) every `public static` guard passes its `result` to `GuardFailure.Throw`; (f) every clause file uses constants from its mapped domain only; (g) no `using PineGuard` line under `src/PineGuard.Core/Codes/`. **Staging**: Rule13 ships in 1a with (a), (b), (c), (f), (g) over the clause tree; 1c adds (d) in the same commit as the attribute rewrite; 1d adds (e) in the same commit as the guard rewrite — so `main` is never red between PRs. Grammar shape, identifier↔value mirroring, uniqueness and `<summary>` agreement need folded constants, so they belong to the reflection test `MustCodesTests` (§5.2), not to Rule13. Taste is the curation review's job. Also extend `tools/audit-cli/README.md` and `docs/ai/specs/tools/audit-cli/spec.md` catalogue tables.

**Other call sites of the factories** — `grep -rn "MustResult<" src --include=*.cs | grep -E "\.(Fail|FromBool)\("` outside `src/PineGuard.MustClauses/` today hits only `MustResultExtension.Combine` (Core). `FluentExtension` and the nullable-struct Fluent overloads call `MustResult<X>.Ok(default)` only, which is unchanged. Guard clauses never call the factories.

### 4.2 `IMustResult` — `+ src/PineGuard.Core/MustClauses/IMustResult.cs`

```csharp
public interface IMustResult
{
    bool Success { get; }
    bool Failed { get; }
    string Code { get; }
    string Message { get; }
    string MessageTemplate { get; }
    string? ParamName { get; }
    object? Value { get; }
    object? Result { get; }
}
```

The non-generic view lets `MustValidationResult.From(params IMustResult[])` collect results of different `T`, and lets Phase 4 bridges accept any result without reflection.

### 4.3 `MustFailure` — `+ src/PineGuard.Core/MustClauses/MustFailure.cs`

```csharp
public sealed record MustFailure(string PropertyPath, string Code, string Message, object? Value)
{
    /// Builds a failure from a failed result. propertyPath null → PropertyPath = result.ParamName ?? "" and Message = result.Message;
    /// propertyPath given → PropertyPath = propertyPath and Message = MessageTemplate rendered with propertyPath.
    public static MustFailure From(IMustResult result, string? propertyPath = null);
}
```

`From` throws `ArgumentNullException` for a null result and `ArgumentException` for a successful one (both are programmer errors). `Value` is the attempted value; adapters must never serialise it into responses — Phase 3 documents this and tests it (a failure whose `Value` is a secret produces a body that does not contain it).

### 4.4 `MustValidationResult` — `+ src/PineGuard.Core/MustClauses/MustValidationResult.cs`

```csharp
public sealed class MustValidationResult
{
    public static MustValidationResult Ok();                                   // shared empty instance
    public static MustValidationResult Fail(MustFailure failure, params MustFailure[] additional);   // the signature makes an empty failure list unrepresentable
    public static MustValidationResult Fail(IEnumerable<MustFailure> failures);
    public static MustValidationResult From(params IMustResult[] results);     // keeps failures only; Ok when none
    public static MustValidationResult From(IEnumerable<IMustResult> results);
    public static MustValidationResult Combine(params MustValidationResult[] results);   // same verb, same meaning as MustResultExtension.Combine: every failure kept
    public static MustValidationResult Combine(IEnumerable<MustValidationResult> results);

    public bool Success { get; }                                     // Failures.Count == 0
    public bool Failed => !Success;
    public IReadOnlyList<MustFailure> Failures { get; }
    public string Message { get; }                                   // "" or "{PropertyPath}: {Message}" joined by "; " (path omitted when empty)

    public MustValidationResult WithPropertyPathPrefix(string prefix);           // re-roots every failure under prefix (PropertyPathUtility.Combine)
    public void ThrowIfFailed();                                     // throws MustValidationException
    public static implicit operator bool(MustValidationResult? result);      // result?.Success ?? false — never NREs on a null local
    public override string ToString() => Message;
}
```

Invariant: `Success ⇔ Failures.Count == 0`; the `Fail` signatures make an empty failure list unrepresentable (`Fail(IEnumerable)` with zero items throws `ArgumentException`). Failures preserve insertion order (rule order, then element order) — adapters rely on that for deterministic responses.

### 4.5 `MustValidationException` — `+ src/PineGuard.Core/MustClauses/MustValidationException.cs`

```csharp
public class MustValidationException : Exception                                    // not sealed: OrderValidationException : MustValidationException is a legitimate consumer catch granularity
{
    public MustValidationException(MustValidationResult result);                 // Message = result.Message; null result → ArgumentNullException
    public MustValidationException(MustValidationResult result, string message);
    public MustValidationException(MustValidationResult result, string message, Exception? innerException);
    public MustValidationResult Result { get; }
}
```

This is the *validation-failed-at-a-boundary* marker. It deliberately does not derive from `ArgumentException`: single-value results throw argument exceptions (`MustResult<T>.ThrowIfFailed`, Guards); validation results throw validation exceptions — Phase 3's exception handler maps only the latter to HTTP 400 by default.

### 4.6 `PropertyPathUtility` — `+ src/PineGuard.Core/Utils/PropertyPathUtility.cs`

```csharp
public static class PropertyPathUtility
{
    public const char PropertySeparator = '.';
    public static string Combine(string? parent, string property);     // ("", "Email") → "Email"; ("Address", "City") → "Address.City"
    public static string Index(string? parent, int index);          // ("Lines", 2) → "Lines[2]"
    public static string Key(string? parent, string key);           // ("Headers", "Accept") → "Headers[Accept]" — no quotes, pinned by test; consistent with Lines[2]
    public static string Transform(string? path, Func<string, string> segmentTransform); // applies to identifier segments only; "[…]" untouched
    public static string FromExpression(LambdaExpression expression);   // x => x.Address.City → "Address.City"; x => x → ""; anything else → ArgumentException
}
```

`FromExpression` accepts `MemberExpression` chains (property or field), unwraps `UnaryExpression` conversions (boxing in `Func<T, object>`), and rejects method calls and indexers with an `ArgumentException` naming the unsupported node type. Phase 3 uses `Transform` to apply the app's JSON naming policy per segment.

### 4.7 Validator contract — `+ IMustValidator.cs`, `+ IMustValidatorOfT.cs`

```csharp
public interface IMustValidator
{
    Type ValidatedType { get; }
    MustValidationResult Validate(object? value);
    ValueTask<MustValidationResult> ValidateAsync(object? value, CancellationToken cancellationToken = default);
}

public interface IMustValidator<in T> : IMustValidator where T : notnull   // notnull: IMustValidator<object> would make Validate(object) ambiguous with the non-generic member
{
    MustValidationResult Validate(T value);
    ValueTask<MustValidationResult> ValidateAsync(T value, CancellationToken cancellationToken = default);

    Type IMustValidator.ValidatedType => typeof(T);
    MustValidationResult IMustValidator.Validate(object? value) => Validate(MustValidatorCast.To<T>(value));
    ValueTask<MustValidationResult> IMustValidator.ValidateAsync(object? value, CancellationToken cancellationToken) => ValidateAsync(MustValidatorCast.To<T>(value), cancellationToken);
}
```

The non-generic members are default interface implementations (C# 8, available on `netstandard2.1`) so a hand-rolled validator that implements **one** `IMustValidator<T>` writes only the two typed methods. `MustValidator<T>` implements them explicitly itself (one `T` per base class). A hand-rolled type implementing **two** closed `IMustValidator<T>`s inherits two candidate defaults and must implement `IMustValidator` explicitly (CS8705 otherwise) — the DI package's "one validator, several types" sample does exactly that. `MustValidatorCast.To<T>` (internal): `value is T t → t`; `value is null && default(T) is null → default!`; otherwise `ArgumentException` (wrong type is a programmer error). `ValidateAsync` is on the interface **now** so Phase 3 can add async rules without an interface change; the base class implements it synchronously in this phase. Future additions to the interface use default interface members — never abstract additions.

### 4.8 `MustValidator<T>` — `+ src/PineGuard.Core/MustClauses/MustValidator.cs`

```csharp
public abstract class MustValidator<T> : IMustValidator<T>
{
    protected MustPropertyRule<T, TProperty> RuleFor<TProperty, TResult>(Expression<Func<T, TProperty>> expression, Func<TProperty, MustResult<TResult>> check);
    protected MustPropertyRule<T, TProperty> RuleFor<TProperty, TResult>(Expression<Func<T, TProperty>> expression, Func<T, TProperty, MustResult<TResult>> check);
    protected MustPropertyRule<T, TProperty> RuleFor<TProperty>(Expression<Func<T, TProperty>> expression, IMustValidator<TProperty> validator);
    protected MustPropertyRule<T, TItem> RuleForEach<TItem, TResult>(Expression<Func<T, IEnumerable<TItem>?>> expression, Func<TItem, MustResult<TResult>> check);
    protected MustPropertyRule<T, TItem> RuleForEach<TItem, TResult>(Expression<Func<T, IEnumerable<TItem>?>> expression, Func<T, TItem, MustResult<TResult>> check);
    protected MustPropertyRule<T, TItem> RuleForEach<TItem>(Expression<Func<T, IEnumerable<TItem>?>> expression, IMustValidator<TItem> validator);

    public MustValidationResult Validate(T value);
    public virtual ValueTask<MustValidationResult> ValidateAsync(T value, CancellationToken cancellationToken = default);
}
```

Semantics (each is a test):

- Rules run in registration order; **all** failures are collected (aggregate mode; fail-fast is Phase 3).
- `Validate(null)` for a reference `T` returns one failure at `PropertyPath = ""` with `Code = MustCodes.Value.State.Null` and message `"{TypeName} must not be null."` — it never throws for the validated value.
- Member path = `PropertyPathUtility.FromExpression(expression)`; the failure message is `MessageTemplate` rendered with that path (so `e => Must.Be.Email(e)` does **not** leak the lambda parameter name `e` into the message — this is why `MessageTemplate` exists).
- Two-argument `check` receives `(instance, propertyValue)` and is attributed to the property — this is the cross-property story.
- Nested validator rules (`RuleFor(x => x.Address, addressValidator)`) skip when the property is `null` (presence is a separate rule) and re-root the nested result under the property path.
- `RuleForEach` skips a `null` collection; each element is checked with path `Property[i]`; a nested validator re-roots under `Property[i]`. Elements are enumerated exactly once.
- A rule's `When`/`Unless` conditions are evaluated against the instance before the check runs; multiple conditions AND together.
- The validator is immutable after construction: `RuleFor` is `protected` and documented as constructor-only; `Validate` is thread-safe, so validators are registered as singletons in DI.

### 4.9 `MustPropertyRule<T, TProperty>` — `+ src/PineGuard.Core/MustClauses/MustPropertyRule.cs`

```csharp
public sealed class MustPropertyRule<T, TProperty>
{
    public string PropertyPath { get; }
    public MustPropertyRule<T, TProperty> When(Func<T, bool> condition);
    public MustPropertyRule<T, TProperty> Unless(Func<T, bool> condition);
    public MustPropertyRule<T, TProperty> WithCode(string code);                  // overrides Code on every failure this rule emits; "" → ArgumentException
    public MustPropertyRule<T, TProperty> WithMessage(string messageTemplate);    // may contain {paramName}; overrides Message
    public MustPropertyRule<T, TProperty> WithPropertyPath(string propertyPath);      // overrides the expression-derived path (e.g. RuleFor(x => x, …) root rules)
}
```

Internally the validator holds a list of `IMustRuleRunner<T>` (internal interface) with one sealed runner per `RuleFor`/`RuleForEach` shape; `MustPropertyRule<T,TProperty>` is the public handle over one runner. Builder methods return `this`.

### 4.10 `InlineMustValidator<T>` — `+ src/PineGuard.Core/MustClauses/InlineMustValidator.cs`

`public sealed class InlineMustValidator<T> : MustValidator<T>` re-exposing the six `RuleFor`/`RuleForEach` overloads as `public new` forwarders. Used by tests and by Phase 2's `ValidateMustRules(v => v.RuleFor(...))`.

### 4.11 `MustResultExtension` additions (`src/PineGuard.Core/MustClauses/MustResult.cs`)

```csharp
public static MustResult<TNext> AndThen<T, TNext>(this MustResult<T> result, Func<T, MustResult<TNext>> next);  // failure propagates Code/Message/Template/ParamName/Value; success calls next(result.Result!)
public static MustResult<T> When<T>(this MustResult<T> result, bool condition);   // condition false → Ok(result.Result, result.Value, result.ParamName)
public static MustResult<T> Unless<T>(this MustResult<T> result, bool condition); // = When(!condition)
public static MustValidationResult ToMustValidationResult<T>(this MustResult<T> result, string? propertyPath = null);   // the lossless lift; named for its target (Plan 00 §12)
```

`Combine` keeps its shape and now returns `FailPreformatted(first.Code, joinedMessage, first.MessageTemplate, first.ParamName, first.Value)`; the XML docs on **both** `Combine`s state the lossiness difference and point at `MustValidationResult.From` / `ToMustValidationResult` as the lossless alternative. `When` evaluates the clause eagerly (clauses are pure and cheap); the doc says so.

### 4.12 FluentValidation adapter (`src/PineGuard.FluentValidation/`)

- `Common/FluentExtension.cs`: all three `MustBe` overloads gain `string? code = null` as the last parameter; when non-null the rule chain ends with `.WithErrorCode(code)`. The failure path is otherwise unchanged (still `.Must(...)` + `{ErrorMessage}` so a consumer's own `.WithMessage()` after a PineGuard extension keeps working).
- Every extension passes its clause's constant: `ruleBuilder.MustBe(val => Must.Be.Email(val, paramName: null), message, MustCodes.Email.Address.Invalid)`. Scripted (regex over `Fluent*Extensions.cs`: resolve the `Must.Be.<X>(` call to its clause class via the file's domain token, then to its constant via the W2 map; print anything ambiguous for manual completion — expect the `String*`/`Number` families that mirror two clause classes to need a look).
- Why static per extension rather than dynamic per failure: FluentValidation's `ErrorCode` is fixed at rule-build time (`WithErrorCode` or the validator's `Name`); the only dynamic route is `Custom(...)`, which changes the return type of every extension and silently disables `.WithMessage()`. Static codes from the catalogue are deterministic and auditable.
- Cross-property temporal overloads (W5): in `FluentDateTimeExtensions`, `FluentDateOnlyExtensions`, `FluentDateTimeOffsetExtensions`, `FluentTimeOnlyExtensions`, add `After`, `OnOrAfter`, `Before`, `OnOrBefore` overloads whose `other` is `Func<TModel, X>` instead of `X` (both the `X` and `X?` property forms, mirroring the parameter list of the existing scalar overload exactly), implemented with the already-present model-aware `MustBe(Func<T, TProp, MustResult<TResult>>)`. Method ordering: positive before `Not*` (`docs/ai/specs/fluent-validation/project.md` §1).

### 4.13 DataAnnotations adapter (`src/PineGuard.DataAnnotations/`)

- `Common/ValidationAttributeBase.cs`: `public abstract class ValidationAttributeBase(Type expectedType, string code, bool allowNull = true) : ValidationAttribute { public string Code { get; } = code; … }`. Every attribute passes the constant of the clause it adapts — `EmailAttribute() : ValidationAttributeBase(typeof(string), MustCodes.Email.Address.Invalid)` — and the intermediate bases (`NumberAttributeBase`, `CollectionAttributeBase`, `ObjectAttributeBase`, `GenericDictionaryAttributeBase`) gain a `string code` parameter and thread it through, which works because codes are per concept: `PositiveNumberAttribute` calls three numeric overloads that all share `number.sign.not-positive`. `FromMustResult`/`BuildFailureResult` keep returning the framework's own `ValidationResult`. Principle (one rule for all four adapters): PineGuard never subclasses a framework result type; it fills the slot the framework provides for a code (FluentValidation `ErrorCode`, ProblemDetails `failures`) and, where there is none, the adapter object carries it — which also gives design-time metadata (form generators, OpenAPI enrichers, the future object-graph walker read `attribute.Code` before any validation runs). Consumer-defined attributes derive from the same base and pass their own code string (`"acme.order.sku.unknown"` — their own domain, same three-segment failure-state grammar). Scripted: the 301 attribute constructor call sites are rewritten from the W2 map (attribute → the `Must.Be.X` it calls → constant); Rule13 checks each attribute's code equals that of the clause it invokes. `Code` is also the natural resource key for a later DataAnnotations localisation hook (`ErrorMessageResourceType`/`Name`).
- `+ ComparePropertyAttributes.cs` (W6): `public abstract class ComparePropertyAttributeBase(string otherProperty, string code) : ValidationAttributeBase(typeof(object), code, allowNull: true)` with `public string OtherProperty { get; }` — every sealed attribute forwards its constant, e.g. `public sealed class AfterPropertyAttribute(string otherProperty) : ComparePropertyAttributeBase(otherProperty, MustCodes.Date.Order.NotAfter)` and `protected object? GetOtherValue(ValidationContext context)` (property or field on `context.ObjectType`; missing member → `InvalidOperationException`, a configuration error). Ten sealed attributes, `[AttributeUsage(Property | Field | Parameter)]`, primary constructors, positive before negative:

  | Attribute | Adapts | Types |
  |---|---|---|
  | `AfterPropertyAttribute`, `OnOrAfterPropertyAttribute`, `BeforePropertyAttribute`, `OnOrBeforePropertyAttribute` | `Must.Be.After/OnOrAfter/Before/OnOrBefore(value, other, paramName: null)` | `DateTime`, `DateOnly`, `DateTimeOffset`, `TimeOnly` via a runtime-type `switch` with a throwing `default` arm (the polymorphic-family exception in `docs/ai/specs/data-annotations/project.md` §4) |
  | `GreaterThanPropertyAttribute`, `GreaterThanOrEqualPropertyAttribute`, `LessThanPropertyAttribute`, `LessThanOrEqualPropertyAttribute` | `MustNumberClauses` via `InvokeGenericMust` | `#if NET8_0_OR_GREATER` (the number clauses do not exist on `netstandard2.1`); value and other must be the same numeric type, else `InvalidOperationException` |
  | `EqualToPropertyAttribute`, `NotEqualToPropertyAttribute` | `MustObjectClauses.EqualTo/NotEqualTo` via generic invoke | any |

  Naming: `<Comparison>Property` says "compared to another property"; the framework's own `[Compare]` is equality-only and the name is left alone (`docs/ai/specs/language/naming-collisions.md`). Add `AfterProperty→After`, …, `NotEqualToProperty→NotEqualTo` to the `aliases` map in `docs/ai/specs/language/vocabulary.json` so Rule06 sees known concepts.

### 4.14 GuardClauses

Guards keep calling Must and keep throwing the BCL `ArgumentException` family by default — but the exception now **carries the code**, and the way an application substitutes its own exceptions is redesigned around one input type and one verb (§4.14.1). The BCL types have no code slot and PineGuard deliberately does not subclass them (analyzers and callers expect the real types), so the standard extensibility bag is used for downstream readers: `Exception.Data`, the same "fill the framework's slot" principle as the other adapters.

```csharp
/// A guard failure: what the guard checked, why it failed, and the standard exception it would throw.
/// Data type + static throw helpers on one type — the BCL idiom since ArgumentNullException.ThrowIfNull.
public sealed record GuardFailure(string Code, string Message, string? ParamName, object? Value, Exception Exception)   // a record with a public constructor: consumers unit-test their own map with new GuardFailure(…)
{
    public const string CodeDataKey = "pineguard.code";              // Data keys are addresses too: lowercase, namespaced
    public const string PropertyPathDataKey = "pineguard.property-path";

    // Exception: what this guard throws when no map is installed — the ArgumentNullException / ArgumentException it built,
    // or the exceptionCreator's result. Data is already stamped on it.

    [DoesNotReturn]
    public static void Throw(IMustResult result, string? message = null, Func<Exception>? exceptionCreator = null);
    // → builds the standard exception (or the exceptionCreator's), stamps Data[CodeDataKey] = result.Code and
    //   Data[PropertyPathDataKey] = result.ParamName ?? "" on it, wraps both in a GuardFailure, then:
    //   exceptionCreator given → throw it as-is (an explicit per-call choice always wins);
    //   otherwise → throw (ambient scope map ?? global map)?.Invoke(failure) ?? failure.Exception.
}
```

- Every guard body changes from `GuardFailure.Throw(message ?? result.Message, paramName, value, exceptionCreator)` to `GuardFailure.Throw(result, message, exceptionCreator)` — one exact regex over 538 call sites, scripted in W6b like the Fluent and DataAnnotations passes. The new overload takes `result.Value`; the script prints any call site whose old `value` argument was not the result's value for manual review. The string-based `Throw` and `ThrowAndReplace` are deleted (a consumer's own guard builds an `IMustResult` with `MustResult<T>.Fail(…)`).
- `MustResult<T>.ThrowIfFailed()`, `ThrowIfFailed<TException>(factory)` and `ThrowNullIfFailed()` stamp the same two keys, so the single-value escalation path and the Guard path agree.
- Messages are unchanged: the code is machine data and lives in `Data`, not in the human text (BCL convention; contrast Options, whose `ValidateOptionsResult` has only strings and therefore gets `[code]` in the text).
- Why this matters beyond Phase 3's guard handling: the map receives the structured failure, so a DDD team maps by code, by code family, or by exception type in one C# switch expression — no message parsing, no per-guard extension methods (the Ardalis gap). Rule13 gains a check that every guard passes its `result` to `Throw`.
- `guard-clauses/project.md` §8 and §11 templates are updated to the new call shape; the vocabulary spec is untouched (no new names).

#### 4.14.1 `GuardExceptionPolicy` — substituting your own exceptions (redesign)

Today `GuardExceptionPolicy` is two settings (`ExceptionReplacer`, `ReplaceDefaultExceptions`), an options bag and a scope method, and "no policy" is `null`/`false`. `ReplaceDefaultExceptions` is provably dead: the only exception that ever reaches the replacer is the default `ArgumentException` family, so with the flag `false` the replacer never fires — the Ardalis pain point (hard-wired exception types, no way to substitute a domain exception) reborn as a latch on our own fix. The class **stays** — startup configuration belongs on its own isolated type, not on `Guard`, which is typed at every call site — but its members are replaced, with no compatibility shims (Plan 00 §4.6):

```csharp
public static class GuardExceptionPolicy
{
    public static void        Map(Func<GuardFailure, Exception> map);          // app-wide: guards throw what the map returns
    public static IDisposable BeginScope(Func<GuardFailure, Exception> map);   // this scope only (AsyncLocal); disposing restores the previous map
    public static void        Clear();                                         // no map → standard argument exceptions
    public static bool        HasMap { get; }
}
```

```csharp
// Program.cs — by code, by code family, by exception type, in one expression; the last arm is the catch-all.
// Call Map once, at the composition root; tests use BeginScope.
using PineGuard.Codes;
using PineGuard.GuardClauses;

GuardExceptionPolicy.Map(failure => failure.Code switch
{
    MustCodes.Value.State.Null                                        => new MissingRequiredValueException(failure.ParamName, failure.Exception),
    var c when c.StartsWith(MustCodes.Owasp.Prefix + '.', StringComparison.Ordinal) => new SecurityViolationException(c, failure.Exception),
    MustCodes.Email.Address.Invalid                                       => new InvalidCustomerEmailException(failure.Message, failure.Exception),
    var c                                                                 => new DomainValidationException(c, failure.Message, failure.Exception),
});

using (GuardExceptionPolicy.BeginScope(f => new CheckoutException(f.Message, f.Exception))) { … }

GuardExceptionPolicy.Clear();
if (GuardExceptionPolicy.HasMap) { … }
```

Design rules that produced these names (recorded so nobody re-litigates them):

- **Name the state, not the switch.** A map has a self-explaining absence — no map, nothing mapped — so "off" needs no `null`, no flag and no `Default` value; `Clear()` says the map is gone. `Disable`/`Enable` were rejected because they imply a retained-but-inactive map: the dead flag in disguise.
- **One input type.** The map receives the `GuardFailure` — code, message, param name, value, and the standard exception — so the consumer switches on `failure.Code` directly. No two-parameter lambda, no `Data` digging, and no `Func<Exception, Exception>` shorthand overload (two single-parameter lambda overloads make `x => …` ambiguous to the compiler).
- **The class carries the noun; members do not repeat it.** `GuardExceptionPolicy.Map()` / `.Clear()` / `.HasMap`, not `.MapExceptions()` / `.ClearExceptionMap()`. `BeginScope` keeps the .NET word (`ILogger.BeginScope`) that PineGuard already used.
- **Isolate configuration from the hot path.** Members on `Guard` itself were tried and rejected: `Guard.` is typed thousands of times, `GuardExceptionPolicy.` once.
- **No DI helper.** It would register nothing; one line in `Program.cs` is the whole story.

Deleted: `ExceptionReplacer`, `ReplaceDefaultExceptions`, `GuardExceptionPolicyOptions`, `BeginScope(Action<GuardExceptionPolicyOptions>)`, `GuardFailure.ThrowAndReplace`, the string-based `GuardFailure.Throw`. Rejected along the way: a `Default`/`Global`/`Current` value model (three kinds of thing — value, slot, read — under one noun; high cognitive load); on/off verbs `ReplaceExceptions()`/`UseDefaultExceptions()`/`Suspend()`; `Replace` as the verb (its noun forms `HasExceptionReplacement`/`ClearExceptionReplacer` are clunky, and "replace with itself" reads oddly for the catch-all arm); `MapExceptionsInScope`/`MapExceptionsScoped` (over-qualified; the latter echoes DI's `AddScoped`); `UnmapExceptions()`; `TranslateExceptions` (Spring's word); `ThrowInstead`/`ThrowArgumentExceptions()` (read as imperative throws); `Disable…` as an alias overload (two names for one action); a `services.ConfigureGuardExceptionPolicy`/`AddGuardExceptionPolicy` helper.

#### 4.14.2 Reading the code downstream

`+ src/PineGuard.Core/GuardClauses/ExceptionExtension.cs` (namespace `PineGuard.GuardClauses`): `TryGetMustCode(this Exception exception, [NotNullWhen(true)] out string? code)`, `HasMustCode(this Exception exception, string code)` (ordinal), `GetMustPropertyPath(this Exception exception)` (returns `""` when absent). They read the two `Data` keys for code that meets an already-thrown exception — the Phase 3 exception handler, logging, a `catch` block — so nobody touches `Data` by string. The map itself never needs them: it receives the `GuardFailure`.

#### 4.14.3 `Guard.Against.Invalid` — the validator in Guard style

`Guard.Against.Invalid<T>(T value, IMustValidator<T> validator, [CallerArgumentExpression] string? paramName = null)` returns `value` when the validator passes and otherwise throws **the `ArgumentException` family through the exception map, exactly like every other guard** — the `GuardFailure` is built from the first failure (`Code`, `Message`, `PropertyPath` as `ParamName`, `Value`), so a map routes it by code like any value guard. It deliberately does **not** throw `MustValidationException`: that type is the boundary marker Phase 3 maps to HTTP 400, and a guard in a DDD constructor is three layers deep (Plan 00 §12). When the whole result is wanted at a boundary, the spelling is `validator.Validate(order).ThrowIfFailed()`. It is the Guard-style spelling of "validate this object or throw" (`Guard.Against.Invalid(this, OrderValidator.Instance)` in a DDD constructor; FV precedent `ValidateAndThrow`). Lives in `+ src/PineGuard.GuardClauses/GuardValidatorClauses.cs` beside the other guard clause classes (the project already references Core, where `IMustValidator<T>` lives); tests in `tests/PineGuard.GuardClauses.UnitTests/`. Rejected: `Guard.Against.Failing` (not a state of the value), `Guard.Against.InvalidObject` (redundant).

### 4.15 `netstandard2.1`

All Phase 1 Core types compile on every TFM. `MustCodes` constants are plain strings and need no gating even for clauses that exist only on net8+ (`MustNumberClauses`); W6's numeric attributes are gated to match the clauses they call.

### 4.16 Specs and docs to update (by PR — each PR updates only what it ships)

**1a** (W7): `must-clauses/project.md`, the `Codes` items, the audit-cli spec/README, the test-spec `Code`/family rows for `MustExpected`, the root README *Object validation* + *Error codes* sections and the `AndThen` example, the memory files, `docs/ai/specs/spec.md` §2.1 (`IMustValidatorOfT.cs` exception). **1b**: `fluent-validation/project.md` §3/§4.3 and `scaffold-fluent`. **1c**: `data-annotations/project.md` and `scaffold-annotation`. **1d**: `guard-clauses/project.md` and the README Guard section (W6b step 5). The list:

- `docs/ai/specs/must-clauses/project.md`: "Core types" list (+ the new types); a new *Error codes* section (format, catalogue, one-clause-one-code, Rule13); the canonical `Between` example updated to pass the code.
- `docs/ai/specs/fluent-validation/project.md` §3: the `code` parameter and the static-code rationale; §4.3 example updated.
- `docs/ai/specs/data-annotations/project.md`: the `code` constructor parameter and `Code` property (§2.1 formatting rule becomes `ValidationAttributeBase(typeof(X), MustCodes.…)`), the never-subclass-a-framework-result principle; §3.2 naming row for `<Comparison>Property`.
- `docs/ai/specs/testing/fixture.md` §1 and `docs/ai/specs/testing/unit-test.md` §2.2: the `Code` parameter on `MustExpected`/`FluentExpected`/`DataAnnotationExpected`; the new `MustValidationExpected`/`MustValidationCase<T>`/`BaseMustValidationUnitTest` family.
- `docs/ai/skills/scaffold-must/SKILL.md` template and success criteria: the code argument; `docs/ai/skills/scaffold-fluent/SKILL.md`: pass the catalogue constant.
- `docs/ai/specs/tools/audit-cli/spec.md`, `tools/audit-cli/README.md`: Rule13.
- Root `README.md`: add an *Object validation* section using §2.2, an *Error codes* section with the Plan 00 §13.6 availability matrix, and an `AndThen` example (the root README has none today); `src/PineGuard.MustClauses/README.md`: its `AndThen` example becomes true; `src/PineGuard.Core/README.md`: mention codes and `MustValidator<T>`.
- `docs/ai/memory/validation-builder.md` and `docs/ai/memory/code-reviewer.md`: "every clause passes its `MustCodes` constant" as a durable pattern / drift signal.

## 5. Testing plan

All tests follow `docs/ai/specs/testing/unit-test.md` + `fixture.md`: `[Theory]` + `TheoryData` + `[MemberData]`, `XxxTests.cs` paired with `XxxTestData.cs`, flat classes, `<Member>_BehavesAsExpected(tc)`, AAA markers, no empty datasets, 100 %/100 %.

### 5.1 New test-infrastructure family (`tests/PineGuard.Testing/UnitTests/MustClauses/`)

```csharp
public sealed record MustValidationExpected(bool IsValid, string? Message = null, int? FailureCount = null, string? PropertyPath = null, string? Code = null) : ReturnExpected(IsValid, Message);   // the family shape every layer uses (testing/project.md §3 rule 2)
public sealed record MustValidationCase<TValue>(string Name, TValue Value, MustValidationExpected Expected) : ReturnCase<TValue, MustValidationExpected>(Name, Value, Expected);
public abstract class BaseMustValidationUnitTest(ITestOutputHelper output) : BaseUnitTest(output)
{
    protected static void AssertResult<TValue>(MustValidationCase<TValue> testCase, MustValidationResult result); // IsValid; then FailureCount; then PropertyPath/Code/Message against Failures[0] when provided
}
```

Justified by `docs/ai/specs/testing/project.md` §3 rule 1: Core (this phase), Options (Phase 2), AspNetCore and MediatR (Phases 3–4) all assert results. Add `string? Code = null` as a trailing positional parameter to `MustExpected`, `FluentExpected`, `DataAnnotationExpected`; `BaseMustUnitTest.AssertResult` asserts `result.Code`, `BaseFluentUnitTest` asserts `result.Errors[0].ErrorCode`, `BaseDataAnnotationUnitTest` asserts the code through an `AssertResult(tc, result, string? actualCode)` overload — the test passes `attribute.Code`, because `PineGuard.Testing` references only `PineGuard.Core` and must not gain a `PineGuard.DataAnnotations` reference (`GuardFailure.CodeDataKey` *is* in Core, so `BaseGuardUnitTest` uses the constant) — each only when the expectation carries a code. Add `MustValidationScenarioExtension.ToMustValidationCases<T>(this RuleScenario<T>[])` mirroring `GuardScenarioExtension`. Tests for the family itself go in `tests/PineGuard.Testing.UnitTests/UnitTests/MustClauses/`.

### 5.2 Core (`tests/PineGuard.Core.UnitTests/`)

Core tests must not reference `PineGuard.MustClauses` (they don't today); sample validators build results with `MustResult<T>.Ok`/`Fail` and local predicates — which also proves the contract needs no clause package.

| Tests / TestData | Covers |
|---|---|
| `MustClauses/MustResultTests` (extend existing) | `Code`/`MessageTemplate` on Ok/Fail/FromBool; empty code → `ArgumentException`; `IMustResult.Result` boxing |
| `MustClauses/MustResultTests` (extend — `MustResultExtension` stays in `MustResult.cs`, and Rule53 maps a test file to a source file by name) | operation groups `AndThen` success/failure/propagation; `When`/`Unless` both branches; `ToMustValidationResult` with and without path; `Combine` code propagation; `ThrowIfFailed<TException>(Func<IMustResult, …>)`; null-safe implicit bool |
| `Codes/MustCodesTests` | reflection Theory over every constant except those named `Prefix` (asserted separately: domain token / parent `Prefix` + kebab(aspect) / every leaf starts with its class's `Prefix + "."`): grammar regex `^[a-z][a-z0-9]*(-[a-z0-9]+)*(\.[a-z][a-z0-9]*(-[a-z0-9]+)*){2}$`, identifier path ↔ value mirroring (`Email.Address.Invalid` ↔ `email.address.invalid`), value uniqueness, every condition in the Plan 00 §5.4 vocabulary |
| `MustClauses/MustFailureTests` | `From` with/without path, success → `ArgumentException`, null → `ArgumentNullException`; `Value` round-trips but is absent from `ToString()` (the PII guard) |
| `MustClauses/MustValidationResultTests` | `Ok` singleton, `Fail` (empty → `ArgumentException`), `From` (mixed success/failure, all success), `Combine`, `WithPropertyPathPrefix`, `Message` formatting (with and without path), `ThrowIfFailed`, implicit bool, `ToString` |
| `MustClauses/MustValidationExceptionTests` | both constructors, `Result` |
| `MustClauses/MustValidatorTests` | every bullet in §4.8 as a scenario: single-member, cross-property attribution, `When`/`Unless`/AND, nested validator (null skip, re-rooting), `RuleForEach` (null skip, indices, nested), root null, `ValidateAsync` parity, non-generic `IMustValidator` dispatch incl. wrong type → `ArgumentException`, `WithCode`/`WithMessage`/`WithPropertyPath`, single enumeration (use a counting enumerable in TestData shared fields) |
| `MustClauses/MustPropertyRuleTests` | builder returns `this`; `PropertyPath`; `WithCode("")` → `ArgumentException` |
| `MustClauses/InlineMustValidatorTests` | all six public forwarders |
| `Utils/PropertyPathUtilityTests` | `Combine`/`Index`/`Key`/`Transform` edge cases (empty parent, nested indices); `FromExpression` for property chain, field, boxing conversion, root, method call → `ArgumentException` |

Sample types (`OrderLine`, `CreateOrder`, validators) live in `tests/PineGuard.Core.UnitTests/MustClauses/Samples/` as ordinary classes; their file names must not end in `Tests` or `TestData` (Rule50 pairs those suffixes in both directions).

### 5.3 MustClauses (`tests/PineGuard.MustClauses.UnitTests/`)

- Existing 541 operation groups keep passing untouched (messages unchanged).
- Add `Code` to the `MustExpected` in the `InvalidCases` factories of one representative group per clause file (≈50 groups), e.g. `new MustExpected(false, "value must be a valid email address.", Code: MustCodes.Email.Address.Invalid)` — a spot check that the wiring is right; Rule13 is the exhaustive check.

### 5.4 FluentValidation (`tests/PineGuard.FluentValidation.UnitTests/`)

- `Common/FluentExtensionTests` (extend): `code` null → `ErrorCode` is FluentValidation's default; `code` set → `ErrorCode == code`; consumer `.WithMessage()` after a PineGuard extension still wins.
- Add `Code:` to one representative `FluentExpected` per extension file (≈50).
- New TestData/Tests groups for the 32 cross-property temporal overloads (fixture-backed via the existing `DateTimeRulesFixtures.IsAfter`-style scenario arrays projected with `Project` into a two-property model).

### 5.5 DataAnnotations (`tests/PineGuard.DataAnnotations.UnitTests/`)

- `Code:` on one representative `DataAnnotationExpected` per attribute file, asserted against the attribute under test; `Common/ValidationAttributeBaseTests` (extend): `Code` round-trips, empty code → `ArgumentException`.
- `ComparePropertyAttributesTests` + TestData: per attribute — valid, invalid, null value skip, missing other property → `InvalidOperationException`, type mismatch → `InvalidOperationException`, each temporal type, `#if NET8_0_OR_GREATER` numeric groups.

### 5.6 GuardClauses (`tests/PineGuard.Core.UnitTests/GuardClauses/` for the Core types; `tests/PineGuard.GuardClauses.UnitTests/` for the clauses)

In `tests/PineGuard.Core.UnitTests/GuardClauses/` (`GuardExceptionPolicy`, `GuardFailure`, `ExceptionExtension` live in Core):

- `GuardExceptionPolicyTests` (rewrite): no map → standard exceptions and `HasMap == false`; `Map(null)` throws; the map receives a `GuardFailure` with code/message/param/value/stamped exception; the map result is thrown; a map returning `failure.Exception` is a no-op; `Clear()` restores standard exceptions; `BeginScope` overrides the global map, nested scopes restore in order, flows across `await` (AsyncLocal), disposal restores; `exceptionCreator` bypasses the map. The scripted cleanup rewrites every `ExceptionReplacer =` / `ReplaceDefaultExceptions =` / `BeginScope(o => …)` use in the existing test files (four files reference them today) to the new members.
- `GuardFailureTests` (extend): property population for null and non-null values; the `IMustResult` overload — default exception stamped, `exceptionCreator` exception stamped and bypasses the map, global-map and scoped-map results thrown, `message` override honoured, `[DoesNotReturn]` contract. `MustResultTests` (extend): the `ThrowIfFailed` family stamps both keys.
- `ExceptionExtensionTests`: each accessor with key present / absent / wrong type; `HasMustCode` ordinal.

In `tests/PineGuard.GuardClauses.UnitTests/`:

- `GuardValidatorClausesTests` (`Guard.Against.Invalid`): passes → returns value; fails → `ArgumentException` (or `ArgumentNullException` for a null value) with `Data` stamped from the first failure and `paramName` captured; map applied and receives the first failure's code; a null validator → `ArgumentNullException`.
- `GuardExpected` gains `string? Code = null`; `BaseGuardUnitTest.AssertThrow` asserts `ex.Data[GuardFailure.CodeDataKey]` when set, and `AssertResult` asserts `Data[GuardFailure.PropertyPathDataKey]` equals the expected `ParamName` when both are set.
- Existing 405 guard operation groups (49 test files; 538 `Throw` call sites) keep passing untouched; add `Code:` to one representative `GuardExpected` per guard file (≈50) as the wiring spot check — Rule13 (e) is the exhaustive one.

### 5.7 Audit and tooling

- Rule13 run against the migrated tree must be clean; deliberately break one clause locally, confirm Rule13 fails, revert.
- Rule06 clean (vocabulary aliases for the `*Property` attributes); Rule08 clean (no reordering).

## 6. Playbook

Commands are run from the worktree path. `<wt>` is the **absolute** worktree path (`d:/…/PineGuard/.claude/worktrees/structural-validation` on the operator's machine); every command below uses it.

### W0 — Set up

1. Plan 00 §6 steps 0–2 with `<slug> = structural-validation`.
2. Read, in order: `docs/ai/specs/spec.md`, `docs/ai/specs/orchestration.md`, `docs/ai/specs/must-clauses/project.md`, `docs/ai/specs/fluent-validation/project.md`, `docs/ai/specs/data-annotations/project.md`, `docs/ai/specs/testing/unit-test.md`, `docs/ai/specs/testing/fixture.md`, `docs/ai/specs/testing/project.md`, `docs/ai/specs/tools/spec.md` (for the Rule13 script).
3. Baseline: `dotnet build <wt>/PineGuard.slnx -c Release` then `pwsh -NoProfile -ExecutionPolicy Bypass -File "<wt>/tools/testing/Run-Tests.ps1" -Solution "<wt>/PineGuard.slnx"`; record the passing test count per TFM.

### W1 — Core result types

1. Add `IMustResult`, extend `MustResult<T>` — add **only** the 4-argument coded `Fail` and the 6-argument coded `FromBool` beside the old overloads (different arities, no ambiguity); the 5-argument coded `FromBool` is added in W2 step 3 in the same commit that deletes the code-less overloads, because it would be ambiguous with the existing 5-argument `FromBool` for `MustResult<string>` — internal `MustMessage`; add `MustFailure`, `MustValidationResult`, `MustValidationException`, `PropertyPathUtility`, and the `MustResultExtension` additions. The W1 commit must build and pass with both overload sets present.
2. Write the tests in §5.2 for these types (TestData first, then Tests).
3. `pwsh … Run-CodeCoverage.ps1 -Mode GenerateAndAnalyze -Scope Core -SkipHtml -Enforce100` → 100/100.
4. Commit: `feat(core): add error codes, MustValidationResult and result combinators`.

### W2 — Codes migration

1. Write `Run-Util03GenerateMustCodes.ps1` in `tools/audit-cli/utils/` (header per `docs/ai/specs/tools/spec.md`; outputs to `artifacts/audit/`). Build Release first; run the **draft** pass → `artifacts/audit/must-codes.map.json`.
2. **Curation checkpoint.** Edit the map domain by domain against Plan 00 §5.4 (grammar, domain map, condition vocabulary, sharing rule, reading test). Render the map as a markdown table — `new-surfaces-must-codes-catalogue.md` in `docs/ai/plans/` (`type: plan`, `status: open`; the folder holds flat `.md` files only, taxonomy §Plans) — commit `docs(must): propose the error-code catalogue`, and **stop for the owner's sign-off** — codes are public API and this is the one place taste enters. Fold review feedback back into the map. Nothing under `src/` has been touched at this point.
3. Run the **render + rewrite** pass. Fix the printed manual cases. Delete the code-less `Fail`/`FromBool` overloads; build — the compiler now proves every call site passes a code.
4. Write `Test-Rule13-MustCodes.ps1` in `tools/audit-cli/rules/`, register in `Load-Catalog.ps1`, `Run-AuditLibraryRules.ps1`, `.vscode/tasks.json`; document in `tools/audit-cli/README.md` and `docs/ai/specs/tools/audit-cli/spec.md`.
5. `pwsh -NoProfile -ExecutionPolicy Bypass -File "<wt>/tools/audit-cli/Run-All.ps1" -Configuration Release -RuleId Rule13,Rule06,Rule08,Rule50` → clean. Break one clause, confirm Rule13 fails, revert.
6. Add `Code` to `MustExpected` (+ `BaseMustUnitTest`), the ≈50 representative code assertions (§5.3); `MustCodesTests` (§5.2).
7. Coverage `-Scope MustClauses` and `-Scope Core` → 100/100.
8. Commits: `feat(must): stamp every clause with its MustCodes constant` and `feat(tools): add audit Rule13 for Must codes`. Commit the JSON map as `tools/audit-cli/utils/must-codes.map.json` (W4 in PR 1b and W6 in PR 1c read it from `main`; `artifacts/` is not shared across worktrees); the last of 1b/1c/1d to merge deletes it, and the markdown review copy moves to `docs/ai/plans/completed/` (status `completed`) in that same PR. Also correct `tools/audit-cli/README.md`'s "Rule01–Rule10" description of `Run-AuditLibraryRules.ps1` to the real range while adding Rule13.

### W3 — Validator keystone

1. Add `IMustValidator`, `IMustValidator<T>`, `MustValidator<T>`, `MustPropertyRule<T,TProperty>`, `InlineMustValidator<T>` and the internal runners/cast helper.
2. Add the `MustValidationExpected`/`MustValidationCase<T>`/`MustValidationScenarioExtension`/`BaseMustValidationUnitTest` family to `tests/PineGuard.Testing/`, with tests following the existing convention: extend `tests/PineGuard.Testing.UnitTests/UnitTests/ExpectedTests(.TestData).cs` and `CaseRecordTests(.TestData).cs`, add `UnitTests/MustClauses/BaseMustValidationUnitTestTests.cs` + `…TestData.cs` covering every branch of `AssertResult` (IsValid only; + FailureCount; + PropertyPath/Code/Message; empty `Failures` guard).
3. Write the validator tests (§5.2) with the sample types.
4. Coverage `-Scope Core` and `-Scope Testing` → 100/100.
5. Commit: `feat(core): add MustValidator<T> object validation keystone`.

### W4 — FluentValidation codes

1. `FluentExtension.MustBe` `code` parameter; scripted pass over `src/PineGuard.FluentValidation/Fluent*Extensions.cs`; fix printed ambiguities by hand.
2. `Code` on `FluentExpected` + `BaseFluentUnitTest`; extension tests; ≈50 representative assertions.
3. Coverage `-Scope FluentValidation` → 100/100. Commit: `feat(fluent): emit Must codes as FluentValidation ErrorCode`.

### W5 — FluentValidation cross-property temporal overloads

1. Add the 32 overloads; tests per §5.4; coverage; commit `feat(fluent): add model-aware After/Before overloads`.

### W6 — DataAnnotations (may be its own PR)

1. `code` parameter on `ValidationAttributeBase` and the intermediate bases; scripted pass over the 301 attributes from the W2 map; `Code` on `DataAnnotationExpected` + the `BaseDataAnnotationUnitTest` overload; representative assertions.
2. `ComparePropertyAttributes.cs`, `vocabulary.json` aliases, tests per §5.5.
3. Coverage `-Scope DataAnnotations` → 100/100; Rule06 clean. Commit(s): `feat(annotations): carry Must codes in validation results`, `feat(annotations): add cross-property comparison attributes`.

### W6b — GuardClauses: exception policy redesign, codes on exceptions, `Guard.Against.Invalid` (PR 1d, may run in parallel with 1b/1c)

0. `GuardExceptionPolicy.Map` / `BeginScope` / `Clear` / `HasMap` and the `GuardFailure` data type (§4.14.1); `GuardExceptionPolicyOptions`, `ThrowAndReplace` and the string-based `Throw` deleted; `ExceptionExtension` accessors (§4.14.2); `Guard.Against.Invalid` (§4.14.3).
1. `GuardFailure.Throw(IMustResult, …)` + the two `Data` key constants; stamping applied before the map runs; `MustResult<T>.ThrowIfFailed`/`ThrowNullIfFailed` stamp too.
2. Scripted rewrite of the 538 guard call sites (`GuardFailure.Throw(message ?? result.Message, paramName, value, exceptionCreator)` → `GuardFailure.Throw(result, message, exceptionCreator)`); print anything the regex does not match, and any site whose `value` argument is not `result.Value`, for manual completion.
3. Rule13: add the guard check (every `public static` guard passes its `result` to `GuardFailure.Throw`).
4. `Code` on `GuardExpected` + `BaseGuardUnitTest`; tests per §5.6; `-Scope GuardClauses` and `-Scope Core` 100/100; `Run-All.ps1 -RuleId Rule03,Rule06,Rule08,Rule13,Rule50` clean.
5. `docs/ai/specs/guard-clauses/project.md` §8/§11 templates and its exception-policy section; README Guard section rewritten around `GuardExceptionPolicy.Map`, `BeginScope`, `Clear`, the §4.14.1 switch example and `Guard.Against.Invalid`; the four doc files that mention `ReplaceDefaultExceptions` updated.
6. Commit: `feat(guard): map guard failures to your own exceptions and carry Must codes on thrown exceptions`.

### W7 — Docs and memory

1. Every item in §4.16. Run `pwsh … Run-All.ps1 -RuleId Rule11` → clean.
2. Commit: `docs(brain): document error codes, MustValidator and Rule13`.

### W8 — Gates, PR, merge

1. Plan 00 §7 in full (`-Scope All` coverage included). `dotnet format PineGuard.slnx` then `--verify-no-changes`.
2. Plan 00 §6 steps 6–9. PR body: the surface diff (§3.2), the migration script name, the Rule13 addition, and the test-count delta.

## 7. Definition of Done (per PR)

Plan 00 §7 applies to each of the four PRs, plus:

**1a** — `MustCodes` has exactly one constant per public clause method; Rule13 (a)(b)(c)(f)(g) clean; Rule06/Rule08/Rule09/Rule10/Rule50 clean; the §2.2 example compiles and prints the three failures with codes; the root README *Object validation* and *Error codes* sections and the `AndThen` example exist; `gold-standard.md` re-verified for Core, Must and Testing.
**1b** — Fluent extensions emit codes; the 32 temporal overloads exist; `gold-standard.md` re-verified for FluentValidation.
**1c** — every attribute passes its constant; Rule13 (d) added and clean; ten cross-property attributes; `gold-standard.md` re-verified for DataAnnotations.
**1d** — the Guard policy redesign, codes on exceptions, `ExceptionExtension`, `Guard.Against.Invalid`; Rule13 (e) added and clean; Rule03 clean; `gold-standard.md` re-verified for GuardClauses and Core.

## 8. Risks

| Risk | Mitigation |
|---|---|
| The 550-clause rewrite is done by hand and drifts | Script it; the compiler (no code-less overloads) + Rule13 prove completeness |
| Lambda parameter names leak into result messages | `MessageTemplate` re-rendering; a test asserts `e =>` never appears in a message |
| Overload resolution surprises between 1-arg and 2-arg `RuleFor` lambdas | Tests cover both; the async variants get an explicit `Async` suffix in Phase 3 for the same reason |
| Interface growth later breaks implementers | `ValidateAsync` is on the interface now; everything else future is a default interface member |
| `vocabulary.json` aliases forgotten → Rule06 red | W6 step 2 is explicit and W8 runs Rule06 |
| The 538 guard call sites and 85 policy-using test files are rewritten by hand and drift | Scripted rewrite; the deleted string-based `Throw` makes the compiler prove completeness; Rule13 (e) audits it |
| `GuardExceptionPolicy` global map leaks between parallel test classes (xUnit runs classes in parallel) | Tests set the global map only inside a dedicated `[Collection]` that disables parallelisation; everything else uses `BeginScope` |

## 9. Out of scope

Async rules, fail-fast mode, clock injection, object-graph walking, localisation, any rename from `docs/ai/plans/core-common-api-decisions.md`.

<!-- footer
last_verified: 2026-08-26
-->

<!-- plan-nav -->
> [Parent](new-surfaces-missing-validation-cases.md) · [00 Program](new-surfaces-missing-validation-cases-00-program.md) · **01 Structural validation** · [02 Options](new-surfaces-missing-validation-cases-02-options.md) · [03 ASP.NET Core](new-surfaces-missing-validation-cases-03-aspnetcore.md) · [04 MediatR & bridges](new-surfaces-missing-validation-cases-04-mediatr-result-bridges.md) · [05 Rule batches](new-surfaces-missing-validation-cases-05-rule-batches.md) · [06 Analyzers](new-surfaces-missing-validation-cases-06-analyzers.md)
<!-- /plan-nav -->
