using System;
using System.Runtime.CompilerServices;

namespace PineGuard.MustClauses;

/// <summary>
/// Boundary probe (plan §4.6): two overloads of the same name whose first
/// parameters both accept null, but only one is reachable with a single
/// argument. The two-parameter overload's second parameter (`upperBound`)
/// has no default, so `Must.Be.Between(null)` can only ever bind to the
/// one-parameter overload below — it is not even a candidate, let alone a
/// colliding one. Mirrors a real pair in the codebase:
/// `MustStringNumbersClauses.GreaterThan(string? value, decimal min, ...)`
/// vs. `MustStringTimeSpanClauses.GreaterThan(string? value, TimeSpan
/// threshold, ...)` — same first-parameter type, same total parameter count,
/// but neither `min` nor `threshold` has a default, so neither overload is
/// reachable by `Must.Be.GreaterThan(null)` at all. Proves the rule keys
/// collisions off single-argument reachability, not just "same parameter
/// count and first parameter accepts null".
/// </summary>
public static class DifferentArityClauses
{
    public static MustResult<string?> Between(this IMustClause _,
        string? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null) =>
        throw new NotSupportedException();

    public static MustResult<string?> Between(this IMustClause _,
        string? value,
        string? upperBound,
        [CallerArgumentExpression(nameof(value))] string? paramName = null) =>
        throw new NotSupportedException();
}
