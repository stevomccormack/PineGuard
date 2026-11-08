# Fable focused scope-fidelity and audit-description ruling

- Reviewer/model: Claude Fable 5.1 (claude-fable-5-1, provider firstParty)
- Claude Code CLI: 2.1.283
- Persistent session: 7342dc25-a35f-42a2-a77e-3ea894c6b4eb
- Result: success; CLI exit 0; 4 turns; duration 1181894 ms; cost USD 16.945537
- Usage: input 584, cache read 6534588, cache creation 520465, output 97935; web searches 0
- Scope: binding follow-up on DST scope fidelity, exact audit descriptions, and T-02 U0 sequencing. Public result only.

# Addendum to the B2/B3 ruling (binding)

## 1. `IsDaylightSavingTime` stays, and becomes a complete all-layer concept

**Decision.** The earlier "remove from T-02 U2" line is withdrawn. `TimeZoneRules.IsDaylightSavingTime` is a legitimate temporal validation predicate, and the correct resolution of the `rules-usage` contract (`audit-cli/spec.md:130`) is to give it its Must, Guard, Fluent and DataAnnotations layers, not to delete it or move it to a utility.

Evidence for "validation, not utility": Core defines Rules as pure boolean predicates written `IsX`/`HasX` over a validated value plus configuration (`core/project.md:50-63, 132-137`); Utils exist for parsing, normalisation and `Try*` out-results (`core/project.md:199-225`). A yes/no question about a `DateTimeOffset` in a supplied zone is a Rule by definition, exactly like the sibling zone predicates `IsInvalidTime`, `IsAmbiguousTime`, `HasOffsetOf` that T-02 already lifts through every layer (`temporal-00-program.md:248-250`). It is a state predicate over the value, so D-15 gives it a `Not*` pair (`temporal-00-program.md:185`, the `Midnight`/`Weekday`/`Utc` class; `MustDateTimeClauses.cs:885-919` is the `Utc`/`NotUtc` precedent). It is not cross-property (one instant, one configured zone), so DA is present, following `InTimeZoneDateTimeOffsetAttribute` (`temporal-02-timezone-core.md:328`). Nothing in `src/` implements it today (grep: none), so no relocation question arises.

Implementation-ready contracts (all cloned from the `InTimeZone` row of T-02):

| Layer | File / unit | Member | Behaviour |
|---|---|---|---|
| Rules | `TimeZoneRules.cs`, T-02 U2 | `bool IsDaylightSavingTime(DateTimeOffset? value, TimeZoneInfo? timeZone)` | Unchanged from T-02 U2 `:120-121`; null value or zone → `false`. Fixtures unchanged (`:163`). |
| Codes | `MustCodes.TimeZone.cs`, T-02 U1 | new node `DaylightSaving` | `Prefix = TimeZone.Prefix + ".daylight-saving"`; `NotInEffect = Prefix + ".not-in-effect"` (`timezone.daylight-saving.not-in-effect`); `InEffect = Prefix + ".in-effect"` (`timezone.daylight-saving.in-effect`). Same `/// <summary><c>…</c></summary>` shape as the ruling's code block; Serves line unchanged. |
| Must | `MustTimeZoneClauses.cs`, T-02 U3 | `MustResult<DateTimeOffset> DaylightSavingTime(this IMustClause _, DateTimeOffset value, TimeZoneInfo timeZone, [CallerArgumentExpression(nameof(value))] string? paramName = null)` and `NotDaylightSavingTime(...)` same shape | Both first return `Fail(MustCodes.TimeZone.Zone.Null, "{paramName} must not be null.", nameof(timeZone), value)` on a null zone (D-13). Then `FromBool(TimeZoneRules.IsDaylightSavingTime(value, timeZone), MustCodes.TimeZone.DaylightSaving.NotInEffect, "{paramName} must be in daylight saving time in the time zone.", paramName, value, value)`; the complement negates the rule and uses `InEffect` with `"{paramName} must not be in daylight saving time in the time zone."`. Declaration order: after `InTimeZone`, before `SameDay`; positive before `Not*` (`must-clauses/project.md:151-157`). |
| Guard | `GuardTimeZoneClauses.cs`, T-02 U4 | `NotDaylightSavingTime(value, timeZone, …)` → `Must.Be.DaylightSavingTime`; `DaylightSavingTime(value, timeZone, …)` → `Must.Be.NotDaylightSavingTime`; both return `DateTimeOffset` | Standard `GuardFailure.Throw(result, message, exceptionCreator)`; ordered by invoked Must clause (`guard-clauses/project.md:128-135`). |
| Fluent | `FluentTimeZoneExtensions.cs`, T-02 U5 | `.DaylightSavingTime(TimeZoneInfo timeZone, string? message = null)` and `.NotDaylightSavingTime(...)` on `DateTimeOffset` and `DateTimeOffset?` | `ThrowHelper.ThrowIfNull(timeZone)` at composition; nullable overload passes on null; codes `NotInEffect` / `InEffect` as the `MustBe` code argument. |
| DA | `TimeZoneAttributes.cs`, T-02 U6 | `DaylightSavingTimeDateTimeOffsetAttribute(string timeZoneId)` : `ValidationAttributeBase(typeof(DateTimeOffset), MustCodes.TimeZone.DaylightSaving.NotInEffect)`; `NotDaylightSavingTimeDateTimeOffsetAttribute(string timeZoneId)` with `InEffect` | Ctor `ThrowHelper.ThrowIfNull(timeZoneId)`; validate: resolve once, `Must.Be.ResolvedTimeZone`, then the semantic clause, exactly as `InTimeZoneDateTimeOffsetAttribute`. |

