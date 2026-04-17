using PineGuard.Common;
using PineGuard.Testing.UnitTests.MustClauses;
using F = PineGuard.Testing.Fixtures.StringRulesFixtures;

namespace PineGuard.MustClauses.UnitTests;

public static class MustStringTimeOnlyClausesTestData
{
    private static readonly TimeOnly T1000 = F.StringTimeOnly.IsBetween.InRangeInclusive.min;
    private static readonly TimeOnly T1200 = F.StringTimeOnly.IsBetween.InRangeInclusive.max;
    private static readonly TimeSpan Win = F.StringTimeOnly.IsWithin.WithinWindow.window;

    public static class BetweenTimeOnly
    {
        public static TheoryData<MustCase<(string? value, TimeOnly min, TimeOnly max)>> Cases =>
        [
            new("in-range", ("11:00", T1000, T1200), new MustExpected(true)),
            new("out-of-range", ("13:00", T1000, T1200), new MustExpected(false, "value must be a time within the expected range.")),
            new("null-value", (null, T1000, T1200), new MustExpected(false, "value must not be null.", "value")),
            new("unparseable", ("invalid", T1000, T1200), new MustExpected(false, "value must be a time within the expected range.")),
            new("min-gt-max", ("11:00", T1200, T1000), new MustExpected(false, "min must be less than or equal to max.", "min"))
        ];
    }

    public static class NotBetweenTimeOnly
    {
        public static TheoryData<MustCase<(string? value, TimeOnly min, TimeOnly max)>> Cases =>
        [
            new("out-of-range", ("13:00", T1000, T1200), new MustExpected(true)),
            new("in-range", ("11:00", T1000, T1200), new MustExpected(false, "value must be a time not within the expected range.")),
            new("null-value", (null, T1000, T1200), new MustExpected(false, "value must not be null.", "value")),
            new("unparseable", ("invalid", T1000, T1200), new MustExpected(false, "value must be a time not within the expected range.")),
            new("min-gt-max", ("11:00", T1200, T1000), new MustExpected(false, "min must be less than or equal to max.", "min"))
        ];
    }

    public static class WithinTimeOnly
    {
        public static TheoryData<MustCase<(string? value, string? reference, TimeSpan window)>> Cases =>
        [
            new("within", ("12:15", "12:00", Win), new MustExpected(true)),
            new("outside", ("13:00", "12:00", Win), new MustExpected(false, "value must be a time within the expected time window.")),
            new("null-value", (null, "12:00", Win), new MustExpected(false, "value must not be null.", "value")),
            new("null-reference", ("12:00", null, Win), new MustExpected(false, "reference must not be null.", "reference")),
            new("negative-window", ("12:00", "12:00", TimeSpan.FromMinutes(-1)), new MustExpected(false, "window requires a non-negative time window.", "window")),
            new("unparseable", ("invalid", "12:00", Win), new MustExpected(false, "value must be a time within the expected time window.")),
            new("unparseable-reference", ("12:00", "invalid", Win), new MustExpected(false, "value must be a time within the expected time window."))
        ];
    }

    public static class NotWithinTimeOnly
    {
        public static TheoryData<MustCase<(string? value, string? reference, TimeSpan window)>> Cases =>
        [
            new("outside", ("13:00", "12:00", Win), new MustExpected(true)),
            new("within", ("12:15", "12:00", Win), new MustExpected(false, "value must be a time not within the expected time window.")),
            new("null-value", (null, "12:00", Win), new MustExpected(false, "value must not be null.", "value")),
            new("null-reference", ("12:00", null, Win), new MustExpected(false, "reference must not be null.", "reference")),
            new("negative-window", ("12:00", "12:00", TimeSpan.FromMinutes(-1)), new MustExpected(false, "window requires a non-negative time window.", "window")),
            new("unparseable", ("invalid", "12:00", Win), new MustExpected(false, "value must be a time not within the expected time window.")),
            new("unparseable-reference", ("12:00", "invalid", Win), new MustExpected(false, "value must be a time not within the expected time window."))
        ];
    }

