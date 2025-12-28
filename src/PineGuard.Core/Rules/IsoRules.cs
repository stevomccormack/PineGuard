using PineGuard.Iso.Countries;
using PineGuard.Iso.Currencies;
using PineGuard.Iso.Languages;
using PineGuard.Rules.Iso;

namespace PineGuard.Rules;

public static class IsoRules
{
    public static bool IsIsoCountryAlpha2(string? value, IIsoCountryProvider? provider = null) =>
        IsoCountryRules.IsIsoCountryAlpha2(value, provider);

    public static bool IsIsoCountryAlpha3(string? value, IIsoCountryProvider? provider = null) =>
        IsoCountryRules.IsIsoCountryAlpha3(value, provider);

    public static bool IsIsoCountryNumeric(string? value, IIsoCountryProvider? provider = null) =>
        IsoCountryRules.IsIsoCountryNumeric(value, provider);

    public static bool IsIsoCurrencyAlpha3(string? value, IIsoCurrencyProvider? provider = null) =>
        IsoCurrencyRules.IsIsoCurrencyAlpha3(value, provider);

    public static bool IsIsoCurrencyNumeric(string? value, IIsoCurrencyProvider? provider = null) =>
        IsoCurrencyRules.IsIsoCurrencyNumeric(value, provider);

    public static bool IsIsoLanguageAlpha2(string? value, IIsoLanguageProvider? provider = null) =>
        IsoLanguageRules.IsIsoLanguageAlpha2(value, provider);

    public static bool IsIsoLanguageAlpha3(string? value, IIsoLanguageProvider? provider = null) =>
        IsoLanguageRules.IsIsoLanguageAlpha3(value, provider);

    public static bool IsIsoDateOnly(string? value) =>
        IsoDateRules.IsIsoDateOnly(value);

    public static bool IsIsoDateTime(string? value) =>
        IsoDateRules.IsIsoDateTime(value);

    public static bool IsIsoDateTimeOffset(string? value) =>
        IsoDateRules.IsIsoDateTimeOffset(value);

    public static bool IsIsoPaymentCard(string? value) =>
        IsoPaymentCardRules.IsIsoPaymentCard(value);
}
