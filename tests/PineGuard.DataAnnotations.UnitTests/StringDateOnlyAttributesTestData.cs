using PineGuard.Codes;
using PineGuard.Testing.UnitTests.DataAnnotations;
using F = PineGuard.Testing.Fixtures.StringRulesFixtures;

namespace PineGuard.DataAnnotations.UnitTests;

public static class StringDateOnlyAttributesTestData
{
    public static class PastDateOnlyString
    {
        public static TheoryData<DataAnnotationCase> Cases => F.DateOnlyIsInPast.AllScenarios.ToDataAnnotationCases(s => s.Name switch
        {
            nameof(F.DateOnlyIsInPast.NullValue) => new DataAnnotationExpected(true),
            _ when s.IsValid => new DataAnnotationExpected(true),
            _ => new DataAnnotationExpected(false, "Value must be a date in the past.", Code: MustCodes.Date.Relative.NotPast)
        });
    }

    public static class FutureDateOnlyString
    {
        public static TheoryData<DataAnnotationCase> Cases => F.DateOnlyIsInFuture.AllScenarios.ToDataAnnotationCases(s => s.Name switch
        {
            nameof(F.DateOnlyIsInFuture.NullValue) => new DataAnnotationExpected(true),
            _ when s.IsValid => new DataAnnotationExpected(true),
            _ => new DataAnnotationExpected(false, "Value must be a date in the future.")
        });
    }

    public static class PastOrPresentDateOnlyString
    {
        public static TheoryData<DataAnnotationCase> Cases => F.DateOnlyIsInPast.AllScenarios.ToDataAnnotationCases(s => s.Name switch
        {
            nameof(F.DateOnlyIsInPast.NullValue) => new DataAnnotationExpected(true),
            _ when s.IsValid => new DataAnnotationExpected(true),
            _ => new DataAnnotationExpected(false, "Value must be a date in the past or present.")
        });
    }

    public static class FutureOrPresentDateOnlyString
    {
        public static TheoryData<DataAnnotationCase> Cases => F.DateOnlyIsInFuture.AllScenarios.ToDataAnnotationCases(s => s.Name switch
        {
            nameof(F.DateOnlyIsInFuture.NullValue) => new DataAnnotationExpected(true),
            _ when s.IsValid => new DataAnnotationExpected(true),
            _ => new DataAnnotationExpected(false, "Value must be a date in the future or present.")
        });
    }

    public static class BeforeDateOnlyString
    {
        public static TheoryData<DataAnnotationCase> Cases =>
        [
            new("before", "2000-01-01", new DataAnnotationExpected(true)),
            new("future", "2999-01-01", new DataAnnotationExpected(false, "Value must be a date before the specified date.")),
            new("not-a-date", "not-a-date", new DataAnnotationExpected(false, "Value must be a date before the specified date.")),
            new("null", null, new DataAnnotationExpected(true))
        ];
    }

    public static class NotBeforeDateOnlyString
    {
        public static TheoryData<DataAnnotationCase> Cases =>
        [
            new("future", "2999-01-01", new DataAnnotationExpected(true)),
            new("past", "2000-01-01", new DataAnnotationExpected(false, "Value must not be a date before the specified date.")),
            new("not-a-date", "not-a-date", new DataAnnotationExpected(false, "Value must not be a date before the specified date.")),
            new("null", null, new DataAnnotationExpected(true))
        ];
    }

    public static class OnOrBeforeDateOnlyString
    {
        public static TheoryData<DataAnnotationCase> Cases =>
        [
            new("before", "2000-01-01", new DataAnnotationExpected(true)),
            new("future", "2999-01-01", new DataAnnotationExpected(false, "Value must be a date on or before the specified date.")),
            new("not-a-date", "not-a-date", new DataAnnotationExpected(false, "Value must be a date on or before the specified date.")),
            new("null", null, new DataAnnotationExpected(true))
        ];
    }

    public static class NotOnOrBeforeDateOnlyString
    {
        public static TheoryData<DataAnnotationCase> Cases =>
        [
            new("future", "2999-01-01", new DataAnnotationExpected(true)),
            new("past", "2000-01-01", new DataAnnotationExpected(false, "Value must not be a date on or before the specified date.")),
            new("not-a-date", "not-a-date", new DataAnnotationExpected(false, "Value must not be a date on or before the specified date.")),
            new("null", null, new DataAnnotationExpected(true))
        ];
    }

    public static class AfterDateOnlyString
    {
        public static TheoryData<DataAnnotationCase> Cases =>
        [
            new("future", "3000-01-01", new DataAnnotationExpected(true)),
            new("past", "2000-01-01", new DataAnnotationExpected(false, "Value must be a date after the specified date.")),
            new("not-a-date", "not-a-date", new DataAnnotationExpected(false, "Value must be a date after the specified date.")),
            new("null", null, new DataAnnotationExpected(true))
        ];
    }

