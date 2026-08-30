using PineGuard.Testing.UnitTests;
using PineGuard.Utils;
using Xunit.Abstractions;

namespace PineGuard.Core.UnitTests.Utils;

public sealed class TokenUtilityTests(ITestOutputHelper output)
    : BaseUnitTest(output)
{
    [Theory]
    [MemberData(nameof(TokenUtilityTestData.TryParseJwt.ValidCases), MemberType = typeof(TokenUtilityTestData.TryParseJwt))]
    [MemberData(nameof(TokenUtilityTestData.TryParseJwt.EdgeCases), MemberType = typeof(TokenUtilityTestData.TryParseJwt))]
    public void TryParseJwt_ReturnsExpected(TokenUtilityTestData.TryParseJwt.ValidCase testCase)
    {
        // Arrange
        var (expectedResult, expectedHeader, expectedPayload, expectedSignature) = testCase.Expected;

        // Act
        var result = TokenUtility.TryParseJwt(testCase.Value, out var header, out var payload, out var signature);

        // Assert
        Assert.Equal(expectedResult, result);
        Assert.Equal(expectedHeader, header);
        Assert.Equal(expectedPayload, payload);
        Assert.Equal(expectedSignature, signature);
    }
}
