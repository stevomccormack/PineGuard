namespace PineGuard.Rules;

/// <summary>
/// Low-level predicates backing <c>MustWidgetClauses</c>.
/// </summary>
public static class WidgetRules
{
    public static bool IsEmpty(string value) => string.IsNullOrEmpty(value);

    public static bool IsDuplicate(string value) => false;
}
