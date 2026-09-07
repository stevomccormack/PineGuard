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

    public static class IsQux
    {
        // P4.3 finding #5 (plan decision 5): NullCases is recognised only
        // when ValidCases AND InvalidCases both co-occur (fixture.md §12.2)
        // — both do here, so this empty `NullCases => [];` scaffolding MUST
        // now be caught by the same check 4 that already forbids it for
        // ValidCases/EdgeCases/InvalidCases/Cases.
        public static TheoryData<RuleCase<string?>> ValidCases =>
        [
            new("good", "ok", new RuleExpected(true)),
        ];

        public static TheoryData<RuleCase<string?>> InvalidCases =>
        [
            new("bad", null, new RuleExpected(false)),
        ];

        public static TheoryData<RuleCase<string?>> NullCases => [];
    }
}
