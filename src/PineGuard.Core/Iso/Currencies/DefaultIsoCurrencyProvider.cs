using PineGuard.Standards.Iso.Currencies;
using System.Collections.Frozen;

namespace PineGuard.Iso.Currencies;

/// <summary>
/// ISO 4217 currency code provider.
/// https://www.iso.org/iso-4217-currency-codes.html
/// </summary>
public sealed class DefaultIsoCurrencyProvider : IIsoCurrencyProvider
{
    private readonly FrozenDictionary<string, IsoCurrency> _currenciesByAlpha3Code;
    private readonly FrozenDictionary<string, IsoCurrency> _currenciesByNumericCode;
    private readonly IReadOnlyCollection<IsoCurrency> _allCurrencies;

    public DefaultIsoCurrencyProvider(
        FrozenDictionary<string, IsoCurrency>? currenciesByAlpha3Code = null,
        FrozenDictionary<string, IsoCurrency>? currenciesByNumericCode = null)
    {
        _currenciesByAlpha3Code = currenciesByAlpha3Code ?? DefaultIsoCurrencyData.CurrenciesByCodeAlpha3;
        _currenciesByNumericCode = currenciesByNumericCode ?? DefaultIsoCurrencyData.CurrenciesByCodeNumeric;
        _allCurrencies = _currenciesByAlpha3Code.Values;
    }

    public bool IsValidAlpha3Code(string? value) =>
        !string.IsNullOrWhiteSpace(value) && _currenciesByAlpha3Code.ContainsKey(value);

    public bool IsValidNumericCode(string? value) =>
        !string.IsNullOrWhiteSpace(value) && _currenciesByNumericCode.ContainsKey(value);

    public bool TryGetCurrencyByAlpha3Code(string? value, out IsoCurrency? currency)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            currency = null;
            return false;
        }

        if (_currenciesByAlpha3Code.TryGetValue(value, out var result))
        {
            currency = result;
            return true;
        }

        currency = null;
        return false;
    }

    public bool TryGetCurrencyByNumericCode(string? value, out IsoCurrency? currency)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            currency = null;
            return false;
        }

        if (_currenciesByNumericCode.TryGetValue(value, out var result))
        {
            currency = result;
            return true;
        }

        currency = null;
        return false;
    }

    public bool TryGetCurrency(string? value, out IsoCurrency? currency)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            currency = null;
            return false;
        }

        return TryGetCurrencyByAlpha3Code(value, out currency) ||
               TryGetCurrencyByNumericCode(value, out currency);
    }

    public IReadOnlyCollection<IsoCurrency> GetAllCurrencies() => _allCurrencies;
}
