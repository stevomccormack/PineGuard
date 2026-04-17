using PineGuard.Common;
using PineGuard.Testing.UnitTests.GuardClauses;
using PineGuard.Testing.UnitTests.Rules;
using F = PineGuard.Testing.Fixtures.NumberRulesFixtures;

namespace PineGuard.GuardClauses.UnitTests;

public static class GuardNumberClausesTestData
{
    // Guard.Against.ZeroOrNegative — throws when value IS zero or negative (delegates to Must.Be.Positive)
    public static class ZeroOrNegative
    {
        public static TheoryData<GuardCase<int>> ValidCases =>
            F.IsPositive.ValidScenarios.Select(s => new RuleScenario<int>(s.Name, s.Inputs!.Value, s.IsValid)).ToArray()
            .ToGuardCases(_ => new GuardExpected(true));

        public static TheoryData<GuardCase<int>> InvalidCases =>
            F.IsPositive.InvalidScenarios.Except(nameof(F.IsPositive.Null))
            .Select(s => new RuleScenario<int>(s.Name, s.Inputs!.Value, s.IsValid)).ToArray()
            .ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    // Guard.Against.ZeroOrPositive — throws when value IS zero or positive (delegates to Must.Be.Negative)
    public static class ZeroOrPositive
    {
        public static TheoryData<GuardCase<int>> ValidCases =>
            F.IsNegative.ValidScenarios.Select(s => new RuleScenario<int>(s.Name, s.Inputs!.Value, s.IsValid)).ToArray()
            .ToGuardCases(_ => new GuardExpected(true));

        public static TheoryData<GuardCase<int>> InvalidCases =>
            F.IsNegative.InvalidScenarios.Except(nameof(F.IsNegative.Null))
            .Select(s => new RuleScenario<int>(s.Name, s.Inputs!.Value, s.IsValid)).ToArray()
            .ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    // Guard.Against.NotZero — throws when value IS NOT zero (delegates to Must.Be.Zero)
    public static class NotZero
    {
        public static TheoryData<GuardCase<int>> ValidCases =>
            F.IsZero.ValidScenarios.Select(s => new RuleScenario<int>(s.Name, s.Inputs!.Value, s.IsValid)).ToArray()
            .ToGuardCases(_ => new GuardExpected(true));

        public static TheoryData<GuardCase<int>> InvalidCases =>
            F.IsZero.InvalidScenarios.Except(nameof(F.IsZero.Null))
            .Select(s => new RuleScenario<int>(s.Name, s.Inputs!.Value, s.IsValid)).ToArray()
            .ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    // Guard.Against.Zero — throws when value IS zero (delegates to Must.Be.NotZero)
    public static class Zero
    {
        public static TheoryData<GuardCase<int>> ValidCases =>
            F.IsNotZero.ValidScenarios.Select(s => new RuleScenario<int>(s.Name, s.Inputs!.Value, s.IsValid)).ToArray()
            .ToGuardCases(_ => new GuardExpected(true));

        public static TheoryData<GuardCase<int>> InvalidCases =>
            F.IsNotZero.InvalidScenarios.Except(nameof(F.IsNotZero.Null))
            .Select(s => new RuleScenario<int>(s.Name, s.Inputs!.Value, s.IsValid)).ToArray()
            .ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    // Guard.Against.Negative — throws when value IS negative (delegates to Must.Be.ZeroOrPositive)
    public static class Negative
    {
        public static TheoryData<GuardCase<int>> ValidCases =>
            F.IsZeroOrPositive.ValidScenarios.Select(s => new RuleScenario<int>(s.Name, s.Inputs!.Value, s.IsValid)).ToArray()
            .ToGuardCases(_ => new GuardExpected(true));

        public static TheoryData<GuardCase<int>> InvalidCases =>
            F.IsZeroOrPositive.InvalidScenarios.Except(nameof(F.IsZeroOrPositive.Null))
            .Select(s => new RuleScenario<int>(s.Name, s.Inputs!.Value, s.IsValid)).ToArray()
            .ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    // Guard.Against.Positive — throws when value IS positive (delegates to Must.Be.ZeroOrNegative)
    public static class Positive
    {
        public static TheoryData<GuardCase<int>> ValidCases =>
            F.IsZeroOrNegative.ValidScenarios.Select(s => new RuleScenario<int>(s.Name, s.Inputs!.Value, s.IsValid)).ToArray()
            .ToGuardCases(_ => new GuardExpected(true));

