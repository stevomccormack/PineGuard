using PineGuard.Rules;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Rules;

public sealed class DateTimeRulesTests : BaseUnitTest
{
    [Theory]
    [MemberData(nameof(DateTimeRulesTestData.IsInPast.ValidCases), MemberType = typeof(DateTimeRulesTestData.IsInPast))]
    public void IsInPast_ReturnsExpected_ForValidCases(DateTimeRulesTestData.IsInPast.Case testCase)
    {
        // Act
        var result = DateTimeRules.IsInPast(testCase.Value);

        // Assert
        Assert.Equal(testCase.ExpectedReturn, result);
    }

    [Theory]
    [MemberData(nameof(DateTimeRulesTestData.IsInFuture.ValidCases), MemberType = typeof(DateTimeRulesTestData.IsInFuture))]
    public void IsInFuture_ReturnsExpected_ForValidCases(DateTimeRulesTestData.IsInFuture.Case testCase)
    {
        // Act
        var result = DateTimeRules.IsInFuture(testCase.Value);

        // Assert
        Assert.Equal(testCase.ExpectedReturn, result);
    }

    [Theory]
    [MemberData(nameof(DateTimeRulesTestData.IsBetween.ValidCases), MemberType = typeof(DateTimeRulesTestData.IsBetween))]
    public void IsBetween_RespectsInclusion_ForValidCases(DateTimeRulesTestData.IsBetween.Case testCase)
    {
        // Act
        var result = DateTimeRules.IsBetween(testCase.Value, testCase.Min, testCase.Max, testCase.Inclusion);

        // Assert
        Assert.Equal(testCase.ExpectedReturn, result);
    }

    [Theory]
    [MemberData(nameof(DateTimeRulesTestData.IsBetween.EdgeCases), MemberType = typeof(DateTimeRulesTestData.IsBetween))]
    public void IsBetween_RespectsInclusion_ForEdgeCases(DateTimeRulesTestData.IsBetween.Case testCase)
    {
        // Act
        var result = DateTimeRules.IsBetween(testCase.Value, testCase.Min, testCase.Max, testCase.Inclusion);

        // Assert
        Assert.Equal(testCase.ExpectedReturn, result);
    }

    [Theory]
    [MemberData(nameof(DateTimeRulesTestData.IsChronological.ValidCases), MemberType = typeof(DateTimeRulesTestData.IsChronological))]
    public void IsChronological_RespectsInclusion_ForValidCases(DateTimeRulesTestData.IsChronological.Case testCase)
    {
        // Act
        var result = DateTimeRules.IsChronological(testCase.Value.Start, testCase.Value.End, testCase.Inclusion);

        // Assert
        Assert.Equal(testCase.ExpectedReturn, result);
    }

    [Theory]
    [MemberData(nameof(DateTimeRulesTestData.IsChronological.EdgeCases), MemberType = typeof(DateTimeRulesTestData.IsChronological))]
    public void IsChronological_ReturnsFalse_ForNullsOrExclusiveSameInstant(DateTimeRulesTestData.IsChronological.Case testCase)
    {
        // Act
        var result = DateTimeRules.IsChronological(testCase.Value.Start, testCase.Value.End, testCase.Inclusion);

        // Assert
        Assert.Equal(testCase.ExpectedReturn, result);
    }

    [Theory]
    [MemberData(nameof(DateTimeRulesTestData.IsOverlapping.ValidCases), MemberType = typeof(DateTimeRulesTestData.IsOverlapping))]
    public void IsOverlapping_RespectsInclusion_ForValidCases(DateTimeRulesTestData.IsOverlapping.Case testCase)
    {
        // Act
        var result = DateTimeRules.IsOverlapping(
            testCase.Value.Start1,
            testCase.Value.End1,
            testCase.Value.Start2,
            testCase.Value.End2,
            testCase.Inclusion);

        // Assert
        Assert.Equal(testCase.ExpectedReturn, result);
    }

