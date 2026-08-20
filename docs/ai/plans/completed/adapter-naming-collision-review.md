<!-- metadata_header
type: plan
id: adapter-naming-collision-review
version: 1.0
status: completed
-->

# Adapter Naming Collision Review

> [!WARNING]
> **Archived — the recommended dialect below was NOT adopted.** This review proposed a
> `String`-suffix dialect (`EmailStringAttribute`, `UrlString(...)`). What shipped instead was a
> targeted collision-safe prefix applied only where a name actually collides: `WebUrlAttribute`,
> `WebUrl(...)`, with `Url(...)` retained as a thin forwarder. `EmailAttribute` and `Email(...)`
> were never renamed, and no `*String` names exist in `src/`.
>
> The normative rule is `docs/ai/specs/language/naming-collisions.md`. Read the sections below as
> the reasoning that led there, never as a rename backlog.

## Context

PineGuard’s adapter layers must be easy to discover, hard to misuse, and visually distinct from the frameworks they integrate with.

That matters most in:

- `PineGuard.DataAnnotations`, where public attribute names sit beside .NET’s built-in DataAnnotations types, and
- `PineGuard.FluentValidation`, where extension methods appear directly in IntelliSense next to FluentValidation’s built-in validators.

This project is greenfield. That means long-term clarity matters more than compatibility. If a public adapter name collides with, mimics, or obscures a framework-native validator, the preferred fix is a direct rename.

The existing specs already point in this direction:

- DataAnnotations string validators should carry `String` where appropriate.
- DataAnnotations collisions should be resolved by suffixing the type/domain for the entire family.
- FluentValidation method names should match Must where practical, but that should not override clarity when framework-native names already own a concept.

---

## Goals

1. Inventory all public DataAnnotations and FluentValidation adapter names.
2. Identify exact collisions, semantic near-collisions, and discoverability ambiguities.
3. Recommend a single naming strategy that prioritizes clarity and consistency across both adapter layers.
4. Produce a rename-first execution plan for greenfield cleanup.
5. Preserve PineGuard’s layered architecture while reducing confusion for end users.

---

## Non-Goals

- Do not implement the renames in this planning slice.
- Do not preserve ambiguous names for compatibility.
- Do not change Must or Guard naming as part of this review unless an adapter rename forces a parity decision later.
- Do not solve unrelated API style issues in the same pass.

---

## Scope

### In scope

- `src/PineGuard.DataAnnotations/**`
- `src/PineGuard.FluentValidation/**`
- adapter-facing docs/examples in `README.md`
- integration-specific specs under `docs/ai/specs/data-annotations/**` and `docs/ai/specs/fluent-validation/**`

### Out of scope

- `PineGuard.Core`
- `PineGuard.MustClauses`
- `PineGuard.GuardClauses`
- adapter implementation logic beyond naming

---

## Collision Taxonomy

Each public adapter symbol must be classified into one of these buckets.

### 1. Exact collision

The public PineGuard name matches a framework-native public validator name after framework normalization.

Examples:

- DataAnnotations compares attribute names after dropping the `Attribute` suffix.
- FluentValidation compares extension method names exactly.

These are the highest-risk items and should be renamed first.

### 2. Semantic near-collision

The PineGuard name is not textually identical, but expresses the same concept strongly enough that users will assume framework-native behavior.

Examples:

- `EmailAttribute` vs .NET `EmailAddressAttribute`
- `Email(...)` vs FluentValidation `EmailAddress()`
- `PhoneNumberAttribute` vs .NET `PhoneAttribute`

These should usually be renamed if the PineGuard behavior is broader, stricter, or simply different.

### 3. Discoverability ambiguity

The name does not clearly reveal:

- the validated type,
- the PineGuard-specific semantics, or
- whether the validator is framework-native vs PineGuard-native.

Examples:

- generic names like `Json`, `Url`, `FilePath`, `Null`, `Empty`
- names that are accurate but visually blend into framework-native IntelliSense