        public static TheoryData<GuardCase<int>> InvalidCases =>
            F.IsZeroOrNegative.InvalidScenarios.Except(nameof(F.IsZeroOrNegative.Null))
            .Select(s => new RuleScenario<int>(s.Name, s.Inputs!.Value, s.IsValid)).ToArray()
            .ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    // Guard.Against.LessThanOrEqual — throws when value IS ≤ min (delegates to Must.Be.GreaterThan)
    public static class LessThanOrEqual
    {
        public static TheoryData<GuardCase<(int value, int min)>> ValidCases =>
            F.IsGreaterThan.ValidScenarios
            .Select(s => new RuleScenario<(int value, int min)>(s.Name, (s.Inputs.value!.Value, s.Inputs.min), s.IsValid)).ToArray()
            .ToGuardCases(_ => new GuardExpected(true));

        public static TheoryData<GuardCase<(int value, int min)>> InvalidCases =>
            F.IsGreaterThan.InvalidScenarios.Except(nameof(F.IsGreaterThan.Null))
            .Select(s => new RuleScenario<(int value, int min)>(s.Name, (s.Inputs.value!.Value, s.Inputs.min), s.IsValid)).ToArray()
            .ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    // Guard.Against.LessThan — throws when value IS < min (delegates to Must.Be.GreaterThanOrEqual)
    public static class LessThan
    {
        public static TheoryData<GuardCase<(int value, int min)>> ValidCases =>
            F.IsGreaterThanOrEqual.ValidScenarios
            .Select(s => new RuleScenario<(int value, int min)>(s.Name, (s.Inputs.value!.Value, s.Inputs.min), s.IsValid)).ToArray()
            .ToGuardCases(_ => new GuardExpected(true));

        public static TheoryData<GuardCase<(int value, int min)>> InvalidCases =>
            F.IsGreaterThanOrEqual.InvalidScenarios.Except(nameof(F.IsGreaterThanOrEqual.Null))
            .Select(s => new RuleScenario<(int value, int min)>(s.Name, (s.Inputs.value!.Value, s.Inputs.min), s.IsValid)).ToArray()
            .ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    // Guard.Against.GreaterThanOrEqual — throws when value IS ≥ max (delegates to Must.Be.LessThan)
    public static class GreaterThanOrEqual
    {
        public static TheoryData<GuardCase<(int value, int max)>> ValidCases =>
            F.IsLessThan.ValidScenarios
            .Select(s => new RuleScenario<(int value, int max)>(s.Name, (s.Inputs.value!.Value, s.Inputs.max), s.IsValid)).ToArray()
            .ToGuardCases(_ => new GuardExpected(true));

        public static TheoryData<GuardCase<(int value, int max)>> InvalidCases =>
            F.IsLessThan.InvalidScenarios.Except(nameof(F.IsLessThan.Null))
            .Select(s => new RuleScenario<(int value, int max)>(s.Name, (s.Inputs.value!.Value, s.Inputs.max), s.IsValid)).ToArray()
            .ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    // Guard.Against.GreaterThan — throws when value IS > min (delegates to Must.Be.LessThanOrEqual)
    public static class GreaterThan
    {
        public static TheoryData<GuardCase<(int value, int min)>> ValidCases =>
            F.IsLessThanOrEqual.ValidScenarios
            .Select(s => new RuleScenario<(int value, int min)>(s.Name, (s.Inputs.value!.Value, s.Inputs.max), s.IsValid)).ToArray()
            .ToGuardCases(_ => new GuardExpected(true));

        public static TheoryData<GuardCase<(int value, int min)>> InvalidCases =>
            F.IsLessThanOrEqual.InvalidScenarios.Except(nameof(F.IsLessThanOrEqual.Null))
            .Select(s => new RuleScenario<(int value, int min)>(s.Name, (s.Inputs.value!.Value, s.Inputs.max), s.IsValid)).ToArray()
            .ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    // Guard.Against.OutOfRange — throws when value IS out of range (delegates to Must.Be.InRange)
    public static class OutOfRange
    {
        public static TheoryData<GuardCase<(int value, int min, int max, Inclusion inclusion)>> ValidCases =>
            F.IsInRange.ValidScenarios
            .Select(s => new RuleScenario<(int value, int min, int max, Inclusion inclusion)>(s.Name, (s.Inputs.value!.Value, s.Inputs.min, s.Inputs.max, s.Inputs.inclusion), s.IsValid)).ToArray()
            .ToGuardCases(_ => new GuardExpected(true));

