<!-- metadata_header
type: plan
id: new-surfaces-05-rule-batches
version: 1.2
status: planned
last_updated: 2026-08-26
parent: new-surfaces-program
-->

# Plan 05 — Phase 5: Rule-level batches (non-ISO)

<!-- plan-nav -->
> [Parent](new-surfaces-missing-validation-cases.md) · [00 Program](new-surfaces-missing-validation-cases-00-program.md) · [01 Structural validation](new-surfaces-missing-validation-cases-01-structural-validation.md) · [02 Options](new-surfaces-missing-validation-cases-02-options.md) · [03 ASP.NET Core](new-surfaces-missing-validation-cases-03-aspnetcore.md) · [04 MediatR & bridges](new-surfaces-missing-validation-cases-04-mediatr-result-bridges.md) · **05 Rule batches** · [06 Analyzers](new-surfaces-missing-validation-cases-06-analyzers.md)
<!-- /plan-nav -->

> **Status**: Planned | **Depends on**: Phase 1 (error codes — every new clause needs its `MustCodes` constant; Rule13 gates it) | **Runs in parallel with**: Phases 2–4 (independent code paths; expect merge conflicts only in a shared `MustCodes.<Domain>.cs`, `vocabulary.json` and `gold-standard.md`)
>
> **Worktrees**: one per batch — `rules-string-content`, `rules-identifiers`, `rules-unicode`, `rules-numeric`, `rules-temporal`, `rules-file-signature` — each its own PR. Batches are independent; run them in the listed order unless the operator pulls Batch E (clock injection, the differentiator) forward.
>
> Read [Plan 00](new-surfaces-missing-validation-cases-00-program.md) first. Every batch is a **vertical slice** executed with `docs/ai/agents/scaffold-vertical-slice.md` → `docs/ai/skills/new-validation/SKILL.md` (Utils → Rules → Must → Guard → Fluent → DataAnnotations → Tests), with the per-layer specs and skills that agent already routes to. This plan only says **what** to build and **what to call it**; the procedure is not restated.

## 1. Business plan

### 1.1 Why these, why now

`competitive-analysis.md` §2 and §6.3 list the formats a Zod- or FluentValidation-literate evaluator expects and does not find: `Contains`/`StartsWith`/`EndsWith` as first-class rules, JWT shape, SemVer, cron, ULID, MAC, media types, Base64Url, grapheme-aware lengths, decimal precision/scale, Luhn, minimum age, and magic-byte file signatures. None decides an evaluation on its own; together they remove the "but it doesn't even have X" objection, and each is a cheap, mechanical vertical slice — exactly the work the repository's generator specs and audit rules were built to make safe.

Batch E carries one genuine differentiator: **clock injection** via `TimeProvider`. Nobody in the ecosystem does testable temporal validation well; after this batch every "in the past / in the future / minimum age" rule accepts a `TimeProvider`, and DataAnnotations resolves it from the `ValidationContext`.

### 1.2 Value and metrics

- +≈45 Core rules across six batches, each with Must/Guard/Fluent/DataAnnotations parity (Rule06) and codes (Rule13); all scopes stay 100 %/100 %.
- The README's rule count and catalogue are updated per batch; `gold-standard.md` re-verified per batch.
- Clock injection lands across four temporal families with a shared `FixedTimeProvider` test double.

## 2. Functional plan — conventions every batch follows

