using System.Runtime.CompilerServices;
using PineGuard.Rules;

namespace PineGuard.MustClauses;

/// <summary>
/// Provides <see cref="IMustClause"/> extension methods that validate nullability of any reference or value type.
/// </summary>
/// <seealso cref="NullRules"/>
/// <seealso href="https://pineguard.ai/docs/must/null">Null Must Clauses documentation</seealso>
public static class MustNullClauses
{
    /// <summary>
    /// Validates that the specified value is <see langword="null"/>.
    /// </summary>
    /// <typeparam name="T">The type of the value being validated.</typeparam>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> is <see langword="null"/>, or <see langword="false"/> with a descriptive
    /// <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Delegates to <see cref="NullRules.IsNull{T}"/>. The failure message follows the pattern
    /// <c>"{paramName} must be null."</c>
    /// </remarks>
    /// <example>
    /// <code>
    /// var result = Must.Be.Null(optionalValue);
    /// if (result.Failed)
    ///     Console.WriteLine(result.Message);
    /// </code>
    /// </example>
    /// <seealso cref="NullRules.IsNull{T}"/>
    /// <seealso href="https://pineguard.ai/docs/must/null">Null Must Clauses documentation</seealso>
    public static MustResult<T> Null<T>(this IMustClause _,
        T? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must be null.";

        var ok = NullRules.IsNull(value);
        return MustResult<T>.FromBool(ok, messageTemplate, paramName, value, result: default);
    }

    /// <summary>
    /// Validates that the specified value is not <see langword="null"/>.
    /// </summary>
    /// <typeparam name="T">The type of the value being validated.</typeparam>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> is not <see langword="null"/>, or <see langword="false"/> with a descriptive
    /// <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Delegates to <see cref="NullRules.IsNotNull{T}"/>. The failure message follows the pattern
    /// <c>"{paramName} must not be null."</c>
    /// </remarks>
    /// <example>
    /// <code>
    /// var result = Must.Be.NotNull(requiredValue);
    /// if (result.Failed)
    ///     Console.WriteLine(result.Message);
    /// </code>
    /// </example>
    /// <seealso cref="NullRules.IsNotNull{T}"/>
    /// <seealso href="https://pineguard.ai/docs/must/null">Null Must Clauses documentation</seealso>
    public static MustResult<T> NotNull<T>(this IMustClause _,
        T? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must not be null.";

        var ok = NullRules.IsNotNull(value);
        return MustResult<T>.FromBool(ok, messageTemplate, paramName, value, value);
    }
}
