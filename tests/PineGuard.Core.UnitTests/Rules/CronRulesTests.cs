using PineGuard.Common;
using PineGuard.Rules;
using PineGuard.Testing.UnitTests.Rules;
using Xunit.Abstractions;

namespace PineGuard.Core.UnitTests.Rules;

public sealed class CronRulesTests(ITestOutputHelper output)
    : BaseRuleUnitTest(output)
{
    [Theory]
    [MemberData(nameof(CronRulesTestData.IsCronExpression.Cases), MemberType = typeof(CronRulesTestData.IsCronExpression))]
    public void IsCronExpression_BehavesAsExpected(RuleCase<(string? value, CronFormat format)> tc)
    {
        // Arrange
        var (value, format) = tc.Value;

        // Act
        var result = CronRules.IsCronExpression(value, format);

        // Assert
        AssertResult(tc, result);
    }
}
