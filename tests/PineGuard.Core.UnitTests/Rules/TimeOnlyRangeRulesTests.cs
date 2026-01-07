using PineGuard.Common;
using PineGuard.Rules;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Rules;

public sealed class TimeOnlyRangeRulesTests : BaseUnitTest
{
    [Theory]
    [MemberData(nameof(TimeOnlyRangeRulesTestData.IsChronological.ValidCases), MemberType = typeof(TimeOnlyRangeRulesTestData.IsChronological))]
    [MemberData(nameof(TimeOnlyRangeRulesTestData.IsChronological.EdgeCases), MemberType = typeof(TimeOnlyRangeRulesTestData.IsChronological))]
    public void IsChronological_ReturnsExpected(TimeOnlyRangeRulesTestData.IsChronological.Case testCase)
    {
        var result = TimeOnlyRangeRules.IsChronological(testCase.Range, testCase.Inclusion);

        Assert.Equal(testCase.Expected, result);
    }

    [Theory]
    [MemberData(nameof(TimeOnlyRangeRulesTestData.IsOverlapping.ValidCases), MemberType = typeof(TimeOnlyRangeRulesTestData.IsOverlapping))]
    [MemberData(nameof(TimeOnlyRangeRulesTestData.IsOverlapping.EdgeCases), MemberType = typeof(TimeOnlyRangeRulesTestData.IsOverlapping))]
    public void IsOverlapping_ReturnsExpected(TimeOnlyRangeRulesTestData.IsOverlapping.Case testCase)
    {
        var result = TimeOnlyRangeRules.IsOverlapping(testCase.Range1, testCase.Range2, testCase.Inclusion);

        Assert.Equal(testCase.Expected, result);
    }
}
