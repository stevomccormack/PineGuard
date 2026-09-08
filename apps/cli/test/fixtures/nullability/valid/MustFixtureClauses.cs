using System;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace PineGuard.MustClauses;

/// <summary>
/// Fixture Must clauses for the nullability VIBE valid/ case (plan
/// docs/ai/plans/audit-cli-rebuild.md §4.6): every primary parameter below
/// follows the hybrid nullability policy, so the rule must report zero
/// findings.
/// </summary>
public static class MustFixtureClauses
{
    /// <summary>Reference type (string), correctly nullable.</summary>
    public static MustResult<string> NotNullOrEmpty(this IMustClause _,
        string? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null) =>
        throw new NotSupportedException();

    /// <summary>Value type (Guid), correctly non-nullable.</summary>
    public static MustResult<Guid> NotEmpty(this IMustClause _,
        Guid value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null) =>
        throw new NotSupportedException();

    /// <summary>
    /// Generic primary parameter constrained `where T : struct` — treated as
    /// a value type, correctly non-nullable. Mirrors the real
    /// MustBitWiseClauses.BitwiseEqualTo signature.
    /// </summary>
    public static MustResult<T> BitwiseEqualTo<T>(this IMustClause _,
        T value,
        T other,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where T : struct, IBinaryInteger<T> =>
        throw new NotSupportedException();

    /// <summary>
    /// Generic primary parameter with no constraint at all — treated as a
    /// reference type (the default bucket), correctly nullable. Mirrors the
    /// real MustObjectClauses.EqualTo signature.
    /// </summary>
    public static MustResult<T> EqualTo<T>(this IMustClause _,
        T? value,
        T? other,
        [CallerArgumentExpression(nameof(value))] string? paramName = null) =>
        throw new NotSupportedException();

    /// <summary>
    /// Generic primary parameter constrained `where T : class` — still the
    /// reference-type bucket, correctly nullable. Mirrors the real
    /// MustObjectClauses.SameReferenceAs signature.
    /// </summary>
    public static MustResult<T> SameReferenceAs<T>(this IMustClause _,
        T? a,
        T? b,
        [CallerArgumentExpression(nameof(a))] string? paramName = null)
        where T : class =>
        throw new NotSupportedException();
}
