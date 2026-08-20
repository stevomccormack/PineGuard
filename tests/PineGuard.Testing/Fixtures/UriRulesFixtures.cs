using PineGuard.Testing.UnitTests.Rules;

namespace PineGuard.Testing.Fixtures;

public static class UriRulesFixtures
{
    public static class IsAbsoluteUri
    {
        public static readonly string? HttpsUrl = "https://example.com";
        public static readonly string? FileUrl = "file:///C:/Temp/file.txt";
        public static readonly string? FtpUrl = "ftp://example.com";
        public static readonly string? HttpWithPath = "http://example.com/path?q=1";
        public static readonly string? NullValue = null;
        public static readonly string? EmptyString = "";
        public static readonly string? WhitespaceOnly = "   ";
        public static readonly string? RelativePath = "relative/path";
        public static readonly string? DotRelative = "./file.txt";
        public static readonly string? SlashRelative = "/foo/bar";

        public static RuleScenario<string?>[] ValidScenarios =>
        [
            new(nameof(HttpsUrl), HttpsUrl, true),
            new(nameof(FileUrl), FileUrl, true),
            new(nameof(FtpUrl), FtpUrl, true),
            new(nameof(HttpWithPath), HttpWithPath, true)
        ];

        public static RuleScenario<string?>[] InvalidScenarios =>
        [
            new(nameof(NullValue), NullValue, false),
            new(nameof(EmptyString), EmptyString, false),
            new(nameof(WhitespaceOnly), WhitespaceOnly, false),
            new(nameof(RelativePath), RelativePath, false),
            new(nameof(DotRelative), DotRelative, false),
            new(nameof(SlashRelative), SlashRelative, false)
        ];

