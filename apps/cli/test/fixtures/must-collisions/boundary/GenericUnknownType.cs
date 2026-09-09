using System;
using System.Runtime.CompilerServices;

namespace PineGuard.MustClauses;

/// <summary>
/// Boundary probe (plan §4.6): a generic overload (`T value`) alongside a
/// concrete, null-accepting overload (`string? value`) of the same name and
/// arity. `T` is an unconstrained generic type parameter — whether it accepts
/// null depends on a `where` constraint this syntax-only rule does not
/// resolve, so it is classified "unknown" and skipped rather than guessed at
/// (see `classifyNullAcceptance`'s doc comment). With only one classifiable
/// "accepts" overload in the group, this must NOT be flagged — proves the
/// rule stays conservative around unclassifiable generic types instead of
/// fabricating a collision from a guess.
/// </summary>
public static class GenericUnknownTypeClauses
{
    public static MustResult<T> Custom<T>(this IMustClause _,
        T value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null) =>
        throw new NotSupportedException();

    public static MustResult<string> Custom(this IMustClause _,
        string? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null) =>
        throw new NotSupportedException();
}
