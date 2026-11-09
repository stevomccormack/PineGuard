<!-- metadata_header
type: plan
id: core-common-api-decisions
version: 2.2
status: implemented
last_updated: 2026-11-09
-->

# Core / Common API Decisions

> **Status**: Implemented following owner approval. Exact declaration reconciliation, decision dispositions and verification evidence are recorded in [Core / Common API implementation](core-common-api-implementation.md). The design rationale and acceptance criteria below are retained as the decision record.
> **Origin**: The core/common adversarial review (merged as `206bbc4`, 2026-08-23), re-read on 2026-11-08 against the flattened `Must.Be` / `Guard.Against` surfaces.
> **Scope**: Core, MustClauses, GuardClauses, all affected FluentValidation and DataAnnotations adapters, PineGuard.Xml, tests/fixtures, canonical specifications, documentation, and audit vocabulary/baselines.

> [!IMPORTANT]
> **No backward compatibility. (Owner, 2026-11-08.)**
> *"Backward compatibility is not a thing. We are designing fresh always (for now)."*
>
> Nothing in this document is weighed against source compatibility. There are no `[Obsolete]`
> forwarders, no alias overloads kept "for ergonomics", no batching of renames for a major
> version, and no migration notes. When a name or shape is agreed, rename it, delete the old
> member, and update every call site, test and doc in the same change. The only questions this
> document asks are *which name is right* and *which behaviour is right*. Revisit at first release.

## Why this document exists

The core/common review raised 174 findings across 116 files. 170 survived independent
adversarial verification, and the 134 with an unambiguous correct answer were fixed and merged in
`206bbc4`.

Version 1 of this document recorded the 36 remaining items as "decisions", mostly because fixing
them would break source compatibility. That reason no longer exists. Version 2 re-sorts the items
by the only test that matters now: **does this have one defensible answer, or more than one?**
Items with one answer are a fix list. Items with more than one are decisions, and every naming
decision now carries its rejected alternatives, because a public name is a product decision.

## Policy provenance and design disposition

The owner's initial instruction was **finish the corrected plan first**. After that review,
the owner explicitly authorized full implementation, 100% line and branch coverage, and commit
and push without a PR. Existing policies are not reopened merely because version 2 overlooked them.

| Policy read | Binding direction used in this review |
|---|---|
| [Global specification](../specs/spec.md), [project architecture](../specs/project.md), [global rules](../rules/global.md) | Greenfield changes; single ownership of validation; validated values versus configuration; full affected-surface updates and verification. |
| [Core specification](../specs/core/project.md) §§3.2–3.3.1, 4.1, 6 | Context-local Rules names; documented Guid/control-character exceptions; parsed outputs; shared comparison and temporal normalization. |
| [Must specification](../specs/must-clauses/project.md), “What MustClauses are” and “Required semantic contract” | `Value` is original input; `Result` is successful parsed/normalized output. Validate configuration before input-dependent returns. Complements operate over the valid domain. |
| [Guard specification](../specs/guard-clauses/project.md) §§4–5, 8–9, 11.6 | Derive forbidden states from Must; prefer `InRange` / `OutOfRange`; return successful Must results; parse once; explicit ordering and object-validator exception. |
| [DataAnnotations specification](../specs/data-annotations/project.md) §§2.1, 3.2 | Domain-wide collision qualifiers; shortest collision-safe names; input-type suffixes where needed; null skipping is intentional. |
| [FluentValidation specification](../specs/fluent-validation/project.md) §§4.2, 5 | Must vocabulary subject to real framework collisions; intentional null skipping and adapter behavior. |
| [Shared vocabulary](../specs/language/vocabulary.md), [machine map](../specs/language/vocabulary.json), [collision policy](../specs/language/naming-collisions.md) | Curated complements, OWASP suffix normalization and framework exceptions; update machine audits with vocabulary changes. |
| [XML schema design](xml-schema-validation.md) §2.2 | `Xml` means well-formedness; `ValidXml` means schema conformance. The distinction and `InvalidXml` are deliberate. |

These recommendations extend policy only where identified below. They are concrete proposed
decisions for this plan, not a list of unanswered questions.

| ID | Recommended design | Existing policy versus new design | Related entries |
|---|---|---|---|
| P1 | Use `InRange` / `OutOfRange` across equivalent range predicates. Use concept-first parsed-type suffixes throughout generic scalar **string-input** families; retain meaningful domain concepts and qualified Core context. | Guard range spelling and annotation collision rules already exist. Library-wide propagation and whole-family Must/Guard scalar suffixes are explicit policy amendments. | A-3/A-8, N-1, A-5, B-6 |
| P2 | Preserve parsed/normalized successful results and remove redundant raw-result twins. | Result semantics are already mandatory. Removing the alternative projections is the proposed surface simplification. No new output-policy approval is needed. | N-2 |
| P3 | Keep `TimeOnlyRange` linear in this correction; add its coherent linear set operations. Design midnight-crossing windows as a distinct circular domain with range-set results before adding that surface. | Prevents a local consistency fix from silently changing every existing range's set model. Circular support is a separate specified feature, not a reason to block the linear corrections. | C-1, C-4/C-5, C-6 |
| P4 | Use a closed, immutable catalogue of declared, completed Enumeration members for GetAll and cached lookups. Keep ordinal string-value identity and independent case-insensitive name lookup. | Catalogue lifecycle is a design decision; existing equality/hash semantics constrain lookup consistency. | E-1, F-1/F-2/F-5 |

Rejected alternatives and their concrete costs remain with the relevant entries. Implementation
must prove the declaration mapping, collision behavior and semantic contracts below; “perfect
alignment” means every difference is explicit and justified, not that adapters with different
responsibilities must have identical signatures.

## The surface that actually collides

Version 1 argued every naming item from `XxxRules` classes. That is the wrong surface. Names in
`NumberRules` and `DateOnlyRules` are qualified by class and cannot collide. The surface that
collides is the one consumers type:

- `Must.Be.*` is 56 static classes contributing extension methods to one receiver (`IMustClause`)
  in one namespace. They share one IntelliSense list and one overload set.
- `Guard.Against.*` is the same shape over `IGuardClause`, and derives its names from Must.
- FluentValidation, DataAnnotations and XML map the same concepts into their own public surfaces.
  Framework collisions, input-representation collisions and qualified Core context are explicit
  reasons for different spelling; existing names alone are not.

Two consequences for this document:

1. **Must owns the approved vocabulary.** Existing Must names are still subject to review.
   Core keeps context-local names where its class already supplies the domain/type; for example,
   `StringRules.Guid.HasVersion` need not repeat Guid merely because a flattened clause needs it.
2. **Inventory actual declarations.** Every rename needs a complete mapping across Rules, Must,
   Guard, FluentValidation, DataAnnotations and XML where applicable, including result type,
   parameter shape, failure code and TFM. The examples below do not substitute for that inventory.

## Integration with temporal Phase 1

The scalar TimeOnly range predicates use `start` / `end` and support midnight crossing
when `start > end`, as specified by temporal Phase 1. Their names are `IsInRange` in Core,
`InRange` / `OutOfRange` on typed Must/Guard, and `InRangeTimeOnly` / `OutOfRangeTimeOnly`
on string-input clauses. DateOnly, DateTime and DateTimeOffset bounds remain `min` / `max`.
`TimeOnlyRange` remains a linear range object under P3; scalar midnight wrapping does not
change its set model. This reconciles A-3/A-8 vocabulary with temporal U3 semantics.

