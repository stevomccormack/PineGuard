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
        public static readonly string? Namespaced = "<d:Document xmlns:d=\"urn:test:doc\"><d:Id>1</d:Id></d:Document>";
        public static readonly string? WithDeclaration = "<?xml version=\"1.0\" encoding=\"utf-8\"?><root/>";
        public static readonly string? MultipleRoots = "<a/><b/>";
        public static readonly string? Doctype = "<!DOCTYPE root [<!ELEMENT root ANY>]><root/>";
        public static readonly string? Fragment = "text only";

        public static RuleScenario<string?>[] ValidScenarios =>
        [
            new(nameof(Valid), Valid, true),
            new(nameof(Namespaced), Namespaced, true),
            new(nameof(WithDeclaration), WithDeclaration, true)
        ];

        public static RuleScenario<string?>[] InvalidScenarios =>
        [
            new(nameof(Invalid), Invalid, false),
            new(nameof(Null), Null, false),
            new(nameof(Whitespace), Whitespace, false),
            new(nameof(MultipleRoots), MultipleRoots, false),
            new(nameof(Doctype), Doctype, false),
            new(nameof(Fragment), Fragment, false)
        ];

        public static RuleScenario<string?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class HasXmlRoot
    {
        public const string LocalName = "Document";
        public const string Namespace = "urn:test:doc";

        public static readonly string? Matching = "<Document xmlns=\"urn:test:doc\"><Id>1</Id></Document>";
        public static readonly string? MatchingNoNamespace = "<Document/>";
        public static readonly string? WrongName = "<Envelope xmlns=\"urn:test:doc\"/>";
        public static readonly string? WrongNamespace = "<Document xmlns=\"urn:other\"/>";
        public static readonly string? Malformed = "<Document";
        public static readonly string? Null = null;
        public static readonly string? Whitespace = "  ";

        public static RuleScenario<string?>[] ValidScenarios => [new(nameof(Matching), Matching, true)];

        public static RuleScenario<string?>[] InvalidScenarios =>
        [
            new(nameof(MatchingNoNamespace), MatchingNoNamespace, false),
            new(nameof(WrongName), WrongName, false),
            new(nameof(WrongNamespace), WrongNamespace, false),
            new(nameof(Malformed), Malformed, false),
            new(nameof(Null), Null, false),
            new(nameof(Whitespace), Whitespace, false)
        ];

        public static RuleScenario<string?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];

        public static RuleScenario<string?>[] AnyNamespaceValidScenarios =>
        [
            new(nameof(Matching), Matching, true),
            new(nameof(MatchingNoNamespace), MatchingNoNamespace, true),
            new(nameof(WrongNamespace), WrongNamespace, true)
        ];

        public static RuleScenario<string?>[] AnyNamespaceInvalidScenarios =>
        [
            new(nameof(WrongName), WrongName, false),
            new(nameof(Malformed), Malformed, false),
            new(nameof(Null), Null, false),
            new(nameof(Whitespace), Whitespace, false)
        ];

        public static RuleScenario<string?>[] AnyNamespaceAllScenarios => [.. AnyNamespaceValidScenarios, .. AnyNamespaceInvalidScenarios];
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
