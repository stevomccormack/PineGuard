# Fable corrective ruling — audit layer and ordering gates

- Reviewer/model: Claude Fable 5.1 (claude-fable-5-1, provider firstParty)
- Claude Code CLI: 2.1.283
- Persistent session: 7342dc25-a35f-42a2-a77e-3ea894c6b4eb
- Result: success; CLI exit 0; 23 turns; duration 1058720 ms; cost USD 15.645574
- Usage: input 550, cache read 5514336, cache creation 493172, output 87961; web searches 0
- Scope: binding technical decision on B2/B3 audit representation. Public result only.

# Binding ruling on B2 and B3 (audit parity and ordering)

**Decision in one line.** Change both the plans and the audit tooling. Neither `ignoreMethods`, `aliases`, `exceptions.json` nor a baseline entry models a deliberate, permanent absence of one layer, so the vocabulary gains one narrow, two-way-enforced declaration and two rules honour it; the `ordering` rule gets its string-family Rules discovery corrected at source; the per-unit gate in T-00 §9 is re-sequenced so cross-layer audits run where they can be true. No baseline key is added anywhere; several are deleted as burn-down.

No Brain rule requires owner approval for these changes. `docs/ai/specs/language/vocabulary.md:110-116` prescribes exactly this path (edit JSON, MD, and the consuming scripts). `docs/ai/specs/tools/audit-cli/spec.md:224-247` mandates VIBE fixtures for any rule change. `safety.md` §7.2 permits modifying `docs/**` and `apps/**` when implementing. The only "sign-off" mention in the tools specs (`audit-cli/spec.md:23`) is historical cutover text. Delegated Fable authority applies (T-00 §0, A-01).

## 1. Decision table

Layer names are the `layer-parity` `LayerSpec.name` values (`layer-parity.ts:127-151`). "Declared absent" means an entry in the new `layerAbsences` map (section 2).

| Concept (normalised) | Must | Guard | Fluent | DataAnnotations | Basis |
|---|---|---|---|---|---|
| `ResolvedTimeZone` | `MustTimeZoneClauses.ResolvedTimeZone` (T-02 U3) | **Add** `GuardTimeZoneClauses.NotResolvedTimeZone` (T-02 U4) | Declared absent | `TimeZoneAttributes` call it (T-02 U6) | Guard is mandated by `guard-clauses/project.md:149-151` ("every public `Must.Be.*` clause must be representable via `Guard.Against.*`"). It is real behaviour, not count parity: it returns the resolved `TimeZoneInfo` at a composition root, mirroring Standards' `Must.Be.SystemTimeZone` returning `MustResult<TimeZoneInfo>` (`MustSystemTimeZoneClauses.cs:47`). Fluent takes a supplied `TimeZoneInfo` and checks it at composition (D-13, T-02 U5), so no Fluent surface exists. |
| `SameDay` (existing unzoned and new zone-aware; `NotSameDay` normalises to it) | present | present | present | Declared absent | Cross-property (T-00 §4.2 `:252`, §12 `:602-604`). Converts today's baseline debt `layer-parity:SameDay:DataAnnotations` (`baseline.json:257`) into a reviewed declaration. |
| `SameMonth`, `SameYear`, `SameIsoWeek` | T-05 U3b | T-05 U4b | T-05 U5b | Declared absent | Cross-property (T-00 §4.5 `:292`; T-05 `:192`). |
| `WithinValidity` | T-04 U3a-c | T-04 U4 | T-04 U5a | Declared absent | Cross-property `[notBefore, notAfter)` (T-00 §4.4 `:274`; T-04 `:315`). |
| Every other new concept in T-02, T-04, T-05, T-06 | present in all four layers per the plans | | | | No declaration; full parity enforced. |

I note the repo does implement some two-operand clauses as DA attributes with a literal bound (`ChronologicalDateOnlyAttribute(string end)`, `DateOnlyAttributes.cs:521-536`). The program's "no DA for cross-property" decisions stand (D-15, T-00 §12); this ruling records the absence rather than reopening them.

