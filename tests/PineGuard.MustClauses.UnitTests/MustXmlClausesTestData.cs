using PineGuard.Codes;
using PineGuard.Testing.UnitTests.MustClauses;
using F = PineGuard.Testing.Fixtures.XmlRulesFixtures;

namespace PineGuard.MustClauses.UnitTests;

public static class MustXmlClausesTestData
{
    public static class Xml
    {
        public static TheoryData<MustCase<string?>> ValidCases => F.IsXml.ValidScenarios.ToMustCases();

        public static TheoryData<MustCase<string?>> InvalidCases => F.IsXml.InvalidScenarios.ToMustCases(s => s.Name switch
        {
            nameof(F.IsXml.Null) => new MustExpected(false, "value must not be null.", "value"),
            _ => new MustExpected(false, "value must be XML.", Code: MustCodes.Xml.Document.Invalid)
        });
    }

    public static class XmlContentType
    {
        public static TheoryData<MustCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> ValidCases => F.IsXmlContentType.ValidScenarios.ToMustCases();

        public static TheoryData<MustCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> InvalidCases => F.IsXmlContentType.InvalidScenarios.ToMustCases(_ => new MustExpected(false, "value must contain an XML Content-Type.", Code: MustCodes.Xml.ContentType.Mismatch));
    }

    public static class XmlDocument
    {
        public static TheoryData<MustCase<string?>> ValidCases => F.IsXml.ValidScenarios.ToMustCases();

        public static TheoryData<MustCase<string?>> InvalidCases => F.IsXml.InvalidScenarios.ToMustCases(s => s.Name switch
        {
            nameof(F.IsXml.Null) => new MustExpected(false, "value must not be null.", "value"),
            _ => new MustExpected(false, "value must be an XML document.", Code: MustCodes.Xml.Document.Invalid)
        });
    }
}
