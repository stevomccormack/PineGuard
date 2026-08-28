using PineGuard.Codes;
using PineGuard.Testing.UnitTests.DataAnnotations;
using F = PineGuard.Testing.Fixtures.TimeOnlyRulesFixtures;

namespace PineGuard.DataAnnotations.UnitTests;

public static class TimeOnlyAttributesTestData
{
    private static readonly TimeOnly T11 = F.IsKnownTimes.T1100!.Value;
    private static readonly TimeOnly T12 = F.IsKnownTimes.T1200!.Value;
    private static readonly TimeOnly T13 = F.IsKnownTimes.T1300!.Value;
    private static readonly TimeOnly T0830 = F.IsKnownTimes.T0830!.Value;
    private static readonly TimeOnly T0930 = F.IsKnownTimes.T0930!.Value;

    public static class BetweenTimeOnly
    {
        public static TheoryData<DataAnnotationCase> Cases =>
        [
            new("in-range", T11, new DataAnnotationExpected(true)),
            new("out-range", T13, new DataAnnotationExpected(false, "Value must be within the expected range.", Code: MustCodes.Time.Range.OutOfRange)),
            new("null", null, new DataAnnotationExpected(true))
        ];
    }

    public static class NotBetweenTimeOnly
    {
        public static TheoryData<DataAnnotationCase> Cases =>
        [
            new("out-range", T13, new DataAnnotationExpected(true)),
            new("in-range", T11, new DataAnnotationExpected(false, "Value must not be within the expected range.")),
            new("null", null, new DataAnnotationExpected(true))
        ];
    }

    public static class BeforeTimeOnly
    {
        public static TheoryData<DataAnnotationCase> Cases =>
        [
            new("before", T11, new DataAnnotationExpected(true)),
            new("after", T13, new DataAnnotationExpected(false, "Value must be before the specified time.")),
            new("exact", T12, new DataAnnotationExpected(false, "Value must be before the specified time.")),
            new("null", null, new DataAnnotationExpected(true))
        ];
    }

    public static class AfterTimeOnly
    {
        public static TheoryData<DataAnnotationCase> Cases =>
        [
            new("after", T13, new DataAnnotationExpected(true)),
            new("before", T11, new DataAnnotationExpected(false, "Value must be after the specified time.")),
            new("exact", T12, new DataAnnotationExpected(false, "Value must be after the specified time.")),
            new("null", null, new DataAnnotationExpected(true))
        ];
    }

    public static class OnOrBeforeTimeOnly
    {
        public static TheoryData<DataAnnotationCase> Cases =>
        [
            new("before", T11, new DataAnnotationExpected(true)),
            new("exact", T12, new DataAnnotationExpected(true)),
            new("after", T13, new DataAnnotationExpected(false, "Value must be on or before the specified time.")),
            new("null", null, new DataAnnotationExpected(true))
        ];
    }

    public static class OnOrAfterTimeOnly
    {
        public static TheoryData<DataAnnotationCase> Cases =>
        [
            new("after", T13, new DataAnnotationExpected(true)),
            new("exact", T12, new DataAnnotationExpected(true)),
            new("before", T11, new DataAnnotationExpected(false, "Value must be on or after the specified time.")),
            new("null", null, new DataAnnotationExpected(true))
        ];
    }

    public static class ChronologicalTimeOnly
    {
        public static TheoryData<DataAnnotationCase> Cases =>
        [
            new("before", T11, new DataAnnotationExpected(true)),
            new("after", T13, new DataAnnotationExpected(false, "Value must be chronological.")),
            new("same", T12, new DataAnnotationExpected(false, "Value must be chronological.")),
            new("null", null, new DataAnnotationExpected(true))
        ];
    }

    public static class NotChronologicalTimeOnly
    {
        public static TheoryData<DataAnnotationCase> Cases =>
        [
            new("after", T13, new DataAnnotationExpected(true)),
            new("same", T12, new DataAnnotationExpected(true)),
            new("before", T11, new DataAnnotationExpected(false, "Value must not be chronological.")),
            new("null", null, new DataAnnotationExpected(true))
        ];
    }

    public static class NotBeforeTimeOnly
    {
        public static TheoryData<DataAnnotationCase> Cases =>
        [
            new("after", T13, new DataAnnotationExpected(true)),
            new("exact", T12, new DataAnnotationExpected(true)),
            new("before", T11, new DataAnnotationExpected(false, "Value must not be before the specified time.")),
            new("null", null, new DataAnnotationExpected(true))
        ];
    }

    public static class NotOnOrBeforeTimeOnly
    {
        public static TheoryData<DataAnnotationCase> Cases =>
        [
            new("after", T13, new DataAnnotationExpected(true)),
            new("exact", T12, new DataAnnotationExpected(false, "Value must not be on or before the specified time.")),
            new("before", T11, new DataAnnotationExpected(false, "Value must not be on or before the specified time.")),
            new("null", null, new DataAnnotationExpected(true))
        ];
    }

    public static class NotAfterTimeOnly
    {
        public static TheoryData<DataAnnotationCase> Cases =>
        [
            new("before", T11, new DataAnnotationExpected(true)),
            new("exact", T12, new DataAnnotationExpected(true)),
            new("after", T13, new DataAnnotationExpected(false, "Value must not be after the specified time.")),
            new("null", null, new DataAnnotationExpected(true))
        ];
    }

    public static class NotOnOrAfterTimeOnly
    {
        public static TheoryData<DataAnnotationCase> Cases =>
        [
            new("before", T11, new DataAnnotationExpected(true)),
            new("exact", T12, new DataAnnotationExpected(false, "Value must not be on or after the specified time.")),
            new("after", T13, new DataAnnotationExpected(false, "Value must not be on or after the specified time.")),
            new("null", null, new DataAnnotationExpected(true))
        ];
    }

    public static class OverlappingTimeOnly
    {
        public static TheoryData<DataAnnotationCase> Cases =>
        [
            new("overlaps", T0830, new DataAnnotationExpected(true)),
            new("disjoint", T0930, new DataAnnotationExpected(false, "Value must be overlapping.")),
            new("null", null, new DataAnnotationExpected(true))
        ];
    }

    public static class NotOverlappingTimeOnly
    {
        public static TheoryData<DataAnnotationCase> Cases =>
        [
            new("disjoint", T0930, new DataAnnotationExpected(true)),
            new("overlaps", T0830, new DataAnnotationExpected(false, "Value must not be overlapping.")),
            new("null", null, new DataAnnotationExpected(true))
        ];
    }
}
