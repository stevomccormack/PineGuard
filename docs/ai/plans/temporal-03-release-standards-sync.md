<!-- metadata_header
type: plan
id: temporal-03-release-standards-sync
version: 1.0
status: planned
last_updated: 2026-09-22
-->

# Plan T-03 — Phase 3: Release `v0.1.0-alpha.8` and bring PineGuard.Standards into sync

> Charter: [T-00](temporal-00-program.md). Decisions: **D-5, D-8, S-01; A-04, A-05, A-12**.
> Depends on Phase 2 merged to PineGuard `main`.
> Repos: PineGuard (release only, from `main`) and **PineGuard.Standards**
> (`D:\Steve McCormack\GitHub\@stevomccormack\PineGuard.Standards`, worktree
> `.claude/worktrees/temporal-timezone`, branch `feature/temporal-timezone`).
> The Standards-local copy of this plan lives at
> `PineGuard.Standards/docs/ai/plans/temporal-timezone-sync.md` (plans are not mirrored by `sync` (the deterministic mirror command in `apps/cli`; not the agent-driven `sync`)).

## 1. Business plan

Standards consumes PineGuard from nuget.org at `0.1.0-alpha.7`. Nothing from Phases 1–2 reaches it
until a prerelease is cut. Cutting it also lands every unreleased change since alpha.7 (F-11), so
this phase measures actual package adaptation first (U2), then applies S-01/A-12 ownership and adapts (U3), then verifies host composition and versioned catalogs (U4–U6). The global D-12 tombstone is not resurrected.

### S-01 — Standards diagnostic ownership [A-12]

| Id | Decision | Recommendation | Rejected | Status |
|---|---|---|---|---|
| S-01 | Standards diagnostic ownership; D-12 remains a global tombstone. | Preserve existing owning Standards diagnostics. Required new adaptation codes use StandardsCodes body/domain computed Prefix grammar. U2 inventories exact pairs against live source; new canonical failure is iana.timezone.not-canonical. No nonexistent Core host-ID code or KnownTimeZone rename. | Empty codes or fake Core host ownership. | Astra final A-12; no owner re-signing |

## 2. Units

### Repo B reference refresh — download only [A-04]

This review refreshed references, not generated classes. The dated receipt is
`tools/code-generators/iana/iana-tz-zone-info/sources/temporal-reference-manifest.md` in the
Standards worktree. It records IANA 2026d and CLDR core XML release 48 from official stable tag
`release-48-2` pinned to commit `11299982335beb974c1c63c45265184e759c0f41`. Unicode archive requests
returned HTTP 522, so the official repository was the fallback. The XML is not cldr-json 48.2.1.

For each future refresh, discover current stable releases from the official IANA/Unicode release
pages, record retrieval UTC, then pin immutable artifact URLs before download. The following
commands replay the reviewed receipt from the Repo B worktree; versions are replay pins, not a
claim that these will always be latest:

```powershell
$ErrorActionPreference = 'Stop'
$referenceStage = Join-Path $PWD 'artifacts/temporal/reference-staging'
New-Item -ItemType Directory -Path $referenceStage -Force | Out-Null
Invoke-WebRequest -Uri 'https://data.iana.org/time-zones/releases/tzdata2026d.tar.gz' -OutFile (Join-Path $referenceStage 'tzdata2026d.tar.gz')
Invoke-WebRequest -Uri 'https://raw.githubusercontent.com/unicode-org/cldr/11299982335beb974c1c63c45265184e759c0f41/common/supplemental/windowsZones.xml' -OutFile (Join-Path $referenceStage 'windowsZones.xml')
Get-FileHash -Algorithm SHA256 -LiteralPath (Join-Path $referenceStage 'tzdata2026d.tar.gz')
Get-FileHash -Algorithm SHA256 -LiteralPath (Join-Path $referenceStage 'windowsZones.xml')
```

Validate HTTP success/content and hashes against the receipt (local checksums, not an asserted
upstream signature); for new releases verify upstream hashes when published and label provenance.
List archive entries before extraction and reject absolute paths / traversal. Extract only inside
staging, verify IANA `version`, preserve the complete archive and all selected standard Zone/Link
source files, plus README/LICENSE and zone.tab/zone1970.tab. Parse CLDR with DTD processing
prohibited and no external resolver; verify `supplementalData` / `windowsZones` / `mapTimezones`
shape before promotion. Validate actual XML data release and licenses, not a marketing patch label.

