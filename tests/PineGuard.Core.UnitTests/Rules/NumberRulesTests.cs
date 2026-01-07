using PineGuard.Common;
using PineGuard.Rules;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Rules;

public sealed class NumberRulesTests : BaseUnitTest
{
    [Theory]
    [MemberData(nameof(NumberRulesTestData.IsPositiveInt.ValidCases), MemberType = typeof(NumberRulesTestData.IsPositiveInt))]
    public void IsPositive_ReturnsTrue_ForPositive(NumberRulesTestData.IsPositiveInt.Case testCase)
    {
        var result = NumberRules.IsPositive<int>(testCase.Value);

        Assert.Equal(testCase.Expected, result);
        Assert.True(result);
    }

    [Theory]
    [MemberData(nameof(NumberRulesTestData.IsPositiveInt.EdgeCases), MemberType = typeof(NumberRulesTestData.IsPositiveInt))]
    public void IsPositive_ReturnsFalse_ForNullOrNonPositive(NumberRulesTestData.IsPositiveInt.Case testCase)
    {
        var result = NumberRules.IsPositive<int>(testCase.Value);

        Assert.Equal(testCase.Expected, result);
        Assert.False(result);
    }

    [Theory]
    [MemberData(nameof(NumberRulesTestData.IsNegativeInt.ValidCases), MemberType = typeof(NumberRulesTestData.IsNegativeInt))]
    public void IsNegative_ReturnsTrue_ForNegative(NumberRulesTestData.IsNegativeInt.Case testCase)
    {
        var result = NumberRules.IsNegative<int>(testCase.Value);

        Assert.Equal(testCase.Expected, result);
        Assert.True(result);
    }

    [Theory]
    [MemberData(nameof(NumberRulesTestData.IsNegativeInt.EdgeCases), MemberType = typeof(NumberRulesTestData.IsNegativeInt))]
    public void IsNegative_ReturnsFalse_ForNullOrNonNegative(NumberRulesTestData.IsNegativeInt.Case testCase)
    {
        var result = NumberRules.IsNegative<int>(testCase.Value);

        Assert.Equal(testCase.Expected, result);
        Assert.False(result);
    }

    [Theory]
    [MemberData(nameof(NumberRulesTestData.IsZeroInt.ValidCases), MemberType = typeof(NumberRulesTestData.IsZeroInt))]
    public void IsZero_ReturnsTrue_ForZero(NumberRulesTestData.IsZeroInt.Case testCase)
    {
        var result = NumberRules.IsZero<int>(testCase.Value);

        Assert.Equal(testCase.Expected, result);
        Assert.True(result);
    }

    [Theory]
    [MemberData(nameof(NumberRulesTestData.IsZeroInt.EdgeCases), MemberType = typeof(NumberRulesTestData.IsZeroInt))]
    public void IsZero_ReturnsFalse_ForNullOrNonZero(NumberRulesTestData.IsZeroInt.Case testCase)
    {
        var result = NumberRules.IsZero<int>(testCase.Value);

        Assert.Equal(testCase.Expected, result);
        Assert.False(result);
    }

    [Theory]
    [MemberData(nameof(NumberRulesTestData.IsNotZeroInt.ValidCases), MemberType = typeof(NumberRulesTestData.IsNotZeroInt))]
    public void IsNotZero_ReturnsTrue_ForNonZero(NumberRulesTestData.IsNotZeroInt.Case testCase)
    {
        var result = NumberRules.IsNotZero<int>(testCase.Value);

        Assert.Equal(testCase.Expected, result);
        Assert.True(result);
    }

    [Theory]
    [MemberData(nameof(NumberRulesTestData.IsNotZeroInt.EdgeCases), MemberType = typeof(NumberRulesTestData.IsNotZeroInt))]
    public void IsNotZero_ReturnsFalse_ForNullOrZero(NumberRulesTestData.IsNotZeroInt.Case testCase)
    {
        var result = NumberRules.IsNotZero<int>(testCase.Value);

        Assert.Equal(testCase.Expected, result);
        Assert.False(result);
    }

    [Theory]
    [MemberData(nameof(NumberRulesTestData.IsZeroOrPositiveInt.ValidCases), MemberType = typeof(NumberRulesTestData.IsZeroOrPositiveInt))]
    public void IsZeroOrPositive_ReturnsTrue_ForZeroOrPositive(NumberRulesTestData.IsZeroOrPositiveInt.Case testCase)
    {
        var result = NumberRules.IsZeroOrPositive<int>(testCase.Value);

        Assert.Equal(testCase.Expected, result);
        Assert.True(result);
    }

    [Theory]
    [MemberData(nameof(NumberRulesTestData.IsZeroOrPositiveInt.EdgeCases), MemberType = typeof(NumberRulesTestData.IsZeroOrPositiveInt))]
    public void IsZeroOrPositive_ReturnsFalse_ForNullOrNegative(NumberRulesTestData.IsZeroOrPositiveInt.Case testCase)
    {
        var result = NumberRules.IsZeroOrPositive<int>(testCase.Value);

        Assert.Equal(testCase.Expected, result);
        Assert.False(result);
    }

