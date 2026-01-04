using PineGuard.Iso.Countries;

namespace PineGuard.Utils;

public static partial class IsoUtility
{
    public static class Countries
    {
        public static bool TryParseAlpha2(string? value, out string alpha2) =>
            Iso.IsoCountryUtility.TryParseAlpha2(value, out alpha2);

        public static bool TryParseAlpha3(string? value, out string alpha3) =>
            Iso.IsoCountryUtility.TryParseAlpha3(value, out alpha3);

        public static bool TryParseNumeric(string? value, out string numeric) =>
            Iso.IsoCountryUtility.TryParseNumeric(value, out numeric);

        public static bool IsIsoCountryAlpha2(string? value, IIsoCountryProvider? provider = null) =>
            Rules.Iso.IsoCountryRules.IsIsoAlpha2Code(value, provider);

        public static bool IsIsoCountryAlpha3(string? value, IIsoCountryProvider? provider = null) =>
            Rules.Iso.IsoCountryRules.IsIsoAlpha3Code(value, provider);

        public static bool IsIsoCountryNumeric(string? value, IIsoCountryProvider? provider = null) =>
            Rules.Iso.IsoCountryRules.IsIsoNumericCode(value, provider);
    }
}
