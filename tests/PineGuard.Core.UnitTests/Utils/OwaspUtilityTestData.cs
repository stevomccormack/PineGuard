using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Utils;

public static class OwaspUtilityTestData
{
    public static class ContainsSqlInjectionRisk
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case(Name: "SELECT * FROM Users => true", Input: "SELECT * FROM Users", ExpectedReturn: true) },
            { new Case(Name: "hello world => false", Input: "hello world", ExpectedReturn: false) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case(Name: "null => false", Input: null, ExpectedReturn: false) },
            { new Case(Name: "whitespace => false", Input: " ", ExpectedReturn: false) },
        };

        public static TheoryData<ThrowsCase> InvalidCases => [];

        #region Cases

        public sealed record Case(string Name, string? Input, bool ExpectedReturn)
            : HasCase<string?>(Name, Input, ExpectedReturn);

        #endregion Cases
    }

    public static class ContainsPathTraversalRisk
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case(Name: "../etc/passwd => true", Input: "../etc/passwd", ExpectedReturn: true) },
            { new Case(Name: "relative/path => false", Input: "relative/path", ExpectedReturn: false) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case(Name: "null => false", Input: null, ExpectedReturn: false) },
            { new Case(Name: "whitespace => false", Input: " ", ExpectedReturn: false) },
        };

        public static TheoryData<ThrowsCase> InvalidCases => [];

        #region Cases

        public sealed record Case(string Name, string? Input, bool ExpectedReturn)
            : HasCase<string?>(Name, Input, ExpectedReturn);

        #endregion Cases
    }

    public static class ContainsCommandInjectionRisk
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case(Name: "foo;rm -rf / => true", Input: "foo;rm -rf /", ExpectedReturn: true) },
            { new Case(Name: "safe => false", Input: "safe", ExpectedReturn: false) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case(Name: "null => false", Input: null, ExpectedReturn: false) },
            { new Case(Name: "whitespace => false", Input: " ", ExpectedReturn: false) },
        };

        public static TheoryData<ThrowsCase> InvalidCases => [];

        #region Cases

        public sealed record Case(string Name, string? Input, bool ExpectedReturn)
            : HasCase<string?>(Name, Input, ExpectedReturn);

        #endregion Cases
    }

    public static class ContainsCrLfRisk
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case(Name: "Header: value\\r\\nX: y => true", Input: "Header: value\r\nX: y", ExpectedReturn: true) },
            { new Case(Name: "Header: value => false", Input: "Header: value", ExpectedReturn: false) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case(Name: "null => false", Input: null, ExpectedReturn: false) },
            { new Case(Name: "whitespace => false", Input: " ", ExpectedReturn: false) },
        };

        public static TheoryData<ThrowsCase> InvalidCases => [];

        #region Cases

        public sealed record Case(string Name, string? Input, bool ExpectedReturn)
            : HasCase<string?>(Name, Input, ExpectedReturn);

        #endregion Cases
    }

    public static class ContainsLdapFilterRisk
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case(Name: "(uid=*) => true", Input: "(uid=*)", ExpectedReturn: true) },
            { new Case(Name: "uid=alice => false", Input: "uid=alice", ExpectedReturn: false) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case(Name: "null => false", Input: null, ExpectedReturn: false) },
            { new Case(Name: "whitespace => false", Input: " ", ExpectedReturn: false) },
        };

        public static TheoryData<ThrowsCase> InvalidCases => [];

        #region Cases

        public sealed record Case(string Name, string? Input, bool ExpectedReturn)
            : HasCase<string?>(Name, Input, ExpectedReturn);

        #endregion Cases
    }

    public static class ContainsOpenRedirectRisk
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case(Name: "http://example.com => true", Input: "http://example.com", ExpectedReturn: true) },
            { new Case(Name: "relative/path => false", Input: "relative/path", ExpectedReturn: false) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case(Name: "null => false", Input: null, ExpectedReturn: false) },
            { new Case(Name: "whitespace => false", Input: " ", ExpectedReturn: false) },
        };

        public static TheoryData<ThrowsCase> InvalidCases => [];

        #region Cases

        public sealed record Case(string Name, string? Input, bool ExpectedReturn)
            : HasCase<string?>(Name, Input, ExpectedReturn);

        #endregion Cases
    }

    public static class ContainsSsrfSchemeRisk
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case(Name: "file://etc/passwd => true", Input: "file://etc/passwd", ExpectedReturn: true) },
            { new Case(Name: "https://example.com => false", Input: "https://example.com", ExpectedReturn: false) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case(Name: "null => false", Input: null, ExpectedReturn: false) },
            { new Case(Name: "whitespace => false", Input: " ", ExpectedReturn: false) },
        };

        public static TheoryData<ThrowsCase> InvalidCases => [];

        #region Cases

        public sealed record Case(string Name, string? Input, bool ExpectedReturn)
            : HasCase<string?>(Name, Input, ExpectedReturn);

        #endregion Cases
    }
}
