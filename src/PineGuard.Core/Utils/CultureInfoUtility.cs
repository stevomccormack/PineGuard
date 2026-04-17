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

    public static bool TryGetCultureName(string? isoLanguageAlpha2Code, out string cultureName) =>
        TryGetCultureName(isoLanguageAlpha2Code, regionCode: null, out cultureName);

    public static bool TryGetCultureName(string? isoLanguageAlpha2Code, string? regionCode, out string cultureName)
    {
        cultureName = string.Empty;

        if (!StringUtility.TryGetTrimmed(isoLanguageAlpha2Code, out var lang))
            return false;

        return !StringUtility.TryGetTrimmed(regionCode, out var reg) ? TryValidateCultureName(lang, out cultureName) : TryValidateCultureName($"{lang}-{reg}", out cultureName);
    }

    public static bool TryGetCultureNameWithDefaultRegion(string? isoLanguageAlpha2Code, out string cultureName)
    {
        cultureName = string.Empty;

        if (!StringUtility.TryGetTrimmed(isoLanguageAlpha2Code, out var lang))
            return false;

        if (TryGetDefaultRegion(lang, out var defaultRegion) && TryGetCultureName(lang, defaultRegion, out cultureName))
            return true;

        return TryGetCultureName(lang, regionCode: null, out cultureName);
    }

    public static bool TryGetCultureInfo(string? isoLanguageAlpha2Code, out CultureInfo? cultureInfo) =>
        TryGetCultureInfo(isoLanguageAlpha2Code, regionCode: null, out cultureInfo);

    public static bool TryGetCultureInfo(string? isoLanguageAlpha2Code, string? regionCode, out CultureInfo? cultureInfo)
    {
        cultureInfo = null;

        if (!TryGetCultureName(isoLanguageAlpha2Code, regionCode, out var cultureName))
            return false;

        cultureInfo = CultureInfo.GetCultureInfo(cultureName);
        return true;
    }

    public static IReadOnlyCollection<string> GetRegionCodes(string? isoLanguageAlpha2Code)
    {
        if (!StringUtility.TryGetTrimmed(isoLanguageAlpha2Code, out var lang))
            return [];

        return RegionCodesByIsoLanguageAlpha2CodeCache.GetOrAdd(lang, static l =>
        {
            var regions = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            ForEachSpecificCultureForLanguage(l, c =>
            {
                if (TryGetTwoLetterIsoRegionName(c, out var regionCode))
                    regions.Add(regionCode);
            });

            return [.. regions];
        });
    }

    internal static bool TryGetTwoLetterIsoRegionName(CultureInfo cultureInfo, out string regionCode)
    {
        try
        {
            var region = new RegionInfo(cultureInfo.Name);
            regionCode = region.TwoLetterISORegionName;
            return true;
        }
        catch (ArgumentException)
        {
            regionCode = string.Empty;
            return false;
        }
    }

    public static IReadOnlyCollection<CultureInfo> GetCultures(string? isoLanguageAlpha2Code)
    {
        if (!StringUtility.TryGetTrimmed(isoLanguageAlpha2Code, out var lang))
            return [];

        return CulturesByIsoLanguageAlpha2CodeCache.GetOrAdd(lang, static l =>
        {
            var result = new List<CultureInfo>();

            ForEachSpecificCultureForLanguage(l, result.Add);

            result.Sort((a, b) => StringComparer.OrdinalIgnoreCase.Compare(a.Name, b.Name));
            return [.. result];
        });
    }

    private static bool TryValidateCultureName(string candidateCultureName, out string validatedCultureName)
    {
        validatedCultureName = string.Empty;

        try
        {
#if NET6_0_OR_GREATER
            _ = CultureInfo.GetCultureInfo(candidateCultureName, predefinedOnly: true);
#else
            _ = CultureInfo.GetCultureInfo(candidateCultureName);
            if (!CultureInfo.GetCultures(CultureTypes.AllCultures).Any(c => string.Equals(c.Name, candidateCultureName, StringComparison.OrdinalIgnoreCase)))
                return false;
#endif
            validatedCultureName = candidateCultureName;
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
        foreach (var culture in SpecificCulturesCache.Value.Where(c => string.Equals(c.TwoLetterISOLanguageName, isoLanguageAlpha2Code, StringComparison.OrdinalIgnoreCase)))
            action(culture);
    }
}
