<!-- metadata_header
type: plan
id: technical-readiness-to-1.0-w04
version: 1.1
status: planned
last_updated: 2026-09-26
-->
# W04 — New failure contract and projections

> **Mandatory completion gate:** This workstream and every major block inherit the [knowledge, documentation and tooling closure contract](../references/knowledge-and-tooling-closure.md). Save evidence-linked lessons/decisions (or an explicit no-new-lesson outcome), synchronize all relevant BRAIN/Markdown/indexes, build or update required supporting scripts/tools and test/run them (or record reviewed N/A), then complete full Astra review → feedback → fix → recheck. Persist the impact matrix, feedback and owned commit evidence; reopen the unit when relevant context changes. Feature implementation alone cannot close this work.

## Purpose

Choose the best globally consistent new failure model EARLY with W03/W02. Greenfield means old shape compatibility is not required. Inventory current evidence to avoid false assumptions, then decide a coherent model, privacy policy and projection boundaries.

The handoff's code/path/message/severity protocol is a candidate, not a complete approved design. Verified current facts: DataAnnotations has no universal code and Guard NotEmail maps Must Email; do not infer new codes from surface names.

Inputs: W01 semantics, W22 terminology, W02 inventory, src/PineGuard.Core/Codes/MustCodes.Email.cs; Luna locates exact result/adapter/serialization paths. Output: docs/reports/readiness-failure-contracts.md. D08/D15/D22 precede implementation.

## Tasks

| Task | Action | Output |
|---|---|---|
| 04.1 | Inventory actual failure/result/exception shapes and field provenance across every surface | Source-backed matrix incl absent/not-applicable information |
| 04.2 | Specify normal invalid, conversion, invalid configuration, cancellation/timeout/resource and programmer-failure categories | Astra-approved new outcome taxonomy and negation/evaluability boundaries |
| 04.3 | Choose code/path/message/severity/value/input-output/aggregation/order semantics, equality/serialization and privacy | Complete new contract, alternatives and global impact |
| 04.4 | Map each actual applicable Must/Guard/object/FV/DA/ASP.NET projection to that contract | Intentional adaptation and representability limits |
| 04.5 | Implement shared model/mappings and remove redundant local patterns across the repo | Coherent diff with100% validation coverage |
| 04.6 | Prove independent outcomes, ordering/path/code behavior, privacy and allocation cost | W05/W10/W14/W17 evidence |

## Acceptance

Every field/outcome has a defined meaning, provenance, optionality and stability promise. Paths/indexes/keys/escaping and aggregation are explicit where supported. Operational failures cannot accidentally become ordinary invalid results or success through negation.

Messages are stable only if the new contract says so; diagnostic/display text and machine code are distinguished. Attempted values are excluded by default as a privacy preference unless an explicit reviewed need/policy establishes otherwise; classify every other potentially sensitive field too.

No surface is forced to expose nonexistent native information simply to fill a table. A new common mapping is permissible only as an explicit new design with documented semantics. New public contract drift is checked by W12, not against old releases.

Checkpoint after inventory, model decision and first full pilot. Astra decides/reviews; Sol codes; Luna reads/evidence. Stop for guessed codes, duplicated adapter truth, information loss, hidden value leakage or arbitrary catches. Follow [quality constitution](../references/quality-constitution.md), [execution](../references/execution-protocol.md), [decisions](../references/decision-register.md) and [estimates](../references/estimates-and-waves.md). Planned.
