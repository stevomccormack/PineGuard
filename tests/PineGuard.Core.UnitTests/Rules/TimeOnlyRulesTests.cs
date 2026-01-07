using PineGuard.Rules;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Rules;

public sealed class TimeOnlyRulesTests : BaseUnitTest
{
    [Theory]
    [MemberData(nameof(TimeOnlyRulesTestData.IsBetween.ValidCases), MemberType = typeof(TimeOnlyRulesTestData.IsBetween))]
    [MemberData(nameof(TimeOnlyRulesTestData.IsBetween.EdgeCases), MemberType = typeof(TimeOnlyRulesTestData.IsBetween))]
    public void IsBetween_ReturnsExpected(TimeOnlyRulesTestData.IsBetween.Case testCase)
    {
        var result = TimeOnlyRules.IsBetween(testCase.Value, testCase.Min, testCase.Max, testCase.Inclusion);

        Assert.Equal(testCase.Expected, result);
    }

    [Theory]
    [MemberData(nameof(TimeOnlyRulesTestData.IsChronological.ValidCases), MemberType = typeof(TimeOnlyRulesTestData.IsChronological))]
    [MemberData(nameof(TimeOnlyRulesTestData.IsChronological.EdgeCases), MemberType = typeof(TimeOnlyRulesTestData.IsChronological))]
    public void IsChronological_ReturnsExpected(TimeOnlyRulesTestData.IsChronological.Case testCase)
    {
        var result = TimeOnlyRules.IsChronological(testCase.Start, testCase.End, testCase.Inclusion);

        Assert.Equal(testCase.Expected, result);
    }

    [Theory]
    [MemberData(nameof(TimeOnlyRulesTestData.IsOverlapping.ValidCases), MemberType = typeof(TimeOnlyRulesTestData.IsOverlapping))]
    [MemberData(nameof(TimeOnlyRulesTestData.IsOverlapping.EdgeCases), MemberType = typeof(TimeOnlyRulesTestData.IsOverlapping))]
    public void IsOverlapping_ReturnsExpected(TimeOnlyRulesTestData.IsOverlapping.Case testCase)
    {
        var result = TimeOnlyRules.IsOverlapping(testCase.Start1, testCase.End1, testCase.Start2, testCase.End2, testCase.Inclusion);

        Assert.Equal(testCase.Expected, result);
    }
}
