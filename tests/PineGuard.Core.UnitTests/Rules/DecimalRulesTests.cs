using PineGuard.Rules;
using PineGuard.Testing.UnitTests.Rules;
using Xunit.Abstractions;

namespace PineGuard.Core.UnitTests.Rules;

public sealed class DecimalRulesTests(ITestOutputHelper output)
    : BaseRuleUnitTest(output)
{
    [Theory]
    [MemberData(nameof(DecimalRulesTestData.HasMaxScale.Cases), MemberType = typeof(DecimalRulesTestData.HasMaxScale))]
    public void HasMaxScale_BehavesAsExpected(RuleCase<(decimal? value, int scale)> tc)
    {
        // Arrange
        var (value, scale) = tc.Value;

        // Act
        var result = DecimalRules.HasMaxScale(value, scale);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(DecimalRulesTestData.HasMaxPrecision.Cases), MemberType = typeof(DecimalRulesTestData.HasMaxPrecision))]
    public void HasMaxPrecision_BehavesAsExpected(RuleCase<(decimal? value, int precision)> tc)
    {
        // Arrange
        var (value, precision) = tc.Value;

        // Act
        var result = DecimalRules.HasMaxPrecision(value, precision);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(DecimalRulesTestData.IsWithinPrecision.Cases), MemberType = typeof(DecimalRulesTestData.IsWithinPrecision))]
    public void IsWithinPrecision_BehavesAsExpected(RuleCase<(decimal? value, int precision, int scale)> tc)
    {
        // Arrange
        var (value, precision, scale) = tc.Value;

        // Act
        var result = DecimalRules.IsWithinPrecision(value, precision, scale);

        // Assert
        AssertResult(tc, result);
    }
}
