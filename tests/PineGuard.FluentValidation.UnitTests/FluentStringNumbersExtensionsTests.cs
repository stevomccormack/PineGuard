using FluentValidation;
using PineGuard.Testing.UnitTests.FluentValidation;
using Xunit.Abstractions;

namespace PineGuard.FluentValidation.UnitTests;

public sealed class FluentStringNumbersExtensionsTests(ITestOutputHelper output) : BaseFluentUnitTest(output)
{
    private sealed record Model { public string? Value { get; init; } }

    private sealed class PositiveValidator : AbstractValidator<Model>
    {
        public PositiveValidator() => RuleFor(x => x.Value).Positive();
    }

    [Theory]
    [MemberData(nameof(FluentStringNumbersExtensionsTestData.Positive.Cases), MemberType = typeof(FluentStringNumbersExtensionsTestData.Positive))]
    public void Positive_BehavesAsExpected(FluentCase<string?> tc)
    {
        // Act
        var result = new PositiveValidator().Validate(new Model { Value = tc.Value });

        // Assert
        AssertResult(tc, result);
    }

    private sealed class NegativeValidator : AbstractValidator<Model>
    {
        public NegativeValidator() => RuleFor(x => x.Value).Negative();
    }

    [Theory]
    [MemberData(nameof(FluentStringNumbersExtensionsTestData.Negative.Cases), MemberType = typeof(FluentStringNumbersExtensionsTestData.Negative))]
    public void Negative_BehavesAsExpected(FluentCase<string?> tc)
    {
        // Act
        var result = new NegativeValidator().Validate(new Model { Value = tc.Value });

        // Assert
        AssertResult(tc, result);
    }

    private sealed class ZeroValidator : AbstractValidator<Model>
    {
        public ZeroValidator() => RuleFor(x => x.Value).Zero();
    }

    [Theory]
    [MemberData(nameof(FluentStringNumbersExtensionsTestData.Zero.Cases), MemberType = typeof(FluentStringNumbersExtensionsTestData.Zero))]
    public void Zero_BehavesAsExpected(FluentCase<string?> tc)
    {
        // Act
        var result = new ZeroValidator().Validate(new Model { Value = tc.Value });

        // Assert
        AssertResult(tc, result);
    }

    private sealed class NotZeroValidator : AbstractValidator<Model>
    {
        public NotZeroValidator() => RuleFor(x => x.Value).NotZero();
    }

    [Theory]
    [MemberData(nameof(FluentStringNumbersExtensionsTestData.NotZero.Cases), MemberType = typeof(FluentStringNumbersExtensionsTestData.NotZero))]
    public void NotZero_BehavesAsExpected(FluentCase<string?> tc)
    {
        // Act
        var result = new NotZeroValidator().Validate(new Model { Value = tc.Value });

        // Assert
        AssertResult(tc, result);
    }

    private sealed class ZeroOrPositiveValidator : AbstractValidator<Model>
    {
        public ZeroOrPositiveValidator() => RuleFor(x => x.Value).ZeroOrPositive();
    }

    [Theory]
    [MemberData(nameof(FluentStringNumbersExtensionsTestData.ZeroOrPositive.Cases), MemberType = typeof(FluentStringNumbersExtensionsTestData.ZeroOrPositive))]
    public void ZeroOrPositive_BehavesAsExpected(FluentCase<string?> tc)
    {
        // Act
        var result = new ZeroOrPositiveValidator().Validate(new Model { Value = tc.Value });

        // Assert
        AssertResult(tc, result);
    }

    private sealed class ZeroOrNegativeValidator : AbstractValidator<Model>
    {
        public ZeroOrNegativeValidator() => RuleFor(x => x.Value).ZeroOrNegative();
    }

    [Theory]
    [MemberData(nameof(FluentStringNumbersExtensionsTestData.ZeroOrNegative.Cases), MemberType = typeof(FluentStringNumbersExtensionsTestData.ZeroOrNegative))]
    public void ZeroOrNegative_BehavesAsExpected(FluentCase<string?> tc)
    {
        // Act
        var result = new ZeroOrNegativeValidator().Validate(new Model { Value = tc.Value });

        // Assert
        AssertResult(tc, result);
    }

    private sealed class GreaterThanValidator : AbstractValidator<Model>
    {
        public GreaterThanValidator(decimal min) => RuleFor(x => x.Value).GreaterThan(min);
    }

    [Theory]
    [MemberData(nameof(FluentStringNumbersExtensionsTestData.GreaterThan.Cases), MemberType = typeof(FluentStringNumbersExtensionsTestData.GreaterThan))]
    public void GreaterThan_BehavesAsExpected(FluentCase<(string? value, decimal min)> tc)
    {
        // Arrange
        var (value, min) = tc.Value;

        // Act
        var result = new GreaterThanValidator(min).Validate(new Model { Value = value });

        // Assert
        AssertResult(tc, result);
    }

