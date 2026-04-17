using PineGuard.Common;
using PineGuard.Testing.UnitTests.MustClauses;
using Xunit.Abstractions;

namespace PineGuard.MustClauses.UnitTests;

public sealed class MustTimeOnlyClausesTests(ITestOutputHelper output) : BaseMustUnitTest(output)
{
    [Theory]
    [MemberData(nameof(MustTimeOnlyClausesTestData.Between.Cases), MemberType = typeof(MustTimeOnlyClausesTestData.Between))]
    public void Between_BehavesAsExpected(MustCase<(TimeOnly value, TimeOnly min, TimeOnly max)> tc)
    { var value = tc.Value.value; var result = Must.Be.Between(value, tc.Value.min, tc.Value.max); AssertResult(tc, result); }

    [Theory]
    [MemberData(nameof(MustTimeOnlyClausesTestData.NotBetween.Cases), MemberType = typeof(MustTimeOnlyClausesTestData.NotBetween))]
    public void NotBetween_BehavesAsExpected(MustCase<(TimeOnly value, TimeOnly min, TimeOnly max)> tc)
    { var value = tc.Value.value; var result = Must.Be.NotBetween(value, tc.Value.min, tc.Value.max); AssertResult(tc, result); }

    [Theory]
    [MemberData(nameof(MustTimeOnlyClausesTestData.Before.Cases), MemberType = typeof(MustTimeOnlyClausesTestData.Before))]
    public void Before_BehavesAsExpected(MustCase<(TimeOnly value, TimeOnly other, TimePrecision? precision)> tc)
    { var value = tc.Value.value; var result = Must.Be.Before(value, tc.Value.other, tc.Value.precision); AssertResult(tc, result); }

    [Theory]
    [MemberData(nameof(MustTimeOnlyClausesTestData.OnOrBefore.Cases), MemberType = typeof(MustTimeOnlyClausesTestData.OnOrBefore))]
    public void OnOrBefore_BehavesAsExpected(MustCase<(TimeOnly value, TimeOnly other, TimePrecision? precision)> tc)
    { var value = tc.Value.value; var result = Must.Be.OnOrBefore(value, tc.Value.other, tc.Value.precision); AssertResult(tc, result); }

    [Theory]
    [MemberData(nameof(MustTimeOnlyClausesTestData.After.Cases), MemberType = typeof(MustTimeOnlyClausesTestData.After))]
    public void After_BehavesAsExpected(MustCase<(TimeOnly value, TimeOnly other, TimePrecision? precision)> tc)
    { var value = tc.Value.value; var result = Must.Be.After(value, tc.Value.other, tc.Value.precision); AssertResult(tc, result); }

    [Theory]
    [MemberData(nameof(MustTimeOnlyClausesTestData.OnOrAfter.Cases), MemberType = typeof(MustTimeOnlyClausesTestData.OnOrAfter))]
    public void OnOrAfter_BehavesAsExpected(MustCase<(TimeOnly value, TimeOnly other, TimePrecision? precision)> tc)
    { var value = tc.Value.value; var result = Must.Be.OnOrAfter(value, tc.Value.other, tc.Value.precision); AssertResult(tc, result); }

    [Theory]
    [MemberData(nameof(MustTimeOnlyClausesTestData.Same.Cases), MemberType = typeof(MustTimeOnlyClausesTestData.Same))]
    public void Same_BehavesAsExpected(MustCase<(TimeOnly value, TimeOnly other, TimePrecision? precision)> tc)
    { var value = tc.Value.value; var result = Must.Be.Same(value, tc.Value.other, tc.Value.precision); AssertResult(tc, result); }

    [Theory]
    [MemberData(nameof(MustTimeOnlyClausesTestData.NotSame.Cases), MemberType = typeof(MustTimeOnlyClausesTestData.NotSame))]
    public void NotSame_BehavesAsExpected(MustCase<(TimeOnly value, TimeOnly other, TimePrecision? precision)> tc)
    { var value = tc.Value.value; var result = Must.Be.NotSame(value, tc.Value.other, tc.Value.precision); AssertResult(tc, result); }

    [Theory]
    [MemberData(nameof(MustTimeOnlyClausesTestData.Within.Cases), MemberType = typeof(MustTimeOnlyClausesTestData.Within))]
    public void Within_BehavesAsExpected(MustCase<(TimeOnly value, TimeOnly reference, TimeSpan window)> tc)
    { var value = tc.Value.value; var result = Must.Be.Within(value, tc.Value.reference, tc.Value.window); AssertResult(tc, result); }

