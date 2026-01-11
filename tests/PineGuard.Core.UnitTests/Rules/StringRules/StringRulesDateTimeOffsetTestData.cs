using PineGuard.Common;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Rules.StringRules;

public static class StringRulesDateTimeOffsetTestData
{
    public static class IsInPast
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("2000-01-01Z => true", "2000-01-01T00:00:00Z", true),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("2999-01-01Z => false", "2999-01-01T00:00:00Z", false),
            new("not-a-date => false", "not-a-date", false),
            new("null => false", null, false),
            new("space => false", " ", false),
        ];

        #region Case Records

        public sealed record Case(string Name, string? Value, bool ExpectedReturn)
            : IsCase<string?>(Name, Value, ExpectedReturn);

        #endregion
    }

    public static class IsInFuture
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("2999-01-01Z => true", "2999-01-01T00:00:00Z", true),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("2000-01-01Z => false", "2000-01-01T00:00:00Z", false),
            new("not-a-date => false", "not-a-date", false),
            new("null => false", null, false),
            new("space => false", " ", false),
        ];

        #region Case Records

        public sealed record Case(string Name, string? Value, bool ExpectedReturn)
            : IsCase<string?>(Name, Value, ExpectedReturn);

        #endregion
    }

    public static class IsBetween
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("2020-01-01T12Z in [2020-01-01Z,2020-01-02Z] => true", ("2020-01-01T12:00:00Z", DateTimeOffset.Parse("2020-01-01T00:00:00Z"), DateTimeOffset.Parse("2020-01-02T00:00:00Z"), Inclusion.Inclusive), true),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("min in exclusive range => false", ("2020-01-01T00:00:00Z", DateTimeOffset.Parse("2020-01-01T00:00:00Z"), DateTimeOffset.Parse("2020-01-02T00:00:00Z"), Inclusion.Exclusive), false),
            new("not-a-date => false", ("not-a-date", DateTimeOffset.Parse("2020-01-01T00:00:00Z"), DateTimeOffset.Parse("2020-01-02T00:00:00Z"), Inclusion.Inclusive), false),
        ];

        #region Case Records

        public sealed record Case(
            string Name,
            (string? Text, DateTimeOffset Min, DateTimeOffset Max, Inclusion Inclusion) Value,
            bool ExpectedReturn)
            : IsCase<(string? Text, DateTimeOffset Min, DateTimeOffset Max, Inclusion Inclusion)>(Name, Value, ExpectedReturn);

        #endregion
    }
}
