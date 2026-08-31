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
    /// The arguments the fix passes to the clause, as the C# source of an argument list without its
    /// parentheses — <c>quantity, min, max</c> for a range guard, <c>name</c> for a null guard.
    /// </summary>
    /// <remarks>
    /// The arguments travel as source rather than as a delimited list because a bound may itself be
    /// a literal containing a separator; the fix parses them back with the C# parser.
    /// </remarks>
    internal const string Arguments = nameof(Arguments);

    private const string ArgumentSeparator = ", ";

    /// <summary>
    /// Creates the property bag carrying a guard and its arguments to the code fix.
    /// </summary>
    /// <param name="clause">The <c>Guard.Against</c> clause name.</param>
    /// <param name="arguments">The arguments to pass to the clause, in call order.</param>
    /// <returns>A property bag holding <paramref name="clause"/> and <paramref name="arguments"/>.</returns>
    internal static ImmutableDictionary<string, string?> ForGuard(string clause, params string[] arguments) =>
        ImmutableDictionary<string, string?>.Empty
            .Add(Clause, clause)
            .Add(Arguments, string.Join(ArgumentSeparator, arguments));
}
