using PineGuard.Common;
using PineGuard.Testing.UnitTests.FluentValidation;
using PineGuard.Testing.UnitTests.Rules;
using F = PineGuard.Testing.Fixtures.TimeSpanRulesFixtures;

namespace PineGuard.FluentValidation.UnitTests;

public static class FluentTimeSpanExtensionsTestData
{
    // DurationBetween — valid when value is within [min, max] (null skipped by non-nullable model)
    public static class DurationBetween
    {
        public static TheoryData<FluentCase<(TimeSpan value, TimeSpan min, TimeSpan max, Inclusion inclusion)>> Cases =>
            F.IsDurationBetween.AllScenarios.Where(s => s.Inputs.value.HasValue)
            .Select(s => new RuleScenario<(TimeSpan, TimeSpan, TimeSpan, Inclusion)>(s.Name, (s.Inputs.value!.Value, s.Inputs.min, s.Inputs.max, s.Inputs.inclusion), s.IsValid)).ToArray()
            .ToFluentCases(s => s.IsValid ? new FluentExpected(true) : new FluentExpected(false, "Value must be within the expected duration range."));
    }

    // NotDurationBetween — valid when value is NOT within [min, max]
    public static class NotDurationBetween
    {
        public static TheoryData<FluentCase<(TimeSpan value, TimeSpan min, TimeSpan max, Inclusion inclusion)>> Cases =>
            F.IsDurationBetween.AllScenarios.Where(s => s.Inputs.value.HasValue)
            .Select(s => new RuleScenario<(TimeSpan, TimeSpan, TimeSpan, Inclusion)>(s.Name, (s.Inputs.value!.Value, s.Inputs.min, s.Inputs.max, s.Inputs.inclusion), s.IsValid)).ToArray()
            .ToFluentCases(s => s.IsValid ? new FluentExpected(false, "Value must not be within the expected duration range.") : new FluentExpected(true));
    }

    // GreaterThan — valid when value is greater than threshold
    public static class GreaterThan
    {
        public static TheoryData<FluentCase<(TimeSpan value, TimeSpan threshold, Inclusion inclusion)>> Cases =>
            F.IsGreaterThan.AllScenarios.Where(s => s.Inputs is { value: not null, threshold: not null })
            .Select(s => new RuleScenario<(TimeSpan, TimeSpan, Inclusion)>(s.Name, (s.Inputs.value!.Value, s.Inputs.threshold!.Value, s.Inputs.inclusion), s.IsValid)).ToArray()
            .ToFluentCases(s => s.IsValid ? new FluentExpected(true) : new FluentExpected(false, "Value must be greater than the threshold."));
    }

    // LessThan — valid when value is less than threshold
    public static class LessThan
    {
        public static TheoryData<FluentCase<(TimeSpan value, TimeSpan threshold, Inclusion inclusion)>> Cases =>
            F.IsLessThan.AllScenarios.Where(s => s.Inputs is { value: not null, threshold: not null })
            .Select(s => new RuleScenario<(TimeSpan, TimeSpan, Inclusion)>(s.Name, (s.Inputs.value!.Value, s.Inputs.threshold!.Value, s.Inputs.inclusion), s.IsValid)).ToArray()
            .ToFluentCases(s => s.IsValid ? new FluentExpected(true) : new FluentExpected(false, "Value must be less than the threshold."));
    }
}
