using PineGuard.Common;
using PineGuard.Testing.UnitTests.Rules;
using PineGuard.Utils;
using Xunit.Abstractions;

namespace PineGuard.Core.UnitTests.Utils;

public sealed class CronUtilityTests(ITestOutputHelper output)
    : BaseRuleUnitTest(output)
{
    [Theory]
    [MemberData(nameof(CronUtilityTestData.TryParse.Cases), MemberType = typeof(CronUtilityTestData.TryParse))]
    public void TryParse_BehavesAsExpected(RuleCase<(string? value, CronFormat format, string[]? fields)> tc)
    {
        // Arrange
        var (value, format, expectedFields) = tc.Value;

        // Act
        var result = CronUtility.TryParse(value, format, out var fields);

        // Assert
        AssertResult(tc, result);
        Assert.Equal(expectedFields, fields);
    }
}
