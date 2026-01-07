namespace PineGuard.Core.UnitTests.Rules;

public static class BoolRulesTestData
{
    public static class IsTrue
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case(Name: "true", Value: true, Expected: true) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case(Name: "null", Value: null, Expected: false) },
            { new Case(Name: "false", Value: false, Expected: false) },
        };

        #region Cases

        public sealed record Case(string Name, bool? Value, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }

    public static class IsFalse
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case(Name: "false", Value: false, Expected: true) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case(Name: "null", Value: null, Expected: false) },
            { new Case(Name: "true", Value: true, Expected: false) },
        };

        #region Cases

        public sealed record Case(string Name, bool? Value, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }

    public static class IsNullOrTrue
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case(Name: "null", Value: null, Expected: true) },
            { new Case(Name: "true", Value: true, Expected: true) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case(Name: "false", Value: false, Expected: false) },
        };

        #region Cases

        public sealed record Case(string Name, bool? Value, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }

    public static class IsNullOrFalse
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case(Name: "null", Value: null, Expected: true) },
            { new Case(Name: "false", Value: false, Expected: true) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case(Name: "true", Value: true, Expected: false) },
        };

        #region Cases

        public sealed record Case(string Name, bool? Value, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }
}
