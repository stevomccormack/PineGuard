using PineGuard.Rules;
using PineGuard.Testing.UnitTests.Rules;
using Xunit.Abstractions;

namespace PineGuard.Core.UnitTests.Rules;

public sealed class NumberRulesTests(ITestOutputHelper output) : BaseRuleUnitTest(output)
{
    [Theory]
    [MemberData(nameof(NumberRulesTestData.IsPositiveInt.Cases), MemberType = typeof(NumberRulesTestData.IsPositiveInt))]
    public void IsPositive_BehavesAsExpected(RuleCase<int?> tc)
    {
        // Act
        var result = NumberRules.IsPositive(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(NumberRulesTestData.IsNegativeInt.Cases), MemberType = typeof(NumberRulesTestData.IsNegativeInt))]
    public void IsNegative_BehavesAsExpected(RuleCase<int?> tc)
    {
        // Act
        var result = NumberRules.IsNegative(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(NumberRulesTestData.IsZeroInt.Cases), MemberType = typeof(NumberRulesTestData.IsZeroInt))]
    public void IsZero_BehavesAsExpected(RuleCase<int?> tc)
    {
        // Act
        var result = NumberRules.IsZero(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(NumberRulesTestData.IsNotZeroInt.Cases), MemberType = typeof(NumberRulesTestData.IsNotZeroInt))]
    public void IsNotZero_BehavesAsExpected(RuleCase<int?> tc)
    {
        // Act
        var result = NumberRules.IsNotZero(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(NumberRulesTestData.IsZeroOrPositiveInt.Cases), MemberType = typeof(NumberRulesTestData.IsZeroOrPositiveInt))]
    public void IsZeroOrPositive_BehavesAsExpected(RuleCase<int?> tc)
    {
        // Act
        var result = NumberRules.IsZeroOrPositive(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(NumberRulesTestData.IsZeroOrNegativeInt.Cases), MemberType = typeof(NumberRulesTestData.IsZeroOrNegativeInt))]
    public void IsZeroOrNegative_BehavesAsExpected(RuleCase<int?> tc)
    {
        // Act
        var result = NumberRules.IsZeroOrNegative(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(NumberRulesTestData.IsGreaterThan.Cases), MemberType = typeof(NumberRulesTestData.IsGreaterThan))]
    public void IsGreaterThan_BehavesAsExpected(RuleCase<(int? value, int min)> tc)
    {
        // Act
        var result = NumberRules.IsGreaterThan(tc.Value.value, tc.Value.min);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(NumberRulesTestData.IsGreaterThanOrEqual.Cases), MemberType = typeof(NumberRulesTestData.IsGreaterThanOrEqual))]
    public void IsGreaterThanOrEqual_BehavesAsExpected(RuleCase<(int? value, int min)> tc)
    {
        // Act
        var result = NumberRules.IsGreaterThanOrEqual(tc.Value.value, tc.Value.min);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(NumberRulesTestData.IsLessThan.Cases), MemberType = typeof(NumberRulesTestData.IsLessThan))]
    public void IsLessThan_BehavesAsExpected(RuleCase<(int? value, int max)> tc)
    {
        // Act
        var result = NumberRules.IsLessThan(tc.Value.value, tc.Value.max);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(NumberRulesTestData.IsLessThanOrEqual.Cases), MemberType = typeof(NumberRulesTestData.IsLessThanOrEqual))]
    public void IsLessThanOrEqual_BehavesAsExpected(RuleCase<(int? value, int max)> tc)
    {
        // Act
        var result = NumberRules.IsLessThanOrEqual(tc.Value.value, tc.Value.max);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(NumberRulesTestData.IsInRange.Cases), MemberType = typeof(NumberRulesTestData.IsInRange))]
    public void IsInRange_BehavesAsExpected(RuleCase<(int? value, int min, int max, PineGuard.Common.Inclusion inclusion)> tc)
    {
        // Act
        var result = NumberRules.IsInRange(tc.Value.value, tc.Value.min, tc.Value.max, tc.Value.inclusion);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(NumberRulesTestData.IsApproximately.Cases), MemberType = typeof(NumberRulesTestData.IsApproximately))]
    public void IsApproximately_BehavesAsExpected(RuleCase<(decimal? value, decimal target, decimal? tolerance)> tc)
    {
        // Act
        var result = NumberRules.IsApproximately(tc.Value.value, tc.Value.target, tc.Value.tolerance);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(NumberRulesTestData.IsApproximatelyUnsignedUnderflow.Cases), MemberType = typeof(NumberRulesTestData.IsApproximatelyUnsignedUnderflow))]
    public void IsApproximately_UnsignedUnderflow_BehavesAsExpected(RuleCase<(uint? value, uint target, uint? tolerance)> tc)
    {
        // Act
        var result = NumberRules.IsApproximately(tc.Value.value, tc.Value.target, tc.Value.tolerance);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(NumberRulesTestData.IsApproximatelySignedOverflowGuard.Cases), MemberType = typeof(NumberRulesTestData.IsApproximatelySignedOverflowGuard))]
    public void IsApproximately_SignedOverflowGuard_BehavesAsExpected(RuleCase<(int? value, int target, int? tolerance)> tc)
    {
        // Act
        var result = NumberRules.IsApproximately(tc.Value.value, tc.Value.target, tc.Value.tolerance);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(NumberRulesTestData.IsMultipleOf.Cases), MemberType = typeof(NumberRulesTestData.IsMultipleOf))]
    public void IsMultipleOf_BehavesAsExpected(RuleCase<(int? value, int factor)> tc)
    {
        // Act
        var result = NumberRules.IsMultipleOf(tc.Value.value, tc.Value.factor);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(NumberRulesTestData.IsEvenInt.Cases), MemberType = typeof(NumberRulesTestData.IsEvenInt))]
    public void IsEven_Int_BehavesAsExpected(RuleCase<int?> tc)
    {
        // Act
        var result = NumberRules.IsEven(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(NumberRulesTestData.IsEvenLong.Cases), MemberType = typeof(NumberRulesTestData.IsEvenLong))]
    public void IsEven_Long_BehavesAsExpected(RuleCase<long?> tc)
    {
        // Act
        var result = NumberRules.IsEven(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(NumberRulesTestData.IsOddInt.Cases), MemberType = typeof(NumberRulesTestData.IsOddInt))]
    public void IsOdd_Int_BehavesAsExpected(RuleCase<int?> tc)
    {
        // Act
        var result = NumberRules.IsOdd(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(NumberRulesTestData.IsOddLong.Cases), MemberType = typeof(NumberRulesTestData.IsOddLong))]
    public void IsOdd_Long_BehavesAsExpected(RuleCase<long?> tc)
    {
        // Act
        var result = NumberRules.IsOdd(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(NumberRulesTestData.IsFiniteFloat.Cases), MemberType = typeof(NumberRulesTestData.IsFiniteFloat))]
    public void IsFinite_Float_BehavesAsExpected(RuleCase<float?> tc)
    {
        // Act
        var result = NumberRules.IsFinite(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(NumberRulesTestData.IsFiniteDouble.Cases), MemberType = typeof(NumberRulesTestData.IsFiniteDouble))]
    public void IsFinite_Double_BehavesAsExpected(RuleCase<double?> tc)
    {
        // Act
        var result = NumberRules.IsFinite(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(NumberRulesTestData.IsNaNFloat.Cases), MemberType = typeof(NumberRulesTestData.IsNaNFloat))]
    public void IsNaN_Float_BehavesAsExpected(RuleCase<float?> tc)
    {
        // Act
        var result = NumberRules.IsNaN(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(NumberRulesTestData.IsNaNDouble.Cases), MemberType = typeof(NumberRulesTestData.IsNaNDouble))]
    public void IsNaN_Double_BehavesAsExpected(RuleCase<double?> tc)
    {
        // Act
        var result = NumberRules.IsNaN(tc.Value);

        // Assert
        AssertResult(tc, result);
    }
}
