<!-- metadata_header
type: plan
id: technical-readiness-to-1.0
version: 1.12
status: planned
last_updated: 2026-11-09
council_verdict_ref: docs/ai/plans/technical-readiness-to-1.0.council-transcript.md
council_verdict_date: 2026-11-09
council_trigger: explicit
-->
# PineGuard technical readiness to 1.0

## Where this plan stands (2026-11-09)

| Question | Answer |
|---|---|
| Council | Run on 2026-11-09 (Fable council, [transcript](../technical-readiness-to-1.0.council-transcript.md)); header stamped. The first half of D19 is satisfied. |
| Owner authorization | **Not yet given.** The second half of D19. Until the owner's explicit go, status is `planned` and nothing below is active. |
| Active path once authorized | One unit only: the **string→DateOnly pilot** as a single PR (see [council verdict](#status-authority-and-preservation)). Not Wave A, not the 23 workstreams. |
| Parked behind the pilot | Email and bounded-numeric pilots (only if PR one passes); W22 naming (runs after the pilot); every wave; Temporal Program Phase 2 (T-02), which waits for the pilot's two-week box. |
| Current API baseline | The Core/Common API decisions are implemented and merged to main on 2026-11-09 (`c124601`, `f76733d`, merged as `148fb95`/`86ba9c9`); see [Current API baseline](#current-api-baseline). They are no longer open decisions. |
| Temporal baseline | Temporal Program Phase 1 merged to main as `ee08ccd` on 2026-11-09. D-1 ratified: without an injected `TimeProvider`, "today" is the UTC calendar date; an injected provider's `LocalTimeZone` chooses the zone. See [temporal-00-program.md](../temporal-00-program.md) §3. |
| Current evidence | Main CI run 42599717255 at `86ba9c9`: 47,011 passing framework executions (23,493 net8.0; 23,518 net10.0), 100% line and 100% branch (8,986 of 8,986 branches) in the audited Coverlet denominator, `MIN_CODE_COVERAGE=100`; main `3c925f7` (run 42611203241) is fully green including the audit job. See [baseline evidence](references/baseline-evidence.md). |
| Process for the pilot | Tracker page (`+ technical-readiness-to-1.0/tracker.md`, created on authorization) with three rows, one Fable code review per PR, owner name sign-off. Closure ledgers, the Astra/Luna/Sol protocol, checkpoints and hash caches are off the pilot's critical path; whether any of them return is decided when the waves are redrawn from pilot evidence. |
| Model policy | Fable for judgment, review, decisions and council; Opus for implementation; Sonnet for reads (owner instruction, 2026-11-08). This replaces the planning-era "Astra model override" language of plan v1.11 and earlier. |

## Code examples

The plan now includes [complete worked code examples](references/code-examples.md) and a substantive example directly in every workstream and major-app subplan. The reference contains a runnable, package-free teaching prototype, complete governance-example scripts, source-grounded current PineGuard insertion examples and reproduction instructions. Each example distinguishes **Current API, source-inspected**, **Proposed illustration** and any unexecuted configuration; it does not silently select an unresolved architecture decision.

The proposed .NET 10 miniature and synthetic governance fixtures were built/run independently in a workspace scratch directory. That evidence applies only to those examples. Current PineGuard adapter insertions, real repository coverage, BenchmarkDotNet, NativeAOT and full-suite validation are not proved by those runs. The positive-integer miniature supplements rather than replaces the string→DateOnly pilot (Email and bounded numeric follow only if that PR passes). All roadmap priorities, hard coverage requirements and Planned activation gates remain unchanged.

This modular plan targets **10/10 in every technical rubric criterion** and **100% line and branch coverage for every declared validation surface within the audited Coverlet denominator**. It is a detailed plan and critical review, not implementation or a claim that readiness has been achieved.

PineGuard is greenfield for this work. Choose the best globally consistent new design; no old-release compatibility, shims or migration work is required. Put small high-value CI/evidence repairs, naming/taxonomy decisions, the **new static rule structure and executable manifest**, and **benchmark baselines EARLY**. Put the largest API, DDD and Clean Architecture sample projects LAST. These current user instructions supersede contrary priorities in v1.6 and the original handoff.

## Current API baseline

The Core/Common API decisions were implemented and merged to main on 2026-11-09 (`c124601`,
`f76733d`; merged as `148fb95` and `86ba9c9`). Use the
[Core/Common API decision record](../core-common-api-decisions.md) and
[implementation evidence](../core-common-api-implementation.md) when inventorying the current
surface. Implemented names include `NetworkRules.IsHostname`, `Must.Be.Hostname`,
`Guard.Against.NotHostname`, and the `InRange` / `OutOfRange` vocabulary. Account for the
integrated temporal duration words and parsed-type suffixes; historical snapshots retain the
spellings of their reviewed commits. Further design changes must update all affected libraries,
callers, tests and documentation together under the [greenfield policy](../../specs/spec.md).

## Approved test-project organization

The user-approved [testing project layout](references/test-project-layout.md) groups added crosscutting checks into Rules and Architecture test projects, hosted composition into IntegrationTests, and distinct Fuzz, Benchmarks and Consumers executables. Testing techniques use folders within those responsibilities. Existing library unit suites, the shared PineGuard.Testing fixture library and its own PineGuard.Testing.UnitTests retain their responsibilities. The layout's stable TP-* identities and workstream mapping are authoritative; implementation must align the solution, relevant discovery/routing/coverage scripts, BRAIN and documentation. The six projects remain planned and must not be reported as present or passing.

## Status, authority and preservation

> **Council verdict (2026-11-09).** The plan-gated Fable council required by D19 has run; its transcript is
> [technical-readiness-to-1.0.council-transcript.md](../technical-readiness-to-1.0.council-transcript.md).
> Verdict: do not activate the plan as written; activate one unit, the string→DateOnly pilot, as a single PR
> with the rule object, all four adapters, a cross-layer consistency test, a BenchmarkDotNet baseline against
> the current path and an owner-signed names table, under the existing CI gates, with a two-week time-box
> and an explicit kill criterion; Email and bounded numeric follow only if it passes; W22 runs after the
> pilot; `core-common-api-decisions.md` is not touched; Temporal Phase 2 waits for the pilot's time-box.
> The owner's implementation authorization (the second half of D19) is still to be given; until then the
> status stays `planned`.

Status is **Planned**. The formal council required by `docs/ai/specs/council.md` §§1,4–6 and the plan-with-council workflow occurred on 2026-11-09 and its verdict reference is recorded in the header above. The one remaining gate before any implementation is the owner's explicit authorization. The planning-era role split (Astra architecture/review, Luna reading, Sol coding, orchestrator coordinating) describes how this plan was written; for the authorized pilot the council replaced it with one Fable code review per PR and the owner's name sign-off, and the owner's current model policy is Fable for judgment and review, Opus for implementation, Sonnet for reads. No model choice waives independent review.

The original uncommitted v1.6, rubric, verification report and existing unrelated edits remain untouched. Portable byte-preserving historical snapshots and hashes are in [snapshot provenance](references/snapshot-provenance.md). The old flat plan is historical reference; this folder master is the sole new canonical README plan row. Children are indexed here, not as23 separate active rows. Preserve `.codex/config.toml`, root README changes and `docs/ai/plans/schema-driven-validation-agent.md` outside this task's ownership. Only the reviewed canonical-index hunk in `docs/ai/README.md` is included.

## Evidence and critical review

The dated revision is2aeac39b88effef5518741a9cf9b505a04f926f4, Windows Debug SDK10.0.401:15 projects×2TFMs produced30TRX and37,749 passing framework executions (18,862 net8;18,887 net10), no failures/skips. This is not unique-test count and collected no fresh coverage. An initial analyzer reference-cache failure was repaired locally; no production source fix was established. The package/docs audit is also dated, not a release-time guarantee. See [baseline evidence](references/baseline-evidence.md).

The current evidence (2026-11-09, main `86ba9c9`, CI run 42599717255) is 47,011 passing framework executions (23,493 net8.0; 23,518 net10.0) across 15 test assemblies per TFM plus the two `core-null-provider-timezone` jobs, with 100% line and 100% branch coverage (8,986 of 8,986 branches) gated at `MIN_CODE_COVERAGE=100`, and the 14-rule audit ratchet in `apps/cli`. These are still framework executions, not unique tests, and the coverage denominator is the audited Coverlet scope, not every assembly.

The historical rubric scores are independent: semantic/failure7; architecture/cross-surface8; assertion/adversarial7; consumer API/composition8; security/data6; performance/resource4; API/package/deployment6; CI/tooling/release5; docs/maintainability7. Confidence differs by row; no weights or aggregate exist. The target is10 EACH, without claiming mathematical perfection.

Read [Astra integrated critical review](references/astra-integrated-review.md), [handoff traceability](references/handoff-traceability.md), [competitor dispositions](references/competitor-integration.md), [Luna research](references/competitor-source-dossier.md) and [Sol analysis](references/sol-comparative-analysis.md). Research identifies ten purpose-selected comparators, not an objective popularity ranking. No unmeasured speed, correctness or unique-breadth claim is adopted.

## Mandatory quality and coverage

**Mandatory knowledge/tooling closure:** every workstream, major task block, major section and wave must pass the [closure contract](references/knowledge-and-tooling-closure.md). Assign a block ID before dispatch; save lessons and decisions, align all relevant BRAIN/Markdown/indexes, maintain and test/run supporting scripts/tools, then complete Astra review → feedback → fix → recheck. Persist evidence in the [closeout ledger](references/closeout-ledger.md). A feature cannot close with missing supporting machinery, stale documentation or unresolved blocking feedback. This applies during work, not only at the final wave. **Pilot exception (council 2026-11-09):** the string→DateOnly pilot PR closes on its exit criteria, one Fable review and the owner's name sign-off; the closure-contract paperwork is not on its critical path. Whether the contract applies to later waves is decided when they are redrawn from pilot evidence.

The [quality constitution](references/quality-constitution.md) applies to every task: BRAIN precedents first; repository-wide root-cause and duplication review; coherent naming; DRY/SOLID without speculative abstractions; formatting/analyzer hygiene; no local hacks, hidden failures or diluted gates.

The [coverage contract](references/validation-coverage-contract.md) preserves 100% line AND branch in audited validation scope across Core/Rules, Must, Guard, FV, DA and all other declared surfaces. Coverlet alone gates. Generated/build/exclusion boundaries are audited so semantics cannot be hidden. Every expected report must exist; absent reports cannot count as no coverage required. Conformance is separate from coverage.

## Workstream index

Numeric IDs organize files; the dependency graph determines execution. The council verdict (2026-11-09) moved W22 to run after the pilot: names are the output of the shape, and D01 makes renames free. No workstream is active; the pilot PR is the only authorized unit once the owner says go.

| ID | Plan | Main placement |
|---|---|---|
| W00 | [CI and evidence trust](workstreams/00-ci-and-evidence-trust.md) | Early; includes mandatory coverage contract |
| W01 | [Semantic specification](workstreams/01-semantic-specification.md) | Pilot: independent expected cases for string→DateOnly feed the consistency test |
| W02 | [Manifest/completeness](workstreams/02-manifest-and-completeness.md) | Early new executable inventory (after the pilot) |
| W03 | [Static structure/lowering](workstreams/03-static-kernel-and-lowering.md) | Pilot: the string→DateOnly PR is the test of the W03 premise; kill criterion returns W03 to redesign |
| W04 | [Failure contracts/projections](workstreams/04-failure-contracts-and-projections.md) | Early coherent new contract |
| W05 | [Conformance/parity](workstreams/05-conformance-and-parity.md) | Pilot then systematic rollout |
| W06 | [Properties](workstreams/06-property-testing.md) | Independent hostile proof |
| W07 | [Fuzz](workstreams/07-fuzz-testing.md) | Bounded risk-led proof |
| W08 | [Differential](workstreams/08-differential-testing.md) | Explicit equivalence domains |
| W09 | [Targeted mutation](workstreams/09-targeted-mutation.md) | Assertion sensitivity |
| W10 | [Performance/allocation](workstreams/10-performance-and-allocation.md) | Pilot: BenchmarkDotNet project benchmarking the new path and the current `StringRules.DateOnly`/Must path; repeated after changes |
| W11 | [Trimmed/native consumers](workstreams/11-trimming-and-native-consumers.md) | Lightweight real consumers before large apps |
| W12 | [New API/forward consistency](workstreams/12-public-api-and-contract-consistency.md) | No legacy compatibility work |
| W13 | [Dependencies/supply chain](workstreams/13-dependencies-and-supply-chain.md) | Intentional new package graph |
| W14 | [Threat/security claims](workstreams/14-threat-model-and-security-claims.md) | Before final operational policy |
| W15 | [Regex](workstreams/15-regex-governance.md) | Early semantics; later hostile proof |
| W16 | [Resource limits](workstreams/16-resource-limits.md) | Measured policies |
| W17 | [Diagnostics](workstreams/17-optional-diagnostics.md) | Explicit need/privacy/overhead decision |
| W18 | [Culture/time/determinism](workstreams/18-culture-time-and-determinism.md) | Independent environment contracts |
| W19 | [Lifecycle/concurrency](workstreams/19-validator-lifecycle-and-concurrency.md) | Early characterization; final proof |
| W20 | [Docs/samples/support](workstreams/20-samples-documentation-and-support.md) | Light examples early; large apps last |
| W21 | [Readiness/release/expansion](workstreams/21-readiness-release-and-expansion.md) | 10/10 adjudication for each row |
| W22 | [Naming/taxonomy/abstractions](workstreams/22-naming-taxonomy-and-abstractions.md) | After the pilot (council 2026-11-09); the pilot's owner-signed names table is its first input |

The largest late projects have separate plans: [W20A API](workstreams/20-samples/api-validation.md), [W20B DDD](workstreams/20-samples/ddd-validation.md), [W20C Clean Architecture](workstreams/20-samples/clean-architecture-validation.md).

## Waves and dependency gates

**No wave is activated.** The council verdict (2026-11-09) placed one pilot PR ahead of Wave A. Waves resume only if the pilot passes its exit criteria and the owner then activates them; if the pilot hits its kill criterion, W03 returns to redesign before any wave starts. The wave text below is retained as a map to be redrawn from pilot evidence, not as a schedule.

Every major block within A–F has its own closure record; the wave gate consolidates those records and performs a full integrated Astra review. The technical exits below are necessary but insufficient without these mandatory knowledge/tooling exits:

| Wave | Required closure evidence |
|---|---|
| A | CI/coverage/context/tooling impact matrix, early decision lessons, all block reviews and corrected BRAIN/docs/tool instructions |
| B | New structure/manifest/contract source→docs/spec/rule/tool/test/index cascade, saved decisions and closed reviewer feedback |
| C | Correctness/security/environment/lifecycle harnesses built or updated, tested and run; lessons/corpus/tool instructions synchronized; all block findings resolved |
| D | Package/deployment/API/dependency tools and support docs aligned, actual commands/artifacts saved, integrated review and rechecks |
| E | Every lightweight/major sample block and each W20A/B/C closure accepted; relevant BRAIN/docs/scripts/tools/indexes and lessons current |
| F | All earlier closure records reconciled with fresh context, remaining feedback resolved or validly rejected, final knowledge/tooling review and owned-commit evidence |

**A — trust and early decisions.** W00 repairs routing/self-check/evidence gaps and enforces audited100% coverage. W22 inventories global naming/abstraction precedents; W01 reproduces OWASP and regex concerns; W19 characterizes lifecycle. W10 captures representative benchmarks immediately. Exit: trustworthy baseline, approved vocabulary and explicit decision docket.

**B — new structure and manifest.** W22/W01 decisions feed the W03 rollout beyond the pilot and the W02 manifest; W04 defines globally consistent failures. Pilot order per the council: string→DateOnly first as one PR; Email and bounded numeric only if it passes. Reuse existing audits, not a second manual code/support list. Rebenchmark before broad rollout. Exit: reviewed pilot structure/manifest/contract plus independent outcomes.

**C — comprehensive validation proof.** W05 rolls conformance across applicable surfaces while coverage stays100%; W06–W09 provide distinct properties/fuzz/differential/mutation evidence. W14 drives W15/W16; W17/W18/W19 settle diagnostics, environment and lifecycle. Exit: no unexplained behavioral/security/assertion gaps in declared scope.

**D — lightweight operational/package proof.** W11 executes real trimmed/native package consumers; W12 establishes new public-contract drift gates; W13 verifies dependencies. Finalize measured budgets. Exit: executed supported-target evidence, coherent new API/package shape.

**E — consumer polish and largest apps last.** W20 aligns truthful documentation and light examples; then W20A/B/C activate independently with exact interface/scope decisions. Exit: runnable reviewed samples without duplicated rules or speculative infrastructure.

**F — every-row10/10 review.** W21 binds all evidence to exact revision/artifacts, resolves council findings and reassesses each row separately. No aggregate can hide a gap. Expansion is a later bounded proposal, not a distraction from readiness.

Retain v1.6's semantic/trust/hostile/operational proof intent, but these waves supersede its literal timing where the user changed priorities. Independent evidence tasks may overlap; dependent implementation cannot outrun its decisions.

## Effort and bounded execution

[Estimates and waves](references/estimates-and-waves.md) gives every workstream, wave and role budget. Base core/light-doc effort remains 192–406 engineering-equivalent hours, 66–158 cumulative active agent/tool hours and 35–73 review hours. With the separate incremental knowledge/tooling closeout reserve, core becomes 204–430, 70–166 and 39–81 hours respectively; including all three major apps becomes 267–556, 90–214 and 48–98. Ordinary implementation review/docs work was already included and is not counted twice. These are separate planning views, not additive or guaranteed elapsed time. Confidence is low to medium-low; re-estimate after each wave.

For wave work, use bounded packets, targeted cached source excerpts keyed by revision AND dirty-file hashes, two-minute checkpoints and explicit escalation. Requested time/token caps are not falsely described as tool-enforced. Full protocol and exact-file commit rules: [execution](references/execution-protocol.md). The pilot PR does not use this machinery; its process is the tracker row, one Fable review and the owner's sign-off. Regular coherent owned commits are reviewed; eventual squash covers only this task's approved commits.

## Decisions, deliverables and completion

Closure records must identify exact source→affected .md/spec/rule/memory/tool/self-test/index relationships, actual script/tool build/update/test/run evidence or reasoned N/A, saved lessons (including an explicit none-found result), durable-rule promotion disposition and the accepted/rejected reviewer feedback log. Reopen a completed block when relevant inputs change. Current planning-session lessons are already saved in the [ledger](references/closeout-ledger.md); publication verification remains evidence-driven.

The [decision register](references/decision-register.md) distinguishes approved design direction from unresolved concrete semantics, representation, budgets and support choices. For wave work, exact code/harness paths not established by intake require an inventory and the owner's approval before coding (D22). For the pilot PR, the council ruled that D22 is satisfied by the ten-line file list in the PR description. No execution agent invents a design to fill silence.

Each child supplies concrete tasks, sources, planned evidence paths, prerequisites, independent acceptance and stop rules. Read [source map](references/source-map.md), [acceptance matrix](references/acceptance-matrix.md) and [council preparation](references/council-preparation.md). Publication of this plan does not claim new tests, coverage, benchmarks, implementation or 1.0 readiness.
