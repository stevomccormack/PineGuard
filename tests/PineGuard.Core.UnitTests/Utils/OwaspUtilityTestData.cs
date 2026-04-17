using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Utils;

public static class OwaspUtilityTestData
{
    public static class ContainsSqlInjectionRisk
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("SELECT * FROM Users => true", "SELECT * FROM Users", true),
            new("hello world => false", "hello world", false)
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("null => false", null, false),
            new("whitespace => false", " ", false)
        ];

        public sealed record ValidCase(string Name, string? Value, bool Expected)
            : HasCase<string?>(Name, Value, Expected);
    }

    public static class ContainsPathTraversalRisk
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("../etc/passwd => true", "../etc/passwd", true),
            new("relative/path => false", "relative/path", false)
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("null => false", null, false),
            new("whitespace => false", " ", false)
        ];

        public sealed record ValidCase(string Name, string? Value, bool Expected)
            : HasCase<string?>(Name, Value, Expected);
    }

    public static class ContainsCommandInjectionRisk
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("foo;rm -rf / => true", "foo;rm -rf /", true),
            new("safe => false", "safe", false)
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("null => false", null, false),
            new("whitespace => false", " ", false)
        ];

        public sealed record ValidCase(string Name, string? Value, bool Expected)
            : HasCase<string?>(Name, Value, Expected);
    }

    public static class ContainsCrLfRisk
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new(@"Header: value\r\nX: y => true", "Header: value\r\nX: y", true),
            new("Header: value => false", "Header: value", false)
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("null => false", null, false),
            new("whitespace => false", " ", false)
        ];

        public sealed record ValidCase(string Name, string? Value, bool Expected)
            : HasCase<string?>(Name, Value, Expected);
    }

    public static class ContainsLdapFilterRisk
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("(uid=*) => true", "(uid=*)", true),
            new("uid=alice => false", "uid=alice", false)
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("null => false", null, false),
            new("whitespace => false", " ", false)
        ];

        public sealed record ValidCase(string Name, string? Value, bool Expected)
            : HasCase<string?>(Name, Value, Expected);
    }

    public static class ContainsOpenRedirectRisk
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("http://example.com => true", "http://example.com", true),
            new("relative/path => false", "relative/path", false)
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("null => false", null, false),
            new("whitespace => false", " ", false)
        ];

        public sealed record ValidCase(string Name, string? Value, bool Expected)
            : HasCase<string?>(Name, Value, Expected);
    }
    public static class ContainsSsrfSchemeRisk
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("file://etc/passwd => true", "file://etc/passwd", true),
            new("https://example.com => false", "https://example.com", false)
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("null => false", null, false),
            new("whitespace => false", " ", false)
        ];

        public sealed record ValidCase(string Name, string? Value, bool Expected)
            : HasCase<string?>(Name, Value, Expected);
    }
}
