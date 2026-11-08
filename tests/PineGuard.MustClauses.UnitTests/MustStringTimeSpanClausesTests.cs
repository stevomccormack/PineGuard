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
    [MemberData(nameof(MustStringTimeSpanClausesTestData.GreaterThan.ValidCases), MemberType = typeof(MustStringTimeSpanClausesTestData.GreaterThan))]
    [MemberData(nameof(MustStringTimeSpanClausesTestData.GreaterThan.InvalidCases), MemberType = typeof(MustStringTimeSpanClausesTestData.GreaterThan))]
    public void GreaterThan_BehavesAsExpected(MustCase<(string? value, TimeSpan other, Inclusion inclusion)> tc)
    { var value = tc.Value.value; var result = Must.Be.GreaterThanTimeSpan(value, tc.Value.other, tc.Value.inclusion); AssertResult(tc, result); }

    [Theory]
    [MemberData(nameof(MustStringTimeSpanClausesTestData.LessThan.ValidCases), MemberType = typeof(MustStringTimeSpanClausesTestData.LessThan))]
    [MemberData(nameof(MustStringTimeSpanClausesTestData.LessThan.InvalidCases), MemberType = typeof(MustStringTimeSpanClausesTestData.LessThan))]
    public void LessThan_BehavesAsExpected(MustCase<(string? value, TimeSpan other, Inclusion inclusion)> tc)
    { var value = tc.Value.value; var result = Must.Be.LessThanTimeSpan(value, tc.Value.other, tc.Value.inclusion); AssertResult(tc, result); }
}