These are lower severity than exact collisions, but still important in a library that values polish and explicitness.

---

## Review Methodology

## Phase 1 — Build the public API inventory

### DataAnnotations

Inventory every public `ValidationAttribute` in:

- `src/PineGuard.DataAnnotations/*.cs`

Capture:

- class name
- effective attribute usage name (class name minus `Attribute`)
- validated type
- underlying Must clause
- likely framework comparison target

### FluentValidation

Inventory every public extension method in:

- `src/PineGuard.FluentValidation/**/*.cs`

Capture:

- method name
- property type
- underlying Must clause
- likely FluentValidation comparison target

---

## Phase 2 — Compare against framework-native validators

### .NET DataAnnotations comparison set

Compare PineGuard attributes against common built-ins in `System.ComponentModel.DataAnnotations`, especially:

- `RequiredAttribute`
- `RangeAttribute`
- `CompareAttribute`
- `StringLengthAttribute`
- `MinLengthAttribute`
- `MaxLengthAttribute`
- `PhoneAttribute`
- `UrlAttribute`
- `EmailAddressAttribute`
- `FileExtensionsAttribute`
- `CreditCardAttribute`
- `Base64StringAttribute`
- `AllowedValuesAttribute`
- `DeniedValuesAttribute`

### FluentValidation comparison set

Compare PineGuard extension names against common FluentValidation built-ins, especially:

- `EmailAddress`
- `Empty`
- `NotEmpty`
- `Null`
- `NotNull`
- `Matches`
- `Length`
- `MinimumLength`
- `MaximumLength`
- `Equal`
- `NotEqual`
- `InclusiveBetween`
- `ExclusiveBetween`
- `CreditCard`
- `IsInEnum`

---

## Phase 3 — Score each symbol

For each public symbol, assign:

- collision type: exact / semantic / discoverability
- severity: high / medium / low
- proposed action: rename / keep / review manually

### Severity guidance

#### High

- exact collision
- strong semantic overlap with different behavior
- likely to create import or IntelliSense confusion immediately

#### Medium

- no exact collision, but likely to be misread as framework-native
- name hides validated type or PineGuard-specific semantics

#### Low

- name is explicit enough already
- overlap risk is minor and acceptable

---

## Current Likely Hotspots

These are the first symbols to inspect because they are already likely to collide or confuse based on the current code.

## DataAnnotations

### High-priority review

- `EmailAttribute`
- `PhoneNumberAttribute`
- `CustomPhoneNumberAttribute`
- `FilePathAttribute`
- `AbsoluteUriAttribute`
- `RelativeUriAttribute`
- `HttpUrlAttribute`
- `HttpsUrlAttribute`
- `FileUriAttribute`
- `JsonAttribute` (if present)
- `XmlAttribute` (if present)

### Medium-priority review

- `StrictEmailAttribute`
- `HasEmailAliasAttribute`
- `NotHasEmailAliasAttribute`
- `SlugAttribute` (if present)
- `Ipv4Attribute` / `Ipv6Attribute` (if present)
- generic temporal names that may need domain/type suffixing

### Immediate observations

- `EmailAttribute` already looks out of alignment with the project spec’s `String`-suffix guidance.
- `PhoneNumberAttribute` is explicit, but still semantically adjacent to .NET `PhoneAttribute`.
- `UrlStringAttribute` looks safer than `UrlAttribute`, which is a good precedent.
- the spec already contains the correct design instinct: when a domain collides, suffix the type/domain consistently across the family.

## FluentValidation

### High-priority review

- `Email(...)`
- `Url(...)`
- `FilePath(...)`
- `Null(...)` / `NotNull(...)` (if present)
- `Empty(...)` / `NotEmpty(...)` (if present)
- `Match(...)` / `Matches(...)` (if present)
- `Equal(...)` / `NotEqual(...)` (if present)

### Medium-priority review

- `PhoneNumber(...)`
- `AbsoluteUri(...)`
- `RelativeUri(...)`
- `Json(...)`
- `Xml(...)`
- short names that are correct but visually indistinguishable from FluentValidation’s own language

