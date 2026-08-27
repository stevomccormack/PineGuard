using System.Runtime.CompilerServices;
using PineGuard.Codes;
using PineGuard.Rules;
using PineGuard.Utils;

namespace PineGuard.MustClauses;

/// <summary>
/// Provides <see cref="IMustClause"/> extension methods that validate boolean string representations,
/// parsing the input string before delegating to boolean rules.
/// </summary>
/// <seealso cref="BoolRules"/>
/// <seealso href="https://pineguard.ai/docs/must/string-bool">String Bool Must Clauses documentation</seealso>
public static class MustStringBoolClauses
{
    /// <summary>
    /// Validates that the specified string can be parsed as a boolean and represents <see langword="true"/>.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The string to parse and validate as <see langword="true"/>.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> parses as <see langword="true"/>, or <see langword="false"/> with a descriptive
    /// <see cref="MustResult{T}.Message"/>. On success, <see cref="MustResult{T}.Result"/> contains the
    /// parsed <see cref="bool"/>.
    /// </returns>
    /// <remarks>
    /// Returns a failed result immediately if <paramref name="value"/> is <see langword="null"/>.
    /// Delegates to <see cref="StringUtility.Bool"/> for parsing and <see cref="BoolRules.IsTrue"/>.
    /// The failure message follows the pattern <c>"{paramName} must be true."</c>
    /// </remarks>
    /// <example>
    /// <code>
    /// var result = Must.Be.True(boolString);
    /// if (result.Failed)
    ///     Console.WriteLine(result.Message);
    /// </code>
    /// </example>
    /// <seealso href="https://pineguard.ai/docs/must/string-bool">String Bool Must Clauses documentation</seealso>
    public static MustResult<bool> True(this IMustClause _,
        string? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<bool>.Fail(MustCodes.Boolean.Value.False, "{paramName} must not be null.", paramName, value);

        const string messageTemplate = "{paramName} must be true.";

        if (!StringUtility.Bool.TryParse(value, out var parsed))
            return MustResult<bool>.FromBool(false, MustCodes.Boolean.Value.False, messageTemplate, paramName, value, result: default);

        var parsedValue = parsed.GetValueOrDefault();
        var ok = BoolRules.IsTrue(parsedValue);
        return MustResult<bool>.FromBool(ok, MustCodes.Boolean.Value.False, messageTemplate, paramName, value, parsedValue);
    }

    /// <summary>
    /// Validates that the specified string can be parsed as a boolean and represents <see langword="false"/>.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The string to parse and validate as <see langword="false"/>.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> parses as <see langword="false"/>, or <see langword="false"/> with a descriptive
    /// <see cref="MustResult{T}.Message"/>. On success, <see cref="MustResult{T}.Result"/> contains the
    /// parsed <see cref="bool"/>.
    /// </returns>
    /// <remarks>
    /// Returns a failed result immediately if <paramref name="value"/> is <see langword="null"/>.
    /// Delegates to <see cref="StringUtility.Bool"/> for parsing and <see cref="BoolRules.IsFalse"/>.
    /// The failure message follows the pattern <c>"{paramName} must be false."</c>
    /// </remarks>
    /// <example>
    /// <code>
    /// var result = Must.Be.False(boolString);
    /// if (result.Failed)
    ///     Console.WriteLine(result.Message);
    /// </code>
    /// </example>
    /// <seealso href="https://pineguard.ai/docs/must/string-bool">String Bool Must Clauses documentation</seealso>
    public static MustResult<bool> False(this IMustClause _,
        string? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<bool>.Fail(MustCodes.Boolean.Value.True, "{paramName} must not be null.", paramName, value);

        const string messageTemplate = "{paramName} must be false.";

        if (!StringUtility.Bool.TryParse(value, out var parsed))
            return MustResult<bool>.FromBool(false, MustCodes.Boolean.Value.True, messageTemplate, paramName, value, result: default);

        var parsedValue = parsed.GetValueOrDefault();
        var ok = BoolRules.IsFalse(parsedValue);
        return MustResult<bool>.FromBool(ok, MustCodes.Boolean.Value.True, messageTemplate, paramName, value, parsedValue);
    }
}
