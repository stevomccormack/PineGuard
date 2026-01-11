using PineGuard.Externals.Iso.Languages;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Rules.Iso;

public static class IsoLanguageRulesTestData
{
    public static class IsIsoAlpha2Code
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("en", "en", true),
            new("EN", "EN", true),
            new("padded", " en ", true),
            new("fr", "fr", true)
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("null", null, false),
            new("empty", string.Empty, false),
            new("space", " ", false),
            new("too short", "e", false),
            new("too long", "eng", false),
            new("unknown", "zz", false),
            new("invalid chars", "e1", false)
        ];

        #region Case Records

        public sealed record Case(string Name, string? Value, bool ExpectedReturn)
            : IsCase<string?>(Name, Value, ExpectedReturn);

        #endregion Cases
    }

    public static class IsIsoAlpha3Code
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("eng", "eng", true),
            new("ENG", "ENG", true),
            new("padded", " eng ", true),
            new("fra", "fra", true)
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("null", null, false),
            new("empty", string.Empty, false),
            new("space", " ", false),
            new("too short", "en", false),
            new("too long", "engg", false),
            new("unknown", "zzz", false),
            new("invalid chars", "e$g", false)
        ];

        #region Case Records

        public sealed record Case(string Name, string? Value, bool ExpectedReturn)
            : IsCase<string?>(Name, Value, ExpectedReturn);

        #endregion Cases
    }

    public static class IsIsoAlpha2CodeWithProvider
    {
        public static TheoryData<Case> Cases =>
        [
            new("known", "en", new FakeIsoLanguageProvider(alpha2: ["en"], alpha3: ["eng"]), true),
            new("unknown", "fr", new FakeIsoLanguageProvider(alpha2: ["en"], alpha3: ["eng"]), false)
        ];

        #region Case Records

        public sealed record Case(string Name, string? Value, IIsoLanguageProvider Provider, bool ExpectedReturn)
            : IsCase<string?>(Name, Value, ExpectedReturn);

        #endregion Cases
    }

    public sealed class FakeIsoLanguageProvider(IEnumerable<string> alpha2, IEnumerable<string> alpha3)
        : IIsoLanguageProvider
    {
        private readonly HashSet<string> _alpha2 = new(alpha2, StringComparer.OrdinalIgnoreCase);
        private readonly HashSet<string> _alpha3 = new(alpha3, StringComparer.OrdinalIgnoreCase);

        public bool ContainsAlpha2Code(string? value) => value is not null && _alpha2.Contains(value);
        public bool ContainsAlpha3Code(string? value) => value is not null && _alpha3.Contains(value);

        public bool TryGetByAlpha2Code(string? value, out IsoLanguage? language)
        {
            language = null;
            return false;
        }

        public bool TryGetByAlpha3Code(string? value, out IsoLanguage? language)
        {
            language = null;
            return false;
        }

        public bool TryGet(string? value, out IsoLanguage? language)
        {
            language = null;
            return false;
        }

        public IReadOnlyCollection<IsoLanguage> GetAll() => [];
    }
}
