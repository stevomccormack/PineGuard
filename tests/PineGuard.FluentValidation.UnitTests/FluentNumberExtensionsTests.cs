using FluentValidation;
using PineGuard.Common;
using PineGuard.Testing.UnitTests.FluentValidation;
using Xunit.Abstractions;

namespace PineGuard.FluentValidation.UnitTests;

public sealed class FluentNumberExtensionsTests(ITestOutputHelper output) : BaseFluentUnitTest(output)
{
    private sealed record IntModel { public int? Value { get; init; } }
    private sealed record LongModel { public long? Value { get; init; } }
    private sealed record FloatModel { public float? Value { get; init; } }
    private sealed record DoubleModel { public double? Value { get; init; } }
    private sealed record DecimalModel { public decimal? Value { get; init; } }
    private sealed record NonNullableIntModel { public int Value { get; init; } }
    private sealed record NonNullableLongModel { public long Value { get; init; } }
    private sealed record NonNullableFloatModel { public float Value { get; init; } }
    private sealed record NonNullableDoubleModel { public double Value { get; init; } }

    private sealed class PositiveValidator : AbstractValidator<IntModel>
    {
        public PositiveValidator() => RuleFor(x => x.Value).Positive();
    }

    [Theory]
    [MemberData(nameof(FluentNumberExtensionsTestData.Positive.Cases), MemberType = typeof(FluentNumberExtensionsTestData.Positive))]
    public void Positive_BehavesAsExpected(FluentCase<int?> tc)
    {
        var result = new PositiveValidator().Validate(new IntModel { Value = tc.Value });
        AssertResult(tc, result);
    }

    private sealed class NegativeValidator : AbstractValidator<IntModel>
    {
        public NegativeValidator() => RuleFor(x => x.Value).Negative();
    }

    [Theory]
    [MemberData(nameof(FluentNumberExtensionsTestData.Negative.Cases), MemberType = typeof(FluentNumberExtensionsTestData.Negative))]
    public void Negative_BehavesAsExpected(FluentCase<int?> tc)
    {
        var result = new NegativeValidator().Validate(new IntModel { Value = tc.Value });
        AssertResult(tc, result);
    }

    private sealed class ZeroValidator : AbstractValidator<IntModel>
    {
        public ZeroValidator() => RuleFor(x => x.Value).Zero();
    }

    [Theory]
    [MemberData(nameof(FluentNumberExtensionsTestData.Zero.Cases), MemberType = typeof(FluentNumberExtensionsTestData.Zero))]
    public void Zero_BehavesAsExpected(FluentCase<int?> tc)
    {
        var result = new ZeroValidator().Validate(new IntModel { Value = tc.Value });
        AssertResult(tc, result);
    }

    private sealed class NotZeroValidator : AbstractValidator<IntModel>
    {
        public NotZeroValidator() => RuleFor(x => x.Value).NotZero();
    }

    [Theory]
    [MemberData(nameof(FluentNumberExtensionsTestData.NotZero.Cases), MemberType = typeof(FluentNumberExtensionsTestData.NotZero))]
    public void NotZero_BehavesAsExpected(FluentCase<int?> tc)
    {
        var result = new NotZeroValidator().Validate(new IntModel { Value = tc.Value });
        AssertResult(tc, result);
    }

    private sealed class ZeroOrPositiveValidator : AbstractValidator<IntModel>
    {
        public ZeroOrPositiveValidator() => RuleFor(x => x.Value).ZeroOrPositive();
    }

    [Theory]
    [MemberData(nameof(FluentNumberExtensionsTestData.ZeroOrPositive.Cases), MemberType = typeof(FluentNumberExtensionsTestData.ZeroOrPositive))]
    public void ZeroOrPositive_BehavesAsExpected(FluentCase<int?> tc)
    {
        var result = new ZeroOrPositiveValidator().Validate(new IntModel { Value = tc.Value });
        AssertResult(tc, result);
    }

