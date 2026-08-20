using PineGuard.Testing.UnitTests;
using PineGuard.Utils;

namespace PineGuard.Core.UnitTests.Utils;

public sealed class StringUtilityTests : BaseUnitTest
{
    [Theory]
    [MemberData(nameof(StringUtilityTestData.TryGetTrimmed.ValidCases), MemberType = typeof(StringUtilityTestData.TryGetTrimmed))]
    [MemberData(nameof(StringUtilityTestData.TryGetTrimmed.EdgeCases), MemberType = typeof(StringUtilityTestData.TryGetTrimmed))]
    public void TryGetTrimmed_ReturnsExpected(StringUtilityTestData.TryGetTrimmed.ValidCase testCase)
    {
        // Act
        var ok = StringUtility.TryGetTrimmed(testCase.Value, out var trimmed);

        // Assert
        Assert.Equal(testCase.Expected, ok);
        Assert.Equal(testCase.ExpectedOutValue, trimmed);
    }

    [Theory]
    [MemberData(nameof(StringUtilityTestData.TryParseDigitsOnly.ValidCases), MemberType = typeof(StringUtilityTestData.TryParseDigitsOnly))]
    [MemberData(nameof(StringUtilityTestData.TryParseDigitsOnly.EdgeCases), MemberType = typeof(StringUtilityTestData.TryParseDigitsOnly))]
    public void TryParseDigitsOnly_ReturnsExpected(StringUtilityTestData.TryParseDigitsOnly.ValidCase testCase)
    {
        // Act
        var ok = StringUtility.TryParseDigitsOnly(testCase.Value, out var digits);

        // Assert
        Assert.Equal(testCase.Expected, ok);
        Assert.Equal(testCase.ExpectedOutValue, digits);
    }

    [Theory]
    [MemberData(nameof(StringUtilityTestData.TryParseDigits.ValidCases), MemberType = typeof(StringUtilityTestData.TryParseDigits))]
    [MemberData(nameof(StringUtilityTestData.TryParseDigits.EdgeCases), MemberType = typeof(StringUtilityTestData.TryParseDigits))]
    public void TryParseDigits_ReturnsExpected(StringUtilityTestData.TryParseDigits.ValidCase testCase)
    {
        // Act
        var ok = StringUtility.TryParseDigits(testCase.Value, out var digits, testCase.AllowedSeparators);

        // Assert
        Assert.Equal(testCase.Expected, ok);
        Assert.Equal(testCase.ExpectedOutValue, digits);
    }

    [Theory]
    [MemberData(nameof(StringUtilityTestData.TitleCase.ValidCases), MemberType = typeof(StringUtilityTestData.TitleCase))]
    [MemberData(nameof(StringUtilityTestData.TitleCase.EdgeCases), MemberType = typeof(StringUtilityTestData.TitleCase))]
    public void TitleCase_WithOutput_ReturnsExpected(StringUtilityTestData.TitleCase.ValidCase testCase)
    {
        // Act
        var ok = StringUtility.TitleCase(testCase.Value, out var titleCased);

        // Assert
        Assert.Equal(testCase.Expected, ok);
        Assert.Equal(testCase.ExpectedOutValue, titleCased);
    }

    [Theory]
    [MemberData(nameof(StringUtilityTestData.TitleCase.ValidCases), MemberType = typeof(StringUtilityTestData.TitleCase))]
    [MemberData(nameof(StringUtilityTestData.TitleCase.EdgeCases), MemberType = typeof(StringUtilityTestData.TitleCase))]
    public void TitleCase_NoOutput_ReturnsExpected(StringUtilityTestData.TitleCase.ValidCase testCase)
    {
        // Act
        var ok = StringUtility.TitleCase(testCase.Value);

        // Assert
        Assert.Equal(testCase.Expected, ok);
    }

    [Theory]
    [MemberData(nameof(StringUtilityTestData.Bool.ValidCases), MemberType = typeof(StringUtilityTestData.Bool))]
    [MemberData(nameof(StringUtilityTestData.Bool.EdgeCases), MemberType = typeof(StringUtilityTestData.Bool))]
    public void BoolTryParse_ReturnsExpected(StringUtilityTestData.Bool.ValidCase testCase)
    {
        // Act
        var ok = StringUtility.Bool.TryParse(testCase.Value, out var boolean);

        // Assert
        Assert.Equal(testCase.Expected, ok);
        Assert.Equal(testCase.ExpectedOutValue, boolean);
    }

    [Theory]
    [MemberData(nameof(StringUtilityTestData.TimeOnlyTryParse.ValidCases), MemberType = typeof(StringUtilityTestData.TimeOnlyTryParse))]
    [MemberData(nameof(StringUtilityTestData.TimeOnlyTryParse.EdgeCases), MemberType = typeof(StringUtilityTestData.TimeOnlyTryParse))]
    public void TimeOnlyTryParse_ReturnsExpected(StringUtilityTestData.TimeOnlyTryParse.ValidCase testCase)
    {
        // Act
        var ok = StringUtility.TimeOnly.TryParse(testCase.Value, out var time);

        // Assert
        Assert.Equal(testCase.Expected, ok);
        Assert.Equal(testCase.ExpectedOutValue, time);
    }

