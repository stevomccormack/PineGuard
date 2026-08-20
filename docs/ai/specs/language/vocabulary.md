---
spec:
  id: pineguard.ai.language.vocabulary
  title: "PineGuard Vocabulary Map (Opposites + Alternatives)"
  version: 1
  template:
    - ../../meta/template-project.md
  parent:
    - ../spec.md
  dependencies:
    - ../dependencies.md
applies_to:
  - "docs/ai/specs/**"
  - "tools/audit-cli/**"
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

- **Alternatives**: accepted aliases/synonyms that are allowed to exist but are not the preferred choice.

## 2) Audit normalization (required)

Audit scripts that compare method-name parity (notably Rule06) **MUST** normalize names to concept names using the machine-readable map at:

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

## 6) Updating this map

When a new naming question arises:

1. Add/adjust entries in `vocabulary.json`.
2. Update this MD file if a human-readable explanation is needed.
3. Update any audit scripts that consume the JSON (Rule06, etc.).

This keeps the spec and tooling aligned.
