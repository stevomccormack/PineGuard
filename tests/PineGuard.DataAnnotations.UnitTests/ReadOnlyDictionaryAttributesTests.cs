using System.ComponentModel.DataAnnotations;

namespace PineGuard.DataAnnotations.UnitTests;

public sealed class ReadOnlyDictionaryAttributesTests
{
    private static void Verify<TAttribute>(TAttribute attribute, ReadOnlyDictionaryAttributesTestData.ValidCase testCase)
        where TAttribute : ValidationAttribute
    {
        var result = attribute.GetValidationResult(testCase.Value, new ValidationContext(new object()));
        Assert.Equal(testCase.Expected, result == ValidationResult.Success);
    }

    private static void VerifyThrows<TAttribute>(TAttribute attribute, ThrowsCase testCase)
        where TAttribute : ValidationAttribute
        => Assert.Throws<InvalidOperationException>(() => attribute.GetValidationResult(testCase.Value, new ValidationContext(new object())));

    [Theory]
    [MemberData(nameof(ReadOnlyDictionaryAttributesTestData.EmptyReadOnlyDictionary.ValidCases), MemberType = typeof(ReadOnlyDictionaryAttributesTestData.EmptyReadOnlyDictionary))]
    [MemberData(nameof(ReadOnlyDictionaryAttributesTestData.EmptyReadOnlyDictionary.EdgeCases), MemberType = typeof(ReadOnlyDictionaryAttributesTestData.EmptyReadOnlyDictionary))]
    [MemberData(nameof(ReadOnlyDictionaryAttributesTestData.EmptyReadOnlyDictionary.InvalidCases), MemberType = typeof(ReadOnlyDictionaryAttributesTestData.EmptyReadOnlyDictionary))]
    public void EmptyReadOnlyDictionary_ShouldReturnExpected(ReadOnlyDictionaryAttributesTestData.ValidCase testCase)
        => Verify(new EmptyReadOnlyDictionaryAttribute(), testCase);

    [Theory]
    [MemberData(nameof(ReadOnlyDictionaryAttributesTestData.NotEmptyReadOnlyDictionary.ValidCases), MemberType = typeof(ReadOnlyDictionaryAttributesTestData.NotEmptyReadOnlyDictionary))]
    [MemberData(nameof(ReadOnlyDictionaryAttributesTestData.NotEmptyReadOnlyDictionary.EdgeCases), MemberType = typeof(ReadOnlyDictionaryAttributesTestData.NotEmptyReadOnlyDictionary))]
    [MemberData(nameof(ReadOnlyDictionaryAttributesTestData.NotEmptyReadOnlyDictionary.InvalidCases), MemberType = typeof(ReadOnlyDictionaryAttributesTestData.NotEmptyReadOnlyDictionary))]
    public void NotEmptyReadOnlyDictionary_ShouldReturnExpected(ReadOnlyDictionaryAttributesTestData.ValidCase testCase)
        => Verify(new NotEmptyReadOnlyDictionaryAttribute(), testCase);

    [Theory]
    [MemberData(nameof(ReadOnlyDictionaryAttributesTestData.TypeMismatchCases), MemberType = typeof(ReadOnlyDictionaryAttributesTestData))]
    public void EmptyReadOnlyDictionary_ShouldThrow_WhenNotADictionary(ThrowsCase testCase)
        => VerifyThrows(new EmptyReadOnlyDictionaryAttribute(), testCase);

    [Theory]
    [MemberData(nameof(ReadOnlyDictionaryAttributesTestData.TypeMismatchCases), MemberType = typeof(ReadOnlyDictionaryAttributesTestData))]
    public void NotEmptyReadOnlyDictionary_ShouldThrow_WhenNotADictionary(ThrowsCase testCase)
        => VerifyThrows(new NotEmptyReadOnlyDictionaryAttribute(), testCase);
}