`Guard.Against.NotResolvedTimeZone` contract (T-02 U4, `GuardTimeZoneClauses.cs`, first method so Guard order matches Must's):

```csharp
public static TimeZoneInfo NotResolvedTimeZone(this IGuardClause _,
    TimeZoneInfo? resolvedZone,
    string? timeZoneId,
    string? message = null,
    Func<Exception>? exceptionCreator = null,
    [CallerArgumentExpression(nameof(timeZoneId))] string? paramName = null)
{
    var result = Must.Be.ResolvedTimeZone(resolvedZone, timeZoneId, paramName); // Guard.Against.NotResolvedTimeZone => Must.Be.ResolvedTimeZone (complement)
    if (result.Failed)
        GuardFailure.Throw(result, message, exceptionCreator);

    return result.Result!;
}
```

Tests in `GuardTimeZoneClausesTests` + `TestData`: null ID throws with message `timeZoneId must not be null.`; unknown ID throws with `Time zone ID 'timeZoneId' is not a known time zone.`; non-null outcome returns the same instance. `nullability` passes (`TimeZoneInfo?` is a nullable reference primary, `nullability.ts:187-194, 331-339`).

## 2. Vocabulary schema change and exact entries

**Why not the existing knobs.** `ignoreMethods` removes a concept from parity in every layer (`layer-parity.ts:223-225, 245-247`), which is the blanket suppression the owner rejected and would also hide a Guard or Fluent regression. `aliases` only renames a concept (`:227-248`); there is no truthful alias target. `exceptions.json` is substring suppression (`audit-cli/spec.md:190-195`). The baseline is debt to burn down (`:186-188`). None can say "this layer is intentionally absent, and it must stay absent."

**Schema.** Add to `VocabularyConfig` (`apps/cli/src/audit/types.ts:72-83`):

```ts
readonly layerAbsences: Readonly<Record<string, {
    readonly absentFrom: readonly ("Guard" | "Fluent" | "DataAnnotations")[];
    readonly reason: string;
}>>;
```

Bump `vocabulary.json` `"version"` from 1 to 2. Exact entries for `docs/ai/specs/language/vocabulary.json`:

```json
"layerAbsences": {
  "SameDay": {
    "absentFrom": ["DataAnnotations"],
    "reason": "Cross-property comparison against a second instant; no single-property attribute (T-00 §4.2, §12)."
  },
  "ResolvedTimeZone": {
    "absentFrom": ["Fluent"],
    "reason": "Outcome validator for an adapter-resolved zone ID. Fluent extensions take a supplied TimeZoneInfo and check it at composition (D-13); there is no Fluent ID surface."
  },
  "WithinValidity": {
    "absentFrom": ["DataAnnotations"],
    "reason": "Cross-property [notBefore, notAfter) window (T-00 §4.4)."
  },
  "SameMonth":   { "absentFrom": ["DataAnnotations"], "reason": "Cross-property calendar comparison (T-00 §4.5)." },
  "SameYear":    { "absentFrom": ["DataAnnotations"], "reason": "Cross-property calendar comparison (T-00 §4.5)." },
  "SameIsoWeek": { "absentFrom": ["DataAnnotations"], "reason": "Cross-property calendar comparison (T-00 §4.5)." }
}
```

**Matching semantics.** Keys are normalised concept names, so `NotSameDay`, `NotSameMonth` and so on are covered without separate entries. `Must` is never a legal layer. Each entry is written by the Must unit that first implements the concept, so the rule's unknown-concept check never fires mid-phase: `SameDay` in T-02 U0 (converts existing debt), `ResolvedTimeZone` in T-02 U3, `WithinValidity` in T-04 U3a, the three calendar entries in T-05 U3b-1. `vocabulary.md` §2 gains a step 5 describing `layerAbsences`, and the entries are recorded in T-00 §5 as a new table "Declared layer absences".

## 3. Audit source changes, contracts, tests, sequencing

**New unit: T-02 U0 "Audit model corrections" (Claude Sonnet 5, `general-purpose`), first in Phase 2, ∥ U1/U2, no source scope; acceptance `pnpm -C apps/cli test` green plus `pnpm -C apps/cli exec tsx src/index.ts audit --no-baseline layer-parity ordering must-usage` output attached.** It lands before any temporal Must unit and closes existing debt honestly. Files:

| File | Change |
|---|---|
| `apps/cli/src/audit/types.ts` | `layerAbsences` on `VocabularyConfig` (above). |
| `+ apps/cli/src/audit/vocabulary.ts` (new) | Move `resolveVocabulary` and `normalizeConcept` out of `layer-parity.ts:186-249` so `must-usage` shares them; `DEFAULT_VOCABULARY` gains `layerAbsences: {}`. |
| `apps/cli/src/audit/rules/layer-parity.ts` | After computing `allConcepts` (`:401-406`): (a) for each `layerAbsences` key not in `allConcepts`, finding `layer-parity:config:unknown-concept:<Key>` on file `docs/ai/specs/language/vocabulary.json`; (b) for each key whose `absentFrom` contains `"Must"` or an unknown layer name, finding `layer-parity:config:invalid-layer:<Key>`; (c) in the missing loop (`:409-434`), skip a `(concept, layer)` pair when declared absent; (d) when a declared-absent layer does implement the concept, finding `layer-parity:<Concept>:<Layer>:declared-absent-but-implemented`. Update `description`. |
| `apps/cli/src/audit/rules/must-usage.ts` | In `run` (`:288-302`), before pushing a finding, compute `normalizeConcept(method.name, vocabulary)` and skip when the layer (mapped `guard→Guard`, `fluent→Fluent`, `annotations→DataAnnotations`) is declared absent for that concept. Keys unchanged. Update `description`. |
| `apps/cli/src/audit/rules/ordering.ts` | In `discoverMustFamilies` (`:329-361`), when `tail.startsWith("String")`: Rules sibling path is `src/PineGuard.Core/Rules/StringRules.<Rest>.cs` with public static nested class `<Rest>` (or `StringRules.cs` / class `StringRules` when `Rest` is empty). `domainName` stays `tail`, so Guard/Fluent/DA normalisation is unchanged. This restores the C# original's `IsStringFamily` discovery the port dropped (`ordering.ts:64-69`) and matches the real layout (`StringRules.Guid.cs:5-11`). Non-string families keep `<Tail>Rules.cs` (`:656-665`). Fix the `missingLayerFinding` expected path accordingly. |
| `docs/ai/specs/language/vocabulary.json`, `vocabulary.md` | Schema and `SameDay` entry; §2 step 5. |
| `docs/ai/specs/tools/audit-cli/spec.md` §3.1 | Copy the two updated `description` strings verbatim (`:122-124` requires exact match). |
| `apps/cli/config/baseline.json` | Delete only: `layer-parity:SameDay:DataAnnotations` (`:257`), `must-usage:annotations:SameDay` (`:458`), `must-usage:annotations:NotSameDay` (`:423`), and every `ordering:MustString*Clauses:rules:missing-layer` key (`:591-606`). No additions. If the corrected Rules discovery surfaces a real `order-mismatch` on an existing string family, fix the declaration order in that Rules or adapter file in this unit (a no-behaviour reorder enforced by `must-clauses/project.md:151-157`), never baseline it. |

Tests (VIBE, `audit-cli/spec.md:224-269`):

- `test/fixtures/layer-parity/absences/`: `vocabulary.json` declares `Solo` absent from `DataAnnotations`, `Twice` absent from `Guard`, `Ghost` absent from `Fluent`, `Bad` with `absentFrom: ["Must"]`. Must, Guard and Fluent implement `Solo`; DA does not. Guard implements `Twice`. Nothing implements `Ghost`. Assert exactly three findings: `Twice:Guard:declared-absent-but-implemented`, `config:unknown-concept:Ghost`, `config:invalid-layer:Bad`; assert no `Solo` finding. Existing `invalid/` fixture (`layer-parity.test.ts:34-51`) proves an undeclared DA gap still flags.
- `test/fixtures/must-usage/absences/`: Must `ResolvedX` called from Guard and DA only; vocabulary declares `Fluent` absent. Assert zero findings for `ResolvedX`; a sibling `Other` with no Fluent call still yields `must-usage:fluent:Other`.
- `test/fixtures/ordering/string-family/`: `MustStringDeltaClauses` (`Delta`, `NotEmptyDelta`) with `GuardStringDeltaClauses`, `FluentStringDeltaExtensions`, `StringDeltaAttributes`, and `StringRules.Delta.cs` nested `Delta` (`IsDelta`, `IsNotEmpty`) → zero findings. `MustStringEchoClauses` with no `StringRules.Echo.cs` → exactly one finding whose message names `+ src/PineGuard.Core/Rules/StringRules.Echo.cs`. Existing `invalid/` Charlie case (`ordering.test.ts:62-83`) stays at four findings.

## 4. UnixTime and string DateTime siblings (exact names)

`ordering` derives sibling names from the Must tail (`ordering.ts:487, 541, 647, 664`), so T-06 must name them:

| Layer | File | Members | Tests | Unit |
|---|---|---|---|---|
| Rules | `+ src/PineGuard.Core/Rules/UnixTimeRules.cs` | `IsUnixSeconds`, `IsUnixMilliseconds` | `UnixTimeRulesTests` + `TestData`; `UnixTimeRulesFixtures.cs` | T-06 U2c |
| Must | `+ src/PineGuard.MustClauses/MustUnixTimeClauses.cs` | `UnixSeconds(long)`, `UnixMilliseconds(long)` | `MustUnixTimeClausesTests` + `TestData` | T-06 U3b |
| Guard | `+ src/PineGuard.GuardClauses/GuardUnixTimeClauses.cs` | `NotUnixSeconds`, `NotUnixMilliseconds` → `long` | `GuardUnixTimeClausesTests` + `TestData` | T-06 U4 |
| Fluent | `+ src/PineGuard.FluentValidation/FluentUnixTimeExtensions.cs` | `UnixSeconds()`, `UnixMilliseconds()` on `long` and `long?` | `FluentUnixTimeExtensionsTests` + `TestData` | T-06 U5 |
| DA | `+ src/PineGuard.DataAnnotations/UnixTimeAttributes.cs` | `UnixSecondsAttribute`, `UnixMillisecondsAttribute` (class-name exception recorded at T-00 §4.6 stands; the file follows `[Domain]Attributes.cs`, `data-annotations/project.md:114-115`) | `UnixTimeAttributesTests` + `TestData` | T-06 U5 |

Same for the string DateTime family: `GuardStringDateTimeClauses.cs`, `FluentStringDateTimeExtensions.cs`, `StringDateTimeAttributes.cs` (already at T-06 `:185`), Rules `StringRules.DateTime.cs` nested `DateTime` (T-06 U2a). T-02's `MustStringUtcOffsetClauses` already has `GuardStringUtcOffsetClauses.cs`, `FluentStringUtcOffsetExtensions.cs`, `StringUtcOffsetAttributes.cs`, `StringRules.UtcOffset.cs` nested `UtcOffset`, which the corrected discovery resolves.

## 5. Gates per unit and per phase

Cross-layer audits are necessarily red between a phase's Must unit and its last adapter unit (a concept exists in Must before Guard/Fluent/DA; `ordering` reports `missing-layer` until `GuardTimeZoneClauses.cs` exists). T-00 §9 therefore changes from "every command per unit" to:

- **Per unit**: format, diagnostics, tests, coverage per scope and TFM, `audit --gate`, `audit nullability` (Must/Guard units), `audit must-codes` (every unit touching Codes, MustClauses, GuardClauses or DataAnnotations; codes-only units and the T1-U2/U2b cohort rule stand).
- **Per phase cohort**, after the last layer unit and before the Fable phase review: full `pnpm -C apps/cli exec tsx src/index.ts audit` (all rules, baseline applied) must exit 0, and `git diff --stat apps/cli/config/baseline.json` must show deletions only. This makes "no new debt" mechanical and keeps `layer-parity`, `ordering`, `must-usage` and `rules-usage` strict and visible.
- Adjust the unit lines that currently promise mid-phase green: T-02 U3 `:253` (drop `ordering`), U4 `:287` (drop `layer-parity`), T-04 U3 `:268` (drop `ordering`), T-01 U4b `:573` (drop `layer-parity`), T-06 DoD `:193` (reword to the cohort run).
- Burn-down duty: the unit that deletes a member deletes every baseline key naming it. T-04 U2/U3b/U4b/U5a-2 delete `layer-parity:WithinDaysFromNow:DataAnnotations` (`:285`), `must-usage:annotations:WithinDaysFromNow` (`:473`), `must-usage:annotations:NotWithinDaysFromNow` (`:439`), and the `test-structure` keys at `:992, :1460, :1474`.

Rejected explicitly: any new `baseline.json` key; any `ignoreMethods` entry; any `exceptions.json` entry; `--update-baseline` in any unit or routine; fake Guard, Fluent or DA members.

## 6. Adjacent same-class defect, decided here

`TimeZoneRules.IsDaylightSavingTime` is planned as "rule only, no clause" (T-00 §4.2 `:251`; T-02 U2 `:120-121`). The `rules-usage` rule requires every public Rules method to have a Must call site (`audit-cli/spec.md:130`), so it would be a new un-baselined finding at the Phase 2 cohort gate. Decision: remove `IsDaylightSavingTime` and its fixture rows from T-02 U2; it stays on the T-00 §12 backlog with its clauses. No exemption.

Plan sections to edit: T-00 §2 (Phase 2 units 7→8), §5 (declared-absences table), §9 (gate model above); T-02 §2 and a new U0 section, U3 (vocabulary entry, acceptance), U4 (`NotResolvedTimeZone`), U2 (drop `IsDaylightSavingTime`); T-04 U3a (vocabulary entry) and baseline burn-down list; T-05 U3b-1 (vocabulary entries); T-06 U2a/U3b/U4/U5 (exact file names); `temporal-orchestration.md` §1 (T2-U0 row, cohort-gate wording in §3.3 step 2).