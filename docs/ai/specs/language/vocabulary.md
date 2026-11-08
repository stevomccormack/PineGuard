---
spec:
  id: pineguard.ai.language.vocabulary
  title: "PineGuard Vocabulary Map (Opposites + Alternatives)"
  version: 1
  parent:
    - ../spec.md
  dependencies:
    - ../dependencies.md
applies_to:
  - "docs/ai/specs/**"
  - "apps/cli/**"
---

# PineGuard Vocabulary Map (Opposites + Alternatives)

This document defines **shared naming vocabulary** used across:

- MustClauses (`Must.Be.*`) — canonical concept names
- GuardClauses (`Guard.Against.*`) — forbidden-state naming (baseline: `Not*`)
- FluentValidation (`ruleBuilder.*`) — positive rule naming (often concept names)
- Audit tooling — normalization rules for parity / omission checks

This exists to prevent repeated churn around:

- `Not*` as the standard negation prefix
- Opposites (e.g. `Lowercase` ↔ `Uppercase`, `Positive` ↔ `Negative`) where a negated form may be redundant

## 1) Definitions

- **Concept name**: the canonical identifier for a validation idea (typically the MustClause method name *without* a leading `Not`).
  - Examples: `Email`, `Url`, `Latitude`, `HexDigit`.

- **Preferred Guard name**: the preferred *forbidden-state* phrasing for GuardClauses.
  - Baseline: `Not<Concept>` (e.g. `NotEmail`, `NotHexDigit`).

- **Preferred Fluent name**: the preferred FluentValidation extension name.
  - Typically the concept name (e.g. `Email`, `Url`, `Latitude`).

- **Alternatives**: rejected candidates recorded for design rationale. The greenfield surface keeps one canonical spelling per operation; a distinct framework adapter must have its own documented collision rationale.

## 2) Audit normalization (required)

Audit scripts that compare method-name parity (notably `layer-parity`) **MUST** normalize names to concept names using the machine-readable map at:

- `docs/ai/specs/language/vocabulary.json`

Normalization is:

1. Apply explicit `aliases` from the JSON.
2. Strip any leading prefixes in `stripPrefixes` (current default: `Not`, `Has`, `Is`).
3. Apply `aliases` again.
4. Drop any names in `ignoreMethods`.

This produces a **concept-set** that is compared for parity.

## 3) Opposites / omissions

Some concepts have an explicit opposite pair (e.g. `Lowercase` ↔ `Uppercase`).

In these cases, audit scripts may treat `Not<Concept>` as redundant *for Guard/Fluent parity purposes* if the opposite is present, depending on the JSON configuration.

This allows PineGuard to avoid growing a public surface full of mechanically-derived `Not*` methods where a clearer opposite already exists.

## 4) Opposites list

The authoritative list lives in `vocabulary.json`. This section is a human-friendly summary.

| Opposite A | Opposite B | Notes |
|---|---|---|
| Lowercase | Uppercase | Prefer the explicit opposite over `NotLowercase`/`NotUppercase` when it is a true semantic opposite.
| Positive | Negative | Prefer the explicit opposite over `NotPositive`/`NotNegative` when it is a true semantic opposite.
| Even | Odd | Prefer the explicit opposite over `NotEven`/`NotOdd` when it is a true semantic opposite.
| Past | Future | Prefer the explicit opposite over `NotPast`/`NotFuture` when it is a true semantic opposite.
| Before | After | Prefer the explicit opposite over `NotBefore`/`NotAfter` when it is a true semantic opposite.
| OwaspSafe | OwaspUnsafe | Special-case: these are opposites even though Guard may express the forbidden state without a suffix (see below).

## 5) Special cases

### 5.1 OWASP “Safe” suffix

OWASP-style validators intentionally use **positive** "Safe" naming in MustClauses (and FluentValidation), while GuardClauses uses a concise **forbidden-state** name without the suffix.

Examples:

- Must: `Must.Be.XssSafe(...)`
- Fluent: `ruleBuilder.XssSafe()`
- Guard: `Guard.Against.Xss(...)`

The vocabulary JSON encodes this as explicit `aliases` so parity compares a shared concept name:

