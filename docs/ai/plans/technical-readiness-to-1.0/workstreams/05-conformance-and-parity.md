<!-- metadata_header
type: plan
id: technical-readiness-to-1.0-w05
version: 1.0
status: planned
last_updated: 2026-09-26
-->
# W05 — Cross-surface conformance and parity

## Purpose

P2 makes semantic agreement observable without using the implementation as its own oracle. v1.6 §7 proposes shared RuleScenario<TInputs> with independently authored expected outcomes. The rule manifest provides coverage/provenance; it does not generate expected truth by calling Must.

Inputs: W01 pilot specifications, W02 inventory and W04 mapping decisions; existing shared fixture conventions identified by Luna; `tests/PineGuard.Testing/Fixtures/OwaspRulesFixtures.cs` is a verified fixture anchor, not an assumed universal harness.

## Preconditions and outputs

D07/D08 must be resolved for the pilot. Luna inventories exact fixture projects, adapters and test paths, then Astra approves D22's path map. Output: `docs/reports/readiness-conformance.md`, approved scenario/adapter fixtures and machine-readable evidence through W00's artifact contract.

## Tasks

| Task | Action | Output |
|---|---|---|
| 05.1 | Select independent pilot cases from W01 and label valid, invalid, conversion/configuration/operational outcomes | Reviewed case catalog with rationale and oracle provenance |
| 05.2 | Design RuleScenario inputs and expected semantics without coupling to a concrete surface return type | Astra-approved fixture contract under test specifications |
| 05.3 | Implement adapters for actual applicable Must/Guard/object/integration entry points | Explicit input adaptation and result normalization mappings |
| 05.4 | Compare actual results to independent expectations and each other only inside approved equivalence domains | Per-rule/surface matrix with meaningful unsupported/not-applicable entries |
| 05.5 | Seed predicate, code, path/ordering and returned-value drift where those fields are contractual | Proof each relevant detector fails, followed by clean seed removal |
| 05.6 | Connect completeness to W02 and gate activation to W00 | Audit references and reviewed rollout by rule family |

## Required scenarios

Include valid/invalid boundaries, null/empty where applicable, conversion success/failure and the approved environment. For each rule establish whether negation is a logical complement over the entire domain or only valid evaluation results. Do not assert complement identities over timeouts or unsupported/configuration states without D05.

Compare codes only through documented provenance. Compare messages/paths/order only to the stability actually promised. Do not make DataAnnotations satisfy a nonexistent universal-code contract. Do not interpret a Guard exception shape as equivalent to an object validator result without an explicit adapter policy.

## Acceptance and control

Every pilot case runs through every applicable approved projection; gaps have a reviewed reason. No expected outcome calls Must, delegates to the same predicate under test or derives directly from its branch logic. A seeded semantic error must fail even if all wrappers share the same faulty implementation.

The pilot checkpoint proves only the pilot. Final W05 completion requires reviewed family-by-family rollout across every declared validation rule and applicable surface, reconciled with the complete W01/W02 inventory and the hard coverage contract. Each rollout packet has independent expectations and explicit supported/not-applicable mappings. Checkpoint after oracle review, after the pilot, after each family and before global gate activation. Stop on an unresolved mismatch rather than modifying expected results to match production output.

Follow the [execution protocol](../references/execution-protocol.md), [decision register](../references/decision-register.md) and [source map](../references/source-map.md). This child remains Planned; no implementation is authorized by this document.
