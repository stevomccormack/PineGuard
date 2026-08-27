using System.Runtime.CompilerServices;
using PineGuard.Codes;
using PineGuard.Rules;

namespace PineGuard.MustClauses;

/// <summary>
/// Provides <see cref="IMustClause"/> extension methods that validate <see cref="bool"/> values.
/// </summary>
/// <seealso cref="BoolRules"/>
/// <seealso href="https://pineguard.ai/docs/must/bool">Bool Must Clauses documentation</seealso>
public static class MustBoolClauses
{
    /// <summary>
    /// Validates that the specified <see cref="bool"/> value is <see langword="true"/>.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The <see cref="bool"/> value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> is <see langword="true"/>, or <see langword="false"/> with a descriptive
    /// <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Delegates to <see cref="BoolRules.IsTrue"/>. The failure message follows the pattern
    /// <c>"{paramName} must be true."</c>
    /// </remarks>
    /// <example>
    /// <code>
    /// var result = Must.Be.True(isEnabled);
    /// if (result.Failed)
    ///     Console.WriteLine(result.Message);
    /// </code>
    /// </example>
    /// <seealso cref="BoolRules.IsTrue"/>
    /// <seealso href="https://pineguard.ai/docs/must/bool">Bool Must Clauses documentation</seealso>
    public static MustResult<bool> True(this IMustClause _,
        bool value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must be true.";

        var ok = BoolRules.IsTrue(value);
        return MustResult<bool>.FromBool(ok, MustCodes.Boolean.Value.False, messageTemplate, paramName, value, result: true);
    }

    /// <summary>
    /// Validates that the specified <see cref="bool"/> value is <see langword="false"/>.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The <see cref="bool"/> value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> is <see langword="false"/>, or <see langword="false"/> with a descriptive
    /// <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Delegates to <see cref="BoolRules.IsFalse"/>. The failure message follows the pattern
    /// <c>"{paramName} must be false."</c>
    /// </remarks>
    /// <example>
    /// <code>
    /// var result = Must.Be.False(isDisabled);
    /// if (result.Failed)
    ///     Console.WriteLine(result.Message);
    /// </code>
    /// </example>
    /// <seealso cref="BoolRules.IsFalse"/>
    /// <seealso href="https://pineguard.ai/docs/must/bool">Bool Must Clauses documentation</seealso>
    public static MustResult<bool> False(this IMustClause _,
        bool value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must be false.";

        var ok = BoolRules.IsFalse(value);
        return MustResult<bool>.FromBool(ok, MustCodes.Boolean.Value.True, messageTemplate, paramName, value, result: false);
    }
}
