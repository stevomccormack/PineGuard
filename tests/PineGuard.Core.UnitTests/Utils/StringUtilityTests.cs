using PineGuard.Testing.UnitTests;
using PineGuard.Utils;

namespace PineGuard.Core.UnitTests.Utils;

public sealed class StringUtilityTests : BaseUnitTest
{
    [Theory]
    [MemberData(nameof(StringUtilityTestData.TryGetTrimmed.ValidCases), MemberType = typeof(StringUtilityTestData.TryGetTrimmed))]
    [MemberData(nameof(StringUtilityTestData.TryGetTrimmed.EdgeCases), MemberType = typeof(StringUtilityTestData.TryGetTrimmed))]
    public void TryGetTrimmed_ReturnsExpected(StringUtilityTestData.TryGetTrimmed.Case testCase)
    {
        // Act
        var ok = StringUtility.TryGetTrimmed(testCase.Value, out var trimmed);

        // Assert
        Assert.Equal(testCase.ExpectedReturn, ok);
        Assert.Equal(testCase.ExpectedOutValue, trimmed);
    }

    [Theory]
    [MemberData(nameof(StringUtilityTestData.TryParseDigitsOnly.ValidCases), MemberType = typeof(StringUtilityTestData.TryParseDigitsOnly))]
    [MemberData(nameof(StringUtilityTestData.TryParseDigitsOnly.EdgeCases), MemberType = typeof(StringUtilityTestData.TryParseDigitsOnly))]
    public void TryParseDigitsOnly_ReturnsExpected(StringUtilityTestData.TryParseDigitsOnly.Case testCase)
    {
        // Act
        var ok = StringUtility.TryParseDigitsOnly(testCase.Value, out var digits);

        // Assert
        Assert.Equal(testCase.ExpectedReturn, ok);
        Assert.Equal(testCase.ExpectedOutValue, digits);
    }

    [Theory]
    [MemberData(nameof(StringUtilityTestData.TryParseDigits.ValidCases), MemberType = typeof(StringUtilityTestData.TryParseDigits))]
    [MemberData(nameof(StringUtilityTestData.TryParseDigits.EdgeCases), MemberType = typeof(StringUtilityTestData.TryParseDigits))]
    public void TryParseDigits_ReturnsExpected(StringUtilityTestData.TryParseDigits.Case testCase)
    {
        // Act
        var ok = StringUtility.TryParseDigits(testCase.Value, out var digits, testCase.AllowedSeparators);

        // Assert
        Assert.Equal(testCase.ExpectedReturn, ok);
        Assert.Equal(testCase.ExpectedOutValue, digits);
    }

    [Theory]
    [MemberData(nameof(StringUtilityTestData.TitleCase.ValidCases), MemberType = typeof(StringUtilityTestData.TitleCase))]
    [MemberData(nameof(StringUtilityTestData.TitleCase.EdgeCases), MemberType = typeof(StringUtilityTestData.TitleCase))]
    public void TitleCase_WithOutput_ReturnsExpected(StringUtilityTestData.TitleCase.Case testCase)
    {
        // Act
        var ok = StringUtility.TitleCase(testCase.Value, out var titleCased);

        // Assert
        Assert.Equal(testCase.ExpectedReturn, ok);
        Assert.Equal(testCase.ExpectedOutValue, titleCased);
    }

    [Theory]
    [MemberData(nameof(StringUtilityTestData.TitleCase.ValidCases), MemberType = typeof(StringUtilityTestData.TitleCase))]
    [MemberData(nameof(StringUtilityTestData.TitleCase.EdgeCases), MemberType = typeof(StringUtilityTestData.TitleCase))]
    public void TitleCase_NoOutput_ReturnsExpected(StringUtilityTestData.TitleCase.Case testCase)
    {
        // Act
        var ok = StringUtility.TitleCase(testCase.Value);

        // Assert
        Assert.Equal(testCase.ExpectedReturn, ok);
    }

    [Theory]
    [MemberData(nameof(StringUtilityTestData.TimeOnlyTryParse.ValidCases), MemberType = typeof(StringUtilityTestData.TimeOnlyTryParse))]
    [MemberData(nameof(StringUtilityTestData.TimeOnlyTryParse.EdgeCases), MemberType = typeof(StringUtilityTestData.TimeOnlyTryParse))]
    public void TimeOnlyTryParse_ReturnsExpected(StringUtilityTestData.TimeOnlyTryParse.Case testCase)
    {
        var ok = StringUtility.TimeOnly.TryParse(testCase.Value, out var time);

        Assert.Equal(testCase.ExpectedReturn, ok);
        Assert.Equal(testCase.ExpectedOutValue, time);
    }

    [Theory]
    [MemberData(nameof(StringUtilityTestData.TimeSpanTryParse.ValidCases), MemberType = typeof(StringUtilityTestData.TimeSpanTryParse))]
    [MemberData(nameof(StringUtilityTestData.TimeSpanTryParse.EdgeCases), MemberType = typeof(StringUtilityTestData.TimeSpanTryParse))]
    public void TimeSpanTryParse_ReturnsExpected(StringUtilityTestData.TimeSpanTryParse.Case testCase)
    {
        var ok = StringUtility.TimeSpan.TryParse(testCase.Value, out var timeSpan);

        Assert.Equal(testCase.ExpectedReturn, ok);
        Assert.Equal(testCase.ExpectedOutValue, timeSpan);
    }
}
