<!-- metadata_header
type: plan
id: technical-readiness-to-1.0-w10
version: 1.2
status: planned
last_updated: 2026-09-26
-->
# W10 — Early benchmarks, performance and allocation budgets

> **Mandatory completion gate:** This workstream and every major block inherit the [knowledge, documentation and tooling closure contract](../references/knowledge-and-tooling-closure.md). Save evidence-linked lessons/decisions (or an explicit no-new-lesson outcome), synchronize all relevant BRAIN/Markdown/indexes, build or update required supporting scripts/tools and test/run them (or record reviewed N/A), then complete full Astra review → feedback → fix → recheck. Persist the impact matrix, feedback and owned commit evidence; reopen the unit when relevant context changes. Feature implementation alone cannot close this work.

## Purpose and new priority

Benchmarks begin EARLY, alongside CI trust and naming/semantic inventory, by explicit user direction. This overrides v1.6 §9's later-phase placement. Baseline representative current behavior before the new rule structure, then measure each structural decision. The final budget gate remains dependent on approved semantics and measured evidence.

Rubric performance/resource is4/10 with high confidence in the evidence gap. The handoff's numeric budgets are proposals, not measurements. No concrete benchmark project was verified; D11/D22 requires inventory and approval.

Output: `docs/reports/readiness-performance.md`; benchmark source/config/result paths selected from inventory.

## Tasks

| Task | Action | Output |
|---|---|---|
| 10.1 | Inventory existing performance tooling and identify common/hot/risk-heavy validation usage | Approved early workload list and exact benchmark path/tool map |
| 10.2 | Establish controlled baseline: static rule, Must, Guard, object/integration paths where applicable; valid/invalid and boundary/hostile inputs | Raw results with runtime/OS/CPU/configuration, warmup/statistics and allocations |
| 10.3 | Compare proposed W03 rule structure and W02 manifest runtime consequences before broad rollout | Decision evidence for allocations, boxing, closures, construction/reuse and code-size tradeoffs |
| 10.4 | Set per-workload budgets and regression policy from baseline and product intent | Astra D11 record: units, thresholds, tolerance, environment and exception process |
| 10.5 | Add reliable regression checks and separate noisy scheduled benchmarking from deterministic checks where warranted | W00 routing/evidence integration with deliberate regression proof |
| 10.6 | Rerun affected workloads after regex/resources/diagnostics/lifecycle changes and publish scoped claims | Final raw evidence, budget disposition and support limitations |

## Required distinctions

Measure cold construction versus steady-state execution, success versus failure, one rule versus composition, small versus representative large inputs, and callback/integration cost where applicable. Include per-operation allocations and relevant tail/worst-case behavior, not only mean throughput. Measure string conversions/date parsing and mutable validator construction/reuse if they matter.

Do not claim zero allocations from static syntax alone, compare dissimilar semantics as equivalent workloads, or reuse publisher competitor benchmark ratios as PineGuard evidence. Comparative benchmarks require the genuine Sol analysis and independently controlled methodology.

## Acceptance

The early baseline is reproducible enough to inform W03 before broad implementation. Final claimed workloads meet approved quantitative budgets with variance and environment disclosed. A seeded performance/allocation regression is detected under the actual gate design. Flaky statistical checks cannot be silently waived; revise method or remove the unsupported claim through Astra review.

## Controls

Checkpoint after workload selection, baseline collection and budget decision. Do not wait for every hostile-testing workstream before obtaining the initial baseline. Conversely, an early benchmark is not final release evidence after structural changes. Required correctness and100% validation coverage must remain satisfied; performance changes may not hide failures or create local special cases.

Follow the [execution protocol](../references/execution-protocol.md), [decision register](../references/decision-register.md), [quality constitution](../references/quality-constitution.md) and [estimates and waves](../references/estimates-and-waves.md). This child remains Planned.

## Worked code example

This example is documentation, not an implemented PineGuard change. Its classification, dependencies and verification scope are stated below; complete source and reproduction instructions are in [Code examples](../references/code-examples.md).

```csharp
const int iterations = 100000;
var timer = new Stopwatch();
var warmupConsumed = 0;
for (var i = 0; i < 1000; i++)
    warmupConsumed += PositiveInt.Evaluate(1).Passed ? 1 : 0;
Checks.Require(warmupConsumed == 1000, "warmup result consumed");
var consumed = 0;
timer.Start();
var before = GC.GetAllocatedBytesForCurrentThread();
for (var i = 0; i < iterations; i++)
    consumed += PositiveInt.Evaluate(1).Passed ? 1 : 0;
var allocated = GC.GetAllocatedBytesForCurrentThread() - before;
timer.Stop();
Checks.Require(consumed == iterations, "timing result consumed");
Console.WriteLine($"TIMING illustration: consumed={consumed}; elapsedTicks={timer.ElapsedTicks}; allocatedBytes={allocated}");
```

Purpose: complete package-free timing probe consumes 100,000 static results after 1,000 consumed warmup calls. Stopwatch construction and warmup are outside the allocation-measurement window; no validator construction/reuse claim. Expected consumed=100000, elapsed/allocation observed, no threshold selected. Timer/counter overhead, JIT tiering and host noise remain uncontrolled; this is a smoke measurement, not BenchmarkDotNet, a regression budget or a publishable benchmark. BDN version/config remain unverified; D10/D11 open. Status pending.