        public static TheoryData<GuardCase<(int value, int min, int max, Inclusion inclusion)>> InvalidCases =>
            F.IsInRange.InvalidScenarios.Except(nameof(F.IsInRange.NullValue))
            .Select(s => new RuleScenario<(int value, int min, int max, Inclusion inclusion)>(s.Name, (s.Inputs.value!.Value, s.Inputs.min, s.Inputs.max, s.Inputs.inclusion), s.IsValid)).ToArray()
            .ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    // Guard.Against.InRange — throws when value IS in range (delegates to Must.Be.OutOfRange)
    public static class InRange
    {
        public static TheoryData<GuardCase<(int value, int min, int max, Inclusion inclusion)>> ValidCases =>
            F.IsInRange.InvalidScenarios.Except(nameof(F.IsInRange.NullValue))
            .Select(s => new RuleScenario<(int value, int min, int max, Inclusion inclusion)>(s.Name, (s.Inputs.value!.Value, s.Inputs.min, s.Inputs.max, s.Inputs.inclusion), s.IsValid)).ToArray()
            .ToGuardCases(_ => new GuardExpected(true));

        public static TheoryData<GuardCase<(int value, int min, int max, Inclusion inclusion)>> InvalidCases =>
            F.IsInRange.ValidScenarios
            .Select(s => new RuleScenario<(int value, int min, int max, Inclusion inclusion)>(s.Name, (s.Inputs.value!.Value, s.Inputs.min, s.Inputs.max, s.Inputs.inclusion), s.IsValid)).ToArray()
            .ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    // Guard.Against.NotApproximately — throws when value IS approximately target (delegates to Must.Be.Approximately)
    // Uses double directly as the fixture uses decimal? which doesn't match the Guard generic constraint
    public static class NotApproximately
    {
        public static TheoryData<GuardCase<(double value, double target, double? tolerance)>> ValidCases =>
        [
            new("Exact",          (10.0,  10.0, 1.0), new GuardExpected(true)),
            new("WithinTolerance",(10.5,  10.0, 1.0), new GuardExpected(true))
        ];

        public static TheoryData<GuardCase<(double value, double target, double? tolerance)>> InvalidCases =>
        [
            new("OutsideTolerance", (12.0, 10.0, 1.0), new GuardExpected(false, typeof(ArgumentException), "value"))
        ];
    }

    // Guard.Against.Approximately — throws when value IS NOT approximately target (delegates to Must.Be.NotApproximately)
    public static class Approximately
    {
        public static TheoryData<GuardCase<(double value, double target, double? tolerance)>> ValidCases =>
        [
            new("OutsideTolerance", (12.0, 10.0, 1.0), new GuardExpected(true))
        ];

        public static TheoryData<GuardCase<(double value, double target, double? tolerance)>> InvalidCases =>
        [
            new("Exact",           (10.0, 10.0, 1.0), new GuardExpected(false, typeof(ArgumentException), "value")),
            new("WithinTolerance", (10.5, 10.0, 1.0), new GuardExpected(false, typeof(ArgumentException), "value"))
        ];
    }

    // Guard.Against.NotMultipleOf — throws when value IS a multiple of factor (delegates to Must.Be.MultipleOf)
    public static class NotMultipleOf
    {
        public static TheoryData<GuardCase<(int value, int factor)>> ValidCases =>
            F.IsMultipleOf.ValidScenarios
            .Select(s => new RuleScenario<(int value, int factor)>(s.Name, (s.Inputs.value!.Value, s.Inputs.factor), s.IsValid)).ToArray()
            .ToGuardCases(_ => new GuardExpected(true));

        public static TheoryData<GuardCase<(int value, int factor)>> InvalidCases =>
            F.IsMultipleOf.InvalidScenarios.Except(nameof(F.IsMultipleOf.Null))
            .Select(s => new RuleScenario<(int value, int factor)>(s.Name, (s.Inputs.value!.Value, s.Inputs.factor), s.IsValid)).ToArray()
            .ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    // Guard.Against.MultipleOf — throws when value IS NOT a multiple of factor (delegates to Must.Be.NotMultipleOf)
    public static class MultipleOf
    {
        public static TheoryData<GuardCase<(int value, int factor)>> ValidCases =>
            F.IsMultipleOf.InvalidScenarios.Except(nameof(F.IsMultipleOf.Null))
            .Select(s => new RuleScenario<(int value, int factor)>(s.Name, (s.Inputs.value!.Value, s.Inputs.factor), s.IsValid)).ToArray()
            .ToGuardCases(_ => new GuardExpected(true));

