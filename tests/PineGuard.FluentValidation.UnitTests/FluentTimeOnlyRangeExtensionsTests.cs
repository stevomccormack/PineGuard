using FluentValidation;
using PineGuard.Common;
using PineGuard.Testing.UnitTests.FluentValidation;
using Xunit.Abstractions;

namespace PineGuard.FluentValidation.UnitTests;

public sealed class FluentTimeOnlyRangeExtensionsTests(ITestOutputHelper output) : BaseFluentUnitTest(output)
{
    private sealed record Model { public TimeOnlyRange? Value { get; init; } }
    private sealed record NonNullableModel { public TimeOnlyRange Value { get; init; } }

    private sealed class ChronologicalValidator : AbstractValidator<Model>
    {
        public ChronologicalValidator() => RuleFor(x => x.Value).Chronological();
    }

    private sealed class OverlappingValidator : AbstractValidator<Model>
    {
        public OverlappingValidator(TimeOnlyRange range2) => RuleFor(x => x.Value).Overlapping(range2);
    }

    private sealed class NotOverlappingValidator : AbstractValidator<Model>
    {
        public NotOverlappingValidator(TimeOnlyRange range2) => RuleFor(x => x.Value).NotOverlapping(range2);
    }

    private sealed class ContainsValidator : AbstractValidator<Model>
    {
        public ContainsValidator(TimeOnly item) => RuleFor(x => x.Value).Contains(item);
    }

    private sealed class NotContainsValidator : AbstractValidator<Model>
    {
        public NotContainsValidator(TimeOnly item) => RuleFor(x => x.Value).NotContains(item);
    }

    private sealed class ChronologicalNonNullableValidator : AbstractValidator<NonNullableModel> { public ChronologicalNonNullableValidator() => RuleFor(x => x.Value).Chronological(); }
    private sealed class OverlappingNonNullableValidator : AbstractValidator<NonNullableModel> { public OverlappingNonNullableValidator(TimeOnlyRange range2) => RuleFor(x => x.Value).Overlapping(range2); }
    private sealed class NotOverlappingNonNullableValidator : AbstractValidator<NonNullableModel> { public NotOverlappingNonNullableValidator(TimeOnlyRange range2) => RuleFor(x => x.Value).NotOverlapping(range2); }
    private sealed class ContainsNonNullableValidator : AbstractValidator<NonNullableModel> { public ContainsNonNullableValidator(TimeOnly item) => RuleFor(x => x.Value).Contains(item); }
    private sealed class NotContainsNonNullableValidator : AbstractValidator<NonNullableModel> { public NotContainsNonNullableValidator(TimeOnly item) => RuleFor(x => x.Value).NotContains(item); }

    [Theory]
    [MemberData(nameof(FluentTimeOnlyRangeExtensionsTestData.Chronological.Cases), MemberType = typeof(FluentTimeOnlyRangeExtensionsTestData.Chronological))]
    public void Chronological_BehavesAsExpected(FluentCase<TimeOnlyRange?> tc)
    {
        var result = new ChronologicalValidator().Validate(new Model { Value = tc.Value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentTimeOnlyRangeExtensionsTestData.Overlapping.Cases), MemberType = typeof(FluentTimeOnlyRangeExtensionsTestData.Overlapping))]
    public void Overlapping_BehavesAsExpected(FluentCase<(TimeOnlyRange? range1, TimeOnlyRange range2)> tc)
    {
        var result = new OverlappingValidator(tc.Value.range2).Validate(new Model { Value = tc.Value.range1 });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentTimeOnlyRangeExtensionsTestData.NotOverlapping.Cases), MemberType = typeof(FluentTimeOnlyRangeExtensionsTestData.NotOverlapping))]
    public void NotOverlapping_BehavesAsExpected(FluentCase<(TimeOnlyRange? range1, TimeOnlyRange range2)> tc)
    {
        var result = new NotOverlappingValidator(tc.Value.range2).Validate(new Model { Value = tc.Value.range1 });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentTimeOnlyRangeExtensionsTestData.Contains.Cases), MemberType = typeof(FluentTimeOnlyRangeExtensionsTestData.Contains))]
    public void Contains_BehavesAsExpected(FluentCase<(TimeOnlyRange? range, TimeOnly value)> tc)
    {
        var result = new ContainsValidator(tc.Value.value).Validate(new Model { Value = tc.Value.range });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentTimeOnlyRangeExtensionsTestData.NotContains.Cases), MemberType = typeof(FluentTimeOnlyRangeExtensionsTestData.NotContains))]
    public void NotContains_BehavesAsExpected(FluentCase<(TimeOnlyRange? range, TimeOnly value)> tc)
    {
        var result = new NotContainsValidator(tc.Value.value).Validate(new Model { Value = tc.Value.range });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentTimeOnlyRangeExtensionsTestData.ChronologicalNonNullable.Cases), MemberType = typeof(FluentTimeOnlyRangeExtensionsTestData.ChronologicalNonNullable))]
    public void ChronologicalNonNullable_BehavesAsExpected(FluentCase<TimeOnlyRange> tc)
    {
        var result = new ChronologicalNonNullableValidator().Validate(new NonNullableModel { Value = tc.Value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentTimeOnlyRangeExtensionsTestData.OverlappingNonNullable.Cases), MemberType = typeof(FluentTimeOnlyRangeExtensionsTestData.OverlappingNonNullable))]
    public void OverlappingNonNullable_BehavesAsExpected(FluentCase<(TimeOnlyRange range1, TimeOnlyRange range2)> tc)
    {
        var result = new OverlappingNonNullableValidator(tc.Value.range2).Validate(new NonNullableModel { Value = tc.Value.range1 });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentTimeOnlyRangeExtensionsTestData.NotOverlappingNonNullable.Cases), MemberType = typeof(FluentTimeOnlyRangeExtensionsTestData.NotOverlappingNonNullable))]
    public void NotOverlappingNonNullable_BehavesAsExpected(FluentCase<(TimeOnlyRange range1, TimeOnlyRange range2)> tc)
    {
        var result = new NotOverlappingNonNullableValidator(tc.Value.range2).Validate(new NonNullableModel { Value = tc.Value.range1 });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentTimeOnlyRangeExtensionsTestData.ContainsNonNullable.Cases), MemberType = typeof(FluentTimeOnlyRangeExtensionsTestData.ContainsNonNullable))]
    public void ContainsNonNullable_BehavesAsExpected(FluentCase<(TimeOnlyRange range, TimeOnly value)> tc)
    {
        var result = new ContainsNonNullableValidator(tc.Value.value).Validate(new NonNullableModel { Value = tc.Value.range });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentTimeOnlyRangeExtensionsTestData.NotContainsNonNullable.Cases), MemberType = typeof(FluentTimeOnlyRangeExtensionsTestData.NotContainsNonNullable))]
    public void NotContainsNonNullable_BehavesAsExpected(FluentCase<(TimeOnlyRange range, TimeOnly value)> tc)
    {
        var result = new NotContainsNonNullableValidator(tc.Value.value).Validate(new NonNullableModel { Value = tc.Value.range });
        AssertResult(tc, result);
    }
}
