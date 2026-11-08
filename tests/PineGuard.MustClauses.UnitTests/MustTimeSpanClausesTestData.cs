using PineGuard.Codes;
using PineGuard.Common;
using PineGuard.Testing.UnitTests.MustClauses;
using PineGuard.Testing.UnitTests.Rules;
using F = PineGuard.Testing.Fixtures.TimeSpanRulesFixtures;

namespace PineGuard.MustClauses.UnitTests;

public static class MustTimeSpanClausesTestData
{
    // Must.Be.InRange — valid when value is within [min, max]
    public static class InRange
    {
        public static TheoryData<MustCase<(TimeSpan value, TimeSpan min, TimeSpan max, Inclusion inclusion)>> ValidCases =>
            F.IsInRange.AllValid.Select(s => new RuleScenario<(TimeSpan, TimeSpan, TimeSpan, Inclusion)>(s.Name, (s.Inputs.value!.Value, s.Inputs.min, s.Inputs.max, s.Inputs.inclusion), s.IsValid)).ToArray()
            .ToMustCases(_ => new MustExpected(true));

        public static TheoryData<MustCase<(TimeSpan value, TimeSpan min, TimeSpan max, Inclusion inclusion)>> InvalidCases =>
            F.IsInRange.AllInvalid.Where(s => s.Inputs.value.HasValue).Select(s => new RuleScenario<(TimeSpan, TimeSpan, TimeSpan, Inclusion)>(s.Name, (s.Inputs.value!.Value, s.Inputs.min, s.Inputs.max, s.Inputs.inclusion), s.IsValid)).ToArray()
            .ToMustCases(_ => new MustExpected(false, "value must be within the expected duration range.", Code: MustCodes.Time.Duration.OutOfRange));

        public static TheoryData<MustCase<(TimeSpan value, TimeSpan min, TimeSpan max, Inclusion inclusion)>> InvalidRangeCases =>
        [
            new("invalid range", (TimeSpan.FromMinutes(30), TimeSpan.FromMinutes(60), TimeSpan.FromMinutes(10), Inclusion.Inclusive), new MustExpected(false, "min requires a valid range.", "min"))
        ];
    }

    // Must.Be.OutOfRange — valid when value is NOT within [min, max]
    public static class OutOfRange
    {
        public static TheoryData<MustCase<(TimeSpan value, TimeSpan min, TimeSpan max, Inclusion inclusion)>> ValidCases =>
            F.IsInRange.AllInvalid.Where(s => s.Inputs.value.HasValue).Select(s => new RuleScenario<(TimeSpan, TimeSpan, TimeSpan, Inclusion)>(s.Name, (s.Inputs.value!.Value, s.Inputs.min, s.Inputs.max, s.Inputs.inclusion), s.IsValid)).ToArray()
            .ToMustCases(_ => new MustExpected(true));

        public static TheoryData<MustCase<(TimeSpan value, TimeSpan min, TimeSpan max, Inclusion inclusion)>> InvalidCases =>
            F.IsInRange.AllValid.Select(s => new RuleScenario<(TimeSpan, TimeSpan, TimeSpan, Inclusion)>(s.Name, (s.Inputs.value!.Value, s.Inputs.min, s.Inputs.max, s.Inputs.inclusion), s.IsValid)).ToArray()
            .ToMustCases(_ => new MustExpected(false, "value must not be within the expected duration range.", Code: MustCodes.Time.Duration.InRange));

        public static TheoryData<MustCase<(TimeSpan value, TimeSpan min, TimeSpan max, Inclusion inclusion)>> InvalidRangeCases =>
        [
            new("invalid range", (TimeSpan.FromMinutes(30), TimeSpan.FromMinutes(60), TimeSpan.FromMinutes(10), Inclusion.Inclusive), new MustExpected(false, "min requires a valid range.", "min"))
        ];
    }

    // Must.Be.GreaterThan — valid when value is greater than other
    public static class GreaterThan
    {
        public static TheoryData<MustCase<(TimeSpan value, TimeSpan other, Inclusion inclusion)>> ValidCases =>
            F.IsGreaterThan.AllValid.Select(s => new RuleScenario<(TimeSpan, TimeSpan, Inclusion)>(s.Name, (s.Inputs.value!.Value, s.Inputs.other!.Value, s.Inputs.inclusion), s.IsValid)).ToArray()
            .ToMustCases(_ => new MustExpected(true));

        public static TheoryData<MustCase<(TimeSpan value, TimeSpan other, Inclusion inclusion)>> InvalidCases =>
            F.IsGreaterThan.AllInvalid.Where(s => s.Inputs is { value: not null, other: not null }).Select(s => new RuleScenario<(TimeSpan, TimeSpan, Inclusion)>(s.Name, (s.Inputs.value!.Value, s.Inputs.other!.Value, s.Inputs.inclusion), s.IsValid)).ToArray()
            .ToMustCases(_ => new MustExpected(false, "value must be greater than the threshold.", Code: MustCodes.Time.Duration.NotGreater));
    }

    // Must.Be.LessThan — valid when value is less than other
    public static class LessThan
    {
        public static TheoryData<MustCase<(TimeSpan value, TimeSpan other, Inclusion inclusion)>> ValidCases =>
            F.IsLessThan.AllValid.Select(s => new RuleScenario<(TimeSpan, TimeSpan, Inclusion)>(s.Name, (s.Inputs.value!.Value, s.Inputs.other!.Value, s.Inputs.inclusion), s.IsValid)).ToArray()
            .ToMustCases(_ => new MustExpected(true));

        public static TheoryData<MustCase<(TimeSpan value, TimeSpan other, Inclusion inclusion)>> InvalidCases =>
            F.IsLessThan.AllInvalid.Where(s => s.Inputs is { value: not null, other: not null }).Select(s => new RuleScenario<(TimeSpan, TimeSpan, Inclusion)>(s.Name, (s.Inputs.value!.Value, s.Inputs.other!.Value, s.Inputs.inclusion), s.IsValid)).ToArray()
            .ToMustCases(_ => new MustExpected(false, "value must be less than the threshold.", Code: MustCodes.Time.Duration.NotLess));
    }
}
