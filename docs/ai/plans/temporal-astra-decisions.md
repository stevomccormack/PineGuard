<!-- metadata_header
type: plan
id: temporal-astra-decisions
version: 1.0
status: planned
last_updated: 2026-09-26
-->

# Temporal program — Astra decision register

This register records the current Astra review and adversarial reconsideration. Luna supplied
repository and primary-source evidence; GPT-6 Sol transcribed the adjudicated recommendations.
It is not a Fable council transcript. The earlier council transcript remains historical evidence.
Claude Fable 5.1 must conduct the revised-plan council and all future decisions/reviews/councils;
Claude Sonnet 5 and Claude Opus 5.5 implement after the program's activation gate.

Each `A-xx` reference in a plan points here. An approved reversal requires reproducible correctness
evidence, applicable Brain clauses and precedent, root-cause alternatives, Fable adjudication and
any required council/spec cascade. Exceptional Brain departure is a demonstrated last resort and
must appear in this register, affected plan/spec sections, the PR and the final report. Numerical
confidence alone is not approval. This governance supersedes stale role routing in the plan set;
it does not rewrite historical transcripts or claim unavailable councils completed.

## A-01 — Authority and truthful activation evidence

**Previous direction:** stale Opus/Haiku routing and an earlier council stamp could be read as
covering subsequent plan changes.

**Astra final decision:** current review authority is Astra; research is Luna; plan editing is
Sol. Future implementation is Claude-only. Fable 5.1 owns all future decisions, reviews and
councils. Revised-plan council execution and owner activation remain explicit gates, recorded
with actual model identity, transcript and outcome. An unavailable model or failed invocation
must never be described as a completed approval.

**Reason / risk resolved:** implementation agents can follow final decisions without quietly
reopening architecture, and review provenance cannot be confused with historical council evidence.
**Brain references:** `docs/ai/specs/spec.md`; `docs/ai/specs/council.md` §1 and §4.8;
`docs/ai/workflows/plan-with-council.md`.

## A-02 — Temporal types and provider behavior

**Previous direction:** missing explicit disposition for other clocks, scheduler types and wrappers.

**Astra final decision:** cover DateTime, DateTimeOffset, DateOnly, TimeOnly, TimeSpan,
TimeZoneInfo and clock-consuming TimeProvider APIs, with nullable parity following existing layers.
System/custom/Fixed/Fake providers are accepted through TimeProvider, not separate validation
families. FixedTimeProvider normalizes its constructor instant to UTC, defaults LocalTimeZone to
UTC and permits a custom zone. It freezes wall-clock reads; inherited timestamp/timer APIs retain
their real behavior unless deliberately overridden. Advancing/timer tests use Microsoft's
FakeTimeProvider only under test-dependency precedent, never as a production dependency.
Monotonic timestamp counters/ITimer/Stopwatch scheduling, arbitrary calendars and NodaTime /
SqlDateTime wrappers are deferred distinct proposals. Existing enum rules cover DayOfWeek and
DateTimeKind legality; offsets and epochs belong T-02/T-06.