Only after every staged input validates, copy reference files into
`tools/code-generators/iana/iana-tz-zone-info/` and
`tools/code-generators/cldr/cldr-windows-zones/windowsZones.xml`, retain original sources under
their `sources/` directories, and update the manifest with URLs/version/UTC/hash/validation result.
Failure leaves prior valid references intact. Check `git diff --name-only`: no generated `.cs`,
catalog/project outputs or generator changes are permitted for a download-only refresh. Never run
Gen-Iana*, Run-Gen*, `-EnableUpdateTarget`, builds with generator hooks or generation targets.
Browser retrieval is last resort and must pass the same immutable provenance/content checks.
If Repo B is unavailable/unwritable, retain verified staging and report its path; do not claim promotion.

Future explicitly authorized U6 generation parses all selected standard tzdata Zone/Link files,
not only zone1970.tab/backward/etcetera. State the default dataset; optional backzone is not merged
blindly. Canonical means a Zone record in that pinned dataset. Resolve Link chains to final Zones
and reject cycles, dangling targets and duplicate conflicts. IANA 2026d and CLDR 48 have different
release cadence; unmapped IDs stay explicit, never inferred. [A-04]

| Unit | Title | Repo | Model / agent | Depends on | Fan-out |
|---|---|---|---|---|---|
| U1 | Cut `v0.1.0-alpha.8` | PineGuard | Owner + Shipper (interactive; Tier 1) | Phase 2 merged | — |
| U2 | Bump + adaptation discovery | Standards | Codex team (writes), GPT-6 Luna (discovery) | U1 actually published on nuget.org | — |
| U3 | Adaptation, per domain; preserve host resolver names | Standards | Codex team | U2, S-01 | ∥ across disjoint projects |
| U4 | Verify existing Standards host composition boundary | Standards | Codex team | U3 | — |
| U6a | tzdata `backward`+`etcetera`: generator + data + abstractions (D-8) | Standards | Codex team | U3 | ∥ U4 (disjoint files) |
| U6b | `backward`/`etcetera` links: canonical rule change + strict clauses + tests (D-8) | Standards | Codex team | U6a, U3 | — |
| U7 | Standards verification + review + merge | Standards | Claude Fable 5.1 per unit and phase, orchestrator | all | last |

## 3. Unit specifications

### U1 — Cut `v0.1.0-alpha.8` (owner-confirmed)

Pre-flight (orchestrator, GPT-6 Luna read): `git -C <PineGuard> status` clean on `main`; main contains the Phase 2 merge commit; record the current HEAD independently (later phase merges may already be present); `git tag --list | Select-Object -Last 1` is `v0.1.0-alpha.7`.
Then the owner runs, interactively:

```
/github-release-publish -Version 0.1.0-alpha.8 -Watch
```

(the agent playbook `docs/ai/agents/github-release-publish.md`; do not pass `-Force` or
`-BypassPR` unless the owner says so). Success = the publish workflow run is green and
`https://www.nuget.org/packages/PineGuard.Core/0.1.0-alpha.8` (and the other five packages
Standards pins) resolve. Record the run URL in the tracker. **A routine must not do this unit.**

### U2 — Bump + adaptation discovery (Codex team writes; GPT-6 Luna reads)

Editing `Directory.Packages.props` is a Tier 1 action (dependency change); the orchestrator obtains
the owner's confirmation before dispatching this unit (it may be the same confirmation as U1).
In the Standards worktree:

1. `Directory.Packages.props`: change every `PineGuard.*` `PackageVersion` from `0.1.0-alpha.7` to
   `0.1.0-alpha.8` (six lines: Core, MustClauses, GuardClauses, DataAnnotations, FluentValidation, Testing).
2. `dotnet restore PineGuard.Standards.slnx` then
   `dotnet build PineGuard.Standards.slnx -c Debug 2>&1 | Tee-Object artifacts/temporal/bump-build.log`
   (create `artifacts/temporal/`).
3. Produce `artifacts/temporal/bump-report.md`: actual error counts per project and exact API
   diagnostic groups, actual restored package versions/XML signatures, owning existing diagnostic
   codes and only demonstrably required new ones, disjoint U3 file/domain ownership and evidence
   of necessary references. Do not invent a StandardsCodes architecture or add project references
   merely because a removed D-12 proposal suggested them. S-01 means body/domain ownership follows
   live Standards precedent and actual package requirements, not a second owner decision gate.
4. Do **not** fix anything. Report the counts.

Also inventory consumer call-sites whose behavior changes without a compiler error:
`IsInPast`, `IsInFuture`, `HasMinimumAge`, `IsSameDay`, and TimeSpan threshold guards.
Record actual caller assumptions, before/after boundary vectors and required migration tests
in `bump-report.md`; a zero build-error count does not establish temporal compatibility.
[Fable corrective council]

