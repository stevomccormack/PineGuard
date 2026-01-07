namespace PineGuard.Core.UnitTests.Rules;

public static class ObjectRulesTestData
{
    public static class IsOfType
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case("string", "abc", true) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case("null", null, false) },
            { new Case("int", 123, false) },
        };

        #region Cases

        public sealed record Case(string Name, object? Value, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }

    public static class IsAssignableToType
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case("string", "abc", true) },
            { new Case("empty", string.Empty, true) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case("null", null, false) },
            { new Case("int", 123, false) },
            { new Case("string", "abc", false) },
            { new Case("object", new object(), false) },
        };

        #region Cases

        public sealed record Case(string Name, object? Value, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }
}