- `XssSafe` → `Xss`
- `SqlInjectionSafe` → `SqlInjection`
- `PathTraversalSafe` → `PathTraversal`
- `CommandInjectionSafe` → `CommandInjection`
- `CrLfSafe` → `CrLf`
- `LdapFilterSafe` → `LdapFilter`
- `OpenRedirectSafe` → `OpenRedirect`
- `SsrfSchemeSafe` → `SsrfScheme`

### 5.2 OWASP “Safe” vs “Unsafe” root concept

The library also exposes:

- Must: `Must.Be.OwaspSafe(...)`
- Guard: `Guard.Against.OwaspUnsafe(...)`

For parity purposes, both are normalized to the shared concept `Owasp` via `aliases`.

### 5.3 Range and scalar parsing vocabulary

See [API target capabilities](api-capabilities.md) for supported framework boundaries and result contracts.

Equivalent scalar range predicates use `InRange` / `OutOfRange` across Rules, Must, Guard,
FluentValidation and attributes. Keep dimensions in compound predicates: `LengthInRange`,
`HasCountInRange`, `HasGraphemeCountInRange`; their negative clauses retain `Not` and Guards
delegate to the corresponding Must complement. `Within` remains the distinct proximity concept.

Generic scalar predicates whose input is a string use `<Concept><ParsedType>` on the flattened
Must/Guard/Fluent surfaces throughout each family: `PositiveDecimal`, `EvenInt32`, `FiniteDouble`,
`TrueBoolean`, `InRangeInt32`, `InRangeTimeSpan`, `MinimumAgeDateOnly`. Use CLR type spellings
consistently. Typed-input clauses keep the short concept. Nested Core classes retain their context;
do not repeat a type already supplied by the class. A multi-type Core class retains the type
qualifier, as in `StringRules.NumberTypes.IsInRangeInt32`.

Domain concepts already name their meaning: `Guid`, `HasGuidVersion`, `NotEmptyGuid`, `Ipv4`,
`Ipv6`, `IpAddress`, `PhoneNumber`, `Email`, `Hostname` and `ValidXml` do not acquire redundant
output-type suffixes. Attribute `String` qualifiers distinguish input representation where needed
(for example `PastDateOnly` versus `PastDateOnlyString`); they do not mean raw-result projection.

Must.Result holds the successful parsed/normalized output and Must.Value the original input.
Guard returns that successful result. Do not add raw-result twins such as `Ipv4String` or
`PhoneNumberString`; a caller can retain its original input. Fluent validation does not replace
the property's value with the parsed result.

Guard applies the existing semantic opposite before the parsed-type suffix:
`ZeroOrNegativeDecimal` calls `PositiveDecimal`, `FalseBoolean` calls `TrueBoolean`, and
`OutOfRangeInt32` calls `InRangeInt32`. Temporal equality boundaries remain explicit; `Past`
and `Future` are not strict complements at the reference instant.

Single-bound comparisons use `other`; two-bound comparisons use `min` / `max`. Required bounds
are configuration, not optional validated values. Explicit nullable peer/equality contracts such
as `IsSame` are separate and must document their null semantics.

### 5.4 XML validity and object validation

XML deliberately distinguishes well-formedness (`XmlRules.IsXml`, `Must.Be.Xml`,
`Guard.Against.NotXml`, `XmlStringAttribute`) from XSD validity
(`XmlSchemaRules.IsValidXml`, `Must.Be.ValidXml`, `Guard.Against.InvalidXml`, `ValidXmlAttribute`).
The word Valid is meaningful here; see the [XML specification](../xml/project.md).
`XmlSchemaRules` and `XmlSchemaMustValidator` also avoid Core/BCL type-name collisions.

The exact `InvalidXml` domain exception and Guard's `Invalid` object-validator entry point do
not authorize a general Invalid-prefix convention. The XML package intentionally has no direct
FluentValidation dependency; use the existing MustBe composition adapter.

### 5.5 Configuration predicates

Parsing-utility `IsValidStyles` helpers validate permitted flag combinations rather than asking
whether input is of a named domain. Multi-parser containers qualify the style family where needed,
as in `StringUtility.NumberTypes.IsValidFloatingPointStyles`. This is a configuration contract;
it does not justify redundant input predicates such as `IsValidHostname`.

## 6) Updating this map

When a new naming question arises:

1. Add/adjust entries in `vocabulary.json`.
2. Update this MD file if a human-readable explanation is needed.
3. Update any audit scripts that consume the JSON (`layer-parity`, etc.).

This keeps the spec and tooling aligned.
