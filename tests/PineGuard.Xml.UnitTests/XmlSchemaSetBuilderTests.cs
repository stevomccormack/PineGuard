using System.Xml.Schema;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;
using PineGuard.Xml.UnitTests.Schemas;
using Xunit.Abstractions;

namespace PineGuard.Xml.UnitTests;

public sealed class XmlSchemaSetBuilderTests(ITestOutputHelper output)
    : BaseUnitTest(output)
{
    [Theory]
    [MemberData(nameof(XmlSchemaSetBuilderTestData.AddFile.ValidCases), MemberType = typeof(XmlSchemaSetBuilderTestData.AddFile))]
    public void AddFile_BehavesAsExpected(XmlSchemaSetBuilderTestData.AddFile.ValidCase tc)
    {
        // Act
        var schemas = tc.Value();

        // Assert
        AssertSet(tc.Expected, schemas);
    }

    [Theory]
    [MemberData(nameof(XmlSchemaSetBuilderTestData.AddFile.InvalidCases), MemberType = typeof(XmlSchemaSetBuilderTestData.AddFile))]
    public void AddFile_ThrowsAsExpected(IThrowsCase tc)
    {
        // Arrange
        var t = (XmlSchemaSetBuilderTestData.AddFile.InvalidCase)tc;

        // Act & Assert
        var ex = Assert.Throws(tc.ExpectedException.Type, () => new XmlSchemaSetBuilder().AddFile(t.Value!));
        ThrowsCaseAssert.Expected(ex, tc);
    }

    [Theory]
    [MemberData(nameof(XmlSchemaSetBuilderTestData.AddStream.ValidCases), MemberType = typeof(XmlSchemaSetBuilderTestData.AddStream))]
    public void AddStream_BehavesAsExpected(XmlSchemaSetBuilderTestData.AddStream.ValidCase tc)
    {
        // Act
        var schemas = tc.Value();

        // Assert
        AssertSet(tc.Expected, schemas);
    }

    [Theory]
    [MemberData(nameof(XmlSchemaSetBuilderTestData.AddStream.InvalidCases), MemberType = typeof(XmlSchemaSetBuilderTestData.AddStream))]
    public void AddStream_ThrowsAsExpected(IThrowsCase tc)
    {
        // Arrange
        var t = (XmlSchemaSetBuilderTestData.AddStream.InvalidCase)tc;

        // Act & Assert
        var ex = Assert.Throws(tc.ExpectedException.Type, () => new XmlSchemaSetBuilder().AddStream(t.Value!));
        ThrowsCaseAssert.Expected(ex, tc);
    }

    [Theory]
    [MemberData(nameof(XmlSchemaSetBuilderTestData.AddReader.ValidCases), MemberType = typeof(XmlSchemaSetBuilderTestData.AddReader))]
    public void AddReader_BehavesAsExpected(XmlSchemaSetBuilderTestData.AddReader.ValidCase tc)
    {
        // Act
        var schemas = tc.Value();

        // Assert
        AssertSet(tc.Expected, schemas);
    }

    [Theory]
    [MemberData(nameof(XmlSchemaSetBuilderTestData.AddReader.InvalidCases), MemberType = typeof(XmlSchemaSetBuilderTestData.AddReader))]
    public void AddReader_ThrowsAsExpected(IThrowsCase tc)
    {
        // Arrange
        var t = (XmlSchemaSetBuilderTestData.AddReader.InvalidCase)tc;

        // Act & Assert
        var ex = Assert.Throws(tc.ExpectedException.Type, () => new XmlSchemaSetBuilder().AddReader(t.Value!));
        ThrowsCaseAssert.Expected(ex, tc);
    }

    [Theory]
    [MemberData(nameof(XmlSchemaSetBuilderTestData.AddText.ValidCases), MemberType = typeof(XmlSchemaSetBuilderTestData.AddText))]
    public void AddText_BehavesAsExpected(XmlSchemaSetBuilderTestData.AddText.ValidCase tc)
    {
        // Act
        var schemas = tc.Value();

        // Assert
        AssertSet(tc.Expected, schemas);
    }

    [Theory]
    [MemberData(nameof(XmlSchemaSetBuilderTestData.AddText.InvalidCases), MemberType = typeof(XmlSchemaSetBuilderTestData.AddText))]
    public void AddText_ThrowsAsExpected(IThrowsCase tc)
    {
        // Arrange
        var t = (XmlSchemaSetBuilderTestData.AddText.InvalidCase)tc;

        // Act & Assert
        var ex = Assert.Throws(tc.ExpectedException.Type, () => new XmlSchemaSetBuilder().AddText(t.Value!));
        ThrowsCaseAssert.Expected(ex, tc);
    }

    [Theory]
    [MemberData(nameof(XmlSchemaSetBuilderTestData.Build.InvalidCases), MemberType = typeof(XmlSchemaSetBuilderTestData.Build))]
    public void Build_ThrowsAsExpected(IThrowsCase tc)
    {
        // Arrange
        var t = (XmlSchemaSetBuilderTestData.Build.InvalidCase)tc;

        // Act & Assert
        var ex = Assert.Throws(tc.ExpectedException.Type, t.Value);
        ThrowsCaseAssert.Expected(ex, tc);
    }

    private static void AssertSet((bool containsDocument, bool containsOther, bool isCompiled) expected, XmlSchemaSet schemas)
    {
        Assert.Equal(expected.containsDocument, schemas.Contains(TestSchemas.DocumentNamespace));
        Assert.Equal(expected.containsOther, schemas.Contains(TestSchemas.OtherNamespace));
        Assert.Equal(expected.isCompiled, schemas.IsCompiled);
    }
}
