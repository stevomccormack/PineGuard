using PineGuard.Testing.UnitTests.Rules;

namespace PineGuard.Testing.Fixtures;

public static class JsonRulesFixtures
{
    public static class IsJson
    {
        public static readonly string? Object = "{}";
        public static readonly string? Array = "[]";
        public static readonly string? String = "\"x\"";
        public static readonly string? Number = "123";
        public static readonly string? Invalid = "{";
        public static readonly string? Null = null;
        public static readonly string? Whitespace = "  ";

        public static RuleScenario<string?>[] ValidScenarios =>
        [
            new(nameof(Object),    Object,    true),
            new(nameof(Array),     Array,     true),
            new(nameof(String),    String,    true),
            new(nameof(Number),    Number,    true)
        ];

        public static RuleScenario<string?>[] InvalidScenarios =>
        [
            new(nameof(Invalid),    Invalid,    false),
            new(nameof(Null),       Null,       false),
            new(nameof(Whitespace), Whitespace, false)
        ];

        public static RuleScenario<string?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsJsonObject
    {
        public static readonly string? Object = "{}";
        public static readonly string? Array = "[]";
        public static readonly string? Invalid = "{";
        public static readonly string? Null = null;

        public static RuleScenario<string?>[] ValidScenarios =>
        [
            new(nameof(Object), Object, true)
        ];

        public static RuleScenario<string?>[] InvalidScenarios =>
        [
            new(nameof(Array),   Array,   false),
            new(nameof(Invalid), Invalid, false),
            new(nameof(Null),    Null,    false)
        ];

        public static RuleScenario<string?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsJsonArray
    {
        public static readonly string? Array = "[]";
        public static readonly string? Object = "{}";
        public static readonly string? Invalid = "{";
        public static readonly string? Null = null;

        public static RuleScenario<string?>[] ValidScenarios =>
        [
            new(nameof(Array), Array, true)
        ];

        public static RuleScenario<string?>[] InvalidScenarios =>
        [
            new(nameof(Object),  Object,  false),
            new(nameof(Invalid), Invalid, false),
            new(nameof(Null),    Null,    false)
        ];

        public static RuleScenario<string?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsJsonContentType
    {
        public static readonly IReadOnlyDictionary<string, IEnumerable<string>>? NullHeaders = null;

        public static readonly IReadOnlyDictionary<string, IEnumerable<string>> ApplicationJson =
            new Dictionary<string, IEnumerable<string>> { ["Content-Type"] = ["application/json"] };

        public static readonly IReadOnlyDictionary<string, IEnumerable<string>> SuffixJson =
            new Dictionary<string, IEnumerable<string>> { ["Content-Type"] = ["application/vnd.github+json"] };

        public static readonly IReadOnlyDictionary<string, IEnumerable<string>> NotJson =
            new Dictionary<string, IEnumerable<string>> { ["Content-Type"] = ["text/plain"] };

        public static readonly IReadOnlyDictionary<string, IEnumerable<string>> MissingHeader =
            new Dictionary<string, IEnumerable<string>> { ["X"] = ["y"] };

        public static RuleScenario<IReadOnlyDictionary<string, IEnumerable<string>>?>[] ValidScenarios =>
        [
            new(nameof(ApplicationJson), ApplicationJson, true),
            new(nameof(SuffixJson),      SuffixJson,      true)
        ];

        public static RuleScenario<IReadOnlyDictionary<string, IEnumerable<string>>?>[] InvalidScenarios =>
        [
            new(nameof(NotJson),       NotJson,       false),
            new(nameof(MissingHeader), MissingHeader, false),
            new(nameof(NullHeaders),   NullHeaders,   false)
        ];

        public static RuleScenario<IReadOnlyDictionary<string, IEnumerable<string>>?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }
}