- **Vocabulary** (`docs/ai/specs/language/vocabulary.md`): Core `IsX`/`HasX`/`ContainsX`; Must positive name, `NotX` only for strict complements; Guard forbidden-state name (`NotX`, or a curated semantic opposite registered as an alias in `vocabulary.json`); Fluent = Must name unless it collides with a FluentValidation built-in (`naming-collisions.md`); DataAnnotations = `<MustName>Attribute` with a type suffix only where a family already uses one (`…DateOnlyAttribute`, `…NumberAttribute`, `…StringAttribute`).
- **Codes**: `MustCodes.<Domain>.<Aspect>.<Condition>` = `<domain>.<aspect>.<condition>` per the Plan 00 §5.4 grammar, domain map and condition vocabulary; the batch table proposes each code and the batch PR review is its curation checkpoint; a new domain (`token`, `version`, `cron`, `checksum`) is a new `MustCodes.<Domain>.cs` partial file in `src/PineGuard.Core/Codes/` with the Plan 01 §4.1 header comment; **any condition not already in Plan 00 §5.4's controlled vocabulary is added to that list in the same PR, before the code is written** (`MustCodesTests` asserts membership); Rule13 and `MustCodesTests` must be clean before the PR.
- **Nullability** (Rule07): reference values nullable, struct values non-nullable at Must/Guard; configuration parameters non-nullable unless the Core rule accepts null.
- **Fixtures** (`docs/ai/specs/testing/fixture.md`): one `XxxRulesFixtures` group per new Core method; boundary shape (four arrays) when the rule has numeric parameters or constants, format shape (two arrays) otherwise; edge constants reference the source `const`s; partials mirror source partials.
- **TFMs**: a rule that needs an API missing on `netstandard2.1` is gated exactly as `MustNumberClauses` is; the batch table says which.
- **Ordering** (Rule08): positive before `Not*` in Must/Fluent/DataAnnotations; Guard ordered by the Must clause it calls.
- **No renames** of existing members even where a batch sits beside an open item in `core-common-api-decisions.md` (e.g. `IsValidHostname` stays).
- **One deliberate signature change** (Batch E, pre-release so permitted by Plan 00 §4.6): inserting `TimeProvider? timeProvider = null` before `paramName`/`message` on every clock-reading member is source-compatible only for callers that never pass those positionally, and binary-breaking for all of them. Plan 00 §3.3's "additive only" claim carries this exception.

## 3. Batches

### Batch A — String content (`rules-string-content`)

| Layer | Members |
|---|---|
| Core `StringRules` (`src/PineGuard.Core/Rules/StringRules.cs`, appended after `ContainsDisallowed`) | `Contains(string? value, string substring, StringComparison comparison = StringComparison.Ordinal)`, `StartsWith(string? value, string prefix, StringComparison comparison = …)`, `EndsWith(string? value, string suffix, StringComparison comparison = …)` — `ThrowHelper.ThrowIfNull` on the configuration string; `null` value → `false`; empty needle → `true` (BCL semantics) |
| Must `MustStringClauses` | `Contains`, `NotContains`, `StartsWith`, `NotStartsWith`, `EndsWith`, `NotEndsWith` — null needle → failure attributed to `nameof(substring)` etc. (`spec.md` §5.4.2). `Must.Be.Contains(string, string)` does not collide with `MustCollectionClauses.Contains<T>(IEnumerable<T>, T)`: a `string` needle is not a `char` |
| Guard `GuardStringClauses` | `NotContains`/`Contains`, `NotStartsWith`/`StartsWith`, `NotEndsWith`/`EndsWith` |
| Fluent `FluentStringExtensions` | same six names (no FluentValidation built-in uses them) with `(string needle, StringComparison comparison = Ordinal, string? message = null)` |
| DataAnnotations `StringAttributes.cs` | `ContainsAttribute(string substring)`, `NotContainsAttribute`, `StartsWithAttribute(string prefix)`, `NotStartsWithAttribute`, `EndsWithAttribute(string suffix)`, `NotEndsWithAttribute` — each with `public StringComparison Comparison { get; init; } = StringComparison.Ordinal` |
| Fixtures | `StringRulesFixtures.Contains`, `.StartsWith`, `.EndsWith` — tuples `(string? value, string substring, StringComparison comparison)`; format shape |
| Codes | `text.content.not-contains` (for `Contains`), `…content.contains` (for `NotContains`), `…content.not-starts-with`, `…content.starts-with`, `…content.not-ends-with`, `…content.ends-with` — each clause's code is the state observed when it fails |

### Batch B — Identifiers, tokens and formats (`rules-identifiers`)

Already covered, no work: hostname (`NetworkRules.IsValidHostname` / `Must.Be.Hostname`), scheme-restricted HTTP URL (`UriRules.IsUrl`/`IsHttpUrl`/`IsHttpsUrl`), slug (`IdentifierRules.IsSlug`). Deliberately skipped (low priority in `competitive-analysis.md` §5): NanoId, CUID/CUID2, emoji, hash formats.

