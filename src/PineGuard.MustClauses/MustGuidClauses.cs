using System.Runtime.CompilerServices;
using PineGuard.Rules;

namespace PineGuard.MustClauses;

/// <summary>
/// Provides <see cref="IMustClause"/> extension methods that validate <see cref="Guid"/> values.
/// </summary>
/// <seealso cref="GuidRules"/>
/// <seealso href="https://pineguard.ai/docs/must/guid">GUID Must Clauses documentation</seealso>
public static class MustGuidClauses
{
    /// <summary>
    /// Validates that the specified <see cref="Guid"/> value is not the empty GUID (<see cref="Guid.Empty"/>).
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The <see cref="Guid"/> value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> is not <see cref="Guid.Empty"/>, or <see langword="false"/> with a descriptive
    /// <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Delegates to <see cref="GuidRules.IsEmpty"/>. The failure message follows the pattern
    /// <c>"{paramName} must not be an empty GUID."</c>
    /// </remarks>
    /// <example>
    /// <code>
    /// var result = Must.Be.NotEmpty(entityId);
    /// if (result.Failed)
    ///     Console.WriteLine(result.Message);
    /// </code>
    /// </example>
    /// <seealso cref="GuidRules.IsEmpty"/>
    /// <seealso href="https://pineguard.ai/docs/must/guid">GUID Must Clauses documentation</seealso>
    public static MustResult<Guid> NotEmpty(
        this IMustClause _,
        Guid value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must not be an empty GUID.";

        var ok = !GuidRules.IsEmpty(value);
        return MustResult<Guid>.FromBool(ok, messageTemplate, paramName, value, value);
    }
}
