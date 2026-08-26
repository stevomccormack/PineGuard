<!-- metadata_header
type: plan
id: library-expansion-roadmap
version: 2.0
status: planned
last_updated: 2026-08-26
-->

# Plan: Library Expansion Roadmap

> **Reconciled (2026-08-26)**: this roadmap is consumed through [Plan 00](new-surfaces-missing-validation-cases-00-program.md) §2, which keeps the parent plan's six-phase numbering and adopts this document's keystone. Names below have been aligned to Plan 00 §5 (`MustValidationResult`, `ValidateMustRules()`, `AddMustValidation(...)`, the `<domain>.<aspect>.<condition>` code grammar); where any remaining detail disagrees with Plan 00, Plan 00 wins.

> **Status**: Planned | **Authors**: Fable intelligence pass (2026-08-20), merged with Fable growth review (2026-08-25)
>
> Supersedes `new-surfaces-missing-validation-cases.md` and `object-validation-and-integration-bridges.md` (merged into this document).
>
> Companion documents: `competitive-analysis.md` (Section 6), `future-language.md` (Section 7).
>
> Scope note: ISO/standards-registry validations (country codes, currencies, IBAN registries, etc.) are explicitly **out of scope** — they belong to the separate standards repo project.

## Context

PineGuard today covers five surfaces from one rule engine: Core Rules, MustClauses, GuardClauses, FluentValidation, and DataAnnotations — and it validates **values**: `MustResult<T>` wraps a single value. But every integration surface where validation actually runs in modern .NET apps (auto-validation filters, options validation, mediator pipelines) validates **request/config models**. This roadmap captures:

1. The **object-level validator keystone** every integration package consumes (Part 1)
2. **New integration surfaces** — the adapter packages (Part 2)
3. **Middleware/pipeline design requirements** and blind spots (Part 3)
4. **Structural validation gaps** that decide head-to-head evaluations (Part 4)
5. The **DataAnnotations object story** — `IValidatableObject` / `ValidationResult` / `ValidationContext` (Part 5)
6. **Json/Xml rule depth** within Core purity limits (Part 6)
7. **Rule-level gaps** (Part 7)
8. **`PineGuard.Testing` as a public package** (Part 8)

Prioritized so effort lands where evaluations against FluentValidation and the .NET 10 built-in validator are decided.

Guiding principle: **Core stays pure** (deterministic, sync, zero-allocation predicates). Everything below is either a new adapter package over the existing engine, or a new rule family that fits the Core character. Anything requiring I/O, schemas, or transformation lives outside Core.

---

## Part 1 — The Keystone: `IMustValidator<T>` + `MustValidationResult`

The single highest-leverage addition. Every integration in Part 2 and the DataAnnotations bridge in Part 5 become thin adapters over this contract; without it, each adapter package would invent its own incompatible object-level shape.

### 1.1 Contract

```csharp
public interface IMustValidator<in T>
{
    MustValidationResult Validate(T instance);
}
```

- `MustValidationResult`: aggregate of named results (member path → failures), with the uniform `IsValid` boolean roll-up matching the repo-wide `Expected.IsValid` convention.
- Member paths must support nesting and collection indexing (`Items[2].Name`) so the collection-element work in Part 4 lands on a ready error-path model.
- Error entries carry the stable machine-readable code (Part 3 item 5) alongside the human message from day one — retrofitting codes into an aggregate type later is breaking.

### 1.2 Base class for composition

```csharp
public sealed class CreateOrderValidator : MustValidator<CreateOrder>
{
    public CreateOrderValidator()
    {
        RuleFor(x => x.Currency, v => v.Must().Be.IsoCurrencyCode());
        RuleFor(x => x.Quantity, v => v.Must().Be.Positive());
    }
}
```

- `RuleFor` captures the member name via expression (or `CallerArgumentExpression`) — this is where the property-name-vs-JSON-naming-policy requirement (Part 3 item 1) gets its seam: the raw member path is stored; adapters apply the app's naming policy at render time.
- `When(...)` / `Unless(...)` and cross-property rules from Part 4 compose here.
- Async variant (`IMustAsyncValidator<T>` or `ValidateAsync` on the same interface) follows the `MustAsync` rule: async exists at the Must layer and above, never in Core.

