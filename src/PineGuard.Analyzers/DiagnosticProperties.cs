using System.Collections.Immutable;

namespace PineGuard.Analyzers;

/// <summary>
/// Keys for the property bag a diagnostic carries from an analyzer to its code fix.
/// </summary>
/// <remarks>
/// The fix reads the guard clause and its arguments from here rather than re-deriving them from
/// syntax, so the shape recognition lives in exactly one place.
/// </remarks>
internal static class DiagnosticProperties
{
    /// <summary>
    /// The <c>Guard.Against</c> clause name the fix calls.
    /// </summary>
    internal const string Clause = nameof(Clause);

    /// <summary>
    /// The name of the identifier the reported check guards.
    /// </summary>
    internal const string Identifier = nameof(Identifier);

    /// <summary>
    /// Creates the property bag carrying a single-argument guard to the code fix.
    /// </summary>
    /// <param name="clause">The <c>Guard.Against</c> clause name.</param>
    /// <param name="identifier">The name of the guarded identifier.</param>
    /// <returns>A property bag holding <paramref name="clause"/> and <paramref name="identifier"/>.</returns>
    internal static ImmutableDictionary<string, string?> ForGuard(string clause, string identifier) =>
        ImmutableDictionary<string, string?>.Empty
            .Add(Clause, clause)
            .Add(Identifier, identifier);
}