Duration comparisons honor the later temporal D-16 domain vocabulary: typed
`LongerThan` / `ShorterThan`, string-input `LongerThanTimeSpan` / `ShorterThanTimeSpan`,
and Core `IsLongerThan` / `IsShorterThan`. The single-bound parameter remains `other`.
This supersedes GreaterThan/LessThan duration examples in the original findings below,
without changing numeric comparison vocabulary or introducing aliases.

## How to read an entry

Every entry states the defect, a concrete consequence, and either the fix or the options.
The original findings were adversarially reviewed; this revision corrects factual and design
errors found in the subsequent source review. P1–P4 record this plan's recommended design. Other entries
state a recommended engineering disposition and its acceptance criteria. No recommendation is a
claim that the corresponding production change has landed.

Severity reflects impact *if the current behaviour is wrong*. It is not a claim that the current
behaviour is wrong.

## Verification baseline

Re-verified on 2026-11-08 against worktree baseline `e88762c` on
`feature/api-decisions-refresh`; this is not a claim to have reviewed later main commits. Since the previous verification (`55eecd7`,
2026-08-25) 89 commits touched Core, Must and Guard, including the `MustCodes` landing. Changes:

- **C-2 is resolved and removed.** `DateOnlyRules` and `DateTimeOffsetRules` now expose
  `IsWeekend`, `IsWeekday`, `IsFirstDayOfMonth` and `IsLastDayOfMonth`.
- **A-2, A-6, A-7 have existing vocabulary to evaluate.** `Must.Be.Hostname`, `Must.Be.NotEmptyGuid` and
  `Must.Be.ContainsControlChars` / `NotContainsControlChars` exist. Their proposed treatment below is justified by meaning and scoped naming, not by existence alone.
- **A-3 was understated.** The range vocabulary split runs through Must, Guard, FluentValidation,
  DataAnnotations, `MustCodes` and the Guard spec, not just four Core members. Rewritten.
- **B-2's quoted signatures were stale.** `Fail` and `FromBool` now take a `code` first; the
  ordering inconsistency with `Ok` persists.
- **H-1's accessor changed.** `DefaultAllowedNonDigitCharacters` is now a copying property, so
  the CA2105 angle is closed; the missing space and the contradicting example remain.
- **G-1 is clarified.** `GeoLocationRules` is un-gated. The whole `DateOnly` / `TimeOnly` family
  is also gated, which is structural (those types do not exist on netstandard2.1) and is not a gap.
- **Four additional entries** cover N-1 (type qualifiers), N-2 (result-projection twins),
  N-3 (`Guard.Against.IsWhitespace`) and N-4 (the established XML schema vocabulary).
- The critical review re-traced C-3, C-6, F-3, F-5 and F-7 and corrected their recommendations.
- New evidence includes duplicate DateOnly range attributes, typed/string annotation collisions,
  direct ungated GeoLocation dependencies, and XML schema naming outside the original scope.

Line numbers below are current as of `e88762c` unless marked otherwise.

---

## Part I. Corrective work and explicit dependencies

Independent corrections can be implemented without separate policy choices once implementation
is requested. E-1/F-2 depend on P4 and must be designed together. Retain completed rationale in
canonical specs and mark completion with evidence; do not delete the only record of an exception.

### A-1. Align `BufferUtility.IsHexString` / `IsBase64String` with `BufferRules.IsHex` / `IsBase64`

**`src/PineGuard.Core/Utils/BufferUtility.cs`:24, 37** - severity **minor** - `naming`

These predicates carry a redundant `String` suffix in Utils while their Rules counterparts
use the domain names. **Fix:** rename the Utils members to `IsHex` / `IsBase64`. Flattened surface:
none (`Must.Be.Hex` / `Base64` already use the short form).

### A-2. Rename `NetworkRules.IsValidHostname` to `IsHostname`

**`src/PineGuard.Core/Rules/NetworkRules.cs`:101** - severity **minor** - `naming`

The redundant `Valid` in Core's hostname predicate adds no domain distinction. XML schema
`IsValidXml` is different: see N-4 for its established well-formedness/validity distinction. Must already says `Hostname`, Guard `NotHostname`.
**Fix:** rename; update the XML examples on lines 97-98 and every call site. No forwarder.

### A-4. Delete the `StringUtility.ToCase` pass-throughs

**`src/PineGuard.Core/Utils/StringUtility.Casing.cs`:54, 68, 81** - severity **minor** - `naming`

Three `ToCase` overloads forward verbatim to `TryToCase` with bool+out semantics. `ToX` is
reserved for transformations, `Try*` for fallible operations. **Fix:** delete the three
`ToCase` members; add a single-style `TryToCase(value, style, out cased)` convenience overload
so no caller loses a shape.

### B-1. `CsvRules.IsCsvLine` omits the separator and misnames its input

**`src/PineGuard.Core/Rules/CsvRules.cs`:22** - severity **minor** - `api-consistency`

`IsCsvHeaderLine` and both `IsCsvRowLine` overloads take `char separator = DefaultCsvSeparator`;
`IsCsvLine` hard-codes `','`. Its parameter is also named `line`, breaking the `value` invariant
B-3 cites. **Consequence.** `IsCsvLine("a;\"b;c\";d")` cannot be told the file is
semicolon-delimited while `IsCsvHeaderLine` can. **Fix:** rename `line` to `value`, add
`char separator = DefaultCsvSeparator`, forward it. Flattened surface: `Must.Be.CsvLine`,
`Guard.Against.NotCsvLine`, FluentValidation `CsvLine` and `CsvLineAttribute` gain the parameter.
Rename single validated `line` parameters across the CSV family, including CallerArgumentExpression
and XML docs. Quote/CR/LF separators cause the utility to throw; Must must reject invalid separator
configuration first with an attributed failure, preserving the no-throw Must contract. Cover all
adapters, failure codes and Guard translation.

### B-2. `MustResult<T>.Ok` orders `value` / `paramName` opposite to `Fail` and `FromBool`

**`src/PineGuard.Core/MustClauses/MustResult.cs`:96, 110, 169** - severity **minor** - `api-consistency`

`Ok(T? result, object? value = null, string? paramName = null)` versus
`Fail(string code, string messageTemplate, string? paramName, object? value)` and
`FromBool(bool? ok, string code, string messageTemplate, string? paramName, object? value, T? result)`.
Both tail parameters are string-ish, so a swap compiles. **Consequence.** A clause author writing
`Ok(parsed, paramName, value)` by analogy with the `Fail` call above it ships success results whose
`ParamName` is the validated value. **Fix:** `Ok(T? result, string? paramName = null, object? value = null)`,
matching the metadata order of the other factories. Update every call site and use named metadata
arguments internally to prevent silent swaps between adjacent string values.

### B-3 (part). Rename `latitude` / `longitude` parameters to `value`

**`src/PineGuard.Core/Rules/StringRules.GeoLocation.cs`:20, 27** - severity **minor** - `api-consistency`

Single-input rules must name the validated input `value`. `IsLatitude(string? latitude)` and
`IsLongitude(string? longitude)` do not; `IsGeoLocation`'s two-parameter case is the legitimate
exception. **Fix:** rename. Add the explicit `styles` configuration described in B-3 below.

