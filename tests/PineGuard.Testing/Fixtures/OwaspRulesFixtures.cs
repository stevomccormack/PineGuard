using PineGuard.Testing.UnitTests.Rules;

namespace PineGuard.Testing.Fixtures;

public static class OwaspRulesFixtures
{
    public static class IsOwaspSafe
    {
        public static readonly string? Simple = "hello-world_123";
        public static readonly string? RelativePath = " relative/path ";
        public static readonly string? Null = null;

        public static RuleScenario<string?>[] ValidScenarios =>
        [
            new(nameof(Simple),       Simple,       true),
            new(nameof(RelativePath), RelativePath, true)
        ];

        public static RuleScenario<string?>[] InvalidScenarios =>
        [
            new(nameof(IsXssSafe.ScriptTag),          IsXssSafe.ScriptTag,          false),
            new(nameof(IsSqlInjectionSafe.Keyword),   IsSqlInjectionSafe.Keyword,   false),
            new(nameof(IsPathTraversalSafe.DotDot),   IsPathTraversalSafe.DotDot,   false),
            new(nameof(IsCommandInjectionSafe.Pipe),  IsCommandInjectionSafe.Pipe,  false),
            new(nameof(IsLdapFilterSafe.SpecialChars),IsLdapFilterSafe.SpecialChars,false),
            new(nameof(IsOpenRedirectSafe.Http),      IsOpenRedirectSafe.Http,      false),
            new(nameof(IsSsrfSchemeSafe.File),        IsSsrfSchemeSafe.File,        false),
            new(nameof(Null),                         Null,                         false)
        ];

