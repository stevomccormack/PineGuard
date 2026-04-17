using PineGuard.Testing.UnitTests;
using PineGuard.Utils;

namespace PineGuard.Core.UnitTests.Utils;

public sealed class ReadOnlyDictionaryUtilityTests : BaseUnitTest
{
    [Theory]
    [MemberData(nameof(ReadOnlyDictionaryUtilityTestData.TryGetCount.ValidCases), MemberType = typeof(ReadOnlyDictionaryUtilityTestData.TryGetCount))]
    [MemberData(nameof(ReadOnlyDictionaryUtilityTestData.TryGetCount.EdgeCases), MemberType = typeof(ReadOnlyDictionaryUtilityTestData.TryGetCount))]
    public void TryGetCount_ReturnsExpected(ReadOnlyDictionaryUtilityTestData.TryGetCount.ValidCase testCase)
    {
        // Act
        var found = ReadOnlyDictionaryUtility.TryGetCount(testCase.Value, out var count);

        // Assert
        Assert.Equal(testCase.Expected, found);
        Assert.Equal(testCase.ExpectedOutValue, count);
    }

    [Theory]
    [MemberData(nameof(ReadOnlyDictionaryUtilityTestData.TryGetValue.ValidCases), MemberType = typeof(ReadOnlyDictionaryUtilityTestData.TryGetValue))]
    [MemberData(nameof(ReadOnlyDictionaryUtilityTestData.TryGetValue.EdgeCases), MemberType = typeof(ReadOnlyDictionaryUtilityTestData.TryGetValue))]
    public void TryGetValue_ReturnsExpected(ReadOnlyDictionaryUtilityTestData.TryGetValue.ValidCase testCase)
    {
        // Act
        var found = ReadOnlyDictionaryUtility.TryGetValue(testCase.Value.dictionary, testCase.Value.key, out var value);

        // Assert
        Assert.Equal(testCase.Expected, found);
        Assert.Equal(testCase.ExpectedOutValue, value);
    }

    [Theory]
    [MemberData(nameof(ReadOnlyDictionaryUtilityTestData.TryGetKeyValue.ValidCases), MemberType = typeof(ReadOnlyDictionaryUtilityTestData.TryGetKeyValue))]
    [MemberData(nameof(ReadOnlyDictionaryUtilityTestData.TryGetKeyValue.EdgeCases), MemberType = typeof(ReadOnlyDictionaryUtilityTestData.TryGetKeyValue))]
    public void TryGetKeyValue_ReturnsExpected(ReadOnlyDictionaryUtilityTestData.TryGetKeyValue.ValidCase testCase)
    {
        // Act
        var found = ReadOnlyDictionaryUtility.TryGetKeyValue(testCase.Value.dictionary, testCase.Value.key, out var pair);

        // Assert
        Assert.Equal(testCase.Expected, found);
        Assert.Equal(testCase.ExpectedOutValue, pair);
    }

    [Theory]
    [MemberData(nameof(ReadOnlyDictionaryUtilityTestData.TryGetKey.ValidCases), MemberType = typeof(ReadOnlyDictionaryUtilityTestData.TryGetKey))]
    [MemberData(nameof(ReadOnlyDictionaryUtilityTestData.TryGetKey.EdgeCases), MemberType = typeof(ReadOnlyDictionaryUtilityTestData.TryGetKey))]
    public void TryGetKey_ReturnsExpected(ReadOnlyDictionaryUtilityTestData.TryGetKey.ValidCase testCase)
    {
        // Act
        var found = ReadOnlyDictionaryUtility.TryGetKey(testCase.Value.dictionary, testCase.Value.searchValue, out var key);

        // Assert
        Assert.Equal(testCase.Expected, found);
        Assert.Equal(testCase.ExpectedOutValue, key);
    }

    [Theory]
    [MemberData(nameof(ReadOnlyDictionaryUtilityTestData.TryGetAnyKey.ValidCases), MemberType = typeof(ReadOnlyDictionaryUtilityTestData.TryGetAnyKey))]
    [MemberData(nameof(ReadOnlyDictionaryUtilityTestData.TryGetAnyKey.EdgeCases), MemberType = typeof(ReadOnlyDictionaryUtilityTestData.TryGetAnyKey))]
    public void TryGetAnyKey_ReturnsExpected(ReadOnlyDictionaryUtilityTestData.TryGetAnyKey.ValidCase testCase)
    {
        // Act
        var found = ReadOnlyDictionaryUtility.TryGetAnyKey(testCase.Value.dictionary, testCase.Value.predicate, out var key);

        // Assert
        Assert.Equal(testCase.Expected, found);
        Assert.Equal(testCase.ExpectedOutValue, key);
    }

