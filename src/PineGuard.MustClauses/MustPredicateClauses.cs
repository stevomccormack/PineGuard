using System.Runtime.CompilerServices;
using PineGuard.Codes;
using PineGuard.Rules;

namespace PineGuard.MustClauses;

/// <summary>
/// Provides <see cref="IMustClause"/> extension methods that validate values against caller-supplied predicates.
/// </summary>
/// <seealso cref="PredicateRules"/>
/// <seealso href="https://pineguard.ai/docs/must/predicate">Predicate Must Clauses documentation</seealso>
public static class MustPredicateClauses
{
    /// <summary>
    /// Validates that the specified value satisfies the given predicate.
    /// </summary>
    /// <typeparam name="T">The type of the value being validated.</typeparam>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="predicate">The predicate function that the value must satisfy. Must not be <see langword="null"/>.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> satisfies <paramref name="predicate"/>, or <see langword="false"/> with
    /// a descriptive <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Returns a failed result immediately if <paramref name="predicate"/> is <see langword="null"/>.
    /// Delegates to <see cref="PredicateRules.Satisfies{T}"/>. The failure message follows the pattern
    /// <c>"{paramName} must satisfy the predicate."</c>
    /// </remarks>
    /// <example>
    /// <code>
    /// var result = Must.Be.Satisfies(age, x => x >= 18);
    /// if (result.Failed)
    ///     Console.WriteLine(result.Message);
    /// </code>
    /// </example>
    /// <seealso cref="PredicateRules.Satisfies{T}"/>
    /// <seealso href="https://pineguard.ai/docs/must/predicate">Predicate Must Clauses documentation</seealso>
    public static MustResult<T> Satisfies<T>(this IMustClause _,
        T? value,
        Func<T, bool> predicate,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (predicate is null)
            return MustResult<T>.Fail(MustCodes.Predicate.Callback.Null, "{paramName} must not be null.", nameof(predicate), predicate);

        const string messageTemplate = "{paramName} must satisfy the predicate.";

        var ok = PredicateRules.Satisfies(value, predicate);
        return MustResult<T>.FromBool(ok, MustCodes.Predicate.Result.False, messageTemplate, paramName, value, result: value!);
    }

    /// <summary>
    /// Validates that the specified value does not satisfy the given predicate.
    /// </summary>
    /// <typeparam name="T">The type of the value being validated.</typeparam>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="predicate">The predicate function that the value must not satisfy. Must not be <see langword="null"/>.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> does not satisfy <paramref name="predicate"/>, or <see langword="false"/> with
    /// a descriptive <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Returns a failed result immediately if <paramref name="predicate"/> is <see langword="null"/>.
    /// Delegates to <see cref="PredicateRules.Satisfies{T}"/>. The failure message follows the pattern
    /// <c>"{paramName} must not satisfy the predicate."</c>
    /// </remarks>
    /// <example>
    /// <code>
    /// var result = Must.Be.NotSatisfies(status, x => x == "blocked");
    /// if (result.Failed)
    ///     Console.WriteLine(result.Message);
    /// </code>
    /// </example>
    /// <seealso cref="PredicateRules.Satisfies{T}"/>
    /// <seealso href="https://pineguard.ai/docs/must/predicate">Predicate Must Clauses documentation</seealso>
    public static MustResult<T> NotSatisfies<T>(this IMustClause _,
        T? value,
        Func<T, bool> predicate,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (predicate is null)
            return MustResult<T>.Fail(MustCodes.Predicate.Callback.Null, "{paramName} must not be null.", nameof(predicate), predicate);

        const string messageTemplate = "{paramName} must not satisfy the predicate.";

        var ok = !PredicateRules.Satisfies(value, predicate);
        return MustResult<T>.FromBool(ok, MustCodes.Predicate.Result.True, messageTemplate, paramName, value, result: value!);
    }
}
