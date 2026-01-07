namespace PineGuard.Core.UnitTests.Rules.StringRules;

public static class StringRulesBoolTestData
{
    public static class IsTrue
    {
        public static TheoryData<Case> Cases => new()
        {
            new("'true'", "true", true),
            new("trimmed true", " True ", true),
            new("'false'", "false", false),
            new("non-bool", "notabool", false),
            new("null", null, false),
            new("whitespace", " ", false),
        };

        #region Cases

        public sealed record Case(string Name, string? Value, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion Cases
    }

    public static class IsFalse
    {
        public static TheoryData<Case> Cases => new()
        {
            new("'false'", "false", true),
            new("trimmed false", " False ", true),
            new("'true'", "true", false),
            new("non-bool", "notabool", false),
            new("null", null, false),
            new("whitespace", " ", false),
        };

        #region Cases

        public sealed record Case(string Name, string? Value, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion Cases
    }

    public static class IsNullOrTrue
    {
        public static TheoryData<Case> Cases => new()
        {
            new("null", null, true),
            new("'true'", "true", true),
            new("'false'", "false", false),
            new("non-bool", "notabool", false),
        };

        #region Cases

        public sealed record Case(string Name, string? Value, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion Cases
    }

    public static class IsNullOrFalse
    {
        public static TheoryData<Case> Cases => new()
        {
            new("null", null, true),
            new("'false'", "false", true),
            new("'true'", "true", false),
            new("non-bool", "notabool", false),
        };

        #region Cases

        public sealed record Case(string Name, string? Value, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion Cases
    }
}
