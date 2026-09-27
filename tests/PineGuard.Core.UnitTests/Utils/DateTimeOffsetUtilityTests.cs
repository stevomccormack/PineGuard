using PineGuard.Testing.UnitTests;
using PineGuard.Utils;

namespace PineGuard.Core.UnitTests.Utils;

public sealed class DateTimeOffsetUtilityTests : BaseUnitTest
{
    [Theory]
    [MemberData(nameof(DateTimeOffsetUtilityTestData.TryGetDateAtOffset.ValidCases), MemberType = typeof(DateTimeOffsetUtilityTestData.TryGetDateAtOffset))]
    [MemberData(nameof(DateTimeOffsetUtilityTestData.TryGetDateAtOffset.EdgeCases), MemberType = typeof(DateTimeOffsetUtilityTestData.TryGetDateAtOffset))]
    public void TryGetDateAtOffset_ReturnsExpected(DateTimeOffsetUtilityTestData.TryGetDateAtOffset.ValidCase testCase)
    {
        // Arrange
        var (instant, offset) = testCase.Value;

        // Act
        var ok = DateTimeOffsetUtility.TryGetDateAtOffset(instant, offset, out var date);

        // Assert
        Assert.Equal(testCase.Expected, ok);
        Assert.Equal(testCase.ExpectedOutValue, date);

        if (ok)
            Assert.Equal(DateTimeKind.Unspecified, date.Kind);
    }
}
