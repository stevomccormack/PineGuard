using FluentValidation;
using PineGuard.Common;
using PineGuard.Testing.UnitTests.FluentValidation;
using Xunit.Abstractions;

namespace PineGuard.FluentValidation.UnitTests;

public sealed class FluentStringDateOnlyExtensionsTests(ITestOutputHelper output) : BaseFluentUnitTest(output)
{
    private sealed record Model { public string? Value { get; init; } }

    private sealed class PastDateOnlyValidator : AbstractValidator<Model>
    {
        public PastDateOnlyValidator() => RuleFor(x => x.Value).PastDateOnly();
    }

    private sealed class PastOrPresentDateOnlyValidator : AbstractValidator<Model>
    {
        public PastOrPresentDateOnlyValidator() => RuleFor(x => x.Value).PastOrPresentDateOnly();
    }

    private sealed class FutureDateOnlyValidator : AbstractValidator<Model>
    {
        public FutureDateOnlyValidator() => RuleFor(x => x.Value).FutureDateOnly();
    }

    private sealed class FutureOrPresentDateOnlyValidator : AbstractValidator<Model>
    {
        public FutureOrPresentDateOnlyValidator() => RuleFor(x => x.Value).FutureOrPresentDateOnly();
    }

    private sealed class BetweenDateOnlyValidator : AbstractValidator<Model>
    {
        public BetweenDateOnlyValidator(DateOnly min, DateOnly max, Inclusion inclusion) =>
            RuleFor(x => x.Value).BetweenDateOnly(min, max, inclusion);
    }

    private sealed class NotBetweenDateOnlyValidator : AbstractValidator<Model>
    {
        public NotBetweenDateOnlyValidator(DateOnly min, DateOnly max, Inclusion inclusion) =>
            RuleFor(x => x.Value).NotBetweenDateOnly(min, max, inclusion);
    }

    private sealed class WithinDaysDateOnlyValidator : AbstractValidator<Model>
    {
        public WithinDaysDateOnlyValidator(DateOnly? reference, int days) =>
            RuleFor(x => x.Value).WithinDaysDateOnly(reference, days);
    }

    private sealed class NotWithinDaysDateOnlyValidator : AbstractValidator<Model>
    {
        public NotWithinDaysDateOnlyValidator(DateOnly? reference, int days) =>
            RuleFor(x => x.Value).NotWithinDaysDateOnly(reference, days);
    }

    private sealed class WithinCalendarMonthsDateOnlyValidator : AbstractValidator<Model>
    {
        public WithinCalendarMonthsDateOnlyValidator(DateOnly? reference, int months) =>
            RuleFor(x => x.Value).WithinCalendarMonthsDateOnly(reference, months);
    }

    private sealed class NotWithinCalendarMonthsDateOnlyValidator : AbstractValidator<Model>
    {
        public NotWithinCalendarMonthsDateOnlyValidator(DateOnly? reference, int months) =>
            RuleFor(x => x.Value).NotWithinCalendarMonthsDateOnly(reference, months);
    }

    private sealed class BeforeDateOnlyValidator : AbstractValidator<Model>
    {
        public BeforeDateOnlyValidator(DateOnly other) =>
            RuleFor(x => x.Value).BeforeDateOnly(other);
    }

    private sealed class NotBeforeDateOnlyValidator : AbstractValidator<Model>
    {
        public NotBeforeDateOnlyValidator(DateOnly other) =>
            RuleFor(x => x.Value).NotBeforeDateOnly(other);
    }

    private sealed class OnOrBeforeDateOnlyValidator : AbstractValidator<Model>
    {
        public OnOrBeforeDateOnlyValidator(DateOnly other) =>
            RuleFor(x => x.Value).OnOrBeforeDateOnly(other);
    }

    private sealed class NotOnOrBeforeDateOnlyValidator : AbstractValidator<Model>
    {
        public NotOnOrBeforeDateOnlyValidator(DateOnly other) =>
            RuleFor(x => x.Value).NotOnOrBeforeDateOnly(other);
    }

