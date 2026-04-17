using PineGuard.Common;
using PineGuard.Testing.UnitTests.GuardClauses;
using F = PineGuard.Testing.Fixtures.StringRulesFixtures;

namespace PineGuard.GuardClauses.UnitTests;

public static class GuardStringTimeOnlyClausesTestData
{
    private static readonly TimeOnly T1000 = F.StringTimeOnly.IsBetween.InRangeInclusive.min;
    private static readonly TimeOnly T1200 = F.StringTimeOnly.IsBetween.InRangeInclusive.max;

    public static class NotBetweenTimeOnly
    {
        public static TheoryData<GuardCase<(string? value, TimeOnly min, TimeOnly max, Inclusion inclusion)>> Cases =>
        [
            new("in-range", ("11:00", T1000, T1200, Inclusion.Inclusive), new GuardExpected(true)),
            new("out-of-range", ("09:00", T1000, T1200, Inclusion.Inclusive), new GuardExpected(false, typeof(ArgumentException), "value")),
            new("null-value", (null, T1000, T1200, Inclusion.Inclusive), new GuardExpected(false, typeof(ArgumentNullException), "value"))
        ];
    }

    public static class BetweenTimeOnly
    {
        public static TheoryData<GuardCase<(string? value, TimeOnly min, TimeOnly max, Inclusion inclusion)>> Cases =>
        [
            new("out-of-range", ("09:00", T1000, T1200, Inclusion.Inclusive), new GuardExpected(true)),
            new("in-range", ("11:00", T1000, T1200, Inclusion.Inclusive), new GuardExpected(false, typeof(ArgumentException), "value")),
            new("null-value", (null, T1000, T1200, Inclusion.Inclusive), new GuardExpected(false, typeof(ArgumentNullException), "value"))
        ];
    }

    public static class NotWithinTimeOnly
    {
        public static TheoryData<GuardCase<(string? value, string? reference, TimeSpan window)>> Cases =>
        [
            new("within", ("12:05", "12:00", TimeSpan.FromMinutes(10)), new GuardExpected(true)),
            new("outside", ("13:00", "12:00", TimeSpan.FromMinutes(10)), new GuardExpected(false, typeof(ArgumentException), "value")),
            new("null-value", (null, "12:00", TimeSpan.FromMinutes(10)), new GuardExpected(false, typeof(ArgumentNullException), "value"))
        ];
    }

    public static class WithinTimeOnly
    {
        public static TheoryData<GuardCase<(string? value, string? reference, TimeSpan window)>> Cases =>
        [
            new("outside", ("13:00", "12:00", TimeSpan.FromMinutes(10)), new GuardExpected(true)),
            new("within", ("12:05", "12:00", TimeSpan.FromMinutes(10)), new GuardExpected(false, typeof(ArgumentException), "value")),
            new("null-value", (null, "12:00", TimeSpan.FromMinutes(10)), new GuardExpected(false, typeof(ArgumentNullException), "value"))
        ];
    }

    public static class BeforeTimeOnly
    {
        public static TheoryData<GuardCase<(string? value, TimeOnly other)>> Cases =>
        [
            new("after", ("13:00", T1200), new GuardExpected(true)),
            new("before", ("10:00", T1200), new GuardExpected(false, typeof(ArgumentException), "value")),
            new("null-value", (null, T1200), new GuardExpected(false, typeof(ArgumentNullException), "value"))
        ];
    }

    public static class OnOrBeforeTimeOnly
    {
        public static TheoryData<GuardCase<(string? value, TimeOnly other)>> Cases =>
        [
            new("after", ("13:00", T1200), new GuardExpected(true)),
            new("before", ("10:00", T1200), new GuardExpected(false, typeof(ArgumentException), "value")),
            new("on", ("12:00", T1200), new GuardExpected(false, typeof(ArgumentException), "value")),
            new("null-value", (null, T1200), new GuardExpected(false, typeof(ArgumentNullException), "value"))
        ];
    }

    public static class NotBeforeTimeOnly
    {
        public static TheoryData<GuardCase<(string? value, TimeOnly other)>> Cases =>
        [
            new("before", ("10:00", T1200), new GuardExpected(true)),
            new("after", ("13:00", T1200), new GuardExpected(false, typeof(ArgumentException), "value")),
            new("null-value", (null, T1200), new GuardExpected(false, typeof(ArgumentNullException), "value"))
        ];
    }

    public static class NotOnOrBeforeTimeOnly
    {
        public static TheoryData<GuardCase<(string? value, TimeOnly other)>> Cases =>
        [
            new("on", ("12:00", T1200), new GuardExpected(true)),
            new("after", ("13:00", T1200), new GuardExpected(false, typeof(ArgumentException), "value")),
            new("null-value", (null, T1200), new GuardExpected(false, typeof(ArgumentNullException), "value"))
        ];
    }

