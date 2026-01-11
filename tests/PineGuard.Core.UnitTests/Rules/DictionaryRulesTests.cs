using PineGuard.Rules;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Rules;

public sealed class DictionaryRulesTests : BaseUnitTest
{
    [Theory]
    [MemberData(nameof(DictionaryRulesTestData.IsEmpty.ValidCases), MemberType = typeof(DictionaryRulesTestData.IsEmpty))]
    public void IsEmpty_ReturnsTrue_ForNullOrEmpty(DictionaryRulesTestData.IsEmpty.Case testCase)
    {
        var dictionary = testCase.Value;

        var result = DictionaryRules.IsEmpty(dictionary);

        Assert.Equal(testCase.ExpectedReturn, result);
        Assert.True(result);
    }

    [Theory]
    [MemberData(nameof(DictionaryRulesTestData.IsEmpty.EdgeCases), MemberType = typeof(DictionaryRulesTestData.IsEmpty))]
    public void IsEmpty_ReturnsFalse_ForNonEmpty(DictionaryRulesTestData.IsEmpty.Case testCase)
    {
        var dictionary = testCase.Value;

        var result = DictionaryRules.IsEmpty(dictionary);

        Assert.Equal(testCase.ExpectedReturn, result);
        Assert.False(result);
    }

    [Theory]
    [MemberData(nameof(DictionaryRulesTestData.HasItems.ValidCases), MemberType = typeof(DictionaryRulesTestData.HasItems))]
    public void HasItems_ReturnsTrue_ForNonEmpty(DictionaryRulesTestData.HasItems.Case testCase)
    {
        var dictionary = testCase.Value;

        var result = DictionaryRules.HasItems(dictionary);

        Assert.Equal(testCase.ExpectedReturn, result);
        Assert.True(result);
    }

    [Theory]
    [MemberData(nameof(DictionaryRulesTestData.HasItems.EdgeCases), MemberType = typeof(DictionaryRulesTestData.HasItems))]
    public void HasItems_ReturnsFalse_ForNullOrEmpty(DictionaryRulesTestData.HasItems.Case testCase)
    {
        var dictionary = testCase.Value;

        var result = DictionaryRules.HasItems(dictionary);

        Assert.Equal(testCase.ExpectedReturn, result);
        Assert.False(result);
    }

    [Theory]
    [MemberData(nameof(DictionaryRulesTestData.HasKey.ValidCases), MemberType = typeof(DictionaryRulesTestData.HasKey))]
    public void HasKey_ReturnsTrue_WhenKeyExists(DictionaryRulesTestData.HasKey.Case testCase)
    {
        var dictionary = testCase.Value.Dictionary;

        var result = DictionaryRules.HasKey(dictionary, testCase.Value.Key);

        Assert.Equal(testCase.ExpectedReturn, result);
        Assert.True(result);
    }

    [Theory]
    [MemberData(nameof(DictionaryRulesTestData.HasKey.EdgeCases), MemberType = typeof(DictionaryRulesTestData.HasKey))]
    public void HasKey_ReturnsFalse_WhenKeyMissingOrDictionaryNull(DictionaryRulesTestData.HasKey.Case testCase)
    {
        var dictionary = testCase.Value.Dictionary;

        var result = DictionaryRules.HasKey(dictionary, testCase.Value.Key);

        Assert.Equal(testCase.ExpectedReturn, result);
        Assert.False(result);
    }

    [Theory]
    [MemberData(nameof(DictionaryRulesTestData.HasValue.ValidCases), MemberType = typeof(DictionaryRulesTestData.HasValue))]
    public void HasValue_ReturnsTrue_WhenValueExists(DictionaryRulesTestData.HasValue.Case testCase)
    {
        var dictionary = testCase.Value.Dictionary;

        var result = DictionaryRules.HasValue(dictionary, testCase.Value.SearchValue);

        Assert.Equal(testCase.ExpectedReturn, result);
        Assert.True(result);
    }

    [Theory]
    [MemberData(nameof(DictionaryRulesTestData.HasValue.EdgeCases), MemberType = typeof(DictionaryRulesTestData.HasValue))]
    public void HasValue_ReturnsFalse_WhenValueMissingOrDictionaryNull(DictionaryRulesTestData.HasValue.Case testCase)
    {
        var dictionary = testCase.Value.Dictionary;

        var result = DictionaryRules.HasValue(dictionary, testCase.Value.SearchValue);

        Assert.Equal(testCase.ExpectedReturn, result);
        Assert.False(result);
    }

    [Theory]
    [MemberData(nameof(DictionaryRulesTestData.HasKeyValue.ValidCases), MemberType = typeof(DictionaryRulesTestData.HasKeyValue))]
    public void HasKeyValue_ReturnsTrue_WhenExactPairExists(DictionaryRulesTestData.HasKeyValue.Case testCase)
    {
        var dictionary = testCase.Value.Dictionary;

        var result = DictionaryRules.HasKeyValue(dictionary, testCase.Value.Key, testCase.Value.SearchValue);

        Assert.Equal(testCase.ExpectedReturn, result);
        Assert.True(result);
    }