### 1.3 Why first

`PineGuard.Extensions.Options`, `PineGuard.AspNetCore`, the mediator adapters, and the DataAnnotations graph walker all need "validate this object, get an aggregate back". Ship the contract (in Core, or a small `PineGuard.Validation` package if Core must stay clause-only) before any adapter, and treat it as unstable until two adapters consume it.

---

## Part 2 — New Integration Surfaces (packages)

Ordered by strategic value.

### 2.1 `PineGuard.Extensions.Options` — startup configuration validation

An `IValidateOptions<TOptions>` implementation that runs Must clauses against bound configuration, wired for `ValidateOnStart()`. Config validation at startup is one of the most common validation needs in modern .NET and is underserved by every competitor.

- `services.AddOptions<SmtpOptions>().BindConfiguration("Smtp").ValidateMustRules().ValidateOnStart();`
- Resolves `IMustValidator<TOptions>` (Part 1); failure aggregates all violations into one `OptionsValidationException` message (never fail one-at-a-time — restart loops are expensive).
- Zero dependencies beyond `Microsoft.Extensions.Options`.

### 2.2 `PineGuard.AspNetCore` — request pipeline integration

One package, several cooperating pieces:

- **Minimal API (.NET 10)**: integrate with `Microsoft.Extensions.Validation` (`AddValidation()`, `[ValidatableType]`, source-generated). This is Microsoft's new built-in story — riding it solves trimming/AOT and positions PineGuard as the rule library for the platform validator. Also ship a plain `IEndpointFilter` for pre-.NET-10 minimal APIs.
- **MVC auto-validation**: an async-safe `IAsyncActionFilter` (FluentValidation dropped official auto-validation because sync model binding can't run async validators — do it correctly and capture that abandoned demand). Both filter types return RFC 9457 `ValidationProblemDetails`, consistently shaped, via a public `MustValidationResult → ValidationProblemDetails` mapper so manual-validation users get the same 400 bodies.
- **Guard exception handling**: an `IExceptionHandler` (net8+) mapping the `ArgumentException` family to `ProblemDetails`. **Critical policy**: a guard firing at the API boundary is a 400; the same guard three layers deep is a programmer error and must stay 500 — blanket 400-mapping masks bugs. Introduce a marker (distinct exception type or exception `Data` tag) so the handler can distinguish boundary validation from invariant violations.
- **Replacement exceptions for Guards**: `exceptionFactory` overloads and/or `Guard.Against.Null<TException>(...)` so domain code can throw `DomainValidationException` instead of `ArgumentNullException`. Framework exceptions remain the default (BCL conventions and analyzers expect them); replacement is opt-in per call or via configured policy.
- **DI registration**: `services.AddMustValidation(params Assembly[])` — assembly scanning for validators, singleton lifetime for stateless ones.
- **`HttpClient` integration — response-first**: by the time a `DelegatingHandler` runs, the outbound model is already serialized into `HttpContent`, so request validation there means buffering and re-deserializing our own body. Revised design:
  - **Primary value = response contract validation** (opt-in, forces buffering): a `DelegatingHandler` asserting status class, content type (the existing `IsJsonContentType(headers)` signature already anticipates exactly this), and optionally payload shape; throws a rich contract-violation exception. Aimed at third-party API integration.
  - **Request-side validation belongs before serialization**: in the typed client via `IMustValidator<T>` — ship `AddHttpClient<T>().ValidateRequests(...)` resolving validators pre-send, and document the pattern; do not deserialize outbound content in a handler.

### 2.3 Mediator pipeline behaviors — mediator-agnostic

A pipeline behavior that runs validators before handlers is the single most copy-pasted validation snippet in .NET codebases; shipping it first-party is high leverage, tiny surface. Design requirements:

- **All validation logic lives on `IMustValidator<T>`** (Part 1); each adapter is a ~40-line shim. MediatR's v13+ commercial licensing change is pushing teams to Wolverine and source-generated Mediator — ship `PineGuard.MediatR` (`IPipelineBehavior<TRequest, TResponse>`) plus equivalent adapters (or documented patterns) for Wolverine and Mediator.
- **Configurable failure mode**: throw `MustValidationException` vs. short-circuit a `Result`-shaped response — support both via options, decided per-registration.
- Equivalent filters for MassTransit (`IFilter<ConsumeContext<T>>`) as a follow-up — message consumers are a major non-HTTP validation entry point.

### 2.4 Result-pattern bridges

Thin adapters mapping Must results onto ErrorOr / FluentResults / OneOf (`mustResult.ToErrorOr()`, etc.). Cheap to build, broadens adoption into result-oriented codebases. One micro-package per target library to keep dependency graphs clean.

### 2.5 `PineGuard.Analyzers` — Roslyn analyzer + code fix

Flag hand-rolled `if (x is null) throw new ArgumentNullException(nameof(x));` and offer a code-fix to `Guard.Against.Null(x)`. This is a **distribution channel** as much as a feature — a continuous in-editor advertisement no competitor ships. Can later also serve the alias-discoverability goal in `future-language.md` §7.2.

### 2.6 Explicitly deferred surfaces

- **XSD schema validation**: `XmlRules` already covers well-formedness. Schema conformance pulls in `XmlSchemaSet`, is slow and I/O-shaped — if ever built, it is a separate `PineGuard.Xml` package, never Core (even though `XmlSchemaSet` is in-box). Same reasoning for JSON Schema, which additionally requires a third-party dependency (JsonSchema.Net) → future `PineGuard.Json`.
- **Transforms/coercion** (Zod-style): PineGuard validates; transformation is a different concern (reaffirming competitive-analysis §5).
- **gRPC interceptors, SignalR hub filters, Hangfire job filters, Azure Functions middleware**: real entry points, listed here so they aren't blind spots — but demand-driven, after 2.1–2.3 prove the adapter pattern.

---

## Part 3 — Middleware Blind Spots (design requirements for Part 2.2)

Failure modes observed in how validation is actually wired in production apps. Each is a requirement, not a suggestion, for `PineGuard.AspNetCore`:

1. **Property-name mismatch**: errors keyed by C# property (`FirstName`) while the client sent `firstName`. Error keys MUST respect the app's `JsonSerializerOptions` naming policy. Almost every library gets this wrong. (Seam lives in `MustValidator<T>` — see Part 1.2.)
2. **Minimal APIs skip MVC filters**: any MVC-only auto-validation silently validates nothing on minimal endpoints. Ship both filter types from day one (see 2.2).
3. **Headers, query strings, and route values**: validation frameworks obsess over the body; provide first-class support for validating bound non-body parameters.
4. **Aggregate vs. short-circuit**: default to aggregating all errors per request; expose fail-fast as configuration. Cancellation tokens must flow through async validators.
5. **Error codes vs. messages**: stable machine-readable codes (e.g. `email.address.invalid`) separate from human messages, carried in `ProblemDetails` extensions. Frontends and API consumers key on codes; messages get localized.
6. **Localization**: message resolution through `IStringLocalizer` seam (English default). Validot shipped 5 languages; PineGuard needs the seam even if translations come later.
7. **Trimming/AOT**: reflection-heavy DataAnnotations paths must be documented per package; the .NET 10 source-generated route is the AOT story.
8. **Response validation is nearly never done**: opt-in response contract checking (DelegatingHandler inbound per 2.2 + endpoint filter outbound) is a differentiator for teams integrating third-party APIs.

---

## Part 4 — Structural Validation Gaps (decide evaluations, build first)

These are what FluentValidation actually wins on when a team evaluates head-to-head. They matter more than any individual rule, and they compose on the `MustValidator<T>` base from Part 1:

| Gap | Shape | Notes |
|---|---|---|
| Cross-property validation | `Must` support for comparing two model properties (`EndDate > StartDate`) with a correct two-property error path | Range rules validate a range *object*; this is different. #1 day-one question from evaluators. DataAnnotations layer needs a `<Comparison>PropertyAttribute`-style story too. |
| Conditional composition | `When(...)` / `Unless(...)` on Must chains | CheckValidators' AndIf/OrIf validates the demand. |
| Collection element validation | Apply a clause per element with indexed error paths (`Items[2].Name`) | FluentValidation's `RuleForEach` equivalent; error-path model owned by `MustValidationResult` (Part 1.1). |
| Async predicate seam | `MustAsync` at the Must layer only | Core stays sync. Needed for DB-uniqueness-style checks; required before the ASP.NET filters can claim full FV parity. |
| Error codes | Stable code per rule, owned by Core alongside the message | Prerequisite for Part 3 item 5 and for `MustValidationResult` (Part 1.1). |
| Clock injection | `TimeProvider`-aware in-past/in-future/min-age rules | Nobody does testable temporal validation well — differentiator. |

---

## Part 5 — Complete the DataAnnotations Object Story

The DataAnnotations layer currently adapts individual clauses to attributes. Three additions make PineGuard a first-class citizen of the `System.ComponentModel.DataAnnotations` object model (`IValidatableObject` / `ValidationResult` / `ValidationContext`):

### 5.1 `MustResult → ValidationResult` conversion

One-liner ergonomics inside `IValidatableObject.Validate`:

```csharp
public IEnumerable<ValidationResult> Validate(ValidationContext context)
{
    yield return Quantity.Must().Be.Positive().ToValidationResult(nameof(Quantity));
    yield return Currency.Must().Be.IsoCurrencyCode().ToValidationResult(nameof(Currency));
}
```

Plus the aggregate form: `MustValidationResult.ToValidationResults()`.

### 5.2 Recursive object-graph validator

`Validator.TryValidateObject` famously does **not** recurse into child objects or collections; teams have hand-rolled this for 15 years. Ship a correct, fully tested graph walker:

- Walks properties, nested objects, and collection elements; honors `[ValidateComplexType]`-style opt-in/opt-out; cycle-safe via reference tracking.
- Runs DataAnnotations attributes, `IValidatableObject`, and any registered `IMustValidator<T>` per node; returns a single `MustValidationResult` with full member paths.
- This is a genuine BCL pain-point fix and a differentiator independent of the fluent API.

### 5.3 `ValidationContext` service resolution

PineGuard attributes and the graph walker honor `ValidationContext.GetService`, so DI-dependent validation works in ASP.NET model validation, Blazor `EditForm`, and options validation without special wiring.

---

## Part 6 — Deepen Json/Xml (within Core purity)

Current coverage is shallow: `JsonRules` has `IsJson` / `IsJsonObject` / `IsJsonArray` / `IsJsonContentType`; `XmlRules` has only `IsXml` / `IsXmlContentType`. In-Core additions (dependency-free, sync, deterministic):

- `TryParseJson` returning `JsonDocument` (matches the existing `TryXxx` util pattern).
- `HasJsonProperty(json, path)` — JSON-Pointer-style existence/lookup check.
- `MaxJsonDepth` / max-payload-size checks — DoS guards for API boundaries.
- Structured content-type checks handling `+json` / `+xml` suffixes (`application/problem+json`, `application/hal+json`) — extend the existing `IsJsonContentType` / `IsXmlContentType` rather than regex-matching literals.
- Xml: secure-parse well-formedness check (XXE-safe `XmlReaderSettings` — DTD processing off, no resolver), `HasXPathMatch`.

Schema conformance (JSON Schema, XSD) stays deferred to satellite packages per 2.6.

---

## Part 7 — Missing Rule-Level Cases (non-ISO)

Existing coverage is broad (see competitive-analysis §2 — base64, hex, IP/CIDR already present). Remaining gaps, grouped; each lands as a full vertical slice (Core → Must → Guard → Fluent → DA → Tests) per repo convention:

### String formats & identifiers
- `Contains` / `StartsWith` / `EndsWith` as first-class (not regex workarounds) — trivial, universal, Zod parity
- JWT structural shape (three base64url segments — not signature verification); SemVer; cron expression; slug; ULID; UUID-version check; hostname (RFC 1123 label) and scheme-restricted HTTP URL; MAC address (verify `NetworkRules` coverage); MIME/content-type; valid-regex-pattern
- Base64Url (distinct from Base64); valid-UTF-8 byte sequence

### Text & Unicode correctness
- Printable-only / no control characters / no BOM / valid surrogate pairs
- Unicode normalization form (NFC/NFD)
- **Grapheme-count length** vs. `char` count — the classic emoji-length bug; high-value, rarely offered

### Numeric & financial semantics
- Decimal precision/scale (money-safe); percentage bounds; Luhn checksum (algorithmic, so it belongs here, not the ISO repo)

### Temporal semantics
- In-past / in-future with `TimeProvider` (see Part 4); minimum-age from date of birth; weekday/weekend

### Files & content
- Magic-bytes file signature vs. extension (spoofing defense — complements existing `FilePathRules`)

---

## Part 8 — Ship `PineGuard.Testing` as a Public NuGet

`tests/PineGuard.Testing/` (12 classes, 100% coverage) currently serves internal suites only. Once consumers write `IMustValidator<T>` implementations, the case-record / `TheoryData` infrastructure becomes a selling point — the `FluentValidation.TestHelper` equivalent: "we give you the test harness too."

- Add validator-level assertion helpers (`ShouldHaveErrorFor(x => x.Currency)`-style) over `MustValidationResult`.
- Requires packaging hygiene: XML docs, README, versioning aligned with the main packages, and the same CI gates.

---

## Part 9 — Phasing

| Phase | Deliverable | Why this order |
|---|---|---|
| 1 | Keystone contract: `IMustValidator<T>` + `MustValidationResult` + `MustValidator<T>` base, incl. error codes (Parts 1, 4) | Everything else consumes it; retrofitting codes into the aggregate later is breaking |
| 2 | Structural gaps: cross-property, conditional, collection-element (Part 4) | Decide competitive evaluations; compose on the Phase 1 base |
| 3 | `PineGuard.Extensions.Options` (2.1) | Smallest new package; stress-tests the contract before it ossifies; immediate real-world value |
| 4 | `PineGuard.AspNetCore` (2.2) + `MustAsync` seam | The flagship integration; consumes Phases 1–2 outputs |
| 5 | DataAnnotations object story (Part 5) | Independent of ASP.NET; can run in parallel with Phases 3–4 |
| 6 | Mediator adapters (2.3) + result bridges (2.4) + `HttpClient` response validation (2.2) | Small surfaces, broad reach |
| 7 | Rule-level batches (Part 7) + Json/Xml Core depth (Part 6), highest-frequency first (Contains/StartsWith/EndsWith → JWT/SemVer/hostname → Unicode/grapheme → the rest) | Continuous cheap wins in parallel with Phases 3–6 |
| 8 | `PineGuard.Testing` public package (Part 8) | After the validator contract stabilizes |
| 9 | `PineGuard.Analyzers` (2.5) | Growth channel once the surface it advertises is stable |

## Risks

| Risk | Mitigation |
|---|---|
| `MustValidationResult` shape ossifies before adapters stress it | Build `Extensions.Options` (smallest adapter) immediately after; treat the contract as unstable until two adapters consume it |
| New packages dilute the 100%-coverage / audit-rule discipline | Every new package adopts the same CI gates (`ci.yml` paths-filter entries, coverage, Rule50) before first commit |
| ASP.NET integration couples releases to framework versions | Multi-target (net8.0;net10.0) as the existing packages do; .NET 10 source-gen pieces behind TFM conditionals |
| `MustAsync` leaks async into Core | Hard rule: async exists only in the Must layer and above; Core signatures stay sync — enforce via audit-cli rule |
| Blanket Guard→400 mapping masks server bugs | Boundary-marker design in 2.2 is mandatory, not optional |
| Object-graph walker correctness (cycles, collections, inheritance) | Full vertical test slice with adversarial fixtures before public release; this feature's value *is* its correctness |
| Duplicating FluentValidation's object API invites feature-parity churn | Scope `MustValidator<T>` deliberately: rules, conditions, collections, async — no transforms, no localization engine beyond the `IStringLocalizer` seam |
| Testing package leaks internal conventions publicly | API review pass before packing; internal-only helpers stay `internal` |

## Out of Scope

- ISO/standards-registry validations (separate repo)
- XSD/JSON Schema conformance (future `PineGuard.Xml` / `PineGuard.Json` if demand appears)
- Transforms/coercion
- Signature verification for JWT rules (structural shape only)
- Error message translations beyond the localization seam (seam ships; translations are demand-driven)
