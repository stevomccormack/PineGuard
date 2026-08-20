using FluentValidation;
using PineGuard.Testing.UnitTests.FluentValidation;
using Xunit.Abstractions;

namespace PineGuard.FluentValidation.UnitTests;

public sealed class FluentSqlDateTimeExtensionsTests(ITestOutputHelper output) : BaseFluentUnitTest(output)
{
    private sealed record DateOnlyModel { public DateOnly Value { get; init; } }
    private sealed record NullableDateOnlyModel { public DateOnly? Value { get; init; } }
    private sealed record DateTimeOffsetModel { public DateTimeOffset Value { get; init; } }
    private sealed record NullableDateTimeOffsetModel { public DateTimeOffset? Value { get; init; } }
    private sealed record NullableDateTimeModel { public DateTime? Value { get; init; } }

    private sealed class DateOnlyValidator : AbstractValidator<DateOnlyModel> { public DateOnlyValidator() => RuleFor(x => x.Value).InSqlDateRange(); }
    private sealed class NullableDateOnlyValidator : AbstractValidator<NullableDateOnlyModel> { public NullableDateOnlyValidator() => RuleFor(x => x.Value).InSqlDateRange(); }
    private sealed class DateTimeOffsetValidator : AbstractValidator<DateTimeOffsetModel> { public DateTimeOffsetValidator() => RuleFor(x => x.Value).InSqlDateTimeRange(); }
    private sealed class NullableDateTimeOffsetValidator : AbstractValidator<NullableDateTimeOffsetModel> { public NullableDateTimeOffsetValidator() => RuleFor(x => x.Value).InSqlDateTimeRange(); }
    private sealed class NullableDateTimeValidator : AbstractValidator<NullableDateTimeModel> { public NullableDateTimeValidator() => RuleFor(x => x.Value).InSqlDateTimeRange(); }

    [Theory]
    [MemberData(nameof(FluentSqlDateTimeExtensionsTestData.InSqlDateRange.Cases), MemberType = typeof(FluentSqlDateTimeExtensionsTestData.InSqlDateRange))]
    public void InSqlDateRange_BehavesAsExpected(FluentCase<DateOnly> tc)
    {
        var result = new DateOnlyValidator().Validate(new DateOnlyModel { Value = tc.Value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentSqlDateTimeExtensionsTestData.InSqlDateRange.NullableCases), MemberType = typeof(FluentSqlDateTimeExtensionsTestData.InSqlDateRange))]
    public void InSqlDateRange_NullableBehavesAsExpected(FluentCase<DateOnly?> tc)
    {
        var result = new NullableDateOnlyValidator().Validate(new NullableDateOnlyModel { Value = tc.Value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentSqlDateTimeExtensionsTestData.InSqlDateTimeRangeOffset.Cases), MemberType = typeof(FluentSqlDateTimeExtensionsTestData.InSqlDateTimeRangeOffset))]
    public void InSqlDateTimeRangeOffset_BehavesAsExpected(FluentCase<DateTimeOffset> tc)
    {
        var result = new DateTimeOffsetValidator().Validate(new DateTimeOffsetModel { Value = tc.Value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentSqlDateTimeExtensionsTestData.InSqlDateTimeRangeOffset.NullableCases), MemberType = typeof(FluentSqlDateTimeExtensionsTestData.InSqlDateTimeRangeOffset))]
    public void InSqlDateTimeRangeOffset_NullableBehavesAsExpected(FluentCase<DateTimeOffset?> tc)
    {
        var result = new NullableDateTimeOffsetValidator().Validate(new NullableDateTimeOffsetModel { Value = tc.Value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentSqlDateTimeExtensionsTestData.InSqlDateTimeRangeDateTime.Cases), MemberType = typeof(FluentSqlDateTimeExtensionsTestData.InSqlDateTimeRangeDateTime))]
    public void InSqlDateTimeRangeDateTime_BehavesAsExpected(FluentCase<DateTime?> tc)
    {
        var result = new NullableDateTimeValidator().Validate(new NullableDateTimeModel { Value = tc.Value });
        AssertResult(tc, result);
    }
}