    private sealed class ZeroOrNegativeValidator : AbstractValidator<IntModel>
    {
        public ZeroOrNegativeValidator() => RuleFor(x => x.Value).ZeroOrNegative();
    }

    [Theory]
    [MemberData(nameof(FluentNumberExtensionsTestData.ZeroOrNegative.Cases), MemberType = typeof(FluentNumberExtensionsTestData.ZeroOrNegative))]
    public void ZeroOrNegative_BehavesAsExpected(FluentCase<int?> tc)
    {
        var result = new ZeroOrNegativeValidator().Validate(new IntModel { Value = tc.Value });
        AssertResult(tc, result);
    }

    private sealed class InRangeValidator : AbstractValidator<IntModel>
    {
        public InRangeValidator(int min, int max, Inclusion inclusion) => RuleFor(x => x.Value).InRange(min, max, inclusion);
    }

    [Theory]
    [MemberData(nameof(FluentNumberExtensionsTestData.InRange.Cases), MemberType = typeof(FluentNumberExtensionsTestData.InRange))]
    public void InRange_BehavesAsExpected(FluentCase<(int? value, int min, int max, Inclusion inclusion)> tc)
    {
        var result = new InRangeValidator(tc.Value.min, tc.Value.max, tc.Value.inclusion).Validate(new IntModel { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    private sealed class OutOfRangeValidator : AbstractValidator<IntModel>
    {
        public OutOfRangeValidator(int min, int max, Inclusion inclusion) => RuleFor(x => x.Value).OutOfRange(min, max, inclusion);
    }

    [Theory]
    [MemberData(nameof(FluentNumberExtensionsTestData.OutOfRange.Cases), MemberType = typeof(FluentNumberExtensionsTestData.OutOfRange))]
    public void OutOfRange_BehavesAsExpected(FluentCase<(int? value, int min, int max, Inclusion inclusion)> tc)
    {
        var result = new OutOfRangeValidator(tc.Value.min, tc.Value.max, tc.Value.inclusion).Validate(new IntModel { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    private sealed class PercentageValidator : AbstractValidator<DecimalModel>
    {
        public PercentageValidator() => RuleFor(x => x.Value).Percentage();
    }

    [Theory]
    [MemberData(nameof(FluentNumberExtensionsTestData.Percentage.Cases), MemberType = typeof(FluentNumberExtensionsTestData.Percentage))]
    public void Percentage_BehavesAsExpected(FluentCase<decimal?> tc)
    {
        var result = new PercentageValidator().Validate(new DecimalModel { Value = tc.Value });
        AssertResult(tc, result);
    }

    private sealed class ApproximatelyValidator : AbstractValidator<DecimalModel>
    {
        public ApproximatelyValidator(decimal target, decimal? tolerance) => RuleFor(x => x.Value).Approximately(target, tolerance);
    }

    [Theory]
    [MemberData(nameof(FluentNumberExtensionsTestData.Approximately.Cases), MemberType = typeof(FluentNumberExtensionsTestData.Approximately))]
    public void Approximately_BehavesAsExpected(FluentCase<(decimal? value, decimal target, decimal? tolerance)> tc)
    {
        var result = new ApproximatelyValidator(tc.Value.target, tc.Value.tolerance).Validate(new DecimalModel { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    private sealed class NotApproximatelyValidator : AbstractValidator<DecimalModel>
    {
        public NotApproximatelyValidator(decimal target, decimal? tolerance) => RuleFor(x => x.Value).NotApproximately(target, tolerance);
    }

    [Theory]
    [MemberData(nameof(FluentNumberExtensionsTestData.NotApproximately.Cases), MemberType = typeof(FluentNumberExtensionsTestData.NotApproximately))]
    public void NotApproximately_BehavesAsExpected(FluentCase<(decimal? value, decimal target, decimal? tolerance)> tc)
    {
        var result = new NotApproximatelyValidator(tc.Value.target, tc.Value.tolerance).Validate(new DecimalModel { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    private sealed class MultipleOfValidator : AbstractValidator<IntModel>
    {
        public MultipleOfValidator(int factor) => RuleFor(x => x.Value).MultipleOf(factor);
    }

    [Theory]
    [MemberData(nameof(FluentNumberExtensionsTestData.MultipleOf.Cases), MemberType = typeof(FluentNumberExtensionsTestData.MultipleOf))]
    public void MultipleOf_BehavesAsExpected(FluentCase<(int? value, int factor)> tc)
    {
        var result = new MultipleOfValidator(tc.Value.factor).Validate(new IntModel { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    private sealed class NotMultipleOfValidator : AbstractValidator<IntModel>
    {
        public NotMultipleOfValidator(int factor) => RuleFor(x => x.Value).NotMultipleOf(factor);
    }

    [Theory]
    [MemberData(nameof(FluentNumberExtensionsTestData.NotMultipleOf.Cases), MemberType = typeof(FluentNumberExtensionsTestData.NotMultipleOf))]
    public void NotMultipleOf_BehavesAsExpected(FluentCase<(int? value, int factor)> tc)
    {
        var result = new NotMultipleOfValidator(tc.Value.factor).Validate(new IntModel { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    private sealed class EvenIntValidator : AbstractValidator<IntModel>
    {
        public EvenIntValidator() => RuleFor(x => x.Value).Even();
    }

    [Theory]
    [MemberData(nameof(FluentNumberExtensionsTestData.Even.IntCases), MemberType = typeof(FluentNumberExtensionsTestData.Even))]
    public void Even_Int_BehavesAsExpected(FluentCase<int?> tc)
    {
        var result = new EvenIntValidator().Validate(new IntModel { Value = tc.Value });
        AssertResult(tc, result);
    }

    private sealed class EvenLongValidator : AbstractValidator<LongModel>
    {
        public EvenLongValidator() => RuleFor(x => x.Value).Even();
    }

    [Theory]
    [MemberData(nameof(FluentNumberExtensionsTestData.Even.LongCases), MemberType = typeof(FluentNumberExtensionsTestData.Even))]
    public void Even_Long_BehavesAsExpected(FluentCase<long?> tc)
    {
        var result = new EvenLongValidator().Validate(new LongModel { Value = tc.Value });
        AssertResult(tc, result);
    }

    private sealed class OddIntValidator : AbstractValidator<IntModel>
    {
        public OddIntValidator() => RuleFor(x => x.Value).Odd();
    }

    [Theory]
    [MemberData(nameof(FluentNumberExtensionsTestData.Odd.IntCases), MemberType = typeof(FluentNumberExtensionsTestData.Odd))]
    public void Odd_Int_BehavesAsExpected(FluentCase<int?> tc)
    {
        var result = new OddIntValidator().Validate(new IntModel { Value = tc.Value });
        AssertResult(tc, result);
    }

    private sealed class OddLongValidator : AbstractValidator<LongModel>
    {
        public OddLongValidator() => RuleFor(x => x.Value).Odd();
    }

    [Theory]
    [MemberData(nameof(FluentNumberExtensionsTestData.Odd.LongCases), MemberType = typeof(FluentNumberExtensionsTestData.Odd))]
    public void Odd_Long_BehavesAsExpected(FluentCase<long?> tc)
    {
        var result = new OddLongValidator().Validate(new LongModel { Value = tc.Value });
        AssertResult(tc, result);
    }

    private sealed class FiniteFloatValidator : AbstractValidator<FloatModel>
    {
        public FiniteFloatValidator() => RuleFor(x => x.Value).Finite();
    }

    [Theory]
    [MemberData(nameof(FluentNumberExtensionsTestData.Finite.FloatCases), MemberType = typeof(FluentNumberExtensionsTestData.Finite))]
    public void Finite_Float_BehavesAsExpected(FluentCase<float?> tc)
    {
        var result = new FiniteFloatValidator().Validate(new FloatModel { Value = tc.Value });
        AssertResult(tc, result);
    }

    private sealed class FiniteDoubleValidator : AbstractValidator<DoubleModel>
    {
        public FiniteDoubleValidator() => RuleFor(x => x.Value).Finite();
    }

    [Theory]
    [MemberData(nameof(FluentNumberExtensionsTestData.Finite.DoubleCases), MemberType = typeof(FluentNumberExtensionsTestData.Finite))]
    public void Finite_Double_BehavesAsExpected(FluentCase<double?> tc)
    {
        var result = new FiniteDoubleValidator().Validate(new DoubleModel { Value = tc.Value });
        AssertResult(tc, result);
    }

    private sealed class NotFiniteFloatValidator : AbstractValidator<FloatModel>
    {
        public NotFiniteFloatValidator() => RuleFor(x => x.Value).NotFinite();
    }

    [Theory]
    [MemberData(nameof(FluentNumberExtensionsTestData.NotFinite.FloatCases), MemberType = typeof(FluentNumberExtensionsTestData.NotFinite))]
    public void NotFinite_Float_BehavesAsExpected(FluentCase<float?> tc)
    {
        var result = new NotFiniteFloatValidator().Validate(new FloatModel { Value = tc.Value });
        AssertResult(tc, result);
    }

    private sealed class NotFiniteDoubleValidator : AbstractValidator<DoubleModel>
    {
        public NotFiniteDoubleValidator() => RuleFor(x => x.Value).NotFinite();
    }

    [Theory]
    [MemberData(nameof(FluentNumberExtensionsTestData.NotFinite.DoubleCases), MemberType = typeof(FluentNumberExtensionsTestData.NotFinite))]
    public void NotFinite_Double_BehavesAsExpected(FluentCase<double?> tc)
    {
        var result = new NotFiniteDoubleValidator().Validate(new DoubleModel { Value = tc.Value });
        AssertResult(tc, result);
    }

    private sealed class NaNFloatValidator : AbstractValidator<FloatModel>
    {
        public NaNFloatValidator() => RuleFor(x => x.Value).NaN();
    }

    [Theory]
    [MemberData(nameof(FluentNumberExtensionsTestData.NaN.FloatCases), MemberType = typeof(FluentNumberExtensionsTestData.NaN))]
    public void NaN_Float_BehavesAsExpected(FluentCase<float?> tc)
    {
        var result = new NaNFloatValidator().Validate(new FloatModel { Value = tc.Value });
        AssertResult(tc, result);
    }

    private sealed class NaNDoubleValidator : AbstractValidator<DoubleModel>
    {
        public NaNDoubleValidator() => RuleFor(x => x.Value).NaN();
    }

    [Theory]
    [MemberData(nameof(FluentNumberExtensionsTestData.NaN.DoubleCases), MemberType = typeof(FluentNumberExtensionsTestData.NaN))]
    public void NaN_Double_BehavesAsExpected(FluentCase<double?> tc)
    {
        var result = new NaNDoubleValidator().Validate(new DoubleModel { Value = tc.Value });
        AssertResult(tc, result);
    }

    private sealed class NotNaNFloatValidator : AbstractValidator<FloatModel>
    {
        public NotNaNFloatValidator() => RuleFor(x => x.Value).NotNaN();
    }

    [Theory]
    [MemberData(nameof(FluentNumberExtensionsTestData.NotNaN.FloatCases), MemberType = typeof(FluentNumberExtensionsTestData.NotNaN))]
    public void NotNaN_Float_BehavesAsExpected(FluentCase<float?> tc)
    {
        var result = new NotNaNFloatValidator().Validate(new FloatModel { Value = tc.Value });
        AssertResult(tc, result);
    }

    private sealed class NotNaNDoubleValidator : AbstractValidator<DoubleModel>
    {
        public NotNaNDoubleValidator() => RuleFor(x => x.Value).NotNaN();
    }

    [Theory]
    [MemberData(nameof(FluentNumberExtensionsTestData.NotNaN.DoubleCases), MemberType = typeof(FluentNumberExtensionsTestData.NotNaN))]
    public void NotNaN_Double_BehavesAsExpected(FluentCase<double?> tc)
    {
        var result = new NotNaNDoubleValidator().Validate(new DoubleModel { Value = tc.Value });
        AssertResult(tc, result);
    }

    // ── Non-nullable overloads ─────────────────────────────────────────────

    private sealed class EvenNonNullableIntValidator : AbstractValidator<NonNullableIntModel>
    {
        public EvenNonNullableIntValidator() => RuleFor(x => x.Value).Even();
    }

    private sealed class EvenNonNullableLongValidator : AbstractValidator<NonNullableLongModel>
    {
        public EvenNonNullableLongValidator() => RuleFor(x => x.Value).Even();
    }

    private sealed class OddNonNullableIntValidator : AbstractValidator<NonNullableIntModel>
    {
        public OddNonNullableIntValidator() => RuleFor(x => x.Value).Odd();
    }

    private sealed class OddNonNullableLongValidator : AbstractValidator<NonNullableLongModel>
    {
        public OddNonNullableLongValidator() => RuleFor(x => x.Value).Odd();
    }

    private sealed class FiniteNonNullableFloatValidator : AbstractValidator<NonNullableFloatModel>
    {
        public FiniteNonNullableFloatValidator() => RuleFor(x => x.Value).Finite();
    }

    private sealed class FiniteNonNullableDoubleValidator : AbstractValidator<NonNullableDoubleModel>
    {
        public FiniteNonNullableDoubleValidator() => RuleFor(x => x.Value).Finite();
    }

    private sealed class NotFiniteNonNullableFloatValidator : AbstractValidator<NonNullableFloatModel>
    {
        public NotFiniteNonNullableFloatValidator() => RuleFor(x => x.Value).NotFinite();
    }

    private sealed class NotFiniteNonNullableDoubleValidator : AbstractValidator<NonNullableDoubleModel>
    {
        public NotFiniteNonNullableDoubleValidator() => RuleFor(x => x.Value).NotFinite();
    }

    private sealed class NaNNonNullableFloatValidator : AbstractValidator<NonNullableFloatModel>
    {
        public NaNNonNullableFloatValidator() => RuleFor(x => x.Value).NaN();
    }

    private sealed class NaNNonNullableDoubleValidator : AbstractValidator<NonNullableDoubleModel>
    {
        public NaNNonNullableDoubleValidator() => RuleFor(x => x.Value).NaN();
    }

    private sealed class NotNaNNonNullableFloatValidator : AbstractValidator<NonNullableFloatModel>
    {
        public NotNaNNonNullableFloatValidator() => RuleFor(x => x.Value).NotNaN();
    }

    private sealed class NotNaNNonNullableDoubleValidator : AbstractValidator<NonNullableDoubleModel>
    {
        public NotNaNNonNullableDoubleValidator() => RuleFor(x => x.Value).NotNaN();
    }

    [Theory]
    [MemberData(nameof(FluentNumberExtensionsTestData.EvenNonNullable.IntCases), MemberType = typeof(FluentNumberExtensionsTestData.EvenNonNullable))]
    public void Even_NonNullableInt_BehavesAsExpected(FluentCase<int> tc)
    {
        var result = new EvenNonNullableIntValidator().Validate(new NonNullableIntModel { Value = tc.Value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentNumberExtensionsTestData.EvenNonNullable.LongCases), MemberType = typeof(FluentNumberExtensionsTestData.EvenNonNullable))]
    public void Even_NonNullableLong_BehavesAsExpected(FluentCase<long> tc)
    {
        var result = new EvenNonNullableLongValidator().Validate(new NonNullableLongModel { Value = tc.Value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentNumberExtensionsTestData.OddNonNullable.IntCases), MemberType = typeof(FluentNumberExtensionsTestData.OddNonNullable))]
    public void Odd_NonNullableInt_BehavesAsExpected(FluentCase<int> tc)
    {
        var result = new OddNonNullableIntValidator().Validate(new NonNullableIntModel { Value = tc.Value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentNumberExtensionsTestData.OddNonNullable.LongCases), MemberType = typeof(FluentNumberExtensionsTestData.OddNonNullable))]
    public void Odd_NonNullableLong_BehavesAsExpected(FluentCase<long> tc)
    {
        var result = new OddNonNullableLongValidator().Validate(new NonNullableLongModel { Value = tc.Value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentNumberExtensionsTestData.FiniteNonNullable.FloatCases), MemberType = typeof(FluentNumberExtensionsTestData.FiniteNonNullable))]
    public void Finite_NonNullableFloat_BehavesAsExpected(FluentCase<float> tc)
    {
        var result = new FiniteNonNullableFloatValidator().Validate(new NonNullableFloatModel { Value = tc.Value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentNumberExtensionsTestData.FiniteNonNullable.DoubleCases), MemberType = typeof(FluentNumberExtensionsTestData.FiniteNonNullable))]
    public void Finite_NonNullableDouble_BehavesAsExpected(FluentCase<double> tc)
    {
        var result = new FiniteNonNullableDoubleValidator().Validate(new NonNullableDoubleModel { Value = tc.Value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentNumberExtensionsTestData.NotFiniteNonNullable.FloatCases), MemberType = typeof(FluentNumberExtensionsTestData.NotFiniteNonNullable))]
    public void NotFinite_NonNullableFloat_BehavesAsExpected(FluentCase<float> tc)
    {
        var result = new NotFiniteNonNullableFloatValidator().Validate(new NonNullableFloatModel { Value = tc.Value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentNumberExtensionsTestData.NotFiniteNonNullable.DoubleCases), MemberType = typeof(FluentNumberExtensionsTestData.NotFiniteNonNullable))]
    public void NotFinite_NonNullableDouble_BehavesAsExpected(FluentCase<double> tc)
    {
        var result = new NotFiniteNonNullableDoubleValidator().Validate(new NonNullableDoubleModel { Value = tc.Value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentNumberExtensionsTestData.NaNNonNullable.FloatCases), MemberType = typeof(FluentNumberExtensionsTestData.NaNNonNullable))]
    public void NaN_NonNullableFloat_BehavesAsExpected(FluentCase<float> tc)
    {
        var result = new NaNNonNullableFloatValidator().Validate(new NonNullableFloatModel { Value = tc.Value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentNumberExtensionsTestData.NaNNonNullable.DoubleCases), MemberType = typeof(FluentNumberExtensionsTestData.NaNNonNullable))]
    public void NaN_NonNullableDouble_BehavesAsExpected(FluentCase<double> tc)
    {
        var result = new NaNNonNullableDoubleValidator().Validate(new NonNullableDoubleModel { Value = tc.Value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentNumberExtensionsTestData.NotNaNNonNullable.FloatCases), MemberType = typeof(FluentNumberExtensionsTestData.NotNaNNonNullable))]
    public void NotNaN_NonNullableFloat_BehavesAsExpected(FluentCase<float> tc)
    {
        var result = new NotNaNNonNullableFloatValidator().Validate(new NonNullableFloatModel { Value = tc.Value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentNumberExtensionsTestData.NotNaNNonNullable.DoubleCases), MemberType = typeof(FluentNumberExtensionsTestData.NotNaNNonNullable))]
    public void NotNaN_NonNullableDouble_BehavesAsExpected(FluentCase<double> tc)
    {
        var result = new NotNaNNonNullableDoubleValidator().Validate(new NonNullableDoubleModel { Value = tc.Value });
        AssertResult(tc, result);
    }
}
