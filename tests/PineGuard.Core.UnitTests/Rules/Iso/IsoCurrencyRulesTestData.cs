using PineGuard.Externals.Iso.Currencies;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Rules.Iso;

public static class IsoCurrencyRulesTestData
{
    public static class IsIsoAlpha3Code
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("USD", "USD", true),
            new("usd", "usd", true),
            new("padded", " USD ", true),
            new("EUR", "EUR", true)
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("null", null, false),
            new("empty", string.Empty, false),
            new("space", " ", false),
            new("too short", "US", false),
            new("too long", "USDD", false),
            new("unknown", "ZZZ", false),
            new("invalid chars", "U$D", false)
        ];

        #region Case Records

        public sealed record Case(string Name, string? Value, bool ExpectedReturn)
            : IsCase<string?>(Name, Value, ExpectedReturn);

        #endregion Cases
    }

    public static class IsIsoNumericCode
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("840", "840", true),
            new("padded", " 840 ", true),
            new("978", "978", true)
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("null", null, false),
            new("empty", string.Empty, false),
            new("space", " ", false),
            new("too short", "84", false),
            new("too long", "8400", false),
            new("not numeric", "ABC", false),
            new("unknown", "000", false)
        ];

        #region Case Records

        public sealed record Case(string Name, string? Value, bool ExpectedReturn)
            : IsCase<string?>(Name, Value, ExpectedReturn);

        #endregion Cases
    }

    public static class IsIsoAlpha3CodeWithProvider
    {
        public static TheoryData<Case> Cases =>
        [
            new("known", "USD", new FakeIsoCurrencyProvider(alpha3: ["USD"], numeric: ["840"]), true),
            new("unknown", "EUR", new FakeIsoCurrencyProvider(alpha3: ["USD"], numeric: ["840"]), false)
        ];

        #region Case Records

        public sealed record Case(string Name, string? Value, IIsoCurrencyProvider Provider, bool ExpectedReturn)
            : IsCase<string?>(Name, Value, ExpectedReturn);

        #endregion Cases
    }

    public sealed class FakeIsoCurrencyProvider(IEnumerable<string> alpha3, IEnumerable<string> numeric)
        : IIsoCurrencyProvider
    {
        private readonly HashSet<string> _alpha3 = new(alpha3, StringComparer.OrdinalIgnoreCase);
        private readonly HashSet<string> _numeric = new(numeric, StringComparer.Ordinal);

        public bool ContainsAlpha3Code(string? value) => value is not null && _alpha3.Contains(value);
        public bool ContainsNumericCode(string? value) => value is not null && _numeric.Contains(value);

        public bool TryGetByAlpha3Code(string? value, out IsoCurrency? currency)
        {
            currency = null;
            return false;
        }

        public bool TryGetByNumericCode(string? value, out IsoCurrency? currency)
        {
            currency = null;
            return false;
        }

        public bool TryGet(string? value, out IsoCurrency? currency)
        {
            currency = null;
            return false;
        }

        public IReadOnlyCollection<IsoCurrency> GetAll() => [];
    }
}
