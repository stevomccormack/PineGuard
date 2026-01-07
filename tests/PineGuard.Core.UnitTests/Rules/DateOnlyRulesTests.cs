using PineGuard.Common;
using PineGuard.Rules;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Rules;

public sealed class DateOnlyRulesTests : BaseUnitTest
{
    [Theory]
    [MemberData(nameof(DateOnlyRulesTestData.IsInPast.ValidCases), MemberType = typeof(DateOnlyRulesTestData.IsInPast))]
    [MemberData(nameof(DateOnlyRulesTestData.IsInPast.EdgeCases), MemberType = typeof(DateOnlyRulesTestData.IsInPast))]
    public void IsInPast_ReturnsExpected(DateOnlyRulesTestData.IsInPast.Case testCase)
    {
        // Arrange

        // Act
        var result = DateOnlyRules.IsInPast(testCase.Value);

        // Assert
        Assert.Equal(testCase.Expected, result);
    }

    [Theory]
    [MemberData(nameof(DateOnlyRulesTestData.IsInFuture.ValidCases), MemberType = typeof(DateOnlyRulesTestData.IsInFuture))]
    [MemberData(nameof(DateOnlyRulesTestData.IsInFuture.EdgeCases), MemberType = typeof(DateOnlyRulesTestData.IsInFuture))]
    public void IsInFuture_ReturnsExpected(DateOnlyRulesTestData.IsInFuture.Case testCase)
    {
        // Arrange

        // Act
        var result = DateOnlyRules.IsInFuture(testCase.Value);

        // Assert
        Assert.Equal(testCase.Expected, result);
    }

    [Theory]
    [MemberData(nameof(DateOnlyRulesTestData.IsBetween.ValidCases), MemberType = typeof(DateOnlyRulesTestData.IsBetween))]
    public void IsBetween_RespectsInclusion(DateOnlyRulesTestData.IsBetween.Case testCase)
    {
        // Arrange

        // Act
        var result = DateOnlyRules.IsBetween(testCase.Value, testCase.Min, testCase.Max, testCase.Inclusion);

        // Assert
        Assert.Equal(testCase.Expected, result);
    }

    [Theory]
    [MemberData(nameof(DateOnlyRulesTestData.IsBetween.EdgeCases), MemberType = typeof(DateOnlyRulesTestData.IsBetween))]
    public void IsBetween_ReturnsFalse_WhenValueIsNull(DateOnlyRulesTestData.IsBetween.Case testCase)
    {
        // Arrange

        // Act
        var result = DateOnlyRules.IsBetween(testCase.Value, testCase.Min, testCase.Max, testCase.Inclusion);

        // Assert
        Assert.Equal(testCase.Expected, result);
    }

    [Theory]
    [MemberData(nameof(DateOnlyRulesTestData.IsBefore.ValidCases), MemberType = typeof(DateOnlyRulesTestData.IsBefore))]
    public void IsBefore_RespectsInclusion(DateOnlyRulesTestData.IsBefore.Case testCase)
    {
        // Arrange

        // Act
        var result = DateOnlyRules.IsBefore(testCase.Value, testCase.Other, testCase.Inclusion);

        // Assert
        Assert.Equal(testCase.Expected, result);
    }

    [Theory]
    [MemberData(nameof(DateOnlyRulesTestData.IsBefore.EdgeCases), MemberType = typeof(DateOnlyRulesTestData.IsBefore))]
    public void IsBefore_ReturnsFalse_WhenValueIsNull(DateOnlyRulesTestData.IsBefore.Case testCase)
    {
        // Arrange

        // Act
        var result = DateOnlyRules.IsBefore(testCase.Value, testCase.Other, testCase.Inclusion);

        // Assert
        Assert.Equal(testCase.Expected, result);
    }

    [Theory]
    [MemberData(nameof(DateOnlyRulesTestData.IsAfter.ValidCases), MemberType = typeof(DateOnlyRulesTestData.IsAfter))]
    public void IsAfter_RespectsInclusion(DateOnlyRulesTestData.IsAfter.Case testCase)
    {
        // Arrange

        // Act
        var result = DateOnlyRules.IsAfter(testCase.Value, testCase.Other, testCase.Inclusion);

        // Assert
        Assert.Equal(testCase.Expected, result);
    }

