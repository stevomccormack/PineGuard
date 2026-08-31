using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using PineGuard.AspNetCore.UnitTests.Samples;
using PineGuard.MustClauses;
using PineGuard.Testing.UnitTests.MustClauses;
using Xunit.Abstractions;

namespace PineGuard.AspNetCore.UnitTests;

public sealed class MustValidationRunnerTests(ITestOutputHelper output)
    : BaseMustValidationUnitTest(output)
{
    [Theory]
    [MemberData(nameof(MustValidationRunnerTestData.ValidateAsync.Cases), MemberType = typeof(MustValidationRunnerTestData.ValidateAsync))]
    public async Task ValidateAsync_BehavesAsExpected(MustValidationCase<(object?[] arguments, Action<IServiceCollection> configureServices, MustValidationMode mode)> tc)
    {
        // Arrange
        var (arguments, configureServices, mode) = tc.Value;
        await using var provider = SampleServices.Build(configureServices);
        using var cancellation = new CancellationTokenSource();
        var httpContext = new DefaultHttpContext { RequestServices = provider, RequestAborted = cancellation.Token };

        // Act
        var result = await MustValidationRunner.ValidateAsync(arguments, httpContext, mode);

        // Assert
        AssertResult(tc, result);

        foreach (var probe in arguments.OfType<TokenProbe>())
            Assert.Equal(cancellation.Token, probe.ObservedToken);
    }
}