    public static class BeforeTimeOnly
    {
        public static TheoryData<MustCase<(string? value, TimeOnly other, TimePrecision? precision)>> Cases =>
        [
            new("before", ("11:00", T1200, null), new MustExpected(true)),
            new("after", ("13:00", T1200, null), new MustExpected(false, "value must be a time before the specified time.")),
            new("null-value", (null, T1200, null), new MustExpected(false, "value must not be null.", "value")),
            new("unparseable", ("invalid", T1200, null), new MustExpected(false, "value must be a time before the specified time.")),
            new("invalid-precision", ("11:00", T1200, (TimePrecision)999), new MustExpected(false, "precision has an invalid time precision.", "precision"))
        ];
    }

    public static class OnOrBeforeTimeOnly
    {
        public static TheoryData<MustCase<(string? value, TimeOnly other, TimePrecision? precision)>> Cases =>
        [
            new("before", ("11:00", T1200, null), new MustExpected(true)),
            new("on", ("12:00", T1200, null), new MustExpected(true)),
            new("after", ("13:00", T1200, null), new MustExpected(false, "value must be a time on or before the specified time.")),
            new("null-value", (null, T1200, null), new MustExpected(false, "value must not be null.", "value")),
            new("unparseable", ("invalid", T1200, null), new MustExpected(false, "value must be a time on or before the specified time.")),
            new("invalid-precision", ("11:00", T1200, (TimePrecision)999), new MustExpected(false, "precision has an invalid time precision.", "precision"))
        ];
    }

    public static class NotBeforeTimeOnly
    {
        public static TheoryData<MustCase<(string? value, TimeOnly other, TimePrecision? precision)>> Cases =>
        [
            new("after", ("13:00", T1200, null), new MustExpected(true)),
            new("on", ("12:00", T1200, null), new MustExpected(true)),
            new("before", ("11:00", T1200, null), new MustExpected(false, "value must not be a time before the specified time.")),
            new("null-value", (null, T1200, null), new MustExpected(false, "value must not be null.", "value")),
            new("unparseable", ("invalid", T1200, null), new MustExpected(false, "value must not be a time before the specified time.")),
            new("invalid-precision", ("11:00", T1200, (TimePrecision)999), new MustExpected(false, "precision has an invalid time precision.", "precision"))
        ];
    }

    public static class NotOnOrBeforeTimeOnly
    {
        public static TheoryData<MustCase<(string? value, TimeOnly other, TimePrecision? precision)>> Cases =>
        [
            new("after", ("13:00", T1200, null), new MustExpected(true)),
            new("on", ("12:00", T1200, null), new MustExpected(false, "value must not be a time on or before the specified time.")),
            new("before", ("11:00", T1200, null), new MustExpected(false, "value must not be a time on or before the specified time.")),
            new("null-value", (null, T1200, null), new MustExpected(false, "value must not be null.", "value")),
            new("unparseable", ("invalid", T1200, null), new MustExpected(false, "value must not be a time on or before the specified time.")),
            new("invalid-precision", ("11:00", T1200, (TimePrecision)999), new MustExpected(false, "precision has an invalid time precision.", "precision"))
        ];
    }

    public static class AfterTimeOnly
    {
        public static TheoryData<MustCase<(string? value, TimeOnly other, TimePrecision? precision)>> Cases =>
        [
            new("after", ("13:00", T1200, null), new MustExpected(true)),
            new("before", ("11:00", T1200, null), new MustExpected(false, "value must be a time after the specified time.")),
            new("null-value", (null, T1200, null), new MustExpected(false, "value must not be null.", "value")),
            new("unparseable", ("invalid", T1200, null), new MustExpected(false, "value must be a time after the specified time.")),
            new("invalid-precision", ("13:00", T1200, (TimePrecision)999), new MustExpected(false, "precision has an invalid time precision.", "precision"))
        ];
    }

