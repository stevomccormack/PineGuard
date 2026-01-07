using PineGuard.Testing.UnitTests;
using PineGuard.Utils;

namespace PineGuard.Core.UnitTests.Utils;

public sealed class DictionaryUtilityTests : BaseUnitTest
{
    [Theory]
    [MemberData(nameof(DictionaryUtilityTestData.TryGetCount.ValidCases), MemberType = typeof(DictionaryUtilityTestData.TryGetCount))]
    [MemberData(nameof(DictionaryUtilityTestData.TryGetCount.EdgeCases), MemberType = typeof(DictionaryUtilityTestData.TryGetCount))]
    public void TryGetCount_ReturnsExpected(DictionaryUtilityTestData.TryGetCount.Case testCase)
    {
        var found = DictionaryUtility.TryGetCount(testCase.Dictionary, out var count);

        Assert.Equal(testCase.ExpectedFound, found);
        Assert.Equal(testCase.ExpectedCount, count);
    }

    [Theory]
    [MemberData(nameof(DictionaryUtilityTestData.TryGetValue.ValidCases), MemberType = typeof(DictionaryUtilityTestData.TryGetValue))]
    [MemberData(nameof(DictionaryUtilityTestData.TryGetValue.EdgeCases), MemberType = typeof(DictionaryUtilityTestData.TryGetValue))]
    public void TryGetValue_ReturnsExpected(DictionaryUtilityTestData.TryGetValue.Case testCase)
    {
        var found = DictionaryUtility.TryGetValue(testCase.Dictionary, testCase.Key, out var value);

        Assert.Equal(testCase.ExpectedFound, found);
        Assert.Equal(testCase.ExpectedValue, value);
    }

    [Theory]
    [MemberData(nameof(DictionaryUtilityTestData.TryGetKeyValue.ValidCases), MemberType = typeof(DictionaryUtilityTestData.TryGetKeyValue))]
    [MemberData(nameof(DictionaryUtilityTestData.TryGetKeyValue.EdgeCases), MemberType = typeof(DictionaryUtilityTestData.TryGetKeyValue))]
    public void TryGetKeyValue_ReturnsExpected(DictionaryUtilityTestData.TryGetKeyValue.Case testCase)
    {
        var found = DictionaryUtility.TryGetKeyValue(testCase.Dictionary, testCase.Key, out var pair);

        Assert.Equal(testCase.ExpectedFound, found);
        Assert.Equal(testCase.ExpectedPair, pair);
    }

    [Theory]
    [MemberData(nameof(DictionaryUtilityTestData.TryGetKey.ValidCases), MemberType = typeof(DictionaryUtilityTestData.TryGetKey))]
    [MemberData(nameof(DictionaryUtilityTestData.TryGetKey.EdgeCases), MemberType = typeof(DictionaryUtilityTestData.TryGetKey))]
    public void TryGetKey_ReturnsExpected(DictionaryUtilityTestData.TryGetKey.Case testCase)
    {
        var found = DictionaryUtility.TryGetKey(testCase.Dictionary, testCase.Value, out var key);

        Assert.Equal(testCase.ExpectedFound, found);
        Assert.Equal(testCase.ExpectedKey, key);
    }

    [Fact]
    public void TryGetAnyKey_Throws_ForNullPredicate()
    {
        Assert.Throws<ArgumentNullException>(() => DictionaryUtility.TryGetAnyKey(new Dictionary<string, int>(), null!, out _));
    }

    [Fact]
    public void TryGetAnyKey_ReturnsExpected_ForNullAndMatches()
    {
        Assert.False(DictionaryUtility.TryGetAnyKey<string, int>(null, _ => true, out var key));
        Assert.Null(key);

        var dict = new Dictionary<string, int> { ["a"] = 1, ["b"] = 2 };

        Assert.False(DictionaryUtility.TryGetAnyKey(dict, k => k == "missing", out key));
        Assert.Null(key);

        Assert.True(DictionaryUtility.TryGetAnyKey(dict, k => k == "b", out key));
        Assert.Equal("b", key);
    }

    [Fact]
    public void TryGetAnyValue_Throws_ForNullPredicate()
    {
        Assert.Throws<ArgumentNullException>(() => DictionaryUtility.TryGetAnyValue(new Dictionary<string, int>(), null!, out _));
    }

    [Fact]
    public void TryGetAnyValue_ReturnsExpected_ForNullAndMatches()
    {
        Assert.False(DictionaryUtility.TryGetAnyValue<string, int>(null, _ => true, out var value));
        Assert.Equal(0, value);

        var dict = new Dictionary<string, int> { ["a"] = 1, ["b"] = 2 };

        Assert.False(DictionaryUtility.TryGetAnyValue(dict, v => v == 99, out value));
        Assert.Equal(0, value);

        Assert.True(DictionaryUtility.TryGetAnyValue(dict, v => v == 2, out value));
        Assert.Equal(2, value);
    }

    [Fact]
    public void TryGetAnyItem_Throws_ForNullPredicate()
    {
        Assert.Throws<ArgumentNullException>(() => DictionaryUtility.TryGetAnyItem(new Dictionary<string, int>(), null!, out _));
    }

    [Fact]
    public void TryGetAnyItem_ReturnsExpected_ForNullAndMatches()
    {
        Assert.False(DictionaryUtility.TryGetAnyItem<string, int>(null, (_, _) => true, out var pair));
        Assert.Equal(default, pair);

        var dict = new Dictionary<string, int> { ["a"] = 1, ["b"] = 2 };

        Assert.False(DictionaryUtility.TryGetAnyItem(dict, (_, v) => v == 99, out pair));
        Assert.Equal(default, pair);

        Assert.True(DictionaryUtility.TryGetAnyItem(dict, (k, v) => k == "b" && v == 2, out pair));
        Assert.Equal(new KeyValuePair<string, int>("b", 2), pair);
    }
}
