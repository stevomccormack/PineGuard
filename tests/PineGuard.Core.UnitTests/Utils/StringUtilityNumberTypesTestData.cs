using System.Globalization;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Utils;

public static class StringUtilityNumberTypesTestData
{
    public static class TryParseInt32
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("trimmed", " 123 ", true, 123)
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("null", null, false, 0),
            new("space", " ", false, 0),
            new("not a number", "not", false, 0)
        ];

        public sealed record ValidCase(string Name, string? Value, bool Expected, int ExpectedInt32)
            : TryCase<string?, int>(Name, Value, Expected, ExpectedInt32);
    }

    public static class TryParseInt64
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("trimmed", " 123 ", true, 123L)
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("null", null, false, 0),
            new("tab", "\t", false, 0),
            new("not a number", "not", false, 0)
        ];

        public sealed record ValidCase(string Name, string? Value, bool Expected, long ExpectedInt64)
            : TryCase<string?, long>(Name, Value, Expected, ExpectedInt64);
    }

    public static class TryParseDecimal
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("trimmed", " 1.23 ", null, true, 1.23m),
            new("fr-FR comma", "1,23", "fr-FR", true, 1.23m)
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("null", null, null, false, 0),
            new("space", " ", null, false, 0),
            new("comma w/o culture", "1,23", null, false, 0)
        ];

        public sealed record ValidCase(string Name, string? Value, string? CultureName, bool Expected, decimal ExpectedDecimal)
            : TryCase<string?, decimal>(Name, Value, Expected, ExpectedDecimal);
    }

    public static class TryParseSingle
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("trimmed", " 1.5 ", null, true, 1.5f),
            new("fr-FR comma", "1,5", "fr-FR", true, 1.5f)
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("null", null, null, false, 0),
            new("space", " ", null, false, 0),
            new("comma w/o culture", "1,5", null, false, 0)
        ];

        public sealed record ValidCase(string Name, string? Value, string? CultureName, bool Expected, float ExpectedSingle)
            : TryCase<string?, float>(Name, Value, Expected, ExpectedSingle);
    }

    public static class TryParseDouble
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("trimmed", " 1.5 ", null, true, 1.5d),
            new("fr-FR comma", "1,5", "fr-FR", true, 1.5d)
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("null", null, null, false, 0),
            new("space", " ", null, false, 0),
            new("comma w/o culture", "1,5", null, false, 0)
        ];

        public sealed record ValidCase(string Name, string? Value, string? CultureName, bool Expected, double ExpectedDouble)
            : TryCase<string?, double>(Name, Value, Expected, ExpectedDouble);
    }

    public static class TryParseDecimalPlaces
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("within places", " 12.34 ", 2, null, true, 12.34m),
            new("fr-FR comma", "12,34", 2, "fr-FR", true, 12.34m)
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("too many places", "12.345", 2, null, false, 12.345m),
            new("negative places", "12.34", -1, null, false, 0),
            new("null", null, 2, null, false, 0),
            new("not a number", "not", 2, null, false, 0)
        ];

        public sealed record ValidCase(string Name, string? Value, int DecimalPlaces, string? CultureName, bool Expected, decimal ExpectedDecimal)
            : TryCase<string?, decimal>(Name, Value, Expected, ExpectedDecimal);
    }

    public static class TryParseExactDecimal
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("exact places", " 12.34 ", 2, null, true, 12.34m),
            new("fr-FR comma", "12,34", 2, "fr-FR", true, 12.34m)
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("fewer places", "12.3", 2, null, false, 12.3m),
            new("negative places", "12.34", -1, null, false, 0),
            new("null", null, 2, null, false, 0),
            new("not a number", "not", 2, null, false, 0)
        ];

        public sealed record ValidCase(string Name, string? Value, int ExactDecimalPlaces, string? CultureName, bool Expected, decimal ExpectedDecimal)
            : TryCase<string?, decimal>(Name, Value, Expected, ExpectedDecimal);
    }

    public static IFormatProvider? GetProvider(string? cultureName)
    {
        return string.IsNullOrWhiteSpace(cultureName) ? null : CultureInfo.GetCultureInfo(cultureName);
    }
}