    [Theory]
    [MemberData(nameof(DateTimeRulesTestData.IsOverlapping.EdgeCases), MemberType = typeof(DateTimeRulesTestData.IsOverlapping))]
    public void IsOverlapping_ReturnsExpected_ForEdgeCases(DateTimeRulesTestData.IsOverlapping.Case testCase)
    {
        // Act
        var result = DateTimeRules.IsOverlapping(
            testCase.Value.Start1,
            testCase.Value.End1,
            testCase.Value.Start2,
            testCase.Value.End2,
            testCase.Inclusion);

        // Assert
        Assert.Equal(testCase.ExpectedReturn, result);
    }

    [Theory]
    [MemberData(nameof(DateTimeRulesTestData.IsBefore.ValidCases), MemberType = typeof(DateTimeRulesTestData.IsBefore))]
    public void IsBefore_RespectsInclusion_ForValidCases(DateTimeRulesTestData.IsBefore.Case testCase)
    {
        // Act
        var result = DateTimeRules.IsBefore(testCase.Value, testCase.Other, testCase.Inclusion);

        // Assert
        Assert.Equal(testCase.ExpectedReturn, result);
    }

    [Theory]
    [MemberData(nameof(DateTimeRulesTestData.IsBefore.EdgeCases), MemberType = typeof(DateTimeRulesTestData.IsBefore))]
    public void IsBefore_RespectsInclusion_ForEdgeCases(DateTimeRulesTestData.IsBefore.Case testCase)
    {
        // Act
        var result = DateTimeRules.IsBefore(testCase.Value, testCase.Other, testCase.Inclusion);

        // Assert
        Assert.Equal(testCase.ExpectedReturn, result);
    }

    [Theory]
    [MemberData(nameof(DateTimeRulesTestData.IsAfter.ValidCases), MemberType = typeof(DateTimeRulesTestData.IsAfter))]
    public void IsAfter_RespectsInclusion_ForValidCases(DateTimeRulesTestData.IsAfter.Case testCase)
    {
        // Act
        var result = DateTimeRules.IsAfter(testCase.Value, testCase.Other, testCase.Inclusion);

        // Assert
        Assert.Equal(testCase.ExpectedReturn, result);
    }

    [Theory]
    [MemberData(nameof(DateTimeRulesTestData.IsAfter.EdgeCases), MemberType = typeof(DateTimeRulesTestData.IsAfter))]
    public void IsAfter_RespectsInclusion_ForEdgeCases(DateTimeRulesTestData.IsAfter.Case testCase)
    {
        // Act
        var result = DateTimeRules.IsAfter(testCase.Value, testCase.Other, testCase.Inclusion);

        // Assert
        Assert.Equal(testCase.ExpectedReturn, result);
    }

    [Theory]
    [MemberData(nameof(DateTimeRulesTestData.IsSame.ValidCases), MemberType = typeof(DateTimeRulesTestData.IsSame))]
    public void IsSame_ComparesUtcNormalized_ForValidCases(DateTimeRulesTestData.IsSame.Case testCase)
    {
        // Act
        var result = DateTimeRules.IsSame(testCase.Value, testCase.Other);

        // Assert
        Assert.Equal(testCase.ExpectedReturn, result);
    }

    [Theory]
    [MemberData(nameof(DateTimeRulesTestData.IsSame.EdgeCases), MemberType = typeof(DateTimeRulesTestData.IsSame))]
    public void IsSame_ComparesUtcNormalized_ForEdgeCases(DateTimeRulesTestData.IsSame.Case testCase)
    {
        // Act
        var result = DateTimeRules.IsSame(testCase.Value, testCase.Other);

        // Assert
        Assert.Equal(testCase.ExpectedReturn, result);
    }

    [Theory]
    [MemberData(nameof(DateTimeRulesTestData.IsWithinDaysFromNow.ValidCases), MemberType = typeof(DateTimeRulesTestData.IsWithinDaysFromNow))]
    public void IsWithinDaysFromNow_ReturnsExpected_ForValidCases(DateTimeRulesTestData.IsWithinDaysFromNow.Case testCase)
    {
        // Act
        var result = DateTimeRules.IsWithinDaysFromNow(testCase.Value, testCase.Days);

        // Assert
        Assert.Equal(testCase.ExpectedReturn, result);
    }

