namespace PineGuard.Rules;

/// <summary>
/// Low-level predicates backing <c>MustAlphaClauses</c>.
/// Kept in Must's Empty-then-Duplicate order so this layer stays clean —
/// only GuardAlphaClauses is reordered in this fixture.
/// </summary>
public static class AlphaRules
{
    public static bool IsEmpty(string value) => string.IsNullOrEmpty(value);

    public static bool IsDuplicate(string value) => false;
}
