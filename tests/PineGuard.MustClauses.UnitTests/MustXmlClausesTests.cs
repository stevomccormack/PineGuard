using PineGuard.Testing.UnitTests;

namespace PineGuard.MustClauses.UnitTests;

public class MustXmlClausesTests : BaseUnitTest
{
    [Theory]
    [MemberData(nameof(MustXmlClausesTestData.Xml.ValidCases), MemberType = typeof(MustXmlClausesTestData.Xml))]
    public void Xml_Checks(MustXmlClausesTestData.Xml.ValidCase testCase)
    {
        var result = Must.Be.Xml(testCase.Input);
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustXmlClausesTestData.XmlContentType.ValidCases), MemberType = typeof(MustXmlClausesTestData.XmlContentType))]
    public void XmlContentType_Checks(MustXmlClausesTestData.XmlContentType.ValidCase testCase)
    {
        var result = Must.Be.XmlContentType(testCase.Input);
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustXmlClausesTestData.XmlDocument.ValidCases), MemberType = typeof(MustXmlClausesTestData.XmlDocument))]
    public void XmlDocument_Checks(MustXmlClausesTestData.XmlDocument.ValidCase testCase)
    {
        var result = Must.Be.XmlDocument(testCase.Input);
        Assert.Equal(testCase.Expected, result.Success);
    }
}
