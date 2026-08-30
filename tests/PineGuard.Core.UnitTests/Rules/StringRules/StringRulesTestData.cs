using System.Text.RegularExpressions;
using PineGuard.Common;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;
using PineGuard.Testing.UnitTests.Rules;
using F = PineGuard.Testing.Fixtures.StringRulesFixtures;

namespace PineGuard.Core.UnitTests.Rules.StringRules;

public static class StringRulesTestData
{
    public static class IsExactLength
    {
        public static TheoryData<RuleCase<(string? value, int length)>> Cases => F.IsExactLength.AllScenarios.ToRuleCases();
    }

    public static class IsLengthBetween
    {
        public static TheoryData<RuleCase<(string? value, int min, int max)>> Cases => F.IsLengthBetween.AllScenarios.ToRuleCases();
    }

    public static class IsLongerThan
    {
        public static TheoryData<RuleCase<(string? value, int length, Inclusion inclusion)>> Cases => F.IsLongerThan.AllScenarios.ToRuleCases();
    }

    public static class IsLongerThanDefaultInclusion
    {
        public static TheoryData<RuleCase<(string? value, int length)>> Cases => F.IsLongerThanDefaultInclusion.AllScenarios.ToRuleCases();
    }

    public static class IsShorterThan
    {
        public static TheoryData<RuleCase<(string? value, int length, Inclusion inclusion)>> Cases => F.IsShorterThan.AllScenarios.ToRuleCases();
    }

    public static class IsShorterThanDefaultInclusion
    {
        public static TheoryData<RuleCase<(string? value, int length)>> Cases => F.IsShorterThanDefaultInclusion.AllScenarios.ToRuleCases();
    }

    public static class IsDigitsOnly
    {
        public static TheoryData<RuleCase<string?>> Cases => F.IsDigitsOnly.AllScenarios.ToRuleCases();
    }

    public static class IsDigitsOnlyWithAllowedNonDigitChars
    {
        public static TheoryData<RuleCase<(string value, char[] allowedNonDigitChars)>> Cases => F.IsDigitsOnlyWithAllowedNonDigitChars.AllScenarios.ToRuleCases();
    }

    public static class IsDigitsOnlyWithNullAllowedNonDigitChars
    {
        public static TheoryData<RuleCase<string?>> Cases => F.IsDigitsOnlyWithNullAllowedNonDigitChars.AllScenarios.ToRuleCases();
    }

    public static class IsUppercase
    {
        public static TheoryData<RuleCase<(string? value, bool lettersOnly)>> Cases => F.IsUppercase.AllScenarios.ToRuleCases();
    }

    public static class IsLowercase
    {
        public static TheoryData<RuleCase<(string? value, bool lettersOnly)>> Cases => F.IsLowercase.AllScenarios.ToRuleCases();
    }

    public static class RulesThatRequireTrim
    {
        public static TheoryData<RuleCase<string?>> Cases => F.RulesThatRequireTrim.AllScenarios.ToRuleCases();
    }

    public static class IsAscii
    {
        public static TheoryData<RuleCase<string?>> Cases => F.IsAscii.AllScenarios.ToRuleCases();
    }

    public static class IsPrintableAscii
    {
        public static TheoryData<RuleCase<(string? value, bool allowCommonWhitespace)>> Cases => F.IsPrintableAscii.AllScenarios.ToRuleCases();
    }

    public static class IsWhitespace
    {
        public static TheoryData<RuleCase<string?>> Cases => F.IsWhitespace.AllScenarios.ToRuleCases();
    }

    public static class ContainsWhitespace
    {
        public static TheoryData<RuleCase<string?>> Cases => F.ContainsWhitespace.AllScenarios.ToRuleCases();
    }

    public static class ContainsControlChars
    {
        public static TheoryData<RuleCase<string?>> Cases => F.ContainsControlChars.AllScenarios.ToRuleCases();
    }

    public static class NotContainsControlChars
    {
        public static TheoryData<RuleCase<string?>> Cases => F.NotContainsControlChars.AllScenarios.ToRuleCases();
    }

    public static class IsMatch
    {
        public static TheoryData<RuleCase<(string? value, Regex pattern)>> Cases => F.IsMatch.AllScenarios.ToRuleCases();

        public static TheoryData<InvalidCase> InvalidCases =>
        [
            new("null pattern", ("abc", null!), new ExpectedException(typeof(ArgumentNullException), "pattern"))
        ];

        public sealed record InvalidCase(string Name, (string? Value, Regex Pattern) Input, ExpectedException ExpectedException)
            : ThrowsCase<(string? Value, Regex Pattern)>(Name, Input, ExpectedException);
    }

    public static class IsAlphabetic
    {
        public static TheoryData<Case> Cases =>
        [
            new("null", null, false, false, false),
            new("letters only", "abc", true, true, true),
            new("dash not alphabetic unless included", "abc-xyz", false, true, false),
            new("empty string", "", false, false, false)
        ];

