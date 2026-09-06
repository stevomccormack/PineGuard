using System;
using System.Runtime.CompilerServices;

namespace PineGuard.MustClauses;

/// <summary>
/// Boundary probe (plan §4.6): the mirror image of
/// StructConstrainedValid.cs — a generic primary parameter still constrained
/// `where T : struct`, but declared nullable (`T?`). This MUST be flagged:
/// the struct constraint puts it in the value-type bucket, and value types
/// must be non-nullable, regardless of the fact that the parameter happens
/// to be generic rather than a concrete type name.
/// </summary>
public static class StructConstrainedInvalidClauses
{
    public static MustResult<T> Defined<T>(this IMustClause _,
        T? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where T : struct, Enum =>
        throw new NotSupportedException();
}
