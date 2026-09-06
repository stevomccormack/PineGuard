using System;
using System.Runtime.CompilerServices;

namespace PineGuard.MustClauses;

/// <summary>
/// Boundary probe (plan §4.6): a generic primary parameter constrained
/// `where T : struct`, declared non-nullable. Per plan §8 this is the
/// trickiest part of the hybrid policy to get right — a syntax-only rule has
/// no semantic model to ask "is T a value type", only the method's own
/// `where` clause. This must NOT be flagged: `struct` is exactly the
/// constraint plan §8 says to treat as the value-type bucket, so a bare
/// (non-nullable) `T` here is correct.
/// </summary>
public static class StructConstrainedValidClauses
{
    public static MustResult<T> Defined<T>(this IMustClause _,
        T value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where T : struct, Enum =>
        throw new NotSupportedException();
}
