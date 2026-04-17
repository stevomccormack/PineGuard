using PineGuard.Testing.UnitTests;
using PineGuard.Utils;

namespace PineGuard.Core.UnitTests.Utils;

public sealed class CollectionUtilityTests : BaseUnitTest
{
    [Theory]
    [MemberData(nameof(CollectionUtilityTestData.TryGetCount.ValidCases), MemberType = typeof(CollectionUtilityTestData.TryGetCount))]
    [MemberData(nameof(CollectionUtilityTestData.TryGetCount.EdgeCases), MemberType = typeof(CollectionUtilityTestData.TryGetCount))]
    public void TryGetCount_ReturnsExpected(CollectionUtilityTestData.TryGetCount.ValidCase testCase)
    {
        // Act
        var result = CollectionUtility.TryGetCount(testCase.Value, out var count);

        // Assert
        Assert.Equal(testCase.Expected, result);
        Assert.Equal(testCase.ExpectedOutValue, count);
    }

    [Theory]
    [MemberData(nameof(CollectionUtilityTestData.TryGet.ValidCases), MemberType = typeof(CollectionUtilityTestData.TryGet))]
    [MemberData(nameof(CollectionUtilityTestData.TryGet.EdgeCases), MemberType = typeof(CollectionUtilityTestData.TryGet))]
    public void TryGet_ReturnsExpected(CollectionUtilityTestData.TryGet.ValidCase testCase)
    {
        // Act
        var result = CollectionUtility.TryGet(testCase.Value.Collection, testCase.Value.Index, out var item);

        // Assert
        Assert.Equal(testCase.Expected, result);
        Assert.Equal(testCase.ExpectedOutValue, item);
    }

    [Theory]
    [MemberData(nameof(CollectionUtilityTestData.TryGetIndex.ValidCases), MemberType = typeof(CollectionUtilityTestData.TryGetIndex))]
    [MemberData(nameof(CollectionUtilityTestData.TryGetIndex.EdgeCases), MemberType = typeof(CollectionUtilityTestData.TryGetIndex))]
    public void TryGetIndex_ReturnsExpected(CollectionUtilityTestData.TryGetIndex.ValidCase testCase)
    {
        // Act
        var result = CollectionUtility.TryGetIndex(testCase.Value.Collection, testCase.Value.Item, out var index);

        // Assert
        Assert.Equal(testCase.Expected, result);
        Assert.Equal(testCase.ExpectedOutValue, index);
    }
}
