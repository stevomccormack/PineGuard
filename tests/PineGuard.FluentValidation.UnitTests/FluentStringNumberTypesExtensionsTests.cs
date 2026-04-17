using FluentValidation;
using PineGuard.Common;
using PineGuard.Testing.UnitTests.FluentValidation;
using Xunit.Abstractions;

namespace PineGuard.FluentValidation.UnitTests;

public sealed class FluentStringNumberTypesExtensionsTests(ITestOutputHelper output) : BaseFluentUnitTest(output)
{
    private sealed record Model { public string? Value { get; init; } }

    private sealed class DecimalValidator : AbstractValidator<Model>
    {
        public DecimalValidator() => RuleFor(x => x.Value).Decimal();
    }

    [Theory]
    [MemberData(nameof(FluentStringNumberTypesExtensionsTestData.Decimal.Cases), MemberType = typeof(FluentStringNumberTypesExtensionsTestData.Decimal))]
    public void Decimal_BehavesAsExpected(FluentCase<string?> tc)
    {
        // Act
        var result = new DecimalValidator().Validate(new Model { Value = tc.Value });

        // Assert
        AssertResult(tc, result);
    }

    private sealed class ExactDecimalValidator : AbstractValidator<Model>
    {
        public ExactDecimalValidator() => RuleFor(x => x.Value).ExactDecimal();
    }

    [Theory]
    [MemberData(nameof(FluentStringNumberTypesExtensionsTestData.ExactDecimal.Cases), MemberType = typeof(FluentStringNumberTypesExtensionsTestData.ExactDecimal))]
    public void ExactDecimal_BehavesAsExpected(FluentCase<string?> tc)
    {
        // Act
        var result = new ExactDecimalValidator().Validate(new Model { Value = tc.Value });

        // Assert
        AssertResult(tc, result);
    }

    private sealed class Int32Validator : AbstractValidator<Model>
    {
        public Int32Validator() => RuleFor(x => x.Value).Int32();
    }

    [Theory]
    [MemberData(nameof(FluentStringNumberTypesExtensionsTestData.Int32.Cases), MemberType = typeof(FluentStringNumberTypesExtensionsTestData.Int32))]
    public void Int32_BehavesAsExpected(FluentCase<string?> tc)
    {
        // Act
        var result = new Int32Validator().Validate(new Model { Value = tc.Value });

        // Assert
        AssertResult(tc, result);
    }

    private sealed class Int64Validator : AbstractValidator<Model>
    {
        public Int64Validator() => RuleFor(x => x.Value).Int64();
    }

    [Theory]
    [MemberData(nameof(FluentStringNumberTypesExtensionsTestData.Int64.Cases), MemberType = typeof(FluentStringNumberTypesExtensionsTestData.Int64))]
    public void Int64_BehavesAsExpected(FluentCase<string?> tc)
    {
        // Act
        var result = new Int64Validator().Validate(new Model { Value = tc.Value });

        // Assert
        AssertResult(tc, result);
    }

    private sealed class Int32InRangeValidator : AbstractValidator<Model>
    {
        public Int32InRangeValidator(int min, int max, Inclusion inclusion) => RuleFor(x => x.Value).Int32InRange(min, max, inclusion);
    }

    [Theory]
    [MemberData(nameof(FluentStringNumberTypesExtensionsTestData.Int32InRange.Cases), MemberType = typeof(FluentStringNumberTypesExtensionsTestData.Int32InRange))]
    public void Int32InRange_BehavesAsExpected(FluentCase<(string text, int min, int max, Inclusion inclusion)> tc)
    {
        // Arrange
        var (text, min, max, inclusion) = tc.Value;

        // Act
        var result = new Int32InRangeValidator(min, max, inclusion).Validate(new Model { Value = text });

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentStringNumberTypesExtensionsTestData.Int32InRangeNull.Cases), MemberType = typeof(FluentStringNumberTypesExtensionsTestData.Int32InRangeNull))]
    public void Int32InRange_NullText_IsValid(FluentCase<string?> tc)
    {
        // Act
        var result = new Int32InRangeValidator(1, 10, Inclusion.Inclusive).Validate(new Model { Value = tc.Value });

        // Assert
        AssertResult(tc, result);
    }

    private sealed class Int32OutOfRangeValidator : AbstractValidator<Model>
    {
        public Int32OutOfRangeValidator() => RuleFor(x => x.Value).Int32OutOfRange(1, 10);
    }

    [Theory]
    [MemberData(nameof(FluentStringNumberTypesExtensionsTestData.Int32OutOfRange.Cases), MemberType = typeof(FluentStringNumberTypesExtensionsTestData.Int32OutOfRange))]
    public void Int32OutOfRange_BehavesAsExpected(FluentCase<string?> tc)
    {
        // Act
        var result = new Int32OutOfRangeValidator().Validate(new Model { Value = tc.Value });

        // Assert
        AssertResult(tc, result);
    }

    private sealed class Int64InRangeValidator : AbstractValidator<Model>
    {
        public Int64InRangeValidator(long min, long max, Inclusion inclusion) => RuleFor(x => x.Value).Int64InRange(min, max, inclusion);
    }

    [Theory]
    [MemberData(nameof(FluentStringNumberTypesExtensionsTestData.Int64InRange.Cases), MemberType = typeof(FluentStringNumberTypesExtensionsTestData.Int64InRange))]
    public void Int64InRange_BehavesAsExpected(FluentCase<(string text, long min, long max, Inclusion inclusion)> tc)
    {
        // Arrange
        var (text, min, max, inclusion) = tc.Value;

        // Act
        var result = new Int64InRangeValidator(min, max, inclusion).Validate(new Model { Value = text });

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentStringNumberTypesExtensionsTestData.Int64InRangeNull.Cases), MemberType = typeof(FluentStringNumberTypesExtensionsTestData.Int64InRangeNull))]
    public void Int64InRange_NullText_IsValid(FluentCase<string?> tc)
    {
        // Act
        var result = new Int64InRangeValidator(1L, 10L, Inclusion.Inclusive).Validate(new Model { Value = tc.Value });

        // Assert
        AssertResult(tc, result);
    }

    private sealed class Int64OutOfRangeValidator : AbstractValidator<Model>
    {
        public Int64OutOfRangeValidator() => RuleFor(x => x.Value).Int64OutOfRange(1L, 10L);
    }

    [Theory]
    [MemberData(nameof(FluentStringNumberTypesExtensionsTestData.Int64OutOfRange.Cases), MemberType = typeof(FluentStringNumberTypesExtensionsTestData.Int64OutOfRange))]
    public void Int64OutOfRange_BehavesAsExpected(FluentCase<string?> tc)
    {
        // Act
        var result = new Int64OutOfRangeValidator().Validate(new Model { Value = tc.Value });

        // Assert
        AssertResult(tc, result);
    }
}
