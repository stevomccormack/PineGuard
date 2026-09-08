using System;
using System.Runtime.CompilerServices;

namespace PineGuard.MustClauses;

/// <summary>
/// Fixture Must clauses for the nullability VIBE invalid/ case (plan
/// docs/ai/plans/audit-cli-rebuild.md §4.6): every method below violates the
/// hybrid nullability policy, so the rule must report at least one finding.
/// This is the exact fixture shape that would have caught the legacy Rule07
/// bug (plan §2.2) — the old regex-based parser never matched a single real
/// parameter, so it reported zero violations against files just like this
/// one, forever.
/// </summary>
public static class MustFixtureClauses
{
    /// <summary>
    /// Reference type (string) declared non-nullable — violates "reference
    /// types must be declared nullable".
    /// </summary>
    public static MustResult<string> NotNullOrEmpty(this IMustClause _,
        string value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null) =>
        throw new NotSupportedException();

    /// <summary>
    /// Value type (Guid) declared nullable — violates "value types must be
    /// declared non-nullable".
    /// </summary>
    public static MustResult<Guid> NotEmpty(this IMustClause _,
        Guid? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null) =>
        throw new NotSupportedException();

    /// <summary>
    /// Same violation shape as <see cref="NotNullOrEmpty"/>, under a distinct
    /// method name reserved for the exemption test in
    /// test/rules/nullability.test.ts — proves `config/exceptions.json`'s
    /// `nullability` entry can suppress a specific method's finding by name
    /// substring without touching every other finding.
    /// </summary>
    public static MustResult<string> LegacyRawValue(this IMustClause _,
        string value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null) =>
        throw new NotSupportedException();
}
