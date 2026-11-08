<!-- metadata_header
type: evidence
id: temporal-fable-council-transcript
version: 1.0
status: complete
last_updated: 2026-09-26
-->

# Revised temporal program — Claude Fable 5.1 council transcript

This is the public-output record of the revised 11-call council and its corrective follow-ups. It preserves the earlier historical transcript separately. Only assistant `text` content and per-call provenance are included; hidden reasoning, thinking blocks, signatures, and tool-intent-only output are excluded. A completed invocation is not itself an approval. The formal chairman verdict was **not accepted as written**; follow-up corrections below control where explicitly stated, and the final plan acceptance gate remains open until a later accepted review.

## Run provenance

- CLI: Claude Code 2.1.283; requested/canonical model: `claude-fable-5-1`; provider: `firstParty` as recorded by verified call provenance.
- 11 successful calls: five advisors, five randomized peer reviewers, one chairman. Each call has nonzero model usage; all initialized with no tools and no MCP servers.
- Failed pre-model attempts are not counted. The original `--bare` authentication failure is recorded in the rerun receipt; `--bare` skipped keychain/OAuth. The successful rerun omitted `--bare` and used `--permission-mode dontAsk`, strict empty tool configuration, no session persistence, and one turn.
- Anonymous advisor mapping: A = First-principles; B = Contrarian; C = Executor; D = Expansionist; E = Outsider. Peer reviewers are identified only by randomized ordinal.

## Advisor — first-principles (anonymous response A)

**Provenance:** requested/canonical model `claude-fable-5-1`; provider `firstParty`; input 2, cache-created input 155893, cache-read input 4020, output 1867 tokens; duration 29336 ms; successful `True`.

**The underlying goal** is not "ship zone awareness"; it is "every PineGuard temporal verdict is reproducible on any host." Judge the program against that, and one assumption fails while everything else holds.

**The assumption:** D-1 says provider `LocalTimeZone` defines "today," and the plan treats that as deterministic. It is only deterministic when a provider is supplied. Every existing signature defaults `timeProvider` to `null`, which resolves to `TimeProvider.System`, whose `LocalTimeZone` is `TimeZoneInfo.Local`: the host registry or tzdata. So after Phase 1, `DateOnlyRules.IsInPast(new DateOnly(2026,6,15))` evaluated at 2026-06-15T23:30Z returns different answers on a Sydney host and a UTC container. Today it returns the same answer everywhere. The program removes host dependence from `IsSameDay` (D-2) on exactly the grounds "non-deterministic across machines," then reintroduces the same defect into the most-called calendar predicates by default. T-01 U2 even concedes it: "null-provider fallback is a separate host integration check."

**This meets the T-00 §0 reopening test**, not preference: reproducible (one fixture, two hosts, two results), governed by spec §5.3 and A-05's own reasoning, with a root-cause alternative that needs no Brain exception: when no provider is supplied, "today" stays the UTC calendar date and is documented as such; an explicit provider's `LocalTimeZone` defines today. Hosts without an injected clock keep reproducible behaviour; callers who want civil today opt in visibly, matching A-03's "policy visible at call sites." Fable adjudication is still required to reopen D-1.

**Reframing:** the missing artefact is a one-line contract, "no environment reaches a verdict unless the caller injects it," applied to every default parameter. The rest of the program already obeys it (A-05, A-06, A-10, custom test zones). D-1's null path is the sole breach.

**Recommendation:** do not approve as written. Amend D-1's null-provider basis to UTC date, restate T-01 U2 fixtures, then proceed. The InspectCode gate does not affect readiness.


## Advisor — contrarian (anonymous response B)

**Provenance:** requested/canonical model `claude-fable-5-1`; provider `firstParty`; input 2, cache-created input 155909, cache-read input 4020, output 9462 tokens; duration 123330 ms; successful `True`.

**Recommendation: do not approve as written.** One architectural correction meets the T-00 §0 reopening test; two plan-consistency defects will fail the program's own gates on the first phase.

**1. D-1 smuggles host-zone IO into Core through the default parameter.** A-05 bans Core host-zone lookup by ID under spec §5.3, yet D-1 makes every `DateOnlyRules.IsInPast` / `HasMinimumAge` call with `timeProvider = null` read `TimeProvider.System.LocalTimeZone`, which is `TimeZoneInfo.Local`, which is the same registry/tzdata read A-05 forbids. Reproducible evidence: identical input, two containers with different `TZ`, at 23:30Z, return different `Must.Be.MinimumAge` results. D-14's own rationale ("could flip the computed age depending on server location") is precisely the defect D-1 institutionalises for `DateOnly`/`DateTime`. `FixedTimeProvider` hides it from CI entirely. Root-cause alternative: null provider projects the UTC date; the local calendar is used only when a provider or zone is supplied explicitly. Fable must adjudicate before T1-U2 starts.

**2. T-01 U2 contradicts itself and the gate.** Its "exact output" for `HasMinimumAge` uses `today.AddYears(-years)`, which A-07 forbids; T-04 then rewrites the same member via `TryGetAge`, which T-01 cannot call because it does not exist yet. U2 also registers `Date.Reference.OutOfRange` with a Must attribution but lists no Must file and scope `Core` only, so the constant is unreferenced at the Phase 1 gate and `audit must-codes` fails. Move `TryGetAge` and the Must wiring into T-01, or delete the promise.

**3. T-02 violates the codes audit twice.** `MustTimeZoneClauses.SameDay` reuses `Date.Equality` constants, breaking "a clause file references only its own mapped domain". `timezone.id.unknown` is produced only by DataAnnotations with invented message text, contradicting §5.5 and the DA-code-must-match-clause check.

Fix these, then re-gate. The InspectCode workstream does not affect readiness.


## Advisor — executor (anonymous response C)

**Provenance:** requested/canonical model `claude-fable-5-1`; provider `firstParty`; input 2, cache-created input 155898, cache-read input 4020, output 2192 tokens; duration 32663 ms; successful `True`.

**Recommendation: approve, with one executable correction to T-01 U2 before Phase 1 opens.**

**The gap that has no path.** The build prompt says "if something is not specified, stop." T-01 U2 will stop on day one. Its A-07 prose assigns it `DateTimeOffsetUtility.TryGetDateAtOffset`, `TryGetTodayDate`, a new `MustCodes.Date.Reference.OutOfRange` constant, a Must consumer of that constant, and counting/advancing-provider fixtures. Its file table lists none of those files: no `DateTimeOffsetUtility.cs`, no `MustCodes.Date.cs`, no `MustDateOnlyClauses.cs`, no counting provider anywhere in U0/U1. Worse, the constant is consumed only in T-04, so the §9 `audit must-codes` gate fails for every Phase 1, 2 and 3 merge on an unreferenced constant. Fix: move the `Date.Reference.OutOfRange` registration and its Must attribution to T-04 U1/U3 where the consumer lands, keep the internal helpers in T-01 U2 with the two utility files added to its table, and add `CountingTimeProvider` to U0's file list. Also confirm the "existing internal-testing precedent" (InternalsVisibleTo) actually exists in Core before U2 relies on it.

**Smallest safe start, Monday.** U0, then U4 alone. U4 is the highest-value change in the program, touches two source files, needs no zone work, and has a one-line decisive test: `Guard.Against.LessThan(5m, 5m)` does not throw. Ship it as its own commit before any rename.

**Decisive tests per phase.** Phase 1: `IsInPast(2026-06-15, DefaultAheadOfUtc)` flips to true; counting provider reads once. Phase 2: gap/overlap with `Kind.Utc` fails as a value failure. Phase 4: `AtMin`/`AtMax` true. Phase 6: both strict profiles reject offset-free input.

**Dependency reality.** Phases 4/5/6 are only independent if `MustCodes.Date.cs` truly has one writer per phase; the merge order 4→5→6 already forces serialisation, so run them sequentially and skip the parallel-worktree ceremony.