    [Theory]
    [MemberData(nameof(NumberRulesTestData.IsZeroOrNegativeInt.ValidCases), MemberType = typeof(NumberRulesTestData.IsZeroOrNegativeInt))]
    public void IsZeroOrNegative_ReturnsTrue_ForZeroOrNegative(NumberRulesTestData.IsZeroOrNegativeInt.Case testCase)
    {
        var result = NumberRules.IsZeroOrNegative<int>(testCase.Value);

        Assert.Equal(testCase.Expected, result);
        Assert.True(result);
    }

    [Theory]
    [MemberData(nameof(NumberRulesTestData.IsZeroOrNegativeInt.EdgeCases), MemberType = typeof(NumberRulesTestData.IsZeroOrNegativeInt))]
    public void IsZeroOrNegative_ReturnsFalse_ForNullOrPositive(NumberRulesTestData.IsZeroOrNegativeInt.Case testCase)
    {
        var result = NumberRules.IsZeroOrNegative<int>(testCase.Value);

        Assert.Equal(testCase.Expected, result);
        Assert.False(result);
    }

    [Theory]
    [MemberData(nameof(NumberRulesTestData.IsGreaterThan.ValidCases), MemberType = typeof(NumberRulesTestData.IsGreaterThan))]
    [MemberData(nameof(NumberRulesTestData.IsGreaterThan.EdgeCases), MemberType = typeof(NumberRulesTestData.IsGreaterThan))]
    public void IsGreaterThan_ReturnsExpected(NumberRulesTestData.IsGreaterThan.Case testCase)
    {
        var result = NumberRules.IsGreaterThan(testCase.Value, testCase.Min);

        Assert.Equal(testCase.Expected, result);
    }

    [Theory]
    [MemberData(nameof(NumberRulesTestData.IsGreaterThanOrEqual.ValidCases), MemberType = typeof(NumberRulesTestData.IsGreaterThanOrEqual))]
    [MemberData(nameof(NumberRulesTestData.IsGreaterThanOrEqual.EdgeCases), MemberType = typeof(NumberRulesTestData.IsGreaterThanOrEqual))]
    public void IsGreaterThanOrEqual_ReturnsExpected(NumberRulesTestData.IsGreaterThanOrEqual.Case testCase)
    {
        var result = NumberRules.IsGreaterThanOrEqual(testCase.Value, testCase.Min);

        Assert.Equal(testCase.Expected, result);
    }

    [Theory]
    [MemberData(nameof(NumberRulesTestData.IsLessThan.ValidCases), MemberType = typeof(NumberRulesTestData.IsLessThan))]
    [MemberData(nameof(NumberRulesTestData.IsLessThan.EdgeCases), MemberType = typeof(NumberRulesTestData.IsLessThan))]
    public void IsLessThan_ReturnsExpected(NumberRulesTestData.IsLessThan.Case testCase)
    {
        var result = NumberRules.IsLessThan(testCase.Value, testCase.Max);

        Assert.Equal(testCase.Expected, result);
    }

    [Theory]
    [MemberData(nameof(NumberRulesTestData.IsLessThanOrEqual.ValidCases), MemberType = typeof(NumberRulesTestData.IsLessThanOrEqual))]
    [MemberData(nameof(NumberRulesTestData.IsLessThanOrEqual.EdgeCases), MemberType = typeof(NumberRulesTestData.IsLessThanOrEqual))]
    public void IsLessThanOrEqual_ReturnsExpected(NumberRulesTestData.IsLessThanOrEqual.Case testCase)
    {
        var result = NumberRules.IsLessThanOrEqual(testCase.Value, testCase.Max);

        Assert.Equal(testCase.Expected, result);
    }

    [Theory]
    [MemberData(nameof(NumberRulesTestData.IsInRange.ValidCases), MemberType = typeof(NumberRulesTestData.IsInRange))]
    [MemberData(nameof(NumberRulesTestData.IsInRange.EdgeCases), MemberType = typeof(NumberRulesTestData.IsInRange))]
    public void IsInRange_RespectsInclusion(NumberRulesTestData.IsInRange.Case testCase)
    {
        var result = NumberRules.IsInRange(testCase.Value, testCase.Min, testCase.Max, testCase.Inclusion);

        Assert.Equal(testCase.Expected, result);
    }

    [Theory]
    [MemberData(nameof(NumberRulesTestData.IsApproximately.ValidCases), MemberType = typeof(NumberRulesTestData.IsApproximately))]
    [MemberData(nameof(NumberRulesTestData.IsApproximately.EdgeCases), MemberType = typeof(NumberRulesTestData.IsApproximately))]
    public void IsApproximately_RespectsTolerance(NumberRulesTestData.IsApproximately.Case testCase)
    {
        var result = NumberRules.IsApproximately(testCase.Value, testCase.Target, testCase.Tolerance);

        Assert.Equal(testCase.Expected, result);
    }

