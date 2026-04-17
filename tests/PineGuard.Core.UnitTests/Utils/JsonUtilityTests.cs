using PineGuard.Testing.UnitTests;
using PineGuard.Utils;

namespace PineGuard.Core.UnitTests.Utils;

public sealed class JsonUtilityTests : BaseUnitTest
{
    [Theory]
    [MemberData(nameof(JsonUtilityTestData.TryGetRootKind.ValidCases), MemberType = typeof(JsonUtilityTestData.TryGetRootKind))]
    [MemberData(nameof(JsonUtilityTestData.TryGetRootKind.EdgeCases), MemberType = typeof(JsonUtilityTestData.TryGetRootKind))]
    public void TryGetRootKind_ReturnsExpected(JsonUtilityTestData.TryGetRootKind.ValidCase testCase)
    {
        // Act
        var ok = JsonUtility.TryGetRootKind(testCase.Value, out var kind);

        // Assert
        Assert.Equal(testCase.Expected, ok);
        Assert.Equal(testCase.ExpectedOutValue, kind);
    }

    [Theory]
    [MemberData(nameof(JsonUtilityTestData.TryGetRootKindSpan.ValidCases), MemberType = typeof(JsonUtilityTestData.TryGetRootKindSpan))]
    [MemberData(nameof(JsonUtilityTestData.TryGetRootKindSpan.EdgeCases), MemberType = typeof(JsonUtilityTestData.TryGetRootKindSpan))]
    public void TryGetRootKind_Span_ReturnsExpected(JsonUtilityTestData.TryGetRootKindSpan.ValidCase testCase)
    {
        // Act
        var ok = JsonUtility.TryGetRootKind(testCase.RawValue.AsSpan(), out var kind);

        // Assert
        Assert.Equal(testCase.Expected, ok);
        Assert.Equal(testCase.ExpectedOutValue, kind);
    }
}
