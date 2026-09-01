using System.Globalization;
using PineGuard.Codes;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests.FluentValidation;
using F = PineGuard.Testing.Fixtures.StringRulesFixtures;

namespace PineGuard.FluentValidation.UnitTests;

public static class FluentStringDateTimeOffsetExtensionsTestData
{
    // Now as the pinned clock reports it, rendered the way the fixtures render timestamps. That instant is
    // itself in the real past, so the hour after it is future for the pinned clock and past for the system
    // clock — an overload that dropped the supplied provider would fail these groups rather than pass by
    // coincidence.
    private static readonly DateTimeOffset PinnedNow = FixedTimeProvider.Default.GetUtcNow();

    private static string Iso(DateTimeOffset value) => value.ToString("O", CultureInfo.InvariantCulture);

    public static class InPast
    {
        public static TheoryData<FluentCase<string?>> Cases => F.DateTimeOffsetIsInPast.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.DateTimeOffsetIsInPast.NullValue) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be a date/time in the past.", Code: MustCodes.Date.Relative.NotPast)
        });
    }

    public static class InPastOrPresent
    {
        public static TheoryData<FluentCase<string?>> Cases => F.DateTimeOffsetIsInPast.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.DateTimeOffsetIsInPast.NullValue) => new FluentExpected(true),
            nameof(F.DateTimeOffsetIsInPast.FutureDate) => new FluentExpected(false, "Value must be a date/time in the past or present."),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be a date/time in the past or present.")
        });
    }

    public static class InFuture
    {
        public static TheoryData<FluentCase<string?>> Cases => F.DateTimeOffsetIsInFuture.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.DateTimeOffsetIsInFuture.NullValue) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be a date/time in the future.")
        });
    }

    public static class InFutureOrPresent
    {
        public static TheoryData<FluentCase<string?>> Cases => F.DateTimeOffsetIsInPast.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.DateTimeOffsetIsInPast.NullValue) => new FluentExpected(true),
            nameof(F.DateTimeOffsetIsInPast.FutureDate) => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be a date/time in the future or present.")
        });
    }

    public static class InPastPinnedClock
    {
        public static TheoryData<FluentCase<string?>> Cases =>
        [
            new("AnHourAgo", Iso(PinnedNow.AddHours(-1)), new FluentExpected(true)),
            new("Null", null, new FluentExpected(true)),
            new("ThisVeryInstant", Iso(PinnedNow), new FluentExpected(false, "Value must be a date/time in the past.", Code: MustCodes.Date.Relative.NotPast)),
            new("AnHourFromNow", Iso(PinnedNow.AddHours(1)), new FluentExpected(false, "Value must be a date/time in the past.", Code: MustCodes.Date.Relative.NotPast))
        ];
    }

    public static class InPastOrPresentPinnedClock
    {
        public static TheoryData<FluentCase<string?>> Cases =>
        [
            new("AnHourAgo", Iso(PinnedNow.AddHours(-1)), new FluentExpected(true)),
            new("ThisVeryInstant", Iso(PinnedNow), new FluentExpected(true)),
            new("Null", null, new FluentExpected(true)),
            new("AnHourFromNow", Iso(PinnedNow.AddHours(1)), new FluentExpected(false, "Value must be a date/time in the past or present.", Code: MustCodes.Date.Relative.Future))
        ];
    }

    public static class InFuturePinnedClock
    {
        public static TheoryData<FluentCase<string?>> Cases =>
        [
            new("AnHourFromNow", Iso(PinnedNow.AddHours(1)), new FluentExpected(true)),
            new("Null", null, new FluentExpected(true)),
            new("ThisVeryInstant", Iso(PinnedNow), new FluentExpected(false, "Value must be a date/time in the future.", Code: MustCodes.Date.Relative.NotFuture)),
            new("AnHourAgo", Iso(PinnedNow.AddHours(-1)), new FluentExpected(false, "Value must be a date/time in the future.", Code: MustCodes.Date.Relative.NotFuture))
        ];
    }

    public static class InFutureOrPresentPinnedClock
    {
        public static TheoryData<FluentCase<string?>> Cases =>
        [
            new("AnHourFromNow", Iso(PinnedNow.AddHours(1)), new FluentExpected(true)),
            new("ThisVeryInstant", Iso(PinnedNow), new FluentExpected(true)),
            new("Null", null, new FluentExpected(true)),
            new("AnHourAgo", Iso(PinnedNow.AddHours(-1)), new FluentExpected(false, "Value must be a date/time in the future or present.", Code: MustCodes.Date.Relative.Past))
        ];
    }

    public static class IsBetween
    {
        public static TheoryData<FluentCase<(string? value, DateTimeOffset min, DateTimeOffset max, PineGuard.Common.Inclusion inclusion)>> Cases =>
            F.DateTimeOffsetIsBetween.AllScenarios.ToFluentCases(s => s.Name switch
            {
                nameof(F.DateTimeOffsetIsBetween.NullValue) => new FluentExpected(true),
                _ when s.IsValid => new FluentExpected(true),
                _ => new FluentExpected(false, "Value must be a date/time within the expected range.")
            });
    }

    public static class IsNotBetween
    {
        public static TheoryData<FluentCase<(string? value, DateTimeOffset min, DateTimeOffset max, PineGuard.Common.Inclusion inclusion)>> Cases =>
            F.DateTimeOffsetIsNotBetween.AllScenarios.ToFluentCases(s => s.Name switch
            {
                nameof(F.DateTimeOffsetIsNotBetween.NullValue) => new FluentExpected(true),
                nameof(F.DateTimeOffsetIsNotBetween.NotADate) => new FluentExpected(false, "Value must be a date/time not within the expected range."),
                _ when s.IsValid => new FluentExpected(true),
                _ => new FluentExpected(false, "Value must be a date/time not within the expected range.")
            });
    }

    public static class IsWithin
    {
        public static TheoryData<FluentCase<(string? value, DateTimeOffset? reference, TimeSpan window)>> Cases =>
            F.DateTimeOffsetIsWithin.AllScenarios.ToFluentCases(s => s.Name switch
            {
                nameof(F.DateTimeOffsetIsWithin.NullValue) => new FluentExpected(true),
                _ when s.IsValid => new FluentExpected(true),
                _ => new FluentExpected(false, "Value must be a date/time within the expected time window.")
            });
    }

    public static class IsNotWithin
    {
        public static TheoryData<FluentCase<(string? value, DateTimeOffset? reference, TimeSpan window)>> Cases =>
            F.DateTimeOffsetIsWithin.AllScenarios.ToFluentCases(s => s.Name switch
            {
                nameof(F.DateTimeOffsetIsWithin.NullValue) => new FluentExpected(true),
                nameof(F.DateTimeOffsetIsWithin.NotADate) => new FluentExpected(false, "Value must be a date/time not within the expected time window."),
                _ when s.IsValid => new FluentExpected(false, "Value must be a date/time not within the expected time window."),
                _ => new FluentExpected(true)
            });
    }

    public static class IsWithinCalendarMonths
    {
        public static TheoryData<FluentCase<(string? value, DateTimeOffset? reference, int months)>> Cases =>
            F.DateTimeOffsetIsWithinCalendarMonths.AllScenarios.ToFluentCases(s => s.Name switch
            {
                nameof(F.DateTimeOffsetIsWithinCalendarMonths.NullValue) => new FluentExpected(true),
                _ when s.IsValid => new FluentExpected(true),
                _ => new FluentExpected(false, "Value must be a date/time within the expected number of calendar months.")
            });
    }

    public static class IsNotWithinCalendarMonths
    {
        public static TheoryData<FluentCase<(string? value, DateTimeOffset? reference, int months)>> Cases =>
            F.DateTimeOffsetIsWithinCalendarMonths.AllScenarios.ToFluentCases(s => s.Name switch
            {
                nameof(F.DateTimeOffsetIsWithinCalendarMonths.NullValue) => new FluentExpected(true),
                nameof(F.DateTimeOffsetIsWithinCalendarMonths.NotADate) => new FluentExpected(false, "Value must be a date/time not within the expected number of calendar months."),
                _ when s.IsValid => new FluentExpected(false, "Value must be a date/time not within the expected number of calendar months."),
                _ => new FluentExpected(true)
            });
    }
}
