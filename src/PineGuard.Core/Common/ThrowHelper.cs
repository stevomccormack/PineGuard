using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace PineGuard.Common;

/// <summary>
/// Provides shared argument-validation helpers that throw standard BCL exceptions on invalid input.
/// </summary>
public static class ThrowHelper
{
    /// <summary>
    /// Throws an <see cref="ArgumentNullException"/> when <paramref name="argument"/> is <see langword="null"/>.
    /// </summary>
    /// <param name="argument">The argument to validate.</param>
    /// <param name="paramName">The argument name. Defaults to the caller expression when omitted.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ThrowIfNull(
        [NotNull] object? argument,
        [CallerArgumentExpression(nameof(argument))] string? paramName = null)
    {
#if NET8_0_OR_GREATER
        ArgumentNullException.ThrowIfNull(argument, paramName);
#else
        if (argument is null) throw new ArgumentNullException(paramName);
#endif
    }

    /// <summary>
    /// Throws an <see cref="ArgumentException"/> when <paramref name="argument"/> is <see langword="null"/> or whitespace.
    /// </summary>
    /// <param name="argument">The argument to validate.</param>
    /// <param name="paramName">The argument name. Defaults to the caller expression when omitted.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ThrowIfNullOrWhiteSpace(
        [NotNull] string? argument,
        [CallerArgumentExpression(nameof(argument))] string? paramName = null)
    {
#if NET8_0_OR_GREATER
        ArgumentException.ThrowIfNullOrWhiteSpace(argument, paramName);
#else
        if (argument is null) throw new ArgumentNullException(paramName);
        if (string.IsNullOrWhiteSpace(argument)) throw new ArgumentException("The value cannot be null or whitespace.", paramName);
#endif
    }
}

