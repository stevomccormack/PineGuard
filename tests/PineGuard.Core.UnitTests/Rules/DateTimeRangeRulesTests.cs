using PineGuard.Rules;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Rules;

public sealed class DateTimeRangeRulesTests : BaseUnitTest
{
    [Theory]
    [MemberData(nameof(DateTimeRangeRulesTestData.IsChronological.ValidCases), MemberType = typeof(DateTimeRangeRulesTestData.IsChronological))]
    public void IsChronological_ReturnsExpected_ForValidCases(DateTimeRangeRulesTestData.IsChronological.Case testCase)
    {
        // Act
        var result = DateTimeRangeRules.IsChronological(testCase.Value, testCase.Inclusion);

        // Assert
        Assert.Equal(testCase.ExpectedReturn, result);
    }

    [Theory]
    [MemberData(nameof(DateTimeRangeRulesTestData.IsChronological.EdgeCases), MemberType = typeof(DateTimeRangeRulesTestData.IsChronological))]
    public void IsChronological_ReturnsExpected_ForEdgeCases(DateTimeRangeRulesTestData.IsChronological.Case testCase)
    {
        // Act
        var result = DateTimeRangeRules.IsChronological(testCase.Value, testCase.Inclusion);

        // Assert
        Assert.Equal(testCase.ExpectedReturn, result);
    }

    [Theory]
    [MemberData(nameof(DateTimeRangeRulesTestData.IsOverlapping.ValidCases), MemberType = typeof(DateTimeRangeRulesTestData.IsOverlapping))]
    public void IsOverlapping_RespectsInclusion_ForValidCases(DateTimeRangeRulesTestData.IsOverlapping.Case testCase)
    {
        // Act
        var result = DateTimeRangeRules.IsOverlapping(testCase.Value.Range1, testCase.Value.Range2, testCase.Inclusion);

        // Assert
        Assert.Equal(testCase.ExpectedReturn, result);
    }

    [Theory]
    [MemberData(nameof(DateTimeRangeRulesTestData.IsOverlapping.EdgeCases), MemberType = typeof(DateTimeRangeRulesTestData.IsOverlapping))]
    public void IsOverlapping_ReturnsExpected_ForEdgeCases(DateTimeRangeRulesTestData.IsOverlapping.Case testCase)
    {
        // Act
        var result = DateTimeRangeRules.IsOverlapping(testCase.Value.Range1, testCase.Value.Range2, testCase.Inclusion);

        // Assert
        Assert.Equal(testCase.ExpectedReturn, result);
    }
}