### B-7. `NetworkRules.IsInCidr` parameter naming and nullability

**`src/PineGuard.Core/Rules/NetworkRules.cs`:76** - severity **info** - `api-consistency`

Rename `ip` to `value`, retaining non-nullable `cidr` as required configuration. A lower-level
parser tolerating null does not imply a nullable configuration contract. Must must reject missing
or malformed CIDR configuration in both positive and complement clauses, attributing the failure
to `cidr`; invalid configuration must not become successful out-of-range validation. Retain the domain-specific `InCidrRange` / `NotInCidrRange` vocabulary unless the complete
mapping establishes a collision or semantic defect; P1 does not turn domain names into generic scalar names.

### E-1. `Enumeration` publishes `this` to static registries before construction completes

**`src/PineGuard.Core/Common/Enumeration.cs`:14-46** - severity **minor** - `thread-safety`

A derived constructor that throws after the base constructor leaves a phantom registration, so
the corrected member later fails with "already exists" until process restart. There is also a
rollback race between name and value registration. The base constructor already validates its
arguments; it cannot validate subsequent derived-constructor work. **Fix under P4:** remove
constructor self-registration and publish an immutable catalogue only from completed members.
Define recursive static-initialization behavior so no partial catalogue is cached. Implement
E-1/F-1/F-2 together.

### E-2. `DefaultAllowedDigitSeparators` is a public mutable `char[]`

**`src/PineGuard.Core/Rules/StringRules.NumberTypes.cs`:36** - severity **minor** - `thread-safety`

`public static readonly char[]` protects the reference, not the elements; any consumer can change
process-wide parsing (CA2105). **Consequence.** Library A writes `[1] = '.'`; library B's
`TryParseDigits("4111-1111-1111-1111")` starts returning false. **Fix:** use a private source array and return a fresh array from the public property, matching
PhoneRules on every TFM. Verify that mutating a returned array cannot affect future parsing.
Do not vary the public return type by TFM or expose the backing array indirectly.

### F-2. `FromValue` / `FromName` reflect on every call; the registries are never read

**`src/PineGuard.Core/Common/Enumeration.cs`:81** - severity **minor** - `performance`

`GetFields` + `GetValue` + list allocation + LINQ per lookup, on a path Rules call per row.
**Fix under P4:** cache one declared-member catalogue and its lookup dictionaries. Do not switch
to the existing registries: they contain constructed instances, which is a different membership
set from reflected public fields. GetAll and lookups must agree. Define initialization, duplicate
detection and ordering with E-1/F-1.

### F-3. `OwaspRules` duplicates `OwaspUtility`'s regex chains; XSS has no Utility counterpart

**`src/PineGuard.Core/Utils/OwaspUtility.cs`:21** - severity **minor** - `layering`

The repo invariant says validation logic lives in exactly one layer. Three OR-chains exist twice
character for character, and `ContainsXssRisk` is missing. **Fix:** one layer owns the patterns
(Rules call `!OwaspUtility.Contains*Risk` after their own null gate), add `ContainsXssRisk`. Preserve null/whitespace gates and raw CR/LF detection before any
normalization. These are heuristic risk checks, not proof that arbitrary input is safe. Test
Rules/Utility verdict parity over the existing fixtures and those boundary cases.

### F-4. `RangeRules` hand-rolls inclusion instead of using `RuleComparison`

**`src/PineGuard.Core/Rules/RangeRules.cs`:13**, also `TimeOnlyRules.cs`:150, `CollectionRules.cs`:168 - severity **minor** - `api-consistency`

`inclusion == Inclusion.Inclusive ? <= : <` treats `(Inclusion)42` as Exclusive, while
`RuleComparison` throws `ArgumentOutOfRangeException` for the same input. **Fix:** route through
`RuleComparison`, with configuration validation before value-dependent early returns. The helper
currently checks reversed bounds before its enum switch; collection fast paths can disagree with
iterators. Audit every family, including DateTimeRangeRules. Core may throw for invalid
configuration; Must must return an attributed failure before calling Core. Test invalid Inclusion
with null/empty inputs, reversed bounds, collections and iterators.

### F-7. `CsvColumnSchema` validates `Type` but not `Name` or `MaxLength`

**`src/PineGuard.Core/Common/CsvColumnSchema.cs`:29** - severity **info** - `api-consistency`

The positional record struct also permits invalid states through its typed constructor, init/with
paths and default value. Checks in the string constructor alone cannot establish an invariant.
**Engineering disposition:** retain the value type and explicitly validate schema configuration
at consumption: nonblank Name, defined Type, positive MaxLength. Preserve Core's false result
for invalid schema configuration, using a shared configuration validator so Must can attribute
failures to `schema` before inspecting row data. Constructor checks are supplementary. Keep `datetime` unmapped rather than introducing an implicit timezone
interpretation. Update tests for every construction route.

### F-8. `SignedIntegerPattern` / `SignedIntegerRegex` are public, unused, and disagree with `IsInt32`

**`src/PineGuard.Core/Rules/StringRules.NumberTypes.cs`:19, 26** - severity **info** - `api-consistency`

Dead public API; `\d` matches Unicode digits that `int.TryParse` rejects, so two members of one
class disagree on "is this a signed integer". **Fix:** delete both and their dedicated tests; no production callers were found. The stronger
reason is lack of a useful supported contract, not an assumption that a lexical regex must match
all of an Int32 parser's range and whitespace semantics.

### N-3. `Guard.Against.IsWhitespace` is an `Is`-prefixed Guard member with a forwarding alias

**`src/PineGuard.GuardClauses/GuardStringClauses.cs`:1243, 1386** - severity **minor** - `naming` - *new 2026-11-08*

