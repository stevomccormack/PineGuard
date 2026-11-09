<!-- metadata_header
type: plan
id: technical-readiness-to-1.0-w03
version: 1.4
status: planned
last_updated: 2026-11-09
-->
# W03 — Early new static rule structure and lowering

> **Status 2026-11-09:** not activated. The plan-gated council ran on 2026-11-09 and ruled that this plan is not activated as written; the only unit that may start, on the owner's explicit go, is the string→DateOnly pilot PR described in [plan.md § Where this plan stands](../plan.md#where-this-plan-stands-2026-11-09) and register decision D29. The completion gate, Astra/Luna/Sol roles, checkpoints and D22 inventory steps below apply to a later wave activation, not to the pilot PR.

> **Mandatory completion gate:** This workstream and every major block inherit the [knowledge, documentation and tooling closure contract](../references/knowledge-and-tooling-closure.md). Save evidence-linked lessons/decisions (or an explicit no-new-lesson outcome), synchronize all relevant BRAIN/Markdown/indexes, build or update required supporting scripts/tools and test/run them (or record reviewed N/A), then complete full Astra review → feedback → fix → recheck. Persist the impact matrix, feedback and owned commit evidence; reopen the unit when relevant context changes. Feature implementation alone cannot close this work.

## Approved direction

Prove the NEW rule structure FIRST through the string→DateOnly pilot PR (council 2026-11-09, register D29): if any adapter needs logic the rule object cannot supply, or the new path allocates where the current one does not, W03 returns to redesign. W22 taxonomy follows the pilot and takes its owner-signed names table as first input. Preserve cheap static executable truth as a design principle, not old API shapes. The user authorizes greenfield design without legacy compatibility/shims/migrations. Avoid per-call polymorphic allocation graphs and speculative abstractions unless concrete evidence justifies a specific choice.

Inputs: W22 inventory/terminology; W01 truth; W02 manifest interface; W04 failures; W10 EARLY baseline. Verified source anchors: src/PineGuard.Core/Rules/OwaspRules.cs, StringRules.cs, src/PineGuard.MustClauses/MustOwaspClauses.cs and MustStringClauses.cs. Output: `+ docs/reports/readiness-static-kernel.md`. Luna inventories remaining exact paths; Astra approves D22.

## Tasks

| Task | Action | Output |
|---|---|---|
| 03.1 | Trace current executable semantics, duplicated branches, normalization/conversion and all adapters repository-wide | Complete responsibility/call-flow map |
| 03.2 | Specify target rule identity/input/output/evaluation responsibilities with W22/W01/W04 | Astra-approved new static structure and exact public/internal boundaries |
| 03.3 | Define how W02 manifest describes executable truth without a second runtime/manual hierarchy | Approved manifest/structure interface and generation decision if justified |
| 03.4 | Implement one vertical pilot slice across actual surfaces; remove superseded redundancy coherently | Global source/test/doc/audit diff with independent expectations |
| 03.5 | Measure construction/steady-state/allocation/boxing costs against W10 baseline | Design tradeoff evidence before broad rollout |
| 03.6 | Extend by reviewed rule-family packets; detect remaining local outliers/duplicate semantics | Consistent structure,100% validation coverage and W05 conformance |

## Acceptance and blocked detail

Every rule has one clear executable semantic authority and each projection only adapts documented representation. Exact structs/functions/delegates/generation choices require Astra decisions; the required early structure is not optional research. No legacy shape justifies redundancy.

Allocation, boxing, closure/reflection and exception costs are measured where relevant. No unsupported zero-allocation claim follows from static syntax. Formatting/analyzer/DRY/SOLID standards apply across the whole affected graph.100% coverage cannot be weakened to make restructuring easier.

Checkpoint after inventory, approved structure, first vertical slice and each measured rollout. Stop for local patches, unexplained abstraction, missing source context, semantic uncertainty or regression without a decision. Use the [quality constitution](../references/quality-constitution.md), [execution protocol](../references/execution-protocol.md), [decision register](../references/decision-register.md) and [estimates](../references/estimates-and-waves.md). Planned; no implementation performed.

## Approved test-project boundary

The [approved testing project layout](../references/test-project-layout.md) is authoritative for this workstream's project identity, folder responsibility and execution kind. Use its workstream mapping; do not create one project per testing technique or move this crosscutting work into existing library unit suites. Reuse PineGuard.Testing fixtures and typed projections. Required projects and artifacts must exist before the applicable implementation gate can close; Planned status is not execution evidence.

## Worked code example

This example is documentation, not an implemented PineGuard change. Its classification, dependencies and verification scope are stated below; complete source and reproduction instructions are in [Code examples](../references/code-examples.md).

```csharp
internal static class PositiveInt
{
    internal const string Id = "illustration.number.positive";
    internal const string FailureCode = "illustration.number.not-positive";
    internal static Verdict Evaluate(int? value)
    {
        var passed = value is not null && value.Value > 0;
        return new(passed, passed ? null : FailureCode);
    }
}
```

Purpose: cheap complete evaluation by one static method without per-call descriptor allocation. Inputs 1/0 → true/false. Proposed miniature; source current NumberRules uses generic INumber under net8+; no runtime rule-object hierarchy proposed. Status pending.
