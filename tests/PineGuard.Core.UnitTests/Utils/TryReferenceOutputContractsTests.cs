using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Utils;

public sealed class TryReferenceOutputContractsTests : BaseUnitTest
{
    [Theory]
    [MemberData(nameof(TryReferenceOutputContractsTestData.Cases), MemberType = typeof(TryReferenceOutputContractsTestData))]
    public void SuccessfulOutput_CanBeConsumedWithoutNullSuppression(TryReferenceOutputContractsTestData.OutputCase testCase)
    {
        // Act
        var actual = testCase.Value();

        // Assert
        Assert.Equal(testCase.Expected, actual);
    }
}
