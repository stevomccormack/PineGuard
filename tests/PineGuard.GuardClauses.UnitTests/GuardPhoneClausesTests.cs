using PineGuard.Testing.UnitTests.GuardClauses;
using Xunit.Abstractions;

namespace PineGuard.GuardClauses.UnitTests;

public sealed class GuardPhoneClausesTests(ITestOutputHelper output) : BaseGuardUnitTest(output)
{
    // Guard.Against.NotPhoneNumber
    [Theory]
    [MemberData(nameof(GuardPhoneClausesTestData.NotPhoneNumber.ValidCases), MemberType = typeof(GuardPhoneClausesTestData.NotPhoneNumber))]
    [MemberData(nameof(GuardPhoneClausesTestData.NotPhoneNumber.InvalidCases), MemberType = typeof(GuardPhoneClausesTestData.NotPhoneNumber))]
    public void NotPhoneNumber_BehavesAsExpected(GuardCase<string?> tc)
    {
        // Act + Assert
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.NotPhoneNumber(value));
        AssertCustomMessage(tc, () => Guard.Against.NotPhoneNumber(value, message: CustomMessage));

        if (tc.Expected.IsValid) Assert.NotNull(result);
    }

    // Guard.Against.NotPhoneNumberString
    [Theory]
    [MemberData(nameof(GuardPhoneClausesTestData.NotPhoneNumberString.ValidCases), MemberType = typeof(GuardPhoneClausesTestData.NotPhoneNumberString))]
    [MemberData(nameof(GuardPhoneClausesTestData.NotPhoneNumberString.InvalidCases), MemberType = typeof(GuardPhoneClausesTestData.NotPhoneNumberString))]
    public void NotPhoneNumberString_BehavesAsExpected(GuardCase<string?> tc)
    {
        // Act + Assert
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.NotPhoneNumberString(value));
        AssertCustomMessage(tc, () => Guard.Against.NotPhoneNumberString(value, message: CustomMessage));

        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }
}
