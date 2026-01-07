using PineGuard.Externals.Iso.Currencies;

namespace PineGuard.Core.UnitTests.Rules.Iso;

public static class IsoCurrencyRulesTestData
{
    public static class IsIsoAlpha3Code
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case("USD", "USD", true) },
            { new Case("usd", "usd", true) },
            { new Case("padded", " USD ", true) },
            { new Case("EUR", "EUR", true) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case("null", null, false) },
            { new Case("empty", string.Empty, false) },
            { new Case("space", " ", false) },
            { new Case("too short", "US", false) },
            { new Case("too long", "USDD", false) },
            { new Case("unknown", "ZZZ", false) },
            { new Case("invalid chars", "U$D", false) },
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
            { new Case("978", "978", true) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case("null", null, false) },
            { new Case("empty", string.Empty, false) },
            { new Case("space", " ", false) },
            { new Case("too short", "84", false) },
            { new Case("too long", "8400", false) },
            { new Case("not numeric", "ABC", false) },
            { new Case("unknown", "000", false) },
        };

        #region Cases

        public sealed record Case(string Name, string? Value, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion Cases
    }

    public static class IsIsoAlpha3CodeWithProvider
    {
        public static TheoryData<Case> Cases => new()
        {
            { new Case("known", "USD", new FakeIsoCurrencyProvider(alpha3: ["USD"], numeric: ["840"]), true) },
            { new Case("unknown", "EUR", new FakeIsoCurrencyProvider(alpha3: ["USD"], numeric: ["840"]), false) },
        };

        #region Cases

        public sealed record Case(string Name, string? Value, IIsoCurrencyProvider Provider, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion Cases
    }

    public sealed class FakeIsoCurrencyProvider : IIsoCurrencyProvider
    {
        private readonly HashSet<string> _alpha3;
        private readonly HashSet<string> _numeric;

        public FakeIsoCurrencyProvider(IEnumerable<string> alpha3, IEnumerable<string> numeric)
        {
            _alpha3 = new HashSet<string>(alpha3, StringComparer.OrdinalIgnoreCase);
            _numeric = new HashSet<string>(numeric, StringComparer.Ordinal);
        }

        public bool ContainsAlpha3Code(string? value) => value is not null && _alpha3.Contains(value);
        public bool ContainsNumericCode(string? value) => value is not null && _numeric.Contains(value);

        public bool TryGetByAlpha3Code(string? value, out PineGuard.Iso.Currencies.IsoCurrency? currency)
        {
            currency = null;
            return false;
        }

        public bool TryGetByNumericCode(string? value, out PineGuard.Iso.Currencies.IsoCurrency? currency)
        {
            currency = null;
            return false;
        }

        public bool TryGet(string? value, out PineGuard.Iso.Currencies.IsoCurrency? currency)
        {
            currency = null;
            return false;
        }

        public IReadOnlyCollection<PineGuard.Iso.Currencies.IsoCurrency> GetAll() => Array.Empty<PineGuard.Iso.Currencies.IsoCurrency>();
    }
}
