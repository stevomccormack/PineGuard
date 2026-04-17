using PineGuard.Testing.UnitTests;

namespace PineGuard.DataAnnotations.UnitTests;

public static class StringAttributesTestData
{
    public sealed record ValidCase(string Name, object? Value, bool Expected) : ReturnCase<object?, bool>(Name, Value, Expected);

    private static TheoryData<ValidCase> CommonEdgeCases() =>
    [
        new("null", null, true)
    ];

    public static class ExactLength
    {
        public static TheoryData<ValidCase> ValidCases => [new("exact", "abc", true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new("short", "ab", false), new("long", "abcd", false)];
    }

    public static class LengthBetween
    {
        public static TheoryData<ValidCase> ValidCases => [new("min", "abc", true), new("max", "abcde", true), new("mid", "abcd", true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new("short", "ab", false), new("long", "abcdef", false)];
    }

    public static class LongerThan
    {
        public static TheoryData<ValidCase> ValidCases => [new("longer", "abcd", true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new("exact", "abc", false), new("shorter", "ab", false)];
    }

    public static class ShorterThan
    {
        public static TheoryData<ValidCase> ValidCases => [new("shorter", "ab", true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new("exact", "abc", false), new("longer", "abcd", false)];
    }

    public static class Match
    {
        public static TheoryData<ValidCase> ValidCases => [new("match", "123", true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new("no match", "abc", false)];
    }

    public static class NotMatch
    {
        public static TheoryData<ValidCase> ValidCases => [new("no match", "abc", true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new("match", "123", false)];
    }

    public static class Alphabetic
    {
        public static TheoryData<ValidCase> ValidCases => [new("alpha", "abc", true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new("numeric", "123", false), new("mixed", "a1", false)];
    }

    public static class NotAlphabetic
    {
        public static TheoryData<ValidCase> ValidCases => [new("numeric", "123", true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new("alpha", "abc", false)];
    }

    public static class Alphanumeric
    {
        public static TheoryData<ValidCase> ValidCases => [new("alpha", "abc", true), new("numeric", "123", true), new("mixed", "a1", true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new("symbol", "a-1", false)];
    }

    public static class NotAlphanumeric
    {
        public static TheoryData<ValidCase> ValidCases => [new("symbol", "---", true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new("alpha", "abc", false), new("numeric", "123", false)];
    }

    public static class NumericString
    {
        public static TheoryData<ValidCase> ValidCases => [new("numeric", "123", true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new("alpha", "abc", false)];
    }

    public static class NotNumericString
    {
        public static TheoryData<ValidCase> ValidCases => [new("alpha", "abc", true), new("decimal", "123.45", true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new("numeric", "123", false)];
    }

    public static class DigitsOnly
    {
        public static TheoryData<ValidCase> ValidCases => [new("digits", "123", true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new("decimal", "12.3", false), new("alpha", "1a", false)];
    }

    public static class NotDigitsOnly
    {
        public static TheoryData<ValidCase> ValidCases => [new("decimal", "12.3", true), new("alpha", "a", true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new("digits", "123", false)];
    }

    public static class EmptyString
    {
        public static TheoryData<ValidCase> ValidCases => [new("empty", "", true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases(); // null is NOT string, so returns Success? Wait.
                                                                            // AttributeBase allows Null. But if Validates Type, it returns Success if null.
                                                                            // EmptyString implies Is Empty (length 0).
                                                                            // Must.Be.Empty allows null?
                                                                            // Code: if (value is not string) return Success. So null -> Success.

        public static TheoryData<ValidCase> InvalidCases => [new("not empty", "a", false)];
    }

    public static class NullOrEmptyString
    {
        public static TheoryData<ValidCase> ValidCases => [new("null", null, true), new("empty", "", true)];
        public static TheoryData<ValidCase> InvalidCases => [new("value", "a", false)];
    }

    public static class NotNullOrEmptyString
    {
        public static TheoryData<ValidCase> ValidCases => [new("value", "a", true)];
        public static TheoryData<ValidCase> EdgeCases => [new("null", null, false)]; // Not Null
        public static TheoryData<ValidCase> InvalidCases => [new("empty", "", false)];
    }

    public static class NullOrWhiteSpaceString
    {
        public static TheoryData<ValidCase> ValidCases => [new("null", null, true), new("empty", "", true), new("whitespace", " ", true)];
        public static TheoryData<ValidCase> EdgeCases => [new("tab", "\t", true), new("newline", "\n", true), new("carriage return + newline", "\r\n", true), new("multiple spaces", "   ", true), new("mixed whitespace", " \t\n\r ", true)];
        public static TheoryData<ValidCase> InvalidCases => [new("value", "a", false)];
    }

    public static class NotNullOrWhiteSpaceString
    {
        public static TheoryData<ValidCase> ValidCases => [new("value", "a", true)];
        public static TheoryData<ValidCase> EdgeCases => [new("null", null, false)];
        public static TheoryData<ValidCase> InvalidCases => [new("empty", "", false), new("whitespace", " ", false)];
    }

    public static class LongerThanOrEqual
    {
        public static TheoryData<ValidCase> ValidCases => [new("longer", "abcd", true), new("equal", "abc", true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new("shorter", "ab", false)];
    }

    public static class ShorterThanOrEqual
    {
        public static TheoryData<ValidCase> ValidCases => [new("shorter", "ab", true), new("equal", "abc", true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new("longer", "abcd", false)];
    }

    public static class AsciiString
    {
        public static TheoryData<ValidCase> ValidCases => [new("ascii", "abc", true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new("unicode", "a\u0300", false)];
    }

    public static class NotAsciiString
    {
        public static TheoryData<ValidCase> ValidCases => [new("unicode", "\u0300", true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new("ascii", "abc", false)];
    }

    public static class ContainsWhitespace
    {
        public static TheoryData<ValidCase> ValidCases => [new("space", "a b", true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new("no space", "ab", false)];
    }

    public static class NotContainsWhitespace
    {
        public static TheoryData<ValidCase> ValidCases => [new("no space", "ab", true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new("space", "a b", false)];
    }

    public static class ContainsControlChars
    {
        public static TheoryData<ValidCase> ValidCases => [new("tab", "a\tb", true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new("no control", "ab", false)];
    }

    public static class NotContainsControlChars
    {
        public static TheoryData<ValidCase> ValidCases => [new("no control", "ab", true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new("tab", "a\tb", false)];
    }

    // Allowed: 'a', 'b'
    public static class ContainsAllowedOnly
    {
        public static TheoryData<ValidCase> ValidCases => [new("allowed", "aba", true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new("not allowed", "abc", false)];
    }

    // Allowed: 'a', 'b' (So NotAllowedOnly means contains something ELSE?)
    // Logic: Not (ContainsAllowedOnly). i.e. Contains at least one char NOT in Allowed.
    public static class NotContainsAllowedOnly
    {
        public static TheoryData<ValidCase> ValidCases => [new("has other", "abc", true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new("only allowed", "aba", false)];
    }

    // Disallowed: 'x', 'y'
    public static class ContainsDisallowed
    {
        public static TheoryData<ValidCase> ValidCases => [new("has disallowed", "axb", true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new("clean", "abc", false)];
    }

    // Disallowed: 'x', 'y'
    public static class NotContainsDisallowed
    {
        public static TheoryData<ValidCase> ValidCases => [new("clean", "abc", true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new("has disallowed", "axb", false)];
    }

    // Any: 'x', 'y'
    public static class ContainsAny
    {
        public static TheoryData<ValidCase> ValidCases => [new("has x", "axb", true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new("none", "abc", false)];
    }
}
