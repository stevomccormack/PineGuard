using PineGuard.Standards.Iso;
using System.Collections.Frozen;

namespace PineGuard.Iso.Languages;

/// <summary>
/// ISO 639 language code provider.
/// https://www.iso.org/iso-639-language-codes.html
/// </summary>
public sealed class DefaultIsoLanguageProvider : IIsoLanguageProvider
{
    private readonly FrozenDictionary<string, IsoLanguage> _languagesByAlpha2Code;
    private readonly FrozenDictionary<string, IsoLanguage> _languagesByAlpha3Code;
    private readonly IReadOnlyCollection<IsoLanguage> _allLanguages;

    public DefaultIsoLanguageProvider(
        FrozenDictionary<string, IsoLanguage>? languagesByAlpha2Code = null,
        FrozenDictionary<string, IsoLanguage>? languagesByAlpha3Code = null)
    {
        _languagesByAlpha2Code = languagesByAlpha2Code ?? DefaultIsoLanguageData.LanguagesByCodeAlpha2;
        _languagesByAlpha3Code = languagesByAlpha3Code ?? DefaultIsoLanguageData.LanguagesByCodeAlpha3;
        _allLanguages = _languagesByAlpha2Code.Values;
    }

    public bool IsValidAlpha2Code(string? value) =>
        !string.IsNullOrWhiteSpace(value) && _languagesByAlpha2Code.ContainsKey(value);

    public bool IsValidAlpha3Code(string? value) =>
        !string.IsNullOrWhiteSpace(value) && _languagesByAlpha3Code.ContainsKey(value);

    public bool TryGetLanguageByAlpha2Code(string? value, out IsoLanguage? language)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            language = null;
            return false;
        }

        if (_languagesByAlpha2Code.TryGetValue(value, out var result))
        {
            language = result;
            return true;
        }

        language = null;
        return false;
    }

    public bool TryGetLanguageByAlpha3Code(string? value, out IsoLanguage? language)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            language = null;
            return false;
        }

        if (_languagesByAlpha3Code.TryGetValue(value, out var result))
        {
            language = result;
            return true;
        }

        language = null;
        return false;
    }

    public bool TryGetLanguage(string? value, out IsoLanguage? language)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            language = null;
            return false;
        }

        return TryGetLanguageByAlpha2Code(value, out language)
               || TryGetLanguageByAlpha3Code(value, out language);
    }

    public IReadOnlyCollection<IsoLanguage> GetAllLanguages() => _allLanguages;
}
