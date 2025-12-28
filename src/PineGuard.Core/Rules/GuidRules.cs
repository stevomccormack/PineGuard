using PineGuard.Utils;

namespace PineGuard.Rules;

public static class GuidRules
{
    public static bool IsEmpty(Guid? value) => value is not null && value.Value == Guid.Empty;

    public static bool IsNullOrEmpty(Guid? value) => value is null || value.Value == Guid.Empty;

    public static bool IsGuid(string? value) => GuidUtility.TryParse(value, out _);

    public static bool IsGuidEmpty(string? value)
    {
        if (!GuidUtility.TryParse(value, out var guid))
            return false;

        return guid == Guid.Empty;
    }
}
