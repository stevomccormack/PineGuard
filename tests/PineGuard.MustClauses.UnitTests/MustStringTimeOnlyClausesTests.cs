using PineGuard.Common;
using PineGuard.Testing.UnitTests.MustClauses;
using Xunit.Abstractions;

namespace PineGuard.MustClauses.UnitTests;

public sealed class MustStringTimeOnlyClausesTests(ITestOutputHelper output) : BaseMustUnitTest(output)
{
    [Theory]
    [MemberData(nameof(MustStringTimeOnlyClausesTestData.BetweenTimeOnly.Cases), MemberType = typeof(MustStringTimeOnlyClausesTestData.BetweenTimeOnly))]
    public void BetweenTimeOnly_BehavesAsExpected(MustCase<(string? value, TimeOnly min, TimeOnly max)> tc)
    { var value = tc.Value.value; var result = Must.Be.BetweenTimeOnly(value, tc.Value.min, tc.Value.max); AssertResult(tc, result); }

    [Theory]
    [MemberData(nameof(MustStringTimeOnlyClausesTestData.NotBetweenTimeOnly.Cases), MemberType = typeof(MustStringTimeOnlyClausesTestData.NotBetweenTimeOnly))]
    public void NotBetweenTimeOnly_BehavesAsExpected(MustCase<(string? value, TimeOnly min, TimeOnly max)> tc)
    { var value = tc.Value.value; var result = Must.Be.NotBetweenTimeOnly(value, tc.Value.min, tc.Value.max); AssertResult(tc, result); }

    [Theory]
    [MemberData(nameof(MustStringTimeOnlyClausesTestData.WithinTimeOnly.Cases), MemberType = typeof(MustStringTimeOnlyClausesTestData.WithinTimeOnly))]
    public void WithinTimeOnly_BehavesAsExpected(MustCase<(string? value, string? reference, TimeSpan window)> tc)
    { var value = tc.Value.value; var result = Must.Be.WithinTimeOnly(value, tc.Value.reference!, tc.Value.window); AssertResult(tc, result); }

    [Theory]
    [MemberData(nameof(MustStringTimeOnlyClausesTestData.NotWithinTimeOnly.Cases), MemberType = typeof(MustStringTimeOnlyClausesTestData.NotWithinTimeOnly))]
    public void NotWithinTimeOnly_BehavesAsExpected(MustCase<(string? value, string? reference, TimeSpan window)> tc)
    { var value = tc.Value.value; var result = Must.Be.NotWithinTimeOnly(value, tc.Value.reference!, tc.Value.window); AssertResult(tc, result); }

    [Theory]
    [MemberData(nameof(MustStringTimeOnlyClausesTestData.BeforeTimeOnly.Cases), MemberType = typeof(MustStringTimeOnlyClausesTestData.BeforeTimeOnly))]
    public void BeforeTimeOnly_BehavesAsExpected(MustCase<(string? value, TimeOnly other, TimePrecision? precision)> tc)
    { var value = tc.Value.value; var result = Must.Be.BeforeTimeOnly(value, tc.Value.other, tc.Value.precision); AssertResult(tc, result); }

    [Theory]
    [MemberData(nameof(MustStringTimeOnlyClausesTestData.OnOrBeforeTimeOnly.Cases), MemberType = typeof(MustStringTimeOnlyClausesTestData.OnOrBeforeTimeOnly))]
    public void OnOrBeforeTimeOnly_BehavesAsExpected(MustCase<(string? value, TimeOnly other, TimePrecision? precision)> tc)
    { var value = tc.Value.value; var result = Must.Be.OnOrBeforeTimeOnly(value, tc.Value.other, tc.Value.precision); AssertResult(tc, result); }

    [Theory]
    [MemberData(nameof(MustStringTimeOnlyClausesTestData.NotBeforeTimeOnly.Cases), MemberType = typeof(MustStringTimeOnlyClausesTestData.NotBeforeTimeOnly))]
    public void NotBeforeTimeOnly_BehavesAsExpected(MustCase<(string? value, TimeOnly other, TimePrecision? precision)> tc)
    { var value = tc.Value.value; var result = Must.Be.NotBeforeTimeOnly(value, tc.Value.other, tc.Value.precision); AssertResult(tc, result); }

    [Theory]
    [MemberData(nameof(MustStringTimeOnlyClausesTestData.NotOnOrBeforeTimeOnly.Cases), MemberType = typeof(MustStringTimeOnlyClausesTestData.NotOnOrBeforeTimeOnly))]
    public void NotOnOrBeforeTimeOnly_BehavesAsExpected(MustCase<(string? value, TimeOnly other, TimePrecision? precision)> tc)
    { var value = tc.Value.value; var result = Must.Be.NotOnOrBeforeTimeOnly(value, tc.Value.other, tc.Value.precision); AssertResult(tc, result); }

    [Theory]
    [MemberData(nameof(MustStringTimeOnlyClausesTestData.AfterTimeOnly.Cases), MemberType = typeof(MustStringTimeOnlyClausesTestData.AfterTimeOnly))]
    public void AfterTimeOnly_BehavesAsExpected(MustCase<(string? value, TimeOnly other, TimePrecision? precision)> tc)
    { var value = tc.Value.value; var result = Must.Be.AfterTimeOnly(value, tc.Value.other, tc.Value.precision); AssertResult(tc, result); }

