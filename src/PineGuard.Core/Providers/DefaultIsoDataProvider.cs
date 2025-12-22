using System.Collections.Frozen;

namespace PineGuard.Providers;

public sealed class DefaultIsoDataProvider : IIsoDataProvider
{
    private readonly FrozenSet<string> _iso3166Alpha2;
    private readonly FrozenSet<string> _iso3166Alpha3;
    private readonly FrozenSet<string> _iso3166Numeric;

    private readonly FrozenSet<string> _iso4217;

    private readonly FrozenSet<string> _iso639Alpha2;
    private readonly FrozenSet<string> _iso639Alpha3;

    public DefaultIsoDataProvider(
        FrozenSet<string>? iso3166Alpha2 = null,
        FrozenSet<string>? iso3166Alpha3 = null,
        FrozenSet<string>? iso3166Numeric = null,
        FrozenSet<string>? iso4217CurrencyCodes = null,
        FrozenSet<string>? iso639Alpha2 = null,
        FrozenSet<string>? iso639Alpha3 = null)
    {
        _iso3166Alpha2 = iso3166Alpha2 ?? FrozenSet.ToFrozenSet(Array.Empty<string>(), StringComparer.OrdinalIgnoreCase);
        _iso3166Alpha3 = iso3166Alpha3 ?? FrozenSet.ToFrozenSet(Array.Empty<string>(), StringComparer.OrdinalIgnoreCase);
        _iso3166Numeric = iso3166Numeric ?? FrozenSet.ToFrozenSet(Array.Empty<string>(), StringComparer.Ordinal);

        _iso4217 = iso4217CurrencyCodes ?? FrozenSet.ToFrozenSet(Array.Empty<string>(), StringComparer.OrdinalIgnoreCase);

        _iso639Alpha2 = iso639Alpha2 ?? FrozenSet.ToFrozenSet(Array.Empty<string>(), StringComparer.OrdinalIgnoreCase);
        _iso639Alpha3 = iso639Alpha3 ?? FrozenSet.ToFrozenSet(Array.Empty<string>(), StringComparer.OrdinalIgnoreCase);
    }

    public bool IsIso3166Alpha2(string value) => _iso3166Alpha2.Contains(value);

    public bool IsIso3166Alpha3(string value) => _iso3166Alpha3.Contains(value);

    public bool IsIso3166Numeric(string value) => _iso3166Numeric.Contains(value);

    public bool IsIso4217CurrencyCode(string value) => _iso4217.Contains(value);

    public bool IsIso639Alpha2(string value) => _iso639Alpha2.Contains(value);

    public bool IsIso639Alpha3(string value) => _iso639Alpha3.Contains(value);
}
