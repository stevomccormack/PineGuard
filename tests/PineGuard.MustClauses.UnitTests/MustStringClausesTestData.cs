using System.Text.RegularExpressions;
using PineGuard.Codes;
using PineGuard.Testing.UnitTests;
using PineGuard.Testing.UnitTests.MustClauses;
using PineGuard.Testing.UnitTests.Rules;
using F = PineGuard.Testing.Fixtures.StringRulesFixtures;

namespace PineGuard.MustClauses.UnitTests;

public static partial class MustStringClausesTestData
{
    public static class NullOrEmpty
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("null", null, true),
            new("empty", "", true),
            new("whitespace", " ", false),
            new("text", "abc", false)
        ];

        public sealed record ValidCase(string Name, string? Value, bool Expected) : IsCase<string?>(Name, Value, Expected);
    }

    public static class NotNullOrEmpty
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("text", "abc", true),
            new("whitespace", " ", true),
            new("empty", "", false),
            new("null", null, false)
        ];

        public sealed record ValidCase(string Name, string? Value, bool Expected) : IsCase<string?>(Name, Value, Expected);
    }

    public static class NullOrWhiteSpace
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("null", null, true),
            new("empty", "", true),
            new("whitespace", " ", true),
            new("text", "abc", false)
        ];

        public sealed record ValidCase(string Name, string? Value, bool Expected) : IsCase<string?>(Name, Value, Expected);
    }

    public static class NotNullOrWhiteSpace
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("text", "abc", true),
            new("whitespace", " ", false),
            new("empty", "", false),
            new("null", null, false)
        ];

        public sealed record ValidCase(string Name, string? Value, bool Expected) : IsCase<string?>(Name, Value, Expected);
    }

    public static class Empty
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("empty", "", true),
            new("null", null, false), // Empty implies not null? C# string.Empty is not null. Testing behavior.
            new("whitespace", " ", false),
            new("text", "abc", false)
        ];

        public sealed record ValidCase(string Name, string? Value, bool Expected) : IsCase<string?>(Name, Value, Expected);
    }

    public static class NotEmpty
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("text", "abc", true),
            new("whitespace", " ", true),
            new("null", null, true), // value != string.Empty is true for null.
            new("empty", "", false)
        ];

        public sealed record ValidCase(string Name, string? Value, bool Expected) : IsCase<string?>(Name, Value, Expected);
    }

    public static class ExactLength
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("matches", ("abc", 3), true),
            new("not matching", ("ab", 3), false)
        ];

        public static TheoryData<EdgeCase> EdgeCases =>
        [
            new("null input", (null, 3), false, "input must not be null."),
            new("negative length", ("abc", -1), false, "length requires a non-negative length.")
        ];

        public sealed record ValidCase(string Name, (string? Value, int Length) Value, bool Expected) : IsCase<(string? Value, int Length)>(Name, Value, Expected);

        public sealed record EdgeCase(string Name, (string? Value, int Length) Value, bool Expected, string ExpectedMessage)
            : IsCase<(string? Value, int Length)>(Name, Value, Expected);
    }

    public static class LengthBetween
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("min boundary", ("a", 1, 3), true),
            new("in range", ("abc", 1, 3), true),
            new("below min", ("", 1, 3), false),
            new("above max", ("abcd", 1, 3), false)
        ];

        public static TheoryData<EdgeCase> EdgeCases =>
        [
            new("null input", (null, 1, 3), false, "input must not be null."),
            new("negative min", ("abc", -1, 5), false, "min requires a non-negative min."),
            new("negative max", ("abc", 0, -1), false, "max requires a non-negative max."),
            new("min > max", ("abc", 5, 2), false, "min requires a valid length range.")
        ];

        public sealed record ValidCase(string Name, (string? Value, int Min, int Max) Value, bool Expected) : IsCase<(string? Value, int Min, int Max)>(Name, Value, Expected);

        public sealed record EdgeCase(string Name, (string? Value, int Min, int Max) Value, bool Expected, string ExpectedMessage)
            : IsCase<(string? Value, int Min, int Max)>(Name, Value, Expected);
    }

    public static partial class Match
    {
        [GeneratedRegex("^abc$")]
        private static partial Regex PatternRegex();

        private static readonly Regex Pattern = PatternRegex();

        public static TheoryData<ValidCase> ValidCases =>
        [
            new("matches", ("abc", Pattern), true),
            new("not matches", ("def", Pattern), false)
        ];

        public static TheoryData<EdgeCase> EdgeCases =>
        [
            new("null input", (null, Pattern), false, "input must not be null.", null),
            new("null pattern", ("abc", null!), false, "pattern must not be null.", "pattern")
        ];

        public sealed record ValidCase(string Name, (string? Value, Regex Pattern) Value, bool Expected) : IsCase<(string? Value, Regex Pattern)>(Name, Value, Expected);

        public sealed record EdgeCase(string Name, (string? Value, Regex Pattern) Value, bool Expected, string ExpectedMessage, string? ParamName)
           : IsCase<(string? Value, Regex Pattern)>(Name, Value, Expected);
    }

    public static class Alphabetic
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("alphabetic", ("abc", null), true),
            new("not alphabetic", ("ab1", null), false),
            new("with inclusions", ("ab1", ['1']), true)
        ];

        public static TheoryData<EdgeCase> EdgeCases =>
       [
            new("null input", (null, null), false, "input must not be null.")
       ];

        public sealed record ValidCase(string Name, (string? Value, char[]? Inclusions) Value, bool Expected) : IsCase<(string? Value, char[]? Inclusions)>(Name, Value, Expected);

        public sealed record EdgeCase(string Name, (string? Value, char[]? Inclusions) Value, bool Expected, string ExpectedMessage)
            : IsCase<(string? Value, char[]? Inclusions)>(Name, Value, Expected);
    }

    public static class Numeric
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("numeric", ("123", null), true),
            new("not numeric", ("12a", null), false),
            new("with inclusions", ("12a", ['a']), true)
        ];

        public static TheoryData<EdgeCase> EdgeCases =>
        [
             new("null input", (null, null), false, "input must not be null.")
        ];

        public sealed record ValidCase(string Name, (string? Value, char[]? Inclusions) Value, bool Expected) : IsCase<(string? Value, char[]? Inclusions)>(Name, Value, Expected);

        public sealed record EdgeCase(string Name, (string? Value, char[]? Inclusions) Value, bool Expected, string ExpectedMessage)
            : IsCase<(string? Value, char[]? Inclusions)>(Name, Value, Expected);
    }

    public static class Alphanumeric
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("alphanumeric", ("a1", null), true),
            new("not alphanumeric", ("a1.", null), false),
            new("with inclusions", ("a1.", ['.']), true)
        ];

        public static TheoryData<EdgeCase> EdgeCases =>
        [
             new("null input", (null, null), false, "input must not be null.")
        ];

        public sealed record ValidCase(string Name, (string? Value, char[]? Inclusions) Value, bool Expected) : IsCase<(string? Value, char[]? Inclusions)>(Name, Value, Expected);

        public sealed record EdgeCase(string Name, (string? Value, char[]? Inclusions) Value, bool Expected, string ExpectedMessage)
            : IsCase<(string? Value, char[]? Inclusions)>(Name, Value, Expected);
    }

    public static class DigitsOnly
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("digits only", ("123", null), true),
            new("signed", ("-123", null), false),
            new("with letter", ("12a", null), false),
            new("allowed chars", ("-123", ['-']), true),
            new("allowed chars fail", ("-12a", ['-']), false)
        ];

        public static TheoryData<EdgeCase> EdgeCases =>
        [
             new("null input", (null, null), false, "input must not be null."),
             new("null input with allowed", (null, ['-']), false, "input must not be null.")
        ];

        // Using char[]? Allowed for the second param
        public sealed record ValidCase(string Name, (string? Value, char[]? AllowedChars) Value, bool Expected)
            : IsCase<(string? Value, char[]? AllowedChars)>(Name, Value, Expected);

        public sealed record EdgeCase(string Name, (string? Value, char[]? AllowedChars) Value, bool Expected, string ExpectedMessage)
            : IsCase<(string? Value, char[]? AllowedChars)>(Name, Value, Expected);
    }

    public static class Uppercase
    {
        public static TheoryData<ValidCase> ValidCases =>
       [
           new("uppercase", ("ABC", false), true),
            new("mixed", ("ABc", false), false),
            new("non-letters allowed", ("A-B", false), true),
            new("lettersOnly fail", ("A-B", true), false),
            new("lettersOnly pass", ("AB", true), true)
       ];

        public static TheoryData<EdgeCase> EdgeCases =>
        [
             new("null input", (null, false), false, "input must not be null.")
        ];

        public sealed record ValidCase(string Name, (string? Value, bool LettersOnly) Value, bool Expected) : IsCase<(string? Value, bool LettersOnly)>(Name, Value, Expected);

        public sealed record EdgeCase(string Name, (string? Value, bool LettersOnly) Value, bool Expected, string ExpectedMessage)
            : IsCase<(string? Value, bool LettersOnly)>(Name, Value, Expected);
    }

    public static class Lowercase
    {
        public static TheoryData<ValidCase> ValidCases =>
       [
           new("lowercase", ("abc", false), true),
            new("mixed", ("abC", false), false),
            new("non-letters allowed", ("a-b", false), true),
            new("lettersOnly fail", ("a-b", true), false),
            new("lettersOnly pass", ("ab", true), true)
       ];

        public static TheoryData<EdgeCase> EdgeCases =>
        [
             new("null input", (null, false), false, "input must not be null.")
        ];

        public sealed record ValidCase(string Name, (string? Value, bool LettersOnly) Value, bool Expected) : IsCase<(string? Value, bool LettersOnly)>(Name, Value, Expected);

        public sealed record EdgeCase(string Name, (string? Value, bool LettersOnly) Value, bool Expected, string ExpectedMessage)
            : IsCase<(string? Value, bool LettersOnly)>(Name, Value, Expected);
    }

    public static class Ascii
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("ascii", "abc", true),
            new("unicode", "abc\u1234", false)
        ];

        public static TheoryData<ValidCase> NotCases =>
        [
             new("unicode", "abc\u1234", true), // NotAscii should pass
             new("ascii", "abc", false)
        ];

        public static TheoryData<EdgeCase> EdgeCases =>
        [
             new("null input", null, false, "input must not be null.")
        ];

        public sealed record ValidCase(string Name, string? Value, bool Expected) : IsCase<string?>(Name, Value, Expected);
        public sealed record EdgeCase(string Name, string? Value, bool Expected, string ExpectedMessage) : IsCase<string?>(Name, Value, Expected);
    }

    public static class PrintableAscii
    {
        public static TheoryData<ValidCase> ValidCases =>
       [
           new("printable", "abc!", true),
           new("control", "\u0000", false)
       ];

        public static TheoryData<EdgeCase> EdgeCases =>
       [
            new("null input", null, false, "input must not be null.")
       ];

        public sealed record ValidCase(string Name, string? Value, bool Expected) : IsCase<string?>(Name, Value, Expected);
        public sealed record EdgeCase(string Name, string? Value, bool Expected, string ExpectedMessage) : IsCase<string?>(Name, Value, Expected);
    }

    public static class Whitespace
    {
        public static TheoryData<ValidCase> NotWhitespaceCases =>
       [
           new("text", "abc", true),
            new("space", " ", false),
            new("tab", "\t", false)
       ];

        public static TheoryData<ValidCase> ContainsWhitespaceCases =>
        [
            new("text with space", "a b", true),
            new("text no space", "ab", false)
        ];

        public static TheoryData<EdgeCase> EdgeCases =>
        [
             new("null input", null, false, "input must not be null.")
        ];

        public sealed record ValidCase(string Name, string? Value, bool Expected) : IsCase<string?>(Name, Value, Expected);
        public sealed record EdgeCase(string Name, string? Value, bool Expected, string ExpectedMessage) : IsCase<string?>(Name, Value, Expected);
    }

    public static class ControlChars
    {
        public static TheoryData<ValidCase> ContainsCases =>
        [
            new("with control", "a\nb", true),
            new("no control", "abc", false)
        ];

        public static TheoryData<ValidCase> NotContainsCases =>
        [
            new("no control", "abc", true),
            new("with control", "a\nb", false)
        ];

        public static TheoryData<EdgeCase> EdgeCases =>
        [
             new("null input", null, false, "input must not be null.")
        ];

        public sealed record ValidCase(string Name, string? Value, bool Expected) : IsCase<string?>(Name, Value, Expected);
        public sealed record EdgeCase(string Name, string? Value, bool Expected, string ExpectedMessage) : IsCase<string?>(Name, Value, Expected);
    }

    public static class AllowedDisallowed
    {
        public static TheoryData<AllowedCase> ContainsAllowedOnlyCases =>
        [
            new("all allowed", ("abc", ['a', 'b', 'c']), true),
            new("mixed", ("abcd", ['a', 'b', 'c']), false)
        ];

        public static TheoryData<AllowedEdgeCase> ContainsAllowedOnlyEdgeCases =>
        [
             new("null allowed", ("abc", null!), false, "allowedChars"),
             new("null input", (null, ['a']), false, "input")
        ];

        public static TheoryData<AllowedCase> ContainsDisallowedCases =>
        [
             new("contains disallowed", ("abc", ['b']), true),
             new("mixed", ("acd", ['b']), false),
             new("all allowed", ("bbb", ['b']), true)
        ];

        public static TheoryData<AllowedEdgeCase> EdgeCases =>
        [
             new("null disallowed", ("abc", null!), false, "disallowedChars"),
             new("null input", (null, ['a']), false, "input")
        ];

        public sealed record AllowedCase(string Name, (string? Value, char[] Allowed) Value, bool Expected)
            : IsCase<(string? Value, char[] Allowed)>(Name, Value, Expected);

        public sealed record AllowedEdgeCase(string Name, (string? Value, char[] Allowed) Value, bool Expected, string ParamName)
            : IsCase<(string? Value, char[] Allowed)>(Name, Value, Expected);
    }

    public static class LongerShorter
    {
        public static TheoryData<ValidCase> LongerThanCases =>
        [
            new("gt", ("abc", 2), true),
            new("eq", ("abc", 3), false),
            new("lt", ("abc", 4), false)
        ];

        public static TheoryData<ValidCase> LongerThanOrEqualCases =>
        [
            new("gt", ("abc", 2), true),
            new("eq", ("abc", 3), true),
            new("lt", ("abc", 4), false)
        ];

        public static TheoryData<ValidCase> ShorterThanCases =>
        [
            new("lt", ("abc", 4), true),
            new("eq", ("abc", 3), false),
            new("gt", ("abc", 2), false)
        ];

        public static TheoryData<ValidCase> ShorterThanOrEqualCases =>
        [
            new("lt", ("abc", 4), true),
            new("eq", ("abc", 3), true),
            new("gt", ("abc", 2), false)
        ];

        public static TheoryData<EdgeCase> EdgeCases =>
        [
             new("null input", (null, 1), false, "input must not be null."),
             new("negative length", ("abc", -1), false, "length requires a non-negative length.")
        ];

        public sealed record ValidCase(string Name, (string? Value, int Length) Value, bool Expected)
            : IsCase<(string? Value, int Length)>(Name, Value, Expected);

        public sealed record EdgeCase(string Name, (string? Value, int Length) Value, bool Expected, string ExpectedMessage)
            : IsCase<(string? Value, int Length)>(Name, Value, Expected);
    }
    public static class ContainsAny
    {
        public static TheoryData<AllowedCase> ValidCases =>
        [
            new("contains any", ("abc", ['a']), true),
            new("contains any multiple", ("abc", ['x', 'b']), true),
            new("not contains", ("abc", ['x', 'y']), false)
        ];

        public static TheoryData<AllowedEdgeCase> EdgeCases =>
        [
             new("null chars", ("abc", null!), false, "characters"),
             new("null input", (null, ['a']), false, "input")
        ];

        public sealed record AllowedCase(string Name, (string? Value, char[] Chars) Value, bool Expected)
            : IsCase<(string? Value, char[] Chars)>(Name, Value, Expected);

        public sealed record AllowedEdgeCase(string Name, (string? Value, char[] Chars) Value, bool Expected, string ParamName)
            : IsCase<(string? Value, char[] Chars)>(Name, Value, Expected);
    }

    public static class Contains
    {
        public static TheoryData<MustCase<(string? value, string substring, StringComparison comparison)>> ValidCases => F.Contains.ValidScenarios.ToMustCases();
        public static TheoryData<MustCase<(string? value, string substring, StringComparison comparison)>> InvalidCases => F.Contains.InvalidScenarios.Except(nameof(F.Contains.NullValue)).ToMustCases(_ => new MustExpected(false, "value must contain the specified substring.", Code: MustCodes.Text.Content.NotContains));
        public static TheoryData<MustCase<(string? value, string substring, StringComparison comparison)>> NullCases => F.Contains.InvalidScenarios.Only(nameof(F.Contains.NullValue)).ToMustCases(_ => new MustExpected(false, "value must not be null.", "value", MustCodes.Text.Content.NotContains));

        public static TheoryData<MustCase<(string? value, string substring, StringComparison comparison)>> NullSubstringCases =>
        [
            new("null substring", ("hello world", null!, StringComparison.Ordinal), new MustExpected(false, "substring must not be null.", "substring", MustCodes.Text.Content.NotContains))
        ];
    }

    public static class NotContains
    {
        public static TheoryData<MustCase<(string? value, string substring, StringComparison comparison)>> ValidCases => F.Contains.InvalidScenarios.Except(nameof(F.Contains.NullValue)).ToMustCases(_ => new MustExpected(true));
        public static TheoryData<MustCase<(string? value, string substring, StringComparison comparison)>> InvalidCases => F.Contains.ValidScenarios.ToMustCases(_ => new MustExpected(false, "value must not contain the specified substring.", Code: MustCodes.Text.Content.Contains));
        public static TheoryData<MustCase<(string? value, string substring, StringComparison comparison)>> NullCases => F.Contains.InvalidScenarios.Only(nameof(F.Contains.NullValue)).ToMustCases(_ => new MustExpected(false, "value must not be null.", "value", MustCodes.Text.Content.Contains));

        public static TheoryData<MustCase<(string? value, string substring, StringComparison comparison)>> NullSubstringCases =>
        [
            new("null substring", ("hello world", null!, StringComparison.Ordinal), new MustExpected(false, "substring must not be null.", "substring", MustCodes.Text.Content.Contains))
        ];
    }

    public static class StartsWith
    {
        public static TheoryData<MustCase<(string? value, string prefix, StringComparison comparison)>> ValidCases => F.StartsWith.ValidScenarios.ToMustCases();
        public static TheoryData<MustCase<(string? value, string prefix, StringComparison comparison)>> InvalidCases => F.StartsWith.InvalidScenarios.Except(nameof(F.StartsWith.NullValue)).ToMustCases(_ => new MustExpected(false, "value must start with the specified prefix.", Code: MustCodes.Text.Content.NotStartsWith));
        public static TheoryData<MustCase<(string? value, string prefix, StringComparison comparison)>> NullCases => F.StartsWith.InvalidScenarios.Only(nameof(F.StartsWith.NullValue)).ToMustCases(_ => new MustExpected(false, "value must not be null.", "value", MustCodes.Text.Content.NotStartsWith));

        public static TheoryData<MustCase<(string? value, string prefix, StringComparison comparison)>> NullPrefixCases =>
        [
            new("null prefix", ("hello world", null!, StringComparison.Ordinal), new MustExpected(false, "prefix must not be null.", "prefix", MustCodes.Text.Content.NotStartsWith))
        ];
    }

    public static class NotStartsWith
    {
        public static TheoryData<MustCase<(string? value, string prefix, StringComparison comparison)>> ValidCases => F.StartsWith.InvalidScenarios.Except(nameof(F.StartsWith.NullValue)).ToMustCases(_ => new MustExpected(true));
        public static TheoryData<MustCase<(string? value, string prefix, StringComparison comparison)>> InvalidCases => F.StartsWith.ValidScenarios.ToMustCases(_ => new MustExpected(false, "value must not start with the specified prefix.", Code: MustCodes.Text.Content.StartsWith));
        public static TheoryData<MustCase<(string? value, string prefix, StringComparison comparison)>> NullCases => F.StartsWith.InvalidScenarios.Only(nameof(F.StartsWith.NullValue)).ToMustCases(_ => new MustExpected(false, "value must not be null.", "value", MustCodes.Text.Content.StartsWith));

        public static TheoryData<MustCase<(string? value, string prefix, StringComparison comparison)>> NullPrefixCases =>
        [
            new("null prefix", ("hello world", null!, StringComparison.Ordinal), new MustExpected(false, "prefix must not be null.", "prefix", MustCodes.Text.Content.StartsWith))
        ];
    }

    public static class EndsWith
    {
        public static TheoryData<MustCase<(string? value, string suffix, StringComparison comparison)>> ValidCases => F.EndsWith.ValidScenarios.ToMustCases();
        public static TheoryData<MustCase<(string? value, string suffix, StringComparison comparison)>> InvalidCases => F.EndsWith.InvalidScenarios.Except(nameof(F.EndsWith.NullValue)).ToMustCases(_ => new MustExpected(false, "value must end with the specified suffix.", Code: MustCodes.Text.Content.NotEndsWith));
        public static TheoryData<MustCase<(string? value, string suffix, StringComparison comparison)>> NullCases => F.EndsWith.InvalidScenarios.Only(nameof(F.EndsWith.NullValue)).ToMustCases(_ => new MustExpected(false, "value must not be null.", "value", MustCodes.Text.Content.NotEndsWith));

        public static TheoryData<MustCase<(string? value, string suffix, StringComparison comparison)>> NullSuffixCases =>
        [
            new("null suffix", ("hello world", null!, StringComparison.Ordinal), new MustExpected(false, "suffix must not be null.", "suffix", MustCodes.Text.Content.NotEndsWith))
        ];
    }

    public static class NotEndsWith
    {
        public static TheoryData<MustCase<(string? value, string suffix, StringComparison comparison)>> ValidCases => F.EndsWith.InvalidScenarios.Except(nameof(F.EndsWith.NullValue)).ToMustCases(_ => new MustExpected(true));
        public static TheoryData<MustCase<(string? value, string suffix, StringComparison comparison)>> InvalidCases => F.EndsWith.ValidScenarios.ToMustCases(_ => new MustExpected(false, "value must not end with the specified suffix.", Code: MustCodes.Text.Content.EndsWith));
        public static TheoryData<MustCase<(string? value, string suffix, StringComparison comparison)>> NullCases => F.EndsWith.InvalidScenarios.Only(nameof(F.EndsWith.NullValue)).ToMustCases(_ => new MustExpected(false, "value must not be null.", "value", MustCodes.Text.Content.EndsWith));

        public static TheoryData<MustCase<(string? value, string suffix, StringComparison comparison)>> NullSuffixCases =>
        [
            new("null suffix", ("hello world", null!, StringComparison.Ordinal), new MustExpected(false, "suffix must not be null.", "suffix", MustCodes.Text.Content.EndsWith))
        ];
    }
}
