using PineGuard.Common;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests.MustClauses;
using Xunit.Abstractions;

namespace PineGuard.MustClauses.UnitTests;

public sealed class MustStringDateOnlyClausesTests(ITestOutputHelper output) : BaseMustUnitTest(output)
{
    [Theory]
    [MemberData(nameof(MustStringDateOnlyClausesTestData.PastDateOnly.Cases), MemberType = typeof(MustStringDateOnlyClausesTestData.PastDateOnly))]
    public void PastDateOnly_BehavesAsExpected(MustCase<string?> tc)
    {
        // Act
        var result = Must.Be.PastDateOnly(tc.Value, timeProvider: FixedTimeProvider.Default, paramName: "value");

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustStringDateOnlyClausesTestData.PastDateOnlyAcrossTimeZones.Cases), MemberType = typeof(MustStringDateOnlyClausesTestData.PastDateOnlyAcrossTimeZones))]
    public void PastDateOnlyAcrossTimeZones_BehavesAsExpected(MustCase<(string? value, TimeProvider timeProvider)> tc)
    {
        // Arrange
        var (value, timeProvider) = tc.Value;

        // Act
        var result = Must.Be.PastDateOnly(value, timeProvider: timeProvider, paramName: "value");

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustStringDateOnlyClausesTestData.PastDateOnlyReferenceProjection.Cases), MemberType = typeof(MustStringDateOnlyClausesTestData.PastDateOnlyReferenceProjection))]
    public void PastDateOnlyReferenceProjection_BehavesAsExpected(MustCase<(string? value, TimeProvider timeProvider)> tc)
    {
        // Arrange
        var (value, timeProvider) = tc.Value;

        // Act
        var result = Must.Be.PastDateOnly(value, timeProvider: timeProvider, paramName: "value");

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustStringDateOnlyClausesTestData.PastDateOnlyClockReads.Cases), MemberType = typeof(MustStringDateOnlyClausesTestData.PastDateOnlyClockReads))]
    public void PastDateOnly_ReadsTheClockAsExpected(MustStringDateOnlyClausesTestData.PastDateOnlyClockReads.ClockReadCase tc)
    {
        // Arrange
        var provider = new CountingTimeProvider(FixedTimeProvider.Default.GetUtcNow());

        // Act
        Must.Be.PastDateOnly(tc.Value, timeProvider: provider, paramName: "value");

        // Assert
        Assert.Equal(tc.Expected, provider.UtcNowCallCount);
    }

    [Theory]
    [MemberData(nameof(MustStringDateOnlyClausesTestData.PastOrPresentDateOnly.Cases), MemberType = typeof(MustStringDateOnlyClausesTestData.PastOrPresentDateOnly))]
    public void PastOrPresentDateOnly_BehavesAsExpected(MustCase<string?> tc)
    {
        // Act
        var result = Must.Be.PastOrPresentDateOnly(tc.Value, timeProvider: FixedTimeProvider.Default, paramName: "value");

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustStringDateOnlyClausesTestData.PastOrPresentDateOnlyAcrossTimeZones.Cases), MemberType = typeof(MustStringDateOnlyClausesTestData.PastOrPresentDateOnlyAcrossTimeZones))]
    public void PastOrPresentDateOnlyAcrossTimeZones_BehavesAsExpected(MustCase<(string? value, TimeProvider timeProvider)> tc)
    {
        // Arrange
        var (value, timeProvider) = tc.Value;

        // Act
        var result = Must.Be.PastOrPresentDateOnly(value, timeProvider: timeProvider, paramName: "value");

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustStringDateOnlyClausesTestData.PastOrPresentDateOnlyReferenceProjection.Cases), MemberType = typeof(MustStringDateOnlyClausesTestData.PastOrPresentDateOnlyReferenceProjection))]
    public void PastOrPresentDateOnlyReferenceProjection_BehavesAsExpected(MustCase<(string? value, TimeProvider timeProvider)> tc)
    {
        // Arrange
        var (value, timeProvider) = tc.Value;

        // Act
        var result = Must.Be.PastOrPresentDateOnly(value, timeProvider: timeProvider, paramName: "value");

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustStringDateOnlyClausesTestData.PastOrPresentDateOnlyClockReads.Cases), MemberType = typeof(MustStringDateOnlyClausesTestData.PastOrPresentDateOnlyClockReads))]
    public void PastOrPresentDateOnly_ReadsTheClockAsExpected(MustStringDateOnlyClausesTestData.PastOrPresentDateOnlyClockReads.ClockReadCase tc)
    {
        // Arrange
        var provider = new CountingTimeProvider(FixedTimeProvider.Default.GetUtcNow());

        // Act
        Must.Be.PastOrPresentDateOnly(tc.Value, timeProvider: provider, paramName: "value");

        // Assert
        Assert.Equal(tc.Expected, provider.UtcNowCallCount);
    }

    [Theory]
    [MemberData(nameof(MustStringDateOnlyClausesTestData.FutureDateOnly.Cases), MemberType = typeof(MustStringDateOnlyClausesTestData.FutureDateOnly))]
    public void FutureDateOnly_BehavesAsExpected(MustCase<string?> tc)
    {
        // Act
        var result = Must.Be.FutureDateOnly(tc.Value, timeProvider: FixedTimeProvider.Default, paramName: "value");

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustStringDateOnlyClausesTestData.FutureDateOnlyAcrossTimeZones.Cases), MemberType = typeof(MustStringDateOnlyClausesTestData.FutureDateOnlyAcrossTimeZones))]
    public void FutureDateOnlyAcrossTimeZones_BehavesAsExpected(MustCase<(string? value, TimeProvider timeProvider)> tc)
    {
        // Arrange
        var (value, timeProvider) = tc.Value;

        // Act
        var result = Must.Be.FutureDateOnly(value, timeProvider: timeProvider, paramName: "value");

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustStringDateOnlyClausesTestData.FutureDateOnlyReferenceProjection.Cases), MemberType = typeof(MustStringDateOnlyClausesTestData.FutureDateOnlyReferenceProjection))]
    public void FutureDateOnlyReferenceProjection_BehavesAsExpected(MustCase<(string? value, TimeProvider timeProvider)> tc)
    {
        // Arrange
        var (value, timeProvider) = tc.Value;

        // Act
        var result = Must.Be.FutureDateOnly(value, timeProvider: timeProvider, paramName: "value");

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustStringDateOnlyClausesTestData.FutureDateOnlyClockReads.Cases), MemberType = typeof(MustStringDateOnlyClausesTestData.FutureDateOnlyClockReads))]
    public void FutureDateOnly_ReadsTheClockAsExpected(MustStringDateOnlyClausesTestData.FutureDateOnlyClockReads.ClockReadCase tc)
    {
        // Arrange
        var provider = new CountingTimeProvider(FixedTimeProvider.Default.GetUtcNow());

        // Act
        Must.Be.FutureDateOnly(tc.Value, timeProvider: provider, paramName: "value");

        // Assert
        Assert.Equal(tc.Expected, provider.UtcNowCallCount);
    }

    [Theory]
    [MemberData(nameof(MustStringDateOnlyClausesTestData.FutureOrPresentDateOnly.Cases), MemberType = typeof(MustStringDateOnlyClausesTestData.FutureOrPresentDateOnly))]
    public void FutureOrPresentDateOnly_BehavesAsExpected(MustCase<string?> tc)
    {
        // Act
        var result = Must.Be.FutureOrPresentDateOnly(tc.Value, timeProvider: FixedTimeProvider.Default, paramName: "value");

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustStringDateOnlyClausesTestData.FutureOrPresentDateOnlyAcrossTimeZones.Cases), MemberType = typeof(MustStringDateOnlyClausesTestData.FutureOrPresentDateOnlyAcrossTimeZones))]
    public void FutureOrPresentDateOnlyAcrossTimeZones_BehavesAsExpected(MustCase<(string? value, TimeProvider timeProvider)> tc)
    {
        // Arrange
        var (value, timeProvider) = tc.Value;

        // Act
        var result = Must.Be.FutureOrPresentDateOnly(value, timeProvider: timeProvider, paramName: "value");

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustStringDateOnlyClausesTestData.FutureOrPresentDateOnlyReferenceProjection.Cases), MemberType = typeof(MustStringDateOnlyClausesTestData.FutureOrPresentDateOnlyReferenceProjection))]
    public void FutureOrPresentDateOnlyReferenceProjection_BehavesAsExpected(MustCase<(string? value, TimeProvider timeProvider)> tc)
    {
        // Arrange
        var (value, timeProvider) = tc.Value;

        // Act
        var result = Must.Be.FutureOrPresentDateOnly(value, timeProvider: timeProvider, paramName: "value");

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustStringDateOnlyClausesTestData.FutureOrPresentDateOnlyClockReads.Cases), MemberType = typeof(MustStringDateOnlyClausesTestData.FutureOrPresentDateOnlyClockReads))]
    public void FutureOrPresentDateOnly_ReadsTheClockAsExpected(MustStringDateOnlyClausesTestData.FutureOrPresentDateOnlyClockReads.ClockReadCase tc)
    {
        // Arrange
        var provider = new CountingTimeProvider(FixedTimeProvider.Default.GetUtcNow());

        // Act
        Must.Be.FutureOrPresentDateOnly(tc.Value, timeProvider: provider, paramName: "value");

        // Assert
        Assert.Equal(tc.Expected, provider.UtcNowCallCount);
    }

    [Theory]
    [MemberData(nameof(MustStringDateOnlyClausesTestData.InRangeDateOnly.Cases), MemberType = typeof(MustStringDateOnlyClausesTestData.InRangeDateOnly))]
    public void InRangeDateOnly_BehavesAsExpected(MustCase<(string? value, DateOnly min, DateOnly max, Inclusion inclusion)> tc)
    {
        var value = tc.Value.value;
        var result = Must.Be.InRangeDateOnly(value, tc.Value.min, tc.Value.max, tc.Value.inclusion);
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustStringDateOnlyClausesTestData.OutOfRangeDateOnly.Cases), MemberType = typeof(MustStringDateOnlyClausesTestData.OutOfRangeDateOnly))]
    public void OutOfRangeDateOnly_BehavesAsExpected(MustCase<(string? value, DateOnly min, DateOnly max, Inclusion inclusion)> tc)
    {
        var value = tc.Value.value;
        var result = Must.Be.OutOfRangeDateOnly(value, tc.Value.min, tc.Value.max, tc.Value.inclusion);
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustStringDateOnlyClausesTestData.WithinDaysDateOnly.Cases), MemberType = typeof(MustStringDateOnlyClausesTestData.WithinDaysDateOnly))]
    public void WithinDaysDateOnly_BehavesAsExpected(MustCase<(string? value, DateOnly? reference, int days)> tc)
    {
        var value = tc.Value.value;
        var result = Must.Be.WithinDaysDateOnly(value, tc.Value.reference, tc.Value.days);
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustStringDateOnlyClausesTestData.NotWithinDaysDateOnly.Cases), MemberType = typeof(MustStringDateOnlyClausesTestData.NotWithinDaysDateOnly))]
    public void NotWithinDaysDateOnly_BehavesAsExpected(MustCase<(string? value, DateOnly? reference, int days)> tc)
    {
        var value = tc.Value.value;
        var result = Must.Be.NotWithinDaysDateOnly(value, tc.Value.reference, tc.Value.days);
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustStringDateOnlyClausesTestData.WithinCalendarMonthsDateOnly.Cases), MemberType = typeof(MustStringDateOnlyClausesTestData.WithinCalendarMonthsDateOnly))]
    public void WithinCalendarMonthsDateOnly_BehavesAsExpected(MustCase<(string? value, DateOnly? reference, int months)> tc)
    {
        var value = tc.Value.value;
        var result = Must.Be.WithinCalendarMonthsDateOnly(value, tc.Value.reference, tc.Value.months);
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustStringDateOnlyClausesTestData.NotWithinCalendarMonthsDateOnly.Cases), MemberType = typeof(MustStringDateOnlyClausesTestData.NotWithinCalendarMonthsDateOnly))]
    public void NotWithinCalendarMonthsDateOnly_BehavesAsExpected(MustCase<(string? value, DateOnly? reference, int months)> tc)
    {
        var value = tc.Value.value;
        var result = Must.Be.NotWithinCalendarMonthsDateOnly(value, tc.Value.reference, tc.Value.months);
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustStringDateOnlyClausesTestData.BeforeDateOnly.Cases), MemberType = typeof(MustStringDateOnlyClausesTestData.BeforeDateOnly))]
    public void BeforeDateOnly_BehavesAsExpected(MustCase<(string? value, DateOnly other)> tc)
    {
        var value = tc.Value.value;
        var result = Must.Be.BeforeDateOnly(value, tc.Value.other);
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustStringDateOnlyClausesTestData.NotBeforeDateOnly.Cases), MemberType = typeof(MustStringDateOnlyClausesTestData.NotBeforeDateOnly))]
    public void NotBeforeDateOnly_BehavesAsExpected(MustCase<(string? value, DateOnly other)> tc)
    {
        var value = tc.Value.value;
        var result = Must.Be.NotBeforeDateOnly(value, tc.Value.other);
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustStringDateOnlyClausesTestData.OnOrBeforeDateOnly.Cases), MemberType = typeof(MustStringDateOnlyClausesTestData.OnOrBeforeDateOnly))]
    public void OnOrBeforeDateOnly_BehavesAsExpected(MustCase<(string? value, DateOnly other)> tc)
    {
        var value = tc.Value.value;
        var result = Must.Be.OnOrBeforeDateOnly(value, tc.Value.other);
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustStringDateOnlyClausesTestData.NotOnOrBeforeDateOnly.Cases), MemberType = typeof(MustStringDateOnlyClausesTestData.NotOnOrBeforeDateOnly))]
    public void NotOnOrBeforeDateOnly_BehavesAsExpected(MustCase<(string? value, DateOnly other)> tc)
    {
        var value = tc.Value.value;
        var result = Must.Be.NotOnOrBeforeDateOnly(value, tc.Value.other);
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustStringDateOnlyClausesTestData.AfterDateOnly.Cases), MemberType = typeof(MustStringDateOnlyClausesTestData.AfterDateOnly))]
    public void AfterDateOnly_BehavesAsExpected(MustCase<(string? value, DateOnly other)> tc)
    {
        var value = tc.Value.value;
        var result = Must.Be.AfterDateOnly(value, tc.Value.other);
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustStringDateOnlyClausesTestData.NotAfterDateOnly.Cases), MemberType = typeof(MustStringDateOnlyClausesTestData.NotAfterDateOnly))]
    public void NotAfterDateOnly_BehavesAsExpected(MustCase<(string? value, DateOnly other)> tc)
    {
        var value = tc.Value.value;
        var result = Must.Be.NotAfterDateOnly(value, tc.Value.other);
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustStringDateOnlyClausesTestData.OnOrAfterDateOnly.Cases), MemberType = typeof(MustStringDateOnlyClausesTestData.OnOrAfterDateOnly))]
    public void OnOrAfterDateOnly_BehavesAsExpected(MustCase<(string? value, DateOnly other)> tc)
    {
        var value = tc.Value.value;
        var result = Must.Be.OnOrAfterDateOnly(value, tc.Value.other);
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustStringDateOnlyClausesTestData.NotOnOrAfterDateOnly.Cases), MemberType = typeof(MustStringDateOnlyClausesTestData.NotOnOrAfterDateOnly))]
    public void NotOnOrAfterDateOnly_BehavesAsExpected(MustCase<(string? value, DateOnly other)> tc)
    {
        var value = tc.Value.value;
        var result = Must.Be.NotOnOrAfterDateOnly(value, tc.Value.other);
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustStringDateOnlyClausesTestData.SameDateOnly.Cases), MemberType = typeof(MustStringDateOnlyClausesTestData.SameDateOnly))]
    public void SameDateOnly_BehavesAsExpected(MustCase<(string? value, DateOnly other)> tc)
    {
        var value = tc.Value.value;
        var result = Must.Be.SameDateOnly(value, tc.Value.other);
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustStringDateOnlyClausesTestData.NotSameDateOnly.Cases), MemberType = typeof(MustStringDateOnlyClausesTestData.NotSameDateOnly))]
    public void NotSameDateOnly_BehavesAsExpected(MustCase<(string? value, DateOnly other)> tc)
    {
        var value = tc.Value.value;
        var result = Must.Be.NotSameDateOnly(value, tc.Value.other);
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustStringDateOnlyClausesTestData.ChronologicalDateOnly.Cases), MemberType = typeof(MustStringDateOnlyClausesTestData.ChronologicalDateOnly))]
    public void ChronologicalDateOnly_BehavesAsExpected(MustCase<(string? start, string? end)> tc)
    {
        var start = tc.Value.start;
        var result = Must.Be.ChronologicalDateOnly(start, tc.Value.end);
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustStringDateOnlyClausesTestData.NotChronologicalDateOnly.Cases), MemberType = typeof(MustStringDateOnlyClausesTestData.NotChronologicalDateOnly))]
    public void NotChronologicalDateOnly_BehavesAsExpected(MustCase<(string? start, string? end)> tc)
    {
        var start = tc.Value.start;
        var result = Must.Be.NotChronologicalDateOnly(start, tc.Value.end);
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustStringDateOnlyClausesTestData.OverlappingDateOnly.Cases), MemberType = typeof(MustStringDateOnlyClausesTestData.OverlappingDateOnly))]
    public void OverlappingDateOnly_BehavesAsExpected(MustCase<(string? start1, string? end1, string? start2, string? end2)> tc)
    {
        var start1 = tc.Value.start1;
        var result = Must.Be.OverlappingDateOnly(start1, tc.Value.end1, tc.Value.start2, tc.Value.end2);
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustStringDateOnlyClausesTestData.NotOverlappingDateOnly.Cases), MemberType = typeof(MustStringDateOnlyClausesTestData.NotOverlappingDateOnly))]
    public void NotOverlappingDateOnly_BehavesAsExpected(MustCase<(string? start1, string? end1, string? start2, string? end2)> tc)
    {
        var start1 = tc.Value.start1;
        var result = Must.Be.NotOverlappingDateOnly(start1, tc.Value.end1, tc.Value.start2, tc.Value.end2);
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustStringDateOnlyClausesTestData.MinimumAge.ValidCases), MemberType = typeof(MustStringDateOnlyClausesTestData.MinimumAge))]
    [MemberData(nameof(MustStringDateOnlyClausesTestData.MinimumAge.InvalidCases), MemberType = typeof(MustStringDateOnlyClausesTestData.MinimumAge))]
    public void MinimumAge_BehavesAsExpected(MustCase<(string? value, int years)> tc)
    {
        // Arrange
        var (value, years) = tc.Value;

        // Act
        var result = Must.Be.MinimumAgeDateOnly(value, years, timeProvider: FixedTimeProvider.Default, paramName: "value");

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustStringDateOnlyClausesTestData.MinimumAgeReferenceProjection.Cases), MemberType = typeof(MustStringDateOnlyClausesTestData.MinimumAgeReferenceProjection))]
    public void MinimumAgeReferenceProjection_BehavesAsExpected(MustCase<(string? value, int years, TimeProvider timeProvider)> tc)
    {
        // Arrange
        var (value, years, timeProvider) = tc.Value;

        // Act
        var result = Must.Be.MinimumAgeDateOnly(value, years, timeProvider: timeProvider, paramName: "value");

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustStringDateOnlyClausesTestData.MinimumAgeClockReads.Cases), MemberType = typeof(MustStringDateOnlyClausesTestData.MinimumAgeClockReads))]
    public void MinimumAge_ReadsTheClockAsExpected(MustStringDateOnlyClausesTestData.MinimumAgeClockReads.ClockReadCase tc)
    {
        // Arrange
        var provider = new CountingTimeProvider(FixedTimeProvider.Default.GetUtcNow());

        // Act
        Must.Be.MinimumAgeDateOnly(tc.Value, 18, timeProvider: provider, paramName: "value");

        // Assert
        Assert.Equal(tc.Expected, provider.UtcNowCallCount);
    }

    [Theory]
    [MemberData(nameof(MustStringDateOnlyClausesTestData.MinimumAgeOnLeapDay.Cases), MemberType = typeof(MustStringDateOnlyClausesTestData.MinimumAgeOnLeapDay))]
    public void MinimumAge_LeapDayBirthDate_MaturesOnTheFirstOfMarch(MustCase<(string? value, int years, DateTimeOffset utcNow)> tc)
    {
        // Arrange
        var (value, years, utcNow) = tc.Value;

        // Act
        var result = Must.Be.MinimumAgeDateOnly(value, years, timeProvider: new FixedTimeProvider(utcNow), paramName: "value");

        // Assert
        AssertResult(tc, result);
    }
}
