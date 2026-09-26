# PineGuard technical readiness rubric — 25 September 2026

This is Astra's engineering assessment of the source, tests, build/release configuration and
consumer contracts inspected in this session. It is not a security certification or an objective
measurement of perfection. No new build, test, coverage, benchmark or workflow run was performed
for this review. The [prior verification report](readme-verification-2026-09-25.md) remains the
evidence for 37,749 passing local Windows Debug test executions across .NET 8/.NET 10.

## How to interpret the rubric

Scores are coarse **technical readiness judgements**, not defect probabilities or measurements
of speed. A low performance-evidence score does not mean the library is slow. Confidence refers
to the supporting observation, not certainty about every code path. No weighted overall score
is calculated: a release-critical CI gap cannot be cancelled out by good documentation or more
test cases. Adoption, community size and market feedback do not earn or lose technical points.

- **0–3:** foundational capability or trustworthy evidence is largely absent.
- **4–6:** useful capability exists, but important contracts or verification gates remain incomplete.
- **7–8:** strong foundation with specific material gaps preventing a fully supported claim.
- **9:** comprehensive evidence within the declared support scope; only minor gaps remain.
- **10:** the observable target in the row is met and enforced for that scope, with known limits
  documented. This is a maintained engineering bar, not proof of correctness for all possible input.

## Rubric

| Dimension | Readiness / confidence | Existing strength | Why it is not 10 | Observable 10/10 technical criterion |
|---|---|---|---|---|
| Semantic correctness and failure contracts | **7/10; high for inspected paths** | Shared predicates/parsers, typed results and extensive regression tests | Source-derived command-newline inconsistency and regex timeout/negation contract gaps remain to be reproduced/resolved | Declared null/options/normalization/error contracts have independent boundary evidence; confirmed material defects resolved; regressions enforced |
| Architecture and cross-surface consistency | **8/10; high** | Core → Must → adapters reuse, stable code constants, shared fixtures and existing audits | Metadata/projection policies remain distributed; parity/code audits are not in the current CI gate; runtime equivalence is not established for every supported projection | One authoritative contract representation, checked bindings and policy-aware conformance across declared projections; intentional omissions explicit |
| Assertion strength and adversarial assurance | **7/10; medium** | 37,749 passing framework executions and configured line/branch coverage thresholds | Counts/coverage do not show independent oracle strength; mutation/property/fuzz/differential work remains planned | Reproducible risk-focused campaigns, meaningful mutant dispositions and regression corpus; independent expectations detect shared algorithm mistakes |
| Consumer API, composition and extensibility | **8/10; medium** | Typed/original results, ordered async validation, cancellation, code/message/path overrides, localization and exception-policy seams | Lifecycle/singleton safety is convention-based and must be qualified for public builders, retained rule handles and consumer callbacks; supported consumer combinations need artifact-level evidence | Clear supported configuration/use lifecycle, targeted concurrency/cancellation tests, documented custom extension examples and packed-consumer verification |
| Security boundaries and data handling | **6/10; medium** | Candid OWASP heuristic limitations; XML prohibits DTDs/external resolution; failure printing omits raw values | Heuristic naming/claims, resource exhaustion, output/logging boundaries and independent security review need a coherent evidence set | Explicit threat model and data-flow contract; bounded hostile-input tests; independent review findings resolved or scoped; no unsupported safety claims |
| Performance and resource predictability | **4/10; high confidence in evidence gap** | Several allocation-conscious primitives and reusable validators; costs are inspectable | No established benchmark/budget evidence; result allocation, boxing and reflective/compiled setup are not yet measured against consumer needs | Approved workload-specific absolute/relative budgets, allocations and resource limits; repeatable enforcement with justified tolerances |
| API/package compatibility and deployment | **6/10; medium** | Multi-target builds, deterministic settings, SourceLink/symbol packaging and integration packages | API baselines, packed-consumer checks and claimed trimming/AOT paths lack dedicated gates; published/source API boundary needs continuing verification | Explicit version/TFM support matrix, checked public API changes, coherent packed dependencies and executable consumers on claimed deployment modes |
| CI, tooling and release trustworthiness | **5/10; high for source-derived paths** | Builds/tests/coverage/static checks, frozen CLI install, weekly NuGet/action updates, short-lived NuGet OIDC credentials | Routing can skip relevant suites/tool tests; missing expected coverage may be treated as no coverage needed; important audits dormant; release workflow does not explicitly require all same-revision readiness evidence | Changed-input routing tested; expected evidence fails closed; CLI verifies itself; semantic audit policy enforced; release requires evidence for the exact artifact/revision |
| Documentation and maintainability | **7/10; high for sampled drift** | Detailed Brain, source XML docs, package guides, CODEOWNERS and corrected README metrics | Contributor guidance and workflow comments have stale facts; supported claims/decisions need one traceable inventory | Executable/versioned examples, accurate support/quality claims, reconciled API decisions and documentation/audit ownership without duplicated policy |

