---
spec:
  id: pineguard.ai.core.project-spec
  title: "PineGuard.Core Project Spec (Rules & Utils)"
  version: 2
  template:
    - ../../meta/template-project.md
  parent:
    - ../project.md
  dependencies:
    - ../dependencies.md
applies_to:
  - "src/PineGuard.Core/Rules/**"
  - "src/PineGuard.Core/Utils/**"
---

# PineGuard.Core Project Spec (Rules & Utils)

This document is the **source-of-truth instruction set** for generating and maintaining **production `Rules` and `Utils` code** in this repository.

**Inheritance**: Inherits from `docs/ai/specs/project.md`.

Coding standards are inherited from `docs/ai/specs/coding-standard.md`.

## Feature Implementation Checklist

See `docs/ai/specs/spec.md` §3 ("Feature Implementation Checklist (Master)").

## Related specs

- Unit tests addendum: `docs/ai/specs/core/unit-test.md`
- Coverage addendum: `docs/ai/specs/core/coverage.md`
- Naming collisions: `docs/ai/specs/language/naming-collisions.md`

---

## Purpose

Define the canonical rules for creating and maintaining Core `Rules` and `Utils`.

## Scope

This spec applies to:

- `src/PineGuard.Core/Rules/**`
- `src/PineGuard.Core/Utils/**`

---

## 1. What `Rules` are (in PineGuard)

**Rules** are pure, reusable boolean predicates (and small helpers) used by:

- MustClauses (`Must.Be.*`) to decide success/failure and to possibly parse/normalize values, and
- any other code that needs a reusable validation primitive.

Rules:

- do **not** throw on invalid input,
- do **not** allocate unnecessarily,
- do **not** own user-facing messages,
- are deterministic,
- are primarily written as `IsX(...)` / `HasX(...)`.

Nuance:

- “Invalid input” above refers to the **validated value**.
- Rules/Utils may still enforce non-null **configuration/dependency parameters** (regexes, predicates, providers, option arrays).

Canonical definition: see `docs/ai/specs/spec.md` (“Validated value vs configuration/dependency parameters”).

### 1.1 Relationship to MustClauses and GuardClauses

Single source of truth (strict). PineGuard layers in one direction — each layer calls only the one before it:

- **Core** (`Rules`/`Utils`) owns validation logic and parsing.
- **MustClauses** call Core and own the canonical, user-facing messages (`MustResult<T>`).
- **GuardClauses** call MustClauses and throw using `MustResult.Message`.
- **FluentValidation** adapts MustClauses into `IRuleBuilder` extensions.
- **DataAnnotations** adapts MustClauses into `ValidationAttribute`s.

Guard, Fluent and DataAnnotations are sibling adapters over Must — none calls another, and none reimplements Core logic.

Do not duplicate parsing/validation logic across layers.

---

## 2. Where code lives (strict)

### 2.1 Rules

- Project: `src/PineGuard.Core`
- Folder: `src/PineGuard.Core/Rules/`
- Namespace: `PineGuard.Rules`

### 2.2 Utils

- Project: `src/PineGuard.Core`
- Folder: `src/PineGuard.Core/Utils/`
- Namespace: `PineGuard.Utils`

Folder conventions (strict):

- Everything lives directly under `Rules/` and `Utils/` unless a domain folder is defined.
- OWASP public facades stay at the folder root — `Rules/OwaspRules.cs` (`namespace PineGuard.Rules`) and `Utils/OwaspUtility.cs` (`namespace PineGuard.Utils`). Supporting OWASP pattern/regex types live in the `Rules/Owasp/` domain folder under `namespace PineGuard.Rules.Owasp` (today: `Rules/Owasp/OwaspRegex.cs`, which is public — the domain folder denotes a sub-namespace, not reduced visibility). There is no `Utils/Owasp/` folder; create one only when a supporting OWASP utility type actually needs it. See §3.1.1 for the facade-over-implementation idea, noting Owasp differs in that its supporting types are public.

---

## 3. Naming conventions (strict)

### 3.1 Public type naming

- One file per domain (simple case): `XxxRules.cs` containing `public static class XxxRules`.
- One file per utility: `XxxUtility.cs` containing `public static class XxxUtility`.
- File name must match the public class name, optionally suffixed with `.<SubDomain>` for partial splits (e.g. `StringRules.Guid.cs`).

Examples:

- `GuidRules` in `Rules/GuidRules.cs`
- `GuidUtility` in `Utils/GuidUtility.cs`

### 3.1.1 Domain facades (public partial class pattern)

