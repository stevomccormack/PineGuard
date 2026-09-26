<!-- metadata_header
type: plan
id: technical-readiness-to-1.0-w16
version: 1.2
status: planned
last_updated: 2026-09-26
-->
# W16 — Cross-operation resource limits

> **Mandatory completion gate:** This workstream and every major block inherit the [knowledge, documentation and tooling closure contract](../references/knowledge-and-tooling-closure.md). Save evidence-linked lessons/decisions (or an explicit no-new-lesson outcome), synchronize all relevant BRAIN/Markdown/indexes, build or update required supporting scripts/tools and test/run them (or record reviewed N/A), then complete full Astra review → feedback → fix → recheck. Persist the impact matrix, feedback and owned commit evidence; reopen the unit when relevant context changes. Feature implementation alone cannot close this work.

## Purpose

W16 defines intentional resource behavior for the supported validation model. The handoff proposes operational bounds; the new greenfield design may adopt the best policy, but numeric limits and APIs require evidence. W15 owns regex-specific engine/pattern/timeout choices; W16 owns general input, traversal, iteration, callback/cancellation and cache budgets where applicable.

Inputs: W14 threat model, W10 early benchmarks, W01 contract domains, W19 lifecycle and W15 regex measurements. Output: `docs/reports/readiness-resource-limits.md`. D11/D14/D22 block arbitrary implementation choices.

## Tasks

| Task | Action | Output |
|---|---|---|
| 16.1 | Inventory unbounded work and allocations: strings, collections, graphs, repetitions, callbacks, caches and result accumulation | Source-backed cost map with attacker/control assumptions |
| 16.2 | Measure representative and hostile growth behavior | Size/depth/count versus time/allocation evidence and failure modes |
| 16.3 | Decide supported domains and needed limits/configuration/cancellation semantics | Astra policy with units, defaults, ownership and operational outcome |
| 16.4 | Implement enforcement in the shared abstraction rather than adapter-specific patches | Exact global diff and approved failure mapping |
| 16.5 | Exercise boundary, just-over-boundary, cancellation, cycle/repeated-reference and huge-input cases where relevant | Independent correctness/resource assertions and100% validation coverage |
| 16.6 | Route bounded hostile checks and document guarantees | W00 artifacts, W14 claims and W20 consumer guidance |

## Decisions that must be explicit

Whether a graph is supported, cycle handling, repeated-reference semantics, maximum depth/node count, failure-collection size, callback cancellation, configurable limits and cache lifecycle are distinct choices. A limit need not be exposed as a new public option unless justified. No default number is selected because it appears in the proposed handoff.

Do not conflate normal invalid input with refusal to evaluate because of resource policy. W04 must map operational outcomes deliberately across applicable surfaces. Async/cancellation requirements apply only where the chosen contract supports them.

## Acceptance

For every declared costly operation, either bounded behavior is demonstrated or the supported domain/usage assumption is explicit and approved. Chosen limits have measured rationale and deterministic boundary outcomes. Policies do not silently truncate failures or accept unchecked data. A seeded missing-limit/incorrect-boundary case must be detected.

## Controls

Checkpoint after cost inventory and policy approval. Sol changes shared enforcement; Luna provides measurements/source reading; Astra decides tradeoffs. Stop for unmeasured arbitrary constants, hidden truncation, unbounded caches, limit bypass through another adapter or tests that hang without approved process bounds. W16 does not require a universal graph engine if the best architecture does not need one.

Follow the [execution protocol](../references/execution-protocol.md), [decision register](../references/decision-register.md), [quality constitution](../references/quality-constitution.md) and [estimates and waves](../references/estimates-and-waves.md). This child remains Planned.

## Worked code example

This example is documentation, not an implemented PineGuard change. Its classification, dependencies and verification scope are stated below; complete source and reproduction instructions are in [Code examples](../references/code-examples.md).

```csharp
internal static class Boundary
{
    internal const int InputLimit = 128;
    internal static Response Invoke(string raw) =>
        TryCreate(raw, out _) ? new(200, []) : new(400, ["illustration.invalid-input"]);
    internal static bool TryCreate(string raw, out PositiveAmount? amount)
    {
        amount = null;
        if (raw.Length > InputLimit || !int.TryParse(raw, NumberStyles.Integer,
                CultureInfo.InvariantCulture, out var number)) return false;
        return PositiveAmount.TryCreate(number, out amount);
    }
}
```

Purpose: provisional D11/D26 128-character raw-input budget, rejection before parsing. 129digits→400; this is not selected production size/error policy, and per-call allocations remain observable rather than budgeted. Status pending.
