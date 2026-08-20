using PineGuard.Testing.UnitTests.GuardClauses;
using Xunit.Abstractions;

namespace PineGuard.GuardClauses.UnitTests;

public sealed class GuardStringGuidClausesTests(ITestOutputHelper output) : BaseGuardUnitTest(output)
{
    [Theory]
    [MemberData(nameof(GuardStringGuidClausesTestData.NotGuid.ValidCases), MemberType = typeof(GuardStringGuidClausesTestData.NotGuid))]
    [MemberData(nameof(GuardStringGuidClausesTestData.NotGuid.InvalidCases), MemberType = typeof(GuardStringGuidClausesTestData.NotGuid))]
    public void NotGuid_BehavesAsExpected(GuardCase<string?> tc)
    {
        var value = tc.Value;
        AssertResult(tc, () => Guard.Against.NotGuid(value));
        AssertCustomMessage(tc, () => Guard.Against.NotGuid(value, message: CustomMessage));
    }

    [Theory]
    [MemberData(nameof(GuardStringGuidClausesTestData.EmptyGuid.ValidCases), MemberType = typeof(GuardStringGuidClausesTestData.EmptyGuid))]
    [MemberData(nameof(GuardStringGuidClausesTestData.EmptyGuid.InvalidCases), MemberType = typeof(GuardStringGuidClausesTestData.EmptyGuid))]
    public void EmptyGuid_BehavesAsExpected(GuardCase<string?> tc)
    {
        var value = tc.Value;
        AssertResult(tc, () => Guard.Against.EmptyGuid(value));
        AssertCustomMessage(tc, () => Guard.Against.EmptyGuid(value, message: CustomMessage));
    }
}
