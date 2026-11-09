using PineGuard.Common;
using PineGuard.Testing.UnitTests.MustClauses;
using Xunit.Abstractions;

namespace PineGuard.MustClauses.UnitTests;

public sealed class MustStringTimeSpanClausesTests(ITestOutputHelper output) : BaseMustUnitTest(output)
{
    [Theory]
    [MemberData(nameof(MustStringTimeSpanClausesTestData.DurationBetween.ValidCases), MemberType = typeof(MustStringTimeSpanClausesTestData.DurationBetween))]
    [MemberData(nameof(MustStringTimeSpanClausesTestData.DurationBetween.InvalidCases), MemberType = typeof(MustStringTimeSpanClausesTestData.DurationBetween))]
    public void DurationBetween_BehavesAsExpected(MustCase<(string? value, TimeSpan min, TimeSpan max, Inclusion inclusion)> tc)
    { var value = tc.Value.value; var result = Must.Be.DurationBetween(value, tc.Value.min, tc.Value.max, tc.Value.inclusion); AssertResult(tc, result); }

    [Theory]
    [MemberData(nameof(MustStringTimeSpanClausesTestData.NotDurationBetween.Cases), MemberType = typeof(MustStringTimeSpanClausesTestData.NotDurationBetween))]
    public void NotDurationBetween_BehavesAsExpected(MustCase<(string? value, TimeSpan min, TimeSpan max, Inclusion inclusion)> tc)
    { var value = tc.Value.value; var result = Must.Be.NotDurationBetween(value, tc.Value.min, tc.Value.max, tc.Value.inclusion); AssertResult(tc, result); }

    [Theory]
    [MemberData(nameof(MustStringTimeSpanClausesTestData.LongerThan.ValidCases), MemberType = typeof(MustStringTimeSpanClausesTestData.LongerThan))]
    [MemberData(nameof(MustStringTimeSpanClausesTestData.LongerThan.InvalidCases), MemberType = typeof(MustStringTimeSpanClausesTestData.LongerThan))]
    public void LongerThan_BehavesAsExpected(MustCase<(string? value, TimeSpan threshold, Inclusion inclusion)> tc)
    { var value = tc.Value.value; var result = Must.Be.LongerThan(value, tc.Value.threshold, tc.Value.inclusion); AssertResult(tc, result); }

    [Theory]
    [MemberData(nameof(MustStringTimeSpanClausesTestData.ShorterThan.ValidCases), MemberType = typeof(MustStringTimeSpanClausesTestData.ShorterThan))]
    [MemberData(nameof(MustStringTimeSpanClausesTestData.ShorterThan.InvalidCases), MemberType = typeof(MustStringTimeSpanClausesTestData.ShorterThan))]
    public void ShorterThan_BehavesAsExpected(MustCase<(string? value, TimeSpan threshold, Inclusion inclusion)> tc)
    { var value = tc.Value.value; var result = Must.Be.ShorterThan(value, tc.Value.threshold, tc.Value.inclusion); AssertResult(tc, result); }
}
