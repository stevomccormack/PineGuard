namespace PineGuard.Core.UnitTests.Rules.Owasp;

public static class OwaspRulesTestData
{
    public static class IsOwaspSafe
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case("simple", "hello-world_123", true) },
            { new Case("relative path", " relative/path ", true) },
        };

        #region Cases

        public sealed record Case(string Name, string? Value, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion Cases
    }

    public static class IsOwaspDangerous
    {
        public static TheoryData<Case> Cases => new()
        {
            { new Case("null", null, true) },
            { new Case("space", " ", true) },
            { new Case("xss", "<b>hi</b>", true) },
            { new Case("sql", "select * from users", true) },
            { new Case("traversal", "../etc/passwd", true) },
            { new Case("command injection", "cmd && whoami", true) },
            { new Case("crlf", "Header: ok\r\nInjected: yes", true) },
            { new Case("ldap", "(uid=*)", true) },
            { new Case("open redirect", "http://example.com", true) },
            { new Case("protocol-relative", "//example.com", true) },
            { new Case("ssrf scheme", "file:///etc/passwd", true) },
        };

        #region Cases

        public sealed record Case(string Name, string? Value, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion Cases
    }

    public static class IsXssSafe
    {
        public static TheoryData<Case> Cases => new()
        {
            { new Case("safe", "hello", true) },
            { new Case("script tag", "<script>alert(1)</script>", false) },
            { new Case("null", null, false) },
            { new Case("space", " ", false) },
        };

        #region Cases

        public sealed record Case(string Name, string? Value, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion Cases
    }

    public static class IsSqlInjectionSafe
    {
        public static TheoryData<Case> Cases => new()
        {
            { new Case("safe", "hello", true) },
            { new Case("keyword", "select", false) },
            { new Case("boolean", "1 OR 1=1", false) },
            { new Case("semicolon", "hello;", false) },
            { new Case("quoted", "'quoted'", false) },
            { new Case("null", null, false) },
            { new Case("space", " ", false) },
        };

        #region Cases

        public sealed record Case(string Name, string? Value, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion Cases
    }

    public static class IsPathTraversalSafe
    {
        public static TheoryData<Case> Cases => new()
        {
            { new Case("relative", "relative/path", true) },
            { new Case("dotdot", "../etc/passwd", false) },
            { new Case("absolute unix", "/etc/passwd", false) },
            { new Case("absolute windows", "C:\\Windows\\System32", false) },
            { new Case("unc", "\\\\server\\share\\file.txt", false) },
            { new Case("null", null, false) },
            { new Case("space", " ", false) },
        };

        #region Cases

        public sealed record Case(string Name, string? Value, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion Cases
    }

    public static class IsCommandInjectionSafe
    {
        public static TheoryData<Case> Cases => new()
        {
            { new Case("safe", "hello", true) },
            { new Case("semicolon", "echo hi; rm -rf /", false) },
            { new Case("pipe", "a|b", false) },
            { new Case("newline", "a\nwhoami", false) },
            { new Case("null", null, false) },
            { new Case("space", " ", false) },
        };

        #region Cases

        public sealed record Case(string Name, string? Value, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion Cases
    }

    public static class IsCrLfSafe
    {
        public static TheoryData<Case> Cases => new()
        {
            { new Case("safe", "Header: ok", true) },
            { new Case("crlf", "Header: ok\r\nInjected: yes", false) },
            { new Case("null", null, false) },
            { new Case("space", " ", false) },
        };

        #region Cases

        public sealed record Case(string Name, string? Value, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion Cases
    }

    public static class IsLdapFilterSafe
    {
        public static TheoryData<Case> Cases => new()
        {
            { new Case("safe", "uid=jdoe", true) },
            { new Case("special chars", "(uid=*)", false) },
            { new Case("null", null, false) },
            { new Case("space", " ", false) },
        };

        #region Cases

        public sealed record Case(string Name, string? Value, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion Cases
    }

    public static class IsOpenRedirectSafe
    {
        public static TheoryData<Case> Cases => new()
        {
            { new Case("relative", "/relative/path", true) },
            { new Case("http", "http://example.com", false) },
            { new Case("protocol-relative", "//example.com", false) },
            { new Case("null", null, false) },
            { new Case("space", " ", false) },
        };

        #region Cases

        public sealed record Case(string Name, string? Value, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion Cases
    }

    public static class IsSsrfSchemeSafe
    {
        public static TheoryData<Case> Cases => new()
        {
            { new Case("https", "https://example.com", true) },
            { new Case("file", "file:///etc/passwd", false) },
            { new Case("gopher", "gopher://example.com", false) },
            { new Case("null", null, false) },
            { new Case("space", " ", false) },
        };

        #region Cases

        public sealed record Case(string Name, string? Value, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion Cases
    }
}
