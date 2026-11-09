# Decision register

Current explicit user instructions override v1.6 and the proposed handoff where they conflict. ACCEPTED means planning/design direction; implementation still requires activation. BLOCKED means the specific answer must be decided by the owner (with Fable review) before dependent work.

**Register state on 2026-11-09.** The council required by D19 has run ([transcript](../../technical-readiness-to-1.0.council-transcript.md)); its verdict is recorded as D29 below and annotated on the rows it changes. The owner's implementation authorization (second half of D19) is still pending. The Core/Common API decisions were implemented and merged to main the same day (D30). Rows not annotated are unchanged and apply only if the owner later activates a wave.

| ID | State | Decision or required evidence | Work |
|---|---|---|---|
| D01 | ACCEPTED | Greenfield best new static rule structure; no old-release compatibility/shims/migrations; cheap shared executable truth, no speculative hierarchy | W03/W12/W22 |
| D02 | ACCEPTED |10/10 EACH technical rubric row; no weights/aggregate or false perfection claim | W21 |
| D03 | ACCEPTED |37,749 is dated framework executions, not unique tests/coverage. Current figure (2026-11-09, main `86ba9c9`, CI run 42599717255): 47,011 passing executions (23,493 net8.0; 23,518 net10.0), 100% line and branch in the audited denominator; still executions, not unique tests | W00/W20 |
| D04 | BLOCKED | Reproduce OWASP trim/newline/returned-value observations and decide best new normalization contract | W01/W14 |
| D05 | BLOCKED | Decide regex invalid-pattern/timeout/resource/negation outcomes from reproduction and new contract intent | W01/W15 |
| D06 | ACCEPTED, re-ordered by council 2026-11-09 | Pilot order is string→DateOnly FIRST as one PR (rule object, four adapters, cross-layer consistency test from one `TheoryData`, BenchmarkDotNet of new and current path, owner-signed names table; two-week box; kill criterion in D29). Email and bounded numeric follow only if PR one passes. Independent expectations; no second support/code list | W01/W02/W05 |
| D07 | BLOCKED concrete design | Early executable manifest is required; select exact representation/provenance through existing audit machinery | W02 |
| D08 | BLOCKED concrete design | Choose best globally consistent failure contract and projection mappings; evaluate minimal protocol/privacy fields | W04 |
| D09 | ACCEPTED evidence boundary | Current DA has no universal code; current Guard NotEmail maps Must Email. New mappings require explicit design, not accidental inferred codes | W04/W05 |
| D10 | BLOCKED | Inventory/approve property/fuzz/differential/mutation harnesses, tools and exact paths | W06–W09 |
| D11 | ACCEPTED priority / BLOCKED numbers | Benchmarks EARLY: the pilot PR carries a BenchmarkDotNet project measuring the new path and the current `StringRules.DateOnly`/Must path (council 2026-11-09); workloads/statistics/budgets are approved from that actual baseline | W10 |
| D12 | BLOCKED | Choose supported packages/TFMs/RIDs/features and execute real lightweight trimmed/native consumers | W11 |
| D13 | ACCEPTED scope / BLOCKED schema | NEW public contract and forward drift gates; no historical release baseline obligation; exact API/tool decisions by Astra | W12 |
| D14 | BLOCKED | Threat claims and meaningful regex/general resource policies | W14–W16 |
| D15 | BLOCKED | Diagnostics necessity, opt-in shape, privacy and overhead; no unreviewed attempted-value capture | W17 |
| D16 | PARTLY DECIDED | Clock/"today" semantics are decided by Temporal D-1 (ratified by the owner, merged `ee08ccd` 2026-11-09): with no injected `TimeProvider`, today is the UTC calendar date; an injected provider's `LocalTimeZone` chooses the zone (the Phase 1 DataAnnotations adapter resolves the provider through `ValidationContext.GetService`; that is implementation, not a D-1 term). The pilot inherits this. Culture, calendar and collection-order semantics remain BLOCKED | W01/W18 |
| D17 | BLOCKED | Characterize and choose construction/mutation/reuse/concurrent lifecycle; no unsupported race/safety claim | W19 |
| D18 | ACCEPTED | Original files untouched; portable byte-preserved .txt snapshots; new folder master sole canonical README row; children only indexed master | Publication |
| D19 | ACCEPTED, half satisfied | Planned until (a) a real council and (b) the owner's implementation authorization. (a) done 2026-11-09 ([transcript](../../technical-readiness-to-1.0.council-transcript.md), header stamped). (b) pending: the owner's explicit go on the string→DateOnly pilot is the authorization, recorded in the plan header and the tracker when given. Model policy is now Fable for judgment/review/council, Opus for implementation, Sonnet for reads; no model choice waives procedure | All |
| D20 | RECEIVED/REVIEWED | Genuine Sol synthesis + Luna official research accepted as input; latest greenfield/early-benchmark instructions supersede stale sequencing | Comparison |
| D21 | ACCEPTED | New structure/manifest/benchmarks early, biggest apps last; later bounded rule expansion | All |
| D22 | BLOCKED per task (wave work) / SATISFIED for the pilot | Wave work: inventory exact implementation/test/project paths and obtain the owner's approval before coding. Pilot PR: satisfied by the ten-line file list in the PR description, no separate approval cycle (council 2026-11-09) | All |
| D23 | BLOCKED | Dependency sources/licenses/security thresholds/tool/exception policy after inventory | W13 |
| D24 | ACCEPTED hard requirement |100% line AND branch in audited validation Coverlet denominator; no hidden semantics/exclusion dilution/missing artifacts | W00/all validation |
| D25 | ACCEPTED quality direction | Early global taxonomy/naming, DRY/SOLID, root-cause fixes, no redundancy/hacks/local outliers/speculative abstraction | W22/all |
| D26 | BLOCKED sample designs | Exact API/DDD/Clean scenarios, frameworks, interfaces and scope chosen separately when late entry gates pass | W20A/B/C |
| D29 | ACCEPTED (council verdict 2026-11-09) | Do not activate the plan as written. Activate one unit on the owner's go: the string→DateOnly pilot as a single PR. Exit: no adapter contains rule logic; consistency test green; benchmarks committed; CI 100% line and branch with no `[ExcludeFromCodeCoverage]`; names signed. Kill: any adapter needs logic the rule object cannot supply, or the new path allocates where the current one does not → W03 returns to redesign. Two-week time-box. Process: `+ technical-readiness-to-1.0/tracker.md` (three rows), one Fable review per PR, owner name sign-off; closure ledgers, Astra/Luna/Sol protocol, checkpoints and hash caches off the critical path. W22 after the pilot. `core-common-api-decisions.md` not edited by the pilot. Temporal Phase 2 waits for the pilot box; if the pilot passes it is the first consumer of the rule object | All |
| D30 | RECORDED | Core/Common API decisions implemented and merged to main 2026-11-09 (`c124601`, `f76733d`; merges `148fb95`, `86ba9c9`). Record: [implementation tracker](../../core-common-api-implementation.md). Vocabulary now `InRange`/`OutOfRange`, `AgeInRange`, `LengthInRange`, Hostname family, parsed-type suffixes. The pilot's names table and benchmark baseline are taken from this surface, not from 2026-09 snapshots | W12/W22 |

