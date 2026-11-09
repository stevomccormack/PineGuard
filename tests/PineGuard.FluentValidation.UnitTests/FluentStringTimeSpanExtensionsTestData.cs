using PineGuard.Codes;
using PineGuard.Testing.UnitTests.FluentValidation;
using F = PineGuard.Testing.Fixtures.StringRulesFixtures;

namespace PineGuard.FluentValidation.UnitTests;

public static class FluentStringTimeSpanExtensionsTestData
{
    public static class InRange
    {
        public static TheoryData<FluentCase<(string? value, TimeSpan min, TimeSpan max, PineGuard.Common.Inclusion inclusion)>> Cases =>
            F.TimeSpanIsInRange.AllScenarios.ToFluentCases(s => s.Name switch
            {
                nameof(F.TimeSpanIsInRange.NullValue) => new FluentExpected(true),
                _ when s.IsValid => new FluentExpected(true),
                _ => new FluentExpected(false, "Value must be a duration within the expected range.", Code: MustCodes.Time.Duration.OutOfRange)
            });
    }

    public static class OutOfRange
    {
        public static TheoryData<FluentCase<(string? value, TimeSpan min, TimeSpan max, PineGuard.Common.Inclusion inclusion)>> Cases =>
            F.TimeSpanIsInRange.AllScenarios.ToFluentCases(s => s.Name switch
            {
                nameof(F.TimeSpanIsInRange.NullValue) => new FluentExpected(true),
                nameof(F.TimeSpanIsInRange.AtMinExclusive) => new FluentExpected(true),
                nameof(F.TimeSpanIsInRange.NotADuration) => new FluentExpected(false, "Value must be a duration not within the expected range."),
                _ when s.IsValid => new FluentExpected(false, "Value must be a duration not within the expected range."),
                _ => new FluentExpected(true)
            });
    }

    public static class LongerThan
    {
        public static TheoryData<FluentCase<(string? value, TimeSpan other, PineGuard.Common.Inclusion inclusion)>> Cases =>
            F.TimeSpanIsLongerThan.AllScenarios.ToFluentCases(s => s.Name switch
            {
                nameof(F.TimeSpanIsLongerThan.NullValue) => new FluentExpected(true),
                _ when s.IsValid => new FluentExpected(true),
                _ => new FluentExpected(false, "Value must be a duration longer than the other.")
            });
    }

    public static class ShorterThan
    {
        public static TheoryData<FluentCase<(string? value, TimeSpan other, PineGuard.Common.Inclusion inclusion)>> Cases =>
            F.TimeSpanIsShorterThan.AllScenarios.ToFluentCases(s => s.Name switch
            {
                nameof(F.TimeSpanIsShorterThan.NullValue) => new FluentExpected(true),
                _ when s.IsValid => new FluentExpected(true),
                _ => new FluentExpected(false, "Value must be a duration shorter than the other.")
            });
    }
}