        public static RuleScenario<string?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsXssSafe
    {
        public static readonly string? Safe = "hello";
        public static readonly string? ScriptTag = "<script>alert(1)</script>";
        public static readonly string? ScriptProtocol = "javascript:alert(1)";
        public static readonly string? EventHandler = "x onload=alert(1)";
        public static readonly string? EntityEncoded = "&lt;script&gt;alert(1)&lt;/script&gt;";
        public static readonly string? Null = null;
        public static readonly string? Space = " ";

        public static RuleScenario<string?>[] ValidScenarios =>
        [
            new(nameof(Safe), Safe, true)
        ];

        public static RuleScenario<string?>[] InvalidScenarios =>
        [
            new(nameof(ScriptTag),      ScriptTag,      false),
            new(nameof(ScriptProtocol), ScriptProtocol, false),
            new(nameof(EventHandler),   EventHandler,   false),
            new(nameof(EntityEncoded),  EntityEncoded,  false),
            new(nameof(Null),           Null,           false),
            new(nameof(Space),          Space,          false)
        ];

        public static RuleScenario<string?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsSqlInjectionSafe
    {
        public static readonly string? Safe = "hello";
        public static readonly string? Keyword = "select";
        public static readonly string? Boolean = "1 OR 1=1";
        public static readonly string? Semicolon = "hello;";
        public static readonly string? Quoted = "'quoted'";
        public static readonly string? Null = null;
        public static readonly string? Space = " ";

        public static RuleScenario<string?>[] ValidScenarios =>
        [
            new(nameof(Safe), Safe, true)
        ];

        public static RuleScenario<string?>[] InvalidScenarios =>
        [
            new(nameof(Keyword),   Keyword,   false),
            new(nameof(Boolean),   Boolean,   false),
            new(nameof(Semicolon), Semicolon, false),
            new(nameof(Quoted),    Quoted,    false),
            new(nameof(Null),      Null,      false),
            new(nameof(Space),     Space,     false)
        ];

        public static RuleScenario<string?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsPathTraversalSafe
    {
        public static readonly string? Relative = "relative/path";
        public static readonly string? DotDot = "../etc/passwd";
        public static readonly string? AbsoluteUnix = "/etc/passwd";
        public static readonly string? AbsoluteWindows = @"C:\Windows\System32";
        public static readonly string? Unc = @"\\server\share\file.txt";
        public static readonly string? EncodedSlashDotDot = "..%2f..%2fetc%2fpasswd";
        public static readonly string? TrailingDotDotSegment = "uploads/..";
        public static readonly string? Null = null;
        public static readonly string? Space = " ";

        public static RuleScenario<string?>[] ValidScenarios =>
        [
            new(nameof(Relative), Relative, true)
        ];

        public static RuleScenario<string?>[] InvalidScenarios =>
        [
            new(nameof(DotDot),                DotDot,                false),
            new(nameof(AbsoluteUnix),          AbsoluteUnix,          false),
            new(nameof(AbsoluteWindows),       AbsoluteWindows,       false),
            new(nameof(Unc),                   Unc,                   false),
            new(nameof(EncodedSlashDotDot),    EncodedSlashDotDot,    false),
            new(nameof(TrailingDotDotSegment), TrailingDotDotSegment, false),
            new(nameof(Null),                  Null,                  false),
            new(nameof(Space),                 Space,                 false)
        ];

        public static RuleScenario<string?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsCommandInjectionSafe
    {
        public static readonly string? Safe = "hello";
        public static readonly string? Semicolon = "echo hi; rm -rf /";
        public static readonly string? Pipe = "a|b";
        public static readonly string? Newline = "a\nwhoami";
        public static readonly string? Null = null;
        public static readonly string? Space = " ";

        public static RuleScenario<string?>[] ValidScenarios =>
        [
            new(nameof(Safe), Safe, true)
        ];

        public static RuleScenario<string?>[] InvalidScenarios =>
        [
            new(nameof(Semicolon), Semicolon, false),
            new(nameof(Pipe),      Pipe,      false),
            new(nameof(Newline),   Newline,   false),
            new(nameof(Null),      Null,      false),
            new(nameof(Space),     Space,     false)
        ];

        public static RuleScenario<string?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsCrLfSafe
    {
        public static readonly string? Safe = "Header: ok";
        public static readonly string? CrLf = "Header: ok\r\nInjected: yes";
        public static readonly string? LeadingCrLf = "\r\nHeader: ok";
        public static readonly string? TrailingCrLf = "sessiontoken\r\n\r\n";
        public static readonly string? Null = null;
        public static readonly string? Space = " ";

        public static RuleScenario<string?>[] ValidScenarios =>
        [
            new(nameof(Safe), Safe, true)
        ];

        public static RuleScenario<string?>[] InvalidScenarios =>
        [
            new(nameof(CrLf),         CrLf,         false),
            new(nameof(LeadingCrLf),  LeadingCrLf,  false),
            new(nameof(TrailingCrLf), TrailingCrLf, false),
            new(nameof(Null),         Null,         false),
            new(nameof(Space),        Space,        false)
        ];

        public static RuleScenario<string?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsLdapFilterSafe
    {
        public static readonly string? Safe = "uid=jdoe";
        public static readonly string? SpecialChars = "(uid=*)";
        public static readonly string? Null = null;
        public static readonly string? Space = " ";

        public static RuleScenario<string?>[] ValidScenarios =>
        [
            new(nameof(Safe), Safe, true)
        ];

        public static RuleScenario<string?>[] InvalidScenarios =>
        [
            new(nameof(SpecialChars), SpecialChars, false),
            new(nameof(Null),         Null,         false),
            new(nameof(Space),        Space,        false)
        ];

        public static RuleScenario<string?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsOpenRedirectSafe
    {
        public static readonly string? Relative = "/relative/path";
        public static readonly string? Http = "http://example.com";
        public static readonly string? ProtocolRelative = "//example.com";
        public static readonly string? Null = null;
        public static readonly string? Space = " ";

        public static RuleScenario<string?>[] ValidScenarios =>
        [
            new(nameof(Relative), Relative, true)
        ];

        public static RuleScenario<string?>[] InvalidScenarios =>
        [
            new(nameof(Http),             Http,             false),
            new(nameof(ProtocolRelative), ProtocolRelative, false),
            new(nameof(Null),             Null,             false),
            new(nameof(Space),            Space,            false)
        ];

        public static RuleScenario<string?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }
    public static class IsSsrfSchemeSafe
    {
        public static readonly string? Https = "https://example.com";
        public static readonly string? File = "file:///etc/passwd";
        public static readonly string? Gopher = "gopher://example.com";
        public static readonly string? Null = null;
        public static readonly string? Space = " ";

        public static RuleScenario<string?>[] ValidScenarios =>
        [
            new(nameof(Https), Https, true)
        ];

        public static RuleScenario<string?>[] InvalidScenarios =>
        [
            new(nameof(File),   File,   false),
            new(nameof(Gopher), Gopher, false),
            new(nameof(Null),   Null,   false),
            new(nameof(Space),  Space,  false)
        ];

        public static RuleScenario<string?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }
}
