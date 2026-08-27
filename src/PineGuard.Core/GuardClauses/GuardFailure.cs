using System.Diagnostics.CodeAnalysis;
using System.Text;
using PineGuard.Common;
using PineGuard.MustClauses;

namespace PineGuard.GuardClauses;

/// <summary>
/// Describes one <c>Guard.Against.*</c> failure: the stable code and message of the rule that failed,
/// the parameter it failed against, and the default exception <see cref="Throw"/> would raise.
/// </summary>
/// <param name="Code">The stable, machine-readable identity of the rule that failed.</param>
/// <param name="Message">The rendered, human-readable failure message.</param>
/// <param name="ParamName">The name of the parameter that failed validation, or <see langword="null"/> if unknown.</param>
/// <param name="Value">
/// The attempted value. Never serialized by any adapter — a value that may hold a secret must not reach a
/// response body, a log line, or a localisation table through this property. See <see cref="MustFailure.Value"/>.
/// </param>
/// <param name="Exception">
/// The default exception <see cref="Throw"/> raises absent an active <see cref="GuardExceptionPolicy"/> map:
/// an <see cref="ArgumentNullException"/> when <see cref="Value"/> is <see langword="null"/>, otherwise an
/// <see cref="ArgumentException"/>. A <see cref="GuardExceptionPolicy"/> map receives this record and returns
/// the exception to throw in its place.
/// </param>
/// <seealso cref="GuardExceptionPolicy"/>
/// <seealso cref="ExceptionExtension"/>
/// <seealso href="https://pineguard.ai/docs/guard">Guard Clauses documentation</seealso>
public sealed record GuardFailure(string Code, string Message, string? ParamName, object? Value, Exception Exception)
{
    /// <summary>
    /// The <see cref="Exception.Data"/> key <see cref="Throw"/> stamps with <see cref="IMustResult.Code"/>.
    /// </summary>
    public const string CodeDataKey = "pineguard.code";

    /// <summary>
    /// The <see cref="Exception.Data"/> key <see cref="Throw"/> stamps with <see cref="IMustResult.ParamName"/>.
    /// </summary>
    public const string PropertyPathDataKey = "pineguard.property-path";

    /// <summary>
    /// Raises an exception for a failed <see cref="IMustResult"/>.
    /// </summary>
    /// <param name="result">The failed result to raise an exception for.</param>
    /// <param name="message">An optional message overriding <paramref name="result"/>'s own.</param>
    /// <param name="exceptionCreator">
    /// An optional factory for a custom exception. When it returns non-<see langword="null"/>, that exception
    /// is thrown as-is — an explicit per-call choice always wins over the active <see cref="GuardExceptionPolicy"/> map.
    /// </param>
    /// <remarks>
    /// Every thrown exception has <see cref="CodeDataKey"/> stamped with <paramref name="result"/>'s
    /// <see cref="IMustResult.Code"/>, and (when known) <see cref="PropertyPathDataKey"/> stamped with its
    /// <see cref="IMustResult.ParamName"/>. Read them back via <see cref="ExceptionExtension"/>.
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="result"/> is <see langword="null"/>, or when the active
    /// <see cref="GuardExceptionPolicy"/> map returns <see langword="null"/>.
    /// </exception>
    /// <exception cref="Exception">
    /// Always thrown when <paramref name="result"/> represents a failure. The exact type depends on
    /// <paramref name="exceptionCreator"/>, the active <see cref="GuardExceptionPolicy"/> map, and whether
    /// <paramref name="result"/>'s value is <see langword="null"/>.
    /// </exception>
    [DoesNotReturn]
    public static void Throw(IMustResult result, string? message = null, Func<Exception>? exceptionCreator = null)
    {
        ThrowHelper.ThrowIfNull(result);

        var explicitException = exceptionCreator?.Invoke();
        if (explicitException is not null)
        {
            Stamp(explicitException, result);
            throw explicitException;
        }

        var defaultException = CreateDefaultException(message ?? result.Message, result.ParamName, result.Value);
        Stamp(defaultException, result);

        var map = GuardExceptionPolicy.GetEffectiveMap();
        if (map is null)
            throw defaultException;

        var failure = new GuardFailure(result.Code, message ?? result.Message, result.ParamName, result.Value, defaultException);
        var mappedException = map(failure);
        ThrowHelper.ThrowIfNull(mappedException);
        Stamp(mappedException, result);
        throw mappedException;
    }

    private static void Stamp(Exception exception, IMustResult result)
    {
        exception.Data[CodeDataKey] = result.Code;
        if (result.ParamName is not null)
            exception.Data[PropertyPathDataKey] = result.ParamName;
    }

    private static Exception CreateDefaultException(string message, string? paramName, object? value) =>
        value is null
            ? new ArgumentNullException(paramName, message)
            : new ArgumentException(message, paramName);

    /// <summary>
    /// Overrides the compiler-generated record printer to omit <see cref="Value"/> — see the same
    /// caution on <see cref="MustFailure.Value"/>: it must never reach a log line via <see cref="ToString"/>
    /// or string interpolation (e.g. <c>$"{failure}"</c>).
    /// </summary>
    private bool PrintMembers(StringBuilder builder)
    {
        builder.Append(nameof(Code)).Append(" = ").Append(Code);
        builder.Append(", ").Append(nameof(Message)).Append(" = ").Append(Message);
        builder.Append(", ").Append(nameof(ParamName)).Append(" = ").Append(ParamName);
        builder.Append(", ").Append(nameof(Exception)).Append(" = ").Append(Exception);
        return true;
    }
}
