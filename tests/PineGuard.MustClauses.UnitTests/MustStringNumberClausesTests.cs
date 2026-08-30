using PineGuard.Common;
using PineGuard.Testing.UnitTests.MustClauses;
using Xunit.Abstractions;

namespace PineGuard.MustClauses.UnitTests;

public sealed class MustStringNumberClausesTests(ITestOutputHelper output) : BaseMustUnitTest(output)
{
    [Theory]
    [MemberData(nameof(MustStringNumberClausesTestData.Positive.ValidCases), MemberType = typeof(MustStringNumberClausesTestData.Positive))]
    [MemberData(nameof(MustStringNumberClausesTestData.Positive.InvalidCases), MemberType = typeof(MustStringNumberClausesTestData.Positive))]
    public void Positive_BehavesAsExpected(MustCase<string?> tc)
    {
        var result = Must.Be.Positive(tc.Value, paramName: "value");
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustStringNumberClausesTestData.Negative.ValidCases), MemberType = typeof(MustStringNumberClausesTestData.Negative))]
    [MemberData(nameof(MustStringNumberClausesTestData.Negative.InvalidCases), MemberType = typeof(MustStringNumberClausesTestData.Negative))]
    public void Negative_BehavesAsExpected(MustCase<string?> tc)
    {
        var result = Must.Be.Negative(tc.Value, paramName: "value");
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustStringNumberClausesTestData.Zero.ValidCases), MemberType = typeof(MustStringNumberClausesTestData.Zero))]
    [MemberData(nameof(MustStringNumberClausesTestData.Zero.InvalidCases), MemberType = typeof(MustStringNumberClausesTestData.Zero))]
    public void Zero_BehavesAsExpected(MustCase<string?> tc)
    {
        var result = Must.Be.Zero(tc.Value, paramName: "value");
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustStringNumberClausesTestData.NotZero.ValidCases), MemberType = typeof(MustStringNumberClausesTestData.NotZero))]
    [MemberData(nameof(MustStringNumberClausesTestData.NotZero.InvalidCases), MemberType = typeof(MustStringNumberClausesTestData.NotZero))]
    public void NotZero_BehavesAsExpected(MustCase<string?> tc)
    {
        var result = Must.Be.NotZero(tc.Value, paramName: "value");
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustStringNumberClausesTestData.ZeroOrPositive.ValidCases), MemberType = typeof(MustStringNumberClausesTestData.ZeroOrPositive))]
    [MemberData(nameof(MustStringNumberClausesTestData.ZeroOrPositive.InvalidCases), MemberType = typeof(MustStringNumberClausesTestData.ZeroOrPositive))]
    public void ZeroOrPositive_BehavesAsExpected(MustCase<string?> tc)
    {
        var result = Must.Be.ZeroOrPositive(tc.Value, paramName: "value");
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustStringNumberClausesTestData.ZeroOrNegative.ValidCases), MemberType = typeof(MustStringNumberClausesTestData.ZeroOrNegative))]
    [MemberData(nameof(MustStringNumberClausesTestData.ZeroOrNegative.InvalidCases), MemberType = typeof(MustStringNumberClausesTestData.ZeroOrNegative))]
    public void ZeroOrNegative_BehavesAsExpected(MustCase<string?> tc)
    {
        var result = Must.Be.ZeroOrNegative(tc.Value, paramName: "value");
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustStringNumberClausesTestData.GreaterThan.ValidCases), MemberType = typeof(MustStringNumberClausesTestData.GreaterThan))]
    [MemberData(nameof(MustStringNumberClausesTestData.GreaterThan.InvalidCases), MemberType = typeof(MustStringNumberClausesTestData.GreaterThan))]
    public void GreaterThan_BehavesAsExpected(MustCase<(string? text, decimal min)> tc)
    {
        var result = Must.Be.GreaterThan(tc.Value.text, tc.Value.min, paramName: "value");
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustStringNumberClausesTestData.GreaterThanOrEqual.ValidCases), MemberType = typeof(MustStringNumberClausesTestData.GreaterThanOrEqual))]
    [MemberData(nameof(MustStringNumberClausesTestData.GreaterThanOrEqual.InvalidCases), MemberType = typeof(MustStringNumberClausesTestData.GreaterThanOrEqual))]
    public void GreaterThanOrEqual_BehavesAsExpected(MustCase<(string? text, decimal min)> tc)
    {
        var result = Must.Be.GreaterThanOrEqual(tc.Value.text, tc.Value.min, paramName: "value");
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustStringNumberClausesTestData.LessThan.ValidCases), MemberType = typeof(MustStringNumberClausesTestData.LessThan))]
    [MemberData(nameof(MustStringNumberClausesTestData.LessThan.InvalidCases), MemberType = typeof(MustStringNumberClausesTestData.LessThan))]
    public void LessThan_BehavesAsExpected(MustCase<(string? text, decimal max)> tc)
    {
        var result = Must.Be.LessThan(tc.Value.text, tc.Value.max, paramName: "value");
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustStringNumberClausesTestData.LessThanOrEqual.ValidCases), MemberType = typeof(MustStringNumberClausesTestData.LessThanOrEqual))]
    [MemberData(nameof(MustStringNumberClausesTestData.LessThanOrEqual.InvalidCases), MemberType = typeof(MustStringNumberClausesTestData.LessThanOrEqual))]
    public void LessThanOrEqual_BehavesAsExpected(MustCase<(string? text, decimal max)> tc)
    {
        var result = Must.Be.LessThanOrEqual(tc.Value.text, tc.Value.max, paramName: "value");
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustStringNumberClausesTestData.InRange.ValidCases), MemberType = typeof(MustStringNumberClausesTestData.InRange))]
    [MemberData(nameof(MustStringNumberClausesTestData.InRange.InvalidCases), MemberType = typeof(MustStringNumberClausesTestData.InRange))]
    public void InRange_BehavesAsExpected(MustCase<(string? text, decimal min, decimal max, Inclusion inclusion)> tc)
    {
        var result = Must.Be.InRange(tc.Value.text, tc.Value.min, tc.Value.max, tc.Value.inclusion, paramName: "value");
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustStringNumberClausesTestData.Approximately.ValidCases), MemberType = typeof(MustStringNumberClausesTestData.Approximately))]
    [MemberData(nameof(MustStringNumberClausesTestData.Approximately.InvalidCases), MemberType = typeof(MustStringNumberClausesTestData.Approximately))]
    public void Approximately_BehavesAsExpected(MustCase<(string? text, decimal target, decimal? tolerance)> tc)
    {
        var result = Must.Be.Approximately(tc.Value.text, tc.Value.target, tc.Value.tolerance, paramName: "value");
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustStringNumberClausesTestData.MultipleOf.ValidCases), MemberType = typeof(MustStringNumberClausesTestData.MultipleOf))]
    [MemberData(nameof(MustStringNumberClausesTestData.MultipleOf.InvalidCases), MemberType = typeof(MustStringNumberClausesTestData.MultipleOf))]
    public void MultipleOf_BehavesAsExpected(MustCase<(string? text, decimal factor)> tc)
    {
        var result = Must.Be.MultipleOf(tc.Value.text, tc.Value.factor, paramName: "value");
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustStringNumberClausesTestData.Even.ValidCases), MemberType = typeof(MustStringNumberClausesTestData.Even))]
    [MemberData(nameof(MustStringNumberClausesTestData.Even.InvalidCases), MemberType = typeof(MustStringNumberClausesTestData.Even))]
    public void Even_BehavesAsExpected(MustCase<string?> tc)
    {
        var result = Must.Be.Even(tc.Value, paramName: "value");
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustStringNumberClausesTestData.Odd.ValidCases), MemberType = typeof(MustStringNumberClausesTestData.Odd))]
    [MemberData(nameof(MustStringNumberClausesTestData.Odd.InvalidCases), MemberType = typeof(MustStringNumberClausesTestData.Odd))]
    public void Odd_BehavesAsExpected(MustCase<string?> tc)
    {
        var result = Must.Be.Odd(tc.Value, paramName: "value");
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustStringNumberClausesTestData.Finite.ValidCases), MemberType = typeof(MustStringNumberClausesTestData.Finite))]
    [MemberData(nameof(MustStringNumberClausesTestData.Finite.InvalidCases), MemberType = typeof(MustStringNumberClausesTestData.Finite))]
    public void Finite_BehavesAsExpected(MustCase<string?> tc)
    {
        var result = Must.Be.Finite(tc.Value, paramName: "value");
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustStringNumberClausesTestData.IsNaN.ValidCases), MemberType = typeof(MustStringNumberClausesTestData.IsNaN))]
    [MemberData(nameof(MustStringNumberClausesTestData.IsNaN.InvalidCases), MemberType = typeof(MustStringNumberClausesTestData.IsNaN))]
    [MemberData(nameof(MustStringNumberClausesTestData.IsNaN.NullCases), MemberType = typeof(MustStringNumberClausesTestData.IsNaN))]
    [MemberData(nameof(MustStringNumberClausesTestData.IsNaN.LettersCases), MemberType = typeof(MustStringNumberClausesTestData.IsNaN))]
    public void IsNaN_BehavesAsExpected(MustCase<string?> tc)
    {
        var result = Must.Be.NotNaN(tc.Value, paramName: "value");
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustStringNumberClausesTestData.OutOfRange.ValidCases), MemberType = typeof(MustStringNumberClausesTestData.OutOfRange))]
    [MemberData(nameof(MustStringNumberClausesTestData.OutOfRange.InvalidCases), MemberType = typeof(MustStringNumberClausesTestData.OutOfRange))]
    [MemberData(nameof(MustStringNumberClausesTestData.OutOfRange.NullCases), MemberType = typeof(MustStringNumberClausesTestData.OutOfRange))]
    [MemberData(nameof(MustStringNumberClausesTestData.OutOfRange.InvalidRangeCases), MemberType = typeof(MustStringNumberClausesTestData.OutOfRange))]
    [MemberData(nameof(MustStringNumberClausesTestData.OutOfRange.LettersCases), MemberType = typeof(MustStringNumberClausesTestData.OutOfRange))]
    public void OutOfRange_BehavesAsExpected(MustCase<(string? text, decimal min, decimal max, Inclusion inclusion)> tc)
    {
        var result = Must.Be.OutOfRange(tc.Value.text, tc.Value.min, tc.Value.max, tc.Value.inclusion, paramName: "value");
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustStringNumberClausesTestData.Percentage.ValidCases), MemberType = typeof(MustStringNumberClausesTestData.Percentage))]
    [MemberData(nameof(MustStringNumberClausesTestData.Percentage.InvalidCases), MemberType = typeof(MustStringNumberClausesTestData.Percentage))]
    [MemberData(nameof(MustStringNumberClausesTestData.Percentage.NullCases), MemberType = typeof(MustStringNumberClausesTestData.Percentage))]
    public void Percentage_BehavesAsExpected(MustCase<string?> tc)
    {
        var result = Must.Be.Percentage(tc.Value, paramName: "value");
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustStringNumberClausesTestData.NotApproximately.ValidCases), MemberType = typeof(MustStringNumberClausesTestData.NotApproximately))]
    [MemberData(nameof(MustStringNumberClausesTestData.NotApproximately.InvalidCases), MemberType = typeof(MustStringNumberClausesTestData.NotApproximately))]
    [MemberData(nameof(MustStringNumberClausesTestData.NotApproximately.NullCases), MemberType = typeof(MustStringNumberClausesTestData.NotApproximately))]
    [MemberData(nameof(MustStringNumberClausesTestData.NotApproximately.NullToleranceCases), MemberType = typeof(MustStringNumberClausesTestData.NotApproximately))]
    [MemberData(nameof(MustStringNumberClausesTestData.NotApproximately.LettersCases), MemberType = typeof(MustStringNumberClausesTestData.NotApproximately))]
    public void NotApproximately_BehavesAsExpected(MustCase<(string? text, decimal target, decimal? tolerance)> tc)
    {
        var result = Must.Be.NotApproximately(tc.Value.text, tc.Value.target, tc.Value.tolerance, paramName: "value");
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustStringNumberClausesTestData.NotMultipleOf.ValidCases), MemberType = typeof(MustStringNumberClausesTestData.NotMultipleOf))]
    [MemberData(nameof(MustStringNumberClausesTestData.NotMultipleOf.InvalidCases), MemberType = typeof(MustStringNumberClausesTestData.NotMultipleOf))]
    [MemberData(nameof(MustStringNumberClausesTestData.NotMultipleOf.NullCases), MemberType = typeof(MustStringNumberClausesTestData.NotMultipleOf))]
    [MemberData(nameof(MustStringNumberClausesTestData.NotMultipleOf.ZeroFactorCases), MemberType = typeof(MustStringNumberClausesTestData.NotMultipleOf))]
    [MemberData(nameof(MustStringNumberClausesTestData.NotMultipleOf.LettersCases), MemberType = typeof(MustStringNumberClausesTestData.NotMultipleOf))]
    public void NotMultipleOf_BehavesAsExpected(MustCase<(string? text, decimal factor)> tc)
    {
        var result = Must.Be.NotMultipleOf(tc.Value.text, tc.Value.factor, paramName: "value");
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustStringNumberClausesTestData.NotFinite.ValidCases), MemberType = typeof(MustStringNumberClausesTestData.NotFinite))]
    [MemberData(nameof(MustStringNumberClausesTestData.NotFinite.InvalidCases), MemberType = typeof(MustStringNumberClausesTestData.NotFinite))]
    [MemberData(nameof(MustStringNumberClausesTestData.NotFinite.NullCases), MemberType = typeof(MustStringNumberClausesTestData.NotFinite))]
    [MemberData(nameof(MustStringNumberClausesTestData.NotFinite.LettersCases), MemberType = typeof(MustStringNumberClausesTestData.NotFinite))]
    public void NotFinite_BehavesAsExpected(MustCase<string?> tc)
    {
        var result = Must.Be.NotFinite(tc.Value, paramName: "value");
        AssertResult(tc, result);
    }
}
