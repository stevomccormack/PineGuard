using System.Runtime.CompilerServices;
using PineGuard.MustClauses;
using PineGuard.Rules;

namespace PineGuard.GuardClauses;

/// <summary>
/// Guard clauses for <see cref="Guid"/> values.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/guard/guid">Guard Guid Clauses documentation</seealso>
public static class GuardGuidClauses
{
    /// <summary>
    /// Throws if <paramref name="value"/> is <see cref="Guid.Empty"/>.
    /// </summary>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The GUID value to guard.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustGuidClauses.NotEmpty"/>.
    /// </param>
    /// <param name="exceptionCreator">
    /// An optional factory to create a custom exception. If <see langword="null"/>,
    /// throws <see cref="ArgumentException"/> via <see cref="GuardFailure.Throw"/>.
    /// </param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated value of <paramref name="value"/> if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="value"/> is <see cref="Guid.Empty"/> and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustGuidClauses.NotEmpty"/>:
    /// <c>Guard.Against.Empty</c> passes when the GUID is not <see cref="Guid.Empty"/>.
    /// </remarks>
    /// <example>
    /// <code>
    /// Guard.Against.Empty(entityId);
    /// </code>
    /// </example>
    /// <seealso cref="MustGuidClauses.NotEmpty"/>
    public static Guid Empty(
        this IGuardClause _,
        Guid value,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var result = Must.Be.NotEmpty(value, paramName);
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> does not carry the specified UUID version.
    /// </summary>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The GUID value to guard.</param>
    /// <param name="version">
    /// The required version, from <see cref="GuidRules.MinVersion"/> to <see cref="GuidRules.MaxVersion"/>.
    /// </param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustGuidClauses.HasGuidVersion"/>.
    /// </param>
    /// <param name="exceptionCreator">
    /// An optional factory to create a custom exception. If <see langword="null"/>,
    /// throws <see cref="ArgumentException"/> via <see cref="GuardFailure.Throw"/>.
    /// </param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated value of <paramref name="value"/> if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="value"/> does not carry version <paramref name="version"/>, or when
    /// <paramref name="version"/> falls outside <see cref="GuidRules.MinVersion"/>–<see cref="GuidRules.MaxVersion"/>,
    /// and no <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustGuidClauses.HasGuidVersion"/>:
    /// <c>Guard.Against.NotHasGuidVersion</c> passes when the GUID's version nibble equals
    /// <paramref name="version"/>. A version outside the supported range is a configuration error and is
    /// attributed to <paramref name="version"/> rather than to <paramref name="value"/>.
    /// </remarks>
    /// <example>
    /// <code>
    /// Guard.Against.NotHasGuidVersion(entityId, 4);
    /// </code>
    /// </example>
    /// <seealso cref="MustGuidClauses.HasGuidVersion"/>
    public static Guid NotHasGuidVersion(
        this IGuardClause _,
        Guid value,
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
