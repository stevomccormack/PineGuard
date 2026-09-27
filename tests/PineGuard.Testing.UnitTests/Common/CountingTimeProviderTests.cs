using PineGuard.Testing.Common;
using Xunit.Abstractions;

namespace PineGuard.Testing.UnitTests.Common;

public sealed class CountingTimeProviderTests(ITestOutputHelper output)
    : BaseUnitTest(output)
{
    [Theory]
    [MemberData(nameof(CountingTimeProviderTestData.GetUtcNow.ValidCases), MemberType = typeof(CountingTimeProviderTestData.GetUtcNow))]
    public void GetUtcNow_BehavesAsExpected(CountingTimeProviderTestData.GetUtcNow.Case tc)
    {
        // Arrange
        var provider = new CountingTimeProvider(tc.Value);
        Assert.Equal(0, provider.UtcNowCallCount);

        // Act
        var first = provider.GetUtcNow();
        Assert.Equal(1, provider.UtcNowCallCount);
        var second = provider.GetUtcNow();
        var third = provider.GetUtcNow();

        // Assert
        Assert.Equal(tc.Expected, first);
        Assert.Equal(tc.Expected, second);
        Assert.Equal(tc.Expected, third);
        Assert.Equal(TimeSpan.Zero, first.Offset);
        Assert.Equal(3, provider.UtcNowCallCount);
    }

    [Theory]
    [MemberData(nameof(CountingTimeProviderTestData.LocalTimeZone.ValidCases), MemberType = typeof(CountingTimeProviderTestData.LocalTimeZone))]
    public void LocalTimeZone_BehavesAsExpected(CountingTimeProviderTestData.LocalTimeZone.Case tc)
    {
        // Act
        var result = tc.Value.LocalTimeZone;

        // Assert
        Assert.Same(tc.Expected, result);
    }
}
