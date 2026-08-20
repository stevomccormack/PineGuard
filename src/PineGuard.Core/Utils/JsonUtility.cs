using System.Buffers;
using System.Text;
using System.Text.Json;

namespace PineGuard.Utils;

/// <summary>
/// Provides JSON parsing utility methods.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/utils/json">JSON Utility documentation</seealso>
public static class JsonUtility
{
    /// <summary>
    /// Attempts to parse the specified string as JSON and determine the root element's <see cref="JsonValueKind"/>.
    /// </summary>
    /// <param name="value">The JSON string to parse. If <see langword="null"/> or whitespace, returns <see langword="false"/>.</param>
    /// <param name="kind">When this method returns, contains the root value kind if successful; otherwise, the default value.</param>
    /// <returns><see langword="true"/> if the JSON was parsed successfully; otherwise, <see langword="false"/>.</returns>
    public static bool TryGetRootKind(string? value, out JsonValueKind kind)
    {
        kind = default;

        return StringUtility.TryGetTrimmed(value, out var trimmed) && TryGetRootKind(trimmed.AsSpan(), out kind);
    }

    /// <summary>
    /// Attempts to parse the specified character span as JSON and determine the root element's <see cref="JsonValueKind"/>.
    /// </summary>
    /// <param name="value">The JSON character span to parse. If empty or whitespace, returns <see langword="false"/>.</param>
    /// <param name="kind">When this method returns, contains the root value kind if successful; otherwise, the default value.</param>
    /// <returns><see langword="true"/> if the JSON was parsed successfully; otherwise, <see langword="false"/>.</returns>
    /// <remarks>
    /// Reads <paramref name="value"/> directly: the span is transcoded into a pooled UTF-8 byte buffer, which
    /// is then parsed, instead of allocating an intermediate <see cref="string"/> copy via
    /// <see cref="ReadOnlySpan{T}.ToString"/>.
    /// </remarks>
    public static bool TryGetRootKind(ReadOnlySpan<char> value, out JsonValueKind kind)
    {
        kind = default;

        if (value.IsEmpty || value.IsWhiteSpace())
            return false;

        var maxByteCount = Encoding.UTF8.GetMaxByteCount(value.Length);
        var rented = ArrayPool<byte>.Shared.Rent(maxByteCount);

        try
        {
            var byteCount = Encoding.UTF8.GetBytes(value, rented);

            using var document = JsonDocument.Parse(rented.AsMemory(0, byteCount));
            kind = document.RootElement.ValueKind;
            return true;
        }
        catch (JsonException)
        {
            return false;
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(rented);
        }
    }
}
