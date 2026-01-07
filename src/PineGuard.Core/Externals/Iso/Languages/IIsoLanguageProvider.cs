using PineGuard.Iso.Languages;

namespace PineGuard.Externals.Iso.Languages;

/// <summary>
/// Provides validation and lookup services for ISO 639 language codes.
/// Thread-safe for concurrent reads. All implementations must be immutable.
/// https://www.iso.org/iso-639-language-codes.html
/// </summary>
public interface IIsoLanguageProvider
{
    bool ContainsAlpha2Code(string? value);

    bool ContainsAlpha3Code(string? value);

    bool TryGetByAlpha2Code(string? value, out IsoLanguage? language);

    bool TryGetByAlpha3Code(string? value, out IsoLanguage? language);

    bool TryGet(string? value, out IsoLanguage? language);

    IReadOnlyCollection<IsoLanguage> GetAll();
}
