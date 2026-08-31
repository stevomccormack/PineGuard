namespace PineGuard.Analyzers;

/// <summary>
/// The diagnostic identifiers reported by the PineGuard analyzers.
/// </summary>
/// <remarks>
/// <c>PG1xxx</c> means <em>prefer a guard clause</em>; <c>PG2xxx</c> means <em>guard or validation
/// misuse</em>. The <c>PG</c> prefix is unclaimed by the well-known diagnostic families.
/// </remarks>
internal static class DiagnosticIds
{
    /// <summary>
    /// PG1001 — a hand-rolled null check that <c>Guard.Against.Null</c> already expresses.
    /// </summary>
    internal const string UseGuardAgainstNull = "PG1001";
}
