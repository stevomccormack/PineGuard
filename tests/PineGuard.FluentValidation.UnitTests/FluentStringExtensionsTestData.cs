using System.Text.RegularExpressions;
using PineGuard.Codes;
using PineGuard.Testing.UnitTests.FluentValidation;
using PineGuard.Testing.UnitTests.Rules;
using F = PineGuard.Testing.Fixtures.StringRulesFixtures;

namespace PineGuard.FluentValidation.UnitTests;

public static partial class FluentStringExtensionsTestData
{
    public static class NotNullOrEmpty
    {
        public static TheoryData<FluentCase<string?>> Cases => F.IsNotNullOrEmpty.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsNotNullOrEmpty.NullValue) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must not be null or empty.", Code: MustCodes.Text.Content.NullOrEmpty)
        });
    }

    public static class NullOrEmpty
    {
        public static TheoryData<FluentCase<string?>> Cases => F.IsNullOrEmpty.AllScenarios.ToFluentCases(s => s.Name switch
        {
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be null or empty.")
        });
    }

    public static class NotNullOrWhiteSpace
    {
        public static TheoryData<FluentCase<string?>> Cases => F.IsNotNullOrWhiteSpace.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsNotNullOrWhiteSpace.NullValue) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must not be null or whitespace.")
        });
    }

    public static class NullOrWhiteSpace
    {
        public static TheoryData<FluentCase<string?>> Cases => F.IsNullOrWhiteSpace.AllScenarios.ToFluentCases(s => s.Name switch
        {
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be null or whitespace.")
        });
    }

    public static class ExactLength
    {
        public static TheoryData<FluentCase<(string? value, int length)>> Cases => F.IsExactLength.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsExactLength.NullValue) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be the expected length.")
        });
    }

    public static class LengthBetween
    {
        public static TheoryData<FluentCase<(string? value, int min, int max)>> Cases => F.IsLengthBetween.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsLengthBetween.NullValue) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must have a length within the expected range.")
        });
    }

    public static class LongerThan
    {
        private static readonly RuleScenario<(string? value, int length)>[] Scenarios =
        [
            new(nameof(F.IsLongerThan.LongerExclusive), (F.IsLongerThan.LongerExclusive.value, F.IsLongerThan.LongerExclusive.length), true),
            new(nameof(F.IsLongerThan.SameLengthExclusive), (F.IsLongerThan.SameLengthExclusive.value, F.IsLongerThan.SameLengthExclusive.length), false),
            new(nameof(F.IsLongerThan.NullValue), (F.IsLongerThan.NullValue.value, F.IsLongerThan.NullValue.length), true)
        ];

        public static TheoryData<FluentCase<(string? value, int length)>> Cases => Scenarios.ToFluentCases(s => s.Name switch
        {
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be longer than the specified length.")
        });
    }

    public static class LongerThanOrEqual
    {
        private static readonly RuleScenario<(string? value, int length)>[] Scenarios =
        [
            new(nameof(F.IsLongerThan.LongerExclusive), (F.IsLongerThan.LongerExclusive.value, F.IsLongerThan.LongerExclusive.length), true),
            new(nameof(F.IsLongerThan.SameLengthInclusive), (F.IsLongerThan.SameLengthInclusive.value, F.IsLongerThan.SameLengthInclusive.length), true),
            new("ShorterThanFails", ("ab", 3), false),
            new(nameof(F.IsLongerThan.NullValue), (F.IsLongerThan.NullValue.value, F.IsLongerThan.NullValue.length), true)
        ];

        public static TheoryData<FluentCase<(string? value, int length)>> Cases => Scenarios.ToFluentCases(s => s.Name switch
        {
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be longer than or equal to the specified length.")
        });
    }

    public static class ShorterThan
    {
        private static readonly RuleScenario<(string? value, int length)>[] Scenarios =
        [
            new(nameof(F.IsShorterThan.ShorterExclusive), (F.IsShorterThan.ShorterExclusive.value, F.IsShorterThan.ShorterExclusive.length), true),
            new(nameof(F.IsShorterThan.SameLengthExclusive), (F.IsShorterThan.SameLengthExclusive.value, F.IsShorterThan.SameLengthExclusive.length), false),
            new(nameof(F.IsShorterThan.NullValue), (F.IsShorterThan.NullValue.value, F.IsShorterThan.NullValue.length), true)
        ];

        public static TheoryData<FluentCase<(string? value, int length)>> Cases => Scenarios.ToFluentCases(s => s.Name switch
        {
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be shorter than the specified length.")
        });
    }

    public static class ShorterThanOrEqual
    {
        private static readonly RuleScenario<(string? value, int length)>[] Scenarios =
        [
            new(nameof(F.IsShorterThan.ShorterExclusive), (F.IsShorterThan.ShorterExclusive.value, F.IsShorterThan.ShorterExclusive.length), true),
            new(nameof(F.IsShorterThan.SameLengthInclusive), (F.IsShorterThan.SameLengthInclusive.value, F.IsShorterThan.SameLengthInclusive.length), true),
            new("LongerThanFails", ("abcd", 3), false),
            new(nameof(F.IsShorterThan.NullValue), (F.IsShorterThan.NullValue.value, F.IsShorterThan.NullValue.length), true)
        ];

        public static TheoryData<FluentCase<(string? value, int length)>> Cases => Scenarios.ToFluentCases(s => s.Name switch
        {
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be shorter than or equal to the specified length.")
        });
    }

    public static class DigitsOnly
    {
        public static TheoryData<FluentCase<string?>> Cases => F.IsDigitsOnly.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsDigitsOnly.NullValue) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must contain digits only.")
        });
    }

    public static class NotDigitsOnly
    {
        private static readonly RuleScenario<string?>[] Scenarios =
        [
            new("HasNonDigit", "12a3", true),
            new("AllDigitsFails", "012345", false),
            new("NullValue", null, true)
        ];

        public static TheoryData<FluentCase<string?>> Cases => Scenarios.ToFluentCases(s => s.Name switch
        {
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must not contain digits only.")
        });
    }

    public static class DigitsOnlyWithAllowed
    {
        private static readonly RuleScenario<(string? value, char[] allowedNonDigitChars)>[] Scenarios =
        [
            new(nameof(F.IsDigitsOnlyWithAllowedNonDigitChars.DashAllowed), ((string?)F.IsDigitsOnlyWithAllowedNonDigitChars.DashAllowed.value, F.IsDigitsOnlyWithAllowedNonDigitChars.DashAllowed.allowedNonDigitChars), true),
            new(nameof(F.IsDigitsOnlyWithAllowedNonDigitChars.DisallowedChar), ((string?)F.IsDigitsOnlyWithAllowedNonDigitChars.DisallowedChar.value, F.IsDigitsOnlyWithAllowedNonDigitChars.DisallowedChar.allowedNonDigitChars), false),
            new("NullValue", (null, []), true)
        ];

        public static TheoryData<FluentCase<(string? value, char[] allowedNonDigitChars)>> Cases => Scenarios.ToFluentCases(s => s.Name switch
        {
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must contain digits only (except for allowed characters).")
        });
    }

    public static class NotDigitsOnlyWithAllowed
    {
        private static readonly RuleScenario<(string? value, char[] allowedNonDigitChars)>[] Scenarios =
        [
            new("HasDisallowedNonDigit", ("123a", [' ']), true),
            new("DigitsWithAllowedFails", ("123 ", [' ']), false),
            new("NullValue", (null, []), true)
        ];

        public static TheoryData<FluentCase<(string? value, char[] allowedNonDigitChars)>> Cases => Scenarios.ToFluentCases(s => s.Name switch
        {
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must not contain digits only (considering allowed characters).")
        });
    }

    public static class Uppercase
    {
        public static TheoryData<FluentCase<(string? value, bool lettersOnly)>> Cases => F.IsUppercase.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsUppercase.NullValue) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be uppercase.")
        });
    }

    public static class NotUppercase
    {
        private static readonly RuleScenario<(string? value, bool lettersOnly)>[] Scenarios =
        [
            new("NotUppercase", ("abc", false), true),
            new("UppercaseFails", ("ABC", false), false),
            new("NullValue", (null, false), true)
        ];

        public static TheoryData<FluentCase<(string? value, bool lettersOnly)>> Cases => Scenarios.ToFluentCases(s => s.Name switch
        {
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must not be uppercase.")
        });
    }

    public static class Lowercase
    {
        public static TheoryData<FluentCase<(string? value, bool lettersOnly)>> Cases => F.IsLowercase.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsLowercase.NullValue) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be lowercase.")
        });
    }

    public static class NotLowercase
    {
        private static readonly RuleScenario<(string? value, bool lettersOnly)>[] Scenarios =
        [
            new("NotLowercase", ("ABC", false), true),
            new("LowercaseFails", ("abc", false), false),
            new("NullValue", (null, false), true)
        ];

        public static TheoryData<FluentCase<(string? value, bool lettersOnly)>> Cases => Scenarios.ToFluentCases(s => s.Name switch
        {
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must not be lowercase.")
        });
    }

    public static class Alphabetic
    {
        private static readonly RuleScenario<string?>[] Scenarios =
        [
            new("Alpha", "abc", true),
            new("NumericFails", "123", false),
            new("NullValue", null, true)
        ];

        public static TheoryData<FluentCase<string?>> Cases => Scenarios.ToFluentCases(s => s.Name switch
        {
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be alphabetic.")
        });
    }

    public static class NotAlphabetic
    {
        private static readonly RuleScenario<string?>[] Scenarios =
        [
            new("NotAlphabetic", "123", true),
            new("AlphaFails", "abc", false),
            new("NullValue", null, true)
        ];

        public static TheoryData<FluentCase<string?>> Cases => Scenarios.ToFluentCases(s => s.Name switch
        {
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must not be alphabetic.")
        });
    }

    public static class Numeric
    {
        private static readonly RuleScenario<string?>[] Scenarios =
        [
            new("Numeric", "123", true),
            new("AlphaFails", "abc", false),
            new("NullValue", null, true)
        ];

        public static TheoryData<FluentCase<string?>> Cases => Scenarios.ToFluentCases(s => s.Name switch
        {
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be numeric.")
        });
    }

    public static class NotNumeric
    {
        private static readonly RuleScenario<string?>[] Scenarios =
        [
            new("NotNumeric", "abc", true),
            new("NumericFails", "123", false),
            new("NullValue", null, true)
        ];

        public static TheoryData<FluentCase<string?>> Cases => Scenarios.ToFluentCases(s => s.Name switch
        {
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must not be numeric.")
        });
    }

    public static class Alphanumeric
    {
        private static readonly RuleScenario<string?>[] Scenarios =
        [
            new("Alphanumeric", "abc123", true),
            new("SymbolFails", "abc-123", false),
            new("NullValue", null, true)
        ];

        public static TheoryData<FluentCase<string?>> Cases => Scenarios.ToFluentCases(s => s.Name switch
        {
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be alphanumeric.")
        });
    }

    public static class NotAlphanumeric
    {
        private static readonly RuleScenario<string?>[] Scenarios =
        [
            new("NotAlphanumeric", "abc-123", true),
            new("AlphanumericFails", "abc123", false),
            new("NullValue", null, true)
        ];

        public static TheoryData<FluentCase<string?>> Cases => Scenarios.ToFluentCases(s => s.Name switch
        {
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must not be alphanumeric.")
        });
    }

    public static class ContainsAny
    {
        private static readonly RuleScenario<(string? value, char[] anyOf)>[] Scenarios =
        [
            new("ContainsAny", ("abc", ['b', 'c']), true),
            new("ContainsNoneFails", ("abc", ['x', 'y']), false),
            new("NullValue", (null, ['b']), true)
        ];

        public static TheoryData<FluentCase<(string? value, char[] anyOf)>> Cases => Scenarios.ToFluentCases(s => s.Name switch
        {
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must contain at least one of the expected characters.")
        });
    }

    public static class Ascii
    {
        public static TheoryData<FluentCase<string?>> Cases => F.IsAscii.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsAscii.Null) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be ASCII.")
        });
    }

    public static class NotAscii
    {
        private static readonly RuleScenario<string?>[] Scenarios =
        [
            new("NonAscii", "abc\u00FF", true),
            new("AsciiFails", "abc", false),
            new("NullValue", null, true)
        ];

        public static TheoryData<FluentCase<string?>> Cases => Scenarios.ToFluentCases(s => s.Name switch
        {
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must not be ASCII.")
        });
    }

    public static class Match
    {
        public static TheoryData<FluentCase<(string? value, Regex pattern)>> Cases => F.IsMatch.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsMatch.NullString) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must match the specified pattern.")
        });
    }

    public static partial class NotMatch
    {
        [GeneratedRegex("^abc$", RegexOptions.CultureInvariant)]
        private static partial Regex PatternRegex();

        private static readonly Regex Pattern = PatternRegex();

        private static readonly RuleScenario<(string? value, Regex pattern)>[] Scenarios =
        [
            new("NoMatch", ("abcd", Pattern), true),
            new("MatchFails", ("abc", Pattern), false),
            new("NullValue", (null, Pattern), true)
        ];

        public static TheoryData<FluentCase<(string? value, Regex pattern)>> Cases => Scenarios.ToFluentCases(s => s.Name switch
        {
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must not match the specified pattern.")
        });
    }

    public static class RegexPattern
    {
        public static TheoryData<FluentCase<string?>> Cases => F.IsRegexPattern.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsRegexPattern.NullValue) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be a valid regular expression pattern.", Code: MustCodes.Text.Pattern.Invalid)
        });
    }

    public static class NotWhitespace
    {
        public static TheoryData<FluentCase<string?>> Cases => F.IsWhitespace.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsWhitespace.NullValue) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(false, "Value must not be whitespace."),
            _ => new FluentExpected(true)
        });
    }

    public static class ContainsWhitespace
    {
        public static TheoryData<FluentCase<string?>> Cases => F.ContainsWhitespace.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.ContainsWhitespace.NullValue) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must contain whitespace.")
        });
    }

    public static class NotContainsWhitespace
    {
        private static readonly RuleScenario<string?>[] Scenarios =
        [
            new("NoWhitespace", "abc", true),
            new("WhitespaceFails", "a b", false),
            new("NullValue", null, true)
        ];

        public static TheoryData<FluentCase<string?>> Cases => Scenarios.ToFluentCases(s => s.Name switch
        {
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must not contain whitespace.")
        });
    }

    public static class ContainsControlChars
    {
        public static TheoryData<FluentCase<string?>> Cases => F.ContainsControlChars.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.ContainsControlChars.NullValue) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must contain control characters.")
        });
    }

    public static class NotContainsControlChars
    {
        public static TheoryData<FluentCase<string?>> Cases => F.NotContainsControlChars.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.NotContainsControlChars.NullValue) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must not contain control characters.")
        });
    }

    public static class PrintableAscii
    {
        public static TheoryData<FluentCase<(string? value, bool allowCommonWhitespace)>> Cases => F.IsPrintableAscii.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsPrintableAscii.NullValue) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be printable ASCII.")
        });
    }

    public static class NotPrintableAscii
    {
        private static readonly RuleScenario<string?>[] Scenarios =
        [
            new("Unprintable", "abc\x01", true),
            new("PrintableFails", "abc!", false),
            new("NullValue", null, true)
        ];

        public static TheoryData<FluentCase<string?>> Cases => Scenarios.ToFluentCases(s => s.Name switch
        {
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must not be printable ASCII.")
        });
    }

    public static class ContainsAllowedOnly
    {
        public static TheoryData<FluentCase<(string? value, char[] allowedChars)>> Cases => F.ContainsAllowedOnly.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.ContainsAllowedOnly.NullValue) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must contain only allowed characters.")
        });
    }

    public static class NotContainsAllowedOnly
    {
        private static readonly RuleScenario<(string? value, char[] allowedChars)>[] Scenarios =
        [
            new("HasDisallowed", ("abc", ['a', 'b']), true),
            new("OnlyAllowedFails", ("ab", ['a', 'b']), false),
            new("NullValue", (null, ['a']), true)
        ];

        public static TheoryData<FluentCase<(string? value, char[] allowedChars)>> Cases => Scenarios.ToFluentCases(s => s.Name switch
        {
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must not contain only allowed characters.")
        });
    }

    public static class ContainsDisallowed
    {
        public static TheoryData<FluentCase<(string? value, char[] disallowedChars)>> Cases => F.ContainsDisallowed.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.ContainsDisallowed.NullValue) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must contain a disallowed character.")
        });
    }

    public static class NotContainsDisallowed
    {
        private static readonly RuleScenario<(string? value, char[] disallowedChars)>[] Scenarios =
        [
            new("NoDisallowed", ("b", ['a']), true),
            new("DisallowedFails", ("ab", ['a']), false),
            new("NullValue", (null, ['a']), true)
        ];

        public static TheoryData<FluentCase<(string? value, char[] disallowedChars)>> Cases => Scenarios.ToFluentCases(s => s.Name switch
        {
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must not contain any disallowed characters.")
        });
    }

    public static class Contains
    {
        public static TheoryData<FluentCase<(string? value, string substring, StringComparison comparison)>> Cases => F.Contains.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.Contains.NullValue) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must contain the specified substring.", Code: MustCodes.Text.Content.NotContains)
        });
    }

    public static class NotContains
    {
        public static TheoryData<FluentCase<(string? value, string substring, StringComparison comparison)>> Cases => F.Contains.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.Contains.NullValue) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(false, "Value must not contain the specified substring.", Code: MustCodes.Text.Content.Contains),
            _ => new FluentExpected(true)
        });
    }

    public static class StartsWith
    {
        public static TheoryData<FluentCase<(string? value, string prefix, StringComparison comparison)>> Cases => F.StartsWith.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.StartsWith.NullValue) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must start with the specified prefix.", Code: MustCodes.Text.Content.NotStartsWith)
        });
    }

    public static class NotStartsWith
    {
        public static TheoryData<FluentCase<(string? value, string prefix, StringComparison comparison)>> Cases => F.StartsWith.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.StartsWith.NullValue) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(false, "Value must not start with the specified prefix.", Code: MustCodes.Text.Content.StartsWith),
            _ => new FluentExpected(true)
        });
    }

    public static class EndsWith
    {
        public static TheoryData<FluentCase<(string? value, string suffix, StringComparison comparison)>> Cases => F.EndsWith.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.EndsWith.NullValue) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must end with the specified suffix.", Code: MustCodes.Text.Content.NotEndsWith)
        });
    }

    public static class NotEndsWith
    {
        public static TheoryData<FluentCase<(string? value, string suffix, StringComparison comparison)>> Cases => F.EndsWith.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.EndsWith.NullValue) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(false, "Value must not end with the specified suffix.", Code: MustCodes.Text.Content.EndsWith),
            _ => new FluentExpected(true)
        });
    }
}
