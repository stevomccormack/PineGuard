using PineGuard.Common;
using Xunit;

namespace PineGuard.Core.UnitTests.Rules.StringRules;

public static class StringRulesTimeOnlyTestData
{
    public static class IsBetween
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case(Name: "12:00 in [11:00,13:00] => true", Value: "12:00:00", Min: new global::System.TimeOnly(11, 0), Max: new global::System.TimeOnly(13, 0), Inclusion: Inclusion.Inclusive, Expected: true) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case(Name: "11:00 in (11:00,13:00) => false", Value: "11:00:00", Min: new global::System.TimeOnly(11, 0), Max: new global::System.TimeOnly(13, 0), Inclusion: Inclusion.Exclusive, Expected: false) },
            { new Case(Name: "not-a-time => false", Value: "not-a-time", Min: new global::System.TimeOnly(11, 0), Max: new global::System.TimeOnly(13, 0), Inclusion: Inclusion.Inclusive, Expected: false) },
        };

        #region Cases

        public sealed record Case(
            string Name,
            string? Value,
            global::System.TimeOnly Min,
            global::System.TimeOnly Max,
            Inclusion Inclusion,
            bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }

    public static class IsChronological
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case(Name: "11:00 < 12:00 => true", Start: "11:00:00", End: "12:00:00", Inclusion: Inclusion.Exclusive, Expected: true) },
            { new Case(Name: "11:00 <= 11:00 (inclusive) => true", Start: "11:00:00", End: "11:00:00", Inclusion: Inclusion.Inclusive, Expected: true) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case(Name: "11:00 < 11:00 (exclusive) => false", Start: "11:00:00", End: "11:00:00", Inclusion: Inclusion.Exclusive, Expected: false) },
            { new Case(Name: "not-a-time start => false", Start: "not-a-time", End: "11:00:00", Inclusion: Inclusion.Exclusive, Expected: false) },
            { new Case(Name: "not-a-time end => false", Start: "11:00:00", End: "not-a-time", Inclusion: Inclusion.Exclusive, Expected: false) },
        };

        #region Cases

        public sealed record Case(string Name, string? Start, string? End, Inclusion Inclusion, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }

    public static class IsOverlapping
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case(Name: "09-10 overlaps 09:30-09:45 => true", Start1: "09:00:00", End1: "10:00:00", Start2: "09:30:00", End2: "09:45:00", Inclusion: Inclusion.Exclusive, Expected: true) },
            { new Case(Name: "touching endpoints inclusive => true", Start1: "09:00:00", End1: "10:00:00", Start2: "10:00:00", End2: "11:00:00", Inclusion: Inclusion.Inclusive, Expected: true) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case(Name: "touching endpoints exclusive => false", Start1: "09:00:00", End1: "10:00:00", Start2: "10:00:00", End2: "11:00:00", Inclusion: Inclusion.Exclusive, Expected: false) },
            { new Case(Name: "not-a-time start1 => false", Start1: "not-a-time", End1: "10:00:00", Start2: "09:30:00", End2: "09:45:00", Inclusion: Inclusion.Exclusive, Expected: false) },
            { new Case(Name: "not-a-time end1 => false", Start1: "09:00:00", End1: "not-a-time", Start2: "09:30:00", End2: "09:45:00", Inclusion: Inclusion.Exclusive, Expected: false) },
            { new Case(Name: "not-a-time start2 => false", Start1: "09:00:00", End1: "10:00:00", Start2: "not-a-time", End2: "09:45:00", Inclusion: Inclusion.Exclusive, Expected: false) },
            { new Case(Name: "not-a-time end2 => false", Start1: "09:00:00", End1: "10:00:00", Start2: "09:30:00", End2: "not-a-time", Inclusion: Inclusion.Exclusive, Expected: false) },
        };

        #region Cases

        public sealed record Case(
            string Name,
            string? Start1,
            string? End1,
            string? Start2,
            string? End2,
            Inclusion Inclusion,
            bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }
}
