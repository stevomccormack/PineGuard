using PineGuard.Externals.Iso.Countries;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Rules.Iso;

public static class IsoCountryRulesTestData
{
    public static class IsIsoAlpha2Code
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("US", "US", true),
            new("us", "us", true),
            new("padded", " US ", true),
            new("GB", "GB", true)
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("null", null, false),
            new("empty", string.Empty, false),
            new("space", " ", false),
            new("too short", "U", false),
            new("too long", "USA", false),
            new("unknown", "ZZ", false),
            new("invalid chars", "U1", false)
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
            new("USA", "USA", true),
            new("usa", "usa", true),
            new("padded", " USA ", true),
            new("GBR", "GBR", true)
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("null", null, false),
            new("empty", string.Empty, false),
            new("space", " ", false),
            new("too short", "US", false),
            new("too long", "USAA", false),
            new("unknown", "ZZZ", false),
            new("invalid chars", "U$A", false)
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
            new("826", "826", true)
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("null", null, false),
            new("empty", string.Empty, false),
            new("space", " ", false),
            new("too short", "84", false),
            new("too long", "8400", false),
            new("not numeric", "ABC", false),
            new("unknown", "999", false)
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
            new("known", "US", new FakeIsoCountryProvider(alpha2: ["US"], alpha3: ["USA"], numeric: ["840"]),
                true),
            new("unknown", "GB", new FakeIsoCountryProvider(alpha2: ["US"], alpha3: ["USA"], numeric: ["840"]),
                false)
        ];

        #region Case Records

        public sealed record Case(string Name, string? Value, IIsoCountryProvider Provider, bool ExpectedReturn)
            : IsCase<string?>(Name, Value, ExpectedReturn);

        #endregion Cases
    }

    public sealed class FakeIsoCountryProvider(
        IEnumerable<string> alpha2,
        IEnumerable<string> alpha3,
        IEnumerable<string> numeric)
        : IIsoCountryProvider
    {
        private readonly HashSet<string> _alpha2 = new(alpha2, StringComparer.OrdinalIgnoreCase);
        private readonly HashSet<string> _alpha3 = new(alpha3, StringComparer.OrdinalIgnoreCase);
        private readonly HashSet<string> _numeric = new(numeric, StringComparer.Ordinal);

        public bool ContainsAlpha2Code(string? value) => value is not null && _alpha2.Contains(value);
        public bool ContainsAlpha3Code(string? value) => value is not null && _alpha3.Contains(value);
        public bool ContainsNumericCode(string? value) => value is not null && _numeric.Contains(value);

        public bool TryGetByAlpha2Code(string? value, out IsoCountry? country)
        {
            country = null;
            return false;
        }

        public bool TryGetByAlpha3Code(string? value, out IsoCountry? country)
        {
            country = null;
            return false;
        }

        public bool TryGetByNumericCode(string? value, out IsoCountry? country)
        {
            country = null;
            return false;
        }

        public bool TryGet(string? value, out IsoCountry? country)
        {
            country = null;
            return false;
        }

        public IReadOnlyCollection<IsoCountry> GetAll() => [];
    }
}
