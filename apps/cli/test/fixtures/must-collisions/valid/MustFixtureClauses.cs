using System;
using System.Runtime.CompilerServices;

namespace PineGuard.MustClauses;

/// <summary>
/// Fixture Must clauses for the must-collisions VIBE valid/ case (plan
/// docs/ai/plans/audit-cli-rebuild.md §4.6): every overload pair below is
/// safely distinguishable, so the rule must report zero findings.
/// </summary>
public static class MustFixtureClauses
{
    /// <summary>
    /// Different arity: the second overload requires an extra, non-defaulted
    /// parameter, so a bare `Must.Be.DigitsOnly(null)` call can only ever
    /// resolve to this one-parameter overload — not ambiguous. Mirrors the
    /// real MustStringClauses.DigitsOnly/NotDigitsOnly overload pair.
    /// </summary>
    public static MustResult<string> DigitsOnly(this IMustClause _,
        string? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null) =>
        throw new NotSupportedException();

    public static MustResult<string> DigitsOnly(this IMustClause _,
        string? value,
        char[]? allowedNonDigitChars,
        [CallerArgumentExpression(nameof(value))] string? paramName = null) =>
        throw new NotSupportedException();

    /// <summary>
    /// Same arity (one caller-supplied parameter), but only one overload's
    /// first parameter type can ever accept a null literal: `int length` is a
    /// non-nullable value type, so `Must.Be.ExactLength(null)` only ever
    /// resolves to the `string? value` overload below. Not ambiguous.
    /// </summary>
    public static MustResult<string> ExactLength(this IMustClause _,
        string? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null) =>
        throw new NotSupportedException();

    public static MustResult<int> ExactLength(this IMustClause _,
        int length,
        [CallerArgumentExpression(nameof(length))] string? paramName = null) =>
        throw new NotSupportedException();
}
