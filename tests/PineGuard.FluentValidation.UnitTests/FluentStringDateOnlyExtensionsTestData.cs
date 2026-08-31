using System.Globalization;
using PineGuard.Codes;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests.FluentValidation;
using F = PineGuard.Testing.Fixtures.StringRulesFixtures;

namespace PineGuard.FluentValidation.UnitTests;

public static class FluentStringDateOnlyExtensionsTestData
{
    // Today as the pinned clock reports it, rendered the way the fixtures render dates. That day is itself
    // in the real past, so the day after it is future for the pinned clock and past for the machine's — an
    // overload that dropped the supplied provider would fail these groups rather than pass by coincidence.
    private static readonly DateOnly PinnedToday = DateOnly.FromDateTime(FixedTimeProvider.Default.GetUtcNow().UtcDateTime);

    private static string Iso(DateOnly value) => value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

    public static class InPast
    {
        public static TheoryData<FluentCase<string?>> Cases => F.DateOnlyIsInPast.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.DateOnlyIsInPast.NullValue) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be a date in the past.", Code: MustCodes.Date.Relative.NotPast)
        });
    }

    public static class InPastOrPresent
    {
        public static TheoryData<FluentCase<string?>> Cases => F.DateOnlyIsInPast.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.DateOnlyIsInPast.NullValue) => new FluentExpected(true),
            nameof(F.DateOnlyIsInPast.FutureDate) => new FluentExpected(false, "Value must be a date in the past or present."),
            nameof(F.DateOnlyIsInPast.NotADate) => new FluentExpected(false, "Value must be a date in the past or present."),
            _ => new FluentExpected(true)
        });
    }

    public static class InFuture
    {
        public static TheoryData<FluentCase<string?>> Cases => F.DateOnlyIsInFuture.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.DateOnlyIsInFuture.NullValue) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be a date in the future.")
        });
    }

    public static class InFutureOrPresent
    {
        public static TheoryData<FluentCase<string?>> Cases => F.DateOnlyIsInPast.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.DateOnlyIsInPast.NullValue) => new FluentExpected(true),
            nameof(F.DateOnlyIsInPast.PastDate) => new FluentExpected(false, "Value must be a date in the future or present."),
            nameof(F.DateOnlyIsInPast.NotADate) => new FluentExpected(false, "Value must be a date in the future or present."),
            _ => new FluentExpected(true)
        });
    }

    public static class InPastPinnedClock
    {
        public static TheoryData<FluentCase<string?>> Cases =>
        [
            new("Yesterday", Iso(PinnedToday.AddDays(-1)), new FluentExpected(true)),
            new("Null", null, new FluentExpected(true)),
            new("Today", Iso(PinnedToday), new FluentExpected(false, "Value must be a date in the past.", Code: MustCodes.Date.Relative.NotPast)),
            new("Tomorrow", Iso(PinnedToday.AddDays(1)), new FluentExpected(false, "Value must be a date in the past.", Code: MustCodes.Date.Relative.NotPast))
        ];
    }

    public static class InPastOrPresentPinnedClock
    {
        public static TheoryData<FluentCase<string?>> Cases =>
        [
            new("Yesterday", Iso(PinnedToday.AddDays(-1)), new FluentExpected(true)),
            new("Today", Iso(PinnedToday), new FluentExpected(true)),
            new("Null", null, new FluentExpected(true)),
            new("Tomorrow", Iso(PinnedToday.AddDays(1)), new FluentExpected(false, "Value must be a date in the past or present.", Code: MustCodes.Date.Relative.Future))
        ];
    }

    public static class InFuturePinnedClock
    {
        public static TheoryData<FluentCase<string?>> Cases =>
        [
            new("Tomorrow", Iso(PinnedToday.AddDays(1)), new FluentExpected(true)),
            new("Null", null, new FluentExpected(true)),
            new("Today", Iso(PinnedToday), new FluentExpected(false, "Value must be a date in the future.", Code: MustCodes.Date.Relative.NotFuture)),
            new("Yesterday", Iso(PinnedToday.AddDays(-1)), new FluentExpected(false, "Value must be a date in the future.", Code: MustCodes.Date.Relative.NotFuture))
        ];
    }

    public static class InFutureOrPresentPinnedClock
    {
        public static TheoryData<FluentCase<string?>> Cases =>
        [
            new("Tomorrow", Iso(PinnedToday.AddDays(1)), new FluentExpected(true)),
            new("Today", Iso(PinnedToday), new FluentExpected(true)),
            new("Null", null, new FluentExpected(true)),
            new("Yesterday", Iso(PinnedToday.AddDays(-1)), new FluentExpected(false, "Value must be a date in the future or present.", Code: MustCodes.Date.Relative.Past))
        ];
    }

    public static class IsBetween
    {
        public static TheoryData<FluentCase<(string? value, DateOnly min, DateOnly max, PineGuard.Common.Inclusion inclusion)>> Cases =>
            F.DateOnlyIsBetween.AllScenarios.ToFluentCases(s => s.Name switch
            {
                nameof(F.DateOnlyIsBetween.NullValue) => new FluentExpected(true),
                _ when s.IsValid => new FluentExpected(true),
                _ => new FluentExpected(false, "Value must be a date within the expected range.")
            });
    }

    public static class IsNotBetween
    {
        public static TheoryData<FluentCase<(string? value, DateOnly min, DateOnly max, PineGuard.Common.Inclusion inclusion)>> Cases =>
            F.DateOnlyIsNotBetween.AllScenarios.ToFluentCases(s => s.Name switch
            {
                nameof(F.DateOnlyIsNotBetween.NullValue) => new FluentExpected(true),
                nameof(F.DateOnlyIsNotBetween.NotADate) => new FluentExpected(false, "Value must be a date not within the expected range."),
                _ when s.IsValid => new FluentExpected(true),
                _ => new FluentExpected(false, "Value must be a date not within the expected range.")
            });
    }

    public static class IsWithinDays
    {
        public static TheoryData<FluentCase<(string? value, DateOnly? reference, int days)>> Cases =>
            F.DateOnlyIsWithinDays.AllScenarios.ToFluentCases(s => s.Name switch
            {
                nameof(F.DateOnlyIsWithinDays.NullValue) => new FluentExpected(true),
                _ when s.IsValid => new FluentExpected(true),
                _ => new FluentExpected(false, "Value must be a date within the expected number of days.")
            });
    }

    public static class IsNotWithinDays
    {
        public static TheoryData<FluentCase<(string? value, DateOnly? reference, int days)>> Cases =>
            F.DateOnlyIsWithinDays.AllScenarios.ToFluentCases(s => s.Name switch
            {
                nameof(F.DateOnlyIsWithinDays.NullValue) => new FluentExpected(true),
                nameof(F.DateOnlyIsWithinDays.NotADate) => new FluentExpected(false, "Value must be a date not within the expected number of days."),
                _ when s.IsValid => new FluentExpected(false, "Value must be a date not within the expected number of days."),
                _ => new FluentExpected(true)
            });
    }

    public static class IsWithinCalendarMonths
    {
        public static TheoryData<FluentCase<(string? value, DateOnly? reference, int months)>> Cases =>
            F.DateOnlyIsWithinCalendarMonths.AllScenarios.ToFluentCases(s => s.Name switch
            {
                nameof(F.DateOnlyIsWithinCalendarMonths.NullValue) => new FluentExpected(true),
                _ when s.IsValid => new FluentExpected(true),
                _ => new FluentExpected(false, "Value must be a date within the expected number of calendar months.")
            });
    }

    public static class IsNotWithinCalendarMonths
    {
        public static TheoryData<FluentCase<(string? value, DateOnly? reference, int months)>> Cases =>
            F.DateOnlyIsWithinCalendarMonths.AllScenarios.ToFluentCases(s => s.Name switch
            {
                nameof(F.DateOnlyIsWithinCalendarMonths.NullValue) => new FluentExpected(true),
                nameof(F.DateOnlyIsWithinCalendarMonths.NotADate) => new FluentExpected(false, "Value must be a date not within the expected number of calendar months."),
                _ when s.IsValid => new FluentExpected(false, "Value must be a date not within the expected number of calendar months."),
                _ => new FluentExpected(true)
            });
    }

    public static class IsBefore
    {
        public static TheoryData<FluentCase<(string? value, DateOnly other)>> Cases =>
            F.DateOnlyIsBefore.AllScenarios.ToFluentCases(s => s.Name switch
            {
                nameof(F.DateOnlyIsBefore.NullValue) => new FluentExpected(true),
                _ when s.IsValid => new FluentExpected(true),
                _ => new FluentExpected(false, "Value must be a date before the specified date.")
            });
    }

    public static class IsNotBefore
    {
        public static TheoryData<FluentCase<(string? value, DateOnly other)>> Cases =>
            F.DateOnlyIsBefore.AllScenarios.ToFluentCases(s => s.Name switch
            {
                nameof(F.DateOnlyIsBefore.NullValue) => new FluentExpected(true),
                nameof(F.DateOnlyIsBefore.NotADate) => new FluentExpected(false, "Value must not be a date before the specified date."),
                _ when s.IsValid => new FluentExpected(false, "Value must not be a date before the specified date."),
                _ => new FluentExpected(true)
            });
    }

    public static class IsOnOrBefore
    {
        public static TheoryData<FluentCase<(string? value, DateOnly other)>> Cases =>
            F.DateOnlyIsOnOrBefore.AllScenarios.ToFluentCases(s => s.Name switch
            {
                nameof(F.DateOnlyIsOnOrBefore.NullValue) => new FluentExpected(true),
                _ when s.IsValid => new FluentExpected(true),
                _ => new FluentExpected(false, "Value must be a date on or before the specified date.")
            });
    }

    public static class IsNotOnOrBefore
    {
        public static TheoryData<FluentCase<(string? value, DateOnly other)>> Cases =>
            F.DateOnlyIsOnOrBefore.AllScenarios.ToFluentCases(s => s.Name switch
            {
                nameof(F.DateOnlyIsOnOrBefore.NullValue) => new FluentExpected(true),
                nameof(F.DateOnlyIsOnOrBefore.NotADate) => new FluentExpected(false, "Value must not be a date on or before the specified date."),
                _ when s.IsValid => new FluentExpected(false, "Value must not be a date on or before the specified date."),
                _ => new FluentExpected(true)
            });
    }

    public static class IsAfter
    {
        public static TheoryData<FluentCase<(string? value, DateOnly other)>> Cases =>
            F.DateOnlyIsAfter.AllScenarios.ToFluentCases(s => s.Name switch
            {
                nameof(F.DateOnlyIsAfter.NullValue) => new FluentExpected(true),
                _ when s.IsValid => new FluentExpected(true),
                _ => new FluentExpected(false, "Value must be a date after the specified date.")
            });
    }

    public static class IsNotAfter
    {
        public static TheoryData<FluentCase<(string? value, DateOnly other)>> Cases =>
            F.DateOnlyIsAfter.AllScenarios.ToFluentCases(s => s.Name switch
            {
                nameof(F.DateOnlyIsAfter.NullValue) => new FluentExpected(true),
                nameof(F.DateOnlyIsAfter.NotADate) => new FluentExpected(false, "Value must not be a date after the specified date."),
                _ when s.IsValid => new FluentExpected(false, "Value must not be a date after the specified date."),
                _ => new FluentExpected(true)
            });
    }

    public static class IsOnOrAfter
    {
        public static TheoryData<FluentCase<(string? value, DateOnly other)>> Cases =>
            F.DateOnlyIsOnOrAfter.AllScenarios.ToFluentCases(s => s.Name switch
            {
                nameof(F.DateOnlyIsOnOrAfter.NullValue) => new FluentExpected(true),
                _ when s.IsValid => new FluentExpected(true),
                _ => new FluentExpected(false, "Value must be a date on or after the specified date.")
            });
    }

    public static class IsNotOnOrAfter
    {
        public static TheoryData<FluentCase<(string? value, DateOnly other)>> Cases =>
            F.DateOnlyIsOnOrAfter.AllScenarios.ToFluentCases(s => s.Name switch
            {
                nameof(F.DateOnlyIsOnOrAfter.NullValue) => new FluentExpected(true),
                nameof(F.DateOnlyIsOnOrAfter.NotADate) => new FluentExpected(false, "Value must not be a date on or after the specified date."),
                _ when s.IsValid => new FluentExpected(false, "Value must not be a date on or after the specified date."),
                _ => new FluentExpected(true)
            });
    }

    public static class IsSame
    {
        public static TheoryData<FluentCase<(string? value, DateOnly other)>> Cases =>
            F.DateOnlyIsSame.AllScenarios.ToFluentCases(s => s.Name switch
            {
                nameof(F.DateOnlyIsSame.NullValue) => new FluentExpected(true),
                _ when s.IsValid => new FluentExpected(true),
                _ => new FluentExpected(false, "Value must be the same date.")
            });
    }

    public static class IsNotSame
    {
        public static TheoryData<FluentCase<(string? value, DateOnly other)>> Cases =>
            F.DateOnlyIsSame.AllScenarios.ToFluentCases(s => s.Name switch
            {
                nameof(F.DateOnlyIsSame.NullValue) => new FluentExpected(true),
                nameof(F.DateOnlyIsSame.NotADate) => new FluentExpected(false, "Value must not be the same date."),
                _ when s.IsValid => new FluentExpected(false, "Value must not be the same date."),
                _ => new FluentExpected(true)
            });
    }

    public static class IsChronological
    {
        public static TheoryData<FluentCase<(string? start, string? end, PineGuard.Common.Inclusion inclusion)>> Cases =>
            F.DateOnlyIsChronological.AllScenarios.ToFluentCases(s => s.Name switch
            {
                nameof(F.DateOnlyIsChronological.NullStart) => new FluentExpected(true),
                _ when s.IsValid => new FluentExpected(true),
                _ => new FluentExpected(false, "Value must be chronological.")
            });
    }

    public static class IsNotChronological
    {
        public static TheoryData<FluentCase<(string? start, string? end, PineGuard.Common.Inclusion inclusion)>> Cases =>
            F.DateOnlyIsChronological.AllScenarios.ToFluentCases(s => s.Name switch
            {
                nameof(F.DateOnlyIsChronological.NullStart) => new FluentExpected(true),
                nameof(F.DateOnlyIsChronological.NotADate) => new FluentExpected(false, "Value must not be chronological."),
                _ when s.IsValid => new FluentExpected(false, "Value must not be chronological."),
                _ => new FluentExpected(true)
            });
    }

    public static class IsOverlapping
    {
        public static TheoryData<FluentCase<(string? start1, string? end1, string? start2, string? end2, PineGuard.Common.Inclusion inclusion)>> Cases =>
            F.DateOnlyIsOverlapping.AllScenarios.ToFluentCases(s => s.Name switch
            {
                nameof(F.DateOnlyIsOverlapping.NullStart1) => new FluentExpected(true),
                _ when s.IsValid => new FluentExpected(true),
                _ => new FluentExpected(false, "Value must be overlapping.")
            });
    }

    public static class IsNotOverlapping
    {
        public static TheoryData<FluentCase<(string? start1, string? end1, string? start2, string? end2, PineGuard.Common.Inclusion inclusion)>> Cases =>
            F.DateOnlyIsOverlapping.AllScenarios.ToFluentCases(s => s.Name switch
            {
                nameof(F.DateOnlyIsOverlapping.NullStart1) => new FluentExpected(true),
                nameof(F.DateOnlyIsOverlapping.NotADate) => new FluentExpected(false, "Value must not be overlapping."),
                _ when s.IsValid => new FluentExpected(false, "Value must not be overlapping."),
                _ => new FluentExpected(true)
            });
    }

    public static class MinimumAge
    {
        public static TheoryData<FluentCase<(string? value, int years)>> Cases => F.DateOnlyHasMinimumAge.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.DateOnlyHasMinimumAge.NullValue) => new FluentExpected(true),
            nameof(F.DateOnlyHasMinimumAge.NegativeYears) => new FluentExpected(false, "years requires a non-negative number of years.", Code: MustCodes.Date.Age.BelowMinimum),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must meet the expected minimum age.", Code: MustCodes.Date.Age.BelowMinimum)
        });
    }

    // A 29-February birth date has no anniversary in a non-leap year, so each case pins its own clock:
    // here the boundary moves and the birth date stays put, which the shared provider cannot express.
    public static class MinimumAgeOnLeapDay
    {
        public static TheoryData<FluentCase<(string? value, int years, DateTimeOffset utcNow)>> Cases =>
        [
            new("TwentyEighthOfFebruaryInANonLeapYear", (LeapDayBirth, 18, Noon(2026, 02, 28)), new FluentExpected(false, "Value must meet the expected minimum age.", Code: MustCodes.Date.Age.BelowMinimum)),
            new("FirstOfMarchInANonLeapYear", (LeapDayBirth, 18, Noon(2026, 03, 01)), new FluentExpected(true)),
            new("TwentyNinthOfFebruaryInALeapYear", (LeapDayBirth, 20, Noon(2028, 02, 29)), new FluentExpected(true))
        ];
    }

    private const string LeapDayBirth = "2008-02-29";

    private static DateTimeOffset Noon(int year, int month, int day) => new(year, month, day, 12, 0, 0, TimeSpan.Zero);
}
