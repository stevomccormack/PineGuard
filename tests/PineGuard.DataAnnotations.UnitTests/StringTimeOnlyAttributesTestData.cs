using PineGuard.Testing.UnitTests.DataAnnotations;
using F = PineGuard.Testing.Fixtures.StringRulesFixtures;

namespace PineGuard.DataAnnotations.UnitTests;

public static class StringTimeOnlyAttributesTestData
{
    private static readonly string Ref = F.StringTimeOnly.IsSame.ExactMatch.value!;
    private static readonly string Past = F.StringTimeOnly.IsBefore.BeforeExclusive.value!;
    private static readonly string Future = F.StringTimeOnly.IsAfter.AfterExclusive.value!;

    public static class BetweenTimeOnlyString
    {
        public static TheoryData<DataAnnotationCase> Cases =>
        [
            new("in-range", Ref, new DataAnnotationExpected(true)),
            new("out-of-range", new TimeOnly(15, 0).ToString("HH:mm"), new DataAnnotationExpected(false, "Value must be a time within the expected range.")),
            new("unparseable", "invalid", new DataAnnotationExpected(false, "Value must be a time within the expected range.")),
            new("null", null, new DataAnnotationExpected(true))
        ];
    }

    public static class BeforeTimeOnlyString
    {
        public static TheoryData<DataAnnotationCase> Cases =>
        [
            new("before", Past, new DataAnnotationExpected(true)),
            new("after", Future, new DataAnnotationExpected(false, "Value must be a time before the specified time.")),
            new("unparseable", "invalid", new DataAnnotationExpected(false, "Value must be a time before the specified time.")),
            new("null", null, new DataAnnotationExpected(true))
        ];
    }

    public static class AfterTimeOnlyString
    {
        public static TheoryData<DataAnnotationCase> Cases =>
        [
            new("after", Future, new DataAnnotationExpected(true)),
            new("before", Past, new DataAnnotationExpected(false, "Value must be a time after the specified time.")),
            new("unparseable", "invalid", new DataAnnotationExpected(false, "Value must be a time after the specified time.")),
            new("null", null, new DataAnnotationExpected(true))
        ];
    }

    public static class NotBetweenTimeOnlyString
    {
        public static TheoryData<DataAnnotationCase> Cases =>
        [
            new("outside", new TimeOnly(15, 0).ToString("HH:mm"), new DataAnnotationExpected(true)),
            new("inside", Ref, new DataAnnotationExpected(false, "Value must be a time not within the expected range.")),
            new("unparseable", "invalid", new DataAnnotationExpected(false, "Value must be a time not within the expected range.")),
            new("null", null, new DataAnnotationExpected(true))
        ];
    }

    public static class NotBeforeTimeOnlyString
    {
        public static TheoryData<DataAnnotationCase> Cases =>
        [
            new("same", Ref, new DataAnnotationExpected(true)),
            new("after", Future, new DataAnnotationExpected(true)),
            new("before", Past, new DataAnnotationExpected(false, "Value must not be a time before the specified time.")),
            new("unparseable", "invalid", new DataAnnotationExpected(false, "Value must not be a time before the specified time.")),
            new("null", null, new DataAnnotationExpected(true))
        ];
    }

    public static class OnOrBeforeTimeOnlyString
    {
        public static TheoryData<DataAnnotationCase> Cases =>
        [
            new("before", Past, new DataAnnotationExpected(true)),
            new("same", Ref, new DataAnnotationExpected(true)),
            new("after", Future, new DataAnnotationExpected(false, "Value must be a time on or before the specified time.")),
            new("unparseable", "invalid", new DataAnnotationExpected(false, "Value must be a time on or before the specified time.")),
            new("null", null, new DataAnnotationExpected(true))
        ];
    }

    public static class NotOnOrBeforeTimeOnlyString
    {
        public static TheoryData<DataAnnotationCase> Cases =>
        [
            new("after", Future, new DataAnnotationExpected(true)),
            new("same", Ref, new DataAnnotationExpected(false, "Value must not be a time on or before the specified time.")),
            new("before", Past, new DataAnnotationExpected(false, "Value must not be a time on or before the specified time.")),
            new("unparseable", "invalid", new DataAnnotationExpected(false, "Value must not be a time on or before the specified time.")),
            new("null", null, new DataAnnotationExpected(true))
        ];
    }

    public static class NotAfterTimeOnlyString
    {
        public static TheoryData<DataAnnotationCase> Cases =>
        [
            new("before", Past, new DataAnnotationExpected(true)),
            new("same", Ref, new DataAnnotationExpected(true)),
            new("after", Future, new DataAnnotationExpected(false, "Value must not be a time after the specified time.")),
            new("unparseable", "invalid", new DataAnnotationExpected(false, "Value must not be a time after the specified time.")),
            new("null", null, new DataAnnotationExpected(true))
        ];
    }

    public static class OnOrAfterTimeOnlyString
    {
        public static TheoryData<DataAnnotationCase> Cases =>
        [
            new("same", Ref, new DataAnnotationExpected(true)),
            new("after", Future, new DataAnnotationExpected(true)),
            new("before", Past, new DataAnnotationExpected(false, "Value must be a time on or after the specified time.")),
            new("unparseable", "invalid", new DataAnnotationExpected(false, "Value must be a time on or after the specified time.")),
            new("null", null, new DataAnnotationExpected(true))
        ];
    }

    public static class NotOnOrAfterTimeOnlyString
    {
        public static TheoryData<DataAnnotationCase> Cases =>
        [
            new("before", Past, new DataAnnotationExpected(true)),
            new("same", Ref, new DataAnnotationExpected(false, "Value must not be a time on or after the specified time.")),
            new("after", Future, new DataAnnotationExpected(false, "Value must not be a time on or after the specified time.")),
            new("unparseable", "invalid", new DataAnnotationExpected(false, "Value must not be a time on or after the specified time.")),
            new("null", null, new DataAnnotationExpected(true))
        ];
    }

    public static class SameTimeOnlyString
    {
        public static TheoryData<DataAnnotationCase> Cases =>
        [
            new("same", Ref, new DataAnnotationExpected(true)),
            new("before", Past, new DataAnnotationExpected(false, "Value must be a time the same as the specified time.")),
            new("after", Future, new DataAnnotationExpected(false, "Value must be a time the same as the specified time.")),
            new("unparseable", "invalid", new DataAnnotationExpected(false, "Value must be a time the same as the specified time.")),
            new("null", null, new DataAnnotationExpected(true))
        ];
    }

    public static class NotSameTimeOnlyString
    {
        public static TheoryData<DataAnnotationCase> Cases =>
        [
            new("before", Past, new DataAnnotationExpected(true)),
            new("after", Future, new DataAnnotationExpected(true)),
            new("same", Ref, new DataAnnotationExpected(false, "Value must be a time not the same as the specified time.")),
            new("unparseable", "invalid", new DataAnnotationExpected(false, "Value must be a time not the same as the specified time.")),
            new("null", null, new DataAnnotationExpected(true))
        ];
    }

    public static class NotChronologicalTimeOnlyString
    {
        public static TheoryData<DataAnnotationCase> Cases =>
        [
            new("reversed", new TimeOnly(15, 0).ToString("HH:mm"), new DataAnnotationExpected(true)),
            new("chronological", Past, new DataAnnotationExpected(false, "Value must not be chronological.")),
            new("unparseable", "invalid", new DataAnnotationExpected(false, "Value must not be chronological.")),
            new("null", null, new DataAnnotationExpected(true))
        ];
    }
}
