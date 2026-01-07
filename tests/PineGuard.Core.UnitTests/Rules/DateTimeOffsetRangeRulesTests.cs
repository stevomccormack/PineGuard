using PineGuard.Common;
using PineGuard.Rules;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Rules;

public sealed class DateTimeOffsetRangeRulesTests : BaseUnitTest
{
    [Theory]
    [MemberData(nameof(DateTimeOffsetRangeRulesTestData.IsChronological.ValidCases), MemberType = typeof(DateTimeOffsetRangeRulesTestData.IsChronological))]
    public void IsChronological_ReturnsExpected_ForValidCases(DateTimeOffsetRangeRulesTestData.IsChronological.Case testCase)
    {
        // Arrange

        // Act
        var result = DateTimeOffsetRangeRules.IsChronological(testCase.Range, testCase.Inclusion);

        // Assert
        Assert.Equal(testCase.Expected, result);
    }

    [Theory]
    [MemberData(nameof(DateTimeOffsetRangeRulesTestData.IsChronological.EdgeCases), MemberType = typeof(DateTimeOffsetRangeRulesTestData.IsChronological))]
    public void IsChronological_ReturnsExpected_ForEdgeCases(DateTimeOffsetRangeRulesTestData.IsChronological.Case testCase)
    {
        // Arrange

        // Act
        var result = DateTimeOffsetRangeRules.IsChronological(testCase.Range, testCase.Inclusion);

        // Assert
        Assert.Equal(testCase.Expected, result);
    }

    [Theory]
    [MemberData(nameof(DateTimeOffsetRangeRulesTestData.IsOverlapping.ValidCases), MemberType = typeof(DateTimeOffsetRangeRulesTestData.IsOverlapping))]
    public void IsOverlapping_RespectsInclusion_ForValidCases(DateTimeOffsetRangeRulesTestData.IsOverlapping.Case testCase)
    {
        // Arrange

        // Act
        var result = DateTimeOffsetRangeRules.IsOverlapping(testCase.Range1, testCase.Range2, testCase.Inclusion);

        // Assert
        Assert.Equal(testCase.Expected, result);
    }

    [Theory]
    [MemberData(nameof(DateTimeOffsetRangeRulesTestData.IsOverlapping.EdgeCases), MemberType = typeof(DateTimeOffsetRangeRulesTestData.IsOverlapping))]
    public void IsOverlapping_ReturnsExpected_ForEdgeCases(DateTimeOffsetRangeRulesTestData.IsOverlapping.Case testCase)
    {
        // Arrange

        // Act
        var result = DateTimeOffsetRangeRules.IsOverlapping(testCase.Range1, testCase.Range2, testCase.Inclusion);

        // Assert
        Assert.Equal(testCase.Expected, result);
    }
}
