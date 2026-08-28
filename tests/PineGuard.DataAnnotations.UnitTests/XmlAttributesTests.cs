using System.ComponentModel.DataAnnotations;
using PineGuard.Codes;

namespace PineGuard.DataAnnotations.UnitTests;

public sealed class XmlAttributesTests
{
    private static void Verify<TAttribute>(TAttribute attribute, XmlAttributesTestData.ValidCase testCase)
        where TAttribute : ValidationAttribute
    {
        var result = attribute.GetValidationResult(testCase.Value, new ValidationContext(new object()));
        Assert.Equal(testCase.Expected, result == ValidationResult.Success);
    }

    private static void VerifyThrows<TAttribute>(TAttribute attribute, ThrowsCase testCase)
        where TAttribute : ValidationAttribute
        => Assert.Throws<InvalidOperationException>(() => attribute.GetValidationResult(testCase.Value, new ValidationContext(new object())));

    [Theory]
    [MemberData(nameof(XmlAttributesTestData.XmlString.ValidCases), MemberType = typeof(XmlAttributesTestData.XmlString))]
    [MemberData(nameof(XmlAttributesTestData.XmlString.EdgeCases), MemberType = typeof(XmlAttributesTestData.XmlString))]
    [MemberData(nameof(XmlAttributesTestData.XmlString.InvalidCases), MemberType = typeof(XmlAttributesTestData.XmlString))]
    public void XmlString_ShouldReturnExpected(XmlAttributesTestData.ValidCase testCase)
    {
        var attribute = new XmlStringAttribute();
        Assert.Equal(MustCodes.Xml.Document.Invalid, attribute.Code);
        Verify(attribute, testCase);
    }

    [Theory]
    [MemberData(nameof(XmlAttributesTestData.XmlDocumentString.ValidCases), MemberType = typeof(XmlAttributesTestData.XmlDocumentString))]
    [MemberData(nameof(XmlAttributesTestData.XmlDocumentString.EdgeCases), MemberType = typeof(XmlAttributesTestData.XmlDocumentString))]
    [MemberData(nameof(XmlAttributesTestData.XmlDocumentString.InvalidCases), MemberType = typeof(XmlAttributesTestData.XmlDocumentString))]
    public void XmlDocumentString_ShouldReturnExpected(XmlAttributesTestData.ValidCase testCase)
        => Verify(new XmlDocumentStringAttribute(), testCase);

    [Theory]
    [MemberData(nameof(XmlAttributesTestData.XmlContentType.ValidCases), MemberType = typeof(XmlAttributesTestData.XmlContentType))]
    [MemberData(nameof(XmlAttributesTestData.XmlContentType.EdgeCases), MemberType = typeof(XmlAttributesTestData.XmlContentType))]
    [MemberData(nameof(XmlAttributesTestData.XmlContentType.InvalidCases), MemberType = typeof(XmlAttributesTestData.XmlContentType))]
    public void XmlContentType_ShouldReturnExpected(XmlAttributesTestData.ValidCase testCase)
        => Verify(new XmlContentTypeAttribute(), testCase);

    [Theory]
    [MemberData(nameof(XmlAttributesTestData.XmlContentType.TypeMismatchCases), MemberType = typeof(XmlAttributesTestData.XmlContentType))]
    public void XmlContentType_ShouldThrow_WhenNotASupportedDictionary(ThrowsCase testCase)
        => VerifyThrows(new XmlContentTypeAttribute(), testCase);
}
