using PineGuard.Externals.Iso.Payments;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Externals.Iso.Payments;

public sealed class PanAlgorithmTests : BaseUnitTest
{
    [Theory]
    [MemberData(nameof(PanAlgorithmTestData.IsValid.ValidCases), MemberType = typeof(PanAlgorithmTestData.IsValid))]
    [MemberData(nameof(PanAlgorithmTestData.IsValid.EdgeCases), MemberType = typeof(PanAlgorithmTestData.IsValid))]
    public void IsValid_ReturnsExpected(PanAlgorithmTestData.IsValid.ValidCase testCase)
    {
        // Act
        var result = PanAlgorithm.IsValid(testCase.Value);

        // Assert
        Assert.Equal(testCase.ExpectedReturn, result);
    }
}