    [Theory]
    [MemberData(nameof(MustTimeOnlyClausesTestData.NotWithin.Cases), MemberType = typeof(MustTimeOnlyClausesTestData.NotWithin))]
    public void NotWithin_BehavesAsExpected(MustCase<(TimeOnly value, TimeOnly reference, TimeSpan window)> tc)
    { var value = tc.Value.value; var result = Must.Be.NotWithin(value, tc.Value.reference, tc.Value.window); AssertResult(tc, result); }

    [Theory]
    [MemberData(nameof(MustTimeOnlyClausesTestData.Chronological.Cases), MemberType = typeof(MustTimeOnlyClausesTestData.Chronological))]
    public void Chronological_BehavesAsExpected(MustCase<(TimeOnly start, TimeOnly end)> tc)
    { var start = tc.Value.start; var result = Must.Be.Chronological(start, tc.Value.end); AssertResult(tc, result); }

    [Theory]
    [MemberData(nameof(MustTimeOnlyClausesTestData.NotChronological.Cases), MemberType = typeof(MustTimeOnlyClausesTestData.NotChronological))]
    public void NotChronological_BehavesAsExpected(MustCase<(TimeOnly start, TimeOnly end)> tc)
    { var start = tc.Value.start; var result = Must.Be.NotChronological(start, tc.Value.end); AssertResult(tc, result); }

    [Theory]
    [MemberData(nameof(MustTimeOnlyClausesTestData.Overlapping.Cases), MemberType = typeof(MustTimeOnlyClausesTestData.Overlapping))]
    public void Overlapping_BehavesAsExpected(MustCase<(TimeOnly start1, TimeOnly end1, TimeOnly start2, TimeOnly end2)> tc)
    { var start1 = tc.Value.start1; var result = Must.Be.Overlapping(start1, tc.Value.end1, tc.Value.start2, tc.Value.end2); AssertResult(tc, result); }

    [Theory]
    [MemberData(nameof(MustTimeOnlyClausesTestData.NotOverlapping.Cases), MemberType = typeof(MustTimeOnlyClausesTestData.NotOverlapping))]
    public void NotOverlapping_BehavesAsExpected(MustCase<(TimeOnly start1, TimeOnly end1, TimeOnly start2, TimeOnly end2)> tc)
    { var start1 = tc.Value.start1; var result = Must.Be.NotOverlapping(start1, tc.Value.end1, tc.Value.start2, tc.Value.end2); AssertResult(tc, result); }

    [Theory]
    [MemberData(nameof(MustTimeOnlyClausesTestData.NotBefore.Cases), MemberType = typeof(MustTimeOnlyClausesTestData.NotBefore))]
    public void NotBefore_BehavesAsExpected(MustCase<(TimeOnly value, TimeOnly other, TimePrecision? precision)> tc)
    { var value = tc.Value.value; var result = Must.Be.NotBefore(value, tc.Value.other, tc.Value.precision); AssertResult(tc, result); }

    [Theory]
    [MemberData(nameof(MustTimeOnlyClausesTestData.NotOnOrBefore.Cases), MemberType = typeof(MustTimeOnlyClausesTestData.NotOnOrBefore))]
    public void NotOnOrBefore_BehavesAsExpected(MustCase<(TimeOnly value, TimeOnly other, TimePrecision? precision)> tc)
    { var value = tc.Value.value; var result = Must.Be.NotOnOrBefore(value, tc.Value.other, tc.Value.precision); AssertResult(tc, result); }

    [Theory]
    [MemberData(nameof(MustTimeOnlyClausesTestData.NotAfter.Cases), MemberType = typeof(MustTimeOnlyClausesTestData.NotAfter))]
    public void NotAfter_BehavesAsExpected(MustCase<(TimeOnly value, TimeOnly other, TimePrecision? precision)> tc)
    { var value = tc.Value.value; var result = Must.Be.NotAfter(value, tc.Value.other, tc.Value.precision); AssertResult(tc, result); }

    [Theory]
    [MemberData(nameof(MustTimeOnlyClausesTestData.NotOnOrAfter.Cases), MemberType = typeof(MustTimeOnlyClausesTestData.NotOnOrAfter))]
    public void NotOnOrAfter_BehavesAsExpected(MustCase<(TimeOnly value, TimeOnly other, TimePrecision? precision)> tc)
    { var value = tc.Value.value; var result = Must.Be.NotOnOrAfter(value, tc.Value.other, tc.Value.precision); AssertResult(tc, result); }
}
