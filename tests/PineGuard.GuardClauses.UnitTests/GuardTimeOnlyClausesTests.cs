using PineGuard.Common;
using PineGuard.Testing.UnitTests.GuardClauses;
using Xunit.Abstractions;

namespace PineGuard.GuardClauses.UnitTests;

public sealed class GuardTimeOnlyClausesTests(ITestOutputHelper output) : BaseGuardUnitTest(output)
{
    [Theory]
    [MemberData(nameof(GuardTimeOnlyClausesTestData.Between.Cases), MemberType = typeof(GuardTimeOnlyClausesTestData.Between))]
    public void Between_BehavesAsExpected(GuardCase<(TimeOnly value, TimeOnly min, TimeOnly max, Inclusion inclusion)> tc)
    { var value = tc.Value.value; var result = AssertResult(tc, () => Guard.Against.Between(value, tc.Value.min, tc.Value.max, tc.Value.inclusion)); if (tc.Expected.IsValid) Assert.Equal(value, result); }

    [Theory]
    [MemberData(nameof(GuardTimeOnlyClausesTestData.NotBetween.Cases), MemberType = typeof(GuardTimeOnlyClausesTestData.NotBetween))]
    public void NotBetween_BehavesAsExpected(GuardCase<(TimeOnly value, TimeOnly min, TimeOnly max, Inclusion inclusion)> tc)
    { var value = tc.Value.value; var result = AssertResult(tc, () => Guard.Against.NotBetween(value, tc.Value.min, tc.Value.max, tc.Value.inclusion)); if (tc.Expected.IsValid) Assert.Equal(value, result); }

    [Theory]
    [MemberData(nameof(GuardTimeOnlyClausesTestData.Before.Cases), MemberType = typeof(GuardTimeOnlyClausesTestData.Before))]
    public void Before_BehavesAsExpected(GuardCase<(TimeOnly value, TimeOnly other)> tc)
    { var value = tc.Value.value; var result = AssertResult(tc, () => Guard.Against.Before(value, tc.Value.other)); if (tc.Expected.IsValid) Assert.Equal(value, result); }

    [Theory]
    [MemberData(nameof(GuardTimeOnlyClausesTestData.OnOrBefore.Cases), MemberType = typeof(GuardTimeOnlyClausesTestData.OnOrBefore))]
    public void OnOrBefore_BehavesAsExpected(GuardCase<(TimeOnly value, TimeOnly other)> tc)
    { var value = tc.Value.value; var result = AssertResult(tc, () => Guard.Against.OnOrBefore(value, tc.Value.other)); if (tc.Expected.IsValid) Assert.Equal(value, result); }

    [Theory]
    [MemberData(nameof(GuardTimeOnlyClausesTestData.After.Cases), MemberType = typeof(GuardTimeOnlyClausesTestData.After))]
    public void After_BehavesAsExpected(GuardCase<(TimeOnly value, TimeOnly other)> tc)
    { var value = tc.Value.value; var result = AssertResult(tc, () => Guard.Against.After(value, tc.Value.other)); if (tc.Expected.IsValid) Assert.Equal(value, result); }

    [Theory]
    [MemberData(nameof(GuardTimeOnlyClausesTestData.OnOrAfter.Cases), MemberType = typeof(GuardTimeOnlyClausesTestData.OnOrAfter))]
    public void OnOrAfter_BehavesAsExpected(GuardCase<(TimeOnly value, TimeOnly other)> tc)
    { var value = tc.Value.value; var result = AssertResult(tc, () => Guard.Against.OnOrAfter(value, tc.Value.other)); if (tc.Expected.IsValid) Assert.Equal(value, result); }

    [Theory]
    [MemberData(nameof(GuardTimeOnlyClausesTestData.Same.Cases), MemberType = typeof(GuardTimeOnlyClausesTestData.Same))]
    public void Same_BehavesAsExpected(GuardCase<(TimeOnly value, TimeOnly other)> tc)
    { var value = tc.Value.value; var result = AssertResult(tc, () => Guard.Against.Same(value, tc.Value.other)); if (tc.Expected.IsValid) Assert.Equal(value, result); }

    [Theory]
    [MemberData(nameof(GuardTimeOnlyClausesTestData.NotSame.Cases), MemberType = typeof(GuardTimeOnlyClausesTestData.NotSame))]
    public void NotSame_BehavesAsExpected(GuardCase<(TimeOnly value, TimeOnly other)> tc)
    { var value = tc.Value.value; var result = AssertResult(tc, () => Guard.Against.NotSame(value, tc.Value.other)); if (tc.Expected.IsValid) Assert.Equal(value, result); }

    [Theory]
    [MemberData(nameof(GuardTimeOnlyClausesTestData.NotChronological.Cases), MemberType = typeof(GuardTimeOnlyClausesTestData.NotChronological))]
    public void NotChronological_BehavesAsExpected(GuardCase<(TimeOnly start, TimeOnly end, Inclusion inclusion)> tc)
    { var start = tc.Value.start; var result = AssertResult(tc, () => Guard.Against.NotChronological(start, tc.Value.end, tc.Value.inclusion)); if (tc.Expected.IsValid) Assert.Equal(start, result); }

    [Theory]
    [MemberData(nameof(GuardTimeOnlyClausesTestData.Chronological.Cases), MemberType = typeof(GuardTimeOnlyClausesTestData.Chronological))]
    public void Chronological_BehavesAsExpected(GuardCase<(TimeOnly start, TimeOnly end, Inclusion inclusion)> tc)
    { var start = tc.Value.start; var result = AssertResult(tc, () => Guard.Against.Chronological(start, tc.Value.end, tc.Value.inclusion)); if (tc.Expected.IsValid) Assert.Equal(start, result); }

    [Theory]
    [MemberData(nameof(GuardTimeOnlyClausesTestData.Overlapping.Cases), MemberType = typeof(GuardTimeOnlyClausesTestData.Overlapping))]
    public void Overlapping_BehavesAsExpected(GuardCase<(TimeOnly start1, TimeOnly end1, TimeOnly start2, TimeOnly end2, Inclusion inclusion)> tc)
    { var start1 = tc.Value.start1; var result = AssertResult(tc, () => Guard.Against.Overlapping(start1, tc.Value.end1, tc.Value.start2, tc.Value.end2, tc.Value.inclusion)); if (tc.Expected.IsValid) Assert.Equal(start1, result); }

    [Theory]
    [MemberData(nameof(GuardTimeOnlyClausesTestData.NotOverlapping.Cases), MemberType = typeof(GuardTimeOnlyClausesTestData.NotOverlapping))]
    public void NotOverlapping_BehavesAsExpected(GuardCase<(TimeOnly start1, TimeOnly end1, TimeOnly start2, TimeOnly end2, Inclusion inclusion)> tc)
    { var start1 = tc.Value.start1; var result = AssertResult(tc, () => Guard.Against.NotOverlapping(start1, tc.Value.end1, tc.Value.start2, tc.Value.end2, tc.Value.inclusion)); if (tc.Expected.IsValid) Assert.Equal(start1, result); }
}