    private sealed class AfterDateOnlyValidator : AbstractValidator<Model>
    {
        public AfterDateOnlyValidator(DateOnly other) =>
            RuleFor(x => x.Value).AfterDateOnly(other);
    }

    private sealed class NotAfterDateOnlyValidator : AbstractValidator<Model>
    {
        public NotAfterDateOnlyValidator(DateOnly other) =>
            RuleFor(x => x.Value).NotAfterDateOnly(other);
    }

    private sealed class OnOrAfterDateOnlyValidator : AbstractValidator<Model>
    {
        public OnOrAfterDateOnlyValidator(DateOnly other) =>
            RuleFor(x => x.Value).OnOrAfterDateOnly(other);
    }

    private sealed class NotOnOrAfterDateOnlyValidator : AbstractValidator<Model>
    {
        public NotOnOrAfterDateOnlyValidator(DateOnly other) =>
            RuleFor(x => x.Value).NotOnOrAfterDateOnly(other);
    }

    private sealed class SameDateOnlyValidator : AbstractValidator<Model>
    {
        public SameDateOnlyValidator(DateOnly other) =>
            RuleFor(x => x.Value).SameDateOnly(other);
    }

    private sealed class NotSameDateOnlyValidator : AbstractValidator<Model>
    {
        public NotSameDateOnlyValidator(DateOnly other) =>
            RuleFor(x => x.Value).NotSameDateOnly(other);
    }

    private sealed class ChronologicalDateOnlyValidator : AbstractValidator<Model>
    {
        public ChronologicalDateOnlyValidator(string end, Inclusion inclusion) =>
            RuleFor(x => x.Value).ChronologicalDateOnly(end, inclusion);
    }

    private sealed class NotChronologicalDateOnlyValidator : AbstractValidator<Model>
    {
        public NotChronologicalDateOnlyValidator(string end, Inclusion inclusion) =>
            RuleFor(x => x.Value).NotChronologicalDateOnly(end, inclusion);
    }

    private sealed class OverlappingDateOnlyValidator : AbstractValidator<Model>
    {
        public OverlappingDateOnlyValidator(string end1, string start2, string end2, Inclusion inclusion) =>
            RuleFor(x => x.Value).OverlappingDateOnly(end1, start2, end2, inclusion);
    }

    private sealed class NotOverlappingDateOnlyValidator : AbstractValidator<Model>
    {
        public NotOverlappingDateOnlyValidator(string end1, string start2, string end2, Inclusion inclusion) =>
            RuleFor(x => x.Value).NotOverlappingDateOnly(end1, start2, end2, inclusion);
    }

    // FluentStringDateOnlyExtensions.PastDateOnly
    [Theory]
    [MemberData(nameof(FluentStringDateOnlyExtensionsTestData.InPast.Cases), MemberType = typeof(FluentStringDateOnlyExtensionsTestData.InPast))]
    public void InPast_BehavesAsExpected(FluentCase<string?> tc)
    {
        var result = new PastDateOnlyValidator().Validate(new Model { Value = tc.Value });
        AssertResult(tc, result);
    }

    // FluentStringDateOnlyExtensions.PastOrPresentDateOnly
    [Theory]
    [MemberData(nameof(FluentStringDateOnlyExtensionsTestData.InPastOrPresent.Cases), MemberType = typeof(FluentStringDateOnlyExtensionsTestData.InPastOrPresent))]
    public void InPastOrPresent_BehavesAsExpected(FluentCase<string?> tc)
    {
        var result = new PastOrPresentDateOnlyValidator().Validate(new Model { Value = tc.Value });
        AssertResult(tc, result);
    }

    // FluentStringDateOnlyExtensions.FutureDateOnly
    [Theory]
    [MemberData(nameof(FluentStringDateOnlyExtensionsTestData.InFuture.Cases), MemberType = typeof(FluentStringDateOnlyExtensionsTestData.InFuture))]
    public void InFuture_BehavesAsExpected(FluentCase<string?> tc)
    {
        var result = new FutureDateOnlyValidator().Validate(new Model { Value = tc.Value });
        AssertResult(tc, result);
    }

