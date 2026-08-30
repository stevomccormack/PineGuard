using System.Text.RegularExpressions;
using PineGuard.Common;
using PineGuard.Testing.UnitTests.Rules;

#pragma warning disable CS8795 // Partial method must have an implementation part (source generator provides it)

namespace PineGuard.Testing.Fixtures;

public static partial class StringRulesFixtures
{
    // ── Main StringRules ────────────────────────────────────────────

    public static class IsNotNullOrEmpty
    {
        public static readonly string? NotEmpty = "hello";
        public static readonly string? Empty = "";
        public static readonly string? NullValue = null;

        public static RuleScenario<string?>[] ValidScenarios => [new(nameof(NotEmpty), NotEmpty, true)];
        public static RuleScenario<string?>[] InvalidScenarios => [new(nameof(Empty), Empty, false), new(nameof(NullValue), NullValue, false)];
        public static RuleScenario<string?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsNullOrEmpty
    {
        public static readonly string? Empty = "";
        public static readonly string? NullValue = null;
        public static readonly string? NotEmpty = "hello";

        public static RuleScenario<string?>[] ValidScenarios => [new(nameof(Empty), Empty, true), new(nameof(NullValue), NullValue, true)];
        public static RuleScenario<string?>[] InvalidScenarios => [new(nameof(NotEmpty), NotEmpty, false)];
        public static RuleScenario<string?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsNotNullOrWhiteSpace
    {
        public static readonly string? Content = "hello";
        public static readonly string? Whitespace = "   ";
        public static readonly string? Empty = "";
        public static readonly string? NullValue = null;

        public static RuleScenario<string?>[] ValidScenarios => [new(nameof(Content), Content, true)];
        public static RuleScenario<string?>[] InvalidScenarios => [new(nameof(Whitespace), Whitespace, false), new(nameof(Empty), Empty, false), new(nameof(NullValue), NullValue, false)];
        public static RuleScenario<string?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsNullOrWhiteSpace
    {
        public static readonly string? Whitespace = "   ";
        public static readonly string? Empty = "";
        public static readonly string? NullValue = null;
        public static readonly string? Content = "hello";

        public static RuleScenario<string?>[] ValidScenarios => [new(nameof(Whitespace), Whitespace, true), new(nameof(Empty), Empty, true), new(nameof(NullValue), NullValue, true)];
        public static RuleScenario<string?>[] InvalidScenarios => [new(nameof(Content), Content, false)];
        public static RuleScenario<string?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsExactLength
    {
        public static readonly (string? value, int length) Matching = ("abc", 3);
        public static readonly (string? value, int length) Shorter = ("abc", 2);
        public static readonly (string? value, int length) Longer = ("abc", 4);
        public static readonly (string? value, int length) NullValue = (null, 1);

        public static RuleScenario<(string? value, int length)>[] ValidScenarios => [new(nameof(Matching), Matching, true)];
        public static RuleScenario<(string? value, int length)>[] ValidEdgeScenarios => [];
        public static RuleScenario<(string? value, int length)>[] InvalidScenarios => [new(nameof(Shorter), Shorter, false), new(nameof(Longer), Longer, false)];
        public static RuleScenario<(string? value, int length)>[] InvalidEdgeScenarios => [new(nameof(NullValue), NullValue, false)];
        public static RuleScenario<(string? value, int length)>[] AllValid => [.. ValidScenarios, .. ValidEdgeScenarios];
        public static RuleScenario<(string? value, int length)>[] AllInvalid => [.. InvalidScenarios, .. InvalidEdgeScenarios];
        public static RuleScenario<(string? value, int length)>[] AllScenarios => [.. AllValid, .. AllInvalid];
    }

    public static class IsLengthBetween
    {
        public static readonly (string? value, int min, int max) WithinBounds = ("abc", 2, 3);
        public static readonly (string? value, int min, int max) OutsideBounds = ("abc", 4, 5);
        public static readonly (string? value, int min, int max) NullValue = (null, 1, 2);

        public static RuleScenario<(string? value, int min, int max)>[] ValidScenarios => [new(nameof(WithinBounds), WithinBounds, true)];
        public static RuleScenario<(string? value, int min, int max)>[] ValidEdgeScenarios => [];
        public static RuleScenario<(string? value, int min, int max)>[] InvalidScenarios => [new(nameof(OutsideBounds), OutsideBounds, false)];
        public static RuleScenario<(string? value, int min, int max)>[] InvalidEdgeScenarios => [new(nameof(NullValue), NullValue, false)];
        public static RuleScenario<(string? value, int min, int max)>[] AllValid => [.. ValidScenarios, .. ValidEdgeScenarios];
        public static RuleScenario<(string? value, int min, int max)>[] AllInvalid => [.. InvalidScenarios, .. InvalidEdgeScenarios];
        public static RuleScenario<(string? value, int min, int max)>[] AllScenarios => [.. AllValid, .. AllInvalid];
    }

    public static class IsLongerThan
    {
        public static readonly (string? value, int length, Inclusion inclusion) LongerExclusive = ("abc", 2, Inclusion.Exclusive);
        public static readonly (string? value, int length, Inclusion inclusion) SameLengthExclusive = ("abc", 3, Inclusion.Exclusive);
        public static readonly (string? value, int length, Inclusion inclusion) SameLengthInclusive = ("abc", 3, Inclusion.Inclusive);
        public static readonly (string? value, int length, Inclusion inclusion) NullValue = (null, 0, Inclusion.Inclusive);

        public static RuleScenario<(string? value, int length, Inclusion inclusion)>[] ValidScenarios => [new(nameof(LongerExclusive), LongerExclusive, true)];
        public static RuleScenario<(string? value, int length, Inclusion inclusion)>[] ValidEdgeScenarios => [new(nameof(SameLengthInclusive), SameLengthInclusive, true)];
        public static RuleScenario<(string? value, int length, Inclusion inclusion)>[] InvalidScenarios => [new(nameof(SameLengthExclusive), SameLengthExclusive, false)];
        public static RuleScenario<(string? value, int length, Inclusion inclusion)>[] InvalidEdgeScenarios => [new(nameof(NullValue), NullValue, false)];
        public static RuleScenario<(string? value, int length, Inclusion inclusion)>[] AllValid => [.. ValidScenarios, .. ValidEdgeScenarios];
        public static RuleScenario<(string? value, int length, Inclusion inclusion)>[] AllInvalid => [.. InvalidScenarios, .. InvalidEdgeScenarios];
        public static RuleScenario<(string? value, int length, Inclusion inclusion)>[] AllScenarios => [.. AllValid, .. AllInvalid];
    }

    public static class IsLongerThanDefaultInclusion
    {
        public static readonly (string? value, int length) Longer = ("abc", 2);
        public static readonly (string? value, int length) SameLength = ("abc", 3);

        public static RuleScenario<(string? value, int length)>[] ValidScenarios => [new(nameof(Longer), Longer, true)];
        public static RuleScenario<(string? value, int length)>[] InvalidScenarios => [new(nameof(SameLength), SameLength, false)];
        public static RuleScenario<(string? value, int length)>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsShorterThan
    {
        public static readonly (string? value, int length, Inclusion inclusion) ShorterExclusive = ("abc", 4, Inclusion.Exclusive);
        public static readonly (string? value, int length, Inclusion inclusion) SameLengthExclusive = ("abc", 3, Inclusion.Exclusive);
        public static readonly (string? value, int length, Inclusion inclusion) SameLengthInclusive = ("abc", 3, Inclusion.Inclusive);
        public static readonly (string? value, int length, Inclusion inclusion) NullValue = (null, 10, Inclusion.Inclusive);

        public static RuleScenario<(string? value, int length, Inclusion inclusion)>[] ValidScenarios => [new(nameof(ShorterExclusive), ShorterExclusive, true)];
        public static RuleScenario<(string? value, int length, Inclusion inclusion)>[] ValidEdgeScenarios => [new(nameof(SameLengthInclusive), SameLengthInclusive, true)];
        public static RuleScenario<(string? value, int length, Inclusion inclusion)>[] InvalidScenarios => [new(nameof(SameLengthExclusive), SameLengthExclusive, false)];
        public static RuleScenario<(string? value, int length, Inclusion inclusion)>[] InvalidEdgeScenarios => [new(nameof(NullValue), NullValue, false)];
        public static RuleScenario<(string? value, int length, Inclusion inclusion)>[] AllValid => [.. ValidScenarios, .. ValidEdgeScenarios];
        public static RuleScenario<(string? value, int length, Inclusion inclusion)>[] AllInvalid => [.. InvalidScenarios, .. InvalidEdgeScenarios];
        public static RuleScenario<(string? value, int length, Inclusion inclusion)>[] AllScenarios => [.. AllValid, .. AllInvalid];
    }

    public static class IsShorterThanDefaultInclusion
    {
        public static readonly (string? value, int length) Shorter = ("abc", 4);
        public static readonly (string? value, int length) SameLength = ("abc", 3);

        public static RuleScenario<(string? value, int length)>[] ValidScenarios => [new(nameof(Shorter), Shorter, true)];
        public static RuleScenario<(string? value, int length)>[] InvalidScenarios => [new(nameof(SameLength), SameLength, false)];
        public static RuleScenario<(string? value, int length)>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsDigitsOnly
    {
        public static readonly string? Digits = "123";
        public static readonly string? Trimmed = " 123 ";
        public static readonly string? WithDash = "12-3";
        public static readonly string? NullValue = null;

        public static RuleScenario<string?>[] ValidScenarios => [new(nameof(Digits), Digits, true), new(nameof(Trimmed), Trimmed, true)];
        public static RuleScenario<string?>[] InvalidScenarios => [new(nameof(WithDash), WithDash, false), new(nameof(NullValue), NullValue, false)];
        public static RuleScenario<string?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsDigitsOnlyWithNullAllowedNonDigitChars
    {
        public static readonly string? Digits = "123";
        public static readonly string? WithDashAndSpace = "12-34 56";

        public static RuleScenario<string?>[] ValidScenarios => [new(nameof(Digits), Digits, true)];
        public static RuleScenario<string?>[] InvalidScenarios => [new(nameof(WithDashAndSpace), WithDashAndSpace, false)];
        public static RuleScenario<string?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsUppercase
    {
        public static readonly (string? value, bool lettersOnly) AllCapsWithNumbers = ("ABC123", false);
        public static readonly (string? value, bool lettersOnly) NumbersOnly = ("123", false);
        public static readonly (string? value, bool lettersOnly) MixedCase = ("AbC", false);
        public static readonly (string? value, bool lettersOnly) AllCapsLettersOnly = ("ABC", true);
        public static readonly (string? value, bool lettersOnly) AllCapsWithNumbersLettersOnly = ("ABC123", true);
        public static readonly (string? value, bool lettersOnly) NullValue = (null, false);
        public static readonly (string? value, bool lettersOnly) Empty = ("", false);
        public static readonly (string? value, bool lettersOnly) EmptyLettersOnly = ("", true);

        public static RuleScenario<(string? value, bool lettersOnly)>[] ValidScenarios => [new(nameof(AllCapsWithNumbers), AllCapsWithNumbers, true), new(nameof(AllCapsLettersOnly), AllCapsLettersOnly, true)];
        public static RuleScenario<(string? value, bool lettersOnly)>[] InvalidScenarios => [new(nameof(NumbersOnly), NumbersOnly, false), new(nameof(MixedCase), MixedCase, false), new(nameof(AllCapsWithNumbersLettersOnly), AllCapsWithNumbersLettersOnly, false), new(nameof(NullValue), NullValue, false), new(nameof(Empty), Empty, false), new(nameof(EmptyLettersOnly), EmptyLettersOnly, false)];
        public static RuleScenario<(string? value, bool lettersOnly)>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsLowercase
    {
        public static readonly (string? value, bool lettersOnly) AllLowerWithNumbers = ("abc123", false);
        public static readonly (string? value, bool lettersOnly) NumbersOnly = ("123", false);
        public static readonly (string? value, bool lettersOnly) MixedCase = ("aBc", false);
        public static readonly (string? value, bool lettersOnly) AllLowerLettersOnly = ("abc", true);
        public static readonly (string? value, bool lettersOnly) AllLowerWithNumbersLettersOnly = ("abc123", true);
        public static readonly (string? value, bool lettersOnly) NullValue = (null, false);
        public static readonly (string? value, bool lettersOnly) Empty = ("", false);
        public static readonly (string? value, bool lettersOnly) EmptyLettersOnly = ("", true);

        public static RuleScenario<(string? value, bool lettersOnly)>[] ValidScenarios => [new(nameof(AllLowerWithNumbers), AllLowerWithNumbers, true), new(nameof(AllLowerLettersOnly), AllLowerLettersOnly, true)];
        public static RuleScenario<(string? value, bool lettersOnly)>[] InvalidScenarios => [new(nameof(NumbersOnly), NumbersOnly, false), new(nameof(MixedCase), MixedCase, false), new(nameof(AllLowerWithNumbersLettersOnly), AllLowerWithNumbersLettersOnly, false), new(nameof(NullValue), NullValue, false), new(nameof(Empty), Empty, false), new(nameof(EmptyLettersOnly), EmptyLettersOnly, false)];
        public static RuleScenario<(string? value, bool lettersOnly)>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsAscii
    {
        public static readonly string? Ascii = " abc ";
        public static readonly string? NonAscii = "caf\u00e9";
        public static readonly string? Null = null;

        public static RuleScenario<string?>[] ValidScenarios => [new(nameof(Ascii), Ascii, true)];
        public static RuleScenario<string?>[] InvalidScenarios => [new(nameof(NonAscii), NonAscii, false), new(nameof(Null), Null, false)];
        public static RuleScenario<string?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsPrintableAscii
    {
        public static readonly (string? value, bool allowCommonWhitespace) Printable = ("Hello", false);
        public static readonly (string? value, bool allowCommonWhitespace) ContainsTab = ("A\tB", false);
        public static readonly (string? value, bool allowCommonWhitespace) TabAllowed = ("A\tB", true);
        public static readonly (string? value, bool allowCommonWhitespace) ContainsCr = ("A\rB", false);
        public static readonly (string? value, bool allowCommonWhitespace) CrAllowed = ("A\rB", true);
        public static readonly (string? value, bool allowCommonWhitespace) ContainsLf = ("A\nB", false);
        public static readonly (string? value, bool allowCommonWhitespace) LfAllowed = ("A\nB", true);
        public static readonly (string? value, bool allowCommonWhitespace) ContainsVtAllowed = ("A\vB", true);
        public static readonly (string? value, bool allowCommonWhitespace) NullValue = (null, false);

        public static RuleScenario<(string? value, bool allowCommonWhitespace)>[] ValidScenarios => [new(nameof(Printable), Printable, true), new(nameof(TabAllowed), TabAllowed, true), new(nameof(CrAllowed), CrAllowed, true), new(nameof(LfAllowed), LfAllowed, true)];
        public static RuleScenario<(string? value, bool allowCommonWhitespace)>[] InvalidScenarios => [new(nameof(ContainsTab), ContainsTab, false), new(nameof(ContainsCr), ContainsCr, false), new(nameof(ContainsLf), ContainsLf, false), new(nameof(ContainsVtAllowed), ContainsVtAllowed, false), new(nameof(NullValue), NullValue, false)];
        public static RuleScenario<(string? value, bool allowCommonWhitespace)>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsWhitespace
    {
        public static readonly string? NullValue = null;
        public static readonly string? Empty = "";
        public static readonly string? SpacesAndTabs = " \t\r\n";
        public static readonly string? WithLetters = " a ";

        public static RuleScenario<string?>[] ValidScenarios => [new(nameof(Empty), Empty, true), new(nameof(SpacesAndTabs), SpacesAndTabs, true)];
        public static RuleScenario<string?>[] InvalidScenarios => [new(nameof(NullValue), NullValue, false), new(nameof(WithLetters), WithLetters, false)];
        public static RuleScenario<string?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class ContainsWhitespace
    {
        public static readonly string? NullValue = null;
        public static readonly string? Empty = "";
        public static readonly string? OnlySpaces = "   ";
        public static readonly string? Surrounding = " a ";
        public static readonly string? Between = "a b";
        public static readonly string? NoWhitespace = "abc";

        public static RuleScenario<string?>[] ValidScenarios => [new(nameof(OnlySpaces), OnlySpaces, true), new(nameof(Surrounding), Surrounding, true), new(nameof(Between), Between, true)];
        public static RuleScenario<string?>[] InvalidScenarios => [new(nameof(NullValue), NullValue, false), new(nameof(Empty), Empty, false), new(nameof(NoWhitespace), NoWhitespace, false)];
        public static RuleScenario<string?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class ContainsControlChars
    {
        public static readonly string? NullValue = null;
        public static readonly string? Empty = "";
        public static readonly string? NoControl = "abc";
        public static readonly string? WithControl = "a\u0001b";

        public static RuleScenario<string?>[] ValidScenarios => [new(nameof(WithControl), WithControl, true)];
        public static RuleScenario<string?>[] InvalidScenarios => [new(nameof(NullValue), NullValue, false), new(nameof(Empty), Empty, false), new(nameof(NoControl), NoControl, false)];
        public static RuleScenario<string?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class NotContainsControlChars
    {
        public static readonly string? NullValue = null;
        public static readonly string? Empty = "";
        public static readonly string? NoControl = "abc";
        public static readonly string? WithControl = "a\nb";

        public static RuleScenario<string?>[] ValidScenarios => [new(nameof(Empty), Empty, true), new(nameof(NoControl), NoControl, true)];
        public static RuleScenario<string?>[] InvalidScenarios => [new(nameof(NullValue), NullValue, false), new(nameof(WithControl), WithControl, false)];
        public static RuleScenario<string?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static partial class IsMatch
    {
        [GeneratedRegex("^abc$", RegexOptions.CultureInvariant)]
        private static partial Regex PatternRegex();

        private static readonly Regex Pattern = PatternRegex();

        public static readonly (string? value, Regex pattern) ExactMatch = ("abc", Pattern);
        public static readonly (string? value, Regex pattern) NoMatch = ("abcd", Pattern);
        public static readonly (string? value, Regex pattern) NullString = (null, Pattern);

        public static RuleScenario<(string? value, Regex pattern)>[] ValidScenarios => [new(nameof(ExactMatch), ExactMatch, true)];
        public static RuleScenario<(string? value, Regex pattern)>[] InvalidScenarios => [new(nameof(NoMatch), NoMatch, false), new(nameof(NullString), NullString, false)];
        public static RuleScenario<(string? value, Regex pattern)>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsDigitsOnlyWithAllowedNonDigitChars
    {
        public static readonly (string value, char[] allowedNonDigitChars) DashAllowed = ("12-3", ['-']);
        public static readonly (string value, char[] allowedNonDigitChars) DisallowedChar = ("12x3", ['-']);

        public static RuleScenario<(string value, char[] allowedNonDigitChars)>[] ValidScenarios => [new(nameof(DashAllowed), DashAllowed, true)];
        public static RuleScenario<(string value, char[] allowedNonDigitChars)>[] InvalidScenarios => [new(nameof(DisallowedChar), DisallowedChar, false)];
        public static RuleScenario<(string value, char[] allowedNonDigitChars)>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class RulesThatRequireTrim
    {
        public static readonly string? NullValue = null;
        public static readonly string? Empty = string.Empty;
        public static readonly string? Space = " ";
        public static readonly string? Whitespace = "\t\r\n";

        public static RuleScenario<string?>[] InvalidScenarios => [new(nameof(NullValue), NullValue, false), new(nameof(Empty), Empty, false), new(nameof(Space), Space, false), new(nameof(Whitespace), Whitespace, false)];
        public static RuleScenario<string?>[] AllScenarios => [.. InvalidScenarios];
    }

    public static class ContainsAllowedOnly
    {
        public static readonly (string? value, char[] allowedChars) AllowedOnly = ("aba", ['a', 'b']);
        public static readonly (string? value, char[] allowedChars) DisallowedIncluded = ("abc", ['a', 'b']);
        public static readonly (string? value, char[] allowedChars) NullValue = (null, ['a', 'b']);

        public static RuleScenario<(string? value, char[] allowedChars)>[] ValidScenarios => [new(nameof(AllowedOnly), AllowedOnly, true)];
        public static RuleScenario<(string? value, char[] allowedChars)>[] InvalidScenarios => [new(nameof(DisallowedIncluded), DisallowedIncluded, false), new(nameof(NullValue), NullValue, false)];
        public static RuleScenario<(string? value, char[] allowedChars)>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class ContainsDisallowed
    {
        public static readonly (string? value, char[] disallowedChars) NoDisallowed = ("c", ['a', 'b']);
        public static readonly (string? value, char[] disallowedChars) HasDisallowed = ("abc", ['a', 'b']);
        public static readonly (string? value, char[] disallowedChars) NullValue = (null, ['a', 'b']);

        public static RuleScenario<(string? value, char[] disallowedChars)>[] ValidScenarios => [new(nameof(HasDisallowed), HasDisallowed, true)];
        public static RuleScenario<(string? value, char[] disallowedChars)>[] InvalidScenarios => [new(nameof(NoDisallowed), NoDisallowed, false), new(nameof(NullValue), NullValue, false)];
        public static RuleScenario<(string? value, char[] disallowedChars)>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class Contains
    {
        public static readonly (string? value, string substring, StringComparison comparison) Present = ("hello world", "lo wo", StringComparison.Ordinal);
        public static readonly (string? value, string substring, StringComparison comparison) PresentIgnoringCase = ("Hello World", "LO WO", StringComparison.OrdinalIgnoreCase);
        public static readonly (string? value, string substring, StringComparison comparison) EmptySubstring = ("hello world", "", StringComparison.Ordinal);
        public static readonly (string? value, string substring, StringComparison comparison) Absent = ("hello world", "planet", StringComparison.Ordinal);
        public static readonly (string? value, string substring, StringComparison comparison) CaseMismatch = ("Hello World", "LO WO", StringComparison.Ordinal);
        public static readonly (string? value, string substring, StringComparison comparison) NullValue = (null, "lo wo", StringComparison.Ordinal);

        public static RuleScenario<(string? value, string substring, StringComparison comparison)>[] ValidScenarios => [new(nameof(Present), Present, true), new(nameof(PresentIgnoringCase), PresentIgnoringCase, true), new(nameof(EmptySubstring), EmptySubstring, true)];
        public static RuleScenario<(string? value, string substring, StringComparison comparison)>[] InvalidScenarios => [new(nameof(Absent), Absent, false), new(nameof(CaseMismatch), CaseMismatch, false), new(nameof(NullValue), NullValue, false)];
        public static RuleScenario<(string? value, string substring, StringComparison comparison)>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class StartsWith
    {
        public static readonly (string? value, string prefix, StringComparison comparison) Prefixed = ("hello world", "hello", StringComparison.Ordinal);
        public static readonly (string? value, string prefix, StringComparison comparison) PrefixedIgnoringCase = ("Hello World", "HELLO", StringComparison.OrdinalIgnoreCase);
        public static readonly (string? value, string prefix, StringComparison comparison) EmptyPrefix = ("hello world", "", StringComparison.Ordinal);
        public static readonly (string? value, string prefix, StringComparison comparison) Absent = ("hello world", "world", StringComparison.Ordinal);
        public static readonly (string? value, string prefix, StringComparison comparison) CaseMismatch = ("Hello World", "HELLO", StringComparison.Ordinal);
        public static readonly (string? value, string prefix, StringComparison comparison) NullValue = (null, "hello", StringComparison.Ordinal);

        public static RuleScenario<(string? value, string prefix, StringComparison comparison)>[] ValidScenarios => [new(nameof(Prefixed), Prefixed, true), new(nameof(PrefixedIgnoringCase), PrefixedIgnoringCase, true), new(nameof(EmptyPrefix), EmptyPrefix, true)];
        public static RuleScenario<(string? value, string prefix, StringComparison comparison)>[] InvalidScenarios => [new(nameof(Absent), Absent, false), new(nameof(CaseMismatch), CaseMismatch, false), new(nameof(NullValue), NullValue, false)];
        public static RuleScenario<(string? value, string prefix, StringComparison comparison)>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class EndsWith
    {
        public static readonly (string? value, string suffix, StringComparison comparison) Suffixed = ("hello world", "world", StringComparison.Ordinal);
        public static readonly (string? value, string suffix, StringComparison comparison) SuffixedIgnoringCase = ("Hello World", "WORLD", StringComparison.OrdinalIgnoreCase);
        public static readonly (string? value, string suffix, StringComparison comparison) EmptySuffix = ("hello world", "", StringComparison.Ordinal);
        public static readonly (string? value, string suffix, StringComparison comparison) Absent = ("hello world", "hello", StringComparison.Ordinal);
        public static readonly (string? value, string suffix, StringComparison comparison) CaseMismatch = ("Hello World", "WORLD", StringComparison.Ordinal);
        public static readonly (string? value, string suffix, StringComparison comparison) NullValue = (null, "world", StringComparison.Ordinal);

        public static RuleScenario<(string? value, string suffix, StringComparison comparison)>[] ValidScenarios => [new(nameof(Suffixed), Suffixed, true), new(nameof(SuffixedIgnoringCase), SuffixedIgnoringCase, true), new(nameof(EmptySuffix), EmptySuffix, true)];
        public static RuleScenario<(string? value, string suffix, StringComparison comparison)>[] InvalidScenarios => [new(nameof(Absent), Absent, false), new(nameof(CaseMismatch), CaseMismatch, false), new(nameof(NullValue), NullValue, false)];
        public static RuleScenario<(string? value, string suffix, StringComparison comparison)>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsRegexPattern
    {
        public static readonly string? Literal = "abc";
        public static readonly string? Anchored = "^abc$";
        public static readonly string? CharacterClass = "[a-z]+";
        public static readonly string? Quantified = @"^\d{3}-\d{4}$";
        public static readonly string? Alternation = "cat|dog";
        public static readonly string? NamedGroup = @"(?<year>\d{4})";
        public static readonly string? Space = " ";
        public static readonly string? NullValue = null;
        public static readonly string? Empty = "";
        public static readonly string? UnclosedCharacterClass = "[unclosed";
        public static readonly string? UnclosedGroup = "(unclosed";
        public static readonly string? UnbalancedCloseParen = "a)b";
        public static readonly string? DanglingQuantifier = "*";
        public static readonly string? ReversedQuantifierRange = "a{3,1}";
        public static readonly string? UnknownUnicodeCategory = @"\p{NotACategory}";

        public static RuleScenario<string?>[] ValidScenarios => [new(nameof(Literal), Literal, true), new(nameof(Anchored), Anchored, true), new(nameof(CharacterClass), CharacterClass, true), new(nameof(Quantified), Quantified, true), new(nameof(Alternation), Alternation, true), new(nameof(NamedGroup), NamedGroup, true), new(nameof(Space), Space, true)];
        public static RuleScenario<string?>[] InvalidScenarios => [new(nameof(NullValue), NullValue, false), new(nameof(Empty), Empty, false), new(nameof(UnclosedCharacterClass), UnclosedCharacterClass, false), new(nameof(UnclosedGroup), UnclosedGroup, false), new(nameof(UnbalancedCloseParen), UnbalancedCloseParen, false), new(nameof(DanglingQuantifier), DanglingQuantifier, false), new(nameof(ReversedQuantifierRange), ReversedQuantifierRange, false), new(nameof(UnknownUnicodeCategory), UnknownUnicodeCategory, false)];
        public static RuleScenario<string?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }
}
