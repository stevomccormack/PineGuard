using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Rules.Owasp;

public static class OwaspRulesTestData
{
    public static class OwaspRegexXss
    {
        public static TheoryData<Case> HtmlTagRegexCases =>
        [
            new("script tag", "<script>alert(1)</script>", true),
            new("simple tag", "<b>hi</b>", true),
            new("img + onerror", "<img src=x onerror=alert(1)>", true),
            new("not a tag", "a < 1 > b", false)
        ];

        public static TheoryData<Case> HtmlEntityEncodedAngleBracketRegexCases =>
        [
            new("&lt;", "&lt;", true),
            new("&gt;", "&gt;", true),
            new("&#60;", "&#60;", true),
            new("&#062;", "&#062;", true),
            new("&#x3c;", "&#x3c;", true),
            new("&#x003E;", "&#x003E;", true),
            new("plain text", "hello", false)
        ];

        public static TheoryData<Case> ScriptProtocolRegexCases =>
        [
            new("javascript", "javascript:alert(1)", true),
            new("mixed case javascript", "JaVaScRiPt:alert(1)", true),
            new("data", "data:text/html;base64,PHNjcmlwdD5hbGVydCgxKTwvc2NyaXB0Pg==", true),
            new("https", "https://example.com", false)
        ];

        public static TheoryData<Case> HtmlEventHandlerAttributeRegexCases =>
        [
            new("onload", "onload=", true),
            new("onclick spaced", "onclick =", true),
            new("onerror mixed", "ONERROR=alert(1)", true),
            new("non-on attr", "role=", false)
        ];

        #region Case Records

        public sealed record Case(string Name, string Value, bool ExpectedReturn)
            : IsCase<string>(Name, Value, ExpectedReturn);

        #endregion Case Records
    }

    public static class IsOwaspSafe
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("simple", "hello-world_123", true),
            new("relative path", " relative/path ", true)
        ];

        #region Case Records

        public sealed record Case(string Name, string? Value, bool ExpectedReturn)
            : IsCase<string?>(Name, Value, ExpectedReturn);

        #endregion Cases
    }

    public static class IsOwaspDangerous
    {
        public static TheoryData<Case> Cases =>
        [
            new("null", null, true),
            new("space", " ", true),
            new("xss", "<b>hi</b>", true),
            new("sql", "select * from users", true),
            new("traversal", "../etc/passwd", true),
            new("command injection", "cmd && whoami", true),
            new("crlf", "Header: ok\r\nInjected: yes", true),
            new("ldap", "(uid=*)", true),
            new("open redirect", "http://example.com", true),
            new("protocol-relative", "//example.com", true),
            new("ssrf scheme", "file:///etc/passwd", true)
        ];

        #region Case Records

        public sealed record Case(string Name, string? Value, bool ExpectedReturn)
            : IsCase<string?>(Name, Value, ExpectedReturn);

        #endregion Cases
    }

    public static class IsXssSafe
    {
        public static TheoryData<Case> Cases =>
        [
            new("safe", "hello", true),
            new("script tag", "<script>alert(1)</script>", false),
            new("null", null, false),
            new("space", " ", false)
        ];

        #region Case Records

        public sealed record Case(string Name, string? Value, bool ExpectedReturn)
            : IsCase<string?>(Name, Value, ExpectedReturn);

        #endregion Cases
    }

    public static class IsSqlInjectionSafe
    {
        public static TheoryData<Case> Cases =>
        [
            new("safe", "hello", true),
            new("keyword", "select", false),
            new("boolean", "1 OR 1=1", false),
            new("semicolon", "hello;", false),
            new("quoted", "'quoted'", false),
            new("null", null, false),
            new("space", " ", false)
        ];

        #region Case Records

        public sealed record Case(string Name, string? Value, bool ExpectedReturn)
            : IsCase<string?>(Name, Value, ExpectedReturn);

        #endregion Cases
    }

    public static class IsPathTraversalSafe
    {
        public static TheoryData<Case> Cases =>
        [
            new("relative", "relative/path", true),
            new("dotdot", "../etc/passwd", false),
            new("absolute unix", "/etc/passwd", false),
            new("absolute windows", @"C:\Windows\System32", false),
            new("unc", @"\\server\share\file.txt", false),
            new("null", null, false),
            new("space", " ", false)
        ];

        #region Case Records

        public sealed record Case(string Name, string? Value, bool ExpectedReturn)
            : IsCase<string?>(Name, Value, ExpectedReturn);

        #endregion Cases
    }

    public static class IsCommandInjectionSafe
    {
        public static TheoryData<Case> Cases =>
        [
            new("safe", "hello", true),
            new("semicolon", "echo hi; rm -rf /", false),
            new("pipe", "a|b", false),
            new("newline", "a\nwhoami", false),
            new("null", null, false),
            new("space", " ", false)
        ];

        #region Case Records

        public sealed record Case(string Name, string? Value, bool ExpectedReturn)
            : IsCase<string?>(Name, Value, ExpectedReturn);

        #endregion Cases
    }

    public static class IsCrLfSafe
    {
        public static TheoryData<Case> Cases =>
        [
            new("safe", "Header: ok", true),
            new("crlf", "Header: ok\r\nInjected: yes", false),
            new("null", null, false),
            new("space", " ", false)
        ];

        #region Case Records

        public sealed record Case(string Name, string? Value, bool ExpectedReturn)
            : IsCase<string?>(Name, Value, ExpectedReturn);

        #endregion Cases
    }

    public static class IsLdapFilterSafe
    {
        public static TheoryData<Case> Cases =>
        [
            new("safe", "uid=jdoe", true),
            new("special chars", "(uid=*)", false),
            new("null", null, false),
            new("space", " ", false)
        ];

        #region Case Records

        public sealed record Case(string Name, string? Value, bool ExpectedReturn)
            : IsCase<string?>(Name, Value, ExpectedReturn);

        #endregion Cases
    }

    public static class IsOpenRedirectSafe
    {
        public static TheoryData<Case> Cases =>
        [
            new("relative", "/relative/path", true),
            new("http", "http://example.com", false),
            new("protocol-relative", "//example.com", false),
            new("null", null, false),
            new("space", " ", false)
        ];

        #region Case Records

        public sealed record Case(string Name, string? Value, bool ExpectedReturn)
            : IsCase<string?>(Name, Value, ExpectedReturn);

        #endregion Cases
    }

    public static class IsSsrfSchemeSafe
    {
        public static TheoryData<Case> Cases =>
        [
            new("https", "https://example.com", true),
            new("file", "file:///etc/passwd", false),
            new("gopher", "gopher://example.com", false),
            new("null", null, false),
            new("space", " ", false)
        ];

        #region Case Records

        public sealed record Case(string Name, string? Value, bool ExpectedReturn)
            : IsCase<string?>(Name, Value, ExpectedReturn);

        #endregion Cases
    }
}
