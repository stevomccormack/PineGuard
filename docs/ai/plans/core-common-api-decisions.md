<!-- metadata_header
type: plan
id: core-common-api-decisions
version: 1.0
status: open
last_updated: 2026-08-25
-->

# Core / Common API Decisions

> **Status**: Open - 36 decisions awaiting an owner's call.
> **Origin**: The core/common adversarial review (merged as `206bbc4`, 2026-08-23).
> **Scope**: `PineGuard.Core`, plus `PineGuard.DataAnnotations/Common` and `PineGuard.FluentValidation/Common`.

## Why this document exists

The core/common review raised 174 findings across 116 files. 170 survived independent
adversarial verification, and the 134 of those with an unambiguous correct answer were fixed
and merged in `206bbc4`.

The 36 items below are the remainder. They were **deliberately not fixed**, because each one
has more than one defensible answer and the choice belongs to whoever owns the public API,
not to a review agent. Most are breaking changes; several are judgement calls about what the
library should mean rather than bugs in what it currently does.

They are recorded here rather than left in a scratch file so the reasoning survives.

## How to read an entry

Every entry states the defect, a concrete consequence, and the options. The **consequence**
lines are reproducible: each was traced through the real code by a verifier whose brief was to
refute the finding.

Severity reflects impact *if the current behaviour is wrong*. It is not a claim that the
current behaviour is wrong, since that is precisely what is being asked.

## Status against current `main`

Re-verified on 2026-08-25 against `55eecd7`. All 36 still apply, with one amendment:

- **G-1** is now *partially* resolved. The remediation removed the `netstandard2.1` exclusion
  from `GeoLocationRules.cs`, but `StringRules.Numbers.cs` and `StringRules.GeoLocation.cs`
  remain gated behind `#if NET8_0_OR_GREATER`, so the gap persists for those two files.

Line numbers below are current as of `55eecd7`.

---

## A. Breaking renames - naming consistency

Each of these is a rename that would break source compatibility for consumers. They are worth doing together, in one major version, or not at all - dribbling them out one release at a time is the worst option.

### A-1. IsHexString/IsBase64String vs BufferRules.IsHex/IsBase64: same predicate, 'String' suffix only in Utils

**`src/PineGuard.Core/Utils/BufferUtility.cs`:23** - severity **minor** - `naming`

BufferRules exposes IsHex(string?) and IsBase64(string?) which delegate to BufferUtility.IsHexString(string?) and IsBase64String(string?). The identical concept carries a 'String' suffix in the Utils layer only; no other Rules/Utils pair renames the predicate between layers (e.g. NetworkRules.IsInCidr delegates to NetworkUtility.IsInCidr).

**Consequence.** A user working at the Utils layer types BufferUtility.IsHex(s) by analogy with BufferRules.IsHex and gets CS0117; grep for 'IsBase64' finds two names for one check.

**Options.** Align the names (IsHex/IsBase64 in both layers) since the parameter type already says 'string'.

### A-2. IsValidHostname is the only 'IsValidX' name on the entire Rules surface

**`src/PineGuard.Core/Rules/NetworkRules.cs`:93** - severity **minor** - `naming`

Every other predicate uses bare IsX: IsIpAddress, IsIpv4, IsIpv6, IsPortNumber (same file), IsEmail, IsUrl, IsGuid, IsJson, IsXml, IsPhoneNumber. IsValidHostname alone inserts 'Valid', implying siblings are somehow less strict validity checks when they are the same kind of predicate.

**Consequence.** A user types NetworkRules.IsHostname("example.com") by analogy with IsIpAddress/IsUrl and gets CS0117; conversely a user hunting for IsValidEmail / IsValidUrl finds nothing.

**Options.** Rename to IsHostname (keeping an [Obsolete] forwarder if surface stability requires it).

### A-3. IsInRange vs IsBetween: same concept named differently across families

**`src/PineGuard.Core/Rules/NumberRules.cs`:116** - severity **minor** - `naming`

NumberRules.IsInRange(value, min, max, inclusion) and StringRules.Numbers.IsInRange / IsInt32InRange / IsInt64InRange use 'InRange', while DateOnlyRules, DateTimeRules, DateTimeOffsetRules, TimeOnlyRules, StringRules.DateOnly, StringRules.DateTimeOffset, and StringRules.TimeOnly all use IsBetween(value, min, max, inclusion) for the identical (value, min, max, Inclusion=Inclusive) shape. StringRules also has IsLengthBetween and CollectionRules HasCountBetween and TimeSpanRules IsDurationBetween, making 'Between' the majority term (10+ members) versus 4 'InRange' members.

**Consequence.** A user types NumberRules.IsBetween(x, 1, 10) by analogy with DateOnlyRules.IsBetween and gets CS0117 (no such member), then must discover the numeric family alone calls it IsInRange.

**Options.** Add IsBetween to the numeric families (or rename IsInRange to IsBetween) so the range predicate has one name library-wide.

### A-4. ToCase overloads are pure pass-through duplicates of TryToCase with a convention-violating name

**`src/PineGuard.Core/Utils/StringUtility.Casing.cs`:54** - severity **minor** - `naming`

