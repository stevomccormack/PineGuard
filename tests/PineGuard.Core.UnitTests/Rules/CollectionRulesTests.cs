using PineGuard.Rules;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Rules;

public sealed class CollectionRulesTests : BaseUnitTest
{
    [Fact]
    public void HasCountBetween_ReturnsFalse_WhenExclusiveMaxIsZero_ForNonCountableEnumerable()
    {
        // Arrange
        var value = CollectionRulesTestData.Enumerate(1);

        // Act
        var result = CollectionRules.HasCountBetween(value, 0, 0, PineGuard.Common.Inclusion.Exclusive);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void HasCountBetween_ReturnsFalse_WhenEnumerableExceedsUpperBound_ForNonCountableEnumerable()
    {
        // Arrange
        var value = CollectionRulesTestData.Enumerate(1, 2, 3);

        // Act
        var result = CollectionRules.HasCountBetween(value, 0, 1, PineGuard.Common.Inclusion.Inclusive);

        // Assert
        Assert.False(result);
    }

    [Theory]
    [MemberData(nameof(CollectionRulesTestData.IsEmpty.ValidCases), MemberType = typeof(CollectionRulesTestData.IsEmpty))]
    public void IsEmpty_ReturnsTrue_ForEmptyCollections(CollectionRulesTestData.IsEmpty.Case testCase)
    {
        // Arrange
        var value = testCase.ValueFactory();

        // Act
        var result = CollectionRules.IsEmpty(value);

        // Assert
        Assert.Equal(testCase.Expected, result);
        Assert.True(result);
    }

    [Theory]
    [MemberData(nameof(CollectionRulesTestData.IsEmpty.EdgeCases), MemberType = typeof(CollectionRulesTestData.IsEmpty))]
    public void IsEmpty_ReturnsFalse_ForNonEmptyOrNullCollections(CollectionRulesTestData.IsEmpty.Case testCase)
    {
        // Arrange
        var value = testCase.ValueFactory();

        // Act
        var result = CollectionRules.IsEmpty(value);

        // Assert
        Assert.Equal(testCase.Expected, result);
        Assert.False(result);
    }

    [Theory]
    [MemberData(nameof(CollectionRulesTestData.HasItems.ValidCases), MemberType = typeof(CollectionRulesTestData.HasItems))]
    public void HasItems_ReturnsTrue_ForNonEmptyCollections(CollectionRulesTestData.HasItems.Case testCase)
    {
        // Arrange
        var value = testCase.ValueFactory();

        // Act
        var result = CollectionRules.HasItems(value);

        // Assert
        Assert.Equal(testCase.Expected, result);
        Assert.True(result);
    }

    [Theory]
    [MemberData(nameof(CollectionRulesTestData.HasItems.EdgeCases), MemberType = typeof(CollectionRulesTestData.HasItems))]
    public void HasItems_ReturnsFalse_ForEmptyOrNullCollections(CollectionRulesTestData.HasItems.Case testCase)
    {
        // Arrange
        var value = testCase.ValueFactory();

        // Act
        var result = CollectionRules.HasItems(value);

        // Assert
        Assert.Equal(testCase.Expected, result);
        Assert.False(result);
    }

    [Theory]
    [MemberData(nameof(CollectionRulesTestData.HasExactCount.ValidCases), MemberType = typeof(CollectionRulesTestData.HasExactCount))]
    public void HasExactCount_ReturnsTrue_ForMatchingCounts(CollectionRulesTestData.HasExactCount.Case testCase)
    {
        // Arrange
        var value = testCase.ValueFactory();

        // Act
        var result = CollectionRules.HasExactCount(value, testCase.Count);

        // Assert
        Assert.Equal(testCase.Expected, result);
        Assert.True(result);
    }

    [Theory]
    [MemberData(nameof(CollectionRulesTestData.HasExactCount.EdgeCases), MemberType = typeof(CollectionRulesTestData.HasExactCount))]
    public void HasExactCount_ReturnsFalse_ForNonMatchingCounts(CollectionRulesTestData.HasExactCount.Case testCase)
    {
        // Arrange
        var value = testCase.ValueFactory();

        // Act
        var result = CollectionRules.HasExactCount(value, testCase.Count);

        // Assert
        Assert.Equal(testCase.Expected, result);
        Assert.False(result);
    }

    [Theory]
    [MemberData(nameof(CollectionRulesTestData.HasMinCount.ValidCases), MemberType = typeof(CollectionRulesTestData.HasMinCount))]
    public void HasMinCount_ReturnsTrue_ForEnoughItems(CollectionRulesTestData.HasMinCount.Case testCase)
    {
        // Arrange
        var value = testCase.ValueFactory();

        // Act
        var result = CollectionRules.HasMinCount(value, testCase.Min);

        // Assert
        Assert.Equal(testCase.Expected, result);
        Assert.True(result);
    }

    [Theory]
    [MemberData(nameof(CollectionRulesTestData.HasMinCount.EdgeCases), MemberType = typeof(CollectionRulesTestData.HasMinCount))]
    public void HasMinCount_ReturnsFalse_ForNotEnoughItems(CollectionRulesTestData.HasMinCount.Case testCase)
    {
        // Arrange
        var value = testCase.ValueFactory();

        // Act
        var result = CollectionRules.HasMinCount(value, testCase.Min);

        // Assert
        Assert.Equal(testCase.Expected, result);
        Assert.False(result);
    }

    [Theory]
    [MemberData(nameof(CollectionRulesTestData.HasMaxCount.ValidCases), MemberType = typeof(CollectionRulesTestData.HasMaxCount))]
    public void HasMaxCount_ReturnsTrue_WhenWithinMax(CollectionRulesTestData.HasMaxCount.Case testCase)
    {
        // Arrange
        var value = testCase.ValueFactory();

        // Act
        var result = CollectionRules.HasMaxCount(value, testCase.Max);

        // Assert
        Assert.Equal(testCase.Expected, result);
        Assert.True(result);
    }

    [Theory]
    [MemberData(nameof(CollectionRulesTestData.HasMaxCount.EdgeCases), MemberType = typeof(CollectionRulesTestData.HasMaxCount))]
    public void HasMaxCount_ReturnsFalse_WhenExceedsMax(CollectionRulesTestData.HasMaxCount.Case testCase)
    {
        // Arrange
        var value = testCase.ValueFactory();

        // Act
        var result = CollectionRules.HasMaxCount(value, testCase.Max);

        // Assert
        Assert.Equal(testCase.Expected, result);
        Assert.False(result);
    }

    [Theory]
    [MemberData(nameof(CollectionRulesTestData.HasCountBetween.ValidCases), MemberType = typeof(CollectionRulesTestData.HasCountBetween))]
    public void HasCountBetween_ReturnsTrue_ForMatchingCounts(CollectionRulesTestData.HasCountBetween.Case testCase)
    {
        // Arrange
        var value = testCase.ValueFactory();

        // Act
        var result = CollectionRules.HasCountBetween(value, testCase.Min, testCase.Max, testCase.Inclusion);

        // Assert
        Assert.Equal(testCase.Expected, result);
        Assert.True(result);
    }

    [Theory]
    [MemberData(nameof(CollectionRulesTestData.HasCountBetween.EdgeCases), MemberType = typeof(CollectionRulesTestData.HasCountBetween))]
    public void HasCountBetween_ReturnsFalse_ForNonMatchingCounts(CollectionRulesTestData.HasCountBetween.Case testCase)
    {
        // Arrange
        var value = testCase.ValueFactory();

        // Act
        var result = CollectionRules.HasCountBetween(value, testCase.Min, testCase.Max, testCase.Inclusion);

        // Assert
        Assert.Equal(testCase.Expected, result);
        Assert.False(result);
    }

    [Theory]
    [MemberData(nameof(CollectionRulesTestData.HasIndex.ValidCases), MemberType = typeof(CollectionRulesTestData.HasIndex))]
    public void HasIndex_ReturnsTrue_ForExistingIndex(CollectionRulesTestData.HasIndex.Case testCase)
    {
        // Arrange
        var value = testCase.ValueFactory();

        // Act
        var result = CollectionRules.HasIndex(value, testCase.Index);

        // Assert
        Assert.Equal(testCase.Expected, result);
        Assert.True(result);
    }

    [Theory]
    [MemberData(nameof(CollectionRulesTestData.HasIndex.EdgeCases), MemberType = typeof(CollectionRulesTestData.HasIndex))]
    public void HasIndex_ReturnsFalse_ForMissingIndex(CollectionRulesTestData.HasIndex.Case testCase)
    {
        // Arrange
        var value = testCase.ValueFactory();

        // Act
        var result = CollectionRules.HasIndex(value, testCase.Index);

        // Assert
        Assert.Equal(testCase.Expected, result);
        Assert.False(result);
    }

    [Theory]
    [MemberData(nameof(CollectionRulesTestData.Contains.ValidCases), MemberType = typeof(CollectionRulesTestData.Contains))]
    public void Contains_ReturnsTrue_ForContainedItem(CollectionRulesTestData.Contains.Case testCase)
    {
        // Arrange
        var value = testCase.ValueFactory();

        // Act
        var result = CollectionRules.Contains(value, testCase.Item);

        // Assert
        Assert.Equal(testCase.Expected, result);
        Assert.True(result);
    }

    [Theory]
    [MemberData(nameof(CollectionRulesTestData.Contains.EdgeCases), MemberType = typeof(CollectionRulesTestData.Contains))]
    public void Contains_ReturnsFalse_ForMissingItem(CollectionRulesTestData.Contains.Case testCase)
    {
        // Arrange
        var value = testCase.ValueFactory();

        // Act
        var result = CollectionRules.Contains(value, testCase.Item);

        // Assert
        Assert.Equal(testCase.Expected, result);
        Assert.False(result);
    }

    [Theory]
    [MemberData(nameof(CollectionRulesTestData.HasAny.ValidCases), MemberType = typeof(CollectionRulesTestData.HasAny))]
    public void HasAny_ReturnsTrue_WhenAnyMatch(CollectionRulesTestData.HasAny.Case testCase)
    {
        // Arrange
        var value = testCase.ValueFactory();

        // Act
        var result = CollectionRules.HasAny(value, testCase.Predicate);

        // Assert
        Assert.Equal(testCase.Expected, result);
        Assert.True(result);
    }

    [Theory]
    [MemberData(nameof(CollectionRulesTestData.HasAny.EdgeCases), MemberType = typeof(CollectionRulesTestData.HasAny))]
    public void HasAny_ReturnsFalse_WhenNoMatchOrNull(CollectionRulesTestData.HasAny.Case testCase)
    {
        // Arrange
        var value = testCase.ValueFactory();

        // Act
        var result = CollectionRules.HasAny(value, testCase.Predicate);

        // Assert
        Assert.Equal(testCase.Expected, result);
        Assert.False(result);
    }

    [Fact]
    public void HasAny_Throws_WhenPredicateIsNull()
    {
        // Arrange
        var value = new[] { 1 };

        // Act
        void Act() => CollectionRules.HasAny(value, null!);

        // Assert
        Assert.Throws<ArgumentNullException>(Act);
    }

    [Theory]
    [MemberData(nameof(CollectionRulesTestData.HasAll.ValidCases), MemberType = typeof(CollectionRulesTestData.HasAll))]
    public void HasAll_ReturnsTrue_WhenAllMatch(CollectionRulesTestData.HasAll.Case testCase)
    {
        // Arrange
        var value = testCase.ValueFactory();

        // Act
        var result = CollectionRules.HasAll(value, testCase.Predicate);

        // Assert
        Assert.Equal(testCase.Expected, result);
        Assert.True(result);
    }

    [Theory]
    [MemberData(nameof(CollectionRulesTestData.HasAll.EdgeCases), MemberType = typeof(CollectionRulesTestData.HasAll))]
    public void HasAll_ReturnsFalse_WhenNotAllMatchOrNull(CollectionRulesTestData.HasAll.Case testCase)
    {
        // Arrange
        var value = testCase.ValueFactory();

        // Act
        var result = CollectionRules.HasAll(value, testCase.Predicate);

        // Assert
        Assert.Equal(testCase.Expected, result);
        Assert.False(result);
    }

    [Fact]
    public void HasAll_Throws_WhenPredicateIsNull()
    {
        // Arrange
        var value = new[] { 1 };

        // Act
        void Act() => CollectionRules.HasAll(value, null!);

        // Assert
        Assert.Throws<ArgumentNullException>(Act);
    }

    [Theory]
    [MemberData(nameof(CollectionRulesTestData.IsSubsetOf.ValidCases), MemberType = typeof(CollectionRulesTestData.IsSubsetOf))]
    public void IsSubsetOf_ReturnsTrue_ForSubsets(CollectionRulesTestData.IsSubsetOf.Case testCase)
    {
        // Arrange
        var value = testCase.ValueFactory();
        var other = testCase.OtherFactory();

        // Act
        var result = CollectionRules.IsSubsetOf(value, other);

        // Assert
        Assert.Equal(testCase.Expected, result);
        Assert.True(result);
    }

    [Theory]
    [MemberData(nameof(CollectionRulesTestData.IsSubsetOf.EdgeCases), MemberType = typeof(CollectionRulesTestData.IsSubsetOf))]
    public void IsSubsetOf_ReturnsFalse_ForNonSubsetsOrNull(CollectionRulesTestData.IsSubsetOf.Case testCase)
    {
        // Arrange
        var value = testCase.ValueFactory();
        var other = testCase.OtherFactory();

        // Act
        var result = CollectionRules.IsSubsetOf(value, other);

        // Assert
        Assert.Equal(testCase.Expected, result);
        Assert.False(result);
    }
}