    [Theory]
    [MemberData(nameof(DateTimeRulesTestData.IsWithinDaysFromNow.EdgeCases), MemberType = typeof(DateTimeRulesTestData.IsWithinDaysFromNow))]
    public void IsWithinDaysFromNow_ReturnsExpected_ForEdgeCases(DateTimeRulesTestData.IsWithinDaysFromNow.Case testCase)
    {
        // Act
        var result = DateTimeRules.IsWithinDaysFromNow(testCase.Value, testCase.Days);

        // Assert
        Assert.Equal(testCase.ExpectedReturn, result);
    }

    [Theory]
    [MemberData(nameof(DateTimeRulesTestData.IsWeekday.ValidCases), MemberType = typeof(DateTimeRulesTestData.IsWeekday))]
    public void IsWeekday_ReturnsExpected_ForValidCases(DateTimeRulesTestData.IsWeekday.Case testCase)
    {
        // Act
        var result = DateTimeRules.IsWeekday(testCase.Value);

        // Assert
        Assert.Equal(testCase.ExpectedReturn, result);
    }

    [Theory]
    [MemberData(nameof(DateTimeRulesTestData.IsWeekday.EdgeCases), MemberType = typeof(DateTimeRulesTestData.IsWeekday))]
    public void IsWeekday_ReturnsExpected_ForEdgeCases(DateTimeRulesTestData.IsWeekday.Case testCase)
    {
        // Act
        var result = DateTimeRules.IsWeekday(testCase.Value);

        // Assert
        Assert.Equal(testCase.ExpectedReturn, result);
    }

    [Theory]
    [MemberData(nameof(DateTimeRulesTestData.IsWeekend.ValidCases), MemberType = typeof(DateTimeRulesTestData.IsWeekend))]
    public void IsWeekend_ReturnsExpected_ForValidCases(DateTimeRulesTestData.IsWeekend.Case testCase)
    {
        // Act
        var result = DateTimeRules.IsWeekend(testCase.Value);

        // Assert
        Assert.Equal(testCase.ExpectedReturn, result);
    }

    [Theory]
    [MemberData(nameof(DateTimeRulesTestData.IsWeekend.EdgeCases), MemberType = typeof(DateTimeRulesTestData.IsWeekend))]
    public void IsWeekend_ReturnsExpected_ForEdgeCases(DateTimeRulesTestData.IsWeekend.Case testCase)
    {
        // Act
        var result = DateTimeRules.IsWeekend(testCase.Value);

        // Assert
        Assert.Equal(testCase.ExpectedReturn, result);
    }

    [Theory]
    [MemberData(nameof(DateTimeRulesTestData.IsFirstDayOfMonth.ValidCases), MemberType = typeof(DateTimeRulesTestData.IsFirstDayOfMonth))]
    public void IsFirstDayOfMonth_ReturnsExpected_ForValidCases(DateTimeRulesTestData.IsFirstDayOfMonth.Case testCase)
    {
        // Act
        var result = DateTimeRules.IsFirstDayOfMonth(testCase.Value);

        // Assert
        Assert.Equal(testCase.ExpectedReturn, result);
    }

    [Theory]
    [MemberData(nameof(DateTimeRulesTestData.IsFirstDayOfMonth.EdgeCases), MemberType = typeof(DateTimeRulesTestData.IsFirstDayOfMonth))]
    public void IsFirstDayOfMonth_ReturnsExpected_ForEdgeCases(DateTimeRulesTestData.IsFirstDayOfMonth.Case testCase)
    {
        // Act
        var result = DateTimeRules.IsFirstDayOfMonth(testCase.Value);

        // Assert
        Assert.Equal(testCase.ExpectedReturn, result);
    }

