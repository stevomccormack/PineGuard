using PineGuard.Testing.UnitTests.GuardClauses;
using Xunit.Abstractions;

namespace PineGuard.GuardClauses.UnitTests;

public sealed class GuardGuidClausesTests(ITestOutputHelper output)
    : BaseGuardUnitTest(output)
{
    [Theory]
    [MemberData(nameof(GuardGuidClausesTestData.Empty.ValidCases), MemberType = typeof(GuardGuidClausesTestData.Empty))]
    [MemberData(nameof(GuardGuidClausesTestData.Empty.InvalidCases), MemberType = typeof(GuardGuidClausesTestData.Empty))]
    public void Empty_BehavesAsExpected(GuardCase<Guid> tc)
    {
        // Act
        var result = AssertResult(tc, () => Guard.Against.Empty(tc.Value, paramName: "value"));
        AssertCustomMessage(tc, () => Guard.Against.Empty(tc.Value, paramName: "value", message: CustomMessage));

        // Assert
        if (tc.Expected.IsValid)
            Assert.Equal(tc.Value, result);
    }
}
