namespace PineGuard.Rules;

public static class CharRules
{
    public const char AsciiMinValue = (char)0x00; // '\0'
    public const char AsciiMaxValue = (char)0x7F; // '\u007F'

    public const char PrintableAsciiMinValue = (char)0x20; // ' '
    public const char PrintableAsciiMaxValue = (char)0x7E; // '~'

    public static bool IsLetter(char c) => char.IsLetter(c);

    public static bool IsDigit(char c) => c is >= '0' and <= '9';

    public static bool IsLetterOrDigit(char c) => char.IsLetterOrDigit(c);

    public static bool IsAscii(char c) => c <= AsciiMaxValue;

    public static bool IsPrintableAscii(char c) => c is >= PrintableAsciiMinValue and <= PrintableAsciiMaxValue;

    public static bool IsWhitespace(char c) => char.IsWhiteSpace(c);

    public static bool IsControl(char c) => char.IsControl(c);

    public static bool IsUppercase(char c) => char.IsUpper(c);

    public static bool IsLowercase(char c) => char.IsLower(c);

    public static bool IsHexDigit(char c) =>
        c is >= '0' and <= '9'
        or >= 'a' and <= 'f'
        or >= 'A' and <= 'F';
}
