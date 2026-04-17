using PineGuard.Testing.UnitTests.FluentValidation;
using F = PineGuard.Testing.Fixtures.StringRulesFixtures;

namespace PineGuard.FluentValidation.UnitTests;

public static class FluentStringTimeSpanExtensionsTestData
{
    public static class DurationBetween
    {
        public static TheoryData<FluentCase<(string? value, TimeSpan min, TimeSpan max, PineGuard.Common.Inclusion inclusion)>> Cases =>
            F.TimeSpanIsDurationBetween.AllScenarios.ToFluentCases(s => s.Name switch
            {
                nameof(F.TimeSpanIsDurationBetween.NullValue) => new FluentExpected(true),
                _ when s.IsValid => new FluentExpected(true),
                _ => new FluentExpected(false, "Value must be a duration within the expected range.")
            });
    }

    public static class NotDurationBetween
    {
        public static TheoryData<FluentCase<(string? value, TimeSpan min, TimeSpan max, PineGuard.Common.Inclusion inclusion)>> Cases =>
            F.TimeSpanIsDurationBetween.AllScenarios.ToFluentCases(s => s.Name switch
            {
                nameof(F.TimeSpanIsDurationBetween.NullValue) => new FluentExpected(true),
                nameof(F.TimeSpanIsDurationBetween.AtMinExclusive) => new FluentExpected(true),
                nameof(F.TimeSpanIsDurationBetween.NotADuration) => new FluentExpected(false, "Value must be a duration not within the expected range."),
                _ when s.IsValid => new FluentExpected(false, "Value must be a duration not within the expected range."),
                _ => new FluentExpected(true)
            });
    }

    public static class GreaterThan
    {
        public static TheoryData<FluentCase<(string? value, TimeSpan threshold, PineGuard.Common.Inclusion inclusion)>> Cases =>
            F.TimeSpanIsGreaterThan.AllScenarios.ToFluentCases(s => s.Name switch
            {
                nameof(F.TimeSpanIsGreaterThan.NullValue) => new FluentExpected(true),
                _ when s.IsValid => new FluentExpected(true),
                _ => new FluentExpected(false, "Value must be a duration greater than the threshold.")
            });
    }

    public static class LessThan
    {
        public static TheoryData<FluentCase<(string? value, TimeSpan threshold, PineGuard.Common.Inclusion inclusion)>> Cases =>
            F.TimeSpanIsLessThan.AllScenarios.ToFluentCases(s => s.Name switch
            {
                nameof(F.TimeSpanIsLessThan.NullValue) => new FluentExpected(true),
                _ when s.IsValid => new FluentExpected(true),
                _ => new FluentExpected(false, "Value must be a duration less than the threshold.")
            });
    }
}