    [Theory]
    [MemberData(nameof(DictionaryRulesTestData.HasKeyValue.EdgeCases), MemberType = typeof(DictionaryRulesTestData.HasKeyValue))]
    public void HasKeyValue_ReturnsFalse_WhenPairMissingOrDictionaryNull(DictionaryRulesTestData.HasKeyValue.Case testCase)
    {
        var dictionary = testCase.Value.Dictionary;

        var result = DictionaryRules.HasKeyValue(dictionary, testCase.Value.Key, testCase.Value.SearchValue);

        Assert.Equal(testCase.ExpectedReturn, result);
        Assert.False(result);
    }

    [Theory]
    [MemberData(nameof(DictionaryRulesTestData.HasAnyKey.ValidCases), MemberType = typeof(DictionaryRulesTestData.HasAnyKey))]
    public void HasAnyKey_ReturnsTrue_WhenAnyKeyMatches(DictionaryRulesTestData.HasAnyKey.Case testCase)
    {
        var dictionary = testCase.Value.Dictionary;

        var result = DictionaryRules.HasAnyKey(dictionary, testCase.Value.Predicate);

        Assert.Equal(testCase.ExpectedReturn, result);
        Assert.True(result);
    }

    [Theory]
    [MemberData(nameof(DictionaryRulesTestData.HasAnyKey.EdgeCases), MemberType = typeof(DictionaryRulesTestData.HasAnyKey))]
    public void HasAnyKey_ReturnsFalse_WhenNoKeyMatchesOrDictionaryNull(DictionaryRulesTestData.HasAnyKey.Case testCase)
    {
        var dictionary = testCase.Value.Dictionary;

        var result = DictionaryRules.HasAnyKey(dictionary, testCase.Value.Predicate);

        Assert.Equal(testCase.ExpectedReturn, result);
        Assert.False(result);
    }

    [Fact]
    public void HasAnyKey_Throws_WhenPredicateIsNull()
    {
        void Act() => DictionaryRules.HasAnyKey(new Dictionary<string, int>(), predicate: null!);

        Assert.Throws<ArgumentNullException>(Act);
    }

    [Theory]
    [MemberData(nameof(DictionaryRulesTestData.HasAnyValue.ValidCases), MemberType = typeof(DictionaryRulesTestData.HasAnyValue))]
    public void HasAnyValue_ReturnsTrue_WhenAnyValueMatches(DictionaryRulesTestData.HasAnyValue.Case testCase)
    {
        var dictionary = testCase.Value.Dictionary;

        var result = DictionaryRules.HasAnyValue(dictionary, testCase.Value.Predicate);

        Assert.Equal(testCase.ExpectedReturn, result);
        Assert.True(result);
    }

    [Theory]
    [MemberData(nameof(DictionaryRulesTestData.HasAnyValue.EdgeCases), MemberType = typeof(DictionaryRulesTestData.HasAnyValue))]
    public void HasAnyValue_ReturnsFalse_WhenNoValueMatchesOrDictionaryNull(DictionaryRulesTestData.HasAnyValue.Case testCase)
    {
        var dictionary = testCase.Value.Dictionary;

        var result = DictionaryRules.HasAnyValue(dictionary, testCase.Value.Predicate);

        Assert.Equal(testCase.ExpectedReturn, result);
        Assert.False(result);
    }

    [Fact]
    public void HasAnyValue_Throws_WhenPredicateIsNull()
    {
        void Act() => DictionaryRules.HasAnyValue(new Dictionary<string, int>(), predicate: null!);

        Assert.Throws<ArgumentNullException>(Act);
    }

    [Theory]
    [MemberData(nameof(DictionaryRulesTestData.HasAnyItem.ValidCases), MemberType = typeof(DictionaryRulesTestData.HasAnyItem))]
    public void HasAnyItem_ReturnsTrue_WhenAnyItemMatches(DictionaryRulesTestData.HasAnyItem.Case testCase)
    {
        var dictionary = testCase.Value.Dictionary;

        var result = DictionaryRules.HasAnyItem(dictionary, testCase.Value.Predicate);

        Assert.Equal(testCase.ExpectedReturn, result);
        Assert.True(result);
    }

    [Theory]
    [MemberData(nameof(DictionaryRulesTestData.HasAnyItem.EdgeCases), MemberType = typeof(DictionaryRulesTestData.HasAnyItem))]
    public void HasAnyItem_ReturnsFalse_WhenNoItemMatchesOrDictionaryNull(DictionaryRulesTestData.HasAnyItem.Case testCase)
    {
        var dictionary = testCase.Value.Dictionary;

        var result = DictionaryRules.HasAnyItem(dictionary, testCase.Value.Predicate);

        Assert.Equal(testCase.ExpectedReturn, result);
        Assert.False(result);
    }

    [Fact]
    public void HasAnyItem_Throws_WhenPredicateIsNull()
    {
        void Act() => DictionaryRules.HasAnyItem(new Dictionary<string, int>(), predicate: null!);

        Assert.Throws<ArgumentNullException>(Act);
    }
}
