<!-- metadata_header
type: plan
id: technical-readiness-to-1.0-w17
version: 1.2
status: planned
last_updated: 2026-09-26
-->
# W17 — Optional diagnostics and privacy

> **Mandatory completion gate:** This workstream and every major block inherit the [knowledge, documentation and tooling closure contract](../references/knowledge-and-tooling-closure.md). Save evidence-linked lessons/decisions (or an explicit no-new-lesson outcome), synchronize all relevant BRAIN/Markdown/indexes, build or update required supporting scripts/tools and test/run them (or record reviewed N/A), then complete full Astra review → feedback → fix → recheck. Persist the impact matrix, feedback and owned commit evidence; reopen the unit when relevant context changes. Feature implementation alone cannot close this work.

## Purpose

W17 decides whether diagnostics provide sufficient value, then defines a coherent opt-in contract if adopted. The handoff proposes optional diagnostics and excluding attempted values by default. Neither a new diagnostics API nor a particular payload is assumed approved.

Inputs: W04 failure fields, W14 privacy/threat model, W10 early cost baseline and W22 naming. Output: `docs/reports/readiness-diagnostics.md`. D15/D22 requires current instrumentation inventory and an Astra decision before adding infrastructure.

## Tasks

| Task | Action | Output |
|---|---|---|
| 17.1 | Inventory current logging/events/activities/metrics and identify concrete diagnostic questions | Source map, user benefit and gap statement |
| 17.2 | Compare no-new-surface, existing mechanisms and a minimal opt-in mechanism | Astra decision with ownership, payload, privacy and cost |
| 17.3 | Specify enabled/disabled behavior, cardinality, correlation and error handling | Exact contract; no accidental attempted-value or secret emission |
| 17.4 | Implement only the selected mechanism at shared execution boundaries | Global instrumentation diff without duplicate adapter logging |
| 17.5 | Test payload redaction/absence, disabled behavior and listener failures; measure overhead | W14/W10 evidence and100% scoped validation coverage |
| 17.6 | Document safe consumer usage and route required tests | W20 guidance and W00 artifacts |

## Required decisions

A code/path/message may itself contain sensitive or attacker-controlled data. Classify each field rather than assuming only AttemptedValue is sensitive. Decide high-cardinality labels, retention and listener callback behavior. Do not collect values by default merely for debugging convenience. If diagnostics are unnecessary, record a reasoned no-new-surface decision and demonstrate existing observability satisfies the declared support scenario.

## Acceptance

Approved diagnostic questions can be answered without changing validation truth or failure order. Disabled overhead and enabled payload/overhead satisfy W10's approved budgets. No hidden allocations, synchronous blocking sink, swallowed validation exception or sensitive value leak appears without an explicit reviewed contract.

## Controls

Checkpoint after need/policy decision and after cost/privacy proof. Sol implements the smallest coherent approved mechanism; Luna inventories and gathers evidence; Astra reviews. Stop for speculative observability frameworks, duplicated events across projections, unbounded cardinality or unapproved payload fields. W17 is independently gateable: completion may be an approved absence of new instrumentation, but cannot be a silent skip of privacy/performance review.

Follow the [execution protocol](../references/execution-protocol.md), [decision register](../references/decision-register.md), [quality constitution](../references/quality-constitution.md) and [estimates and waves](../references/estimates-and-waves.md). This child remains Planned.

## Worked code example

This example is documentation, not an implemented PineGuard change. Its classification, dependencies and verification scope are stated below; complete source and reproduction instructions are in [Code examples](../references/code-examples.md).

```csharp
internal static class Diagnostics
{
    internal static string? Event(bool enabled, Verdict verdict) =>
        enabled && !verdict.Passed ? "rule=" + PositiveInt.Id + ";code=" + verdict.Code : null;
}
```

Purpose: candidate D15 opt-in defaults off, enabled output contains only fixed rule/code. Failed -987654321→null when off, exact safe event when on. No attempted value, free-form input, property value or sensitive parameter name in output. Current MustResult.Value is not logging consent. Status pending.
