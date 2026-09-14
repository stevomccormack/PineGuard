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
    [MemberData(nameof(XmlAttributesTestData.HasXmlRoot.ValidCases), MemberType = typeof(XmlAttributesTestData.HasXmlRoot))]
    [MemberData(nameof(XmlAttributesTestData.HasXmlRoot.EdgeCases), MemberType = typeof(XmlAttributesTestData.HasXmlRoot))]
    [MemberData(nameof(XmlAttributesTestData.HasXmlRoot.InvalidCases), MemberType = typeof(XmlAttributesTestData.HasXmlRoot))]
    public void HasXmlRoot_ShouldReturnExpected(XmlAttributesTestData.ValidCase testCase)
    {
        var attribute = new HasXmlRootAttribute(XmlAttributesTestData.HasXmlRoot.LocalName, XmlAttributesTestData.HasXmlRoot.Namespace);
        Assert.Equal(MustCodes.Xml.Root.Mismatch, attribute.Code);
        Verify(attribute, testCase);
    }

    [Theory]
    [MemberData(nameof(XmlAttributesTestData.HasXmlRoot.AnyNamespaceValidCases), MemberType = typeof(XmlAttributesTestData.HasXmlRoot))]
    [MemberData(nameof(XmlAttributesTestData.HasXmlRoot.AnyNamespaceInvalidCases), MemberType = typeof(XmlAttributesTestData.HasXmlRoot))]
    public void HasXmlRoot_ShouldReturnExpected_WhenAnyNamespace(XmlAttributesTestData.ValidCase testCase)
    {
        var attribute = new HasXmlRootAttribute(XmlAttributesTestData.HasXmlRoot.LocalName);
        Assert.Equal(MustCodes.Xml.Root.Mismatch, attribute.Code);
        Verify(attribute, testCase);
    }

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
