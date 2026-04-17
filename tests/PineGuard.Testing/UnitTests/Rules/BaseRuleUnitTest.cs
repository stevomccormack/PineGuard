using Xunit;
using Xunit.Abstractions;

namespace PineGuard.Testing.UnitTests.Rules;

public abstract class BaseRuleUnitTest(ITestOutputHelper output) : BaseUnitTest(output)
{
    protected static void AssertRule(RuleExpected expected, bool result) =>
        Assert.Equal(expected.IsValid, result);

    protected static void AssertResult<TValue>(RuleCase<TValue> testCase, bool result) =>
        AssertRule(testCase.Expected, result);
}
