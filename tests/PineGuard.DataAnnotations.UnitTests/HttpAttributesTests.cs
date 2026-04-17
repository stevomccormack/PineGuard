using System.ComponentModel.DataAnnotations;

namespace PineGuard.DataAnnotations.UnitTests;

public sealed class HttpAttributesTests
{
    private static void Verify<TAttribute>(TAttribute attribute, HttpAttributesTestData.ValidCase testCase)
        where TAttribute : ValidationAttribute
    {
        var result = attribute.GetValidationResult(testCase.Value, new ValidationContext(new object()));
        Assert.Equal(testCase.Expected, result == ValidationResult.Success);
    }

    [Theory]
    [MemberData(nameof(HttpAttributesTestData.HttpHeaderName.ValidCases), MemberType = typeof(HttpAttributesTestData.HttpHeaderName))]
    [MemberData(nameof(HttpAttributesTestData.HttpHeaderName.EdgeCases), MemberType = typeof(HttpAttributesTestData.HttpHeaderName))]
    [MemberData(nameof(HttpAttributesTestData.HttpHeaderName.InvalidCases), MemberType = typeof(HttpAttributesTestData.HttpHeaderName))]
    public void HttpHeaderName_ShouldReturnExpected(HttpAttributesTestData.ValidCase testCase)
        => Verify(new HttpHeaderNameAttribute(), testCase);

    [Theory]
    [MemberData(nameof(HttpAttributesTestData.HttpHeaderValue.ValidCases), MemberType = typeof(HttpAttributesTestData.HttpHeaderValue))]
    [MemberData(nameof(HttpAttributesTestData.HttpHeaderValue.EdgeCases), MemberType = typeof(HttpAttributesTestData.HttpHeaderValue))]
    [MemberData(nameof(HttpAttributesTestData.HttpHeaderValue.InvalidCases), MemberType = typeof(HttpAttributesTestData.HttpHeaderValue))]
    public void HttpHeaderValue_ShouldReturnExpected(HttpAttributesTestData.ValidCase testCase)
        => Verify(new HttpHeaderValueAttribute(), testCase);

    [Theory]
    [MemberData(nameof(HttpAttributesTestData.HttpStatusCode.ValidCases), MemberType = typeof(HttpAttributesTestData.HttpStatusCode))]
    [MemberData(nameof(HttpAttributesTestData.HttpStatusCode.EdgeCases), MemberType = typeof(HttpAttributesTestData.HttpStatusCode))]
    [MemberData(nameof(HttpAttributesTestData.HttpStatusCode.InvalidCases), MemberType = typeof(HttpAttributesTestData.HttpStatusCode))]
    public void HttpStatusCode_ShouldReturnExpected(HttpAttributesTestData.ValidCase testCase)
        => Verify(new HttpStatusCodeAttribute(), testCase);

    [Theory]
    [MemberData(nameof(HttpAttributesTestData.HttpStatusSuccess.ValidCases), MemberType = typeof(HttpAttributesTestData.HttpStatusSuccess))]
    [MemberData(nameof(HttpAttributesTestData.HttpStatusSuccess.EdgeCases), MemberType = typeof(HttpAttributesTestData.HttpStatusSuccess))]
    [MemberData(nameof(HttpAttributesTestData.HttpStatusSuccess.InvalidCases), MemberType = typeof(HttpAttributesTestData.HttpStatusSuccess))]
    public void HttpStatusSuccess_ShouldReturnExpected(HttpAttributesTestData.ValidCase testCase)
        => Verify(new HttpStatusSuccessAttribute(), testCase);
}