For some domains, PineGuard keeps concrete implementations **internal** (under a domain folder), while exposing a stable public surface via a single **public facade** class in `namespace PineGuard.Rules`.

Additional note: `StringRules` is also a public facade

- `src/PineGuard.Core/Rules/StringRules*.cs` defines `public static partial class StringRules` with nested public types.
- This surface is intended to be used by external consumers (not just Core-internal call sites).
- Do not make `StringRules` nested types `internal` to “reduce noise” in mapping audits; if a member is public in `PineGuard.Rules`, treat it as part of the supported public surface.

### 3.1.2 Partial-file split

When a domain class grows beyond one file, declare it `public static partial class XxxRules` and split it across `XxxRules.cs` (shared/entry members) plus one `XxxRules.<SubDomain>.cs` per sub-domain. The same applies to utilities: `XxxUtility.cs` plus `XxxUtility.<SubDomain>.cs`.

Today: `Rules/StringRules.cs` + `StringRules.Bool.cs`, `StringRules.Casing.cs`, `StringRules.Guid.cs`, … and `Utils/StringUtility.cs` + its `StringUtility.<SubDomain>.cs` partials.

Test fixtures MUST mirror the source split one-for-one — see `docs/ai/specs/testing/fixture.md`.

### 3.2 Method naming

Rules should overwhelmingly be **positive**:

- `IsX(...)`
- `HasX(...)`

Avoid `NotX(...)` in Rules.

Permitted exceptions (common primitives already used in the repository):

- `IsEmpty(...)`, `HasItems(...)`, `IsZero(...)`, `IsNaN(...)`, etc.

Utilities use multiple naming conventions depending on the operation:

- `TryParseX(...)`, `TryCreateX(...)`, `TryGetX(...)` — fallible operations
- `IsX(...)`, `HasX(...)`, `ContainsX(...)` — predicate checks
- `ToX(...)`, `GetX(...)` — transformations/lookups
- `Mask(...)`, `Sanitize(...)`, `Format(...)`, `Diff(...)` — domain operations

### 3.3 Nullability

Rules must be explicit and consistent:

- **Value types (structs)**: Prefer `T` (non-nullable) signatures. Do not overload `T?` unless null is explicitly meaningful and the method name encodes that intent.
- **Reference types**: Accept `string?` / `T?` and return `false` for null (standard predicate behavior) unless the method name explicitly represents null handling (e.g., `IsNullOrWhiteSpace`, `IsNullOrX`).
- If a rule accepts nullable inputs (`string?`, `T?`), then **null must return `false`** unless the method name explicitly represents null handling.
- If a Rule is generic over `T : struct`, expose `IsX(T value)` and rely on callers to handle nulls/unwrapping.

> [!CAUTION]
> **Why `T` (non-nullable) is required for struct validated values — do NOT change to `T?`.**
>
> MustClauses and GuardClauses expose **parallel typed and string overloads** with the **same method name** on the **same extension receiver** in the **same namespace**:
>
> - `MustNumberClauses.Positive(this IMustClause _, T value, ...)` (struct)
> - `MustStringNumbersClauses.Positive(this IMustClause _, string? value, ...)` (string parse+validate)
>
> C# overload resolution disambiguates these by the first positional argument: `T value` (non-nullable struct) vs `string? value` (nullable reference).
>
> If the struct overload were `T? value`, the compiler would produce **CS0121 ambiguous method call** errors when callers pass nullable values, because both `T?` and `string?` accept `null`. This pattern applies identically to GuardClauses (`IGuardClause` receiver).
>
> The same applies to every domain that has both typed and string overloads: Numbers, DateTime, DateOnly, TimeOnly, DateTimeOffset, Guid, Bool, GeoLocation, TimeSpan, etc.
>
> **Rules are the exception**: Rules are standalone static methods on separate classes (`NumberRules`, `StringRules`), not competing extension methods. Rules may use `T?` to absorb null internally, but the Must/Guard public surface must use `T` for struct validated values.
>
> See also: `docs/ai/specs/must-clauses/project.md §Nullability` (hybrid strategy).

#### 3.3.1 Permitted negation methods in Rules

The spec §3.2 says "Avoid `NotX(...)` in Rules." The following are **explicit permitted exceptions** in the Rules layer because they are direct, zero-ambiguity boolean complements of fundamental primitives:

