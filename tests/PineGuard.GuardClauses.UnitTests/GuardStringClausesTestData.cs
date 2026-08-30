using System.Text.RegularExpressions;
using PineGuard.Codes;
using PineGuard.Testing.UnitTests.GuardClauses;
using PineGuard.Testing.UnitTests.Rules;
using F = PineGuard.Testing.Fixtures.StringRulesFixtures;

namespace PineGuard.GuardClauses.UnitTests;

public static partial class GuardStringClausesTestData
{
    // Guard.Against.NotNullOrEmpty — throws when value is NOT (null or empty)
    public static class NotNullOrEmpty
    {
        public static TheoryData<GuardCase<string?>> ValidCases => F.IsNullOrEmpty.ValidScenarios.Except(nameof(F.IsNullOrEmpty.NullValue)).ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<string?>> InvalidCases => F.IsNullOrEmpty.InvalidScenarios.ToGuardCases("value");
    }

    // Guard.Against.NullOrEmpty — throws when value IS null or empty
    public static class NullOrEmpty
    {
        public static TheoryData<GuardCase<string?>> ValidCases => F.IsNotNullOrEmpty.ValidScenarios.ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<string?>> InvalidCases => F.IsNotNullOrEmpty.InvalidScenarios.ToGuardCases("value");
    }

    // Guard.Against.NotNullOrWhiteSpace — throws when value is NOT (null or whitespace)
    public static class NotNullOrWhiteSpace
    {
        public static TheoryData<GuardCase<string?>> ValidCases => F.IsNullOrWhiteSpace.ValidScenarios.Except(nameof(F.IsNullOrWhiteSpace.NullValue)).ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<string?>> InvalidCases => F.IsNullOrWhiteSpace.InvalidScenarios.ToGuardCases("value");
    }

    // Guard.Against.NullOrWhiteSpace — throws when value IS null or whitespace
    public static class NullOrWhiteSpace
    {
        public static TheoryData<GuardCase<string?>> ValidCases => F.IsNotNullOrWhiteSpace.ValidScenarios.ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<string?>> InvalidCases => F.IsNotNullOrWhiteSpace.InvalidScenarios.ToGuardCases("value");
    }

    // Guard.Against.NotExactLength — throws when value does NOT have exact length (inline: fixture has 3-tuple)
    public static class NotExactLength
    {
        private static readonly (string? value, int length) Matching = ("abc", 3);
        private static readonly (string? value, int length) TooShort = ("ab", 3);
        private static readonly (string? value, int length) TooLong = ("abcd", 3);
        private static readonly (string? value, int length) NullValue = (null, 3);

        public static TheoryData<GuardCase<(string? value, int length)>> ValidCases =>
        [
            new(nameof(Matching), Matching, new GuardExpected(true))
        ];

        public static TheoryData<GuardCase<(string? value, int length)>> InvalidCases =>
        [
            new(nameof(TooShort), TooShort, new GuardExpected(false, typeof(ArgumentException), "value")),
            new(nameof(TooLong), TooLong, new GuardExpected(false, typeof(ArgumentException), "value")),
            new(nameof(NullValue), NullValue, new GuardExpected(false, typeof(ArgumentNullException), "value"))
        ];
    }

    // Guard.Against.NotLengthBetween — throws when length is NOT between min and max (inline: fixture has simple 3-tuple)
    public static class NotLengthBetween
    {
        private static readonly (string? value, int min, int max) WithinBounds = ("abc", 2, 4);
        private static readonly (string? value, int min, int max) TooShort = ("a", 2, 4);
        private static readonly (string? value, int min, int max) TooLong = ("abcde", 2, 4);
        private static readonly (string? value, int min, int max) NullValue = (null, 2, 4);

        public static TheoryData<GuardCase<(string? value, int min, int max)>> ValidCases =>
        [
            new(nameof(WithinBounds), WithinBounds, new GuardExpected(true))
        ];

        public static TheoryData<GuardCase<(string? value, int min, int max)>> InvalidCases =>
        [
            new(nameof(TooShort), TooShort, new GuardExpected(false, typeof(ArgumentException), "value")),
            new(nameof(TooLong), TooLong, new GuardExpected(false, typeof(ArgumentException), "value")),
            new(nameof(NullValue), NullValue, new GuardExpected(false, typeof(ArgumentNullException), "value"))
        ];
    }

