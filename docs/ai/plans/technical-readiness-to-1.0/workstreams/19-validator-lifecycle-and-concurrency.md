<!-- metadata_header
type: plan
id: technical-readiness-to-1.0-w19
version: 1.0
status: planned
last_updated: 2026-09-26
-->
# W19 — Validator lifecycle and concurrency

## Purpose and baseline

Characterize lifecycle EARLY because it affects new structure, construction costs and public contracts; complete concurrency proof after decisions. Source inspection identified mutable validator structures, not a reproduced race. Never state they are thread-safe or racy without evidence.

Verified inputs: `src/PineGuard.Core/MustClauses/MustValidator.cs`, `InlineMustValidator.cs`, `MustPropertyRule.cs`, and `tests/PineGuard.Core.UnitTests/MustClauses/MustValidatorTests.cs`. Output: `docs/reports/readiness-lifecycle-concurrency.md`.

## Decisions and tasks

D17 governs construction, mutation, reuse and concurrent invocation. The greenfield design may change these APIs for a better coherent contract; preserving old usage is not required.

| Task | Action | Output |
|---|---|---|
| 19.1 | Inventory state ownership, caches, rule construction, callbacks, per-run data and registration lifetimes | Lifecycle/state diagram with exact source/test ownership |
| 19.2 | Characterize sequential reuse, mutation timing, concurrent independent instances and shared-instance execution | Reproductions classified as supported/unknown until decided |
| 19.3 | Compare immutable/freeze-after-build/mutable or constrained-use models against simplicity and benchmarks | Astra decision, allowed operations and invalid-use outcome |
| 19.4 | Implement the selected model at shared state boundaries | Global diff and consumer guidance; no ad hoc locks as an unexplained patch |
| 19.5 | Test allowed interleavings, isolation, cache safety and callback/ambient-state interactions | Deterministic stress/replay evidence and100% validation coverage |
| 19.6 | Benchmark construction/reuse and finalize DI/consumer guidance | W10/W11/W20 inputs and forward-contract checks |

## Acceptance

The public lifecycle contract states exactly what can be shared, when configuration may change, and what happens on invalid lifecycle usage. Concurrent validation results do not contaminate each other under supported usage. Cache growth/lifetime complies with W16. Stress tests supplement a state-ownership argument; a passing stress run alone is not proof.

User callbacks and externally mutable objects have explicit boundaries. Do not promise to make arbitrary consumer state thread-safe. Any synchronization/freeze/copy strategy is reviewed for allocations, deadlocks and performance.

## Controls

Checkpoint after characterization and before selecting a lifecycle model. Sol implements approved architecture; Luna supplies readings and runs evidence collection; Astra decides and reviews. Stop for unverified race claims, blanket locking, undocumented singleton assumptions or flaky tests that rely only on sleep/timing. This workstream may start early independently, but final acceptance depends on new W03/W04 contracts and W10 measurements.

Follow the [execution protocol](../references/execution-protocol.md), [decision register](../references/decision-register.md), [quality constitution](../references/quality-constitution.md) and [estimates and waves](../references/estimates-and-waves.md). This child remains Planned.
