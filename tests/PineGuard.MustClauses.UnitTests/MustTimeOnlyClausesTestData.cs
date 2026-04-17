using PineGuard.Common;
using PineGuard.Testing.UnitTests.MustClauses;
using F = PineGuard.Testing.Fixtures.TimeOnlyRulesFixtures;

namespace PineGuard.MustClauses.UnitTests;

public static class MustTimeOnlyClausesTestData
{
    private static readonly TimeOnly T1 = F.IsKnownTimes.T1000!.Value;
    private static readonly TimeOnly T2 = F.IsKnownTimes.T1200!.Value;
    private static readonly TimeOnly T3 = F.IsKnownTimes.T1400!.Value;

    public static class Between
    {
        public static TheoryData<MustCase<(TimeOnly value, TimeOnly min, TimeOnly max)>> Cases =>
        [
            new("between", (T2, T1, T3), new MustExpected(true)),
            new("not-between", (new TimeOnly(9, 0), T1, T3), new MustExpected(false, "value must be within the expected range.")),
            new("min-gt-max", (T2, T3, T1), new MustExpected(false, "min requires a valid range.", "min"))
        ];
    }

    public static class NotBetween
    {
        public static TheoryData<MustCase<(TimeOnly value, TimeOnly min, TimeOnly max)>> Cases =>
        [
            new("not-between", (new TimeOnly(9, 0), T1, T3), new MustExpected(true)),
            new("between", (T2, T1, T3), new MustExpected(false, "value must not be within the expected range.")),
            new("min-gt-max", (T2, T3, T1), new MustExpected(false, "min requires a valid range.", "min"))
        ];
    }

    public static class Before
    {
        public static TheoryData<MustCase<(TimeOnly value, TimeOnly other, TimePrecision? precision)>> Cases =>
        [
            new("before", (T1, T2, null), new MustExpected(true)),
            new("after", (T2, T1, null), new MustExpected(false, "value must be before the specified time.")),
            new("same", (T1, T1, null), new MustExpected(false, "value must be before the specified time.")),
            new("invalid-precision", (T1, T2, (TimePrecision)999), new MustExpected(false, "precision requires a valid precision.", "precision"))
        ];
    }

    public static class OnOrBefore
    {
        public static TheoryData<MustCase<(TimeOnly value, TimeOnly other, TimePrecision? precision)>> Cases =>
        [
            new("before", (T1, T2, null), new MustExpected(true)),
            new("same", (T1, T1, null), new MustExpected(true)),
            new("after", (T2, T1, null), new MustExpected(false, "value must be on or before the specified time.")),
            new("invalid-precision", (T1, T2, (TimePrecision)999), new MustExpected(false, "precision requires a valid precision.", "precision"))
        ];
    }

    public static class After
    {
        public static TheoryData<MustCase<(TimeOnly value, TimeOnly other, TimePrecision? precision)>> Cases =>
        [
            new("after", (T2, T1, null), new MustExpected(true)),
            new("before", (T1, T2, null), new MustExpected(false, "value must be after the specified time.")),
            new("same", (T1, T1, null), new MustExpected(false, "value must be after the specified time.")),
            new("invalid-precision", (T1, T2, (TimePrecision)999), new MustExpected(false, "precision requires a valid precision.", "precision"))
        ];
    }

    public static class OnOrAfter
    {
        public static TheoryData<MustCase<(TimeOnly value, TimeOnly other, TimePrecision? precision)>> Cases =>
        [
            new("after", (T2, T1, null), new MustExpected(true)),
            new("same", (T1, T1, null), new MustExpected(true)),
            new("before", (T1, T2, null), new MustExpected(false, "value must be on or after the specified time.")),
            new("invalid-precision", (T1, T2, (TimePrecision)999), new MustExpected(false, "precision requires a valid precision.", "precision"))
        ];
    }

    public static class Same
    {
        public static TheoryData<MustCase<(TimeOnly value, TimeOnly other, TimePrecision? precision)>> Cases =>
        [
            new("same", (T1, T1, null), new MustExpected(true)),
            new("not-same", (T1, T2, null), new MustExpected(false, "value must be the same time.")),
            new("invalid-precision", (T1, T2, (TimePrecision)999), new MustExpected(false, "precision requires a valid precision.", "precision"))
        ];
    }

    public static class NotSame
    {
        public static TheoryData<MustCase<(TimeOnly value, TimeOnly other, TimePrecision? precision)>> Cases =>
        [
            new("not-same", (T1, T2, null), new MustExpected(true)),
            new("same", (T1, T1, null), new MustExpected(false, "value must not be the same time.")),
            new("invalid-precision", (T1, T2, (TimePrecision)999), new MustExpected(false, "precision requires a valid precision.", "precision"))
        ];
    }

    public static class Within
    {
        private static readonly TimeSpan OneHour = TimeSpan.FromHours(1);

        public static TheoryData<MustCase<(TimeOnly value, TimeOnly reference, TimeSpan window)>> Cases =>
        [
            new("within", (T1.AddHours(0.5), T1, OneHour), new MustExpected(true)),
            new("outside", (T1.AddHours(2), T1, OneHour), new MustExpected(false, "value must be within the expected time window.")),
            new("negative-window", (T1, T1, TimeSpan.FromHours(-1)), new MustExpected(false, "window requires a non-negative window.", "window"))
        ];
    }

