using PineGuard.Common;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Rules.StringRules;

public sealed class StringRulesNumbersTests : BaseUnitTest
{
    [Theory]
    [MemberData(nameof(StringRulesNumbersTestData.IsPositive.ValidCases), MemberType = typeof(StringRulesNumbersTestData.IsPositive))]
    [MemberData(nameof(StringRulesNumbersTestData.IsPositive.EdgeCases), MemberType = typeof(StringRulesNumbersTestData.IsPositive))]
    public void IsPositive_ReturnsExpected(StringRulesNumbersTestData.IsPositive.Case testCase)
    {
        var result = PineGuard.Rules.StringRules.Numbers.IsPositive(testCase.Value);

        Assert.Equal(testCase.Expected, result);
    }

    [Theory]
    [MemberData(nameof(StringRulesNumbersTestData.IsNegative.ValidCases), MemberType = typeof(StringRulesNumbersTestData.IsNegative))]
    [MemberData(nameof(StringRulesNumbersTestData.IsNegative.EdgeCases), MemberType = typeof(StringRulesNumbersTestData.IsNegative))]
    public void IsNegative_ReturnsExpected(StringRulesNumbersTestData.IsNegative.Case testCase)
    {
        var result = PineGuard.Rules.StringRules.Numbers.IsNegative(testCase.Value);

        Assert.Equal(testCase.Expected, result);
    }

    [Theory]
    [MemberData(nameof(StringRulesNumbersTestData.IsZero.ValidCases), MemberType = typeof(StringRulesNumbersTestData.IsZero))]
    [MemberData(nameof(StringRulesNumbersTestData.IsZero.EdgeCases), MemberType = typeof(StringRulesNumbersTestData.IsZero))]
    public void IsZero_ReturnsExpected(StringRulesNumbersTestData.IsZero.Case testCase)
    {
        var result = PineGuard.Rules.StringRules.Numbers.IsZero(testCase.Value);

        Assert.Equal(testCase.Expected, result);
    }

    [Theory]
    [MemberData(nameof(StringRulesNumbersTestData.IsNotZero.ValidCases), MemberType = typeof(StringRulesNumbersTestData.IsNotZero))]
    [MemberData(nameof(StringRulesNumbersTestData.IsNotZero.EdgeCases), MemberType = typeof(StringRulesNumbersTestData.IsNotZero))]
    public void IsNotZero_ReturnsExpected(StringRulesNumbersTestData.IsNotZero.Case testCase)
    {
        var result = PineGuard.Rules.StringRules.Numbers.IsNotZero(testCase.Value);

        Assert.Equal(testCase.Expected, result);
    }

    [Theory]
    [MemberData(nameof(StringRulesNumbersTestData.IsZeroOrPositive.ValidCases), MemberType = typeof(StringRulesNumbersTestData.IsZeroOrPositive))]
    [MemberData(nameof(StringRulesNumbersTestData.IsZeroOrPositive.EdgeCases), MemberType = typeof(StringRulesNumbersTestData.IsZeroOrPositive))]
    public void IsZeroOrPositive_ReturnsExpected(StringRulesNumbersTestData.IsZeroOrPositive.Case testCase)
    {
        var result = PineGuard.Rules.StringRules.Numbers.IsZeroOrPositive(testCase.Value);

        Assert.Equal(testCase.Expected, result);
    }

    [Theory]
    [MemberData(nameof(StringRulesNumbersTestData.IsZeroOrNegative.ValidCases), MemberType = typeof(StringRulesNumbersTestData.IsZeroOrNegative))]
    [MemberData(nameof(StringRulesNumbersTestData.IsZeroOrNegative.EdgeCases), MemberType = typeof(StringRulesNumbersTestData.IsZeroOrNegative))]
    public void IsZeroOrNegative_ReturnsExpected(StringRulesNumbersTestData.IsZeroOrNegative.Case testCase)
    {
        var result = PineGuard.Rules.StringRules.Numbers.IsZeroOrNegative(testCase.Value);

        Assert.Equal(testCase.Expected, result);
    }

    [Theory]
    [MemberData(nameof(StringRulesNumbersTestData.IsGreaterThan.ValidCases), MemberType = typeof(StringRulesNumbersTestData.IsGreaterThan))]
    [MemberData(nameof(StringRulesNumbersTestData.IsGreaterThan.EdgeCases), MemberType = typeof(StringRulesNumbersTestData.IsGreaterThan))]
    public void IsGreaterThan_ReturnsExpected(StringRulesNumbersTestData.IsGreaterThan.Case testCase)
    {
        var result = PineGuard.Rules.StringRules.Numbers.IsGreaterThan(testCase.Value, testCase.Min);

        Assert.Equal(testCase.Expected, result);
    }

    [Theory]
    [MemberData(nameof(StringRulesNumbersTestData.IsGreaterThanOrEqual.ValidCases), MemberType = typeof(StringRulesNumbersTestData.IsGreaterThanOrEqual))]
    [MemberData(nameof(StringRulesNumbersTestData.IsGreaterThanOrEqual.EdgeCases), MemberType = typeof(StringRulesNumbersTestData.IsGreaterThanOrEqual))]
    public void IsGreaterThanOrEqual_ReturnsExpected(StringRulesNumbersTestData.IsGreaterThanOrEqual.Case testCase)
    {
        var result = PineGuard.Rules.StringRules.Numbers.IsGreaterThanOrEqual(testCase.Value, testCase.Min);

        Assert.Equal(testCase.Expected, result);
    }

