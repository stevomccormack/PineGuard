using System.Collections.Concurrent;
using System.Globalization;

namespace PineGuard.Utils;

/// <summary>
/// Provides culture information lookup and validation utility methods.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/utils/cultureinfo">CultureInfo Utility documentation</seealso>
public static class CultureInfoUtility
{
    private static readonly Lazy<CultureInfo[]> SpecificCulturesCache =
        new(() => CultureInfo.GetCultures(CultureTypes.SpecificCultures));

    private static readonly ConcurrentDictionary<string, IReadOnlyCollection<string>> RegionCodesByIsoLanguageAlpha2CodeCache =
        new(StringComparer.OrdinalIgnoreCase);

    private static readonly ConcurrentDictionary<string, IReadOnlyCollection<CultureInfo>> CulturesByIsoLanguageAlpha2CodeCache =
        new(StringComparer.OrdinalIgnoreCase);

#if !NET8_0_OR_GREATER
    private static readonly Lazy<HashSet<string>> AllCultureNamesCache =
        new(() => new HashSet<string>(
            CultureInfo.GetCultures(CultureTypes.AllCultures).Select(c => c.Name),
            StringComparer.OrdinalIgnoreCase));
#endif

    /// <summary>
    /// Attempts to resolve a well-formed, predefined culture name for the specified ISO language code.
    /// </summary>
    /// <param name="isoLanguageAlpha2Code">
    /// The ISO 639-1 two-letter language code (e.g., <c>"en"</c>). If <see langword="null"/> or whitespace,
    /// returns <see langword="false"/>.
    /// </param>
    /// <param name="cultureName">
    /// When this method returns <see langword="true"/>, contains the resolved culture name (e.g., <c>"en"</c>).
    /// When <see langword="false"/>, contains <see cref="string.Empty"/>.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="isoLanguageAlpha2Code"/> resolves to a predefined culture;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    /// <example>
    /// <code>
    /// CultureInfoUtility.TryGetCultureName("en", out var cultureName); // true, cultureName = "en"
    /// </code>
    /// </example>
    public static bool TryGetCultureName(string? isoLanguageAlpha2Code, out string cultureName) =>
        TryGetCultureName(isoLanguageAlpha2Code, regionCode: null, out cultureName);

    /// <summary>
    /// Attempts to resolve a well-formed, predefined culture name for the specified ISO language and region codes.
    /// </summary>
    /// <param name="isoLanguageAlpha2Code">
    /// The ISO 639-1 two-letter language code (e.g., <c>"en"</c>). If <see langword="null"/> or whitespace,
    /// returns <see langword="false"/>.
    /// </param>
    /// <param name="regionCode">
    /// An optional ISO 3166-1 region code (e.g., <c>"US"</c>) combined with the language code as
    /// <c>"{language}-{region}"</c>. If <see langword="null"/> or whitespace, only the language code is used.
    /// </param>
    /// <param name="cultureName">
    /// When this method returns <see langword="true"/>, contains the resolved culture name (e.g., <c>"en-US"</c>).
    /// When <see langword="false"/>, contains <see cref="string.Empty"/>.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if the combined language/region name resolves to a predefined culture;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    /// <remarks>
    /// On net8.0 and later, resolution uses <c>CultureInfo.GetCultureInfo(string, bool)</c> with
    /// <c>predefinedOnly: true</c>. On netstandard2.1, no such overload exists, so resolution instead checks
    /// membership in <see cref="CultureInfo.GetCultures(CultureTypes)"/> with <see cref="CultureTypes.AllCultures"/>,
    /// which may additionally accept culture names registered on the host (e.g., via
    /// <c>CultureAndRegionInfoBuilder</c> on Windows) that are not predefined by the runtime itself. This is a
    /// polyfill, not an exact match; results may differ between targets for host-registered culture names.
    /// </remarks>
    /// <example>
    /// <code>
    /// CultureInfoUtility.TryGetCultureName("en", "US", out var cultureName); // true, cultureName = "en-US"
    /// </code>
    /// </example>
    public static bool TryGetCultureName(string? isoLanguageAlpha2Code, string? regionCode, out string cultureName)
    {
        cultureName = string.Empty;

        if (!StringUtility.TryGetTrimmed(isoLanguageAlpha2Code, out var lang))
            return false;

        return !StringUtility.TryGetTrimmed(regionCode, out var reg) ? TryValidateCultureName(lang, out cultureName) : TryValidateCultureName($"{lang}-{reg}", out cultureName);
    }

