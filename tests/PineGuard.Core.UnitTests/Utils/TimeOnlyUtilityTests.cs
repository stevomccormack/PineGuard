using PineGuard.Testing.UnitTests;
using PineGuard.Utils;

namespace PineGuard.Core.UnitTests.Utils;

public sealed class TimeOnlyUtilityTests : BaseUnitTest
{
    [Theory]
    [MemberData(nameof(TimeOnlyUtilityTestData.TryTruncateToPrecision.ValidCases), MemberType = typeof(TimeOnlyUtilityTestData.TryTruncateToPrecision))]
    [MemberData(nameof(TimeOnlyUtilityTestData.TryTruncateToPrecision.EdgeCases), MemberType = typeof(TimeOnlyUtilityTestData.TryTruncateToPrecision))]
    public void TryTruncateToPrecision_ReturnsExpected(TimeOnlyUtilityTestData.TryTruncateToPrecision.ValidCase testCase)
    {
        // Act
        var ok = TimeOnlyUtility.TryTruncateToPrecision(testCase.Value.Value, testCase.Value.Precision, out var truncated);

        // Assert
        Assert.Equal(testCase.Expected, ok);
        Assert.Equal(testCase.ExpectedOutValue, truncated);
    }
}
