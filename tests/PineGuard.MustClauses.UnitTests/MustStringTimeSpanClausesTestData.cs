using PineGuard.Codes;
using PineGuard.Common;
using PineGuard.Testing.UnitTests.MustClauses;
using F = PineGuard.Testing.Fixtures.StringRulesFixtures;

namespace PineGuard.MustClauses.UnitTests;

public static class MustStringTimeSpanClausesTestData
{
    public static class InRange
    {
        public static TheoryData<MustCase<(string? value, TimeSpan min, TimeSpan max, Inclusion inclusion)>> ValidCases =>
            F.TimeSpanIsInRange.AllValid.ToMustCases();
        public static TheoryData<MustCase<(string? value, TimeSpan min, TimeSpan max, Inclusion inclusion)>> InvalidCases =>
            F.TimeSpanIsInRange.AllInvalid.ToMustCases(s => s.Name switch
            {
                nameof(F.TimeSpanIsInRange.NullValue) => new MustExpected(false, "value must not be null.", "value"),
                nameof(F.TimeSpanIsInRange.NotADuration) => new MustExpected(false, "value must be a duration within the expected range.", Code: MustCodes.Time.Format.Invalid),
                _ => new MustExpected(false, "value must be a duration within the expected range.", Code: MustCodes.Time.Duration.OutOfRange)
            });
    }

    public static class OutOfRange
    {
        public static TheoryData<MustCase<(string? value, TimeSpan min, TimeSpan max, Inclusion inclusion)>> Cases =>
        [
            new("in-range", ("00:30:00", TimeSpan.FromMinutes(10), TimeSpan.FromMinutes(60), Inclusion.Inclusive), new MustExpected(false, "value must be a duration not within the expected range.", Code: MustCodes.Time.Duration.InRange)),
            new("out-of-range", ("02:00:00", TimeSpan.FromMinutes(10), TimeSpan.FromMinutes(60), Inclusion.Inclusive), new MustExpected(true)),
            new("not-a-duration", ("not-a-duration", TimeSpan.FromMinutes(10), TimeSpan.FromMinutes(60), Inclusion.Inclusive), new MustExpected(false, "value must be a duration not within the expected range.")),
            new("null-value", (null, TimeSpan.FromMinutes(10), TimeSpan.FromMinutes(60), Inclusion.Inclusive), new MustExpected(false, "value must not be null.", "value")),
            new("at-min-exclusive", ("00:10:00", TimeSpan.FromMinutes(10), TimeSpan.FromMinutes(60), Inclusion.Exclusive), new MustExpected(true))
        ];
    }

    public static class LongerThan
    {
        public static TheoryData<MustCase<(string? value, TimeSpan other, Inclusion inclusion)>> ValidCases =>
            F.TimeSpanIsLongerThan.AllValid.ToMustCases();
        public static TheoryData<MustCase<(string? value, TimeSpan other, Inclusion inclusion)>> InvalidCases =>
            F.TimeSpanIsLongerThan.AllInvalid.ToMustCases(s => s.Name switch
            {
                nameof(F.TimeSpanIsLongerThan.NullValue) => new MustExpected(false, "value must not be null.", "value"),
                nameof(F.TimeSpanIsLongerThan.EqualExclusive) => new MustExpected(false, "value must be a duration longer than the other.", Code: MustCodes.Time.Duration.NotLonger),
                _ => new MustExpected(false, "value must be a duration longer than the other.")
            });
    }

    public static class ShorterThan
    {
        public static TheoryData<MustCase<(string? value, TimeSpan other, Inclusion inclusion)>> ValidCases =>
            F.TimeSpanIsShorterThan.AllValid.ToMustCases();
        public static TheoryData<MustCase<(string? value, TimeSpan other, Inclusion inclusion)>> InvalidCases =>
            F.TimeSpanIsShorterThan.AllInvalid.ToMustCases(s => s.Name switch
            {
                nameof(F.TimeSpanIsShorterThan.NullValue) => new MustExpected(false, "value must not be null.", "value"),
                nameof(F.TimeSpanIsShorterThan.EqualExclusive) => new MustExpected(false, "value must be a duration shorter than the other.", Code: MustCodes.Time.Duration.NotShorter),
                _ => new MustExpected(false, "value must be a duration shorter than the other.")
            });
    }
}
