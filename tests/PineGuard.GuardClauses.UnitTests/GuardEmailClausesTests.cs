using PineGuard.Testing.UnitTests.GuardClauses;
using Xunit.Abstractions;

namespace PineGuard.GuardClauses.UnitTests;

public sealed class GuardEmailClausesTests(ITestOutputHelper output) : BaseGuardUnitTest(output)
{
    [Theory]
    [MemberData(nameof(GuardEmailClausesTestData.NotEmail.ValidCases), MemberType = typeof(GuardEmailClausesTestData.NotEmail))]
    [MemberData(nameof(GuardEmailClausesTestData.NotEmail.InvalidCases), MemberType = typeof(GuardEmailClausesTestData.NotEmail))]
    public void NotEmail_BehavesAsExpected(GuardCase<string?> tc)
    {
        // Act + Assert
        var result = AssertResult(tc, () => Guard.Against.NotEmail(tc.Value!, paramName: "value"));
        AssertCustomMessage(tc, () => Guard.Against.NotEmail(tc.Value!, paramName: "value", message: CustomMessage));

        if (tc.Expected.IsValid) Assert.Equal(tc.Value, result);
    }

    [Theory]
    [MemberData(nameof(GuardEmailClausesTestData.NotStrictEmail.ValidCases), MemberType = typeof(GuardEmailClausesTestData.NotStrictEmail))]
    [MemberData(nameof(GuardEmailClausesTestData.NotStrictEmail.InvalidCases), MemberType = typeof(GuardEmailClausesTestData.NotStrictEmail))]
    public void NotStrictEmail_BehavesAsExpected(GuardCase<string?> tc)
    {
        // Act + Assert
        var result = AssertResult(tc, () => Guard.Against.NotStrictEmail(tc.Value!, paramName: "value"));
        AssertCustomMessage(tc, () => Guard.Against.NotStrictEmail(tc.Value!, paramName: "value", message: CustomMessage));

        if (tc.Expected.IsValid) Assert.Equal(tc.Value, result);
    }

    [Theory]
    [MemberData(nameof(GuardEmailClausesTestData.NotHasEmailAlias.ValidCases), MemberType = typeof(GuardEmailClausesTestData.NotHasEmailAlias))]
    [MemberData(nameof(GuardEmailClausesTestData.NotHasEmailAlias.InvalidCases), MemberType = typeof(GuardEmailClausesTestData.NotHasEmailAlias))]
    public void NotHasEmailAlias_BehavesAsExpected(GuardCase<string?> tc)
    {
        // Act + Assert
        var result = AssertResult(tc, () => Guard.Against.NotHasEmailAlias(tc.Value!, paramName: "value"));
        AssertCustomMessage(tc, () => Guard.Against.NotHasEmailAlias(tc.Value!, paramName: "value", message: CustomMessage));

        if (tc.Expected.IsValid) Assert.Equal(tc.Value, result);
    }

    [Theory]
    [MemberData(nameof(GuardEmailClausesTestData.HasEmailAlias.ValidCases), MemberType = typeof(GuardEmailClausesTestData.HasEmailAlias))]
    [MemberData(nameof(GuardEmailClausesTestData.HasEmailAlias.InvalidCases), MemberType = typeof(GuardEmailClausesTestData.HasEmailAlias))]
    public void HasEmailAlias_BehavesAsExpected(GuardCase<string?> tc)
    {
        // Act + Assert
        var result = AssertResult(tc, () => Guard.Against.HasEmailAlias(tc.Value!, paramName: "value"));
        AssertCustomMessage(tc, () => Guard.Against.HasEmailAlias(tc.Value!, paramName: "value", message: CustomMessage));

        if (tc.Expected.IsValid) Assert.Equal(tc.Value, result);
    }
}
