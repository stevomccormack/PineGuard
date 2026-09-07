using PineGuard.Testing.UnitTests.Rules;

namespace PineGuard.Core.UnitTests.Rules;

public static class BarRulesTestData
{
    public static class IsBar
    {
        public static TheoryData<RuleCase<string?>> Cases =>
        [
            new("bar", "bar", new RuleExpected(true)),
        ];
    }
}
