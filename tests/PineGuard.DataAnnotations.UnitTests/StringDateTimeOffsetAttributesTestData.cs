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
            _ => new DataAnnotationExpected(false, "Value must be a date/time in the past.")
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
                    _ when s.IsValid => new DataAnnotationExpected(true),
                    _ => new DataAnnotationExpected(false, "Value must be a date/time within the expected range.")
                });
    }
}
