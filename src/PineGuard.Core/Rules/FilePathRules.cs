using PineGuard.Utils;

namespace PineGuard.Rules;

/// <summary>
/// Provides pure file path and file name validation predicates.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/rules/filepath">File Path Rules documentation</seealso>
public static class FilePathRules
{
    /// <summary>
    /// Determines whether the specified value is a safe file name.
    /// </summary>
    /// <param name="value">The value to validate. If <see langword="null"/> or whitespace, returns <see langword="false"/>.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="value"/> is a non-empty string that does not contain
    /// invalid file name characters, is not a Windows reserved device name (e.g., <c>CON</c>, <c>NUL</c>),
    /// is not <c>.</c> or <c>..</c>, has no leading or trailing whitespace, and does not end with a
    /// period; otherwise, <see langword="false"/>. Leading/trailing whitespace and a trailing period
    /// are both silently stripped by Win32 file name normalization, so both are rejected here rather
    /// than validated as if already normalized.
    /// </returns>
    /// <example>
    /// <code>
    /// bool safe = FilePathRules.IsSafeFileName("my-document.pdf"); // true
    /// bool unsafe = FilePathRules.IsSafeFileName("CON");           // false (reserved)
    /// bool unsafe = FilePathRules.IsSafeFileName("file?.txt");     // false (invalid char)
    /// bool unsafe = FilePathRules.IsSafeFileName("file.txt ");     // false (trailing whitespace)
    /// </code>
    /// </example>
    public static bool IsSafeFileName(string? value)
    {
        if (!StringUtility.TryGetTrimmed(value, out var trimmed))
            return false;

        if (trimmed is "." or "..")
            return false;

        if (trimmed.EndsWith('.') || trimmed.Length != value!.Length)
            return false;

        if (FilePathUtility.ContainsInvalidFileNameChars(trimmed))
            return false;

        return !FilePathUtility.IsWindowsReservedDeviceName(trimmed);
    }

    /// <summary>
    /// Determines whether the specified path has one of the allowed file extensions.
    /// </summary>
    /// <param name="path">The file path to validate. If <see langword="null"/> or whitespace, returns <see langword="false"/>.</param>
    /// <param name="allowed">
    /// The set of allowed extensions. Extensions may be specified with or without a leading dot
    /// (e.g., <c>".pdf"</c> and <c>"pdf"</c> are both accepted). If <see langword="null"/> or empty,
    /// returns <see langword="false"/>.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if the file extension of <paramref name="path"/> matches one of the
    /// <paramref name="allowed"/> extensions (case-insensitive); otherwise, <see langword="false"/>.
    /// </returns>
    /// <example>
    /// <code>
    /// bool valid = FilePathRules.HasFileExtension("report.pdf", ".pdf", ".docx"); // true
    /// bool valid = FilePathRules.HasFileExtension("image.jpg", "jpg");            // true
    /// bool invalid = FilePathRules.HasFileExtension("script.exe", ".pdf");        // false
    /// </code>
    /// </example>
    public static bool HasFileExtension(string? path, params string[]? allowed)
    {
        if (!StringUtility.TryGetTrimmed(path, out var trimmed))
            return false;

        if (allowed is null || allowed.Length == 0)
            return false;

        var extension = Path.GetExtension(trimmed);
        if (string.IsNullOrWhiteSpace(extension))
            return false;

        foreach (var candidate in allowed)
        {
            if (!StringUtility.TryGetTrimmed(candidate, out var candidateTrimmed))
                continue;

            var normalized = candidateTrimmed.StartsWith('.') ? candidateTrimmed : "." + candidateTrimmed;

            if (string.Equals(extension, normalized, StringComparison.OrdinalIgnoreCase))
                return true;
        }

        return false;
    }
}