The tooling gate does not affect readiness; nothing here needs InspectCode.


## Advisor — expansionist (anonymous response D)

**Provenance:** requested/canonical model `claude-fable-5-1`; provider `firstParty`; input 2, cache-created input 155895, cache-read input 4020, output 1858 tokens; duration 34207 ms; successful `True`.

**Recommendation: approve as written, with one additive gap closed before T-01 U0 dispatches.**

The program undervalues its own platform upside. Three things it builds are worth more than the predicates they serve:

- **A-05 makes PineGuard the portable choice.** Supplied `TimeZoneInfo` in Core means every zone rule works under invariant globalization, Blazor WebAssembly, trimmed and NativeAOT apps, where host-ID lookup fails. Competitors that resolve IDs internally cannot claim this. T-07 U1 should state it as a headline README property, not bury it in `<remarks>`.
- **`TestTimeZones` and zone-aware `FixedTimeProvider` are a shippable product.** `PineGuard.Testing` is a published NuGet package (testing/project.md §1). A custom DST zone with documented gap and overlap instants, plus the half-hour transition zone from T-01 U1, is exactly the fixture every consumer writing DST tests wishes existed. Document it as a consumer feature in T-07.
- **The code catalogue becomes a contract.** `date.kind.not-utc`, `date.reference.out-of-range` and the `timezone.*` family (A-12, T-00 §5) let adapters and Standards branch on cause without parsing prose. That is the seam a future `IStringLocalizer` pass (T-00 §12) plugs into cheaply.

The gap: A-03 and T-01 U0 require "counting/advancing-provider read-contract fixtures", and T-04 U2 requires "a counting provider and a provider that advances on each read", yet no unit names a file, type or owner. Three phases need it, which meets testing/project.md §3.1's two-consumer bar, so it belongs in `PineGuard.Testing` as a named public type specified in T-01 U0. Left unspecified, each Sonnet dispatch invents a private double, violating T-00's "nothing asks an implementer to decide" contract. This is a completeness fix, not an Astra reopening.

Adjacent cheap wins to log, not add now: `IsTimeOfDayBetween(instant, open, close, zone)` is nearly free once T-01 U3 wrapping and T-02 offset projection land, and `IsDaylightSavingTime` clauses cost one Must row each.


## Advisor — outsider (anonymous response E)

**Provenance:** requested/canonical model `claude-fable-5-1`; provider `firstParty`; input 2, cache-created input 155896, cache-read input 4020, output 3280 tokens; duration 43037 ms; successful `True`.

**Recommendation: do not approve as written. One structural change is necessary; the rest is clarity.**

**The necessary change: error-code ownership and attribution are internally contradictory.** Reading only the packet:

- T-01 U2 registers `MustCodes.Date.Reference.OutOfRange` with a message attributed to `timeProvider`, but its file list contains no `MustCodes.Date.cs` and no Must file. Nothing in Phase 1 can emit that code, so the `must-codes` audit ("every declared constant is referenced") fails, and `Must.Be.Past(DateOnly)` keeps reporting a provider projection failure as "must be in the past". That is exactly the §5.4.2 misattribution the plan claims to prevent.
- The `Date.Kind` node is created by both T-02 U1 (`NotUnspecified`) and T-04 U1 (`NotUtc`). Phases 2 and 4 are declared concurrent and `MustCodes.*.cs` is single-writer (§7.3). Nobody owns the node.
- `timezone.id.unknown` is declared in Core `MustCodes.TimeZone` but produced only by a DataAnnotations adapter. The audit rule states a DA attribute's code must match what the clause it invokes produces; `Must.Be.ValidLocalTime` never produces it.
- D-8 says the canonical condition is `canonical`; §4.3 and T-03 U6b say `not-canonical`.

Fix: one table in T-00 §5 listing every new node, the unit that creates it, the clause that emits it, and the audit exemption where an adapter owns it. Then add the Must-layer edits to T-01 U2's file list or move the code to T-04.

**Confusing assumptions an implementer cannot resolve from the packet:** "Astra", "Luna" and "Sol" appear nowhere in the Brain, yet outrank a Fable council unless a reopening test is met. The T-00 header carries `council_verdict_ref` dated 2026-09-26 while the rerun receipt says no council ran. The tracker marks P0 Done and T1-U0 "Blocked (P0)". §6.3 names `TestTimeZones.PlatformSample`; T-01 defines only `PlatformSampleId`. "Existing internal-testing precedent" and "existing internal-rule precedent" are cited without a path.

The tooling gate does not affect readiness; the contradictions above do.


## Peer reviewer — randomized reviewer 1

**Provenance:** requested/canonical model `claude-fable-5-1`; provider `firstParty`; input 2, cache-created input 159954, cache-read input 4020, output 4068 tokens; duration 55105 ms; successful `True`.

**1. Strongest: B.** It alone pairs the architectural correction with the §0 reopening evidence: T-01 U2's `TryGetTodayDate` selects `timeProvider ?? TimeProvider.System` and reads `LocalTimeZone`, which is `TimeZoneInfo.Local`, so the default path is the host read A-05 bans under spec §5.3 (T-01 §6 even concedes "when the caller does not pin a provider"). It also catches the `today.AddYears(-years)` snippet A-07 forbids and the `Date.Reference.OutOfRange` constant with no Must consumer before T-04, which fails the §9 `must-codes` gate. A makes the same D-1 point more elegantly but single-issue; E's node-ownership table (Date.Kind written by both T-02 U1 and T-04 U1 despite "No other file" and §7.3 single-writer; D-8 `canonical` vs §4.3 `not-canonical`) is the best complement to B.

**2. Biggest blind spot: D.** It recommends approval and headlines A-05 portability while the D-1 null-provider path silently breaks exactly that portability, and it ignores every gate-failing code contradiction.

**3. All five missed:** T-01 U3 adds midnight-wrap rows expecting `true` at Must/Guard/Fluent/DA with "clause code unchanged," yet the Must spec's canonical `Between` pattern fails `min > max` up front with `Range.Invalid`. If `MustTimeOnlyClauses.Between` follows that precedent, the wrap never reaches the rule above Core, and A-09's inclusion contract, the U3 fixtures and `layer-parity` diverge. Someone must read that clause before U3 dispatches.


## Peer reviewer — randomized reviewer 2

**Provenance:** requested/canonical model `claude-fable-5-1`; provider `firstParty`; input 2, cache-created input 159954, cache-read input 4020, output 4552 tokens; duration 61256 ms; successful `True`.

**1. Strongest: B.** It pairs a genuine T-00 §0 reopening (D-1's null provider reads `TimeZoneInfo.Local`, contradicting A-05/§5.3 and D-14's own rationale) with three verifiable gate failures: T-01 U2's `today.AddYears(-years)` snippet violates A-07, `Date.Reference.OutOfRange` has no Must file or consumer until T-04 so `must-codes` fails at Phase 1, and `MustTimeZoneClauses.SameDay` reusing `Date.Equality` breaks the own-domain audit rule. E is close and adds the dual-writer `Date.Kind` node (T-02 U1 vs T-04 U1, §7.3) and D-8 `canonical` vs §4.3 `not-canonical`.

**2. Biggest blind spot: D.** It approves while ignoring that its "portable" headline is undercut by A-03 itself: every un-injected `DateOnlyRules.IsInPast` call now consults the host zone. It also misses every codes contradiction B/C/E found. C's "U4 alone Monday" violates U4's declared dependency on U1.

**3. All five missed:** `System.Globalization.ISOWeek` is absent from `netstandard2.1`; T-05 U2b's "one code path on every TFM" will not compile, and §6.2 guards only DateOnly. Legacy `DateTime` instant rules still normalise via `DateTimeUtility.ToUtc` (core/project.md §6.2), so `Kind.Local` host conversion survives in `IsInPast`/`IsBefore`, and T-06's `StringRules.DateTime` mirrors them. Routine environments pin `TZ=UTC`, guaranteeing CI never surfaces the null-provider defect.


