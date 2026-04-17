using PineGuard.Common;
using PineGuard.Testing.UnitTests.GuardClauses;
using F = PineGuard.Testing.Fixtures.TimeOnlyRulesFixtures;

namespace PineGuard.GuardClauses.UnitTests;

public static class GuardTimeOnlyClausesTestData
{
    private static readonly TimeOnly Morning = F.IsKnownTimes.T0800!.Value;
    private static readonly TimeOnly Noon = F.IsKnownTimes.T1200!.Value;
    private static readonly TimeOnly Evening = F.IsKnownTimes.T2000!.Value;

    public static class Between
    {
        public static TheoryData<GuardCase<(TimeOnly value, TimeOnly min, TimeOnly max, Inclusion inclusion)>> Cases =>
        [
            new("out-of-range-low", (Morning.AddHours(-1), Morning, Evening, Inclusion.Inclusive), new GuardExpected(true)),
            new("out-of-range-high", (Evening.AddHours(1), Morning, Evening, Inclusion.Inclusive), new GuardExpected(true)),
            new("in-range", (Noon, Morning, Evening, Inclusion.Inclusive), new GuardExpected(false, typeof(ArgumentException), "value"))
        ];
    }

    public static class NotBetween
    {
        public static TheoryData<GuardCase<(TimeOnly value, TimeOnly min, TimeOnly max, Inclusion inclusion)>> Cases =>
        [
            new("in-range", (Noon, Morning, Evening, Inclusion.Inclusive), new GuardExpected(true)),
            new("out-of-range-low", (Morning.AddHours(-1), Morning, Evening, Inclusion.Inclusive), new GuardExpected(false, typeof(ArgumentException), "value")),
            new("out-of-range-high", (Evening.AddHours(1), Morning, Evening, Inclusion.Inclusive), new GuardExpected(false, typeof(ArgumentException), "value"))
        ];
    }

    public static class Before
    {
        public static TheoryData<GuardCase<(TimeOnly value, TimeOnly other)>> Cases =>
        [
            new("after", (Evening, Noon), new GuardExpected(true)),
            new("same", (Noon, Noon), new GuardExpected(true)),
            new("before", (Morning, Noon), new GuardExpected(false, typeof(ArgumentException), "value"))
        ];
    }

    public static class OnOrBefore
    {
        public static TheoryData<GuardCase<(TimeOnly value, TimeOnly other)>> Cases =>
        [
            new("after", (Evening, Noon), new GuardExpected(true)),
            new("same", (Noon, Noon), new GuardExpected(false, typeof(ArgumentException), "value")),
            new("before", (Morning, Noon), new GuardExpected(false, typeof(ArgumentException), "value"))
        ];
    }

    public static class After
    {
        public static TheoryData<GuardCase<(TimeOnly value, TimeOnly other)>> Cases =>
        [
            new("before", (Morning, Noon), new GuardExpected(true)),
            new("same", (Noon, Noon), new GuardExpected(true)),
            new("after", (Evening, Noon), new GuardExpected(false, typeof(ArgumentException), "value"))
        ];
    }

    public static class OnOrAfter
    {
        public static TheoryData<GuardCase<(TimeOnly value, TimeOnly other)>> Cases =>
        [
            new("before", (Morning, Noon), new GuardExpected(true)),
            new("same", (Noon, Noon), new GuardExpected(false, typeof(ArgumentException), "value")),
            new("after", (Evening, Noon), new GuardExpected(false, typeof(ArgumentException), "value"))
        ];
    }

    public static class Same
    {
        public static TheoryData<GuardCase<(TimeOnly value, TimeOnly other)>> Cases =>
        [
            new("different", (Morning, Evening), new GuardExpected(true)),
            new("same", (Noon, Noon), new GuardExpected(false, typeof(ArgumentException), "value"))
        ];
    }

    public static class NotSame
    {
        public static TheoryData<GuardCase<(TimeOnly value, TimeOnly other)>> Cases =>
        [
            new("same", (Noon, Noon), new GuardExpected(true)),
            new("different", (Morning, Evening), new GuardExpected(false, typeof(ArgumentException), "value"))
        ];
    }

    public static class NotChronological
    {
        public static TheoryData<GuardCase<(TimeOnly start, TimeOnly end, Inclusion inclusion)>> Cases =>
        [
            new("chronological", (Morning, Evening, Inclusion.Exclusive), new GuardExpected(true)),
            new("reverse", (Evening, Morning, Inclusion.Exclusive), new GuardExpected(false, typeof(ArgumentException), "start"))
        ];
    }

    public static class Chronological
    {
        public static TheoryData<GuardCase<(TimeOnly start, TimeOnly end, Inclusion inclusion)>> Cases =>
        [
            new("reverse", (Evening, Morning, Inclusion.Exclusive), new GuardExpected(true)),
            new("chronological", (Morning, Evening, Inclusion.Exclusive), new GuardExpected(false, typeof(ArgumentException), "start"))
        ];
    }

    public static class Overlapping
    {
        public static TheoryData<GuardCase<(TimeOnly start1, TimeOnly end1, TimeOnly start2, TimeOnly end2, Inclusion inclusion)>> Cases =>
        [
            new("non-overlapping", (Morning, Noon, Evening, Evening.AddHours(1), Inclusion.Inclusive), new GuardExpected(true)),
            new("overlapping", (Morning, Evening, Noon, Evening.AddHours(1), Inclusion.Inclusive), new GuardExpected(false, typeof(ArgumentException), "start1"))
        ];
    }

    public static class NotOverlapping
    {
        public static TheoryData<GuardCase<(TimeOnly start1, TimeOnly end1, TimeOnly start2, TimeOnly end2, Inclusion inclusion)>> Cases =>
        [
            new("overlapping", (Morning, Evening, Noon, Evening.AddHours(1), Inclusion.Inclusive), new GuardExpected(true)),
            new("non-overlapping", (Morning, Noon, Evening, Evening.AddHours(1), Inclusion.Inclusive), new GuardExpected(false, typeof(ArgumentException), "start1"))
        ];
    }
}
