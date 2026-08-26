using System.Runtime.CompilerServices;
using PineGuard.Codes;
using PineGuard.Rules;

namespace PineGuard.MustClauses;

/// <summary>
/// Provides <see cref="IMustClause"/> extension methods that validate values against their type's default value.
/// </summary>
/// <seealso cref="DefaultEqualityRules"/>
/// <seealso href="https://pineguard.ai/docs/must/default">Default Equality Must Clauses documentation</seealso>
public static class MustDefaultEqualityClauses
{
    /// <summary>
    /// Validates that the specified value is equal to the default value for its type.
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
    /// if <paramref name="value"/> equals <see langword="default"/>(<typeparamref name="T"/>), or
    /// <see langword="false"/> with a descriptive <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Delegates to <see cref="DefaultEqualityRules.IsDefault{T}"/>. The failure message follows the pattern
    /// <c>"{paramName} must be the default value."</c>
    /// </remarks>
    /// <example>
    /// <code>
    /// var result = Must.Be.Default(count);
    /// if (result.Failed)
    ///     Console.WriteLine(result.Message);
    /// </code>
    /// </example>
    /// <seealso cref="DefaultEqualityRules.IsDefault{T}"/>
    /// <seealso href="https://pineguard.ai/docs/must/default">Default Equality Must Clauses documentation</seealso>
    public static MustResult<T> Default<T>(this IMustClause _,
        T? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must be the default value.";

        var ok = DefaultEqualityRules.IsDefault(value);
        return MustResult<T>.FromBool(ok, MustCodes.Value.State.NotDefault, messageTemplate, paramName, value, result: value!);
    }

    /// <summary>
    /// Validates that the specified value is not equal to the default value for its type.
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
    /// if <paramref name="value"/> does not equal <see langword="default"/>(<typeparamref name="T"/>), or
    /// <see langword="false"/> with a descriptive <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Delegates to <see cref="DefaultEqualityRules.IsDefault{T}"/>. The failure message follows the pattern
    /// <c>"{paramName} must not be the default value."</c>
    /// </remarks>
    /// <example>
    /// <code>
    /// var result = Must.Be.NotDefault(amount);
    /// if (result.Failed)
    ///     Console.WriteLine(result.Message);
    /// </code>
    /// </example>
    /// <seealso cref="DefaultEqualityRules.IsDefault{T}"/>
    /// <seealso href="https://pineguard.ai/docs/must/default">Default Equality Must Clauses documentation</seealso>
    public static MustResult<T> NotDefault<T>(this IMustClause _,
        T? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must not be the default value.";

        var ok = !DefaultEqualityRules.IsDefault(value);
        return MustResult<T>.FromBool(ok, MustCodes.Value.State.Default, messageTemplate, paramName, value, result: value!);
    }

    /// <summary>
    /// Validates that the specified value is <see langword="null"/> or equal to the default value for its type.
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
    /// if <paramref name="value"/> is <see langword="null"/> or equal to <see langword="default"/>(<typeparamref name="T"/>),
    /// or <see langword="false"/> with a descriptive <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Delegates to <see cref="DefaultEqualityRules.IsNullOrDefault{T}"/>. The failure message follows the pattern
    /// <c>"{paramName} must be null or the default value."</c>
    /// </remarks>
    /// <example>
    /// <code>
    /// var result = Must.Be.NullOrDefault(optionalCount);
    /// if (result.Failed)
    ///     Console.WriteLine(result.Message);
    /// </code>
    /// </example>
    /// <seealso cref="DefaultEqualityRules.IsNullOrDefault{T}"/>
    /// <seealso href="https://pineguard.ai/docs/must/default">Default Equality Must Clauses documentation</seealso>
    public static MustResult<T?> NullOrDefault<T>(this IMustClause _,
        T? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must be null or the default value.";

        var ok = DefaultEqualityRules.IsNullOrDefault(value);
        return MustResult<T?>.FromBool(ok, MustCodes.Value.State.NotNullOrDefault, messageTemplate, paramName, value, result: value);
    }

    /// <summary>
    /// Validates that the specified value is neither <see langword="null"/> nor equal to the default value for its type.
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
    /// if <paramref name="value"/> is neither <see langword="null"/> nor <see langword="default"/>(<typeparamref name="T"/>),
    /// or <see langword="false"/> with a descriptive <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Delegates to <see cref="DefaultEqualityRules.IsNullOrDefault{T}"/>. The failure message follows the pattern
    /// <c>"{paramName} must not be null or the default value."</c>
    /// </remarks>
    /// <example>
    /// <code>
    /// var result = Must.Be.NotNullOrDefault(requiredAmount);
    /// if (result.Failed)
    ///     Console.WriteLine(result.Message);
    /// </code>
    /// </example>
    /// <seealso cref="DefaultEqualityRules.IsNullOrDefault{T}"/>
    /// <seealso href="https://pineguard.ai/docs/must/default">Default Equality Must Clauses documentation</seealso>
    public static MustResult<T?> NotNullOrDefault<T>(this IMustClause _,
        T? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must not be null or the default value.";

        var ok = !DefaultEqualityRules.IsNullOrDefault(value);
        return MustResult<T?>.FromBool(ok, MustCodes.Value.State.NullOrDefault, messageTemplate, paramName, value, result: value);
    }
}
