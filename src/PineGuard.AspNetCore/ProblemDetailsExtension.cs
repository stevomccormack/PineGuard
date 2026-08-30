using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using PineGuard.Common;
using PineGuard.MustClauses;
using PineGuard.Utils;
using HttpJsonOptions = Microsoft.AspNetCore.Http.Json.JsonOptions;
using MvcJsonOptions = Microsoft.AspNetCore.Mvc.JsonOptions;

namespace PineGuard.AspNetCore;

/// <summary>
/// Turns a <see cref="MustValidationResult"/> into the RFC 9457 <see cref="ValidationProblemDetails"/>
/// body every PineGuard entry point returns — the Minimal API filter, the MVC filter and the exception
/// handler all build the response here, so all three answer identically.
/// </summary>
/// <remarks>
/// Error keys are the failures' property paths run through the application's JSON naming policy, and each
/// message is re-rendered with the same transformed path, so a client never reads
/// <c>"email": ["Email must be a valid email address."]</c> — key and message name the field the same way.
/// </remarks>
/// <seealso cref="MustValidationOptions"/>
/// <seealso cref="MustFailureDetail"/>
public static class ProblemDetailsExtension
{
    /// <summary>
    /// The <c>type</c> of every PineGuard validation response: RFC 9110's <c>400 Bad Request</c>, the same
    /// URI ASP.NET Core's own problem-details defaults use.
    /// </summary>
    internal const string BadRequestType = "https://tools.ietf.org/html/rfc9110#section-15.5.1";

    /// <summary>
    /// The extension member carrying one <see cref="MustFailureDetail"/> per failure.
    /// </summary>
    internal const string FailuresExtensionKey = "failures";

    /// <summary>
    /// Builds the response body for <paramref name="result"/>.
    /// </summary>
    /// <param name="result">The failed validation result to describe.</param>
    /// <param name="options">The validation options deciding the title and whether codes are published.</param>
    /// <param name="namingPolicy">The policy applied to every property path, or <see langword="null"/> to publish paths unchanged.</param>
    /// <param name="resolver">The resolver producing each failure's message.</param>
    /// <param name="httpContext">The request being answered, handed to <paramref name="resolver"/>.</param>
    /// <returns>A <see cref="ValidationProblemDetails"/> with <c>Status</c> 400, one <c>errors</c> entry per property path, and — when <see cref="MustValidationOptions.IncludeCodes"/> is set — a <c>failures</c> extension.</returns>
    /// <remarks>
    /// Two failures on one property produce one <c>errors</c> entry holding both messages, in the order the
    /// validator found them; <c>failures</c> keeps one entry per failure in the same order.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="result"/>, <paramref name="options"/>, <paramref name="resolver"/> or <paramref name="httpContext"/> is <see langword="null"/>.</exception>
    public static ValidationProblemDetails ToValidationProblemDetails(
        this MustValidationResult result,
        MustValidationOptions options,
        JsonNamingPolicy? namingPolicy,
        IMustFailureMessageResolver resolver,
        HttpContext httpContext)
    {
        ThrowHelper.ThrowIfNull(result);
        ThrowHelper.ThrowIfNull(options);
        ThrowHelper.ThrowIfNull(resolver);
        ThrowHelper.ThrowIfNull(httpContext);

        var problemDetails = new ValidationProblemDetails
        {
            Type = BadRequestType,
            Title = options.Title,
            Status = StatusCodes.Status400BadRequest
        };

        var details = options.IncludeCodes ? new List<MustFailureDetail>(result.Failures.Count) : null;

        foreach (var failure in result.Failures)
        {
            var propertyPath = TransformPath(failure.PropertyPath, namingPolicy);
            var message = RenamePropertyPath(resolver.Resolve(failure, httpContext), failure.PropertyPath, propertyPath);

            AddError(problemDetails.Errors, propertyPath, message);
            details?.Add(new MustFailureDetail(propertyPath, failure.Code, message));
        }

        if (details is not null)
            problemDetails.Extensions[FailuresExtensionKey] = details;

        return problemDetails;
    }

    /// <summary>
    /// Builds the response body for <paramref name="result"/>, taking the options, naming policy and
    /// message resolver from the request's services.
    /// </summary>
    /// <param name="result">The failed validation result to describe.</param>
    /// <param name="httpContext">The request being answered, and the source of every collaborator.</param>
    /// <returns>The same body the filters return.</returns>
    /// <remarks>This is the overload a handler that validates for itself reaches for.</remarks>
    /// <example>
    /// <code>
    /// var result = validator.Validate(order);
    /// if (result.Failed)
    ///     return TypedResults.Problem(result.ToValidationProblemDetails(httpContext));
    /// </code>
    /// </example>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="result"/> or <paramref name="httpContext"/> is <see langword="null"/>.</exception>
    public static ValidationProblemDetails ToValidationProblemDetails(this MustValidationResult result, HttpContext httpContext)
    {
        ThrowHelper.ThrowIfNull(result);
        ThrowHelper.ThrowIfNull(httpContext);

        var options = httpContext.RequestServices.GetRequiredService<IOptions<MustValidationOptions>>().Value;
        var resolver = httpContext.RequestServices.GetRequiredService<IMustFailureMessageResolver>();

        return result.ToValidationProblemDetails(options, ResolveNamingPolicy(httpContext, options), resolver, httpContext);
    }

    /// <summary>
    /// Resolves the naming policy applied to property paths: <see cref="MustValidationOptions.PropertyNamingPolicy"/>
    /// first, then — unless <see cref="MustValidationOptions.UseJsonNamingPolicy"/> is off — the Minimal API
    /// JSON policy, then the MVC one, then none.
    /// </summary>
    /// <param name="httpContext">The request whose services carry the JSON options.</param>
    /// <param name="options">The validation options, already resolved by the caller so they are read once.</param>
    internal static JsonNamingPolicy? ResolveNamingPolicy(HttpContext httpContext, MustValidationOptions options)
    {
        if (options.PropertyNamingPolicy is not null)
            return options.PropertyNamingPolicy;

        if (!options.UseJsonNamingPolicy)
            return null;

        var services = httpContext.RequestServices;

        return services.GetService<IOptions<HttpJsonOptions>>()?.Value.SerializerOptions.PropertyNamingPolicy
            ?? services.GetService<IOptions<MvcJsonOptions>>()?.Value.JsonSerializerOptions.PropertyNamingPolicy;
    }

    private static string TransformPath(string propertyPath, JsonNamingPolicy? namingPolicy) =>
        namingPolicy is null ? propertyPath : PropertyPathUtility.Transform(propertyPath, namingPolicy.ConvertName);

    /// <summary>
    /// Rewrites the property path already rendered into <paramref name="message"/> to its transformed
    /// spelling, so key and message agree.
    /// </summary>
    /// <remarks>
    /// A <see cref="MustFailure"/> carries the message a rule already rendered, not the template it came
    /// from, so re-rendering means substituting the path the rule used for the one published.
    /// </remarks>
    private static string RenamePropertyPath(string message, string propertyPath, string transformedPath) =>
        propertyPath.Length == 0 || string.Equals(propertyPath, transformedPath, StringComparison.Ordinal)
            ? message
            : message.Replace(propertyPath, transformedPath, StringComparison.Ordinal);

    private static void AddError(IDictionary<string, string[]> errors, string propertyPath, string message) =>
        errors[propertyPath] = errors.TryGetValue(propertyPath, out var existing) ? [.. existing, message] : [message];
}
