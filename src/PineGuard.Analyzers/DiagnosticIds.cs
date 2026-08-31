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

    /// <summary>
    /// PG1002 — a hand-rolled null-or-whitespace check that <c>Guard.Against.NullOrWhiteSpace</c>
    /// already expresses.
    /// </summary>
    internal const string UseGuardAgainstNullOrWhiteSpace = "PG1002";

    /// <summary>
    /// PG1003 — a hand-rolled null-or-empty check that <c>Guard.Against.NullOrEmpty</c> already
    /// expresses.
    /// </summary>
    internal const string UseGuardAgainstNullOrEmpty = "PG1003";

    /// <summary>
    /// PG1004 — a hand-rolled range check that <c>Guard.Against.OutOfRange</c> already expresses.
    /// </summary>
    internal const string UseGuardAgainstOutOfRange = "PG1004";

    /// <summary>
    /// PG2001 — a <c>Must.Be</c> call whose <c>MustResult</c> is thrown away, so nothing checks it.
    /// </summary>
    internal const string DiscardedMustResult = "PG2001";

    /// <summary>
    /// PG2002 — a validator call whose <c>MustValidationResult</c> is thrown away, so nothing checks
    /// it.
    /// </summary>
    internal const string DiscardedMustValidationResult = "PG2002";
}
