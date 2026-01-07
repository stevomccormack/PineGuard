using PineGuard.Rules;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Rules;

public sealed class PhoneRulesTests : BaseUnitTest
{
    [Theory]
    [MemberData(nameof(PhoneRulesTestData.IsPhoneNumber.ValidCases), MemberType = typeof(PhoneRulesTestData.IsPhoneNumber))]
    [MemberData(nameof(PhoneRulesTestData.IsPhoneNumber.EdgeCases), MemberType = typeof(PhoneRulesTestData.IsPhoneNumber))]
    public void IsPhoneNumber_ReturnsExpected(PhoneRulesTestData.IsPhoneNumber.Case testCase)
    {
        var result = PhoneRules.IsPhoneNumber(testCase.Value);

        Assert.Equal(testCase.Expected, result);
    }

    [Fact]
    public void IsPhoneNumber_RespectsCustomAllowedCharacters()
    {
        var result = PhoneRules.IsPhoneNumber("425 555 0123", allowedNonDigitCharacters: []);

        Assert.False(result);
    }
}
