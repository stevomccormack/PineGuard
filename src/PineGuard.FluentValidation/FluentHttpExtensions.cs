using FluentValidation;
using PineGuard.Codes;
using PineGuard.FluentValidation.Common;
using PineGuard.MustClauses;

namespace PineGuard.FluentValidation;

/// <summary>
/// Provides FluentValidation extension methods for HTTP property validation including
/// header names, header values, status codes, header collection checks, and content type validation.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/fluent/http">Fluent HTTP Extensions documentation</seealso>
public static class FluentHttpExtensions
{
    /// <summary>Validates that the string value is a valid HTTP header name.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <example><code>RuleFor(x => x.Header).HeaderName();</code></example>
    public static IRuleBuilderOptions<TModel, string?> HeaderName<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.HeaderName(val, paramName: null),
            message, MustCodes.Http.HeaderName.Malformed);

    /// <summary>Validates that the string value is not a valid HTTP header name.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    public static IRuleBuilderOptions<TModel, string?> NotHeaderName<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.NotHeaderName(val, paramName: null),
            message, MustCodes.Http.HeaderName.WellFormed);

    /// <summary>Validates that the string value is a valid HTTP header value.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    public static IRuleBuilderOptions<TModel, string?> HeaderValue<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.HeaderValue(val, paramName: null),
            message, MustCodes.Http.HeaderValue.Malformed);

    /// <summary>Validates that the string value is not a valid HTTP header value.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    public static IRuleBuilderOptions<TModel, string?> NotHeaderValue<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.NotHeaderValue(val, paramName: null),
            message, MustCodes.Http.HeaderValue.WellFormed);

    /// <summary>Validates that the integer value is a valid HTTP status code (100-599).</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, int?> HttpStatusCode<TModel>(this IRuleBuilder<TModel, int?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => val.HasValue
                ? Must.Be.HttpStatusCode(val.Value, paramName: null)
                : MustResult<int>.Ok(0),
            message, MustCodes.Http.Status.OutOfRange);

    /// <summary>Validates that the integer value is not a valid HTTP status code.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, int?> NotHttpStatusCode<TModel>(this IRuleBuilder<TModel, int?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => val.HasValue
                ? Must.Be.NotHttpStatusCode(val.Value, paramName: null)
                : MustResult<int>.Ok(0),
            message, MustCodes.Http.Status.InRange);

    /// <summary>Validates that the integer value is an HTTP informational status code (1xx).</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, int?> HttpStatusInformational<TModel>(this IRuleBuilder<TModel, int?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => val.HasValue
                ? Must.Be.HttpStatusInformational(val.Value, paramName: null)
                : MustResult<int>.Ok(0),
            message, MustCodes.Http.Status.NotInformational);

    /// <summary>Validates that the integer value is not an HTTP informational status code (1xx).</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, int?> NotHttpStatusInformational<TModel>(this IRuleBuilder<TModel, int?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => val.HasValue
                ? Must.Be.NotHttpStatusInformational(val.Value, paramName: null)
                : MustResult<int>.Ok(0),
            message, MustCodes.Http.Status.Informational);

    /// <summary>Validates that the integer value is an HTTP success status code (2xx).</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, int?> HttpStatusSuccess<TModel>(this IRuleBuilder<TModel, int?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => val.HasValue
                ? Must.Be.HttpStatusSuccess(val.Value, paramName: null)
                : MustResult<int>.Ok(0),
            message, MustCodes.Http.Status.NotSuccess);

    /// <summary>Validates that the integer value is not an HTTP success status code (2xx).</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, int?> NotHttpStatusSuccess<TModel>(this IRuleBuilder<TModel, int?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => val.HasValue
                ? Must.Be.NotHttpStatusSuccess(val.Value, paramName: null)
                : MustResult<int>.Ok(0),
            message, MustCodes.Http.Status.Success);

    /// <summary>Validates that the integer value is an HTTP redirect status code (3xx).</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, int?> HttpStatusRedirect<TModel>(this IRuleBuilder<TModel, int?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => val.HasValue
                ? Must.Be.HttpStatusRedirect(val.Value, paramName: null)
                : MustResult<int>.Ok(0),
            message, MustCodes.Http.Status.NotRedirect);

    /// <summary>Validates that the integer value is not an HTTP redirect status code (3xx).</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, int?> NotHttpStatusRedirect<TModel>(this IRuleBuilder<TModel, int?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => val.HasValue
                ? Must.Be.NotHttpStatusRedirect(val.Value, paramName: null)
                : MustResult<int>.Ok(0),
            message, MustCodes.Http.Status.Redirect);

    /// <summary>Validates that the integer value is an HTTP client error status code (4xx).</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, int?> HttpStatusClientError<TModel>(this IRuleBuilder<TModel, int?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => val.HasValue
                ? Must.Be.HttpStatusClientError(val.Value, paramName: null)
                : MustResult<int>.Ok(0),
            message, MustCodes.Http.Status.NotClientError);

    /// <summary>Validates that the integer value is not an HTTP client error status code (4xx).</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, int?> NotHttpStatusClientError<TModel>(this IRuleBuilder<TModel, int?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => val.HasValue
                ? Must.Be.NotHttpStatusClientError(val.Value, paramName: null)
                : MustResult<int>.Ok(0),
            message, MustCodes.Http.Status.ClientError);

    /// <summary>Validates that the integer value is an HTTP server error status code (5xx).</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, int?> HttpStatusServerError<TModel>(this IRuleBuilder<TModel, int?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => val.HasValue
                ? Must.Be.HttpStatusServerError(val.Value, paramName: null)
                : MustResult<int>.Ok(0),
            message, MustCodes.Http.Status.NotServerError);

    /// <summary>Validates that the integer value is not an HTTP server error status code (5xx).</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, int?> NotHttpStatusServerError<TModel>(this IRuleBuilder<TModel, int?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => val.HasValue
                ? Must.Be.NotHttpStatusServerError(val.Value, paramName: null)
                : MustResult<int>.Ok(0),
            message, MustCodes.Http.Status.ServerError);

    /// <summary>Validates that the header collection contains a header with the specified name.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="name">The header name to look for.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    public static IRuleBuilderOptions<TModel, IReadOnlyDictionary<string, IEnumerable<string>>?> HasHeader<TModel>(this IRuleBuilder<TModel, IReadOnlyDictionary<string, IEnumerable<string>>?> ruleBuilder,
        string? name,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.HasHeader(val, name, paramName: null),
            message, MustCodes.Http.Header.Missing);

    /// <summary>Validates that the header collection does not contain a header with the specified name.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="name">The header name to check.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    public static IRuleBuilderOptions<TModel, IReadOnlyDictionary<string, IEnumerable<string>>?> NotHasHeader<TModel>(this IRuleBuilder<TModel, IReadOnlyDictionary<string, IEnumerable<string>>?> ruleBuilder,
        string? name,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.NotHasHeader(val, name, paramName: null),
            message, MustCodes.Http.Header.Present);

    /// <summary>Validates that the header collection contains a non-empty value for the specified header.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="name">The header name to check.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    public static IRuleBuilderOptions<TModel, IReadOnlyDictionary<string, IEnumerable<string>>?> HasHeaderValue<TModel>(this IRuleBuilder<TModel, IReadOnlyDictionary<string, IEnumerable<string>>?> ruleBuilder,
        string? name,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.HasHeaderValue(val, name, paramName: null),
            message, MustCodes.Http.HeaderValue.Missing);

    /// <summary>Validates that the header collection does not contain a non-empty value for the specified header.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="name">The header name to check.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    public static IRuleBuilderOptions<TModel, IReadOnlyDictionary<string, IEnumerable<string>>?> NotHasHeaderValue<TModel>(this IRuleBuilder<TModel, IReadOnlyDictionary<string, IEnumerable<string>>?> ruleBuilder,
        string? name,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.NotHasHeaderValue(val, name, paramName: null),
            message, MustCodes.Http.HeaderValue.Present);

    /// <summary>Validates that the header collection contains the specified header with a value equal to the expected value.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="name">The header name to check.</param>
    /// <param name="expectedValue">The expected header value.</param>
    /// <param name="comparison">The string comparison type to use.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    public static IRuleBuilderOptions<TModel, IReadOnlyDictionary<string, IEnumerable<string>>?> HasHeaderValueEqualTo<TModel>(this IRuleBuilder<TModel, IReadOnlyDictionary<string, IEnumerable<string>>?> ruleBuilder,
        string? name,
        string? expectedValue,
        StringComparison comparison = StringComparison.OrdinalIgnoreCase,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.HasHeaderValueEqualTo(val, name, expectedValue, comparison, paramName: null),
            message, MustCodes.Http.HeaderValue.Mismatch);

    /// <summary>Validates that the header collection does not contain the specified header with a value equal to the expected value.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="name">The header name to check.</param>
    /// <param name="expectedValue">The value to check against.</param>
    /// <param name="comparison">The string comparison type to use.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    public static IRuleBuilderOptions<TModel, IReadOnlyDictionary<string, IEnumerable<string>>?> NotHasHeaderValueEqualTo<TModel>(this IRuleBuilder<TModel, IReadOnlyDictionary<string, IEnumerable<string>>?> ruleBuilder,
        string? name,
        string? expectedValue,
        StringComparison comparison = StringComparison.OrdinalIgnoreCase,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.NotHasHeaderValueEqualTo(val, name, expectedValue, comparison, paramName: null),
            message, MustCodes.Http.HeaderValue.Match);

    /// <summary>Validates that the header collection contains exactly one value for the specified header.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="name">The header name to check.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    public static IRuleBuilderOptions<TModel, IReadOnlyDictionary<string, IEnumerable<string>>?> HasSingleHeaderValue<TModel>(this IRuleBuilder<TModel, IReadOnlyDictionary<string, IEnumerable<string>>?> ruleBuilder,
        string? name,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.HasSingleHeaderValue(val, name, paramName: null),
            message, MustCodes.Http.HeaderValue.NotSingle);

    /// <summary>Validates that the header collection does not contain exactly one value for the specified header.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="name">The header name to check.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    public static IRuleBuilderOptions<TModel, IReadOnlyDictionary<string, IEnumerable<string>>?> NotHasSingleHeaderValue<TModel>(this IRuleBuilder<TModel, IReadOnlyDictionary<string, IEnumerable<string>>?> ruleBuilder,
        string? name,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.NotHasSingleHeaderValue(val, name, paramName: null),
            message, MustCodes.Http.HeaderValue.Single);

    /// <summary>Validates that the header collection contains a Content-Type header matching one of the allowed types.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="allowed">The allowed content type values.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    public static IRuleBuilderOptions<TModel, IReadOnlyDictionary<string, IEnumerable<string>>?> HasContentType<TModel>(this IRuleBuilder<TModel, IReadOnlyDictionary<string, IEnumerable<string>>?> ruleBuilder,
        string[]? allowed,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.HasContentType(val, allowed, paramName: null),
            message, MustCodes.Http.ContentType.NotAllowed);

    /// <summary>Validates that the header collection does not contain a Content-Type header matching any of the specified types.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="allowed">The disallowed content type values.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    public static IRuleBuilderOptions<TModel, IReadOnlyDictionary<string, IEnumerable<string>>?> NotHasContentType<TModel>(this IRuleBuilder<TModel, IReadOnlyDictionary<string, IEnumerable<string>>?> ruleBuilder,
        string[]? allowed,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.NotHasContentType(val, allowed, paramName: null),
            message, MustCodes.Http.ContentType.Allowed);

    /// <summary>Validates that the string value is a valid media type.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustHttpClauses.MediaType"/>, so the verdict is about the <c>type/subtype</c>
    /// the value leads with — a trailing <c>; parameter=value</c> list is accepted and ignored. This validates
    /// a media type in its own right, which is what a caller wants for a property carrying one; matching a
    /// request's Content-Type against an allow-list is <c>HasContentType</c>'s job.
    /// If the value is <see langword="null"/>, validation passes.
    /// </remarks>
    /// <example><code>RuleFor(x => x.RequestedFormat).MediaType();</code></example>
    /// <seealso cref="MustHttpClauses.MediaType"/>
    public static IRuleBuilderOptions<TModel, string?> MediaType<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.MediaType(val, paramName: null) : MustResult<string>.Ok(null!),
            message, MustCodes.Http.MediaType.Invalid);
}