    private sealed class GreaterThanOrEqualValidator : AbstractValidator<Model>
    {
        public GreaterThanOrEqualValidator(decimal min) => RuleFor(x => x.Value).GreaterThanOrEqual(min);
    }

    [Theory]
    [MemberData(nameof(FluentStringNumbersExtensionsTestData.GreaterThanOrEqual.Cases), MemberType = typeof(FluentStringNumbersExtensionsTestData.GreaterThanOrEqual))]
    public void GreaterThanOrEqual_BehavesAsExpected(FluentCase<(string? value, decimal min)> tc)
    {
        // Arrange
        var (value, min) = tc.Value;

        // Act
        var result = new GreaterThanOrEqualValidator(min).Validate(new Model { Value = value });

        // Assert
        AssertResult(tc, result);
    }

    private sealed class LessThanValidator : AbstractValidator<Model>
    {
        public LessThanValidator(decimal max) => RuleFor(x => x.Value).LessThan(max);
    }

    [Theory]
    [MemberData(nameof(FluentStringNumbersExtensionsTestData.LessThan.Cases), MemberType = typeof(FluentStringNumbersExtensionsTestData.LessThan))]
    public void LessThan_BehavesAsExpected(FluentCase<(string? value, decimal max)> tc)
    {
        // Arrange
        var (value, max) = tc.Value;

        // Act
        var result = new LessThanValidator(max).Validate(new Model { Value = value });

        // Assert
        AssertResult(tc, result);
    }

    private sealed class LessThanOrEqualValidator : AbstractValidator<Model>
    {
        public LessThanOrEqualValidator(decimal max) => RuleFor(x => x.Value).LessThanOrEqual(max);
    }

    [Theory]
    [MemberData(nameof(FluentStringNumbersExtensionsTestData.LessThanOrEqual.Cases), MemberType = typeof(FluentStringNumbersExtensionsTestData.LessThanOrEqual))]
    public void LessThanOrEqual_BehavesAsExpected(FluentCase<(string? value, decimal max)> tc)
    {
        // Arrange
        var (value, max) = tc.Value;

        // Act
        var result = new LessThanOrEqualValidator(max).Validate(new Model { Value = value });

        // Assert
        AssertResult(tc, result);
    }

    private sealed class InRangeValidator : AbstractValidator<Model>
    {
        public InRangeValidator(decimal min, decimal max, PineGuard.Common.Inclusion inclusion) => RuleFor(x => x.Value).InRange(min, max, inclusion);
    }

    [Theory]
    [MemberData(nameof(FluentStringNumbersExtensionsTestData.InRange.Cases), MemberType = typeof(FluentStringNumbersExtensionsTestData.InRange))]
    public void InRange_BehavesAsExpected(FluentCase<(string? value, decimal min, decimal max, PineGuard.Common.Inclusion inclusion)> tc)
    {
        // Arrange
        var (value, min, max, inclusion) = tc.Value;

        // Act
        var result = new InRangeValidator(min, max, inclusion).Validate(new Model { Value = value });

        // Assert
        AssertResult(tc, result);
    }

    private sealed class OutOfRangeValidator : AbstractValidator<Model>
    {
        public OutOfRangeValidator() => RuleFor(x => x.Value).OutOfRange(10m, 20m);
    }

    [Theory]
    [MemberData(nameof(FluentStringNumbersExtensionsTestData.OutOfRange.Cases), MemberType = typeof(FluentStringNumbersExtensionsTestData.OutOfRange))]
    public void OutOfRange_BehavesAsExpected(FluentCase<string?> tc)
    {
        // Act
        var result = new OutOfRangeValidator().Validate(new Model { Value = tc.Value });

        // Assert
        AssertResult(tc, result);
    }

    private sealed class ApproximatelyValidator : AbstractValidator<Model>
    {
        public ApproximatelyValidator(decimal target, decimal? tolerance) => RuleFor(x => x.Value).Approximately(target, tolerance);
    }

    [Theory]
    [MemberData(nameof(FluentStringNumbersExtensionsTestData.Approximately.Cases), MemberType = typeof(FluentStringNumbersExtensionsTestData.Approximately))]
    public void Approximately_BehavesAsExpected(FluentCase<(string? value, decimal target, decimal? tolerance)> tc)
    {
        // Arrange
        var (value, target, tolerance) = tc.Value;

        // Act
        var result = new ApproximatelyValidator(target, tolerance).Validate(new Model { Value = value });

        // Assert
        AssertResult(tc, result);
    }

    private sealed class NotApproximatelyValidator : AbstractValidator<Model>
    {
        public NotApproximatelyValidator() => RuleFor(x => x.Value).NotApproximately(10.0m, 0.2m);
    }