    /// <summary>
    /// Attempts to resolve a culture name for the specified ISO language code, preferring a well-known
    /// default region for that language when a valid one is available.
    /// </summary>
    /// <param name="isoLanguageAlpha2Code">
    /// The ISO 639-1 two-letter language code (e.g., <c>"pt"</c>). If <see langword="null"/> or whitespace,
    /// returns <see langword="false"/>.
    /// </param>
    /// <param name="cultureName">
    /// When this method returns <see langword="true"/>, contains the resolved culture name — the
    /// language's default region combination (e.g., <c>"pt-BR"</c>) if one is known and valid, otherwise
    /// the language-only culture name. When <see langword="false"/>, contains <see cref="string.Empty"/>.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if either the default-region culture or the language-only culture resolves
    /// to a predefined culture; otherwise, <see langword="false"/>.
    /// </returns>
    /// <example>
    /// <code>
    /// CultureInfoUtility.TryGetCultureNameWithDefaultRegion("pt", out var cultureName); // true, cultureName = "pt-BR"
    /// </code>
    /// </example>
    public static bool TryGetCultureNameWithDefaultRegion(string? isoLanguageAlpha2Code, out string cultureName)
    {
        cultureName = string.Empty;

        if (!StringUtility.TryGetTrimmed(isoLanguageAlpha2Code, out var lang))
            return false;

        if (TryGetDefaultRegion(lang, out var defaultRegion) && TryGetCultureName(lang, defaultRegion, out cultureName))
            return true;

        return TryGetCultureName(lang, regionCode: null, out cultureName);
    }

    /// <summary>
    /// Attempts to resolve a <see cref="CultureInfo"/> for the specified ISO language code.
    /// </summary>
    /// <param name="isoLanguageAlpha2Code">
    /// The ISO 639-1 two-letter language code (e.g., <c>"en"</c>). If <see langword="null"/> or whitespace,
    /// returns <see langword="false"/>.
    /// </param>
    /// <param name="cultureInfo">
    /// When this method returns <see langword="true"/>, contains the resolved <see cref="CultureInfo"/>.
    /// When <see langword="false"/>, contains <see langword="null"/>.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="isoLanguageAlpha2Code"/> resolves to a predefined culture;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    /// <example>
    /// <code>
    /// CultureInfoUtility.TryGetCultureInfo("en", out var cultureInfo); // true, cultureInfo.Name = "en"
    /// </code>
    /// </example>
    public static bool TryGetCultureInfo(string? isoLanguageAlpha2Code, out CultureInfo? cultureInfo) =>
        TryGetCultureInfo(isoLanguageAlpha2Code, regionCode: null, out cultureInfo);

    /// <summary>
    /// Attempts to resolve a <see cref="CultureInfo"/> for the specified ISO language and region codes.
    /// </summary>
    /// <param name="isoLanguageAlpha2Code">
    /// The ISO 639-1 two-letter language code (e.g., <c>"en"</c>). If <see langword="null"/> or whitespace,
    /// returns <see langword="false"/>.
    /// </param>
    /// <param name="regionCode">
    /// An optional ISO 3166-1 region code (e.g., <c>"US"</c>) combined with the language code. If
    /// <see langword="null"/> or whitespace, only the language code is used.
    /// </param>
    /// <param name="cultureInfo">
    /// When this method returns <see langword="true"/>, contains the resolved <see cref="CultureInfo"/>.
    /// When <see langword="false"/>, contains <see langword="null"/>.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if the combined language/region name resolves to a predefined culture;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    /// <example>
    /// <code>
    /// CultureInfoUtility.TryGetCultureInfo("en", "US", out var cultureInfo); // true, cultureInfo.Name = "en-US"
    /// </code>
    /// </example>
    public static bool TryGetCultureInfo(string? isoLanguageAlpha2Code, string? regionCode, out CultureInfo? cultureInfo)
    {
        cultureInfo = null;

        if (!TryGetCultureName(isoLanguageAlpha2Code, regionCode, out var cultureName))
            return false;

        cultureInfo = CultureInfo.GetCultureInfo(cultureName);
        return true;
    }

    /// <summary>
    /// Gets the distinct ISO 3166-1 two-letter region codes of all specific cultures for the specified
    /// ISO language code. Results are cached per language after the first lookup.
    /// </summary>
    /// <param name="isoLanguageAlpha2Code">
    /// The ISO 639-1 two-letter language code (e.g., <c>"en"</c>). If <see langword="null"/> or whitespace,
    /// returns an empty collection.
    /// </param>
    /// <returns>
    /// A read-only collection of region codes associated with <paramref name="isoLanguageAlpha2Code"/>, or
    /// an empty collection if the language code is invalid or has no specific cultures.
    /// </returns>
    /// <example>
    /// <code>
    /// var regions = CultureInfoUtility.GetRegionCodes("en"); // ["US", "GB", "AU", ...]
    /// </code>
    /// </example>
    public static IReadOnlyCollection<string> GetRegionCodes(string? isoLanguageAlpha2Code)
    {
        if (!StringUtility.TryGetTrimmed(isoLanguageAlpha2Code, out var lang))
            return [];

        if (RegionCodesByIsoLanguageAlpha2CodeCache.TryGetValue(lang, out var cached))
            return cached;

        var regions = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        ForEachSpecificCultureForLanguage(lang, c => AddRegionCode(regions, c));

        IReadOnlyCollection<string> result = [.. regions];

        // Only cache languages that resolved to at least one region, so caller-supplied garbage
        // (invalid or unbounded-length language codes) can never grow the cache without limit.
        if (result.Count > 0)
            RegionCodesByIsoLanguageAlpha2CodeCache.TryAdd(lang, result);

        return result;
    }

