<!-- metadata_header
type: plan
id: technical-readiness-to-1.0-w01
version: 1.3
status: planned
last_updated: 2026-09-26
-->
# W01 — Semantic specification and independent truth

> **Mandatory completion gate:** This workstream and every major block inherit the [knowledge, documentation and tooling closure contract](../references/knowledge-and-tooling-closure.md). Save evidence-linked lessons/decisions (or an explicit no-new-lesson outcome), synchronize all relevant BRAIN/Markdown/indexes, build or update required supporting scripts/tools and test/run them (or record reviewed N/A), then complete full Astra review → feedback → fix → recheck. Persist the impact matrix, feedback and owned commit evidence; reopen the unit when relevant context changes. Feature implementation alone cannot close this work.

## Purpose and current evidence

P0–P1 establishes reviewed truth before adapting implementation. Use v1.6 §§3,5.1,6 and the rubric's semantic/failure row. The initial pilot is Email, bounded numeric validation and string-to-DateOnly past-date behavior. The handoff's specification-first direction is useful; its unverified API examples and broad redesign are not requirements.

Verified inputs: `src/PineGuard.Core/Rules/OwaspRules.cs`, `src/PineGuard.MustClauses/MustOwaspClauses.cs`, `tests/PineGuard.Testing/Fixtures/OwaspRulesFixtures.cs`, `src/PineGuard.Core/Rules/StringRules.cs`, `src/PineGuard.MustClauses/MustStringClauses.cs`, `src/PineGuard.Core/Codes/MustCodes.Email.cs`.

## Preconditions and unresolved choices

D04/D05 require runtime reproductions before a bug classification. D16 governs culture/time choices. Luna must identify exact pilot overloads, fixtures and return/failure shapes. Astra resolves null/empty/whitespace, normalization, conversion, bound inclusivity, date source/clock, exceptions and returned-value semantics. Current behavior is evidence only: the greenfield contract may deliberately choose better semantics without backward-compatibility constraints.

## Ordered tasks

| Task | Action | Output |
|---|---|---|
| 01.1 | Extract current public contracts, relevant source, fixtures and docs for each pilot | `docs/reports/readiness-semantic-contracts.md`: source-backed behavior inventory |
| 01.2 | Reproduce trimming/newline and regex timeout/negation concerns with minimal inputs across the specifically implicated paths | Reproduction command, input, actual result, environment and source-only/reproduced status |
| 01.3 | Write decision packets for each ambiguity; compare the simplest coherent alternatives and repository-wide consequences | D04/D05/D16 records with explicit selected answers before edits |
| 01.4 | Specify input domain, valid/invalid conditions, normalization/conversion, output, error classification and environmental dependencies for each pilot | Reviewed normative pilot specification at a path approved under governing spec conventions |
| 01.5 | Author expected-case tables without invoking Must or copying implementation branches | Independent expected outcomes with rationale and boundary representatives |
| 01.6 | Reconcile approved semantics with every existing applicable surface through W04/W05 | Contract-to-surface traceability and globally coherent change requests |
| 01.7 | After pilot review, specify every declared rule family in bounded packets using the approved taxonomy and independent oracle method | Complete semantic contract inventory reconciled with W02; no unspecified declared rule at final acceptance |

## Required case families

Email includes null/empty/whitespace and explicitly supported normalization/syntax boundaries. Bounded numeric includes endpoints, immediately adjacent values, invalid range configuration and type-specific exceptional numeric values only where the selected type supports them. String-to-DateOnly includes parse success/failure, agreed format/culture, past/today/future relative to the chosen clock and conversion output. These are review prompts, not invented semantics or commitments to every possible overload.

The OWASP investigation must distinguish individual rules, composite OwaspSafe behavior, normalization and the value returned by Must. Do not generalize the source observation into an exploit or a claim about all composite behavior.

## Acceptance and controls

C1 requires every pilot outcome to be explainable from the specification without reading implementation. Each ambiguity has an Astra decision or blocks the dependent case. Expected outcomes are independently reviewed; implementations cannot be used to manufacture their own oracle. W12 reviews consistency with the newly chosen public contract, not old releases.

