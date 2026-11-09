<!-- metadata_header
type: plan
id: technical-readiness-to-1.0-w15
version: 1.4
status: planned
last_updated: 2026-11-09
-->
# W15 — Regex semantics and governance

> **Status 2026-11-09:** not activated. The plan-gated council ran on 2026-11-09 and ruled that this plan is not activated as written; the only unit that may start, on the owner's explicit go, is the string→DateOnly pilot PR described in [plan.md § Where this plan stands](../plan.md#where-this-plan-stands-2026-11-09) and register decision D29. The completion gate, Astra/Luna/Sol roles, checkpoints and D22 inventory steps below apply to a later wave activation, not to the pilot PR.

> **Mandatory completion gate:** This workstream and every major block inherit the [knowledge, documentation and tooling closure contract](../references/knowledge-and-tooling-closure.md). Save evidence-linked lessons/decisions (or an explicit no-new-lesson outcome), synchronize all relevant BRAIN/Markdown/indexes, build or update required supporting scripts/tools and test/run them (or record reviewed N/A), then complete full Astra review → feedback → fix → recheck. Persist the impact matrix, feedback and owned commit evidence; reopen the unit when relevant context changes. Feature implementation alone cannot close this work.

## Purpose and baseline

W15 resolves regex-specific correctness/resource behavior early where it blocks W01, then supplies P3 enforcement. Source-only observations: `src/PineGuard.Core/Rules/StringRules.cs` exposes raw regex timeout behavior and `src/PineGuard.MustClauses/MustStringClauses.cs` Match/NotMatch lack a verified mapping; built-in regex timeout behavior is anchored by `src/PineGuard.Core/Rules/Owasp/OwaspRegex.cs`. Reproduction remains necessary.

Inputs: W01 contracts, W14 threat priorities, W10 early measurements and W16 general resource policy. Output: `+ docs/reports/readiness-regex.md`.

## Decisions and tasks

D05/D14 require Astra answers before implementation: invalid-pattern versus invalid-value behavior; timeout/operational outcome; negation; built-in versus caller patterns; engine/options/culture; timeout source and configurability; resource defaults.

| Task | Action | Output |
|---|---|---|
| 15.1 | Inventory regex use, source-generated/compiled/interpreted paths, patterns/options/timeouts and public entry points | Complete regex governance map with exact owned paths |
| 15.2 | Reproduce Match/NotMatch invalid-pattern/timeout behavior and built-in cases | Environment-specific independent outcome evidence |
| 15.3 | Decide a globally consistent outcome taxonomy and pattern/timeout ownership policy | Astra records; no timeout-to-success negation accident |
| 15.4 | Implement shared policy at the correct abstraction and update every impacted surface | Global diff, independent cases and100% scoped coverage |
| 15.5 | Evaluate hostile patterns/inputs under bounded measurements and fuzz cases | W07/W10 evidence and approved finite runtime policy |
| 15.6 | Enforce inventory/review for newly introduced patterns/options and publish truthful semantics | CI/audit checks, deliberate missing-policy proof and documentation |

## Acceptance

Every supported regex path has an intentional finite-resource policy or a specifically approved boundary that states what can be controlled. A timeout is not silently converted into ordinary non-match, especially under NotMatch, unless that is the explicitly reviewed semantic decision. Invalid patterns/configuration and normal invalid inputs are distinguished.

Engine changes, nonbacktracking choices, generated regex and caches require measured semantics/performance/deployment evidence. Do not assume an engine option supports every pattern. Cache bounds/thread-safety, if a cache exists or is proposed, require W16/W19 review.

## Controls

Checkpoint after inventory/reproduction, after semantic decision and before broad migration. Sol fixes the shared cause; Luna supplies source and official runtime documentation; Astra decides policy. Stop for arbitrary timeout constants, blanket exception catching, local special-case wrappers, unbounded pattern caches or claims extending beyond tested engine/target behavior. W16 owns cross-operation limits; W15 owns regex policy and contributes its measurements.

Follow the [execution protocol](../references/execution-protocol.md), [decision register](../references/decision-register.md), [quality constitution](../references/quality-constitution.md) and [estimates and waves](../references/estimates-and-waves.md). This child remains Planned.

## Approved test-project boundary

The [approved testing project layout](../references/test-project-layout.md) is authoritative for this workstream's project identity, folder responsibility and execution kind. Use its workstream mapping; do not create one project per testing technique or move this crosscutting work into existing library unit suites. Reuse PineGuard.Testing fixtures and typed projections. Required projects and artifacts must exist before the applicable implementation gate can close; Planned status is not execution evidence.

### Mechanical illustration

This package-free code is a mechanical teaching example only. It is not repository test guidance, a domain test, or evidence that the proposed technique is implemented. Complete source and reproduction scope are in [Code examples](../references/code-examples.md).

This example is documentation, not an implemented PineGuard change. Its classification, dependencies and verification scope are stated below; complete source and reproduction instructions are in [Code examples](../references/code-examples.md).

```csharp
internal static class PatternProbe
{
    internal static RegexOutcome Evaluate(string input, string pattern, bool negate)
    {
        try
        {
            var matched = Regex.IsMatch(input, pattern, RegexOptions.CultureInvariant,
                TimeSpan.FromMilliseconds(20));
            return matched != negate ? RegexOutcome.Match : RegexOutcome.NoMatch;
        }
        catch (ArgumentException) { return RegexOutcome.OperationalError; }
        catch (RegexMatchTimeoutException) { return RegexOutcome.OperationalError; }
    }
}
```

Purpose: candidate D05 typed operational state keeps invalid pattern failure from becoming negated success. `[` on x→OperationalError under both negation settings; known match/nonmatch are explicit. Timeout catch uses same operational state, but deterministic timeout injection is not supplied, so timeout path is unverified. Candidate20ms is only miniature input; existing Email generated helper200ms must not be generalized to all StringRules. Status: mechanical illustration only; not repository-test evidence.

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

| W15 regex | InvalidPatternNonNegated and InvalidPatternNegated both→OperationalError; KnownMatch→Match; KnownNegatedMatch→NoMatch. Timeout requires deterministic injection/actual evidence and must never become negated success. | Complete PatternProbe.Evaluate try/catch/status body; candidate D05 semantics,20ms not selected. |
