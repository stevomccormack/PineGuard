using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using PineGuard.Common;

namespace PineGuard.AspNetCore;

/// <summary>
/// The Minimal API filter that validates every bound argument before the endpoint's handler runs, and
/// answers a failing request with the RFC 9457 body instead of invoking the handler.
/// </summary>
/// <remarks>
/// Add it with <see cref="EndpointConventionBuilderExtension.AddMustValidation{TBuilder}"/> rather than
/// directly: that overload only attaches the filter to endpoints that actually have a validator to run, so
/// the rest of the application pays nothing.
/// </remarks>
/// <seealso cref="EndpointConventionBuilderExtension"/>
/// <seealso cref="MustValidationActionFilter"/>
public sealed class MustValidationEndpointFilter : IEndpointFilter
{
    /// <summary>
    /// Validates <paramref name="context"/>'s arguments and either invokes <paramref name="next"/> or
    /// short-circuits with a 400.
    /// </summary>
    /// <param name="context">The invocation being filtered, carrying the bound arguments.</param>
    /// <param name="next">The rest of the pipeline, invoked only when every argument is valid.</param>
    /// <returns>The handler's own result when validation succeeds; otherwise a <see cref="ProblemDetails"/> result with status 400.</returns>
    /// <remarks>
    /// Validators are resolved from the request's services — a validator that depends on a scoped service is
    /// itself scoped — and observe <see cref="HttpContext.RequestAborted"/>, so an abandoned request stops
    /// paying for validation it will never answer.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="context"/> or <paramref name="next"/> is <see langword="null"/>.</exception>
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        ThrowHelper.ThrowIfNull(context);
        ThrowHelper.ThrowIfNull(next);

        var httpContext = context.HttpContext;
        var options = httpContext.RequestServices.GetRequiredService<IOptions<MustValidationOptions>>().Value;

        var result = await MustValidationRunner.ValidateAsync(context.Arguments, httpContext, options.Mode).ConfigureAwait(false);

        if (result.Success)
            return await next(context).ConfigureAwait(false);

        var resolver = httpContext.RequestServices.GetRequiredService<IMustFailureMessageResolver>();
        var namingPolicy = ProblemDetailsExtension.ResolveNamingPolicy(httpContext, options);

        return TypedResults.Problem(result.ToValidationProblemDetails(options, namingPolicy, resolver, httpContext));
    }
}
