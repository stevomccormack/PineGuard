using PineGuard.Common;
using PineGuard.Testing.UnitTests.GuardClauses;
using Xunit.Abstractions;
using TD = PineGuard.GuardClauses.UnitTests.GuardStringNumbersClausesTestData;

namespace PineGuard.GuardClauses.UnitTests;

public sealed class GuardStringNumbersClausesTests(ITestOutputHelper output) : BaseGuardUnitTest(output)
{
    // ── Simple string? ops ──────────────────────────────────────────

    [Theory]
    [MemberData(nameof(TD.ZeroOrNegative.ValidCases), MemberType = typeof(TD.ZeroOrNegative))]
    [MemberData(nameof(TD.ZeroOrNegative.InvalidCases), MemberType = typeof(TD.ZeroOrNegative))]
    public void ZeroOrNegative_BehavesAsExpected(GuardCase<string?> tc)
    {
        var value = tc.Value;
        AssertResult(tc, () => Guard.Against.ZeroOrNegative(value));
        AssertCustomMessage(tc, () => Guard.Against.ZeroOrNegative(value, message: CustomMessage));
    }

    [Theory]
    [MemberData(nameof(TD.ZeroOrPositive.ValidCases), MemberType = typeof(TD.ZeroOrPositive))]
    [MemberData(nameof(TD.ZeroOrPositive.InvalidCases), MemberType = typeof(TD.ZeroOrPositive))]
    public void ZeroOrPositive_BehavesAsExpected(GuardCase<string?> tc)
    {
        var value = tc.Value;
        AssertResult(tc, () => Guard.Against.ZeroOrPositive(value));
        AssertCustomMessage(tc, () => Guard.Against.ZeroOrPositive(value, message: CustomMessage));
    }

    [Theory]
    [MemberData(nameof(TD.NotZero.ValidCases), MemberType = typeof(TD.NotZero))]
    [MemberData(nameof(TD.NotZero.InvalidCases), MemberType = typeof(TD.NotZero))]
    public void NotZero_BehavesAsExpected(GuardCase<string?> tc)
    {
        var value = tc.Value;
        AssertResult(tc, () => Guard.Against.NotZero(value));
        AssertCustomMessage(tc, () => Guard.Against.NotZero(value, message: CustomMessage));
    }

    [Theory]
    [MemberData(nameof(TD.Zero.ValidCases), MemberType = typeof(TD.Zero))]
    [MemberData(nameof(TD.Zero.InvalidCases), MemberType = typeof(TD.Zero))]
    public void Zero_BehavesAsExpected(GuardCase<string?> tc)
    {
        var value = tc.Value;
        AssertResult(tc, () => Guard.Against.Zero(value));
        AssertCustomMessage(tc, () => Guard.Against.Zero(value, message: CustomMessage));
    }

    [Theory]
    [MemberData(nameof(TD.Negative.ValidCases), MemberType = typeof(TD.Negative))]
    [MemberData(nameof(TD.Negative.InvalidCases), MemberType = typeof(TD.Negative))]
    public void Negative_BehavesAsExpected(GuardCase<string?> tc)
    {
        var value = tc.Value;
        AssertResult(tc, () => Guard.Against.Negative(value));
        AssertCustomMessage(tc, () => Guard.Against.Negative(value, message: CustomMessage));
    }

    [Theory]
    [MemberData(nameof(TD.Positive.ValidCases), MemberType = typeof(TD.Positive))]
    [MemberData(nameof(TD.Positive.InvalidCases), MemberType = typeof(TD.Positive))]
    public void Positive_BehavesAsExpected(GuardCase<string?> tc)
    {
        var value = tc.Value;
        AssertResult(tc, () => Guard.Against.Positive(value));
        AssertCustomMessage(tc, () => Guard.Against.Positive(value, message: CustomMessage));
    }

    [Theory]
    [MemberData(nameof(TD.Odd.ValidCases), MemberType = typeof(TD.Odd))]
    [MemberData(nameof(TD.Odd.InvalidCases), MemberType = typeof(TD.Odd))]
    public void Odd_BehavesAsExpected(GuardCase<string?> tc)
    {
        var value = tc.Value;
        AssertResult(tc, () => Guard.Against.Odd(value));
        AssertCustomMessage(tc, () => Guard.Against.Odd(value, message: CustomMessage));
    }

    [Theory]
    [MemberData(nameof(TD.Even.ValidCases), MemberType = typeof(TD.Even))]
    [MemberData(nameof(TD.Even.InvalidCases), MemberType = typeof(TD.Even))]
    public void Even_BehavesAsExpected(GuardCase<string?> tc)
    {
        var value = tc.Value;
        AssertResult(tc, () => Guard.Against.Even(value));
        AssertCustomMessage(tc, () => Guard.Against.Even(value, message: CustomMessage));
    }

