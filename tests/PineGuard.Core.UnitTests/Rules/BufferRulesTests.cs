using PineGuard.Rules;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Rules;

public sealed class BufferRulesTests : BaseUnitTest
{
    [Fact]
    public void Constants_AreExpected()
    {
        // Act

        // Assert
        Assert.Equal(4, BufferRules.Base64CharsPerQuantum);
        Assert.Equal(3, BufferRules.Base64BytesPerQuantum);
    }

    [Theory]
    [MemberData(nameof(BufferRulesTestData.IsHex.ValidCases), MemberType = typeof(BufferRulesTestData.IsHex))]
    [MemberData(nameof(BufferRulesTestData.IsHex.EdgeCases), MemberType = typeof(BufferRulesTestData.IsHex))]
    public void IsHex_ReturnsExpected(Utils.BufferUtilityTestData.IsHexString.Case testCase)
    {
        // Act
        var result = BufferRules.IsHex(testCase.Value);

        // Assert
        Assert.Equal(testCase.ExpectedReturn, result);
    }

    [Theory]
    [MemberData(nameof(BufferRulesTestData.IsBase64.ValidCases), MemberType = typeof(BufferRulesTestData.IsBase64))]
    [MemberData(nameof(BufferRulesTestData.IsBase64.EdgeCases), MemberType = typeof(BufferRulesTestData.IsBase64))]
    public void IsBase64_ReturnsExpected(Utils.BufferUtilityTestData.IsBase64String.Case testCase)
    {
        // Act
        var result = BufferRules.IsBase64(testCase.Value);

        // Assert
        Assert.Equal(testCase.ExpectedReturn, result);
    }
}
