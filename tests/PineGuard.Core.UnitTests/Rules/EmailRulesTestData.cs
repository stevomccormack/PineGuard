using Xunit;

namespace PineGuard.Core.UnitTests.Rules;

public static class EmailRulesTestData
{
    public static class IsEmail
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case(Name: "user@example.com => true", Value: "user@example.com", Expected: true) },
            { new Case(Name: "display name form => true", Value: "User <user@example.com>", Expected: true) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case(Name: "not-an-email => false", Value: "not-an-email", Expected: false) },
            { new Case(Name: "null => false", Value: null, Expected: false) },
        };

        #region Cases

        public sealed record Case(string Name, string? Value, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }

    public static class IsStrictEmail
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case(Name: "user@example.com => true", Value: "user@example.com", Expected: true) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case(Name: "display name form => false", Value: "User <user@example.com>", Expected: false) },
            { new Case(Name: "space in local => false", Value: "user example@example.com", Expected: false) },
            { new Case(Name: "user@localhost => false", Value: "user@localhost", Expected: false) },
            { new Case(Name: "null => false", Value: null, Expected: false) },
        };

        #region Cases

        public sealed record Case(string Name, string? Value, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }

    public static class HasAlias
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case(Name: "user+alias@example.com => true", Value: "user+alias@example.com", Expected: true) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case(Name: "user@example.com => false", Value: "user@example.com", Expected: false) },
            { new Case(Name: "display name form => false", Value: "User <user+alias@example.com>", Expected: false) },
            { new Case(Name: "null => false", Value: null, Expected: false) },
        };

        #region Cases

        public sealed record Case(string Name, string? Value, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }
}