    public static class NotAfterDateOnlyString
    {
        public static TheoryData<DataAnnotationCase> Cases =>
        [
            new("past", "2000-01-01", new DataAnnotationExpected(true)),
            new("future", "2999-01-01", new DataAnnotationExpected(false, "Value must not be a date after the specified date.")),
            new("not-a-date", "not-a-date", new DataAnnotationExpected(false, "Value must not be a date after the specified date.")),
            new("null", null, new DataAnnotationExpected(true))
        ];
    }

    public static class OnOrAfterDateOnlyString
    {
        public static TheoryData<DataAnnotationCase> Cases =>
        [
            new("future", "2999-01-01", new DataAnnotationExpected(true)),
            new("past", "2000-01-01", new DataAnnotationExpected(false, "Value must be a date on or after the specified date.")),
            new("not-a-date", "not-a-date", new DataAnnotationExpected(false, "Value must be a date on or after the specified date.")),
            new("null", null, new DataAnnotationExpected(true))
        ];
    }

    public static class NotOnOrAfterDateOnlyString
    {
        public static TheoryData<DataAnnotationCase> Cases =>
        [
            new("past", "2000-01-01", new DataAnnotationExpected(true)),
            new("future", "2999-01-01", new DataAnnotationExpected(false, "Value must not be a date on or after the specified date.")),
            new("not-a-date", "not-a-date", new DataAnnotationExpected(false, "Value must not be a date on or after the specified date.")),
            new("null", null, new DataAnnotationExpected(true))
        ];
    }

    public static class SameDateOnlyString
    {
        public static TheoryData<DataAnnotationCase> Cases =>
        [
            new("same", "2000-01-01", new DataAnnotationExpected(true)),
            new("different", "2999-01-01", new DataAnnotationExpected(false, "Value must be the same date.")),
            new("not-a-date", "not-a-date", new DataAnnotationExpected(false, "Value must be the same date.")),
            new("null", null, new DataAnnotationExpected(true))
        ];
    }

    public static class NotSameDateOnlyString
    {
        public static TheoryData<DataAnnotationCase> Cases =>
        [
            new("different", "2999-01-01", new DataAnnotationExpected(true)),
            new("same", "2000-01-01", new DataAnnotationExpected(false, "Value must not be the same date.")),
            new("not-a-date", "not-a-date", new DataAnnotationExpected(false, "Value must not be the same date.")),
            new("null", null, new DataAnnotationExpected(true))
        ];
    }

    public static class ChronologicalDateOnlyString
    {
        public static TheoryData<DataAnnotationCase> Cases =>
        [
            new("chronological", "2000-01-01", new DataAnnotationExpected(true)),
            new("non-chronological", "3000-01-01", new DataAnnotationExpected(false, "Value must be chronological.")),
            new("not-a-date", "not-a-date", new DataAnnotationExpected(false, "Value must be chronological.")),
            new("null", null, new DataAnnotationExpected(true))
        ];
    }

    public static class NotChronologicalDateOnlyString
    {
        public static TheoryData<DataAnnotationCase> Cases =>
        [
            new("non-chronological", "3000-01-01", new DataAnnotationExpected(true)),
            new("chronological", "2000-01-01", new DataAnnotationExpected(false, "Value must not be chronological.")),
            new("not-a-date", "not-a-date", new DataAnnotationExpected(false, "Value must not be chronological.")),
            new("null", null, new DataAnnotationExpected(true))
        ];
    }

    public static class OverlappingDateOnlyString
    {
        public static TheoryData<DataAnnotationCase> Cases =>
        [
            new("overlapping", "2020-01-05", new DataAnnotationExpected(true)),
            new("non-overlapping", "2020-07-01", new DataAnnotationExpected(false, "Value must be overlapping.")),
            new("not-a-date", "not-a-date", new DataAnnotationExpected(false, "Value must be overlapping.")),
            new("null", null, new DataAnnotationExpected(true))
        ];
    }

    public static class NotOverlappingDateOnlyString
    {
        public static TheoryData<DataAnnotationCase> Cases =>
        [
            new("non-overlapping", "2020-07-01", new DataAnnotationExpected(true)),
            new("overlapping", "2020-01-05", new DataAnnotationExpected(false, "Value must not be overlapping.")),
            new("not-a-date", "not-a-date", new DataAnnotationExpected(false, "Value must not be overlapping.")),
            new("null", null, new DataAnnotationExpected(true))
        ];
    }
}