    [Theory]
    [MemberData(nameof(NumberRulesTestData.IsMultipleOf.ValidCases), MemberType = typeof(NumberRulesTestData.IsMultipleOf))]
    [MemberData(nameof(NumberRulesTestData.IsMultipleOf.EdgeCases), MemberType = typeof(NumberRulesTestData.IsMultipleOf))]
    public void IsMultipleOf_ReturnsExpected(NumberRulesTestData.IsMultipleOf.Case testCase)
    {
        var result = NumberRules.IsMultipleOf(testCase.Value, testCase.Factor);

        Assert.Equal(testCase.Expected, result);
    }

    [Theory]
    [MemberData(nameof(NumberRulesTestData.IsEvenInt.ValidCases), MemberType = typeof(NumberRulesTestData.IsEvenInt))]
    [MemberData(nameof(NumberRulesTestData.IsEvenInt.EdgeCases), MemberType = typeof(NumberRulesTestData.IsEvenInt))]
    public void IsEven_Int_ReturnsExpected(NumberRulesTestData.IsEvenInt.Case testCase)
    {
        var result = NumberRules.IsEven(testCase.Value);

        Assert.Equal(testCase.Expected, result);
    }

    [Theory]
    [MemberData(nameof(NumberRulesTestData.IsEvenLong.ValidCases), MemberType = typeof(NumberRulesTestData.IsEvenLong))]
    [MemberData(nameof(NumberRulesTestData.IsEvenLong.EdgeCases), MemberType = typeof(NumberRulesTestData.IsEvenLong))]
    public void IsEven_Long_ReturnsExpected(NumberRulesTestData.IsEvenLong.Case testCase)
    {
        var result = NumberRules.IsEven(testCase.Value);

        Assert.Equal(testCase.Expected, result);
    }

    [Theory]
    [MemberData(nameof(NumberRulesTestData.IsOddInt.ValidCases), MemberType = typeof(NumberRulesTestData.IsOddInt))]
    [MemberData(nameof(NumberRulesTestData.IsOddInt.EdgeCases), MemberType = typeof(NumberRulesTestData.IsOddInt))]
    public void IsOdd_Int_ReturnsExpected(NumberRulesTestData.IsOddInt.Case testCase)
    {
        var result = NumberRules.IsOdd(testCase.Value);

        Assert.Equal(testCase.Expected, result);
    }

    [Theory]
    [MemberData(nameof(NumberRulesTestData.IsOddLong.ValidCases), MemberType = typeof(NumberRulesTestData.IsOddLong))]
    [MemberData(nameof(NumberRulesTestData.IsOddLong.EdgeCases), MemberType = typeof(NumberRulesTestData.IsOddLong))]
    public void IsOdd_Long_ReturnsExpected(NumberRulesTestData.IsOddLong.Case testCase)
    {
        var result = NumberRules.IsOdd(testCase.Value);

        Assert.Equal(testCase.Expected, result);
    }

    [Theory]
    [MemberData(nameof(NumberRulesTestData.IsFiniteFloat.ValidCases), MemberType = typeof(NumberRulesTestData.IsFiniteFloat))]
    [MemberData(nameof(NumberRulesTestData.IsFiniteFloat.EdgeCases), MemberType = typeof(NumberRulesTestData.IsFiniteFloat))]
    public void IsFinite_Float_ReturnsExpected(NumberRulesTestData.IsFiniteFloat.Case testCase)
    {
        var result = NumberRules.IsFinite(testCase.Value);

        Assert.Equal(testCase.Expected, result);
    }

    [Theory]
    [MemberData(nameof(NumberRulesTestData.IsFiniteDouble.ValidCases), MemberType = typeof(NumberRulesTestData.IsFiniteDouble))]
    [MemberData(nameof(NumberRulesTestData.IsFiniteDouble.EdgeCases), MemberType = typeof(NumberRulesTestData.IsFiniteDouble))]
    public void IsFinite_Double_ReturnsExpected(NumberRulesTestData.IsFiniteDouble.Case testCase)
    {
        var result = NumberRules.IsFinite(testCase.Value);

        Assert.Equal(testCase.Expected, result);
    }

    [Theory]
    [MemberData(nameof(NumberRulesTestData.IsNaNFloat.ValidCases), MemberType = typeof(NumberRulesTestData.IsNaNFloat))]
    [MemberData(nameof(NumberRulesTestData.IsNaNFloat.EdgeCases), MemberType = typeof(NumberRulesTestData.IsNaNFloat))]
    public void IsNaN_Float_ReturnsExpected(NumberRulesTestData.IsNaNFloat.Case testCase)
    {
        var result = NumberRules.IsNaN(testCase.Value);

        Assert.Equal(testCase.Expected, result);
    }

    [Theory]
    [MemberData(nameof(NumberRulesTestData.IsNaNDouble.ValidCases), MemberType = typeof(NumberRulesTestData.IsNaNDouble))]
    [MemberData(nameof(NumberRulesTestData.IsNaNDouble.EdgeCases), MemberType = typeof(NumberRulesTestData.IsNaNDouble))]
    public void IsNaN_Double_ReturnsExpected(NumberRulesTestData.IsNaNDouble.Case testCase)
    {
        var result = NumberRules.IsNaN(testCase.Value);

        Assert.Equal(testCase.Expected, result);
    }
}
