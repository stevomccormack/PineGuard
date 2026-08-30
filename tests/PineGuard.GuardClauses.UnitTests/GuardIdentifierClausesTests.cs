using PineGuard.Testing.UnitTests.GuardClauses;
using Xunit.Abstractions;

namespace PineGuard.GuardClauses.UnitTests;

public sealed class GuardIdentifierClausesTests(ITestOutputHelper output) : BaseGuardUnitTest(output)
{
    [Theory]
    [MemberData(nameof(GuardIdentifierClausesTestData.NotSlug.ValidCases), MemberType = typeof(GuardIdentifierClausesTestData.NotSlug))]
    [MemberData(nameof(GuardIdentifierClausesTestData.NotSlug.InvalidCases), MemberType = typeof(GuardIdentifierClausesTestData.NotSlug))]
    public void NotSlug_BehavesAsExpected(GuardCase<string?> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.NotSlug(value));
        AssertCustomMessage(tc, () => Guard.Against.NotSlug(value, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    // Guard.Against.NotUlid
    [Theory]
    [MemberData(nameof(GuardIdentifierClausesTestData.NotUlid.ValidCases), MemberType = typeof(GuardIdentifierClausesTestData.NotUlid))]
    [MemberData(nameof(GuardIdentifierClausesTestData.NotUlid.InvalidCases), MemberType = typeof(GuardIdentifierClausesTestData.NotUlid))]
    public void NotUlid_BehavesAsExpected(GuardCase<string?> tc)
    {
        // Arrange
        var value = tc.Value;

        // Act + Assert
        var result = AssertResult(tc, () => Guard.Against.NotUlid(value));
        AssertCustomMessage(tc, () => Guard.Against.NotUlid(value, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }
}
