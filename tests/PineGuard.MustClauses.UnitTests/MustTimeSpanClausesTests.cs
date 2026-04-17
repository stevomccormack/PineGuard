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
    [MemberData(nameof(MustTimeSpanClausesTestData.GreaterThan.ValidCases), MemberType = typeof(MustTimeSpanClausesTestData.GreaterThan))]
    [MemberData(nameof(MustTimeSpanClausesTestData.GreaterThan.InvalidCases), MemberType = typeof(MustTimeSpanClausesTestData.GreaterThan))]
    public void GreaterThan_BehavesAsExpected(MustCase<(TimeSpan value, TimeSpan threshold, Inclusion inclusion)> tc)
    {
        var result = Must.Be.GreaterThan(tc.Value.value, tc.Value.threshold, tc.Value.inclusion, paramName: "value");
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustTimeSpanClausesTestData.LessThan.ValidCases), MemberType = typeof(MustTimeSpanClausesTestData.LessThan))]
    [MemberData(nameof(MustTimeSpanClausesTestData.LessThan.InvalidCases), MemberType = typeof(MustTimeSpanClausesTestData.LessThan))]
    public void LessThan_BehavesAsExpected(MustCase<(TimeSpan value, TimeSpan threshold, Inclusion inclusion)> tc)
    {
        var result = Must.Be.LessThan(tc.Value.value, tc.Value.threshold, tc.Value.inclusion, paramName: "value");
        AssertResult(tc, result);
    }
}
