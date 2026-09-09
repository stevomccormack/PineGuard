namespace PineGuard.Rules;

/// <summary>
/// Low-level predicates backing <c>MustDeltaClauses</c>.
///
/// BOUNDARY: `IsInstance` strips the Rules-layer "Is" prefix down to
/// "Instance" — and "Instance" itself starts with "In", but must NOT be
/// further stripped by the narrow "InPast"/"InFuture"-style whitelist (its
/// remainder, "stance", is not one of the whitelisted concepts). `InPast`,
/// by contrast, is exactly the whitelisted shape and SHOULD normalise down
/// to "Past". Kept in Must's Instance-then-Past order.
/// </summary>
public static class DeltaRules
{
    public static bool IsInstance(string value) => true;

    public static bool InPast(string value) => true;
}