| Concept | Core | Must / Guard / Fluent / DA | Notes |
|---|---|---|---|
| ULID | `IdentifierRules.IsUlid(string? value)` — 26 chars, Crockford base32 (`0-9A-HJKMNP-TV-Z`, case-insensitive), first char `0–7` | `Ulid` / `NotUlid` / `Ulid()` / `[Ulid]` | fixture `IdentifierRulesFixtures.IsUlid` |
| GUID version | `GuidRules.HasVersion(Guid? value, int version)` (version nibble from `ToByteArray()[7] >> 4`, portable across TFMs; constants `GuidRules.MinVersion = 1`, `MaxVersion = 8`) and `StringRules.Guid.HasVersion(string? value, int version)` | `HasGuidVersion(value, version)` in `MustGuidClauses` and `MustStringGuidClauses` / `NotHasGuidVersion` / `HasGuidVersion(4)` / `[HasGuidVersion(4)]`, `[HasGuidVersionString(4)]` (predicate-shaped like `HasEmailAlias`; `GuidVersion` read as a noun) | `version` outside 1–8 → Must failure attributed to `nameof(version)`; boundary fixtures |
| JWT (shape only) | new `Rules/TokenRules.cs` `IsJwt(string? value)` + `Utils/TokenUtility.cs` `TryParseJwt(string? value, out string header, out string payload, out string signature)` — three non-empty Base64Url segments, header and payload decode to JSON objects (`JsonUtility`); **no** signature verification | new `MustTokenClauses.Jwt` / `NotJwt` / `Jwt()` / `[Jwt]` | fixture `TokenRulesFixtures` |
| SemVer 2.0.0 | new `Rules/VersionRules.cs` `IsSemVer(string? value)` — the official semver.org regex as a `public const string SemVerPattern` + `[GeneratedRegex]` (net8+) with the existing `new Regex(…, timeout)` fallback | new `MustVersionClauses.SemVer` / `NotSemVer` / `SemVer()` / `[SemVer]` | fixture `VersionRulesFixtures` |
| Cron | new `Common/CronFormat.cs` enum `{ Standard, WithSeconds }`; new `Rules/CronRules.cs` `IsCronExpression(string? value, CronFormat format = CronFormat.Standard)` + `Utils/CronUtility.cs` `TryParse` — 5 fields (6 with seconds), `*`, lists, ranges, steps, `JAN–DEC`/`SUN–SAT` names, field-range validation; no `@yearly`-style macros in v1 | new `MustCronClauses.CronExpression(value, format)` / `NotCronExpression` / `CronExpression(format)` / `[CronExpression]` (+ `Format` property) | fixture `CronRulesFixtures` |
| MAC address | `NetworkRules.IsMacAddress(string? value)` — `xx:xx:xx:xx:xx:xx`, `xx-xx-…`, `xxxx.xxxx.xxxx`; uppercase or lowercase hex | `MacAddress` in `MustNetworkClauses` / `NotMacAddress` / `MacAddress()` / `[MacAddress]` | fixture `NetworkRulesFixtures.IsMacAddress` |
| Media type | `HttpRules.IsMediaType(string? value)` — RFC 6838 `type/subtype`, optional `+suffix`, optional `; param=value` list, via `HttpContentTypeUtility.TryGetMediaType` plus a token check | `MediaType` in `MustHttpClauses` / `NotMediaType` / `MediaType()` / `[MediaType]` | fixture `HttpRulesFixtures.IsMediaType` |
| Regex pattern | `StringRules.IsRegexPattern(string? value)` via `StringUtility.TryCreateRegex(string? value, out Regex? regex)` — the one place a `try/catch ArgumentException` is justified (the BCL has no `Regex.TryParse`); uses the repo's standard match timeout | `RegexPattern` / `NotRegexPattern` / `RegexPattern()` / `[RegexPattern]` | fixture `StringRulesFixtures.IsRegexPattern` |
| Base64Url | `BufferRules.IsBase64Url(string? value)` — RFC 4648 §5 alphabet, padding optional | `Base64Url` / `NotBase64Url` / `Base64Url()` / `[Base64Url]` | fixture `BufferRulesFixtures.IsBase64Url` |
| UTF-8 bytes | `BufferRules.IsUtf8(byte[]? value)` — net8+: `System.Text.Unicode.Utf8.IsValid`; `netstandard2.1`: strict `UTF8Encoding(false, true)` decode in `BufferUtility.TryDecodeUtf8` | `Utf8` / `NotUtf8` / `Utf8()` (on `byte[]?`) / `[Utf8]` (`typeof(byte[])`) | fixture `BufferRulesFixtures.IsUtf8` |