    [Theory]
    [MemberData(nameof(TD.NotFinite.ValidCases), MemberType = typeof(TD.NotFinite))]
    [MemberData(nameof(TD.NotFinite.InvalidCases), MemberType = typeof(TD.NotFinite))]
    public void NotFinite_BehavesAsExpected(GuardCase<string?> tc)
    {
        var value = tc.Value;
        AssertResult(tc, () => Guard.Against.NotFinite(value));
        AssertCustomMessage(tc, () => Guard.Against.NotFinite(value, message: CustomMessage));
    }

    [Theory]
    [MemberData(nameof(TD.NotPercentage.ValidCases), MemberType = typeof(TD.NotPercentage))]
    [MemberData(nameof(TD.NotPercentage.InvalidCases), MemberType = typeof(TD.NotPercentage))]
    public void NotPercentage_BehavesAsExpected(GuardCase<string?> tc)
    {
        var value = tc.Value;
        AssertResult(tc, () => Guard.Against.NotPercentage(value));
        AssertCustomMessage(tc, () => Guard.Against.NotPercentage(value, message: CustomMessage));
    }

    [Theory]
    [MemberData(nameof(TD.Finite.ValidCases), MemberType = typeof(TD.Finite))]
    [MemberData(nameof(TD.Finite.InvalidCases), MemberType = typeof(TD.Finite))]
    public void Finite_BehavesAsExpected(GuardCase<string?> tc)
    {
        var value = tc.Value;
        AssertResult(tc, () => Guard.Against.Finite(value));
        AssertCustomMessage(tc, () => Guard.Against.Finite(value, message: CustomMessage));
    }

    [Theory]
    [MemberData(nameof(TD.NaN.ValidCases), MemberType = typeof(TD.NaN))]
    [MemberData(nameof(TD.NaN.InvalidCases), MemberType = typeof(TD.NaN))]
    public void NaN_BehavesAsExpected(GuardCase<string?> tc)
    {
        var value = tc.Value;
        AssertResult(tc, () => Guard.Against.NaN(value));
        AssertCustomMessage(tc, () => Guard.Against.NaN(value, message: CustomMessage));
    }

    // ── Comparison ops (tuple) ──────────────────────────────────────

    [Theory]
    [MemberData(nameof(TD.LessThanOrEqual.ValidCases), MemberType = typeof(TD.LessThanOrEqual))]
    [MemberData(nameof(TD.LessThanOrEqual.InvalidCases), MemberType = typeof(TD.LessThanOrEqual))]
    public void LessThanOrEqual_BehavesAsExpected(GuardCase<(string? text, decimal min)> tc)
    {
        var value = tc.Value.text;
        AssertResult(tc, () => Guard.Against.LessThanOrEqual(value, tc.Value.min));
        AssertCustomMessage(tc, () => Guard.Against.LessThanOrEqual(value, tc.Value.min, message: CustomMessage));
    }

    [Theory]
    [MemberData(nameof(TD.LessThan.ValidCases), MemberType = typeof(TD.LessThan))]
    [MemberData(nameof(TD.LessThan.InvalidCases), MemberType = typeof(TD.LessThan))]
    public void LessThan_BehavesAsExpected(GuardCase<(string? text, decimal min)> tc)
    {
        var value = tc.Value.text;
        AssertResult(tc, () => Guard.Against.LessThan(value, tc.Value.min));
        AssertCustomMessage(tc, () => Guard.Against.LessThan(value, tc.Value.min, message: CustomMessage));
    }

    [Theory]
    [MemberData(nameof(TD.GreaterThanOrEqual.ValidCases), MemberType = typeof(TD.GreaterThanOrEqual))]
    [MemberData(nameof(TD.GreaterThanOrEqual.InvalidCases), MemberType = typeof(TD.GreaterThanOrEqual))]
    public void GreaterThanOrEqual_BehavesAsExpected(GuardCase<(string? text, decimal max)> tc)
    {
        var value = tc.Value.text;
        AssertResult(tc, () => Guard.Against.GreaterThanOrEqual(value, tc.Value.max));
        AssertCustomMessage(tc, () => Guard.Against.GreaterThanOrEqual(value, tc.Value.max, message: CustomMessage));
    }

    [Theory]
    [MemberData(nameof(TD.GreaterThan.ValidCases), MemberType = typeof(TD.GreaterThan))]
    [MemberData(nameof(TD.GreaterThan.InvalidCases), MemberType = typeof(TD.GreaterThan))]
    public void GreaterThan_BehavesAsExpected(GuardCase<(string? text, decimal max)> tc)
    {
        var value = tc.Value.text;
        AssertResult(tc, () => Guard.Against.GreaterThan(value, tc.Value.max));
        AssertCustomMessage(tc, () => Guard.Against.GreaterThan(value, tc.Value.max, message: CustomMessage));
    }

