namespace PineGuard.Utils;

/// <summary>
/// Provides magic-byte file signature lookup utility methods.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/utils/filesignature">File Signature Utility documentation</seealso>
public static class FileSignatureUtility
{
    /// <summary>
    /// The number of leading bytes that are sufficient to test every registered signature — the amount
    /// of a file a caller needs to read before detection can be conclusive.
    /// </summary>
    public const int MaxSignatureLength = 12;

    private static readonly byte?[] ZipLocalFileHeader = [0x50, 0x4B, 0x03, 0x04];
    private static readonly byte?[] ZipEmptyArchive = [0x50, 0x4B, 0x05, 0x06];
    private static readonly byte?[] ZipSpannedArchive = [0x50, 0x4B, 0x07, 0x08];

    // Order is significant: TryDetectExtension reports the first entry that matches, so a format
    // whose bytes are shared with another (an OOXML document is a ZIP archive; ".jpeg" is ".jpg")
    // is registered after the extension detection should report.
    private static readonly FileSignature[] Signatures =
    [
        new(".png", [0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A]),
        new(".jpg", [0xFF, 0xD8, 0xFF]),
        new(".jpeg", [0xFF, 0xD8, 0xFF]),
        new(".gif", [0x47, 0x49, 0x46, 0x38, 0x37, 0x61], [0x47, 0x49, 0x46, 0x38, 0x39, 0x61]),
        new(".bmp", [0x42, 0x4D]),
        new(".webp", [0x52, 0x49, 0x46, 0x46, null, null, null, null, 0x57, 0x45, 0x42, 0x50]),
        new(".tiff", [0x49, 0x49, 0x2A, 0x00], [0x4D, 0x4D, 0x00, 0x2A]),
        new(".ico", [0x00, 0x00, 0x01, 0x00]),
        new(".pdf", [0x25, 0x50, 0x44, 0x46, 0x2D]),
        new(".zip", ZipLocalFileHeader, ZipEmptyArchive, ZipSpannedArchive),
        new(".docx", ZipLocalFileHeader, ZipEmptyArchive, ZipSpannedArchive),
        new(".xlsx", ZipLocalFileHeader, ZipEmptyArchive, ZipSpannedArchive),
        new(".pptx", ZipLocalFileHeader, ZipEmptyArchive, ZipSpannedArchive),
        new(".gz", [0x1F, 0x8B]),
        new(".7z", [0x37, 0x7A, 0xBC, 0xAF, 0x27, 0x1C]),
        new(".rar", [0x52, 0x61, 0x72, 0x21, 0x1A, 0x07, 0x00], [0x52, 0x61, 0x72, 0x21, 0x1A, 0x07, 0x01, 0x00]),
        new(".mp3", [0x49, 0x44, 0x33]),
        new(".mp4", [null, null, null, null, 0x66, 0x74, 0x79, 0x70])
    ];

    /// <summary>
    /// The file extensions that have a registered signature, lowercase and with a leading dot, in
    /// the order <see cref="TryDetectExtension"/> tests them.
    /// </summary>
    public static IReadOnlyCollection<string> KnownExtensions { get; } =
        Signatures.Select(signature => signature.Extension).ToArray();

    /// <summary>
    /// Determines whether the specified file extension has a registered signature.
    /// </summary>
    /// <param name="extension">
    /// The extension to look up, with or without a leading dot and in any casing. If <see langword="null"/>
    /// or whitespace, returns <see langword="false"/>.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="extension"/> is one of <see cref="KnownExtensions"/>;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    public static bool IsKnownExtension(string? extension) =>
        StringUtility.TryGetTrimmed(extension, out var trimmed) && KnownExtensions.Contains(NormalizeExtension(trimmed));

    /// <summary>
    /// Attempts to detect the file extension whose signature matches the specified header bytes.
    /// </summary>
    /// <param name="header">
    /// The leading bytes of the file, of which at most <see cref="MaxSignatureLength"/> are read. If
    /// <see langword="null"/>, detection fails.
    /// </param>
    /// <param name="extension">
    /// When this method returns, contains the matched extension, lowercase and with a leading dot;
    /// otherwise, <see langword="null"/>.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="header"/> matches a registered signature; otherwise,
    /// <see langword="false"/>.
    /// </returns>
    /// <remarks>
    /// The first matching signature wins, so bytes shared by several formats report the most general
    /// of them: an OOXML document reports <c>.zip</c>, and JPEG bytes report <c>.jpg</c> rather than
    /// <c>.jpeg</c>. Use <see cref="PineGuard.Rules.FileSignatureRules.HasSignature"/> to test a
    /// header against one specific extension instead.
    /// </remarks>
    public static bool TryDetectExtension(byte[]? header, out string? extension)
    {
        extension = null;

        if (header is null)
            return false;

        foreach (var signature in Signatures)
        {
            if (!signature.Matches(header))
                continue;

            extension = signature.Extension;
            return true;
        }

        return false;
    }

    // Callers reach this through FileSignatureRules.HasSignature, which has already rejected an
    // extension that IsKnownExtension does not recognise.
    internal static bool HasSignature(byte[]? header, string extension)
    {
        if (header is null)
            return false;

        var normalized = NormalizeExtension(extension);

        foreach (var signature in Signatures)
        {
            if (string.Equals(signature.Extension, normalized, StringComparison.Ordinal) && signature.Matches(header))
                return true;
        }

        return false;
    }

    private static string NormalizeExtension(string extension)
    {
        var trimmed = extension.Trim();

        return (trimmed.StartsWith('.') ? trimmed : "." + trimmed).ToLowerInvariant();
    }

    // A registered signature: the extension it identifies, plus every leading-byte pattern that
    // identifies it. A null entry inside a pattern matches any byte at that position, which is how a
    // format with a variable prefix (".mp4" carries a box size before "ftyp") or an embedded chunk
    // size (".webp" carries one between "RIFF" and "WEBP") is expressed.
    private sealed class FileSignature(string extension, params byte?[][] patterns)
    {
        public string Extension { get; } = extension;

        public bool Matches(byte[] header)
        {
            foreach (var pattern in patterns)
            {
                if (MatchesPattern(header, pattern))
                    return true;
            }

            return false;
        }

        private static bool MatchesPattern(byte[] header, byte?[] pattern)
        {
            if (header.Length < pattern.Length)
                return false;

            for (var index = 0; index < pattern.Length; index++)
            {
                if (pattern[index] is { } expected && header[index] != expected)
                    return false;
            }

            return true;
        }
    }
}
