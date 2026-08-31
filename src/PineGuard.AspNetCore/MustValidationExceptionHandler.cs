using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PineGuard.Common;
using PineGuard.GuardClauses;
using PineGuard.MustClauses;

namespace PineGuard.AspNetCore;

/// <summary>
/// Turns a validation failure thrown at the boundary into the same RFC 9457 body the filters return,
/// leaving every other exception to the rest of the pipeline.
/// </summary>
/// <remarks>
/// Only <see cref="MustValidationException"/> is a 400 by default, because only code that means "this
/// request is invalid" throws it — <c>MustValidationResult.ThrowIfFailed()</c> and the adapters built on it.
/// A guard's <see cref="ArgumentException"/> is a programmer error and stays a 500 unless
/// <see cref="MustValidationOptions.HandleGuardExceptions"/> is turned on. Registered by
/// <see cref="MustValidationServiceCollectionExtension.AddMustValidation(Microsoft.Extensions.DependencyInjection.IServiceCollection, System.Reflection.Assembly[])"/>;
/// the application still calls <c>app.UseExceptionHandler()</c> for it to run.
/// </remarks>
/// <seealso cref="MustValidationOptions"/>
/// <seealso cref="ProblemDetailsExtension"/>
public sealed class MustValidationExceptionHandler : IExceptionHandler
{
    private readonly MustValidationOptions _options;
    private readonly IMustFailureMessageResolver _resolver;

    /// <summary>
    /// Initializes a new instance of the <see cref="MustValidationExceptionHandler"/> class.
    /// </summary>
    /// <param name="options">The application's validation options, read once.</param>
    /// <param name="resolver">The resolver producing each failure's message.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="options"/> or <paramref name="resolver"/> is <see langword="null"/>.</exception>
    public MustValidationExceptionHandler(IOptions<MustValidationOptions> options, IMustFailureMessageResolver resolver)
    {
        ThrowHelper.ThrowIfNull(options);
        ThrowHelper.ThrowIfNull(resolver);

        _options = options.Value;
        _resolver = resolver;
    }

    /// <summary>
    /// Answers <paramref name="exception"/> with a 400 when it describes an invalid request.
    /// </summary>
    /// <param name="httpContext">The request being answered.</param>
    /// <param name="exception">The exception that reached the pipeline's end.</param>
    /// <param name="cancellationToken">Ignored; the response is written to <paramref name="httpContext"/>, which carries its own token.</param>
    /// <returns><see langword="true"/> when the response has been written; <see langword="false"/> to let the next handler try.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="httpContext"/> or <paramref name="exception"/> is <see langword="null"/>.</exception>
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        ThrowHelper.ThrowIfNull(httpContext);
        ThrowHelper.ThrowIfNull(exception);

        if (!TryGetResult(exception, out var result))
            return false;

        var namingPolicy = ProblemDetailsExtension.ResolveNamingPolicy(httpContext, _options);
        var problemDetails = result.ToValidationProblemDetails(_options, namingPolicy, _resolver, httpContext);

        await TypedResults.Problem(problemDetails).ExecuteAsync(httpContext).ConfigureAwait(false);

        return true;
    }

    /// <summary>
    /// Maps <paramref name="exception"/> to the failures it describes, if it describes any.
    /// </summary>
    /// <param name="exception">The exception to interpret.</param>
    /// <param name="result">When this method returns <see langword="true"/>, the failed result to publish; otherwise <see langword="null"/>.</param>
    /// <returns><see langword="true"/> when <paramref name="exception"/> is a bad request; otherwise <see langword="false"/>.</returns>
    internal bool TryGetResult(Exception exception, [NotNullWhen(true)] out MustValidationResult? result)
    {
        switch (exception)
        {
            case MustValidationException validationException:
                result = validationException.Result;
                return true;

            case ArgumentException argumentException when _options.HandleGuardExceptions:
                result = MustValidationResult.Fail(ToFailure(argumentException));
                return true;

            default:
                result = null;
                return false;
        }
    }

    /// <summary>
    /// Describes a guard's <see cref="ArgumentException"/> as the single failure it stands for.
    /// </summary>
    /// <param name="exception">The argument exception to describe.</param>
    /// <remarks>
    /// A guard PineGuard threw carries the property path and the code it failed on; an argument exception
    /// from anywhere else carries neither, so the parameter name stands in for the path and
    /// <see cref="MustValidationOptions.UnknownGuardCode"/> for the code.
    /// </remarks>
    private MustFailure ToFailure(ArgumentException exception) =>
        new(ResolvePropertyPath(exception),
            exception.TryGetMustCode(out var code) ? code : _options.UnknownGuardCode,
            exception.Message,
            Value: null);

    private static string ResolvePropertyPath(ArgumentException exception) =>
        exception.GetMustPropertyPath() is { Length: > 0 } propertyPath ? propertyPath : exception.ParamName ?? string.Empty;
}
