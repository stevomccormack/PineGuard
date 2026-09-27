using PineGuard.Common;
using PineGuard.Testing.UnitTests.GuardClauses;
using PineGuard.Testing.UnitTests.Rules;
using F = PineGuard.Testing.Fixtures.StringRulesFixtures;

namespace PineGuard.GuardClauses.UnitTests;

public static class GuardStringTimeSpanClausesTestData
{
    public static class NotDurationBetween
    {
        public static TheoryData<GuardCase<(string? value, TimeSpan min, TimeSpan max, Inclusion inclusion)>> Cases =>
            F.TimeSpanIsDurationBetween.AllScenarios.ToGuardCases(s => s.Name switch
            {
                nameof(F.TimeSpanIsDurationBetween.NullValue) => new GuardExpected(false, typeof(ArgumentNullException), "value"),
                _ when s.IsValid => new GuardExpected(true),
                _ => new GuardExpected(false, typeof(ArgumentException), "value")
            });
    }

    public static class DurationBetween
    {
        public static TheoryData<GuardCase<(string? value, TimeSpan min, TimeSpan max, Inclusion inclusion)>> Cases =>
        [
            new("out-of-range", ("02:00:00", TimeSpan.FromMinutes(10), TimeSpan.FromMinutes(60), Inclusion.Inclusive), new GuardExpected(true)),
            new("in-range", ("00:30:00", TimeSpan.FromMinutes(10), TimeSpan.FromMinutes(60), Inclusion.Inclusive), new GuardExpected(false, typeof(ArgumentException), "value")),
            new("not-a-duration", ("not-a-duration", TimeSpan.FromMinutes(10), TimeSpan.FromMinutes(60), Inclusion.Inclusive), new GuardExpected(false, typeof(ArgumentException), "value")),
            new("null-value", (null, TimeSpan.FromMinutes(10), TimeSpan.FromMinutes(60), Inclusion.Inclusive), new GuardExpected(false, typeof(ArgumentNullException), "value")),
            new("at-min-exclusive", ("00:10:00", TimeSpan.FromMinutes(10), TimeSpan.FromMinutes(60), Inclusion.Exclusive), new GuardExpected(true)),
            new(nameof(F.TimeSpanIsDurationBetween.AtMinInclusive), F.TimeSpanIsDurationBetween.AtMinInclusive, new GuardExpected(false, typeof(ArgumentException), "value"))
        ];
    }

    public static class ShorterThan
    {
        public static TheoryData<GuardCase<(string? value, TimeSpan threshold, Inclusion inclusion)>> Cases =>
            F.TimeSpanIsLongerThan.AllScenarios
            .Select(s => new RuleScenario<(string?, TimeSpan, Inclusion)>(
                s.Name,
                (s.Inputs.value, s.Inputs.threshold, s.Inputs.inclusion == Inclusion.Inclusive
                    ? Inclusion.Exclusive
                    : Inclusion.Inclusive),
                s.IsValid))
            .ToArray()
            .ToGuardCases(s => s.Name switch
            {
                nameof(F.TimeSpanIsLongerThan.NullValue) => new GuardExpected(false, typeof(ArgumentNullException), "value"),
                _ when s.IsValid => new GuardExpected(true),
                _ => new GuardExpected(false, typeof(ArgumentException), "value")
            });
    }

    public static class LongerThan
    {
        public static TheoryData<GuardCase<(string? value, TimeSpan threshold, Inclusion inclusion)>> Cases =>
            F.TimeSpanIsShorterThan.AllScenarios
            .Select(s => new RuleScenario<(string?, TimeSpan, Inclusion)>(
                s.Name,
                (s.Inputs.value, s.Inputs.threshold, s.Inputs.inclusion == Inclusion.Inclusive
                    ? Inclusion.Exclusive
                    : Inclusion.Inclusive),
                s.IsValid))
            .ToArray()
            .ToGuardCases(s => s.Name switch
            {
                nameof(F.TimeSpanIsShorterThan.NullValue) => new GuardExpected(false, typeof(ArgumentNullException), "value"),
                _ when s.IsValid => new GuardExpected(true),
                _ => new GuardExpected(false, typeof(ArgumentException), "value")
            });
    }
}