## Peer reviewer — randomized reviewer 3

**Provenance:** requested/canonical model `claude-fable-5-1`; provider `firstParty`; input 2, cache-created input 159954, cache-read input 4020, output 3674 tokens; duration 50836 ms; successful `True`.

**1. Strongest: Response B.** It is the only response that both meets the T-00 §0 reopening bar and audits the plan's own gates. Its D-1 null-provider argument is reproducible (T-01 U2 resolves `null` to `TimeProvider.System`, whose `LocalTimeZone` is `TimeZoneInfo.Local`), governed by spec §5.3 and A-05, and offers a root-cause alternative needing no Brain exception. Its second point is verifiable in the packet: T-01 U2's exact output uses `today.AddYears(-years)` while A-07 forbids threshold AddYears, and the `Date.Reference.OutOfRange` registration has no Must consumer until T-04. Response A makes the same D-1 case more elegantly but stops there; Response E catches the most internal contradictions (dual `Date.Kind` writers, `canonical` vs `not-canonical`, `PlatformSample` vs `PlatformSampleId`) but misses the architectural defect.

**2. Biggest blind spot: Response D.** It approves as written without engaging any correctness risk, and its "shippable `TestTimeZones` product" framing ignores that the same unit list contradicts A-07. Response C shares the blind spot and also proposes "U0 then U4," though U4 depends on U1.

**3. Missed by all five:** A-06 makes every new `DateTime` instant predicate (`NotExpired`, `WithinLast`, …) require `Kind.Utc`. ASP.NET model binding and `System.Text.Json` routinely produce `Local`/`Unspecified`, so the T-04 DA attributes and Fluent extensions on `DateTime` properties will fail real user input with `date.kind.not-utc`. Also, the routine environment pins `TZ=UTC` (orchestration §3.1), which structurally masks the D-1 host-zone dependence in every cloud run.


## Peer reviewer — randomized reviewer 4

**Provenance:** requested/canonical model `claude-fable-5-1`; provider `firstParty`; input 2, cache-created input 159954, cache-read input 4020, output 5752 tokens; duration 80075 ms; successful `True`.

**1. Strongest: E.** Every defect it names is verifiable in the packet and gate-blocking: `Date.Reference.OutOfRange` registered in T-01 U2 with no Must file (fails §9 `must-codes`, and the unit is not on the codes-only skip list); `Date.Kind` written by both T-02 U1 and T-04 U1 while T-00 §2 lets Phases 2 and 4 run concurrently and §7.3 makes `MustCodes.*.cs` single-writer; `timezone.id.unknown` declared in Core but emitted only by DA; D-8 `canonical` versus §4.3/U6b `not-canonical`; `PlatformSample` versus `PlatformSampleId`; a `council_verdict_ref` stamped despite the rerun receipt. B is close and adds the U2 `AddYears` contradiction with A-07, but its D-1 reopening is contestable: A-03 explicitly chose provider `LocalTimeZone`, `GetUtcNow` on `TimeProvider.System` is already host-dependent, and A/B's alternative creates two "today" semantics depending on whether a provider is passed.

**2. Biggest blind spot: D.** It approves as written while T-01 U2 cannot pass its own gate, and treats the counting provider as the only gap.

**3. All five missed:** (a) T-03 provenance conflict: U6a has the owner commit `backward`/`etcetera` from unpinned "latest" tzdb URLs while A-04 mandates pinned, hashed artifacts; (b) routines set `TZ=UTC` and tests pin `FixedTimeProvider`, so the null-provider D-1 path is never exercised on a non-UTC host, whichever way D-1 resolves.


## Peer reviewer — randomized reviewer 5

**Provenance:** requested/canonical model `claude-fable-5-1`; provider `firstParty`; input 2, cache-created input 159954, cache-read input 4020, output 4763 tokens; duration 68009 ms; successful `True`.