Checkpoint after reproductions and after each rule's reviewed truth table. Sol changes implementation only after formal activation/authorization and contract approval. No rule expansion or wholesale rewrites occur in this workstream.

Follow the [execution protocol](../references/execution-protocol.md), [decision register](../references/decision-register.md) and [source map](../references/source-map.md). This child remains Planned; no implementation is authorized by this document.

## Approved test-project boundary

The [approved testing project layout](../references/test-project-layout.md) is authoritative for this workstream's project identity, folder responsibility and execution kind. Use its workstream mapping; do not create one project per testing technique or move this crosscutting work into existing library unit suites. Reuse PineGuard.Testing fixtures and typed projections. Required projects and artifacts must exist before the applicable implementation gate can close; Planned status is not execution evidence.

### Mechanical illustration

This package-free code is a mechanical teaching example only. It is not repository test guidance, a domain test, or evidence that the proposed technique is implemented. Complete source and reproduction scope are in [Code examples](../references/code-examples.md).

This example is documentation, not an implemented PineGuard change. Its classification, dependencies and verification scope are stated below; complete source and reproduction instructions are in [Code examples](../references/code-examples.md).

```csharp
(int? Input, bool Expected)[] cases =
    [(null, false), (int.MinValue, false), (-1, false),
     (0, false), (1, true), (int.MaxValue, true)];
foreach (var row in cases)
{
    var result = PositiveInt.Evaluate(row.Input);
    Checks.Require(result.Passed == row.Expected, "explicit semantic row");
    Checks.Require(result.Code == (row.Expected ? null :
        "illustration.number.not-positive"), "explicit code");
}
```

Purpose: literal null/boundary/valid truth rows, independent stable codes. Exact inputs/outputs are in the canonical contract and method; current paired TheoryData/MemberData insertion above supplies the actual Must example. Status: mechanical illustration only; not repository-test evidence., current tests not run.

### Repository test excerpt

This documentation excerpt shows the canonical fixture-to-typed-case-to-TheoryData-to-MemberData-to-assertion chain. It does not implement PineGuard behavior; the full source-inspected patterns and scope limits are in [Code examples](../references/code-examples.md).

#### W01 and canonical semantics: nullable Core fixture and paired test

Exact current NumberRulesFixtures.IsPositive fixture excerpt; Null stays visible:

```csharp
public static class IsPositive
{
    public static readonly int? Positive = 1;
    public static readonly int? Zero = 0;
    public static readonly int? Negative = -1;
    public static readonly int? Null = null;
    public static RuleScenario<int?>[] ValidScenarios => [new(nameof(Positive), Positive, true)];
    public static RuleScenario<int?>[] InvalidScenarios =>
    [new(nameof(Zero), Zero, false), new(nameof(Negative), Negative, false), new(nameof(Null), Null, false)];
    public static RuleScenario<int?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
}
```

Exact existing NumberRulesTestData.IsPositiveInt projection in its paired file:

```csharp
public static class IsPositiveInt
{
    public static TheoryData<RuleCase<int?>> Cases => F.IsPositive.AllScenarios.ToRuleCases();
}
```

Existing NumberRulesTests method with proposed AAA comments; preserve its base AssertResult:

```csharp
[Theory]
[MemberData(nameof(NumberRulesTestData.IsPositiveInt.Cases), MemberType = typeof(NumberRulesTestData.IsPositiveInt))]
public void IsPositive_BehavesAsExpected(RuleCase<int?> tc)
{
    // Arrange: tc comes from the independently authored shared fixture.
    // Act.
    var result = NumberRules.IsPositive(tc.Value);
    // Assert.
    AssertResult(tc, result);
}
```

Inputs Positive1→true; Zero0/Negative-1/Null→false. Must's exact Positive group/test from manuscript.md remains a separate nonnullable MustCase<int> projection; it deliberately excludes Null because Must.Positive<T> takes T, not T?. This Core excerpt supplies actual nullable behavior evidence. Extend the canonical numeric fixture with int.MinValue, int.MaxValue and the reviewed boundary/edge scenarios, preserving independent expectations and layer-specific typed projections; do not reintroduce local hardcoded TestData tables.
