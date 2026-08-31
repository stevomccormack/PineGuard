using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PineGuard.AspNetCore.UnitTests.Samples;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;
using Xunit.Abstractions;

namespace PineGuard.AspNetCore.UnitTests;

public sealed class MustValidationEndpointFilterTests(ITestOutputHelper output)
    : BaseUnitTest(output)
{
    private static readonly object HandlerResult = new();

    [Theory]
    [MemberData(nameof(MustValidationEndpointFilterTestData.InvokeAsync.Cases), MemberType = typeof(MustValidationEndpointFilterTestData.InvokeAsync))]
    public async Task InvokeAsync_BehavesAsExpected(MustValidationEndpointFilterTestData.InvokeAsync.Case tc)
    {
        // Arrange
        var (arguments, configureServices) = tc.Value;
        await using var provider = SampleServices.Build(configureServices);
        using var cancellation = new CancellationTokenSource();
        var httpContext = new DefaultHttpContext { RequestServices = provider, RequestAborted = cancellation.Token };
        var context = new DefaultEndpointFilterInvocationContext(httpContext, arguments);

        var handlerInvoked = false;

        ValueTask<object?> Next(EndpointFilterInvocationContext _)
        {
            handlerInvoked = true;
            return ValueTask.FromResult<object?>(HandlerResult);
        }

        // Act
        var result = await new MustValidationEndpointFilter().InvokeAsync(context, Next);

        // Assert
        Assert.Equal(tc.Expected.IsValid, handlerInvoked);

        foreach (var probe in arguments.OfType<TokenProbe>())
            Assert.Equal(cancellation.Token, probe.ObservedToken);

        if (tc.Expected.IsValid)
        {
            Assert.Same(HandlerResult, result);
            return;
        }

        var problem = Assert.IsType<ProblemHttpResult>(result);
        Assert.Equal(StatusCodes.Status400BadRequest, problem.StatusCode);
        ProblemDetailsAssert.Expected(tc.Expected, Assert.IsType<ValidationProblemDetails>(problem.ProblemDetails));
    }

    [Theory]
    [MemberData(nameof(MustValidationEndpointFilterTestData.InvokeAsync.InvalidCases), MemberType = typeof(MustValidationEndpointFilterTestData.InvokeAsync))]
    public async Task InvokeAsync_ThrowsAsExpected(IThrowsCase tc)
    {
        // Arrange
        var action = ((ValueCase<Func<Task>>)tc).Value;

        // Act & Assert
        var ex = await Assert.ThrowsAsync(tc.ExpectedException.Type, action);
        ThrowsCaseAssert.Expected(ex, tc);
    }
}
