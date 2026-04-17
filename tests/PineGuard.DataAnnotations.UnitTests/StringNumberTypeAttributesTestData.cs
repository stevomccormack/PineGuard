using PineGuard.Testing.UnitTests.DataAnnotations;
using F = PineGuard.Testing.Fixtures.StringRulesFixtures;

namespace PineGuard.DataAnnotations.UnitTests;

public static class StringNumberTypeAttributesTestData
{
    public static class DecimalString
    {
        public static TheoryData<DataAnnotationCase> Cases => F.IsDecimal.AllScenarios.ToDataAnnotationCases(s => s.Name switch
        {
            nameof(F.IsDecimal.NullValue) => new DataAnnotationExpected(true),
            _ when s.IsValid => new DataAnnotationExpected(true),
            _ => new DataAnnotationExpected(false, "Value must be a decimal number.")
        });
    }

    public static class ExactDecimalString
    {
        public static TheoryData<DataAnnotationCase> Cases => F.IsExactDecimal.AllScenarios.ToDataAnnotationCases(s => s.Name switch
        {
            nameof(F.IsExactDecimal.NullValue) => new DataAnnotationExpected(true),
            _ when s.IsValid => new DataAnnotationExpected(true),
            _ => new DataAnnotationExpected(false, "Value must be an exact decimal number.")
        });
    }

    public static class Int32String
    {
        public static TheoryData<DataAnnotationCase> Cases => F.IsInt32.AllScenarios.ToDataAnnotationCases(s => s.Name switch
        {
            nameof(F.IsInt32.NullValue) => new DataAnnotationExpected(true),
            _ when s.IsValid => new DataAnnotationExpected(true),
            _ => new DataAnnotationExpected(false, "Value must be a 32-bit integer.")
        });
    }

    public static class Int64String
    {
        public static TheoryData<DataAnnotationCase> Cases => F.IsInt64.AllScenarios.ToDataAnnotationCases(s => s.Name switch
        {
            nameof(F.IsInt64.NullValue) => new DataAnnotationExpected(true),
            _ when s.IsValid => new DataAnnotationExpected(true),
            _ => new DataAnnotationExpected(false, "Value must be a 64-bit integer.")
        });
    }

    public static class Int32InRangeStringInclusive
    {
        public static TheoryData<DataAnnotationCase> Cases =>
        [
            new("between", "5", new DataAnnotationExpected(true)),
            new("at-min", "1", new DataAnnotationExpected(true)),
            new("not-numeric", "not", new DataAnnotationExpected(false, "Value must be a 32-bit integer within the expected range.")),
            new("null", null, new DataAnnotationExpected(true))
        ];
    }

    public static class Int32InRangeStringExclusive
    {
        public static TheoryData<DataAnnotationCase> Cases =>
        [
            new("between", "5", new DataAnnotationExpected(true)),
            new("at-min-excluded", "1", new DataAnnotationExpected(false, "Value must be a 32-bit integer within the expected range."))
        ];
    }

    public static class Int32OutOfRangeString
    {
        public static TheoryData<DataAnnotationCase> Cases =>
        [
            new("outside", "11", new DataAnnotationExpected(true)),
            new("inside-fails", "5", new DataAnnotationExpected(false, "Value must be a 32-bit integer out of the expected range.")),
            new("not-numeric", "abc", new DataAnnotationExpected(false, "Value must be a 32-bit integer out of the expected range.")),
            new("null", null, new DataAnnotationExpected(true))
        ];
    }

    public static class Int64InRangeStringInclusive
    {
        public static TheoryData<DataAnnotationCase> Cases =>
        [
            new("between", "5", new DataAnnotationExpected(true)),
            new("at-min", "1", new DataAnnotationExpected(true)),
            new("not-numeric", "not", new DataAnnotationExpected(false, "Value must be a 64-bit integer within the expected range.")),
            new("null", null, new DataAnnotationExpected(true))
        ];
    }

    public static class Int64InRangeStringExclusive
    {
        public static TheoryData<DataAnnotationCase> Cases =>
        [
            new("between", "5", new DataAnnotationExpected(true)),
            new("at-min-excluded", "1", new DataAnnotationExpected(false, "Value must be a 64-bit integer within the expected range."))
        ];
    }

    public static class Int64OutOfRangeString
    {
        public static TheoryData<DataAnnotationCase> Cases =>
        [
            new("outside", "11", new DataAnnotationExpected(true)),
            new("inside-fails", "5", new DataAnnotationExpected(false, "Value must be a 64-bit integer out of the expected range.")),
            new("not-numeric", "abc", new DataAnnotationExpected(false, "Value must be a 64-bit integer out of the expected range.")),
            new("null", null, new DataAnnotationExpected(true))
        ];
    }
}