    [Theory]
    [MemberData(nameof(StringRulesNumbersTestData.IsLessThan.ValidCases), MemberType = typeof(StringRulesNumbersTestData.IsLessThan))]
    [MemberData(nameof(StringRulesNumbersTestData.IsLessThan.EdgeCases), MemberType = typeof(StringRulesNumbersTestData.IsLessThan))]
    public void IsLessThan_ReturnsExpected(StringRulesNumbersTestData.IsLessThan.Case testCase)
    {
        var result = PineGuard.Rules.StringRules.Numbers.IsLessThan(testCase.Value, testCase.Max);

        Assert.Equal(testCase.Expected, result);
    }

    [Theory]
    [MemberData(nameof(StringRulesNumbersTestData.IsLessThanOrEqual.ValidCases), MemberType = typeof(StringRulesNumbersTestData.IsLessThanOrEqual))]
    [MemberData(nameof(StringRulesNumbersTestData.IsLessThanOrEqual.EdgeCases), MemberType = typeof(StringRulesNumbersTestData.IsLessThanOrEqual))]
    public void IsLessThanOrEqual_ReturnsExpected(StringRulesNumbersTestData.IsLessThanOrEqual.Case testCase)
    {
        var result = PineGuard.Rules.StringRules.Numbers.IsLessThanOrEqual(testCase.Value, testCase.Max);

        Assert.Equal(testCase.Expected, result);
    }

    [Theory]
    [MemberData(nameof(StringRulesNumbersTestData.IsInRange.ValidCases), MemberType = typeof(StringRulesNumbersTestData.IsInRange))]
    [MemberData(nameof(StringRulesNumbersTestData.IsInRange.EdgeCases), MemberType = typeof(StringRulesNumbersTestData.IsInRange))]
    public void IsInRange_ReturnsExpected(StringRulesNumbersTestData.IsInRange.Case testCase)
    {
        var result = PineGuard.Rules.StringRules.Numbers.IsInRange(
            testCase.Value,
            testCase.Min,
            testCase.Max,
            testCase.Inclusion);

        Assert.Equal(testCase.Expected, result);
    }

    [Theory]
    [MemberData(nameof(StringRulesNumbersTestData.IsApproximately.ValidCases), MemberType = typeof(StringRulesNumbersTestData.IsApproximately))]
    [MemberData(nameof(StringRulesNumbersTestData.IsApproximately.EdgeCases), MemberType = typeof(StringRulesNumbersTestData.IsApproximately))]
    public void IsApproximately_ReturnsExpected(StringRulesNumbersTestData.IsApproximately.Case testCase)
    {
        var result = PineGuard.Rules.StringRules.Numbers.IsApproximately(testCase.Value, testCase.Target, testCase.Tolerance);

        Assert.Equal(testCase.Expected, result);
    }

    [Theory]
    [MemberData(nameof(StringRulesNumbersTestData.IsMultipleOf.ValidCases), MemberType = typeof(StringRulesNumbersTestData.IsMultipleOf))]
    [MemberData(nameof(StringRulesNumbersTestData.IsMultipleOf.EdgeCases), MemberType = typeof(StringRulesNumbersTestData.IsMultipleOf))]
    public void IsMultipleOf_ReturnsExpected(StringRulesNumbersTestData.IsMultipleOf.Case testCase)
    {
        var result = PineGuard.Rules.StringRules.Numbers.IsMultipleOf(testCase.Value, testCase.Factor);

        Assert.Equal(testCase.Expected, result);
    }

    [Theory]
    [MemberData(nameof(StringRulesNumbersTestData.IsEven.ValidCases), MemberType = typeof(StringRulesNumbersTestData.IsEven))]
    [MemberData(nameof(StringRulesNumbersTestData.IsEven.EdgeCases), MemberType = typeof(StringRulesNumbersTestData.IsEven))]
    public void IsEven_ReturnsExpected(StringRulesNumbersTestData.IsEven.Case testCase)
    {
        var result = PineGuard.Rules.StringRules.Numbers.IsEven(testCase.Value);

        Assert.Equal(testCase.Expected, result);
    }

    [Theory]
    [MemberData(nameof(StringRulesNumbersTestData.IsOdd.ValidCases), MemberType = typeof(StringRulesNumbersTestData.IsOdd))]
    [MemberData(nameof(StringRulesNumbersTestData.IsOdd.EdgeCases), MemberType = typeof(StringRulesNumbersTestData.IsOdd))]
    public void IsOdd_ReturnsExpected(StringRulesNumbersTestData.IsOdd.Case testCase)
    {
        var result = PineGuard.Rules.StringRules.Numbers.IsOdd(testCase.Value);

        Assert.Equal(testCase.Expected, result);
    }

    [Theory]
    [MemberData(nameof(StringRulesNumbersTestData.IsFinite.ValidCases), MemberType = typeof(StringRulesNumbersTestData.IsFinite))]
    [MemberData(nameof(StringRulesNumbersTestData.IsFinite.EdgeCases), MemberType = typeof(StringRulesNumbersTestData.IsFinite))]
    public void IsFinite_ReturnsExpected(StringRulesNumbersTestData.IsFinite.Case testCase)
    {
        var result = PineGuard.Rules.StringRules.Numbers.IsFinite(testCase.Value);

        Assert.Equal(testCase.Expected, result);
    }

    [Theory]
    [MemberData(nameof(StringRulesNumbersTestData.IsNaN.ValidCases), MemberType = typeof(StringRulesNumbersTestData.IsNaN))]
    [MemberData(nameof(StringRulesNumbersTestData.IsNaN.EdgeCases), MemberType = typeof(StringRulesNumbersTestData.IsNaN))]
    public void IsNaN_ReturnsExpected(StringRulesNumbersTestData.IsNaN.Case testCase)
    {
        var result = PineGuard.Rules.StringRules.Numbers.IsNaN(testCase.Value);

        Assert.Equal(testCase.Expected, result);
    }
}
