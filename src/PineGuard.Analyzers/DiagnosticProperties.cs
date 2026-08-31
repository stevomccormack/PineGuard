using System.Collections.Immutable;

namespace PineGuard.Analyzers;

/// <summary>
/// Keys for the property bag a diagnostic carries from an analyzer to its code fix.
/// </summary>
/// <remarks>
/// The fix reads the guarded identifier from here rather than re-deriving it from syntax, so the
/// shape recognition lives in exactly one place.
/// </remarks>
internal static class DiagnosticProperties
{
    /// <summary>
    /// The name of the identifier the reported check guards.
    /// </summary>
    internal const string Identifier = nameof(Identifier);

    /// <summary>
    /// Creates the property bag carrying <paramref name="identifier"/> to the code fix.
    /// </summary>
    /// <param name="identifier">The name of the guarded identifier.</param>
    /// <returns>A property bag holding <paramref name="identifier"/> under <see cref="Identifier"/>.</returns>
    internal static ImmutableDictionary<string, string?> ForIdentifier(string identifier) =>
        ImmutableDictionary<string, string?>.Empty.Add(Identifier, identifier);
}
