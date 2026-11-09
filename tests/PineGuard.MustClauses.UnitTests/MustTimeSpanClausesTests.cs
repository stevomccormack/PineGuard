using PineGuard.Common;
using PineGuard.Testing.UnitTests.MustClauses;
using Xunit.Abstractions;

namespace PineGuard.MustClauses.UnitTests;

public sealed class MustTimeSpanClausesTests(ITestOutputHelper output) : BaseMustUnitTest(output)
{
    [Theory]
    [MemberData(nameof(MustTimeSpanClausesTestData.DurationBetween.ValidCases), MemberType = typeof(MustTimeSpanClausesTestData.DurationBetween))]
    [MemberData(nameof(MustTimeSpanClausesTestData.DurationBetween.InvalidCases), MemberType = typeof(MustTimeSpanClausesTestData.DurationBetween))]
    [MemberData(nameof(MustTimeSpanClausesTestData.DurationBetween.InvalidRangeCases), MemberType = typeof(MustTimeSpanClausesTestData.DurationBetween))]
    public void DurationBetween_BehavesAsExpected(MustCase<(TimeSpan value, TimeSpan min, TimeSpan max, Inclusion inclusion)> tc)
    {
        var result = Must.Be.DurationBetween(tc.Value.value, tc.Value.min, tc.Value.max, tc.Value.inclusion, paramName: "value");
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustTimeSpanClausesTestData.NotDurationBetween.ValidCases), MemberType = typeof(MustTimeSpanClausesTestData.NotDurationBetween))]
    [MemberData(nameof(MustTimeSpanClausesTestData.NotDurationBetween.InvalidCases), MemberType = typeof(MustTimeSpanClausesTestData.NotDurationBetween))]
    [MemberData(nameof(MustTimeSpanClausesTestData.NotDurationBetween.InvalidRangeCases), MemberType = typeof(MustTimeSpanClausesTestData.NotDurationBetween))]
    public void NotDurationBetween_BehavesAsExpected(MustCase<(TimeSpan value, TimeSpan min, TimeSpan max, Inclusion inclusion)> tc)
    {
        var result = Must.Be.NotDurationBetween(tc.Value.value, tc.Value.min, tc.Value.max, tc.Value.inclusion, paramName: "value");
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustTimeSpanClausesTestData.LongerThan.ValidCases), MemberType = typeof(MustTimeSpanClausesTestData.LongerThan))]
    [MemberData(nameof(MustTimeSpanClausesTestData.LongerThan.InvalidCases), MemberType = typeof(MustTimeSpanClausesTestData.LongerThan))]
    public void LongerThan_BehavesAsExpected(MustCase<(TimeSpan value, TimeSpan threshold, Inclusion inclusion)> tc)
    {
        var result = Must.Be.LongerThan(tc.Value.value, tc.Value.threshold, tc.Value.inclusion, paramName: "value");
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustTimeSpanClausesTestData.ShorterThan.ValidCases), MemberType = typeof(MustTimeSpanClausesTestData.ShorterThan))]
    [MemberData(nameof(MustTimeSpanClausesTestData.ShorterThan.InvalidCases), MemberType = typeof(MustTimeSpanClausesTestData.ShorterThan))]
    public void ShorterThan_BehavesAsExpected(MustCase<(TimeSpan value, TimeSpan threshold, Inclusion inclusion)> tc)
    {
        var result = Must.Be.ShorterThan(tc.Value.value, tc.Value.threshold, tc.Value.inclusion, paramName: "value");
        AssertResult(tc, result);
    }
}