`IsWhitespace(string?)` is the only `Is*` name on the Guard surface (`Is` is Core's prefix), and
`Whitespace(string?)` forwards to it. Two names for one action; IntelliSense readers stop to diff
them. **Fix:** delete `IsWhitespace`, keep `Whitespace` (which already matches the `char`
overload in `GuardCharClauses`).

---

## Part II. Naming design and exact propagation

Apply existing policy and the explicit P1 amendments as one coherent naming pass when
implementation is requested. Do not treat a source filename as a public type, a shared return
type as an identical contract, or current implementation as sufficient justification.

### A-3 (absorbs A-8). One range vocabulary across equivalent concepts

**Source families:** NumberRules, date/time Rules and their StringRules counterparts,
TimeSpanRules, RuleComparison, string length, collection count and grapheme count;
all corresponding Must, Guard, FluentValidation and DataAnnotations declarations.

**Decision:** use `InRange` / `OutOfRange` for scalar range membership. Preserve the
dimension in compound predicates: `LengthInRange`, `HasCountInRange`,
`HasGraphemeCountInRange`; their outside-range counterparts preserve that dimension and use
`OutOfRange`. Fold `DurationBetween(TimeSpan, …)` into
`InRange(TimeSpan, …)`; string-input TimeSpan uses N-1's `InRangeTimeSpan`.
Core `IsBetween` becomes `IsInRange`; exposed Core negative range predicates use their
corresponding `IsOutOfRange` or dimension-qualified `*OutOfRange` form. Do not
invent an unqualified `OutOfRange` for a count or length predicate.

| Concept | Core | Must | Guard, implemented through the shown Must concept | FluentValidation |
|---|---|---|---|---|
| Scalar inside | `IsInRange` | `InRange` | `OutOfRange` → `InRange` | `InRange` |
| Scalar outside | `IsOutOfRange` | `OutOfRange` | `InRange` → `OutOfRange` | `OutOfRange` |
| String length inside | `IsLengthInRange` | `LengthInRange` | `LengthOutOfRange` → `LengthInRange` | `LengthInRange` |
| Collection count inside | `HasCountInRange` | `HasCountInRange` | `HasCountOutOfRange` → `HasCountInRange` | `HasCountInRange` |
| Grapheme count inside | `HasGraphemeCountInRange` | `HasGraphemeCountInRange` | `HasGraphemeCountOutOfRange` → `HasGraphemeCountInRange` | `HasGraphemeCountInRange` |

For dimensioned outside-range clauses, use `LengthOutOfRange`, `HasCountOutOfRange` and
`HasGraphemeCountOutOfRange`; Guards use the opposite member through Must. The owner identified
the previous mechanical `Not` prefix as an unjustified inconsistency on 2026-11-09. The dimension
does not change the range relationship, so the same `InRange` / `OutOfRange` pair applies.
Preserve inclusion parameters, valid-domain parse gates and existing surface availability.

**DataAnnotations inventory correction.** `DateOnlyRangeAttributes` and its siblings are
source containers, not scalar attribute names. Actual declarations include
`BetweenDateOnlyAttribute`, `NotBetweenDateOnlyAttribute`, `BetweenTimeOnlyAttribute`,
`DurationBetweenTimeSpanAttribute`, `NotDurationBetweenTimeSpanAttribute`,
`BetweenDateTimeOffsetStringAttribute`, `LengthBetweenAttribute`,
`HasCountBetweenCollectionAttribute` and the grapheme attributes. Rename their
**concept** segments using the table, retaining required domain/input qualifiers:
`InRangeDateOnlyAttribute`, `OutOfRangeDateOnlyAttribute`,
`InRangeTimeOnlyAttribute`, `InRangeTimeSpanAttribute`,
`OutOfRangeTimeSpanAttribute`, `InRangeDateTimeOffsetStringAttribute`,
`LengthInRangeAttribute`, `HasCountInRangeCollectionAttribute`.
Do not rename range-object attributes merely because their filenames contain “Range”.

`DateOnlyBetweenAttribute` in TimeAttributes and `BetweenDateOnlyAttribute` in
DateOnlyAttributes duplicate the same scalar validation. Consolidate into the single
`InRangeDateOnlyAttribute`, with one canonical declaration and test family.
There is no current `BetweenDateOnlyStringAttribute`; do not invent inventory entries.

**Why this choice:** Guard §5.4.5 already prefers the pair. Extending it across scalar concepts
removes the number/time split. The code catalogue is already range-based; retain existing
semantic codes and verify mappings. Guard's default failure path is GuardFailure; this decision
does **not** imply all range failures throw ArgumentOutOfRangeException.

**Rejected:** keep `Between` in Must and curated `OutOfRange` in Guard (two words for the
same ordinary range concept); rename everything to `Between` (contradicts established Guard
policy); use `WithinRange` (adds vocabulary beside existing proximity `Within`).
Neither ecosystem popularity nor filenames substitute for this internal rationale.

### N-1. One qualifier policy for string-input scalar clauses

The current mix of `Int32InRange`, bare decimal `InRange`,
`BetweenDateOnly` and `GreaterThan(string, TimeSpan)` is inconsistent on the
flattened entry points. A suffix cannot mean parsed type in one family and raw-result projection
in another.

**Proposed amendment P1:** generic scalar predicates taking strings use
`<Concept><ParsedType>`, throughout each family. Typed-input overloads retain the short
concept. Core nested classes keep the type in the class rather than duplicating it in members.
This extends existing policy: Core §3.3 currently permits short typed/string overloads; whole-family
suffixing on Must/Guard is a deliberate new rule, not an existing requirement.

| Current string-input Must member | Proposed Must member | Successful type / reason |
|---|---|---|
| `Int32InRange`, `Int64InRange` | `InRangeInt32`, `InRangeInt64` | Int32, Int64 |
| `InRange`, `OutOfRange` for decimal | `InRangeDecimal`, `OutOfRangeDecimal` | Decimal |
| `Positive`, `Negative` for decimal | `PositiveDecimal`, `NegativeDecimal` | Decimal; same rule for every generic numeric comparison in that family |
| `True`, `False` | `TrueBoolean`, `FalseBoolean` | Boolean, using CLR type names consistently with Int32; no mixed Bool/Boolean spelling |
| `Even`, `Odd` | `EvenInt32`, `OddInt32` | Int32 |
| `Finite`, `NotNaN` | `FiniteDouble`, `NotNaNDouble` | Double; preserve IEEE predicate meaning |
| `GreaterThan`, `DurationBetween` for TimeSpan | `GreaterThanTimeSpan`, `InRangeTimeSpan` | TimeSpan |
| `BetweenDateOnly`, `PastDateOnly`, `MinimumAge` | `InRangeDateOnly`, `PastDateOnly`, `MinimumAgeDateOnly` | DateOnly |
| `BetweenTimeOnly`, `BetweenDateTimeOffset` | `InRangeTimeOnly`, `InRangeDateTimeOffset` | TimeOnly, DateTimeOffset |

The table demonstrates the rule; the implementation inventory must enumerate **every declaration**,
including negative/complement forms, equality, chronology, proximity, calendar and numeric
families. Do not finish by renaming only the range examples.

**Domain concepts are not generic scalar predicates.** `Guid`, `HasGuidVersion`,
`Ipv4`, `Ipv6`, `IpAddress`, `PhoneNumber`, `Email`, `Hostname` and
`ValidXml` already state their domain. Keep them: no `Ipv4IPAddress`,
`PhoneNumberString` or `HasGuidVersionGuid`. A type already at the end is not appended
twice (`NotEmptyGuid` remains); embedded domain nouns such as `HasGuidVersion` are explicit
semantic names, not ad hoc qualifier placement. Document each family boundary in the manifest.

**Propagation:** Guard takes the corresponding forbidden-state vocabulary and the same terminal
parsed-type qualifier (`ZeroOrNegativeDecimal` → `PositiveDecimal`, `FalseBoolean`,
`OutOfRangeInt32`). FluentValidation follows Must subject to actual framework collisions.
DataAnnotations retain their own specified shape: domain/type qualifier, then `String` where
it distinguishes string input from a typed attribute. For example `PastDateOnly` and
`PastDateOnlyString` are an existing, justified distinction. This is **input representation**,
not N-2's redundant return projection. Do not mechanically double qualifiers when combining rules.

**Rejected alternatives:** type-first families (`DateOnlyPast`) undermine concept grouping;
qualify only compiler collisions permits lean standard overloads but perpetuates mixed
`InRange(string, DateOnly, DateOnly)` / `PastDateOnly(string)` families.
Whole-family qualification is chosen for predictable discovery. Its cost is longer names and
additional renames, not compatibility. Do not claim every existing short overload is ambiguous;
prove actual collisions with compiled consumer examples.

### N-2. Remove raw-result twins; preserve the established result contract

`Ipv4`, `Ipv6` and `IpAddress` produce parsed IPAddress results;
their `*String` twins preserve input text. `PhoneNumber` produces normalized digits;
`PhoneNumberString` preserves the original input. The successful output policy is already
settled by Must and Core: `Value` retains input and `Result` holds the parsed/normalized
result.

**Decision:** keep that policy and delete raw-result twins and corresponding redundant adapters.
Do not change PhoneNumber.Result to original text and do not add a PhoneDigits alias.
Inspect each polarity before consolidating: `Guard.Against.NotIpv4` returns IPAddress, while
`NotIpv4String` returns string. The guards forbidding a valid IPv4 value both return string
when the corresponding valid-domain complement succeeds. The two directions are not all
identical signatures/contracts.

Guard returns the successful Must result; it has no `Value` property. A caller needing original
text alongside the Guard output retains its input variable. FluentValidation validates a property
without replacing that property with the parsed result. Attribute `String` input discriminators
are governed by N-1 and are not deleted by a suffix search.

**Rejected:** change every successful result to raw input (violates established parse/normalize
architecture); introduce `Raw` / `Text` aliases (duplicates validation vocabulary for an
output projection). Tests must assert original Value, successful Result and Guard return value
separately, including formatted phone numbers and both network polarities.

### A-5. Document affirmative Contains vocabulary and support nullable value-type elements

Keep `CollectionRules.Contains`, `ContainsNullItems` and their established Must/Guard
concepts. Membership and containment are useful domain predicates, consistent with the existing
range and collection surfaces. Add the affirmative `Contains*` convention to Core §3.2;
§3.3.1 is a **negation-exception table**, not a general waiver. Retain its specific existing
Guid/control-character exceptions separately.

Add nullable value-type element support alongside reference-type support across Rules, Must,
Guard and applicable adapters. Reject null collections consistently with their existing contract;
a null element is distinct from an empty collection. Do not implement the negative clause as
unconditional Boolean negation over invalid input.

For any successful non-null-element result, document the actual result type. Converting
`IEnumerable<T?>` to `IEnumerable<T>` for structs requires projecting each nullable value's
`Value`; null-forgiving annotations cannot change the runtime element type. Enumerate a
one-shot source once and reuse the materialized validation/result data. Verify generic inference,
nullable reference annotations and struct overload resolution.

**Rejected:** Core-only `HasItem` renaming would split a domain concept already shared by
range and collection consumers; pretending `!` implements a typed sequence projection is
incorrect. Any method whose result remains the original nullable-element collection must say so,
rather than promising narrowed output.

### B-6. Comparison-bound parameter naming and nullability drift

**`src/PineGuard.Core/Rules/TimeSpanRules.cs`:29**, `StringRules.Numbers.cs`:83, `NumberRules`, the date families - severity **minor** - `api-consistency`

The same "compare value to a bound" concept names the bound `min` / `max` (numbers, `T`),
`threshold` (TimeSpan: `TimeSpan?` in `TimeSpanRules`, non-nullable in `StringRules.TimeSpan`),
or `other` (dates, nullable). **Flattened surface.** The `Must.Be.GreaterThan` overloads disagree
with each other: `GreaterThan(T value, T min)`, `GreaterThan(string, decimal min)`,
`GreaterThan(TimeSpan, TimeSpan threshold)`, `GreaterThan(string, TimeSpan threshold)`.
Overloads of one method name with different names for the same role is a named-argument trap
the Core framing could not see.

**Consequence.** `Must.Be.GreaterThan(TimeSpan.FromMinutes(10), threshold: TimeSpan.FromMinutes(5))`
compiles, but `Must.Be.GreaterThan(10, threshold: 5)` does not because the numeric bound is
named `min`; `TimeSpanRules.IsGreaterThan(v, null)` compiles and returns false while
`StringRules.TimeSpan.IsGreaterThan(v, null)` does not compile.

**Decision.** Two-bound members use `min` / `max`; single-bound comparisons use `other`
across every family (`GreaterThan(value, other)`, `Before(value, other)`). Required bounds are non-nullable configuration. Core may reject invalid configuration;
Must returns a configuration-attributed failure before invoking Core, and Guard translates that
failure. A deliberately optional bound needs a distinct documented contract, not accidental
nullable drift. Rename parameters through CallerArgumentExpression, XML docs and named call sites.

| Candidate | Story | Why it lost |
|---|---|---|
| **`other` for one bound, `min`/`max` for two** (recommended) | "greater than the other" | — Already the date families' word; `Equals(other)` / `CompareTo(other)` precedent in the BCL. |
| `threshold` everywhere | "crosses the threshold" | Wrong for `Before` / `After` and for equality; only reads for magnitudes. |
| `min` for `GreaterThan`, `max` for `LessThan` | mirrors the range pair | Direction-specific parameter names fragment named calls across otherwise parallel predicates; inclusion remains an explicit separate property. |

---

## Part III. Shape and semantics dispositions

The following recommendations are resolved for this plan. Their testable contracts matter as much
as naming. Policy amendments and independent future features are identified explicitly.

### B-3 (part). Expose NumberStyles on string coordinate predicates

Add `NumberStyles styles` with the existing parser default to the latitude/longitude and
combined coordinate-string families. Forward it consistently through applicable clauses and
adapters, with invariant culture and configuration-first validation. Preserve the existing default
accepted grammar; allow explicit scientific notation when requested. Tests cover `"9e1"`,
out-of-range coordinates, invalid styles and culture independence. Pair this with G-1's removal
of unnecessary target gating.

**Rejected:** permanently hard-code fixed-point syntax while sibling parsers expose styles; this
creates avoidable configuration inconsistency without a domain requirement.

### B-4 / B-5 / B-8 (merged). Type-sensitive Try output contracts

Delete the duplicate nullable Guid output overload: `out var` cannot choose between
`out Guid` and `out Guid?`. Use `out Guid` with default on failure.
Use the same value-type rule for truncation outputs; callers must check the Boolean result.

“Non-nullable outputs everywhere” is incorrect for reference types. Use:

| Output category | Contract |
|---|---|
| Concrete value type | `out T`, default on failure; no redundant `out T?` overload |
| Reference type that returns null on failure | `[NotNullWhen(true)] out T?` |
| Generic output that may be default/null on failure | Appropriate `[MaybeNullWhen(false)] out T` contract and constraints |
| Existing non-null empty string/list output | Preserve the useful non-null empty-output contract; document it explicitly |

Audit Try members rather than regex-replacing question marks. Successful-null outputs, if any,
need an explicit reason and cannot carry a false NotNullWhen promise. Verify consumer nullable
flow and inference on every supported target. See the [.NET nullability guidelines](https://github.com/dotnet/runtime/blob/main/docs/coding-guidelines/api-guidelines/nullability.md)
for the conditional-output annotation model.

### C-1. Separate range duration from inclusive cardinality

Set DateOnlyRange.Duration to the calendar-day distance `End.DayNumber - Start.DayNumber`;
retain the inclusive count under `DayCount`. A singleton has zero duration and one included day.
DateOnly is a calendar domain, not a timezone-dependent elapsed-time measurement.

For DateTimeRange, use the same normalized instant endpoints for Duration as for ordering and
equality. Raw `End - Start` can disagree when Kind differs. DateTimeOffsetRange uses elapsed
instants. TimeOnlyRange remains linear under P3. Test singleton, extrema, mixed DateTime kinds,
offsets and daylight-saving boundaries using deterministic timezone setup where required.
Specify each unit in XML documentation; a shared property name does not erase domain differences.

### C-3. Keep exact header matching distinct from browser security processing

Keep generic `HttpRules.HasHeaderValue` as exact whole-value matching and document it.
Do not split arbitrary headers on commas. Even security headers do not share one token algorithm.

For `HasXContentTypeOptionsWithDefaults`, explicitly document the current
whole-value presence check: an exact `nosniff` entry is accepted even when another supplied
entry is `garbage`; a single combined value is not accepted. This is not a configuration-conformance
or browser-effectiveness oracle. The [Fetch nosniff algorithm](https://fetch.spec.whatwg.org/#x-content-type-options-header)
examines the first extracted value: `nosniff, garbage` and `garbage, nosniff` are not
interchangeable. An “any token equals nosniff” rewrite would give false assurance.

**Decision:** retain exact-value presence matching in this correction and correct claims/examples
that equate rejection with absent browser protection. If browser-effectiveness validation is
introduced, give it a separately specified contract and header-specific parser; do not change
this predicate silently. Test repeated field values, combined fields, ordering, case and whitespace
against the chosen exact-value presence contract. In particular, `["garbage", "nosniff"]`
passes this presence predicate; `"garbage, nosniff"` does not. Neither result is a claim about
complete header conformance. Review the other security headers independently.

### C-4 / C-5 (merged). Preserve a coherent linear TimeOnlyRange; specify circular windows separately

The TimeOnlyRange constructor **already documents** `Start <= End`. Its linear domain is not
an undocumented bug. Keep this invariant for P3; add the missing linear operations with the same
closed-set semantics as the other ranges, rather than changing Contains alone to wrap around.

Use `Intersect` for a nullable single-range intersection. A true `Union` of two disjoint
linear ranges cannot return one range: return an ordered, normalized read-only range collection
across the four range families. Empty collection means empty set; coalesce overlapping or adjacent
ranges under the domain's representable-step model. Rename existing gap-filling union behavior
to `Enclose`; it returns the smallest linear range containing both operands and may include
values absent from either. Do not use Union to mean an enclosing hull.

**Circular follow-on specification:** a midnight-crossing interval needs an explicit representation
of empty, singleton and full-day states, endpoint inclusion, and range sets for disconnected results.
For example, closed arcs 22:00–10:00 and 08:00–02:00 intersect in two arcs:
22:00–02:00 and 08:00–10:00. A single nullable range cannot represent that result. Any circular
enclosure operation also needs a deterministic tie rule where two equal hulls exist.

**Rejected:** simply permit Start > End and copy linear Intersect/Union (mathematically wrong);
silently return a hull from Union (adds values). Circular support remains a separate designed
feature requirement recorded in this section; the linear naming and semantic corrections do
not depend on its introduction.

### C-6. Make closed-range overlap, intersection and adjacency agree

Default Overlaps currently uses exclusive comparisons on several range types while Contains and
Intersect include endpoints. Fix the whole model: default Overlaps for closed ranges is true iff
Intersect is nonempty. Shared endpoints overlap; a singleton overlaps itself.

IsAdjacentTo means disjoint with no representable value between the ranges: one day for DateOnly,
one tick for the other linear temporal ranges. Compare safely near MinValue/MaxValue rather than
adding a unit that can overflow. Normalize DateTime instants before comparison.

Overlap and adjacency are **mutually exclusive**, not an exhaustive XOR: separated ranges have
neither relation. Define `Overlaps(other, Inclusion.Exclusive)` as intersection of the two open intervals on
that type's representable domain: both operands exclude their endpoints. A singleton and a
one-step interval have empty open interiors. An overlap exists only when a representable value
is strictly greater than both starts and strictly less than both ends; compute this without
overflow. Do not retain the current strict-cross-comparison shortcut, which can report a
singleton contained in another range as overlapping despite its empty interior. This explicit
mode does not redefine the default closed-set Intersect.
Test endpoint touch, singleton, containment, one-step adjacency, larger gaps, reversed operands
and extrema across all four range types. Apply the same model to Rules and adapters.

### D-1. `CollectionRules.Contains` uses the collection's comparer for `ICollection<T>` only

**`src/PineGuard.Core/Rules/CollectionRules.cs`:279** - severity **minor** - `api-consistency`

`HashSet<string>(OrdinalIgnoreCase){"ABC"}`: `Contains(set, "abc")` is true, `Contains(set.ToList(), "abc")`
is false, and the XML doc promises the default comparer. **Options.** Drop the fast path; or document
that the collection's semantics win; and either way consider an `IEqualityComparer<T>?` overload like
`HasDistinctItems`. Recommendation: add the comparer parameter and make the verdict depend on
contents, not runtime type.

### D-2. `IsSubsetOf` silently adopts a caller-supplied `HashSet<T>`'s comparer

**`src/PineGuard.Core/Rules/CollectionRules.cs`:298** - severity **minor** - `api-consistency`

Same defect as D-1 for `other`. **Options.** Reuse `other` only when its `Comparer` is the default, or
add `IEqualityComparer<T>? comparer = null` and always build the set with it. Recommendation: the
comparer parameter, decided together with D-1.

### F-1. Define Enumeration catalogue membership and inheritance explicitly

Removing `DeclaredOnly` alone does not solve inheritance. Static reflection needs intentional
hierarchy traversal/FlattenHierarchy, and an instance of PaymentMethod cannot be returned as
ExtendedPaymentMethod merely because its field is declared on a base type.

**Decision under P4:** each queried enumeration type has one closed catalogue built from public
static readonly member fields on that type and its relevant base hierarchy. A field is eligible
when its declared type is assignable to the queried type; include only completed instances of
that type. This deliberately does not inspect every base-typed field for a possible runtime subtype.
An eligible member field containing null is a
catalogue-definition/initialization error, never a silently skipped entry. Exclude arbitrary static
properties and unrelated constructed instances. Order the accepted catalogue by ordinal Name;
reject duplicate names under the name comparer and duplicate values under the value comparer
before publication.

Do not promise discovery of every subclass in every loaded assembly. A family root may hold
declared subtype instances assignable to that root; a subtype query cannot contain base instances.
Catalogue queries are supported only after the queried type's static initialization completes
successfully. Queries from that type's static initializer, member initializer or construction path
are unsupported: an initializer cannot require its own completed catalogue. Normal external
discovery first ensures successful initialization of the queried type and each participating field's
declaring type, then validates and atomically publishes the immutable catalogue. Eligible null
fields, reentrant catalogue builds and duplicate names/values cause deterministic failures;
cache build failures without exposing partial results. These checks detect common initialization
misuse but do not promise detection of every unsupported same-thread initialization-time query.

Test supported external initialization, base/derived fields, unassignable members, concurrent first
access, failed static initialization, failed construction, duplicates and detected null/reentrant
misuse. Include a first-ever lookup during member initialization before later fields are assigned;
it must not silently cache a catalogue missing those fields. Preserve the existing reflection/
trimming annotations through the cache and hierarchy-discovery path; verify the supported trimmed
publication scenario rather than assuming a generic cache automatically retains reflected fields.

**Rejected:** reuse constructor registries (different, dynamic membership); cast inherited base
members into a subtype (invalid); switch to dynamic registration without defining its lifecycle.
Implement with E-1/F-2 as one change.

### F-5. Preserve string-value identity; document name lookup separately

Keep string-valued Enumeration equality, hashing, ordering and value lookup ordinal. A display/name
lookup may remain OrdinalIgnoreCase. These are different roles, so case-insensitive lookup of
the name `"usd"` does not imply case-insensitive identity of the value `"USD"`.

Changing only lookup to OrdinalIgnoreCase would collapse values such as `"abc"` and `"ABC"`
while equality still distinguishes them. Verify duplicate detection and caches use the same
comparer as their respective role. Document this deliberate distinction.

**Rejected:** “the asymmetry has no user” is unsupported. If configurable value comparers are ever
added, one immutable comparer must govern the entire family, including equality, hash codes,
ordering, duplicate detection and lookup. That additional feature is not needed here.

### F-6. Enforce the documented lowercase SpaceCase contract

StringCasing.SpaceCase explicitly documents lowercase words separated by spaces; this is not
merely an ambiguous example. Fix both the transform (currently renders title case) and validation
(currently accepts mixed case) to that documented contract. Do not rewrite the documentation to
bless the implementation accident.

Tests cover lowercase, uppercase/mixed-case rejection, digits, separator placement, transform
idempotence and the invariant that a successful transform satisfies the matching predicate.
Keep word splitting and Unicode/culture policy consistent with the shared casing machinery.

### G-1. Remove unnecessary target gating; document genuine capability differences

netstandard2.1 is a supported target, not a promise that unavailable BCL types exist there.
Maintain a capability matrix: DateOnly/TimeOnly and generic-math APIs need appropriate target
support; common string/number behavior should not disappear merely because a whole file was gated.

StringRules.GeoLocation calls an ungated double parser and GeoLocationRules directly.
Lift that file's unnecessary gate; it does **not** need a decimal fallback.
Review StringRules.Numbers separately: where it relies on generic NumberRules, supply equivalent
concrete numeric implementations only with the same parsing, rounding, overflow and NaN contracts.
Do not replace double semantics with decimal as a blanket fallback.

Publish the exact supported-family/TFM matrix and test all common members for equivalent results.
Any intentionally unavailable signature must have a concrete platform reason, not an unexplained
conditional compilation outlier. Reconcile target constants and package READMEs with the matrix.

### H-1. Make the phone-format example true without overclaiming validation

Add ASCII space to the default permitted non-digit characters. The documented formatted example
`"+1 (555) 123-4567"` should pass the existing permissive phone-format contract.
Verify the parsed/normalized Result contains the digits and Value preserves the original input.

This is formatting tolerance, not proof of an allocated number or complete E.164 conformance.
Correct any documentation that makes that stronger claim. Coordinate with E-2's defensive-copy
pattern and N-2's preserved normalized output.

### N-4. Preserve and formally register XML's well-formedness/validity distinction

The XML schema design §2.2 deliberately separates the two concepts:

| Concept | Rules | Must | Guard | DataAnnotations | FluentValidation |
|---|---|---|---|---|---|
| Well-formed XML document | `XmlRules.IsXml` | `Xml` | `NotXml` | `XmlStringAttribute` | `Xml` |
| XSD schema conformance | `XmlSchemaRules.IsValidXml` | `ValidXml` | `InvalidXml` | `ValidXmlAttribute` | Existing `MustBe` composition recipe; no dedicated adapter in PineGuard.Xml |

Keep these names. XML “validity” is an established domain distinction; removing Valid would
conflate different validations. `XmlSchemaRules` avoids colliding with Core's XmlRules when both
namespaces are imported. `XmlSchemaMustValidator` follows the validator naming pattern and
avoids `System.Xml.Schema.XmlSchemaValidator`. The dedicated `PineGuard.Xml` namespace
and absence of a direct FluentValidation dependency are intentional package boundaries.

Carry the rationale from the XML design into the canonical vocabulary, Core/Guard specifications
and audit map. Scope the `InvalidXml` exception to this concept; it is not permission to invent
InvalidEmail or InvalidGuid. Also record `Guard.Against.Invalid` as the existing object-validator
entry point exception from Guard §11.6.

Repair the dangling `src/PineGuard.Xml/AGENTS.md` reference to
`docs/ai/rules/xml.md` by completing XML scope onboarding during implementation. The missing
rules page does not erase the existing design rationale. Keep schema failures, unknown namespaces
and malformed XML distinct in codes, messages and tests. Verify import collisions with System.Xml,
System.Xml.Schema, System.Xml.Serialization and the core PineGuard layers.

---

## Existing exceptions retained and Guid symmetry completed

- **A-6. Guid predicate names retained; missing twin implemented.**
  Core §3.3.1 explicitly permits `StringRules.Guid.IsNotEmpty`;
  `Must.Be.NotEmptyGuid` and `Guard.Against.EmptyGuid` are aligned with that concept.
  The implementation adds the Core `IsEmpty` twin and tests both predicates.
  Both must require successful Guid parsing; malformed/null strings are false for both.
  They are complements within the parsed-Guid domain, not over every possible string.
  Completion evidence is recorded in the linked implementation tracker.
- **A-7. Control-character exception retained.** Core §3.3.1 permits
  `NotContainsControlChars`; Must exposes both polarities and Guard derives the forbidden
  state through Must. `IsControlCharSafe` would borrow the OWASP-specific Safe convention.
- **C-2. Calendar predicates resolved at the baseline.** Retain the historical disposition;
  no new implementation is required for this finding.

## Exception register: differences must earn their place

This is the minimum register, not permission to ignore unnamed outliers. Every affected declaration
must be mapped to the general rule or a specific, documented exception.

| Difference | Reason and governing policy | Required verification |
|---|---|---|
| Context-local Core names versus qualified flattened clauses | Rules class supplies domain/type; Must/Guard share an extension receiver. Core §3.3 and P1/N-1. | Consumer examples and exact mapping; no duplicated nouns without a collision reason. |
| Attribute domain/type and final String qualifiers | Attributes do not overload by validated property type. DataAnnotations §§2.1, 3.2. | Typed/string property examples; unique declarations; whole affected-domain family reviewed. |
| Fluent `WebUrl`, `Required`, `NotRequired` and other proven framework collisions | FluentValidation and BCL imports can make otherwise matching extensions ambiguous. Collision policy and Fluent §4.2. | Compile with the relevant framework imports; select one canonical collision-safe spelling. |
| XML Valid/Invalid vocabulary and dedicated namespace | Well-formedness versus XSD validity, package isolation and BCL type-name collisions. XML design §2.2; N-4. | Distinct semantic tests, import tests and exact audit-map entry. |
| Guard `Invalid` object-validator entry point | Existing Guard §11.6 umbrella-validation contract. | Limit exemption to the exact entry point; no general Invalid-prefix waiver. |
| Guid `IsNotEmpty`; control-character `NotContainsControlChars` | Explicit Core §3.3.1 domain exceptions. | Valid-domain gating and correct Must/Guard polarity. |
| Contains predicate spelling | Affirmative domain membership/containment; A-5 adds explicit Core §3.2 policy. | Collection and range vocabulary parity; no Core-only synonym. |
| Guard temporal, distinct/duplicate and OWASP opposites | Curated semantic complements in Guard §5.4 and the shared map, not mechanical Not-prefix stripping. | Boundary/equality tests and positive/negative mapping; OWASP Safe normalization remains scoped. |
| Guard file ordering | Ordered by the Must clause invoked, per Guard §4. | Review method pairs by delegation, not just the literal negative prefix. |
| Annotation/Fluent null skipping versus direct Must/Guard rejection | Optional property validation differs from direct validation; their layer specs explicitly define it. | Null tests per adapter; requiredness composed explicitly. |
| Enumeration name lookup versus value identity | Human-facing name lookup versus identifier equality; F-5. | Role-specific comparer tests across lookup, duplicate detection, equality and hashing. |
| Platform-specific types | DateOnly/TimeOnly and generic-math availability; G-1. | Published capability matrix and shared-contract tests for common signatures. |

The register must identify the exact family/member and rationale; “legacy,” “already exists,”
“more ergonomic,” or “avoids a breaking change” is insufficient. Compatibility aliases are absent.
A framework-required facade is not a compatibility shim, but redundant public spellings for the
same PineGuard operation are not retained merely because a current spec permits optional aliases.

## Canonical policy reconciliation required by implementation

Update policy before or with its corresponding source change; do not leave this plan as a
competing permanent source of truth.

1. Must/Core/Guard/FV/DataAnnotations: add the P1 range and string-input qualifier mappings;
   replace stale Between examples and stale MustResult factory examples, preserving required codes.
2. Core §3.2: explicitly permit affirmative Contains vocabulary. Keep §3.3.1 focused on
   the existing negation exceptions; describe parsed-domain rather than universal Boolean complements.
3. Core/Guard/shared vocabulary: register XML validity and the exact Guard Invalid entry point.
   Qualify Fluent §4.2's broad statement that Guard may prefer Invalid*; its current wording
   conflicts with Guard's general prohibition.
4. Clarify nonthrowing validation versus invalid configuration using the existing scope contracts:
   Core helpers can reject bad configuration; Must turns configuration errors into attributed
   failures; Guard throws from Must failures. Do not weaken this to “all layers never throw.”
5. Update vocabulary.json, collision normalization and audit baselines with specific mappings.
   Do not silence new findings through broad exemptions or regenerate baselines without review.
6. Complete the XML rules/spec onboarding reference and preserve the established no-direct-FV-
   dependency architecture. Align affected READMEs, XML comments, examples, fixture names and tests.
7. Resolve any allowance for optional convenience aliases in the affected naming specs explicitly:
   this greenfield pass keeps one canonical spelling per operation, except a distinct documented
   framework facade. Remove historical examples of deleted aliases.

## Entry points remain as specified

Retain `Must.Be` and `Guard.Against`. Has/Contains members have a grammar tradeoff under
Must.Be, but the assertion that a second entry point is the “only cure” is not established.
No Must.Have redesign is needed to complete this plan. Preserve the one approved vocabulary and
do not invent synonyms to make isolated call sites read more naturally.

## Order of work

1. **Plan completion (completed before implementation):** reconcile review findings with existing policy, supply the
   recommended decisions and acceptance gates, and leave the plan reviewable and uncommitted.
2. **Establish the complete API manifest before renaming:** inventory actual public declarations,
   not filenames. Include Core/Utils, Must, Guard, FluentValidation, DataAnnotations and XML;
   record input/output types, named parameters/defaults, nullable flow, codes, complements,
   configuration behavior and TFM availability. Resolve all collisions before editing consumers.
3. **Independent corrections:** H-1, A-1/A-2/A-4, B-1/B-2/B-3/B-7, E-2, F-3/F-4/F-7/F-8,
   N-3 and the A-6 twin. Apply dependencies explicitly (CSV schema/config validation together;
   phone result tests with N-2; common configuration handling before adapter propagation).
4. **Vocabulary and surface changes:** update canonical policy and machine maps, then A-3/A-8,
   N-1/N-2/N-4, A-5 and B-6 across all affected layers. Consolidate duplicate attributes and
   delete obsolete spellings outright. Group by complete semantic family, not isolated project.
5. **Semantic work:** B-4/B-5/B-8; C-1/C-4/C-5/C-6 as a coherent range pass; D-1/D-2 together;
   E-1/F-1/F-2/F-5 together; B-3/G-1 target parity; F-6 casing. C-3 is an exact-match
   documentation/contract correction, not a generic token-splitting patch.
6. **Verify and record completion:** satisfy the acceptance gates below, mark each original ID
   with its completion evidence, and retain rationale in canonical policy. No item is “done”
   merely because its paragraph has been deleted.

## Acceptance gates

- **Complete declaration mapping:** every affected public member has an old/new disposition
  (retain, rename, merge, remove, add) with all layers and target frameworks accounted for.
  Every exception has a precise register entry and source policy; no unexplained outlier remains.
- **Compiler proof:** consumer examples exercise imports together, overload inference,
  `out var`, nullable flow, named arguments and omitted defaults. Include typed/string
  collisions, LINQ/FV/BCL imports, attributes and XML namespace/type collisions.
- **Semantic parity:** Rules/Must/Guard/adapters agree on the applicable valid domain, parsed
  result and canonical failure code. Test malformed input, null, empty values, configuration
  errors and every complement boundary. Respect explicit adapter null-skipping policy.
- **Configuration precedence:** invalid separators, schemas, CIDR, bounds, Inclusion and styles
  cannot be hidden by input-dependent short-circuits or negative predicates. Must failures name
  the configuration parameter; Guard uses the corresponding canonical failure.
- **Result contracts:** assert original Value versus Result, normalized phone digits, parsed IP
  addresses, Guard returns, one-pass enumerables and nullable-element projection separately.
- **Range laws:** default Overlaps iff Intersect is nonempty; adjacency implies no overlap;
  Union contains exactly the operands' values; Enclose may fill gaps and is named accordingly.
  Include singleton/empty results, disconnected union, boundary inclusions, extrema, normalized
  instants and commutativity. Explicit exclusive overlap tests include empty open interiors for
  singleton/one-step intervals and the first nonempty open interval. No circular semantics leak
  into the linear range type.
- **Enumeration invariants:** closed membership shared by GetAll/lookups, no partial publication,
  deterministic ordering and failure, assignability, duplicate/comparer consistency, concurrent
  initialization and failed derived construction.
- **Documentation and vocabulary:** stale-name search distinguishes historical rationale from
  live examples. Update XML documentation, README examples, canonical specs, fixtures, tooling
  and machine maps. Audit links and the XML onboarding reference.
- **Repository verification:** run the affected build/test/format/audit checks and the repository's
  required coverage gates for each affected supported test TFM. Compile netstandard2.1 surfaces
  even where test projects target newer runtimes. Broaden to required cross-layer checks after
  shared Core changes. Actual results belong in the linked implementation evidence rather than
  being inferred from this list of requirements.

## Completion boundary

This decision record is now implemented. The linked implementation tracker records each original
decision, the exact baseline-to-final declaration map, consumer compilation, independent review
and 100% line/branch coverage on both supported test runtimes. A written plan alone does not
prove API correctness; those concrete checks are the completion evidence. No backward-compatibility
aliases or migration work were introduced.