    public static class NotWithin
    {
        private static readonly TimeSpan OneHour = TimeSpan.FromHours(1);

        public static TheoryData<MustCase<(TimeOnly value, TimeOnly reference, TimeSpan window)>> Cases =>
        [
            new("outside", (T1.AddHours(2), T1, OneHour), new MustExpected(true)),
            new("within", (T1.AddHours(0.5), T1, OneHour), new MustExpected(false, "value must not be within the expected time window.")),
            new("negative-window", (T1, T1, TimeSpan.FromHours(-1)), new MustExpected(false, "window requires a non-negative window.", "window"))
        ];
    }

    public static class Chronological
    {
        public static TheoryData<MustCase<(TimeOnly start, TimeOnly end)>> Cases =>
        [
            new("chronological", (T1, T2), new MustExpected(true)),
            new("reverse", (T2, T1), new MustExpected(false, "start must be chronological.")),
            new("same", (T1, T1), new MustExpected(false, "start must be chronological."))
        ];
    }

    public static class NotChronological
    {
        public static TheoryData<MustCase<(TimeOnly start, TimeOnly end)>> Cases =>
        [
            new("reverse", (T2, T1), new MustExpected(true)),
            new("same", (T1, T1), new MustExpected(true)),
            new("chronological", (T1, T2), new MustExpected(false, "start must not be chronological."))
        ];
    }

    public static class Overlapping
    {
        public static TheoryData<MustCase<(TimeOnly start1, TimeOnly end1, TimeOnly start2, TimeOnly end2)>> Cases =>
        [
            new("overlapping", (T1, T2, new TimeOnly(11, 0), T3), new MustExpected(true)),
            new("not-overlapping", (T1, T2, T3, new TimeOnly(16, 0)), new MustExpected(false, "start1 must be overlapping.")),
            new("invalid-range1", (T3, T1, new TimeOnly(11, 0), T3), new MustExpected(false, "start1 must be overlapping.")),
            new("invalid-range2", (T1, T2, T3, T1), new MustExpected(false, "start1 must be overlapping."))
        ];
    }

    public static class NotOverlapping
    {
        public static TheoryData<MustCase<(TimeOnly start1, TimeOnly end1, TimeOnly start2, TimeOnly end2)>> Cases =>
        [
            new("not-overlapping", (T1, T2, T3, new TimeOnly(16, 0)), new MustExpected(true)),
            new("invalid-range1", (T3, T1, new TimeOnly(11, 0), T3), new MustExpected(true)),
            new("invalid-range2", (T1, T2, T3, T1), new MustExpected(true)),
            new("overlapping", (T1, T2, new TimeOnly(11, 0), T3), new MustExpected(false, "start1 must not be overlapping."))
        ];
    }

    public static class NotBefore
    {
        public static TheoryData<MustCase<(TimeOnly value, TimeOnly other, TimePrecision? precision)>> Cases =>
        [
            new("not-before-after", (T3, T2, null), new MustExpected(true)),
            new("not-before-same", (T2, T2, null), new MustExpected(true)),
            new("before", (T1, T2, null), new MustExpected(false, "value must not be before the specified time.")),
            new("invalid-precision", (T1, T2, (TimePrecision)999), new MustExpected(false, "precision requires a valid precision.", "precision"))
        ];
    }

    public static class NotOnOrBefore
    {
        public static TheoryData<MustCase<(TimeOnly value, TimeOnly other, TimePrecision? precision)>> Cases =>
        [
            new("after", (T3, T2, null), new MustExpected(true)),
            new("same", (T2, T2, null), new MustExpected(false, "value must not be on or before the specified time.")),
            new("before", (T1, T2, null), new MustExpected(false, "value must not be on or before the specified time.")),
            new("invalid-precision", (T1, T2, (TimePrecision)999), new MustExpected(false, "precision requires a valid precision.", "precision"))
        ];
    }

    public static class NotAfter
    {
        public static TheoryData<MustCase<(TimeOnly value, TimeOnly other, TimePrecision? precision)>> Cases =>
        [
            new("not-after-before", (T1, T2, null), new MustExpected(true)),
            new("not-after-same", (T2, T2, null), new MustExpected(true)),
            new("after", (T3, T2, null), new MustExpected(false, "value must not be after the specified time.")),
            new("invalid-precision", (T1, T2, (TimePrecision)999), new MustExpected(false, "precision requires a valid precision.", "precision"))
        ];
    }

    public static class NotOnOrAfter
    {
        public static TheoryData<MustCase<(TimeOnly value, TimeOnly other, TimePrecision? precision)>> Cases =>
        [
            new("before", (T1, T2, null), new MustExpected(true)),
            new("same", (T2, T2, null), new MustExpected(false, "value must not be on or after the specified time.")),
            new("after", (T3, T2, null), new MustExpected(false, "value must not be on or after the specified time.")),
            new("invalid-precision", (T1, T2, (TimePrecision)999), new MustExpected(false, "precision requires a valid precision.", "precision"))
        ];
    }
}
