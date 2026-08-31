using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using PineGuard.AspNetCore.UnitTests.Samples;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;
using Xunit.Abstractions;

namespace PineGuard.AspNetCore.UnitTests;

public sealed class MustValidationActionFilterTests(ITestOutputHelper output)
    : BaseUnitTest(output)
{
    [Theory]
    [MemberData(nameof(MustValidationActionFilterTestData.Constructor.InvalidCases), MemberType = typeof(MustValidationActionFilterTestData.Constructor))]
    public void Constructor_ThrowsAsExpected(IThrowsCase tc)
    {
        // Arrange
        var action = ((ValueCase<Action>)tc).Value;

        // Act & Assert
        var ex = Assert.Throws(tc.ExpectedException.Type, action);
        ThrowsCaseAssert.Expected(ex, tc);
    }

    [Theory]
    [MemberData(nameof(MustValidationActionFilterTestData.OnActionExecutionAsync.Cases), MemberType = typeof(MustValidationActionFilterTestData.OnActionExecutionAsync))]
    public async Task OnActionExecutionAsync_BehavesAsExpected(MustValidationActionFilterTestData.OnActionExecutionAsync.Case tc)
    {
        // Arrange
        var (actionArguments, configureServices) = tc.Value;
        await using var provider = SampleServices.Build(configureServices);
        var httpContext = new DefaultHttpContext { RequestServices = provider };
        var context = SampleActions.Executing(httpContext, actionArguments);
        var filter = new MustValidationActionFilter(provider.GetRequiredService<IOptions<MustValidationOptions>>(), provider.GetRequiredService<IMustFailureMessageResolver>());

        var actionInvoked = false;

        Task<ActionExecutedContext> Next()
        {
            actionInvoked = true;
            return Task.FromResult(SampleActions.Executed(context));
        }

        // Act
        await filter.OnActionExecutionAsync(context, Next);

        // Assert
        Assert.Equal(tc.Expected.IsValid, actionInvoked);

        if (tc.Expected.IsValid)
        {
            Assert.Null(context.Result);
            Assert.True(context.ModelState.IsValid);
            return;
        }

        var badRequest = Assert.IsType<BadRequestObjectResult>(context.Result);
        Assert.Equal(StatusCodes.Status400BadRequest, badRequest.StatusCode);

        var problemDetails = Assert.IsType<ValidationProblemDetails>(badRequest.Value);
        ProblemDetailsAssert.Expected(tc.Expected, problemDetails);
        AssertModelState(context.ModelState, problemDetails);
    }

    [Theory]
    [MemberData(nameof(MustValidationActionFilterTestData.OnActionExecutionAsync.InvalidCases), MemberType = typeof(MustValidationActionFilterTestData.OnActionExecutionAsync))]
    public async Task OnActionExecutionAsync_ThrowsAsExpected(IThrowsCase tc)
    {
        // Arrange
        var action = ((ValueCase<Func<Task>>)tc).Value;

        // Act & Assert
        var ex = await Assert.ThrowsAsync(tc.ExpectedException.Type, action);
        ThrowsCaseAssert.Expected(ex, tc);
    }

    /// <summary>
    /// Asserts the model state publishes exactly what the response body does — the same keys, and the same
    /// messages under each of them.
    /// </summary>
    private static void AssertModelState(ModelStateDictionary modelState, ValidationProblemDetails problemDetails)
    {
        Assert.False(modelState.IsValid);
        Assert.Equal(problemDetails.Errors.Values.Sum(messages => messages.Length), modelState.ErrorCount);

        foreach (var error in problemDetails.Errors)
        {
            var entry = modelState[error.Key];

            Assert.NotNull(entry);
            Assert.Equal(error.Value, entry.Errors.Select(modelError => modelError.ErrorMessage));
        }
    }
}
