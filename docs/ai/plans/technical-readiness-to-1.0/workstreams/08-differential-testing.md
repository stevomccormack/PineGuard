<!-- metadata_header
type: plan
id: technical-readiness-to-1.0-w08
version: 1.4
status: planned
last_updated: 2026-11-09
-->
# W08 — Differential testing with explicit equivalence domains

> **Status 2026-11-09:** planned; owner authorization is pending. Only the string→DateOnly pilot may be authorized under [the current plan disposition](../plan.md#where-this-plan-stands-2026-11-09) (register D29). The wave procedures and role assignments below are historical proposals, subject to redrafting from pilot evidence and owner approval.

> **Mandatory completion gate:** This workstream and every major block inherit the [knowledge, documentation and tooling closure contract](../references/knowledge-and-tooling-closure.md). Save evidence-linked lessons/decisions (or an explicit no-new-lesson outcome), synchronize all relevant BRAIN/Markdown/indexes, build or update required supporting scripts/tools and test/run them (or record reviewed N/A), then complete full Astra review → feedback → fix → recheck. Persist the impact matrix, feedback and owned commit evidence; reopen the unit when relevant context changes. Feature implementation alone cannot close this work.

## Purpose and scope

W08 supplies independent comparative correctness evidence. It is not an old-release compatibility gate. Compare a selected implementation with an independently justified reference only where their semantic domains are equivalent. A competitor's behavior is evidence, not PineGuard's definition of truth.

Inputs: W01 new contracts, W05 scenarios, official competitor dossiers and genuine Sol comparative synthesis where relevant. v1.6 §8 supports risk-focused differential testing. No exact harness was verified; D10/D22 requires inventory/tool/path approval. Output: `+ docs/reports/readiness-differential.md`.

## Tasks

| Task | Action | Output |
|---|---|---|
| 08.1 | Identify high-risk rules with independent reference models, platform parsers or relevant established validators | Candidate reference matrix, independence and licensing/dependency review |
| 08.2 | Define domains where semantics genuinely match, including null, culture, syntax and normalization | Astra-approved equivalence specification and explicit exclusions |
| 08.3 | Design adapter normalization without erasing meaningful discrepancies | Exact harness/path map and expected result categories |
| 08.4 | Run curated boundaries and bounded generated inputs against both implementations | Replayable input/result/environment artifacts |
| 08.5 | Triage every mismatch against W01, never by majority vote | Defect/reference limitation/specification question/expected divergence decision |
| 08.6 | Retain minimized confirmed regressions and seed a detector failure | Demonstrated sensitivity, CI cost/routing and artifact requirements |

## Acceptance

The reference is independent of PineGuard's implementation and shares only the explicitly documented contract domain. Results distinguish reference agreement from specification correctness. Unsupported reference features and known semantic divergence remain visible; they are not deleted merely to improve agreement statistics.

A seeded discrepancy fails. A known justified divergence is recorded without becoming an unexplained permanent suppression. Package versions, environment, source/license and input provenance are captured. Mismatch fixes address shared root causes across the repository and rerun every affected surface.

## Controls

Checkpoint after reference/domain review and after mismatch triage. Luna does all research/reading; Sol implements and runs the approved harness; Astra decides disputed semantics. Stop for unlicensed copied reference code, shared-implementation oracles, unsupported equivalence assumptions or uncontrolled dependency expansion. W08 does not create an obligation to copy competitor behavior or maintain historical PineGuard APIs.

Follow the [execution protocol](../references/execution-protocol.md), [decision register](../references/decision-register.md), [quality constitution](../references/quality-constitution.md) and [estimates and waves](../references/estimates-and-waves.md). This child remains Planned.

## Approved test-project boundary

The [approved testing project layout](../references/test-project-layout.md) is authoritative for this workstream's project identity, folder responsibility and execution kind. Use its workstream mapping; do not create one project per testing technique or move this crosscutting work into existing library unit suites. Reuse PineGuard.Testing fixtures and typed projections. Required projects and artifacts must exist before the applicable implementation gate can close; Planned status is not execution evidence.

### Mechanical illustration

This package-free code is a mechanical teaching example only. It is not repository test guidance, a domain test, or evidence that the proposed technique is implemented. Complete source and reproduction scope are in [Code examples](../references/code-examples.md).

This example is documentation, not an implemented PineGuard change. Its classification, dependencies and verification scope are stated below; complete source and reproduction instructions are in [Code examples](../references/code-examples.md).

```csharp
var random = new Random(731);
for (var i = 0; i < 1000; i++)
{
    var input = random.Next(-10000, 10001);
    var expected = Math.Sign(input) == 1;
    Checks.Require(PositiveInt.Evaluate(input).Passed == expected, "independent sign oracle");
}
```

Purpose: BCL Math.Sign is an independent reference over the identical bounded int domain. Expected positive sign only; nullable boundary expectations live in literal W01 rows. Do not compare email syntax or dates against a reference with different semantics. Status: mechanical illustration only; not repository-test evidence.

### Repository test excerpt

This documentation excerpt shows the canonical fixture-to-typed-case-to-TheoryData-to-MemberData-to-assertion chain. It does not implement PineGuard behavior; the full source-inspected patterns and scope limits are in [Code examples](../references/code-examples.md).

W08 differential assertion over the identical nullable-int domain, using the independent BCL sign model:

```csharp
[Theory]
[MemberData(nameof(NumberRulesTestData.IsPositiveInt.Cases), MemberType = typeof(NumberRulesTestData.IsPositiveInt))]
public void IsPositive_BehavesAsExpected(RuleCase<int?> tc)
{
    // Arrange: reference semantics cover exactly int? and treat null as invalid.
    var reference = tc.Value is int value && Math.Sign(value) == 1;
    // Act.
    var result = NumberRules.IsPositive(tc.Value);
    // Assert independent fixture truth and independent reference agreement.
    AssertResult(tc, result);
    Assert.Equal(reference, result);
}
```

Scenario acceptance: Positive1→true; Zero/Negative/Null→false; canonical `int.MinValue`/`int.MaxValue` edge extensions→false/true. No email/date/reference-domain equivalence is inferred.
