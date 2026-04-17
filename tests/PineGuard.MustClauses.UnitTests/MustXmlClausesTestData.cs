using PineGuard.Testing.UnitTests;
using F = PineGuard.Testing.Fixtures.XmlRulesFixtures;

namespace PineGuard.MustClauses.UnitTests;

public static class MustXmlClausesTestData
{
    public static class Xml
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new(nameof(F.IsXml.Valid),   F.IsXml.Valid,   true),
            new(nameof(F.IsXml.Invalid), F.IsXml.Invalid, false),
            new(nameof(F.IsXml.Null),    F.IsXml.Null,    false)
        ];

        public sealed record ValidCase(string Name, string? Input, bool Expected) : IsCase<string?>(Name, Input, Expected);
    }

    public static class XmlContentType
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new(nameof(F.IsXmlContentType.ApplicationXml), F.IsXmlContentType.ApplicationXml, true),
            new(nameof(F.IsXmlContentType.NotXml),         F.IsXmlContentType.NotXml,         false),
            new(nameof(F.IsXmlContentType.NullHeaders),    F.IsXmlContentType.NullHeaders,    false)
        ];

        public sealed record ValidCase(string Name, IReadOnlyDictionary<string, IEnumerable<string>>? Input, bool Expected)
            : IsCase<IReadOnlyDictionary<string, IEnumerable<string>>?>(Name, Input, Expected);
    }

    public static class XmlDocument
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new(nameof(F.IsXml.Valid),   F.IsXml.Valid,   true),
            new(nameof(F.IsXml.Invalid), F.IsXml.Invalid, false),
            new(nameof(F.IsXml.Null),    F.IsXml.Null,    false)
        ];

        public sealed record ValidCase(string Name, string? Input, bool Expected) : IsCase<string?>(Name, Input, Expected);
    }
}
