using PineGuard.Rules;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Rules;

public sealed class TimeSpanRulesTests : BaseUnitTest
{
    [Theory]
    [MemberData(nameof(TimeSpanRulesTestData.IsDurationBetween.ValidCases), MemberType = typeof(TimeSpanRulesTestData.IsDurationBetween))]
    [MemberData(nameof(TimeSpanRulesTestData.IsDurationBetween.EdgeCases), MemberType = typeof(TimeSpanRulesTestData.IsDurationBetween))]
    public void IsDurationBetween_ReturnsExpected(TimeSpanRulesTestData.IsDurationBetween.Case testCase)
    {
        var result = TimeSpanRules.IsDurationBetween(testCase.Value, testCase.Min, testCase.Max, testCase.Inclusion);

        Assert.Equal(testCase.ExpectedReturn, result);
    }

    [Theory]
    [MemberData(nameof(TimeSpanRulesTestData.IsGreaterThan.ValidCases), MemberType = typeof(TimeSpanRulesTestData.IsGreaterThan))]
    [MemberData(nameof(TimeSpanRulesTestData.IsGreaterThan.EdgeCases), MemberType = typeof(TimeSpanRulesTestData.IsGreaterThan))]
    public void IsGreaterThan_ReturnsExpected(TimeSpanRulesTestData.IsGreaterThan.Case testCase)
    {
        var result = TimeSpanRules.IsGreaterThan(testCase.Value, testCase.Threshold, testCase.Inclusion);

        Assert.Equal(testCase.ExpectedReturn, result);
    }

    [Theory]
    [MemberData(nameof(TimeSpanRulesTestData.IsLessThan.ValidCases), MemberType = typeof(TimeSpanRulesTestData.IsLessThan))]
    [MemberData(nameof(TimeSpanRulesTestData.IsLessThan.EdgeCases), MemberType = typeof(TimeSpanRulesTestData.IsLessThan))]
    public void IsLessThan_ReturnsExpected(TimeSpanRulesTestData.IsLessThan.Case testCase)
    {
        var result = TimeSpanRules.IsLessThan(testCase.Value, testCase.Threshold, testCase.Inclusion);

        Assert.Equal(testCase.ExpectedReturn, result);
    }
}
