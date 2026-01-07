using PineGuard.Common;

namespace PineGuard.Core.UnitTests.Rules.Iso;

public static class IsoDateRulesTestData
{
    public static class IsIsoDateOnly
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case("valid", "2026-01-06", true) },
            { new Case("padded", " 2026-01-06 ", true) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case("null", null, false) },
            { new Case("empty", string.Empty, false) },
            { new Case("space", " ", false) },
            { new Case("wrong format", "2026-1-06", false) },
            { new Case("invalid month", "2026-13-01", false) },
            { new Case("compact", "20260106", false) },
        };

        #region Cases

        public sealed record Case(string Name, string? Value, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion Cases
    }

    public static class IsIsoDateTime
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case("seconds", "2026-01-06T12:34:56", true) },
            { new Case("milliseconds", "2026-01-06T12:34:56.123", true) },
            { new Case("padded", " 2026-01-06T12:34:56 ", true) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case("null", null, false) },
            { new Case("empty", string.Empty, false) },
            { new Case("space", " ", false) },
            { new Case("date only", "2026-01-06", false) },
            { new Case("no seconds", "2026-01-06T12:34", false) },
            { new Case("Z", "2026-01-06T12:34:56Z", false) },
            { new Case("offset", "2026-01-06T12:34:56+00:00", false) },
        };

        #region Cases

        public sealed record Case(string Name, string? Value, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion Cases
    }

    public static class IsIsoDateTimeOffset
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case("Z", "2026-01-06T12:34:56Z", true) },
            { new Case("Z milliseconds", "2026-01-06T12:34:56.123Z", true) },
            { new Case("+00:00", "2026-01-06T12:34:56+00:00", true) },
            { new Case("+01:30 milliseconds", "2026-01-06T12:34:56.123+01:30", true) },
            { new Case("padded", " 2026-01-06T12:34:56Z ", true) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case("null", null, false) },
            { new Case("empty", string.Empty, false) },
            { new Case("space", " ", false) },
            { new Case("date only", "2026-01-06", false) },
            { new Case("no seconds", "2026-01-06T12:34", false) },
            { new Case("offset compact", "2026-01-06T12:34:56+0000", false) },
            { new Case("offset short", "2026-01-06T12:34:56+00", false) },
            { new Case("too many fractional seconds", "2026-01-06T12:34:56.12345678Z", false) },
        };

        #region Cases

        public sealed record Case(string Name, string? Value, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion Cases
    }

    public static class StringDateOnlyIsBetween
    {
        public static TheoryData<Case> Cases => new()
        {
            { new Case("between", "2026-01-02", new DateOnly(2026, 01, 01), new DateOnly(2026, 01, 03), Inclusion.Inclusive, true) },
            { new Case("min inclusive", "2026-01-01", new DateOnly(2026, 01, 01), new DateOnly(2026, 01, 03), Inclusion.Inclusive, true) },
            { new Case("min exclusive", "2026-01-01", new DateOnly(2026, 01, 01), new DateOnly(2026, 01, 03), Inclusion.Exclusive, false) },
            { new Case("null", null, new DateOnly(2026, 01, 01), new DateOnly(2026, 01, 03), Inclusion.Inclusive, false) },
            { new Case("not a date", "not-a-date", new DateOnly(2026, 01, 01), new DateOnly(2026, 01, 03), Inclusion.Inclusive, false) },
        };

        #region Cases

        public sealed record Case(string Name, string? Value, DateOnly Min, DateOnly Max, Inclusion Inclusion, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion Cases
    }
}
