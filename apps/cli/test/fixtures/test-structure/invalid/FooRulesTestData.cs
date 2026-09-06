using PineGuard.Testing.UnitTests.Rules;

namespace PineGuard.Core.UnitTests.Rules;

public static class FooRulesTestData
{
    public static class IsBar
    {
        // Deliberately out of order for the invalid/ fixture: v11 §4.4
        // requires ValidCases -> EdgeCases -> InvalidCases; InvalidCases
        // must not come first.
        public static TheoryData<RuleCase<string?>> InvalidCases =>
        [
            new("empty", "", new RuleExpected(false)),
        ];

        public static TheoryData<RuleCase<string?>> ValidCases =>
        [
            new("bar", "bar", new RuleExpected(true)),
        ];
    }

    public static class IsBaz
    {
        // Deliberately empty scaffolding for the invalid/ fixture: v11 §4.1
        // forbids `=> [];` — omit the dataset entirely instead.
        public static TheoryData<RuleCase<string?>> ValidCases => [];
    }
}
