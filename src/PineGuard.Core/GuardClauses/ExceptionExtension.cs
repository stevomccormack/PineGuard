using System.Diagnostics.CodeAnalysis;
using PineGuard.Common;

namespace PineGuard.GuardClauses;

/// <summary>
/// Reads the <c>MustCodes</c> catalogue code and property path that <see cref="GuardFailure.Throw"/> stamps
/// onto every exception it raises, including one returned by a <see cref="GuardExceptionPolicy"/> map.
/// </summary>
/// <seealso cref="GuardFailure"/>
/// <seealso cref="GuardExceptionPolicy"/>
/// <seealso href="https://pineguard.ai/docs/guard">Guard Clauses documentation</seealso>
public static class ExceptionExtension
{
    /// <summary>
    /// Attempts to read the <c>MustCodes</c> catalogue code stamped on <paramref name="exception"/> by a
    /// PineGuard guard clause.
    /// </summary>
    /// <param name="exception">The exception to inspect.</param>
    /// <param name="code">The stamped code, or <see langword="null"/> when none was stamped.</param>
    /// <returns><see langword="true"/> when a code was stamped; otherwise <see langword="false"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="exception"/> is <see langword="null"/>.</exception>
    public static bool TryGetMustCode(this Exception exception, [NotNullWhen(true)] out string? code)
    {
        ThrowHelper.ThrowIfNull(exception);

        code = exception.Data[GuardFailure.CodeDataKey] as string;
        return code is not null;
    }

    /// <summary>
    /// Determines whether <paramref name="exception"/> was raised for the given <c>MustCodes</c> catalogue code.
    /// </summary>
    /// <param name="exception">The exception to inspect.</param>
    /// <param name="code">The <c>MustCodes</c> catalogue code to compare against.</param>
    /// <returns><see langword="true"/> when the stamped code equals <paramref name="code"/>; otherwise <see langword="false"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="exception"/> or <paramref name="code"/> is <see langword="null"/>.
    /// </exception>
    public static bool HasMustCode(this Exception exception, string code)
    {
        ThrowHelper.ThrowIfNull(exception);
        ThrowHelper.ThrowIfNull(code);

        return string.Equals(exception.Data[GuardFailure.CodeDataKey] as string, code, StringComparison.Ordinal);
    }

    /// <summary>
    /// Reads the property path (the guarded parameter's name) stamped on <paramref name="exception"/> by a
    /// PineGuard guard clause.
    /// </summary>
    /// <param name="exception">The exception to inspect.</param>
    /// <returns>The stamped property path, or <see cref="string.Empty"/> when none was stamped.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="exception"/> is <see langword="null"/>.</exception>
    public static string GetMustPropertyPath(this Exception exception)
    {
        ThrowHelper.ThrowIfNull(exception);

        return exception.Data[GuardFailure.PropertyPathDataKey] as string ?? string.Empty;
    }
}
