using Microsoft.CodeAnalysis;

namespace PineGuard.Analyzers;

/// <summary>
/// Pairs a <c>PG1xxx</c> descriptor with the name of the guard clause its code fix emits.
/// </summary>
/// <remarks>
/// This is the single place that states which <c>Guard.Against</c> clause replaces which
/// hand-rolled check. The analyzer writes <see cref="Clause"/> into the diagnostic's property bag
/// so the fix never has to map an identifier back to a clause name.
/// </remarks>
internal sealed class GuardSuggestion
{
    /// <summary>
    /// PG1001 — <c>Guard.Against.Null</c>.
    /// </summary>
    internal static readonly GuardSuggestion Null = new(DiagnosticDescriptors.UseGuardAgainstNull, "Null");

    /// <summary>
    /// PG1002 — <c>Guard.Against.NullOrWhiteSpace</c>.
    /// </summary>
    internal static readonly GuardSuggestion NullOrWhiteSpace = new(DiagnosticDescriptors.UseGuardAgainstNullOrWhiteSpace, "NullOrWhiteSpace");

    /// <summary>
    /// PG1003 — <c>Guard.Against.NullOrEmpty</c>.
    /// </summary>
    internal static readonly GuardSuggestion NullOrEmpty = new(DiagnosticDescriptors.UseGuardAgainstNullOrEmpty, "NullOrEmpty");

    /// <summary>
    /// PG1004 — <c>Guard.Against.OutOfRange</c>.
    /// </summary>
    internal static readonly GuardSuggestion OutOfRange = new(DiagnosticDescriptors.UseGuardAgainstOutOfRange, "OutOfRange");

    private GuardSuggestion(DiagnosticDescriptor descriptor, string clause)
    {
        Descriptor = descriptor;
        Clause = clause;
    }

    /// <summary>
    /// Gets the descriptor reported when this suggestion applies.
    /// </summary>
    internal DiagnosticDescriptor Descriptor { get; }

    /// <summary>
    /// Gets the <c>Guard.Against</c> clause name the code fix calls.
    /// </summary>
    internal string Clause { get; }
}
