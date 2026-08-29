using PineGuard.Common;
using PineGuard.Testing.UnitTests.Rules;
using Xunit.Abstractions;

namespace PineGuard.Core.UnitTests.Rules.StringRules;

public sealed class StringRulesNumbersTests(ITestOutputHelper output) : BaseRuleUnitTest(output)
{
    [Theory]
    [MemberData(nameof(StringRulesNumbersTestData.IsPositive.Cases), MemberType = typeof(StringRulesNumbersTestData.IsPositive))]
    public void IsPositive_BehavesAsExpected(RuleCase<string?> tc)
    {
        // Act
        var result = PineGuard.Rules.StringRules.Numbers.IsPositive(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringRulesNumbersTestData.IsNegative.Cases), MemberType = typeof(StringRulesNumbersTestData.IsNegative))]
    public void IsNegative_BehavesAsExpected(RuleCase<string?> tc)
    {
        // Act
        var result = PineGuard.Rules.StringRules.Numbers.IsNegative(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringRulesNumbersTestData.IsZero.Cases), MemberType = typeof(StringRulesNumbersTestData.IsZero))]
    public void IsZero_BehavesAsExpected(RuleCase<string?> tc)
    {
        // Act
        var result = PineGuard.Rules.StringRules.Numbers.IsZero(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringRulesNumbersTestData.IsNotZero.Cases), MemberType = typeof(StringRulesNumbersTestData.IsNotZero))]
    public void IsNotZero_BehavesAsExpected(RuleCase<string?> tc)
    {
        // Act
        var result = PineGuard.Rules.StringRules.Numbers.IsNotZero(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringRulesNumbersTestData.IsZeroOrPositive.Cases), MemberType = typeof(StringRulesNumbersTestData.IsZeroOrPositive))]
    public void IsZeroOrPositive_BehavesAsExpected(RuleCase<string?> tc)
    {
        // Act
        var result = PineGuard.Rules.StringRules.Numbers.IsZeroOrPositive(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringRulesNumbersTestData.IsZeroOrNegative.Cases), MemberType = typeof(StringRulesNumbersTestData.IsZeroOrNegative))]
    public void IsZeroOrNegative_BehavesAsExpected(RuleCase<string?> tc)
    {
        // Act
        var result = PineGuard.Rules.StringRules.Numbers.IsZeroOrNegative(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringRulesNumbersTestData.IsGreaterThan.Cases), MemberType = typeof(StringRulesNumbersTestData.IsGreaterThan))]
    public void IsGreaterThan_BehavesAsExpected(RuleCase<(string? text, decimal min)> tc)
    {
        // Act
        var result = PineGuard.Rules.StringRules.Numbers.IsGreaterThan(tc.Value.text, tc.Value.min);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringRulesNumbersTestData.IsGreaterThanOrEqual.Cases), MemberType = typeof(StringRulesNumbersTestData.IsGreaterThanOrEqual))]
    public void IsGreaterThanOrEqual_BehavesAsExpected(RuleCase<(string? text, decimal min)> tc)
    {
        // Act
        var result = PineGuard.Rules.StringRules.Numbers.IsGreaterThanOrEqual(tc.Value.text, tc.Value.min);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringRulesNumbersTestData.IsLessThan.Cases), MemberType = typeof(StringRulesNumbersTestData.IsLessThan))]
    public void IsLessThan_BehavesAsExpected(RuleCase<(string? text, decimal max)> tc)
    {
        // Act
        var result = PineGuard.Rules.StringRules.Numbers.IsLessThan(tc.Value.text, tc.Value.max);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringRulesNumbersTestData.IsLessThanOrEqual.Cases), MemberType = typeof(StringRulesNumbersTestData.IsLessThanOrEqual))]
    public void IsLessThanOrEqual_BehavesAsExpected(RuleCase<(string? text, decimal max)> tc)
    {
        // Act
        var result = PineGuard.Rules.StringRules.Numbers.IsLessThanOrEqual(tc.Value.text, tc.Value.max);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringRulesNumbersTestData.IsInRange.Cases), MemberType = typeof(StringRulesNumbersTestData.IsInRange))]
    public void IsInRange_BehavesAsExpected(RuleCase<(string? text, decimal min, decimal max, Inclusion inclusion)> tc)
    {
        // Act
        var result = PineGuard.Rules.StringRules.Numbers.IsInRange(tc.Value.text, tc.Value.min, tc.Value.max, tc.Value.inclusion);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringRulesNumbersTestData.IsPercentage.Cases), MemberType = typeof(StringRulesNumbersTestData.IsPercentage))]
    public void IsPercentage_BehavesAsExpected(RuleCase<string?> tc)
    {
        // Act
        var result = PineGuard.Rules.StringRules.Numbers.IsPercentage(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringRulesNumbersTestData.IsApproximately.Cases), MemberType = typeof(StringRulesNumbersTestData.IsApproximately))]
    public void IsApproximately_BehavesAsExpected(RuleCase<(string? text, decimal target, decimal? tolerance)> tc)
    {
        // Act
        var result = PineGuard.Rules.StringRules.Numbers.IsApproximately(tc.Value.text, tc.Value.target, tc.Value.tolerance);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringRulesNumbersTestData.IsMultipleOf.Cases), MemberType = typeof(StringRulesNumbersTestData.IsMultipleOf))]
    public void IsMultipleOf_BehavesAsExpected(RuleCase<(string? text, decimal factor)> tc)
    {
        // Act
        var result = PineGuard.Rules.StringRules.Numbers.IsMultipleOf(tc.Value.text, tc.Value.factor);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringRulesNumbersTestData.IsEven.Cases), MemberType = typeof(StringRulesNumbersTestData.IsEven))]
    public void IsEven_BehavesAsExpected(RuleCase<string?> tc)
    {
        // Act
        var result = PineGuard.Rules.StringRules.Numbers.IsEven(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringRulesNumbersTestData.IsOdd.Cases), MemberType = typeof(StringRulesNumbersTestData.IsOdd))]
    public void IsOdd_BehavesAsExpected(RuleCase<string?> tc)
    {
        // Act
        var result = PineGuard.Rules.StringRules.Numbers.IsOdd(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringRulesNumbersTestData.IsFinite.Cases), MemberType = typeof(StringRulesNumbersTestData.IsFinite))]
    public void IsFinite_BehavesAsExpected(RuleCase<string?> tc)
    {
        // Act
        var result = PineGuard.Rules.StringRules.Numbers.IsFinite(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringRulesNumbersTestData.IsNaN.Cases), MemberType = typeof(StringRulesNumbersTestData.IsNaN))]
    public void IsNaN_BehavesAsExpected(RuleCase<string?> tc)
    {
        // Act
        var result = PineGuard.Rules.StringRules.Numbers.IsNaN(tc.Value);

        // Assert
        AssertResult(tc, result);
    }
}
