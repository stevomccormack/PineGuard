namespace PineGuard.Rules;

public static class GuidRules
{
    public static bool IsGuid(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return false;
        }

        return Guid.TryParse(value, out _);
    }

    public static bool IsNotGuid(string? value) => !IsGuid(value);

    public static bool IsEmpty(Guid value) => value == Guid.Empty;

    public static bool IsNotEmpty(Guid value) => value != Guid.Empty;

    public static bool IsNullOrEmpty(Guid? value) => value is null || value.Value == Guid.Empty;

    public static bool IsNotNullOrEmpty(Guid? value) => !IsNullOrEmpty(value);
}