    [Theory]
    [MemberData(nameof(StringUtilityTestData.TimeSpanTryParse.ValidCases), MemberType = typeof(StringUtilityTestData.TimeSpanTryParse))]
    [MemberData(nameof(StringUtilityTestData.TimeSpanTryParse.EdgeCases), MemberType = typeof(StringUtilityTestData.TimeSpanTryParse))]
    public void TimeSpanTryParse_ReturnsExpected(StringUtilityTestData.TimeSpanTryParse.ValidCase testCase)
    {
        // Act
        var ok = StringUtility.TimeSpan.TryParse(testCase.Value, out var timeSpan);

        // Assert
        Assert.Equal(testCase.Expected, ok);
        Assert.Equal(testCase.ExpectedOutValue, timeSpan);
    }

    [Theory]
    [MemberData(nameof(StringUtilityTestData.DateTimeOffsetTryParse.ValidCases), MemberType = typeof(StringUtilityTestData.DateTimeOffsetTryParse))]
    [MemberData(nameof(StringUtilityTestData.DateTimeOffsetTryParse.EdgeCases), MemberType = typeof(StringUtilityTestData.DateTimeOffsetTryParse))]
    public void DateTimeOffsetTryParse_ReturnsExpected(StringUtilityTestData.DateTimeOffsetTryParse.ValidCase testCase)
    {
        // Act
        var ok = StringUtility.DateTimeOffset.TryParse(testCase.Value, out var dateTimeOffset);

        // Assert
        Assert.Equal(testCase.Expected, ok);
        Assert.Equal(testCase.ExpectedOutValue, dateTimeOffset);
    }

    [Theory]
    [MemberData(nameof(StringUtilityTestData.DateOnlyRangeTryParse.ValidCases), MemberType = typeof(StringUtilityTestData.DateOnlyRangeTryParse))]
    [MemberData(nameof(StringUtilityTestData.DateOnlyRangeTryParse.EdgeCases), MemberType = typeof(StringUtilityTestData.DateOnlyRangeTryParse))]
    public void DateOnlyRangeTryParse_ReturnsExpected(StringUtilityTestData.DateOnlyRangeTryParse.ValidCase testCase)
    {
        // Act
        var ok = StringUtility.DateOnlyRange.TryParse(testCase.Value.Start, testCase.Value.End, out var range);

        // Assert
        Assert.Equal(testCase.Expected, ok);
        Assert.Equal(testCase.ExpectedOutValue, range);
    }

    [Theory]
    [MemberData(nameof(StringUtilityTestData.TimeOnlyRangeTryParse.ValidCases), MemberType = typeof(StringUtilityTestData.TimeOnlyRangeTryParse))]
    [MemberData(nameof(StringUtilityTestData.TimeOnlyRangeTryParse.EdgeCases), MemberType = typeof(StringUtilityTestData.TimeOnlyRangeTryParse))]
    public void TimeOnlyRangeTryParse_ReturnsExpected(StringUtilityTestData.TimeOnlyRangeTryParse.ValidCase testCase)
    {
        // Act
        var ok = StringUtility.TimeOnlyRange.TryParse(testCase.Value.Start, testCase.Value.End, out var range);

        // Assert
        Assert.Equal(testCase.Expected, ok);
        Assert.Equal(testCase.ExpectedOutValue, range);
    }

    [Theory]
    [MemberData(nameof(StringUtilityTestData.DateTimeRangeTryParse.ValidCases), MemberType = typeof(StringUtilityTestData.DateTimeRangeTryParse))]
    [MemberData(nameof(StringUtilityTestData.DateTimeRangeTryParse.EdgeCases), MemberType = typeof(StringUtilityTestData.DateTimeRangeTryParse))]
    public void DateTimeRangeTryParse_ReturnsExpected(StringUtilityTestData.DateTimeRangeTryParse.ValidCase testCase)
    {
        // Act
        var ok = StringUtility.DateTimeRange.TryParse(testCase.Value.Start, testCase.Value.End, out var range);

        // Assert
        Assert.Equal(testCase.Expected, ok);
        Assert.Equal(testCase.ExpectedOutValue, range);
    }

    [Theory]
    [MemberData(nameof(StringUtilityTestData.DateTimeOffsetRangeTryParse.ValidCases), MemberType = typeof(StringUtilityTestData.DateTimeOffsetRangeTryParse))]
    [MemberData(nameof(StringUtilityTestData.DateTimeOffsetRangeTryParse.EdgeCases), MemberType = typeof(StringUtilityTestData.DateTimeOffsetRangeTryParse))]
    public void DateTimeOffsetRangeTryParse_ReturnsExpected(StringUtilityTestData.DateTimeOffsetRangeTryParse.ValidCase testCase)
    {
        // Act
        var ok = StringUtility.DateTimeOffsetRange.TryParse(testCase.Value.Start, testCase.Value.End, out var range);

        // Assert
        Assert.Equal(testCase.Expected, ok);
        Assert.Equal(testCase.ExpectedOutValue, range);
    }
}
