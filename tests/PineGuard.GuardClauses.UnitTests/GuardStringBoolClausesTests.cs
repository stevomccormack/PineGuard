using PineGuard.Testing.UnitTests.GuardClauses;
using Xunit.Abstractions;

namespace PineGuard.GuardClauses.UnitTests;

public sealed class GuardStringBoolClausesTests(ITestOutputHelper output) : BaseGuardUnitTest(output)
{
    [Theory]
    [MemberData(nameof(GuardStringBoolClausesTestData.False.ValidCases), MemberType = typeof(GuardStringBoolClausesTestData.False))]
    [MemberData(nameof(GuardStringBoolClausesTestData.False.InvalidCases), MemberType = typeof(GuardStringBoolClausesTestData.False))]
    public void False_BehavesAsExpected(GuardCase<string?> tc)
    {
        var value = tc.Value;
        AssertResult(tc, () => Guard.Against.False(value));
    }

    [Theory]
    [MemberData(nameof(GuardStringBoolClausesTestData.True.ValidCases), MemberType = typeof(GuardStringBoolClausesTestData.True))]
    [MemberData(nameof(GuardStringBoolClausesTestData.True.InvalidCases), MemberType = typeof(GuardStringBoolClausesTestData.True))]
    public void True_BehavesAsExpected(GuardCase<string?> tc)
    {
        var value = tc.Value;
        AssertResult(tc, () => Guard.Against.True(value));
    }
}
