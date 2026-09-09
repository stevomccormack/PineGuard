using System.Runtime.CompilerServices;
using PineGuard.Codes;
using PineGuard.Rules;

namespace PineGuard.MustClauses;

/// <summary>
/// Provides <see cref="IMustClause"/> extension methods that validate delta values.
/// Canonical order: Instance, then Past.
///
/// BOUNDARY: DeltaRules pairs an `IsInstance` method (which must normalise
/// to "Instance", NOT be further mangled by the Rules-layer "In"-prefix
/// stripping into "stance") with an `InPast` method (which SHOULD be
/// stripped down to "Past", since that's the one narrow whitelisted case
/// the normalisation table's "In" handling targets). Both must line up with
/// Must's own Instance/Past order for this fixture to stay clean — a naive,
/// non-word-boundary-safe "strip any leading In" implementation would wrongly
/// turn "IsInstance" into "stance" and silently drop it out of the
/// comparison, which this fixture would not catch as a mismatch by itself,
/// but the direct unit tests in ordering.test.ts assert the normalisation
/// function's output for these exact inputs.
/// </summary>
public static class MustDeltaClauses
{
    public static MustResult<string> Instance(
        this IMustClause _,
        string value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var ok = DeltaRules.IsInstance(value);
        return MustResult<string>.FromBool(ok, MustCodes.Delta.Instancing.Instance, "{paramName} must be a known instance.", paramName, value, value);
    }

    public static MustResult<string> Past(
        this IMustClause _,
        string value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var ok = DeltaRules.InPast(value);
        return MustResult<string>.FromBool(ok, MustCodes.Delta.Timing.Past, "{paramName} must be in the past.", paramName, value, value);
    }
}
