using PineGuard.Testing.UnitTests.Rules;
using F = PineGuard.Testing.Fixtures.NullRulesFixtures;

namespace PineGuard.Core.UnitTests.Rules;

public static class NullRulesTestData
{
    public static class IsNull
    {
        public static TheoryData<RuleCase<object?>> Cases => F.IsNull.AllScenarios.ToRuleCases();
    }

    public static class IsNotNull
    {
        public static TheoryData<RuleCase<object?>> Cases => F.IsNotNull.AllScenarios.ToRuleCases();
    }

    // fixture.md §12.2 — NullCases legitimately supplements an explicit
    // ValidCases/InvalidCases pair in an inverted-guard nullable variant
    // (plan decision 5, P4.3 §7.5). Must NOT be flagged: the co-occurrence
    // condition (both ValidCases and InvalidCases present) is met.
    public static class IsInvertedNullable
    {
        public static TheoryData<RuleCase<string?>> ValidCases =>
        [
            new("good", "ok", new RuleExpected(true)),
        ];

        public static TheoryData<RuleCase<string?>> InvalidCases =>
        [
            new("bad", "no", new RuleExpected(false)),
        ];

        public static TheoryData<RuleCase<string?>> NullCases =>
        [
            new("null", null, new RuleExpected(false)),
        ];
    }

    // fixture.md §11.6 — AdHocCases legitimately supplements a
    // RuleScenario-derived Cases rollup with layer-specific cases not
    // derivable from RuleScenarios (plan decision 5). Must NOT be flagged:
    // the co-occurrence condition (Cases present) is met.
    public static class IsAdHocFriendly
    {
        public static TheoryData<RuleCase<string?>> Cases =>
        [
            new("derived", "ok", new RuleExpected(true)),
        ];

        public static TheoryData<RuleCase<string?>> AdHocCases =>
        [
            new("custom", "special", new RuleExpected(true)),
        ];
    }
}
