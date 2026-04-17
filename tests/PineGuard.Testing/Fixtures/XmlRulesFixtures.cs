using PineGuard.Testing.UnitTests.Rules;

namespace PineGuard.Testing.Fixtures;

public static class XmlRulesFixtures
{
    public static class IsXml
    {
        public static readonly string? Valid = "<root />";
        public static readonly string? Invalid = "<root>";
        public static readonly string? Null = null;
        public static readonly string? Whitespace = "  ";

        public static RuleScenario<string?>[] ValidScenarios => [new(nameof(Valid), Valid, true)];

        public static RuleScenario<string?>[] InvalidScenarios =>
        [
            new(nameof(Invalid), Invalid, false),
            new(nameof(Null), Null, false),
            new(nameof(Whitespace), Whitespace, false)
        ];

        public static RuleScenario<string?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsXmlContentType
    {
        public static readonly IReadOnlyDictionary<string, IEnumerable<string>> ApplicationXml =
            new Dictionary<string, IEnumerable<string>> { ["Content-Type"] = ["application/xml"] };

        public static readonly IReadOnlyDictionary<string, IEnumerable<string>> TextXml =
            new Dictionary<string, IEnumerable<string>> { ["Content-Type"] = ["text/xml"] };

        public static readonly IReadOnlyDictionary<string, IEnumerable<string>> SuffixXml =
            new Dictionary<string, IEnumerable<string>> { ["Content-Type"] = ["application/vnd.test+xml"] };

        public static readonly IReadOnlyDictionary<string, IEnumerable<string>> NotXml =
            new Dictionary<string, IEnumerable<string>> { ["Content-Type"] = ["application/json"] };

        public static readonly IReadOnlyDictionary<string, IEnumerable<string>> MissingHeader =
            new Dictionary<string, IEnumerable<string>> { ["X"] = ["y"] };

        public static readonly IReadOnlyDictionary<string, IEnumerable<string>>? NullHeaders = null;

        public static RuleScenario<IReadOnlyDictionary<string, IEnumerable<string>>?>[] ValidScenarios =>
        [
            new(nameof(ApplicationXml), ApplicationXml, true),
            new(nameof(TextXml), TextXml, true),
            new(nameof(SuffixXml), SuffixXml, true)
        ];

        public static RuleScenario<IReadOnlyDictionary<string, IEnumerable<string>>?>[] InvalidScenarios =>
        [
            new(nameof(NotXml), NotXml, false),
            new(nameof(MissingHeader), MissingHeader, false),
            new(nameof(NullHeaders), NullHeaders, false)
        ];

        public static RuleScenario<IReadOnlyDictionary<string, IEnumerable<string>>?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }
}
