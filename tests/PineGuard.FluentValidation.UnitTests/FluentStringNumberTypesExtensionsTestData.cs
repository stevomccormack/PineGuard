using PineGuard.Testing.UnitTests.FluentValidation;
using F = PineGuard.Testing.Fixtures.StringRulesFixtures;

namespace PineGuard.FluentValidation.UnitTests;

public static class FluentStringNumberTypesExtensionsTestData
{
    public static class Decimal
    {
        public static TheoryData<FluentCase<string?>> Cases => F.IsDecimal.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsDecimal.NullValue) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be a decimal number.")
        });
    }

    public static class ExactDecimal
    {
        public static TheoryData<FluentCase<string?>> Cases => F.IsExactDecimal.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsExactDecimal.NullValue) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be an exact decimal number.")
        });
    }

    public static class Int32
    {
        public static TheoryData<FluentCase<string?>> Cases => F.IsInt32.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsInt32.NullValue) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be a 32-bit integer.")
        });
    }

    public static class Int64
    {
        public static TheoryData<FluentCase<string?>> Cases => F.IsInt64.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsInt64.NullValue) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be a 64-bit integer.")
        });
    }

    public static class Int32InRange
    {
        public static TheoryData<FluentCase<(string text, int min, int max, PineGuard.Common.Inclusion inclusion)>> Cases => F.IsInt32InRange.AllScenarios.ToFluentCases(s => s.Name switch
        {
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be a 32-bit integer within the expected range.")
        });
    }

    public static class Int32InRangeNull
    {
        public static TheoryData<FluentCase<string?>> Cases =>
        [
            new("null-text", null, new FluentExpected(true))
        ];
    }

    public static class Int32OutOfRange
    {
        public static TheoryData<FluentCase<string?>> Cases =>
        [
            new("outside", "11", new FluentExpected(true)),
            new("inside-fails", "5", new FluentExpected(false, "Value must be a 32-bit integer out of the expected range.")),
            new("invalid-fails", "abc", new FluentExpected(false, "Value must be a 32-bit integer out of the expected range.")),
            new("null", null, new FluentExpected(true))
        ];
    }

    public static class Int64InRange
    {
        public static TheoryData<FluentCase<(string text, long min, long max, PineGuard.Common.Inclusion inclusion)>> Cases => F.IsInt64InRange.AllScenarios.ToFluentCases(s => s.Name switch
        {
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be a 64-bit integer within the expected range.")
        });
    }

    public static class Int64InRangeNull
    {
        public static TheoryData<FluentCase<string?>> Cases =>
        [
            new("null-text", null, new FluentExpected(true))
        ];
    }

    public static class Int64OutOfRange
    {
        public static TheoryData<FluentCase<string?>> Cases =>
        [
            new("outside", "11", new FluentExpected(true)),
            new("inside-fails", "5", new FluentExpected(false, "Value must be a 64-bit integer out of the expected range.")),
            new("invalid-fails", "abc", new FluentExpected(false, "Value must be a 64-bit integer out of the expected range.")),
            new("null", null, new FluentExpected(true))
        ];
    }
}
