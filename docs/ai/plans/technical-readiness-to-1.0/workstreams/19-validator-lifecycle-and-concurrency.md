<!-- metadata_header
type: plan
id: technical-readiness-to-1.0-w19
version: 1.4
status: planned
last_updated: 2026-11-09
-->
# W19 — Validator lifecycle and concurrency

> **Status 2026-11-09:** not activated. The plan-gated council ran on 2026-11-09 and ruled that this plan is not activated as written; the only unit that may start, on the owner's explicit go, is the string→DateOnly pilot PR described in [plan.md § Where this plan stands](../plan.md#where-this-plan-stands-2026-11-09) and register decision D29. The completion gate, Astra/Luna/Sol roles, checkpoints and D22 inventory steps below apply to a later wave activation, not to the pilot PR.

> **Mandatory completion gate:** This workstream and every major block inherit the [knowledge, documentation and tooling closure contract](../references/knowledge-and-tooling-closure.md). Save evidence-linked lessons/decisions (or an explicit no-new-lesson outcome), synchronize all relevant BRAIN/Markdown/indexes, build or update required supporting scripts/tools and test/run them (or record reviewed N/A), then complete full Astra review → feedback → fix → recheck. Persist the impact matrix, feedback and owned commit evidence; reopen the unit when relevant context changes. Feature implementation alone cannot close this work.

## Purpose and baseline

Characterize lifecycle EARLY because it affects new structure, construction costs and public contracts; complete concurrency proof after decisions. Source inspection identified mutable validator structures, not a reproduced race. Never state they are thread-safe or racy without evidence.

Verified inputs: `src/PineGuard.Core/MustClauses/MustValidator.cs`, `InlineMustValidator.cs`, `MustPropertyRule.cs`, and `tests/PineGuard.Core.UnitTests/MustClauses/MustValidatorTests.cs`. Output: `+ docs/reports/readiness-lifecycle-concurrency.md`.

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

## Approved test-project boundary

The [approved testing project layout](../references/test-project-layout.md) is authoritative for this workstream's project identity, folder responsibility and execution kind. Use its workstream mapping; do not create one project per testing technique or move this crosscutting work into existing library unit suites. Reuse PineGuard.Testing fixtures and typed projections. Required projects and artifacts must exist before the applicable implementation gate can close; Planned status is not execution evidence.

### Mechanical illustration

This package-free code is a mechanical teaching example only. It is not repository test guidance, a domain test, or evidence that the proposed technique is implemented. Complete source and reproduction scope are in [Code examples](../references/code-examples.md).

This example is documentation, not an implemented PineGuard change. Its classification, dependencies and verification scope are stated below; complete source and reproduction instructions are in [Code examples](../references/code-examples.md).

```csharp
var successCount = 0;
Parallel.For(0, 1000, _ =>
{
    if (PositiveInt.Evaluate(1).Passed) Interlocked.Increment(ref successCount);
});
Checks.Require(successCount == 1000, "concurrent stateless calls");
```

Purpose: 1,000 concurrent calls of this stateless miniature→1,000 successes. This proves neither mutable current MustValidator sharing nor a selected validator reuse contract; D17 remains blocked. Framework lifecycle needs actual chosen-contract tests. Status: mechanical illustration only; not repository-test evidence.

### Repository test excerpt

This documentation excerpt shows the canonical fixture-to-typed-case-to-TheoryData-to-MemberData-to-assertion chain. It does not implement PineGuard behavior; the full source-inspected patterns and scope limits are in [Code examples](../references/code-examples.md).

#### Group 5: W14/W15/W17/W18/W19 operational test guidance

Use the complete approved typed pipeline below as the structural repository-test reference, preserving the current operation name/type/MemberData/AssertResult. This numeric structural example does not verify security, regex, privacy, date determinism or concurrency. Those workstreams retain concrete relevant implementation illustrations separately, plus the acceptance rows below. Unselected operational expectation contracts must be approved before representing specialized production tests as ready to integrate; do not invent replacement case types or rebrand smoke loops as conformant tests.

```csharp
public static class IsPositiveInt
{
    public static TheoryData<RuleCase<int?>> Cases => F.IsPositive.AllScenarios.ToRuleCases();
}
```

```csharp
[Theory]
[MemberData(nameof(NumberRulesTestData.IsPositiveInt.Cases), MemberType = typeof(NumberRulesTestData.IsPositiveInt))]
public void IsPositive_BehavesAsExpected(RuleCase<int?> tc)
{
    // Arrange: independently authored canonical fixture and typed case.
    // Act.
    var result = NumberRules.IsPositive(tc.Value);
    // Assert.
    AssertResult(tc, result);
}
```

The structural numeric example does not verify this workstream's operational behavior. The named acceptance rows below are future fixture inputs, and the separate mechanical illustration is not domain-test evidence.

| W19 concurrency | SharedStatelessPositive1000Calls→1000successes; bounded worker count recorded. Current mutable MustValidator lifecycle remains outside this claim until D17 chosen. | Program.cs ConcurrentReuse loop, explicitly mechanical static-function evidence only. |
