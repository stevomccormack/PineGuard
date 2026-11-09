using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;
using PineGuard.Utils;

namespace PineGuard.Core.UnitTests.Utils;

public sealed class DateTimeUtilityTests : BaseUnitTest
{
    [Theory]
    [MemberData(nameof(DateTimeUtilityTestData.GetUtcNow.ValidCases), MemberType = typeof(DateTimeUtilityTestData.GetUtcNow))]
    [MemberData(nameof(DateTimeUtilityTestData.GetUtcNow.EdgeCases), MemberType = typeof(DateTimeUtilityTestData.GetUtcNow))]
    public void GetUtcNow_ReturnsExpected(DateTimeUtilityTestData.GetUtcNow.ValidCase testCase)
    {
        // Arrange
        var before = DateTimeOffset.UtcNow;

        // Act
        var result = DateTimeUtility.GetUtcNow(testCase.Value);

        // Assert
        if (testCase.Expected is { } expected)
        {
            Assert.Equal(expected, result);
            return;
        }

        Assert.InRange(result, before.AddSeconds(-1), DateTimeOffset.UtcNow.AddSeconds(1));
    }

    [Theory]
    [MemberData(nameof(DateTimeUtilityTestData.GetLocalNow.ValidCases), MemberType = typeof(DateTimeUtilityTestData.GetLocalNow))]
    [MemberData(nameof(DateTimeUtilityTestData.GetLocalNow.EdgeCases), MemberType = typeof(DateTimeUtilityTestData.GetLocalNow))]
    public void GetLocalNow_ReturnsExpected(DateTimeUtilityTestData.GetLocalNow.ValidCase testCase)
    {
        // Arrange
        var before = DateTimeOffset.UtcNow;

        // Act
        var result = DateTimeUtility.GetLocalNow(testCase.Value);

        // Assert
        if (testCase.Expected is { } expected)
        {
            Assert.Equal(expected, result);
            return;
        }

        Assert.InRange(result.ToUniversalTime(), before.AddSeconds(-1), DateTimeOffset.UtcNow.AddSeconds(1));
    }

    [Theory]
    [MemberData(nameof(DateTimeUtilityTestData.TryGetTodayDate.ValidCases), MemberType = typeof(DateTimeUtilityTestData.TryGetTodayDate))]
    [MemberData(nameof(DateTimeUtilityTestData.TryGetTodayDate.EdgeCases), MemberType = typeof(DateTimeUtilityTestData.TryGetTodayDate))]
    public void TryGetTodayDate_ReturnsExpected(DateTimeUtilityTestData.TryGetTodayDate.ValidCase testCase)
    {
        // Act
        var ok = DateTimeUtility.TryGetTodayDate(testCase.Value, out var today);

        // Assert
        Assert.Equal(testCase.Expected, ok);
        Assert.Equal(testCase.ExpectedOutValue, today);
    }

    [Theory]
    [MemberData(nameof(DateTimeUtilityTestData.TryGetTodayDateNullProvider.Cases), MemberType = typeof(DateTimeUtilityTestData.TryGetTodayDateNullProvider))]
    public void TryGetTodayDate_NullProviderUsesUtcDate(string _)
    {
        // Arrange
        // Bracket the real system clock read on both sides of the call, so a midnight rollover during
        // the test cannot cause a false failure. The expectation is the UTC calendar date, never the
        // host's zone (D-1); the CI job that runs this under TZ=Pacific/Auckland proves the independence.
        var beforeUtc = DateTimeOffset.UtcNow;
        var beforeExpected = beforeUtc.UtcDateTime.Date;

        // Act
        var ok = DateTimeUtility.TryGetTodayDate(null, out var today);

        // Assert
        var afterUtc = DateTimeOffset.UtcNow;
        var afterExpected = afterUtc.UtcDateTime.Date;

        Assert.True(ok);
        Assert.True(today == beforeExpected || today == afterExpected);
    }

    [Theory]
    [MemberData(nameof(DateTimeUtilityTestData.OneReadBehavior.Cases), MemberType = typeof(DateTimeUtilityTestData.OneReadBehavior))]
    public void TryGetTodayDate_ReadsTheClockExactlyOnce(string _)
    {
        // Arrange
        var instant = FixedTimeProvider.Default.GetUtcNow();
        var zone = FixedTimeProvider.Default.LocalTimeZone;
        var expectedToday = TimeZoneInfo.ConvertTime(instant, zone).Date;
        var provider = new CountingTimeProvider(instant, zone);

        // Act
        var ok = DateTimeUtility.TryGetTodayDate(provider, out var today);

        // Assert
        Assert.True(ok);
        Assert.Equal(expectedToday, today);
        Assert.Equal(1, provider.UtcNowCallCount);
    }

#if NET8_0_OR_GREATER
    [Theory]
    [MemberData(nameof(DateTimeUtilityTestData.TryGetAgeDateOnly.ValidCases), MemberType = typeof(DateTimeUtilityTestData.TryGetAgeDateOnly))]
    [MemberData(nameof(DateTimeUtilityTestData.TryGetAgeDateOnly.EdgeCases), MemberType = typeof(DateTimeUtilityTestData.TryGetAgeDateOnly))]
    public void TryGetAge_DateOnly_ReturnsExpected(DateTimeUtilityTestData.TryGetAgeDateOnly.ValidCase testCase)
    {
        // Arrange
        var (birthDate, asOf) = testCase.Value;

        // Act
        var ok = DateTimeUtility.TryGetAge(birthDate, asOf, out var years);

        // Assert
        Assert.Equal(testCase.Expected, ok);
        Assert.Equal(testCase.ExpectedOutValue, years);
    }
#endif

