using PineGuard.Common;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Rules.StringRules;

public sealed class StringRulesTimeSpanTests : BaseUnitTest
{
    [Theory]
    [MemberData(nameof(StringRulesTimeSpanTestData.IsDurationBetween.ValidCases), MemberType = typeof(StringRulesTimeSpanTestData.IsDurationBetween))]
    [MemberData(nameof(StringRulesTimeSpanTestData.IsDurationBetween.EdgeCases), MemberType = typeof(StringRulesTimeSpanTestData.IsDurationBetween))]
    public void IsDurationBetween_ReturnsExpected(StringRulesTimeSpanTestData.IsDurationBetween.Case testCase)
    {
        var result = PineGuard.Rules.StringRules.TimeSpan.IsDurationBetween(
            testCase.Value,
            testCase.Min,
            testCase.Max,
            testCase.Inclusion);

        Assert.Equal(testCase.Expected, result);
    }

    [Theory]
    [MemberData(nameof(StringRulesTimeSpanTestData.IsGreaterThan.ValidCases), MemberType = typeof(StringRulesTimeSpanTestData.IsGreaterThan))]
    [MemberData(nameof(StringRulesTimeSpanTestData.IsGreaterThan.EdgeCases), MemberType = typeof(StringRulesTimeSpanTestData.IsGreaterThan))]
    public void IsGreaterThan_ReturnsExpected(StringRulesTimeSpanTestData.IsGreaterThan.Case testCase)
    {
        var result = PineGuard.Rules.StringRules.TimeSpan.IsGreaterThan(
            testCase.Value,
            testCase.Threshold,
            testCase.Inclusion);

        Assert.Equal(testCase.Expected, result);
    }

    [Theory]
    [MemberData(nameof(StringRulesTimeSpanTestData.IsLessThan.ValidCases), MemberType = typeof(StringRulesTimeSpanTestData.IsLessThan))]
    [MemberData(nameof(StringRulesTimeSpanTestData.IsLessThan.EdgeCases), MemberType = typeof(StringRulesTimeSpanTestData.IsLessThan))]
    public void IsLessThan_ReturnsExpected(StringRulesTimeSpanTestData.IsLessThan.Case testCase)
    {
        var result = PineGuard.Rules.StringRules.TimeSpan.IsLessThan(
            testCase.Value,
            testCase.Threshold,
            testCase.Inclusion);

        Assert.Equal(testCase.Expected, result);
    }
}
