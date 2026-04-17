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
    [MemberData(nameof(MustStringTimeSpanClausesTestData.GreaterThan.ValidCases), MemberType = typeof(MustStringTimeSpanClausesTestData.GreaterThan))]
    [MemberData(nameof(MustStringTimeSpanClausesTestData.GreaterThan.InvalidCases), MemberType = typeof(MustStringTimeSpanClausesTestData.GreaterThan))]
    public void GreaterThan_BehavesAsExpected(MustCase<(string? value, TimeSpan threshold, Inclusion inclusion)> tc)
    { var value = tc.Value.value; var result = Must.Be.GreaterThan(value, tc.Value.threshold, tc.Value.inclusion); AssertResult(tc, result); }

    [Theory]
    [MemberData(nameof(MustStringTimeSpanClausesTestData.LessThan.ValidCases), MemberType = typeof(MustStringTimeSpanClausesTestData.LessThan))]
    [MemberData(nameof(MustStringTimeSpanClausesTestData.LessThan.InvalidCases), MemberType = typeof(MustStringTimeSpanClausesTestData.LessThan))]
    public void LessThan_BehavesAsExpected(MustCase<(string? value, TimeSpan threshold, Inclusion inclusion)> tc)
    { var value = tc.Value.value; var result = Must.Be.LessThan(value, tc.Value.threshold, tc.Value.inclusion); AssertResult(tc, result); }
}
