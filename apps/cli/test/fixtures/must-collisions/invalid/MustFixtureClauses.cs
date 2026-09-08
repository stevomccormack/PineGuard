using System;
using System.Runtime.CompilerServices;

namespace PineGuard.MustClauses;

/// <summary>
/// Fixture Must clauses for the must-collisions VIBE invalid/ case (plan
/// docs/ai/plans/audit-cli-rebuild.md §4.6): two same-arity overloads of
/// `Foo` whose first parameter types (`string?` and `int?`) both accept a
/// null literal, so `Must.Be.Foo(null)` is a genuine overload-resolution
/// ambiguity (CS0121) at any real call site.
/// </summary>
public static class MustFixtureClauses
{
    public static MustResult<string> Foo(this IMustClause _,
        string? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null) =>
        throw new NotSupportedException();

    public static MustResult<int> Foo(this IMustClause _,
        int? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null) =>
        throw new NotSupportedException();
}
