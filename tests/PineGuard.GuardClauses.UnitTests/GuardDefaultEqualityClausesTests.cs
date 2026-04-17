using PineGuard.Testing.UnitTests.GuardClauses;
using Xunit.Abstractions;

namespace PineGuard.GuardClauses.UnitTests;

public sealed class GuardDefaultEqualityClausesTests(ITestOutputHelper output) : BaseGuardUnitTest(output)
{
    [Theory]
    [MemberData(nameof(GuardDefaultEqualityClausesTestData.Default.ValidCases), MemberType = typeof(GuardDefaultEqualityClausesTestData.Default))]
    [MemberData(nameof(GuardDefaultEqualityClausesTestData.Default.InvalidCases), MemberType = typeof(GuardDefaultEqualityClausesTestData.Default))]
    public void Default_BehavesAsExpected(GuardCase<int> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.Default(value));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(GuardDefaultEqualityClausesTestData.NotDefault.ValidCases), MemberType = typeof(GuardDefaultEqualityClausesTestData.NotDefault))]
    [MemberData(nameof(GuardDefaultEqualityClausesTestData.NotDefault.InvalidCases), MemberType = typeof(GuardDefaultEqualityClausesTestData.NotDefault))]
    public void NotDefault_BehavesAsExpected(GuardCase<int> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.NotDefault(value));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(GuardDefaultEqualityClausesTestData.NullOrDefault.ValidCases), MemberType = typeof(GuardDefaultEqualityClausesTestData.NullOrDefault))]
    [MemberData(nameof(GuardDefaultEqualityClausesTestData.NullOrDefault.InvalidCases), MemberType = typeof(GuardDefaultEqualityClausesTestData.NullOrDefault))]
    public void NullOrDefault_BehavesAsExpected(GuardCase<string?> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.NullOrDefault(value));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(GuardDefaultEqualityClausesTestData.NotNullOrDefault.ValidCases), MemberType = typeof(GuardDefaultEqualityClausesTestData.NotNullOrDefault))]
    [MemberData(nameof(GuardDefaultEqualityClausesTestData.NotNullOrDefault.InvalidCases), MemberType = typeof(GuardDefaultEqualityClausesTestData.NotNullOrDefault))]
    public void NotNullOrDefault_BehavesAsExpected(GuardCase<string?> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.NotNullOrDefault(value));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }
}
