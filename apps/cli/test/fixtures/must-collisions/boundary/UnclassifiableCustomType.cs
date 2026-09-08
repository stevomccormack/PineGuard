using System;
using System.Runtime.CompilerServices;

namespace PineGuard.MustClauses;

/// <summary>
/// Boundary probe (plan §4.6), directly motivated by a real false positive
/// found while writing this rule against the actual repo: two overloads of
/// the same name and same single-argument reachability, whose first
/// parameter is a bare custom type name with no `?` and no `I`-prefix —
/// `SampleRange`, standing in for the real
/// `MustDateOnlyRangeClauses.Chronological(this IMustClause _, DateOnlyRange
/// range, ...)` and its three siblings (`DateTimeRange`/
/// `DateTimeOffsetRange`/`TimeOnlyRange`). `DateOnlyRange` is a `public
/// readonly struct` (`src/PineGuard.Core/Common/DateOnlyRange.cs`) — a value
/// type that does NOT accept `null` — but nothing in the bare name
/// `DateOnlyRange` says so syntactically. This rule classifies any such
/// unrecognised, non-`I`-prefixed custom type name as `"unknown"` rather
/// than guessing "reference type", so this must NOT be flagged even though
/// two same-named, same-shape overloads exist. Proves the rule stays
/// conservative around custom struct types instead of fabricating a
/// collision from a name it cannot read confidently.
/// </summary>
public static class UnclassifiableCustomTypeClauses
{
    public static MustResult<SampleRange> Chronological(this IMustClause _,
        SampleRange range,
        [CallerArgumentExpression(nameof(range))] string? paramName = null) =>
        throw new NotSupportedException();
}

public static class UnclassifiableCustomTypeSiblingClauses
{
    public static MustResult<OtherSampleRange> Chronological(this IMustClause _,
        OtherSampleRange range,
        [CallerArgumentExpression(nameof(range))] string? paramName = null) =>
        throw new NotSupportedException();
}
