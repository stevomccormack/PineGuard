using PineGuard.Rules;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Rules;

public sealed class DateOnlyRangeRulesTests : BaseUnitTest
{
    [Theory]
    [MemberData(
        nameof(DateOnlyRangeRulesTestData.IsChronological.ValidCases),
        MemberType = typeof(DateOnlyRangeRulesTestData.IsChronological))]
    public void IsChronological_ReturnsExpected_ForValidCases(DateOnlyRangeRulesTestData.IsChronological.Case testCase)
    {
        // Act
        var result = DateOnlyRangeRules.IsChronological(testCase.Value, testCase.Inclusion);

        // Assert
        Assert.Equal(testCase.ExpectedReturn, result);
    }

    [Theory]
    [MemberData(
        nameof(DateOnlyRangeRulesTestData.IsChronological.EdgeCases),
        MemberType = typeof(DateOnlyRangeRulesTestData.IsChronological))]
    public void IsChronological_ReturnsFalse_ForNullOrExclusiveSameDay(DateOnlyRangeRulesTestData.IsChronological.Case testCase)
    {
        // Act
        var result = DateOnlyRangeRules.IsChronological(testCase.Value, testCase.Inclusion);

        // Assert
        Assert.Equal(testCase.ExpectedReturn, result);
    }

    [Theory]
    [MemberData(
        nameof(DateOnlyRangeRulesTestData.IsOverlapping.ValidCases),
        MemberType = typeof(DateOnlyRangeRulesTestData.IsOverlapping))]
    public void IsOverlapping_RespectsInclusion(DateOnlyRangeRulesTestData.IsOverlapping.Case testCase)
    {
        // Act
        var result = DateOnlyRangeRules.IsOverlapping(testCase.Value.Range1, testCase.Value.Range2, testCase.Inclusion);

        // Assert
        Assert.Equal(testCase.ExpectedReturn, result);
    }

    [Theory]
    [MemberData(
        nameof(DateOnlyRangeRulesTestData.IsOverlapping.EdgeCases),
        MemberType = typeof(DateOnlyRangeRulesTestData.IsOverlapping))]
    public void IsOverlapping_ReturnsFalse_WhenEitherRangeIsNull(DateOnlyRangeRulesTestData.IsOverlapping.Case testCase)
    {
        // Act
        var result = DateOnlyRangeRules.IsOverlapping(testCase.Value.Range1, testCase.Value.Range2, testCase.Inclusion);

        // Assert
        Assert.Equal(testCase.ExpectedReturn, result);
    }
}
