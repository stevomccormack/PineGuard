using FluentValidation;
using PineGuard.Testing.UnitTests;

namespace PineGuard.FluentValidation.UnitTests;

public sealed class FluentDateExtensionsTests : BaseUnitTest
{
    private sealed record DateModel { public DateOnly Value { get; init; } }
    private sealed record TaskModel { public Task? Value { get; init; } }
    private sealed record GenericModel<T> { public T? Value { get; init; } }

    private sealed class InSqlDateRangeValidator : AbstractValidator<DateModel>
    {
        public InSqlDateRangeValidator() => RuleFor(x => x.Value).InSqlDateRange();
    }
    [Theory]
    [MemberData(nameof(FluentDateExtensionsTestData.InSqlDateRange.ValidCases), MemberType = typeof(FluentDateExtensionsTestData.InSqlDateRange))]
    [MemberData(nameof(FluentDateExtensionsTestData.InSqlDateRange.EdgeCases), MemberType = typeof(FluentDateExtensionsTestData.InSqlDateRange))]
    public void InSqlDateRange_BehavesAsExpected(FluentDateExtensionsTestData.InSqlDateRange.ValidCase testCase)
    {
        var result = new InSqlDateRangeValidator().Validate(new DateModel { Value = testCase.Value });
        Assert.Equal(testCase.Expected, result.IsValid);
        if (testCase is { Expected: false, ExpectedMessage: not null }) Assert.EndsWith(testCase.ExpectedMessage, result.Errors[0].ErrorMessage);
    }

    private sealed class CompletedValidator : AbstractValidator<TaskModel>
    {
        public CompletedValidator() => RuleFor(x => x.Value).Completed();
    }
    [Theory]
    [MemberData(nameof(FluentDateExtensionsTestData.Completed.ValidCases), MemberType = typeof(FluentDateExtensionsTestData.Completed))]
    [MemberData(nameof(FluentDateExtensionsTestData.Completed.EdgeCases), MemberType = typeof(FluentDateExtensionsTestData.Completed))]
    public void Completed_BehavesAsExpected(FluentDateExtensionsTestData.Completed.ValidCase testCase)
    {
        var result = new CompletedValidator().Validate(new TaskModel { Value = testCase.Value });
        Assert.Equal(testCase.Expected, result.IsValid);
        if (testCase is { Expected: false, ExpectedMessage: not null }) Assert.EndsWith(testCase.ExpectedMessage, result.Errors[0].ErrorMessage);
    }

    private sealed class DefaultValidator : AbstractValidator<GenericModel<int?>>
    {
        public DefaultValidator() => RuleFor(x => x.Value).Default();
    }
    [Theory]
    [MemberData(nameof(FluentDateExtensionsTestData.Default.ValidCases), MemberType = typeof(FluentDateExtensionsTestData.Default))]
    [MemberData(nameof(FluentDateExtensionsTestData.Default.EdgeCases), MemberType = typeof(FluentDateExtensionsTestData.Default))]
    public void Default_BehavesAsExpected(FluentDateExtensionsTestData.Default.ValidCase testCase)
    {
        var result = new DefaultValidator().Validate(new GenericModel<int?> { Value = testCase.Value });
        Assert.Equal(testCase.Expected, result.IsValid);
        if (testCase is { Expected: false, ExpectedMessage: not null }) Assert.EndsWith(testCase.ExpectedMessage, result.Errors[0].ErrorMessage);
    }

    private sealed class SatisfiesValidator : AbstractValidator<GenericModel<int?>>
    {
        public SatisfiesValidator() => RuleFor(x => x.Value).Satisfies(x => x > 0);
    }
    [Theory]
    [InlineData(1, true)]
    [InlineData(0, false)]
    public void Satisfies_BehavesAsExpected(int? value, bool expected)
    {
        var result = new SatisfiesValidator().Validate(new GenericModel<int?> { Value = value });
        Assert.Equal(expected, result.IsValid);
        if (!expected) Assert.EndsWith("Value must satisfy the predicate.", result.Errors[0].ErrorMessage);
    }
}