    // Guard.Against.ShorterThanOrEqual — throws when value is shorter than or equal to length (calls Must.Be.LongerThan)
    public static class ShorterThanOrEqual
    {
        private static readonly (string? value, int length) Longer = ("abcd", 3);
        private static readonly (string? value, int length) Equal = ("abc", 3);
        private static readonly (string? value, int length) Shorter = ("ab", 3);
        private static readonly (string? value, int length) NullValue = (null, 3);

        public static TheoryData<GuardCase<(string? value, int length)>> ValidCases =>
        [
            new(nameof(Longer), Longer, new GuardExpected(true))
        ];

        public static TheoryData<GuardCase<(string? value, int length)>> InvalidCases =>
        [
            new(nameof(Equal), Equal, new GuardExpected(false, typeof(ArgumentException), "value")),
            new(nameof(Shorter), Shorter, new GuardExpected(false, typeof(ArgumentException), "value")),
            new(nameof(NullValue), NullValue, new GuardExpected(false, typeof(ArgumentNullException), "value"))
        ];
    }

    // Guard.Against.ShorterThan — throws when value is shorter than length (calls Must.Be.LongerThanOrEqual)
    public static class ShorterThan
    {
        private static readonly (string? value, int length) LongerOrEqual1 = ("abcd", 3);
        private static readonly (string? value, int length) LongerOrEqual2 = ("abc", 3);
        private static readonly (string? value, int length) TooShort = ("ab", 3);
        private static readonly (string? value, int length) NullValue = (null, 3);

        public static TheoryData<GuardCase<(string? value, int length)>> ValidCases =>
        [
            new(nameof(LongerOrEqual1), LongerOrEqual1, new GuardExpected(true)),
            new(nameof(LongerOrEqual2), LongerOrEqual2, new GuardExpected(true))
        ];

        public static TheoryData<GuardCase<(string? value, int length)>> InvalidCases =>
        [
            new(nameof(TooShort), TooShort, new GuardExpected(false, typeof(ArgumentException), "value")),
            new(nameof(NullValue), NullValue, new GuardExpected(false, typeof(ArgumentNullException), "value"))
        ];
    }

    // Guard.Against.LongerThanOrEqual — throws when value is longer than or equal to length (calls Must.Be.ShorterThan)
    public static class LongerThanOrEqual
    {
        private static readonly (string? value, int length) Shorter = ("ab", 3);
        private static readonly (string? value, int length) Equal = ("abc", 3);
        private static readonly (string? value, int length) Longer = ("abcd", 3);
        private static readonly (string? value, int length) NullValue = (null, 3);

        public static TheoryData<GuardCase<(string? value, int length)>> ValidCases =>
        [
            new(nameof(Shorter), Shorter, new GuardExpected(true))
        ];

        public static TheoryData<GuardCase<(string? value, int length)>> InvalidCases =>
        [
            new(nameof(Equal), Equal, new GuardExpected(false, typeof(ArgumentException), "value")),
            new(nameof(Longer), Longer, new GuardExpected(false, typeof(ArgumentException), "value")),
            new(nameof(NullValue), NullValue, new GuardExpected(false, typeof(ArgumentNullException), "value"))
        ];
    }

    // Guard.Against.LongerThan — throws when value is longer than length (calls Must.Be.ShorterThanOrEqual)
    public static class LongerThan
    {
        private static readonly (string? value, int length) ShorterOrEqual1 = ("ab", 3);
        private static readonly (string? value, int length) ShorterOrEqual2 = ("abc", 3);
        private static readonly (string? value, int length) TooLong = ("abcd", 3);
        private static readonly (string? value, int length) NullValue = (null, 3);

        public static TheoryData<GuardCase<(string? value, int length)>> ValidCases =>
        [
            new(nameof(ShorterOrEqual1), ShorterOrEqual1, new GuardExpected(true)),
            new(nameof(ShorterOrEqual2), ShorterOrEqual2, new GuardExpected(true))
        ];

