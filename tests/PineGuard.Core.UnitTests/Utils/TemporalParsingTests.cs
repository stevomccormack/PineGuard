using System.Globalization;
using PineGuard.Testing.UnitTests.Rules;
using PineGuard.Utils;
using Xunit.Abstractions;

namespace PineGuard.Core.UnitTests.Utils;

public sealed class TemporalParsingTests(ITestOutputHelper output)
    : BaseRuleUnitTest(output)
{
    [Theory]
    [MemberData(nameof(TemporalParsingTestData.Styles.DateAndTimeOnlyCases), MemberType = typeof(TemporalParsingTestData.Styles))]
    public void DateOnly_ValidatesStylesBeforeParsing(RuleCase<DateTimeStyles> tc)
    {
        // Act
        var validStyles = StringUtility.DateOnly.IsValidStyles(tc.Value);
        var parsed = StringUtility.DateOnly.TryParse("2024-01-15", out var value, tc.Value);
        var parsedNull = StringUtility.DateOnly.TryParse(null, out var nullValue, tc.Value);

        // Assert
        AssertResult(tc, validStyles);
        AssertResult(tc, parsed);
        if (!parsed)
            Assert.Equal(default, value);
        Assert.False(parsedNull);
        Assert.Equal(default, nullValue);
    }

    [Theory]
    [MemberData(nameof(TemporalParsingTestData.Styles.DateAndTimeOnlyCases), MemberType = typeof(TemporalParsingTestData.Styles))]
    public void TimeOnly_ValidatesStylesBeforeParsing(RuleCase<DateTimeStyles> tc)
    {
        // Act
        var validStyles = StringUtility.TimeOnly.IsValidStyles(tc.Value);
        var parsed = StringUtility.TimeOnly.TryParse("12:30:00", out var value, tc.Value);
        var parsedNull = StringUtility.TimeOnly.TryParse(null, out var nullValue, tc.Value);

        // Assert
        AssertResult(tc, validStyles);
        AssertResult(tc, parsed);
        if (!parsed)
            Assert.Equal(default, value);
        Assert.False(parsedNull);
        Assert.Equal(default, nullValue);
    }

    [Theory]
    [MemberData(nameof(TemporalParsingTestData.Styles.OffsetCases), MemberType = typeof(TemporalParsingTestData.Styles))]
    public void DateTimeOffset_ValidatesStylesBeforeParsing(RuleCase<DateTimeStyles> tc)
    {
        // Act
        var validStyles = StringUtility.DateTimeOffset.IsValidStyles(tc.Value);
        var parsed = StringUtility.DateTimeOffset.TryParse("2024-01-15T12:30:00+00:00", out var value, tc.Value);
        var parsedNull = StringUtility.DateTimeOffset.TryParse(null, out var nullValue, tc.Value);

        // Assert
        AssertResult(tc, validStyles);
        AssertResult(tc, parsed);
        if (!parsed)
            Assert.Equal(default, value);
        Assert.False(parsedNull);
        Assert.Equal(default, nullValue);
    }

}