### Immediate observations

- `Email(...)` is the strongest current hotspot because users will naturally compare it to FluentValidation’s built-in `EmailAddress()`.
- `Url(...)` and `FilePath(...)` read naturally, but they also blend into framework-native validator expectations.
- fluent adapters that use short, generic names should be reviewed against the project’s goal of explicit, premium API language.

---

## Recommended Resolution Strategy

## Recommendation

Choose one explicit naming dialect and apply it consistently across adapter layers.

### Recommended dialect (not adopted — see the banner at the top of this file)

#### DataAnnotations

Prefer type-revealing names for string-backed adapters:

- `EmailAttribute` → `EmailStringAttribute`
- `PhoneNumberAttribute` → `PhoneNumberStringAttribute`
- `FilePathAttribute` → `FilePathStringAttribute`
- `AbsoluteUriAttribute` → `AbsoluteUriStringAttribute`
- `RelativeUriAttribute` → `RelativeUriStringAttribute`

If a domain has one collision-prone member, strongly consider renaming the **entire related family** for consistency.

#### FluentValidation

Avoid using bare framework-like names when they overlap with FluentValidation concepts or common developer expectations.

Likely direction:

- `Email(...)` → `EmailString(...)`
- `Url(...)` → `UrlString(...)`
- `FilePath(...)` → `FilePathString(...)`

If a method is already clearly PineGuard-specific and not framework-native, it may stay short.

## Why this is the preferred strategy

- it is explicit
- it aligns with the DataAnnotations spec already in the repo
- it scales better than one-off exceptions
- it gives PineGuard a more intentional adapter language
- it reduces IntelliSense confusion in both adapter ecosystems

---

## Decision Framework

When a collision is found, prefer this resolution order:

1. **Rename for explicitness**
2. **Rename the family, not just the one outlier, when consistency benefits**
3. **Keep only if the name is already explicit and collision risk is low**
4. **Do not rely on documentation to compensate for an ambiguous public name**

Greenfield rule:

- if a name feels ambiguous enough to require explanation, rename it

---

## Execution Plan

## Phase 0 — Audit and recommendation lock

Use `Plan` to:

- complete the inventory
- assign collision classes and severity
- finalize the naming dialect decision

Deliverable:

- approved collision matrix and rename strategy

## Phase 1 — DataAnnotations rename proposal

Use `validation-builder` to draft the exact rename list for:

- attributes
- file names
- tests
- docs/examples

Deliverable:

- file-by-file DataAnnotations rename map

## Phase 2 — FluentValidation rename proposal

Use `validation-builder` to draft the exact rename list for:

- extension methods
- file names if needed
- tests
- docs/examples

Deliverable:

- file-by-file FluentValidation rename map

## Phase 3 — Review

Use `code-reviewer` to confirm:

- naming clarity improved
- Must/Guard vocabulary integrity was not accidentally weakened
- adapter naming remains coherent with the Brain

Deliverable:

- approved rename execution slice

## Phase 4 — Coverage/testing follow-up

Use `test-writer` and `coverage-analyst` during implementation, not in this planning slice.

Deliverable:

- targeted rename-safe tests and coverage confirmation during the actual rename task

---

## Deliverables for the Follow-Up Implementation Task

The implementation task that follows this plan should produce:

- renamed DataAnnotations attributes
- renamed FluentValidation extension methods
- updated tests
- updated specs/examples
- updated README snippets if public API examples changed

---

## Success Criteria

- Every public adapter symbol is classified.
- High-risk collisions have a rename recommendation.
- A single naming dialect is chosen and documented.
- The recommendation favors clarity over compatibility.
- The follow-up implementation slice can proceed without redoing discovery.

---

## Recommended Rollout Decision

**Proceed with a rename-first adapter audit.**

The best long-term outcome is not to defend ambiguous names one by one. It is to define a clear adapter naming dialect, apply it consistently, and let PineGuard’s adapter APIs look unmistakably intentional.
