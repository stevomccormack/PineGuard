using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using PineGuard.AspNetCore.UnitTests.Samples;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;
using Xunit.Abstractions;

namespace PineGuard.AspNetCore.UnitTests;

public sealed class MustValidationExceptionHandlerTests(ITestOutputHelper output)
    : BaseUnitTest(output)
{
    [Theory]
    [MemberData(nameof(MustValidationExceptionHandlerTestData.Constructor.InvalidCases), MemberType = typeof(MustValidationExceptionHandlerTestData.Constructor))]
    public void Constructor_ThrowsAsExpected(IThrowsCase tc)
    {
        // Arrange
        var action = ((ValueCase<Action>)tc).Value;

        // Act & Assert
        var ex = Assert.Throws(tc.ExpectedException.Type, action);
        ThrowsCaseAssert.Expected(ex, tc);
    }

    [Theory]
    [MemberData(nameof(MustValidationExceptionHandlerTestData.TryHandleAsync.Cases), MemberType = typeof(MustValidationExceptionHandlerTestData.TryHandleAsync))]
    public async Task TryHandleAsync_BehavesAsExpected(MustValidationExceptionHandlerTestData.TryHandleAsync.Case tc)
    {
        // Arrange
        var (exception, configureServices) = tc.Value;
        await using var provider = SampleServices.Build(configureServices);
        var httpContext = SampleResponses.Recording(provider);
        var handler = new MustValidationExceptionHandler(provider.GetRequiredService<IOptions<MustValidationOptions>>(), provider.GetRequiredService<IMustFailureMessageResolver>());

        // Act
        var handled = await handler.TryHandleAsync(httpContext, exception, CancellationToken.None);

        // Assert
        Assert.Equal(tc.Expected.IsValid, handled);

        if (!tc.Expected.IsValid)
        {
            Assert.Equal(StatusCodes.Status200OK, httpContext.Response.StatusCode);
            Assert.Equal(0, httpContext.Response.Body.Length);
            return;
        }

        Assert.Equal(StatusCodes.Status400BadRequest, httpContext.Response.StatusCode);

        var expectedBody = Assert.IsType<ProblemDetailsExpected>(tc.Expected.Body);
        ProblemDetailsAssert.Expected(expectedBody, await SampleResponses.ReadJsonAsync(httpContext));
    }

    [Theory]
    [MemberData(nameof(MustValidationExceptionHandlerTestData.TryHandleAsync.InvalidCases), MemberType = typeof(MustValidationExceptionHandlerTestData.TryHandleAsync))]
    public async Task TryHandleAsync_ThrowsAsExpected(IThrowsCase tc)
    {
        // Arrange
        var action = ((ValueCase<Func<Task>>)tc).Value;

        // Act & Assert
        var ex = await Assert.ThrowsAsync(tc.ExpectedException.Type, action);
        ThrowsCaseAssert.Expected(ex, tc);
    }
}
