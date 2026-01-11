using PineGuard.Common;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Rules;

public static class TimeSpanRulesTestData
{
    public static class IsDurationBetween
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("middle inclusive", TimeSpan.FromMinutes(30), TimeSpan.FromMinutes(10), TimeSpan.FromMinutes(60), Inclusion.Inclusive, true),
            new("at min inclusive", TimeSpan.FromMinutes(10), TimeSpan.FromMinutes(10), TimeSpan.FromMinutes(60), Inclusion.Inclusive, true),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("at min exclusive", TimeSpan.FromMinutes(10), TimeSpan.FromMinutes(10), TimeSpan.FromMinutes(60), Inclusion.Exclusive, false),
            new("null", null, TimeSpan.FromMinutes(10), TimeSpan.FromMinutes(60), Inclusion.Inclusive, false),
        ];

        #region Case Records

        public sealed record Case(string Name, TimeSpan? Value, TimeSpan Min, TimeSpan Max, Inclusion Inclusion, bool ExpectedReturn)
            : IsCase<TimeSpan?>(Name, Value, ExpectedReturn);

        #endregion
    }

    public static class IsGreaterThan
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("above exclusive", TimeSpan.FromMinutes(11), TimeSpan.FromMinutes(10), Inclusion.Exclusive, true),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("equal exclusive", TimeSpan.FromMinutes(10), TimeSpan.FromMinutes(10), Inclusion.Exclusive, false),
            new("equal inclusive", TimeSpan.FromMinutes(10), TimeSpan.FromMinutes(10), Inclusion.Inclusive, true),
            new("null", null, TimeSpan.FromMinutes(10), Inclusion.Inclusive, false),
        ];

        #region Case Records

        public sealed record Case(string Name, TimeSpan? Value, TimeSpan Threshold, Inclusion Inclusion, bool ExpectedReturn)
            : IsCase<TimeSpan?>(Name, Value, ExpectedReturn);

        #endregion
    }

    public static class IsLessThan
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("below exclusive", TimeSpan.FromMinutes(9), TimeSpan.FromMinutes(10), Inclusion.Exclusive, true),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("equal exclusive", TimeSpan.FromMinutes(10), TimeSpan.FromMinutes(10), Inclusion.Exclusive, false),
            new("equal inclusive", TimeSpan.FromMinutes(10), TimeSpan.FromMinutes(10), Inclusion.Inclusive, true),
            new("null", null, TimeSpan.FromMinutes(10), Inclusion.Inclusive, false),
        ];

        #region Case Records

        public sealed record Case(string Name, TimeSpan? Value, TimeSpan Threshold, Inclusion Inclusion, bool ExpectedReturn)
            : IsCase<TimeSpan?>(Name, Value, ExpectedReturn);

        #endregion
    }
}