### TEST-LAYOUT-01 — Approved concept-based test projects

Accepted: the six concept project identities and folder responsibilities in [test-project layout](test-project-layout.md). Crosscutting in-process Rules tests and Architecture checks stay in their respective test projects; host integrations, dedicated fuzz harnesses, consumer executables and BenchmarkDotNet workloads stay separate by execution kind. Keep existing library unit suites, PineGuard.Testing and PineGuard.Testing.UnitTests responsibilities. Extend shared test discovery and coverage filters to approved test project name families while preserving explicit filters and the Testing scope's helper-only TestCsproj behavior. Do not send fuzz, benchmark or consumer executables to ordinary dotnet test. Update solution, CI, inherited test props, documentation and scripts at implementation gates; no project is claimed present or passing while status is Planned.

## Decision record schema

### Mandatory maintenance decisions

**D27 — ACCEPTED hard requirement (wave work); suspended for the pilot PR by D29:** every workstream, major block/section and wave passes the knowledge/BRAIN/docs/scripts/tools closure contract. Lessons and decisions are saved; required supporting machinery is maintained, tested and run; full review feedback is resolved/rechecked; missing closure prevents completion. See [contract](knowledge-and-tooling-closure.md) and [session ledger](closeout-ledger.md). The pilot PR closes on its D29 exit criteria, one Fable review and the owner's sign-off instead.

**D28 — REQUIRED scoped promotion task (wave work):** at Wave A's first knowledge-closure review, the owner (with Fable review) resolves PROMOTE-01: whether this project maintenance constraint warrants durable guidance in `docs/ai/specs/orchestration.md` (execution/closure scope) or genuinely cross-cutting root `docs/ai/specs/spec.md` under §9. Luna provides applicability/duplicate guidance evidence. If promoted, update affected child specs, Rule pointers, agent/skill/workflow/adapters and indexes through the existing cascade; if not, record the reason and retain the binding project contract. No invented generic memory agent or parallel global rule hierarchy is permitted. This is a future authorized-execution task, not a claim that those root files were edited during planning.

ID; question; source/excerpt/hash/reproduction; alternatives including a simpler/no-new-abstraction option; new-contract consequences; privacy/security/resource impact; exact selected behavior/signatures and rationale; all affected files/surfaces; required evidence; review identity; council requirement/verdict; task IDs; rollback of implementation if needed.

Greenfield does not mean agents guess semantics. It removes the obligation to preserve old contracts while requiring a better explicit new contract. Observed current behavior is evidence, not automatic normative truth.
