using PineGuard.Common;
using PineGuard.Testing.UnitTests.GuardClauses;
using Xunit.Abstractions;

namespace PineGuard.GuardClauses.UnitTests;

public sealed class GuardStringTimeOnlyClausesTests(ITestOutputHelper output) : BaseGuardUnitTest(output)
{
    [Theory]
    [MemberData(nameof(GuardStringTimeOnlyClausesTestData.NotBetweenTimeOnly.Cases), MemberType = typeof(GuardStringTimeOnlyClausesTestData.NotBetweenTimeOnly))]
    public void NotBetweenTimeOnly_BehavesAsExpected(GuardCase<(string? value, TimeOnly min, TimeOnly max, Inclusion inclusion)> tc)
    { var value = tc.Value.value; var result = AssertResult(tc, () => Guard.Against.NotBetweenTimeOnly(value, tc.Value.min, tc.Value.max, tc.Value.inclusion)); if (tc.Expected.IsValid && value is not null) Assert.Equal(TimeOnly.Parse(value), result); }

    [Theory]
    [MemberData(nameof(GuardStringTimeOnlyClausesTestData.BetweenTimeOnly.Cases), MemberType = typeof(GuardStringTimeOnlyClausesTestData.BetweenTimeOnly))]
    public void BetweenTimeOnly_BehavesAsExpected(GuardCase<(string? value, TimeOnly min, TimeOnly max, Inclusion inclusion)> tc)
    { var value = tc.Value.value; var result = AssertResult(tc, () => Guard.Against.BetweenTimeOnly(value, tc.Value.min, tc.Value.max, tc.Value.inclusion)); if (tc.Expected.IsValid && value is not null) Assert.Equal(TimeOnly.Parse(value), result); }

    [Theory]
    [MemberData(nameof(GuardStringTimeOnlyClausesTestData.NotWithinTimeOnly.Cases), MemberType = typeof(GuardStringTimeOnlyClausesTestData.NotWithinTimeOnly))]
    public void NotWithinTimeOnly_BehavesAsExpected(GuardCase<(string? value, string? reference, TimeSpan window)> tc)
    { var value = tc.Value.value; var result = AssertResult(tc, () => Guard.Against.NotWithinTimeOnly(value, tc.Value.reference!, tc.Value.window)); if (tc.Expected.IsValid && value is not null) Assert.Equal(TimeOnly.Parse(value), result); }

    [Theory]
    [MemberData(nameof(GuardStringTimeOnlyClausesTestData.WithinTimeOnly.Cases), MemberType = typeof(GuardStringTimeOnlyClausesTestData.WithinTimeOnly))]
    public void WithinTimeOnly_BehavesAsExpected(GuardCase<(string? value, string? reference, TimeSpan window)> tc)
    { var value = tc.Value.value; var result = AssertResult(tc, () => Guard.Against.WithinTimeOnly(value, tc.Value.reference!, tc.Value.window)); if (tc.Expected.IsValid && value is not null) Assert.Equal(TimeOnly.Parse(value), result); }

    [Theory]
    [MemberData(nameof(GuardStringTimeOnlyClausesTestData.BeforeTimeOnly.Cases), MemberType = typeof(GuardStringTimeOnlyClausesTestData.BeforeTimeOnly))]
    public void BeforeTimeOnly_BehavesAsExpected(GuardCase<(string? value, TimeOnly other)> tc)
    { var value = tc.Value.value; var result = AssertResult(tc, () => Guard.Against.BeforeTimeOnly(value, tc.Value.other)); if (tc.Expected.IsValid && value is not null) Assert.Equal(TimeOnly.Parse(value), result); }

    [Theory]
    [MemberData(nameof(GuardStringTimeOnlyClausesTestData.OnOrBeforeTimeOnly.Cases), MemberType = typeof(GuardStringTimeOnlyClausesTestData.OnOrBeforeTimeOnly))]
    public void OnOrBeforeTimeOnly_BehavesAsExpected(GuardCase<(string? value, TimeOnly other)> tc)
    { var value = tc.Value.value; var result = AssertResult(tc, () => Guard.Against.OnOrBeforeTimeOnly(value, tc.Value.other)); if (tc.Expected.IsValid && value is not null) Assert.Equal(TimeOnly.Parse(value), result); }

