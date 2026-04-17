using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Utils;

public static class PhoneUtilityTestData
{
    public static class TryParsePhone
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("US Format", ("+1(425)555-0123", 7, 15, null), true, "14255550123"),
            new("Simple Digits", ("1234567890", 7, 15, null), true, "1234567890")
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("Null returns false", (null, 7, 15, null), false, string.Empty),
            new("Whitespace returns false", ("   ", 7, 15, null), false, string.Empty),
            new("Invalid Config Min > Max", ("1234567890", 10, 5, null), false, string.Empty),
            new("Invalid Config Min < 1", ("1234567890", 0, 15, null), false, string.Empty),
            new("Invalid Config Max < 1", ("1234567890", 7, 0, null), false, string.Empty),
            new("Disallowed Char", ("12x3", 1, 15, ['-']), false, string.Empty),
            new("Too Short", ("123", 4, 10, null), false, "123"),
            new("Too Long", ("123456", 1, 5, null), false, "123456")
        ];

        public sealed record ValidCase(string Name, (string? Value, int MinDigits, int MaxDigits, char[]? AllowedNonDigitCharacters) Value, bool Expected, string ExpectedOutValue)
            : TryCase<(string? Value, int MinDigits, int MaxDigits, char[]? AllowedNonDigitCharacters), string>(Name, Value, Expected, ExpectedOutValue);
    }
}
