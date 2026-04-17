using System.ComponentModel.DataAnnotations;

namespace PineGuard.DataAnnotations.UnitTests;

public sealed class CollectionAttributesTests
{
    private static void Verify<TAttribute>(TAttribute attribute, CollectionAttributesTestData.ValidCase testCase)
        where TAttribute : ValidationAttribute
    {
        var result = attribute.GetValidationResult(testCase.Value, new ValidationContext(new object()));
        Assert.Equal(testCase.Expected, result == ValidationResult.Success);
    }

    [Theory]
    [MemberData(nameof(CollectionAttributesTestData.EmptyCollection.ValidCases), MemberType = typeof(CollectionAttributesTestData.EmptyCollection))]
    [MemberData(nameof(CollectionAttributesTestData.EmptyCollection.EdgeCases), MemberType = typeof(CollectionAttributesTestData.EmptyCollection))]
    [MemberData(nameof(CollectionAttributesTestData.EmptyCollection.InvalidCases), MemberType = typeof(CollectionAttributesTestData.EmptyCollection))]
    public void EmptyCollection_ShouldReturnExpected(CollectionAttributesTestData.ValidCase testCase)
        => Verify(new EmptyCollectionAttribute(), testCase);

    [Theory]
    [MemberData(nameof(CollectionAttributesTestData.NotEmptyCollection.ValidCases), MemberType = typeof(CollectionAttributesTestData.NotEmptyCollection))]
    [MemberData(nameof(CollectionAttributesTestData.NotEmptyCollection.EdgeCases), MemberType = typeof(CollectionAttributesTestData.NotEmptyCollection))]
    [MemberData(nameof(CollectionAttributesTestData.NotEmptyCollection.InvalidCases), MemberType = typeof(CollectionAttributesTestData.NotEmptyCollection))]
    public void NotEmptyCollection_ShouldReturnExpected(CollectionAttributesTestData.ValidCase testCase)
        => Verify(new NotEmptyCollectionAttribute(), testCase);

    [Theory]
    [MemberData(nameof(CollectionAttributesTestData.HasExactCountCollection.ValidCases), MemberType = typeof(CollectionAttributesTestData.HasExactCountCollection))]
    [MemberData(nameof(CollectionAttributesTestData.HasExactCountCollection.EdgeCases), MemberType = typeof(CollectionAttributesTestData.HasExactCountCollection))]
    [MemberData(nameof(CollectionAttributesTestData.HasExactCountCollection.InvalidCases), MemberType = typeof(CollectionAttributesTestData.HasExactCountCollection))]
    public void HasExactCountCollection_ShouldReturnExpected(CollectionAttributesTestData.ValidCase testCase)
        => Verify(new HasExactCountCollectionAttribute(2), testCase);

    [Theory]
    [MemberData(nameof(CollectionAttributesTestData.HasMinCountCollection.ValidCases), MemberType = typeof(CollectionAttributesTestData.HasMinCountCollection))]
    [MemberData(nameof(CollectionAttributesTestData.HasMinCountCollection.EdgeCases), MemberType = typeof(CollectionAttributesTestData.HasMinCountCollection))]
    [MemberData(nameof(CollectionAttributesTestData.HasMinCountCollection.InvalidCases), MemberType = typeof(CollectionAttributesTestData.HasMinCountCollection))]
    public void HasMinCountCollection_ShouldReturnExpected(CollectionAttributesTestData.ValidCase testCase)
        => Verify(new HasMinCountCollectionAttribute(2), testCase);

    [Theory]
    [MemberData(nameof(CollectionAttributesTestData.HasMaxCountCollection.ValidCases), MemberType = typeof(CollectionAttributesTestData.HasMaxCountCollection))]
    [MemberData(nameof(CollectionAttributesTestData.HasMaxCountCollection.EdgeCases), MemberType = typeof(CollectionAttributesTestData.HasMaxCountCollection))]
    [MemberData(nameof(CollectionAttributesTestData.HasMaxCountCollection.InvalidCases), MemberType = typeof(CollectionAttributesTestData.HasMaxCountCollection))]
    public void HasMaxCountCollection_ShouldReturnExpected(CollectionAttributesTestData.ValidCase testCase)
        => Verify(new HasMaxCountCollectionAttribute(2), testCase);

    [Theory]
    [MemberData(nameof(CollectionAttributesTestData.HasCountBetweenCollection.ValidCases), MemberType = typeof(CollectionAttributesTestData.HasCountBetweenCollection))]
    [MemberData(nameof(CollectionAttributesTestData.HasCountBetweenCollection.EdgeCases), MemberType = typeof(CollectionAttributesTestData.HasCountBetweenCollection))]
    [MemberData(nameof(CollectionAttributesTestData.HasCountBetweenCollection.InvalidCases), MemberType = typeof(CollectionAttributesTestData.HasCountBetweenCollection))]
    public void HasCountBetweenCollection_ShouldReturnExpected(CollectionAttributesTestData.ValidCase testCase)
        => Verify(new HasCountBetweenCollectionAttribute(1, 3), testCase);

    [Theory]
    [MemberData(nameof(CollectionAttributesTestData.HasDistinctItemsCollection.ValidCases), MemberType = typeof(CollectionAttributesTestData.HasDistinctItemsCollection))]
    [MemberData(nameof(CollectionAttributesTestData.HasDistinctItemsCollection.EdgeCases), MemberType = typeof(CollectionAttributesTestData.HasDistinctItemsCollection))]
    [MemberData(nameof(CollectionAttributesTestData.HasDistinctItemsCollection.InvalidCases), MemberType = typeof(CollectionAttributesTestData.HasDistinctItemsCollection))]
    public void HasDistinctItemsCollection_ShouldReturnExpected(CollectionAttributesTestData.ValidCase testCase)
        => Verify(new HasDistinctItemsCollectionAttribute(), testCase);

    [Theory]
    [MemberData(nameof(CollectionAttributesTestData.HasDuplicateItemsCollection.ValidCases), MemberType = typeof(CollectionAttributesTestData.HasDuplicateItemsCollection))]
    [MemberData(nameof(CollectionAttributesTestData.HasDuplicateItemsCollection.EdgeCases), MemberType = typeof(CollectionAttributesTestData.HasDuplicateItemsCollection))]
    [MemberData(nameof(CollectionAttributesTestData.HasDuplicateItemsCollection.InvalidCases), MemberType = typeof(CollectionAttributesTestData.HasDuplicateItemsCollection))]
    public void HasDuplicateItemsCollection_ShouldReturnExpected(CollectionAttributesTestData.ValidCase testCase)
        => Verify(new HasDuplicateItemsCollectionAttribute(), testCase);
}
