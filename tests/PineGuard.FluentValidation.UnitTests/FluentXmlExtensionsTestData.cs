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

    public static class XmlDocument
    {
        public static TheoryData<FluentCase<string?>> Cases => F.IsXml.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsXml.Null) => new FluentExpected(false, "Value must not be null."),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be an XML document.")
        });
    }
}
