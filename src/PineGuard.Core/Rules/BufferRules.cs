using PineGuard.Utils;

namespace PineGuard.Rules;

public static class BufferRules
{
    public const int Base64CharsPerQuantum = 4;
    public const int Base64BytesPerQuantum = 3;

    public static bool IsHex(string? value) =>
        BufferUtility.IsHexString(value);

    public static bool IsBase64(string? value) =>
        BufferUtility.IsBase64String(value);
}
