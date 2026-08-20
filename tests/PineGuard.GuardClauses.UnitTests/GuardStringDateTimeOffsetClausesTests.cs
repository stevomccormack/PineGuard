using PineGuard.Testing.UnitTests.GuardClauses;
using Xunit.Abstractions;
using TD = PineGuard.GuardClauses.UnitTests.GuardStringDateTimeOffsetClausesTestData;

namespace PineGuard.GuardClauses.UnitTests;

public sealed class GuardStringDateTimeOffsetClausesTests(ITestOutputHelper output) : BaseGuardUnitTest(output)
{
    [Theory]
    [MemberData(nameof(TD.FutureOrPresent.ValidCases), MemberType = typeof(TD.FutureOrPresent))]
    [MemberData(nameof(TD.FutureOrPresent.InvalidCases), MemberType = typeof(TD.FutureOrPresent))]
    public void FutureOrPresentDateTimeOffset_BehavesAsExpected(GuardCase<string?> tc)
    {
        var value = tc.Value;
        AssertResult(tc, () => Guard.Against.FutureOrPresentDateTimeOffset(value));
        AssertCustomMessage(tc, () => Guard.Against.FutureOrPresentDateTimeOffset(value, message: CustomMessage));
    }

    [Theory]
    [MemberData(nameof(TD.Future.ValidCases), MemberType = typeof(TD.Future))]
    [MemberData(nameof(TD.Future.InvalidCases), MemberType = typeof(TD.Future))]
    public void FutureDateTimeOffset_BehavesAsExpected(GuardCase<string?> tc)
    {
        var value = tc.Value;
        AssertResult(tc, () => Guard.Against.FutureDateTimeOffset(value));
        AssertCustomMessage(tc, () => Guard.Against.FutureDateTimeOffset(value, message: CustomMessage));
    }

    [Theory]
    [MemberData(nameof(TD.PastOrPresent.ValidCases), MemberType = typeof(TD.PastOrPresent))]
    [MemberData(nameof(TD.PastOrPresent.InvalidCases), MemberType = typeof(TD.PastOrPresent))]
    public void PastOrPresentDateTimeOffset_BehavesAsExpected(GuardCase<string?> tc)
    {
        var value = tc.Value;
        AssertResult(tc, () => Guard.Against.PastOrPresentDateTimeOffset(value));
        AssertCustomMessage(tc, () => Guard.Against.PastOrPresentDateTimeOffset(value, message: CustomMessage));
    }

    [Theory]
    [MemberData(nameof(TD.Past.ValidCases), MemberType = typeof(TD.Past))]
    [MemberData(nameof(TD.Past.InvalidCases), MemberType = typeof(TD.Past))]
    public void PastDateTimeOffset_BehavesAsExpected(GuardCase<string?> tc)
    {
        var value = tc.Value;
        AssertResult(tc, () => Guard.Against.PastDateTimeOffset(value));
        AssertCustomMessage(tc, () => Guard.Against.PastDateTimeOffset(value, message: CustomMessage));
    }

    [Theory]
    [MemberData(nameof(TD.NotBetween.ValidCases), MemberType = typeof(TD.NotBetween))]
    [MemberData(nameof(TD.NotBetween.InvalidCases), MemberType = typeof(TD.NotBetween))]
    public void NotBetweenDateTimeOffset_BehavesAsExpected(GuardCase<(string? value, DateTimeOffset min, DateTimeOffset max, Common.Inclusion inclusion)> tc)
    {
        var value = tc.Value.value;
        AssertResult(tc, () => Guard.Against.NotBetweenDateTimeOffset(value, tc.Value.min, tc.Value.max, tc.Value.inclusion));
        AssertCustomMessage(tc, () => Guard.Against.NotBetweenDateTimeOffset(value, tc.Value.min, tc.Value.max, tc.Value.inclusion, message: CustomMessage));
    }

