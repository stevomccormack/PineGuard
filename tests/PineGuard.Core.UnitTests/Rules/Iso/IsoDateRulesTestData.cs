using PineGuard.Common;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Rules.Iso;

public static class IsoDateRulesTestData
{
    public static class IsIsoDateOnly
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("valid", "2026-01-06", true),
            new("padded", " 2026-01-06 ", true)
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("null", null, false),
            new("empty", string.Empty, false),
            new("space", " ", false),
            new("wrong format", "2026-1-06", false),
            new("invalid month", "2026-13-01", false),
            new("compact", "20260106", false)
        ];

        #region Case Records

        public sealed record Case(string Name, string? Value, bool ExpectedReturn)
            : IsCase<string?>(Name, Value, ExpectedReturn);

        #endregion Cases
    }

    public static class IsIsoDateTime
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("seconds", "2026-01-06T12:34:56", true),
            new("milliseconds", "2026-01-06T12:34:56.123", true),
            new("padded", " 2026-01-06T12:34:56 ", true)
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("null", null, false),
            new("empty", string.Empty, false),
            new("space", " ", false),
            new("date only", "2026-01-06", false),
            new("no seconds", "2026-01-06T12:34", false),
            new("Z", "2026-01-06T12:34:56Z", false),
            new("offset", "2026-01-06T12:34:56+00:00", false)
        ];

        #region Case Records

        public sealed record Case(string Name, string? Value, bool ExpectedReturn)
            : IsCase<string?>(Name, Value, ExpectedReturn);

        #endregion Cases
    }

    public static class IsIsoDateTimeOffset
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("Z", "2026-01-06T12:34:56Z", true),
            new("Z milliseconds", "2026-01-06T12:34:56.123Z", true),
            new("+00:00", "2026-01-06T12:34:56+00:00", true),
            new("+01:30 milliseconds", "2026-01-06T12:34:56.123+01:30", true),
            new("padded", " 2026-01-06T12:34:56Z ", true)
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("null", null, false),
            new("empty", string.Empty, false),
            new("space", " ", false),
            new("date only", "2026-01-06", false),
            new("no seconds", "2026-01-06T12:34", false),
            new("offset compact", "2026-01-06T12:34:56+0000", false),
            new("offset short", "2026-01-06T12:34:56+00", false),
            new("too many fractional seconds", "2026-01-06T12:34:56.12345678Z", false)
        ];

        #region Case Records

        public sealed record Case(string Name, string? Value, bool ExpectedReturn)
            : IsCase<string?>(Name, Value, ExpectedReturn);

        #endregion Cases
    }

    public static class StringDateOnlyIsBetween
    {
        public static TheoryData<Case> Cases =>
        [
            new("between", "2026-01-02", new DateOnly(2026, 01, 01), new DateOnly(2026, 01, 03),
                Inclusion.Inclusive, true),
            new("min inclusive", "2026-01-01", new DateOnly(2026, 01, 01), new DateOnly(2026, 01, 03),
                Inclusion.Inclusive, true),
            new("min exclusive", "2026-01-01", new DateOnly(2026, 01, 01), new DateOnly(2026, 01, 03),
                Inclusion.Exclusive, false),
            new("null", null, new DateOnly(2026, 01, 01), new DateOnly(2026, 01, 03), Inclusion.Inclusive, false),
            new("not a date", "not-a-date", new DateOnly(2026, 01, 01), new DateOnly(2026, 01, 03),
                Inclusion.Inclusive, false)
        ];

        #region Case Records

        public sealed record Case(string Name, string? Value, DateOnly Min, DateOnly Max, Inclusion Inclusion, bool ExpectedReturn)
            : IsCase<string?>(Name, Value, ExpectedReturn);

        #endregion Cases
    }
}
