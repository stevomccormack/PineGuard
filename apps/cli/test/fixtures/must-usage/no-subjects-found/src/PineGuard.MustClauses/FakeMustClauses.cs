using PineGuard.Core;

namespace PineGuard.MustClauses;

/// <summary>
/// P4.1 regression fixture (`must-usage:no-subjects-found`): the
/// `src/PineGuard.MustClauses` directory exists and has real content, but
/// nothing in it matches the extension-method shape `collectMustMethods`
/// looks for — no `public static` method whose first parameter is
/// `this IMustClause`. This proves the diagnostic finding fires because
/// detection genuinely found zero subjects, not merely because the
/// directory was empty or absent.
/// </summary>
public static class FakeMustClauses
{
    // Not an extension method at all (no `this` parameter) — does not
    // qualify as a Must clause under any circumstance.
    public static bool NotAnExtensionMethod(string value)
    {
        return value.Length > 0;
    }

    // `this`-qualified, but on the wrong type — Must clauses only ever
    // extend `IMustClause`.
    public static string WrongExtendedType(this string value)
    {
        return value;
    }
}