The architecture target is enforced contract ownership, not a required class hierarchy. A manifest
is the planned pilot mechanism; equivalent checks over authoritative existing declarations can
satisfy the same criterion if they provide the same guarantees with less maintenance cost.

## Highest-priority findings and evidence

### R1 — CI change routing does not cover all shared inputs

**Source-derived defect; high confidence; not reproduced by dispatching workflows.**
[CI path filters](../../.github/workflows/ci.yml), lines 103–121, include `tests/**` in `any-src`
but only root-level build inputs in `shared`. A PR changing only `tests/Directory.Build.props`
therefore selects the build without a project-specific flag or RUN_ALL. The per-project decisions
at lines 264–313 can skip all relevant test suites.

The same `any-src` filter omits the CLI/workspace/lockfile paths. The audit job is gated by it
or main (line 743). Its steps build/run the CLI (lines 760–771), but do not invoke its existing
Vitest/typecheck/lint scripts in [the CLI package](../../apps/cli/package.json), lines 15–21.
Workflow, coverage-setting and documentation-only changes also need an explicit appropriate
verification route; they need not all trigger every .NET test.

**Why it matters:** adding sophisticated new checks is insufficient if edits to their own
implementation or shared inputs do not run them. Fix routing and test the routing policy first.

### R2 — coverage evidence can be absent without failing the coverage path

**Source-derived fail-open path; high confidence; no claim of a past failed release.**
[CI](../../.github/workflows/ci.yml) allows test-result download failure (line 403), reports
`found=false` when the directory/files are missing (lines 409–421), and runs thresholds only
when files were found (line 550). It does not first require the expected artifact set for the
test jobs selected by that run.

**Why it matters:** "no tests were intentionally selected" and "selected tests produced no usable
evidence" are different states. A 100% threshold cannot validate missing measurements. Require
an expected-scope/artifact manifest and fail for missing/malformed expected evidence, while
allowing an explicitly empty expected set for legitimate skipped runs.

### R3 — useful semantic audits exist but are not active CI gates

**Configuration fact; high confidence.** [layer-parity](../../apps/cli/src/audit/rules/layer-parity.ts)
line 364 and [must-codes](../../apps/cli/src/audit/rules/must-codes.ts) line 94 declare `gate: false`.
The [engine](../../apps/cli/src/audit/engine.ts), lines 220–221, filters those out for the workflow's
`--gate` invocation. Existing audits must be credited without calling these two blocking checks.

**Why it matters:** the proposed rule manifest should build on useful existing machinery, but
its enforcement must be demonstrated. Inventory active/dormant rules, run a reviewed baseline,
triage semantic debt and enable selected high-value checks. Do not indiscriminately activate
every style rule or suppress all findings to obtain green CI.

### R4 — semantic defects outrank architectural rewrites

