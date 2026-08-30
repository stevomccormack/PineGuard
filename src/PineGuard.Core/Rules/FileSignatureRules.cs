using PineGuard.Common;
using PineGuard.Utils;

namespace PineGuard.Rules;

/// <summary>
/// Provides pure magic-byte file signature validation predicates.
/// </summary>
/// <remarks>
/// These predicates take the leading bytes of a file and return a verdict; reading those bytes from
/// disk, a stream or an upload is the caller's job.
/// </remarks>
/// <seealso href="https://pineguard.ai/docs/rules/filesignature">File Signature Rules documentation</seealso>
public static class FileSignatureRules
{
    /// <summary>
    /// Determines whether the specified header bytes match the signature registered for the given file extension.
    /// </summary>
    /// <param name="value">
    /// The leading bytes of the file, of which at most <see cref="FileSignatureUtility.MaxSignatureLength"/>
    /// are read. If <see langword="null"/>, returns <see langword="false"/>.
    /// </param>
    /// <param name="extension">
    /// The extension the file claims to have, with or without a leading dot and in any casing. It must
    /// be one of <see cref="FileSignatureUtility.KnownExtensions"/>.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="value"/> starts with a signature registered for
    /// <paramref name="extension"/>; otherwise, <see langword="false"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="extension"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="extension"/> has no registered signature.</exception>
    /// <example>
    /// <code>
    /// bool valid = FileSignatureRules.HasSignature([0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A], ".png"); // true
    /// bool spoofed = FileSignatureRules.HasSignature([0xFF, 0xD8, 0xFF], ".png");                             // false (JPEG bytes)
    /// </code>
    /// </example>
    public static bool HasSignature(byte[]? value, string extension)
    {
        ThrowHelper.ThrowIfNull(extension);

        if (!FileSignatureUtility.IsKnownExtension(extension))
            throw new ArgumentException($"No file signature is registered for extension '{extension}'.", nameof(extension));

        return FileSignatureUtility.HasSignature(value, extension);
    }

    /// <summary>
    /// Determines whether the specified header bytes match any registered file signature.
    /// </summary>
    /// <param name="value">
    /// The leading bytes of the file, of which at most <see cref="FileSignatureUtility.MaxSignatureLength"/>
    /// are read. If <see langword="null"/>, returns <see langword="false"/>.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="value"/> starts with the signature of one of
    /// <see cref="FileSignatureUtility.KnownExtensions"/>; otherwise, <see langword="false"/>.
    /// </returns>
    /// <example>
    /// <code>
    /// bool known = FileSignatureRules.HasKnownSignature([0x25, 0x50, 0x44, 0x46, 0x2D]); // true (PDF)
    /// bool unknown = FileSignatureRules.HasKnownSignature([0x00, 0x00]);                  // false
    /// </code>
    /// </example>
    public static bool HasKnownSignature(byte[]? value) =>
        FileSignatureUtility.TryDetectExtension(value, out _);
}
