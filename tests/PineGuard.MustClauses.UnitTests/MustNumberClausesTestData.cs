using PineGuard.Codes;
using PineGuard.Common;
using PineGuard.Testing.UnitTests.MustClauses;
using PineGuard.Testing.UnitTests.Rules;
using F = PineGuard.Testing.Fixtures.NumberRulesFixtures;

namespace PineGuard.MustClauses.UnitTests;

public static class MustNumberClausesTestData
{
    public static class Positive
    {
        public static TheoryData<MustCase<int>> ValidCases => F.IsPositive.ValidScenarios.Except(nameof(F.IsPositive.Null)).Project(v => v!.Value).ToMustCases();
        public static TheoryData<MustCase<int>> InvalidCases => F.IsPositive.InvalidScenarios.Except(nameof(F.IsPositive.Null)).Project(v => v!.Value).ToMustCases(_ => new MustExpected(false, "value must be positive.", Code: MustCodes.Number.Sign.NotPositive));
    }

    public static class Negative
    {
        public static TheoryData<MustCase<int>> ValidCases => F.IsNegative.ValidScenarios.Except(nameof(F.IsNegative.Null)).Project(v => v!.Value).ToMustCases();
        public static TheoryData<MustCase<int>> InvalidCases => F.IsNegative.InvalidScenarios.Except(nameof(F.IsNegative.Null)).Project(v => v!.Value).ToMustCases(_ => new MustExpected(false, "value must be negative.", Code: MustCodes.Number.Sign.NotNegative));
    }

    public static class Zero
    {
        public static TheoryData<MustCase<int>> ValidCases => F.IsZero.ValidScenarios.Except(nameof(F.IsZero.Null)).Project(v => v!.Value).ToMustCases();
        public static TheoryData<MustCase<int>> InvalidCases => F.IsZero.InvalidScenarios.Except(nameof(F.IsZero.Null)).Project(v => v!.Value).ToMustCases(_ => new MustExpected(false, "value must be zero.", Code: MustCodes.Number.Sign.NotZero));
    }

    public static class NotZero
    {
        public static TheoryData<MustCase<int>> ValidCases => F.IsNotZero.ValidScenarios.Except(nameof(F.IsNotZero.Null)).Project(v => v!.Value).ToMustCases();
        public static TheoryData<MustCase<int>> InvalidCases => F.IsNotZero.InvalidScenarios.Except(nameof(F.IsNotZero.Null)).Project(v => v!.Value).ToMustCases(_ => new MustExpected(false, "value must not be zero.", Code: MustCodes.Number.Sign.Zero));
    }

    public static class ZeroOrPositive
    {
        public static TheoryData<MustCase<int>> ValidCases => F.IsZeroOrPositive.ValidScenarios.Except(nameof(F.IsZeroOrPositive.Null)).Project(v => v!.Value).ToMustCases();
        public static TheoryData<MustCase<int>> InvalidCases => F.IsZeroOrPositive.InvalidScenarios.Except(nameof(F.IsZeroOrPositive.Null)).Project(v => v!.Value).ToMustCases(_ => new MustExpected(false, "value must be zero or positive.", Code: MustCodes.Number.Sign.Negative));
    }

    public static class ZeroOrNegative
    {
        public static TheoryData<MustCase<int>> ValidCases => F.IsZeroOrNegative.ValidScenarios.Except(nameof(F.IsZeroOrNegative.Null)).Project(v => v!.Value).ToMustCases();
        public static TheoryData<MustCase<int>> InvalidCases => F.IsZeroOrNegative.InvalidScenarios.Except(nameof(F.IsZeroOrNegative.Null)).Project(v => v!.Value).ToMustCases(_ => new MustExpected(false, "value must be zero or negative.", Code: MustCodes.Number.Sign.Positive));
    }

    public static class GreaterThan
    {
        public static TheoryData<MustCase<(int value, int min)>> ValidCases => F.IsGreaterThan.ValidScenarios.Except(nameof(F.IsGreaterThan.Null)).Project(v => (v.value!.Value, v.min)).ToMustCases();
        public static TheoryData<MustCase<(int value, int min)>> InvalidCases => F.IsGreaterThan.InvalidScenarios.Except(nameof(F.IsGreaterThan.Null)).Project(v => (v.value!.Value, v.min)).ToMustCases(_ => new MustExpected(false, "value must be greater than the minimum.", Code: MustCodes.Number.Range.NotGreater));
    }

