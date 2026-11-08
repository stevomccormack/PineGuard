using PineGuard.Codes;
using PineGuard.Common;
using PineGuard.Testing.UnitTests.FluentValidation;
using PineGuard.Testing.UnitTests.Rules;
using F = PineGuard.Testing.Fixtures.TimeSpanRulesFixtures;

namespace PineGuard.FluentValidation.UnitTests;

public static class FluentTimeSpanExtensionsTestData
{
    // InRange — valid when value is within [min, max] (null skipped by non-nullable model)
    public static class InRange
    {
        public static TheoryData<FluentCase<(TimeSpan value, TimeSpan min, TimeSpan max, Inclusion inclusion)>> Cases =>
            F.IsInRange.AllScenarios.Where(s => s.Inputs.value.HasValue)
            .Select(s => new RuleScenario<(TimeSpan, TimeSpan, TimeSpan, Inclusion)>(s.Name, (s.Inputs.value!.Value, s.Inputs.min, s.Inputs.max, s.Inputs.inclusion), s.IsValid)).ToArray()
            .ToFluentCases(s => s.IsValid ? new FluentExpected(true) : new FluentExpected(false, "Value must be within the expected duration range.", Code: MustCodes.Time.Duration.OutOfRange));
    }

    // OutOfRange — valid when value is NOT within [min, max]
    public static class OutOfRange
    {
        public static TheoryData<FluentCase<(TimeSpan value, TimeSpan min, TimeSpan max, Inclusion inclusion)>> Cases =>
            F.IsInRange.AllScenarios.Where(s => s.Inputs.value.HasValue)
            .Select(s => new RuleScenario<(TimeSpan, TimeSpan, TimeSpan, Inclusion)>(s.Name, (s.Inputs.value!.Value, s.Inputs.min, s.Inputs.max, s.Inputs.inclusion), s.IsValid)).ToArray()
            .ToFluentCases(s => s.IsValid ? new FluentExpected(false, "Value must not be within the expected duration range.") : new FluentExpected(true));
    }

    // GreaterThan — valid when value is greater than other
    public static class GreaterThan
    {
        public static TheoryData<FluentCase<(TimeSpan value, TimeSpan other, Inclusion inclusion)>> Cases =>
            F.IsGreaterThan.AllScenarios.Where(s => s.Inputs is { value: not null, other: not null })
            .Select(s => new RuleScenario<(TimeSpan, TimeSpan, Inclusion)>(s.Name, (s.Inputs.value!.Value, s.Inputs.other!.Value, s.Inputs.inclusion), s.IsValid)).ToArray()
            .ToFluentCases(s => s.IsValid ? new FluentExpected(true) : new FluentExpected(false, "Value must be greater than the threshold."));
    }

    // LessThan — valid when value is less than other
    public static class LessThan
    {
        public static TheoryData<FluentCase<(TimeSpan value, TimeSpan other, Inclusion inclusion)>> Cases =>
            F.IsLessThan.AllScenarios.Where(s => s.Inputs is { value: not null, other: not null })
            .Select(s => new RuleScenario<(TimeSpan, TimeSpan, Inclusion)>(s.Name, (s.Inputs.value!.Value, s.Inputs.other!.Value, s.Inputs.inclusion), s.IsValid)).ToArray()
            .ToFluentCases(s => s.IsValid ? new FluentExpected(true) : new FluentExpected(false, "Value must be less than the threshold."));
    }
}
