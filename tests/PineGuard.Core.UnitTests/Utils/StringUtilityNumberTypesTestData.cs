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

    public static class TryGetLastIntegerDigit
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("simple", ("123", NumberStyles.Integer), true, '3'),
            new("single digit", ("7", NumberStyles.Integer), true, '7'),
            new("unbounded length beyond Int128", ("170141183460469231731687303715884105728", NumberStyles.Integer), true, '8'),
            new("leading plus honored with AllowLeadingSign", ("+42", NumberStyles.AllowLeadingSign), true, '2'),
            new("leading minus honored with AllowLeadingSign", ("-42", NumberStyles.AllowLeadingSign), true, '2'),
            new("leading white honored with AllowLeadingWhite", (" 42", NumberStyles.AllowLeadingWhite), true, '2'),
            new("trailing white honored with AllowTrailingWhite", ("42 ", NumberStyles.AllowTrailingWhite), true, '2'),
            new("leading sign and both whites honored", (" +123 ", NumberStyles.Integer), true, '3'),
            new("tab leading white honored with AllowLeadingWhite", ("\t42", NumberStyles.AllowLeadingWhite), true, '2'),
            new("tab trailing white honored with AllowTrailingWhite", ("42\t", NumberStyles.AllowTrailingWhite), true, '2')
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("null", ((string?)null, NumberStyles.Integer), false, '\0'),
            new("empty", ("", NumberStyles.Integer), false, '\0'),
            new("whitespace only", ("   ", NumberStyles.Integer), false, '\0'),
            new("sign only", ("-", NumberStyles.AllowLeadingSign), false, '\0'),
            new("sign rejected without AllowLeadingSign", ("-5", NumberStyles.None), false, '\0'),
            new("leading white rejected without AllowLeadingWhite", (" 42", NumberStyles.None), false, '\0'),
            new("trailing white rejected without AllowTrailingWhite", ("42 ", NumberStyles.None), false, '\0'),
            new("nbsp leading rejected despite AllowLeadingWhite", (" 42", NumberStyles.AllowLeadingWhite), false, '\0'),
            new("nbsp trailing rejected despite AllowTrailingWhite", ("42 ", NumberStyles.AllowTrailingWhite), false, '\0'),
            new("ideographic space rejected despite AllowLeadingWhite", ("　42", NumberStyles.AllowLeadingWhite), false, '\0'),
            new("non-digit characters", ("12a3", NumberStyles.Integer), false, '\0'),
            new("thousands separator rejected", ("1,234", NumberStyles.AllowThousands), false, '\0'),
            new("decimal point rejected", ("12.3", NumberStyles.AllowDecimalPoint), false, '\0')
        ];

        public sealed record ValidCase(string Name, (string? value, NumberStyles styles) Value, bool Expected, char ExpectedLastDigit)
            : TryCase<(string? value, NumberStyles styles), char>(Name, Value, Expected, ExpectedLastDigit);
    }

    public static class IsAsciiWhite
    {
        public static TheoryData<Case> Cases =>
        [
            new("space", ' ', true),
            new("tab", '\t', true),
            new("line feed", '\n', true),
            new("carriage return", '\r', true),
            new("digit", '4', false),
            new("null character", '\0', false),
            new("non-breaking space", '\u00a0', false)
        ];

        public sealed record Case(string Name, char Value, bool Expected)
            : ReturnCase<char, bool>(Name, Value, Expected);
    }

    public static class TrimLeadingSign
    {
        public static TheoryData<Case> Cases =>
        [
            new("plus stripped", ("+42", NumberStyles.AllowLeadingSign), "42"),
            new("minus stripped", ("-42", NumberStyles.AllowLeadingSign), "42"),
            new("no sign present", ("42", NumberStyles.AllowLeadingSign), "42"),
            new("sign kept without AllowLeadingSign", ("+42", NumberStyles.None), "+42"),
            new("empty span", ("", NumberStyles.AllowLeadingSign), "")
        ];

        public sealed record Case(string Name, (string value, NumberStyles styles) Value, string Expected)
            : ReturnCase<(string value, NumberStyles styles), string>(Name, Value, Expected);
    }

    public static class TrimAsciiWhite
    {
        public static TheoryData<Case> StartCases =>
        [
            new("no leading whitespace", "42", "42"),
            new("space", " 42", "42"),
            new("tab", "\t42", "42"),
            new("all ascii whitespace", " \t\n\v\f\r ", ""),
            new("non-ascii whitespace kept", "\u00a042", "\u00a042")
        ];

        public static TheoryData<Case> EndCases =>
        [
            new("no trailing whitespace", "42", "42"),
            new("space", "42 ", "42"),
            new("tab", "42\t", "42"),
            new("all ascii whitespace", " \t\n\v\f\r ", ""),
            new("non-ascii whitespace kept", "42\u00a0", "42\u00a0")
        ];

        public sealed record Case(string Name, string Value, string Expected)
            : ReturnCase<string, string>(Name, Value, Expected);
    }

    public static IFormatProvider? GetProvider(string? cultureName)
    {
        return string.IsNullOrWhiteSpace(cultureName) ? null : CultureInfo.GetCultureInfo(cultureName);
    }

    public static class InvalidStyles
    {
        public static TheoryData<NumberStyles> Int32IncompatibleHexStyles => [NumberStyles.HexNumber | NumberStyles.AllowDecimalPoint];
        public static TheoryData<NumberStyles> Int64IncompatibleHexStyles => [NumberStyles.HexNumber | NumberStyles.AllowDecimalPoint];
        public static TheoryData<NumberStyles> DecimalUnsupportedStyles => [NumberStyles.HexNumber];
        public static TheoryData<NumberStyles> DecimalWithPlacesUnsupportedStyles => [NumberStyles.HexNumber];
        public static TheoryData<NumberStyles> ExactDecimalUnsupportedStyles => [NumberStyles.HexNumber];
        public static TheoryData<NumberStyles> SingleUnsupportedStyles => [NumberStyles.HexNumber];
        public static TheoryData<NumberStyles> DoubleUnsupportedStyles => [NumberStyles.HexNumber];
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
}