    public static class GreaterThanOrEqual
    {
        public static TheoryData<MustCase<(int value, int min)>> ValidCases => F.IsGreaterThanOrEqual.ValidScenarios.Except(nameof(F.IsGreaterThanOrEqual.Null)).Project(v => (v.value!.Value, v.min)).ToMustCases();
        public static TheoryData<MustCase<(int value, int min)>> InvalidCases => F.IsGreaterThanOrEqual.InvalidScenarios.Except(nameof(F.IsGreaterThanOrEqual.Null)).Project(v => (v.value!.Value, v.min)).ToMustCases(_ => new MustExpected(false, "value must be greater than or equal to the minimum.", Code: MustCodes.Number.Range.BelowMinimum));
    }

    public static class LessThan
    {
        public static TheoryData<MustCase<(int value, int max)>> ValidCases => F.IsLessThan.ValidScenarios.Except(nameof(F.IsLessThan.Null)).Project(v => (v.value!.Value, v.max)).ToMustCases();
        public static TheoryData<MustCase<(int value, int max)>> InvalidCases => F.IsLessThan.InvalidScenarios.Except(nameof(F.IsLessThan.Null)).Project(v => (v.value!.Value, v.max)).ToMustCases(_ => new MustExpected(false, "value must be less than the maximum.", Code: MustCodes.Number.Range.NotLess));
    }

    public static class LessThanOrEqual
    {
        public static TheoryData<MustCase<(int value, int max)>> ValidCases => F.IsLessThanOrEqual.ValidScenarios.Except(nameof(F.IsLessThanOrEqual.Null)).Project(v => (v.value!.Value, v.max)).ToMustCases();
        public static TheoryData<MustCase<(int value, int max)>> InvalidCases => F.IsLessThanOrEqual.InvalidScenarios.Except(nameof(F.IsLessThanOrEqual.Null)).Project(v => (v.value!.Value, v.max)).ToMustCases(_ => new MustExpected(false, "value must be less than or equal to the maximum.", Code: MustCodes.Number.Range.Exceeded));
    }

    public static class InRange
    {
        public static TheoryData<MustCase<(int value, int min, int max, Inclusion inclusion)>> ValidCases => F.IsInRange.ValidScenarios.Except(nameof(F.IsInRange.NullValue)).Project(v => (v.value!.Value, v.min, v.max, v.inclusion)).ToMustCases();
        public static TheoryData<MustCase<(int value, int min, int max, Inclusion inclusion)>> InvalidCases
        {
            get
            {
                var data = F.IsInRange.InvalidScenarios.Except(nameof(F.IsInRange.NullValue)).Project(v => (v.value!.Value, v.min, v.max, v.inclusion)).ToMustCases(_ => new MustExpected(false, "value must be within the expected range.", Code: MustCodes.Number.Range.OutOfRange));
                data.Add(new MustCase<(int Value, int min, int max, Inclusion inclusion)>(nameof(F.IsInRange.AtMinExclusive), (5, 10, 0, Inclusion.Inclusive), new MustExpected(false, "min requires a valid range.", "min", Code: MustCodes.Number.Range.Invalid)));
                return data;
            }
        }
    }

    public static class OutOfRange
    {
        public static TheoryData<MustCase<(int value, int min, int max, Inclusion inclusion)>> ValidCases => F.IsInRange.InvalidScenarios.Except(nameof(F.IsInRange.NullValue)).Project(v => (v.value!.Value, v.min, v.max, v.inclusion)).ToMustCases(_ => new MustExpected(true));
        public static TheoryData<MustCase<(int value, int min, int max, Inclusion inclusion)>> InvalidCases
        {
            get
            {
                var data = F.IsInRange.ValidScenarios.Except(nameof(F.IsInRange.NullValue)).Project(v => (v.value!.Value, v.min, v.max, v.inclusion)).ToMustCases(_ => new MustExpected(false, "value must be out of the expected range.", Code: MustCodes.Number.Range.InRange));
                data.Add(new MustCase<(int Value, int min, int max, Inclusion inclusion)>(nameof(F.IsInRange.AtMinExclusive), (5, 10, 0, Inclusion.Inclusive), new MustExpected(false, "min requires a valid range.", "min", Code: MustCodes.Number.Range.Invalid)));
                return data;
            }
        }
    }

    public static class Percentage
    {
        public static TheoryData<MustCase<decimal>> ValidCases => F.IsPercentage.AllValid.Project(v => v!.Value).ToMustCases();
        public static TheoryData<MustCase<decimal>> InvalidCases => F.IsPercentage.AllInvalid.Except(nameof(F.IsPercentage.NullValue)).Project(v => v!.Value).ToMustCases(_ => new MustExpected(false, "value must be a percentage between 0 and 100.", Code: MustCodes.Number.Range.NotPercentage));
    }

