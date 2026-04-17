using PineGuard.Common;
using PineGuard.Testing.UnitTests.GuardClauses;
using Xunit.Abstractions;

namespace PineGuard.GuardClauses.UnitTests;

public sealed class GuardNumberClausesTests(ITestOutputHelper output) : BaseGuardUnitTest(output)
{
    // Guard.Against.ZeroOrNegative
    [Theory]
    [MemberData(nameof(GuardNumberClausesTestData.ZeroOrNegative.ValidCases), MemberType = typeof(GuardNumberClausesTestData.ZeroOrNegative))]
    [MemberData(nameof(GuardNumberClausesTestData.ZeroOrNegative.InvalidCases), MemberType = typeof(GuardNumberClausesTestData.ZeroOrNegative))]
    public void ZeroOrNegative_BehavesAsExpected(GuardCase<int> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.ZeroOrNegative(value));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    // Guard.Against.ZeroOrPositive
    [Theory]
    [MemberData(nameof(GuardNumberClausesTestData.ZeroOrPositive.ValidCases), MemberType = typeof(GuardNumberClausesTestData.ZeroOrPositive))]
    [MemberData(nameof(GuardNumberClausesTestData.ZeroOrPositive.InvalidCases), MemberType = typeof(GuardNumberClausesTestData.ZeroOrPositive))]
    public void ZeroOrPositive_BehavesAsExpected(GuardCase<int> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.ZeroOrPositive(value));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    // Guard.Against.NotZero
    [Theory]
    [MemberData(nameof(GuardNumberClausesTestData.NotZero.ValidCases), MemberType = typeof(GuardNumberClausesTestData.NotZero))]
    [MemberData(nameof(GuardNumberClausesTestData.NotZero.InvalidCases), MemberType = typeof(GuardNumberClausesTestData.NotZero))]
    public void NotZero_BehavesAsExpected(GuardCase<int> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.NotZero(value));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    // Guard.Against.Zero
    [Theory]
    [MemberData(nameof(GuardNumberClausesTestData.Zero.ValidCases), MemberType = typeof(GuardNumberClausesTestData.Zero))]
    [MemberData(nameof(GuardNumberClausesTestData.Zero.InvalidCases), MemberType = typeof(GuardNumberClausesTestData.Zero))]
    public void Zero_BehavesAsExpected(GuardCase<int> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.Zero(value));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    // Guard.Against.Negative
    [Theory]
    [MemberData(nameof(GuardNumberClausesTestData.Negative.ValidCases), MemberType = typeof(GuardNumberClausesTestData.Negative))]
    [MemberData(nameof(GuardNumberClausesTestData.Negative.InvalidCases), MemberType = typeof(GuardNumberClausesTestData.Negative))]
    public void Negative_BehavesAsExpected(GuardCase<int> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.Negative(value));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    // Guard.Against.Positive
    [Theory]
    [MemberData(nameof(GuardNumberClausesTestData.Positive.ValidCases), MemberType = typeof(GuardNumberClausesTestData.Positive))]
    [MemberData(nameof(GuardNumberClausesTestData.Positive.InvalidCases), MemberType = typeof(GuardNumberClausesTestData.Positive))]
    public void Positive_BehavesAsExpected(GuardCase<int> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.Positive(value));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    // Guard.Against.LessThanOrEqual
    [Theory]
    [MemberData(nameof(GuardNumberClausesTestData.LessThanOrEqual.ValidCases), MemberType = typeof(GuardNumberClausesTestData.LessThanOrEqual))]
    [MemberData(nameof(GuardNumberClausesTestData.LessThanOrEqual.InvalidCases), MemberType = typeof(GuardNumberClausesTestData.LessThanOrEqual))]
    public void LessThanOrEqual_BehavesAsExpected(GuardCase<(int value, int min)> tc)
    {
        var value = tc.Value.value;
        var result = AssertResult(tc, () => Guard.Against.LessThanOrEqual(value, tc.Value.min));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    // Guard.Against.LessThan
    [Theory]
    [MemberData(nameof(GuardNumberClausesTestData.LessThan.ValidCases), MemberType = typeof(GuardNumberClausesTestData.LessThan))]
    [MemberData(nameof(GuardNumberClausesTestData.LessThan.InvalidCases), MemberType = typeof(GuardNumberClausesTestData.LessThan))]
    public void LessThan_BehavesAsExpected(GuardCase<(int value, int min)> tc)
    {
        var value = tc.Value.value;
        var result = AssertResult(tc, () => Guard.Against.LessThan(value, tc.Value.min));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    // Guard.Against.GreaterThanOrEqual
    [Theory]
    [MemberData(nameof(GuardNumberClausesTestData.GreaterThanOrEqual.ValidCases), MemberType = typeof(GuardNumberClausesTestData.GreaterThanOrEqual))]
    [MemberData(nameof(GuardNumberClausesTestData.GreaterThanOrEqual.InvalidCases), MemberType = typeof(GuardNumberClausesTestData.GreaterThanOrEqual))]
    public void GreaterThanOrEqual_BehavesAsExpected(GuardCase<(int value, int max)> tc)
    {
        var value = tc.Value.value;
        var result = AssertResult(tc, () => Guard.Against.GreaterThanOrEqual(value, tc.Value.max));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    // Guard.Against.GreaterThan
    [Theory]
    [MemberData(nameof(GuardNumberClausesTestData.GreaterThan.ValidCases), MemberType = typeof(GuardNumberClausesTestData.GreaterThan))]
    [MemberData(nameof(GuardNumberClausesTestData.GreaterThan.InvalidCases), MemberType = typeof(GuardNumberClausesTestData.GreaterThan))]
    public void GreaterThan_BehavesAsExpected(GuardCase<(int value, int min)> tc)
    {
        var value = tc.Value.value;
        var result = AssertResult(tc, () => Guard.Against.GreaterThan(value, tc.Value.min));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    // Guard.Against.OutOfRange
    [Theory]
    [MemberData(nameof(GuardNumberClausesTestData.OutOfRange.ValidCases), MemberType = typeof(GuardNumberClausesTestData.OutOfRange))]
    [MemberData(nameof(GuardNumberClausesTestData.OutOfRange.InvalidCases), MemberType = typeof(GuardNumberClausesTestData.OutOfRange))]
    public void OutOfRange_BehavesAsExpected(GuardCase<(int value, int min, int max, Inclusion inclusion)> tc)
    {
        var value = tc.Value.value;
        var result = AssertResult(tc, () => Guard.Against.OutOfRange(value, tc.Value.min, tc.Value.max, tc.Value.inclusion));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    // Guard.Against.InRange
    [Theory]
    [MemberData(nameof(GuardNumberClausesTestData.InRange.ValidCases), MemberType = typeof(GuardNumberClausesTestData.InRange))]
    [MemberData(nameof(GuardNumberClausesTestData.InRange.InvalidCases), MemberType = typeof(GuardNumberClausesTestData.InRange))]
    public void InRange_BehavesAsExpected(GuardCase<(int value, int min, int max, Inclusion inclusion)> tc)
    {
        var value = tc.Value.value;
        var result = AssertResult(tc, () => Guard.Against.InRange(value, tc.Value.min, tc.Value.max, tc.Value.inclusion));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    // Guard.Against.NotApproximately
    [Theory]
    [MemberData(nameof(GuardNumberClausesTestData.NotApproximately.ValidCases), MemberType = typeof(GuardNumberClausesTestData.NotApproximately))]
    [MemberData(nameof(GuardNumberClausesTestData.NotApproximately.InvalidCases), MemberType = typeof(GuardNumberClausesTestData.NotApproximately))]
    public void NotApproximately_BehavesAsExpected(GuardCase<(double value, double target, double? tolerance)> tc)
    {
        var value = tc.Value.value;
        var result = AssertResult(tc, () => Guard.Against.NotApproximately(value, tc.Value.target, tc.Value.tolerance));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    // Guard.Against.Approximately
    [Theory]
    [MemberData(nameof(GuardNumberClausesTestData.Approximately.ValidCases), MemberType = typeof(GuardNumberClausesTestData.Approximately))]
    [MemberData(nameof(GuardNumberClausesTestData.Approximately.InvalidCases), MemberType = typeof(GuardNumberClausesTestData.Approximately))]
    public void Approximately_BehavesAsExpected(GuardCase<(double value, double target, double? tolerance)> tc)
    {
        var value = tc.Value.value;
        var result = AssertResult(tc, () => Guard.Against.Approximately(value, tc.Value.target, tc.Value.tolerance));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    // Guard.Against.NotMultipleOf
    [Theory]
    [MemberData(nameof(GuardNumberClausesTestData.NotMultipleOf.ValidCases), MemberType = typeof(GuardNumberClausesTestData.NotMultipleOf))]
    [MemberData(nameof(GuardNumberClausesTestData.NotMultipleOf.InvalidCases), MemberType = typeof(GuardNumberClausesTestData.NotMultipleOf))]
    public void NotMultipleOf_BehavesAsExpected(GuardCase<(int value, int factor)> tc)
    {
        var value = tc.Value.value;
        var result = AssertResult(tc, () => Guard.Against.NotMultipleOf(value, tc.Value.factor));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    // Guard.Against.MultipleOf
    [Theory]
    [MemberData(nameof(GuardNumberClausesTestData.MultipleOf.ValidCases), MemberType = typeof(GuardNumberClausesTestData.MultipleOf))]
    [MemberData(nameof(GuardNumberClausesTestData.MultipleOf.InvalidCases), MemberType = typeof(GuardNumberClausesTestData.MultipleOf))]
    public void MultipleOf_BehavesAsExpected(GuardCase<(int value, int factor)> tc)
    {
        var value = tc.Value.value;
        var result = AssertResult(tc, () => Guard.Against.MultipleOf(value, tc.Value.factor));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    // Guard.Against.Odd (int overload)
    [Theory]
    [MemberData(nameof(GuardNumberClausesTestData.OddInt.ValidCases), MemberType = typeof(GuardNumberClausesTestData.OddInt))]
    [MemberData(nameof(GuardNumberClausesTestData.OddInt.InvalidCases), MemberType = typeof(GuardNumberClausesTestData.OddInt))]
    public void OddInt_BehavesAsExpected(GuardCase<int> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.Odd(value));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    // Guard.Against.Odd (long overload)
    [Theory]
    [MemberData(nameof(GuardNumberClausesTestData.OddLong.ValidCases), MemberType = typeof(GuardNumberClausesTestData.OddLong))]
    [MemberData(nameof(GuardNumberClausesTestData.OddLong.InvalidCases), MemberType = typeof(GuardNumberClausesTestData.OddLong))]
    public void OddLong_BehavesAsExpected(GuardCase<long> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.Odd(value));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    // Guard.Against.Even (int overload)
    [Theory]
    [MemberData(nameof(GuardNumberClausesTestData.EvenInt.ValidCases), MemberType = typeof(GuardNumberClausesTestData.EvenInt))]
    [MemberData(nameof(GuardNumberClausesTestData.EvenInt.InvalidCases), MemberType = typeof(GuardNumberClausesTestData.EvenInt))]
    public void EvenInt_BehavesAsExpected(GuardCase<int> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.Even(value));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    // Guard.Against.Even (long overload)
    [Theory]
    [MemberData(nameof(GuardNumberClausesTestData.EvenLong.ValidCases), MemberType = typeof(GuardNumberClausesTestData.EvenLong))]
    [MemberData(nameof(GuardNumberClausesTestData.EvenLong.InvalidCases), MemberType = typeof(GuardNumberClausesTestData.EvenLong))]
    public void EvenLong_BehavesAsExpected(GuardCase<long> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.Even(value));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    // Guard.Against.NotFinite (float overload)
    [Theory]
    [MemberData(nameof(GuardNumberClausesTestData.NotFiniteFloat.ValidCases), MemberType = typeof(GuardNumberClausesTestData.NotFiniteFloat))]
    [MemberData(nameof(GuardNumberClausesTestData.NotFiniteFloat.InvalidCases), MemberType = typeof(GuardNumberClausesTestData.NotFiniteFloat))]
    public void NotFiniteFloat_BehavesAsExpected(GuardCase<float> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.NotFinite(value));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    // Guard.Against.NotFinite (double overload)
    [Theory]
    [MemberData(nameof(GuardNumberClausesTestData.NotFiniteDouble.ValidCases), MemberType = typeof(GuardNumberClausesTestData.NotFiniteDouble))]
    [MemberData(nameof(GuardNumberClausesTestData.NotFiniteDouble.InvalidCases), MemberType = typeof(GuardNumberClausesTestData.NotFiniteDouble))]
    public void NotFiniteDouble_BehavesAsExpected(GuardCase<double> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.NotFinite(value));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    // Guard.Against.Finite (float overload)
    [Theory]
    [MemberData(nameof(GuardNumberClausesTestData.FiniteFloat.ValidCases), MemberType = typeof(GuardNumberClausesTestData.FiniteFloat))]
    [MemberData(nameof(GuardNumberClausesTestData.FiniteFloat.InvalidCases), MemberType = typeof(GuardNumberClausesTestData.FiniteFloat))]
    public void FiniteFloat_BehavesAsExpected(GuardCase<float> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.Finite(value));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    // Guard.Against.Finite (double overload)
    [Theory]
    [MemberData(nameof(GuardNumberClausesTestData.FiniteDouble.ValidCases), MemberType = typeof(GuardNumberClausesTestData.FiniteDouble))]
    [MemberData(nameof(GuardNumberClausesTestData.FiniteDouble.InvalidCases), MemberType = typeof(GuardNumberClausesTestData.FiniteDouble))]
    public void FiniteDouble_BehavesAsExpected(GuardCase<double> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.Finite(value));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    // Guard.Against.NaN (float overload)
    [Theory]
    [MemberData(nameof(GuardNumberClausesTestData.NaNFloat.ValidCases), MemberType = typeof(GuardNumberClausesTestData.NaNFloat))]
    [MemberData(nameof(GuardNumberClausesTestData.NaNFloat.InvalidCases), MemberType = typeof(GuardNumberClausesTestData.NaNFloat))]
    public void NaNFloat_BehavesAsExpected(GuardCase<float> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.NaN(value));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    // Guard.Against.NaN (double overload)
    [Theory]
    [MemberData(nameof(GuardNumberClausesTestData.NaNDouble.ValidCases), MemberType = typeof(GuardNumberClausesTestData.NaNDouble))]
    [MemberData(nameof(GuardNumberClausesTestData.NaNDouble.InvalidCases), MemberType = typeof(GuardNumberClausesTestData.NaNDouble))]
    public void NaNDouble_BehavesAsExpected(GuardCase<double> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.NaN(value));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    // Guard.Against.NotNaN (float overload)
    [Theory]
    [MemberData(nameof(GuardNumberClausesTestData.NotNaNFloat.ValidCases), MemberType = typeof(GuardNumberClausesTestData.NotNaNFloat))]
    [MemberData(nameof(GuardNumberClausesTestData.NotNaNFloat.InvalidCases), MemberType = typeof(GuardNumberClausesTestData.NotNaNFloat))]
    public void NotNaNFloat_BehavesAsExpected(GuardCase<float> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.NotNaN(value));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    // Guard.Against.NotNaN (double overload)
    [Theory]
    [MemberData(nameof(GuardNumberClausesTestData.NotNaNDouble.ValidCases), MemberType = typeof(GuardNumberClausesTestData.NotNaNDouble))]
    [MemberData(nameof(GuardNumberClausesTestData.NotNaNDouble.InvalidCases), MemberType = typeof(GuardNumberClausesTestData.NotNaNDouble))]
    public void NotNaNDouble_BehavesAsExpected(GuardCase<double> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.NotNaN(value));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }
}
