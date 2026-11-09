<!-- metadata_header
type: plan
id: technical-readiness-to-1.0-w14
version: 1.4
status: planned
last_updated: 2026-11-09
-->
# W14 — Threat model and defensible security claims

> **Status 2026-11-09:** not activated. The plan-gated council ran on 2026-11-09 and ruled that this plan is not activated as written; the only unit that may start, on the owner's explicit go, is the string→DateOnly pilot PR described in [plan.md § Where this plan stands](../plan.md#where-this-plan-stands-2026-11-09) and register decision D29. The completion gate, Astra/Luna/Sol roles, checkpoints and D22 inventory steps below apply to a later wave activation, not to the pilot PR.

> **Mandatory completion gate:** This workstream and every major block inherit the [knowledge, documentation and tooling closure contract](../references/knowledge-and-tooling-closure.md). Save evidence-linked lessons/decisions (or an explicit no-new-lesson outcome), synchronize all relevant BRAIN/Markdown/indexes, build or update required supporting scripts/tools and test/run them (or record reviewed N/A), then complete full Astra review → feedback → fix → recheck. Persist the impact matrix, feedback and owned commit evidence; reopen the unit when relevant context changes. Feature implementation alone cannot close this work.

## Purpose

W14 specifies what security-relevant validation does and does not establish, based on the actual new contracts and consumer boundaries. Rubric security/data is6/10 with medium confidence; v1.6 §8 and the handoff call for formalized claims and hostile evidence.

Inputs: W01 OWASP reproduction and semantics, W03 call paths, W04 failure/privacy contract, W07 fuzz priorities, W15 regex and W16 resource policy. Verified anchors: `src/PineGuard.Core/Rules/OwaspRules.cs`, `src/PineGuard.MustClauses/MustOwaspClauses.cs`, `tests/PineGuard.Testing/Fixtures/OwaspRulesFixtures.cs`. Output: `+ docs/reports/readiness-threat-model.md`.

## Tasks

| Task | Action | Output |
|---|---|---|
| 14.1 | Identify assets, actors, trust boundaries, attacker-controlled inputs and deployment assumptions | Threat model with concrete call/consumer boundaries |
| 14.2 | Inventory security-related rule names/messages/docs and inferable claims | Claim register linked to source and current evidence |
| 14.3 | Reproduce specific candidate concerns without generalizing to untested composites | Minimal demonstrations, preconditions, consequence and uncertainty |
| 14.4 | Decide names/contracts/claim language consistent with actual guarantees | Astra decisions and W22/W01 changes where needed |
| 14.5 | Map threats to independent cases, fuzz/regex/resource controls and privacy tests | Threat-to-mitigation-to-proof matrix |
| 14.6 | Review residual limitations and package/sample documentation | Scoped approved claims for W20/W21 |

## Required distinctions

Validation is not automatically sanitization, encoding, authorization or a complete injection defense. Any security name or documentation must identify the boundary it addresses and the assumptions it requires. A source pattern or scanner finding is not proof of exploitability; conversely a green test suite is not a threat model.

The trimming/newline observation must be reproduced with the implicated individual/composite paths and returned value before assigning a consequence. Do not label every OWASP path vulnerable or safe based on one observation. Review leakage through attempted values, messages, paths, exception payloads and diagnostics.

## Acceptance

Every security-relevant affirmative claim has explicit scope, threat assumptions and executable evidence. Important threats have a mitigation or are outside a clearly declared supported boundary; unresolved material threats block the affected10/10 security claim. Test data is synthetic unless an approved handling policy permits otherwise.

## Controls

Checkpoint after threat/claim inventory and before changing behavior or public claim language. Astra approves security semantics; Sol implements approved shared fixes; Luna gathers source/research evidence. Fix root causes across all impacted projections, not a single failing test path. New limits/regex policies belong to W15/W16 and must not be guessed inside a security test.

Follow the [execution protocol](../references/execution-protocol.md), [decision register](../references/decision-register.md), [quality constitution](../references/quality-constitution.md) and [estimates and waves](../references/estimates-and-waves.md). This child remains Planned.

## Approved test-project boundary

The [approved testing project layout](../references/test-project-layout.md) is authoritative for this workstream's project identity, folder responsibility and execution kind. Use its workstream mapping; do not create one project per testing technique or move this crosscutting work into existing library unit suites. Reuse PineGuard.Testing fixtures and typed projections. Required projects and artifacts must exist before the applicable implementation gate can close; Planned status is not execution evidence.

### Mechanical illustration

This package-free code is a mechanical teaching example only. It is not repository test guidance, a domain test, or evidence that the proposed technique is implemented. Complete source and reproduction scope are in [Code examples](../references/code-examples.md).

This example is documentation, not an implemented PineGuard change. Its classification, dependencies and verification scope are stated below; complete source and reproduction instructions are in [Code examples](../references/code-examples.md).

```csharp
Checks.Require(Boundary.Invoke(new string('9', 129)).Status == 400, "size rejection");
Checks.Require(Boundary.Invoke("2147483648").Status == 400, "overflow rejection");
var verdict = PositiveInt.Evaluate(-987654321);
Checks.Require(Diagnostics.Event(false, verdict) is null, "diagnostics opt-in");
Checks.Require(Diagnostics.Event(true, verdict) ==
    "rule=illustration.number.positive;code=illustration.number.not-positive", "exact allowlist");
```

Purpose: claim-specific tests only: oversize raw numeric input rejected before parsing and diagnostic output has exact allowlist. Overflow/129 digits→400; attempted -987654321 never appears in event. No general security claim or adversarial regex-timeout proof follows. Status: mechanical illustration only; not repository-test evidence.

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

| W14 claim-specific security | Oversize129-character numeric input→rejected before parsing; overflow2147483648→rejected; no attempted numeric value in diagnostic event. These are candidate boundary claims, not general security proof. | Program.cs Boundary.TryCreate and Diagnostics.Event; claimed limits/operational responses remain provisional. |
