using PineGuard.Testing.UnitTests.MustClauses;
using Xunit.Abstractions;
using F = PineGuard.Testing.Fixtures.XmlRulesFixtures;

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
    [MemberData(nameof(MustXmlClausesTestData.HasXmlRoot.ValidCases), MemberType = typeof(MustXmlClausesTestData.HasXmlRoot))]
    [MemberData(nameof(MustXmlClausesTestData.HasXmlRoot.InvalidCases), MemberType = typeof(MustXmlClausesTestData.HasXmlRoot))]
    public void HasXmlRoot_BehavesAsExpected(MustCase<string?> tc)
    {
        // Act
        var result = Must.Be.HasXmlRoot(tc.Value, F.HasXmlRoot.LocalName, F.HasXmlRoot.Namespace, paramName: "value");

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustXmlClausesTestData.HasXmlRootAnyNamespace.ValidCases), MemberType = typeof(MustXmlClausesTestData.HasXmlRootAnyNamespace))]
    [MemberData(nameof(MustXmlClausesTestData.HasXmlRootAnyNamespace.InvalidCases), MemberType = typeof(MustXmlClausesTestData.HasXmlRootAnyNamespace))]
    public void HasXmlRootAnyNamespace_BehavesAsExpected(MustCase<string?> tc)
    {
        // Act
        var result = Must.Be.HasXmlRoot(tc.Value, F.HasXmlRoot.LocalName, paramName: "value");

        // Assert
        AssertResult(tc, result);
    }
}