Tests: `TimeZoneRulesTests`/`TestData` (existing group), `MustTimeZoneClausesTests`/`TestData` groups `DaylightSavingTime` and `NotDaylightSavingTime` (rows `Summer`, `Winter`, `FixedZone`, `timeZone = null!` asserting `timezone.zone.null`; one row each asserting the exact message and code), `GuardTimeZoneClausesTests`, `FluentTimeZoneExtensionsTests` (plus the `ThrowIfNull` row), `TimeZoneAttributesTests`. DA tests need real instants in a resolvable zone under T-00 §6.3, so T-01 U1 adds two named constants to `TestTimeZones`: `PlatformDaylightSavingInstant = new DateTimeOffset(2026, 7, 1, 12, 0, 0, TimeSpan.FromHours(-7))` and `PlatformStandardTimeInstant = new DateTimeOffset(2026, 1, 15, 12, 0, 0, TimeSpan.FromHours(-8))` (US Pacific 2026 DST runs 8 March to 1 November), plus `"UTC"` rows proving the positive attribute fails and the negative passes in a zone without DST.

Audit consequences: `rules-usage` satisfied; `layer-parity` and `must-usage` need no `layerAbsences` entry (all four layers present); `ordering` intersection order holds (Rules `IsUtcOffset` < `IsDaylightSavingTime` < `IsSameDay` matches Must `UtcOffset` < `DaylightSavingTime` < `SameDay`); `must-codes` (b) and (d) satisfied by the two DA base codes. Plan edits: T-00 §4.2 row `:251` becomes a full row; §5 code block and single-writer table gain `TimeZone.DaylightSaving` (T-02 U1 creates; `MustTimeZoneClauses` emits); D-15 state list gains `DaylightSavingTime`; T-02 U1/U3/U4/U5/U6 tables gain the rows above; T-01 U1 gains the two constants; T-00 §12 drops the "clauses on demand" backlog line.

## 2. Exact rule description strings

Current strings: `layer-parity.ts:365-366` and `must-usage.ts:322-323`. Replacement literals (single strings, no line breaks):

```text
layer-parity:
Every normalised validation concept implemented by Must, Guard, Fluent, or DataAnnotations is implemented by all of them, except in a layer that vocabulary.json layerAbsences declares absent for that one concept, which must then stay absent (concept-set parity via vocabulary.json; no build, no dotnet publish).

must-usage:
Every public Must clause (src/PineGuard.MustClauses) is invoked from Guard, Fluent, and DataAnnotations, except in a layer that vocabulary.json layerAbsences declares absent for that clause's concept. Supersedes legacy Rule03 (Guard), Rule04 (Fluent), and Rule05 (DataAnnotations) — see this module's header comment.
```

The spec table must match: `audit-cli/spec.md:122-124` states descriptions are "copied verbatim" and "match `pineguard audit --list` output exactly". Text is identical; the table may wrap identifiers in backticks as its existing rows do (`:132` already backticks `vocabulary.json`). No CLI test pins descriptions against the spec (only fake-rule literals in `apps/cli/test/**`), so the constraint is documentary, enforced by review.

## 3. Unit and gate sequence for the audit-model change

T-02 units become eight. Phase row in T-00 §2: `| 2 Time-zone Core | PineGuard | temporal-02-timezone | 8 | U0 ∥ U1 ∥ U2 first; U3 after U0, U1, U2; U4 after U3; U5 ∥ U6 after U4 | Phase 1 merged |`.

| Unit | Title | Model / agent | Depends on | Fan-out | Scope |
|---|---|---|---|---|---|
| U0 | Audit model corrections (`apps/cli`, vocabulary, spec, baseline burn-down) | Claude Sonnet 5 / `general-purpose` | Phase 1 merged | ∥ U1, U2 | none (TypeScript only; touches no `src/` or `tests/` PineGuard project) |
| U1 … U7 | as in T-02 §2, with U3 additionally depending on U0 | | | | |

U0 files: exactly those in the ruling's section 3 table plus `docs/ai/specs/tools/audit-cli/spec.md` §3.1 rows for `layer-parity` and `must-usage`. U0 acceptance, all exit 0: `pnpm -C apps/cli typecheck`, `pnpm -C apps/cli lint`, `pnpm -C apps/cli test` (scripts at `apps/cli/package.json:16-19`), `pnpm -C apps/cli exec tsx src/index.ts audit --gate` (doc-links and surface-parity, because Brain files change), and the full `pnpm -C apps/cli exec tsx src/index.ts audit` against the live repo, with `git diff --stat apps/cli/config/baseline.json` showing deletions only (the `SameDay` and `MustString*:rules:missing-layer` keys). Attach `audit --no-baseline layer-parity ordering must-usage` output as evidence.

Gates (unchanged from the ruling, restated for the tracker): per unit, format, diagnostics, tests, coverage per scope and TFM, `audit --gate`, `audit nullability` for Must/Guard units, `audit must-codes` for any unit touching Codes, MustClauses, GuardClauses or DataAnnotations. Per phase cohort, after the last layer unit and before the Fable phase review, the full `audit` exits 0 and the baseline diff shows deletions only. Tracker: add row `T2-U0 | Audit model corrections | Pending | temporal-02-timezone | | lands before T2-U3; no baseline additions`. No strictness is relaxed and no baseline debt is added.