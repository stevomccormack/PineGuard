<!-- metadata_header
type: plan
id: technical-readiness-to-1.0-w12
version: 1.0
status: planned
last_updated: 2026-09-26
-->
# W12 — New public API and forward contract consistency

## Purpose and explicit greenfield policy

The user declares PineGuard greenfield for this work. There is **no backward-compatibility, old-release parity, shim or migration requirement**. W12 defines the best coherent new public contract and establishes future drift detection from the approved new baseline. Existing published APIs are inventory evidence, not constraints on design.

This explicitly supersedes legacy compatibility requirements in v1.6 §10 and the original handoff. Naming and abstraction decisions begin in early W22; W12 finalizes the complete public contract after new structure/manifest/failure decisions.

Inputs: W22 taxonomy, W01 semantics, W02 inventory, W03 structure, W04 failures and actual package boundaries. Output: `docs/reports/readiness-public-contract.md`. Exact API baseline/tool/config paths are selected under D13/D22.

## Tasks

| Task | Action | Output |
|---|---|---|
| 12.1 | Inventory all public entry points, overloads, generic constraints, nullability, attributes, extension scopes and package boundaries | Repository-wide public API map |
| 12.2 | Review against W22 naming/taxonomy and DRY/SOLID principles; identify redundancy and inconsistent composition | Astra decisions on removals, renames, consolidation and necessary distinctions |
| 12.3 | Review new contracts for return/failure behavior, discoverability, cancellation/async where present and environment dependence | Approved API design record with exact signature/behavior choices |
| 12.4 | Implement approved global changes through shared abstractions, updating every consumer/test/doc in scope | Complete blast-radius diff; no legacy shims unless a new independent requirement justifies them |
| 12.5 | Capture the approved new API/semantic baseline and configure drift checks | Forward-consistency evidence and deliberate API/semantic drift detection |
| 12.6 | Review final package-facing API with runnable W11/W20 consumers | Contract usability findings and closed discrepancies |

## Acceptance

Every public naming/signature choice follows the approved taxonomy or has a justified exception. No redundant overload/surface remains merely because it existed before. Necessary distinctions are documented; consolidation must not produce speculative abstractions or obscure simple use cases.

The baseline is the newly approved contract. A seeded unreviewed public API change must fail the chosen drift gate. Semantic changes not visible in signatures are checked through W01/W05. Passing an API tool does not prove semantic correctness.

## Controls

Checkpoint after complete API map, after new API approval and after forward drift proof. Astra owns all API decisions; Sol implements; Luna supplies source context and package facts. Stop for a local rename that leaves repository-wide inconsistency, accidental public exposure, hidden redundant code or a proposal to spend effort on old-consumer migration.100% validation coverage applies throughout; baseline replacement is reviewed, never a mechanism to ignore an unexplained change.

Follow the [execution protocol](../references/execution-protocol.md), [decision register](../references/decision-register.md), [quality constitution](../references/quality-constitution.md) and [estimates and waves](../references/estimates-and-waves.md). This child remains Planned.