Review the mechanical table against live Brain and S-01/A-12. Claude Fable 5.1 handles any
implementation discrepancy; no new owner signature and no D-12 resurrection.
Budget 60k / 15 min.

### U3 — Adaptation, per domain group (Codex team ×N, ∥)

Each dispatch receives one U2 domain group and the S-01 diagnostic inventory. Mechanical adaptations only; live Brain precedent controls shapes. [A-12]
rewrites only:

| From (alpha.7 shape) | To (alpha.8 shape) |
|---|---|
| `MustResult<T>.FromBool(ok, messageTemplate, paramName, value, result)` | `MustResult<T>.FromBool(ok, StandardsCodes.<Body>.<Subject>.<Condition>, messageTemplate, paramName, value, result)` |
| `MustResult<T>.Fail(messageTemplate, paramName, value)` | the alpha.8 `Fail` overload that takes a code (read `PineGuard.Core` XML docs in the restored package: `~/.nuget/packages/pineguard.core/0.1.0-alpha.8/lib/net10.0/PineGuard.Core.xml`) |
| `GuardFailure.Throw(message ?? result.Message, paramName, value, exceptionCreator)` | `GuardFailure.Throw(result, message, exceptionCreator)` |
| `FluentXxx` `ruleBuilder.MustBe(val => …, message)` | `ruleBuilder.MustBe(val => …, message, <code>)` |
| DataAnnotations `ValidationAttributeBase(typeof(T))` | `ValidationAttributeBase(typeof(T), <code>)` |
| Any other group listed by U2 | exactly as the U2 report prescribes |

Codes files are created by the **first** group dispatched (`iso-countries` owns
`StandardsCodes.cs` root + `StandardsCodes.Iso.cs`; each body group owns its own partial file).
Tests: `MustExpected` rows gain `Code` where the test already asserts a message. Each dispatch
ends with `dotnet build` of its own projects and `dotnet test` of its own test projects only
(scope discipline; the hook serialises).

**Astra override D-7 [A-12]**: preserve current Standards SystemTimeZone names, host-resolution
behavior and owning diagnostics. There is no competing Core host family, so no collision rename
or delegation to unavailable Core utilities is needed. Bundled IANA membership and canonical
membership remain deterministic separate APIs; never relabel host-backed lookup KnownTimeZone.
**D-6 acceptance check (all groups)**: every domain group's acceptance additionally requires zero
Standards references to `IsWithinDaysFromNow` / `NotWithinDaysFromNow` (deleted from PineGuard at
every layer in Phase 4, D-6). Confirmed 2026-09-26: `grep -rn
"IsWithinDaysFromNow|NotWithinDaysFromNow" src/` in the Standards worktree returns no matches today,
so this check currently passes vacuously; it must stay green as later PineGuard releases are
absorbed, so add it to each group's acceptance script rather than treating it as already satisfied
forever.

**Acceptance per group**: its projects build warning-free; its tests pass; `git diff --stat`
limited to the group's projects (+ the codes file it owns), plus the A-12 host-name preservation and D-6 checks above.
Budget 150k / 45 min each; no D-7 rename scope exists under A-12.

### U4 — Verify existing host composition boundary [A-05, A-12]

**Files** (Standards): `src/PineGuard.Standards.Iana.TimeZones/Utils/SystemTimeZoneUtility.cs`,
`src/PineGuard.Standards.Iana.TimeZones/Utils/CldrTimeZoneUtility.cs`, their tests.

Preserve existing `SystemTimeZone` public clauses, utilities, resolver behavior, diagnostics and
CLDR fallback. There is no Core host-ID utility to delegate to. Verify actual BCL API availability
on each target and runtime/OS/globalization combination; retain necessary target forks. Document
registry/files/ICU access and host dependence honestly. Separate host resolution from bundled
IANA membership/CLDR mapping; never assert zero legitimate BCL resolver calls.

**Acceptance**: Iana.TimeZones + Cldr.TimeZones build and test; coverage 100 % for both projects.
Budget 100k / 30 min.

> U5 stays removed; A-12 removes the prior rename task entirely. Preserve unit IDs for tracker continuity.
### U6a — tzdata `backward`+`etcetera` links: generator + data + abstractions (D-8)

**Later implementation authorization required [A-04; owner constraint]**: this unit's
generator changes and generated classes are future program design, not actions authorized
by the current plan review or reference refresh. The current refresh only downloads, verifies
and promotes reference files/provenance. No refresh command invokes or chains into a
generator. Do not dispatch U6a/U6b generation work until separately and explicitly authorized.

