namespace PineGuard.Core.UnitTests.Rules;

public static class UriRulesTestData
{
    public static class IsAbsoluteUri
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case("https", "https://example.com", true) },
            { new Case("file", "file:///C:/Temp/file.txt", true) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case("null", null, false) },
            { new Case("empty", string.Empty, false) },
            { new Case("relative", "relative/path", false) },
        };

        #region Cases

        public sealed record Case(string Name, string? Value, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }

    public static class IsRelativeUri
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case("relative path", "relative/path", true) },
            { new Case("dot", "./file.txt", true) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case("null", null, false) },
            { new Case("empty", string.Empty, false) },
            { new Case("absolute", "https://example.com", false) },
        };

        #region Cases

        public sealed record Case(string Name, string? Value, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }

    public static class IsUrl
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case("https", "https://example.com", true) },
            { new Case("http", "http://example.com", true) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case("null", null, false) },
            { new Case("empty", string.Empty, false) },
            { new Case("file", "file:///C:/Temp/file.txt", false) },
            { new Case("relative", "relative/path", false) },
        };

        #region Cases

        public sealed record Case(string Name, string? Value, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }

    public static class IsHttpsUrl
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case("https", "https://example.com", true) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case("null", null, false) },
            { new Case("empty", string.Empty, false) },
            { new Case("http", "http://example.com", false) },
        };

        #region Cases

        public sealed record Case(string Name, string? Value, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }

    public static class IsHttpUrl
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case("http", "http://example.com", true) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case("null", null, false) },
            { new Case("empty", string.Empty, false) },
            { new Case("https", "https://example.com", false) },
        };

        #region Cases

        public sealed record Case(string Name, string? Value, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }

    public static class IsFileUri
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case("file", "file:///C:/Temp/file.txt", true) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case("null", null, false) },
            { new Case("empty", string.Empty, false) },
            { new Case("https", "https://example.com", false) },
        };

        #region Cases

        public sealed record Case(string Name, string? Value, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }

    public static class IsFilePath
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case("windows", "C:\\Temp\\file.txt", true) },
            { new Case("unc", "\\\\server\\share\\file.txt", true) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case("null", null, false) },
            { new Case("empty", string.Empty, false) },
            { new Case("https", "https://example.com", false) },
        };

        #region Cases

        public sealed record Case(string Name, string? Value, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }

    public static class HasScheme
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case("https", "https://example.com", "https", true) },
            { new Case("case insensitive", "HTTPS://example.com", "https", true) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case("null", null, "https", false) },
            { new Case("empty", string.Empty, "https", false) },
            { new Case("wrong scheme", "http://example.com", "https", false) },
            { new Case("relative", "relative/path", "https", false) },
        };

        #region Cases

        public sealed record Case(string Name, string? Value, string Scheme, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }
}
