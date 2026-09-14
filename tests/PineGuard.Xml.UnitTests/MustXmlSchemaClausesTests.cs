using System.Xml.Schema;
using PineGuard.MustClauses;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;
using PineGuard.Testing.UnitTests.MustClauses;
using PineGuard.Xml.UnitTests.Schemas;
using Xunit.Abstractions;

namespace PineGuard.Xml.UnitTests;

public sealed class MustXmlSchemaClausesTests(ITestOutputHelper output)
    : BaseMustUnitTest(output)
{
    [Theory]
    [MemberData(nameof(MustXmlSchemaClausesTestData.ValidXml.ValidCases), MemberType = typeof(MustXmlSchemaClausesTestData.ValidXml))]
    [MemberData(nameof(MustXmlSchemaClausesTestData.ValidXml.InvalidCases), MemberType = typeof(MustXmlSchemaClausesTestData.ValidXml))]
    public void ValidXml_BehavesAsExpected(MustCase<string?> tc)
    {
        // Act
        var result = Must.Be.ValidXml(tc.Value, TestSchemas.DocumentSet, paramName: "value");

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustXmlSchemaClausesTestData.ValidXmlWithOptions.InvalidCases), MemberType = typeof(MustXmlSchemaClausesTestData.ValidXmlWithOptions))]
    public void ValidXmlWithOptions_BehavesAsExpected(MustCase<(string? value, XmlSchemaValidationOptions? options)> tc)
    {
        // Arrange
        var (value, options) = tc.Value;

        // Act
        var result = Must.Be.ValidXml(value, TestSchemas.DocumentSet, options, paramName: "value");

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustXmlSchemaClausesTestData.ValidXmlNullSchemas.InvalidCases), MemberType = typeof(MustXmlSchemaClausesTestData.ValidXmlNullSchemas))]
    public void ValidXmlNullSchemas_ThrowsAsExpected(IThrowsCase tc)
    {
        // Arrange
        var t = (MustXmlSchemaClausesTestData.ValidXmlNullSchemas.InvalidCase)tc;

        // Act & Assert
        var ex = Assert.Throws(tc.ExpectedException.Type, () => Must.Be.ValidXml(t.Value, null!, paramName: "value"));
        ThrowsCaseAssert.Expected(ex, tc);
    }
}
