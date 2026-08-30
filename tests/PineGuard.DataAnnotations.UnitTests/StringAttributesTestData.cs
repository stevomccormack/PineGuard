using PineGuard.Codes;
using PineGuard.Testing.UnitTests;
using PineGuard.Testing.UnitTests.DataAnnotations;
using F = PineGuard.Testing.Fixtures.StringRulesFixtures;

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
        public static TheoryData<ValidCase> ValidCases => [new("digits", F.IsDigitsOnly.Digits, true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new("decimal", "12.3", false), new("alpha", "1a", false)];
    }

    public static class NotDigitsOnly
    {
        public static TheoryData<ValidCase> ValidCases => [new("decimal", "12.3", true), new("alpha", "a", true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new("digits", F.IsDigitsOnly.Digits, false)];
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
        public static TheoryData<ValidCase> ValidCases => [new("null", F.IsNullOrEmpty.NullValue, true), new("empty", F.IsNullOrEmpty.Empty, true)];
        public static TheoryData<ValidCase> InvalidCases => [new("value", "a", false)];
    }

    public static class NotNullOrEmptyString
    {
        public static TheoryData<ValidCase> ValidCases => [new("value", "a", true)];
        public static TheoryData<ValidCase> EdgeCases => [new("null", F.IsNotNullOrEmpty.NullValue, false)]; // Not Null
        public static TheoryData<ValidCase> InvalidCases => [new("empty", F.IsNotNullOrEmpty.Empty, false)];
    }

    public static class NullOrWhiteSpaceString
    {
        public static TheoryData<ValidCase> ValidCases => [new("null", F.IsNullOrWhiteSpace.NullValue, true), new("empty", F.IsNullOrWhiteSpace.Empty, true), new("whitespace", " ", true)];
        public static TheoryData<ValidCase> EdgeCases => [new("tab", "\t", true), new("newline", "\n", true), new("carriage return + newline", "\r\n", true), new("multiple spaces", F.IsNullOrWhiteSpace.Whitespace, true), new("mixed whitespace", " \t\n\r ", true)];
        public static TheoryData<ValidCase> InvalidCases => [new("value", "a", false)];
    }

    public static class NotNullOrWhiteSpaceString
    {
        public static TheoryData<ValidCase> ValidCases => [new("value", "a", true)];
        public static TheoryData<ValidCase> EdgeCases => [new("null", F.IsNotNullOrWhiteSpace.NullValue, false)];
        public static TheoryData<ValidCase> InvalidCases => [new("empty", F.IsNotNullOrWhiteSpace.Empty, false), new("whitespace", " ", false)];
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
        public static TheoryData<ValidCase> ValidCases => [new("space", F.ContainsWhitespace.Between, true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new("no space", "ab", false)];
    }

    public static class NotContainsWhitespace
    {
        public static TheoryData<ValidCase> ValidCases => [new("no space", "ab", true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new("space", F.ContainsWhitespace.Between, false)];
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

    public static class Contains
    {
        public static readonly string Substring = F.Contains.Present.substring;

        public static TheoryData<DataAnnotationCase> Cases =>
        [
            new(nameof(F.Contains.Present), F.Contains.Present.value, new DataAnnotationExpected(true)),
            new(nameof(F.Contains.CaseMismatch), F.Contains.CaseMismatch.value, new DataAnnotationExpected(false, "Value must contain the specified substring.", Code: MustCodes.Text.Content.NotContains)),
            new(nameof(F.Contains.NullValue), F.Contains.NullValue.value, new DataAnnotationExpected(true))
        ];
    }

    public static class ContainsIgnoringCase
    {
        public static readonly string Substring = F.Contains.PresentIgnoringCase.substring;
        public static readonly StringComparison Comparison = F.Contains.PresentIgnoringCase.comparison;

        public static TheoryData<DataAnnotationCase> Cases =>
        [
            new(nameof(F.Contains.PresentIgnoringCase), F.Contains.PresentIgnoringCase.value, new DataAnnotationExpected(true))
        ];
    }

    public static class NotContains
    {
        public static readonly string Substring = F.Contains.Present.substring;

        public static TheoryData<DataAnnotationCase> Cases =>
        [
            new(nameof(F.Contains.CaseMismatch), F.Contains.CaseMismatch.value, new DataAnnotationExpected(true)),
            new(nameof(F.Contains.Present), F.Contains.Present.value, new DataAnnotationExpected(false, "Value must not contain the specified substring.", Code: MustCodes.Text.Content.Contains)),
            new(nameof(F.Contains.NullValue), F.Contains.NullValue.value, new DataAnnotationExpected(true))
        ];
    }

    public static class NotContainsIgnoringCase
    {
        public static readonly string Substring = F.Contains.PresentIgnoringCase.substring;
        public static readonly StringComparison Comparison = F.Contains.PresentIgnoringCase.comparison;

        public static TheoryData<DataAnnotationCase> Cases =>
        [
            new(nameof(F.Contains.PresentIgnoringCase), F.Contains.PresentIgnoringCase.value, new DataAnnotationExpected(false, "Value must not contain the specified substring.", Code: MustCodes.Text.Content.Contains))
        ];
    }

    public static class StartsWith
    {
        public static readonly string Prefix = F.StartsWith.Prefixed.prefix;

        public static TheoryData<DataAnnotationCase> Cases =>
        [
            new(nameof(F.StartsWith.Prefixed), F.StartsWith.Prefixed.value, new DataAnnotationExpected(true)),
            new(nameof(F.StartsWith.CaseMismatch), F.StartsWith.CaseMismatch.value, new DataAnnotationExpected(false, "Value must start with the specified prefix.", Code: MustCodes.Text.Content.NotStartsWith)),
            new(nameof(F.StartsWith.NullValue), F.StartsWith.NullValue.value, new DataAnnotationExpected(true))
        ];
    }

    public static class StartsWithIgnoringCase
    {
        public static readonly string Prefix = F.StartsWith.PrefixedIgnoringCase.prefix;
        public static readonly StringComparison Comparison = F.StartsWith.PrefixedIgnoringCase.comparison;

        public static TheoryData<DataAnnotationCase> Cases =>
        [
            new(nameof(F.StartsWith.PrefixedIgnoringCase), F.StartsWith.PrefixedIgnoringCase.value, new DataAnnotationExpected(true))
        ];
    }

    public static class NotStartsWith
    {
        public static readonly string Prefix = F.StartsWith.Prefixed.prefix;

        public static TheoryData<DataAnnotationCase> Cases =>
        [
            new(nameof(F.StartsWith.CaseMismatch), F.StartsWith.CaseMismatch.value, new DataAnnotationExpected(true)),
            new(nameof(F.StartsWith.Prefixed), F.StartsWith.Prefixed.value, new DataAnnotationExpected(false, "Value must not start with the specified prefix.", Code: MustCodes.Text.Content.StartsWith)),
            new(nameof(F.StartsWith.NullValue), F.StartsWith.NullValue.value, new DataAnnotationExpected(true))
        ];
    }

    public static class NotStartsWithIgnoringCase
    {
        public static readonly string Prefix = F.StartsWith.PrefixedIgnoringCase.prefix;
        public static readonly StringComparison Comparison = F.StartsWith.PrefixedIgnoringCase.comparison;

        public static TheoryData<DataAnnotationCase> Cases =>
        [
            new(nameof(F.StartsWith.PrefixedIgnoringCase), F.StartsWith.PrefixedIgnoringCase.value, new DataAnnotationExpected(false, "Value must not start with the specified prefix.", Code: MustCodes.Text.Content.StartsWith))
        ];
    }

    public static class EndsWith
    {
        public static readonly string Suffix = F.EndsWith.Suffixed.suffix;

        public static TheoryData<DataAnnotationCase> Cases =>
        [
            new(nameof(F.EndsWith.Suffixed), F.EndsWith.Suffixed.value, new DataAnnotationExpected(true)),
            new(nameof(F.EndsWith.CaseMismatch), F.EndsWith.CaseMismatch.value, new DataAnnotationExpected(false, "Value must end with the specified suffix.", Code: MustCodes.Text.Content.NotEndsWith)),
            new(nameof(F.EndsWith.NullValue), F.EndsWith.NullValue.value, new DataAnnotationExpected(true))
        ];
    }

    public static class EndsWithIgnoringCase
    {
        public static readonly string Suffix = F.EndsWith.SuffixedIgnoringCase.suffix;
        public static readonly StringComparison Comparison = F.EndsWith.SuffixedIgnoringCase.comparison;

        public static TheoryData<DataAnnotationCase> Cases =>
        [
            new(nameof(F.EndsWith.SuffixedIgnoringCase), F.EndsWith.SuffixedIgnoringCase.value, new DataAnnotationExpected(true))
        ];
    }

    public static class NotEndsWith
    {
        public static readonly string Suffix = F.EndsWith.Suffixed.suffix;

        public static TheoryData<DataAnnotationCase> Cases =>
        [
            new(nameof(F.EndsWith.CaseMismatch), F.EndsWith.CaseMismatch.value, new DataAnnotationExpected(true)),
            new(nameof(F.EndsWith.Suffixed), F.EndsWith.Suffixed.value, new DataAnnotationExpected(false, "Value must not end with the specified suffix.", Code: MustCodes.Text.Content.EndsWith)),
            new(nameof(F.EndsWith.NullValue), F.EndsWith.NullValue.value, new DataAnnotationExpected(true))
        ];
    }

    public static class NotEndsWithIgnoringCase
    {
        public static readonly string Suffix = F.EndsWith.SuffixedIgnoringCase.suffix;
        public static readonly StringComparison Comparison = F.EndsWith.SuffixedIgnoringCase.comparison;

        public static TheoryData<DataAnnotationCase> Cases =>
        [
            new(nameof(F.EndsWith.SuffixedIgnoringCase), F.EndsWith.SuffixedIgnoringCase.value, new DataAnnotationExpected(false, "Value must not end with the specified suffix.", Code: MustCodes.Text.Content.EndsWith))
        ];
    }

    public static class RegexPattern
    {
        public static TheoryData<DataAnnotationCase> Cases => F.IsRegexPattern.AllScenarios.ToDataAnnotationCases(s => s.Name switch
        {
            nameof(F.IsRegexPattern.NullValue) => new DataAnnotationExpected(true),
            _ when s.IsValid => new DataAnnotationExpected(true),
            _ => new DataAnnotationExpected(false, "Value must be a valid regular expression pattern.", Code: MustCodes.Text.Pattern.Invalid)
        });
    }

    public static class HasByteOrderMark
    {
        public static TheoryData<DataAnnotationCase> Cases => F.HasByteOrderMark.AllScenarios.ToDataAnnotationCases(s => s.Name switch
        {
            nameof(F.HasByteOrderMark.NullValue) => new DataAnnotationExpected(true),
            _ when s.IsValid => new DataAnnotationExpected(true),
            _ => new DataAnnotationExpected(false, "Value must start with a byte-order mark.", Code: MustCodes.Text.Bom.Missing)
        });
    }

    public static class NotHasByteOrderMark
    {
        public static TheoryData<DataAnnotationCase> Cases => F.HasByteOrderMark.AllScenarios.ToDataAnnotationCases(s => s.Name switch
        {
            nameof(F.HasByteOrderMark.NullValue) => new DataAnnotationExpected(true),
            _ when s.IsValid => new DataAnnotationExpected(false, "Value must not start with a byte-order mark.", Code: MustCodes.Text.Bom.Present),
            _ => new DataAnnotationExpected(true)
        });
    }

    public static class WellFormedUtf16
    {
        public static TheoryData<DataAnnotationCase> Cases => F.IsWellFormedUtf16.AllScenarios.ToDataAnnotationCases(s => s.Name switch
        {
            nameof(F.IsWellFormedUtf16.NullValue) => new DataAnnotationExpected(true),
            _ when s.IsValid => new DataAnnotationExpected(true),
            _ => new DataAnnotationExpected(false, "Value must be well-formed UTF-16.", Code: MustCodes.Text.Unicode.Malformed)
        });
    }

    public static class NotWellFormedUtf16
    {
        public static TheoryData<DataAnnotationCase> Cases => F.IsWellFormedUtf16.AllScenarios.ToDataAnnotationCases(s => s.Name switch
        {
            nameof(F.IsWellFormedUtf16.NullValue) => new DataAnnotationExpected(true),
            _ when s.IsValid => new DataAnnotationExpected(false, "Value must not be well-formed UTF-16.", Code: MustCodes.Text.Unicode.WellFormed),
            _ => new DataAnnotationExpected(true)
        });
    }

    public static class Normalized
    {
        public static TheoryData<DataAnnotationCase> Cases => F.IsNormalized.AllScenarios.ToDataAnnotationCases(v => (object?)v, s => s.Name switch
        {
            nameof(F.IsNormalized.NullValue) => new DataAnnotationExpected(true),
            nameof(F.IsNormalized.UnknownForm) => new DataAnnotationExpected(false, "form requires a defined normalization form.", Code: MustCodes.Text.Unicode.NotNormalized),
            _ when s.IsValid => new DataAnnotationExpected(true),
            _ => new DataAnnotationExpected(false, "Value must be in the specified normalization form.", Code: MustCodes.Text.Unicode.NotNormalized)
        });
    }

    public static class NotNormalized
    {
        public static TheoryData<DataAnnotationCase> Cases => F.IsNormalized.AllScenarios.ToDataAnnotationCases(v => (object?)v, s => s.Name switch
        {
            nameof(F.IsNormalized.NullValue) => new DataAnnotationExpected(true),
            nameof(F.IsNormalized.UnknownForm) => new DataAnnotationExpected(false, "form requires a defined normalization form.", Code: MustCodes.Text.Unicode.Normalized),
            _ when s.IsValid => new DataAnnotationExpected(false, "Value must not be in the specified normalization form.", Code: MustCodes.Text.Unicode.Normalized),
            _ => new DataAnnotationExpected(true)
        });
    }
}
