using System.Diagnostics.CodeAnalysis;

namespace PineGuard.GuardClauses;

/// <summary>
/// Provides the exception-throwing infrastructure used by all <c>Guard.Against.*</c> methods.
/// </summary>
/// <remarks>
/// <para>
/// Guards call <see cref="Throw"/> after a failed <see cref="MustClauses.MustResult{T}"/> to raise
/// an appropriate exception. <see cref="ThrowAndReplace"/> is a public extension point for callers
/// that need a per-call exception replacer; it is not called by any built-in guard clause.
/// </para>
/// <para>
/// When the value is <see langword="null"/>, an <see cref="ArgumentNullException"/> is raised;
/// otherwise, an <see cref="ArgumentException"/> is raised.
/// Both are subject to overrides configured via <see cref="GuardExceptionPolicy"/>.
/// </para>
/// </remarks>
/// <seealso cref="GuardExceptionPolicy"/>
/// <seealso href="https://pineguard.ai/docs/guard">Guard Clauses documentation</seealso>
public static class GuardFailure
{
    /// <summary>
    /// Throws an exception for the given failure message, applying any active
    /// <see cref="GuardExceptionPolicy"/> replacer.
    /// </summary>
    /// <param name="message">The human-readable failure message.</param>
    /// <param name="paramName">The name of the parameter that failed validation.</param>
    /// <param name="value">The original value that failed validation. Determines the default exception type.</param>
    /// <param name="exceptionCreator">
    /// An optional factory that returns a custom exception. If <see langword="null"/>,
    /// the default <see cref="ArgumentException"/> or <see cref="ArgumentNullException"/> is created.
    /// </param>
    /// <exception cref="Exception">
    /// Always thrown. The exact type depends on <paramref name="exceptionCreator"/>, the configured
    /// <see cref="GuardExceptionPolicy"/>, and whether <paramref name="value"/> is <see langword="null"/>.
    /// </exception>
    [DoesNotReturn]
    public static void Throw(
        string message,
        string? paramName,
        object? value,
        Func<Exception>? exceptionCreator = null)
        => ThrowCore(message, paramName, value, exceptionCreator, exceptionReplacer: null);

    /// <summary>
    /// Throws an exception for the given failure message, applying a caller-supplied replacer
    /// instead of any active <see cref="GuardExceptionPolicy"/> replacer.
    /// </summary>
    /// <param name="message">The human-readable failure message.</param>
    /// <param name="paramName">The name of the parameter that failed validation.</param>
    /// <param name="value">The original value that failed validation.</param>
    /// <param name="exceptionCreator">
    /// An optional factory that returns a custom exception. If it returns non-<see langword="null"/>,
    /// that exception is thrown directly and neither <paramref name="exceptionReplacer"/> nor the
    /// global <see cref="GuardExceptionPolicy"/> replacer is applied.
    /// </param>
    /// <param name="exceptionReplacer">
    /// An optional factory that maps the default exception to a replacement. When supplied, it takes
    /// precedence over the global/scoped <see cref="GuardExceptionPolicy.ExceptionReplacer"/> — the
    /// policy replacer and its <see cref="GuardExceptionPolicy.ReplaceDefaultExceptions"/> gate are
    /// not consulted.
    /// </param>
    /// <exception cref="Exception">Always thrown.</exception>
    [DoesNotReturn]
    public static void ThrowAndReplace(
        string message,
        string? paramName,
        object? value,
        Func<Exception>? exceptionCreator = null,
        Func<Exception, Exception>? exceptionReplacer = null)
        => ThrowCore(message, paramName, value, exceptionCreator, exceptionReplacer);

    [DoesNotReturn]
    private static void ThrowCore(
        string message,
        string? paramName,
        object? value,
        Func<Exception>? exceptionCreator,
        Func<Exception, Exception>? exceptionReplacer)
    {
        var exception = exceptionCreator?.Invoke();
        if (exception is not null)
            throw exception;

        var defaultException = CreateDefaultException(message, paramName, value);

        if (exceptionReplacer is not null)
            throw exceptionReplacer(defaultException);

        var configuredReplacer = GuardExceptionPolicy.ExceptionReplacer;
        if (configuredReplacer is null || !GuardExceptionPolicy.ShouldReplace(defaultException))
            throw defaultException;

        throw configuredReplacer(defaultException);
    }

    private static Exception CreateDefaultException(string message, string? paramName, object? value) =>
        value is null
            ? new ArgumentNullException(paramName, message)
            : new ArgumentException(message, paramName);
}
