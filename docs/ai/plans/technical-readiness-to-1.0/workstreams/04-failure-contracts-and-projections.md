<!-- metadata_header
type: plan
id: technical-readiness-to-1.0-w04
version: 1.4
status: planned
last_updated: 2026-11-09
-->
# W04 — New failure contract and projections

> **Status 2026-11-09:** not activated. The plan-gated council ran on 2026-11-09 and ruled that this plan is not activated as written; the only unit that may start, on the owner's explicit go, is the string→DateOnly pilot PR described in [plan.md § Where this plan stands](../plan.md#where-this-plan-stands-2026-11-09) and register decision D29. The completion gate, Astra/Luna/Sol roles, checkpoints and D22 inventory steps below apply to a later wave activation, not to the pilot PR.

> **Mandatory completion gate:** This workstream and every major block inherit the [knowledge, documentation and tooling closure contract](../references/knowledge-and-tooling-closure.md). Save evidence-linked lessons/decisions (or an explicit no-new-lesson outcome), synchronize all relevant BRAIN/Markdown/indexes, build or update required supporting scripts/tools and test/run them (or record reviewed N/A), then complete full Astra review → feedback → fix → recheck. Persist the impact matrix, feedback and owned commit evidence; reopen the unit when relevant context changes. Feature implementation alone cannot close this work.

## Purpose

Choose the best globally consistent new failure model EARLY with W03/W02. Greenfield means old shape compatibility is not required. Inventory current evidence to avoid false assumptions, then decide a coherent model, privacy policy and projection boundaries.

The handoff's code/path/message/severity protocol is a candidate, not a complete approved design. Verified current facts: DataAnnotations has no universal code and Guard NotEmail maps Must Email; do not infer new codes from surface names.

Inputs: W01 semantics, W22 terminology, W02 inventory, src/PineGuard.Core/Codes/MustCodes.Email.cs; Luna locates exact result/adapter/serialization paths. Output: `+ docs/reports/readiness-failure-contracts.md`. D08/D15/D22 precede implementation.

## Tasks

| Task | Action | Output |
|---|---|---|
| 04.1 | Inventory actual failure/result/exception shapes and field provenance across every surface | Source-backed matrix incl absent/not-applicable information |
| 04.2 | Specify normal invalid, conversion, invalid configuration, cancellation/timeout/resource and programmer-failure categories | Astra-approved new outcome taxonomy and negation/evaluability boundaries |
| 04.3 | Choose code/path/message/severity/value/input-output/aggregation/order semantics, equality/serialization and privacy | Complete new contract, alternatives and global impact |
| 04.4 | Map each actual applicable Must, Guard, object, FV, DA and ASP.NET projection to that contract | Intentional adaptation and representability limits |
| 04.5 | Implement shared model/mappings and remove redundant local patterns across the repo | Coherent diff with100% validation coverage |
| 04.6 | Prove independent outcomes, ordering/path/code behavior, privacy and allocation cost | W05/W10/W14/W17 evidence |

## Acceptance

Every field/outcome has a defined meaning, provenance, optionality and stability promise. Paths/indexes/keys/escaping and aggregation are explicit where supported. Operational failures cannot accidentally become ordinary invalid results or success through negation.

Messages are stable only if the new contract says so; diagnostic/display text and machine code are distinguished. Attempted values are excluded by default as a privacy preference unless an explicit reviewed need/policy establishes otherwise; classify every other potentially sensitive field too.

No surface is forced to expose nonexistent native information simply to fill a table. A new common mapping is permissible only as an explicit new design with documented semantics. New public contract drift is checked by W12, not against old releases.

Checkpoint after inventory, model decision and first full pilot. Astra decides/reviews; Sol codes; Luna reads/evidence. Stop for guessed codes, duplicated adapter truth, information loss, hidden value leakage or arbitrary catches. Follow [quality constitution](../references/quality-constitution.md), [execution](../references/execution-protocol.md), [decisions](../references/decision-register.md) and [estimates](../references/estimates-and-waves.md). Planned.

## Approved test-project boundary

The [approved testing project layout](../references/test-project-layout.md) is authoritative for this workstream's project identity, folder responsibility and execution kind. Use its workstream mapping; do not create one project per testing technique or move this crosscutting work into existing library unit suites. Reuse PineGuard.Testing fixtures and typed projections. Required projects and artifacts must exist before the applicable implementation gate can close; Planned status is not execution evidence.

### Mechanical illustration

This package-free code is a mechanical teaching example only. It is not repository test guidance, a domain test, or evidence that the proposed technique is implemented. Complete source and reproduction scope are in [Code examples](../references/code-examples.md).

This example is documentation, not an implemented PineGuard change. Its classification, dependencies and verification scope are stated below; complete source and reproduction instructions are in [Code examples](../references/code-examples.md).

