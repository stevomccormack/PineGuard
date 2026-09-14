using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;
using F = PineGuard.Testing.Fixtures.XmlRulesFixtures;

namespace PineGuard.DataAnnotations.UnitTests;

public static class XmlAttributesTestData
{
    public sealed record ValidCase(string Name, object? Value, bool Expected) : ReturnCase<object?, bool>(Name, Value, Expected);

    private static TheoryData<ValidCase> CommonEdgeCases() =>
    [
        new("null", null, true)
    ];

    public static class XmlString
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new(nameof(F.IsXml.Valid),   F.IsXml.Valid,   true),
            new("decl", "<?xml version=\"1.0\"?><root/>", true)
        ];

        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();

        public static TheoryData<ValidCase> InvalidCases =>
        [
            new(nameof(F.IsXml.Invalid),    F.IsXml.Invalid,    false),
            new(nameof(F.IsXml.Whitespace), F.IsXml.Whitespace, false)
        ];
    }

    public static class HasXmlRoot
    {
        public const string LocalName = F.HasXmlRoot.LocalName;
        public const string Namespace = F.HasXmlRoot.Namespace;

        public static TheoryData<ValidCase> ValidCases =>
        [
            new(nameof(F.HasXmlRoot.Matching), F.HasXmlRoot.Matching, true)
        ];

        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();

        public static TheoryData<ValidCase> InvalidCases =>
        [
            new(nameof(F.HasXmlRoot.WrongName),      F.HasXmlRoot.WrongName,      false),
            new(nameof(F.HasXmlRoot.WrongNamespace),  F.HasXmlRoot.WrongNamespace,  false),
            new(nameof(F.HasXmlRoot.Malformed),       F.HasXmlRoot.Malformed,       false),
            new(nameof(F.HasXmlRoot.Whitespace),      F.HasXmlRoot.Whitespace,      false)
        ];

        public static TheoryData<ValidCase> AnyNamespaceValidCases =>
        [
            new(nameof(F.HasXmlRoot.MatchingNoNamespace), F.HasXmlRoot.MatchingNoNamespace, true),
            new(nameof(F.HasXmlRoot.WrongNamespace),      F.HasXmlRoot.WrongNamespace,      true)
        ];

        public static TheoryData<ValidCase> AnyNamespaceInvalidCases =>
        [
            new(nameof(F.HasXmlRoot.WrongName),  F.HasXmlRoot.WrongName,  false),
            new(nameof(F.HasXmlRoot.Malformed),  F.HasXmlRoot.Malformed,  false),
            new(nameof(F.HasXmlRoot.Whitespace), F.HasXmlRoot.Whitespace, false)
        ];
    }

    public static class XmlContentType
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new(nameof(F.IsXmlContentType.ApplicationXml), F.IsXmlContentType.ApplicationXml, true),
            new(nameof(F.IsXmlContentType.TextXml),        F.IsXmlContentType.TextXml,        true)
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("null", null, true)
        ];

        public static TheoryData<ValidCase> InvalidCases =>
        [
            new(nameof(F.IsXmlContentType.NotXml),       F.IsXmlContentType.NotXml,       false),
            new(nameof(F.IsXmlContentType.MissingHeader),F.IsXmlContentType.MissingHeader, false)
        ];

        public static TheoryData<ThrowsCase> TypeMismatchCases =>
        [
            new("not dictionary (int)", 123, new ExpectedException(typeof(InvalidOperationException), null, "can only be applied to properties implementing")),
            new("string-array values", new Dictionary<string, string[]> { ["Content-Type"] = ["application/json"] }, new ExpectedException(typeof(InvalidOperationException), null, "can only be applied to properties implementing"))
        ];
    }
}
