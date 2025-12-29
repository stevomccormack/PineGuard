using System.Collections.Frozen;

namespace PineGuard.Iso.Countries;

/// <summary>
/// ISO 3166 country code provider.
/// https://www.iso.org/iso-3166-country-codes.html
/// </summary>
public sealed class DefaultIsoCountryProvider : IIsoCountryProvider
{
    public static DefaultIsoCountryProvider Instance { get; } = new();

    private readonly FrozenDictionary<string, IsoCountry> _countriesByAlpha2Code;
    private readonly FrozenDictionary<string, IsoCountry> _countriesByAlpha3Code;
    private readonly FrozenDictionary<string, IsoCountry> _countriesByNumericCode;
    private readonly IReadOnlyCollection<IsoCountry> _allCountries;

    public DefaultIsoCountryProvider(
        FrozenDictionary<string, IsoCountry>? countriesByAlpha2Code = null,
        FrozenDictionary<string, IsoCountry>? countriesByCodeAlpha3 = null,
        FrozenDictionary<string, IsoCountry>? countriesByCodeNumeric = null)
    {
        _countriesByAlpha2Code = countriesByAlpha2Code ?? DefaultIsoCountryData.CountriesByCodeAlpha2;
        _countriesByAlpha3Code = countriesByCodeAlpha3 ?? DefaultIsoCountryData.CountriesByCodeAlpha3;
        _countriesByNumericCode = countriesByCodeNumeric ?? DefaultIsoCountryData.CountriesByCodeNumeric;

        _allCountries = _countriesByAlpha2Code.Values;
    }

    public bool IsValidAlpha2Code(string? value) =>
        !string.IsNullOrWhiteSpace(value) && _countriesByAlpha2Code.ContainsKey(value);

    public bool IsValidAlpha3Code(string? value) =>
        !string.IsNullOrWhiteSpace(value) && _countriesByAlpha3Code.ContainsKey(value);

    public bool IsValidNumericCode(string? value) =>
        !string.IsNullOrWhiteSpace(value) && _countriesByNumericCode.ContainsKey(value);

    public bool TryGetByAlpha2Code(string? value, out IsoCountry? country)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            country = null;
            return false;
        }

        if (_countriesByAlpha2Code.TryGetValue(value, out var result))
        {
            country = result;
            return true;
        }

        country = null;
        return false;
    }

    public bool TryGetByAlpha3Code(string? value, out IsoCountry? country)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            country = null;
            return false;
        }

        if (_countriesByAlpha3Code.TryGetValue(value, out var result))
        {
            country = result;
            return true;
        }

        country = null;
        return false;
    }

    public bool TryGetByNumericCode(string? value, out IsoCountry? country)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            country = null;
            return false;
        }

        if (_countriesByNumericCode.TryGetValue(value, out var result))
        {
            country = result;
            return true;
        }

        country = null;
        return false;
    }

    public bool TryGet(string? value, out IsoCountry? country)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            country = null;
            return false;
        }

        return TryGetByAlpha2Code(value, out country) ||
               TryGetByAlpha3Code(value, out country) ||
               TryGetByNumericCode(value, out country);
    }

    public IReadOnlyCollection<IsoCountry> GetAll() => _allCountries;
}