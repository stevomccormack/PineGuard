# Plan: New Surfaces & Missing Validation Cases (Fable)

> **Status**: Proposed | **Author**: Fable intelligence pass | **Created**: 2026-08-20
>
> Companion documents: `competitive-analysis.md` (Section 6), `future-language.md` (Section 7).
>
> Scope note: ISO/standards-registry validations (country codes, currencies, IBAN registries, etc.) are explicitly **out of scope** — they belong to the separate standards repo project.

## Context

PineGuard today covers five surfaces from one rule engine: Core Rules, MustClauses, GuardClauses, FluentValidation, and DataAnnotations. This plan captures (1) new integration surfaces where validation actually runs in modern .NET apps, (2) middleware/pipeline design considerations and blind spots, and (3) rule-level and structural validation gaps — prioritized so effort lands where head-to-head evaluations against FluentValidation and the .NET 10 built-in validator are decided.

Guiding principle: **Core stays pure** (deterministic, sync, zero-allocation predicates). Everything below is either a new adapter package over the existing engine, or a new rule family that fits the Core character. Anything requiring I/O, schemas, or transformation lives outside Core.

---

## Part 1 — New Integration Surfaces (packages)

Ordered by strategic value.

### 1.1 `PineGuard.Extensions.Options` — startup configuration validation

An `IValidateOptions<TOptions>` implementation that runs Must clauses against bound configuration, wired for `ValidateOnStart()`. Config validation at startup is one of the most common validation needs in modern .NET and is underserved by every competitor.

- `services.AddOptions<SmtpOptions>().BindConfiguration("Smtp").ValidateWithPineGuard().ValidateOnStart();`
- Failure aggregates all violations into one `OptionsValidationException` message (never fail one-at-a-time — restart loops are expensive).
- Zero dependencies beyond `Microsoft.Extensions.Options`.

### 1.2 `PineGuard.AspNetCore` — request pipeline integration

One package, several cooperating pieces:

- **Minimal API (.NET 10)**: integrate with `Microsoft.Extensions.Validation` (`AddValidation()`, `[ValidatableType]`, source-generated). This is Microsoft's new built-in story — riding it solves trimming/AOT and positions PineGuard as the rule library for the platform validator. Also ship a plain `IEndpointFilter` for pre-.NET-10 minimal APIs.
- **MVC auto-validation**: an async-safe `IAsyncActionFilter` (FluentValidation dropped official auto-validation because sync model binding can't run async validators — do it correctly and capture that abandoned demand). Both filter types return RFC 9457 `ValidationProblemDetails`, consistently shaped.
- **Guard exception handling**: an `IExceptionHandler` (net8+) mapping the `ArgumentException` family to `ProblemDetails`. **Critical policy**: a guard firing at the API boundary is a 400; the same guard three layers deep is a programmer error and must stay 500 — blanket 400-mapping masks bugs. Introduce a marker (distinct exception type or exception `Data` tag) so the handler can distinguish boundary validation from invariant violations.
- **Replacement exceptions for Guards**: `exceptionFactory` overloads and/or `Guard.Against.Null<TException>(...)` so domain code can throw `DomainValidationException` instead of `ArgumentNullException`. Framework exceptions remain the default (BCL conventions and analyzers expect them); replacement is opt-in per call or via configured policy.
- **DI registration**: `services.AddPineGuard(params Assembly[])` — assembly scanning for validators, singleton lifetime for stateless ones.
- **DelegatingHandler** for `HttpClient`: validate outgoing request payloads (cheap, on by default when registered); response validation opt-in (forces buffering + double deserialization) but valuable for third-party API contract enforcement.

### 1.3 `PineGuard.MediatR` — pipeline behavior

An `IPipelineBehavior<TRequest, TResponse>` that runs validators before handlers. This is the single most copy-pasted validation snippet in .NET codebases; shipping it first-party is high leverage, tiny surface. Equivalent filters for MassTransit (`IFilter<ConsumeContext<T>>`) as a follow-up — message consumers are a major non-HTTP validation entry point.

### 1.4 Result-pattern bridges

Thin adapters mapping Must results onto ErrorOr / FluentResults / OneOf (`mustResult.ToErrorOr()`, etc.). Cheap to build, broadens adoption into result-oriented codebases. One micro-package per target library to keep dependency graphs clean.

### 1.5 `PineGuard.Analyzers` — Roslyn analyzer + code fix

Flag hand-rolled `if (x is null) throw new ArgumentNullException(nameof(x));` and offer a code-fix to `Guard.Against.Null(x)`. This is a **distribution channel** as much as a feature — a continuous in-editor advertisement no competitor ships. Can later also serve the alias-discoverability goal in `future-language.md` §7.2.

### 1.6 Explicitly deferred surfaces

- **XSD schema validation**: `XmlRules` already covers well-formedness. Schema conformance pulls in `XmlSchemaSet`, is slow and I/O-shaped — if ever built, it is a separate `PineGuard.Xml` package, never Core. Same reasoning for JSON Schema.
- **Transforms/coercion** (Zod-style): PineGuard validates; transformation is a different concern (reaffirming competitive-analysis §5).
- **gRPC interceptors, SignalR hub filters, Hangfire job filters, Azure Functions middleware**: real entry points, listed here so they aren't blind spots — but demand-driven, after 1.1–1.3 prove the adapter pattern.

---

## Part 2 — Middleware Blind Spots (design requirements for Part 1.2)

Failure modes observed in how validation is actually wired in production apps. Each is a requirement, not a suggestion, for `PineGuard.AspNetCore`:

1. **Property-name mismatch**: errors keyed by C# property (`FirstName`) while the client sent `firstName`. Error keys MUST respect the app's `JsonSerializerOptions` naming policy. Almost every library gets this wrong.
2. **Minimal APIs skip MVC filters**: any MVC-only auto-validation silently validates nothing on minimal endpoints. Ship both filter types from day one (see 1.2).
3. **Headers, query strings, and route values**: validation frameworks obsess over the body; provide first-class support for validating bound non-body parameters.
4. **Aggregate vs. short-circuit**: default to aggregating all errors per request; expose fail-fast as configuration. Cancellation tokens must flow through async validators.
5. **Error codes vs. messages**: stable machine-readable codes (e.g. `pineguard.string.email`) separate from human messages, carried in `ProblemDetails` extensions. Frontends and API consumers key on codes; messages get localized.
6. **Localization**: message resolution through `IStringLocalizer` seam (English default). Validot shipped 5 languages; PineGuard needs the seam even if translations come later.
7. **Trimming/AOT**: reflection-heavy DataAnnotations paths must be documented per package; the .NET 10 source-generated route is the AOT story.
8. **Response validation is nearly never done**: opt-in response contract checking (DelegatingHandler inbound + endpoint filter outbound) is a differentiator for teams integrating third-party APIs.

---

## Part 3 — Structural Validation Gaps (decide evaluations, build first)

These are what FluentValidation actually wins on when a team evaluates head-to-head. They matter more than any individual rule:

| Gap | Shape | Notes |
|---|---|---|
| Cross-property validation | `Must` support for comparing two model properties (`EndDate > StartDate`) with a correct two-property error path | Range rules validate a range *object*; this is different. #1 day-one question from evaluators. DataAnnotations layer needs a `[PgCompare*]`-style story too. |
| Conditional composition | `When(...)` / `Unless(...)` on Must chains | CheckValidators' AndIf/OrIf validates the demand. |
| Collection element validation | Apply a clause per element with indexed error paths (`Items[2].Name`) | FluentValidation's `RuleForEach` equivalent. |
| Async predicate seam | `MustAsync` at the Must layer only | Core stays sync. Needed for DB-uniqueness-style checks; required before the ASP.NET filters can claim full FV parity. |
| Error codes | Stable code per rule, owned by Core alongside the message | Prerequisite for Part 2 item 5. |
| Clock injection | `TimeProvider`-aware in-past/in-future/min-age rules | Nobody does testable temporal validation well — differentiator. |

---

## Part 4 — Missing Rule-Level Cases (non-ISO)

Existing coverage is broad (see competitive-analysis §2 — base64, hex, IP/CIDR already present). Remaining gaps, grouped; each lands as a full vertical slice (Core → Must → Guard → Fluent → DA → Tests) per repo convention:

### String formats & identifiers
- `Contains` / `StartsWith` / `EndsWith` as first-class (not regex workarounds) — trivial, universal, Zod parity
- JWT structural shape; SemVer; cron expression; slug; ULID; UUID-version check; hostname (RFC 1123 label) and scheme-restricted HTTP URL; MAC address (verify `NetworkRules` coverage); MIME/content-type; valid-regex-pattern
- Base64Url (distinct from Base64); valid-UTF-8 byte sequence

### Text & Unicode correctness
- Printable-only / no control characters / no BOM / valid surrogate pairs
- Unicode normalization form (NFC/NFD)
- **Grapheme-count length** vs. `char` count — the classic emoji-length bug; high-value, rarely offered

### Numeric & financial semantics
- Decimal precision/scale (money-safe); percentage bounds; Luhn checksum (algorithmic, so it belongs here, not the ISO repo)

### Temporal semantics
- In-past / in-future with `TimeProvider` (see Part 3); minimum-age from date of birth; weekday/weekend

### Files & content
- Magic-bytes file signature vs. extension (spoofing defense — complements existing `FilePathRules`)

---

## Part 5 — Phasing

| Phase | Deliverable | Why this order |
|---|---|---|
| 1 | Structural gaps: cross-property, conditional, collection-element, error codes (Part 3) | Decide competitive evaluations; prerequisites for Phase 3 |
| 2 | `PineGuard.Extensions.Options` (1.1) | Smallest new package; proves the adapter pattern; immediate real-world value |
| 3 | `PineGuard.AspNetCore` (1.2) + `MustAsync` seam | The flagship integration; consumes Phase 1 outputs |
| 4 | `PineGuard.MediatR` (1.3) + result bridges (1.4) | Small surfaces, broad reach |
| 5 | Rule-level batches (Part 4), highest-frequency first (Contains/StartsWith/EndsWith → JWT/SemVer/hostname → Unicode/grapheme → the rest) | Continuous cheap wins in parallel with Phases 2–4 |
| 6 | `PineGuard.Analyzers` (1.5) | Growth channel once the surface it advertises is stable |

## Risks

| Risk | Mitigation |
|---|---|
| New packages dilute the 100%-coverage / audit-rule discipline | Every new package adopts the same CI gates (`ci.yml` paths-filter entries, coverage, Rule50) before first commit |
| ASP.NET integration couples releases to framework versions | Multi-target (net8.0;net10.0) as the existing packages do; .NET 10 source-gen pieces behind TFM conditionals |
| `MustAsync` leaks async into Core | Hard rule: async exists only in the Must layer and above; Core signatures stay sync — enforce via audit-cli rule |
| Blanket Guard→400 mapping masks server bugs | Boundary-marker design in 1.2 is mandatory, not optional |

## Out of Scope

- ISO/standards-registry validations (separate repo)
- XSD/JSON Schema conformance (future `PineGuard.Xml` / `PineGuard.Json` if demand appears)
- Transforms/coercion
- Error message translations beyond the localization seam (seam ships; translations are demand-driven)