**1. Strongest: B.** It pairs the one arguable architectural reopening (D-1's null path reads `TimeZoneInfo.Local`, cutting against A-05/§5.3) with three executable defects nobody else fully caught: T-01 U2's `today.AddYears(-years)` contradicts A-07, T-01 needs T-04's `TryGetAge`, and `MustTimeZoneClauses.SameDay` borrowing `Date.Equality` codes breaks the must-codes domain rule. E is a close second; its Date.Kind double-writer across concurrent Phases 2 and 4 and the `canonical` versus `not-canonical` split (D-8 vs §4.3/T-03 U6b) are genuine plan contradictions.

**2. Biggest blind spot: D.** It approves as written despite T-01 U2 being unexecutable under the build prompt's "stop if unspecified" rule, and its fix adds a new public type to the shipped `PineGuard.Testing` package with no §4 naming row or rejected alternatives, which T-00 §1.3 requires.

**3. All five missed:** A and B's alternative leaves `Must.Be.Past(d, TimeProvider.System)` host-dependent, so it narrows the defect rather than removing it. More importantly, D-1, D-2 and D-9 are silent runtime semantic changes with no compile signal, yet T-03 U2 measures Standards adaptation solely by build-error counts. Standards may compile green against alpha.8 and change verdicts. T-03 needs a behavioral diff step, not just error counts.


## Chairman — formal council verdict

**Provenance:** requested/canonical model `claude-fable-5-1`; provider `firstParty`; input 2, cache-created input 163145, cache-read input 4020, output 14259 tokens; duration 185461 ms; successful `True`.

## Council Verdict: PineGuard temporal program

### Where the Council Agrees

- **Not ready as written.** The Contrarian, First Principles Thinker and Outsider say do not approve; the Executor and Expansionist approve only with a correction. All five reviewers agree the plan cannot pass its own gates in Phase 1, and all five name the Expansionist as the weakest voice for approving without engaging that.
- **T-01 U2 is unexecutable under the build prompt's "stop if unspecified" rule.** Its prose assigns `DateTimeOffsetUtility.TryGetDateAtOffset`, a new `MustCodes.Date.Reference.OutOfRange` constant, a Must attribution to `nameof(timeProvider)`, and counting/advancing provider fixtures, yet its file table lists none of those files. The constant has no Must consumer until T-04, so the §9 `must-codes` gate fails at every Phase 1, 2 and 3 merge, and U2 is not on the codes-only skip list. Meanwhile `Must.Be.Past(DateOnly)` would report a projection failure as "must be in the past", the exact §5.4.2 misattribution the plan claims to prevent.
- **T-01 U2 contradicts A-07 on its face.** Its "exact output" for `DateTimeRules.HasMinimumAge` uses `today.AddYears(-years)`, which A-07 forbids, and depends on `TryGetAge`, which does not exist until T-04 U2.
- **Code ownership is internally inconsistent.** The `Date.Kind` node is created by both T-02 U1 and T-04 U1 while T-00 §2 declares Phases 2 and 4 concurrent and §7.3 makes `MustCodes.*.cs` single-writer. `timezone.id.unknown` is declared in Core but emitted only by a DataAnnotations adapter with invented message text (spec §5.5). `MustTimeZoneClauses.SameDay` reuses `Date.Equality` constants, breaking the "own mapped domain" audit rule. D-8 says condition `canonical`; §4.3 and T-03 U6b say `not-canonical`. §6.3 cites `TestTimeZones.PlatformSample`; T-01 defines `PlatformSampleId`.
- **The counting/advancing provider is named by A-03, T-01 U0 and T-04 U2 but has no type, file or §4 naming row**, despite being a new public member of the shipped `PineGuard.Testing` package.
- **The Qodana-to-InspectCode transition does not affect readiness.** No unit here depends on it; its final Fable tooling acceptance stays a separate gate.

### Where the Council Clashes

**D-1 / A-03: does the null-provider "today" meet T-00 §0's reopening threshold?**

The Contrarian and First Principles Thinker say yes. The evidence is reproducible: the same `DateOnly` input evaluated at 23:30Z on a UTC container and a Sydney host returns different `IsInPast` and `HasMinimumAge` verdicts, because `timeProvider ?? TimeProvider.System` resolves `LocalTimeZone` to `TimeZoneInfo.Local`. The plan's own D-2, D-4 and D-14 rationales treat "non-deterministic across machines" and "could flip the computed age depending on server location" as defects. T-01 §6 concedes the exposure and the routine environment pins `TZ=UTC`, so CI never surfaces it.

Peer Reviews 4 and 5 push back. A-03 chose provider `LocalTimeZone` explicitly. `TimeProvider.System.GetUtcNow` is already host-dependent and reading a provider property through the existing `?? TimeProvider.System` default is the repo's established dependency pattern, not the host-ID lookup A-05 bans under spec §5.3. The proposed alternative gives `null` and `TimeProvider.System` two different "today" semantics and still leaves `Must.Be.Past(d, TimeProvider.System)` host-dependent, so it narrows the defect rather than removing it.

**Chairman adjudication (Fable):** A-03 stands. The evidence is real and grounded in precedent, so this is not confidence alone, but it does not demonstrate a Brain violation: §5.3 as drawn by A-05 forbids Core ID resolution, and the null-to-System default is existing Core precedent. The split-semantics alternative is a preference between two defensible policies, which §0 reserves to Astra. What the argument does establish is a documentation and verification gap, which becomes a required change below.

**Smallest safe start.** The Executor wants U0 then U4 alone; Peers 2 and 3 note U4 declares a dependency on U1. The dependency is unjustified: U4 touches only TimeSpan guard files and needs no test zones. Relax it to U0, and the Executor's sequence is correct.

### Blind Spots the Council Caught

- **`System.Globalization.ISOWeek` is absent from `netstandard2.1`** (Peer 2). T-05 U2b's "one code path on every TFM" will not compile, and §6.2 only discusses `DateOnly`. Guard `IsSameIsoWeek` behind the existing `NET8_0_OR_GREATER` pattern the Must spec already mandates for BCL gaps.
- **T-01 U3's wrap never reaches the layers above Core if `MustTimeOnlyClauses.Between` follows the Must spec's canonical `Between` pattern and fails `min > max` with `Range.Invalid`** (Peer 1). Nobody read that clause. U3 must state whether that guard exists, and if so add the Must file to its list and redefine the `min > max` contract for `TimeOnly`.
- **A-06 UTC-only `DateTime` instant predicates will reject ordinary user input** (Peer 3). ASP.NET model binding and System.Text.Json routinely produce `Local` or `Unspecified`, so the T-04 DA attributes and Fluent extensions on `DateTime` properties fail with `date.kind.not-utc`. A-06 stands as the honest choice, but T-04 XML docs and T-07 README rows must steer adapters to the `DateTimeOffset` family. Legacy `DateTime` rules still host-convert via `DateTimeUtility.ToUtc` (core/project.md §6.2), so two policies coexist until migrated; T-07 must record that.
- **T-03 U6a contradicts A-04.** It has the owner download `backward` and `etcetera` from unpinned "latest" tzdb URLs; A-04 mandates pinned, hashed artifacts. Source both files from the `tzdata2026d.tar.gz` already in the manifest receipt (Peer 4).
- **T-03 U2 measures Standards adaptation by build-error counts only** (Peer 5). D-1, D-2, D-6 and D-9 are silent runtime changes. U2 must also inventory Standards call sites of `IsInPast`, `IsInFuture`, `HasMinimumAge`, `IsSameDay` and the TimeSpan threshold guards for U7's review.
- **`TZ=UTC` in routines structurally masks any host-zone dependence**, whichever way D-1 is read (Peers 2, 3, 4).
- **Provenance and tracker inconsistencies** (Outsider): "Astra", "Luna" and "Sol" appear nowhere in the Brain yet outrank a Fable council; the T-00 header carries a `council_verdict_ref` while the rerun receipt says no council ran; the tracker marks P0 Done and T1-U0 "Blocked (P0)"; "existing internal-testing precedent" is cited without a path.

### The Recommendation

**Do not approve as written.** A-01 through A-12 all stand; no A decision meets the T-00 §0 exceptional reopening threshold, and no Brain exception is required. Every necessary change is a plan-consistency correction that brings the phase plans into line with the A-register and the repo's own audit gates. Make these changes, then re-gate:

1. **T-00 §5 ownership table.** One row per new code node: creating unit, emitting clause, audit treatment. Resolve `Date.Kind` by making T-02 U1 create the node and T-04 U1 append to it, which means T-04's U1 depends on Phase 2 merged and merge order is 2 → 4 → 5 → 6; the Executor is right that parallel worktrees for 4/5/6 are ceremony, since `MustCodes.Date.cs` already serialises them. Fix D-8 to `not-canonical`. Move the zone-aware `SameDay`/`NotSameDay` pair from `MustTimeZoneClauses` into `MustDateTimeOffsetClauses` (and the matching Guard/Fluent files) so codes stay in-domain; names and signatures are unchanged. For `timezone.id.unknown`, cite the existing adapter configuration-diagnostic precedent by path, or record a documented `must-codes` DA-match exemption; do not leave it implicit.
2. **T-01 U2 made executable.** Add `DateTimeOffsetUtility.cs`, `MustCodes.Date.cs` and their test pairs to the file table. Move `TryGetAge` from T-04 U2 into T-01 U2 and delete the `AddYears` snippet per A-07. Add a Phase 1 Must unit (U2b) that wires `Date.Reference.OutOfRange` into `Must.Be.Past`/`Future`/`MinimumAge` for `DateOnly` and `DateTime` so the constant has a consumer and misattribution cannot ship. Name the counting provider with a §4 row and rejected alternatives; the council proposes `CountingTimeProvider` in `PineGuard.Testing/Common`, listed in U0's file table.
3. **D-1 documentation and verification, not reversal.** Every null-provider `<remarks>` states that "today" follows the host zone unless a provider is injected, T-00 §11 records the risk, and the "separate host integration check" T-01 U2 mentions becomes a named test executed once with a non-UTC `TZ` in CI.
4. **Blind-spot fixes as listed above:** ISOWeek TFM guard in T-05, the `Between` guard read before T-01 U3 dispatches, pinned-archive sourcing in T-03 U6a, call-site inventory in T-03 U2, A-06 consumer guidance in T-04/T-07, U4's dependency relaxed to U0, `PlatformSampleId` corrected in §6.3, tracker P0/T1-U0 rows reconciled, and a Brain path for the internal-testing precedent.
5. **Provenance.** Persist this session as the plan-gated transcript per council.md §6, restamp `council_verdict_ref` to it, and keep status `planned`. Log the Expansionist's README and `TestTimeZones` consumer-feature items under T-07 U1 as additive, not blocking.

Temporal readiness is independent of the InspectCode workstream; its tooling acceptance remains a separate pending Fable gate.

### The One Thing to Do First

Write the T-00 §5 code-ownership table: every new node with its creating unit, emitting clause and audit treatment. Building it forces the `Date.Kind`, `timezone.id.unknown`, `SameDay` domain and `Date.Reference.OutOfRange` consumer defects to be resolved on paper before any Phase 1 dispatch.

## Corrective Fable follow-ups — controlling public rulings

These concise outcome records preserve the public conclusions of the subsequent correction calls. They are not private reasoning and do not replace the verbatim public call results above.

1. **Follow-up #1 — retracted by #3:** the chairman initially proposed a separate `Window` family and a Must-owned host-ID lookup. Those positions were withdrawn and are not decisions.
2. **Follow-up #2 — no adjudication:** the response only described intended tool use; the call ran with tools disabled and produced no design ruling.
3. **Follow-up #3 — controlling:** A-01–A-12 remain final with no exceptional reopening. A-09 TimeOnly wrap semantics stand across Core/Must/Guard/Fluent/DataAnnotations using `start`/`end`; reversed TimeOnly bounds are valid and the TimeOnly-only invalid-bound check is removed; no `Window` family. Zone IDs are resolved at the DataAnnotations boundary; no Core/Must host lookup. Must receives a resolved-zone outcome and owns `TimeZone.Id.Unknown`; DA calls that Must result and then the semantic clause.
4. **Follow-up #4:** add separate public test-only `CountingTimeProvider` and `AdvancingTimeProvider`, with deterministic LocalTimeZone defaults, no timers and no production references; counting asserts a single `GetUtcNow`, and advancing fails on sequence exhaustion.
5. **Follow-up #5:** exact unknown-zone contract is `MustCodes.TimeZone.Id.Unknown`, `nameof(timeZoneId)`, supplied ID, template `Time zone ID '{paramName}' is not a known time zone.`; null ID uses `MustCodes.Value.State.Null` and the existing null template. The prior `{0}` placeholder suggestion was retracted because MustMessage only substitutes `{paramName}`.

## Final correction review — current result

A subsequent full-context Fable 5.1 gate returned **NOT ACCEPTED**. It confirmed that corrective follow-ups #3–#5, A-09 and the download-only timezone refresh were represented, but identified six critical plan defects and nonblocking polish. Its complete public findings and actual nonzero usage are preserved in `artifacts/fable-final-acceptance-2026-09-26.stream.jsonl`; no hidden reasoning is included. That review omitted the new transcript file and the Core project file; its assumption that Core lacked Must friend access is corrected by live `PineGuard.Core.csproj`, which already grants `InternalsVisibleTo` to `PineGuard.MustClauses`. Resolve the findings, include this transcript/receipt and exact source evidence in the next Fable gate, then append that gate's public verdict here.

No implementation, generation, commit, merge or release was performed by this council record.

## Final correction review — complete public output

The following is the complete public text from the first full-context final acceptance call. The only excluded stream fields are non-public hidden reasoning and system/tool metadata.

**Verdict: NOT ACCEPTED. Six critical defects require patching before activation. The corrective rulings (#3, #4, #5, A-09, download-only refresh) are correctly represented in the plan text; the blockers are elsewhere.**

## Critical blockers

**1. Stale "council not executed / pending authentication" receipt is still cited as activation evidence.**
Location: `docs/ai/plans/temporal-review-council-rerun.md` (whole file, "Actual revised Fable council: not executed … Not logged in … do not mark implementation Active, council complete"); cited from the Standards plan `temporal-timezone-sync.md` header ("recorded in … fable-council-transcript.md and its rerun receipt").
Defect: T-00 §0/§3 and the tracker P0 row state the council completed on 2026-09-26 with corrective findings. The receipt says the opposite and instructs readers not to activate. Orchestration §0 step 2 tells the routine to "confirm council evidence before activation", so an unattended run will find contradictory evidence and stop.
Patch: rewrite the receipt as a dated log: first attempt failed on authentication (keep the JSON facts as history); second run completed 2026-09-26 with transcript path, resolved model IDs, verdict and follow-ups #1 to #5 with #3 controlling over #1; append this acceptance review's outcome once blockers close. Standards plan header cites the transcript only.
Basis: criterion (1); A-01 ("an unavailable model or failed invocation must never be described as a completed approval", and its converse).

**2. `MustCodes.Date.Kind` already exists in source; T-02 U1 and T-04 U1 instruct creating it.**
Location: `src/PineGuard.Core/Codes/MustCodes.Date.cs` (`Kind` node with `NotUtc`, `Utc`, `NotLocal`, `Local`, `NotUnspecified`, `Unspecified`); T-02 U1 "register value Kind diagnostics in MustCodes.Date.Kind … Prefix = …; NotUnspecified = …"; T-04 U1 "append NotUtc (date.kind.not-utc)"; T-00 §5 single-writer row "Date.Kind | T-02 U1 creates NotUnspecified; T-04 U1 appends NotUtc"; A-08 "T-02 owns Date.Kind.NotUnspecified; T-04 owns Date.Kind.NotUtc".
Defect: literal execution duplicates existing constants (CS0102) or forces a stop. The A-12 values match the existing source exactly, so no decision is reopened.
Patch: in T-00 §5, T-02 U1, T-04 U1 and A-08, replace create/append with "reuse existing `MustCodes.Date.Kind.NotUnspecified` / `NotUtc`; no constants added; the A-12 templates apply to the new clauses". Remove the T-02 U1 `MustCodes.Date.cs` file edit.
Basis: spec.md §6.2 (do not invent repo facts); criterion (1) source precedent.

**3. Phase 1 Must consumption of the internal guarded reference has no stated access mechanism, and T1-U2's own gate cannot pass.**
Location: T-01 U2 ("internal `DateTimeUtility.TryGetTodayDate`", "Must evaluates this single sampled reference through existing internal-rule precedent"; precedent cited is `InternalsVisibleTo` to Core.UnitTests only); T-01 U2b; T-00 §9 codes-skip list (T2-U1, T4-U1, T5-U1, T6-U1) and §5 "Date.Reference … no skip exemption".
Defect: MustClauses is a separate assembly. It cannot call an `internal` Core member unless Core grants it, and the plan cites no such grant; A-08 forbids public wrappers. Separately, U2 registers `Date.Reference.OutOfRange` with no consumer until U2b, yet U2's acceptance is the full §9 gate including `audit must-codes`, which fails on an unreferenced constant.
Patch (Fable ruling, no new surface): U2 verifies from disk whether `PineGuard.Core` grants `InternalsVisibleTo("PineGuard.MustClauses")`; if absent, add that one attribute in U2 and list the granting file in U2's file list. U2 skips `audit must-codes`; U2b runs it. Add T1-U2 to the §9 skip sentence and reword §5 to "no exemption at Phase 1 merge".
Basis: A-08 (internal-only reference computation); must-clauses/project.md "must-codes" ("every declared constant is referenced somewhere"); T-00 §9 Definition of Done.

**4. Parallel Phase 1 units edit the same test-data files.**
Location: T-01 §2 (U3 "∥ U4, ∥ U4a"); U4 sweep ("regression-proof row added to every range-style guard's existing Between-family test data", which includes `GuardTimeOnlyClausesTestData.cs`); U3 edits the same Guard TimeOnly test data for the start/end rename. Also U5 edits `TimeOnlyUtility.cs` and the TimeOnly `IsBefore`/`IsAfter`/`IsSame` TestData groups while U3 edits `TimeOnlyRulesTestData.cs`, with no ordering between U3 and U5.
Defect: violates T-00 §7.3 "Never dispatch two agents that edit the same file."
Patch: exclude the TimeOnly range family from U4's sweep (U3 already owns the equal-endpoint rows); add "U5 after U3" to the U5 dependency cell and the tracker.
Basis: T-00 §7.3; criterion (3).

**5. T-05 U1 omits code constants that T-05 U3 clauses must emit.**
Location: T-05 U1 versus T-00 §5. Missing from U1: `Duration.WholeSeconds`, `WholeMinutes`, `Multiple`; `Interval.OnInterval`; `Calendar.LeapYear`, `LeapDay`, `DayOfWeek`, `Midnight`.
Defect: the `Not*` clauses D-15 mandates (`NotWholeSeconds`, `NotOnInterval`, `NotLeapYear`, `NotMidnight`, …) have no code; U3 cannot add them (single-writer rule) and `must-codes` fails.
Patch: replace U1's lists with "exactly the T-00 §5 additions for `Time.Duration` (excluding `NotIso8601`), `Time.Interval`, `Date.Calendar`, `Date.Equality`".
Basis: T-00 §5 single-writer ownership; D-15.

**6. The half-hour transition zone required by A-10 is unspecified.**
Location: T-01 U1 ("Add a custom zone with a half-hour transition and prove gap/overlap boundaries").
Defect: no name, offsets, rules or fixture dates; the build prompt forbids choosing.
Patch (Fable ruling, Lord Howe rules): `TestTimeZones.HalfHourDaylightSaving` = base `+10:30`, delta `+00:30`, forward at 02:00 first Sunday of October, back at 02:00 first Sunday of April; `HalfHourInvalidLocalTime = 2026-10-04 02:15 Unspecified`, `HalfHourAmbiguousLocalTime = 2026-04-05 01:45 Unspecified`, `HalfHourOrdinaryLocalTime = 2026-06-15 12:00 Unspecified`; add the matching rows to `TestTimeZonesTests.DaylightSaving_ClassifiesLocalTimes` and to the T-02 `IsInvalidTime`/`IsAmbiguousTime` fixture groups.
Basis: A-10; criterion (3).

## Polish (non-blocking, fix in the same pass)

- Stale owner-signature language: T-00 §10.2 ("decisions are Signed"), §11 ("D-row … Signed"), README Active Plans ("awaiting Phase 0 decision sign-off"). Replace with the Fable-acceptance gate.
- T-01 U3 "retain other secondary checks (including Inclusion enum validity)": no such check exists in `Between`/`NotBetween`. Ruling: strike the parenthetical; add no new check.
- T-01 U2b's first paragraph describes U2 work (the null-provider CI test and `.github/workflows/ci.yml`); move it into U2 and add both files to U2's file table.
- T-00 §4.1 table: the `DateTimeUtility` and `TimeOnly` rows have the wrong column count and do not render.
- T-03 U3: "extra D-7 rename scope … 200k" and "D-7 … checks" are stale after A-12; U1 pre-flight "HEAD is the Phase 2 merge" can be false if Phase 4 merges first; require "main contains the Phase 2 merge" and record HEAD.
- T-04 U2 file list and fixture table still include `DateTimeUtility.cs`, `DateTimeUtilityTests` and `TryGetAge` rows that T-01 U2 now owns; mark as existing.
- T-04 `MidnightBehind` row uses `−10:00` while the Astra fixture correction says `−08:00`; results are identical, harmonise the text.
- T-06: the ISO/RFC fixture block sits under the U2d heading but belongs to U2b's files.
- T-02 U6 netstandard2.1 resolver: state explicitly `TryFindSystemTimeZoneById` under `NET8_0_OR_GREATER`, otherwise `FindSystemTimeZoneById` catching `TimeZoneNotFoundException` and `InvalidTimeZoneException` to a null outcome.

## Operational gates once patched

- Verify on disk that `temporal-00-program.fable-council-transcript.md` exists and matches the corrected receipt; it was not supplied to this review.
- T2-U3 runs `audit must-codes` first: `MustTimeZoneClauses` references `Value.State.Null` and `Date.Kind.NotUnspecified` outside its mapped domain. If the audit rejects cross-domain references, stop and escalate to Fable; no exemption or new code is authorized.
- T2-U6 asserts the DataAnnotations error message reads exactly `Time zone ID 'timeZoneId' is not a known time zone.` with the property in `MemberNames`; if the existing `FromMustResult` path re-substitutes the member name into the template, stop and escalate.
- After patching, update tracker P0 to Done with this review as evidence, unblock T1-U0, and fill the §2.1 evidence path.

## Final correction acceptance review — 2026-09-27

**Public verdict: NOT ACCEPTED.** The corrected plan set resolves all six defects from the prior final gate and implements controlling Fable follow-ups #3–#5. One blocking Brain audit conflict and three precise non-design corrections remain. No plan was activated.

**Invocation provenance:** Claude CLI final-review call; model `claude-fable-5-1`, provider `firstParty`; session `e31dcbce-b2b5-4d0f-a83b-7f4d19f0dd9e`; exit 0; 93 turns; 587,569 ms total / 585,645 ms API. Model usage: input 290; cache-read input 2,175,377; cache-creation input 396,195; output 42,534; web searches 0; cost USD 10.59734425. Only public review text is recorded below; no hidden reasoning is included.

### Full public review

NOT ACCEPTED

The corrected plan set now implements Fable rulings #3, #4 and #5 and fixes all six prior blockers. One new gate-failing contradiction remains: the plans prescribe cross-domain error-code references that the mandatory `audit must-codes` check rejects, while asserting the audit is unchanged and no exemption exists. That is a plan-consistency defect against a live gate, not a reopening of Astra A-01 to A-12. Two further items need text corrections before activation but do not touch design.

**What I verified as fixed or correct**

- Receipt rewritten as a dated log with the second run recorded. `temporal-review-council-rerun.md` lines 15 to 27.
- `Date.Kind` is reused, never created. `temporal-00-program.md` §5 line 351; `temporal-02-timezone-core.md` U1 lines 44 to 49; `temporal-04-validity-relative.md` U1 lines 58 to 59. Source confirms the node exists at `MustCodes.Date.cs` lines 173 to 195.
- The friend grant exists. `src/PineGuard.Core/PineGuard.Core.csproj` line 19 grants `InternalsVisibleTo` to `PineGuard.MustClauses`. T-01 U2 lines 257 to 261 cite it correctly. The prior claim was wrong.
- U2 and U2b form one atomic cohort with no exemption. T-00 §9 line 536; T-01 U2 line 227 and U2b lines 370 to 373; §5 table line 350.
- Phase 1 file ownership is disjoint. U4 excludes the TimeOnly range family at T-01 line 436; U5 depends on U2 and U3 at line 40.
- T-05 U1 lists every §5 constant, including the positive pairs. T-05 lines 48 to 55.
- Half-hour zone fully specified. T-01 U1 lines 204 to 213; T-02 U2 fixture row line 158.
- Rulings #3 to #5 present: TimeOnly wrap with start/end across all layers and no Window family at T-01 U3 lines 391 to 432; DataAnnotations-only resolution with `Must.Be.ResolvedTimeZone(resolvedZone, timeZoneId)` at T-02 U3 lines 172 to 183 and U6 lines 313 to 328; `CountingTimeProvider` and `AdvancingTimeProvider` at T-00 §4.1 lines 213 to 216 and T-01 U0 lines 105 to 117.
- The unknown-zone message survives the DataAnnotations path unchanged. `MustResult.cs` line 114 renders the template once; `ValidationAttributeBase.cs` lines 106 to 115 only replace a token that is no longer present. The prior operational escalation is closed by source.
- Phase order 2 → 4 → 5 → 6 is stated consistently in T-00 §2, §5 and §7.3, orchestration §1, T-05 and T-06. Release, tag push and branch deletion are owner-only in orchestration §3.6.
- Download-only refresh and future generation are separated consistently: T-03 lines 34 to 78 and 186 to 192, the Standards manifest, and `temporal-timezone-sync.md` lines 26 to 28 and 102 to 117. No generation was authorized.
- Status and routing are consistent: T-00 header `planned`, README Active Plans row line 54, tracker P0 "Needs fix". Fable decides and reviews; Sonnet and Opus implement.

## Blocker

**Check (f) of `audit must-codes` rejects references the plans mandate.** `apps/cli/src/audit/rules/must-codes.ts` lines 392 to 407 flag any `MustCodes.<X>` reference whose domain differs from the file's single mapped domain, derived from the `// Serves:` comment at lines 322 to 331. There is no allowlist, and `exceptions.json` has no `must-codes` entry. The rule is excluded from `--gate` but T-00 §9 line 518 runs it explicitly per unit, and T-02 U3 line 236 requires it green. Current source confirms the convention: `Value.State.Null` appears only in files served by `MustCodes.Value.cs` line 3, and `Date.Kind.*` only in `MustDateTimeClauses.cs`. The plan says "Existing audit unchanged; no exemption" at T-00 §5 lines 351 to 352, so an implementer must stop. Five instances:

1. `MustTimeZoneClauses.ResolvedTimeZone` emitting `MustCodes.Value.State.Null` for a null ID. T-02 U3 lines 175 to 177 and 206; T-00 §4.2 line 237; transcript follow-up #5 line 220.
2. `MustTimeZoneClauses` emitting `MustCodes.Date.Kind.NotUnspecified`. T-02 U1 lines 48 to 49; T-00 §5 line 351; `temporal-astra-decisions.md` A-08 line 192.
3. Zone-aware `SameDay`/`NotSameDay` relocated into `MustDateTimeOffsetClauses` yet required by D-13 to emit `TimeZone.Zone.Null`. T-02 U3 lines 190 to 193, 212 to 213 and 217 to 218; T-00 §5 line 362.
4. `OnInterval` on `DateTime` and `DateTimeOffset` in Date-domain files emitting `time.interval.*`. T-00 §4.5 line 294 versus §5 line 358; T-05 U3b line 158.
5. New clause files `MustStringDateTimeClauses.cs` and `MustUnixTimeClauses.cs` are never added to the `// Serves:` comment of `MustCodes.Date.cs`, so check (f) reports them unmapped. T-06 U1 lines 53 to 56 and U3a/U3b lines 161 to 173.

**Minimum correction and Fable ruling.** Check (f) stands; no audit exemption is authorized. Every clause file emits only its own domain's constants. Record the following in T-00 §5, the A-register per §0, and the affected units:

- Null `timeZoneId` uses a new `MustCodes.TimeZone.Id.Null` with value `timezone.id.null`, the existing null template and `ParamName = nameof(timeZoneId)`. This corrects only the constant chosen in follow-up #5; the precedent is D-13's `TimeZone.Zone.Null` and `Predicate.Callback.Null`.
- Every zone-taking clause lives in `MustTimeZoneClauses` and emits TimeZone-domain codes for every failure path. The Kind precheck uses a new `TimeZone.LocalTime.NotUnspecified` with A-12's message and value attribution unchanged. The zone-aware `SameDay` pair returns to `MustTimeZoneClauses` with TimeZone-domain equality codes instead of reusing `Date.Equality`. The matching Guard, Fluent and DataAnnotations placements in T-02 U4 to U6 follow. This supersedes the chairman's relocation, which fixed one cross-domain reference by creating another.
- `OnInterval` for `DateTime` and `DateTimeOffset` uses a Date-domain `Date.Interval` node mirroring `Time.Interval`; `TimeOnly` keeps `Time.Interval`.
- T-06 U1 adds both new clause files to the `// Serves:` list of `MustCodes.Date.cs`.

## Minimum corrections, non-design

- **Scanner gate.** The Qodana and Sonar requirement at T-00 §9 lines 524 to 529 and §1.3 line 108 is accurate against the Brain, which still documents both tools. Locally it is an environmental prerequisite, not a design defect. The plan text is wrong in one place: orchestration §3.1 lines 209 to 210 state no secrets are needed, and §3.3 line 268 runs `Run-Qodana.ps1` with the default linter, which `Run-Qodana.ps1` lines 248 to 249 reject without `QODANA_TOKEN`. Correct §3.1 and §3.3 to name the prerequisite, either a token or `-Linter auto`. Do not substitute InspectCode; `qodana.md` lines 24 to 28 and the InspectCode receipt describe it as a token-free alternative with a shared engine, not an identical gate. Whether InspectCode may satisfy the phase gate is an owner decision to record in §9, not an implementer choice.
- **Merge and push authority.** T-00 §8 lines 494 to 496 have the orchestrator merge to `main` and push. Safety.md §1 classifies push as Tier 1. State that merge to `main` and push are owner-confirmed each time, or record an explicit owner pre-authorization for this program. The earlier pre-authorization covered New Surfaces only.
- **Must wiring path.** T-01 U2b lines 379 to 382 says "pass the checked reference into internal evaluation" without naming members. State the exact sequence, `TryGetTodayDate` then `DateOnlyRules.IsBefore`/`IsAfter` or `TryGetAge`, so no internal overload is invented.

## Non-blocking polish

- Orchestration §1 line 69 and §3.6 still say the owner supplies `backward`; the manifest shows `backward` and `etcetera` already extracted and hashed.
- T-01 U4 line 436 should also name `GuardStringTimeOnlyClausesTestData.cs`.
- T-01 U3 should state that `TimeOnlyRange`, `TimeOnlyRangeRules` and `MustTimeOnlyRangeClauses` are out of scope; `TimeOnlyRange.cs` line 27 still rejects `start > end` and F-4 mentions it.
- T-04 U2 fixture table line 215 lists `TryGetAge` rows that T-01 U2 owns; mark as existing.
- T-00 §2 line 148 omits "U5 after U2, U3" from the Phase 1 fan-out.
- Orchestration §3.3 step 2 should name the T1-U2 cohort exception beside the codes-only skip.

Status stays `planned`. Resolve the blocker, apply the corrections, and re-gate with the updated §5 table and A-register text.

## Focused MustCodes audit correction ruling — 2026-09-27

The persistent Claude Fable 5.1 follow-up returned **ACCEPTED as corrected** for the exact audit-ownership contract, not for the complete temporal plan set. The audit remains unchanged with no exemption. It binds new TimeZone.Id.Null, TimeZone.Kind.NotUnspecified, TimeZone.Equality.SameDay/NotSameDay and Date.Interval codes; moves zone-aware SameDay clauses into MustTimeZoneClauses; preserves unzoned SameDay and TimeOnly interval ownership; and corrects Date/TimeZone code ownership and the A-08 sentence. This is a corrective application of the existing audit contract, not a Brain deviation. See the complete public ruling and citations in temporal-audit-code-ruling.md.

Provenance: Claude CLI 2.1.283, session 7342dc25-a35f-42a2-a77e-3ea894c6b4eb (persistent), model claude-fable-5-1, provider firstParty, exit 0, 32 turns, duration 234,465 ms, model usage input 162, cache-read 380,100, cache-created 137,773, output 20,727, web searches 0, cost USD 3.888455. Only public verdict text is retained.
## Final plan acceptance and corrective rulings — 2026-09-27

The focused plan acceptance returned **P0 NOT READY** because three new gate-consistency defects remained after all earlier blockers and the MustCodes audit correction were closed. See [temporal-final-fable-acceptance.md](temporal-final-fable-acceptance.md) for the full public closure matrix and exact source citations.

Two focused binding follow-ups are preserved in [temporal-audit-gate-ruling.md](temporal-audit-gate-ruling.md) and [temporal-audit-gate-followup.md](temporal-audit-gate-followup.md). The first chooses a new versioned per-concept layer-absence contract plus strict validation, fixes string-family Rules discovery, adds VIBE tests, removes existing baseline debt rather than adding new debt, and moves full audits to the per-phase adapter-complete cohort. The second withdraws the proposed deletion of IsDaylightSavingTime, specifies its full Must/Guard/Fluent/DataAnnotations surface and literal rule descriptions, and resolves T-02 U0 ordering. These are public binding Fable outputs; no hidden reasoning is persisted.

At this update, plan-only corrections for B1–B3 and the remaining polish are in progress. Full-program readiness remains unaccepted pending the next actual Fable acceptance. No source implementation, test/scan pass, commit, push, release or generation is claimed.
## Final plan-readiness acceptance — 2026-09-27

Claude Fable 5.1 completed the focused final acceptance against the corrected PineGuard and PineGuard.Standards worktree documents. The public result and complete closure matrix are preserved in [temporal-final-fable-acceptance-2.md](temporal-final-fable-acceptance-2.md) (SHA-256 `49FBAA61E6DDBF51F7608E50F893152CC38C3CBFDA2BD18C98D0AAE00D9E3025`).

**Verdict: P0 READY FOR IMPLEMENTATION.** All seven finding groups close, including the follow-up decisions, B1–B3 audit-gate corrections, DST scope preservation and operational constraints. This is plan readiness only; it claims no implementation, tests, scanner pass, merge, release, generated classes or InspectCode gate substitution.

Fable identified two non-blocking editorial residues for the activation edit: T-00 §2's Phase 4/5 summary fan-out cells should match the dependency tables in T-04/T-05; T-01 should cite the existing MustClauses friend grant at PineGuard.Core.csproj line 19 only (line 20 is GuardClauses). Fable found neither residue changes execution because T-00 §0.1 and unit `Depends on` rows control dispatch. These corrections are being applied as editorial cleanup; they do not reopen the approved temporal decisions or block P0.

Provenance as reported by the resumed Claude Code CLI result: Claude Code 2.1.283, model `claude-fable-5-1`, provider `firstParty`, persistent session `7342dc25-a35f-42a2-a77e-3ea894c6b4eb`, subtype success, exit 0, 26 turns, duration_api_ms 1,348,461, total_cost_usd 19.94766475; ModelUsage input 682, cache-read 8,785,139, cache-creation 609,168, output 111,224; web searches 0. Resumed-session accounting scope is not independently verified; these are CLI-reported counters, not asserted as independent per-call time or usage. Only public Fable text is persisted; hidden reasoning and intermediate payloads are excluded.

## Post-acceptance editorial closeout and worktree fingerprint — 2026-09-27

Sol applied both editorial residues explicitly listed as non-blocking in the final Fable acceptance: the T-00 Phase 4/5 fan-out summaries now match T-04/T-05 dependencies, and T-01 cites only the MustClauses IVT grant line. T-00 §0 and D-3 status now cite the completed P0 READY verdict. T-00 metadata/README are Active; orchestration records P0 Done and T1-U0 Running under Claude Sonnet 5 via Luna_research. No implementation unit is marked Done by this status update.

The final acceptance artifact remains `temporal-final-fable-acceptance-2.md` (SHA-256 `49FBAA61E6DDBF51F7608E50F893152CC38C3CBFDA2BD18C98D0AAE00D9E3025`). The following hashes fingerprint the post-acceptance documents after only the two stated editorial corrections and activation/status metadata updates; they do not imply Fable reread those exact post-edit bytes:

| File | SHA-256 |
|---|---|
| `temporal-00-program.md` | `FA57C554CDE7A9CB5FBE2372DF548C50880CDD7151BED185660BD48139F602AB` |
| `temporal-01-foundation.md` | `1AF0AD905B6C284C4AF1626E3EBA449A303D44168A3C2F542A295CFAD41B5636` |
| `temporal-02-timezone-core.md` | `BF10383E59F8DE35012F0CB76996805F104152CE6F3B6DFD76378A8BBF07EAEB` |
| `temporal-03-release-standards-sync.md` | `7C9A0D3C0EE25E407F1F4DEA0D43471269624C0E1B769FAD3C7AFF4E87440CA6` |
| `temporal-04-validity-relative.md` | `F0B81A477002929CA97516EB79944941D93EB1B87ECFF50116F96F1C176A0207` |
| `temporal-05-durations-calendar.md` | `AC1C0115C259A67FB13444D1D0844A359BFE455E19856C51C72B4D0FC9FAB8AC` |
| `temporal-06-formats-epoch.md` | `E486EE647A3298CA1401D8E990401FC012DFDB6F575C0A88FA832C4FB90DBAB2` |
| `temporal-07-closeout.md` | `D9AEAD23FD9359D25019CB3E5227728B78A62451FAF2B6B55CE7089125F24484` |
| `temporal-astra-decisions.md` | `FD92225D4D72097E313445E3B6CB302799E854724BCE5B4C69BB86B0FEEF6F8A` |
| `temporal-orchestration.md` | `3FB54F9AFE9F3F34D0756E3E834FBA24C64A3B5549BA535A3285F188AADC1C17` |
| `docs/ai/README.md` | `EA4D3CA642550296C59F4DFE37896CBB2150BDC1F678C47A60F5F976FCE98081` |
| Standards `temporal-timezone-sync.md` | `001EF2D997307D0B7F54E2FCE467C617EC436BDD84926ED287747BBF88E1188B` |


## T1-U0 unit review and coverage-runner ruling — 2026-09-27

A fresh, bounded Claude Fable 5.1 code review was run read-only against the T1-U0 worktree. Public findings and the binding coverage-runner contract are preserved in `artifacts/reviews/T1-U0-fable-review.md` (SHA-256 `C3B78C75BA0725486DEAC8661946FF807171EDE17CAE90DF5489E87ECD9F419B`).

**Code review: FIX. Unit acceptance: BLOCKED.** Fable found three concrete items: (1) the null/default zone assertions use `Assert.Equal`, which cannot distinguish `TimeZoneInfo.Local` from `TimeZoneInfo.Utc` on UTC CI; change the three zone assertions to `Assert.Same`; (2) add plan-required remarks to Fixed, Counting and Advancing providers clarifying that wall-clock reads are controlled but inherited timestamps/timers remain real-clock behavior; (3) low-priority recommended improvement: assert a new counting provider has count 0 and count 1 after one read, since future consumers rely on a strict single-read contract.

Fable bound the isolated multi-target coverage fix to shared framework discovery and publish helpers, per-TFM Coverlet isolated execution, reuse of the resolver/publish helpers in the existing dotCover path, and a Pester regression suite in `tools/.tests` covering selected and unbound TFMs, isolated/non-isolated behavior, error visibility, and one slow end-to-end Testing run. The detailed ownership, behavior matrix, exact test vectors and documentation changes are in the public review artifact. The unchanged 100% coverage gate and `-Isolated` behavior remain required; no bypass is authorized.

Verification reported to Fable: all-scope format passed; compiler diagnostics passed with zero warnings/errors; all-scope tests passed with 678 Testing tests per TFM and no failures/skips. The Testing coverage command failed before test execution because isolated multi-target publish omitted the target framework and produced NETSDK1129. The first audit attempt had aborted during pnpm preflight, so Fable correctly treated it as unexecuted based on its review input. After that review, Luna_research resolved pnpm using the pinned frozen-lockfile install in the ignored worktree artifact area; `audit --gate` then exited 0 with surface-parity 0 and test-files 0. Thus audit is now passed, while coverage remains blocked pending the approved runner repair and successful net8.0/net10.0 isolated Testing coverage runs.

Provenance as reported by Claude Code: fresh session `04825da4-0e28-4721-b665-2adbc91d7796`, model `claude-fable-5-1`, provider `firstParty`, exit 0, 63 turns, duration_ms 413,772, total_cost_usd 6.638071; ModelUsage input 194, output 31,373, cache-read 791,204, cache-creation 243,484, webSearchRequests 0. CLI accounting scope is not independently verified; counters are recorded as reported. The session was restricted to Read/Glob/Grep with explicit strict empty MCP configuration. No hidden reasoning or intermediate payloads are retained.

## Review-status clarification — 2026-09-27

Correction to the preceding status summary: T1-U0 is not blocked solely on coverage. Fable's two blocking provider findings remain open until implemented and reviewed: (1) use reference identity assertions to prove UTC fallback instead of host-local fallback; and (2) document that inherited timestamp/timer behavior is real-clock behavior. In addition, the required isolated Testing coverage gate must pass for both net8.0 and net10.0 after the runner fix. The audit gate has since passed. The separate low-priority CountingTimeProvider zero/one-read assertion is recommended but is not independently blocking.

