using FluentValidation;
using PineGuard.Common;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests.FluentValidation;
using Xunit.Abstractions;

namespace PineGuard.FluentValidation.UnitTests;

public sealed class FluentStringDateTimeOffsetExtensionsTests(ITestOutputHelper output) : BaseFluentUnitTest(output)
{
    private sealed record Model { public string? Value { get; init; } }

    // FluentStringDateTimeOffsetExtensions.InPast
    private sealed class InPastValidator : AbstractValidator<Model>
    {
        public InPastValidator() => RuleFor(x => x.Value).PastDateTimeOffset();
    }

    [Theory]
    [MemberData(nameof(FluentStringDateTimeOffsetExtensionsTestData.InPast.Cases), MemberType = typeof(FluentStringDateTimeOffsetExtensionsTestData.InPast))]
    public void InPast_BehavesAsExpected(FluentCase<string?> tc)
    {
        // Act
        var result = new InPastValidator().Validate(new Model { Value = tc.Value });

        // Assert
        AssertResult(tc, result);
    }

    // FluentStringDateTimeOffsetExtensions.PastOrPresentDateTimeOffset
    private sealed class InPastOrPresentValidator : AbstractValidator<Model>
    {
        public InPastOrPresentValidator() => RuleFor(x => x.Value).PastOrPresentDateTimeOffset();
    }

    [Theory]
    [MemberData(nameof(FluentStringDateTimeOffsetExtensionsTestData.InPastOrPresent.Cases), MemberType = typeof(FluentStringDateTimeOffsetExtensionsTestData.InPastOrPresent))]
    public void InPastOrPresent_BehavesAsExpected(FluentCase<string?> tc)
    {
        // Act
        var result = new InPastOrPresentValidator().Validate(new Model { Value = tc.Value });

        // Assert
        AssertResult(tc, result);
    }

    // FluentStringDateTimeOffsetExtensions.InFuture
    private sealed class InFutureValidator : AbstractValidator<Model>
    {
        public InFutureValidator() => RuleFor(x => x.Value).FutureDateTimeOffset();
    }

    [Theory]
    [MemberData(nameof(FluentStringDateTimeOffsetExtensionsTestData.InFuture.Cases), MemberType = typeof(FluentStringDateTimeOffsetExtensionsTestData.InFuture))]
    public void InFuture_BehavesAsExpected(FluentCase<string?> tc)
    {
        // Act
        var result = new InFutureValidator().Validate(new Model { Value = tc.Value });

        // Assert
        AssertResult(tc, result);
    }

    // FluentStringDateTimeOffsetExtensions.FutureOrPresentDateTimeOffset
    private sealed class InFutureOrPresentValidator : AbstractValidator<Model>
    {
        public InFutureOrPresentValidator() => RuleFor(x => x.Value).FutureOrPresentDateTimeOffset();
    }

    [Theory]
    [MemberData(nameof(FluentStringDateTimeOffsetExtensionsTestData.InFutureOrPresent.Cases), MemberType = typeof(FluentStringDateTimeOffsetExtensionsTestData.InFutureOrPresent))]
    public void InFutureOrPresent_BehavesAsExpected(FluentCase<string?> tc)
    {
        // Act
        var result = new InFutureOrPresentValidator().Validate(new Model { Value = tc.Value });

        // Assert
        AssertResult(tc, result);
    }

    // FluentStringDateTimeOffsetExtensions.PastDateTimeOffset
    private sealed class InPastPinnedClockValidator : AbstractValidator<Model>
    {
        public InPastPinnedClockValidator(TimeProvider timeProvider) => RuleFor(x => x.Value).PastDateTimeOffset(timeProvider);
    }

    [Theory]
    [MemberData(nameof(FluentStringDateTimeOffsetExtensionsTestData.InPastPinnedClock.Cases), MemberType = typeof(FluentStringDateTimeOffsetExtensionsTestData.InPastPinnedClock))]
    public void InPast_WithPinnedClock_BehavesAsExpected(FluentCase<string?> tc)
    {
        // Act
        var result = new InPastPinnedClockValidator(FixedTimeProvider.Default).Validate(new Model { Value = tc.Value });

        // Assert
        AssertResult(tc, result);
    }

