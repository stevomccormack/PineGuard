using PineGuard.Common;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Rules.StringRules;

public static class StringRulesNumbersTestData
{
    public static class IsPositive
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("1 => true", "1", true),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("0 => false", "0", false),
            new("-1 => false", "-1", false),
            new("null => false", null, false),
            new("space => false", " ", false),
            new("abc => false", "abc", false),
        ];

        #region Case Records

        public sealed record Case(string Name, string? Value, bool ExpectedReturn)
            : IsCase<string?>(Name, Value, ExpectedReturn);

        #endregion
    }

    public static class IsNegative
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("-1 => true", "-1", true),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("0 => false", "0", false),
            new("1 => false", "1", false),
            new("abc => false", "abc", false),
        ];

        #region Case Records

        public sealed record Case(string Name, string? Value, bool ExpectedReturn)
            : IsCase<string?>(Name, Value, ExpectedReturn);

        #endregion
    }

    public static class IsZero
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("0 => true", "0", true),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("1 => false", "1", false),
            new("abc => false", "abc", false),
        ];

        #region Case Records

        public sealed record Case(string Name, string? Value, bool ExpectedReturn)
            : IsCase<string?>(Name, Value, ExpectedReturn);

        #endregion
    }

    public static class IsNotZero
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("1 => true", "1", true),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("0 => false", "0", false),
            new("abc => false", "abc", false),
        ];

        #region Case Records

        public sealed record Case(string Name, string? Value, bool ExpectedReturn)
            : IsCase<string?>(Name, Value, ExpectedReturn);

        #endregion
    }

    public static class IsZeroOrPositive
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("0 => true", "0", true),
            new("1 => true", "1", true),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("-1 => false", "-1", false),
            new("abc => false", "abc", false),
        ];

        #region Case Records

        public sealed record Case(string Name, string? Value, bool ExpectedReturn)
            : IsCase<string?>(Name, Value, ExpectedReturn);

        #endregion
    }

    public static class IsZeroOrNegative
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("0 => true", "0", true),
            new("-1 => true", "-1", true),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("1 => false", "1", false),
            new("abc => false", "abc", false),
        ];

        #region Case Records

        public sealed record Case(string Name, string? Value, bool ExpectedReturn)
            : IsCase<string?>(Name, Value, ExpectedReturn);

        #endregion
    }

    public static class IsGreaterThan
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("2 > 1 => true", ("2", 1m), true),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("1 > 1 => false", ("1", 1m), false),
            new("abc > 1 => false", ("abc", 1m), false),
        ];

        #region Case Records

        public sealed record Case(string Name, (string? Text, decimal Min) Value, bool ExpectedReturn)
            : IsCase<(string? Text, decimal Min)>(Name, Value, ExpectedReturn);

        #endregion
    }

    public static class IsGreaterThanOrEqual
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("2 >= 1 => true", ("2", 1m), true),
            new("1 >= 1 => true", ("1", 1m), true),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("0 >= 1 => false", ("0", 1m), false),
            new("abc >= 1 => false", ("abc", 1m), false),
        ];

        #region Case Records

        public sealed record Case(string Name, (string? Text, decimal Min) Value, bool ExpectedReturn)
            : IsCase<(string? Text, decimal Min)>(Name, Value, ExpectedReturn);

        #endregion
    }

    public static class IsLessThan
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("0 < 1 => true", ("0", 1m), true),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("1 < 1 => false", ("1", 1m), false),
            new("abc < 1 => false", ("abc", 1m), false),
        ];

        #region Case Records

        public sealed record Case(string Name, (string? Text, decimal Max) Value, bool ExpectedReturn)
            : IsCase<(string? Text, decimal Max)>(Name, Value, ExpectedReturn);

        #endregion
    }

    public static class IsLessThanOrEqual
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("0 <= 1 => true", ("0", 1m), true),
            new("1 <= 1 => true", ("1", 1m), true),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("2 <= 1 => false", ("2", 1m), false),
            new("abc <= 1 => false", ("abc", 1m), false),
        ];

        #region Case Records

        public sealed record Case(string Name, (string? Text, decimal Max) Value, bool ExpectedReturn)
            : IsCase<(string? Text, decimal Max)>(Name, Value, ExpectedReturn);

        #endregion
    }

    public static class IsInRange
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("5 in [1,10] => true", ("5", 1m, 10m, Inclusion.Inclusive), true),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("1 in (1,10) => false", ("1", 1m, 10m, Inclusion.Exclusive), false),
            new("abc in [1,10] => false", ("abc", 1m, 10m, Inclusion.Inclusive), false),
        ];

        #region Case Records

        public sealed record Case(
            string Name,
            (string? Text, decimal Min, decimal Max, Inclusion Inclusion) Value,
            bool ExpectedReturn)
            : IsCase<(string? Text, decimal Min, decimal Max, Inclusion Inclusion)>(Name, Value, ExpectedReturn);

        #endregion
    }

    public static class IsApproximately
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("10.0 ~= 10.1 +/- 0.2 => true", ("10.0", 10.1m, 0.2m), true),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("10.0 ~= 10.3 +/- 0.2 => false", ("10.0", 10.3m, 0.2m), false),
            new("abc => false", ("abc", 10.0m, 0.2m), false),
            new("null tolerance => false", ("10.0", 10.0m, null), false),
            new("negative tolerance => false", ("10.0", 10.0m, -0.1m), false),
        ];

        #region Case Records

        public sealed record Case(
            string Name,
            (string? Text, decimal Target, decimal? Tolerance) Value,
            bool ExpectedReturn)
            : IsCase<(string? Text, decimal Target, decimal? Tolerance)>(Name, Value, ExpectedReturn);

        #endregion
    }

    public static class IsMultipleOf
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("4 multiple of 2 => true", ("4", 2m), true),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("5 multiple of 2 => false", ("5", 2m), false),
            new("abc => false", ("abc", 2m), false),
        ];

        #region Case Records

        public sealed record Case(string Name, (string? Text, decimal Factor) Value, bool ExpectedReturn)
            : IsCase<(string? Text, decimal Factor)>(Name, Value, ExpectedReturn);

        #endregion
    }

    public static class IsEven
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("4 => true", "4", true),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("5 => false", "5", false),
            new("4.0 => false", "4.0", false),
            new("abc => false", "abc", false),
        ];

        #region Case Records

        public sealed record Case(string Name, string? Value, bool ExpectedReturn)
            : IsCase<string?>(Name, Value, ExpectedReturn);

        #endregion
    }

    public static class IsOdd
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("5 => true", "5", true),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("4 => false", "4", false),
            new("5.0 => false", "5.0", false),
            new("abc => false", "abc", false),
        ];

        #region Case Records

        public sealed record Case(string Name, string? Value, bool ExpectedReturn)
            : IsCase<string?>(Name, Value, ExpectedReturn);

        #endregion
    }

    public static class IsFinite
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("1.23 => true", "1.23", true),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("abc => false", "abc", false),
        ];

        #region Case Records

        public sealed record Case(string Name, string? Value, bool ExpectedReturn)
            : IsCase<string?>(Name, Value, ExpectedReturn);

        #endregion
    }

    public static class IsNaN
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("NaN => true", "NaN", true),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("1.23 => false", "1.23", false),
            new("abc => false", "abc", false),
        ];

        #region Case Records

        public sealed record Case(string Name, string? Value, bool ExpectedReturn)
            : IsCase<string?>(Name, Value, ExpectedReturn);

        #endregion
    }
}