    [Theory]
    [MemberData(nameof(DateTimeRulesTestData.IsLastDayOfMonth.ValidCases), MemberType = typeof(DateTimeRulesTestData.IsLastDayOfMonth))]
    public void IsLastDayOfMonth_ReturnsExpected_ForValidCases(DateTimeRulesTestData.IsLastDayOfMonth.Case testCase)
    {
        // Act
        var result = DateTimeRules.IsLastDayOfMonth(testCase.Value);

        // Assert
        Assert.Equal(testCase.ExpectedReturn, result);
    }

    [Theory]
    [MemberData(nameof(DateTimeRulesTestData.IsLastDayOfMonth.EdgeCases), MemberType = typeof(DateTimeRulesTestData.IsLastDayOfMonth))]
    public void IsLastDayOfMonth_ReturnsExpected_ForEdgeCases(DateTimeRulesTestData.IsLastDayOfMonth.Case testCase)
    {
        // Act
        var result = DateTimeRules.IsLastDayOfMonth(testCase.Value);

        // Assert
        Assert.Equal(testCase.ExpectedReturn, result);
    }

    [Theory]
    [MemberData(nameof(DateTimeRulesTestData.IsSameDay.ValidCases), MemberType = typeof(DateTimeRulesTestData.IsSameDay))]
    public void IsSameDay_ReturnsExpected_ForValidCases(DateTimeRulesTestData.IsSameDay.Case testCase)
    {
        // Act
        var result = DateTimeRules.IsSameDay(testCase.Value, testCase.Other);

        // Assert
        Assert.Equal(testCase.ExpectedReturn, result);
    }

    [Theory]
    [MemberData(nameof(DateTimeRulesTestData.IsSameDay.EdgeCases), MemberType = typeof(DateTimeRulesTestData.IsSameDay))]
    public void IsSameDay_ReturnsExpected_ForEdgeCases(DateTimeRulesTestData.IsSameDay.Case testCase)
    {
        // Act
        var result = DateTimeRules.IsSameDay(testCase.Value, testCase.Other);

        // Assert
        Assert.Equal(testCase.ExpectedReturn, result);
    }

    [Theory]
    [MemberData(nameof(DateTimeRulesTestData.KindChecks.ValidCases), MemberType = typeof(DateTimeRulesTestData.KindChecks))]
    public void KindChecks_ReturnExpected_ForValidCases(DateTimeRulesTestData.KindChecks.Case testCase)
    {
        // Act
        var isUtc = DateTimeRules.IsUtc(testCase.Value);
        var isLocal = DateTimeRules.IsLocal(testCase.Value);
        var isUnspecified = DateTimeRules.IsUnspecified(testCase.Value);

        // Assert
        if (testCase.Kind == DateTimeKind.Utc)
        {
            Assert.Equal(testCase.ExpectedReturn, isUtc);
            Assert.False(isLocal);
            Assert.False(isUnspecified);
        }
        else if (testCase.Kind == DateTimeKind.Local)
        {
            Assert.Equal(testCase.ExpectedReturn, isLocal);
            Assert.False(isUtc);
            Assert.False(isUnspecified);
        }
        else
        {
            Assert.Equal(testCase.ExpectedReturn, isUnspecified);
            Assert.False(isUtc);
            Assert.False(isLocal);
        }
    }

    [Theory]
    [MemberData(nameof(DateTimeRulesTestData.HasExplicitKind.ValidCases), MemberType = typeof(DateTimeRulesTestData.HasExplicitKind))]
    public void HasExplicitKind_ReturnsExpected_ForValidCases(DateTimeRulesTestData.HasExplicitKind.Case testCase)
    {
        // Act
        var result = DateTimeRules.HasExplicitKind(testCase.Value);

        // Assert
        Assert.Equal(testCase.ExpectedReturn, result);
    }

    [Theory]
    [MemberData(nameof(DateTimeRulesTestData.HasExplicitKind.EdgeCases), MemberType = typeof(DateTimeRulesTestData.HasExplicitKind))]
    public void HasExplicitKind_ReturnsExpected_ForEdgeCases(DateTimeRulesTestData.HasExplicitKind.Case testCase)
    {
        // Act
        var result = DateTimeRules.HasExplicitKind(testCase.Value);

        // Assert
        Assert.Equal(testCase.ExpectedReturn, result);
    }
}
