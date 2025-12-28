namespace PineGuard.Utils;

public static class GuidUtility
{
    public static bool TryParse(string? value, out Guid guid)
    {
        guid = default;

        if (!StringUtility.TryGetNonEmptyTrimmed(value, out var trimmed))
            return false;

        return Guid.TryParse(trimmed, out guid);
    }
}
