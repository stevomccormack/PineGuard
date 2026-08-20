<!-- metadata_header
type: plan
id: future-language
version: 1.0
status: non-binding
last_updated: 2026-08-20
-->

# PineGuard — Future Language Considerations (Must/Guard)

This document captures **non-binding** ideas for future naming/vocabulary improvements in PineGuard.

Status:

- Non-binding (does not override any generator specs). An idea backlog, not scheduled work.

It exists because we have two competing goals:

- **Deterministic, consistent generation rules today** (current generator specs are strict).
- **Better, more natural public language tomorrow** (especially around opposites / complements).

The goal is to capture ideas we might want to fast-track later, without weakening the current specs.

Nothing in this document overrides the current source-of-truth rules in:

- `docs/ai/specs/must-clauses/project.md`
- `docs/ai/specs/guard-clauses/project.md`

---

## 0) “Pointer helpers” / aliases for improved language (discoverability)

### The opportunity

When callers want “the opposite” of something, it can be hard to discover the correct API if the opposite is expressed as a more semantic bad-state name (e.g., `MissingKey`) instead of the mechanical complement (`NotHasKey`).

A future solution is to provide an “improved language” set that is simply a collection of **aliases** (or thin forwarding methods) that point at the canonical `Not*` complements.

Examples (conceptual):

- `MissingKey(...)` forwards to `NotHasKey(...)`.
- `NoMatchingKey(...)` forwards to `NotHasAnyKey(...)`.
- `MissingScheme(...)` forwards to `NotHasScheme(...)`.

This preserves:

- **One canonical implementation** (the existing `Not*` method remains the single place with logic + message).
- **Easy discoverability** (semantic names exist without requiring users to remember the exact `Not*` form).

If adopted, this should be a curated, deterministic mapping (not ad-hoc) and should follow the greenfield approach: rename/add directly and update all usages.

## 1) “Missing* / NoMatching*” vocabulary (absence / predicate)

### The opportunity

Some complements read more naturally as **bad-states** than as `NotHas*`.

Examples we encountered:

- Dictionaries:
  - Current (Must-derived): `NotHasKey`, `NotHasAnyKey`, `NotHasValue`, `NotHasAnyValue`, `NotHasAnyItem`
  - Potential future “bad-state” language:
    - `MissingKey`, `NoMatchingKey`, `MissingValue`, `NoMatchingValue`, `NoMatchingItem`

- Strings / emails:
  - Current: `NotHasEmailAlias`
  - Potential future: `MissingEmailAlias`

- URIs:
  - Current: `NotHasScheme(value, scheme)`
  - Potential future: `MissingScheme(value, scheme)`

- Bitwise:
  - Current: `NotHasAllBits`, `NotHasAnyBits`
  - Potential future: `MissingBits`, `NoMatchingBits`

### Constraints to respect

If we adopt this vocabulary, it should be **curated and consistent**, not ad-hoc.

Potential approaches:

- **Approach A (preferred): Must defines the vocabulary**
  - Add `Missing*` / `NoMatching*` Must clauses that are semantic aliases of the existing `NotHas*` complements.
  - Then Guard can map cleanly without inventing new synonyms.

- **Approach B: Guard has an allowed “bad-state synonym list”**
  - Guard may use a curated list of bad-state names that map to Must complements.
  - Requires an explicit allowlist and strong rules to avoid drift.

---

## 2) “Non*” vs “Not*” (complements)

Current policy (today):

- Standardize on `Not*` for negation/complements.
- Do not introduce `Non*` variants (even for character classes) while the vocabulary is being stabilized.

### The opportunity

`Non*` often reads more naturally than `Not*` for **classification complements**, especially:

- character classes: `Digit/NonDigit`, `Ascii/NonAscii`, `PrintableAscii/NonPrintableAscii`
- “type-like” or “format-like” sets where `NotX` could be misread (e.g., `NotAscii` can read like “ASCII is forbidden”).

### Possible future refinements

- Expand the “Non\*” rule beyond characters **only where it is unambiguous** and already culturally established.
- Prefer `Non*` for complements that are used primarily as **bad-states** (Guard-facing language), and keep Must positive when possible.

---

## 3) Richer “bad-state” names for complements

### The opportunity

Some complement names could become clearer if expressed as a domain-specific bad-state rather than `NotX`.

Examples from the thread:

- Bitwise:
  - Current: `NotBitwiseEqualTo`, `NotHasNoBits`, `NotHasOnlyBits`
  - Potential future:
    - `BitwiseMismatch`
    - `ContainsForbiddenBits` (complement of `HasNoBits`)
    - `ContainsDisallowedBits` (complement of `HasOnlyBits`)

### Constraint to respect

If adopted, these names must remain **deterministic** and should ideally exist in Must as semantic aliases, or be governed by an allowlist.

---

## 4) “Invalid\*” as a consistent bad-state layer

Current policy (today):

- Do not introduce `Invalid*` names; prefer `Not*` complements derived from Must.

### The opportunity

For parse/format validations we can consider a stable, readable pattern in the future, but today we standardize on `Not*`.

Future standardization ideas:

- If we ever reintroduce an `Invalid*` layer, it must be curated, explicitly documented, and reflected in the shared vocabulary map.

---

## 5) Symmetry and coverage expectations

### The opportunity

Where Must exposes both sides (`X` and `NotX`), we can provide both Guard entry points:

- `Guard.Against.NotX` (implemented via `Must.Be.X`)
- `Guard.Against.X` (implemented via `Must.Be.NotX`)

Future refinement:

- Add “symmetry” systematically for clause families where callers commonly want both directions.

---

## 6) Migration strategy if/when we adopt language changes

Because this is a greenfield library, a future naming upgrade can be done as a **direct rename** (no `[Obsolete]` shims) as long as we:

- update all internal usages/tests/docs
- keep the mapping deterministic
- keep canonical messages owned by Must

Recommended process for a naming upgrade:

1. Decide the new vocabulary (and whether it lives in Must, Guard, or both).
2. Add/rename Must clauses first (canonical vocabulary).
3. Update Guard mappings and specs.
4. Run full build + tests.