    [Theory]
    [MemberData(nameof(GuardStringTimeOnlyClausesTestData.NotBeforeTimeOnly.Cases), MemberType = typeof(GuardStringTimeOnlyClausesTestData.NotBeforeTimeOnly))]
    public void NotBeforeTimeOnly_BehavesAsExpected(GuardCase<(string? value, TimeOnly other)> tc)
    { var value = tc.Value.value; var result = AssertResult(tc, () => Guard.Against.NotBeforeTimeOnly(value, tc.Value.other)); if (tc.Expected.IsValid && value is not null) Assert.Equal(TimeOnly.Parse(value), result); }

    [Theory]
    [MemberData(nameof(GuardStringTimeOnlyClausesTestData.NotOnOrBeforeTimeOnly.Cases), MemberType = typeof(GuardStringTimeOnlyClausesTestData.NotOnOrBeforeTimeOnly))]
    public void NotOnOrBeforeTimeOnly_BehavesAsExpected(GuardCase<(string? value, TimeOnly other)> tc)
    { var value = tc.Value.value; var result = AssertResult(tc, () => Guard.Against.NotOnOrBeforeTimeOnly(value, tc.Value.other)); if (tc.Expected.IsValid && value is not null) Assert.Equal(TimeOnly.Parse(value), result); }

    [Theory]
    [MemberData(nameof(GuardStringTimeOnlyClausesTestData.AfterTimeOnly.Cases), MemberType = typeof(GuardStringTimeOnlyClausesTestData.AfterTimeOnly))]
    public void AfterTimeOnly_BehavesAsExpected(GuardCase<(string? value, TimeOnly other)> tc)
    { var value = tc.Value.value; var result = AssertResult(tc, () => Guard.Against.AfterTimeOnly(value, tc.Value.other)); if (tc.Expected.IsValid && value is not null) Assert.Equal(TimeOnly.Parse(value), result); }

    [Theory]
    [MemberData(nameof(GuardStringTimeOnlyClausesTestData.OnOrAfterTimeOnly.Cases), MemberType = typeof(GuardStringTimeOnlyClausesTestData.OnOrAfterTimeOnly))]
    public void OnOrAfterTimeOnly_BehavesAsExpected(GuardCase<(string? value, TimeOnly other)> tc)
    { var value = tc.Value.value; var result = AssertResult(tc, () => Guard.Against.OnOrAfterTimeOnly(value, tc.Value.other)); if (tc.Expected.IsValid && value is not null) Assert.Equal(TimeOnly.Parse(value), result); }

    [Theory]
    [MemberData(nameof(GuardStringTimeOnlyClausesTestData.NotAfterTimeOnly.Cases), MemberType = typeof(GuardStringTimeOnlyClausesTestData.NotAfterTimeOnly))]
    public void NotAfterTimeOnly_BehavesAsExpected(GuardCase<(string? value, TimeOnly other)> tc)
    { var value = tc.Value.value; var result = AssertResult(tc, () => Guard.Against.NotAfterTimeOnly(value, tc.Value.other)); if (tc.Expected.IsValid && value is not null) Assert.Equal(TimeOnly.Parse(value), result); }

    [Theory]
    [MemberData(nameof(GuardStringTimeOnlyClausesTestData.NotOnOrAfterTimeOnly.Cases), MemberType = typeof(GuardStringTimeOnlyClausesTestData.NotOnOrAfterTimeOnly))]
    public void NotOnOrAfterTimeOnly_BehavesAsExpected(GuardCase<(string? value, TimeOnly other)> tc)
    { var value = tc.Value.value; var result = AssertResult(tc, () => Guard.Against.NotOnOrAfterTimeOnly(value, tc.Value.other)); if (tc.Expected.IsValid && value is not null) Assert.Equal(TimeOnly.Parse(value), result); }