    [Theory]
    [MemberData(nameof(DateOnlyRulesTestData.IsAfter.EdgeCases), MemberType = typeof(DateOnlyRulesTestData.IsAfter))]
    public void IsAfter_ReturnsFalse_WhenValueIsNull(DateOnlyRulesTestData.IsAfter.Case testCase)
    {
        // Arrange

        // Act
        var result = DateOnlyRules.IsAfter(testCase.Value, testCase.Other, testCase.Inclusion);

        // Assert
        Assert.Equal(testCase.Expected, result);
    }

    [Theory]
    [MemberData(nameof(DateOnlyRulesTestData.IsSame.ValidCases), MemberType = typeof(DateOnlyRulesTestData.IsSame))]
    public void IsSame_ComparesByDateOnlyEquals(DateOnlyRulesTestData.IsSame.Case testCase)
    {
        // Arrange

        // Act
        var result = DateOnlyRules.IsSame(testCase.Value, testCase.Other);

        // Assert
        Assert.Equal(testCase.Expected, result);
    }

    [Theory]
    [MemberData(nameof(DateOnlyRulesTestData.IsSame.EdgeCases), MemberType = typeof(DateOnlyRulesTestData.IsSame))]
    public void IsSame_ReturnsFalse_WhenValueIsNull(DateOnlyRulesTestData.IsSame.Case testCase)
    {
        // Arrange

        // Act
        var result = DateOnlyRules.IsSame(testCase.Value, testCase.Other);

        // Assert
        Assert.Equal(testCase.Expected, result);
    }

    [Theory]
    [MemberData(
        nameof(DateOnlyRulesTestData.IsChronological.ValidCases),
        MemberType = typeof(DateOnlyRulesTestData.IsChronological))]
    public void IsChronological_RespectsInclusion(DateOnlyRulesTestData.IsChronological.Case testCase)
    {
        // Arrange

        // Act
        var result = DateOnlyRules.IsChronological(testCase.Start, testCase.End, testCase.Inclusion);

        // Assert
        Assert.Equal(testCase.Expected, result);
    }

    [Theory]
    [MemberData(
        nameof(DateOnlyRulesTestData.IsChronological.EdgeCases),
        MemberType = typeof(DateOnlyRulesTestData.IsChronological))]
    public void IsChronological_ReturnsFalse_ForNullsOrExclusiveSameDay(DateOnlyRulesTestData.IsChronological.Case testCase)
    {
        // Arrange

        // Act
        var result = DateOnlyRules.IsChronological(testCase.Start, testCase.End, testCase.Inclusion);

        // Assert
        Assert.Equal(testCase.Expected, result);
    }

    [Theory]
    [MemberData(
        nameof(DateOnlyRulesTestData.IsOverlapping.ValidCases),
        MemberType = typeof(DateOnlyRulesTestData.IsOverlapping))]
    public void IsOverlapping_RespectsInclusion(DateOnlyRulesTestData.IsOverlapping.Case testCase)
    {
        // Arrange

        // Act
        var result = DateOnlyRules.IsOverlapping(
            testCase.Start1,
            testCase.End1,
            testCase.Start2,
            testCase.End2,
            testCase.Inclusion);

        // Assert
        Assert.Equal(testCase.Expected, result);
    }

    [Theory]
    [MemberData(
        nameof(DateOnlyRulesTestData.IsOverlapping.EdgeCases),
        MemberType = typeof(DateOnlyRulesTestData.IsOverlapping))]
    public void IsOverlapping_ReturnsFalse_WhenAnyBoundIsNull(DateOnlyRulesTestData.IsOverlapping.Case testCase)
    {
        // Arrange

        // Act
        var result = DateOnlyRules.IsOverlapping(
            testCase.Start1,
            testCase.End1,
            testCase.Start2,
            testCase.End2,
            testCase.Inclusion);

        // Assert
        Assert.Equal(testCase.Expected, result);
    }
}