        public static TheoryData<GuardCase<(int value, int factor)>> InvalidCases =>
            F.IsMultipleOf.ValidScenarios
            .Select(s => new RuleScenario<(int value, int factor)>(s.Name, (s.Inputs.value!.Value, s.Inputs.factor), s.IsValid)).ToArray()
            .ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    // Guard.Against.Odd (int) — throws when value IS odd (delegates to Must.Be.Even)
    public static class OddInt
    {
        public static TheoryData<GuardCase<int>> ValidCases =>
            F.IsOdd.IntInvalidScenarios.Except("NullInt")
            .Select(s => new RuleScenario<int>(s.Name, s.Inputs!.Value, s.IsValid)).ToArray()
            .ToGuardCases(_ => new GuardExpected(true));

        public static TheoryData<GuardCase<int>> InvalidCases =>
            F.IsOdd.IntValidScenarios
            .Select(s => new RuleScenario<int>(s.Name, s.Inputs!.Value, s.IsValid)).ToArray()
            .ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    // Guard.Against.Odd (long) — throws when value IS odd (delegates to Must.Be.Even)
    public static class OddLong
    {
        public static TheoryData<GuardCase<long>> ValidCases =>
            F.IsOdd.LongInvalidScenarios.Except("NullLong")
            .Select(s => new RuleScenario<long>(s.Name, s.Inputs!.Value, s.IsValid)).ToArray()
            .ToGuardCases(_ => new GuardExpected(true));

        public static TheoryData<GuardCase<long>> InvalidCases =>
            F.IsOdd.LongValidScenarios
            .Select(s => new RuleScenario<long>(s.Name, s.Inputs!.Value, s.IsValid)).ToArray()
            .ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    // Guard.Against.Even (int) — throws when value IS even (delegates to Must.Be.Odd)
    public static class EvenInt
    {
        public static TheoryData<GuardCase<int>> ValidCases =>
            F.IsEven.IntInvalidScenarios.Except("NullInt")
            .Select(s => new RuleScenario<int>(s.Name, s.Inputs!.Value, s.IsValid)).ToArray()
            .ToGuardCases(_ => new GuardExpected(true));

        public static TheoryData<GuardCase<int>> InvalidCases =>
            F.IsEven.IntValidScenarios
            .Select(s => new RuleScenario<int>(s.Name, s.Inputs!.Value, s.IsValid)).ToArray()
            .ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    // Guard.Against.Even (long) — throws when value IS even (delegates to Must.Be.Odd)
    public static class EvenLong
    {
        public static TheoryData<GuardCase<long>> ValidCases =>
            F.IsEven.LongInvalidScenarios.Except("NullLong")
            .Select(s => new RuleScenario<long>(s.Name, s.Inputs!.Value, s.IsValid)).ToArray()
            .ToGuardCases(_ => new GuardExpected(true));

        public static TheoryData<GuardCase<long>> InvalidCases =>
            F.IsEven.LongValidScenarios
            .Select(s => new RuleScenario<long>(s.Name, s.Inputs!.Value, s.IsValid)).ToArray()
            .ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    // Guard.Against.NotFinite (float) — throws when value IS not finite (delegates to Must.Be.Finite)
    public static class NotFiniteFloat
    {
        public static TheoryData<GuardCase<float>> ValidCases =>
            F.IsFinite.FloatValidScenarios
            .Select(s => new RuleScenario<float>(s.Name, s.Inputs!.Value, s.IsValid)).ToArray()
            .ToGuardCases(_ => new GuardExpected(true));

        public static TheoryData<GuardCase<float>> InvalidCases =>
            F.IsFinite.FloatInvalidScenarios.Except("NullFloat")
            .Select(s => new RuleScenario<float>(s.Name, s.Inputs!.Value, s.IsValid)).ToArray()
            .ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    // Guard.Against.NotFinite (double) — throws when value IS not finite (delegates to Must.Be.Finite)
    public static class NotFiniteDouble
    {
        public static TheoryData<GuardCase<double>> ValidCases =>
            F.IsFinite.DoubleValidScenarios
            .Select(s => new RuleScenario<double>(s.Name, s.Inputs!.Value, s.IsValid)).ToArray()
            .ToGuardCases(_ => new GuardExpected(true));

