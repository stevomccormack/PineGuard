using FluentValidation;
using PineGuard.Common;
using PineGuard.Testing.UnitTests.FluentValidation;
using Xunit.Abstractions;

namespace PineGuard.FluentValidation.UnitTests;

public sealed class FluentDateOnlyRangeExtensionsTests(ITestOutputHelper output) : BaseFluentUnitTest(output)
{
    private sealed record Model { public DateOnlyRange? Value { get; init; } }
    private sealed record NonNullableModel { public DateOnlyRange Value { get; init; } }

    private sealed class ChronologicalValidator : AbstractValidator<Model> { public ChronologicalValidator() => RuleFor(x => x.Value).Chronological(); }
    private sealed class OverlappingValidator : AbstractValidator<Model> { public OverlappingValidator(DateOnlyRange other) => RuleFor(x => x.Value).Overlapping(other); }
    private sealed class NotOverlappingValidator : AbstractValidator<Model> { public NotOverlappingValidator(DateOnlyRange other) => RuleFor(x => x.Value).NotOverlapping(other); }
    private sealed class ContainsValidator : AbstractValidator<Model> { public ContainsValidator(DateOnly item) => RuleFor(x => x.Value).Contains(item); }
    private sealed class NotContainsValidator : AbstractValidator<Model> { public NotContainsValidator(DateOnly item) => RuleFor(x => x.Value).NotContains(item); }

    private sealed class ChronologicalNonNullableValidator : AbstractValidator<NonNullableModel> { public ChronologicalNonNullableValidator() => RuleFor(x => x.Value).Chronological(); }
    private sealed class OverlappingNonNullableValidator : AbstractValidator<NonNullableModel> { public OverlappingNonNullableValidator(DateOnlyRange other) => RuleFor(x => x.Value).Overlapping(other); }
    private sealed class NotOverlappingNonNullableValidator : AbstractValidator<NonNullableModel> { public NotOverlappingNonNullableValidator(DateOnlyRange other) => RuleFor(x => x.Value).NotOverlapping(other); }
    private sealed class ContainsNonNullableValidator : AbstractValidator<NonNullableModel> { public ContainsNonNullableValidator(DateOnly item) => RuleFor(x => x.Value).Contains(item); }
    private sealed class NotContainsNonNullableValidator : AbstractValidator<NonNullableModel> { public NotContainsNonNullableValidator(DateOnly item) => RuleFor(x => x.Value).NotContains(item); }

    [Theory]
    [MemberData(nameof(FluentDateOnlyRangeExtensionsTestData.Chronological.Cases), MemberType = typeof(FluentDateOnlyRangeExtensionsTestData.Chronological))]
    public void Chronological_BehavesAsExpected(FluentCase<DateOnlyRange?> tc)
    {
        var result = new ChronologicalValidator().Validate(new Model { Value = tc.Value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateOnlyRangeExtensionsTestData.Overlapping.Cases), MemberType = typeof(FluentDateOnlyRangeExtensionsTestData.Overlapping))]
    public void Overlapping_BehavesAsExpected(FluentCase<(DateOnlyRange? value, DateOnlyRange other)> tc)
    {
        var result = new OverlappingValidator(tc.Value.other).Validate(new Model { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateOnlyRangeExtensionsTestData.NotOverlapping.Cases), MemberType = typeof(FluentDateOnlyRangeExtensionsTestData.NotOverlapping))]
    public void NotOverlapping_BehavesAsExpected(FluentCase<(DateOnlyRange? value, DateOnlyRange other)> tc)
    {
        var result = new NotOverlappingValidator(tc.Value.other).Validate(new Model { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateOnlyRangeExtensionsTestData.Contains.Cases), MemberType = typeof(FluentDateOnlyRangeExtensionsTestData.Contains))]
    public void Contains_BehavesAsExpected(FluentCase<(DateOnlyRange? value, DateOnly item)> tc)
    {
        var result = new ContainsValidator(tc.Value.item).Validate(new Model { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateOnlyRangeExtensionsTestData.NotContains.Cases), MemberType = typeof(FluentDateOnlyRangeExtensionsTestData.NotContains))]
    public void NotContains_BehavesAsExpected(FluentCase<(DateOnlyRange? value, DateOnly item)> tc)
    {
        var result = new NotContainsValidator(tc.Value.item).Validate(new Model { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateOnlyRangeExtensionsTestData.ChronologicalNonNullable.Cases), MemberType = typeof(FluentDateOnlyRangeExtensionsTestData.ChronologicalNonNullable))]
    public void ChronologicalNonNullable_BehavesAsExpected(FluentCase<DateOnlyRange> tc)
    {
        var result = new ChronologicalNonNullableValidator().Validate(new NonNullableModel { Value = tc.Value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateOnlyRangeExtensionsTestData.OverlappingNonNullable.Cases), MemberType = typeof(FluentDateOnlyRangeExtensionsTestData.OverlappingNonNullable))]
    public void OverlappingNonNullable_BehavesAsExpected(FluentCase<(DateOnlyRange value, DateOnlyRange other)> tc)
    {
        var result = new OverlappingNonNullableValidator(tc.Value.other).Validate(new NonNullableModel { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateOnlyRangeExtensionsTestData.NotOverlappingNonNullable.Cases), MemberType = typeof(FluentDateOnlyRangeExtensionsTestData.NotOverlappingNonNullable))]
    public void NotOverlappingNonNullable_BehavesAsExpected(FluentCase<(DateOnlyRange value, DateOnlyRange other)> tc)
    {
        var result = new NotOverlappingNonNullableValidator(tc.Value.other).Validate(new NonNullableModel { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateOnlyRangeExtensionsTestData.ContainsNonNullable.Cases), MemberType = typeof(FluentDateOnlyRangeExtensionsTestData.ContainsNonNullable))]
    public void ContainsNonNullable_BehavesAsExpected(FluentCase<(DateOnlyRange value, DateOnly item)> tc)
    {
        var result = new ContainsNonNullableValidator(tc.Value.item).Validate(new NonNullableModel { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateOnlyRangeExtensionsTestData.NotContainsNonNullable.Cases), MemberType = typeof(FluentDateOnlyRangeExtensionsTestData.NotContainsNonNullable))]
    public void NotContainsNonNullable_BehavesAsExpected(FluentCase<(DateOnlyRange value, DateOnly item)> tc)
    {
        var result = new NotContainsNonNullableValidator(tc.Value.item).Validate(new NonNullableModel { Value = tc.Value.value });
        AssertResult(tc, result);
    }
}
