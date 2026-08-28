using PineGuard.Codes;
using PineGuard.Testing.UnitTests.FluentValidation;
using F = PineGuard.Testing.Fixtures.StringRulesFixtures;

namespace PineGuard.FluentValidation.UnitTests;

public static class FluentStringDateOnlyExtensionsTestData
{
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
}
