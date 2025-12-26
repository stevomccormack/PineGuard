namespace PineGuard.Iso.Languages;

/// <summary>
/// Provides validation and lookup services for ISO 639 language codes.
/// Thread-safe for concurrent reads. All implementations must be immutable.
/// https://www.iso.org/iso-639-language-codes.html
/// </summary>
public interface IIsoLanguageProvider
{
    bool IsValidAlpha2Code(string? value);

    bool IsValidAlpha3Code(string? value);

    bool TryGetLanguageByAlpha2Code(string? value, out IsoLanguage? language);

    bool TryGetLanguageByAlpha3Code(string? value, out IsoLanguage? language);

    bool TryGetLanguage(string? value, out IsoLanguage? language);

    IReadOnlyCollection<IsoLanguage> GetAllLanguages();
}
