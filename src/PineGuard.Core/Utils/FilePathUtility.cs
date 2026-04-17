namespace PineGuard.Utils;

/// <summary>
/// Provides file path validation utility methods.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/utils/filepath">FilePath Utility documentation</seealso>
public static class FilePathUtility
{
    // Union of platform-specific and Windows-specific invalid file name characters so that
    // validation is consistent across Windows (NLS) and Linux/macOS (where Path.GetInvalidFileNameChars
    // returns only '/' and '\0', omitting Windows chars such as '\', ':', '*', '?', etc.).
    private static readonly char[] InvalidFileNameChars =
        Path.GetInvalidFileNameChars()
            .Union(['"', '<', '>', '|', ':', '*', '?', '\\'])
            .ToArray();

    /// <summary>
    /// Determines whether the specified string contains characters that are invalid in a file name.
    /// </summary>
    /// <param name="value">The value to validate. If <see langword="null"/> or empty, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if the value contains invalid file name characters; otherwise, <see langword="false"/>.</returns>
    public static bool ContainsInvalidFileNameChars(string? value)
    {
        if (string.IsNullOrEmpty(value))
            return false;

        return value.IndexOfAny(InvalidFileNameChars) >= 0;
    }

    /// <summary>
    /// Determines whether the specified string is a Windows reserved device name (e.g., CON, PRN, AUX, NUL, COM1-9, LPT1-9).
    /// </summary>
    /// <param name="value">The value to validate. If <see langword="null"/> or whitespace, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if the value is a Windows reserved device name; otherwise, <see langword="false"/>.</returns>
    public static bool IsWindowsReservedDeviceName(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return false;

        var trimmed = value.Trim();

        // Windows reserved device names are reserved even with extensions (e.g., "CON.txt")
        var dot = trimmed.IndexOf('.');
        var baseName = dot < 0 ? trimmed : trimmed[..dot];

        return baseName.Equals("CON", StringComparison.OrdinalIgnoreCase)
            || baseName.Equals("PRN", StringComparison.OrdinalIgnoreCase)
            || baseName.Equals("AUX", StringComparison.OrdinalIgnoreCase)
            || baseName.Equals("NUL", StringComparison.OrdinalIgnoreCase)
            || IsComPort(baseName)
            || IsLptPort(baseName);
    }

    private static bool IsComPort(string baseName)
    {
        if (!baseName.StartsWith("COM", StringComparison.OrdinalIgnoreCase))
            return false;

        if (baseName.Length != 4)
            return false;

        return baseName[3] is >= '1' and <= '9';
    }

    private static bool IsLptPort(string baseName)
    {
        if (!baseName.StartsWith("LPT", StringComparison.OrdinalIgnoreCase))
            return false;

        if (baseName.Length != 4)
            return false;

        return baseName[3] is >= '1' and <= '9';
    }
}
