using PineGuard.Testing.Common;
using Xunit.Abstractions;

namespace PineGuard.Testing.UnitTests.Common;

public sealed class FixedTimeProviderTests(ITestOutputHelper output)
    : BaseUnitTest(output)
{
    [Theory]
    [MemberData(nameof(FixedTimeProviderTestData.GetUtcNow.ValidCases), MemberType = typeof(FixedTimeProviderTestData.GetUtcNow))]
    public void GetUtcNow_BehavesAsExpected(FixedTimeProviderTestData.GetUtcNow.Case tc)
    {
        // Arrange
        var provider = new FixedTimeProvider(tc.Value);

        // Act
        var first = provider.GetUtcNow();
        var second = provider.GetUtcNow();

        // Assert
        Assert.Equal(tc.Expected, first);
        Assert.Equal(tc.Expected, second);
    }

    [Theory]
    [MemberData(nameof(FixedTimeProviderTestData.Default.ValidCases), MemberType = typeof(FixedTimeProviderTestData.Default))]
    public void Default_BehavesAsExpected(FixedTimeProviderTestData.Default.Case tc)
    {
        // Act
        var result = tc.Value.GetUtcNow();

        // Assert
        Assert.Equal(tc.Expected, result);
    }
}
