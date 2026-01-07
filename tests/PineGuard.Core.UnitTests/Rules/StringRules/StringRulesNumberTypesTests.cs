using PineGuard.Common;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Rules.StringRules;

public sealed class StringRulesNumberTypesTests : BaseUnitTest
{
    [Theory]
    [MemberData(nameof(StringRulesNumberTypesTestData.IsDecimal.ValidCases), MemberType = typeof(StringRulesNumberTypesTestData.IsDecimal))]
    [MemberData(nameof(StringRulesNumberTypesTestData.IsDecimal.EdgeCases), MemberType = typeof(StringRulesNumberTypesTestData.IsDecimal))]
    public void IsDecimal_ReturnsExpected(StringRulesNumberTypesTestData.IsDecimal.Case testCase)
    {
        var result = PineGuard.Rules.StringRules.NumberTypes.IsDecimal(testCase.Value, decimalPlaces: 2);

        Assert.Equal(testCase.Expected, result);
    }

    [Theory]
    [MemberData(nameof(StringRulesNumberTypesTestData.IsDecimalWithZeroPlaces.ValidCases), MemberType = typeof(StringRulesNumberTypesTestData.IsDecimalWithZeroPlaces))]
    [MemberData(nameof(StringRulesNumberTypesTestData.IsDecimalWithZeroPlaces.EdgeCases), MemberType = typeof(StringRulesNumberTypesTestData.IsDecimalWithZeroPlaces))]
    public void IsDecimal_WithZeroPlaces_UsesSignedInteger(StringRulesNumberTypesTestData.IsDecimalWithZeroPlaces.Case testCase)
    {
        var result = PineGuard.Rules.StringRules.NumberTypes.IsDecimal(testCase.Value, decimalPlaces: 0);

        Assert.Equal(testCase.Expected, result);
    }

    [Fact]
    public void IsDecimal_ReturnsFalse_WhenDecimalPlacesNegative()
    {
        Assert.False(PineGuard.Rules.StringRules.NumberTypes.IsDecimal("1.23", decimalPlaces: -1));
    }

    [Theory]
    [MemberData(nameof(StringRulesNumberTypesTestData.IsExactDecimal.ValidCases), MemberType = typeof(StringRulesNumberTypesTestData.IsExactDecimal))]
    [MemberData(nameof(StringRulesNumberTypesTestData.IsExactDecimal.EdgeCases), MemberType = typeof(StringRulesNumberTypesTestData.IsExactDecimal))]
    public void IsExactDecimal_ReturnsExpected(StringRulesNumberTypesTestData.IsExactDecimal.Case testCase)
    {
        var result = PineGuard.Rules.StringRules.NumberTypes.IsExactDecimal(testCase.Value, exactDecimalPlaces: 2);

        Assert.Equal(testCase.Expected, result);
    }

    [Theory]
    [MemberData(nameof(StringRulesNumberTypesTestData.IsExactDecimalWithZeroPlaces.ValidCases), MemberType = typeof(StringRulesNumberTypesTestData.IsExactDecimalWithZeroPlaces))]
    [MemberData(nameof(StringRulesNumberTypesTestData.IsExactDecimalWithZeroPlaces.EdgeCases), MemberType = typeof(StringRulesNumberTypesTestData.IsExactDecimalWithZeroPlaces))]
    public void IsExactDecimal_WithZeroPlaces_UsesSignedInteger(
        StringRulesNumberTypesTestData.IsExactDecimalWithZeroPlaces.Case testCase)
    {
        var result = PineGuard.Rules.StringRules.NumberTypes.IsExactDecimal(testCase.Value, exactDecimalPlaces: 0);

        Assert.Equal(testCase.Expected, result);
    }

    [Fact]
    public void IsExactDecimal_ReturnsFalse_WhenPlacesNegative()
    {
        Assert.False(PineGuard.Rules.StringRules.NumberTypes.IsExactDecimal("1.23", exactDecimalPlaces: -1));
    }

    [Theory]
    [MemberData(nameof(StringRulesNumberTypesTestData.IsInt32.ValidCases), MemberType = typeof(StringRulesNumberTypesTestData.IsInt32))]
    [MemberData(nameof(StringRulesNumberTypesTestData.IsInt32.EdgeCases), MemberType = typeof(StringRulesNumberTypesTestData.IsInt32))]
    public void IsInt32_ReturnsExpected(StringRulesNumberTypesTestData.IsInt32.Case testCase)
    {
        var result = PineGuard.Rules.StringRules.NumberTypes.IsInt32(testCase.Value);

        Assert.Equal(testCase.Expected, result);
    }

    [Theory]
    [MemberData(nameof(StringRulesNumberTypesTestData.IsInt64.ValidCases), MemberType = typeof(StringRulesNumberTypesTestData.IsInt64))]
    [MemberData(nameof(StringRulesNumberTypesTestData.IsInt64.EdgeCases), MemberType = typeof(StringRulesNumberTypesTestData.IsInt64))]
    public void IsInt64_ReturnsExpected(StringRulesNumberTypesTestData.IsInt64.Case testCase)
    {
        var result = PineGuard.Rules.StringRules.NumberTypes.IsInt64(testCase.Value);

        Assert.Equal(testCase.Expected, result);
    }

    [Theory]
    [MemberData(nameof(StringRulesNumberTypesTestData.IsInt32InRange.ValidCases), MemberType = typeof(StringRulesNumberTypesTestData.IsInt32InRange))]
    [MemberData(nameof(StringRulesNumberTypesTestData.IsInt32InRange.EdgeCases), MemberType = typeof(StringRulesNumberTypesTestData.IsInt32InRange))]
    public void IsInt32InRange_ReturnsExpected(StringRulesNumberTypesTestData.IsInt32InRange.Case testCase)
    {
        var result = PineGuard.Rules.StringRules.NumberTypes.IsInt32InRange(
            testCase.Value,
            testCase.Min,
            testCase.Max,
            testCase.Inclusion);

        Assert.Equal(testCase.Expected, result);
    }

    [Theory]
    [MemberData(nameof(StringRulesNumberTypesTestData.IsInt64InRange.ValidCases), MemberType = typeof(StringRulesNumberTypesTestData.IsInt64InRange))]
    [MemberData(nameof(StringRulesNumberTypesTestData.IsInt64InRange.EdgeCases), MemberType = typeof(StringRulesNumberTypesTestData.IsInt64InRange))]
    public void IsInt64InRange_ReturnsExpected(StringRulesNumberTypesTestData.IsInt64InRange.Case testCase)
    {
        var result = PineGuard.Rules.StringRules.NumberTypes.IsInt64InRange(
            testCase.Value,
            testCase.Min,
            testCase.Max,
            testCase.Inclusion);

        Assert.Equal(testCase.Expected, result);
    }
}
