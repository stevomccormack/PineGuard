using PineGuard.Testing.UnitTests.GuardClauses;
using Xunit.Abstractions;

namespace PineGuard.GuardClauses.UnitTests;

public sealed class GuardNullClausesTests(ITestOutputHelper output) : BaseGuardUnitTest(output)
{
    [Theory]
    [MemberData(nameof(GuardNullClausesTestData.NotNull.ValidCases), MemberType = typeof(GuardNullClausesTestData.NotNull))]
    [MemberData(nameof(GuardNullClausesTestData.NotNull.InvalidCases), MemberType = typeof(GuardNullClausesTestData.NotNull))]
    public void NotNull_BehavesAsExpected(GuardCase<object?> tc)
    {
        var value = tc.Value;
        AssertResult(tc, () => Guard.Against.NotNull(value));
    }

    [Theory]
    [MemberData(nameof(GuardNullClausesTestData.Null.ValidCases), MemberType = typeof(GuardNullClausesTestData.Null))]
    [MemberData(nameof(GuardNullClausesTestData.Null.InvalidCases), MemberType = typeof(GuardNullClausesTestData.Null))]
    public void Null_BehavesAsExpected(GuardCase<object?> tc)
    {
        var value = tc.Value;
        AssertResult(tc, () => Guard.Against.Null(value));
    }
}