        public static TheoryData<GuardCase<double>> InvalidCases =>
            F.IsFinite.DoubleInvalidScenarios.Except("NullDouble")
            .Select(s => new RuleScenario<double>(s.Name, s.Inputs!.Value, s.IsValid)).ToArray()
            .ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    // Guard.Against.Finite (float) — throws when value IS finite (delegates to Must.Be.NotFinite)
    public static class FiniteFloat
    {
        public static TheoryData<GuardCase<float>> ValidCases =>
            F.IsFinite.FloatInvalidScenarios.Except("NullFloat")
            .Select(s => new RuleScenario<float>(s.Name, s.Inputs!.Value, s.IsValid)).ToArray()
            .ToGuardCases(_ => new GuardExpected(true));

        public static TheoryData<GuardCase<float>> InvalidCases =>
            F.IsFinite.FloatValidScenarios
            .Select(s => new RuleScenario<float>(s.Name, s.Inputs!.Value, s.IsValid)).ToArray()
            .ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    // Guard.Against.Finite (double) — throws when value IS finite (delegates to Must.Be.NotFinite)
    public static class FiniteDouble
    {
        public static TheoryData<GuardCase<double>> ValidCases =>
            F.IsFinite.DoubleInvalidScenarios.Except("NullDouble")
            .Select(s => new RuleScenario<double>(s.Name, s.Inputs!.Value, s.IsValid)).ToArray()
            .ToGuardCases(_ => new GuardExpected(true));

        public static TheoryData<GuardCase<double>> InvalidCases =>
            F.IsFinite.DoubleValidScenarios
            .Select(s => new RuleScenario<double>(s.Name, s.Inputs!.Value, s.IsValid)).ToArray()
            .ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    // Guard.Against.NaN (float) — throws when value IS NaN (delegates to Must.Be.NotNaN)
    public static class NaNFloat
    {
        public static TheoryData<GuardCase<float>> ValidCases =>
            F.IsNaN.FloatInvalidScenarios.Except("NullFloat")
            .Select(s => new RuleScenario<float>(s.Name, s.Inputs!.Value, s.IsValid)).ToArray()
            .ToGuardCases(_ => new GuardExpected(true));

        public static TheoryData<GuardCase<float>> InvalidCases =>
            F.IsNaN.FloatValidScenarios
            .Select(s => new RuleScenario<float>(s.Name, s.Inputs!.Value, s.IsValid)).ToArray()
            .ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    // Guard.Against.NaN (double) — throws when value IS NaN (delegates to Must.Be.NotNaN)
    public static class NaNDouble
    {
        public static TheoryData<GuardCase<double>> ValidCases =>
            F.IsNaN.DoubleInvalidScenarios.Except("NullDouble")
            .Select(s => new RuleScenario<double>(s.Name, s.Inputs!.Value, s.IsValid)).ToArray()
            .ToGuardCases(_ => new GuardExpected(true));

        public static TheoryData<GuardCase<double>> InvalidCases =>
            F.IsNaN.DoubleValidScenarios
            .Select(s => new RuleScenario<double>(s.Name, s.Inputs!.Value, s.IsValid)).ToArray()
            .ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    // Guard.Against.NotNaN (float) — throws when value IS not NaN (delegates to Must.Be.NaN)
    public static class NotNaNFloat
    {
        public static TheoryData<GuardCase<float>> ValidCases =>
            F.IsNaN.FloatValidScenarios
            .Select(s => new RuleScenario<float>(s.Name, s.Inputs!.Value, s.IsValid)).ToArray()
            .ToGuardCases(_ => new GuardExpected(true));

        public static TheoryData<GuardCase<float>> InvalidCases =>
            F.IsNaN.FloatInvalidScenarios.Except("NullFloat")
            .Select(s => new RuleScenario<float>(s.Name, s.Inputs!.Value, s.IsValid)).ToArray()
            .ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    // Guard.Against.NotNaN (double) — throws when value IS not NaN (delegates to Must.Be.NaN)
    public static class NotNaNDouble
    {
        public static TheoryData<GuardCase<double>> ValidCases =>
            F.IsNaN.DoubleValidScenarios
            .Select(s => new RuleScenario<double>(s.Name, s.Inputs!.Value, s.IsValid)).ToArray()
            .ToGuardCases(_ => new GuardExpected(true));

        public static TheoryData<GuardCase<double>> InvalidCases =>
            F.IsNaN.DoubleInvalidScenarios.Except("NullDouble")
            .Select(s => new RuleScenario<double>(s.Name, s.Inputs!.Value, s.IsValid)).ToArray()
            .ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value"));
    }
}
