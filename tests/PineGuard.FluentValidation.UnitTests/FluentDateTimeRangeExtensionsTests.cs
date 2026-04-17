using FluentValidation;
using PineGuard.Common;
using PineGuard.Testing.UnitTests.FluentValidation;
using Xunit.Abstractions;

namespace PineGuard.FluentValidation.UnitTests;

public sealed class FluentDateTimeRangeExtensionsTests(ITestOutputHelper output) : BaseFluentUnitTest(output)
{
    private sealed record Model { public DateTimeRange? Value { get; init; } }
    private sealed record NonNullableModel { public DateTimeRange Value { get; init; } }

    private sealed class ChronologicalValidator : AbstractValidator<Model> { public ChronologicalValidator() => RuleFor(x => x.Value).Chronological(); }
    private sealed class OverlappingValidator : AbstractValidator<Model> { public OverlappingValidator(DateTimeRange other) => RuleFor(x => x.Value).Overlapping(other); }
    private sealed class NotOverlappingValidator : AbstractValidator<Model> { public NotOverlappingValidator(DateTimeRange other) => RuleFor(x => x.Value).NotOverlapping(other); }
    private sealed class ContainsValidator : AbstractValidator<Model> { public ContainsValidator(DateTime item) => RuleFor(x => x.Value).Contains(item); }
    private sealed class NotContainsValidator : AbstractValidator<Model> { public NotContainsValidator(DateTime item) => RuleFor(x => x.Value).NotContains(item); }

    private sealed class ChronologicalNonNullableValidator : AbstractValidator<NonNullableModel> { public ChronologicalNonNullableValidator() => RuleFor(x => x.Value).Chronological(); }
    private sealed class OverlappingNonNullableValidator : AbstractValidator<NonNullableModel> { public OverlappingNonNullableValidator(DateTimeRange other) => RuleFor(x => x.Value).Overlapping(other); }
    private sealed class NotOverlappingNonNullableValidator : AbstractValidator<NonNullableModel> { public NotOverlappingNonNullableValidator(DateTimeRange other) => RuleFor(x => x.Value).NotOverlapping(other); }
    private sealed class ContainsNonNullableValidator : AbstractValidator<NonNullableModel> { public ContainsNonNullableValidator(DateTime item) => RuleFor(x => x.Value).Contains(item); }
    private sealed class NotContainsNonNullableValidator : AbstractValidator<NonNullableModel> { public NotContainsNonNullableValidator(DateTime item) => RuleFor(x => x.Value).NotContains(item); }

    [Theory]
    [MemberData(nameof(FluentDateTimeRangeExtensionsTestData.Chronological.Cases), MemberType = typeof(FluentDateTimeRangeExtensionsTestData.Chronological))]
    public void Chronological_BehavesAsExpected(FluentCase<DateTimeRange?> tc)
    {
        var result = new ChronologicalValidator().Validate(new Model { Value = tc.Value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateTimeRangeExtensionsTestData.Overlapping.Cases), MemberType = typeof(FluentDateTimeRangeExtensionsTestData.Overlapping))]
    public void Overlapping_BehavesAsExpected(FluentCase<(DateTimeRange? value, DateTimeRange other)> tc)
    {
        var result = new OverlappingValidator(tc.Value.other).Validate(new Model { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateTimeRangeExtensionsTestData.NotOverlapping.Cases), MemberType = typeof(FluentDateTimeRangeExtensionsTestData.NotOverlapping))]
    public void NotOverlapping_BehavesAsExpected(FluentCase<(DateTimeRange? value, DateTimeRange other)> tc)
    {
        var result = new NotOverlappingValidator(tc.Value.other).Validate(new Model { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateTimeRangeExtensionsTestData.Contains.Cases), MemberType = typeof(FluentDateTimeRangeExtensionsTestData.Contains))]
    public void Contains_BehavesAsExpected(FluentCase<(DateTimeRange? value, DateTime item)> tc)
    {
        var result = new ContainsValidator(tc.Value.item).Validate(new Model { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateTimeRangeExtensionsTestData.NotContains.Cases), MemberType = typeof(FluentDateTimeRangeExtensionsTestData.NotContains))]
    public void NotContains_BehavesAsExpected(FluentCase<(DateTimeRange? value, DateTime item)> tc)
    {
        var result = new NotContainsValidator(tc.Value.item).Validate(new Model { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateTimeRangeExtensionsTestData.ChronologicalNonNullable.Cases), MemberType = typeof(FluentDateTimeRangeExtensionsTestData.ChronologicalNonNullable))]
    public void ChronologicalNonNullable_BehavesAsExpected(FluentCase<DateTimeRange> tc)
    {
        var result = new ChronologicalNonNullableValidator().Validate(new NonNullableModel { Value = tc.Value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateTimeRangeExtensionsTestData.OverlappingNonNullable.Cases), MemberType = typeof(FluentDateTimeRangeExtensionsTestData.OverlappingNonNullable))]
    public void OverlappingNonNullable_BehavesAsExpected(FluentCase<(DateTimeRange value, DateTimeRange other)> tc)
    {
        var result = new OverlappingNonNullableValidator(tc.Value.other).Validate(new NonNullableModel { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateTimeRangeExtensionsTestData.NotOverlappingNonNullable.Cases), MemberType = typeof(FluentDateTimeRangeExtensionsTestData.NotOverlappingNonNullable))]
    public void NotOverlappingNonNullable_BehavesAsExpected(FluentCase<(DateTimeRange value, DateTimeRange other)> tc)
    {
        var result = new NotOverlappingNonNullableValidator(tc.Value.other).Validate(new NonNullableModel { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateTimeRangeExtensionsTestData.ContainsNonNullable.Cases), MemberType = typeof(FluentDateTimeRangeExtensionsTestData.ContainsNonNullable))]
    public void ContainsNonNullable_BehavesAsExpected(FluentCase<(DateTimeRange value, DateTime item)> tc)
    {
        var result = new ContainsNonNullableValidator(tc.Value.item).Validate(new NonNullableModel { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateTimeRangeExtensionsTestData.NotContainsNonNullable.Cases), MemberType = typeof(FluentDateTimeRangeExtensionsTestData.NotContainsNonNullable))]
    public void NotContainsNonNullable_BehavesAsExpected(FluentCase<(DateTimeRange value, DateTime item)> tc)
    {
        var result = new NotContainsNonNullableValidator(tc.Value.item).Validate(new NonNullableModel { Value = tc.Value.value });
        AssertResult(tc, result);
    }
}