Codes: `identifier.ulid.invalid`, `guid.version.mismatch` (shared by the `Guid` and string variants), `token.jwt.invalid`, `version.semver.invalid`, `cron.expression.invalid`, `network.mac.invalid`, `http.media-type.invalid`, `text.pattern.invalid`, `encoding.base64url.invalid`, `encoding.utf8.invalid`.

### Batch C — Text and Unicode correctness (`rules-unicode`)

| Concept | Core | Must / Guard / Fluent / DA | Notes |
|---|---|---|---|
| Byte-order mark | `StringRules.HasByteOrderMark(string? value)` (starts with U+FEFF) | `HasByteOrderMark` + `NotHasByteOrderMark` / Guard both `NotHasByteOrderMark` (via `Must.Be.HasByteOrderMark`) and `HasByteOrderMark` (via `NotHas…`) / Fluent both / DA both | the forbidden state most callers want is `Guard.Against.HasByteOrderMark` |
| Well-formed UTF-16 | `StringRules.IsWellFormedUtf16(string? value)` — no unpaired surrogates | `WellFormedUtf16` / `NotWellFormedUtf16` / `WellFormedUtf16()` / `[WellFormedUtf16]` | |
| Normalization | `StringRules.IsNormalized(string? value, NormalizationForm form = NormalizationForm.FormC)` | `Normalized(value, form)` / `NotNormalized` / `Normalized(form)` / `[Normalized]` (+ `Form` property) | **At risk**: under globalization-invariant mode `string.IsNormalized` throws `PlatformNotSupportedException` for non-ASCII input. Treat as a genuinely exceptional environment condition (`docs/ai/specs/must-clauses/project.md` non-negotiables) and let it bubble; test on CI first — if the CI image runs invariant mode, drop this row from the batch and record why |
| Grapheme counts | new partial `Rules/StringRules.Graphemes.cs` with nested `StringRules.Graphemes`: `HasExactCount(string? value, int count)`, `HasMinCount(value, int min)`, `HasMaxCount(value, int max)`, `HasCountBetween(value, int min, int max, Inclusion inclusion = Inclusive)` — via `Utils/StringUtility.Graphemes.cs` `TryGetCount(string? value, out int count)` (`StringInfo`); mirrors `CollectionRules` naming inside the sub-scope | new `MustStringGraphemesClauses`: `HasExactGraphemeCount`, `NotHasExactGraphemeCount`, `HasMinGraphemeCount`, `NotHas…`, `HasMaxGraphemeCount`, `NotHas…`, `HasGraphemeCountBetween`, `NotHas…` / Guard eight (`NotHasExactGraphemeCount` via `HasExact…`, etc.) / Fluent eight / DA eight (`HasExactGraphemeCountAttribute` …) | fixture partial `StringRulesFixtures.Graphemes.cs` with inner classes prefixed `Graphemes…` (collision rule, `fixture.md` §10); scenarios include ZWJ emoji sequences, combining marks, CRLF (one grapheme), surrogate pairs; boundary shape; the README explains the emoji-length bug in one sentence |

Codes: `text.bom.missing` (for `HasByteOrderMark`) / `…bom.present` (for `NotHas…`); `text.unicode.malformed` / `…unicode.well-formed`; `text.unicode.not-normalized` / `…unicode.normalized`; `text.graphemes.mismatch`, `…graphemes.too-few`, `…graphemes.too-many`, `…graphemes.out-of-range` for the positive clauses, with the exact inverse state for each `NotHas*` complement (curated in the batch).

