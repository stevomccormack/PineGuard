namespace PineGuard.Rules;

public static class GuidRules
{
    public static bool IsEmpty(Guid? value) => value is not null && value.Value == Guid.Empty;

    public static bool IsNullOrEmpty(Guid? value) => value is null || value.Value == Guid.Empty;

    public static bool IsGuid(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return false;

        return Guid.TryParse(value, out _);
    }
}