    [Theory]
    [MemberData(nameof(ReadOnlyDictionaryUtilityTestData.TryGetAnyKey.InvalidCases), MemberType = typeof(ReadOnlyDictionaryUtilityTestData.TryGetAnyKey))]
    public void TryGetAnyKey_Throws_ForInvalidInput(ReadOnlyDictionaryUtilityTestData.TryGetAnyKey.InvalidCase testCase)
    {
        // Act & Assert
        Assert.Throws(testCase.ExpectedException.Type, () => ReadOnlyDictionaryUtility.TryGetAnyKey(testCase.Value.dictionary, testCase.Value.predicate, out _));
    }

    [Theory]
    [MemberData(nameof(ReadOnlyDictionaryUtilityTestData.TryGetAnyValue.ValidCases), MemberType = typeof(ReadOnlyDictionaryUtilityTestData.TryGetAnyValue))]
    [MemberData(nameof(ReadOnlyDictionaryUtilityTestData.TryGetAnyValue.EdgeCases), MemberType = typeof(ReadOnlyDictionaryUtilityTestData.TryGetAnyValue))]
    public void TryGetAnyValue_ReturnsExpected(ReadOnlyDictionaryUtilityTestData.TryGetAnyValue.ValidCase testCase)
    {
        // Act
        var found = ReadOnlyDictionaryUtility.TryGetAnyValue(testCase.Value.dictionary, testCase.Value.predicate, out var value);

        // Assert
        Assert.Equal(testCase.Expected, found);
        Assert.Equal(testCase.ExpectedOutValue, value);
    }

    [Theory]
    [MemberData(nameof(ReadOnlyDictionaryUtilityTestData.TryGetAnyValue.InvalidCases), MemberType = typeof(ReadOnlyDictionaryUtilityTestData.TryGetAnyValue))]
    public void TryGetAnyValue_Throws_ForInvalidInput(ReadOnlyDictionaryUtilityTestData.TryGetAnyValue.InvalidCase testCase)
    {
        // Act & Assert
        Assert.Throws(testCase.ExpectedException.Type, () => ReadOnlyDictionaryUtility.TryGetAnyValue(testCase.Value.dictionary, testCase.Value.predicate, out _));
    }

    [Theory]
    [MemberData(nameof(ReadOnlyDictionaryUtilityTestData.TryGetAnyItem.ValidCases), MemberType = typeof(ReadOnlyDictionaryUtilityTestData.TryGetAnyItem))]
    [MemberData(nameof(ReadOnlyDictionaryUtilityTestData.TryGetAnyItem.EdgeCases), MemberType = typeof(ReadOnlyDictionaryUtilityTestData.TryGetAnyItem))]
    public void TryGetAnyItem_ReturnsExpected(ReadOnlyDictionaryUtilityTestData.TryGetAnyItem.ValidCase testCase)
    {
        // Act
        var found = ReadOnlyDictionaryUtility.TryGetAnyItem(testCase.Value.dictionary, testCase.Value.predicate, out var pair);

        // Assert
        Assert.Equal(testCase.Expected, found);
        Assert.Equal(testCase.ExpectedOutValue, pair);
    }

    [Theory]
    [MemberData(nameof(ReadOnlyDictionaryUtilityTestData.TryGetAnyItem.InvalidCases), MemberType = typeof(ReadOnlyDictionaryUtilityTestData.TryGetAnyItem))]
    public void TryGetAnyItem_Throws_ForInvalidInput(ReadOnlyDictionaryUtilityTestData.TryGetAnyItem.InvalidCase testCase)
    {
        // Act & Assert
        Assert.Throws(testCase.ExpectedException.Type, () => ReadOnlyDictionaryUtility.TryGetAnyItem(testCase.Value.dictionary, testCase.Value.predicate, out _));
    }
}