    /// <summary>
    /// Adds the two-letter ISO region code of <paramref name="cultureInfo"/> to <paramref name="regions"/>,
    /// skipping cultures that have no associated region.
    /// </summary>
    /// <param name="regions">The set collecting the discovered region codes.</param>
    /// <param name="cultureInfo">The culture to resolve a region code for.</param>
    internal static void AddRegionCode(HashSet<string> regions, CultureInfo cultureInfo)
    {
        if (TryGetTwoLetterIsoRegionName(cultureInfo, out var regionCode))
            regions.Add(regionCode);
    }

    internal static bool TryGetTwoLetterIsoRegionName(CultureInfo cultureInfo, out string regionCode)
    {
        try
        {
            var region = new RegionInfo(cultureInfo.Name);
            var code = region.TwoLetterISORegionName;

            if (!IsIsoAlpha2RegionCode(code))
            {
                regionCode = string.Empty;
                return false;
            }

            regionCode = code;
            return true;
        }
        catch (ArgumentException)
        {
            regionCode = string.Empty;
            return false;
        }
    }

    // TwoLetterISORegionName returns UN M.49 numeric codes (e.g. "001", "419") for region-group
    // cultures such as "en-001" or "es-419"; those are not ISO 3166-1 alpha-2 codes.
    private static bool IsIsoAlpha2RegionCode(string code) =>
        code.Length == 2 && code[0] is >= 'A' and <= 'Z' && code[1] is >= 'A' and <= 'Z';

    /// <summary>
    /// Gets all specific <see cref="CultureInfo"/> instances for the specified ISO language code, sorted by
    /// culture name. Results are cached per language after the first lookup.
    /// </summary>
    /// <param name="isoLanguageAlpha2Code">
    /// The ISO 639-1 two-letter language code (e.g., <c>"en"</c>). If <see langword="null"/> or whitespace,
    /// returns an empty collection.
    /// </param>
    /// <returns>
    /// A read-only collection of <see cref="CultureInfo"/> instances for
    /// <paramref name="isoLanguageAlpha2Code"/>, sorted ordinally by culture name (case-insensitive), or an
    /// empty collection if the language code is invalid or has no specific cultures.
    /// </returns>
    /// <example>
    /// <code>
    /// var cultures = CultureInfoUtility.GetCultures("en"); // [en-AU, en-GB, en-US, ...]
    /// </code>
    /// </example>
    public static IReadOnlyCollection<CultureInfo> GetCultures(string? isoLanguageAlpha2Code)
    {
        if (!StringUtility.TryGetTrimmed(isoLanguageAlpha2Code, out var lang))
            return [];

        if (CulturesByIsoLanguageAlpha2CodeCache.TryGetValue(lang, out var cached))
            return cached;

        var list = new List<CultureInfo>();

        ForEachSpecificCultureForLanguage(lang, list.Add);

        list.Sort((a, b) => StringComparer.OrdinalIgnoreCase.Compare(a.Name, b.Name));

        IReadOnlyCollection<CultureInfo> result = [.. list];

        // Only cache languages that resolved to at least one culture, so caller-supplied garbage
        // (invalid or unbounded-length language codes) can never grow the cache without limit.
        if (result.Count > 0)
            CulturesByIsoLanguageAlpha2CodeCache.TryAdd(lang, result);

        return result;
    }

    private static bool TryValidateCultureName(string candidateCultureName, out string validatedCultureName)
    {
        validatedCultureName = string.Empty;

        try
        {
#if NET8_0_OR_GREATER
            var culture = CultureInfo.GetCultureInfo(candidateCultureName, predefinedOnly: true);
#else
            var culture = CultureInfo.GetCultureInfo(candidateCultureName);
            if (!AllCultureNamesCache.Value.Contains(candidateCultureName))
                return false;
#endif
            validatedCultureName = culture.Name;
            return true;
        }
        catch (CultureNotFoundException)
        {
            return false;
        }
    }

    private static bool TryGetDefaultRegion(string isoLanguageAlpha2Code, out string regionCode)
    {
        regionCode = string.Empty;

        switch (isoLanguageAlpha2Code.ToLowerInvariant())
        {
            case "en":
                regionCode = "US";
                return true;
            case "pt":
                regionCode = "BR";
                return true;
            case "es":
                regionCode = "ES";
                return true;
            case "fr":
                regionCode = "FR";
                return true;
            case "de":
                regionCode = "DE";
                return true;
            case "zh":
                regionCode = "CN";
                return true;
            default:
                return false;
        }
    }

    private static void ForEachSpecificCultureForLanguage(string isoLanguageAlpha2Code, Action<CultureInfo> action)
    {
        var cultures = SpecificCulturesCache.Value
            .Where(c => string.Equals(c.TwoLetterISOLanguageName, isoLanguageAlpha2Code, StringComparison.OrdinalIgnoreCase));

        foreach (var culture in cultures)
            action(culture);
    }
}