    public static class AfterTimeOnly
    {
        public static TheoryData<GuardCase<(string? value, TimeOnly other)>> Cases =>
        [
            new("before", ("10:00", T1200), new GuardExpected(true)),
            new("after", ("13:00", T1200), new GuardExpected(false, typeof(ArgumentException), "value")),
            new("null-value", (null, T1200), new GuardExpected(false, typeof(ArgumentNullException), "value"))
        ];
    }

    public static class OnOrAfterTimeOnly
    {
        public static TheoryData<GuardCase<(string? value, TimeOnly other)>> Cases =>
        [
            new("before", ("10:00", T1200), new GuardExpected(true)),
            new("after", ("13:00", T1200), new GuardExpected(false, typeof(ArgumentException), "value")),
            new("on", ("12:00", T1200), new GuardExpected(false, typeof(ArgumentException), "value")),
            new("null-value", (null, T1200), new GuardExpected(false, typeof(ArgumentNullException), "value"))
        ];
    }

    public static class NotAfterTimeOnly
    {
        public static TheoryData<GuardCase<(string? value, TimeOnly other)>> Cases =>
        [
            new("after", ("13:00", T1200), new GuardExpected(true)),
            new("before", ("10:00", T1200), new GuardExpected(false, typeof(ArgumentException), "value")),
            new("on", ("12:00", T1200), new GuardExpected(false, typeof(ArgumentException), "value")),
            new("null-value", (null, T1200), new GuardExpected(false, typeof(ArgumentNullException), "value"))
        ];
    }

    public static class NotOnOrAfterTimeOnly
    {
        public static TheoryData<GuardCase<(string? value, TimeOnly other)>> Cases =>
        [
            new("on", ("12:00", T1200), new GuardExpected(true)),
            new("before", ("10:00", T1200), new GuardExpected(false, typeof(ArgumentException), "value")),
            new("null-value", (null, T1200), new GuardExpected(false, typeof(ArgumentNullException), "value"))
        ];
    }

    public static class SameTimeOnly
    {
        public static TheoryData<GuardCase<(string? value, TimeOnly other)>> Cases =>
        [
            new("different", ("11:00", T1200), new GuardExpected(true)),
            new("same", ("12:00", T1200), new GuardExpected(false, typeof(ArgumentException), "value")),
            new("null-value", (null, T1200), new GuardExpected(false, typeof(ArgumentNullException), "value"))
        ];
    }

    public static class NotSameTimeOnly
    {
        public static TheoryData<GuardCase<(string? value, TimeOnly other)>> Cases =>
        [
            new("same", ("12:00", T1200), new GuardExpected(true)),
            new("different", ("11:00", T1200), new GuardExpected(false, typeof(ArgumentException), "value")),
            new("null-value", (null, T1200), new GuardExpected(false, typeof(ArgumentNullException), "value"))
        ];
    }

    public static class ChronologicalTimeOnly
    {
        public static TheoryData<GuardCase<(string? start, string? end)>> Cases =>
        [
            new("not-chrono", ("13:00", "12:00"), new GuardExpected(true)),
            new("chrono", ("10:00", "12:00"), new GuardExpected(false, typeof(ArgumentException), "start")),
            new("null-start", (null, "12:00"), new GuardExpected(false, typeof(ArgumentNullException), "start"))
        ];
    }

    public static class NotChronologicalTimeOnly
    {
        public static TheoryData<GuardCase<(string? start, string? end)>> Cases =>
        [
            new("chrono", ("10:00", "12:00"), new GuardExpected(true)),
            new("not-chrono", ("13:00", "12:00"), new GuardExpected(false, typeof(ArgumentException), "start")),
            new("null-start", (null, "12:00"), new GuardExpected(false, typeof(ArgumentNullException), "start"))
        ];
    }

    public static class OverlappingTimeOnly
    {
        public static TheoryData<GuardCase<(string? start1, string? end1, string? start2, string? end2)>> Cases =>
        [
            new("disjoint", ("10:00", "11:00", "12:00", "13:00"), new GuardExpected(true)),
            new("overlapping", ("10:00", "12:00", "11:00", "13:00"), new GuardExpected(false, typeof(ArgumentException), "start1")),
            new("null-start1", (null, "12:00", "11:00", "13:00"), new GuardExpected(false, typeof(ArgumentNullException), "start1"))
        ];
    }

    public static class NotOverlappingTimeOnly
    {
        public static TheoryData<GuardCase<(string? start1, string? end1, string? start2, string? end2)>> Cases =>
        [
            new("overlapping", ("10:00", "12:00", "11:00", "13:00"), new GuardExpected(true)),
            new("disjoint", ("10:00", "11:00", "12:00", "13:00"), new GuardExpected(false, typeof(ArgumentException), "start1")),
            new("null-start1", (null, "12:00", "11:00", "13:00"), new GuardExpected(false, typeof(ArgumentNullException), "start1"))
        ];
    }
}
