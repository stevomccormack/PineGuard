using System.ComponentModel.DataAnnotations;
using PineGuard.Codes;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;
using PineGuard.Testing.UnitTests.DataAnnotations;
using Xunit.Abstractions;

namespace PineGuard.Xml.UnitTests;

public sealed class ValidXmlAttributeTests(ITestOutputHelper output)
    : BaseDataAnnotationUnitTest(output)
{
    [Theory]
    [MemberData(nameof(ValidXmlAttributeTestData.ValidXml.Cases), MemberType = typeof(ValidXmlAttributeTestData.ValidXml))]
    public void ValidXml_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attribute = new ValidXmlAttribute();
        var ctx = new ValidationContext(new object(), ValidXmlAttributeTestData.DocumentSchemaProvider, items: null);

        // Act
        var result = attribute.GetValidationResult(tc.Value, ctx);

        // Assert
        Assert.Equal(MustCodes.Xml.Schema.Mismatch, attribute.Code);
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(ValidXmlAttributeTestData.ValidXmlTreatWarningsAsViolations.Cases), MemberType = typeof(ValidXmlAttributeTestData.ValidXmlTreatWarningsAsViolations))]
    public void ValidXmlTreatWarningsAsViolations_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attribute = new ValidXmlAttribute();
        var ctx = new ValidationContext(new object(), ValidXmlAttributeTestData.DocumentSchemaWarningsAsViolationsProvider, items: null);

        // Act
        var result = attribute.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(ValidXmlAttributeTestData.ValidXmlTypeMismatch.Cases), MemberType = typeof(ValidXmlAttributeTestData.ValidXmlTypeMismatch))]
    public void ValidXmlTypeMismatch_ThrowsAsExpected(IThrowsCase tc)
    {
        // Arrange
        var action = ((ThrowsCase<Action>)tc).Value;

        // Act
        var ex = Assert.Throws(tc.ExpectedException.Type, action);

        // Assert
        ThrowsCaseAssert.Expected(ex, tc);
    }

    [Theory]
    [MemberData(nameof(ValidXmlAttributeTestData.ValidXmlMissingSchemaService.Cases), MemberType = typeof(ValidXmlAttributeTestData.ValidXmlMissingSchemaService))]
    public void ValidXmlMissingSchemaService_ThrowsAsExpected(IThrowsCase tc)
    {
        // Arrange
        var action = ((ThrowsCase<Action>)tc).Value;

        // Act
        var ex = Assert.Throws(tc.ExpectedException.Type, action);

        // Assert
        ThrowsCaseAssert.Expected(ex, tc);
    }
}
