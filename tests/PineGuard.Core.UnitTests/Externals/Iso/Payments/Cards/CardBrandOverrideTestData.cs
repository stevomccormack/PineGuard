using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Externals.Iso.Payments.Cards;

public static class CardBrandOverrideTestData
{
    public static class MastercardMatchesPan
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("51", "5100000000000000", true),
            new("55", "5500000000000000", true),
            new("2221", "2221000000000000", true),
            new("2720", "2720000000000000", true),
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("null", null, false),
            new("space", " ", false),
            new("wrong length", "51", false),
            new("56", "5600000000000000", false),
            new("2721", "2721000000000000", false),
        ];

        public sealed record ValidCase(string Name, string? Value, bool ExpectedReturn)
            : IsCase<string?>(Name, Value, ExpectedReturn);
    }

    public static class JcbMatchesPan
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("3528", "3528000000000000", true),
            new("3589", "3589000000000000", true),
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("null", null, false),
            new("tab", "\t", false),
            new("wrong length", "3528", false),
            new("3527", "3527000000000000", false),
            new("3590", "3590000000000000", false),
        ];

        public sealed record ValidCase(string Name, string? Value, bool ExpectedReturn)
            : IsCase<string?>(Name, Value, ExpectedReturn);
    }

    public static class DiscoverMatchesPan
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("6011", "6011000000000000", true),
            new("65", "6500000000000000", true),
            new("644", "6440000000000000", true),
            new("649", "6490000000000000", true),
            new("65 other", "6500000000000001", true),
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("null", null, false),
            new("space", " ", false),
            new("wrong length", "6011", false),
            new("643", "6430000000000000", false),
        ];

        public sealed record ValidCase(string Name, string? Value, bool ExpectedReturn)
            : IsCase<string?>(Name, Value, ExpectedReturn);
    }

    public static class DinersClubMatchesPan
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("300", "30000000000000", true),
            new("305", "30500000000000", true),
            new("360", "36000000000000", true),
            new("380", "38000000000000", true),
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("null", null, false),
            new("space", " ", false),
            new("sanitize to empty", "----", false),
            new("306", "30600000000000", false),
            new("390", "39000000000000", false),
        ];

        public sealed record ValidCase(string Name, string? Value, bool ExpectedReturn)
            : IsCase<string?>(Name, Value, ExpectedReturn);
    }

    public static class VisaMatchesPan
    {
        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("null", null, false),
            new("empty", "", false),
            new("space", " ", false),
        ];

        public sealed record ValidCase(string Name, string? Value, bool ExpectedReturn)
            : IsCase<string?>(Name, Value, ExpectedReturn);
    }
}