    public static class OnOrAfterTimeOnly
    {
        public static TheoryData<MustCase<(string? value, TimeOnly other, TimePrecision? precision)>> Cases =>
        [
            new("after", ("13:00", T1200, null), new MustExpected(true)),
            new("on", ("12:00", T1200, null), new MustExpected(true)),
            new("before", ("11:00", T1200, null), new MustExpected(false, "value must be a time on or after the specified time.")),
            new("null-value", (null, T1200, null), new MustExpected(false, "value must not be null.", "value")),
            new("unparseable", ("invalid", T1200, null), new MustExpected(false, "value must be a time on or after the specified time.")),
            new("invalid-precision", ("13:00", T1200, (TimePrecision)999), new MustExpected(false, "precision has an invalid time precision.", "precision"))
        ];
    }

    public static class NotAfterTimeOnly
    {
        public static TheoryData<MustCase<(string? value, TimeOnly other, TimePrecision? precision)>> Cases =>
        [
            new("before", ("11:00", T1200, null), new MustExpected(true)),
            new("on", ("12:00", T1200, null), new MustExpected(true)),
            new("after", ("13:00", T1200, null), new MustExpected(false, "value must not be a time after the specified time.")),
            new("null-value", (null, T1200, null), new MustExpected(false, "value must not be null.", "value")),
            new("unparseable", ("invalid", T1200, null), new MustExpected(false, "value must not be a time after the specified time.")),
            new("invalid-precision", ("13:00", T1200, (TimePrecision)999), new MustExpected(false, "precision has an invalid time precision.", "precision"))
        ];
    }

    public static class NotOnOrAfterTimeOnly
    {
        public static TheoryData<MustCase<(string? value, TimeOnly other, TimePrecision? precision)>> Cases =>
        [
            new("before", ("11:00", T1200, null), new MustExpected(true)),
            new("on", ("12:00", T1200, null), new MustExpected(false, "value must not be a time on or after the specified time.")),
            new("after", ("13:00", T1200, null), new MustExpected(false, "value must not be a time on or after the specified time.")),
            new("null-value", (null, T1200, null), new MustExpected(false, "value must not be null.", "value")),
            new("unparseable", ("invalid", T1200, null), new MustExpected(false, "value must not be a time on or after the specified time.")),
            new("invalid-precision", ("11:00", T1200, (TimePrecision)999), new MustExpected(false, "precision has an invalid time precision.", "precision"))
        ];
    }

    public static class SameTimeOnly
    {
        public static TheoryData<MustCase<(string? value, TimeOnly other, TimePrecision? precision)>> Cases =>
        [
            new("same", ("12:00", T1200, null), new MustExpected(true)),
            new("different", ("11:00", T1200, null), new MustExpected(false, "value must be a time the same as the specified time.")),
            new("null-value", (null, T1200, null), new MustExpected(false, "value must not be null.", "value")),
            new("unparseable", ("invalid", T1200, null), new MustExpected(false, "value must be a time the same as the specified time.")),
            new("invalid-precision", ("12:00", T1200, (TimePrecision)999), new MustExpected(false, "precision has an invalid time precision.", "precision"))
        ];
    }

    public static class NotSameTimeOnly
    {
        public static TheoryData<MustCase<(string? value, TimeOnly other, TimePrecision? precision)>> Cases =>
        [
            new("different", ("11:00", T1200, null), new MustExpected(true)),
            new("same", ("12:00", T1200, null), new MustExpected(false, "value must be a time not the same as the specified time.")),
            new("null-value", (null, T1200, null), new MustExpected(false, "value must not be null.", "value")),
            new("unparseable", ("invalid", T1200, null), new MustExpected(false, "value must be a time not the same as the specified time.")),
            new("invalid-precision", ("12:00", T1200, (TimePrecision)999), new MustExpected(false, "precision has an invalid time precision.", "precision"))
        ];
    }

    public static class ChronologicalTimeOnly
    {
        public static TheoryData<MustCase<(string? start, string? end)>> Cases =>
        [
            new("chrono", ("12:00", "13:00"), new MustExpected(true)),
            new("not-chrono", ("13:00", "12:00"), new MustExpected(false, "start must be chronological.")),
            new("null-start", (null, "13:00"), new MustExpected(false, "start must not be null.", "start")),
            new("null-end", ("12:00", null), new MustExpected(false, "end must not be null.", "end")),
            new("unparseable-start", ("invalid", "13:00"), new MustExpected(false, "start must be chronological.")),
            new("unparseable-end", ("12:00", "invalid"), new MustExpected(false, "end must be chronological.", "end"))
        ];
    }

