<!-- metadata_header
type: plan
id: technical-readiness-to-1.0-w20
version: 1.0
status: planned
last_updated: 2026-09-26
-->
# W20 — Documentation, lightweight examples and late major samples

## Purpose and sequencing

W20 aligns product claims and consumer guidance with executed evidence. Lightweight snippets/smoke consumers occur as soon as they clarify the new contract. The largest API, DDD and Clean Architecture sample projects occur LAST, as separately gated subplans. This explicit user direction supersedes any implication that broad application projects precede core readiness.

Inputs: README.md §§Quality metrics, Packages, Where PineGuard fits, Built by AI; docs/reports/readme-verification-2026-09-25.md §Release and documentation checks; W11–W13, W22 and approved competitor dispositions. Output: docs/reports/readiness-documentation-support.md.

## Tasks

| Task | Action | Output |
|---|---|---|
| 20.1 | Inventory current product/package/support/quality claims and examples | Claim-to-evidence register, published/source-only distinctions |
| 20.2 | Rewrite guidance around approved new semantics and taxonomy | Coherent terminology and call-site guidance; no legacy migration requirement |
| 20.3 | Execute minimal snippets against selected real packages | Commands and expected success/failure results |
| 20.4 | Verify links, anchors, diagrams, formatting and package/support tables | Current proof; historical41/29/3 checks remain dated evidence |
| 20.5 | After core/deployment/contract gates, activate major subplans separately | Exact interface packets and individual acceptance records |
| 20.6 | Reconcile README/quality/positioning claims with W21 | No claim beyond current evidence |

## Major projects, scheduled last

- [W20A API validation sample](20-samples/api-validation.md)
- [W20B DDD validation sample](20-samples/ddd-validation.md)
- [W20C Clean Architecture validation sample](20-samples/clean-architecture-validation.md)

Each has independent ownership, estimates and acceptance. They reuse stable package contracts and do not force speculative core abstractions merely to make an elaborate sample.

## Acceptance

Every advertised package/surface/target has current evidence. Explain Must, Guard, object validation and attributes using concrete caller-specific scenarios. Coverage claims name the measured Coverlet line/branch scope and exclusions; conformance counts are separate. Competitor claims use checked official evidence and avoid unmeasured superiority.

Large samples demonstrate validation integration, not production certification of unrelated infrastructure. Non-validation application code has no automatic100% coverage mandate; all validation logic/projections remain under the hard requirement.

## Controls

Checkpoint after inventory, lightweight examples and each major sample. Astra reviews architecture/docs; Sol codes; Luna reads/tests/looks up. Inventory and approve exact sample paths first. Stop for speculative scope, duplicated validation logic, undocumented package availability or using samples to hide missing core contracts.

Follow the [execution protocol](../references/execution-protocol.md), [decision register](../references/decision-register.md), [quality constitution](../references/quality-constitution.md) and [estimates and waves](../references/estimates-and-waves.md). This child remains Planned.