    // FluentStringDateOnlyExtensions.FutureOrPresentDateOnly
    [Theory]
    [MemberData(nameof(FluentStringDateOnlyExtensionsTestData.InFutureOrPresent.Cases), MemberType = typeof(FluentStringDateOnlyExtensionsTestData.InFutureOrPresent))]
    public void InFutureOrPresent_BehavesAsExpected(FluentCase<string?> tc)
    {
        var result = new FutureOrPresentDateOnlyValidator().Validate(new Model { Value = tc.Value });
        AssertResult(tc, result);
    }

    // FluentStringDateOnlyExtensions.BetweenDateOnly
    [Theory]
    [MemberData(nameof(FluentStringDateOnlyExtensionsTestData.IsBetween.Cases), MemberType = typeof(FluentStringDateOnlyExtensionsTestData.IsBetween))]
    public void IsBetween_BehavesAsExpected(FluentCase<(string? value, DateOnly min, DateOnly max, Inclusion inclusion)> tc)
    {
        var result = new BetweenDateOnlyValidator(tc.Value.min, tc.Value.max, tc.Value.inclusion)
            .Validate(new Model { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    // FluentStringDateOnlyExtensions.NotBetweenDateOnly
    [Theory]
    [MemberData(nameof(FluentStringDateOnlyExtensionsTestData.IsNotBetween.Cases), MemberType = typeof(FluentStringDateOnlyExtensionsTestData.IsNotBetween))]
    public void IsNotBetween_BehavesAsExpected(FluentCase<(string? value, DateOnly min, DateOnly max, Inclusion inclusion)> tc)
    {
        var result = new NotBetweenDateOnlyValidator(tc.Value.min, tc.Value.max, tc.Value.inclusion)
            .Validate(new Model { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    // FluentStringDateOnlyExtensions.WithinDaysDateOnly
    [Theory]
    [MemberData(nameof(FluentStringDateOnlyExtensionsTestData.IsWithinDays.Cases), MemberType = typeof(FluentStringDateOnlyExtensionsTestData.IsWithinDays))]
    public void IsWithinDays_BehavesAsExpected(FluentCase<(string? value, DateOnly? reference, int days)> tc)
    {
        var result = new WithinDaysDateOnlyValidator(tc.Value.reference, tc.Value.days)
            .Validate(new Model { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    // FluentStringDateOnlyExtensions.NotWithinDaysDateOnly
    [Theory]
    [MemberData(nameof(FluentStringDateOnlyExtensionsTestData.IsNotWithinDays.Cases), MemberType = typeof(FluentStringDateOnlyExtensionsTestData.IsNotWithinDays))]
    public void IsNotWithinDays_BehavesAsExpected(FluentCase<(string? value, DateOnly? reference, int days)> tc)
    {
        var result = new NotWithinDaysDateOnlyValidator(tc.Value.reference, tc.Value.days)
            .Validate(new Model { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    // FluentStringDateOnlyExtensions.WithinCalendarMonthsDateOnly
    [Theory]
    [MemberData(nameof(FluentStringDateOnlyExtensionsTestData.IsWithinCalendarMonths.Cases), MemberType = typeof(FluentStringDateOnlyExtensionsTestData.IsWithinCalendarMonths))]
    public void IsWithinCalendarMonths_BehavesAsExpected(FluentCase<(string? value, DateOnly? reference, int months)> tc)
    {
        var result = new WithinCalendarMonthsDateOnlyValidator(tc.Value.reference, tc.Value.months)
            .Validate(new Model { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    // FluentStringDateOnlyExtensions.NotWithinCalendarMonthsDateOnly
    [Theory]
    [MemberData(nameof(FluentStringDateOnlyExtensionsTestData.IsNotWithinCalendarMonths.Cases), MemberType = typeof(FluentStringDateOnlyExtensionsTestData.IsNotWithinCalendarMonths))]
    public void IsNotWithinCalendarMonths_BehavesAsExpected(FluentCase<(string? value, DateOnly? reference, int months)> tc)
    {
        var result = new NotWithinCalendarMonthsDateOnlyValidator(tc.Value.reference, tc.Value.months)
            .Validate(new Model { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    // FluentStringDateOnlyExtensions.BeforeDateOnly
    [Theory]
    [MemberData(nameof(FluentStringDateOnlyExtensionsTestData.IsBefore.Cases), MemberType = typeof(FluentStringDateOnlyExtensionsTestData.IsBefore))]
    public void IsBefore_BehavesAsExpected(FluentCase<(string? value, DateOnly other)> tc)
    {
        var result = new BeforeDateOnlyValidator(tc.Value.other)
            .Validate(new Model { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    // FluentStringDateOnlyExtensions.NotBeforeDateOnly
    [Theory]
    [MemberData(nameof(FluentStringDateOnlyExtensionsTestData.IsNotBefore.Cases), MemberType = typeof(FluentStringDateOnlyExtensionsTestData.IsNotBefore))]
    public void IsNotBefore_BehavesAsExpected(FluentCase<(string? value, DateOnly other)> tc)
    {
        var result = new NotBeforeDateOnlyValidator(tc.Value.other)
            .Validate(new Model { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    // FluentStringDateOnlyExtensions.OnOrBeforeDateOnly
    [Theory]
    [MemberData(nameof(FluentStringDateOnlyExtensionsTestData.IsOnOrBefore.Cases), MemberType = typeof(FluentStringDateOnlyExtensionsTestData.IsOnOrBefore))]
    public void IsOnOrBefore_BehavesAsExpected(FluentCase<(string? value, DateOnly other)> tc)
    {
        var result = new OnOrBeforeDateOnlyValidator(tc.Value.other)
            .Validate(new Model { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    // FluentStringDateOnlyExtensions.NotOnOrBeforeDateOnly
    [Theory]
    [MemberData(nameof(FluentStringDateOnlyExtensionsTestData.IsNotOnOrBefore.Cases), MemberType = typeof(FluentStringDateOnlyExtensionsTestData.IsNotOnOrBefore))]
    public void IsNotOnOrBefore_BehavesAsExpected(FluentCase<(string? value, DateOnly other)> tc)
    {
        var result = new NotOnOrBeforeDateOnlyValidator(tc.Value.other)
            .Validate(new Model { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    // FluentStringDateOnlyExtensions.AfterDateOnly
    [Theory]
    [MemberData(nameof(FluentStringDateOnlyExtensionsTestData.IsAfter.Cases), MemberType = typeof(FluentStringDateOnlyExtensionsTestData.IsAfter))]
    public void IsAfter_BehavesAsExpected(FluentCase<(string? value, DateOnly other)> tc)
    {
        var result = new AfterDateOnlyValidator(tc.Value.other)
            .Validate(new Model { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    // FluentStringDateOnlyExtensions.NotAfterDateOnly
    [Theory]
    [MemberData(nameof(FluentStringDateOnlyExtensionsTestData.IsNotAfter.Cases), MemberType = typeof(FluentStringDateOnlyExtensionsTestData.IsNotAfter))]
    public void IsNotAfter_BehavesAsExpected(FluentCase<(string? value, DateOnly other)> tc)
    {
        var result = new NotAfterDateOnlyValidator(tc.Value.other)
            .Validate(new Model { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    // FluentStringDateOnlyExtensions.OnOrAfterDateOnly
    [Theory]
    [MemberData(nameof(FluentStringDateOnlyExtensionsTestData.IsOnOrAfter.Cases), MemberType = typeof(FluentStringDateOnlyExtensionsTestData.IsOnOrAfter))]
    public void IsOnOrAfter_BehavesAsExpected(FluentCase<(string? value, DateOnly other)> tc)
    {
        var result = new OnOrAfterDateOnlyValidator(tc.Value.other)
            .Validate(new Model { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    // FluentStringDateOnlyExtensions.NotOnOrAfterDateOnly
    [Theory]
    [MemberData(nameof(FluentStringDateOnlyExtensionsTestData.IsNotOnOrAfter.Cases), MemberType = typeof(FluentStringDateOnlyExtensionsTestData.IsNotOnOrAfter))]
    public void IsNotOnOrAfter_BehavesAsExpected(FluentCase<(string? value, DateOnly other)> tc)
    {
        var result = new NotOnOrAfterDateOnlyValidator(tc.Value.other)
            .Validate(new Model { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    // FluentStringDateOnlyExtensions.SameDateOnly
    [Theory]
    [MemberData(nameof(FluentStringDateOnlyExtensionsTestData.IsSame.Cases), MemberType = typeof(FluentStringDateOnlyExtensionsTestData.IsSame))]
    public void IsSame_BehavesAsExpected(FluentCase<(string? value, DateOnly other)> tc)
    {
        var result = new SameDateOnlyValidator(tc.Value.other)
            .Validate(new Model { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    // FluentStringDateOnlyExtensions.NotSameDateOnly
    [Theory]
    [MemberData(nameof(FluentStringDateOnlyExtensionsTestData.IsNotSame.Cases), MemberType = typeof(FluentStringDateOnlyExtensionsTestData.IsNotSame))]
    public void IsNotSame_BehavesAsExpected(FluentCase<(string? value, DateOnly other)> tc)
    {
        var result = new NotSameDateOnlyValidator(tc.Value.other)
            .Validate(new Model { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    // FluentStringDateOnlyExtensions.ChronologicalDateOnly
    [Theory]
    [MemberData(nameof(FluentStringDateOnlyExtensionsTestData.IsChronological.Cases), MemberType = typeof(FluentStringDateOnlyExtensionsTestData.IsChronological))]
    public void IsChronological_BehavesAsExpected(FluentCase<(string? start, string? end, Inclusion inclusion)> tc)
    {
        var result = new ChronologicalDateOnlyValidator(tc.Value.end!, tc.Value.inclusion)
            .Validate(new Model { Value = tc.Value.start });
        AssertResult(tc, result);
    }

    // FluentStringDateOnlyExtensions.NotChronologicalDateOnly
    [Theory]
    [MemberData(nameof(FluentStringDateOnlyExtensionsTestData.IsNotChronological.Cases), MemberType = typeof(FluentStringDateOnlyExtensionsTestData.IsNotChronological))]
    public void IsNotChronological_BehavesAsExpected(FluentCase<(string? start, string? end, Inclusion inclusion)> tc)
    {
        var result = new NotChronologicalDateOnlyValidator(tc.Value.end!, tc.Value.inclusion)
            .Validate(new Model { Value = tc.Value.start });
        AssertResult(tc, result);
    }

    // FluentStringDateOnlyExtensions.OverlappingDateOnly
    [Theory]
    [MemberData(nameof(FluentStringDateOnlyExtensionsTestData.IsOverlapping.Cases), MemberType = typeof(FluentStringDateOnlyExtensionsTestData.IsOverlapping))]
    public void IsOverlapping_BehavesAsExpected(FluentCase<(string? start1, string? end1, string? start2, string? end2, Inclusion inclusion)> tc)
    {
        var result = new OverlappingDateOnlyValidator(tc.Value.end1!, tc.Value.start2!, tc.Value.end2!, tc.Value.inclusion)
            .Validate(new Model { Value = tc.Value.start1 });
        AssertResult(tc, result);
    }

    // FluentStringDateOnlyExtensions.NotOverlappingDateOnly
    [Theory]
    [MemberData(nameof(FluentStringDateOnlyExtensionsTestData.IsNotOverlapping.Cases), MemberType = typeof(FluentStringDateOnlyExtensionsTestData.IsNotOverlapping))]
    public void IsNotOverlapping_BehavesAsExpected(FluentCase<(string? start1, string? end1, string? start2, string? end2, Inclusion inclusion)> tc)
    {
        var result = new NotOverlappingDateOnlyValidator(tc.Value.end1!, tc.Value.start2!, tc.Value.end2!, tc.Value.inclusion)
            .Validate(new Model { Value = tc.Value.start1 });
        AssertResult(tc, result);
    }
}
