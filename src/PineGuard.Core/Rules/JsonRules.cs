using System.Text.Json;
using PineGuard.Utils;

namespace PineGuard.Rules;

/// <summary>
/// Provides pure JSON content validation predicates.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/rules/json">JSON Rules documentation</seealso>
public static class JsonRules
{
    /// <summary>
    /// Determines whether the specified value is valid JSON.
    /// </summary>
    /// <param name="value">The value to validate. If <see langword="null"/> or whitespace, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> is parseable JSON; otherwise, <see langword="false"/>.</returns>
    /// <example>
    /// <code>
    /// bool valid = JsonRules.IsJson("{\"key\": 1}"); // true
    /// bool invalid = JsonRules.IsJson("not json");   // false
    /// </code>
    /// </example>
    public static bool IsJson(string? value) =>
        JsonUtility.TryGetRootKind(value, out _);

    /// <summary>
    /// Determines whether the HTTP headers indicate a JSON content type
    /// (<c>application/json</c> or any <c>*+json</c> media type).
    /// </summary>
    /// <param name="headers">
    /// The HTTP response/request headers to inspect. If <see langword="null"/>, returns <see langword="false"/>.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if the <c>Content-Type</c> header indicates JSON; otherwise, <see langword="false"/>.
    /// </returns>
    public static bool IsJsonContentType(IReadOnlyDictionary<string, IEnumerable<string>>? headers)
    {
        if (!HttpContentTypeUtility.TryGetContentTypeMediaTypes(headers, out var mediaTypes) || mediaTypes is null)
            return false;

        foreach (var mediaType in mediaTypes)
        {
            if (string.Equals(mediaType, "application/json", StringComparison.OrdinalIgnoreCase))
                return true;

            if (mediaType.EndsWith("+json", StringComparison.OrdinalIgnoreCase))
                return true;
        }

        return false;
    }

    /// <summary>
    /// Determines whether the specified value is a valid JSON object (<c>{ ... }</c>).
    /// </summary>
    /// <param name="value">The value to validate. If <see langword="null"/> or whitespace, returns <see langword="false"/>.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="value"/> is a JSON object with root kind
    /// <see cref="JsonValueKind.Object"/>; otherwise, <see langword="false"/>.
    /// </returns>
    /// <example>
    /// <code>
    /// bool obj = JsonRules.IsJsonObject("{\"key\": 1}"); // true
    /// bool obj = JsonRules.IsJsonObject("[1, 2, 3]");    // false
    /// </code>
    /// </example>
    public static bool IsJsonObject(string? value) =>
        JsonUtility.TryGetRootKind(value, out var kind) && kind == JsonValueKind.Object;

    /// <summary>
    /// Determines whether the specified value is a valid JSON array (<c>[ ... ]</c>).
    /// </summary>
    /// <param name="value">The value to validate. If <see langword="null"/> or whitespace, returns <see langword="false"/>.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="value"/> is a JSON array with root kind
    /// <see cref="JsonValueKind.Array"/>; otherwise, <see langword="false"/>.
    /// </returns>
    /// <example>
    /// <code>
    /// bool arr = JsonRules.IsJsonArray("[1, 2, 3]");    // true
    /// bool arr = JsonRules.IsJsonArray("{\"key\": 1}"); // false
    /// </code>
    /// </example>
    public static bool IsJsonArray(string? value) =>
        JsonUtility.TryGetRootKind(value, out var kind) && kind == JsonValueKind.Array;
}