    [Theory]
    [MemberData(nameof(FluentStringNumbersExtensionsTestData.NotApproximately.Cases), MemberType = typeof(FluentStringNumbersExtensionsTestData.NotApproximately))]
    public void NotApproximately_BehavesAsExpected(FluentCase<string?> tc)
    {
        // Act
        var result = new NotApproximatelyValidator().Validate(new Model { Value = tc.Value });

        // Assert
        AssertResult(tc, result);
    }

    private sealed class MultipleOfValidator : AbstractValidator<Model>
    {
        public MultipleOfValidator(decimal factor) => RuleFor(x => x.Value).MultipleOf(factor);
    }

    [Theory]
    [MemberData(nameof(FluentStringNumbersExtensionsTestData.MultipleOf.Cases), MemberType = typeof(FluentStringNumbersExtensionsTestData.MultipleOf))]
    public void MultipleOf_BehavesAsExpected(FluentCase<(string? value, decimal factor)> tc)
    {
        // Arrange
        var (value, factor) = tc.Value;

        // Act
        var result = new MultipleOfValidator(factor).Validate(new Model { Value = value });

        // Assert
        AssertResult(tc, result);
    }

    private sealed class NotMultipleOfValidator : AbstractValidator<Model>
    {
        public NotMultipleOfValidator() => RuleFor(x => x.Value).NotMultipleOf(2m);
    }

    [Theory]
    [MemberData(nameof(FluentStringNumbersExtensionsTestData.NotMultipleOf.Cases), MemberType = typeof(FluentStringNumbersExtensionsTestData.NotMultipleOf))]
    public void NotMultipleOf_BehavesAsExpected(FluentCase<string?> tc)
    {
        // Act
        var result = new NotMultipleOfValidator().Validate(new Model { Value = tc.Value });

        // Assert
        AssertResult(tc, result);
    }

    private sealed class EvenValidator : AbstractValidator<Model>
    {
        public EvenValidator() => RuleFor(x => x.Value).Even();
    }

    [Theory]
    [MemberData(nameof(FluentStringNumbersExtensionsTestData.Even.Cases), MemberType = typeof(FluentStringNumbersExtensionsTestData.Even))]
    public void Even_BehavesAsExpected(FluentCase<string?> tc)
    {
        // Act
        var result = new EvenValidator().Validate(new Model { Value = tc.Value });

        // Assert
        AssertResult(tc, result);
    }

    private sealed class OddValidator : AbstractValidator<Model>
    {
        public OddValidator() => RuleFor(x => x.Value).Odd();
    }

    [Theory]
    [MemberData(nameof(FluentStringNumbersExtensionsTestData.Odd.Cases), MemberType = typeof(FluentStringNumbersExtensionsTestData.Odd))]
    public void Odd_BehavesAsExpected(FluentCase<string?> tc)
    {
        // Act
        var result = new OddValidator().Validate(new Model { Value = tc.Value });

        // Assert
        AssertResult(tc, result);
    }

    private sealed class FiniteValidator : AbstractValidator<Model>
    {
        public FiniteValidator() => RuleFor(x => x.Value).Finite();
    }

    [Theory]
    [MemberData(nameof(FluentStringNumbersExtensionsTestData.Finite.Cases), MemberType = typeof(FluentStringNumbersExtensionsTestData.Finite))]
    public void Finite_BehavesAsExpected(FluentCase<string?> tc)
    {
        // Act
        var result = new FiniteValidator().Validate(new Model { Value = tc.Value });

        // Assert
        AssertResult(tc, result);
    }

    private sealed class NotFiniteValidator : AbstractValidator<Model>
    {
        public NotFiniteValidator() => RuleFor(x => x.Value).NotFinite();
    }

    [Theory]
    [MemberData(nameof(FluentStringNumbersExtensionsTestData.NotFinite.Cases), MemberType = typeof(FluentStringNumbersExtensionsTestData.NotFinite))]
    public void NotFinite_BehavesAsExpected(FluentCase<string?> tc)
    {
        // Act
        var result = new NotFiniteValidator().Validate(new Model { Value = tc.Value });

        // Assert
        AssertResult(tc, result);
    }

    private sealed class NotNaNValidator : AbstractValidator<Model>
    {
        public NotNaNValidator() => RuleFor(x => x.Value).NotNaN();
    }

    [Theory]
    [MemberData(nameof(FluentStringNumbersExtensionsTestData.NotNaN.Cases), MemberType = typeof(FluentStringNumbersExtensionsTestData.NotNaN))]
    public void NotNaN_BehavesAsExpected(FluentCase<string?> tc)
    {
        // Act
        var result = new NotNaNValidator().Validate(new Model { Value = tc.Value });

        // Assert
        AssertResult(tc, result);
    }
}
