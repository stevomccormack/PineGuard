using PineGuard.Codes;
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

    public static class BetweenDateTimeOffsetString
    {
        public static TheoryData<DataAnnotationCase> Cases =>
            F.DateTimeOffsetIsBetween.AllScenarios.ToDataAnnotationCases(
                s => s.value,
                s => s.Name switch
                {
                    nameof(F.DateTimeOffsetIsBetween.NullValue) => new DataAnnotationExpected(true),
                    nameof(F.DateTimeOffsetIsBetween.MinExclusive) => new DataAnnotationExpected(true), // DA uses Inclusion.Inclusive by default; at min = valid
                    nameof(F.DateTimeOffsetIsBetween.OffsetLessAssumedUtc) => new DataAnnotationExpected(false, "Value must be a date/time within the expected range."), // this test pins a fixed 2020 window; the fixture's 2024 value falls outside it (assume-UTC parsing is pinned by BetweenDateTimeOffsetString_OffsetLessValue_IsAssumedUtc)
                    _ when s.IsValid => new DataAnnotationExpected(true),
                    _ => new DataAnnotationExpected(false, "Value must be a date/time within the expected range.")
                });
    }

    public static class BetweenDateTimeOffsetStringAssumeUtc
    {
        public static TheoryData<DataAnnotationCase> Cases =>
        [
            new("offset-less value assumed utc", F.DateTimeOffsetIsBetween.OffsetLessAssumedUtc.value, new DataAnnotationExpected(true)),
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
}
