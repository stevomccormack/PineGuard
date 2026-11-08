<!-- metadata_header
type: plan
id: technical-readiness-to-1.0-w05
version: 1.3
status: planned
last_updated: 2026-09-26
-->
# W05 — Cross-surface conformance and parity

> **Mandatory completion gate:** This workstream and every major block inherit the [knowledge, documentation and tooling closure contract](../references/knowledge-and-tooling-closure.md). Save evidence-linked lessons/decisions (or an explicit no-new-lesson outcome), synchronize all relevant BRAIN/Markdown/indexes, build or update required supporting scripts/tools and test/run them (or record reviewed N/A), then complete full Astra review → feedback → fix → recheck. Persist the impact matrix, feedback and owned commit evidence; reopen the unit when relevant context changes. Feature implementation alone cannot close this work.

## Purpose

P2 makes semantic agreement observable without using the implementation as its own oracle. v1.6 §7 proposes shared RuleScenario<TInputs> with independently authored expected outcomes. The rule manifest provides coverage/provenance; it does not generate expected truth by calling Must.

Inputs: W01 pilot specifications, W02 inventory and W04 mapping decisions; existing shared fixture conventions identified by Luna; `tests/PineGuard.Testing/Fixtures/OwaspRulesFixtures.cs` is a verified fixture anchor, not an assumed universal harness.

## Preconditions and outputs

D07/D08 must be resolved for the pilot. Luna inventories exact fixture projects, adapters and test paths, then Astra approves D22's path map. Output: `+ docs/reports/readiness-conformance.md`, approved scenario/adapter fixtures and machine-readable evidence through W00's artifact contract.

## Tasks

| Task | Action | Output |
|---|---|---|
| 05.1 | Select independent pilot cases from W01 and label valid, invalid, conversion/configuration/operational outcomes | Reviewed case catalog with rationale and oracle provenance |
| 05.2 | Design RuleScenario inputs and expected semantics without coupling to a concrete surface return type | Astra-approved fixture contract under test specifications |
| 05.3 | Implement adapters for actual applicable Must/Guard/object/integration entry points | Explicit input adaptation and result normalization mappings |
| 05.4 | Compare actual results to independent expectations and each other only inside approved equivalence domains | Per-rule/surface matrix with meaningful unsupported/not-applicable entries |
| 05.5 | Seed predicate, code, path/ordering and returned-value drift where those fields are contractual | Proof each relevant detector fails, followed by clean seed removal |
| 05.6 | Connect completeness to W02 and gate activation to W00 | Audit references and reviewed rollout by rule family |

## Required scenarios

Include valid/invalid boundaries, null/empty where applicable, conversion success/failure and the approved environment. For each rule establish whether negation is a logical complement over the entire domain or only valid evaluation results. Do not assert complement identities over timeouts or unsupported/configuration states without D05.

Compare codes only through documented provenance. Compare messages/paths/order only to the stability actually promised. Do not make DataAnnotations satisfy a nonexistent universal-code contract. Do not interpret a Guard exception shape as equivalent to an object validator result without an explicit adapter policy.

## Acceptance and control

Every pilot case runs through every applicable approved projection; gaps have a reviewed reason. No expected outcome calls Must, delegates to the same predicate under test or derives directly from its branch logic. A seeded semantic error must fail even if all wrappers share the same faulty implementation.

The pilot checkpoint proves only the pilot. Final W05 completion requires reviewed family-by-family rollout across every declared validation rule and applicable surface, reconciled with the complete W01/W02 inventory and the hard coverage contract. Each rollout packet has independent expectations and explicit supported/not-applicable mappings. Checkpoint after oracle review, after the pilot, after each family and before global gate activation. Stop on an unresolved mismatch rather than modifying expected results to match production output.

Follow the [execution protocol](../references/execution-protocol.md), [decision register](../references/decision-register.md) and [source map](../references/source-map.md). This child remains Planned; no implementation is authorized by this document.

## Approved test-project boundary

The [approved testing project layout](../references/test-project-layout.md) is authoritative for this workstream's project identity, folder responsibility and execution kind. Use its workstream mapping; do not create one project per testing technique or move this crosscutting work into existing library unit suites. Reuse PineGuard.Testing fixtures and typed projections. Required projects and artifacts must exist before the applicable implementation gate can close; Planned status is not execution evidence.

### Mechanical illustration

This package-free code is a mechanical teaching example only. It is not repository test guidance, a domain test, or evidence that the proposed technique is implemented. Complete source and reproduction scope are in [Code examples](../references/code-examples.md).