        public static RuleScenario<string?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsRelativeUri
    {
        public static readonly string? RelativePath = "relative/path";
        public static readonly string? DotPath = "./file.txt";
        public static readonly string? SlashPath = "/foo/bar";
        public static readonly string? FileName = "foo.txt";
        public static readonly string? NullValue = null;
        public static readonly string? EmptyString = "";
        public static readonly string? AbsoluteHttps = "https://example.com";
        public static readonly string? AbsoluteHttp = "http://example.com";

        public static RuleScenario<string?>[] ValidScenarios =>
        [
            new(nameof(RelativePath), RelativePath, true),
            new(nameof(DotPath), DotPath, true),
            new(nameof(SlashPath), SlashPath, true),
            new(nameof(FileName), FileName, true)
        ];

        public static RuleScenario<string?>[] InvalidScenarios =>
        [
            new(nameof(NullValue), NullValue, false),
            new(nameof(EmptyString), EmptyString, false),
            new(nameof(AbsoluteHttps), AbsoluteHttps, false),
            new(nameof(AbsoluteHttp), AbsoluteHttp, false)
        ];

        public static RuleScenario<string?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsUrl
    {
        public static readonly string? HttpsUrl = "https://example.com";
        public static readonly string? HttpUrl = "http://example.com";
        public static readonly string? HttpWithPath = "http://example.com/foo?bar=1";
        public static readonly string? NullValue = null;
        public static readonly string? EmptyString = "";
        public static readonly string? FileUrl = "file:///C:/Temp/file.txt";
        public static readonly string? RelativePath = "relative/path";
        public static readonly string? FtpUrl = "ftp://example.com";
        public static readonly string? NotAUrl = "not-a-url";

        public static RuleScenario<string?>[] ValidScenarios =>
        [
            new(nameof(HttpsUrl), HttpsUrl, true),
            new(nameof(HttpUrl), HttpUrl, true),
            new(nameof(HttpWithPath), HttpWithPath, true)
        ];

        public static RuleScenario<string?>[] InvalidScenarios =>
        [
            new(nameof(NullValue), NullValue, false),
            new(nameof(EmptyString), EmptyString, false),
            new(nameof(FileUrl), FileUrl, false),
            new(nameof(RelativePath), RelativePath, false),
            new(nameof(FtpUrl), FtpUrl, false),
            new(nameof(NotAUrl), NotAUrl, false)
        ];

        public static RuleScenario<string?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsHttpsUrl
    {
        public static readonly string? HttpsSimple = "https://example.com";
        public static readonly string? HttpsWithPath = "https://example.com/path";
        public static readonly string? NullValue = null;
        public static readonly string? EmptyString = "";
        public static readonly string? HttpUrl = "http://example.com";
        public static readonly string? FtpUrl = "ftp://example.com";
        public static readonly string? NotAUrl = "not-a-url";

        public static RuleScenario<string?>[] ValidScenarios =>
        [
            new(nameof(HttpsSimple), HttpsSimple, true),
            new(nameof(HttpsWithPath), HttpsWithPath, true)
        ];

        public static RuleScenario<string?>[] InvalidScenarios =>
        [
            new(nameof(NullValue), NullValue, false),
            new(nameof(EmptyString), EmptyString, false),
            new(nameof(HttpUrl), HttpUrl, false),
            new(nameof(FtpUrl), FtpUrl, false),
            new(nameof(NotAUrl), NotAUrl, false)
        ];

        public static RuleScenario<string?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsHttpUrl
    {
        public static readonly string? HttpSimple = "http://example.com";
        public static readonly string? HttpWithPath = "http://example.com/path";
        public static readonly string? NullValue = null;
        public static readonly string? EmptyString = "";
        public static readonly string? HttpsUrl = "https://example.com";
        public static readonly string? FtpUrl = "ftp://example.com";
        public static readonly string? NotAUrl = "not-a-url";

        public static RuleScenario<string?>[] ValidScenarios =>
        [
            new(nameof(HttpSimple), HttpSimple, true),
            new(nameof(HttpWithPath), HttpWithPath, true)
        ];

        public static RuleScenario<string?>[] InvalidScenarios =>
        [
            new(nameof(NullValue), NullValue, false),
            new(nameof(EmptyString), EmptyString, false),
            new(nameof(HttpsUrl), HttpsUrl, false),
            new(nameof(FtpUrl), FtpUrl, false),
            new(nameof(NotAUrl), NotAUrl, false)
        ];

        public static RuleScenario<string?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsFileUri
    {
        public static readonly string? FileWindows = "file:///C:/Temp/file.txt";
        public static readonly string? FileUnix = "file:///tmp/file.txt";
        public static readonly string? NullValue = null;
        public static readonly string? EmptyString = "";
        public static readonly string? HttpsUrl = "https://example.com";
        public static readonly string? RelativePath = "relative/path";

        public static RuleScenario<string?>[] ValidScenarios =>
        [
            new(nameof(FileWindows), FileWindows, true),
            new(nameof(FileUnix), FileUnix, true)
        ];

        public static RuleScenario<string?>[] InvalidScenarios =>
        [
            new(nameof(NullValue), NullValue, false),
            new(nameof(EmptyString), EmptyString, false),
            new(nameof(HttpsUrl), HttpsUrl, false),
            new(nameof(RelativePath), RelativePath, false)
        ];

        public static RuleScenario<string?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsFilePath
    {
        public static readonly string? WindowsPath = @"C:\Temp\file.txt";
        public static readonly string? WindowsForwardSlashPath = "C:/Temp/file.txt";
        public static readonly string? UncPath = @"\\server\share\file.txt";
        public static readonly string? NullValue = null;
        public static readonly string? EmptyString = "";
        public static readonly string? HttpsUrl = "https://example.com";
        public static readonly string? InvalidChars = "C:|Foo";
        public static readonly string? DriveRelativePath = "C:file.txt";
        public static readonly string? RelativeParentPath = @"..\file.txt";
        public static readonly string? ShorterThanDrivePrefix = "ab";

        public static RuleScenario<string?>[] ValidScenarios =>
        [
            new(nameof(WindowsPath), WindowsPath, true),
            new(nameof(WindowsForwardSlashPath), WindowsForwardSlashPath, true),
            new(nameof(UncPath), UncPath, true)
        ];

        public static RuleScenario<string?>[] InvalidScenarios =>
        [
            new(nameof(NullValue), NullValue, false),
            new(nameof(EmptyString), EmptyString, false),
            new(nameof(HttpsUrl), HttpsUrl, false),
            new(nameof(InvalidChars), InvalidChars, false),
            new(nameof(DriveRelativePath), DriveRelativePath, false),
            new(nameof(RelativeParentPath), RelativeParentPath, false),
            new(nameof(ShorterThanDrivePrefix), ShorterThanDrivePrefix, false)
        ];

        public static RuleScenario<string?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class HasScheme
    {
        public static readonly (string? value, string scheme) HttpsMatch = ("https://example.com", "https");
        public static readonly (string? value, string scheme) CaseInsensitive = ("HTTPS://example.com", "https");
        public static readonly (string? value, string scheme) FtpMatch = ("ftp://example.com", "ftp");
        public static readonly (string? value, string scheme) NullValue = (null, "https");
        public static readonly (string? value, string scheme) EmptyValue = ("", "https");
        public static readonly (string? value, string scheme) WrongScheme = ("http://example.com", "https");
        public static readonly (string? value, string scheme) RelativePath = ("relative/path", "https");
        public static readonly (string? value, string scheme) NullScheme = ("https://example.com", null!);

        public static RuleScenario<(string? value, string scheme)>[] ValidScenarios =>
        [
            new(nameof(HttpsMatch), HttpsMatch, true),
            new(nameof(CaseInsensitive), CaseInsensitive, true),
            new(nameof(FtpMatch), FtpMatch, true)
        ];

        public static RuleScenario<(string? value, string scheme)>[] InvalidScenarios =>
        [
            new(nameof(NullValue), NullValue, false),
            new(nameof(EmptyValue), EmptyValue, false),
            new(nameof(WrongScheme), WrongScheme, false),
            new(nameof(RelativePath), RelativePath, false)
        ];

        public static RuleScenario<(string? value, string scheme)>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }
}
