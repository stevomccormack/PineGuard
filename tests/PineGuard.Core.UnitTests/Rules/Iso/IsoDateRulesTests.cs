using PineGuard.Common;
using PineGuard.Rules.Iso;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Rules.Iso;

public sealed class IsoDateRulesTests : BaseUnitTest
{
    [Theory]
    [MemberData(nameof(IsoDateRulesTestData.IsIsoDateOnly.ValidCases), MemberType = typeof(IsoDateRulesTestData.IsIsoDateOnly))]
    public void IsIsoDateOnly_ReturnsTrue_ForValid(IsoDateRulesTestData.IsIsoDateOnly.Case testCase)
    {
        var result = IsoDateRules.IsIsoDateOnly(testCase.Value);

        Assert.Equal(testCase.Expected, result);
        Assert.True(result);
    }

    [Theory]
    [MemberData(nameof(IsoDateRulesTestData.IsIsoDateOnly.EdgeCases), MemberType = typeof(IsoDateRulesTestData.IsIsoDateOnly))]
    public void IsIsoDateOnly_ReturnsFalse_ForInvalid(IsoDateRulesTestData.IsIsoDateOnly.Case testCase)
    {
        var result = IsoDateRules.IsIsoDateOnly(testCase.Value);

        Assert.Equal(testCase.Expected, result);
        Assert.False(result);
    }

    [Theory]
    [MemberData(nameof(IsoDateRulesTestData.IsIsoDateTime.ValidCases), MemberType = typeof(IsoDateRulesTestData.IsIsoDateTime))]
    public void IsIsoDateTime_ReturnsTrue_ForValid(IsoDateRulesTestData.IsIsoDateTime.Case testCase)
    {
        var result = IsoDateRules.IsIsoDateTime(testCase.Value);

        Assert.Equal(testCase.Expected, result);
        Assert.True(result);
    }

    [Theory]
    [MemberData(nameof(IsoDateRulesTestData.IsIsoDateTime.EdgeCases), MemberType = typeof(IsoDateRulesTestData.IsIsoDateTime))]
    public void IsIsoDateTime_ReturnsFalse_ForInvalid(IsoDateRulesTestData.IsIsoDateTime.Case testCase)
    {
        var result = IsoDateRules.IsIsoDateTime(testCase.Value);

        Assert.Equal(testCase.Expected, result);
        Assert.False(result);
    }

    [Theory]
    [MemberData(nameof(IsoDateRulesTestData.IsIsoDateTimeOffset.ValidCases), MemberType = typeof(IsoDateRulesTestData.IsIsoDateTimeOffset))]
    public void IsIsoDateTimeOffset_ReturnsTrue_ForValid(IsoDateRulesTestData.IsIsoDateTimeOffset.Case testCase)
    {
        var result = IsoDateRules.IsIsoDateTimeOffset(testCase.Value);

        Assert.Equal(testCase.Expected, result);
        Assert.True(result);
    }

    [Theory]
    [MemberData(nameof(IsoDateRulesTestData.IsIsoDateTimeOffset.EdgeCases), MemberType = typeof(IsoDateRulesTestData.IsIsoDateTimeOffset))]
    public void IsIsoDateTimeOffset_ReturnsFalse_ForInvalid(IsoDateRulesTestData.IsIsoDateTimeOffset.Case testCase)
    {
        var result = IsoDateRules.IsIsoDateTimeOffset(testCase.Value);

        Assert.Equal(testCase.Expected, result);
        Assert.False(result);
    }

    [Theory]
    [MemberData(nameof(IsoDateRulesTestData.StringDateOnlyIsBetween.Cases), MemberType = typeof(IsoDateRulesTestData.StringDateOnlyIsBetween))]
    public void StringDateOnly_IsBetween_ReturnsExpected(IsoDateRulesTestData.StringDateOnlyIsBetween.Case testCase)
    {
        var result = PineGuard.Rules.StringRules.DateOnly.IsBetween(testCase.Value, testCase.Min, testCase.Max, testCase.Inclusion);

        Assert.Equal(testCase.Expected, result);
    }
}