`IsPrintable` is not added: `NotContainsControlChars` already covers it.

### Batch D — Numeric and financial semantics (`rules-numeric`)

| Concept | Core | Must / Guard / Fluent / DA | Notes |
|---|---|---|---|
| Decimal scale / precision | new `Rules/DecimalRules.cs` (non-generic → available on `netstandard2.1`): `HasMaxScale(decimal? value, int scale)`, `HasMaxPrecision(decimal? value, int precision)`, `IsWithinPrecision(decimal? value, int precision, int scale)`; constants `MaxPrecision = 29`, `MaxScale = 28`; via `Utils/DecimalUtility.cs` `TryGetPrecisionAndScale(decimal value, out int precision, out int scale)` on the **value** (trailing zeros ignored: `1.500m` has scale 1) | new `MustDecimalClauses`: `ScaleAtMost(value, scale)`, `PrecisionAtMost(value, precision)`, `WithinPrecision(value, precision, scale)` / Guard `ScaleAbove`, `PrecisionAbove`, `NotWithinPrecision` / Fluent `ScaleAtMost(2)`, `PrecisionAtMost(18)`, `WithinPrecision(18, 2)` / DA `[ScaleAtMost(2)]`, `[PrecisionAtMost(18)]`, `[WithinPrecision(18, 2)]` (`typeof(decimal)`) | `vocabulary.json` aliases `ScaleAbove → ScaleAtMost`, `PrecisionAbove → PrecisionAtMost`; configuration values outside `0..MaxScale` / `1..MaxPrecision` → failure attributed to the parameter; boundary fixtures `DecimalRulesFixtures` |
| Percentage | `NumberRules.IsPercentage<T>(T? value) where T : struct, INumber<T>` (0 ≤ v ≤ 100) and `StringRules.Numbers.IsPercentage` | `Percentage` in `MustNumberClauses` and `MustStringNumbersClauses` / `NotPercentage` / `Percentage()` / `[PercentageNumber]`, `[PercentageString]` | net8+ gated like the rest of the numeric family; boundary fixtures at `0` and `100` |
| Luhn | new `Rules/ChecksumRules.cs` `IsLuhn(string? value)` + `Utils/ChecksumUtility.cs` `IsLuhn(ReadOnlySpan<char> digits)`; input normalised with the existing `StringUtility.TryParseDigits` (strips `' '`/`'-'`); minimum two digits | new `MustChecksumClauses.Luhn` / `NotLuhn` / `Luhn()` / `[Luhn]` | the README states this is the algorithm behind `[CreditCard]`, without claiming card semantics (payment-instrument validation left with the v2 standards prune); fixture `ChecksumRulesFixtures` |

Codes: `number.scale.exceeded`, `number.precision.exceeded`, `number.precision.out-of-range`; `number.range.not-percentage` (shared by the numeric and string variants); `checksum.luhn.invalid`.

### Batch E — Temporal semantics and clock injection (`rules-temporal`)

**Clock injection (cross-cutting for this batch).**

