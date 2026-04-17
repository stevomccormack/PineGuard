using FluentValidation;
using PineGuard.Common;
using PineGuard.Testing.UnitTests.FluentValidation;
using Xunit.Abstractions;

namespace PineGuard.FluentValidation.UnitTests;

public sealed class FluentDateTimeOffsetRangeExtensionsTests(ITestOutputHelper output) : BaseFluentUnitTest(output)
{
    private sealed record Model { public DateTimeOffsetRange? Value { get; init; } }
    private sealed record NonNullableModel { public DateTimeOffsetRange Value { get; init; } }

    private sealed class ChronologicalValidator : AbstractValidator<Model> { public ChronologicalValidator() => RuleFor(x => x.Value).Chronological(); }
    private sealed class OverlappingValidator : AbstractValidator<Model> { public OverlappingValidator(DateTimeOffsetRange other) => RuleFor(x => x.Value).Overlapping(other); }
    private sealed class NotOverlappingValidator : AbstractValidator<Model> { public NotOverlappingValidator(DateTimeOffsetRange other) => RuleFor(x => x.Value).NotOverlapping(other); }
    private sealed class ContainsValidator : AbstractValidator<Model> { public ContainsValidator(DateTimeOffset item) => RuleFor(x => x.Value).Contains(item); }
    private sealed class NotContainsValidator : AbstractValidator<Model> { public NotContainsValidator(DateTimeOffset item) => RuleFor(x => x.Value).NotContains(item); }

    private sealed class ChronologicalNonNullableValidator : AbstractValidator<NonNullableModel> { public ChronologicalNonNullableValidator() => RuleFor(x => x.Value).Chronological(); }
    private sealed class OverlappingNonNullableValidator : AbstractValidator<NonNullableModel> { public OverlappingNonNullableValidator(DateTimeOffsetRange other) => RuleFor(x => x.Value).Overlapping(other); }
    private sealed class NotOverlappingNonNullableValidator : AbstractValidator<NonNullableModel> { public NotOverlappingNonNullableValidator(DateTimeOffsetRange other) => RuleFor(x => x.Value).NotOverlapping(other); }
    private sealed class ContainsNonNullableValidator : AbstractValidator<NonNullableModel> { public ContainsNonNullableValidator(DateTimeOffset item) => RuleFor(x => x.Value).Contains(item); }
    private sealed class NotContainsNonNullableValidator : AbstractValidator<NonNullableModel> { public NotContainsNonNullableValidator(DateTimeOffset item) => RuleFor(x => x.Value).NotContains(item); }

    [Theory]
    [MemberData(nameof(FluentDateTimeOffsetRangeExtensionsTestData.Chronological.Cases), MemberType = typeof(FluentDateTimeOffsetRangeExtensionsTestData.Chronological))]
    public void Chronological_BehavesAsExpected(FluentCase<DateTimeOffsetRange?> tc)
    {
        var result = new ChronologicalValidator().Validate(new Model { Value = tc.Value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateTimeOffsetRangeExtensionsTestData.Overlapping.Cases), MemberType = typeof(FluentDateTimeOffsetRangeExtensionsTestData.Overlapping))]
    public void Overlapping_BehavesAsExpected(FluentCase<(DateTimeOffsetRange? value, DateTimeOffsetRange other)> tc)
    {
        var result = new OverlappingValidator(tc.Value.other).Validate(new Model { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateTimeOffsetRangeExtensionsTestData.NotOverlapping.Cases), MemberType = typeof(FluentDateTimeOffsetRangeExtensionsTestData.NotOverlapping))]
    public void NotOverlapping_BehavesAsExpected(FluentCase<(DateTimeOffsetRange? value, DateTimeOffsetRange other)> tc)
    {
        var result = new NotOverlappingValidator(tc.Value.other).Validate(new Model { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateTimeOffsetRangeExtensionsTestData.Contains.Cases), MemberType = typeof(FluentDateTimeOffsetRangeExtensionsTestData.Contains))]
    public void Contains_BehavesAsExpected(FluentCase<(DateTimeOffsetRange? value, DateTimeOffset item)> tc)
    {
        var result = new ContainsValidator(tc.Value.item).Validate(new Model { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateTimeOffsetRangeExtensionsTestData.NotContains.Cases), MemberType = typeof(FluentDateTimeOffsetRangeExtensionsTestData.NotContains))]
    public void NotContains_BehavesAsExpected(FluentCase<(DateTimeOffsetRange? value, DateTimeOffset item)> tc)
    {
        var result = new NotContainsValidator(tc.Value.item).Validate(new Model { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateTimeOffsetRangeExtensionsTestData.ChronologicalNonNullable.Cases), MemberType = typeof(FluentDateTimeOffsetRangeExtensionsTestData.ChronologicalNonNullable))]
    public void ChronologicalNonNullable_BehavesAsExpected(FluentCase<DateTimeOffsetRange> tc)
    {
        var result = new ChronologicalNonNullableValidator().Validate(new NonNullableModel { Value = tc.Value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateTimeOffsetRangeExtensionsTestData.OverlappingNonNullable.Cases), MemberType = typeof(FluentDateTimeOffsetRangeExtensionsTestData.OverlappingNonNullable))]
    public void OverlappingNonNullable_BehavesAsExpected(FluentCase<(DateTimeOffsetRange value, DateTimeOffsetRange other)> tc)
    {
        var result = new OverlappingNonNullableValidator(tc.Value.other).Validate(new NonNullableModel { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateTimeOffsetRangeExtensionsTestData.NotOverlappingNonNullable.Cases), MemberType = typeof(FluentDateTimeOffsetRangeExtensionsTestData.NotOverlappingNonNullable))]
    public void NotOverlappingNonNullable_BehavesAsExpected(FluentCase<(DateTimeOffsetRange value, DateTimeOffsetRange other)> tc)
    {
        var result = new NotOverlappingNonNullableValidator(tc.Value.other).Validate(new NonNullableModel { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateTimeOffsetRangeExtensionsTestData.ContainsNonNullable.Cases), MemberType = typeof(FluentDateTimeOffsetRangeExtensionsTestData.ContainsNonNullable))]
    public void ContainsNonNullable_BehavesAsExpected(FluentCase<(DateTimeOffsetRange value, DateTimeOffset item)> tc)
    {
        var result = new ContainsNonNullableValidator(tc.Value.item).Validate(new NonNullableModel { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateTimeOffsetRangeExtensionsTestData.NotContainsNonNullable.Cases), MemberType = typeof(FluentDateTimeOffsetRangeExtensionsTestData.NotContainsNonNullable))]
    public void NotContainsNonNullable_BehavesAsExpected(FluentCase<(DateTimeOffsetRange value, DateTimeOffset item)> tc)
    {
        var result = new NotContainsNonNullableValidator(tc.Value.item).Validate(new NonNullableModel { Value = tc.Value.value });
        AssertResult(tc, result);
    }
}
