using PineGuard.Common;
using PineGuard.MustClauses;

namespace PineGuard.ErrorOr;

/// <summary>
/// Bridges PineGuard results into <c>ErrorOr</c>, carrying every failure's rule <see cref="MustFailure.Code"/>,
/// rendered <see cref="MustFailure.Message"/> and <see cref="MustFailure.PropertyPath"/> across unchanged.
/// </summary>
/// <remarks>
/// <para>
/// Every produced error is an <c>Error.Validation</c>: <c>Code</c> is PineGuard's three-segment rule address
/// (for example <c>email.address.invalid</c>), <c>Description</c> is the rendered message, and
/// <c>Metadata["propertyPath"]</c> is where in the validated object the failure was found
/// (<see cref="string.Empty"/> at the root).
/// </para>
/// <para>
/// The bridge follows the clause's own <see cref="MustResult{T}.Result"/> contract: a clause that succeeds
/// with a <see langword="null"/> result produces a successful <c>ErrorOr&lt;T&gt;</c> whose value is
/// <see langword="default"/>. That is the clause's answer carried faithfully across, not a conversion failure.
/// </para>
/// </remarks>
/// <seealso cref="MustResult{T}"/>
/// <seealso cref="MustValidationResult"/>
/// <seealso cref="MustFailure"/>
public static class ErrorOrExtension
{
    /// <summary>
    /// The metadata key every produced error carries its <see cref="MustFailure.PropertyPath"/> under.
    /// </summary>
    /// <remarks>
    /// camelCase because metadata bags are wire-shaped and <c>ErrorOr</c>'s own conventions are camelCase.
    /// </remarks>
    public const string PropertyPathMetadataKey = "propertyPath";

    /// <summary>
    /// Converts a single <see cref="MustResult{T}"/> into an <c>ErrorOr&lt;T&gt;</c>.
    /// </summary>
    /// <typeparam name="T">The validated or parsed result type.</typeparam>
    /// <param name="result">The result to convert.</param>
    /// <returns>
    /// <see cref="MustResult{T}.Result"/> when <paramref name="result"/> succeeded; otherwise a single
    /// <c>Error.Validation</c> built by <see cref="ToError"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="result"/> is <see langword="null"/>.</exception>
    public static global::ErrorOr.ErrorOr<T> ToErrorOr<T>(this MustResult<T> result)
    {
        ThrowHelper.ThrowIfNull(result);

        if (result.Success)
            return result.Result!;

        return MustFailure.From(result).ToError();
    }

    /// <summary>
    /// Converts a single <see cref="MustFailure"/> into an <c>Error.Validation</c>.
    /// </summary>
    /// <param name="failure">The failure to convert.</param>
    /// <returns>
    /// An <c>Error</c> whose <c>Code</c> is <see cref="MustFailure.Code"/>, <c>Description</c> is
    /// <see cref="MustFailure.Message"/>, and <c>Metadata[<see cref="PropertyPathMetadataKey"/>]</c> is
    /// <see cref="MustFailure.PropertyPath"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="failure"/> is <see langword="null"/>.</exception>
    public static global::ErrorOr.Error ToError(this MustFailure failure)
    {
        ThrowHelper.ThrowIfNull(failure);

        return global::ErrorOr.Error.Validation(
            failure.Code,
            failure.Message,
            new Dictionary<string, object> { [PropertyPathMetadataKey] = failure.PropertyPath });
    }

    /// <summary>
    /// Converts every failure in a <see cref="MustValidationResult"/> into an <c>Error</c>.
    /// </summary>
    /// <param name="result">The result to convert.</param>
    /// <returns>
    /// One <c>Error</c> per <see cref="MustValidationResult.Failures"/> entry, in the order the validator
    /// reported them; an empty list when <paramref name="result"/> succeeded.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="result"/> is <see langword="null"/>.</exception>
    public static List<global::ErrorOr.Error> ToErrors(this MustValidationResult result)
    {
        ThrowHelper.ThrowIfNull(result);

        return result.Failures.Select(ToError).ToList();
    }

    /// <summary>
    /// Converts a <see cref="MustValidationResult"/> into an <c>ErrorOr&lt;T&gt;</c> carrying the validated value.
    /// </summary>
    /// <typeparam name="T">The type of the validated object.</typeparam>
    /// <param name="result">The result to convert.</param>
    /// <param name="value">The value to carry when <paramref name="result"/> succeeded.</param>
    /// <returns>
    /// <paramref name="value"/> when <paramref name="result"/> succeeded; otherwise every failure as
    /// <see cref="ToErrors"/> produces them.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="result"/> is <see langword="null"/>.</exception>
    public static global::ErrorOr.ErrorOr<T> ToErrorOr<T>(this MustValidationResult result, T value)
    {
        ThrowHelper.ThrowIfNull(result);

        if (result.Success)
            return value;

        return result.ToErrors();
    }
}