    [Theory]
    [MemberData(nameof(TD.Between.ValidCases), MemberType = typeof(TD.Between))]
    [MemberData(nameof(TD.Between.InvalidCases), MemberType = typeof(TD.Between))]
    public void BetweenDateTimeOffset_BehavesAsExpected(GuardCase<(string? value, DateTimeOffset min, DateTimeOffset max, Common.Inclusion inclusion)> tc)
    {
        var value = tc.Value.value;
        AssertResult(tc, () => Guard.Against.BetweenDateTimeOffset(value, tc.Value.min, tc.Value.max, tc.Value.inclusion));
        AssertCustomMessage(tc, () => Guard.Against.BetweenDateTimeOffset(value, tc.Value.min, tc.Value.max, tc.Value.inclusion, message: CustomMessage));
    }

    [Theory]
    [MemberData(nameof(TD.NotWithin.ValidCases), MemberType = typeof(TD.NotWithin))]
    [MemberData(nameof(TD.NotWithin.InvalidCases), MemberType = typeof(TD.NotWithin))]
    public void NotWithinDateTimeOffset_BehavesAsExpected(GuardCase<(string? value, DateTimeOffset? reference, TimeSpan window)> tc)
    {
        var value = tc.Value.value;
        AssertResult(tc, () => Guard.Against.NotWithinDateTimeOffset(value, tc.Value.reference, tc.Value.window));
        AssertCustomMessage(tc, () => Guard.Against.NotWithinDateTimeOffset(value, tc.Value.reference, tc.Value.window, message: CustomMessage));
    }

    [Theory]
    [MemberData(nameof(TD.Within.ValidCases), MemberType = typeof(TD.Within))]
    [MemberData(nameof(TD.Within.InvalidCases), MemberType = typeof(TD.Within))]
    public void WithinDateTimeOffset_BehavesAsExpected(GuardCase<(string? value, DateTimeOffset? reference, TimeSpan window)> tc)
    {
        var value = tc.Value.value;
        AssertResult(tc, () => Guard.Against.WithinDateTimeOffset(value, tc.Value.reference, tc.Value.window));
        AssertCustomMessage(tc, () => Guard.Against.WithinDateTimeOffset(value, tc.Value.reference, tc.Value.window, message: CustomMessage));
    }

    [Theory]
    [MemberData(nameof(TD.NotWithinCalendarMonths.ValidCases), MemberType = typeof(TD.NotWithinCalendarMonths))]
    [MemberData(nameof(TD.NotWithinCalendarMonths.InvalidCases), MemberType = typeof(TD.NotWithinCalendarMonths))]
    public void NotWithinCalendarMonthsDateTimeOffset_BehavesAsExpected(GuardCase<(string? value, DateTimeOffset? reference, int months)> tc)
    {
        var value = tc.Value.value;
        AssertResult(tc, () => Guard.Against.NotWithinCalendarMonthsDateTimeOffset(value, tc.Value.reference, tc.Value.months));
        AssertCustomMessage(tc, () => Guard.Against.NotWithinCalendarMonthsDateTimeOffset(value, tc.Value.reference, tc.Value.months, message: CustomMessage));
    }

    [Theory]
    [MemberData(nameof(TD.WithinCalendarMonths.ValidCases), MemberType = typeof(TD.WithinCalendarMonths))]
    [MemberData(nameof(TD.WithinCalendarMonths.InvalidCases), MemberType = typeof(TD.WithinCalendarMonths))]
    public void WithinCalendarMonthsDateTimeOffset_BehavesAsExpected(GuardCase<(string? value, DateTimeOffset? reference, int months)> tc)
    {
        var value = tc.Value.value;
        AssertResult(tc, () => Guard.Against.WithinCalendarMonthsDateTimeOffset(value, tc.Value.reference, tc.Value.months));
        AssertCustomMessage(tc, () => Guard.Against.WithinCalendarMonthsDateTimeOffset(value, tc.Value.reference, tc.Value.months, message: CustomMessage));
    }
}
