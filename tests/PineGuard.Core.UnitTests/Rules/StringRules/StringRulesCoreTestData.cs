using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Rules.StringRules;

public static class StringRulesCoreTestData
{
    public static class IsAlphabetic
    {
        public static TheoryData<Case> Cases =>
        [
            new("null", null, false, false, false),
            new("letters only", "abc", true, true, true),
            new("dash not alphabetic unless included", "abc-xyz", false, true, false),
        ];

        public sealed record Case(
            string Name,
            string? Value,
            bool ExpectedReturn,
            bool ExpectedWithDashInclusions,
            bool ExpectedWithUnderscoreInclusions)
            : IsCase<string?>(Name, Value, ExpectedReturn);
    }

    public static class IsNumeric
    {
        public static TheoryData<Case> Cases =>
        [
            new("null", null, false, false, false),
            new("digits only", "123", true, true, true),
            new("dash not numeric unless included", "12-3", false, true, false),
        ];

        public sealed record Case(
            string Name,
            string? Value,
            bool ExpectedReturn,
            bool ExpectedWithDashInclusions,
            bool ExpectedWithUnderscoreInclusions)
            : IsCase<string?>(Name, Value, ExpectedReturn);
    }

    public static class IsAlphanumeric
    {
        public static TheoryData<Case> Cases =>
        [
            new("null", null, false, false, false),
            new("letters+digits", "abc123", true, true, true),
            new("dash not alphanumeric unless included", "abc-123", false, true, false),
        ];

        public sealed record Case(
            string Name,
            string? Value,
            bool ExpectedReturn,
            bool ExpectedWithDashInclusions,
            bool ExpectedWithUnderscoreInclusions)
            : IsCase<string?>(Name, Value, ExpectedReturn);
    }

    public static class IsDigitsOnly
    {
        public static TheoryData<Case> Cases =>
        [
            new("digits", "123", true),
            new("trimmed", " 123 ", true),
            new("contains dash", "12-3", false),
            new("null", null, false),
        ];

        public sealed record Case(string Name, string? Value, bool ExpectedReturn)
            : IsCase<string?>(Name, Value, ExpectedReturn);
    }

    public static class IsDigitsOnlyWithAllowedNonDigitChars
    {
        public static TheoryData<Case> Cases =>
        [
            new("dash allowed", "12-3", true),
            new("disallowed char", "12x3", false),
        ];

        public sealed record Case(string Name, string Value, bool ExpectedReturn)
            : IsCase<string>(Name, Value, ExpectedReturn);
    }

    public static class RulesThatRequireTrim
    {
        public static TheoryData<Case> Cases =>
        [
            new("null", null, false),
            new("empty", string.Empty, false),
            new("space", " ", false),
            new("whitespace", "\t\r\n", false),
        ];

        public sealed record Case(string Name, string? Value, bool ExpectedReturn)
            : IsCase<string?>(Name, Value, ExpectedReturn);
    }
}