    // ── Range ops (tuple) ───────────────────────────────────────────

    [Theory]
    [MemberData(nameof(TD.OutOfRange.ValidCases), MemberType = typeof(TD.OutOfRange))]
    [MemberData(nameof(TD.OutOfRange.InvalidCases), MemberType = typeof(TD.OutOfRange))]
    public void OutOfRange_BehavesAsExpected(GuardCase<(string? text, decimal min, decimal max, Inclusion inclusion)> tc)
    {
        var value = tc.Value.text;
        AssertResult(tc, () => Guard.Against.OutOfRange(value, tc.Value.min, tc.Value.max, tc.Value.inclusion));
        AssertCustomMessage(tc, () => Guard.Against.OutOfRange(value, tc.Value.min, tc.Value.max, tc.Value.inclusion, message: CustomMessage));
    }

    [Theory]
    [MemberData(nameof(TD.InRange.ValidCases), MemberType = typeof(TD.InRange))]
    [MemberData(nameof(TD.InRange.InvalidCases), MemberType = typeof(TD.InRange))]
    public void InRange_BehavesAsExpected(GuardCase<(string? text, decimal min, decimal max, Inclusion inclusion)> tc)
    {
        var value = tc.Value.text;
        AssertResult(tc, () => Guard.Against.InRange(value, tc.Value.min, tc.Value.max, tc.Value.inclusion));
        AssertCustomMessage(tc, () => Guard.Against.InRange(value, tc.Value.min, tc.Value.max, tc.Value.inclusion, message: CustomMessage));
    }

    // ── Approximation ops (tuple) ───────────────────────────────────

    [Theory]
    [MemberData(nameof(TD.NotApproximately.ValidCases), MemberType = typeof(TD.NotApproximately))]
    [MemberData(nameof(TD.NotApproximately.InvalidCases), MemberType = typeof(TD.NotApproximately))]
    public void NotApproximately_BehavesAsExpected(GuardCase<(string? text, decimal target, decimal? tolerance)> tc)
    {
        var value = tc.Value.text;
        AssertResult(tc, () => Guard.Against.NotApproximately(value, tc.Value.target, tc.Value.tolerance));
        AssertCustomMessage(tc, () => Guard.Against.NotApproximately(value, tc.Value.target, tc.Value.tolerance, message: CustomMessage));
    }

    [Theory]
    [MemberData(nameof(TD.Approximately.ValidCases), MemberType = typeof(TD.Approximately))]
    [MemberData(nameof(TD.Approximately.InvalidCases), MemberType = typeof(TD.Approximately))]
    public void Approximately_BehavesAsExpected(GuardCase<(string? text, decimal target, decimal? tolerance)> tc)
    {
        var value = tc.Value.text;
        AssertResult(tc, () => Guard.Against.Approximately(value, tc.Value.target, tc.Value.tolerance));
        AssertCustomMessage(tc, () => Guard.Against.Approximately(value, tc.Value.target, tc.Value.tolerance, message: CustomMessage));
    }

    // ── Multiple ops (tuple) ────────────────────────────────────────

    [Theory]
    [MemberData(nameof(TD.NotMultipleOf.ValidCases), MemberType = typeof(TD.NotMultipleOf))]
    [MemberData(nameof(TD.NotMultipleOf.InvalidCases), MemberType = typeof(TD.NotMultipleOf))]
    public void NotMultipleOf_BehavesAsExpected(GuardCase<(string? text, decimal factor)> tc)
    {
        var value = tc.Value.text;
        AssertResult(tc, () => Guard.Against.NotMultipleOf(value, tc.Value.factor));
        AssertCustomMessage(tc, () => Guard.Against.NotMultipleOf(value, tc.Value.factor, message: CustomMessage));
    }

    [Theory]
    [MemberData(nameof(TD.MultipleOf.ValidCases), MemberType = typeof(TD.MultipleOf))]
    [MemberData(nameof(TD.MultipleOf.InvalidCases), MemberType = typeof(TD.MultipleOf))]
    public void MultipleOf_BehavesAsExpected(GuardCase<(string? text, decimal factor)> tc)
    {
        var value = tc.Value.text;
        AssertResult(tc, () => Guard.Against.MultipleOf(value, tc.Value.factor));
        AssertCustomMessage(tc, () => Guard.Against.MultipleOf(value, tc.Value.factor, message: CustomMessage));
    }
}