- `src/PineGuard.Core/PineGuard.Core.csproj`: `<PackageReference Include="Microsoft.Bcl.TimeProvider" Condition="'$(TargetFramework)' == 'netstandard2.1'" />` (+ `PackageVersion`). It is a Microsoft first-party BCL package, consistent with Core's "first-party BCL packages only" policy; the Core README's dependency sentence and the root README's dependency note are updated. `TimeProvider` is in-box on net8+.
- `Utils/DateTimeUtility.cs`: `GetUtcNow(TimeProvider? timeProvider)` → `(timeProvider ?? TimeProvider.System).GetUtcNow()`; every `DateTime.UtcNow` / `DateTimeOffset.UtcNow` in `Rules/` and `Utils/` is replaced by it (`grep -rn "UtcNow" src/PineGuard.Core` is the work list — today `DateTimeRules` ×3, `DateOnlyRules` ×2, `DateTimeOffsetRules` ×2, plus any string variants).
- Every rule that reads the clock gains a trailing `TimeProvider? timeProvider = null` parameter: `IsInPast`, `IsInFuture`, `IsWithinDaysFromNow` on `DateTimeRules`, `DateTimeOffsetRules`, `DateOnlyRules`, and the `StringRules.DateOnly` / `StringRules.DateTimeOffset` counterparts.
- Every Must clause that reads the clock (`Past`, `PastOrPresent`, `Future`, `FutureOrPresent`, `WithinDaysFromNow`, `NotWithinDaysFromNow` across the typed and `String*` families) gains `TimeProvider? timeProvider = null` **immediately before `paramName`**; Guard gains it before `message`; Fluent gains it before `message`; DataAnnotations attributes cannot take it in a constructor and instead resolve `validationContext.GetService(typeof(TimeProvider)) as TimeProvider` (null → system). Message templates are unchanged. **This is scripted, like Plan 01 W6b**: enumerate the affected members (`grep -rn "UtcNow\|timeProvider" src`) and every call site that passes `paramName`/`message` positionally (`tests/**` included), insert the parameter, rewrite positional callers to named arguments, and state the member and test-file counts in the PR body; Rule07/Rule08 re-run afterwards, and a Rule08 assertion that `paramName` stays the last parameter is added.
- Test double: `+ tests/PineGuard.Testing/Common/FixedTimeProvider.cs` (ships in the **published** `PineGuard.Testing` package: it gets its own `+ tests/PineGuard.Testing.UnitTests/Common/FixedTimeProviderTests.cs` + `…TestData.cs` — ctor, `GetUtcNow`, `Default` — a README line, and a Plan 00 §5.3 canon row) — `public sealed class FixedTimeProvider(DateTimeOffset utcNow) : TimeProvider` overriding `GetUtcNow()`, plus `public static readonly FixedTimeProvider Default = new(new DateTimeOffset(2026, 6, 15, 12, 0, 0, TimeSpan.Zero))`. Shared by all five layer test projects (the ≥2-projects rule). Existing far-past/far-future scenarios stay; each temporal group gains deterministic scenarios that pass `FixedTimeProvider.Default`, and the DataAnnotations tests build a `ValidationContext` whose service provider returns it.
- Rule07: `TimeProvider?` is a nullable secondary parameter, permitted because the Core rule accepts null. Rule01 naming: the parameter is `timeProvider` everywhere.

| Concept | Core | Must / Guard / Fluent / DA | Notes |
|---|---|---|---|
| Calendar predicates on the date-only types (open decision C-2, additive so allowed) | `DateOnlyRules` and `DateTimeOffsetRules`: `IsWeekday`, `IsWeekend`, `IsFirstDayOfMonth`, `IsLastDayOfMonth` | `Weekday`, `Weekend`, `FirstDayOfMonth`, `NotFirstDayOfMonth`, `LastDayOfMonth`, `NotLastDayOfMonth` on both types (mirroring the existing `DateTime` set) / Guard mirrors `GuardDateTimeClauses` / Fluent mirrors / DA `WeekdayDateOnlyAttribute` … and `…DateTimeOffsetAttribute` | fixtures in `DateOnlyRulesFixtures`, `DateTimeOffsetRulesFixtures` |
| Minimum age | `DateOnlyRules.HasMinimumAge(DateOnly? value, int years, TimeProvider? timeProvider = null)` and `DateTimeRules.HasMinimumAge(DateTime? value, int years, TimeProvider? …)` — `value` is the date of birth; true when `today.AddYears(-years) >= value` (documented: a 29-Feb birthday matures on 28-Feb in non-leap years, which is what `AddYears` does); `years < 0` is a configuration error; `StringRules.DateOnly.HasMinimumAge` string variant | `MinimumAge(value, years)` in `MustDateOnlyClauses`, `MustDateTimeClauses`, `MustStringDateOnlyClauses` / Guard `BelowMinimumAge(value, years)` (alias `BelowMinimumAge → MinimumAge`; mirrors the code `date.age.below-minimum`; `UnderAge` rejected as legally loaded and not a state of the value) / Fluent `MinimumAge(18)` / DA `[MinimumAge(18)]` (`DateOnly`), `[MinimumAgeDateTime(18)]`, `[MinimumAgeString(18)]` | boundary fixtures with `(DateOnly value, int years)` tuples; tests inject `FixedTimeProvider.Default` |

