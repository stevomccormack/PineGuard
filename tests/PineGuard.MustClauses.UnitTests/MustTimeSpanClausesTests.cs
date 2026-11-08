using PineGuard.Common;
using PineGuard.Testing.UnitTests.MustClauses;
using Xunit.Abstractions;

namespace PineGuard.MustClauses.UnitTests;

public sealed class MustTimeSpanClausesTests(ITestOutputHelper output) : BaseMustUnitTest(output)
{
    [Theory]
    [MemberData(nameof(MustTimeSpanClausesTestData.InRange.ValidCases), MemberType = typeof(MustTimeSpanClausesTestData.InRange))]
    [MemberData(nameof(MustTimeSpanClausesTestData.InRange.InvalidCases), MemberType = typeof(MustTimeSpanClausesTestData.InRange))]
    [MemberData(nameof(MustTimeSpanClausesTestData.InRange.InvalidRangeCases), MemberType = typeof(MustTimeSpanClausesTestData.InRange))]
    public void InRange_BehavesAsExpected(MustCase<(TimeSpan value, TimeSpan min, TimeSpan max, Inclusion inclusion)> tc)
    {
        var result = Must.Be.InRange(tc.Value.value, tc.Value.min, tc.Value.max, tc.Value.inclusion, paramName: "value");
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustTimeSpanClausesTestData.OutOfRange.ValidCases), MemberType = typeof(MustTimeSpanClausesTestData.OutOfRange))]
    [MemberData(nameof(MustTimeSpanClausesTestData.OutOfRange.InvalidCases), MemberType = typeof(MustTimeSpanClausesTestData.OutOfRange))]
    [MemberData(nameof(MustTimeSpanClausesTestData.OutOfRange.InvalidRangeCases), MemberType = typeof(MustTimeSpanClausesTestData.OutOfRange))]
    public void OutOfRange_BehavesAsExpected(MustCase<(TimeSpan value, TimeSpan min, TimeSpan max, Inclusion inclusion)> tc)
    {
        var result = Must.Be.OutOfRange(tc.Value.value, tc.Value.min, tc.Value.max, tc.Value.inclusion, paramName: "value");
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustTimeSpanClausesTestData.GreaterThan.ValidCases), MemberType = typeof(MustTimeSpanClausesTestData.GreaterThan))]
    [MemberData(nameof(MustTimeSpanClausesTestData.GreaterThan.InvalidCases), MemberType = typeof(MustTimeSpanClausesTestData.GreaterThan))]
    public void GreaterThan_BehavesAsExpected(MustCase<(TimeSpan value, TimeSpan other, Inclusion inclusion)> tc)
    {
        var result = Must.Be.GreaterThan(tc.Value.value, tc.Value.other, tc.Value.inclusion, paramName: "value");
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustTimeSpanClausesTestData.LessThan.ValidCases), MemberType = typeof(MustTimeSpanClausesTestData.LessThan))]
    [MemberData(nameof(MustTimeSpanClausesTestData.LessThan.InvalidCases), MemberType = typeof(MustTimeSpanClausesTestData.LessThan))]
    public void LessThan_BehavesAsExpected(MustCase<(TimeSpan value, TimeSpan other, Inclusion inclusion)> tc)
    {
        var result = Must.Be.LessThan(tc.Value.value, tc.Value.other, tc.Value.inclusion, paramName: "value");
        AssertResult(tc, result);
    }
}