**Source files**: `tools/code-generators/iana/iana-tz-zone-info/backward` and `etcetera`,
selected from the pinned `tzdata2026d.tar.gz` already verified in the A-04 manifest receipt,
with its recorded hash/version and the archive provenance retained. Do not fetch mutable
latest `tzdb/backward` or `tzdb/etcetera` URLs, and do not mix releases. Commit the validated
reference inputs alongside `zone1970.tab`; generation remains a separate authorized action.
Record the same version in the generator README. Lookup (b) on 2026-09-26
confirmed `Etc/UTC`, `UTC` and `Etc/GMT+1` are all **absent** from the existing
`DefaultIanaTimeZoneData.cs` (a direct read of the file and a case-insensitive grep for `utc`/`gmt`
both returned zero matches) — `zone1970.tab` excludes the `Etc/*` zones and the bare `UTC`/`GMT`
links by design, so ingesting `backward` alone would still leave `UTC` unresolvable. `etcetera`
supplies both real zones (`Zone Etc/UTC`, `Zone Etc/GMT`, `Zone Etc/GMT+1` … `Etc/GMT-14`) and its
own `Link` lines (`Link Etc/UTC UTC`, `Link Etc/UTC Universal`, `Link Etc/UTC Zulu`, `Link Etc/GMT
GMT`, …).

**Files**: extend `tools/code-generators/iana/Gen-IanaTimeZones.ps1` (or add
`New-IanaTimeZoneLinkDataGenerator.ps1` beside the existing generator, following that file's shape)
to parse lines matching `^Link\s+(\S+)\s+(\S+)` (target, link) from **both** `backward` and
`etcetera`, and emit
`src/PineGuard.Standards.Iana.TimeZones/Abstractions/DefaultIanaTimeZoneLinkData.cs` with the same
header comment convention ("generated from IANA tzdb backward + etcetera; do NOT edit"). Separately,
extend the existing zone generator to parse `etcetera`'s `^Zone\s+(\S+)` blocks (no country codes;
`null` for the country-code column) into `DefaultIanaTimeZoneData.cs` alongside the `zone1970.tab`
rows, so `Etc/UTC`, `Etc/GMT` and `Etc/GMT±N` become canonical, resolvable ids. Add to
`PineGuard.Standards.Iana.TimeZones.Abstractions`: `public sealed record IanaTimeZoneLink(string LinkId, string TargetId)`;
extend `IIanaTimeZoneProvider` with `bool TryResolveLink(string? value, out string? canonicalId)`;
implement in `DefaultIanaTimeZoneProvider` (case-sensitive ordinal lookup, trimmed input; a
canonical id resolves to itself and returns `true`; unknown → `false`). Update the generator README
and the Standards agent playbook `docs/ai/standards/agents/generate-iana.md` to run both the zone
and link generation, and to record the `etcetera` version alongside `backward`'s.

**Tests**: provider tests: `US/Eastern` → `America/New_York`; `Asia/Calcutta` → `Asia/Kolkata`;
`UTC` → `Etc/UTC`; `America/New_York` → itself; `Etc/UTC` → itself; `Not/AZone` → false; null/empty
→ false. Generator test: add `tools/.tests/Gen-IanaTimeZoneLinks.Tests.ps1` (Pester, same header as
the existing files in `tools/.tests/`) with cases: parses `Link\tAmerica/New_York\tUS/Eastern` →
(`US/Eastern`, `America/New_York`); parses `Link\tEtc/UTC\tUTC` → (`UTC`, `Etc/UTC`); ignores `#`
comment lines and blank lines. If `tools/.tests/` does not exist in Standards, create it with just
this file and register it the way the upstream `Test-Tools.ps1` runner does.

**Acceptance**: generated files regenerate byte-identically (`-EnableUpdateTarget $true` then
`git diff --exit-code`); build, tests, coverage green. Budget 150k / 45 min.

### U6b — `backward`/`etcetera` links: canonical rule change + strict clauses + tests (D-8)

The council's 2026-09-26 verdict **REJECTED** the canonical-only default plus an `*OrLink` opt-in
family (`IsIanaTimeZoneIdOrLink`, `Must.Be.IanaTimeZoneIdOrLink`,
`Guard.Against.NotIanaTimeZoneIdOrLink`, `.IanaTimeZoneIdOrLink()`) — that design rejects
`Europe/Oslo`, `Europe/Amsterdam`, `Europe/Stockholm`, `Europe/Copenhagen` and `US/Eastern` under
the default clause name even though every OS accepts them. Links are accepted **by default**
instead; a new strict opt-in family covers the canonical-only case.

