using System.Runtime.CompilerServices;
using PineGuard.MustClauses;
using PineGuard.Rules;

namespace PineGuard.GuardClauses;

/// <summary>
/// Guard clauses for string-to-GUID parsing guard clauses.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/guard/string-guid">Guard StringGuid documentation</seealso>
public static class GuardStringGuidClauses
{
    /// <summary>
    /// Throws if <paramref name="value"/> violates the NotGuid constraint.
    /// </summary>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The value to guard.</param>
    /// <param name="message">An optional custom error message.</param>
    /// <param name="exceptionCreator">An optional factory to create a custom exception.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated value if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when the guard condition is violated and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <seealso cref="MustStringGuidClauses.Guid"/>
    public static Guid NotGuid(this IGuardClause _,
        string? value,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var result = Must.Be.Guid(value, paramName);
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> violates the EmptyGuid constraint.
    /// </summary>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The value to guard.</param>
    /// <param name="message">An optional custom error message.</param>
    /// <param name="exceptionCreator">An optional factory to create a custom exception.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated value if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when the guard condition is violated and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <seealso cref="MustStringGuidClauses.NotEmptyGuid"/>
    public static Guid EmptyGuid(this IGuardClause _,
        string? value,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var result = Must.Be.NotEmptyGuid(value, paramName);
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> does not parse as a <see cref="Guid"/> carrying the specified
    /// UUID version.
    /// </summary>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The string to parse and guard.</param>
    /// <param name="version">
    /// The required version, from <see cref="GuidRules.MinVersion"/> to <see cref="GuidRules.MaxVersion"/>.
    /// </param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustStringGuidClauses.HasGuidVersion"/>.
    /// </param>
    /// <param name="exceptionCreator">
    /// An optional factory to create a custom exception. If <see langword="null"/>,
    /// throws <see cref="ArgumentException"/> via <see cref="GuardFailure.Throw"/>.
    /// </param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The parsed <see cref="Guid"/> if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="value"/> does not parse as a GUID carrying version
    /// <paramref name="version"/>, or when <paramref name="version"/> falls outside
    /// <see cref="GuidRules.MinVersion"/>–<see cref="GuidRules.MaxVersion"/>, and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustStringGuidClauses.HasGuidVersion"/>:
    /// <c>Guard.Against.NotHasGuidVersion</c> passes when the string parses and its version nibble equals
    /// <paramref name="version"/>. A version outside the supported range is a configuration error and is
    /// attributed to <paramref name="version"/> rather than to <paramref name="value"/>.
    /// </remarks>
    /// <example>
    /// <code>
    /// Guard.Against.NotHasGuidVersion(idHeader, 4);
    /// </code>
    /// </example>
    /// <seealso cref="MustStringGuidClauses.HasGuidVersion"/>
    public static Guid NotHasGuidVersion(this IGuardClause _,
        string? value,
        int version,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var result = Must.Be.HasGuidVersion(value, version, paramName); // Guard.Against.NotHasGuidVersion => Must.Be.HasGuidVersion (complement)
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result;
    }
}
