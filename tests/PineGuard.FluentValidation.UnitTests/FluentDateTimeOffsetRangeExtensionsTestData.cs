using PineGuard.Codes;
using PineGuard.Common;
using PineGuard.Testing.UnitTests.FluentValidation;

namespace PineGuard.FluentValidation.UnitTests;

public static class FluentDateTimeOffsetRangeExtensionsTestData
{
    private static readonly DateTimeOffset Now = DateTimeOffset.UtcNow;
    private static readonly DateTimeOffset Tomorrow = Now.AddDays(1);
    private static readonly DateTimeOffset Yesterday = Now.AddDays(-1);
    private static readonly DateTimeOffsetRange RangeNow = new(Now, Now);
    private static readonly DateTimeOffsetRange RangeTomorrow = new(Tomorrow, Tomorrow);
    private static readonly DateTimeOffsetRange RangeYesterday = new(Yesterday, Yesterday);
    private static readonly DateTimeOffsetRange RangeStandard = new(Now, Tomorrow);

    public static class Chronological
    {
        public static TheoryData<FluentCase<DateTimeOffsetRange?>> Cases =>
        [
            new("Valid range", RangeStandard, new FluentExpected(true)),
            new("Null", null, new FluentExpected(true)),
            new("Zero duration", RangeNow, new FluentExpected(false, "Value must be chronological."))
        ];
    }

    public static class Overlapping
    {
        public static TheoryData<FluentCase<(DateTimeOffsetRange? value, DateTimeOffsetRange other)>> Cases =>
        [
            new("Overlapping", (RangeStandard, RangeStandard), new FluentExpected(true)),
            new("Null", (null, RangeStandard), new FluentExpected(true)),
            new("Not overlapping", (RangeYesterday, RangeTomorrow), new FluentExpected(false, "Value must be overlapping."))
        ];
    }

    public static class NotOverlapping
    {
        public static TheoryData<FluentCase<(DateTimeOffsetRange? value, DateTimeOffsetRange other)>> Cases =>
        [
            new("Not overlapping", (RangeYesterday, RangeTomorrow), new FluentExpected(true)),
            new("Null", (null, RangeStandard), new FluentExpected(true)),
            new("Overlapping", (RangeStandard, RangeStandard), new FluentExpected(false, "Value must not be overlapping."))
        ];
    }

    public static class Contains
    {
        public static TheoryData<FluentCase<(DateTimeOffsetRange? value, DateTimeOffset item)>> Cases =>
        [
            new("Contains", (RangeNow, Now), new FluentExpected(true)),
            new("Contains tomorrow", (RangeTomorrow, Tomorrow), new FluentExpected(true)),
            new("Null", (null, Now), new FluentExpected(true)),
            new("Does not contain", (RangeNow, Tomorrow), new FluentExpected(false, "Value must contain the specified date/time."))
        ];
    }

    public static class NotContains
    {
        public static TheoryData<FluentCase<(DateTimeOffsetRange? value, DateTimeOffset item)>> Cases =>
        [
            new("Does not contain", (RangeNow, Tomorrow), new FluentExpected(true)),
            new("Null", (null, Now), new FluentExpected(true)),
            new("Contains", (RangeNow, Now), new FluentExpected(false, "Value must not contain the specified date/time."))
        ];
    }

    // ── Non-nullable overloads ─────────────────────────────────────────────

    public static class ChronologicalNonNullable
    {
        public static TheoryData<FluentCase<DateTimeOffsetRange>> Cases =>
        [
            new("Valid range",   RangeStandard, new FluentExpected(true)),
            new("Zero duration", RangeNow,      new FluentExpected(false, "Value must be chronological.", Code: MustCodes.Range.Order.NotChronological))
        ];
    }

    public static class OverlappingNonNullable
    {
        public static TheoryData<FluentCase<(DateTimeOffsetRange value, DateTimeOffsetRange other)>> Cases =>
        [
            new("Overlapping",     (RangeStandard,  RangeStandard),  new FluentExpected(true)),
            new("Not overlapping", (RangeYesterday, RangeTomorrow),  new FluentExpected(false, "Value must be overlapping."))
        ];
    }

    public static class NotOverlappingNonNullable
    {
        public static TheoryData<FluentCase<(DateTimeOffsetRange value, DateTimeOffsetRange other)>> Cases =>
        [
            new("Not overlapping", (RangeYesterday, RangeTomorrow),  new FluentExpected(true)),
            new("Overlapping",     (RangeStandard,  RangeStandard),  new FluentExpected(false, "Value must not be overlapping."))
        ];
    }

    public static class ContainsNonNullable
    {
        public static TheoryData<FluentCase<(DateTimeOffsetRange value, DateTimeOffset item)>> Cases =>
        [
            new("Contains",         (RangeNow, Now),      new FluentExpected(true)),
            new("Does not contain", (RangeNow, Tomorrow), new FluentExpected(false, "Value must contain the specified date/time."))
        ];
    }

    public static class NotContainsNonNullable
    {
        public static TheoryData<FluentCase<(DateTimeOffsetRange value, DateTimeOffset item)>> Cases =>
        [
            new("Does not contain", (RangeNow, Tomorrow), new FluentExpected(true)),
            new("Contains",         (RangeNow, Now),      new FluentExpected(false, "Value must not contain the specified date/time."))
        ];
    }
}