    [Theory]
    [MemberData(nameof(GuardStringTimeOnlyClausesTestData.SameTimeOnly.Cases), MemberType = typeof(GuardStringTimeOnlyClausesTestData.SameTimeOnly))]
    public void SameTimeOnly_BehavesAsExpected(GuardCase<(string? value, TimeOnly other)> tc)
    { var value = tc.Value.value; var result = AssertResult(tc, () => Guard.Against.SameTimeOnly(value, tc.Value.other)); if (tc.Expected.IsValid && value is not null) Assert.Equal(TimeOnly.Parse(value), result); }

    [Theory]
    [MemberData(nameof(GuardStringTimeOnlyClausesTestData.NotSameTimeOnly.Cases), MemberType = typeof(GuardStringTimeOnlyClausesTestData.NotSameTimeOnly))]
    public void NotSameTimeOnly_BehavesAsExpected(GuardCase<(string? value, TimeOnly other)> tc)
    { var value = tc.Value.value; var result = AssertResult(tc, () => Guard.Against.NotSameTimeOnly(value, tc.Value.other)); if (tc.Expected.IsValid && value is not null) Assert.Equal(TimeOnly.Parse(value), result); }

    [Theory]
    [MemberData(nameof(GuardStringTimeOnlyClausesTestData.ChronologicalTimeOnly.Cases), MemberType = typeof(GuardStringTimeOnlyClausesTestData.ChronologicalTimeOnly))]
    public void ChronologicalTimeOnly_BehavesAsExpected(GuardCase<(string? start, string? end)> tc)
    { var start = tc.Value.start; var result = AssertResult(tc, () => Guard.Against.ChronologicalTimeOnly(start!, tc.Value.end!)); if (tc.Expected.IsValid && start is not null) Assert.Equal(TimeOnly.Parse(start), result); }

    [Theory]
    [MemberData(nameof(GuardStringTimeOnlyClausesTestData.NotChronologicalTimeOnly.Cases), MemberType = typeof(GuardStringTimeOnlyClausesTestData.NotChronologicalTimeOnly))]
    public void NotChronologicalTimeOnly_BehavesAsExpected(GuardCase<(string? start, string? end)> tc)
    { var start = tc.Value.start; var result = AssertResult(tc, () => Guard.Against.NotChronologicalTimeOnly(start!, tc.Value.end!)); if (tc.Expected.IsValid && start is not null) Assert.Equal(TimeOnly.Parse(start), result); }

    [Theory]
    [MemberData(nameof(GuardStringTimeOnlyClausesTestData.OverlappingTimeOnly.Cases), MemberType = typeof(GuardStringTimeOnlyClausesTestData.OverlappingTimeOnly))]
    public void OverlappingTimeOnly_BehavesAsExpected(GuardCase<(string? start1, string? end1, string? start2, string? end2)> tc)
    { var start1 = tc.Value.start1; var result = AssertResult(tc, () => Guard.Against.OverlappingTimeOnly(start1!, tc.Value.end1!, tc.Value.start2!, tc.Value.end2!)); if (tc.Expected.IsValid && start1 is not null) Assert.Equal(TimeOnly.Parse(start1), result); }

    [Theory]
    [MemberData(nameof(GuardStringTimeOnlyClausesTestData.NotOverlappingTimeOnly.Cases), MemberType = typeof(GuardStringTimeOnlyClausesTestData.NotOverlappingTimeOnly))]
    public void NotOverlappingTimeOnly_BehavesAsExpected(GuardCase<(string? start1, string? end1, string? start2, string? end2)> tc)
    { var start1 = tc.Value.start1; var result = AssertResult(tc, () => Guard.Against.NotOverlappingTimeOnly(start1!, tc.Value.end1!, tc.Value.start2!, tc.Value.end2!)); if (tc.Expected.IsValid && start1 is not null) Assert.Equal(TimeOnly.Parse(start1), result); }
}
