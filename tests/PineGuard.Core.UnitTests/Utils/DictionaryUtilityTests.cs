using PineGuard.Testing.UnitTests;
using PineGuard.Utils;

namespace PineGuard.Core.UnitTests.Utils;

public sealed class DictionaryUtilityTests : BaseUnitTest
{
    [Theory]
    [MemberData(nameof(DictionaryUtilityTestData.TryGetCount.ValidCases), MemberType = typeof(DictionaryUtilityTestData.TryGetCount))]
    [MemberData(nameof(DictionaryUtilityTestData.TryGetCount.EdgeCases), MemberType = typeof(DictionaryUtilityTestData.TryGetCount))]
    public void TryGetCount_ReturnsExpected(DictionaryUtilityTestData.TryGetCount.ValidCase testCase)
    {
        // Act
        var found = DictionaryUtility.TryGetCount(testCase.Value, out var count);

        // Assert
        Assert.Equal(testCase.Expected, found);
        Assert.Equal(testCase.ExpectedOutValue, count);
    }

    [Theory]
    [MemberData(nameof(DictionaryUtilityTestData.TryGetValue.ValidCases), MemberType = typeof(DictionaryUtilityTestData.TryGetValue))]
    [MemberData(nameof(DictionaryUtilityTestData.TryGetValue.EdgeCases), MemberType = typeof(DictionaryUtilityTestData.TryGetValue))]
    public void TryGetValue_ReturnsExpected(DictionaryUtilityTestData.TryGetValue.ValidCase testCase)
    {
        // Act
        var found = DictionaryUtility.TryGetValue(testCase.Value.dictionary, testCase.Value.key, out var value);

        // Assert
        Assert.Equal(testCase.Expected, found);
        Assert.Equal(testCase.ExpectedOutValue, value);
    }

    [Theory]
    [MemberData(nameof(DictionaryUtilityTestData.TryGetKeyValue.ValidCases), MemberType = typeof(DictionaryUtilityTestData.TryGetKeyValue))]
    [MemberData(nameof(DictionaryUtilityTestData.TryGetKeyValue.EdgeCases), MemberType = typeof(DictionaryUtilityTestData.TryGetKeyValue))]
    public void TryGetKeyValue_ReturnsExpected(DictionaryUtilityTestData.TryGetKeyValue.ValidCase testCase)
    {
        // Act
        var found = DictionaryUtility.TryGetKeyValue(testCase.Value.dictionary, testCase.Value.key, out var pair);

        // Assert
        Assert.Equal(testCase.Expected, found);
        Assert.Equal(testCase.ExpectedOutValue, pair);
    }

    [Theory]
    [MemberData(nameof(DictionaryUtilityTestData.TryGetKey.ValidCases), MemberType = typeof(DictionaryUtilityTestData.TryGetKey))]
    [MemberData(nameof(DictionaryUtilityTestData.TryGetKey.EdgeCases), MemberType = typeof(DictionaryUtilityTestData.TryGetKey))]
    public void TryGetKey_ReturnsExpected(DictionaryUtilityTestData.TryGetKey.ValidCase testCase)
    {
        // Act
        var found = DictionaryUtility.TryGetKey(testCase.Value.dictionary, testCase.Value.searchValue, out var key);

        // Assert
        Assert.Equal(testCase.Expected, found);
        Assert.Equal(testCase.ExpectedOutValue, key);
    }

    [Theory]
    [MemberData(nameof(DictionaryUtilityTestData.TryGetAnyKey.ValidCases), MemberType = typeof(DictionaryUtilityTestData.TryGetAnyKey))]
    [MemberData(nameof(DictionaryUtilityTestData.TryGetAnyKey.EdgeCases), MemberType = typeof(DictionaryUtilityTestData.TryGetAnyKey))]
    public void TryGetAnyKey_ReturnsExpected(DictionaryUtilityTestData.TryGetAnyKey.ValidCase testCase)
    {
        // Act
        var found = DictionaryUtility.TryGetAnyKey(testCase.Value.dictionary, testCase.Value.predicate, out var key);

        // Assert
        Assert.Equal(testCase.Expected, found);
        Assert.Equal(testCase.ExpectedOutValue, key);
    }

    [Theory]
    [MemberData(nameof(DictionaryUtilityTestData.TryGetAnyKey.InvalidCases), MemberType = typeof(DictionaryUtilityTestData.TryGetAnyKey))]
    public void TryGetAnyKey_Throws_ForInvalidInput(DictionaryUtilityTestData.TryGetAnyKey.InvalidCase testCase)
    {
        // Act & Assert
        Assert.Throws(testCase.ExpectedException.Type, () => DictionaryUtility.TryGetAnyKey(testCase.Value.dictionary, testCase.Value.predicate, out _));
    }

    [Theory]
    [MemberData(nameof(DictionaryUtilityTestData.TryGetAnyValue.ValidCases), MemberType = typeof(DictionaryUtilityTestData.TryGetAnyValue))]
    [MemberData(nameof(DictionaryUtilityTestData.TryGetAnyValue.EdgeCases), MemberType = typeof(DictionaryUtilityTestData.TryGetAnyValue))]
    public void TryGetAnyValue_ReturnsExpected(DictionaryUtilityTestData.TryGetAnyValue.ValidCase testCase)
    {
        // Act
        var found = DictionaryUtility.TryGetAnyValue(testCase.Value.dictionary, testCase.Value.predicate, out var value);

        // Assert
        Assert.Equal(testCase.Expected, found);
        Assert.Equal(testCase.ExpectedOutValue, value);
    }

    [Theory]
    [MemberData(nameof(DictionaryUtilityTestData.TryGetAnyValue.InvalidCases), MemberType = typeof(DictionaryUtilityTestData.TryGetAnyValue))]
    public void TryGetAnyValue_Throws_ForInvalidInput(DictionaryUtilityTestData.TryGetAnyValue.InvalidCase testCase)
    {
        // Act & Assert
        Assert.Throws(testCase.ExpectedException.Type, () => DictionaryUtility.TryGetAnyValue(testCase.Value.dictionary, testCase.Value.predicate, out _));
    }

    [Theory]
    [MemberData(nameof(DictionaryUtilityTestData.TryGetAnyItem.ValidCases), MemberType = typeof(DictionaryUtilityTestData.TryGetAnyItem))]
    [MemberData(nameof(DictionaryUtilityTestData.TryGetAnyItem.EdgeCases), MemberType = typeof(DictionaryUtilityTestData.TryGetAnyItem))]
    public void TryGetAnyItem_ReturnsExpected(DictionaryUtilityTestData.TryGetAnyItem.ValidCase testCase)
    {
        // Act
        var found = DictionaryUtility.TryGetAnyItem(testCase.Value.dictionary, testCase.Value.predicate, out var pair);

        // Assert
        Assert.Equal(testCase.Expected, found);
        Assert.Equal(testCase.ExpectedOutValue, pair);
    }

    [Theory]
    [MemberData(nameof(DictionaryUtilityTestData.TryGetAnyItem.InvalidCases), MemberType = typeof(DictionaryUtilityTestData.TryGetAnyItem))]
    public void TryGetAnyItem_Throws_ForInvalidInput(DictionaryUtilityTestData.TryGetAnyItem.InvalidCase testCase)
    {
        // Act & Assert
        Assert.Throws(testCase.ExpectedException.Type, () => DictionaryUtility.TryGetAnyItem(testCase.Value.dictionary, testCase.Value.predicate, out _));
    }
}
