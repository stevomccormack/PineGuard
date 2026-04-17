using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace PineGuard.Testing.Common;

internal static class ThrowHelper
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static void ThrowIfNull(
        [NotNull] object? argument,
        [CallerArgumentExpression(nameof(argument))] string? paramName = null) =>
        ArgumentNullException.ThrowIfNull(argument, paramName);
}
