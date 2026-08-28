using PineGuard.Codes;
using PineGuard.Testing.UnitTests.FluentValidation;
using F = PineGuard.Testing.Fixtures.StringRulesFixtures;

namespace PineGuard.FluentValidation.UnitTests;

public static class FluentStringTimeOnlyExtensionsTestData
{
    private static readonly string S1000 = F.StringTimeOnly.IsChronological.BeforeExclusive.start!;
    private static readonly string S1100 = F.StringTimeOnly.IsBetween.InRangeInclusive.value!;
    private static readonly string S1200 = F.StringTimeOnly.IsSame.ExactMatch.value!;
    private static readonly string S1300 = F.StringTimeOnly.IsAfter.AfterExclusive.value!;

    public static class BetweenTimeOnly
    {
        public static TheoryData<FluentCase<string?>> Cases =>
        [
            new("in-range", S1100, new FluentExpected(true)),
            new("out-of-range", S1300, new FluentExpected(false, "Value must be a time within the expected range.", Code: MustCodes.Time.Range.OutOfRange)),
            new("null", null, new FluentExpected(true))
        ];
    }

    public static class NotBetweenTimeOnly
    {
        public static TheoryData<FluentCase<string?>> Cases =>
        [
            new("out-of-range", S1300, new FluentExpected(true)),
            new("in-range", S1100, new FluentExpected(false, "Value must be a time not within the expected range.")),
            new("null", null, new FluentExpected(true))
        ];
    }

    public static class WithinTimeOnly
    {
        public static TheoryData<FluentCase<string?>> Cases =>
        [
            new("within", S1200, new FluentExpected(true)),
            new("outside", S1300, new FluentExpected(false, "Value must be a time within the expected time window.")),
            new("null", null, new FluentExpected(true))
        ];
    }

    public static class NotWithinTimeOnly
    {
        public static TheoryData<FluentCase<string?>> Cases =>
        [
            new("outside", S1300, new FluentExpected(true)),
            new("within", S1200, new FluentExpected(false, "Value must be a time not within the expected time window.")),
            new("null", null, new FluentExpected(true))
        ];
    }

    public static class BeforeTimeOnly
    {
        public static TheoryData<FluentCase<string?>> Cases =>
        [
            new("before", S1000, new FluentExpected(true)),
            new("after", S1300, new FluentExpected(false, "Value must be a time before the specified time.")),
            new("null", null, new FluentExpected(true))
        ];
    }

    public static class NotBeforeTimeOnly
    {
        public static TheoryData<FluentCase<string?>> Cases =>
        [
            new("after", S1300, new FluentExpected(true)),
            new("before", S1000, new FluentExpected(false, "Value must not be a time before the specified time.")),
            new("null", null, new FluentExpected(true))
        ];
    }

    public static class OnOrBeforeTimeOnly
    {
        public static TheoryData<FluentCase<string?>> Cases =>
        [
            new("before", S1000, new FluentExpected(true)),
            new("on", S1200, new FluentExpected(true)),
            new("after", S1300, new FluentExpected(false, "Value must be a time on or before the specified time.")),
            new("null", null, new FluentExpected(true))
        ];
    }

    public static class NotOnOrBeforeTimeOnly
    {
        public static TheoryData<FluentCase<string?>> Cases =>
        [
            new("after", S1300, new FluentExpected(true)),
            new("on", S1200, new FluentExpected(false, "Value must not be a time on or before the specified time.")),
            new("null", null, new FluentExpected(true))
        ];
    }

    public static class AfterTimeOnly
    {
        public static TheoryData<FluentCase<string?>> Cases =>
        [
            new("after", S1300, new FluentExpected(true)),
            new("before", S1000, new FluentExpected(false, "Value must be a time after the specified time.")),
            new("null", null, new FluentExpected(true))
        ];
    }

    public static class NotAfterTimeOnly
    {
        public static TheoryData<FluentCase<string?>> Cases =>
        [
            new("before", S1000, new FluentExpected(true)),
            new("after", S1300, new FluentExpected(false, "Value must not be a time after the specified time.")),
            new("null", null, new FluentExpected(true))
        ];
    }

    public static class OnOrAfterTimeOnly
    {
        public static TheoryData<FluentCase<string?>> Cases =>
        [
            new("after", S1300, new FluentExpected(true)),
            new("on", S1200, new FluentExpected(true)),
            new("before", S1000, new FluentExpected(false, "Value must be a time on or after the specified time.")),
            new("null", null, new FluentExpected(true))
        ];
    }

    public static class NotOnOrAfterTimeOnly
    {
        public static TheoryData<FluentCase<string?>> Cases =>
        [
            new("before", S1000, new FluentExpected(true)),
            new("on", S1200, new FluentExpected(false, "Value must not be a time on or after the specified time.")),
            new("null", null, new FluentExpected(true))
        ];
    }

    public static class SameTimeOnly
    {
        public static TheoryData<FluentCase<string?>> Cases =>
        [
            new("same", S1200, new FluentExpected(true)),
            new("different", S1000, new FluentExpected(false, "Value must be a time the same as the specified time.")),
            new("null", null, new FluentExpected(true))
        ];
    }

    public static class NotSameTimeOnly
    {
        public static TheoryData<FluentCase<string?>> Cases =>
        [
            new("different", S1000, new FluentExpected(true)),
            new("same", S1200, new FluentExpected(false, "Value must be a time not the same as the specified time.")),
            new("null", null, new FluentExpected(true))
        ];
    }

    public static class ChronologicalTimeOnly
    {
        public static TheoryData<FluentCase<string?>> Cases =>
        [
            new("chrono", S1000, new FluentExpected(true)),
            new("not-chrono", S1300, new FluentExpected(false, "Value must be chronological.")),
            new("null", null, new FluentExpected(true))
        ];
    }

    public static class NotChronologicalTimeOnly
    {
        public static TheoryData<FluentCase<string?>> Cases =>
        [
            new("not-chrono", S1300, new FluentExpected(true)),
            new("chrono", S1000, new FluentExpected(false, "Value must not be chronological.")),
            new("null", null, new FluentExpected(true))
        ];
    }

    public static class OverlappingTimeOnly
    {
        public static TheoryData<FluentCase<string?>> Cases =>
        [
            new("overlapping", "08:30", new FluentExpected(true)),
            new("disjoint", "09:30", new FluentExpected(false, "Value must be overlapping.")),
            new("null", null, new FluentExpected(true))
        ];
    }

    public static class NotOverlappingTimeOnly
    {
        public static TheoryData<FluentCase<string?>> Cases =>
        [
            new("disjoint", "09:30", new FluentExpected(true)),
            new("overlapping", "08:30", new FluentExpected(false, "Value must not be overlapping.")),
            new("null", null, new FluentExpected(true))
        ];
    }
}