    [Theory]
    [MemberData(nameof(DateTimeUtilityTestData.TryGetAgeDateTime.ValidCases), MemberType = typeof(DateTimeUtilityTestData.TryGetAgeDateTime))]
    [MemberData(nameof(DateTimeUtilityTestData.TryGetAgeDateTime.EdgeCases), MemberType = typeof(DateTimeUtilityTestData.TryGetAgeDateTime))]
    public void TryGetAge_DateTime_ReturnsExpected(DateTimeUtilityTestData.TryGetAgeDateTime.ValidCase testCase)
    {
        // Arrange
        var (birthDate, asOf) = testCase.Value;

        // Act
        var ok = DateTimeUtility.TryGetAge(birthDate, asOf, out var years);

        // Assert
        Assert.Equal(testCase.Expected, ok);
        Assert.Equal(testCase.ExpectedOutValue, years);
    }

    [Theory]
    [MemberData(nameof(DateTimeUtilityTestData.ToUtc.ValidCases), MemberType = typeof(DateTimeUtilityTestData.ToUtc))]
    [MemberData(nameof(DateTimeUtilityTestData.ToUtc.EdgeCases), MemberType = typeof(DateTimeUtilityTestData.ToUtc))]
    public void ToUtc_ReturnsExpected(DateTimeUtilityTestData.ToUtc.ValidCase testCase)
    {
        // Act
        var result = DateTimeUtility.ToUtc(testCase.Value);

        // Assert
        Assert.Equal(testCase.Expected, result);

        if (!result.HasValue) return;
        Assert.Equal(DateTimeKind.Utc, result.Value.Kind);

        if (testCase.Value is { Kind: DateTimeKind.Unspecified })
        {
            Assert.Equal(testCase.Value.Value.Ticks, result.Value.Ticks);
        }
    }

    [Theory]
    [MemberData(nameof(DateTimeUtilityTestData.Diff.ValidCases), MemberType = typeof(DateTimeUtilityTestData.Diff))]
    [MemberData(nameof(DateTimeUtilityTestData.Diff.EdgeCases), MemberType = typeof(DateTimeUtilityTestData.Diff))]
    public void Diff_ReturnsExpected(DateTimeUtilityTestData.Diff.ValidCase testCase)
    {
        // Act
        var result = DateTimeUtility.Diff(testCase.Value.Start, testCase.Value.End);

        // Assert
        Assert.Equal(testCase.Expected, result);
    }

    [Theory]
    [MemberData(nameof(DateTimeUtilityTestData.TryTruncateToPrecisionUtc.ValidCases), MemberType = typeof(DateTimeUtilityTestData.TryTruncateToPrecisionUtc))]
    [MemberData(nameof(DateTimeUtilityTestData.TryTruncateToPrecisionUtc.EdgeCases), MemberType = typeof(DateTimeUtilityTestData.TryTruncateToPrecisionUtc))]
    public void TryTruncateToPrecisionUtc_ReturnsExpected(DateTimeUtilityTestData.TryTruncateToPrecisionUtc.ValidCase testCase)
    {
        // Act
        var ok = DateTimeUtility.TryTruncateToPrecisionUtc(testCase.Value.Value, testCase.Value.Precision, out var truncated);

        // Assert
        Assert.Equal(testCase.Expected, ok);
        Assert.Equal(testCase.ExpectedOutValue, truncated);
    }

    [Theory]
    [MemberData(nameof(DateTimeUtilityTestData.TryTruncateToPrecisionUtcOffset.ValidCases), MemberType = typeof(DateTimeUtilityTestData.TryTruncateToPrecisionUtcOffset))]
    [MemberData(nameof(DateTimeUtilityTestData.TryTruncateToPrecisionUtcOffset.EdgeCases), MemberType = typeof(DateTimeUtilityTestData.TryTruncateToPrecisionUtcOffset))]
    public void TryTruncateToPrecisionUtcOffset_ReturnsExpected(DateTimeUtilityTestData.TryTruncateToPrecisionUtcOffset.ValidCase testCase)
    {
        // Act
        var ok = DateTimeUtility.TryTruncateToPrecisionUtc(testCase.Value.Value, testCase.Value.Precision, out var truncated);

        // Assert
        Assert.Equal(testCase.Expected, ok);
        Assert.Equal(testCase.ExpectedOutValue, truncated);
    }

    [Theory]
    [MemberData(nameof(DateTimeUtilityTestData.TryTruncateToPrecision.ValidCases), MemberType = typeof(DateTimeUtilityTestData.TryTruncateToPrecision))]
    [MemberData(nameof(DateTimeUtilityTestData.TryTruncateToPrecision.EdgeCases), MemberType = typeof(DateTimeUtilityTestData.TryTruncateToPrecision))]
    public void TryTruncateToPrecision_ReturnsExpected(DateTimeUtilityTestData.TryTruncateToPrecision.ValidCase testCase)
    {
        // Act
        var ok = DateTimeUtility.TryTruncateToPrecision(testCase.Value.Value, testCase.Value.Precision, out var truncated);

        // Assert
        Assert.Equal(testCase.Expected, ok);
        Assert.Equal(testCase.ExpectedOutValue, truncated);
    }
}
