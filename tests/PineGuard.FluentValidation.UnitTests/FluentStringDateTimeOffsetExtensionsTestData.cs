using PineGuard.Testing.UnitTests.FluentValidation;
using F = PineGuard.Testing.Fixtures.StringRulesFixtures;

namespace PineGuard.FluentValidation.UnitTests;

public static class FluentStringDateTimeOffsetExtensionsTestData
{
    public static class InPast
    {
        public static TheoryData<FluentCase<string?>> Cases => F.DateTimeOffsetIsInPast.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.DateTimeOffsetIsInPast.NullValue) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be a date/time in the past.")
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
