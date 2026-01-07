namespace PineGuard.Core.UnitTests.Rules.StringRules;

public static class StringRulesCoreTestData
{
    public static class IsAlphabetic
    {
        public static TheoryData<Case> Cases => new()
        {
            new("null", null, ExpectedDefault: false, ExpectedWithDashInclusions: false, ExpectedWithUnderscoreInclusions: false),
            new("letters only", "abc", ExpectedDefault: true, ExpectedWithDashInclusions: true, ExpectedWithUnderscoreInclusions: true),
            new("dash not alphabetic unless included", "abc-xyz", ExpectedDefault: false, ExpectedWithDashInclusions: true, ExpectedWithUnderscoreInclusions: false),
        };

        #region Cases

        public sealed record Case(
            string Name,
            string? Value,
            bool ExpectedDefault,
            bool ExpectedWithDashInclusions,
            bool ExpectedWithUnderscoreInclusions)
        {
            public override string ToString() => Name;
        }

        #endregion Cases
    }

    public static class IsNumeric
    {
        public static TheoryData<Case> Cases => new()
        {
            new("null", null, ExpectedDefault: false, ExpectedWithDashInclusions: false, ExpectedWithUnderscoreInclusions: false),
            new("digits only", "123", ExpectedDefault: true, ExpectedWithDashInclusions: true, ExpectedWithUnderscoreInclusions: true),
            new("dash not numeric unless included", "12-3", ExpectedDefault: false, ExpectedWithDashInclusions: true, ExpectedWithUnderscoreInclusions: false),
        };

        #region Cases

        public sealed record Case(
            string Name,
            string? Value,
            bool ExpectedDefault,
            bool ExpectedWithDashInclusions,
            bool ExpectedWithUnderscoreInclusions)
        {
            public override string ToString() => Name;
        }

        #endregion Cases
    }

    public static class IsAlphanumeric
    {
        public static TheoryData<Case> Cases => new()
        {
            new("null", null, ExpectedDefault: false, ExpectedWithDashInclusions: false, ExpectedWithUnderscoreInclusions: false),
            new("letters+digits", "abc123", ExpectedDefault: true, ExpectedWithDashInclusions: true, ExpectedWithUnderscoreInclusions: true),
            new("dash not alphanumeric unless included", "abc-123", ExpectedDefault: false, ExpectedWithDashInclusions: true, ExpectedWithUnderscoreInclusions: false),
        };

        #region Cases

        public sealed record Case(
            string Name,
            string? Value,
            bool ExpectedDefault,
            bool ExpectedWithDashInclusions,
            bool ExpectedWithUnderscoreInclusions)
        {
            public override string ToString() => Name;
        }

        #endregion Cases
    }

    public static class IsDigitsOnly
    {
        public static TheoryData<Case> Cases => new()
        {
            new("digits", "123", true),
            new("trimmed", " 123 ", true),
            new("contains dash", "12-3", false),
            new("null", null, false),
        };

        #region Cases

        public sealed record Case(string Name, string? Value, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion Cases
    }

    public static class IsDigitsOnlyWithAllowedNonDigitChars
    {
        public static TheoryData<Case> Cases => new()
        {
            new("dash allowed", "12-3", true),
            new("disallowed char", "12x3", false),
        };

        #region Cases

        public sealed record Case(string Name, string Value, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion Cases
    }

    public static class RulesThatRequireTrim
    {
        public static TheoryData<Case> Cases => new()
        {
            new("null", null),
            new("empty", string.Empty),
            new("space", " "),
            new("whitespace", "\t\r\n"),
        };

        #region Cases

        public sealed record Case(string Name, string? Value)
        {
            public override string ToString() => Name;
        }

        #endregion Cases
    }
}
