using PineGuard.Common;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Rules.StringRules;

public static class StringRulesTimeSpanTestData
{
    public static class IsDurationBetween
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("00:30 in [00:10,01:00] => true", ("00:30:00", TimeSpan.FromMinutes(10), TimeSpan.FromMinutes(60), Inclusion.Inclusive), true),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("00:10 in (00:10,01:00) => false", ("00:10:00", TimeSpan.FromMinutes(10), TimeSpan.FromMinutes(60), Inclusion.Exclusive), false),
            new("not-a-duration => false", ("not-a-duration", TimeSpan.FromMinutes(10), TimeSpan.FromMinutes(60), Inclusion.Inclusive), false),
        ];

        #region Case Records

        public sealed record Case(
            string Name,
            (string? Text, TimeSpan Min, TimeSpan Max, Inclusion Inclusion) Value,
            bool ExpectedReturn)
            : IsCase<(string? Text, TimeSpan Min, TimeSpan Max, Inclusion Inclusion)>(Name, Value, ExpectedReturn);

        #endregion
    }

    public static class IsGreaterThan
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("00:11 > 00:10 (exclusive) => true", ("00:11:00", TimeSpan.FromMinutes(10), Inclusion.Exclusive), true),
            new("00:10 >= 00:10 (inclusive) => true", ("00:10:00", TimeSpan.FromMinutes(10), Inclusion.Inclusive), true),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("00:10 > 00:10 (exclusive) => false", ("00:10:00", TimeSpan.FromMinutes(10), Inclusion.Exclusive), false),
            new("not-a-duration => false", ("not-a-duration", TimeSpan.FromMinutes(10), Inclusion.Inclusive), false),
        ];

        #region Case Records

        public sealed record Case(
            string Name,
            (string? Text, TimeSpan Threshold, Inclusion Inclusion) Value,
            bool ExpectedReturn)
            : IsCase<(string? Text, TimeSpan Threshold, Inclusion Inclusion)>(Name, Value, ExpectedReturn);

        #endregion
    }

    public static class IsLessThan
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("00:09 < 00:10 (exclusive) => true", ("00:09:00", TimeSpan.FromMinutes(10), Inclusion.Exclusive), true),
            new("00:10 <= 00:10 (inclusive) => true", ("00:10:00", TimeSpan.FromMinutes(10), Inclusion.Inclusive), true),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("00:10 < 00:10 (exclusive) => false", ("00:10:00", TimeSpan.FromMinutes(10), Inclusion.Exclusive), false),
            new("not-a-duration => false", ("not-a-duration", TimeSpan.FromMinutes(10), Inclusion.Inclusive), false),
        ];

        #region Case Records

        public sealed record Case(
            string Name,
            (string? Text, TimeSpan Threshold, Inclusion Inclusion) Value,
            bool ExpectedReturn)
            : IsCase<(string? Text, TimeSpan Threshold, Inclusion Inclusion)>(Name, Value, ExpectedReturn);

        #endregion
    }
}
