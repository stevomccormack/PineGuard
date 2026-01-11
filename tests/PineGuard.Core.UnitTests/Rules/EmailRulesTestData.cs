using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Rules;

public static class EmailRulesTestData
{
    public static class IsEmail
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("user@example.com => true", "user@example.com", true),
            new("display name form => true", "User <user@example.com>", true),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("not-an-email => false", "not-an-email", false),
            new("null => false", null, false),
        ];

        #region Case Records

        public sealed record Case(string Name, string? Value, bool ExpectedReturn)
            : IsCase<string?>(Name, Value, ExpectedReturn);

        #endregion
    }

    public static class IsStrictEmail
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("user@example.com => true", "user@example.com", true),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("display name form => false", "User <user@example.com>", false),
            new("space in local => false", "user example@example.com", false),
            new("user@localhost => false", "user@localhost", false),
            new("null => false", null, false),
        ];

        #region Case Records

        public sealed record Case(string Name, string? Value, bool ExpectedReturn)
            : IsCase<string?>(Name, Value, ExpectedReturn);

        #endregion
    }

    public static class HasAlias
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("user+alias@example.com => true", "user+alias@example.com", true),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("user@example.com => false", "user@example.com", false),
            new("display name form => false", "User <user+alias@example.com>", false),
            new("null => false", null, false),
        ];

        #region Case Records

        public sealed record Case(string Name, string? Value, bool ExpectedReturn)
            : IsCase<string?>(Name, Value, ExpectedReturn);

        #endregion
    }
}
