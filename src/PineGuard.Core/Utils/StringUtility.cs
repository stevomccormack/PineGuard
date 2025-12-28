namespace PineGuard.Utils;

public static class StringUtility
{
    public static bool TryGetNonEmptyTrimmed(string? value, out string trimmed)
    {
        trimmed = string.Empty;

        if (string.IsNullOrWhiteSpace(value))
            return false;

        trimmed = value.Trim();
        return trimmed.Length != 0;
    }
}
