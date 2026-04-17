using PineGuard.Testing.UnitTests.Rules;

namespace PineGuard.Testing.UnitTests.UnitTests.Rules;

public static class BaseRuleUnitTestTestData
{
    public static class AssertRuleOps
    {
        public sealed record Case(string Name, (RuleExpected expected, bool result) Value)
            : BaseCase(Name);

        public static TheoryData<Case> ValidCases =>
        [
            new("valid matches true", (new RuleExpected(true), true)),
            new("invalid matches false", (new RuleExpected(false), false))
        ];
    }

    public static class AssertResultOps
    {
        public sealed record Case(string Name, (RuleCase<string?> ruleCase, bool result) Value)
            : BaseCase(Name);

        public static TheoryData<Case> ValidCases =>
        [
            new("valid case passes", (new RuleCase<string?>("test", "x", new RuleExpected(true)), true)),
            new("invalid case passes", (new RuleCase<string?>("test", null, new RuleExpected(false)), false))
        ];
    }
}
