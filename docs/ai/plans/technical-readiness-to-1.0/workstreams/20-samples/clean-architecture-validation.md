<!-- metadata_header
type: plan
id: technical-readiness-to-1.0-w20c
version: 1.2
status: planned
last_updated: 2026-09-26
-->
# W20C — Major Clean Architecture validation sample, scheduled last

> **Mandatory completion gate:** This workstream and every major block inherit the [knowledge, documentation and tooling closure contract](../../references/knowledge-and-tooling-closure.md). Save evidence-linked lessons/decisions (or an explicit no-new-lesson outcome), synchronize all relevant BRAIN/Markdown/indexes, build or update required supporting scripts/tools and test/run them (or record reviewed N/A), then complete full Astra review → feedback → fix → recheck. Persist the impact matrix, feedback and owned commit evidence; reopen the unit when relevant context changes. Feature implementation alone cannot close this work.

## Purpose and entry gate

Demonstrate validation responsibilities across approved application boundaries and dependency directions after the core library and lightweight consumer proof are stable. This is intentionally one of the final and largest bodies of work. It must not dictate a speculative framework rewrite.

Inputs: W12 new public API/package boundaries, W04 error mapping, W19 lifetime policy, W11 deployment scope and the accepted API/DDD sample lessons where reused. Astra must approve exact layers, dependency direction, use case, adapters and framework choices before coding.

## Tasks

1. Luna inventories existing solution/project/sample conventions and supplies an exact project/file ownership map. Output `docs/reports/readiness-sample-clean-architecture.md`.
2. Astra specifies a small end-to-end use case, validation entry/exit boundaries, domain/application/adapter responsibilities and allowed dependencies.
3. Sol creates only necessary projects/abstractions and composes canonical validation; adapters translate representation without reimplementing rules.
4. Add architecture/dependency checks for the selected boundaries and seed a forbidden reference to prove detection.
5. Exercise one complete valid flow plus independent invalid/conversion/operational flows through the actual selected adapters. Verify failure information and privacy across boundaries.
6. Confirm validation coverage, package/target compatibility with the NEW approved scope, deterministic commands and run instructions.
7. Astra reviews solution clarity, DRY/SOLID, no redundant interfaces, global naming and evidence; commit owned files.

## Acceptance and boundaries

Each layer has a demonstrated responsibility and dependency direction. No project exists merely to mimic a diagram; no service/interface is created for a single unneeded indirection. The end-to-end example runs and detects skipped validation, wrong failure translation and forbidden dependencies.

All validation branches meet100% audited line/branch coverage. Unrelated infrastructure has proportionate tests without an automatic100% demand. The sample does not certify production infrastructure or require compatibility with old PineGuard releases.

## Estimate and controls

Engineering-equivalent24–48 hours; active model/tool work8–18 hours; review3–6 hours, low confidence until exact scope. Dispatch separate architecture, composition, end-to-end evidence and documentation packets with [execution protocol](../../references/execution-protocol.md) bounds.

Stop for unjustified framework breadth, duplicated rules, unclear ownership or shared abstractions built for imagined future scenarios. W20C cannot be marked complete from a diagram/build alone.

## Worked code example

This example is documentation, not an implemented PineGuard change. Its classification, dependencies and verification scope are stated below; complete source and reproduction instructions are in [Code examples](../../references/code-examples.md).

```csharp
internal interface IAmountHandler
{
    Response Handle(string raw);
}
internal sealed class AmountHandler : IAmountHandler
{
    public Response Handle(string raw) => Boundary.Invoke(raw);
}
```

Purpose: minimal candidate handler/interface adapter calls boundary; no additional rule runtime hierarchy or DI/layer allocation is implied. Inputs1/0→200/400. Existing PineGuard public adapters still require actual consumer evidence. Status pending.
