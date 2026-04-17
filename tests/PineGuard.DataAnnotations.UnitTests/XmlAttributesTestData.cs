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

    public static class XmlDocumentString
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("decl", "<?xml version=\"1.0\"?><root/>", true),
            new(nameof(F.IsXml.Valid), F.IsXml.Valid, true)
        ];

        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();

        public static TheoryData<ValidCase> InvalidCases =>
        [
            new(nameof(F.IsXml.Whitespace), F.IsXml.Whitespace, false)
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
            new("null", null, true),
            new("not dictionary", 123, true)
        ];

        public static TheoryData<ValidCase> InvalidCases =>
        [
            new(nameof(F.IsXmlContentType.NotXml),       F.IsXmlContentType.NotXml,       false),
            new(nameof(F.IsXmlContentType.MissingHeader),F.IsXmlContentType.MissingHeader, false)
        ];
    }
}
