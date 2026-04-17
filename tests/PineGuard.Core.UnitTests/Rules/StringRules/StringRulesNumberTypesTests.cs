using PineGuard.Common;
using PineGuard.Testing.UnitTests.Rules;
using Xunit.Abstractions;

namespace PineGuard.Core.UnitTests.Rules.StringRules;

public sealed class StringRulesNumberTypesTests(ITestOutputHelper output) : BaseRuleUnitTest(output)
{
    [Theory]
    [MemberData(nameof(StringRulesNumberTypesTestData.IsDecimal.Cases), MemberType = typeof(StringRulesNumberTypesTestData.IsDecimal))]
    public void IsDecimal_BehavesAsExpected(RuleCase<string?> tc)
    {
        // Act
        var result = PineGuard.Rules.StringRules.NumberTypes.IsDecimal(tc.Value, decimalPlaces: 2);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringRulesNumberTypesTestData.IsDecimalWithZeroPlaces.Cases), MemberType = typeof(StringRulesNumberTypesTestData.IsDecimalWithZeroPlaces))]
    public void IsDecimal_WithZeroPlaces_BehavesAsExpected(RuleCase<string?> tc)
    {
        // Act
        var result = PineGuard.Rules.StringRules.NumberTypes.IsDecimal(tc.Value, decimalPlaces: 0);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringRulesNumberTypesTestData.IsDecimalNegativePlaces.Cases), MemberType = typeof(StringRulesNumberTypesTestData.IsDecimalNegativePlaces))]
    public void IsDecimal_ReturnsFalse_WhenDecimalPlacesNegative(RuleCase<(string? value, int decimalPlaces)> tc)
    {
        // Arrange
        var (value, decimalPlaces) = tc.Value;

        // Act
        var result = PineGuard.Rules.StringRules.NumberTypes.IsDecimal(value, decimalPlaces: decimalPlaces);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringRulesNumberTypesTestData.IsExactDecimal.Cases), MemberType = typeof(StringRulesNumberTypesTestData.IsExactDecimal))]
    public void IsExactDecimal_BehavesAsExpected(RuleCase<string?> tc)
    {
        // Act
        var result = PineGuard.Rules.StringRules.NumberTypes.IsExactDecimal(tc.Value, exactDecimalPlaces: 2);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringRulesNumberTypesTestData.IsExactDecimalWithZeroPlaces.Cases), MemberType = typeof(StringRulesNumberTypesTestData.IsExactDecimalWithZeroPlaces))]
    public void IsExactDecimal_WithZeroPlaces_BehavesAsExpected(RuleCase<string?> tc)
    {
        // Act
        var result = PineGuard.Rules.StringRules.NumberTypes.IsExactDecimal(tc.Value, exactDecimalPlaces: 0);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringRulesNumberTypesTestData.IsExactDecimalNegativePlaces.Cases), MemberType = typeof(StringRulesNumberTypesTestData.IsExactDecimalNegativePlaces))]
    public void IsExactDecimal_ReturnsFalse_WhenPlacesNegative(RuleCase<(string? value, int exactDecimalPlaces)> tc)
    {
        // Arrange
        var (value, exactDecimalPlaces) = tc.Value;

        // Act
        var result = PineGuard.Rules.StringRules.NumberTypes.IsExactDecimal(value, exactDecimalPlaces: exactDecimalPlaces);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringRulesNumberTypesTestData.IsInt32.Cases), MemberType = typeof(StringRulesNumberTypesTestData.IsInt32))]
    public void IsInt32_BehavesAsExpected(RuleCase<string?> tc)
    {
        // Act
        var result = PineGuard.Rules.StringRules.NumberTypes.IsInt32(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringRulesNumberTypesTestData.IsInt64.Cases), MemberType = typeof(StringRulesNumberTypesTestData.IsInt64))]
    public void IsInt64_BehavesAsExpected(RuleCase<string?> tc)
    {
        // Act
        var result = PineGuard.Rules.StringRules.NumberTypes.IsInt64(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringRulesNumberTypesTestData.IsInt32InRange.Cases), MemberType = typeof(StringRulesNumberTypesTestData.IsInt32InRange))]
    public void IsInt32InRange_BehavesAsExpected(RuleCase<(string text, int min, int max, Inclusion inclusion)> tc)
    {
        // Arrange
        var (text, min, max, inclusion) = tc.Value;

        // Act
        var result = PineGuard.Rules.StringRules.NumberTypes.IsInt32InRange(text, min, max, inclusion);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringRulesNumberTypesTestData.IsInt64InRange.Cases), MemberType = typeof(StringRulesNumberTypesTestData.IsInt64InRange))]
    public void IsInt64InRange_BehavesAsExpected(RuleCase<(string text, long min, long max, Inclusion inclusion)> tc)
    {
        // Arrange
        var (text, min, max, inclusion) = tc.Value;

        // Act
        var result = PineGuard.Rules.StringRules.NumberTypes.IsInt64InRange(text, min, max, inclusion);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringRulesNumberTypesTestData.SignedIntegerRegex.ValidCases), MemberType = typeof(StringRulesNumberTypesTestData.SignedIntegerRegex))]
    [MemberData(nameof(StringRulesNumberTypesTestData.SignedIntegerRegex.EdgeCases), MemberType = typeof(StringRulesNumberTypesTestData.SignedIntegerRegex))]
    public void SignedIntegerRegex_BehavesAsExpected(StringRulesNumberTypesTestData.SignedIntegerRegex.ValidCase testCase)
    {
        // Act
        var result = PineGuard.Rules.StringRules.NumberTypes.SignedIntegerRegex().IsMatch(testCase.Value);

        // Assert
        Assert.Equal(testCase.Expected, result);
    }
}