| Method | Location | Rationale |
|---|---|---|
| `IsNotNull<T>` | `NullRules` | Complement of `IsNull`; fundamental primitive used by many callers |
| `IsNotEmpty<T>` | `CollectionRules` | Complement of `IsEmpty`; common collection predicate |
| `IsNotEmpty<TKey,TValue>` | `DictionaryRules` | Complement of `IsEmpty`; dictionary-specific overload |
| `IsNotEmpty<TKey,TValue>` | `ReadOnlyDictionaryRules` | Complement of `IsEmpty`; read-only dictionary overload |
| `IsNotEmpty` | `GuidRules` | Complement of `IsEmpty`; direct Guid non-empty check |
| `IsNotEmpty` | `StringRules.Guid` | Complement of `IsEmpty`; string-based Guid non-empty check |
| `IsNotZero<T>` | `NumberRules` | Complement of `IsZero`; common numeric guard |
| `IsNotZero` | `StringRules.Numbers` | Complement of `IsZero`; string-based numeric non-zero check |
| `NotContainsControlChars` | `StringRules` | Complement of `ContainsControlChars`; common input sanitization |

All other negation methods in Rules require an explicit spec exemption entry in this table.

---

## 4. `Utils` responsibilities (strict)

Utilities exist to centralize:

- parsing,
- normalization,
- low-level helpers shared by multiple Rules,
- operations that would otherwise require repeated `try/catch`.

Rules should call `Utils` rather than duplicating parsing/normalization logic.

### 4.1 Utils and the parsed-result contract

Utils return parsed/normalized values via `out` parameters. This is their **primary reason for existing as a separate layer from Rules**.

- **Rules** answer a boolean question: `IsXxx(value) → bool`.
- **Utils** answer a parsing question: `TryXxx(value, out T parsed) → bool` + typed result.

MustClauses and GuardClauses need the parsed/normalized result for `MustResult<T>.Result`. When a parsed result is required, **prefer calling `Utility.TryXxx()` directly** rather than `Rules.IsXxx()`, because the Try method gives you both the boolean and the parsed value in a single call.

Rules remain valuable as the **semantic API** for callers that only need a yes/no answer. Rules internally call Utils when they need parsing — no logic is duplicated.

Summary:

- **Need just a boolean?** → Call `Rules.IsXxx()`.
- **Need the parsed/normalized value back?** → Call `Utility.TryXxx()`.
- **Implementing a MustClause?** → Prefer `Utility.TryXxx()` so you can pass the parsed result to `MustResult<T>.FromBool(..., result: parsed)`.

### 4.2 Allocation and lifetime guidance

- Prefer `TryParse` over `Parse`.
- Prefer no-allocation patterns (`ReadOnlySpan<char>` where it meaningfully helps and matches existing style).
- Do not return disposable objects from `Try*` unless ownership is explicit.
  - Example: for JSON, prefer returning `JsonValueKind` rather than returning a `JsonDocument` that must be disposed.

---

## 5. Correctness and standards guidance

Rules should be **correct and pragmatic**:

- If full standards compliance is complex (email, HTTP header ABNF, CSP), implement the pragmatic subset and name it accordingly.
- Prefer using the .NET BCL where it already implements a standard correctly (e.g., `Uri.TryCreate`, `IPAddress.TryParse`, `Guid.TryParse`, `System.Text.Json`).

Do not create a generic “RFC library” up front. Implement focused rules, and only extract shared primitives if duplication emerges.

---

## 6. Implementation patterns (strict)

### 6.1 Use shared comparison helpers

When comparing `IComparable<T>` values and supporting inclusive/exclusive behavior, prefer `RuleComparison` rather than duplicating logic.

### 6.2 Date/time normalization

When comparing `DateTime`, normalize with `DateTimeUtility.ToUtc` first.

### 6.3 Dictionaries and case-insensitivity

If adding Rules over header dictionaries:

- header names are case-insensitive
- do not require the input dictionary to have a specific comparer
- handle case-insensitive matching in a `Utils` helper
- prefer supporting multi-value headers via `IReadOnlyDictionary<string, IEnumerable<string>>` and optionally provide overloads for `IReadOnlyDictionary<string, string>`

---

## Appendix — Intended new rule areas (non-binding)

This spec supports generating Rules/Utils for common validation inputs such as:

- GUID parsing and version/variant checks (`GuidRules`, `GuidUtility`)
- Email parsing and domain allowlisting (`EmailRules`, `EmailUtility`)
- URI parsing and host/scheme allowlisting (`UriRules`, `UriUtility`)
- IP/CIDR membership (`NetworkRules` + `NetworkUtility`)
- JSON validity and root-kind checks (`JsonRules` + `JsonUtility`)
- safe file names (`FilePathRules` + `FilePathUtility`)
- password validation (`PasswordRules` + `PasswordUtility`)
- HTTP header syntax and header-dictionary helpers (`HttpRules` + `HttpUtility`)