**Reason / risk resolved:** a frozen wall clock must not be advertised as a virtual scheduler,
and existing .NET abstractions prevent needless provider families or package dependencies.
**Sources:** [TimeProvider overview](https://learn.microsoft.com/en-us/dotnet/standard/datetime/timeprovider-overview),
[TimeProvider testing](https://learn.microsoft.com/en-us/dotnet/core/extensions/timeprovider-testing),
[Choosing temporal types](https://learn.microsoft.com/en-us/dotnet/standard/datetime/choosing-between-datetime).

## A-03 — Explicit configuration and civil-date / instant separation

**A-02/A-05 framework clarification (Astra approved):** PineGuard's native .NET 8+ targets are
currently `net8.0` and `net10.0`; it also ships the `netstandard2.1` compatibility compile surface.
Tests target `net8.0` and `net10.0`. This plan claims no .NET 6/7 runtime target. D-5 names the
current supported targets rather than irrelevant BCL introduction history; BCL zone conversion
is unavailable in the netstandard2.1 API surface, so that path uses Standards' versioned CLDR
mapping. Runtime/globalization fallibility remains explicit. Framework declarations and production
code are unchanged. Evidence: [root target declarations](../../../Directory.Build.props) line 3,
[test target declarations](../../../tests/Directory.Build.props) line 7,
[Must target-framework requirement](../specs/must-clauses/project.md) line 371 and
[Guard target-framework requirement](../specs/guard-clauses/project.md) line 677, verified by Luna.
This is factual alignment, not a new architecture decision.

**Previous direction:** implicit host policy and repeated now reads could leave calendar and instant
rules inconsistent or suggest composition shares one atomic clock snapshot.

**Astra final decision:** use existing explicit TimeProvider/TimeZoneInfo parameters; narrowly
scoped immutable options only for an actual distinct policy. No global mutable clock/zone/culture
or umbrella TemporalOptions. Preserve D-1 DateTime civil-date input behavior and D-2 raw `.Date`
IsSameDay (Kind ignored). Preserve D-14 DateTimeOffset age on its fixed-offset calendar, not a
named-zone/DST calendar. Instant comparisons use actual instants and document DateTime Kind
acceptance/normalization separately. Each predicate reads now once. Composition independently
evaluates its rules unless callers explicitly supply a shared snapshot; no atomic composition
promise is introduced. Calendar today comes from the provider's LocalTimeZone.

**Reason / risk resolved:** policy remains visible at call sites; midnight boundaries and advancing
clocks do not change a single predicate midway through evaluation. Counting/advancing provider
tests prove the read contract.
**Sources:** [DateTime equality](https://learn.microsoft.com/en-us/dotnet/api/system.datetime.equals),
[DateTimeOffset equality](https://learn.microsoft.com/en-us/dotnet/api/system.datetimeoffset.equals),
[Temporal arithmetic](https://learn.microsoft.com/en-us/dotnet/standard/datetime/performing-arithmetic-operations).

## A-04 — Latest reference acquisition without class generation

**Previous direction:** incomplete zone1970.tab membership and generation coupled to refresh could
produce incomplete or unexpectedly changed catalog classes.

**Astra final decision:** a dedicated download-only path cannot call generation scripts/targets or
build hooks. Retrieve latest stable IANA and latest actually published stable CLDR XML data,
then pin immutable artifact URLs and versions in a receipt with retrieval UTC and SHA256. Label
locally computed hashes honestly unless verified against published upstream checksums. Preserve
the complete IANA source archive, VERSION and all selected Zone/Link source files (including
backward and etcetera), zone.tab and zone1970.tab references, CLDR windowsZones.xml and license/version
references. Download to staging; validate content, archive path safety, version, XML/schema and
published hashes where supplied before replacing valid reference files. Failure preserves old
references. Browser retrieval is a last resort with the same validation and provenance.

Future separately authorized generation parses all selected standard tzdata Zone and Link files.
zone1970.tab is not exhaustive canonical membership. State the selected default dataset; do not
blindly merge optional backzone. Canonical means a Zone record in the pinned dataset. Validate
Link chains against final Zone targets and reject cycles, dangling targets and duplicate conflicts.
OS resolver data stays distinct from pinned references. No generated classes are part of this review.

**Reason / risk resolved:** refresh is reproducible, preserves valid inputs on partial failure and
cannot silently mutate public catalog outputs. CLDR marketing patch labels must not relabel an
older actual XML artifact.
**Sources:** [IANA releases](https://www.iana.org/time-zones/releases),
[Unicode CLDR downloads](https://cldr.unicode.org/index/downloads),
[Published CLDR artifacts](https://www.unicode.org/Public/cldr/latest/).

## A-05 — Preserve the Core no-IO invariant (final adversarial override of D-3)

**Previous direction:** allow Core host-registry lookup by declaring culture/timezone access
"not IO" and creating a generic environment exception.

**Astra final decision:** preserve Brain spec §5.3. Remove the proposed Core
`IsSystemTimeZoneId` / `TryFindSystemTimeZone` host-lookup family from this program. Core predicates
accept supplied TimeZoneInfo and support custom zones deterministically. Resolve IDs once at an
existing adapter/composition boundary, including DA, or document consumer BCL usage. Keep
Standards snapshot membership separate from host resolvability and mapping. No new satellite
package is justified. A future host predicate proposal requires a concrete unmet use case and
Brain-first Fable review/council; a Brain exception remains last resort, not authorized here.

**Alternatives reconsidered:** snapshot membership answers a different question; supplied-zone
API plus composition lookup meets zone-validation needs; six-layer symmetry does not justify a
forbidden Core dependency. This second adversarial decision supersedes the earlier conditional
Core exception considered during this review.

**Risk controls:** host resolution can access registry/files/ICU and varies with runtime, OS,
globalization mode and data. netstandard2.1 is a compile surface, not a runtime behavior guarantee.
Verify actual API availability on every target before selecting TryFindSystemTimeZoneById or
FindSystemTimeZoneById with documented catches. Do not introduce positive/negative global caches
without measured need and explicit invalidation/ClearCachedData semantics. Use synthetic custom
zones for deterministic DST correctness, and identify host integration tests separately.
**Brain reference:** `docs/ai/specs/spec.md` §5.3.
**Sources:** [TimeZoneInfo identifiers](https://learn.microsoft.com/en-us/dotnet/api/system.timezoneinfo.id),
[Windows/IANA conversion requirements](https://learn.microsoft.com/en-us/dotnet/api/system.timezoneinfo.tryconvertwindowsidtoianaid),
[TimeZoneInfo conversion](https://learn.microsoft.com/en-us/dotnet/api/system.timezoneinfo.converttime).

## A-06 — Relative windows and instant domains

**Astra override:** replace `now ± span` / DateOnly AddDays arithmetic with ordered elapsed
differences. Last: value≤now and now−value≤span; Next: value≥now and value−now≤span; Older:
value<now and now−value>age. DateOnly uses DayNumber differences. Nonnegative spans/bounds;
one now read. New DateTime instant families require Kind.Utc; Local/Unspecified fail value
validation, while existing APIs and civil D-1/D-2 semantics remain scoped as documented.
Instant validity is [start,end), date validity [start,end]; inverted bounds fail, equal instant
bounds are empty, equal date bounds can succeed that date. Test exact extreme inclusivity rather
than rejecting extremes by assumption. **Reason:** avoids overflow and machine-zone conversion.
**Sources:** [Temporal arithmetic](https://learn.microsoft.com/en-us/dotnet/standard/datetime/performing-arithmetic-operations),
[DateTime equality](https://learn.microsoft.com/en-us/dotnet/api/system.datetime.equals).

## A-07 — Completed age and explicit calendar configuration

**Astra override:** age predicates call shared TryGetAge once then compare completed integer
years; remove `(years+1)` / threshold AddYears. Negative thresholds/inversion/future birth fail;
large positive int thresholds compare safely. Preserve existing March-1 leap-birthday convention:
2008-02-29 is 17 on 2026-02-28 and 18 on 2026-03-01. This is PineGuard's calendar convention,
not a legal/regional age guarantee. DTO asOf uses D-14's value offset; local civil today uses D-1.
Guard projection representability before conversion; failure is provider/reference configuration,
not invalid birthday. Internal `DateTimeOffsetUtility.TryGetDateAtOffset(DateTimeOffset instant,
TimeSpan offset, out DateTime date)` produces Unspecified midnight and reads no clock; false/out
default on invalid whole-minute ±14h offset or unrepresentable projected ticks. DateOnly wrappers
convert on supported targets. No clamp and no extra public clock abstraction.

Weekend/holiday policy stays explicit. DefaultWeekend uses private-backed truly read-only storage;
adapters snapshot collections once; Core direct-call collections remain stable caller-owned inputs.
Invalid DayOfWeek anywhere fails configuration; null weekend means Sat/Sun, empty means none,
duplicates harmless. Business-day/holiday comparison uses own wall date.
**Reason:** avoids threshold overflow, host policy and mutable configuration surprises.
**Source:** [Temporal arithmetic](https://learn.microsoft.com/en-us/dotnet/standard/datetime/performing-arithmetic-operations).

## A-08 — Representable strict parsing and explicit epoch units

**A-07/A-10 dependency correction:** pure offset-date projection lands in T-01 U2, with internal
`DateTimeUtility.TryGetTodayDate(TimeProvider? timeProvider, out DateTime today)`: sample GetUtcNow
once, read provider.LocalTimeZone, obtain that zone's offset at the sampled instant once, then
call TryGetDateAtOffset. New calendar predicates use this guarded reference and do not accept
unverified GetLocalNow boundary behavior. Existing unrelated public GetLocalNow behavior is unchanged. Must evaluates
the sampled reference once rather than sampling again through a rule. Foundation owns
Date.Reference.OutOfRange; T-04 reuses existing Date.Kind.NotUtc without duplicating its node or constant. Focused Fable audit ruling supersedes only the prior T-02 Date.Kind reuse: zone clauses use new `TimeZone.Kind.NotUnspecified` (`timezone.kind.not-unspecified`) under the unchanged A-12 template and value attribution. Existing Date.Kind.NotUnspecified and its DateTime consumers remain. This corrects mandatory audit ownership, not Astra semantic policy.
T-02 zone-day comparison uses each input instant's zone offset and the foundation projection
helper, avoiding implicit ConvertTime boundary handling; T-04 consumes the same helper. No later-phase dependency
or duplicate constant/helper is introduced.

New public GetToday/GetTodayDate convenience wrappers are removed: they add redundant surface
and would need an extra failure contract. Only internal guarded reference computation feeds the
new predicates; existing unrelated public GetLocalNow behavior remains unchanged. The D-1/D-14
register rows express these checked paths, not direct unchecked GetLocalNow/ToOffset.

The contract explicitly rejects out-of-range projection rather than relying on unspecified BCL
boundary handling. This does not assert that GetLocalNow definitely saturates: the verified
documentation describes provider-zone projection; ToOffset documents representability failures.
[TimeProvider.GetLocalNow](https://learn.microsoft.com/en-us/dotnet/api/system.timeprovider.getlocalnow?view=net-10.0),
[DateTimeOffset.ToOffset](https://learn.microsoft.com/en-us/dotnet/api/system.datetimeoffset.tooffset?view=net-10.0).

**Astra override:** strict named ISO/RFC profiles use one shared Try parser returning the value;
Must never reparses through general styles. Instant profiles require explicit offset; remove
offset-free DTO defaults. RFC3339 profile: ASCII digits, T/t and Z/z, required seconds, 0 or 1–7
fraction digits; reject whitespace/space separator/leap seconds/unknown -00:00/out-of-DTO-range
offsets/excess precision. Document the supported representable subset rather than claiming every
standard form. General configurable invariant parsing remains separate, with configuration errors
contained under Brain Must policy. Duration: nonnegative PnW or ordered day/time components,
T needs a time component, no years/date-months/mixed weeks/signs/exponents, seconds fraction
point/comma 1–7 digits, exact checked ticks and TryParse. Zero valid; overflow/precision loss
false; bounded grammar and existing regex timeout if regex retained. Epoch methods name units,
accept inclusive DTO bounds and integral input; never infer units by magnitude. A number may be
valid under both units. **Reason:** preserves precision, no parsing exceptions or hidden host zone.
**Sources:** [DateTimeOffset parsing](https://learn.microsoft.com/en-us/dotnet/api/system.datetimeoffset.tryparseexact),
[RFC 3339](https://www.rfc-editor.org/rfc/rfc3339),
[Unix seconds](https://learn.microsoft.com/en-us/dotnet/api/system.datetimeoffset.fromunixtimeseconds).

## A-09 — Timeout domain and PineGuard interval boundaries

**Astra override:** Timeout accepts zero, all positive durations and exactly InfiniteTimeSpan;
other negatives fail. It validates a semantic timeout, not every Timer/Task API's distinct limits.
Interval alignment uses strictly positive intervals and exact ticks. TimeOnly IsBetween preserves
PineGuard Inclusion: nonwrap delegates RuleComparison; wrap Inclusive ≥start OR ≤end,
Exclusive >start OR <end. Equal endpoints are Inclusive singleton / Exclusive empty, never full
day. BCL's half-open method shares the wrapping concept only. Not* state pairs validate the common
domain first; unsupported configuration cannot succeed by naive negation.
**Reason:** zero is a valid immediate timeout and consistent PineGuard boundaries prevent drift.
**Precedent:** `RuleComparison`, `Inclusion`, existing `TimeOnlyRules.IsBetween`.

## A-10 — Gap/overlap domain and failure attribution

**Astra override:** local gap/overlap DateTime values require Kind.Unspecified. Utc/Local fail
value validation; no machine conversion. Positive and Not* variants validate identical domains
before negation. Zone configuration is checked even when both equality values are null; valid-zone
both-null equality follows existing convention. Must attributes Kind to value, zone faults to zone,
reference projection faults to timeProvider, unknown adapter IDs distinctly to ID. Input instant
projection failures are rule value failures, never null-zone failures. Do not accept implicit BCL boundary handling
silently. Contain expected BCL/provider/range errors at existing Must boundaries without broad
fatal-error catches. Test custom DST rules, half-hour/non-hour transitions and exact boundaries;
host integration stays separate. **Reason:** prevents false success and misleading diagnostics.
**Sources:** [TimeZoneInfo invalid time](https://learn.microsoft.com/en-us/dotnet/api/system.timezoneinfo.isinvalidtime),
[TimeZoneInfo ambiguous time](https://learn.microsoft.com/en-us/dotnet/api/system.timezoneinfo.isambiguoustime).

## A-11 — Live Brain evidence and executable handoff

Every phase starts from live Brain index, owning specs/rules/playbook and exact nearest source
precedent with path/section citations. One acceptance matrix records API/type/null/Kind/config/
boundaries/codes/layer/TFM and complement semantics; stale hand counts never define coverage.
Release pins the actually published Repo A version consumed by Repo B and tests the built package
graph. Closeout proves removals/migrations across layers, source receipts, required checks and
actual Fable review/council status; consumer breaking behavior is documented. Design-authorized
scope does not waive mandatory activation gates. **Reason:** other implementation models can
execute without inventing missing policy or claiming checks that did not run.

## A-12 — Preserve Standards host APIs and owning diagnostics

**Astra override of D-7:** removing the competing Core host family eliminates the rename premise.
Preserve existing Standards SystemTimeZone APIs/utilities, observable host behavior, resolver
target branches and owning diagnostics. Remove Core delegation and the assertion that all BCL
resolver calls disappear. Never label host resolution KnownTimeZone or describe it as bundled
membership. Versioned IANA membership/aliases/canonical membership remain separate. Global D-12
stays a tombstone; local S-01 records Standards body/domain diagnostic ownership. Preserve
existing codes; inventory only genuine adaptation requirements from actual restored packages,
without nonexistent Core host diagnostics or additional owner signatures.

**Exact A-06/A-10 value diagnostics:** `MustCodes.Date.Kind.Prefix = Date.Prefix + ".kind"`;
`NotUtc = Prefix + ".not-utc"`, `NotUnspecified = Prefix + ".not-unspecified"`. Messages:
`"{paramName} must have DateTimeKind.Utc."` / `"{paramName} must have DateTimeKind.Unspecified."`;
attribute failure to the value parameter. Reference projection diagnostic:
`MustCodes.Date.Reference.Prefix = Date.Prefix + ".reference"`;
`OutOfRange = Prefix + ".out-of-range"`, message
`"{paramName} requires a reference date within the supported range."`, attributed to
`nameof(timeProvider)` and actual provider argument under existing dependency precedent.
Do not blanket-catch exceptions from arbitrary custom-provider code; only expected
configuration/validation/range failures follow established boundary behavior.

**Reason / risk resolved:** no misleading public rename, nonexistent dependency or hidden provider
failure; diagnostic prefixes and parameter attribution follow the Brain's computed-prefix and
Must configuration conventions (`spec.md` §5.4.2).
