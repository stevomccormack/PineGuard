using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using PineGuard.MustClauses;

namespace PineGuard.AspNetCore.UnitTests.Samples;

/// <summary>
/// The application of Plan 03's stories 5 and 6 — handlers that throw rather than filters that validate,
/// answered by <c>app.UseExceptionHandler()</c> and the registered
/// <see cref="MustValidationExceptionHandler"/>.
/// </summary>
/// <remarks>
/// This is where the boundary policy is actually visible: a <see cref="MustValidationException"/> is a bad
/// request because the code that threw it meant "this request is invalid", while a guard's
/// <see cref="ArgumentException"/> is a 500 because it usually means a bug three layers down. The two are
/// mapped by exception type, not by heuristics, so the only way to prove it is to throw both through a real
/// pipeline and read the two statuses.
/// <para>
/// <c>AddProblemDetails</c> is what answers everything PineGuard declines: without it the exception-handler
/// middleware has no fallback and rethrows, which would make a declined exception indistinguishable from a
/// handler that crashed.
/// </para>
/// </remarks>
/// <seealso cref="MustValidationExceptionHandler"/>
public static class SampleBoundaryApi
{
    /// <summary>
    /// Registers request validation, the boundary exception handler and the framework fallback.
    /// </summary>
    /// <param name="services">The application's service collection.</param>
    /// <param name="handleGuardExceptions">Story 6's opt-in: whether the argument-exception family becomes a 400.</param>
    public static void ConfigureServices(IServiceCollection services, bool handleGuardExceptions)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddProblemDetails();
        services.AddMustValidation(options => options.HandleGuardExceptions = handleGuardExceptions);
    }

    /// <summary>
    /// Installs the exception-handler middleware and maps one endpoint per exception the policy separates.
    /// </summary>
    /// <param name="app">The application to configure.</param>
    public static void Map(WebApplication app)
    {
        ArgumentNullException.ThrowIfNull(app);

        app.UseExceptionHandler();

        app.MapGet("/boundary/validation", ThrowValidation);
        app.MapGet("/boundary/guard", ThrowGuard);
        app.MapGet("/boundary/unrelated", ThrowUnrelated);
    }

    /// <summary>
    /// Story 5: a handler that validated for itself and threw, reporting the story-2 failures.
    /// </summary>
    private static IResult ThrowValidation() =>
        throw new MustValidationException(MustValidationResult.Fail(SampleFailures.Email, SampleFailures.LineSku));

    /// <summary>
    /// Story 6: a guard, which is a 500 until the application opts in.
    /// </summary>
    private static IResult ThrowGuard() => throw new ArgumentNullException("email");

    /// <summary>
    /// Anything else, which PineGuard never claims.
    /// </summary>
    private static IResult ThrowUnrelated() => throw new InvalidOperationException("the order store is offline");
}