    public static class Approximately
    {
        public static TheoryData<MustCase<(decimal value, decimal target, decimal? tolerance)>> ValidCases => F.IsApproximately.ValidScenarios.Except(nameof(F.IsApproximately.NullValue)).Project(v => (v.value!.Value, v.target, v.tolerance)).ToMustCases();
        public static TheoryData<MustCase<(decimal value, decimal target, decimal? tolerance)>> InvalidCases => F.IsApproximately.InvalidScenarios.Except(nameof(F.IsApproximately.NullValue)).Project(v => (v.value!.Value, v.target, v.tolerance)).ToMustCases(s => s.Name switch
        {
            nameof(F.IsApproximately.NullTolerance) => new MustExpected(false, "tolerance requires a non-null tolerance.", "tolerance", Code: MustCodes.Number.Tolerance.Null),
            _ => new MustExpected(false, "value must be approximately the target value.", Code: MustCodes.Number.Proximity.NotApproximate)
        });
    }

    public static class NotApproximately
    {
        public static TheoryData<MustCase<(decimal value, decimal target, decimal? tolerance)>> ValidCases => F.IsApproximately.InvalidScenarios.Except(nameof(F.IsApproximately.NullValue), nameof(F.IsApproximately.NullTolerance)).Project(v => (v.value!.Value, v.target, v.tolerance)).ToMustCases(_ => new MustExpected(true));
        public static TheoryData<MustCase<(decimal value, decimal target, decimal? tolerance)>> InvalidCases
        {
            get
            {
                var data = F.IsApproximately.ValidScenarios.Except(nameof(F.IsApproximately.NullValue)).Project(v => (v.value!.Value, v.target, v.tolerance)).ToMustCases(_ => new MustExpected(false, "value must not be approximately the target value.", Code: MustCodes.Number.Proximity.Approximate));
                data.Add(new MustCase<(decimal Value, decimal target, decimal? tolerance)>(nameof(F.IsApproximately.NullTolerance), (10.0m, 10.0m, null), new MustExpected(false, "tolerance requires a non-null tolerance.", "tolerance", Code: MustCodes.Number.Tolerance.Null)));
                return data;
            }
        }
    }

    public static class MultipleOf
    {
        public static TheoryData<MustCase<(int value, int factor)>> ValidCases => F.IsMultipleOf.ValidScenarios.Except(nameof(F.IsMultipleOf.Null)).Project(v => (v.value!.Value, v.factor)).ToMustCases();
        public static TheoryData<MustCase<(int value, int factor)>> InvalidCases => F.IsMultipleOf.InvalidScenarios.Except(nameof(F.IsMultipleOf.Null)).Project(v => (v.value!.Value, v.factor)).ToMustCases(_ => new MustExpected(false, "value must be a multiple of the specified factor.", Code: MustCodes.Number.Divisibility.NotMultiple));
    }

    public static class NotMultipleOf
    {
        public static TheoryData<MustCase<(int value, int factor)>> ValidCases => F.IsMultipleOf.InvalidScenarios.Except(nameof(F.IsMultipleOf.Null)).Project(v => (v.value!.Value, v.factor)).ToMustCases(_ => new MustExpected(true));
        public static TheoryData<MustCase<(int value, int factor)>> InvalidCases => F.IsMultipleOf.ValidScenarios.Except(nameof(F.IsMultipleOf.Null)).Project(v => (v.value!.Value, v.factor)).ToMustCases(_ => new MustExpected(false, "value must not be a multiple of the specified factor.", Code: MustCodes.Number.Divisibility.Multiple));
    }

    public static class Even
    {
        public static TheoryData<MustCase<int>> ValidCasesInt => F.IsEven.IntValidScenarios.Project(v => v!.Value).ToMustCases();
        public static TheoryData<MustCase<int>> InvalidCasesInt => F.IsEven.IntInvalidScenarios.Except("NullInt").Project(v => v!.Value).ToMustCases(_ => new MustExpected(false, "value must be even.", Code: MustCodes.Number.Parity.Odd));
        public static TheoryData<MustCase<long>> ValidCasesLong => F.IsEven.LongValidScenarios.Project(v => v!.Value).ToMustCases();
        public static TheoryData<MustCase<long>> InvalidCasesLong => F.IsEven.LongInvalidScenarios.Except("NullLong").Project(v => v!.Value).ToMustCases(_ => new MustExpected(false, "value must be even.", Code: MustCodes.Number.Parity.Odd));
    }

