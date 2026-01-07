using PineGuard.Common;
using Xunit;

namespace PineGuard.Core.UnitTests.Rules.StringRules;

public static class StringRulesTimeSpanTestData
{
    public static class IsDurationBetween
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case(Name: "00:30 in [00:10,01:00] => true", Value: "00:30:00", Min: global::System.TimeSpan.FromMinutes(10), Max: global::System.TimeSpan.FromMinutes(60), Inclusion: Inclusion.Inclusive, Expected: true) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case(Name: "00:10 in (00:10,01:00) => false", Value: "00:10:00", Min: global::System.TimeSpan.FromMinutes(10), Max: global::System.TimeSpan.FromMinutes(60), Inclusion: Inclusion.Exclusive, Expected: false) },
            { new Case(Name: "not-a-duration => false", Value: "not-a-duration", Min: global::System.TimeSpan.FromMinutes(10), Max: global::System.TimeSpan.FromMinutes(60), Inclusion: Inclusion.Inclusive, Expected: false) },
        };

        #region Cases

        public sealed record Case(
            string Name,
            string? Value,
            global::System.TimeSpan Min,
            global::System.TimeSpan Max,
            Inclusion Inclusion,
            bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }

    public static class IsGreaterThan
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case(Name: "00:11 > 00:10 (exclusive) => true", Value: "00:11:00", Threshold: global::System.TimeSpan.FromMinutes(10), Inclusion: Inclusion.Exclusive, Expected: true) },
            { new Case(Name: "00:10 >= 00:10 (inclusive) => true", Value: "00:10:00", Threshold: global::System.TimeSpan.FromMinutes(10), Inclusion: Inclusion.Inclusive, Expected: true) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case(Name: "00:10 > 00:10 (exclusive) => false", Value: "00:10:00", Threshold: global::System.TimeSpan.FromMinutes(10), Inclusion: Inclusion.Exclusive, Expected: false) },
            { new Case(Name: "not-a-duration => false", Value: "not-a-duration", Threshold: global::System.TimeSpan.FromMinutes(10), Inclusion: Inclusion.Inclusive, Expected: false) },
        };

        #region Cases

        public sealed record Case(
            string Name,
            string? Value,
            global::System.TimeSpan Threshold,
            Inclusion Inclusion,
            bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }

    public static class IsLessThan
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case(Name: "00:09 < 00:10 (exclusive) => true", Value: "00:09:00", Threshold: global::System.TimeSpan.FromMinutes(10), Inclusion: Inclusion.Exclusive, Expected: true) },
            { new Case(Name: "00:10 <= 00:10 (inclusive) => true", Value: "00:10:00", Threshold: global::System.TimeSpan.FromMinutes(10), Inclusion: Inclusion.Inclusive, Expected: true) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case(Name: "00:10 < 00:10 (exclusive) => false", Value: "00:10:00", Threshold: global::System.TimeSpan.FromMinutes(10), Inclusion: Inclusion.Exclusive, Expected: false) },
            { new Case(Name: "not-a-duration => false", Value: "not-a-duration", Threshold: global::System.TimeSpan.FromMinutes(10), Inclusion: Inclusion.Inclusive, Expected: false) },
        };

        #region Cases

        public sealed record Case(
            string Name,
            string? Value,
            global::System.TimeSpan Threshold,
            Inclusion Inclusion,
            bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }
}
