using System.Globalization;
using PineGuard.Codes;
using PineGuard.Common;
using PineGuard.Testing.UnitTests.DataAnnotations;
using F = PineGuard.Testing.Fixtures.StringRulesFixtures;

namespace PineGuard.DataAnnotations.UnitTests;

public static class StringDateTimeOffsetAttributesTestData
{
    public static class PastDateTimeOffsetString
    {
        public static TheoryData<DataAnnotationCase> Cases => F.DateTimeOffsetIsInPast.AllScenarios.ToDataAnnotationCases(s => s.Name switch
        {
            nameof(F.DateTimeOffsetIsInPast.NullValue) => new DataAnnotationExpected(true),
            _ when s.IsValid => new DataAnnotationExpected(true),
            _ => new DataAnnotationExpected(false, "Value must be a date/time in the past.", Code: MustCodes.Date.Relative.NotPast)
        });
    }

    public static class FutureDateTimeOffsetString
    {
        public static TheoryData<DataAnnotationCase> Cases => F.DateTimeOffsetIsInFuture.AllScenarios.ToDataAnnotationCases(s => s.Name switch
        {
            nameof(F.DateTimeOffsetIsInFuture.NullValue) => new DataAnnotationExpected(true),
            _ when s.IsValid => new DataAnnotationExpected(true),
            _ => new DataAnnotationExpected(false, "Value must be a date/time in the future.")
        });
    }

    public static class InRangeDateTimeOffsetString
    {
        public static TheoryData<DataAnnotationCase> Cases =>
            F.DateTimeOffsetIsInRange.AllScenarios.ToDataAnnotationCases(
                s => s.value,
                s => s.Name switch
                {
                    nameof(F.DateTimeOffsetIsInRange.NullValue) => new DataAnnotationExpected(true),
                    nameof(F.DateTimeOffsetIsInRange.MinExclusive) => new DataAnnotationExpected(true), // DA uses Inclusion.Inclusive by default; at min = valid
                    nameof(F.DateTimeOffsetIsInRange.OffsetLessAssumedUtc) => new DataAnnotationExpected(false, "Value must be a date/time within the expected range."), // this test pins a fixed 2020 window; the fixture's 2024 value falls outside it (assume-UTC parsing is pinned by InRangeDateTimeOffsetString_OffsetLessValue_IsAssumedUtc)
                    _ when s.IsValid => new DataAnnotationExpected(true),
                    _ => new DataAnnotationExpected(false, "Value must be a date/time within the expected range.")
                });
    }

    public static class InRangeDateTimeOffsetStringAssumeUtc
    {
        public static TheoryData<DataAnnotationCase> Cases =>
        [
            new("offset-less value assumed utc", F.DateTimeOffsetIsInRange.OffsetLessAssumedUtc.value, new DataAnnotationExpected(true)),
            new("explicit utc offset", "2024-01-15T10:30:00Z", new DataAnnotationExpected(true)),
            new("explicit non-utc offset outside window", "2024-01-15T10:30:00+05:00", new DataAnnotationExpected(false, "Value must be a date/time within the expected range."))
        ];
    }

    // See DateOnlyAttributesTestData for why each row carries its own instant.
    private const string ClockSubject = "2100-01-01T12:00:00+00:00";
    private static readonly DateTimeOffset ClockAfterSubject = new(2200, 01, 01, 12, 0, 0, TimeSpan.Zero);
    private static readonly DateTimeOffset ClockBeforeSubject = new(2000, 01, 01, 12, 0, 0, TimeSpan.Zero);

    public static class PastDateTimeOffsetStringOnAnInjectedClock
    {
        public static TheoryData<DataAnnotationCase> Cases =>
        [
            new("ClockAfterTheSubject", (ClockSubject, ClockAfterSubject), new DataAnnotationExpected(true)),
            new("ClockBeforeTheSubject", (ClockSubject, ClockBeforeSubject), new DataAnnotationExpected(false, "Value must be a date/time in the past.", Code: MustCodes.Date.Relative.NotPast))
        ];
    }

    public static class FutureDateTimeOffsetStringOnAnInjectedClock
    {
        public static TheoryData<DataAnnotationCase> Cases =>
        [
            new("ClockBeforeTheSubject", (ClockSubject, ClockBeforeSubject), new DataAnnotationExpected(true)),
            new("ClockAfterTheSubject", (ClockSubject, ClockAfterSubject), new DataAnnotationExpected(false, "Value must be a date/time in the future.", Code: MustCodes.Date.Relative.NotFuture))
        ];
    }
    public static class OutOfRangeDateTimeOffsetStringConfiguration
    {
        public static TheoryData<DataAnnotationCase> Cases =>
        [
            new("inside", ("2024-06-01", "2024-01-01", "2024-12-31", Inclusion.Inclusive, DateTimeStyles.None), new(false, Code: MustCodes.Date.Range.InRange)),
            new("outside", ("2025-01-01", "2024-01-01", "2024-12-31", Inclusion.Inclusive, DateTimeStyles.None), new(true, Code: MustCodes.Date.Range.InRange)),
            new("boundary-exclusive", ("2024-01-01", "2024-01-01", "2024-12-31", Inclusion.Exclusive, DateTimeStyles.None), new(true, Code: MustCodes.Date.Range.InRange)),
            new("malformed", ("bad", "2024-01-01", "2024-12-31", Inclusion.Inclusive, DateTimeStyles.None), new(false, Code: MustCodes.Date.Range.InRange)),
            new("null", ((string?)null, "2024-01-01", "2024-12-31", Inclusion.Inclusive, DateTimeStyles.None), new(true, Code: MustCodes.Date.Range.InRange)),
            new("reversed", ("2024-06-01", "2024-12-31", "2024-01-01", Inclusion.Inclusive, DateTimeStyles.None), new(false, Code: MustCodes.Date.Range.InRange)),
            new("invalid-inclusion", ("bad", "2024-01-01", "2024-12-31", (Inclusion)999, DateTimeStyles.None), new(false, Code: MustCodes.Date.Range.InRange)),
            new("invalid-styles", ("bad", "2024-01-01", "2024-12-31", Inclusion.Inclusive, (DateTimeStyles)999), new(false, Code: MustCodes.Date.Range.InRange))
        ];
    }
}
