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

    // LongerThan — valid when value is longer than other
    public static class LongerThan
    {
        public static TheoryData<FluentCase<(TimeSpan value, TimeSpan other, Inclusion inclusion)>> Cases =>
            F.IsLongerThan.AllScenarios.Where(s => s.Inputs is { value: not null, other: not null })
            .Select(s => new RuleScenario<(TimeSpan, TimeSpan, Inclusion)>(s.Name, (s.Inputs.value!.Value, s.Inputs.other!.Value, s.Inputs.inclusion), s.IsValid)).ToArray()
            .ToFluentCases(s => s.IsValid ? new FluentExpected(true) : new FluentExpected(false, "Value must be longer than the other."));
    }

    // ShorterThan — valid when value is shorter than other
    public static class ShorterThan
    {
        public static TheoryData<FluentCase<(TimeSpan value, TimeSpan other, Inclusion inclusion)>> Cases =>
            F.IsShorterThan.AllScenarios.Where(s => s.Inputs is { value: not null, other: not null })
            .Select(s => new RuleScenario<(TimeSpan, TimeSpan, Inclusion)>(s.Name, (s.Inputs.value!.Value, s.Inputs.other!.Value, s.Inputs.inclusion), s.IsValid)).ToArray()
            .ToFluentCases(s => s.IsValid ? new FluentExpected(true) : new FluentExpected(false, "Value must be shorter than the other."));
    }
}
