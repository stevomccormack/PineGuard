namespace PineGuard.Core.UnitTests.Rules;

public static class DefaultEqualityRulesTestData
{
    public static class IsDefaultInt32
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case(Name: "default", Value: 0, Expected: true) },
            { new Case(Name: "positive", Value: 1, Expected: false) },
            { new Case(Name: "negative", Value: -1, Expected: false) },
        };

        #region Cases

        public sealed record Case(string Name, int Value, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }

    public static class IsDefaultNullableInt32
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case(Name: "null", Value: null, Expected: true) },
            { new Case(Name: "zero", Value: 0, Expected: false) },
            { new Case(Name: "one", Value: 1, Expected: false) },
        };

        #region Cases

        public sealed record Case(string Name, int? Value, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }

    public static class IsDefaultString
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case(Name: "null", Value: null, Expected: true) },
            { new Case(Name: "empty", Value: string.Empty, Expected: false) },
            { new Case(Name: "whitespace", Value: " ", Expected: false) },
            { new Case(Name: "text", Value: "abc", Expected: false) },
        };

        #region Cases

        public sealed record Case(string Name, string? Value, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }

    public static class IsNullOrDefaultInt32
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case(Name: "default", Value: 0, Expected: true) },
            { new Case(Name: "non-default", Value: 1, Expected: false) },
        };

        #region Cases

        public sealed record Case(string Name, int Value, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }

    public static class IsNullOrDefaultNullableInt32
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case(Name: "null", Value: null, Expected: true) },
            { new Case(Name: "zero", Value: 0, Expected: false) },
        };

        #region Cases

        public sealed record Case(string Name, int? Value, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }

    public static class IsNullOrDefaultString
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case(Name: "null", Value: null, Expected: true) },
            { new Case(Name: "empty", Value: string.Empty, Expected: false) },
            { new Case(Name: "text", Value: "abc", Expected: false) },
        };

        #region Cases

        public sealed record Case(string Name, string? Value, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }
}
