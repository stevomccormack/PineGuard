using PineGuard.Codes;
using PineGuard.Testing.UnitTests.FluentValidation;
using F = PineGuard.Testing.Fixtures.XmlRulesFixtures;

namespace PineGuard.FluentValidation.UnitTests;

public static class FluentXmlExtensionsTestData
{
    public static class Xml
    {
        public static TheoryData<FluentCase<string?>> Cases => F.IsXml.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsXml.Null) => new FluentExpected(false, "Value must not be null."),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be XML.", Code: MustCodes.Xml.Document.Invalid)
        });
    }

    public static class XmlContentType
    {
        public static TheoryData<FluentCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> Cases => F.IsXmlContentType.AllScenarios.ToFluentCases(s => s.Name switch
        {
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must contain an XML Content-Type.")
        });
    }

    public static class HasXmlRoot
    {
        public static TheoryData<FluentCase<string?>> Cases => F.HasXmlRoot.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.HasXmlRoot.Null) => new FluentExpected(false, "Value must not be null."),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be XML with root element 'Document' in namespace 'urn:test:doc'.", Code: MustCodes.Xml.Root.Mismatch)
        });
    }

    public static class HasXmlRootAnyNamespace
    {
        public static TheoryData<FluentCase<string?>> Cases => F.HasXmlRoot.AnyNamespaceAllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.HasXmlRoot.Null) => new FluentExpected(false, "Value must not be null."),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be XML with root element 'Document'.", Code: MustCodes.Xml.Root.Mismatch)
        });
    }
}
