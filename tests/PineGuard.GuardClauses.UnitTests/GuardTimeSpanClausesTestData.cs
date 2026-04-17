using PineGuard.Common;
using PineGuard.Testing.UnitTests.GuardClauses;
using PineGuard.Testing.UnitTests.Rules;
using F = PineGuard.Testing.Fixtures.TimeSpanRulesFixtures;

namespace PineGuard.GuardClauses.UnitTests;

public static class GuardTimeSpanClausesTestData
{
    // Guard.Against.NotDurationBetween — throws when value is NOT in [min, max]
    public static class NotDurationBetween
    {
        public static TheoryData<GuardCase<(TimeSpan value, TimeSpan min, TimeSpan max, Inclusion inclusion)>> ValidCases =>
            F.IsDurationBetween.AllValid.Select(s => new RuleScenario<(TimeSpan, TimeSpan, TimeSpan, Inclusion)>(s.Name, (s.Inputs.value!.Value, s.Inputs.min, s.Inputs.max, s.Inputs.inclusion), s.IsValid)).ToArray()
            .ToGuardCases(_ => new GuardExpected(true));

        public static TheoryData<GuardCase<(TimeSpan value, TimeSpan min, TimeSpan max, Inclusion inclusion)>> InvalidCases =>
            F.IsDurationBetween.AllInvalid.Where(s => s.Inputs.value.HasValue).Select(s => new RuleScenario<(TimeSpan, TimeSpan, TimeSpan, Inclusion)>(s.Name, (s.Inputs.value!.Value, s.Inputs.min, s.Inputs.max, s.Inputs.inclusion), s.IsValid)).ToArray()
            .ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    // Guard.Against.DurationBetween — throws when value IS in [min, max]
    public static class DurationBetween
    {
        public static TheoryData<GuardCase<(TimeSpan value, TimeSpan min, TimeSpan max, Inclusion inclusion)>> ValidCases =>
            F.IsDurationBetween.AllInvalid.Where(s => s.Inputs.value.HasValue).Select(s => new RuleScenario<(TimeSpan, TimeSpan, TimeSpan, Inclusion)>(s.Name, (s.Inputs.value!.Value, s.Inputs.min, s.Inputs.max, s.Inputs.inclusion), s.IsValid)).ToArray()
            .ToGuardCases(_ => new GuardExpected(true));

        public static TheoryData<GuardCase<(TimeSpan value, TimeSpan min, TimeSpan max, Inclusion inclusion)>> InvalidCases =>
            F.IsDurationBetween.AllValid.Select(s => new RuleScenario<(TimeSpan, TimeSpan, TimeSpan, Inclusion)>(s.Name, (s.Inputs.value!.Value, s.Inputs.min, s.Inputs.max, s.Inputs.inclusion), s.IsValid)).ToArray()
            .ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    // Guard.Against.LessThan — throws when value IS less than threshold (delegates to Must.Be.GreaterThan)
    public static class LessThan
    {
        public static TheoryData<GuardCase<(TimeSpan value, TimeSpan threshold, Inclusion inclusion)>> ValidCases =>
            F.IsGreaterThan.AllValid.Select(s => new RuleScenario<(TimeSpan, TimeSpan, Inclusion)>(s.Name, (s.Inputs.value!.Value, s.Inputs.threshold!.Value, s.Inputs.inclusion), s.IsValid)).ToArray()
            .ToGuardCases(_ => new GuardExpected(true));

        public static TheoryData<GuardCase<(TimeSpan value, TimeSpan threshold, Inclusion inclusion)>> InvalidCases =>
            F.IsGreaterThan.AllInvalid.Where(s => s.Inputs is { value: not null, threshold: not null }).Select(s => new RuleScenario<(TimeSpan, TimeSpan, Inclusion)>(s.Name, (s.Inputs.value!.Value, s.Inputs.threshold!.Value, s.Inputs.inclusion), s.IsValid)).ToArray()
            .ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    // Guard.Against.GreaterThan — throws when value IS greater than threshold (delegates to Must.Be.LessThan)
    public static class GreaterThan
    {
        public static TheoryData<GuardCase<(TimeSpan value, TimeSpan threshold, Inclusion inclusion)>> ValidCases =>
            F.IsLessThan.AllValid.Select(s => new RuleScenario<(TimeSpan, TimeSpan, Inclusion)>(s.Name, (s.Inputs.value!.Value, s.Inputs.threshold!.Value, s.Inputs.inclusion), s.IsValid)).ToArray()
            .ToGuardCases(_ => new GuardExpected(true));

        public static TheoryData<GuardCase<(TimeSpan value, TimeSpan threshold, Inclusion inclusion)>> InvalidCases =>
            F.IsLessThan.AllInvalid.Where(s => s.Inputs is { value: not null, threshold: not null }).Select(s => new RuleScenario<(TimeSpan, TimeSpan, Inclusion)>(s.Name, (s.Inputs.value!.Value, s.Inputs.threshold!.Value, s.Inputs.inclusion), s.IsValid)).ToArray()
            .ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value"));
    }
}
