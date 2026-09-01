using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;
using PineGuard.Common;

namespace PineGuard.AspNetCore;

/// <summary>
/// The MVC filter that validates every action argument before the action runs, and answers a failing
/// request with the same body the Minimal API filter returns.
/// </summary>
/// <remarks>
/// Registered by <see cref="MvcBuilderExtension.AddMustValidation"/>. Failures are written to
/// <see cref="ModelStateDictionary"/> as well as to the response, so a view or an existing
/// <c>ModelState.IsValid</c> check still sees them.
/// </remarks>
/// <seealso cref="MvcBuilderExtension"/>
/// <seealso cref="MustValidationEndpointFilter"/>
public sealed class MustValidationActionFilter : IAsyncActionFilter
{
    private readonly MustValidationOptions _options;
    private readonly IMustFailureMessageResolver _resolver;

    /// <summary>
    /// Initializes a new instance of the <see cref="MustValidationActionFilter"/> class.
    /// </summary>
    /// <param name="options">The application's validation options, read once.</param>
    /// <param name="resolver">The resolver producing each failure's message.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="options"/> or <paramref name="resolver"/> is <see langword="null"/>.</exception>
    public MustValidationActionFilter(IOptions<MustValidationOptions> options, IMustFailureMessageResolver resolver)
    {
        ThrowHelper.ThrowIfNull(options);
        ThrowHelper.ThrowIfNull(resolver);

        _options = options.Value;
        _resolver = resolver;
    }

    /// <summary>
    /// Validates <paramref name="context"/>'s action arguments and either invokes <paramref name="next"/> or
    /// short-circuits with a 400.
    /// </summary>
    /// <param name="context">The action about to execute, carrying the bound arguments.</param>
    /// <param name="next">The rest of the pipeline, invoked only when every argument is valid.</param>
    /// <returns>A task that completes once the action — or the short-circuit response — has been decided.</returns>
    /// <remarks>
    /// Model-binding errors MVC found for itself are left alone: this filter only adds what PineGuard's
    /// validators found, using the same error keys the response body publishes.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="context"/> or <paramref name="next"/> is <see langword="null"/>.</exception>
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        ThrowHelper.ThrowIfNull(context);
        ThrowHelper.ThrowIfNull(next);

        var httpContext = context.HttpContext;
        var result = await MustValidationRunner.ValidateAsync(context.ActionArguments.Values, httpContext, _options.Mode).ConfigureAwait(false);

        if (result.Success)
        {
            await next().ConfigureAwait(false);
            return;
        }

        var namingPolicy = ProblemDetailsExtension.ResolveNamingPolicy(httpContext, _options);
        var problemDetails = result.ToValidationProblemDetails(_options, namingPolicy, _resolver, httpContext);

        AddModelErrors(context.ModelState, problemDetails);

        context.Result = new BadRequestObjectResult(problemDetails);
    }

    private static void AddModelErrors(ModelStateDictionary modelState, ValidationProblemDetails problemDetails)
    {
        foreach (var error in problemDetails.Errors)
        {
            foreach (var message in error.Value)
                modelState.AddModelError(error.Key, message);
        }
    }
}
