using PineGuard.Externals.Iso.Languages;

namespace PineGuard.Core.UnitTests.Rules.Iso;

public static class IsoLanguageRulesTestData
{
    public static class IsIsoAlpha2Code
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case("en", "en", true) },
            { new Case("EN", "EN", true) },
            { new Case("padded", " en ", true) },
            { new Case("fr", "fr", true) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case("null", null, false) },
            { new Case("empty", string.Empty, false) },
            { new Case("space", " ", false) },
            { new Case("too short", "e", false) },
            { new Case("too long", "eng", false) },
            { new Case("unknown", "zz", false) },
            { new Case("invalid chars", "e1", false) },
        };

        #region Cases

        public sealed record Case(string Name, string? Value, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion Cases
    }

    public static class IsIsoAlpha3Code
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case("eng", "eng", true) },
            { new Case("ENG", "ENG", true) },
            { new Case("padded", " eng ", true) },
            { new Case("fra", "fra", true) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case("null", null, false) },
            { new Case("empty", string.Empty, false) },
            { new Case("space", " ", false) },
            { new Case("too short", "en", false) },
            { new Case("too long", "engg", false) },
            { new Case("unknown", "zzz", false) },
            { new Case("invalid chars", "e$g", false) },
        };

        #region Cases

        public sealed record Case(string Name, string? Value, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion Cases
    }

    public static class IsIsoAlpha2CodeWithProvider
    {
        public static TheoryData<Case> Cases => new()
        {
            { new Case("known", "en", new FakeIsoLanguageProvider(alpha2: ["en"], alpha3: ["eng"]), true) },
            { new Case("unknown", "fr", new FakeIsoLanguageProvider(alpha2: ["en"], alpha3: ["eng"]), false) },
        };

        #region Cases

        public sealed record Case(string Name, string? Value, IIsoLanguageProvider Provider, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion Cases
    }

    public sealed class FakeIsoLanguageProvider : IIsoLanguageProvider
    {
        private readonly HashSet<string> _alpha2;
        private readonly HashSet<string> _alpha3;

        public FakeIsoLanguageProvider(IEnumerable<string> alpha2, IEnumerable<string> alpha3)
        {
            _alpha2 = new HashSet<string>(alpha2, StringComparer.OrdinalIgnoreCase);
            _alpha3 = new HashSet<string>(alpha3, StringComparer.OrdinalIgnoreCase);
        }

        public bool ContainsAlpha2Code(string? value) => value is not null && _alpha2.Contains(value);
        public bool ContainsAlpha3Code(string? value) => value is not null && _alpha3.Contains(value);

        public bool TryGetByAlpha2Code(string? value, out PineGuard.Iso.Languages.IsoLanguage? language)
        {
            language = null;
            return false;
        }

        public bool TryGetByAlpha3Code(string? value, out PineGuard.Iso.Languages.IsoLanguage? language)
        {
            language = null;
            return false;
        }

        public bool TryGet(string? value, out PineGuard.Iso.Languages.IsoLanguage? language)
        {
            language = null;
            return false;
        }

        public IReadOnlyCollection<PineGuard.Iso.Languages.IsoLanguage> GetAll() => Array.Empty<PineGuard.Iso.Languages.IsoLanguage>();
    }
}