    // FluentStringDateTimeOffsetExtensions.PastOrPresentDateTimeOffset
    private sealed class InPastOrPresentPinnedClockValidator : AbstractValidator<Model>
    {
        public InPastOrPresentPinnedClockValidator(TimeProvider timeProvider) => RuleFor(x => x.Value).PastOrPresentDateTimeOffset(timeProvider);
    }

    [Theory]
    [MemberData(nameof(FluentStringDateTimeOffsetExtensionsTestData.InPastOrPresentPinnedClock.Cases), MemberType = typeof(FluentStringDateTimeOffsetExtensionsTestData.InPastOrPresentPinnedClock))]
    public void InPastOrPresent_WithPinnedClock_BehavesAsExpected(FluentCase<string?> tc)
    {
        // Act
        var result = new InPastOrPresentPinnedClockValidator(FixedTimeProvider.Default).Validate(new Model { Value = tc.Value });

        // Assert
        AssertResult(tc, result);
    }

    // FluentStringDateTimeOffsetExtensions.FutureDateTimeOffset
    private sealed class InFuturePinnedClockValidator : AbstractValidator<Model>
    {
        public InFuturePinnedClockValidator(TimeProvider timeProvider) => RuleFor(x => x.Value).FutureDateTimeOffset(timeProvider);
    }

    [Theory]
    [MemberData(nameof(FluentStringDateTimeOffsetExtensionsTestData.InFuturePinnedClock.Cases), MemberType = typeof(FluentStringDateTimeOffsetExtensionsTestData.InFuturePinnedClock))]
    public void InFuture_WithPinnedClock_BehavesAsExpected(FluentCase<string?> tc)
    {
        // Act
        var result = new InFuturePinnedClockValidator(FixedTimeProvider.Default).Validate(new Model { Value = tc.Value });

        // Assert
        AssertResult(tc, result);
    }

    // FluentStringDateTimeOffsetExtensions.FutureOrPresentDateTimeOffset
    private sealed class InFutureOrPresentPinnedClockValidator : AbstractValidator<Model>
    {
        public InFutureOrPresentPinnedClockValidator(TimeProvider timeProvider) => RuleFor(x => x.Value).FutureOrPresentDateTimeOffset(timeProvider);
    }

    [Theory]
    [MemberData(nameof(FluentStringDateTimeOffsetExtensionsTestData.InFutureOrPresentPinnedClock.Cases), MemberType = typeof(FluentStringDateTimeOffsetExtensionsTestData.InFutureOrPresentPinnedClock))]
    public void InFutureOrPresent_WithPinnedClock_BehavesAsExpected(FluentCase<string?> tc)
    {
        // Act
        var result = new InFutureOrPresentPinnedClockValidator(FixedTimeProvider.Default).Validate(new Model { Value = tc.Value });

        // Assert
        AssertResult(tc, result);
    }

    // FluentStringDateTimeOffsetExtensions.IsBetween
    private sealed class IsBetweenValidator : AbstractValidator<Model>
    {
        public IsBetweenValidator(DateTimeOffset min, DateTimeOffset max, Inclusion inclusion) =>
            RuleFor(x => x.Value).BetweenDateTimeOffset(min, max, inclusion);
    }

    [Theory]
    [MemberData(nameof(FluentStringDateTimeOffsetExtensionsTestData.IsBetween.Cases), MemberType = typeof(FluentStringDateTimeOffsetExtensionsTestData.IsBetween))]
    public void IsBetween_BehavesAsExpected(FluentCase<(string? value, DateTimeOffset min, DateTimeOffset max, Inclusion inclusion)> tc)
    {
        // Arrange
        var (value, min, max, inclusion) = tc.Value;

        // Act
        var result = new IsBetweenValidator(min, max, inclusion).Validate(new Model { Value = value });

        // Assert
        AssertResult(tc, result);
    }

    // FluentStringDateTimeOffsetExtensions.NotBetweenDateTimeOffset
    private sealed class IsNotBetweenValidator : AbstractValidator<Model>
    {
        public IsNotBetweenValidator(DateTimeOffset min, DateTimeOffset max, Inclusion inclusion) =>
            RuleFor(x => x.Value).NotBetweenDateTimeOffset(min, max, inclusion);
    }