    public static class Odd
    {
        public static TheoryData<MustCase<int>> ValidCasesInt => F.IsOdd.IntValidScenarios.Project(v => v!.Value).ToMustCases();
        public static TheoryData<MustCase<int>> InvalidCasesInt => F.IsOdd.IntInvalidScenarios.Except("NullInt").Project(v => v!.Value).ToMustCases(_ => new MustExpected(false, "value must be odd.", Code: MustCodes.Number.Parity.Even));
        public static TheoryData<MustCase<long>> ValidCasesLong => F.IsOdd.LongValidScenarios.Project(v => v!.Value).ToMustCases();
        public static TheoryData<MustCase<long>> InvalidCasesLong => F.IsOdd.LongInvalidScenarios.Except("NullLong").Project(v => v!.Value).ToMustCases(_ => new MustExpected(false, "value must be odd.", Code: MustCodes.Number.Parity.Even));
    }

    public static class Finite
    {
        public static TheoryData<MustCase<float>> ValidCasesFloat => F.IsFinite.FloatValidScenarios.Project(v => v!.Value).ToMustCases();
        public static TheoryData<MustCase<float>> InvalidCasesFloat => F.IsFinite.FloatInvalidScenarios.Except("NullFloat").Project(v => v!.Value).ToMustCases(_ => new MustExpected(false, "value must be finite.", Code: MustCodes.Number.Form.NotFinite));
        public static TheoryData<MustCase<double>> ValidCasesDouble => F.IsFinite.DoubleValidScenarios.Project(v => v!.Value).ToMustCases();
        public static TheoryData<MustCase<double>> InvalidCasesDouble => F.IsFinite.DoubleInvalidScenarios.Except("NullDouble").Project(v => v!.Value).ToMustCases(_ => new MustExpected(false, "value must be finite.", Code: MustCodes.Number.Form.NotFinite));
    }

    public static class NotFinite
    {
        public static TheoryData<MustCase<float>> ValidCasesFloat => F.IsFinite.FloatInvalidScenarios.Except("NullFloat").Project(v => v!.Value).ToMustCases(_ => new MustExpected(true));
        public static TheoryData<MustCase<float>> InvalidCasesFloat => F.IsFinite.FloatValidScenarios.Project(v => v!.Value).ToMustCases(_ => new MustExpected(false, "value must not be finite.", Code: MustCodes.Number.Form.Finite));
        public static TheoryData<MustCase<double>> ValidCasesDouble => F.IsFinite.DoubleInvalidScenarios.Except("NullDouble").Project(v => v!.Value).ToMustCases(_ => new MustExpected(true));
        public static TheoryData<MustCase<double>> InvalidCasesDouble => F.IsFinite.DoubleValidScenarios.Project(v => v!.Value).ToMustCases(_ => new MustExpected(false, "value must not be finite.", Code: MustCodes.Number.Form.Finite));
    }

    public static class NotNaN
    {
        public static TheoryData<MustCase<float>> ValidCasesFloat => F.IsNaN.FloatInvalidScenarios.Except("NullFloat").Project(v => v!.Value).ToMustCases(_ => new MustExpected(true));
        public static TheoryData<MustCase<float>> InvalidCasesFloat => F.IsNaN.FloatValidScenarios.Project(v => v!.Value).ToMustCases(_ => new MustExpected(false, "value must not be NaN.", Code: MustCodes.Number.Form.Nan));
        public static TheoryData<MustCase<double>> ValidCasesDouble => F.IsNaN.DoubleInvalidScenarios.Except("NullDouble").Project(v => v!.Value).ToMustCases(_ => new MustExpected(true));
        public static TheoryData<MustCase<double>> InvalidCasesDouble => F.IsNaN.DoubleValidScenarios.Project(v => v!.Value).ToMustCases(_ => new MustExpected(false, "value must not be NaN.", Code: MustCodes.Number.Form.Nan));
    }

    public static class NaN
    {
        public static TheoryData<MustCase<float>> ValidCasesFloat => F.IsNaN.FloatValidScenarios.Project(v => v!.Value).ToMustCases(_ => new MustExpected(true));
        public static TheoryData<MustCase<float>> InvalidCasesFloat => F.IsNaN.FloatInvalidScenarios.Except("NullFloat").Project(v => v!.Value).ToMustCases(_ => new MustExpected(false, "value must be NaN.", Code: MustCodes.Number.Form.NotNan));
        public static TheoryData<MustCase<double>> ValidCasesDouble => F.IsNaN.DoubleValidScenarios.Project(v => v!.Value).ToMustCases(_ => new MustExpected(true));
        public static TheoryData<MustCase<double>> InvalidCasesDouble => F.IsNaN.DoubleInvalidScenarios.Except("NullDouble").Project(v => v!.Value).ToMustCases(_ => new MustExpected(false, "value must be NaN.", Code: MustCodes.Number.Form.NotNan));
    }
}