**Source-derived findings; runtime confirmation pending.** The command-newline and regex
timeout/negation findings, with source links and qualifications, are recorded in
[the plan's evidence baseline](../ai/plans/technical-readiness-to-1.0.md).

**Why it matters:** every projection can consistently repeat the same wrong predicate. A new
class hierarchy or manifest cannot replace independent correctness expectations. Preserve
existing executable reuse and fix confirmed semantics before expanding generation.

### R5 — release evidence and reproducibility need explicit contracts

**Verification/governance gap; medium confidence about effective remote protection.**
[Publish workflow](../../.github/workflows/publish.yml), lines 33–63, restores, builds, tests,
packs and publishes with OIDC. It does not itself require coverage/audit/compatibility/consumer
evidence from the same revision. Remote branch/environment protection was not inspected, so
this is not a claim that an unauthorized release is possible.

SDK selection uses moving `8.0.x` and `10.0.x` versions (lines 29–31); reviewed root configuration has
no global.json. A controlled reference toolchain plus an upgrade-compatibility lane is one
option; automatic upgrades are not inherently wrong. [Dependabot](../../.github/dependabot.yml)
covers NuGet and GitHub Actions but has no npm ecosystem entry for the pnpm-based CLI.

**Why it matters:** enforce artifact/revision traceability, deliberate toolchain changes and
dependency review across the tooling that enforces product quality. Do not mandate a particular
provenance product or silently assume remote protections.

### R6 — qualify validator lifecycle promises, retaining current extensibility

**Intentional design boundary needing decision/evidence; not a confirmed race in normal use.**
[MustValidator](../../src/PineGuard.Core/MustClauses/MustValidator.cs), lines 25–35, states
constructor-only configuration by convention and stores a mutable runner list. The
[inline builder](../../src/PineGuard.Core/MustClauses/InlineMustValidator.cs) exposes registration,
and [rule handles](../../src/PineGuard.Core/MustClauses/MustPropertyRule.cs), lines 27–82, permit
later refinements. "Immutable after construction" is not an enforced universal property.

Async execution already checks cancellation and preserves order, and corresponding tests exist
in [MustValidatorTests](../../tests/PineGuard.Core.UnitTests/MustClauses/MustValidatorTests.cs),
including cancellation between rules at line 591. Per-rule overrides and
[ASP.NET localization](../../src/PineGuard.AspNetCore/StringLocalizerMustFailureMessageResolver.cs),
lines 42–55, are real strengths rather than missing features.

**Why it matters:** document whether configuration must stop before first use, qualify concurrent
reuse for user callbacks/dependencies, and test the supported lifecycle. Enforced freezing is
one possible decision, not a compulsory rewrite or reason to remove useful builder APIs.

### R7 — maintenance claims need continued reconciliation

**Confirmed documentation drift; high confidence.** [CONTRIBUTING](../../CONTRIBUTING.md) still
says 14 test projects (line 29) and unconditionally describes Must as never throwing (line 70).
The current [CI comment](../../.github/workflows/ci.yml), lines 735–739, says the audit baseline
is not committed, although [baseline.json](../../apps/cli/config/baseline.json) now exists.
Neither its line count nor that stale comment is a current finding count.

**Why it matters:** quality rules and consumer promises are themselves part of the contract.
Reconcile user guidance, source contracts, active audit configuration and dated evidence. Keep
the existing README corrections; do not replace them with fresh unsupported claims.

## Assessment decision

PineGuard has a strong architectural and API foundation. The immediate constraint is not lack
of another abstraction; it is incomplete assurance that the intended checks always execute,
that independent tests establish semantics, and that the shipped artifact carries that evidence.

Prioritize R1–R3 alongside the concrete semantic fixes. Then prove the small manifest/conformance
pilot, supported consumer lifecycle and release evidence. The already planned property, fuzz,
differential, Stryker.NET and performance work strengthens those contracts; it is not a checklist
of tools whose mere installation earns points. The [implementation plan](../ai/plans/technical-readiness-to-1.0.md)
contains the updated work and acceptance criteria. No implementation was performed by this review.
