using System.Collections.Frozen;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace PineGuard.MustClauses;

public static partial class MustIsoExtension
{
    public const string Iso2CountryCodePattern = "^[A-Za-z]{2}$";
    public const string Iso3CountryCodePattern = "^[A-Za-z]{3}$";
    public const string IsoCurrencyCodePattern = "^[A-Za-z]{3}$";

    [GeneratedRegex(Iso2CountryCodePattern, RegexOptions.Compiled)]
    private static partial Regex Iso2CountryCodeRegex();

    [GeneratedRegex(Iso3CountryCodePattern, RegexOptions.Compiled)]
    private static partial Regex Iso3CountryCodeRegex();

    [GeneratedRegex(IsoCurrencyCodePattern, RegexOptions.Compiled)]
    private static partial Regex IsoCurrencyCodeRegex();

    public static readonly FrozenSet<string> Iso2CountryCodes = new[]
    {
        "AF","AX","AL","DZ","AS","AD","AO","AI","AQ","AG","AR","AM","AW","AU","AT","AZ",
        "BS","BH","BD","BB","BY","BE","BZ","BJ","BM","BT","BO","BQ","BA","BW","BV","BR",
        "IO","BN","BG","BF","BI","KH","CM","CA","CV","KY","CF","TD","CL","CN","CX","CC",
        "CO","KM","CG","CD","CK","CR","CI","HR","CU","CW","CY","CZ","DK","DJ","DM","DO",
        "EC","EG","SV","GQ","ER","EE","ET","FK","FO","FJ","FI","FR","GF","PF","TF","GA",
        "GM","GE","DE","GH","GI","GR","GL","GD","GP","GU","GT","GG","GN","GW","GY","HT",
        "HM","VA","HN","HK","HU","IS","IN","ID","IR","IQ","IE","IM","IL","IT","JM","JP",
        "JE","JO","KZ","KE","KI","KP","KR","KW","KG","LA","LV","LB","LS","LR","LY","LI",
        "LT","LU","MO","MG","MW","MY","MV","ML","MT","MH","MQ","MR","MU","YT","MX","FM",
        "MD","MC","MN","ME","MS","MA","MZ","MM","NA","NR","NP","NL","NC","NZ","NI","NE",
        "NG","NU","NF","MK","MP","NO","OM","PK","PW","PS","PA","PG","PY","PE","PH","PN",
        "PL","PT","PR","QA","RE","RO","RU","RW","BL","SH","KN","LC","MF","PM","VC","WS",
        "SM","ST","SA","SN","RS","SC","SL","SG","SX","SK","SI","SB","SO","ZA","GS","SS",
        "ES","LK","SD","SR","SJ","SE","CH","SY","TW","TJ","TZ","TH","TL","TG","TK","TO",
        "TT","TN","TR","TM","TC","TV","UG","UA","AE","GB","US","UM","UY","UZ","VU","VE",
        "VN","VG","VI","WF","EH","YE","ZM","ZW"
    }.ToFrozenSet(StringComparer.OrdinalIgnoreCase);

    public static MustResult Iso2CountryCode(
        this IMustClause _,
        string value,
        [CallerArgumentExpression("value")] string? paramName = null)
    {
        var messageTemplate = "{paramName} must be ISO 3166-1 alpha-2 country code.";

        if (string.IsNullOrEmpty(value))
            return MustResult.FromBool(false, messageTemplate, paramName, value);

        var ok = value.Length == 2
            && Iso2CountryCodeRegex().IsMatch(value)
            && Iso2CountryCodes.Contains(value.Trim());

        return MustResult.FromBool(ok, messageTemplate, paramName, value);
    }

    public static MustResult Iso3CountryCode(
        this IMustClause _,
        string value,
        [CallerArgumentExpression("value")] string? paramName = null)
    {
        var messageTemplate = "{paramName} must be ISO 3166-1 alpha-3 country code.";

        if (string.IsNullOrEmpty(value))
            return MustResult.FromBool(false, messageTemplate, paramName, value);

        var ok = value.Length == 3 && Iso3CountryCodeRegex().IsMatch(value);
        return MustResult.FromBool(ok, messageTemplate, paramName, value);
    }

    public static MustResult IsoCurrencyCode(
        this IMustClause _,
        string value,
        [CallerArgumentExpression("value")] string? paramName = null)
    {
        var messageTemplate = "{paramName} must be ISO 4217 currency code.";

        if (string.IsNullOrEmpty(value))
            return MustResult.FromBool(false, messageTemplate, paramName, value);

        var ok = value.Length == 3 && IsoCurrencyCodeRegex().IsMatch(value);
        return MustResult.FromBool(ok, messageTemplate, paramName, value);
    }
}
