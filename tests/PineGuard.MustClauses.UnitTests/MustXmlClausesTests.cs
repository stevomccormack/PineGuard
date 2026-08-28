using PineGuard.Testing.UnitTests.MustClauses;
using Xunit.Abstractions;

namespace PineGuard.MustClauses.UnitTests;

public sealed class MustXmlClausesTests(ITestOutputHelper output)
    : BaseMustUnitTest(output)
{
    [Theory]
    [MemberData(nameof(MustXmlClausesTestData.Xml.ValidCases), MemberType = typeof(MustXmlClausesTestData.Xml))]
    [MemberData(nameof(MustXmlClausesTestData.Xml.InvalidCases), MemberType = typeof(MustXmlClausesTestData.Xml))]
    public void Xml_BehavesAsExpected(MustCase<string?> tc)
    {
        // Act
        var result = Must.Be.Xml(tc.Value, paramName: "value");

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustXmlClausesTestData.XmlContentType.ValidCases), MemberType = typeof(MustXmlClausesTestData.XmlContentType))]
    [MemberData(nameof(MustXmlClausesTestData.XmlContentType.InvalidCases), MemberType = typeof(MustXmlClausesTestData.XmlContentType))]
    public void XmlContentType_BehavesAsExpected(MustCase<IReadOnlyDictionary<string, IEnumerable<string>>?> tc)
    {
        // Act
        var result = Must.Be.XmlContentType(tc.Value, paramName: "value");

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustXmlClausesTestData.XmlDocument.ValidCases), MemberType = typeof(MustXmlClausesTestData.XmlDocument))]
    [MemberData(nameof(MustXmlClausesTestData.XmlDocument.InvalidCases), MemberType = typeof(MustXmlClausesTestData.XmlDocument))]
    public void XmlDocument_BehavesAsExpected(MustCase<string?> tc)
    {
        // Act
        var result = Must.Be.XmlDocument(tc.Value, paramName: "value");

        // Assert
        AssertResult(tc, result);
    }
}
