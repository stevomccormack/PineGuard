using PineGuard.Rules;

namespace PineGuard.Iso.Languages;

/// <summary>
/// Represents an ISO 639 language.
/// https://www.iso.org/iso-639-language-codes.html
/// </summary>
public sealed record IsoLanguage
{
    public const int Alpha2ExactLength = 2;
    public const int Alpha3ExactLength = 3;

    public string Alpha2Code { get; }
    public string Alpha3Code { get; }
    public string Name { get; }

    public IsoLanguage(string alpha2Code, string alpha3Code, string name)
    {
        ArgumentNullException.ThrowIfNull(alpha2Code);
        ArgumentNullException.ThrowIfNull(alpha3Code);
        ArgumentNullException.ThrowIfNull(name);

        if (!StringRules.IsExactLength(alpha2Code, Alpha2ExactLength) || !StringRules.IsAlphabetic(alpha2Code))
            throw new ArgumentException($"Alpha2Code must be exactly {Alpha2ExactLength} alphabetic characters.", nameof(alpha2Code));

        if (!StringRules.IsExactLength(alpha3Code, Alpha3ExactLength) || !StringRules.IsAlphabetic(alpha3Code))
            throw new ArgumentException($"Alpha3Code must be exactly {Alpha3ExactLength} alphabetic characters.", nameof(alpha3Code));

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be null or whitespace.", nameof(name));

        Alpha2Code = alpha2Code.ToLowerInvariant();
        Alpha3Code = alpha3Code.ToLowerInvariant();
        Name = name;
    }
}
