using FluentValidation;
using PineGuard.Testing.UnitTests.FluentValidation;
using Xunit.Abstractions;

namespace PineGuard.FluentValidation.UnitTests;

public sealed class FluentNumberComparisonExtensionsTests(ITestOutputHelper output) : BaseFluentUnitTest(output)
{
    private sealed record Model(int Value);
    private sealed record NullableModel(int? Value);

    [Theory]
    [MemberData(nameof(FluentNumberComparisonExtensionsTestData.Cases), MemberType = typeof(FluentNumberComparisonExtensionsTestData))]
    public void Comparisons_AreUnambiguousWithFluentValidationImports(int value, int other, bool greater, bool greaterOrEqual, bool less, bool lessOrEqual)
    {
        // Arrange
        var model = new Model(value);
        var gt = new InlineValidator<Model>();
        var ge = new InlineValidator<Model>();
        var lt = new InlineValidator<Model>();
        var le = new InlineValidator<Model>();
        gt.RuleFor(x => x.Value).GreaterThanNumber(other);
        ge.RuleFor(x => x.Value).GreaterThanOrEqualNumber(other);
        lt.RuleFor(x => x.Value).LessThanNumber(other);
        le.RuleFor(x => x.Value).LessThanOrEqualNumber(other);

        // Act / Assert
        Assert.Equal(greater, gt.Validate(model).IsValid);
        Assert.Equal(greaterOrEqual, ge.Validate(model).IsValid);
        Assert.Equal(less, lt.Validate(model).IsValid);
        Assert.Equal(lessOrEqual, le.Validate(model).IsValid);
    }

    [Theory]
    [MemberData(nameof(FluentNumberComparisonExtensionsTestData.NullableCases), MemberType = typeof(FluentNumberComparisonExtensionsTestData))]
    public void NullableComparisons_PreserveOptionalValues(int? value, int other, bool greater, bool greaterOrEqual, bool less, bool lessOrEqual)
    {
        // Arrange
        var model = new NullableModel(value);
        var gt = new InlineValidator<NullableModel>();
        var ge = new InlineValidator<NullableModel>();
        var lt = new InlineValidator<NullableModel>();
        var le = new InlineValidator<NullableModel>();
        gt.RuleFor(x => x.Value).GreaterThanNumber(other);
        ge.RuleFor(x => x.Value).GreaterThanOrEqualNumber(other);
        lt.RuleFor(x => x.Value).LessThanNumber(other);
        le.RuleFor(x => x.Value).LessThanOrEqualNumber(other);

        // Act / Assert
        Assert.Equal(greater, gt.Validate(model).IsValid);
        Assert.Equal(greaterOrEqual, ge.Validate(model).IsValid);
        Assert.Equal(less, lt.Validate(model).IsValid);
        Assert.Equal(lessOrEqual, le.Validate(model).IsValid);
    }
}