        public static TheoryData<GuardCase<(string? value, int length)>> InvalidCases =>
        [
            new(nameof(TooLong), TooLong, new GuardExpected(false, typeof(ArgumentException), "value")),
            new(nameof(NullValue), NullValue, new GuardExpected(false, typeof(ArgumentNullException), "value"))
        ];
    }

    // Guard.Against.NotMatch — throws when value does NOT match pattern (calls Must.Be.Match)
    public static class NotMatch
    {
        public static TheoryData<GuardCase<(string? value, Regex pattern)>> ValidCases => F.IsMatch.ValidScenarios.ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<(string? value, Regex pattern)>> InvalidCases => F.IsMatch.InvalidScenarios.ToGuardCases(s => s.Inputs.value is null ? new GuardExpected(false, typeof(ArgumentNullException), "value") : new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    // Guard.Against.Match — throws when value DOES match pattern (calls Must.Be.NotMatch)
    public static partial class Match
    {
        [GeneratedRegex(@"^\d+$")]
        private static partial Regex PatternRegex();

        private static readonly Regex Pattern = PatternRegex();
        private static readonly (string? value, Regex pattern) NoMatch = ("abc", Pattern);
        private static readonly (string? value, Regex pattern) DoesMatch = ("123", Pattern);
        private static readonly (string? value, Regex pattern) NullValue = (null, Pattern);

        public static TheoryData<GuardCase<(string? value, Regex pattern)>> ValidCases =>
        [
            new(nameof(NoMatch), NoMatch, new GuardExpected(true))
        ];

        public static TheoryData<GuardCase<(string? value, Regex pattern)>> InvalidCases =>
        [
            new(nameof(DoesMatch), DoesMatch, new GuardExpected(false, typeof(ArgumentException), "value")),
            new(nameof(NullValue), NullValue, new GuardExpected(false, typeof(ArgumentNullException), "value"))
        ];
    }

    // Guard.Against.NotRegexPattern — throws when value is NOT a valid regex pattern (delegates to Must.Be.RegexPattern)
    public static class NotRegexPattern
    {
        public static TheoryData<GuardCase<string?>> ValidCases => F.IsRegexPattern.ValidScenarios.ToGuardCases();

        public static TheoryData<GuardCase<string?>> InvalidCases => F.IsRegexPattern.InvalidScenarios.ToGuardCases(s => new GuardExpected(false, s.IsNull ? typeof(ArgumentNullException) : typeof(ArgumentException), "value", Code: MustCodes.Text.Pattern.Invalid));
    }

    // Guard.Against.NotAlphabetic — throws when value is NOT alphabetic (calls Must.Be.Alphabetic)
    public static class NotAlphabetic
    {
        private static readonly string? Alphabetic = "abc";
        private static readonly string? Numeric = "123";
        private static readonly string? Mixed = "a1b";
        private static readonly string? NullValue = null;

        public static TheoryData<GuardCase<string?>> ValidCases =>
        [
            new(nameof(Alphabetic), Alphabetic, new GuardExpected(true))
        ];

        public static TheoryData<GuardCase<string?>> InvalidCases =>
        [
            new(nameof(Numeric), Numeric, new GuardExpected(false, typeof(ArgumentException), "value")),
            new(nameof(Mixed), Mixed, new GuardExpected(false, typeof(ArgumentException), "value")),
            new(nameof(NullValue), NullValue, new GuardExpected(false, typeof(ArgumentNullException), "value"))
        ];
    }

    // Guard.Against.NotNumeric — throws when value is NOT numeric (calls Must.Be.Numeric)
    public static class NotNumeric
    {
        private static readonly string? Numeric = "123";
        private static readonly string? Alphabetic = "abc";
        private static readonly string? Mixed = "a1b";
        private static readonly string? NullValue = null;

        public static TheoryData<GuardCase<string?>> ValidCases =>
        [
            new(nameof(Numeric), Numeric, new GuardExpected(true))
        ];

        public static TheoryData<GuardCase<string?>> InvalidCases =>
        [
            new(nameof(Alphabetic), Alphabetic, new GuardExpected(false, typeof(ArgumentException), "value")),
            new(nameof(Mixed), Mixed, new GuardExpected(false, typeof(ArgumentException), "value")),
            new(nameof(NullValue), NullValue, new GuardExpected(false, typeof(ArgumentNullException), "value"))
        ];
    }

    // Guard.Against.NotAlphanumeric — throws when value is NOT alphanumeric (calls Must.Be.Alphanumeric)
    public static class NotAlphanumeric
    {
        private static readonly string? Alphanumeric = "abc123";
        private static readonly string? Special = "@#$";
        private static readonly string? NullValue = null;

        public static TheoryData<GuardCase<string?>> ValidCases =>
        [
            new(nameof(Alphanumeric), Alphanumeric, new GuardExpected(true))
        ];

        public static TheoryData<GuardCase<string?>> InvalidCases =>
        [
            new(nameof(Special), Special, new GuardExpected(false, typeof(ArgumentException), "value")),
            new(nameof(NullValue), NullValue, new GuardExpected(false, typeof(ArgumentNullException), "value"))
        ];
    }

    // Guard.Against.NotDigitsOnly — throws when value is NOT digits-only (calls Must.Be.DigitsOnly, no allowed chars)
    public static class NotDigitsOnly
    {
        public static TheoryData<GuardCase<string?>> ValidCases => F.IsDigitsOnly.ValidScenarios.ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<string?>> InvalidCases => F.IsDigitsOnly.InvalidScenarios.ToGuardCases("value");
    }

    // Guard.Against.NotDigitsOnly (with allowedNonDigitChars overload)
    public static class NotDigitsOnlyWithAllowed
    {
        public static TheoryData<GuardCase<(string value, char[] allowedNonDigitChars)>> ValidCases => F.IsDigitsOnlyWithAllowedNonDigitChars.ValidScenarios.ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<(string value, char[] allowedNonDigitChars)>> InvalidCases => F.IsDigitsOnlyWithAllowedNonDigitChars.InvalidScenarios.ToGuardCases("value");
    }

    // Guard.Against.Alphabetic — throws when value IS alphabetic (calls Must.Be.NotAlphabetic)
    public static class Alphabetic
    {
        private static readonly string? Alphabetic1 = "abc";
        private static readonly string? Numeric = "123";
        private static readonly string? NullValue = null;

        public static TheoryData<GuardCase<string?>> ValidCases =>
        [
            new(nameof(Numeric), Numeric, new GuardExpected(true))
        ];

        public static TheoryData<GuardCase<string?>> InvalidCases =>
        [
            new(nameof(Alphabetic1), Alphabetic1, new GuardExpected(false, typeof(ArgumentException), "value")),
            new(nameof(NullValue), NullValue, new GuardExpected(false, typeof(ArgumentNullException), "value"))
        ];
    }

    // Guard.Against.Numeric — throws when value IS numeric (calls Must.Be.NotNumeric)
    public static class Numeric
    {
        private static readonly string? Numeric1 = "123";
        private static readonly string? Alphabetic = "abc";
        private static readonly string? NullValue = null;

        public static TheoryData<GuardCase<string?>> ValidCases =>
        [
            new(nameof(Alphabetic), Alphabetic, new GuardExpected(true))
        ];

        public static TheoryData<GuardCase<string?>> InvalidCases =>
        [
            new(nameof(Numeric1), Numeric1, new GuardExpected(false, typeof(ArgumentException), "value")),
            new(nameof(NullValue), NullValue, new GuardExpected(false, typeof(ArgumentNullException), "value"))
        ];
    }

    // Guard.Against.Alphanumeric — throws when value IS alphanumeric (calls Must.Be.NotAlphanumeric)
    public static class Alphanumeric
    {
        private static readonly string? Alphanumeric1 = "abc123";
        private static readonly string? Special = "@#$";
        private static readonly string? NullValue = null;

        public static TheoryData<GuardCase<string?>> ValidCases =>
        [
            new(nameof(Special), Special, new GuardExpected(true))
        ];

        public static TheoryData<GuardCase<string?>> InvalidCases =>
        [
            new(nameof(Alphanumeric1), Alphanumeric1, new GuardExpected(false, typeof(ArgumentException), "value")),
            new(nameof(NullValue), NullValue, new GuardExpected(false, typeof(ArgumentNullException), "value"))
        ];
    }

    // Guard.Against.DigitsOnly — throws when value IS digits-only (calls Must.Be.NotDigitsOnly, no allowed chars)
    public static class DigitsOnly
    {
        private static readonly string? Digits = "123";
        private static readonly string? Alphabetic = "abc";
        private static readonly string? NullValue = null;

        public static TheoryData<GuardCase<string?>> ValidCases =>
        [
            new(nameof(Alphabetic), Alphabetic, new GuardExpected(true))
        ];

        public static TheoryData<GuardCase<string?>> InvalidCases =>
        [
            new(nameof(Digits), Digits, new GuardExpected(false, typeof(ArgumentException), "value")),
            new(nameof(NullValue), NullValue, new GuardExpected(false, typeof(ArgumentNullException), "value"))
        ];
    }

    // Guard.Against.DigitsOnly (with allowedNonDigitChars overload) — throws when IS digits-only with allowed chars
    public static class DigitsOnlyWithAllowed
    {
        private static readonly char[] Allowed = ['-'];
        private static readonly (string? value, char[] allowedNonDigitChars) WithDash = ("12-3", Allowed);
        private static readonly (string? value, char[] allowedNonDigitChars) Alpha = ("12a3", Allowed);
        private static readonly (string? value, char[] allowedNonDigitChars) NullValue = (null, Allowed);

        public static TheoryData<GuardCase<(string? value, char[] allowedNonDigitChars)>> ValidCases =>
        [
            new(nameof(Alpha), Alpha, new GuardExpected(true))
        ];

        public static TheoryData<GuardCase<(string? value, char[] allowedNonDigitChars)>> InvalidCases =>
        [
            new(nameof(WithDash), WithDash, new GuardExpected(false, typeof(ArgumentException), "value")),
            new(nameof(NullValue), NullValue, new GuardExpected(false, typeof(ArgumentNullException), "value"))
        ];
    }

    // Guard.Against.Uppercase — throws when value IS uppercase (calls Must.Be.NotUppercase)
    public static class Uppercase
    {
        public static TheoryData<GuardCase<(string? value, bool lettersOnly)>> ValidCases => F.IsUppercase.InvalidScenarios.Where(s => s.Inputs.value is not null).ToArray().ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<(string? value, bool lettersOnly)>> InvalidCases => [.. F.IsUppercase.ValidScenarios.ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value")), .. F.IsUppercase.InvalidScenarios.Where(s => s.Inputs.value is null).ToArray().ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentNullException), "value"))];
    }

    // Guard.Against.Lowercase — throws when value IS lowercase (calls Must.Be.NotLowercase)
    public static class Lowercase
    {
        public static TheoryData<GuardCase<(string? value, bool lettersOnly)>> ValidCases => F.IsLowercase.InvalidScenarios.Where(s => s.Inputs.value is not null).ToArray().ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<(string? value, bool lettersOnly)>> InvalidCases => [.. F.IsLowercase.ValidScenarios.ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value")), .. F.IsLowercase.InvalidScenarios.Where(s => s.Inputs.value is null).ToArray().ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentNullException), "value"))];
    }

    // Guard.Against.NotAscii — throws when value is NOT ASCII (calls Must.Be.Ascii)
    public static class NotAscii
    {
        public static TheoryData<GuardCase<string?>> ValidCases => F.IsAscii.ValidScenarios.ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<string?>> InvalidCases => F.IsAscii.InvalidScenarios.ToGuardCases("value");
    }

    // Guard.Against.Ascii — throws when value IS ASCII (calls Must.Be.NotAscii)
    public static class Ascii
    {
        private static readonly string? AsciiValue = "abc";
        private static readonly string? NonAscii = "caf\u00e9";
        private static readonly string? NullValue = null;

        public static TheoryData<GuardCase<string?>> ValidCases =>
        [
            new(nameof(NonAscii), NonAscii, new GuardExpected(true))
        ];

        public static TheoryData<GuardCase<string?>> InvalidCases =>
        [
            new(nameof(AsciiValue), AsciiValue, new GuardExpected(false, typeof(ArgumentException), "value")),
            new(nameof(NullValue), NullValue, new GuardExpected(false, typeof(ArgumentNullException), "value"))
        ];
    }

    // Guard.Against.NotPrintableAscii — throws when value is NOT printable ASCII (calls Must.Be.PrintableAscii)
    public static class NotPrintableAscii
    {
        public static TheoryData<GuardCase<(string? value, bool allowCommonWhitespace)>> ValidCases => F.IsPrintableAscii.ValidScenarios.ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<(string? value, bool allowCommonWhitespace)>> InvalidCases => F.IsPrintableAscii.InvalidScenarios.ToGuardCases(s => s.Inputs.value is null ? new GuardExpected(false, typeof(ArgumentNullException), "value") : new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    // Guard.Against.PrintableAscii — throws when value IS printable ASCII (calls Must.Be.NotPrintableAscii)
    public static class PrintableAscii
    {
        private static readonly (string? value, bool allowCommonWhitespace) Printable = ("Hello", false);
        private static readonly (string? value, bool allowCommonWhitespace) ContainsTab = ("A\tB", false);
        private static readonly (string? value, bool allowCommonWhitespace) NullValue = (null, false);

        public static TheoryData<GuardCase<(string? value, bool allowCommonWhitespace)>> ValidCases =>
        [
            new(nameof(ContainsTab), ContainsTab, new GuardExpected(true))
        ];

        public static TheoryData<GuardCase<(string? value, bool allowCommonWhitespace)>> InvalidCases =>
        [
            new(nameof(Printable), Printable, new GuardExpected(false, typeof(ArgumentException), "value")),
            new(nameof(NullValue), NullValue, new GuardExpected(false, typeof(ArgumentNullException), "value"))
        ];
    }

    // Guard.Against.IsWhitespace — throws when value IS whitespace (calls Must.Be.NotWhitespace)
    public static class IsWhitespace
    {
        public static TheoryData<GuardCase<string?>> ValidCases => F.IsWhitespace.InvalidScenarios.Where(s => s.Inputs is not null).ToArray().ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<string?>> InvalidCases => [.. F.IsWhitespace.ValidScenarios.ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value")), .. F.IsWhitespace.InvalidScenarios.Where(s => s.Inputs is null).ToArray().ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentNullException), "value"))];
    }

    // Guard.Against.Whitespace — throws when value IS whitespace (calls Must.Be.NotWhitespace)
    public static class Whitespace
    {
        public static TheoryData<GuardCase<string?>> ValidCases => F.IsWhitespace.InvalidScenarios.Where(s => s.Inputs is not null).ToArray().ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<string?>> InvalidCases => [.. F.IsWhitespace.ValidScenarios.ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value")), .. F.IsWhitespace.InvalidScenarios.Where(s => s.Inputs is null).ToArray().ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentNullException), "value"))];
    }

    // Guard.Against.NotContainsWhitespace — throws when value does NOT contain whitespace (calls Must.Be.ContainsWhitespace)
    public static class NotContainsWhitespace
    {
        public static TheoryData<GuardCase<string?>> ValidCases => F.ContainsWhitespace.ValidScenarios.ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<string?>> InvalidCases => F.ContainsWhitespace.InvalidScenarios.ToGuardCases("value");
    }

    // Guard.Against.ContainsWhitespace — throws when value DOES contain whitespace (calls Must.Be.NotContainsWhitespace)
    public static class ContainsWhitespace
    {
        private static readonly string? NoWhitespace = "abc";
        private static readonly string? WithSpace = "a b";
        private static readonly string? NullValue = null;
        private static readonly string? Empty = "";

        public static TheoryData<GuardCase<string?>> ValidCases =>
        [
            new(nameof(NoWhitespace), NoWhitespace, new GuardExpected(true)),
            new(nameof(Empty), Empty, new GuardExpected(true))
        ];

        public static TheoryData<GuardCase<string?>> InvalidCases =>
        [
            new(nameof(WithSpace), WithSpace, new GuardExpected(false, typeof(ArgumentException), "value")),
            new(nameof(NullValue), NullValue, new GuardExpected(false, typeof(ArgumentNullException), "value"))
        ];
    }

    // Guard.Against.NotContainsControlChars — throws when value does NOT contain control chars (calls Must.Be.ContainsControlChars)
    public static class NotContainsControlChars
    {
        public static TheoryData<GuardCase<string?>> ValidCases => F.ContainsControlChars.ValidScenarios.ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<string?>> InvalidCases => F.ContainsControlChars.InvalidScenarios.ToGuardCases("value");
    }

    // Guard.Against.ContainsControlChars — throws when value DOES contain control chars (calls Must.Be.NotContainsControlChars)
    public static class ContainsControlChars
    {
        public static TheoryData<GuardCase<string?>> ValidCases => F.NotContainsControlChars.ValidScenarios.ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<string?>> InvalidCases => F.NotContainsControlChars.InvalidScenarios.ToGuardCases("value");
    }

    // Guard.Against.NotContainsAllowedOnly — throws when value does NOT contain allowed chars only (calls Must.Be.ContainsAllowedOnly)
    public static class NotContainsAllowedOnly
    {
        public static TheoryData<GuardCase<(string? value, char[] allowedChars)>> ValidCases => F.ContainsAllowedOnly.ValidScenarios.ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<(string? value, char[] allowedChars)>> InvalidCases => F.ContainsAllowedOnly.InvalidScenarios.ToGuardCases(s => s.Inputs.value is null ? new GuardExpected(false, typeof(ArgumentNullException), "value") : new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    // Guard.Against.ContainsAllowedOnly — throws when value DOES contain allowed chars only (calls Must.Be.NotContainsAllowedOnly)
    public static class ContainsAllowedOnly
    {
        private static readonly char[] Allowed = ['a', 'b'];
        private static readonly (string? value, char[] allowedChars) AllAllowed = ("aba", Allowed);
        private static readonly (string? value, char[] allowedChars) HasDisallowed = ("abc", Allowed);
        private static readonly (string? value, char[] allowedChars) NullValue = (null, Allowed);

        public static TheoryData<GuardCase<(string? value, char[] allowedChars)>> ValidCases =>
        [
            new(nameof(HasDisallowed), HasDisallowed, new GuardExpected(true))
        ];

        public static TheoryData<GuardCase<(string? value, char[] allowedChars)>> InvalidCases =>
        [
            new(nameof(AllAllowed), AllAllowed, new GuardExpected(false, typeof(ArgumentException), "value")),
            new(nameof(NullValue), NullValue, new GuardExpected(false, typeof(ArgumentNullException), "value"))
        ];
    }

    // Guard.Against.ContainsDisallowed — throws when value DOES contain disallowed chars (calls Must.Be.NotContainsDisallowed)
    public static class ContainsDisallowed
    {
        public static TheoryData<GuardCase<(string? value, char[] disallowedChars)>> ValidCases => F.ContainsDisallowed.InvalidScenarios.Where(s => s.Inputs.value is not null).ToArray().ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<(string? value, char[] disallowedChars)>> InvalidCases => [.. F.ContainsDisallowed.ValidScenarios.ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value")), .. F.ContainsDisallowed.InvalidScenarios.Where(s => s.Inputs.value is null).ToArray().ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentNullException), "value"))];
    }

    // Guard.Against.NotContainsAny — throws when value does NOT contain any of the given chars (calls Must.Be.ContainsAny)
    public static class NotContainsAny
    {
        private static readonly char[] Characters = ['x', 'y', 'z'];
        private static readonly (string? value, char[] characters) ContainsOne = ("axb", Characters);
        private static readonly (string? value, char[] characters) ContainsNone = ("abc", Characters);
        private static readonly (string? value, char[] characters) NullValue = (null, Characters);

        public static TheoryData<GuardCase<(string? value, char[] characters)>> ValidCases =>
        [
            new(nameof(ContainsOne), ContainsOne, new GuardExpected(true))
        ];

        public static TheoryData<GuardCase<(string? value, char[] characters)>> InvalidCases =>
        [
            new(nameof(ContainsNone), ContainsNone, new GuardExpected(false, typeof(ArgumentException), "value")),
            new(nameof(NullValue), NullValue, new GuardExpected(false, typeof(ArgumentNullException), "value"))
        ];
    }

    // Guard.Against.NotContainsDisallowed — throws when value does NOT contain any disallowed chars (calls Must.Be.ContainsAny)
    public static class NotContainsDisallowed
    {
        private static readonly char[] Disallowed = ['x', 'y', 'z'];
        private static readonly (string? value, char[] disallowedChars) ContainsDisallowed1 = ("axb", Disallowed);
        private static readonly (string? value, char[] disallowedChars) NoDisallowed = ("abc", Disallowed);
        private static readonly (string? value, char[] disallowedChars) NullValue = (null, Disallowed);

        public static TheoryData<GuardCase<(string? value, char[] disallowedChars)>> ValidCases =>
        [
            new(nameof(ContainsDisallowed1), ContainsDisallowed1, new GuardExpected(true))
        ];

        public static TheoryData<GuardCase<(string? value, char[] disallowedChars)>> InvalidCases =>
        [
            new(nameof(NoDisallowed), NoDisallowed, new GuardExpected(false, typeof(ArgumentException), "value")),
            new(nameof(NullValue), NullValue, new GuardExpected(false, typeof(ArgumentNullException), "value"))
        ];
    }

    // Guard.Against.NotContains — throws when value does NOT contain the substring (calls Must.Be.Contains)
    public static class NotContains
    {
        public static TheoryData<GuardCase<(string? value, string substring, StringComparison comparison)>> ValidCases => F.Contains.ValidScenarios.ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<(string? value, string substring, StringComparison comparison)>> InvalidCases => F.Contains.InvalidScenarios.ToGuardCases(s => s.Inputs.value is null ? new GuardExpected(false, typeof(ArgumentNullException), "value", Code: MustCodes.Text.Content.NotContains) : new GuardExpected(false, typeof(ArgumentException), "value", Code: MustCodes.Text.Content.NotContains));
    }

    // Guard.Against.Contains — throws when value DOES contain the substring (calls Must.Be.NotContains)
    public static class Contains
    {
        public static TheoryData<GuardCase<(string? value, string substring, StringComparison comparison)>> ValidCases => F.Contains.InvalidScenarios.Except(nameof(F.Contains.NullValue)).ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<(string? value, string substring, StringComparison comparison)>> InvalidCases => [.. F.Contains.ValidScenarios.ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value")), .. F.Contains.InvalidScenarios.Only(nameof(F.Contains.NullValue)).ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentNullException), "value"))];
    }

    // Guard.Against.NotStartsWith — throws when value does NOT start with the prefix (calls Must.Be.StartsWith)
    public static class NotStartsWith
    {
        public static TheoryData<GuardCase<(string? value, string prefix, StringComparison comparison)>> ValidCases => F.StartsWith.ValidScenarios.ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<(string? value, string prefix, StringComparison comparison)>> InvalidCases => F.StartsWith.InvalidScenarios.ToGuardCases(s => s.Inputs.value is null ? new GuardExpected(false, typeof(ArgumentNullException), "value") : new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    // Guard.Against.StartsWith — throws when value DOES start with the prefix (calls Must.Be.NotStartsWith)
    public static class StartsWith
    {
        public static TheoryData<GuardCase<(string? value, string prefix, StringComparison comparison)>> ValidCases => F.StartsWith.InvalidScenarios.Except(nameof(F.StartsWith.NullValue)).ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<(string? value, string prefix, StringComparison comparison)>> InvalidCases => [.. F.StartsWith.ValidScenarios.ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value")), .. F.StartsWith.InvalidScenarios.Only(nameof(F.StartsWith.NullValue)).ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentNullException), "value"))];
    }

    // Guard.Against.NotEndsWith — throws when value does NOT end with the suffix (calls Must.Be.EndsWith)
    public static class NotEndsWith
    {
        public static TheoryData<GuardCase<(string? value, string suffix, StringComparison comparison)>> ValidCases => F.EndsWith.ValidScenarios.ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<(string? value, string suffix, StringComparison comparison)>> InvalidCases => F.EndsWith.InvalidScenarios.ToGuardCases(s => s.Inputs.value is null ? new GuardExpected(false, typeof(ArgumentNullException), "value") : new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    // Guard.Against.EndsWith — throws when value DOES end with the suffix (calls Must.Be.NotEndsWith)
    public static class EndsWith
    {
        public static TheoryData<GuardCase<(string? value, string suffix, StringComparison comparison)>> ValidCases => F.EndsWith.InvalidScenarios.Except(nameof(F.EndsWith.NullValue)).ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<(string? value, string suffix, StringComparison comparison)>> InvalidCases => [.. F.EndsWith.ValidScenarios.ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value")), .. F.EndsWith.InvalidScenarios.Only(nameof(F.EndsWith.NullValue)).ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentNullException), "value"))];
    }
}
