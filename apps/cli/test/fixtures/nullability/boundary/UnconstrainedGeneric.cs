using System;
using System.Runtime.CompilerServices;

namespace PineGuard.MustClauses;

/// <summary>
/// Boundary probe (plan §4.6): a generic primary parameter with no `where`
/// clause at all, declared nullable. Per plan §8, an unconstrained generic
/// defaults to the reference-type bucket exactly like a `class` constraint,
/// so a nullable `T?` here is correct and must NOT be flagged — even though
/// T could be instantiated with a value type at some call site, PineGuard's
/// convention (mirrored by the real MustObjectClauses.EqualTo signature)
/// treats the unconstrained case as reference-shaped.
/// </summary>
public static class UnconstrainedGenericClauses
{
    public static MustResult<T> EqualTo<T>(this IMustClause _,
        T? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null) =>
        throw new NotSupportedException();
}
