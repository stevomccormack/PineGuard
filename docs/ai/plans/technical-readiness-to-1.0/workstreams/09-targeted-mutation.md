<!-- metadata_header
type: plan
id: technical-readiness-to-1.0-w09
version: 1.4
status: planned
last_updated: 2026-11-09
-->
# W09 — Targeted mutation and assertion strength

> **Status 2026-11-09:** not activated. The plan-gated council ran on 2026-11-09 and ruled that this plan is not activated as written; the only unit that may start, on the owner's explicit go, is the string→DateOnly pilot PR described in [plan.md § Where this plan stands](../plan.md#where-this-plan-stands-2026-11-09) and register decision D29. The completion gate, Astra/Luna/Sol roles, checkpoints and D22 inventory steps below apply to a later wave activation, not to the pilot PR.

> **Mandatory completion gate:** This workstream and every major block inherit the [knowledge, documentation and tooling closure contract](../references/knowledge-and-tooling-closure.md). Save evidence-linked lessons/decisions (or an explicit no-new-lesson outcome), synchronize all relevant BRAIN/Markdown/indexes, build or update required supporting scripts/tools and test/run them (or record reviewed N/A), then complete full Astra review → feedback → fix → recheck. Persist the impact matrix, feedback and owned commit evidence; reopen the unit when relevant context changes. Feature implementation alone cannot close this work.

## Purpose

W09 demonstrates that important assertions detect realistic defects. v1.6 §8 supports targeted mutation. The substantial passing execution count and100% coverage requirement do not themselves prove assertions are strong; this workstream complements both.

Inputs: W01 contracts, W05 conformance, W06–W08 tests and W14 risk priorities. No dedicated mutation configuration was established. D10/D22 requires tool/target/file approval. Output: `+ docs/reports/readiness-mutation.md`.

## Tasks

| Task | Action | Output |
|---|---|---|
| 09.1 | Inventory existing tools and identify risk-ranked semantic sites | Exact target methods/files, supported frameworks and runner options |
| 09.2 | Select fault models tied to requirements | Mutation catalog: boundary flip, negation, missing check, wrong code, normalization/order changes where applicable |
| 09.3 | Approve bounded operators, exclusions and run scheduling | Astra-reviewed cost/coverage policy; no vanity score target |
| 09.4 | Execute pilot mutations and inspect survivors/timeouts | Reproducible mutant outcomes and tested-assertion mapping |
| 09.5 | Classify survivors as meaningful gaps, equivalent, unreachable under approved domain or tool defects | Explicit rationale with no blanket ignore category |
| 09.6 | Strengthen independent assertions and rerun affected mutants plus validation coverage | Closed meaningful survivors, clean source restoration and W00 artifacts |

## Acceptance

Every targeted contractual fault has a demonstrably failing assertion. Meaningful surviving mutants are resolved or block the affected10/10 criterion. Equivalent/unreachable classifications are individually justified and reviewed; source is not changed solely to game the mutation tool.

Record generated, executed, killed, survived, timed-out and excluded counts with denominator and tool settings. A timeout does not automatically count as a meaningful kill. Coverage thresholds remain100% for validation scope; mutation is a separate evidence dimension, never an excuse to lower coverage.

## Controls

Checkpoint after risk/target selection, after first results and before assertion changes. Use approved isolated mutation facilities so temporary production mutations never enter committed code. Sol owns harness/test changes, Luna supplies context/evidence, Astra reviews survivors. Stop for broad expensive campaigns without value, unexplained runner errors or exclusions hiding gaps. Do not impose an arbitrary universal mutation percentage before the target domain and meaningful fault set are approved.

Follow the [execution protocol](../references/execution-protocol.md), [decision register](../references/decision-register.md), [quality constitution](../references/quality-constitution.md) and [estimates and waves](../references/estimates-and-waves.md). This child remains Planned.

## Approved test-project boundary

The [approved testing project layout](../references/test-project-layout.md) is authoritative for this workstream's project identity, folder responsibility and execution kind. Use its workstream mapping; do not create one project per testing technique or move this crosscutting work into existing library unit suites. Reuse PineGuard.Testing fixtures and typed projections. Required projects and artifacts must exist before the applicable implementation gate can close; Planned status is not execution evidence.

### Mechanical illustration

This package-free code is a mechanical teaching example only. It is not repository test guidance, a domain test, or evidence that the proposed technique is implemented. Complete source and reproduction scope are in [Code examples](../references/code-examples.md).

This example is documentation, not an implemented PineGuard change. Its classification, dependencies and verification scope are stated below; complete source and reproduction instructions are in [Code examples](../references/code-examples.md).

```csharp
static bool SeededBoundaryMutant(int? value) => value is not null && value.Value >= 0;
Checks.Require(SeededBoundaryMutant(0), "mutant accepts zero");
Checks.Require(!PositiveInt.Evaluate(0).Passed, "explicit zero expectation kills mutant");
```

Purpose: explicit zero expectation kills seeded `>=0` boundary mutant; zero must fail canonical and pass mutant. This does not measure a selected mutation engine or score. Tool selection remains D10. Status: mechanical illustration only; not repository-test evidence.

### Repository test excerpt

This documentation excerpt shows the canonical fixture-to-typed-case-to-TheoryData-to-MemberData-to-assertion chain. It does not implement PineGuard behavior; the full source-inspected patterns and scope limits are in [Code examples](../references/code-examples.md).

W09 source-grounded fixture assertions distinguish one deliberately seeded boundary fault. The local mutant is intentionally wrong and never a second executable authority; no mutation-engine selection/score is claimed:

```csharp
[Theory]
[MemberData(nameof(NumberRulesTestData.IsPositiveInt.Cases), MemberType = typeof(NumberRulesTestData.IsPositiveInt))]
public void IsPositive_BehavesAsExpected(RuleCase<int?> tc)
{
    // Arrange: canonical Zero independently expects invalid.
    static bool SeededBoundaryMutant(int? input) => input is not null && input.Value >= 0;
    // Act.
    var result = NumberRules.IsPositive(tc.Value);
    // Assert through the existing fixture-backed helper.
    AssertResult(tc, result);
    if (tc.Value == 0)
    {
        Assert.False(result);
        Assert.True(SeededBoundaryMutant(tc.Value));
    }
}
```

Scenario acceptance: Zero0 fails canonical and passes the mutant; substituting that mutant into the actual evaluator would fail the existing zero case. This is one distinguishable seed, not an automated campaign.
