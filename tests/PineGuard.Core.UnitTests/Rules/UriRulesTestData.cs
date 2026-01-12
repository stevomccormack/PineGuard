using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Rules;

public static class UriRulesTestData
{
    public static class IsAbsoluteUri
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("https", "https://example.com", true),
            new("file", "file:///C:/Temp/file.txt", true)
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("null", null, false),
            new("empty", string.Empty, false),
            new("relative", "relative/path", false)
        ];

        #region Case Records

        public sealed record Case(string Name, string? Value, bool ExpectedReturn)
            : IsCase<string?>(Name, Value, ExpectedReturn);

        #endregion
    }

    public static class IsRelativeUri
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("relative path", "relative/path", true),
            new("dot", "./file.txt", true)
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("null", null, false),
            new("empty", string.Empty, false),
            new("absolute", "https://example.com", false)
        ];

        #region Case Records

        public sealed record Case(string Name, string? Value, bool ExpectedReturn)
            : IsCase<string?>(Name, Value, ExpectedReturn);

        #endregion
    }

    public static class IsUrl
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("https", "https://example.com", true),
            new("http", "http://example.com", true)
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("null", null, false),
            new("empty", string.Empty, false),
            new("file", "file:///C:/Temp/file.txt", false),
            new("relative", "relative/path", false)
        ];

        #region Case Records

        public sealed record Case(string Name, string? Value, bool ExpectedReturn)
            : IsCase<string?>(Name, Value, ExpectedReturn);

        #endregion
    }

    public static class IsHttpsUrl
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("https", "https://example.com", true)
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("null", null, false),
            new("empty", string.Empty, false),
            new("http", "http://example.com", false)
        ];

        #region Case Records

        public sealed record Case(string Name, string? Value, bool ExpectedReturn)
            : IsCase<string?>(Name, Value, ExpectedReturn);

        #endregion
    }

    public static class IsHttpUrl
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("http", "http://example.com", true)
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("null", null, false),
            new("empty", string.Empty, false),
            new("https", "https://example.com", false)
        ];

        #region Case Records

        public sealed record Case(string Name, string? Value, bool ExpectedReturn)
            : IsCase<string?>(Name, Value, ExpectedReturn);

        #endregion
    }

    public static class IsFileUri
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("file", "file:///C:/Temp/file.txt", true)
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("null", null, false),
            new("empty", string.Empty, false),
            new("https", "https://example.com", false)
        ];

        #region Case Records

        public sealed record Case(string Name, string? Value, bool ExpectedReturn)
            : IsCase<string?>(Name, Value, ExpectedReturn);

        #endregion
    }

    public static class IsFilePath
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("windows", @"C:\Temp\file.txt", true),
            new("unc", @"\\server\share\file.txt", true)
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("null", null, false),
            new("empty", string.Empty, false),
            new("https", "https://example.com", false)
        ];

        #region Case Records

        public sealed record Case(string Name, string? Value, bool ExpectedReturn)
            : IsCase<string?>(Name, Value, ExpectedReturn);

        #endregion
    }

    public static class HasScheme
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("https", "https://example.com", "https", true),
            new("case insensitive", "HTTPS://example.com", "https", true)
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("null", null, "https", false),
            new("empty", string.Empty, "https", false),
            new("wrong scheme", "http://example.com", "https", false),
            new("relative", "relative/path", "https", false)
        ];

        #region Case Records

        public sealed record Case(string Name, string? Value, string Scheme, bool ExpectedReturn)
            : HasCase<string?>(Name, Value, ExpectedReturn);

        #endregion
    }
}
