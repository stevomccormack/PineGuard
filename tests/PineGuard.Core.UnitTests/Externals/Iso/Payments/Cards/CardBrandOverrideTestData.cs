namespace PineGuard.Core.UnitTests.Externals.Iso.Payments.Cards;

public static class CardBrandOverrideTestData
{
    public static class MastercardMatchesPan
    {
        private static ValidCase V(string name, string? pan, bool expected) => new(Name: name, Pan: pan, Expected: expected);

        public static TheoryData<ValidCase> ValidCases => new()
        {
            { V("51", "5100000000000000", expected: true) },
            { V("55", "5500000000000000", expected: true) },
            { V("2221", "2221000000000000", expected: true) },
            { V("2720", "2720000000000000", expected: true) },
        };

        public static TheoryData<ValidCase> EdgeCases => new()
        {
            { V("null", null, expected: false) },
            { V("space", " ", expected: false) },
            { V("wrong length", "51", expected: false) },
            { V("56", "5600000000000000", expected: false) },
            { V("2721", "2721000000000000", expected: false) },
        };

        #region Cases

        public record Case(string Name, string? Pan);

        public sealed record ValidCase(string Name, string? Pan, bool Expected)
            : Case(Name, Pan);

        #endregion
    }

    public static class JcbMatchesPan
    {
        private static ValidCase V(string name, string? pan, bool expected) => new(Name: name, Pan: pan, Expected: expected);

        public static TheoryData<ValidCase> ValidCases => new()
        {
            { V("3528", "3528000000000000", expected: true) },
            { V("3589", "3589000000000000", expected: true) },
        };

        public static TheoryData<ValidCase> EdgeCases => new()
        {
            { V("null", null, expected: false) },
            { V("tab", "\t", expected: false) },
            { V("wrong length", "3528", expected: false) },
            { V("3527", "3527000000000000", expected: false) },
            { V("3590", "3590000000000000", expected: false) },
        };

        #region Cases

        public record Case(string Name, string? Pan);

        public sealed record ValidCase(string Name, string? Pan, bool Expected)
            : Case(Name, Pan);

        #endregion
    }

    public static class DiscoverMatchesPan
    {
        private static ValidCase V(string name, string? pan, bool expected) => new(Name: name, Pan: pan, Expected: expected);

        public static TheoryData<ValidCase> ValidCases => new()
        {
            { V("6011", "6011000000000000", expected: true) },
            { V("65", "6500000000000000", expected: true) },
            { V("644", "6440000000000000", expected: true) },
            { V("649", "6490000000000000", expected: true) },
            { V("65 other", "6500000000000001", expected: true) },
        };

        public static TheoryData<ValidCase> EdgeCases => new()
        {
            { V("null", null, expected: false) },
            { V("space", " ", expected: false) },
            { V("wrong length", "6011", expected: false) },
            { V("643", "6430000000000000", expected: false) },
        };

        #region Cases

        public record Case(string Name, string? Pan);

        public sealed record ValidCase(string Name, string? Pan, bool Expected)
            : Case(Name, Pan);

        #endregion
    }

    public static class DinersClubMatchesPan
    {
        private static ValidCase V(string name, string? pan, bool expected) => new(Name: name, Pan: pan, Expected: expected);

        public static TheoryData<ValidCase> ValidCases => new()
        {
            { V("300", "30000000000000", expected: true) },
            { V("305", "30500000000000", expected: true) },
            { V("360", "36000000000000", expected: true) },
            { V("380", "38000000000000", expected: true) },
        };

        public static TheoryData<ValidCase> EdgeCases => new()
        {
            { V("null", null, expected: false) },
            { V("space", " ", expected: false) },
            { V("sanitize to empty", "----", expected: false) },
            { V("306", "30600000000000", expected: false) },
            { V("390", "39000000000000", expected: false) },
        };

        #region Cases

        public record Case(string Name, string? Pan);

        public sealed record ValidCase(string Name, string? Pan, bool Expected)
            : Case(Name, Pan);

        #endregion
    }

    public static class VisaMatchesPan
    {
        private static ValidCase V(string name, string? pan, bool expected) => new(Name: name, Pan: pan, Expected: expected);

        public static TheoryData<ValidCase> EdgeCases => new()
        {
            { V("null", null, expected: false) },
            { V("empty", "", expected: false) },
            { V("space", " ", expected: false) },
        };

        #region Cases

        public record Case(string Name, string? Pan);

        public sealed record ValidCase(string Name, string? Pan, bool Expected)
            : Case(Name, Pan);

        #endregion
    }
}
