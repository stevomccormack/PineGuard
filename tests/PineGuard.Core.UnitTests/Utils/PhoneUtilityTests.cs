using PineGuard.Testing.UnitTests;
using PineGuard.Utils;

namespace PineGuard.Core.UnitTests.Utils;

public sealed class PhoneUtilityTests : BaseUnitTest
{
    [Theory]
    [MemberData(nameof(PhoneUtilityTestData.TryParsePhone.ValidCases), MemberType = typeof(PhoneUtilityTestData.TryParsePhone))]
    [MemberData(nameof(PhoneUtilityTestData.TryParsePhone.EdgeCases), MemberType = typeof(PhoneUtilityTestData.TryParsePhone))]
    public void TryParsePhone_ReturnsExpected(PhoneUtilityTestData.TryParsePhone.ValidCase testCase)
    {
        // Arrange
        var (value, min, max, allowed) = testCase.Value;

        // Use optional parameters if null/defaults in tuple?
        // Actually the utility has defaults: min=7, max=15, allowed=null.
        // Test cases must specify explicit logic, or we overload the call.
        // To cover usage, we can call with all args.

        // Act
        var result = PhoneUtility.TryParsePhone(value, out var digits, min, max, allowed);

        // Assert
        Assert.Equal(testCase.Expected, result);
        Assert.Equal(testCase.ExpectedOutValue, digits);
    }
}
