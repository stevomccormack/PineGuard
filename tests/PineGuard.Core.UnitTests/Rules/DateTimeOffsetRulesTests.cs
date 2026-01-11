using PineGuard.Rules;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Rules;

public sealed class DateTimeOffsetRulesTests : BaseUnitTest
{
    [Theory]
    [MemberData(nameof(DateTimeOffsetRulesTestData.IsInPast.ValidCases), MemberType = typeof(DateTimeOffsetRulesTestData.IsInPast))]
    public void IsInPast_ReturnsExpected_ForValidCases(DateTimeOffsetRulesTestData.IsInPast.Case testCase)
    {
        // Act
        var result = DateTimeOffsetRules.IsInPast(testCase.Value);

        // Assert
        Assert.Equal(testCase.ExpectedReturn, result);
    }

    [Theory]
    [MemberData(nameof(DateTimeOffsetRulesTestData.IsInPast.EdgeCases), MemberType = typeof(DateTimeOffsetRulesTestData.IsInPast))]
    public void IsInPast_ReturnsExpected_ForEdgeCases(DateTimeOffsetRulesTestData.IsInPast.Case testCase)
    {
        // Act
        var result = DateTimeOffsetRules.IsInPast(testCase.Value);

        // Assert
        Assert.Equal(testCase.ExpectedReturn, result);
    }

    [Theory]
    [MemberData(nameof(DateTimeOffsetRulesTestData.IsInFuture.ValidCases), MemberType = typeof(DateTimeOffsetRulesTestData.IsInFuture))]
    public void IsInFuture_ReturnsExpected_ForValidCases(DateTimeOffsetRulesTestData.IsInFuture.Case testCase)
    {
        // Act
        var result = DateTimeOffsetRules.IsInFuture(testCase.Value);

        // Assert
        Assert.Equal(testCase.ExpectedReturn, result);
    }

    [Theory]
    [MemberData(nameof(DateTimeOffsetRulesTestData.IsInFuture.EdgeCases), MemberType = typeof(DateTimeOffsetRulesTestData.IsInFuture))]
    public void IsInFuture_ReturnsExpected_ForEdgeCases(DateTimeOffsetRulesTestData.IsInFuture.Case testCase)
    {
        // Act
        var result = DateTimeOffsetRules.IsInFuture(testCase.Value);

        // Assert
        Assert.Equal(testCase.ExpectedReturn, result);
    }

    [Theory]
    [MemberData(nameof(DateTimeOffsetRulesTestData.IsBetween.ValidCases), MemberType = typeof(DateTimeOffsetRulesTestData.IsBetween))]
    public void IsBetween_RespectsInclusion_ForValidCases(DateTimeOffsetRulesTestData.IsBetween.Case testCase)
    {
        // Act
        var result = DateTimeOffsetRules.IsBetween(testCase.Value, testCase.Min, testCase.Max, testCase.Inclusion);

        // Assert
        Assert.Equal(testCase.ExpectedReturn, result);
    }

    [Theory]
    [MemberData(nameof(DateTimeOffsetRulesTestData.IsBetween.EdgeCases), MemberType = typeof(DateTimeOffsetRulesTestData.IsBetween))]
    public void IsBetween_RespectsInclusion_ForEdgeCases(DateTimeOffsetRulesTestData.IsBetween.Case testCase)
    {
        // Act
        var result = DateTimeOffsetRules.IsBetween(testCase.Value, testCase.Min, testCase.Max, testCase.Inclusion);

        // Assert
        Assert.Equal(testCase.ExpectedReturn, result);
    }

    [Theory]
    [MemberData(nameof(DateTimeOffsetRulesTestData.IsBefore.ValidCases), MemberType = typeof(DateTimeOffsetRulesTestData.IsBefore))]
    public void IsBefore_RespectsInclusion_ForValidCases(DateTimeOffsetRulesTestData.IsBefore.Case testCase)
    {
        // Act
        var result = DateTimeOffsetRules.IsBefore(testCase.Value, testCase.Other, testCase.Inclusion);

        // Assert
        Assert.Equal(testCase.ExpectedReturn, result);
    }

    [Theory]
    [MemberData(nameof(DateTimeOffsetRulesTestData.IsBefore.EdgeCases), MemberType = typeof(DateTimeOffsetRulesTestData.IsBefore))]
    public void IsBefore_RespectsInclusion_ForEdgeCases(DateTimeOffsetRulesTestData.IsBefore.Case testCase)
    {
        // Act
        var result = DateTimeOffsetRules.IsBefore(testCase.Value, testCase.Other, testCase.Inclusion);

        // Assert
        Assert.Equal(testCase.ExpectedReturn, result);
    }

