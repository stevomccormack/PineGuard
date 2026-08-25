namespace PineGuard.Utils;

/// <summary>
/// Provides file path validation utility methods.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/utils/filepath">FilePath Utility documentation</seealso>
public static class FilePathUtility
{
    // Union of platform-specific and Windows-specific invalid file name characters, plus the full
    // ASCII control range, so that validation is genuinely consistent across Windows (NLS) and
    // Linux/macOS. Path.GetInvalidFileNameChars() returns only '/' and '\0' on Linux/macOS, omitting
    // both the printable Windows-invalid chars ('\', ':', '*', '?', etc.) and control characters
    // 0x01-0x1F that Windows also rejects; both are added explicitly here so the resulting set does
    // not depend on the OS the code happens to run on.
    private static readonly char[] InvalidFileNameChars =
        Path.GetInvalidFileNameChars()
            .Union(['"', '<', '>', '|', ':', '*', '?', '\\'])
            .Union(Enumerable.Range(0x00, 0x20).Select(codePoint => (char)codePoint))
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

        // Windows reserved device names are reserved even with extensions (e.g., "CON.txt"), and
        // Win32 name normalization strips trailing spaces/dots from the base name before matching
        // a reserved device, so "CON .txt" also resolves to the CON device.
        var dot = trimmed.IndexOf('.');
        var baseName = (dot < 0 ? trimmed : trimmed[..dot]).TrimEnd(' ', '.');

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
