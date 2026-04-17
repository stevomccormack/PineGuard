using PineGuard.Common;
using PineGuard.Testing.UnitTests.MustClauses;
using Xunit.Abstractions;

namespace PineGuard.MustClauses.UnitTests;

public sealed class MustNumberClausesTests(ITestOutputHelper output) : BaseMustUnitTest(output)
{
    [Theory]
    [MemberData(nameof(MustNumberClausesTestData.Positive.ValidCases), MemberType = typeof(MustNumberClausesTestData.Positive))]
    [MemberData(nameof(MustNumberClausesTestData.Positive.InvalidCases), MemberType = typeof(MustNumberClausesTestData.Positive))]
    public void Positive_BehavesAsExpected(MustCase<int> tc)
    {
        var result = Must.Be.Positive(tc.Value, paramName: "value");
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustNumberClausesTestData.Negative.ValidCases), MemberType = typeof(MustNumberClausesTestData.Negative))]
    [MemberData(nameof(MustNumberClausesTestData.Negative.InvalidCases), MemberType = typeof(MustNumberClausesTestData.Negative))]
    public void Negative_BehavesAsExpected(MustCase<int> tc)
    {
        var result = Must.Be.Negative(tc.Value, paramName: "value");
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustNumberClausesTestData.Zero.ValidCases), MemberType = typeof(MustNumberClausesTestData.Zero))]
    [MemberData(nameof(MustNumberClausesTestData.Zero.InvalidCases), MemberType = typeof(MustNumberClausesTestData.Zero))]
    public void Zero_BehavesAsExpected(MustCase<int> tc)
    {
        var result = Must.Be.Zero(tc.Value, paramName: "value");
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustNumberClausesTestData.NotZero.ValidCases), MemberType = typeof(MustNumberClausesTestData.NotZero))]
    [MemberData(nameof(MustNumberClausesTestData.NotZero.InvalidCases), MemberType = typeof(MustNumberClausesTestData.NotZero))]
    public void NotZero_BehavesAsExpected(MustCase<int> tc)
    {
        var result = Must.Be.NotZero(tc.Value, paramName: "value");
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustNumberClausesTestData.ZeroOrPositive.ValidCases), MemberType = typeof(MustNumberClausesTestData.ZeroOrPositive))]
    [MemberData(nameof(MustNumberClausesTestData.ZeroOrPositive.InvalidCases), MemberType = typeof(MustNumberClausesTestData.ZeroOrPositive))]
    public void ZeroOrPositive_BehavesAsExpected(MustCase<int> tc)
    {
        var result = Must.Be.ZeroOrPositive(tc.Value, paramName: "value");
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustNumberClausesTestData.ZeroOrNegative.ValidCases), MemberType = typeof(MustNumberClausesTestData.ZeroOrNegative))]
    [MemberData(nameof(MustNumberClausesTestData.ZeroOrNegative.InvalidCases), MemberType = typeof(MustNumberClausesTestData.ZeroOrNegative))]
    public void ZeroOrNegative_BehavesAsExpected(MustCase<int> tc)
    {
        var result = Must.Be.ZeroOrNegative(tc.Value, paramName: "value");
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustNumberClausesTestData.GreaterThan.ValidCases), MemberType = typeof(MustNumberClausesTestData.GreaterThan))]
    [MemberData(nameof(MustNumberClausesTestData.GreaterThan.InvalidCases), MemberType = typeof(MustNumberClausesTestData.GreaterThan))]
    public void GreaterThan_BehavesAsExpected(MustCase<(int value, int min)> tc)
    {
        var result = Must.Be.GreaterThan(tc.Value.value, tc.Value.min, paramName: "value");
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustNumberClausesTestData.GreaterThanOrEqual.ValidCases), MemberType = typeof(MustNumberClausesTestData.GreaterThanOrEqual))]
    [MemberData(nameof(MustNumberClausesTestData.GreaterThanOrEqual.InvalidCases), MemberType = typeof(MustNumberClausesTestData.GreaterThanOrEqual))]
    public void GreaterThanOrEqual_BehavesAsExpected(MustCase<(int value, int min)> tc)
    {
        var result = Must.Be.GreaterThanOrEqual(tc.Value.value, tc.Value.min, paramName: "value");
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustNumberClausesTestData.LessThan.ValidCases), MemberType = typeof(MustNumberClausesTestData.LessThan))]
    [MemberData(nameof(MustNumberClausesTestData.LessThan.InvalidCases), MemberType = typeof(MustNumberClausesTestData.LessThan))]
    public void LessThan_BehavesAsExpected(MustCase<(int value, int max)> tc)
    {
        var result = Must.Be.LessThan(tc.Value.value, tc.Value.max, paramName: "value");
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustNumberClausesTestData.LessThanOrEqual.ValidCases), MemberType = typeof(MustNumberClausesTestData.LessThanOrEqual))]
    [MemberData(nameof(MustNumberClausesTestData.LessThanOrEqual.InvalidCases), MemberType = typeof(MustNumberClausesTestData.LessThanOrEqual))]
    public void LessThanOrEqual_BehavesAsExpected(MustCase<(int value, int max)> tc)
    {
        var result = Must.Be.LessThanOrEqual(tc.Value.value, tc.Value.max, paramName: "value");
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustNumberClausesTestData.InRange.ValidCases), MemberType = typeof(MustNumberClausesTestData.InRange))]
    [MemberData(nameof(MustNumberClausesTestData.InRange.InvalidCases), MemberType = typeof(MustNumberClausesTestData.InRange))]
    public void InRange_BehavesAsExpected(MustCase<(int value, int min, int max, Inclusion inclusion)> tc)
    {
        var result = Must.Be.InRange(tc.Value.value, tc.Value.min, tc.Value.max, tc.Value.inclusion, paramName: "value");
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustNumberClausesTestData.OutOfRange.ValidCases), MemberType = typeof(MustNumberClausesTestData.OutOfRange))]
    [MemberData(nameof(MustNumberClausesTestData.OutOfRange.InvalidCases), MemberType = typeof(MustNumberClausesTestData.OutOfRange))]
    public void OutOfRange_BehavesAsExpected(MustCase<(int value, int min, int max, Inclusion inclusion)> tc)
    {
        var result = Must.Be.OutOfRange(tc.Value.value, tc.Value.min, tc.Value.max, tc.Value.inclusion, paramName: "value");
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustNumberClausesTestData.Approximately.ValidCases), MemberType = typeof(MustNumberClausesTestData.Approximately))]
    [MemberData(nameof(MustNumberClausesTestData.Approximately.InvalidCases), MemberType = typeof(MustNumberClausesTestData.Approximately))]
    public void Approximately_BehavesAsExpected(MustCase<(decimal value, decimal target, decimal? tolerance)> tc)
    {
        var result = Must.Be.Approximately(tc.Value.value, tc.Value.target, tc.Value.tolerance, paramName: "value");
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustNumberClausesTestData.NotApproximately.ValidCases), MemberType = typeof(MustNumberClausesTestData.NotApproximately))]
    [MemberData(nameof(MustNumberClausesTestData.NotApproximately.InvalidCases), MemberType = typeof(MustNumberClausesTestData.NotApproximately))]
    public void NotApproximately_BehavesAsExpected(MustCase<(decimal value, decimal target, decimal? tolerance)> tc)
    {
        var result = Must.Be.NotApproximately(tc.Value.value, tc.Value.target, tc.Value.tolerance, paramName: "value");
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustNumberClausesTestData.MultipleOf.ValidCases), MemberType = typeof(MustNumberClausesTestData.MultipleOf))]
    [MemberData(nameof(MustNumberClausesTestData.MultipleOf.InvalidCases), MemberType = typeof(MustNumberClausesTestData.MultipleOf))]
    public void MultipleOf_BehavesAsExpected(MustCase<(int value, int factor)> tc)
    {
        var result = Must.Be.MultipleOf(tc.Value.value, tc.Value.factor, paramName: "value");
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustNumberClausesTestData.NotMultipleOf.ValidCases), MemberType = typeof(MustNumberClausesTestData.NotMultipleOf))]
    [MemberData(nameof(MustNumberClausesTestData.NotMultipleOf.InvalidCases), MemberType = typeof(MustNumberClausesTestData.NotMultipleOf))]
    public void NotMultipleOf_BehavesAsExpected(MustCase<(int value, int factor)> tc)
    {
        var result = Must.Be.NotMultipleOf(tc.Value.value, tc.Value.factor, paramName: "value");
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustNumberClausesTestData.Even.ValidCasesInt), MemberType = typeof(MustNumberClausesTestData.Even))]
    [MemberData(nameof(MustNumberClausesTestData.Even.InvalidCasesInt), MemberType = typeof(MustNumberClausesTestData.Even))]
    public void Even_Int_BehavesAsExpected(MustCase<int> tc)
    {
        var result = Must.Be.Even(tc.Value, paramName: "value");
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustNumberClausesTestData.Even.ValidCasesLong), MemberType = typeof(MustNumberClausesTestData.Even))]
    [MemberData(nameof(MustNumberClausesTestData.Even.InvalidCasesLong), MemberType = typeof(MustNumberClausesTestData.Even))]
    public void Even_Long_BehavesAsExpected(MustCase<long> tc)
    {
        var result = Must.Be.Even(tc.Value, paramName: "value");
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustNumberClausesTestData.Odd.ValidCasesInt), MemberType = typeof(MustNumberClausesTestData.Odd))]
    [MemberData(nameof(MustNumberClausesTestData.Odd.InvalidCasesInt), MemberType = typeof(MustNumberClausesTestData.Odd))]
    public void Odd_Int_BehavesAsExpected(MustCase<int> tc)
    {
        var result = Must.Be.Odd(tc.Value, paramName: "value");
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustNumberClausesTestData.Odd.ValidCasesLong), MemberType = typeof(MustNumberClausesTestData.Odd))]
    [MemberData(nameof(MustNumberClausesTestData.Odd.InvalidCasesLong), MemberType = typeof(MustNumberClausesTestData.Odd))]
    public void Odd_Long_BehavesAsExpected(MustCase<long> tc)
    {
        var result = Must.Be.Odd(tc.Value, paramName: "value");
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustNumberClausesTestData.Finite.ValidCasesFloat), MemberType = typeof(MustNumberClausesTestData.Finite))]
    [MemberData(nameof(MustNumberClausesTestData.Finite.InvalidCasesFloat), MemberType = typeof(MustNumberClausesTestData.Finite))]
    public void Finite_Float_BehavesAsExpected(MustCase<float> tc)
    {
        var result = Must.Be.Finite(tc.Value, paramName: "value");
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustNumberClausesTestData.Finite.ValidCasesDouble), MemberType = typeof(MustNumberClausesTestData.Finite))]
    [MemberData(nameof(MustNumberClausesTestData.Finite.InvalidCasesDouble), MemberType = typeof(MustNumberClausesTestData.Finite))]
    public void Finite_Double_BehavesAsExpected(MustCase<double> tc)
    {
        var result = Must.Be.Finite(tc.Value, paramName: "value");
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustNumberClausesTestData.NotFinite.ValidCasesFloat), MemberType = typeof(MustNumberClausesTestData.NotFinite))]
    [MemberData(nameof(MustNumberClausesTestData.NotFinite.InvalidCasesFloat), MemberType = typeof(MustNumberClausesTestData.NotFinite))]
    public void NotFinite_Float_BehavesAsExpected(MustCase<float> tc)
    {
        var result = Must.Be.NotFinite(tc.Value, paramName: "value");
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustNumberClausesTestData.NotFinite.ValidCasesDouble), MemberType = typeof(MustNumberClausesTestData.NotFinite))]
    [MemberData(nameof(MustNumberClausesTestData.NotFinite.InvalidCasesDouble), MemberType = typeof(MustNumberClausesTestData.NotFinite))]
    public void NotFinite_Double_BehavesAsExpected(MustCase<double> tc)
    {
        var result = Must.Be.NotFinite(tc.Value, paramName: "value");
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustNumberClausesTestData.NotNaN.ValidCasesFloat), MemberType = typeof(MustNumberClausesTestData.NotNaN))]
    [MemberData(nameof(MustNumberClausesTestData.NotNaN.InvalidCasesFloat), MemberType = typeof(MustNumberClausesTestData.NotNaN))]
    public void NotNaN_Float_BehavesAsExpected(MustCase<float> tc)
    {
        var result = Must.Be.NotNaN(tc.Value, paramName: "value");
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustNumberClausesTestData.NotNaN.ValidCasesDouble), MemberType = typeof(MustNumberClausesTestData.NotNaN))]
    [MemberData(nameof(MustNumberClausesTestData.NotNaN.InvalidCasesDouble), MemberType = typeof(MustNumberClausesTestData.NotNaN))]
    public void NotNaN_Double_BehavesAsExpected(MustCase<double> tc)
    {
        var result = Must.Be.NotNaN(tc.Value, paramName: "value");
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustNumberClausesTestData.NaN.ValidCasesFloat), MemberType = typeof(MustNumberClausesTestData.NaN))]
    [MemberData(nameof(MustNumberClausesTestData.NaN.InvalidCasesFloat), MemberType = typeof(MustNumberClausesTestData.NaN))]
    public void NaN_Float_BehavesAsExpected(MustCase<float> tc)
    {
        var result = Must.Be.NaN(tc.Value, paramName: "value");
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustNumberClausesTestData.NaN.ValidCasesDouble), MemberType = typeof(MustNumberClausesTestData.NaN))]
    [MemberData(nameof(MustNumberClausesTestData.NaN.InvalidCasesDouble), MemberType = typeof(MustNumberClausesTestData.NaN))]
    public void NaN_Double_BehavesAsExpected(MustCase<double> tc)
    {
        var result = Must.Be.NaN(tc.Value, paramName: "value");
        AssertResult(tc, result);
    }
}