**Files**: `IanaTimeZoneRules.cs` — `IsIanaTimeZoneId(string? value, IIanaTimeZoneProvider? provider
= null)` is changed **in place** (not renamed) to accept canonical ids **and** links, falling back
to the U6a `TryResolveLink` when `IIanaTimeZoneProvider.IsValidTimeZoneId` returns `false`; add a
new, separate `IsCanonicalIanaTimeZoneId(string? value, IIanaTimeZoneProvider? provider = null)`
(canonical ids only — no link fallback). `MustIanaTimeZoneClauses.cs` (+ `CanonicalIanaTimeZoneId`
→ `MustResult<string>`, message `"{paramName} must be a canonical IANA time zone id."`; code: the Standards-owned canonical diagnostic is `iana.timezone.not-canonical`, using the
actual body/domain computed-prefix precedent. Do not defer this semantic choice, resurrect D-12,
invent a Core host-ID code or require owner confirmation. [A-12]).
`GuardIanaTimeZoneClauses.cs` (+ `NotCanonicalIanaTimeZoneId`), Fluent (+
`.CanonicalIanaTimeZoneId()`), DataAnnotations (+ `CanonicalIanaTimeZoneIdAttribute`), fixtures
(`TimeZoneIanaRulesFixtures.IsCanonicalIanaTimeZoneId`: `Canonical = "America/New_York"`,
`Link = "US/Eastern"`, `Invalid`, `NullValue`, `Empty`), tests for all four layers.
`IsIanaTimeZoneId`'s `<summary>` first sentence now states it accepts canonical ids and tzdata
links, and cross-references `IsCanonicalIanaTimeZoneId` for the strict form.

**Tests**: `Europe/Oslo`, `Europe/Amsterdam`, `Europe/Stockholm`, `Europe/Copenhagen`, `US/Eastern`
are accepted by `IsIanaTimeZoneId` / `Must.Be.IanaTimeZoneId`, and rejected by
`IsCanonicalIanaTimeZoneId` / `Must.Be.CanonicalIanaTimeZoneId`. `TryResolveLink("Europe/Oslo")` →
`Europe/Berlin`; `TryResolveLink("US/Eastern")` → `America/New_York` (both provider-level checks,
from U6a).

**Acceptance**: Standards gate green for the Iana.TimeZones family; `git grep -n
"IanaTimeZoneIdOrLink" src tests` returns nothing (the rejected family was never committed).
Budget 150k / 45 min.

### U7 — Standards verification + review + merge

Run, from the Standards worktree, the commands exactly as written in Standards'
`docs/ai/workflows/format.md`, `scan-roslyn.md`, `test.md`, `coverage.md` (every affected scope,
both TFMs) and `docs/ai/workflows/audit.md` if present; then Qodana and Sonar per its
`scan-qodana.md` / `scan-sonar.md`. Claude Fable 5.1 review per unit; Claude Fable 5.1 phase review with the U2 report
attached (it must confirm every adaptation is mechanical and no unjustified message or behavior change; actual required diagnostic changes are evidenced against the restored package graph). Commit via Standards' `/commit-*` agents with the body pattern from commit
`97131d28` (`chore(deps): bump PineGuard upstream pins to 0.1.0-alpha.8` + rationale), then
`git merge --no-ff feature/temporal-timezone` into Standards `main`, push, remove the worktree.

## 4. Definition of Done

- Standards `main` builds against `0.1.0-alpha.8` with zero warnings; all tests green; 100 % coverage per TFM.
- Existing Standards host resolution is retained and documented; deterministic membership is separately tested. [A-12]
- Existing Standards host clauses retain current names/behavior; host integration tests declare runtime/OS/globalization prerequisites. [A-12]
- `Must.Be.IanaTimeZoneId("US/Eastern").Success == true`; `Must.Be.CanonicalIanaTimeZoneId("US/Eastern").Success == false`.
- Existing Standards host clauses retain current names/behavior; host integration tests declare runtime/OS/globalization prerequisites. [A-12]

## 5. Risks

| Risk | Mitigation |
|---|---|
| Adaptation debt (F-11) is larger than a day | U2 measures before anyone edits; the owner may pause after U2. U3 fans out. |
| `backward`/`etcetera` download needs network the cloud lacks | Owner commits both files; U6a never downloads inside a routine. |
| Cloud runs cannot cut releases | U1 is owner-only; the routine's prompt refuses it. |
