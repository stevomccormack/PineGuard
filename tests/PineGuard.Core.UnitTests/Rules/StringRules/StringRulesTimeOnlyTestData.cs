using PineGuard.Common;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Rules.StringRules;

public static class StringRulesTimeOnlyTestData
{
    public static class IsBetween
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("12:00 in [11:00,13:00] => true", ("12:00:00", new TimeOnly(11, 0), new TimeOnly(13, 0), Inclusion.Inclusive), true),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("11:00 in (11:00,13:00) => false", ("11:00:00", new TimeOnly(11, 0), new TimeOnly(13, 0), Inclusion.Exclusive), false),
            new("not-a-time => false", ("not-a-time", new TimeOnly(11, 0), new TimeOnly(13, 0), Inclusion.Inclusive), false),
        ];

        #region Case Records

        public sealed record Case(
            string Name,
            (string? Text, TimeOnly Min, TimeOnly Max, Inclusion Inclusion) Value,
            bool ExpectedReturn)
            : IsCase<(string? Text, TimeOnly Min, TimeOnly Max, Inclusion Inclusion)>(Name, Value, ExpectedReturn);

        #endregion
    }

    public static class IsChronological
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("11:00 < 12:00 => true", ("11:00:00", "12:00:00", Inclusion.Exclusive), true),
            new("11:00 <= 11:00 (inclusive) => true", ("11:00:00", "11:00:00", Inclusion.Inclusive), true),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("11:00 < 11:00 (exclusive) => false", ("11:00:00", "11:00:00", Inclusion.Exclusive), false),
            new("not-a-time start => false", ("not-a-time", "11:00:00", Inclusion.Exclusive), false),
            new("not-a-time end => false", ("11:00:00", "not-a-time", Inclusion.Exclusive), false),
        ];

        #region Case Records

        public sealed record Case(string Name, (string? Start, string? End, Inclusion Inclusion) Value, bool ExpectedReturn)
            : IsCase<(string? Start, string? End, Inclusion Inclusion)>(Name, Value, ExpectedReturn);

        #endregion
    }

    public static class IsOverlapping
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("09-10 overlaps 09:30-09:45 => true", ("09:00:00", "10:00:00", "09:30:00", "09:45:00", Inclusion.Exclusive), true),
            new("touching endpoints inclusive => true", ("09:00:00", "10:00:00", "10:00:00", "11:00:00", Inclusion.Inclusive), true),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("touching endpoints exclusive => false", ("09:00:00", "10:00:00", "10:00:00", "11:00:00", Inclusion.Exclusive), false),
            new("not-a-time start1 => false", ("not-a-time", "10:00:00", "09:30:00", "09:45:00", Inclusion.Exclusive), false),
            new("not-a-time end1 => false", ("09:00:00", "not-a-time", "09:30:00", "09:45:00", Inclusion.Exclusive), false),
            new("not-a-time start2 => false", ("09:00:00", "10:00:00", "not-a-time", "09:45:00", Inclusion.Exclusive), false),
            new("not-a-time end2 => false", ("09:00:00", "10:00:00", "09:30:00", "not-a-time", Inclusion.Exclusive), false),
        ];

        #region Case Records

        public sealed record Case(
            string Name,
            (string? Start1, string? End1, string? Start2, string? End2, Inclusion Inclusion) Value,
            bool ExpectedReturn)
            : IsCase<(string? Start1, string? End1, string? Start2, string? End2, Inclusion Inclusion)>(Name, Value, ExpectedReturn);

        #endregion
    }
}