    [Theory]
    [MemberData(nameof(FluentStringDateTimeOffsetExtensionsTestData.IsNotBetween.Cases), MemberType = typeof(FluentStringDateTimeOffsetExtensionsTestData.IsNotBetween))]
    public void IsNotBetween_BehavesAsExpected(FluentCase<(string? value, DateTimeOffset min, DateTimeOffset max, Inclusion inclusion)> tc)
    {
        // Arrange
        var (value, min, max, inclusion) = tc.Value;

        // Act
        var result = new IsNotBetweenValidator(min, max, inclusion).Validate(new Model { Value = value });

        // Assert
        AssertResult(tc, result);
    }

    // FluentStringDateTimeOffsetExtensions.WithinDateTimeOffset
    private sealed class IsWithinValidator : AbstractValidator<Model>
    {
        public IsWithinValidator(DateTimeOffset? reference, TimeSpan window) =>
            RuleFor(x => x.Value).WithinDateTimeOffset(reference, window);
    }

    [Theory]
    [MemberData(nameof(FluentStringDateTimeOffsetExtensionsTestData.IsWithin.Cases), MemberType = typeof(FluentStringDateTimeOffsetExtensionsTestData.IsWithin))]
    public void IsWithin_BehavesAsExpected(FluentCase<(string? value, DateTimeOffset? reference, TimeSpan window)> tc)
    {
        // Arrange
        var (value, reference, window) = tc.Value;

        // Act
        var result = new IsWithinValidator(reference, window).Validate(new Model { Value = value });

        // Assert
        AssertResult(tc, result);
    }

    // FluentStringDateTimeOffsetExtensions.NotWithinDateTimeOffset
    private sealed class IsNotWithinValidator : AbstractValidator<Model>
    {
        public IsNotWithinValidator(DateTimeOffset? reference, TimeSpan window) =>
            RuleFor(x => x.Value).NotWithinDateTimeOffset(reference, window);
    }

    [Theory]
    [MemberData(nameof(FluentStringDateTimeOffsetExtensionsTestData.IsNotWithin.Cases), MemberType = typeof(FluentStringDateTimeOffsetExtensionsTestData.IsNotWithin))]
    public void IsNotWithin_BehavesAsExpected(FluentCase<(string? value, DateTimeOffset? reference, TimeSpan window)> tc)
    {
        // Arrange
        var (value, reference, window) = tc.Value;

        // Act
        var result = new IsNotWithinValidator(reference, window).Validate(new Model { Value = value });

        // Assert
        AssertResult(tc, result);
    }

    // FluentStringDateTimeOffsetExtensions.WithinCalendarMonthsDateTimeOffset
    private sealed class IsWithinCalendarMonthsValidator : AbstractValidator<Model>
    {
        public IsWithinCalendarMonthsValidator(DateTimeOffset? reference, int months) =>
            RuleFor(x => x.Value).WithinCalendarMonthsDateTimeOffset(reference, months);
    }

    [Theory]
    [MemberData(nameof(FluentStringDateTimeOffsetExtensionsTestData.IsWithinCalendarMonths.Cases), MemberType = typeof(FluentStringDateTimeOffsetExtensionsTestData.IsWithinCalendarMonths))]
    public void IsWithinCalendarMonths_BehavesAsExpected(FluentCase<(string? value, DateTimeOffset? reference, int months)> tc)
    {
        // Arrange
        var (value, reference, months) = tc.Value;

        // Act
        var result = new IsWithinCalendarMonthsValidator(reference, months).Validate(new Model { Value = value });

        // Assert
        AssertResult(tc, result);
    }

    // FluentStringDateTimeOffsetExtensions.NotWithinCalendarMonthsDateTimeOffset
    private sealed class IsNotWithinCalendarMonthsValidator : AbstractValidator<Model>
    {
        public IsNotWithinCalendarMonthsValidator(DateTimeOffset? reference, int months) =>
            RuleFor(x => x.Value).NotWithinCalendarMonthsDateTimeOffset(reference, months);
    }

    [Theory]
    [MemberData(nameof(FluentStringDateTimeOffsetExtensionsTestData.IsNotWithinCalendarMonths.Cases), MemberType = typeof(FluentStringDateTimeOffsetExtensionsTestData.IsNotWithinCalendarMonths))]
    public void IsNotWithinCalendarMonths_BehavesAsExpected(FluentCase<(string? value, DateTimeOffset? reference, int months)> tc)
    {
        // Arrange
        var (value, reference, months) = tc.Value;

        // Act
        var result = new IsNotWithinCalendarMonthsValidator(reference, months).Validate(new Model { Value = value });

        // Assert
        AssertResult(tc, result);
    }
}