Codes: `date.calendar.not-weekday`, `…calendar.not-weekend`, `…calendar.not-first-day-of-month`, `…calendar.not-last-day-of-month` (the new `DateOnly`/`DateTimeOffset` clauses share the existing `DateTime` constants); `date.age.below-minimum` (shared by the `DateOnly`, `DateTime` and string variants).

### Batch F — File signatures (`rules-file-signature`)

| Layer | Members |
|---|---|
| Core `Utils/FileSignatureUtility.cs` | a static, readonly signature table (`extension` → byte pattern(s) + offset) for `.png`, `.jpg`/`.jpeg`, `.gif`, `.bmp`, `.webp` (`RIFF….WEBP`), `.tiff`, `.ico`, `.pdf`, `.zip` (also `.docx`/`.xlsx`/`.pptx`), `.gz`, `.7z`, `.rar`, `.mp3` (`ID3`), `.mp4` (`ftyp` at offset 4); `KnownExtensions` (`IReadOnlyCollection<string>`), `MaxSignatureLength` (`const int`), `TryDetectExtension(byte[]? header, out string? extension)` (first match, lowercase, with dot), `IsKnownExtension(string? extension)` |
| Core `Rules/FileSignatureRules.cs` | `HasSignature(byte[]? value, string extension)` — `true` when the header matches the signature registered for `extension` (case-insensitive, dot optional); unknown extension → `ArgumentException` (configuration parameter); `HasKnownSignature(byte[]? value)`. Pure: bytes in, bool out — reading the file is the caller's job |
| Must `MustFileSignatureClauses` | `FileSignature(value, extension)` (unknown extension → failure attributed to `nameof(extension)`), `KnownFileSignature(value)` |
| Guard | `NotFileSignature`, `NotKnownFileSignature` |
| Fluent `FluentFileSignatureExtensions` | `FileSignature(".png")`, `KnownFileSignature()` on `byte[]?` |
| DataAnnotations `FileSignatureAttributes.cs` | `FileSignatureAttribute(string extension)`, `KnownFileSignatureAttribute` (`typeof(byte[])`) |
| Fixtures | `FileSignatureRulesFixtures` — a header byte array per format (format shape), plus truncated, empty and spoofed (`.png` extension with JPEG bytes) scenarios |
| Codes | `file.signature.mismatch` (does not match the declared extension), `file.signature.unknown` |

## 4. Testing plan (per batch)

- Fixtures first, then Core, Must, Guard, Fluent, DataAnnotations TestData/Tests — each layer's addendum (`docs/ai/specs/<layer>/unit-test.md`) governs dataset shape; Must groups assert `Code` in at least one `InvalidCases` factory.
- Coverage: `-Scope Core`, `MustClauses`, `GuardClauses`, `FluentValidation`, `DataAnnotations` each 100/100 on both `-Framework`s; then `-Scope All` 100/100 (for Batch E, `-Scope All` also covers `FixedTimeProvider` — `-Scope Testing` runs every test project too, so run one of them, not both).
- Audit: `Run-All.ps1 -RuleId Rule06,Rule07,Rule08,Rule13,Rule50` clean; `vocabulary.json` aliases added where the batch table says so.
- Determinism (`unit-test.md` §7): Batch E tests never read the real clock except in the pre-existing far-past/far-future scenarios; Batch C grapheme scenarios are chosen to segment identically on net8.0 and net10.0 (both ICU-backed) — if a case differs between TFMs, replace the case rather than gate the test.
- `docs/ai/specs/testing/gold-standard.md` counts and the dated verification section are refreshed in every batch PR.

## 5. Playbook (per batch)

**W0** Plan 00 §6 with the batch slug; read `docs/ai/agents/scaffold-vertical-slice.md`, `docs/ai/skills/new-validation/SKILL.md`, the per-layer specs it routes to, `docs/ai/specs/language/vocabulary.md`, `docs/ai/specs/language/naming-collisions.md`; baseline gates.

