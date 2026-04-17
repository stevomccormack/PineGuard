using PineGuard.Testing.UnitTests.MustClauses;
using Xunit.Abstractions;

namespace PineGuard.MustClauses.UnitTests;

public sealed class MustPhoneClausesTests(ITestOutputHelper output) : BaseMustUnitTest(output)
{
    [Theory]
    [MemberData(nameof(MustPhoneClausesTestData.PhoneNumber.ValidCases), MemberType = typeof(MustPhoneClausesTestData.PhoneNumber))]
    [MemberData(nameof(MustPhoneClausesTestData.PhoneNumber.InvalidCases), MemberType = typeof(MustPhoneClausesTestData.PhoneNumber))]
    public void PhoneNumber_BehavesAsExpected(MustCase<string?> tc)
    {
        // Act
        var result = Must.Be.PhoneNumber(tc.Value, paramName: "value");

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustPhoneClausesTestData.PhoneNumberString.ValidCases), MemberType = typeof(MustPhoneClausesTestData.PhoneNumberString))]
    [MemberData(nameof(MustPhoneClausesTestData.PhoneNumberString.InvalidCases), MemberType = typeof(MustPhoneClausesTestData.PhoneNumberString))]
    public void PhoneNumberString_BehavesAsExpected(MustCase<string?> tc)
    {
        // Act
        var result = Must.Be.PhoneNumberString(tc.Value, paramName: "value");

        // Assert
        AssertResult(tc, result);
    }
}