        public static TheoryData<Case> EmptyInclusions =>
        [
            new("letters only with empty inclusions", "abc", true, true, true),
            new("dash not alphabetic with empty inclusions", "abc-", false, false, false)
        ];

        public sealed record Case(string Name, string? Value, bool Expected, bool ExpectedWithDashInclusions, bool ExpectedWithUnderscoreInclusions)
            : BaseCase(Name);
    }

    public static class IsNumeric
    {
        public static TheoryData<Case> Cases =>
        [
            new("null", null, false, false, false),
            new("digits only", "123", true, true, true),
            new("dash not numeric unless included", "12-3", false, true, false),
            new("empty string", "", false, false, false)
        ];

        public static TheoryData<Case> EmptyInclusions =>
        [
            new("digits only with empty inclusions", "123", true, true, true),
            new("dash not numeric with empty inclusions", "123-", false, false, false)
        ];

        public sealed record Case(string Name, string? Value, bool Expected, bool ExpectedWithDashInclusions, bool ExpectedWithUnderscoreInclusions)
            : BaseCase(Name);
    }

    public static class IsAlphanumeric
    {
        public static TheoryData<Case> Cases =>
        [
            new("null", null, false, false, false),
            new("letters+digits", "abc123", true, true, true),
            new("dash not alphanumeric unless included", "abc-123", false, true, false),
            new("empty string", "", false, false, false)
        ];

        public static TheoryData<Case> EmptyInclusions =>
        [
            new("letters+digits with empty inclusions", "abc123", true, true, true),
            new("dash not alphanumeric with empty inclusions", "abc123-", false, false, false)
        ];

        public sealed record Case(string Name, string? Value, bool Expected, bool ExpectedWithDashInclusions, bool ExpectedWithUnderscoreInclusions)
            : BaseCase(Name);
    }

    public static class ContainsAllowedOnly
    {
        public static TheoryData<RuleCase<(string? value, char[] allowedChars)>> Cases => F.ContainsAllowedOnly.AllScenarios.ToRuleCases();

        public static TheoryData<InvalidCase> InvalidCases =>
        [
            new("null allowed chars", ("abc", null!), new ExpectedException(typeof(ArgumentNullException), "allowedChars"))
        ];

        public sealed record InvalidCase(string Name, (string? Value, char[] AllowedChars) Input, ExpectedException ExpectedException)
            : ThrowsCase<(string? Value, char[] AllowedChars)>(Name, Input, ExpectedException);
    }

    public static class ContainsDisallowed
    {
        public static TheoryData<RuleCase<(string? value, char[] disallowedChars)>> Cases => F.ContainsDisallowed.AllScenarios.ToRuleCases();

        public static TheoryData<InvalidCase> InvalidCases =>
        [
            new("null disallowed chars", ("abc", null!), new ExpectedException(typeof(ArgumentNullException), "disallowedChars"))
        ];

        public sealed record InvalidCase(string Name, (string? Value, char[] DisallowedChars) Input, ExpectedException ExpectedException)
            : ThrowsCase<(string? Value, char[] DisallowedChars)>(Name, Input, ExpectedException);
    }

    public static class Contains
    {
        public static TheoryData<RuleCase<(string? value, string substring, StringComparison comparison)>> Cases => F.Contains.AllScenarios.ToRuleCases();

        public static TheoryData<InvalidCase> InvalidCases =>
        [
            new("null substring", ("abc", null!), new ExpectedException(typeof(ArgumentNullException), "substring"))
        ];

        public sealed record InvalidCase(string Name, (string? Value, string Substring) Input, ExpectedException ExpectedException)
            : ThrowsCase<(string? Value, string Substring)>(Name, Input, ExpectedException);
    }

    public static class StartsWith
    {
        public static TheoryData<RuleCase<(string? value, string prefix, StringComparison comparison)>> Cases => F.StartsWith.AllScenarios.ToRuleCases();

        public static TheoryData<InvalidCase> InvalidCases =>
        [
            new("null prefix", ("abc", null!), new ExpectedException(typeof(ArgumentNullException), "prefix"))
        ];

        public sealed record InvalidCase(string Name, (string? Value, string Prefix) Input, ExpectedException ExpectedException)
            : ThrowsCase<(string? Value, string Prefix)>(Name, Input, ExpectedException);
    }

    public static class EndsWith
    {
        public static TheoryData<RuleCase<(string? value, string suffix, StringComparison comparison)>> Cases => F.EndsWith.AllScenarios.ToRuleCases();

        public static TheoryData<InvalidCase> InvalidCases =>
        [
            new("null suffix", ("abc", null!), new ExpectedException(typeof(ArgumentNullException), "suffix"))
        ];

        public sealed record InvalidCase(string Name, (string? Value, string Suffix) Input, ExpectedException ExpectedException)
            : ThrowsCase<(string? Value, string Suffix)>(Name, Input, ExpectedException);
    }

    public static class HasByteOrderMark
    {
        public static TheoryData<RuleCase<string?>> Cases => F.HasByteOrderMark.AllScenarios.ToRuleCases();
    }
}