**W1** Utils + Rules + fixtures for every concept in the batch table; Core tests; `-Scope Core` 100/100. Commit `feat(core): add <batch> rules`.

**W2** Must clauses (+ `MustCodes` constants, Rule13); Must tests; `-Scope MustClauses`. Commit `feat(must): add <batch> clauses`.

**W3** Guard clauses (+ `vocabulary.json` aliases); Guard tests; `-Scope GuardClauses`. Commit `feat(guard): …`.

**W4** Fluent extensions (+ codes); tests; `-Scope FluentValidation`. Commit `feat(fluent): …`.

**W5** DataAnnotations attributes; tests; `-Scope DataAnnotations`. Commit `feat(annotations): …`.

**W6** Batch E only — and it runs **first**, as W0.5, not after W1–W5: the `Microsoft.Bcl.TimeProvider` reference (Tier 1 edit of `Directory.Packages.props` and Core csproj — state the lines first), `FixedTimeProvider` + its tests, `DateTimeUtility.GetUtcNow` and the `UtcNow` sweep, then the parameter additions happen inside each layer's own W1–W5 so no temporal member is written twice.

**W7** README rule catalogue/counts, per-layer package READMEs where a family is new (`Cron`, `Token`, `Version`, `Checksum`, `Decimal`, `FileSignature`), `gold-standard.md`; `Run-All.ps1 -RuleId Rule06,Rule07,Rule08,Rule11,Rule13,Rule50`. Commit `docs(brain): record <batch> in the rule catalogue and gold standard`.

**W8** Plan 00 §7; `git merge origin/main` (expect `MustCodes.<Domain>.cs`/`vocabulary.json`/`gold-standard.md` conflicts from sibling batches — resolve by union); PR; merge; cleanup.

## 6. Definition of Done (per batch)

Plan 00 §7, plus: every concept row in the batch's table exists in all five layers with tests; Rule06/07/08/13 clean; codes registered; README counts updated; Batch C's at-risk row either delivered or explicitly dropped with the CI evidence in the PR body; Batch E's `FixedTimeProvider` is used by at least Core, Must and DataAnnotations tests and has its own tests in `PineGuard.Testing.UnitTests`; every new condition is in Plan 00 §5.4's vocabulary in the same PR.

## 7. Risks

| Risk | Mitigation |
|---|---|
| Sibling batches conflict in shared files | Small, well-known set; merge `origin/main` before the PR; union resolution |
| Grapheme segmentation differs across runtimes | Choose cases stable on both CI TFMs; document that segmentation follows the host runtime's Unicode tables |
| `string.IsNormalized` under invariant globalization | At-risk row with an explicit drop criterion |
| `Microsoft.Bcl.TimeProvider` seen as a "new dependency" for Core | First-party BCL package, `netstandard2.1`-only; README sentence updated; the alternative (`DateTime now` overloads) would fork the temporal API per TFM |
| Cron dialect arguments | v1 scope is explicit (5/6 fields, names, no macros); the enum leaves room |

## 8. Out of scope

ISO/standards-registry validations (separate repository), NanoId/CUID/emoji/hash formats, IBAN/VAT, JSON Schema/XSD, transforms/coercion, any rename from `core-common-api-decisions.md`.

<!-- footer
last_verified: 2026-08-26
-->

<!-- plan-nav -->
> [Parent](new-surfaces-missing-validation-cases.md) · [00 Program](new-surfaces-missing-validation-cases-00-program.md) · [01 Structural validation](new-surfaces-missing-validation-cases-01-structural-validation.md) · [02 Options](new-surfaces-missing-validation-cases-02-options.md) · [03 ASP.NET Core](new-surfaces-missing-validation-cases-03-aspnetcore.md) · [04 MediatR & bridges](new-surfaces-missing-validation-cases-04-mediatr-result-bridges.md) · **05 Rule batches** · [06 Analyzers](new-surfaces-missing-validation-cases-06-analyzers.md)
<!-- /plan-nav -->
