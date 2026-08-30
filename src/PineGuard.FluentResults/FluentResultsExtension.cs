using PineGuard.Common;
using PineGuard.MustClauses;

namespace PineGuard.FluentResults;

/// <summary>
/// Bridges PineGuard results into <c>FluentResults</c>, failing with <see cref="MustError"/> so the rule
/// code and property path survive the crossing instead of collapsing into a bare message.
/// </summary>
/// <remarks>
/// The bridge follows the clause's own <see cref="MustResult{T}.Result"/> contract: a clause that succeeds
/// with a <see langword="null"/> result produces a successful <c>Result&lt;T&gt;</c> whose value is
/// <see langword="default"/>. That is the clause's answer carried faithfully across, not a conversion failure.
/// </remarks>
/// <seealso cref="MustError"/>
/// <seealso cref="MustResult{T}"/>
/// <seealso cref="MustValidationResult"/>
public static class FluentResultsExtension
{
    /// <summary>
    /// Converts a single <see cref="MustResult{T}"/> into a <c>Result&lt;T&gt;</c>.
    /// </summary>
    /// <typeparam name="T">The validated or parsed result type.</typeparam>
    /// <param name="result">The result to convert.</param>
    /// <returns>
    /// A success carrying <see cref="MustResult{T}.Result"/> when <paramref name="result"/> succeeded;
    /// otherwise a failure carrying one <see cref="MustError"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="result"/> is <see langword="null"/>.</exception>
    public static global::FluentResults.Result<T> ToResult<T>(this MustResult<T> result)
    {
        ThrowHelper.ThrowIfNull(result);

        return result.Success
            ? global::FluentResults.Result.Ok(result.Result!)
            : global::FluentResults.Result.Fail<T>(MustError.From(result));
    }

    /// <summary>
    /// Converts a <see cref="MustValidationResult"/> into a valueless <c>Result</c>, for a caller that only
    /// needs pass or fail.
    /// </summary>
    /// <param name="result">The result to convert.</param>
    /// <returns>
    /// <c>Result.Ok()</c> when <paramref name="result"/> succeeded; otherwise a failure carrying one
    /// <see cref="MustError"/> per <see cref="MustValidationResult.Failures"/> entry, in the order the
    /// validator reported them.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="result"/> is <see langword="null"/>.</exception>
    public static global::FluentResults.Result ToResult(this MustValidationResult result)
    {
        ThrowHelper.ThrowIfNull(result);

        return result.Success
            ? global::FluentResults.Result.Ok()
            : global::FluentResults.Result.Fail(result.Failures.Select(MustError.From));
    }

    /// <summary>
    /// Converts a <see cref="MustValidationResult"/> into a <c>Result&lt;T&gt;</c> carrying the validated value.
    /// </summary>
    /// <typeparam name="T">The type of the validated object.</typeparam>
    /// <param name="result">The result to convert.</param>
    /// <param name="value">The value to carry when <paramref name="result"/> succeeded.</param>
    /// <returns>
    /// A success carrying <paramref name="value"/> when <paramref name="result"/> succeeded; otherwise a
    /// failure carrying one <see cref="MustError"/> per <see cref="MustValidationResult.Failures"/> entry,
    /// in the order the validator reported them.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="result"/> is <see langword="null"/>.</exception>
    public static global::FluentResults.Result<T> ToResult<T>(this MustValidationResult result, T value)
    {
        ThrowHelper.ThrowIfNull(result);

        return result.Success
            ? global::FluentResults.Result.Ok(value)
            : global::FluentResults.Result.Fail<T>(result.Failures.Select(MustError.From));
    }
}
