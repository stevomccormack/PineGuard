using System.Text.Json;
using PineGuard.Rules;

namespace PineGuard.Utils;

/// <summary>
/// Provides security-token parsing utility methods.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/utils/token">Token Utility documentation</seealso>
public static class TokenUtility
{
    /// <summary>
    /// Attempts to split the specified value into the three segments of a JWT compact serialization.
    /// </summary>
    /// <param name="value">The token to parse. If <see langword="null"/> or whitespace, returns <see langword="false"/>.</param>
    /// <param name="header">When this method returns, contains the encoded header segment if successful; otherwise, an empty string.</param>
    /// <param name="payload">When this method returns, contains the encoded payload segment if successful; otherwise, an empty string.</param>
    /// <param name="signature">When this method returns, contains the encoded signature segment if successful; otherwise, an empty string.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> is a well-formed JWT compact serialization; otherwise, <see langword="false"/>.</returns>
    /// <remarks>
    /// <para>
    /// The segments are returned still encoded, exactly as they appeared in <paramref name="value"/>, because
    /// the signature is a byte string with no textual form and returning two decoded segments beside one
    /// encoded one would be the more surprising contract. A caller that wants the claims decodes the payload
    /// itself.
    /// </para>
    /// <para>
    /// Each segment must be a non-empty run of Base64Url characters carrying neither padding nor whitespace,
    /// which is what RFC 7515 §2's "with all trailing <c>'='</c> characters omitted" requires; the header and
    /// the payload must in addition decode, as UTF-8, to a JSON object. A fourth segment means a JWE
    /// compact serialization rather than the JWS form validated here, and is rejected.
    /// </para>
    /// </remarks>
    public static bool TryParseJwt(string? value, out string header, out string payload, out string signature)
    {
        header = string.Empty;
        payload = string.Empty;
        signature = string.Empty;

        if (!StringUtility.TryGetTrimmed(value, out var trimmed))
            return false;

        var firstSeparator = trimmed.IndexOf(TokenRules.JwtSegmentSeparator);

        if (firstSeparator < 0)
            return false;

        var secondSeparator = trimmed.IndexOf(TokenRules.JwtSegmentSeparator, firstSeparator + 1);

        if (secondSeparator < 0)
            return false;

        if (trimmed.IndexOf(TokenRules.JwtSegmentSeparator, secondSeparator + 1) >= 0)
            return false;

        var headerSegment = trimmed[..firstSeparator];
        var payloadSegment = trimmed[(firstSeparator + 1)..secondSeparator];
        var signatureSegment = trimmed[(secondSeparator + 1)..];

        if (!IsSegment(headerSegment) || !IsSegment(payloadSegment) || !IsSegment(signatureSegment))
            return false;

        if (!IsJsonObjectSegment(headerSegment) || !IsJsonObjectSegment(payloadSegment))
            return false;

        header = headerSegment;
        payload = payloadSegment;
        signature = signatureSegment;
        return true;
    }

    private static bool IsSegment(string value)
    {
        foreach (var ch in value)
        {
            // BufferUtility.IsBase64UrlString tolerates padding and surrounding whitespace; the compact
            // serialization allows neither, so they are ruled out before delegating to it.
            if (ch == BufferRules.Base64PaddingChar || char.IsWhiteSpace(ch))
                return false;
        }

        return BufferUtility.IsBase64UrlString(value);
    }

    private static bool IsJsonObjectSegment(string value) =>
        BufferUtility.TryDecodeUtf8(DecodeSegment(value), out var text)
        && JsonUtility.TryGetRootKind(text, out var kind)
        && kind == JsonValueKind.Object;

    private static byte[] DecodeSegment(string value)
    {
        var remainder = value.Length % BufferRules.Base64CharsPerQuantum;
        var paddingLength = remainder == 0 ? 0 : BufferRules.Base64CharsPerQuantum - remainder;
        var chars = new char[value.Length + paddingLength];

        for (var index = 0; index < value.Length; index++)
        {
            chars[index] = value[index] switch
            {
                '-' => '+',
                '_' => '/',
                var ch => ch
            };
        }

        for (var index = value.Length; index < chars.Length; index++)
            chars[index] = BufferRules.Base64PaddingChar;

        // IsSegment has already established the alphabet and the length, so the non-throwing overload would
        // only ever report success and its failure branch could never be reached.
        return Convert.FromBase64CharArray(chars, 0, chars.Length);
    }
}
