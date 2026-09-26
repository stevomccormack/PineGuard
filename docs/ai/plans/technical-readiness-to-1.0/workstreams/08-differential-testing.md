<!-- metadata_header
type: plan
id: technical-readiness-to-1.0-w08
version: 1.0
status: planned
last_updated: 2026-09-26
-->
# W08 — Differential testing with explicit equivalence domains

## Purpose and scope

W08 supplies independent comparative correctness evidence. It is not an old-release compatibility gate. Compare a selected implementation with an independently justified reference only where their semantic domains are equivalent. A competitor's behavior is evidence, not PineGuard's definition of truth.

Inputs: W01 new contracts, W05 scenarios, official competitor dossiers and genuine Sol comparative synthesis where relevant. v1.6 §8 supports risk-focused differential testing. No exact harness was verified; D10/D22 requires inventory/tool/path approval. Output: `docs/reports/readiness-differential.md`.

## Tasks

| Task | Action | Output |
|---|---|---|
| 08.1 | Identify high-risk rules with independent reference models, platform parsers or relevant established validators | Candidate reference matrix, independence and licensing/dependency review |
| 08.2 | Define domains where semantics genuinely match, including null, culture, syntax and normalization | Astra-approved equivalence specification and explicit exclusions |
| 08.3 | Design adapter normalization without erasing meaningful discrepancies | Exact harness/path map and expected result categories |
| 08.4 | Run curated boundaries and bounded generated inputs against both implementations | Replayable input/result/environment artifacts |
| 08.5 | Triage every mismatch against W01, never by majority vote | Defect/reference limitation/specification question/expected divergence decision |
| 08.6 | Retain minimized confirmed regressions and seed a detector failure | Demonstrated sensitivity, CI cost/routing and artifact requirements |

## Acceptance

The reference is independent of PineGuard's implementation and shares only the explicitly documented contract domain. Results distinguish reference agreement from specification correctness. Unsupported reference features and known semantic divergence remain visible; they are not deleted merely to improve agreement statistics.

A seeded discrepancy fails. A known justified divergence is recorded without becoming an unexplained permanent suppression. Package versions, environment, source/license and input provenance are captured. Mismatch fixes address shared root causes across the repository and rerun every affected surface.

## Controls

Checkpoint after reference/domain review and after mismatch triage. Luna does all research/reading; Sol implements and runs the approved harness; Astra decides disputed semantics. Stop for unlicensed copied reference code, shared-implementation oracles, unsupported equivalence assumptions or uncontrolled dependency expansion. W08 does not create an obligation to copy competitor behavior or maintain historical PineGuard APIs.

Follow the [execution protocol](../references/execution-protocol.md), [decision register](../references/decision-register.md), [quality constitution](../references/quality-constitution.md) and [estimates and waves](../references/estimates-and-waves.md). This child remains Planned.
