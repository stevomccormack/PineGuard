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

    public static class HasXmlRoot
    {
        public static TheoryData<MustCase<string?>> ValidCases => F.HasXmlRoot.ValidScenarios.ToMustCases();

        public static TheoryData<MustCase<string?>> InvalidCases => F.HasXmlRoot.InvalidScenarios.ToMustCases(s => s.Name switch
        {
            nameof(F.HasXmlRoot.Null) => new MustExpected(false, "value must not be null.", "value"),
            _ => new MustExpected(false, "value must be XML with root element 'Document' in namespace 'urn:test:doc'.", Code: MustCodes.Xml.Root.Mismatch)
        });
    }

    public static class HasXmlRootAnyNamespace
    {
        public static TheoryData<MustCase<string?>> ValidCases => F.HasXmlRoot.AnyNamespaceValidScenarios.ToMustCases();

        public static TheoryData<MustCase<string?>> InvalidCases => F.HasXmlRoot.AnyNamespaceInvalidScenarios.ToMustCases(s => s.Name switch
        {
            nameof(F.HasXmlRoot.Null) => new MustExpected(false, "value must not be null.", "value"),
            _ => new MustExpected(false, "value must be XML with root element 'Document'.", Code: MustCodes.Xml.Root.Mismatch)
        });
    }
}
