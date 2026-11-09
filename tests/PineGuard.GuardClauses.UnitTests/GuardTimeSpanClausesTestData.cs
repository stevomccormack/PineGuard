using PineGuard.Common;
using PineGuard.Testing.UnitTests.GuardClauses;
using PineGuard.Testing.UnitTests.Rules;
using F = PineGuard.Testing.Fixtures.TimeSpanRulesFixtures;

namespace PineGuard.GuardClauses.UnitTests;

public static class GuardTimeSpanClausesTestData
{
    // Guard.Against.OutOfRange — throws when value is NOT in [min, max]
    public static class OutOfRange
    {
        public static TheoryData<GuardCase<(TimeSpan value, TimeSpan min, TimeSpan max, Inclusion inclusion)>> ValidCases =>
            F.IsInRange.AllValid.Select(s => new RuleScenario<(TimeSpan, TimeSpan, TimeSpan, Inclusion)>(s.Name, (s.Inputs.value!.Value, s.Inputs.min, s.Inputs.max, s.Inputs.inclusion), s.IsValid)).ToArray()
            .ToGuardCases(_ => new GuardExpected(true));

        public static TheoryData<GuardCase<(TimeSpan value, TimeSpan min, TimeSpan max, Inclusion inclusion)>> InvalidCases =>
            F.IsInRange.AllInvalid.Where(s => s.Inputs.value.HasValue).Select(s => new RuleScenario<(TimeSpan, TimeSpan, TimeSpan, Inclusion)>(s.Name, (s.Inputs.value!.Value, s.Inputs.min, s.Inputs.max, s.Inputs.inclusion), s.IsValid)).ToArray()
            .ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    // Guard.Against.InRange — throws when value IS in [min, max]
    public static class InRange
    {
        public static TheoryData<GuardCase<(TimeSpan value, TimeSpan min, TimeSpan max, Inclusion inclusion)>> ValidCases =>
            F.IsInRange.AllInvalid.Where(s => s.Inputs.value.HasValue).Select(s => new RuleScenario<(TimeSpan, TimeSpan, TimeSpan, Inclusion)>(s.Name, (s.Inputs.value!.Value, s.Inputs.min, s.Inputs.max, s.Inputs.inclusion), s.IsValid)).ToArray()
            .ToGuardCases(_ => new GuardExpected(true));

        public static TheoryData<GuardCase<(TimeSpan value, TimeSpan min, TimeSpan max, Inclusion inclusion)>> InvalidCases =>
            F.IsInRange.AllValid.Select(s => new RuleScenario<(TimeSpan, TimeSpan, TimeSpan, Inclusion)>(s.Name, (s.Inputs.value!.Value, s.Inputs.min, s.Inputs.max, s.Inputs.inclusion), s.IsValid)).ToArray()
            .ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    // Guard.Against.ShorterThan — throws when value IS shorter than other (delegates to Must.Be.LongerThan)
    public static class ShorterThan
    {
        public static TheoryData<GuardCase<(TimeSpan value, TimeSpan other, Inclusion inclusion)>> ValidCases =>
            F.IsLongerThan.AllValid.Select(s => new RuleScenario<(TimeSpan, TimeSpan, Inclusion)>(s.Name, (s.Inputs.value!.Value, s.Inputs.other!.Value, s.Inputs.inclusion == Inclusion.Inclusive ? Inclusion.Exclusive : Inclusion.Inclusive), s.IsValid)).ToArray()
            .ToGuardCases(_ => new GuardExpected(true));

        public static TheoryData<GuardCase<(TimeSpan value, TimeSpan other, Inclusion inclusion)>> InvalidCases =>
            F.IsLongerThan.AllInvalid.Where(s => s.Inputs is { value: not null, other: not null })
            .Select(s => new RuleScenario<(TimeSpan, TimeSpan, Inclusion)>(s.Name, (s.Inputs.value!.Value, s.Inputs.other!.Value, s.Inputs.inclusion == Inclusion.Inclusive ? Inclusion.Exclusive : Inclusion.Inclusive), s.IsValid)).ToArray()
            .ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    // Guard.Against.LongerThan — throws when value IS longer than other (delegates to Must.Be.ShorterThan)
    public static class LongerThan
    {
        public static TheoryData<GuardCase<(TimeSpan value, TimeSpan other, Inclusion inclusion)>> ValidCases =>
            F.IsShorterThan.AllValid.Select(s => new RuleScenario<(TimeSpan, TimeSpan, Inclusion)>(s.Name, (s.Inputs.value!.Value, s.Inputs.other!.Value, s.Inputs.inclusion == Inclusion.Inclusive ? Inclusion.Exclusive : Inclusion.Inclusive), s.IsValid)).ToArray()
            .ToGuardCases(_ => new GuardExpected(true));

        public static TheoryData<GuardCase<(TimeSpan value, TimeSpan other, Inclusion inclusion)>> InvalidCases =>
            F.IsShorterThan.AllInvalid.Where(s => s.Inputs is { value: not null, other: not null })
            .Select(s => new RuleScenario<(TimeSpan, TimeSpan, Inclusion)>(s.Name, (s.Inputs.value!.Value, s.Inputs.other!.Value, s.Inputs.inclusion == Inclusion.Inclusive ? Inclusion.Exclusive : Inclusion.Inclusive), s.IsValid)).ToArray()
            .ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value"));
    }
}
