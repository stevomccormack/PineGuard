using FluentValidation;
using PineGuard.Testing.UnitTests.FluentValidation;
using Xunit.Abstractions;

namespace PineGuard.FluentValidation.UnitTests;

public sealed class FluentDecimalExtensionsTests(ITestOutputHelper output) : BaseFluentUnitTest(output)
{
    private sealed record Model { public decimal? Value { get; init; } }

    private sealed class ScaleAtMostValidator : AbstractValidator<Model>
    {
        public ScaleAtMostValidator(int scale) => RuleFor(x => x.Value).ScaleAtMost(scale);
    }

    private sealed class PrecisionAtMostValidator : AbstractValidator<Model>
    {
        public PrecisionAtMostValidator(int precision) => RuleFor(x => x.Value).PrecisionAtMost(precision);
    }

    private sealed class WithinPrecisionValidator : AbstractValidator<Model>
    {
        public WithinPrecisionValidator(int precision, int scale) => RuleFor(x => x.Value).WithinPrecision(precision, scale);
    }

    // FluentDecimalExtensions.ScaleAtMost
    [Theory]
    [MemberData(nameof(FluentDecimalExtensionsTestData.ScaleAtMost.Cases), MemberType = typeof(FluentDecimalExtensionsTestData.ScaleAtMost))]
    public void ScaleAtMost_BehavesAsExpected(FluentCase<(decimal? value, int scale)> tc)
    {
        // Act
        var result = new ScaleAtMostValidator(tc.Value.scale).Validate(new Model { Value = tc.Value.value });

        // Assert
        AssertResult(tc, result);
    }

    // FluentDecimalExtensions.PrecisionAtMost
    [Theory]
    [MemberData(nameof(FluentDecimalExtensionsTestData.PrecisionAtMost.Cases), MemberType = typeof(FluentDecimalExtensionsTestData.PrecisionAtMost))]
    public void PrecisionAtMost_BehavesAsExpected(FluentCase<(decimal? value, int precision)> tc)
    {
        // Act
        var result = new PrecisionAtMostValidator(tc.Value.precision).Validate(new Model { Value = tc.Value.value });

        // Assert
        AssertResult(tc, result);
    }

    // FluentDecimalExtensions.WithinPrecision
    [Theory]
    [MemberData(nameof(FluentDecimalExtensionsTestData.WithinPrecision.Cases), MemberType = typeof(FluentDecimalExtensionsTestData.WithinPrecision))]
    public void WithinPrecision_BehavesAsExpected(FluentCase<(decimal? value, int precision, int scale)> tc)
    {
        // Act
        var result = new WithinPrecisionValidator(tc.Value.precision, tc.Value.scale).Validate(new Model { Value = tc.Value.value });

        // Assert
        AssertResult(tc, result);
    }
}
