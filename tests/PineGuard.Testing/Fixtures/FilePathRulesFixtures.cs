using PineGuard.Testing.UnitTests.Rules;

namespace PineGuard.Testing.Fixtures;

public static class FilePathRulesFixtures
{
    public static class IsSafeFileName
    {
        public static readonly string? Normal = "file.txt";
        public static readonly string? LeadingTrailingSpace = "  file.txt  ";
        public static readonly string? TrailingSpace = "file.txt ";
        public static readonly string? LeadingSpace = " file.txt";
        public static readonly string? Dot = ".";
        public static readonly string? DotDot = "..";
        public static readonly string? EndsWithDot = "file.";
        public static readonly string? InvalidChar = "fi|le.txt";
        public static readonly string? Slash = "in/valid.txt";
        public static readonly string? Backslash = @"in\valid.txt";
        public static readonly string? Colon = "in:valid.txt";
        public static readonly string? Reserved = "CON";
        public static readonly string? Null = null;
        public static readonly string? Empty = "";
        public static readonly string? Whitespace = "  ";

        public static RuleScenario<string?>[] ValidScenarios =>
        [
            new(nameof(Normal), Normal, true)
        ];

        public static RuleScenario<string?>[] InvalidScenarios =>
        [
            new(nameof(LeadingTrailingSpace), LeadingTrailingSpace, false),
            new(nameof(TrailingSpace),  TrailingSpace, false),
            new(nameof(LeadingSpace),   LeadingSpace,  false),
            new(nameof(Dot),        Dot,        false),
            new(nameof(DotDot),     DotDot,     false),
            new(nameof(EndsWithDot),EndsWithDot,false),
            new(nameof(InvalidChar),InvalidChar, false),
            new(nameof(Slash),      Slash,       false),
            new(nameof(Backslash),  Backslash,   false),
            new(nameof(Colon),      Colon,       false),
            new(nameof(Reserved),   Reserved,    false),
            new(nameof(Null),       Null,        false),
            new(nameof(Empty),      Empty,       false),
            new(nameof(Whitespace), Whitespace,  false)
        ];

        public static RuleScenario<string?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class HasFileExtension
    {
        public static readonly (string? path, string[]? allowed) MatchesWithDot = ("file.txt", [".txt"]);
        public static readonly (string? path, string[]? allowed) MatchesWithoutDot = ("file.txt", ["txt"]);
        public static readonly (string? path, string[]? allowed) CaseInsensitive = ("file.TXT", ["txt"]);
        public static readonly (string? path, string[]? allowed) SkipsInvalidCandidates = ("file.txt", ["  ", "txt"]);
        public static readonly (string? path, string[]? allowed) NoMatch = ("file.txt", ["csv"]);
        public static readonly (string? path, string[]? allowed) NullPath = (null, ["txt"]);
        public static readonly (string? path, string[]? allowed) EmptyPath = ("", ["txt"]);
        public static readonly (string? path, string[]? allowed) NullAllowed = ("file.txt", null);
        public static readonly (string? path, string[]? allowed) EmptyAllowed = ("file.txt", []);
        public static readonly (string? path, string[]? allowed) NoExtension = ("file", ["txt"]);

        public static RuleScenario<(string? path, string[]? allowed)>[] ValidScenarios =>
        [
            new(nameof(MatchesWithDot), MatchesWithDot, true),
            new(nameof(MatchesWithoutDot), MatchesWithoutDot, true),
            new(nameof(CaseInsensitive), CaseInsensitive, true),
            new(nameof(SkipsInvalidCandidates), SkipsInvalidCandidates, true)
        ];

        public static RuleScenario<(string? path, string[]? allowed)>[] InvalidScenarios =>
        [
            new(nameof(NoMatch), NoMatch, false),
            new(nameof(NullPath), NullPath, false),
            new(nameof(EmptyPath), EmptyPath, false),
            new(nameof(NullAllowed), NullAllowed, false),
            new(nameof(EmptyAllowed), EmptyAllowed, false),
            new(nameof(NoExtension), NoExtension, false)
        ];

        public static RuleScenario<(string? path, string[]? allowed)>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }
}
