using PineGuard.Common;
using PineGuard.Testing.UnitTests.MustClauses;
using F = PineGuard.Testing.Fixtures.StringRulesFixtures;

namespace PineGuard.MustClauses.UnitTests;

public static class MustStringTimeSpanClausesTestData
{
    public static class DurationBetween
    {
        public static TheoryData<MustCase<(string? value, TimeSpan min, TimeSpan max, Inclusion inclusion)>> ValidCases =>
            F.TimeSpanIsDurationBetween.AllValid.ToMustCases();
        public static TheoryData<MustCase<(string? value, TimeSpan min, TimeSpan max, Inclusion inclusion)>> InvalidCases =>
            F.TimeSpanIsDurationBetween.AllInvalid.ToMustCases(s => s.Name switch
            {
                nameof(F.TimeSpanIsDurationBetween.NullValue) => new MustExpected(false, "value must not be null.", "value"),
                _ => new MustExpected(false, "value must be a duration within the expected range.")
            });
    }

    public static class NotDurationBetween
    {
        public static TheoryData<MustCase<(string? value, TimeSpan min, TimeSpan max, Inclusion inclusion)>> Cases =>
        [
            new("in-range", ("00:30:00", TimeSpan.FromMinutes(10), TimeSpan.FromMinutes(60), Inclusion.Inclusive), new MustExpected(false, "value must be a duration not within the expected range.")),
            new("out-of-range", ("02:00:00", TimeSpan.FromMinutes(10), TimeSpan.FromMinutes(60), Inclusion.Inclusive), new MustExpected(true)),
            new("not-a-duration", ("not-a-duration", TimeSpan.FromMinutes(10), TimeSpan.FromMinutes(60), Inclusion.Inclusive), new MustExpected(false, "value must be a duration not within the expected range.")),
            new("null-value", (null, TimeSpan.FromMinutes(10), TimeSpan.FromMinutes(60), Inclusion.Inclusive), new MustExpected(false, "value must not be null.", "value")),
            new("at-min-exclusive", ("00:10:00", TimeSpan.FromMinutes(10), TimeSpan.FromMinutes(60), Inclusion.Exclusive), new MustExpected(true))
        ];
    }

    public static class GreaterThan
    {
        public static TheoryData<MustCase<(string? value, TimeSpan threshold, Inclusion inclusion)>> ValidCases =>
            F.TimeSpanIsGreaterThan.AllValid.ToMustCases();
        public static TheoryData<MustCase<(string? value, TimeSpan threshold, Inclusion inclusion)>> InvalidCases =>
            F.TimeSpanIsGreaterThan.AllInvalid.ToMustCases(s => s.Name switch
            {
                nameof(F.TimeSpanIsGreaterThan.NullValue) => new MustExpected(false, "value must not be null.", "value"),
                _ => new MustExpected(false, "value must be a duration greater than the threshold.")
            });
    }

    public static class LessThan
    {
        public static TheoryData<MustCase<(string? value, TimeSpan threshold, Inclusion inclusion)>> ValidCases =>
            F.TimeSpanIsLessThan.AllValid.ToMustCases();
        public static TheoryData<MustCase<(string? value, TimeSpan threshold, Inclusion inclusion)>> InvalidCases =>
            F.TimeSpanIsLessThan.AllInvalid.ToMustCases(s => s.Name switch
            {
                nameof(F.TimeSpanIsLessThan.NullValue) => new MustExpected(false, "value must not be null.", "value"),
                _ => new MustExpected(false, "value must be a duration less than the threshold.")
            });
    }
}