Three ToCase overloads (lines 54, 68, 81) each forward directly to TryToCase with identical signatures and bool+out fallible semantics. The naming convention reserves ToX for transformations and Try* for fallible operations; publishing both names for the same behavior doubles the surface and leaves callers unsure which is canonical (and whether ToCase throws where TryToCase does not — it doesn't).

**Consequence.** IntelliSense shows both ToCase and TryToCase with the same parameters; a user assumes ToCase is the throwing/infallible variant (per BCL ToX conventions) and writes error handling that never triggers, or asks which to use in review.

**Options.** Delete the ToCase pass-throughs (or make ToCase a genuine string-returning transformation) and keep only TryToCase, adding a single-style TryToCase(value, style, out cased) convenience overload.

### A-5. Contains/ContainsNullItems break the Rules IsX/HasX naming convention and have no Nullable<T> counterpart

**`src/PineGuard.Core/Rules/CollectionRules.cs`:263** - severity **info** - `api-consistency`

The spec reserves `ContainsX` for Utils; Rules methods are IsX/HasX (DictionaryRules uses HasKey/HasValue for the same semantic). CollectionRules.Contains and ContainsNullItems are the only Contains-prefixed Rules methods in this chunk. Additionally ContainsNullItems is constrained `where T : class`, so a collection of nullable value types (IEnumerable<int?>) cannot be checked for null items at all.

**Consequence.** Naming inconsistency only: a consumer looking for HasItem/HasNullItems by convention finds Contains-prefixed methods instead; IEnumerable<int?> null-item validation has no API.

**Options.** Consider HasItem/HasNullItems aliases (or a documented exemption) and a `where T : struct` overload for Nullable<T> elements.

### A-6. StringRules.Guid.IsNotEmpty means 'parses to a non-empty Guid', unlike every other IsNotEmpty, and has no IsEmpty twin

**`src/PineGuard.Core/Rules/StringRules.Guid.cs`:32** - severity **info** - `naming`

Everywhere else IsNotEmpty checks emptiness of the value itself (CollectionRules.IsNotEmpty, DictionaryRules.IsNotEmpty, GuidRules.IsNotEmpty). StringRules.Guid.IsNotEmpty(string? value) instead means 'parses as a Guid AND is not Guid.Empty' — a non-empty string like "abc" returns false. It also breaks the family symmetry: GuidRules has the IsEmpty/IsNotEmpty pair, StringRules.Guid has only IsNotEmpty (alongside IsGuid).

**Consequence.** StringRules.Guid.IsNotEmpty("abc") returns false even though the string is plainly not empty — a reader skimming call sites (or a user picking the method from IntelliSense inside StringRules) assumes it is a string-emptiness check; and StringRules.Guid.IsEmpty(s) does not exist for the inverse validation.

**Options.** Rename to IsNonEmptyGuid (or IsGuidNotEmpty) and add the IsEmpty counterpart if the pair is wanted.

### A-7. NotContainsControlChars is the only 'NotX' verb-negation on the surface

**`src/PineGuard.Core/Rules/StringRules.cs`:338** - severity **info** - `naming`

The spec allows positive names with explicit exemptions (IsNotNull, IsNotEmpty, IsNotZero — all 'IsNot*'). 'NotContainsControlChars' is a unique grammatical form: negations elsewhere are expressed as IsNot* (GuidRules.IsNotEmpty) or as safe-predicates (OwaspRules.IsCrLfSafe wraps !ContainsCrLfRisk). No other Contains* method has a Not twin (ContainsWhitespace, ContainsNullItems, ContainsDisallowed do not).

**Consequence.** A user looking for the negation of ContainsWhitespace finds none and, by analogy with this member, guesses NotContainsWhitespace (CS0117); a user looking for the negation of ContainsControlChars searches 'IsFreeOfControlChars'/'HasNoControlChars' and misses NotContainsControlChars in IntelliSense's alphabetical list.

**Options.** Rename to something matching an existing pattern (e.g. IsControlCharSafe, mirroring OwaspRules.Is*Safe) or drop it since it is just !ContainsControlChars plus a null-handling twist worth documenting.

### A-8. IsDurationBetween breaks the IsBetween naming pattern used by every other temporal rule class

**`src/PineGuard.Core/Rules/TimeSpanRules.cs`:19** - severity **info** - `api-consistency`

DateOnlyRules, DateTimeRules, DateTimeOffsetRules, and TimeOnlyRules all expose IsBetween(value, min, max, inclusion). TimeSpanRules alone names the identical operation IsDurationBetween. Its siblings IsGreaterThan/IsLessThan carry no 'Duration' prefix, so the prefix is inconsistent even within the file.

**Consequence.** No behavioral impact; discoverability inconsistency only — a consumer typing TimeSpanRules.IsBetween finds nothing and must guess the Duration prefix.

**Options.** Rename to IsBetween (or add an IsBetween alias and obsolete the old name) for cross-class symmetry.


## B. API shape - parameter order, naming and nullability

Signature-level inconsistencies. Some are source-breaking (parameter renames break named arguments; overload changes break `out var`), so they belong with theme A in a major version.

### B-1. IsCsvLine omits the separator parameter that every sibling CSV rule exposes

**`src/PineGuard.Core/Rules/CsvRules.cs`:22** - severity **minor** - `api-consistency`

CsvRules.IsCsvHeaderLine and both IsCsvRowLine overloads take char separator = DefaultCsvSeparator and forward it, but IsCsvLine hard-codes the CsvUtility.TryParseCsvLine default (','), even though the underlying utility accepts a separator. A caller validating semicolon-separated files can validate headers and rows but not bare lines.

**Consequence.** CsvRules.IsCsvLine("a;\"b;c\";d") for a semicolon-delimited file — caller has no way to pass ';', and the quoted field parses differently than intended (quote after non-empty builder -> false), while CsvRules.IsCsvHeaderLine handles the same file correctly via its separator parameter.

**Options.** Add char separator = DefaultCsvSeparator to IsCsvLine and forward it.

### B-2. Ok and Fail factories order value/paramName oppositely

**`src/PineGuard.Core/MustClauses/MustResult.cs`:110** - severity **minor** - `api-consistency`

Ok is `Ok(T? result, object? value = null, string? paramName = null)` (value before paramName) while Fail is `Fail(string messageTemplate, string? paramName, object? value)` (paramName before value). FromBool follows Fail's order (`..., paramName, value, result`). Within the same type, positional callers of the two factories must remember opposite orderings of two parameters that are both object?/string? adjacent, and value/paramName are both frequently-null string-ish arguments, so a swap compiles when value is a string.

**Consequence.** A Must clause author writes `MustResult<string>.Ok(parsed, paramName, value)` by analogy with the Fail call two lines above; because both arguments are strings it compiles, and every success result now reports the validated value as its ParamName (e.g. ParamName == "admin@example.com") and the parameter name as its Value.

**Options.** Align the factory signatures (e.g. make Fail's tail `value, paramName` to match Ok, or vice versa) before the public surface is frozen.

### B-3. Validated parameter named latitude/longitude instead of value; no styles parameter unlike sibling string rules

**`src/PineGuard.Core/Rules/StringRules.GeoLocation.cs`:20** - severity **minor** - `api-consistency`

The repo invariant states the validated input parameter must be named `value`. StringRules.Numbers and StringRules.NumberTypes comply, but StringRules.GeoLocation.IsLatitude(string? latitude) and IsLongitude(string? longitude) do not (IsGeoLocation's two-parameter case is a legitimate exception). This matters for named-argument callers and for Must/Guard layers that mirror parameter names. Secondary inconsistency in the same class: every rule in StringRules.Numbers/NumberTypes exposes a NumberStyles styles parameter, but the GeoLocation rules hard-code the default styles, so e.g. exponent notation ("9e1" = 90) or thousands separators can never be opted into for coordinates while they can for every other numeric string rule.

**Consequence.** StringRules.Numbers.IsPositive(value: s) compiles while StringRules.GeoLocation.IsLatitude(value: s) is a CS1739 compile error; and IsLatitude("9e1") is false with no way to opt in, while IsPositive("9e1", NumberStyles.Float) is true.

**Options.** Rename the single-input parameters to value (keeping latitude/longitude only on the pair overload), and either add the styles parameter or document that coordinate strings must be plain fixed-point notation.

### B-4. Guid.TryParse has dual overloads differing only in out-parameter nullability, breaking `out var`

**`src/PineGuard.Core/Utils/StringUtility.Guid.cs`:52** - severity **minor** - `api-consistency`

StringUtility.Guid exposes TryParse(string?, out System.Guid? guid) and TryParse(string?, out System.Guid guid). Every other nested StringUtility parser (DateOnly, DateTimeOffset, TimeOnly, TimeSpan, Bool) exposes exactly one overload with a nullable out. Overloading purely on out-type nullability means the idiomatic call `StringUtility.Guid.TryParse(s, out var guid)` fails with CS8130 (cannot infer the type of implicitly-typed out variable), a failure mode no sibling parser has.

**Consequence.** StringUtility.Guid.TryParse(input, out var guid) — compiles fine for StringUtility.DateOnly.TryParse and every sibling, but fails with CS8130 here; the user must write an explicit out type, and callers copy-pasting from sibling usage break.

**Options.** Keep only the nullable-out overload for parity with siblings (StringRules.Guid.IsGuid can use System.Guid.TryParse directly), or give the non-nullable variant a distinct name such as TryParseExact.

### B-5. TryTruncateToPrecision out parameter is non-nullable TimeOnly while all DateTimeUtility counterparts use nullable outs

**`src/PineGuard.Core/Utils/TimeOnlyUtility.cs`:19** - severity **minor** - `api-consistency`

DateTimeUtility.TryTruncateToPrecisionUtc(DateTime?, ..., out DateTime?), (DateTimeOffset?, ..., out DateTimeOffset?), and TryTruncateToPrecision(DateOnly?, ..., out DateOnly?) all declare nullable out parameters, forcing callers into 't!.Value' dances (see DateOnlyRules lines 65-66, DateTimeRules lines 66-67). The TimeOnly overload alone uses out TimeOnly (non-nullable, default on failure), letting TimeOnlyRules consume the result directly (lines 47-48). Same family of parallel Try* APIs, two different out-param shapes.

**Consequence.** No wrong verdict; the inconsistency shows up as API friction: a consumer switching from the TimeOnly overload to the DateOnly overload must add null-forgiveness/Value handling for an out param that is never null when the method returns true.

**Options.** Standardize on non-nullable outs with default-on-failure (the conventional BCL Try* shape), updating the DateTime/DateTimeOffset/DateOnly overloads and their call sites.

### B-6. Comparison-bound parameter naming and nullability drift: threshold (TimeSpan?) vs min/max (T) vs other (T?)

**`src/PineGuard.Core/Rules/TimeSpanRules.cs`:29** - severity **minor** - `api-consistency`

For the same 'compare value to a bound' concept: NumberRules.IsGreaterThan names the bound 'min' (non-nullable T) and IsLessThan names it 'max'; TimeSpanRules and StringRules.TimeSpan name it 'threshold' (nullable TimeSpan? in TimeSpanRules, non-nullable System.TimeSpan in StringRules.TimeSpan — drift even between those two adjacent files); date families name it 'other' (nullable). Named-argument call sites and null-passing behavior therefore differ per family: TimeSpanRules.IsGreaterThan(v, null) compiles and returns false, while StringRules.TimeSpan.IsGreaterThan(v, null) does not compile.

**Consequence.** A user writes NumberRules.IsGreaterThan(x, threshold: 5) after using TimeSpanRules and gets CS1739 (parameter is named 'min'); a helper generic over 'nullable bound returns false' behavior works with TimeSpanRules but fails to compile against StringRules.TimeSpan.

**Options.** Pick one bound name (threshold or other) and one nullability policy for bounds across families.

### B-7. IsInCidr parameter naming and nullability diverge from the chunk's conventions

**`src/PineGuard.Core/Rules/NetworkRules.cs`:68** - severity **info** - `api-consistency`

Every other rule in this chunk names the validated input 'value' (per the repo invariant) and annotates it nullable; IsInCidr uses 'ip' (string?) and 'cidr' declared as non-nullable string even though the implementation tolerates null (TryParseCidr takes string? and returns false). A caller with NRT disabled passing a null cidr silently gets false rather than the annotation contract implying non-null; siblings would have been annotated string?.

**Consequence.** API consumers see 'string cidr' and assume null is invalid to pass (compiler warning), while ip is 'string?' — inconsistent surface within one class; renaming later would be a breaking change for named-argument callers.

**Options.** Declare cidr as string? and consider value/cidr naming alignment before the surface stabilizes.

### B-8. Inconsistent overload usage within StringRules.Guid and asymmetric nullable-out overload availability across the parsing utilities

**`src/PineGuard.Core/Rules/StringRules.Guid.cs`:33** - severity **info** - `api-consistency`

IsGuid (line 25) calls the non-nullable overload StringUtility.Guid.TryParse(value, out System.Guid _), while IsNotEmpty (line 33) calls the nullable overload and then routes through GuidRules.IsNotEmpty(Guid?) — the nullable box is pointless because TryParse==true guarantees non-null. Separately, StringUtility.Guid is the only utility in this chunk offering both nullable and non-nullable out overloads; StringUtility.DateOnly/TimeOnly/DateTimeOffset/TimeSpan expose only 'out T?', forcing every Rules delegate through the nullable path. No behavioral impact, but the surface is uneven for consumers.

**Consequence.** No wrong verdict; inconsistency only: a consumer of StringUtility.DateOnly must handle 'DateOnly?' where a consumer of StringUtility.Guid can choose 'Guid', and the two methods inside StringRules.Guid model the identical parse step two different ways.

**Options.** Have IsNotEmpty use the non-nullable overload (TryParse(value, out System.Guid parsed) && GuidRules.IsNotEmpty(parsed)), and consider adding non-nullable out overloads to the other parsing utilities for symmetry.


## C. Missing capability - real gaps in what can be expressed

These are not inconsistencies but genuine holes: things a user will reasonably try to validate and cannot. Additive, so they can ship in a minor version.

### C-1. Duration is inclusive day count (End-Start+1 days) while sibling range types use End-Start

**`src/PineGuard.Core/Common/DateOnlyRange.cs`:66** - severity **minor** - `api-consistency`

DateOnlyRange.Duration = TimeSpan.FromDays(DayCount) where DayCount includes both endpoints, so a single-day range has Duration 1.00:00:00 and [Jan1, Jan2] has 2 days. DateTimeRange/DateTimeOffsetRange/TimeOnlyRange all define Duration = End - Start (a degenerate range is Zero). Code treating 'Duration' uniformly across the four range types is off by one day for DateOnlyRange.

**Consequence.** new DateOnlyRange(Jan1, Jan1).Duration == TimeSpan.FromDays(1) but new DateTimeRange(Jan1_00:00, Jan1_00:00).Duration == TimeSpan.Zero; converting the same conceptual range between types changes its Duration.

**Options.** Either document loudly, or make Duration = End.DayNumber - Start.DayNumber days and let DayCount carry the inclusive semantics.

### C-2. Pure calendar predicates (IsWeekday, IsWeekend, IsFirstDayOfMonth, IsLastDayOfMonth) exist only on DateTimeRules, not DateOnlyRules

**`src/PineGuard.Core/Rules/DateOnlyRules.cs`:19** - severity **minor** - `missing-symmetry`

DateTimeRules has IsWeekday, IsWeekend, IsFirstDayOfMonth, IsLastDayOfMonth, IsSameDay, and IsWithinDaysFromNow. These are date-only concepts (no time component involved), yet DateOnlyRules — the type purpose-built for dates — lacks all of them. The general pattern elsewhere is member parity between DateOnly/DateTime/DateTimeOffset families (IsInPast, IsInFuture, IsBetween, IsBefore, IsAfter, IsSame, IsChronological, IsOverlapping, IsWithinCalendarMonths all exist in each).

**Consequence.** A user modeling a date as DateOnly (the idiomatic type) writes DateOnlyRules.IsWeekend(d) and gets CS0117, forced to convert to DateTime just to use the calendar predicates.

**Options.** Add the calendar predicates to DateOnlyRules (and DateTimeOffsetRules where meaningful) or document why they are DateTime-only.

### C-3. HasHeaderValue compares whole raw values only; comma-merged list headers never match

**`src/PineGuard.Core/Rules/HttpRules.cs`:120** - severity **minor** - `edge-case`

HasHeaderValue trims and compares each stored value string in full. HTTP intermediaries commonly merge repeated headers into one comma-separated value ('nosniff, nosniff' for X-Content-Type-Options is seen in the wild behind proxies), and list-typed headers carry multiple tokens in one value. HasXContentTypeOptionsWithDefaults then returns false for a response that effectively has nosniff set. Whether to split on commas is a design decision (not valid for all headers, e.g. dates), but the current behavior is worth documenting on the security-header wrappers.

**Consequence.** headers = { "X-Content-Type-Options": ["nosniff, nosniff"] } (proxy-merged duplicate) -> HasXContentTypeOptionsWithDefaults returns false although the protection is active.

**Options.** Document exact-match semantics, or split list-typed header values on ',' before comparison in the security-header rules.

### C-4. TimeOnlyRange lacks Intersect/Union/IsAdjacentTo and cannot represent midnight-crossing ranges

**`src/PineGuard.Core/Common/TimeOnlyRange.cs`:7** - severity **minor** - `api-consistency`

The other three range types expose IsAdjacentTo, Intersect, and Union; TimeOnlyRange stops at Overlaps, an unexplained gap in an otherwise parallel API family. Separately, the Start <= End constraint makes wrap-around time windows (22:00-06:00 — night shifts, quiet hours, the primary use case for time-of-day ranges) unrepresentable, even though the BCL's own TimeOnly.IsBetween supports wrap-around. Contains cannot express what TimeOnly natively can.

**Consequence.** Modeling quiet hours 22:00-06:00: new TimeOnlyRange(new TimeOnly(22,0), new TimeOnly(6,0)) throws ArgumentException; there is no supported way to validate 'value is within 22:00-06:00' with this type.

**Options.** Either support wrap-around (Contains: Start <= End ? normal : value >= Start || value <= End, as TimeOnly.IsBetween does) or document the limitation; add the missing set operations for parity.

### C-5. Midnight-wrapping time ranges (e.g. 22:00-06:00) are unrepresentable, so overnight windows cannot be validated

**`src/PineGuard.Core/Rules/TimeOnlyRangeRules.cs`:21** - severity **minor** - `edge-case`

TimeOnlyRange's constructor throws and TryCreate returns false when start > end, and TimeOnlyRules.IsChronological/IsOverlapping treat start > end as non-chronological/never-overlapping. There is therefore no supported way to express or validate an overnight window (a very common TimeOnly use case: night shifts, maintenance windows, quiet hours). This is a coherent design choice, but nothing in the XML docs states that wrap-around ranges are unsupported.

**Consequence.** TimeOnlyRange.TryCreate(new TimeOnly(22,0), new TimeOnly(6,0), out _) returns false; a caller validating a 22:00-06:00 quiet-hours window has no API that models it and may incorrectly conclude the configuration is invalid.

**Options.** Document the no-wrap constraint on TimeOnlyRange and TimeOnlyRangeRules, or add explicit wrap-aware support (e.g., a ContainsWrapped/IsInWindow helper) if overnight windows are in scope.

### C-6. IsAdjacentTo semantics contradict inclusive overlap and differ from DateOnlyRange's definition

**`src/PineGuard.Core/Common/DateTimeRange.cs`:134** - severity **info** - `api-consistency`

DateTimeRange/DateTimeOffsetRange define adjacency as sharing an endpoint (Start == other.End || End == other.Start). But for inclusive ranges, sharing an endpoint IS an overlap: [a,b] and [b,c] both Contains(b), and Overlaps(other, Inclusion.Inclusive) returns true — so ranges can be simultaneously 'adjacent (no overlap implied)' and 'overlapping'. DateOnlyRange (line 105) uses the disjoint definition (End.DayNumber + 1 == other.Start.DayNumber), i.e. genuinely gap-free and non-overlapping. The two definitions are mutually inconsistent across the same family of types.

**Consequence.** Interval-merging code that partitions pairs into adjacent XOR overlapping: for DateTimeRange [a,b],[b,c] both IsAdjacentTo and Overlaps(Inclusive) return true (double-handled); porting the same logic to DateOnlyRange behaves differently because adjacency there excludes the shared-day case.

**Options.** Pick one adjacency semantic (disjoint-and-gap-free, i.e. End + 1 tick == other.Start, matching DateOnlyRange) and apply it to all range types.


## D. Comparer semantics - whose equality wins?

Both concern a rule silently adopting a comparer the caller did not ask for. The behaviour may be defensible; the documentation currently promises something else. Decide the contract, then make code and docs agree.

### D-1. Contains uses the collection's own comparer for ICollection<T> but the default comparer otherwise, while the doc promises the default comparer

**`src/PineGuard.Core/Rules/CollectionRules.cs`:279** - severity **minor** - `api-consistency`

The switch fast-path `ICollection<T> c => c.Contains(item)` delegates to the collection's membership semantics (e.g. a HashSet<string> built with StringComparer.OrdinalIgnoreCase), whereas the fallback branch explicitly uses EqualityComparer<T>.Default. The XML doc (lines 272-273) states the check is 'according to the default equality comparer'. The verdict therefore depends on the runtime type of the sequence, not on its contents.

**Consequence.** var set = new HashSet<string>(StringComparer.OrdinalIgnoreCase){"ABC"}; CollectionRules.Contains(set, "abc") returns true, but CollectionRules.Contains(set.ToList(), "abc") returns false — two different validation verdicts for the same logical data, and the first contradicts the documented default-comparer contract.

**Options.** Either drop the ICollection fast-path (always use EqualityComparer<T>.Default) or document that the collection's own semantics win; consider adding an IEqualityComparer<T>? overload like HasDistinctItems has.

### D-2. IsSubsetOf silently adopts the comparer of a caller-supplied HashSet, otherwise the default comparer

**`src/PineGuard.Core/Rules/CollectionRules.cs`:298** - severity **minor** - `api-consistency`

`var otherSet = other as HashSet<T> ?? [.. other];` reuses an existing HashSet including its custom IEqualityComparer, while any other sequence type is materialized into a HashSet with the default comparer. Membership verdicts thus change with the runtime type of `other`. Unlike HasDistinctItems/HasDuplicateItems, there is no comparer parameter to make the intent explicit, and the XML doc says nothing about comparers.

**Consequence.** IsSubsetOf(new[]{"A"}, new HashSet<string>(StringComparer.OrdinalIgnoreCase){"a"}) returns true, but IsSubsetOf(new[]{"A"}, new List<string>{"a"}) returns false — same logical superset, opposite verdicts.

**Options.** Only reuse `other` when it is a HashSet<T> whose Comparer equals EqualityComparer<T>.Default, or add an `IEqualityComparer<T>? comparer = null` parameter and always build the set with it.


## E. Mutable shared state

Process-wide state that one consumer can mutate to change another's validation verdicts. Fixing these is behaviour-preserving for well-behaved callers but removes a supported (if unwise) mutation path.

### E-1. Constructor publishes 'this' to static registries before construction completes; failed derived constructors leave permanent phantom registrations

**`src/PineGuard.Core/Common/Enumeration.cs`:46** - severity **minor** - `thread-safety`

The base constructor stores 'this' into the static ConcurrentDictionaries before derived-class construction finishes (leaked-this: other threads can observe a partially constructed member via a registry-backed lookup if registries are ever used for lookup). Worse, if a derived constructor throws AFTER the base constructor completes, the half-constructed instance stays registered forever — the name and value are permanently blocked in that AppDomain and any retry throws 'already exists'. There is also a small rollback race: thread A adds name X, fails the value TryAdd, and removes X (line 46); a concurrent thread B legitimately registering name X in that window is spuriously rejected.

**Consequence.** class Currency : StringEnumeration { Currency(string v, string n) : base(v, n) { if (v.Length != 3) throw ... } } — a failed construction with value 'USDX' still registers 'USDX'; constructing the corrected member with the same name later throws "already exists" until process restart.

**Options.** Validate everything before registering, or move registration out of the constructor (e.g. lazily build registries from GetAll<T> on first lookup).

### E-2. DefaultAllowedDigitSeparators is a public mutable static char[] that feeds process-wide parsing behavior

**`src/PineGuard.Core/Rules/StringRules.NumberTypes.cs`:36** - severity **minor** - `thread-safety`

`public static readonly char[] DefaultAllowedDigitSeparators = [' ', '-'];` — readonly protects the reference, not the elements. Any consumer can write `StringRules.NumberTypes.DefaultAllowedDigitSeparators[0] = '.'` (accidentally or maliciously) and silently change the default separator-stripping behavior of StringUtility.TryParseDigits for every caller in the process (StringUtility.cs line 53 uses it as the null default). This is shared mutable static state: a data race on element writes is possible, and there is no way to detect or restore the original values. Analyzer rule CA2105 exists precisely for this pattern.

**Consequence.** Library A mutates DefaultAllowedDigitSeparators[1] = '.'; thereafter library B's call TryParseDigits("4111-1111-1111-1111", out d) returns false (hyphen no longer allowed) while TryParseDigits("41.11", out d) starts returning true — global validation drift with no local cause.

**Options.** Expose it as `public static ReadOnlySpan<char> DefaultAllowedDigitSeparators => [' ', '-'];` (net8+ compiles this to immutable data) or an ImmutableArray<char>/IReadOnlyList<char>, keeping a private char[] for internal use.


## F. Internal structure - layering, dead code and duplication

No behaviour change for consumers; these are about the codebase not fighting its own conventions. Safe to do incrementally in patch releases.

### F-1. GetAll<T> uses DeclaredOnly, so members declared on a base enumeration class are invisible to lookups on a derived type

**`src/PineGuard.Core/Common/Enumeration.cs`:65** - severity **minor** - `edge-case`

GetFields(BindingFlags.DeclaredOnly) skips inherited static fields. In a hierarchy where a shared abstract subclass declares common members and concrete types extend it, GetAll<Derived>/FromName<Derived>/FromValue<Derived> silently miss the base-declared members even though they are assignable to Derived is false... more precisely: members declared as fields on the base type are excluded from GetAll<Derived> because DeclaredOnly only reflects Derived's own fields.

**Consequence.** abstract class PaymentMethod declares public static readonly fields; class ExtendedPaymentMethod : PaymentMethod adds more. GetAll<ExtendedPaymentMethod>() returns only the extended members; FromName<ExtendedPaymentMethod>("Visa") for a base-declared 'Visa' field returns null.

**Options.** Drop DeclaredOnly (keeping the IsAssignableFrom filter) or document that members must be declared directly on the queried type.

### F-2. FromValue/FromName do a reflection scan per call; the static registries are written but never read

**`src/PineGuard.Core/Common/Enumeration.cs`:81** - severity **minor** - `performance`

NameRegistries/ValueRegistries (lines 14-15) are populated in the constructor solely to enforce uniqueness and are never consulted for lookups. FromValue<T>/FromName<T> instead call GetAll<T>() on every invocation: GetFields reflection + f.GetValue(null) per field + a new list allocation + LINQ FirstOrDefault. Enumeration lookups are exactly the kind of thing Rules call in hot validation paths (e.g. validating a code against a smart enum for every request/row).

**Consequence.** A Must clause validating a CSV column against FromName<T> performs O(members) reflection and allocates a fresh list for every one of a million rows.

**Options.** Serve FromName/FromValue from the existing concurrent registries (after ensuring the type's static fields are initialized via RuntimeHelpers.RunClassConstructor), or cache GetAll<T> per closed type.

### F-3. OwaspRules duplicates OwaspUtility's regex OR-chains verbatim instead of sharing one implementation; XSS has no Utility counterpart

**`src/PineGuard.Core/Utils/OwaspUtility.cs`:21** - severity **minor** - `layering`

ContainsSqlInjectionRisk / ContainsPathTraversalRisk / ContainsCommandInjectionRisk are character-for-character the same six/four/three-regex OR-chains as OwaspRules.IsSqlInjectionSafe / IsPathTraversalSafe / IsCommandInjectionSafe (negated). The repo invariant says validation logic lives in exactly one layer with no duplication. The drift risk is already realized: OwaspUtility has Contains* methods for 7 of the 8 categories but no ContainsXssRisk, and the Rules-side XSS check has silently diverged from its own pattern set (see the IsXssSafe finding). Any future pattern-list edit must now be made in two files.

**Consequence.** A developer adds a new pattern to ContainsSqlInjectionRisk in OwaspUtility but not to OwaspRules.IsSqlInjectionSafe; the same payload is then simultaneously reported 'risky' by the Utility and 'safe' by the Rule.

**Options.** Make one layer the single source (e.g. Rules call !OwaspUtility.Contains*Risk after their own null/whitespace gate, or extract a private shared matcher), and add ContainsXssRisk for symmetry.

### F-4. RangeRules duplicates inclusion logic instead of using RuleComparison and silently treats undefined Inclusion values as Exclusive

**`src/PineGuard.Core/Rules/RangeRules.cs`:13** - severity **minor** - `api-consistency`

IsChronological and IsOverlapping hand-roll `inclusion == Inclusion.Inclusive ? <= : <` instead of using the shared RuleComparison helper mandated by the repo style invariant ('Use RuleComparison shared helper for IComparable<T> with inclusive/exclusive support'). Behaviorally, an out-of-range enum value such as `(Inclusion)42` falls through to the Exclusive branch here, whereas RuleComparison.IsBetween/IsGreaterThan/IsLessThan throw ArgumentOutOfRangeException for the same input — two different conventions for the same configuration error inside one Rules namespace.

**Consequence.** RangeRules.IsChronological(1, 1, (Inclusion)42) returns false (treated as Exclusive), while NumberRules.IsInRange(1, 0, 2, (Inclusion)42) throws ArgumentOutOfRangeException via RuleComparison — inconsistent handling of an identical invalid configuration value across sibling rules.

**Options.** Route both methods through RuleComparison (e.g. RuleComparison.IsLessThan(start, end, inclusion) for IsChronological) so undefined enum values are rejected uniformly.

### F-5. Name lookup is case-insensitive but Value lookup is case-sensitive ordinal for string enumerations

**`src/PineGuard.Core/Common/StringEnumeration.cs`:8** - severity **minor** - `api-consistency`

FromName/TryFromName use OrdinalIgnoreCase and the name registry is OrdinalIgnoreCase, but FromValue/TryFromValue for StringEnumeration use EqualityComparer<string>.Default (case-sensitive ordinal), and the value registry is case-sensitive too. For string-valued enums where Value and Name are often the same token, the asymmetry surprises: 'usd' finds the member by name but not by value.

**Consequence.** StringEnumeration member (value: "USD", name: "USD"): TryFromName("usd") succeeds while TryFromValue("usd") fails.

**Options.** Document the asymmetry or let StringEnumeration pin a StringComparer for value lookups.

### F-6. SpaceCase validation enforces no letter casing, unlike every other separated style

**`src/PineGuard.Core/Utils/StringUtility.Casing.cs`:165** - severity **minor** - `api-consistency`

TryCreateWordsFromSpaceCase only checks single-space separation and char.IsLetterOrDigit; it applies no RequiredLetterCasing, whereas Snake/Kebab/Dot require lower, UpperSnake requires upper, and Train requires Title/Acronym. Meanwhile the output direction (TryToCase, line 131) renders SpaceCase with WordTransform.Title. So IsSpaceCase accepts strings that ToCase(SpaceCase) could never produce, and the IsSpaceCase XML doc example ("my variable name") implies lowercase.

**Consequence.** StringRules.IsSpaceCase("hELLo WoRLD") returns TRUE; StringUtility.ToCase("hELLo WoRLD", StringCasing.SpaceCase, out var cased) returns "Hello World" — validation accepts a value that round-trips to a different string, unlike all sibling styles which are closed under their own ToCase.

**Options.** Decide the canonical letter casing for SpaceCase (Title, to match the output transform, or Lower to match the doc example) and enforce it in TryCreateWordsFromSpaceCase.

### F-7. Type string is strictly validated but Name and MaxLength are not validated at all

**`src/PineGuard.Core/Common/CsvColumnSchema.cs`:29** - severity **info** - `api-consistency`

ParseColumnType throws for null/empty/unknown type, yet Name may be null or whitespace (both constructors) and MaxLength may be zero or negative — nothing checks them, despite Name being declared non-nullable. default(CsvColumnSchema) likewise has a null Name. Also note ParseColumnType accepts 'datetimeoffset' but there is no 'datetime' mapping and CsvColumnType has no DateTime member — schema authors writing the most common type name 'datetime' get an ArgumentException; if that is a deliberate push toward DateTimeOffset it deserves a doc note.

**Consequence.** new CsvColumnSchema(null!, "int", maxLength: -5) constructs successfully; downstream validation using MaxLength = -5 rejects every value and Name-based error messages NRE or print empty names.

**Options.** Validate name via ThrowHelper.ThrowIfNullOrWhiteSpace and require MaxLength > 0; document (or add) 'datetime' handling.

### F-8. SignedIntegerPattern/SignedIntegerRegex are public but unused, and the regex accepts Unicode digits the parse-based rules reject

**`src/PineGuard.Core/Rules/StringRules.NumberTypes.cs`:19** - severity **info** - `api-consistency`

Grep across src shows no consumer of SignedIntegerRegex() or SignedIntegerPattern — they are dead public API inside the rules class. Worse, `\d` without RegexOptions.ECMAScript matches all Unicode Nd digits (RegexOptions.CultureInvariant does not change this), so SignedIntegerRegex().IsMatch("٤٢") (Arabic-Indic 42) returns true, while IsInt32("٤٢") in the same class returns false (int.TryParse with InvariantCulture rejects non-ASCII digits). Two public members of the same class thus give contradictory answers to "is this a signed integer". Additionally the netstandard2.1 fallback (line 32) has a 250ms match timeout while the net8+ [GeneratedRegex] version has none — a per-TFM behavior difference (harmless for this non-backtracking pattern, but gratuitous drift).

**Consequence.** Caller uses the advertised public SignedIntegerRegex() as a pre-filter: "٤٢" passes the regex but fails IsInt32, so the two "same" checks disagree; conversely "0x2A"-style inputs behave consistently. On netstandard2.1 a pathological input could throw RegexMatchTimeoutException where net8.0 would not.

**Options.** Either delete the unused members or change the pattern to ^[+\-]?[0-9]+$ and align the timeout behavior across TFMs.


## G. Multi-target coverage

netstandard2.1 consumers silently get a smaller API surface than the package advertises.

### G-1. netstandard2.1 build ships StringRules.NumberTypes but not StringRules.Numbers/GeoLocation

**`src/PineGuard.Core/Rules/StringRules.Numbers.cs`:1** - severity **minor** - `multi-target`

Directory.Build.props targets netstandard2.1;net8.0;net10.0. StringRules.Numbers.cs, StringRules.GeoLocation.cs and GeoLocationRules.cs are wholly inside #if NET8_0_OR_GREATER (forced by NumberRules' INumber<T> dependency), while StringRules.NumberTypes.cs and StringUtility.NumberTypes.cs compile for all TFMs. So a netstandard2.1 consumer gets IsInt32/IsDecimal but no IsPositive/IsLatitude, an arbitrary-looking API split. GeoLocationRules in particular needs nothing newer than double.IsFinite (available on netstandard2.1); its absence there is purely transitive via NumberRules.IsFinite and could be lifted with a trivial local finiteness check. Compile-time visible, so no silent behavior difference — noting for API-surface planning.

**Consequence.** A netstandard2.1 consumer calling GeoLocationRules.IsLatitude(51.5) gets CS0103 (type does not exist) although every API the method needs exists on that TFM.

> **Amended 2026-08-25.** The `GeoLocationRules` half of this item was resolved during
> remediation: its `#if NET8_0_OR_GREATER` gate is gone and it now compiles for every target.
> `StringRules.Numbers.cs` and `StringRules.GeoLocation.cs` remain gated, so the decision below
> still stands for those two files and for the question of whether netstandard2.1 is first-class.

**Options.** Un-gate GeoLocationRules (replace NumberRules.IsFinite with double.IsFinite or an #if-free helper) and consider non-generic netstandard2.1 fallbacks for the decimal-based Numbers rules if that TFM is meant to be first-class.


## H. Documentation that contradicts behaviour

The one item here is the highest-severity entry in this document, because a shipped example that returns the opposite of what it claims will be copied verbatim by users.

### H-1. Documented example IsPhoneNumber("+1 (555) 123-4567") actually returns false — space is not an allowed character

**`src/PineGuard.Core/Rules/PhoneRules.cs`:11** - severity **major** - `doc-drift`

DefaultAllowedNonDigitCharacters = ['+','(',')','-','.','/'] contains no space, and StringUtility.TryParseDigits (StringUtility.cs line 82) returns false on the first disallowed character; only leading/trailing whitespace is removed by Trim. Yet the XML doc on the allowedNonDigitCharacters parameter (line 33) says 'e.g., spaces, dashes' and the <example> block (line 42) claims IsPhoneNumber("+1 (555) 123-4567") // true. Interior spaces are the single most common phone formatting character, so the default rejects the library's own canonical example and most real-world formatted numbers.

**Consequence.** PhoneRules.IsPhoneNumber("+1 (555) 123-4567") returns false because the interior ' ' characters are neither digits nor in the default allowed set — directly contradicting the shipped XML doc example.

**Options.** Add ' ' to DefaultAllowedNonDigitCharacters (matching the docs and E.164 formatting reality), or fix the docs/example if space rejection is intended.


---

## Suggested sequencing

1. **H-1 first, on its own.** It is a documentation bug with a one-line fix either way, and it
   misleads users today. It does not need to wait for a major version.
2. **F and E next.** Internal structure and shared-state fixes are invisible to well-behaved
   consumers and can ship continuously.
3. **C and G when convenient.** Both are additive - new capability and wider target coverage
   break nobody.
4. **A, B and D together, in one major version.** These are the source-breaking ones. Batch
   them, write one migration note, and take the hit once.

## What this document is not

It is not a backlog of known bugs. Every unambiguous defect the review found is already fixed
and on `main`. If an item here turns out on closer reading to have only one defensible answer,
that makes it a bug: fix it and delete the entry.
