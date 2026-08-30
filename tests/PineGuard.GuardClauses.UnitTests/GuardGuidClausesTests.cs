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

    // Guard.Against.NotHasGuidVersion
    [Theory]
    [MemberData(nameof(GuardGuidClausesTestData.NotHasGuidVersion.ValidCases), MemberType = typeof(GuardGuidClausesTestData.NotHasGuidVersion))]
    [MemberData(nameof(GuardGuidClausesTestData.NotHasGuidVersion.InvalidCases), MemberType = typeof(GuardGuidClausesTestData.NotHasGuidVersion))]
    public void NotHasGuidVersion_BehavesAsExpected(GuardCase<(Guid value, int version)> tc)
    {
        // Arrange
        var value = tc.Value.value;

        // Act + Assert
        var result = AssertResult(tc, () => Guard.Against.NotHasGuidVersion(value, tc.Value.version));
        AssertCustomMessage(tc, () => Guard.Against.NotHasGuidVersion(value, tc.Value.version, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }
}
