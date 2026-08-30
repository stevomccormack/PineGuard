using PineGuard.Common;
using PineGuard.MustClauses;

namespace PineGuard.OneOf;

/// <summary>
/// Bridges PineGuard results into <c>OneOf</c>, turning a result into a two-case union the caller matches on
/// instead of inspecting a success flag.
/// </summary>
/// <remarks>
/// <para>
/// The failure case is PineGuard's own type — <see cref="MustFailure"/> for a single clause,
/// <see cref="MustValidationResult"/> for a whole object — so the rule <see cref="MustFailure.Code"/>,
/// rendered <see cref="MustFailure.Message"/> and <see cref="MustFailure.PropertyPath"/> cross unchanged
/// with nothing to translate and nothing to lose.
/// </para>
/// <para>
/// The bridge follows the clause's own <see cref="MustResult{T}.Result"/> contract: a clause that succeeds
/// with a <see langword="null"/> result produces the value case carrying <see langword="default"/>. That is
/// the clause's answer carried faithfully across, not a conversion failure.
/// </para>
/// </remarks>
/// <seealso cref="MustResult{T}"/>
/// <seealso cref="MustValidationResult"/>
/// <seealso cref="MustFailure"/>
public static class OneOfExtension
{
    /// <summary>
    /// Converts a single <see cref="MustResult{T}"/> into a <c>OneOf&lt;T, MustFailure&gt;</c>.
    /// </summary>
    /// <typeparam name="T">The validated or parsed result type.</typeparam>
    /// <param name="result">The result to convert.</param>
    /// <returns>
    /// The first case carrying <see cref="MustResult{T}.Result"/> when <paramref name="result"/> succeeded;
    /// otherwise the second case carrying <see cref="MustFailure.From(IMustResult, string)"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="result"/> is <see langword="null"/>.</exception>
    public static global::OneOf.OneOf<T, MustFailure> ToOneOf<T>(this MustResult<T> result)
    {
        ThrowHelper.ThrowIfNull(result);

        return result.Success
            ? global::OneOf.OneOf<T, MustFailure>.FromT0(result.Result!)
            : global::OneOf.OneOf<T, MustFailure>.FromT1(MustFailure.From(result));
    }

    /// <summary>
    /// Converts a <see cref="MustValidationResult"/> into a <c>OneOf&lt;T, MustValidationResult&gt;</c> carrying
    /// the validated value.
    /// </summary>
    /// <typeparam name="T">The type of the validated object.</typeparam>
    /// <param name="result">The result to convert.</param>
    /// <param name="value">The value to carry when <paramref name="result"/> succeeded.</param>
    /// <returns>
    /// The first case carrying <paramref name="value"/> when <paramref name="result"/> succeeded; otherwise the
    /// second case carrying <paramref name="result"/> itself, with every failure intact and in the order the
    /// validator reported them.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="result"/> is <see langword="null"/>.</exception>
    public static global::OneOf.OneOf<T, MustValidationResult> ToOneOf<T>(this MustValidationResult result, T value)
    {
        ThrowHelper.ThrowIfNull(result);

        return result.Success
            ? global::OneOf.OneOf<T, MustValidationResult>.FromT0(value)
            : global::OneOf.OneOf<T, MustValidationResult>.FromT1(result);
    }
}
