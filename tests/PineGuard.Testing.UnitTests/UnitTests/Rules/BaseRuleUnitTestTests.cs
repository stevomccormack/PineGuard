using PineGuard.Testing.UnitTests.Rules;

namespace PineGuard.Testing.UnitTests.UnitTests.Rules;

public sealed class BaseRuleUnitTestTests
{
    // ReSharper disable once ClassNeverInstantiated.Local
    private sealed class Testable() : BaseRuleUnitTest(null!)
    {
        public static void InvokeAssertRule(RuleExpected expected, bool result) =>
            AssertRule(expected, result);

        public static void InvokeAssertResult<TValue>(RuleCase<TValue> testCase, bool result) =>
            AssertResult(testCase, result);
    }

    public static class AssertRuleOps
    {
        [Theory]
        [MemberData(nameof(BaseRuleUnitTestTestData.AssertRuleOps.ValidCases), MemberType = typeof(BaseRuleUnitTestTestData.AssertRuleOps))]
        public static void ValidAndEdge_BehavesAsExpected(BaseRuleUnitTestTestData.AssertRuleOps.Case testCase)
        {
            var (expected, result) = testCase.Value;
            Testable.InvokeAssertRule(expected, result);
        }
    }

    public static class AssertResultOps
    {
        [Theory]
        [MemberData(nameof(BaseRuleUnitTestTestData.AssertResultOps.ValidCases), MemberType = typeof(BaseRuleUnitTestTestData.AssertResultOps))]
        public static void ValidAndEdge_BehavesAsExpected(BaseRuleUnitTestTestData.AssertResultOps.Case testCase)
        {
            var (ruleCase, result) = testCase.Value;
            Testable.InvokeAssertResult(ruleCase, result);
        }
    }
}
