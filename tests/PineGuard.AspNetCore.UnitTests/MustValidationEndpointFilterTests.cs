using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.TestHost;
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

    [Theory]
    [MemberData(nameof(MustValidationEndpointFilterTestData.EndToEnd.Cases), MemberType = typeof(MustValidationEndpointFilterTestData.EndToEnd))]
    public async Task EndToEnd_BehavesAsExpected(MustValidationEndpointFilterTestData.EndToEnd.Case tc)
    {
        // Arrange
        var (method, requestUri, json) = tc.Value;
        await using var app = await SampleHost.StartAsync(SampleMinimalApi.ConfigureServices, SampleMinimalApi.Map);
        using var client = app.GetTestClient();
        using var request = SampleHost.Request(method, requestUri, json);

        // Act
        using var response = await client.SendAsync(request);

        // Assert
        Assert.Equal(tc.Expected.IsValid, response.IsSuccessStatusCode);
        Assert.Equal(tc.Expected.Status, (int)response.StatusCode);

        if (tc.Expected.Body is null)
            return;

        ProblemDetailsAssert.Expected(tc.Expected.Body, await SampleResponses.ReadJsonAsync(response));
    }
}
