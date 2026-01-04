using PineGuard.Utils.Iso;
using System.Text.RegularExpressions;

namespace PineGuard.Iso.Languages;

/// <summary>
/// Represents an ISO 639 language.
/// https://www.iso.org/iso-639-language-codes.html
/// </summary>
public sealed partial record IsoLanguage
{
    public const string IsoStandard = "ISO 639";

    public const int Alpha2CodeExactLength = 2;
    public const int Alpha3CodeExactLength = 3;

    public const string Alpha2CodePattern = "^[A-Za-z]{2}$";
    public const string Alpha3CodePattern = "^[A-Za-z]{3}$";

    [GeneratedRegex(Alpha2CodePattern, RegexOptions.CultureInvariant)]
    public static partial Regex Alpha2CodeRegex();

    [GeneratedRegex(Alpha3CodePattern, RegexOptions.CultureInvariant)]
    public static partial Regex Alpha3CodeRegex();

    public string Alpha2Code { get; }
    public string Alpha3Code { get; }
    public string Name { get; }

    public IsoLanguage(string alpha2Code, string alpha3Code, string name)
    {
        ArgumentNullException.ThrowIfNull(alpha2Code);
        ArgumentNullException.ThrowIfNull(alpha3Code);
        ArgumentNullException.ThrowIfNull(name);

        if (!Alpha2CodeRegex().IsMatch(alpha2Code))
            throw new ArgumentException($"Alpha2Code should be alphabetical with exact length of {Alpha2CodeExactLength} characters.", nameof(alpha2Code));

        if (!Alpha3CodeRegex().IsMatch(alpha3Code))
            throw new ArgumentException($"Alpha3Code should be alphabetical with exact length of {Alpha3CodeExactLength} characters.", nameof(alpha3Code));

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be null or whitespace.", nameof(name));

        Alpha2Code = alpha2Code.ToLowerInvariant();
        Alpha3Code = alpha3Code.ToLowerInvariant();
        Name = name;
    }

    public static bool TryParse(string? value, out IsoLanguage language)
    {
        language = null!;

        if (string.IsNullOrWhiteSpace(value))
            return false;

        var provider = DefaultIsoLanguageProvider.Instance;

        if (IsoLanguageUtility.TryParseAlpha2(value, out var alpha2)
            && provider.TryGetByAlpha2Code(alpha2, out var alpha2Language)
            && alpha2Language is not null)
        {
            language = alpha2Language;
            return true;
        }

        if (IsoLanguageUtility.TryParseAlpha3(value, out var alpha3)
            && provider.TryGetByAlpha3Code(alpha3, out var alpha3Language)
            && alpha3Language is not null)
        {
            language = alpha3Language;
            return true;
        }

        language = null!;
        return false;
    }

    public static IsoLanguage Parse(string? value)
    {
        if (TryParse(value, out var language))
            return language;

        throw new FormatException("Value must be an ISO 639 alpha-2 or alpha-3 language code.");
    }

    public override string ToString() => $"[{IsoStandard}] {Name} ({Alpha3Code})";
}
