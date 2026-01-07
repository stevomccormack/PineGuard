using PineGuard.Externals.Iso.Countries;

namespace PineGuard.Core.UnitTests.Rules.Iso;

public static class IsoCountryRulesTestData
{
    public static class IsIsoAlpha2Code
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case("US", "US", true) },
            { new Case("us", "us", true) },
            { new Case("padded", " US ", true) },
            { new Case("GB", "GB", true) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case("null", null, false) },
            { new Case("empty", string.Empty, false) },
            { new Case("space", " ", false) },
            { new Case("too short", "U", false) },
            { new Case("too long", "USA", false) },
            { new Case("unknown", "ZZ", false) },
            { new Case("invalid chars", "U1", false) },
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
            { new Case("USA", "USA", true) },
            { new Case("usa", "usa", true) },
            { new Case("padded", " USA ", true) },
            { new Case("GBR", "GBR", true) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case("null", null, false) },
            { new Case("empty", string.Empty, false) },
            { new Case("space", " ", false) },
            { new Case("too short", "US", false) },
            { new Case("too long", "USAA", false) },
            { new Case("unknown", "ZZZ", false) },
            { new Case("invalid chars", "U$A", false) },
        };

        #region Cases

        public sealed record Case(string Name, string? Value, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion Cases
    }

    public static class IsIsoNumericCode
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case("840", "840", true) },
            { new Case("padded", " 840 ", true) },
            { new Case("826", "826", true) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case("null", null, false) },
            { new Case("empty", string.Empty, false) },
            { new Case("space", " ", false) },
            { new Case("too short", "84", false) },
            { new Case("too long", "8400", false) },
            { new Case("not numeric", "ABC", false) },
            { new Case("unknown", "999", false) },
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
            { new Case("known", "US", new FakeIsoCountryProvider(alpha2: ["US"], alpha3: ["USA"], numeric: ["840"]), true) },
            { new Case("unknown", "GB", new FakeIsoCountryProvider(alpha2: ["US"], alpha3: ["USA"], numeric: ["840"]), false) },
        };

        #region Cases

        public sealed record Case(string Name, string? Value, IIsoCountryProvider Provider, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion Cases
    }

    public sealed class FakeIsoCountryProvider : IIsoCountryProvider
    {
        private readonly HashSet<string> _alpha2;
        private readonly HashSet<string> _alpha3;
        private readonly HashSet<string> _numeric;

        public FakeIsoCountryProvider(IEnumerable<string> alpha2, IEnumerable<string> alpha3, IEnumerable<string> numeric)
        {
            _alpha2 = new HashSet<string>(alpha2, StringComparer.OrdinalIgnoreCase);
            _alpha3 = new HashSet<string>(alpha3, StringComparer.OrdinalIgnoreCase);
            _numeric = new HashSet<string>(numeric, StringComparer.Ordinal);
        }

        public bool ContainsAlpha2Code(string? value) => value is not null && _alpha2.Contains(value);
        public bool ContainsAlpha3Code(string? value) => value is not null && _alpha3.Contains(value);
        public bool ContainsNumericCode(string? value) => value is not null && _numeric.Contains(value);

        public bool TryGetByAlpha2Code(string? value, out PineGuard.Iso.Countries.IsoCountry? country)
        {
            country = null;
            return false;
        }

        public bool TryGetByAlpha3Code(string? value, out PineGuard.Iso.Countries.IsoCountry? country)
        {
            country = null;
            return false;
        }

        public bool TryGetByNumericCode(string? value, out PineGuard.Iso.Countries.IsoCountry? country)
        {
            country = null;
            return false;
        }

        public bool TryGet(string? value, out PineGuard.Iso.Countries.IsoCountry? country)
        {
            country = null;
            return false;
        }

        public IReadOnlyCollection<PineGuard.Iso.Countries.IsoCountry> GetAll() => Array.Empty<PineGuard.Iso.Countries.IsoCountry>();
    }
}
