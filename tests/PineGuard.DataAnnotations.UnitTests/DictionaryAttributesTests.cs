using System.ComponentModel.DataAnnotations;

namespace PineGuard.DataAnnotations.UnitTests;

public sealed class DictionaryAttributesTests
{
    private static void Verify<TAttribute>(TAttribute attribute, DictionaryAttributesTestData.ValidCase testCase)
        where TAttribute : ValidationAttribute
    {
        var result = attribute.GetValidationResult(testCase.Value, new ValidationContext(new object()));
        Assert.Equal(testCase.Expected, result == ValidationResult.Success);
    }

    [Theory]
    [MemberData(nameof(DictionaryAttributesTestData.EmptyDictionary.ValidCases), MemberType = typeof(DictionaryAttributesTestData.EmptyDictionary))]
    [MemberData(nameof(DictionaryAttributesTestData.EmptyDictionary.EdgeCases), MemberType = typeof(DictionaryAttributesTestData.EmptyDictionary))]
    [MemberData(nameof(DictionaryAttributesTestData.EmptyDictionary.InvalidCases), MemberType = typeof(DictionaryAttributesTestData.EmptyDictionary))]
    public void EmptyDictionary_ShouldReturnExpected(DictionaryAttributesTestData.ValidCase testCase)
        => Verify(new EmptyDictionaryAttribute(), testCase);

    [Theory]
    [MemberData(nameof(DictionaryAttributesTestData.NotEmptyDictionary.ValidCases), MemberType = typeof(DictionaryAttributesTestData.NotEmptyDictionary))]
    [MemberData(nameof(DictionaryAttributesTestData.NotEmptyDictionary.EdgeCases), MemberType = typeof(DictionaryAttributesTestData.NotEmptyDictionary))]
    [MemberData(nameof(DictionaryAttributesTestData.NotEmptyDictionary.InvalidCases), MemberType = typeof(DictionaryAttributesTestData.NotEmptyDictionary))]
    public void NotEmptyDictionary_ShouldReturnExpected(DictionaryAttributesTestData.ValidCase testCase)
        => Verify(new NotEmptyDictionaryAttribute(), testCase);
}
