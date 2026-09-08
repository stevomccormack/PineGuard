using System;
using System.Runtime.CompilerServices;

namespace PineGuard.MustClauses;

/// <summary>
/// Boundary probe (plan §4.6): a generic primary parameter constrained
/// `where T : class`, declared nullable. Per plan §8, only a `struct`
/// constraint moves a generic parameter into the value-type bucket — a
/// `class` constraint is still the reference-type default, so a nullable
/// `T?` here is correct and must NOT be flagged.
/// </summary>
public static class ClassConstrainedClauses
{
    public static MustResult<T> SameReferenceAs<T>(this IMustClause _,
        T? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where T : class =>
        throw new NotSupportedException();
}
