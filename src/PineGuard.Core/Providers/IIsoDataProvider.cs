namespace PineGuard.Providers;

public interface IIsoDataProvider
{
    bool IsIso3166Alpha2(string value);
    bool IsIso3166Alpha3(string value);
    bool IsIso3166Numeric(string value);

    bool IsIso4217CurrencyCode(string value);

    bool IsIso639Alpha2(string value);
    bool IsIso639Alpha3(string value);
}