This example is documentation, not an implemented PineGuard change. Its classification, dependencies and verification scope are stated below; complete source and reproduction instructions are in [Code examples](../references/code-examples.md).

```csharp
Checks.Require(Projections.Must(1).Passed, "Must positive");
Checks.Require(!Projections.Must(0).Passed, "Must zero");
Projections.Guard(1);
var threw = false;
try { Projections.Guard(0); }
catch (ArgumentOutOfRangeException) { threw = true; }
Checks.Require(threw, "Guard zero throws");
Checks.Require(Projections.ObjectErrors(0).SequenceEqual(
    new[] { "Value:illustration.number.not-positive" }), "object path/code");
```

Purpose: independent literal expectations for candidate projections. Add actual adapter cases in their existing paired Tests/TestData files, including null/indexed children/sync-async semantics; do not assume identical framework null handling. Same 1/0 expectations as W04; candidate Status: mechanical illustration only; not repository-test evidence., actual frameworks unverified.

### Repository test excerpt

This documentation excerpt shows the canonical fixture-to-typed-case-to-TheoryData-to-MemberData-to-assertion chain. It does not implement PineGuard behavior; the full source-inspected patterns and scope limits are in [Code examples](../references/code-examples.md).

#### W04/W05: FluentValidation pipeline with its actual null contract

Exact FluentNumberExtensionsTestData.Positive group:

```csharp
public static class Positive
{
    public static TheoryData<FluentCase<int?>> Cases => F.IsPositive.AllScenarios.ToFluentCases(s => s.Name switch
    {
        nameof(F.IsPositive.Null) => new FluentExpected(true),
        _ when s.IsValid => new FluentExpected(true),
        _ => new FluentExpected(false, "Value must be positive.")
    });
}
```

Existing validator and test within FluentNumberExtensionsTests; do not duplicate them:

```csharp
private sealed class PositiveValidator : AbstractValidator<IntModel>
{
    public PositiveValidator() => RuleFor(x => x.Value).Positive();
}
[Theory]
[MemberData(nameof(FluentNumberExtensionsTestData.Positive.Cases), MemberType = typeof(FluentNumberExtensionsTestData.Positive))]
public void Positive_BehavesAsExpected(FluentCase<int?> tc)
{
    // Arrange.
    var validator = new PositiveValidator();
    var model = new IntModel { Value = tc.Value };
    // Act.
    var result = validator.Validate(model);
    // Assert.
    AssertResult(tc, result);
}
```

Positive1 and Null→valid under this framework contract; Zero0/Negative-1→failure message Value must be positive. Null acceptance is independently explicit in FluentExpected and differs from nullable Core; no blanket adapter-parity claim is made.

#### W04/W05: normative DataAnnotations typed pipeline

Use the actual conformant existing StringBoolAttributesTestData.TrueString example to show the normative DataAnnotations single-Cases shape, rather than presenting legacy NumberAttributes split streams as the standard. Context: its existing F.BoolIsTrue alias and implemented DataAnnotationCase/DataAnnotationExpected/scenario extensions:

```csharp
public static class TrueString
{
    public static TheoryData<DataAnnotationCase> Cases => F.BoolIsTrue.AllScenarios.ToDataAnnotationCases(s => s.Name switch
    {
        nameof(F.BoolIsTrue.NullValue) => new DataAnnotationExpected(true),
        _ when s.IsValid => new DataAnnotationExpected(true),
        _ => new DataAnnotationExpected(false, "Value must be true.", Code: MustCodes.Boolean.Value.False)
    });
}
```

Existing StringBoolAttributesTests derives from BaseDataAnnotationUnitTest. Complete method view with proposed AAA comments:

```csharp
[Theory]
[MemberData(nameof(StringBoolAttributesTestData.TrueString.Cases), MemberType = typeof(StringBoolAttributesTestData.TrueString))]
public void TrueString_BehavesAsExpected(DataAnnotationCase tc)
{
    // Arrange.
    var attr = new TrueStringAttribute();
    var ctx = new ValidationContext(new object()) { MemberName = "Value" };
    // Act.
    var result = attr.GetValidationResult(tc.Value, ctx);
    // Assert.
    AssertResult(tc, result, attr.Code);
}
```

NullValue and independently valid true scenarios→ValidationResult.Success; invalid scenarios→message Value must be true and Boolean.Value.False code. PositiveNumber consumer invocation remains a distinct API ergonomics example, not a fabricated typed pipeline or evidence of numeric DA null handling.