    [Theory]
    [MemberData(nameof(DateTimeOffsetRulesTestData.IsAfter.ValidCases), MemberType = typeof(DateTimeOffsetRulesTestData.IsAfter))]
    public void IsAfter_RespectsInclusion_ForValidCases(DateTimeOffsetRulesTestData.IsAfter.Case testCase)
    {
        // Act
        var result = DateTimeOffsetRules.IsAfter(testCase.Value, testCase.Other, testCase.Inclusion);

        // Assert
        Assert.Equal(testCase.ExpectedReturn, result);
    }

    [Theory]
    [MemberData(nameof(DateTimeOffsetRulesTestData.IsAfter.EdgeCases), MemberType = typeof(DateTimeOffsetRulesTestData.IsAfter))]
    public void IsAfter_RespectsInclusion_ForEdgeCases(DateTimeOffsetRulesTestData.IsAfter.Case testCase)
    {
        // Act
        var result = DateTimeOffsetRules.IsAfter(testCase.Value, testCase.Other, testCase.Inclusion);

        // Assert
        Assert.Equal(testCase.ExpectedReturn, result);
    }

    [Theory]
    [MemberData(nameof(DateTimeOffsetRulesTestData.IsSame.ValidCases), MemberType = typeof(DateTimeOffsetRulesTestData.IsSame))]
    public void IsSame_ComparesValues_ForValidCases(DateTimeOffsetRulesTestData.IsSame.Case testCase)
    {
        // Act
        var result = DateTimeOffsetRules.IsSame(testCase.Value, testCase.Other);

        // Assert
        Assert.Equal(testCase.ExpectedReturn, result);
    }

    [Theory]
    [MemberData(nameof(DateTimeOffsetRulesTestData.IsSame.EdgeCases), MemberType = typeof(DateTimeOffsetRulesTestData.IsSame))]
    public void IsSame_ComparesValues_ForEdgeCases(DateTimeOffsetRulesTestData.IsSame.Case testCase)
    {
        // Act
        var result = DateTimeOffsetRules.IsSame(testCase.Value, testCase.Other);

        // Assert
        Assert.Equal(testCase.ExpectedReturn, result);
    }

    [Theory]
    [MemberData(nameof(DateTimeOffsetRulesTestData.IsChronological.ValidCases), MemberType = typeof(DateTimeOffsetRulesTestData.IsChronological))]
    public void IsChronological_RespectsInclusion_ForValidCases(DateTimeOffsetRulesTestData.IsChronological.Case testCase)
    {
        // Act
        var result = DateTimeOffsetRules.IsChronological(testCase.Value.Start, testCase.Value.End, testCase.Inclusion);

        // Assert
        Assert.Equal(testCase.ExpectedReturn, result);
    }

    [Theory]
    [MemberData(nameof(DateTimeOffsetRulesTestData.IsChronological.EdgeCases), MemberType = typeof(DateTimeOffsetRulesTestData.IsChronological))]
    public void IsChronological_ReturnsExpected_ForEdgeCases(DateTimeOffsetRulesTestData.IsChronological.Case testCase)
    {
        // Act
        var result = DateTimeOffsetRules.IsChronological(testCase.Value.Start, testCase.Value.End, testCase.Inclusion);

        // Assert
        Assert.Equal(testCase.ExpectedReturn, result);
    }

    [Theory]
    [MemberData(nameof(DateTimeOffsetRulesTestData.IsOverlapping.ValidCases), MemberType = typeof(DateTimeOffsetRulesTestData.IsOverlapping))]
    public void IsOverlapping_RespectsInclusion_ForValidCases(DateTimeOffsetRulesTestData.IsOverlapping.Case testCase)
    {
        // Act
        var result = DateTimeOffsetRules.IsOverlapping(
            testCase.Value.Start1,
            testCase.Value.End1,
            testCase.Value.Start2,
            testCase.Value.End2,
            testCase.Inclusion);

        // Assert
        Assert.Equal(testCase.ExpectedReturn, result);
    }

    [Theory]
    [MemberData(nameof(DateTimeOffsetRulesTestData.IsOverlapping.EdgeCases), MemberType = typeof(DateTimeOffsetRulesTestData.IsOverlapping))]
    public void IsOverlapping_ReturnsExpected_ForEdgeCases(DateTimeOffsetRulesTestData.IsOverlapping.Case testCase)
    {
        // Act
        var result = DateTimeOffsetRules.IsOverlapping(
            testCase.Value.Start1,
            testCase.Value.End1,
            testCase.Value.Start2,
            testCase.Value.End2,
            testCase.Inclusion);

        // Assert
        Assert.Equal(testCase.ExpectedReturn, result);
    }
}
