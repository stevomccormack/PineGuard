using PineGuard.Common;
using PineGuard.Testing.UnitTests.MustClauses;
using Xunit.Abstractions;

namespace PineGuard.MustClauses.UnitTests;

public sealed class MustStringTimeSpanClausesTests(ITestOutputHelper output) : BaseMustUnitTest(output)
{
    [Theory]
    [MemberData(nameof(MustStringTimeSpanClausesTestData.InRange.ValidCases), MemberType = typeof(MustStringTimeSpanClausesTestData.InRange))]
    [MemberData(nameof(MustStringTimeSpanClausesTestData.InRange.InvalidCases), MemberType = typeof(MustStringTimeSpanClausesTestData.InRange))]
    public void InRange_BehavesAsExpected(MustCase<(string? value, TimeSpan min, TimeSpan max, Inclusion inclusion)> tc)
    { var value = tc.Value.value; var result = Must.Be.InRangeTimeSpan(value, tc.Value.min, tc.Value.max, tc.Value.inclusion); AssertResult(tc, result); }

    [Theory]
    [MemberData(nameof(MustStringTimeSpanClausesTestData.OutOfRange.Cases), MemberType = typeof(MustStringTimeSpanClausesTestData.OutOfRange))]
    public void OutOfRange_BehavesAsExpected(MustCase<(string? value, TimeSpan min, TimeSpan max, Inclusion inclusion)> tc)
    { var value = tc.Value.value; var result = Must.Be.OutOfRangeTimeSpan(value, tc.Value.min, tc.Value.max, tc.Value.inclusion); AssertResult(tc, result); }

    [Theory]
    [MemberData(nameof(MustStringTimeSpanClausesTestData.LongerThan.ValidCases), MemberType = typeof(MustStringTimeSpanClausesTestData.LongerThan))]
    [MemberData(nameof(MustStringTimeSpanClausesTestData.LongerThan.InvalidCases), MemberType = typeof(MustStringTimeSpanClausesTestData.LongerThan))]
    public void LongerThan_BehavesAsExpected(MustCase<(string? value, TimeSpan other, Inclusion inclusion)> tc)
    { var value = tc.Value.value; var result = Must.Be.LongerThanTimeSpan(value, tc.Value.other, tc.Value.inclusion); AssertResult(tc, result); }

    [Theory]
    [MemberData(nameof(MustStringTimeSpanClausesTestData.ShorterThan.ValidCases), MemberType = typeof(MustStringTimeSpanClausesTestData.ShorterThan))]
    [MemberData(nameof(MustStringTimeSpanClausesTestData.ShorterThan.InvalidCases), MemberType = typeof(MustStringTimeSpanClausesTestData.ShorterThan))]
    public void ShorterThan_BehavesAsExpected(MustCase<(string? value, TimeSpan other, Inclusion inclusion)> tc)
    { var value = tc.Value.value; var result = Must.Be.ShorterThanTimeSpan(value, tc.Value.other, tc.Value.inclusion); AssertResult(tc, result); }
}
