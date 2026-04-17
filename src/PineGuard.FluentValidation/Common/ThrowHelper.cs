using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace PineGuard.FluentValidation.Common;

internal static class ThrowHelper
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static void ThrowIfNull(
        [NotNull] object? argument,
        [CallerArgumentExpression(nameof(argument))] string? paramName = null)
    {
#if NET8_0_OR_GREATER
        ArgumentNullException.ThrowIfNull(argument, paramName);
#else
        if (argument is null) throw new ArgumentNullException(paramName);
#endif
    }
}
