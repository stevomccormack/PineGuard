using PineGuard.Externals.Iso.Payments;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Externals.Iso.Payments;

public sealed class LuhnAlgorithmTests : BaseUnitTest
{
    [Theory]
    [MemberData(nameof(LuhnAlgorithmTestData.IsValid.ValidCases), MemberType = typeof(LuhnAlgorithmTestData.IsValid))]
    [MemberData(nameof(LuhnAlgorithmTestData.IsValid.EdgeCases), MemberType = typeof(LuhnAlgorithmTestData.IsValid))]
    public void IsValid_ReturnsExpected(LuhnAlgorithmTestData.IsValid.ValidCase testCase)
    {
        // Act
        var result = LuhnAlgorithm.IsValid(testCase.Value);

        // Assert
        Assert.Equal(testCase.ExpectedReturn, result);
    }
}
