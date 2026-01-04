using System.Collections.Frozen;

namespace PineGuard.Iso.Currencies;

/// <summary>
/// ISO 4217 currency code provider.
/// https://www.iso.org/iso-4217-currency-codes.html
/// </summary>
public sealed class DefaultIsoCurrencyProvider : IIsoCurrencyProvider
{
    public static DefaultIsoCurrencyProvider Instance { get; } = new();

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

    public bool ContainsAlpha3Code(string? value) =>
        !string.IsNullOrWhiteSpace(value) && _currenciesByAlpha3Code.ContainsKey(value);

    public bool ContainsNumericCode(string? value) =>
        !string.IsNullOrWhiteSpace(value) && _currenciesByNumericCode.ContainsKey(value);

    public bool TryGetByAlpha3Code(string? value, out IsoCurrency? currency)
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

    public bool TryGetByNumericCode(string? value, out IsoCurrency? currency)
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

    public bool TryGet(string? value, out IsoCurrency? currency)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            currency = null;
            return false;
        }

        return TryGetByAlpha3Code(value, out currency) || TryGetByNumericCode(value, out currency);
    }

    public IReadOnlyCollection<IsoCurrency> GetAll() => _allCurrencies;
}
