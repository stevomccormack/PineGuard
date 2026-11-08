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

    private sealed class InRangeInt32Validator : AbstractValidator<Model>
    {
        public InRangeInt32Validator(int min, int max, Inclusion inclusion) => RuleFor(x => x.Value).InRangeInt32(min, max, inclusion);
    }

    [Theory]
    [MemberData(nameof(FluentStringNumberTypesExtensionsTestData.InRangeInt32.Cases), MemberType = typeof(FluentStringNumberTypesExtensionsTestData.InRangeInt32))]
    public void InRangeInt32_BehavesAsExpected(FluentCase<(string text, int min, int max, Inclusion inclusion)> tc)
    {
        // Arrange
        var (text, min, max, inclusion) = tc.Value;

        // Act
        var result = new InRangeInt32Validator(min, max, inclusion).Validate(new Model { Value = text });

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentStringNumberTypesExtensionsTestData.InRangeInt32Null.Cases), MemberType = typeof(FluentStringNumberTypesExtensionsTestData.InRangeInt32Null))]
    public void InRangeInt32_NullText_IsValid(FluentCase<string?> tc)
    {
        // Act
        var result = new InRangeInt32Validator(1, 10, Inclusion.Inclusive).Validate(new Model { Value = tc.Value });

        // Assert
        AssertResult(tc, result);
    }

    private sealed class OutOfRangeInt32Validator : AbstractValidator<Model>
    {
        public OutOfRangeInt32Validator() => RuleFor(x => x.Value).OutOfRangeInt32(1, 10);
    }

    [Theory]
    [MemberData(nameof(FluentStringNumberTypesExtensionsTestData.OutOfRangeInt32.Cases), MemberType = typeof(FluentStringNumberTypesExtensionsTestData.OutOfRangeInt32))]
    public void OutOfRangeInt32_BehavesAsExpected(FluentCase<string?> tc)
    {
        // Act
        var result = new OutOfRangeInt32Validator().Validate(new Model { Value = tc.Value });

        // Assert
        AssertResult(tc, result);
    }

    private sealed class InRangeInt64Validator : AbstractValidator<Model>
    {
        public InRangeInt64Validator(long min, long max, Inclusion inclusion) => RuleFor(x => x.Value).InRangeInt64(min, max, inclusion);
    }

    [Theory]
    [MemberData(nameof(FluentStringNumberTypesExtensionsTestData.InRangeInt64.Cases), MemberType = typeof(FluentStringNumberTypesExtensionsTestData.InRangeInt64))]
    public void InRangeInt64_BehavesAsExpected(FluentCase<(string text, long min, long max, Inclusion inclusion)> tc)
    {
        // Arrange
        var (text, min, max, inclusion) = tc.Value;

        // Act
        var result = new InRangeInt64Validator(min, max, inclusion).Validate(new Model { Value = text });

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentStringNumberTypesExtensionsTestData.InRangeInt64Null.Cases), MemberType = typeof(FluentStringNumberTypesExtensionsTestData.InRangeInt64Null))]
    public void InRangeInt64_NullText_IsValid(FluentCase<string?> tc)
    {
        // Act
        var result = new InRangeInt64Validator(1L, 10L, Inclusion.Inclusive).Validate(new Model { Value = tc.Value });

        // Assert
        AssertResult(tc, result);
    }

    private sealed class OutOfRangeInt64Validator : AbstractValidator<Model>
    {
        public OutOfRangeInt64Validator() => RuleFor(x => x.Value).OutOfRangeInt64(1L, 10L);
    }

    [Theory]
    [MemberData(nameof(FluentStringNumberTypesExtensionsTestData.OutOfRangeInt64.Cases), MemberType = typeof(FluentStringNumberTypesExtensionsTestData.OutOfRangeInt64))]
    public void OutOfRangeInt64_BehavesAsExpected(FluentCase<string?> tc)
    {
        // Act
        var result = new OutOfRangeInt64Validator().Validate(new Model { Value = tc.Value });

        // Assert
        AssertResult(tc, result);
    }
}
