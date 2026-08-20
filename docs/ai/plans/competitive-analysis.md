<!-- metadata_header
type: plan
id: competitive-analysis
version: 1.0
status: living
last_updated: 2026-08-20
-->

# PineGuard Competitive Analysis

> **Status**: Living document — a standing reference, not a work item.
> **Last updated**: 2026-08-20 (Fable intelligence pass)

## 1. The Competitive Landscape

### Tier 1 -- Direct Competitors (High Relevance)

#### Ardalis.GuardClauses

**What it does**: Throw-on-failure guard methods via `Guard.Against.X()`.
**Coverage**: ~12 built-in guards (Null, NullOrEmpty, NullOrWhiteSpace, OutOfRange, Zero, Negative, NegativeOrZero, EnumOutOfRange, OutOfSQLDateRange, Expression, InvalidFormat, NotFound).
**Extensibility**: Static extension methods on `IGuardClause`.
**Strengths**: Massive adoption. Simple. Extensible pattern.
**Weaknesses**: Limited rule coverage (~12 vs PineGuard's 300+). Inconsistent naming. No broader validation ecosystem. No deterministic rule engine underneath -- each guard is a standalone island.
**PineGuard advantage**: PineGuard.GuardClauses delivers the same `Guard.Against.X` pattern but backed by 300+ deterministic Core Rules. Every guard shares the same rule engine as Must, Fluent, and DataAnnotations layers.

#### FluentValidation (846M+ downloads)

**What it does**: Fluent DSL for building validator classes (`RuleFor(x => x.Property).NotEmpty().EmailAddress()`).
**Coverage**: ~40 built-in validators. Custom rules via `Must()`. ASP.NET Core integration.
**Strengths**: Dominant adoption. Expressive DSL. Excellent ASP.NET integration. Strong community.
**Weaknesses**: Runtime rule model -- rules are scattered across validator classes, not reusable outside that context. No guard clauses. No DataAnnotations. No deterministic rule engine. Rules cannot be shared across layers.
**PineGuard advantage**: PineGuard.FluentValidation extends FluentValidation's `IRuleBuilder` with 300+ validators, all backed by the same Core Rules. You get FluentValidation's DSL with PineGuard's rule engine -- not a replacement, an amplification.

#### Zod (TypeScript -- conceptual benchmark)

**What it does**: Schema-first validation with static type inference. The gold standard in TypeScript.
**Coverage**: Comprehensive -- 30+ string validators (email, url, uuid, nanoid, cuid, ulid, ip, cidr, mac, jwt, base64, hex, hash, iso.date/time/datetime/duration, emoji, hostname, httpUrl), number validators (min, max, int, positive, negative, finite, safe, multipleOf), date validators, branded types, coercion, transforms, pipe.
**Strengths**: Schema = type. Immutable fluent API. Exhaustive string format coverage. Transforms and coercion built-in.
**Weaknesses**: TypeScript only. No guard clause pattern. No DataAnnotations equivalent.

**PineGuard parity check**:

| Zod Feature | PineGuard Status |
|---|---|
| email, url | Has (Email, StrictEmail, Uri, AbsoluteUri) |
| uuid | Has (IsGuid) |
| ip (v4/v6), cidr | Has (IsIpAddress, Ipv4Address, Ipv6Address, NetworkAddress) |
| base64, hex | Has (IsBase64, IsHex) |
| jwt | **Gap** |
| nanoid, cuid, cuid2, ulid | **Gap** |
| emoji | **Gap** |
| hostname | Has (`NetworkRules.IsValidHostname`, `Must.Be.Hostname` / `Must.Not.Be.Hostname`) |
| mac address | **Gap** |
| hash (md5/sha256/etc.) | **Gap** |
| regex, includes, startsWith, endsWith | Has (IsMatch) -- **Gap**: includes/startsWith/endsWith as first-class |
| trim, toLowerCase, toUpperCase | Has casing validation -- no transform/coercion layer |
| branded types | **Gap** (C# nominal types serve similar purpose) |
| transforms / pipe / coercion | **Gap** (PineGuard validates, does not transform) |

### Tier 2 -- Niche Competitors (Medium Relevance)

#### Validot (Archived May 2025)

**What it does**: Specification-based validation. Fluent spec builder, thread-safe singleton validators.
**Coverage**: Email, length, numeric, collection, nullable. Built-in translations (PL, ES, RU, PT, DE).
**Strengths**: 2.5x faster than FluentValidation. 8x less memory. Template pattern for API documentation. Fail-fast mode. Translation support.
**Weaknesses**: **Archived** -- no longer maintained. Specification-only (no guards, no attributes).
**PineGuard insight**: Validot's performance claims and translation architecture are worth studying. PineGuard's Core Rules are already pure static methods (zero allocation), which should match or exceed Validot's performance.

#### CheckValidators

**What it does**: Fluent `new Check<T>(value).IfNull().IfNotEmail().ThrowErrors()` pattern.
**Coverage**: Null, string (empty, length, email, regex, contains), DateTime (UTC), custom LINQ predicates.
**Strengths**: Simple generic `Check<T>` class. Multiple terminal operations (ThrowErrors, ReturnErrors, IsValid, HasErrors, GetErrors). AndIf/OrIf conditional chaining. Nested validation composition.
**Weaknesses**: Small coverage. No ecosystem. No integration with FluentValidation or DataAnnotations.
**PineGuard insight**: The AndIf/OrIf conditional chaining pattern is interesting for short-circuit validation flows.

#### SimpleValidator

**What it does**: `Must()` and `MustNot()` extensions on a validator object.
**Coverage**: String (length, email, password, credit card), numeric (range, comparison), DateTime (comparison, DateOnly variants), type checking, null.
**Strengths**: `Must`/`MustNot` naming (familiar pattern). `IRule` interface for business rules. Staged validation. Conversion methods (ToInt, ToBool, etc.). `ValidationMethodResult<T>` pattern.
**Weaknesses**: Small ecosystem. Limited validators. No guard clauses. No DataAnnotations.
**PineGuard insight**: The `Must`/`MustNot` naming validates PineGuard's `Must.Be.X` / `Must.Not.Be.X` API design.

#### Ensure.That

**What it does**: `Ensure.That(value).IsNotNull()` fluent guard pattern.
**Strengths**: Readable. Simple API.
**Weaknesses**: Small ecosystem. Limited extensibility.

#### Throw

**What it does**: `value.Throw().IfNull().IfEmpty()` -- extension method guards.
**Strengths**: Concise syntax.
**Weaknesses**: Not rule-based. Tiny ecosystem.

#### Dawn.Guard

**What it does**: `Guard.Argument(value).NotNull().NotEmpty()` -- modern fluent guards.
**Strengths**: Fluent. Modern C#.
**Weaknesses**: Small ecosystem. Not deterministic rule-driven.

### Tier 3 -- Adjacent Libraries (Low Direct Competition)

#### MiniValidation (Damian Edwards)

**What it does**: Lightweight DataAnnotations + IValidatableObject validation via `MiniValidator.TryValidate()`.
**Coverage**: Delegates to System.ComponentModel.DataAnnotations. Recursive validation with cycle detection.
**Strengths**: Single-line validation. Minimal APIs integration. .NET Standard 2.0.
**Weaknesses**: No custom rules beyond DataAnnotations. No guard clauses. No fluent API.
**PineGuard insight**: MiniValidation targets Minimal APIs -- PineGuard.DataAnnotations attributes work anywhere MiniValidation does, with 300+ validators instead of the ~12 built-in ones.

#### PakValidate.DataAnnotations

**What it does**: Pakistan-specific validators (CNIC, Mobile, NTN, IBAN, Postal Code, Landline, Vehicle Plate, STRN) as DataAnnotations attributes.
**Strengths**: Validation + metadata extraction in one call (e.g., validate IBAN and extract bank name). Country-specific domain expertise.
**Weaknesses**: Pakistan-only. Niche.
**PineGuard insight**: The validation-plus-extraction pattern is compelling for domain-specific validation scenarios.

#### ExpressiveAnnotations

**What it does**: `[RequiredIf("IsCustomer == true")]` -- conditional attribute validation via expression strings.
**Strengths**: Attribute-based conditional rules.
**Weaknesses**: Reflection-heavy. Brittle string expressions.

#### Nager.Validation

**What it does**: IBAN, VAT, postal code validation.
**Weaknesses**: Data validators, not a developer framework.

#### ASP.NET Core Built-in DataAnnotations

**Coverage**: ~12 attributes (Required, StringLength, Range, RegularExpression, Compare, EmailAddress, Phone, Url, CreditCard, DataType, Remote, ValidateNever). `CreditCard` has no PineGuard counterpart — payment-instrument validation left the repo with the v2 standards prune.
**.NET 10 change**: Validation APIs moved to `Microsoft.Extensions.Validation` package.
**Collision risks**: PineGuard attributes use PineGuard-specific names (e.g., `[PgEmail]` vs `[EmailAddress]`), avoiding namespace conflicts with built-in attributes.

---

## 2. Feature Gap Matrix -- PineGuard vs. The Field

### Validations PineGuard Has That Nobody Else Does

| Domain | PineGuard Exclusive |
|---|---|
| HTTP | Header name/value validation, status code classification, security headers |
| OWASP Security | SQL injection detection, XSS payload detection, CSRF detection |
| CSV/XML/JSON | Format validation as first-class rules |
| Bitwise | AllBits, AnyBits, NoBits, OnlyBits, PowerOfTwo |
| SQL DateTime | SQL Server date range validation |
| Geo Location | Latitude, Longitude, GeoLocation coordinate validation |
| File Paths | Safe filename, file extension validation |
| Temporal ranges | `DateOnlyRange`, `DateTimeRange`, `DateTimeOffsetRange`, `TimeOnlyRange` as first-class validated range types |
| String-parse validators | `StringRules.TimeSpan`, `StringRules.GeoLocation` — validate the text form before parsing |
| Default equality | `DefaultEqualityRules` / `NullOrDefault` — a single check for "null or `default(T)`" across value and reference types |

### Validations Zod Has That PineGuard Should Consider

| Validation | Zod | PineGuard | Priority |
|---|---|---|---|
| JWT format | `.jwt()` | Gap | High |
| UUID versions | `.uuid({version})` | Has GUID, no version | Medium |
| ULID | `.ulid()` | Gap | Medium |
| Nano ID | `.nanoid()` | Gap | Low |
| CUID/CUID2 | `.cuid()`, `.cuid2()` | Gap | Low |
| Emoji | `.emoji()` | Gap | Low |
| Hostname | `.hostname()` | Has (`NetworkRules.IsValidHostname`) | -- |
| MAC address | `.mac()` | Gap | Medium |
| Hash format | `.hash({algorithm})` | Gap | Low |
| ISO Duration | `.iso.duration()` | Gap | Medium |
| String contains | `.includes()` | Gap (has regex) | High |
| String startsWith | `.startsWith()` | Gap (has regex) | High |
| String endsWith | `.endsWith()` | Gap (has regex) | High |
| HTTP URL (scheme-restricted) | `.httpUrl()` | Gap | Medium |

### Validations From Other Libraries Worth Considering

| Source | Validation | PineGuard Gap |
|---|---|---|
| Ardalis | NotFound (returns entity or throws) | Different pattern -- Guard returns input |
| PakValidate | Validation + metadata extraction | Gap -- validate and extract in one call |
| Validot | Built-in translations (5 languages) | Gap -- PineGuard uses English error messages |
| Validot | Template (all possible errors) | Gap -- useful for API documentation |
| CheckValidators | AndIf/OrIf conditional chaining | Gap -- short-circuit conditional validation |
| SimpleValidator | Type conversion (ToInt, ToBool) | Gap -- PineGuard validates, doesn't convert |
| Nager | IBAN validation (international) | Gap -- no payment-instrument validation at all since the v2 standards prune |
| Nager | VAT number validation | Gap |

---

## 3. Architectural Differentiator -- Why PineGuard Is Unique

No library in the .NET ecosystem provides a complete layered validation system:

```
                    +----------------------------------+
                    |     PineGuard.Core (Rules)        |  <-- Deterministic. Pure. Testable.
                    |     300+ static validation         |     Single source of truth.
                    |     methods. Zero allocation.      |
                    +--------+---------+----------------+
                             |         |
              +--------------+         +--------------+
              |              |         |              |
    +---------v------+ +-----v-------+ +v-----------+ +v-----------------+
    | Must Clauses   | | Guard       | | FluentValid | | DataAnnotations  |
    | Must.Be.X()    | | Guard.Against| | .RuleFor() | | [AttributeX]     |
    | Returns result | | Throws      | | Extends FV  | | Extends DA       |
    +----------------+ +-------------+ +-------------+ +------------------+
```

**Every competitor does one layer.** PineGuard does all four from a single rule engine.

| Library | Rules | Must | Guard | Fluent | DA | Tests |
|---|---|---|---|---|---|---|
| Ardalis.GuardClauses | -- | -- | 12 | -- | -- | -- |
| FluentValidation | -- | -- | -- | ~40 | -- | -- |
| Ensure.That | -- | ~15 | -- | -- | -- | -- |
| Validot | -- | -- | -- | ~20 | -- | -- |
| CheckValidators | -- | -- | ~10 | -- | -- | -- |
| ASP.NET Built-in | -- | -- | -- | -- | ~12 | -- |
| **PineGuard** | **300+** | **300+** | **300+** | **300+** | **300+** | **100%** |

---

## 4. Strategic Position -- "The Serilog of Validation"

Serilog succeeded because it provided:

1. A **single core** (structured logging) consumed by **many sinks** (Console, File, Seq, etc.)
2. A **pluggable ecosystem** where community sinks extend reach without fragmenting the core
3. **Zero-friction adoption** -- drop it in, configure a sink, done

PineGuard mirrors this architecture exactly:

1. **Single core** (300+ deterministic Rules) consumed by **four integration layers** (Must, Guard, Fluent, DA)
2. **Pluggable** -- each layer is an independent NuGet package. Use Guard only? Fine. Use Fluent only? Fine. Use all four? They share the same rules.
3. **Zero-friction** -- `Guard.Against.Email(value)` is one line. `[PgEmail]` on a property is one attribute.

The ecosystem gap PineGuard fills:

> **Centralized deterministic rules, consumed everywhere.**
>
> `EmailRules.IsEmail()` -- one rule, four surfaces.
>
> No other library does this.

---

## 5. Recommended Enhancement Priorities

### High Priority (close Zod parity, high API utility)

| Enhancement | Rationale |
|---|---|
| `String.Contains`, `StartsWith`, `EndsWith` | First-class validators (not regex workarounds). Zod has them. Universal need. |
| JWT format validation | Ubiquitous in modern APIs. Zod has `.jwt()`. |
| IBAN validation (international) | Financial APIs. Nager does this. PakValidate does Pakistan-only. |
| Payment card / Luhn validation | Greenfield gap since the v2 standards prune removed the payment-card rules. ASP.NET ships `[CreditCard]`; PineGuard currently ships nothing. |
| HTTP URL (scheme-restricted) | Zod's `.httpUrl()` -- only http/https, not ftp/mailto. |
| ISO 8601 Duration parsing | ISO 8601 duration (`P1Y2M3D`). Zod has `.iso.duration()`. |

### Medium Priority (differentiation, modern identifiers)

| Enhancement | Rationale |
|---|---|
| MAC address validation | Network infrastructure. Zod has `.mac()`. |
| ULID validation | Growing adoption as UUID alternative. |
| UUID version validation | Zod supports `uuid({version: 7})`. |
| Validation + extraction pattern | PakValidate's model: validate IBAN then extract bank name. |
| Error message translations | Validot supports 5 languages. PineGuard's global reach demands this eventually. |

### Low Priority (niche, diminishing returns)

| Enhancement | Rationale |
|---|---|
| Nano ID, CUID/CUID2 | Niche identifier formats. |
| Emoji validation | Edge case. |
| Hash format (md5/sha256) | Rare validation need. |
| Branded types / nominal typing | C# records and strong typing serve this role. |
| Transforms / coercion | PineGuard validates -- transformation is a different concern. |

---

## 6. Fable Intelligence (2026-08-20)

Additions from a strategic review pass. These extend, not replace, the analysis above.

### 6.1 The landscape is shifting under the incumbents

Two platform-level changes reframe the competitive picture:

1. **.NET 10's `Microsoft.Extensions.Validation`** (`AddValidation()`, `[ValidatableType]`, source-generated validation for Minimal APIs) is Microsoft entering the request-validation space directly. This is simultaneously a threat (built-in "good enough" validation) and PineGuard's biggest opportunity: the built-in layer validates *models* but ships almost no *rules*. A PineGuard integration that surfaces 300+ rules through the source-generated pipeline rides Microsoft's adoption wave instead of fighting it. It also solves trimming/AOT, where reflection-based DataAnnotations struggles.
2. **FluentValidation dropped official auto-validation** (async validators cannot run inside sync model binding; DI lifetime issues). The community fills this with SharpGrip and hand-rolled filters. A first-party PineGuard ASP.NET Core integration that does auto-validation *correctly* (async-safe `IAsyncActionFilter` + minimal-API `IEndpointFilter`) captures demand FluentValidation deliberately abandoned.

### 6.2 Gaps the matrix above misses (structural, not rule-level)

The Zod parity table is rule-by-rule. The bigger competitive gaps are structural — these are what FluentValidation actually wins on:

| Capability | Who has it | PineGuard status |
|---|---|---|
| Cross-property validation (`EndDate > StartDate`) | FluentValidation, ExpressiveAnnotations | Gap — Range rules validate a range object, not two model properties |
| Conditional composition (`When`/`Unless`) | FluentValidation, CheckValidators (AndIf/OrIf) | Gap |
| Collection element validation with indexed paths (`Items[2].Name`) | FluentValidation (`RuleForEach`) | Gap |
| Async predicates (DB uniqueness checks) | FluentValidation (`MustAsync`) | Gap — Core can stay sync; Must layer needs the seam |
| Stable machine-readable error codes (separate from messages) | FluentValidation (`ErrorCode`) | Gap — needed by API consumers and frontends |
| Clock injection (`TimeProvider`) for in-past/in-future rules | Nobody does this well | Opportunity — testable temporal validation is a differentiator |

These matter more than JWT or ULID: a team choosing between PineGuard and FluentValidation for request validation will hit "how do I compare two properties?" on day one.

### 6.3 Rule-level gaps to add to Section 2 tables

Non-ISO cases not yet in the matrix: SemVer, cron expression, slug, MIME/content-type, magic-bytes file signature (vs. extension spoofing), Luhn checksum (algorithmic — belongs here, not the ISO repo), decimal precision/scale (money), grapheme-count length (vs. `char` count — the classic emoji-length bug), no-control-characters / no-BOM / valid-surrogate-pairs, Unicode normalization (NFC/NFD), minimum-age from date of birth.

### 6.4 Distribution strategy — the analyzer as growth channel

Serilog's sinks were its distribution mechanism. PineGuard's equivalent is a **Roslyn analyzer + code-fix package**: flag hand-rolled `if (x is null) throw new ArgumentNullException(...)` and offer a one-click fix to `Guard.Against.Null(x)`. Every codebase that installs the analyzer gets a continuous, in-editor advertisement for the library. No competitor ships this.

### 6.5 Revised priority view

1. **Structural gaps first** (cross-property, conditional, collection element, error codes) — these decide head-to-head evaluations against FluentValidation.
2. **Platform integrations second** (`IValidateOptions<T>` startup config validation, .NET 10 Minimal API validation, MediatR `IPipelineBehavior`) — these are where validation actually runs in 2026 apps; see `docs/ai/plans/new-surfaces-missing-vaidation-cases-fable.md`.
3. **Zod string-format parity third** (Contains/StartsWith/EndsWith, JWT, hostname, MAC) — cheap wins, keep them flowing, but they don't win evaluations on their own.

---

## 7. Next Steps

This competitive analysis feeds into:

1. **Feature roadmap** -- prioritized enhancement backlog based on gap analysis
2. **Positioning doc** -- how to communicate PineGuard's value vs. competitors
3. **Brain docs** -- this file (`docs/ai/plans/competitive-analysis.md`) as permanent reference
4. **New surfaces plan** -- `docs/ai/plans/new-surfaces-missing-vaidation-cases-fable.md` (integration packages, middleware, structural gaps)