    public static class NotChronologicalTimeOnly
    {
        public static TheoryData<MustCase<(string? start, string? end)>> Cases =>
        [
            new("not-chrono", ("13:00", "12:00"), new MustExpected(true)),
            new("chrono", ("12:00", "13:00"), new MustExpected(false, "start must not be chronological.")),
            new("null-start", (null, "13:00"), new MustExpected(false, "start must not be null.", "start")),
            new("null-end", ("12:00", null), new MustExpected(false, "end must not be null.", "end")),
            new("unparseable-start", ("invalid", "13:00"), new MustExpected(false, "start must not be chronological.")),
            new("unparseable-end", ("12:00", "invalid"), new MustExpected(false, "end must not be chronological.", "end"))
        ];
    }

    public static class OverlappingTimeOnly
    {
        public static TheoryData<MustCase<(string? start1, string? end1, string? start2, string? end2)>> Cases =>
        [
            new("overlapping", ("10:00", "12:00", "11:00", "13:00"), new MustExpected(true)),
            new("disjoint", ("10:00", "11:00", "12:00", "13:00"), new MustExpected(false, "start1 must be overlapping.", "start1")),
            new("null-start1", (null, "12:00", "11:00", "13:00"), new MustExpected(false, "start1 must not be null.", "start1")),
            new("null-end1", ("10:00", null, "11:00", "13:00"), new MustExpected(false, "end1 must not be null.", "end1")),
            new("null-start2", ("10:00", "12:00", null, "13:00"), new MustExpected(false, "start2 must not be null.", "start2")),
            new("null-end2", ("10:00", "12:00", "11:00", null), new MustExpected(false, "end2 must not be null.", "end2")),
            new("unparseable-start1", ("invalid", "12:00", "11:00", "13:00"), new MustExpected(false, "start1 must be overlapping.", "start1")),
            new("unparseable-end1", ("10:00", "invalid", "11:00", "13:00"), new MustExpected(false, "end1 must be overlapping.", "end1")),
            new("unparseable-start2", ("10:00", "12:00", "invalid", "13:00"), new MustExpected(false, "start2 must be overlapping.", "start2")),
            new("unparseable-end2", ("10:00", "12:00", "11:00", "invalid"), new MustExpected(false, "end2 must be overlapping.", "end2"))
        ];
    }

    public static class NotOverlappingTimeOnly
    {
        public static TheoryData<MustCase<(string? start1, string? end1, string? start2, string? end2)>> Cases =>
        [
            new("disjoint", ("10:00", "11:00", "12:00", "13:00"), new MustExpected(true)),
            new("overlapping", ("10:00", "12:00", "11:00", "13:00"), new MustExpected(false, "start1 must not be overlapping.", "start1")),
            new("null-start1", (null, "12:00", "11:00", "13:00"), new MustExpected(false, "start1 must not be null.", "start1")),
            new("null-end1", ("10:00", null, "11:00", "13:00"), new MustExpected(false, "end1 must not be null.", "end1")),
            new("null-start2", ("10:00", "12:00", null, "13:00"), new MustExpected(false, "start2 must not be null.", "start2")),
            new("null-end2", ("10:00", "12:00", "11:00", null), new MustExpected(false, "end2 must not be null.", "end2")),
            new("unparseable-start1", ("invalid", "12:00", "11:00", "13:00"), new MustExpected(false, "start1 must not be overlapping.", "start1")),
            new("unparseable-end1", ("10:00", "invalid", "11:00", "13:00"), new MustExpected(false, "end1 must not be overlapping.", "end1")),
            new("unparseable-start2", ("10:00", "12:00", "invalid", "13:00"), new MustExpected(false, "start2 must not be overlapping.", "start2")),
            new("unparseable-end2", ("10:00", "12:00", "11:00", "invalid"), new MustExpected(false, "end2 must not be overlapping.", "end2"))
        ];
    }
}
