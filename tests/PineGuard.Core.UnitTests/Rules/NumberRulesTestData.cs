using PineGuard.Common;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Rules;

public static class NumberRulesTestData
{
    public static class IsPositiveInt
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("positive", 1, true)
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("null", null, false),
            new("zero", 0, false),
            new("negative", -1, false)
        ];

        #region Case Records

        public sealed record Case(string Name, int? Value, bool ExpectedReturn)
            : IsCase<int?>(Name, Value, ExpectedReturn);

        #endregion
    }

    public static class IsNegativeInt
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("negative", -1, true)
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("null", null, false),
            new("zero", 0, false),
            new("positive", 1, false)
        ];

        #region Case Records

        public sealed record Case(string Name, int? Value, bool ExpectedReturn)
            : IsCase<int?>(Name, Value, ExpectedReturn);

        #endregion
    }

    public static class IsZeroInt
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("zero", 0, true)
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("null", null, false),
            new("non-zero", 1, false)
        ];

        #region Case Records

        public sealed record Case(string Name, int? Value, bool ExpectedReturn)
            : IsCase<int?>(Name, Value, ExpectedReturn);

        #endregion
    }

    public static class IsNotZeroInt
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("positive", 1, true),
            new("negative", -1, true)
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("null", null, false),
            new("zero", 0, false)
        ];

        #region Case Records

        public sealed record Case(string Name, int? Value, bool ExpectedReturn)
            : IsCase<int?>(Name, Value, ExpectedReturn);

        #endregion
    }

    public static class IsZeroOrPositiveInt
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("zero", 0, true),
            new("positive", 1, true)
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("null", null, false),
            new("negative", -1, false)
        ];

        #region Case Records

        public sealed record Case(string Name, int? Value, bool ExpectedReturn)
            : IsCase<int?>(Name, Value, ExpectedReturn);

        #endregion
    }

    public static class IsZeroOrNegativeInt
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("zero", 0, true),
            new("negative", -1, true)
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("null", null, false),
            new("positive", 1, false)
        ];

        #region Case Records

        public sealed record Case(string Name, int? Value, bool ExpectedReturn)
            : IsCase<int?>(Name, Value, ExpectedReturn);

        #endregion
    }

    public static class IsGreaterThan
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("greater", 2, 1, true)
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("equal", 1, 1, false),
            new("null", null, 1, false)
        ];

        #region Case Records

        public sealed record Case(string Name, int? Value, int Min, bool ExpectedReturn)
            : IsCase<int?>(Name, Value, ExpectedReturn);

        #endregion
    }

    public static class IsGreaterThanOrEqual
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("greater", 2, 1, true),
            new("equal", 1, 1, true)
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("less", 0, 1, false),
            new("null", null, 1, false)
        ];

        #region Case Records

        public sealed record Case(string Name, int? Value, int Min, bool ExpectedReturn)
            : IsCase<int?>(Name, Value, ExpectedReturn);

        #endregion
    }

    public static class IsLessThan
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("less", 0, 1, true)
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("equal", 1, 1, false),
            new("null", null, 1, false)
        ];

        #region Case Records

        public sealed record Case(string Name, int? Value, int Max, bool ExpectedReturn)
            : IsCase<int?>(Name, Value, ExpectedReturn);

        #endregion
    }

    public static class IsLessThanOrEqual
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("less", 0, 1, true),
            new("equal", 1, 1, true)
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("greater", 2, 1, false),
            new("null", null, 1, false)
        ];

        #region Case Records

        public sealed record Case(string Name, int? Value, int Max, bool ExpectedReturn)
            : IsCase<int?>(Name, Value, ExpectedReturn);

        #endregion
    }

    public static class IsInRange
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("between inclusive", 5, 1, 10, Inclusion.Inclusive, true),
            new("min inclusive", 1, 1, 10, Inclusion.Inclusive, true)
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("min exclusive", 1, 1, 10, Inclusion.Exclusive, false),
            new("null", null, 1, 10, Inclusion.Inclusive, false)
        ];

        #region Case Records

        public sealed record Case(string Name, int? Value, int Min, int Max, Inclusion Inclusion, bool ExpectedReturn)
            : IsCase<int?>(Name, Value, ExpectedReturn);

        #endregion
    }

    public static class IsApproximately
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("within tolerance", 10.0m, 10.1m, 0.2m, true)
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("outside tolerance", 10.0m, 10.3m, 0.2m, false),
            new("null value", null, 10.0m, 0.2m, false),
            new("null tolerance", 10.0m, 10.0m, null, false),
            new("negative tolerance", 10.0m, 10.0m, -0.1m, false)
        ];

        #region Case Records

        public sealed record Case(string Name, decimal? Value, decimal Target, decimal? Tolerance, bool ExpectedReturn)
            : IsCase<decimal?>(Name, Value, ExpectedReturn);

        #endregion
    }

    public static class IsMultipleOf
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("multiple", 4, 2, true)
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("not multiple", 5, 2, false),
            new("null", null, 2, false),
            new("zero factor", 4, 0, false)
        ];

        #region Case Records

        public sealed record Case(string Name, int? Value, int Factor, bool ExpectedReturn)
            : IsCase<int?>(Name, Value, ExpectedReturn);

        #endregion
    }

    public static class IsEvenInt
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("even", 4, true)
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("odd", 5, false),
            new("null", null, false)
        ];

        #region Case Records

        public sealed record Case(string Name, int? Value, bool ExpectedReturn)
            : IsCase<int?>(Name, Value, ExpectedReturn);

        #endregion
    }

    public static class IsEvenLong
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("even", 4L, true)
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("odd", 5L, false),
            new("null", null, false)
        ];

        #region Case Records

        public sealed record Case(string Name, long? Value, bool ExpectedReturn)
            : IsCase<long?>(Name, Value, ExpectedReturn);

        #endregion
    }

    public static class IsOddInt
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("odd", 5, true)
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("even", 4, false),
            new("null", null, false)
        ];

        #region Case Records

        public sealed record Case(string Name, int? Value, bool ExpectedReturn)
            : IsCase<int?>(Name, Value, ExpectedReturn);

        #endregion
    }

    public static class IsOddLong
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("odd", 5L, true)
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("even", 4L, false),
            new("null", null, false)
        ];

        #region Case Records

        public sealed record Case(string Name, long? Value, bool ExpectedReturn)
            : IsCase<long?>(Name, Value, ExpectedReturn);

        #endregion
    }

    public static class IsFiniteFloat
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("finite", 1.0f, true)
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("infinity", float.PositiveInfinity, false),
            new("null", null, false)
        ];

        #region Case Records

        public sealed record Case(string Name, float? Value, bool ExpectedReturn)
            : IsCase<float?>(Name, Value, ExpectedReturn);

        #endregion
    }

    public static class IsFiniteDouble
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("finite", 1.0, true)
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("infinity", double.PositiveInfinity, false),
            new("null", null, false)
        ];

        #region Case Records

        public sealed record Case(string Name, double? Value, bool ExpectedReturn)
            : IsCase<double?>(Name, Value, ExpectedReturn);

        #endregion
    }

    public static class IsNaNFloat
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("NaN", float.NaN, true)
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("finite", 1.0f, false),
            new("null", null, false)
        ];

        #region Case Records

        public sealed record Case(string Name, float? Value, bool ExpectedReturn)
            : IsCase<float?>(Name, Value, ExpectedReturn);

        #endregion
    }

    public static class IsNaNDouble
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("NaN", double.NaN, true)
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("finite", 1.0, false),
            new("null", null, false)
        ];

        #region Case Records

        public sealed record Case(string Name, double? Value, bool ExpectedReturn)
            : IsCase<double?>(Name, Value, ExpectedReturn);

        #endregion
    }
}