```csharp
internal static class Projections
{
    internal static Verdict Must(int? value) => PositiveInt.Evaluate(value);
    internal static void Guard(int? value)
    {
        if (!PositiveInt.Evaluate(value).Passed)
            throw new ArgumentOutOfRangeException(nameof(value));
    }
    internal static string[] ObjectErrors(int? value) =>
        PositiveInt.Evaluate(value) is { Passed: false, Code: { } code }
            ? ["Value:" + code] : [];
}
```

Purpose: candidate D08 failure shape, safe object path and complement guard, all calling canonical authority. Input 1 → Must pass/no guard exception; input 0 → Must fail/guard exception/object `Value:illustration.number.not-positive`. Current Must/Guard/FV/DA excerpts above show verified names separately; they are not proved by this miniature. Existing MustFailure/MustValidationResult remain current object contract; protected RuleFor/RuleForEach and internal MustChildValidator are not invented public adapters. Status: mechanical illustration only; not repository-test evidence.

### Repository test excerpt

This documentation excerpt shows the canonical fixture-to-typed-case-to-TheoryData-to-MemberData-to-assertion chain. It does not implement PineGuard behavior; the full source-inspected patterns and scope limits are in [Code examples](../references/code-examples.md).

#### W04/W05: Guard complement pipeline

Existing GuardNumberClausesTestData.ZeroOrNegative group, exact source projection:

```csharp
public static class ZeroOrNegative
{
    public static TheoryData<GuardCase<int>> ValidCases => F.IsPositive.ValidScenarios
        .Select(s => new RuleScenario<int>(s.Name, s.Inputs!.Value, s.IsValid))
        .ToArray().ToGuardCases(_ => new GuardExpected(true));
    public static TheoryData<GuardCase<int>> InvalidCases => F.IsPositive.InvalidScenarios
        .Except(nameof(F.IsPositive.Null))
        .Select(s => new RuleScenario<int>(s.Name, s.Inputs!.Value, s.IsValid))
        .ToArray().ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value"));
}
```

Focused view of the existing GuardNumberClausesTests group method. Keep the existing custom-message assertions in that file when integrating; this excerpt shows the default-exception/return contract and does not authorize deleting other assertions:

```csharp
[Theory]
[MemberData(nameof(GuardNumberClausesTestData.ZeroOrNegative.ValidCases), MemberType = typeof(GuardNumberClausesTestData.ZeroOrNegative))]
[MemberData(nameof(GuardNumberClausesTestData.ZeroOrNegative.InvalidCases), MemberType = typeof(GuardNumberClausesTestData.ZeroOrNegative))]
public void ZeroOrNegative_BehavesAsExpected(GuardCase<int> tc)
{
    // Arrange.
    var value = tc.Value;
    // Act and assert the expected guard contract through the existing helper.
    var result = AssertResult(tc, () => Guard.Against.ZeroOrNegative(value));
    if (tc.Expected.IsValid) Assert.Equal(value, result);
}
```

Positive1 returns1; Zero0/Negative-1 throw ArgumentException with parameter value. This nonnullable projection does not test Null.

#### W04/W05: current object aggregate pipeline

Existing MustValidatorTestData.Validate.Case extends ValueCase<CreateOrder?>; existing Cases remains authoritative and unchanged. The packet did not supply its literal input rows, so they are referenced by exact MemberData rather than recreated or populated from production results. Existing type declarations in that group:

```csharp
public sealed record FailureExpectation(string PropertyPath, string Code, string? Message = null);
public sealed record Case(string Name, CreateOrder? Order, bool ExpectedSuccess,
    FailureExpectation[] ExpectedFailures) : ValueCase<CreateOrder?>(Name, Order);
```

Complete focused assertion method view in MustValidatorTests, using the existing dataset and CreateOrderValidator. Aggregate assertions are explicit because this is the supplied current composite precedent; this does not fabricate a universal new case/helper taxonomy:

```csharp
[Theory]
[MemberData(nameof(MustValidatorTestData.Validate.Cases), MemberType = typeof(MustValidatorTestData.Validate))]
public void Validate_CollectsFailuresInRegistrationOrder(MustValidatorTestData.Validate.Case testCase)
{
    // Arrange.
    var validator = new CreateOrderValidator();
    // Act.
    var result = validator.Validate(testCase.Value!);
    // Assert independently specified aggregate expectations in registration order.
    Assert.Equal(testCase.ExpectedSuccess, result.Success);
    var failures = result.Failures.ToArray();
    Assert.Equal(testCase.ExpectedFailures.Length, failures.Length);
    for (var i = 0; i < failures.Length; i++)
    {
        var expected = testCase.ExpectedFailures[i];
        Assert.Equal(expected.PropertyPath, failures[i].PropertyPath);
        Assert.Equal(expected.Code, failures[i].Code);
        if (expected.Message is not null) Assert.Equal(expected.Message, failures[i].Message);
    }
}
```

Expected success, failure count/order/path/code/optional message come from the existing independently authored aggregate dataset. This preserves its actual composite naming precedent and does not create a duplicate group/test.
