using PineGuard.Testing.UnitTests;
using PineGuard.Utils;

namespace PineGuard.Core.UnitTests.Utils;

public sealed class DateTimeUtilityTests : BaseUnitTest
{
    [Theory]
    [MemberData(nameof(DateTimeUtilityTestData.ToUtc.ValidCases), MemberType = typeof(DateTimeUtilityTestData.ToUtc))]
    [MemberData(nameof(DateTimeUtilityTestData.ToUtc.EdgeCases), MemberType = typeof(DateTimeUtilityTestData.ToUtc))]
    public void ToUtc_ReturnsExpected(DateTimeUtilityTestData.ToUtc.ValidCase testCase)
    {
        // Act
        var result = DateTimeUtility.ToUtc(testCase.Value);

        // Assert
        Assert.Equal(testCase.Expected, result);

        if (!result.HasValue) return;
        Assert.Equal(DateTimeKind.Utc, result.Value.Kind);

        if (testCase.Value is { Kind: DateTimeKind.Unspecified })
        {
            Assert.Equal(testCase.Value.Value.Ticks, result.Value.Ticks);
        }
    }

    [Theory]
    [MemberData(nameof(DateTimeUtilityTestData.Diff.ValidCases), MemberType = typeof(DateTimeUtilityTestData.Diff))]
    [MemberData(nameof(DateTimeUtilityTestData.Diff.EdgeCases), MemberType = typeof(DateTimeUtilityTestData.Diff))]
    public void Diff_ReturnsExpected(DateTimeUtilityTestData.Diff.ValidCase testCase)
    {
        // Act
        var result = DateTimeUtility.Diff(testCase.Value.Start, testCase.Value.End);

        // Assert
        Assert.Equal(testCase.Expected, result);
    }

    [Theory]
    [MemberData(nameof(DateTimeUtilityTestData.TryTruncateToPrecisionUtc.ValidCases), MemberType = typeof(DateTimeUtilityTestData.TryTruncateToPrecisionUtc))]
    [MemberData(nameof(DateTimeUtilityTestData.TryTruncateToPrecisionUtc.EdgeCases), MemberType = typeof(DateTimeUtilityTestData.TryTruncateToPrecisionUtc))]
    public void TryTruncateToPrecisionUtc_ReturnsExpected(DateTimeUtilityTestData.TryTruncateToPrecisionUtc.ValidCase testCase)
    {
        // Act
        var ok = DateTimeUtility.TryTruncateToPrecisionUtc(testCase.Value.Value, testCase.Value.Precision, out var truncated);

        // Assert
        Assert.Equal(testCase.Expected, ok);
        Assert.Equal(testCase.ExpectedOutValue, truncated);
    }

    [Theory]
    [MemberData(nameof(DateTimeUtilityTestData.TryTruncateToPrecisionUtcOffset.ValidCases), MemberType = typeof(DateTimeUtilityTestData.TryTruncateToPrecisionUtcOffset))]
    [MemberData(nameof(DateTimeUtilityTestData.TryTruncateToPrecisionUtcOffset.EdgeCases), MemberType = typeof(DateTimeUtilityTestData.TryTruncateToPrecisionUtcOffset))]
    public void TryTruncateToPrecisionUtcOffset_ReturnsExpected(DateTimeUtilityTestData.TryTruncateToPrecisionUtcOffset.ValidCase testCase)
    {
        // Act
        var ok = DateTimeUtility.TryTruncateToPrecisionUtc(testCase.Value.Value, testCase.Value.Precision, out var truncated);

        // Assert
        Assert.Equal(testCase.Expected, ok);
        Assert.Equal(testCase.ExpectedOutValue, truncated);
    }

    [Theory]
    [MemberData(nameof(DateTimeUtilityTestData.TryTruncateToPrecision.ValidCases), MemberType = typeof(DateTimeUtilityTestData.TryTruncateToPrecision))]
    [MemberData(nameof(DateTimeUtilityTestData.TryTruncateToPrecision.EdgeCases), MemberType = typeof(DateTimeUtilityTestData.TryTruncateToPrecision))]
    public void TryTruncateToPrecision_ReturnsExpected(DateTimeUtilityTestData.TryTruncateToPrecision.ValidCase testCase)
    {
        // Act
        var ok = DateTimeUtility.TryTruncateToPrecision(testCase.Value.Value, testCase.Value.Precision, out var truncated);

        // Assert
        Assert.Equal(testCase.Expected, ok);
        Assert.Equal(testCase.ExpectedOutValue, truncated);
    }
}
