using PineGuard.Common;
using Xunit;

namespace PineGuard.Core.UnitTests.Rules.StringRules;

public static class StringRulesDateTimeOffsetTestData
{
    public static class IsInPast
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case(Name: "2000-01-01Z => true", Value: "2000-01-01T00:00:00Z", Expected: true) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case(Name: "2999-01-01Z => false", Value: "2999-01-01T00:00:00Z", Expected: false) },
            { new Case(Name: "not-a-date => false", Value: "not-a-date", Expected: false) },
            { new Case(Name: "null => false", Value: null, Expected: false) },
            { new Case(Name: "space => false", Value: " ", Expected: false) },
        };

        #region Cases

        public sealed record Case(string Name, string? Value, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }

    public static class IsInFuture
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case(Name: "2999-01-01Z => true", Value: "2999-01-01T00:00:00Z", Expected: true) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case(Name: "2000-01-01Z => false", Value: "2000-01-01T00:00:00Z", Expected: false) },
            { new Case(Name: "not-a-date => false", Value: "not-a-date", Expected: false) },
            { new Case(Name: "null => false", Value: null, Expected: false) },
            { new Case(Name: "space => false", Value: " ", Expected: false) },
        };

        #region Cases

        public sealed record Case(string Name, string? Value, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }

    public static class IsBetween
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case(Name: "2020-01-01T12Z in [2020-01-01Z,2020-01-02Z] => true", Value: "2020-01-01T12:00:00Z", Min: global::System.DateTimeOffset.Parse("2020-01-01T00:00:00Z"), Max: global::System.DateTimeOffset.Parse("2020-01-02T00:00:00Z"), Inclusion: Inclusion.Inclusive, Expected: true) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case(Name: "min in exclusive range => false", Value: "2020-01-01T00:00:00Z", Min: global::System.DateTimeOffset.Parse("2020-01-01T00:00:00Z"), Max: global::System.DateTimeOffset.Parse("2020-01-02T00:00:00Z"), Inclusion: Inclusion.Exclusive, Expected: false) },
            { new Case(Name: "not-a-date => false", Value: "not-a-date", Min: global::System.DateTimeOffset.Parse("2020-01-01T00:00:00Z"), Max: global::System.DateTimeOffset.Parse("2020-01-02T00:00:00Z"), Inclusion: Inclusion.Inclusive, Expected: false) },
        };

        #region Cases

        public sealed record Case(
            string Name,
            string? Value,
            global::System.DateTimeOffset Min,
            global::System.DateTimeOffset Max,
            Inclusion Inclusion,
            bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }
}