    [Theory]
    [MemberData(nameof(MustStringTimeOnlyClausesTestData.OnOrAfterTimeOnly.Cases), MemberType = typeof(MustStringTimeOnlyClausesTestData.OnOrAfterTimeOnly))]
    public void OnOrAfterTimeOnly_BehavesAsExpected(MustCase<(string? value, TimeOnly other, TimePrecision? precision)> tc)
    { var value = tc.Value.value; var result = Must.Be.OnOrAfterTimeOnly(value, tc.Value.other, tc.Value.precision); AssertResult(tc, result); }

    [Theory]
    [MemberData(nameof(MustStringTimeOnlyClausesTestData.NotAfterTimeOnly.Cases), MemberType = typeof(MustStringTimeOnlyClausesTestData.NotAfterTimeOnly))]
    public void NotAfterTimeOnly_BehavesAsExpected(MustCase<(string? value, TimeOnly other, TimePrecision? precision)> tc)
    { var value = tc.Value.value; var result = Must.Be.NotAfterTimeOnly(value, tc.Value.other, tc.Value.precision); AssertResult(tc, result); }

    [Theory]
    [MemberData(nameof(MustStringTimeOnlyClausesTestData.NotOnOrAfterTimeOnly.Cases), MemberType = typeof(MustStringTimeOnlyClausesTestData.NotOnOrAfterTimeOnly))]
    public void NotOnOrAfterTimeOnly_BehavesAsExpected(MustCase<(string? value, TimeOnly other, TimePrecision? precision)> tc)
    { var value = tc.Value.value; var result = Must.Be.NotOnOrAfterTimeOnly(value, tc.Value.other, tc.Value.precision); AssertResult(tc, result); }

    [Theory]
    [MemberData(nameof(MustStringTimeOnlyClausesTestData.SameTimeOnly.Cases), MemberType = typeof(MustStringTimeOnlyClausesTestData.SameTimeOnly))]
    public void SameTimeOnly_BehavesAsExpected(MustCase<(string? value, TimeOnly other, TimePrecision? precision)> tc)
    { var value = tc.Value.value; var result = Must.Be.SameTimeOnly(value, tc.Value.other, tc.Value.precision); AssertResult(tc, result); }

    [Theory]
    [MemberData(nameof(MustStringTimeOnlyClausesTestData.NotSameTimeOnly.Cases), MemberType = typeof(MustStringTimeOnlyClausesTestData.NotSameTimeOnly))]
    public void NotSameTimeOnly_BehavesAsExpected(MustCase<(string? value, TimeOnly other, TimePrecision? precision)> tc)
    { var value = tc.Value.value; var result = Must.Be.NotSameTimeOnly(value, tc.Value.other, tc.Value.precision); AssertResult(tc, result); }

    [Theory]
    [MemberData(nameof(MustStringTimeOnlyClausesTestData.ChronologicalTimeOnly.Cases), MemberType = typeof(MustStringTimeOnlyClausesTestData.ChronologicalTimeOnly))]
    public void ChronologicalTimeOnly_BehavesAsExpected(MustCase<(string? start, string? end)> tc)
    { var start = tc.Value.start; var result = Must.Be.ChronologicalTimeOnly(start, tc.Value.end); AssertResult(tc, result); }

    [Theory]
    [MemberData(nameof(MustStringTimeOnlyClausesTestData.NotChronologicalTimeOnly.Cases), MemberType = typeof(MustStringTimeOnlyClausesTestData.NotChronologicalTimeOnly))]
    public void NotChronologicalTimeOnly_BehavesAsExpected(MustCase<(string? start, string? end)> tc)
    { var start = tc.Value.start; var result = Must.Be.NotChronologicalTimeOnly(start, tc.Value.end); AssertResult(tc, result); }

    [Theory]
    [MemberData(nameof(MustStringTimeOnlyClausesTestData.OverlappingTimeOnly.Cases), MemberType = typeof(MustStringTimeOnlyClausesTestData.OverlappingTimeOnly))]
    public void OverlappingTimeOnly_BehavesAsExpected(MustCase<(string? start1, string? end1, string? start2, string? end2)> tc)
    { var start1 = tc.Value.start1; var result = Must.Be.OverlappingTimeOnly(start1, tc.Value.end1, tc.Value.start2, tc.Value.end2); AssertResult(tc, result); }

    [Theory]
    [MemberData(nameof(MustStringTimeOnlyClausesTestData.NotOverlappingTimeOnly.Cases), MemberType = typeof(MustStringTimeOnlyClausesTestData.NotOverlappingTimeOnly))]
    public void NotOverlappingTimeOnly_BehavesAsExpected(MustCase<(string? start1, string? end1, string? start2, string? end2)> tc)
    { var start1 = tc.Value.start1; var result = Must.Be.NotOverlappingTimeOnly(start1, tc.Value.end1, tc.Value.start2, tc.Value.end2); AssertResult(tc, result); }
}
