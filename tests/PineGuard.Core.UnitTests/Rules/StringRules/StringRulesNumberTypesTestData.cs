using PineGuard.Common;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Rules.StringRules;

public static class StringRulesNumberTypesTestData
{
    public static class IsDecimal
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("1.23 => true", "1.23", true),
            new("-1.2 => true", "-1.2", true),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("null => false", null, false),
            new("space => false", " ", false),
            new("not => false", "not", false),
            new("too many decimals => false", "1.234", false),
        ];

        #region Case Records

        public sealed record Case(string Name, string? Value, bool ExpectedReturn)
            : IsCase<string?>(Name, Value, ExpectedReturn);

        #endregion
    }

    public static class IsDecimalWithZeroPlaces
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("123 => true", "123", true),
            new("+123 => true", "+123", true),
            new("-0 => true", "-0", true),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("1.0 => false", "1.0", false),
        ];

        #region Case Records

        public sealed record Case(string Name, string Value, bool ExpectedReturn)
            : IsCase<string>(Name, Value, ExpectedReturn);

        #endregion
    }

    public static class IsExactDecimal
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("1.20 => true", "1.20", true),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("null => false", null, false),
            new("space => false", " ", false),
            new("not => false", "not", false),
            new("not enough decimals => false", "1.2", false),
            new("too many decimals => false", "1.230", false),
        ];

        #region Case Records

        public sealed record Case(string Name, string? Value, bool ExpectedReturn)
            : IsCase<string?>(Name, Value, ExpectedReturn);

        #endregion
    }

    public static class IsExactDecimalWithZeroPlaces
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("123 => true", "123", true),
            new("+123 => true", "+123", true),
            new("-0 => true", "-0", true),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("1.0 => false", "1.0", false),
        ];

        #region Case Records

        public sealed record Case(string Name, string Value, bool ExpectedReturn)
            : IsCase<string>(Name, Value, ExpectedReturn);

        #endregion
    }

    public static class IsInt32
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("int.MaxValue => true", "2147483647", true),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("int.MaxValue+1 => false", "2147483648", false),
            new("null => false", null, false),
        ];

        #region Case Records

        public sealed record Case(string Name, string? Value, bool ExpectedReturn)
            : IsCase<string?>(Name, Value, ExpectedReturn);

        #endregion
    }

    public static class IsInt64
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("long.MaxValue => true", "9223372036854775807", true),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("long.MaxValue+1 => false", "9223372036854775808", false),
            new("null => false", null, false),
        ];

        #region Case Records

        public sealed record Case(string Name, string? Value, bool ExpectedReturn)
            : IsCase<string?>(Name, Value, ExpectedReturn);

        #endregion
    }

    public static class IsInt32InRange
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("5 in [1,10] => true", ("5", 1, 10, Inclusion.Inclusive), true),
            new("1 in [1,10] => true", ("1", 1, 10, Inclusion.Inclusive), true),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("1 in (1,10) => false", ("1", 1, 10, Inclusion.Exclusive), false),
            new("not => false", ("not", 1, 10, Inclusion.Inclusive), false),
        ];

        #region Case Records

        public sealed record Case(
            string Name,
            (string Text, int Min, int Max, Inclusion Inclusion) Value,
            bool ExpectedReturn)
            : IsCase<(string Text, int Min, int Max, Inclusion Inclusion)>(Name, Value, ExpectedReturn);

        #endregion
    }

    public static class IsInt64InRange
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("5 in [1,10] => true", ("5", 1L, 10L, Inclusion.Inclusive), true),
            new("1 in [1,10] => true", ("1", 1L, 10L, Inclusion.Inclusive), true),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("1 in (1,10) => false", ("1", 1L, 10L, Inclusion.Exclusive), false),
            new("not => false", ("not", 1L, 10L, Inclusion.Inclusive), false),
        ];

        #region Case Records

        public sealed record Case(
            string Name,
            (string Text, long Min, long Max, Inclusion Inclusion) Value,
            bool ExpectedReturn)
            : IsCase<(string Text, long Min, long Max, Inclusion Inclusion)>(Name, Value, ExpectedReturn);

        #endregion
    }
}
