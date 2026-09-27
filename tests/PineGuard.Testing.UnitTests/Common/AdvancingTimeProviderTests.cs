using PineGuard.Testing.Common;
using Xunit.Abstractions;

namespace PineGuard.Testing.UnitTests.Common;

public sealed class AdvancingTimeProviderTests(ITestOutputHelper output)
    : BaseUnitTest(output)
{
    [Theory]
    [MemberData(nameof(AdvancingTimeProviderTestData.GetUtcNow.ValidCases), MemberType = typeof(AdvancingTimeProviderTestData.GetUtcNow))]
    public void GetUtcNow_BehavesAsExpected(AdvancingTimeProviderTestData.GetUtcNow.Case tc)
    {
        // Arrange
        var provider = new AdvancingTimeProvider(tc.Value);

        // Act
        var results = new DateTimeOffset[tc.Expected.Length];
        for (var i = 0; i < tc.Expected.Length; i++)
            results[i] = provider.GetUtcNow();

        // Assert
        Assert.Equal(tc.Expected, results);
        Assert.Equal(tc.Expected.Length, provider.UtcNowCallCount);
        Assert.All(results, result => Assert.Equal(TimeSpan.Zero, result.Offset));
    }

    [Theory]
    [MemberData(nameof(AdvancingTimeProviderTestData.GetUtcNow.InvalidCases), MemberType = typeof(AdvancingTimeProviderTestData.GetUtcNow))]
    public void GetUtcNow_ThrowsAsExpected(IThrowsCase tc)
    {
        // Arrange
        var t = (AdvancingTimeProviderTestData.GetUtcNow.InvalidCase)tc;
        var (sequence, successfulReads) = t.Value;
        var provider = new AdvancingTimeProvider(sequence);

        for (var i = 0; i < successfulReads; i++)
            provider.GetUtcNow();

        // Act & Assert
        var ex = Assert.Throws(t.ExpectedException.Type, () => provider.GetUtcNow());
        ThrowsCaseAssert.Expected(ex, tc);
        Assert.Equal(successfulReads + 1, provider.UtcNowCallCount);
    }

    [Theory]
    [MemberData(nameof(AdvancingTimeProviderTestData.LocalTimeZone.ValidCases), MemberType = typeof(AdvancingTimeProviderTestData.LocalTimeZone))]
    public void LocalTimeZone_BehavesAsExpected(AdvancingTimeProviderTestData.LocalTimeZone.Case tc)
    {
        // Act
        var result = tc.Value.LocalTimeZone;

        // Assert
        Assert.Same(tc.Expected, result);
    }
}
