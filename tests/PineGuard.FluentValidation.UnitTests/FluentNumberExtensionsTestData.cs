using PineGuard.Codes;
using PineGuard.Common;
using PineGuard.Testing.UnitTests.FluentValidation;
using PineGuard.Testing.UnitTests.Rules;
using F = PineGuard.Testing.Fixtures.NumberRulesFixtures;

namespace PineGuard.FluentValidation.UnitTests;

public static class FluentNumberExtensionsTestData
{
    public static class Positive
    {
        public static TheoryData<FluentCase<int?>> Cases => F.IsPositive.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsPositive.Null) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be positive.")
        });
    }

    public static class Negative
    {
        public static TheoryData<FluentCase<int?>> Cases => F.IsNegative.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsNegative.Null) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be negative.")
        });
    }

    public static class Zero
    {
        public static TheoryData<FluentCase<int?>> Cases => F.IsZero.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsZero.Null) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be zero.")
        });
    }

    public static class NotZero
    {
        public static TheoryData<FluentCase<int?>> Cases => F.IsNotZero.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsNotZero.Null) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must not be zero.")
        });
    }

    public static class ZeroOrPositive
    {
        public static TheoryData<FluentCase<int?>> Cases => F.IsZeroOrPositive.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsZeroOrPositive.Null) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be zero or positive.")
        });
    }

    public static class ZeroOrNegative
    {
        public static TheoryData<FluentCase<int?>> Cases => F.IsZeroOrNegative.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsZeroOrNegative.Null) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be zero or negative.")
        });
    }

    public static class InRange
    {
        public static TheoryData<FluentCase<(int? value, int min, int max, Inclusion inclusion)>> Cases => F.IsInRange.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsInRange.NullValue) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be within the expected range.")
        });
    }

    public static class OutOfRange
    {
        private static RuleScenario<(int? value, int min, int max, Inclusion inclusion)>[] AllScenarios =>
        [
            new("LessThanMin",    (0,    1, 10, Inclusion.Inclusive), true),
            new("GreaterThanMax", (11,   1, 10, Inclusion.Inclusive), true),
            new("InRange",        (5,    1, 10, Inclusion.Inclusive), false),
            new("AtMinInclusive", (1,    1, 10, Inclusion.Inclusive), false),
            new("NullValue",      (null, 1, 10, Inclusion.Inclusive), false)
        ];

        public static TheoryData<FluentCase<(int? value, int min, int max, Inclusion inclusion)>> Cases => AllScenarios.ToFluentCases(s => s.Name switch
        {
            "NullValue" => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be out of the expected range.")
        });
    }

    public static class Percentage
    {
        public static TheoryData<FluentCase<decimal?>> Cases => F.IsPercentage.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsPercentage.NullValue) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be a percentage between 0 and 100.", Code: MustCodes.Number.Range.NotPercentage)
        });
    }

    public static class Approximately
    {
        public static TheoryData<FluentCase<(decimal? value, decimal target, decimal? tolerance)>> Cases => F.IsApproximately.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsApproximately.NullValue) => new FluentExpected(true),
            nameof(F.IsApproximately.NullTolerance) => new FluentExpected(false, "tolerance requires a non-null tolerance."),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be approximately the target value.")
        });
    }

    public static class NotApproximately
    {
        private static RuleScenario<(decimal? value, decimal target, decimal? tolerance)>[] AllScenarios =>
        [
            new("OutsideTolerance",  (10.3m, 10.0m, 0.2m),  true),
            new("WithinTolerance",   (10.1m, 10.0m, 0.2m),  false),
            new("ExactMatch",        (10.0m, 10.0m, 0.2m),  false),
            new("NegativeTolerance", (10.0m, 10.0m, -0.1m), false),
            new("NullValue",         (null,  10.0m, 0.2m),  false),
            new("NullTolerance",     (10.0m, 10.0m, null),  false)
        ];

        public static TheoryData<FluentCase<(decimal? value, decimal target, decimal? tolerance)>> Cases => AllScenarios.ToFluentCases(s => s.Name switch
        {
            "NullValue" => new FluentExpected(true),
            "NegativeTolerance" => new FluentExpected(true),
            "NullTolerance" => new FluentExpected(false, "tolerance requires a non-null tolerance."),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must not be approximately the target value.")
        });
    }

    public static class MultipleOf
    {
        public static TheoryData<FluentCase<(int? value, int factor)>> Cases => F.IsMultipleOf.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsMultipleOf.Null) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be a multiple of the specified factor.")
        });
    }

    public static class NotMultipleOf
    {
        private static RuleScenario<(int? value, int factor)>[] AllScenarios =>
        [
            new("NotMultiple", (5,    2), true),
            new("Multiple",    (4,    2), false),
            new("ZeroFactor",  (4,    0), false),
            new("Null",        (null, 2), false)
        ];

        public static TheoryData<FluentCase<(int? value, int factor)>> Cases => AllScenarios.ToFluentCases(s => s.Name switch
        {
            "Null" => new FluentExpected(true),
            "ZeroFactor" => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must not be a multiple of the specified factor.")
        });
    }

    public static class Even
    {
        public static TheoryData<FluentCase<int?>> IntCases => F.IsEven.IntAllScenarios.ToFluentCases(s => s.Name switch
        {
            "NullInt" => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be even.")
        });

        public static TheoryData<FluentCase<long?>> LongCases => F.IsEven.LongAllScenarios.ToFluentCases(s => s.Name switch
        {
            "NullLong" => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be even.")
        });
    }

    public static class Odd
    {
        public static TheoryData<FluentCase<int?>> IntCases => F.IsOdd.IntAllScenarios.ToFluentCases(s => s.Name switch
        {
            "NullInt" => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be odd.")
        });

        public static TheoryData<FluentCase<long?>> LongCases => F.IsOdd.LongAllScenarios.ToFluentCases(s => s.Name switch
        {
            "NullLong" => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be odd.")
        });
    }

    public static class Finite
    {
        public static TheoryData<FluentCase<float?>> FloatCases => F.IsFinite.FloatAllScenarios.ToFluentCases(s => s.Name switch
        {
            "NullFloat" => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be finite.")
        });

        public static TheoryData<FluentCase<double?>> DoubleCases => F.IsFinite.DoubleAllScenarios.ToFluentCases(s => s.Name switch
        {
            "NullDouble" => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be finite.")
        });
    }

    public static class NotFinite
    {
        private static RuleScenario<float?>[] FloatAllScenarios =>
        [
            new("PositiveInfinityFloat", float.PositiveInfinity, true),
            new("NegativeInfinityFloat", float.NegativeInfinity, true),
            new("NaNFloat",              float.NaN,              true),
            new("FiniteFloat",           1.0f,                   false),
            new("NullFloat",             null,                   false)
        ];

        private static RuleScenario<double?>[] DoubleAllScenarios =>
        [
            new("PositiveInfinityDouble", double.PositiveInfinity, true),
            new("NegativeInfinityDouble", double.NegativeInfinity, true),
            new("NaNDouble",              double.NaN,              true),
            new("FiniteDouble",           1.0,                     false),
            new("NullDouble",             null,                    false)
        ];

        public static TheoryData<FluentCase<float?>> FloatCases => FloatAllScenarios.ToFluentCases(s => s.Name switch
        {
            "NullFloat" => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must not be finite.")
        });

        public static TheoryData<FluentCase<double?>> DoubleCases => DoubleAllScenarios.ToFluentCases(s => s.Name switch
        {
            "NullDouble" => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must not be finite.")
        });
    }

    public static class NaN
    {
        public static TheoryData<FluentCase<float?>> FloatCases => F.IsNaN.FloatAllScenarios.ToFluentCases(s => s.Name switch
        {
            "NullFloat" => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be NaN.")
        });

        public static TheoryData<FluentCase<double?>> DoubleCases => F.IsNaN.DoubleAllScenarios.ToFluentCases(s => s.Name switch
        {
            "NullDouble" => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be NaN.")
        });
    }

    public static class NotNaN
    {
        private static RuleScenario<float?>[] FloatAllScenarios =>
        [
            new("FiniteFloat",   1.0f,                   true),
            new("InfinityFloat", float.PositiveInfinity, true),
            new("NaNFloat",      float.NaN,              false),
            new("NullFloat",     null,                   false)
        ];

        private static RuleScenario<double?>[] DoubleAllScenarios =>
        [
            new("FiniteDouble",   1.0,                    true),
            new("InfinityDouble", double.PositiveInfinity, true),
            new("NaNDouble",      double.NaN,             false),
            new("NullDouble",     null,                   false)
        ];

        public static TheoryData<FluentCase<float?>> FloatCases => FloatAllScenarios.ToFluentCases(s => s.Name switch
        {
            "NullFloat" => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must not be NaN.")
        });

        public static TheoryData<FluentCase<double?>> DoubleCases => DoubleAllScenarios.ToFluentCases(s => s.Name switch
        {
            "NullDouble" => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must not be NaN.")
        });
    }

    // ── Non-nullable overloads ─────────────────────────────────────────────

    public static class EvenNonNullable
    {
        public static TheoryData<FluentCase<int>> IntCases => F.IsEven.IntNonNullableAllScenarios.ToFluentCases(s => s.Name switch
        {
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be even.")
        });

        public static TheoryData<FluentCase<long>> LongCases => F.IsEven.LongNonNullableAllScenarios.ToFluentCases(s => s.Name switch
        {
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be even.")
        });
    }

    public static class OddNonNullable
    {
        public static TheoryData<FluentCase<int>> IntCases => F.IsOdd.IntNonNullableAllScenarios.ToFluentCases(s => s.Name switch
        {
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be odd.")
        });

        public static TheoryData<FluentCase<long>> LongCases => F.IsOdd.LongNonNullableAllScenarios.ToFluentCases(s => s.Name switch
        {
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be odd.")
        });
    }

    public static class FiniteNonNullable
    {
        public static TheoryData<FluentCase<float>> FloatCases => F.IsFinite.FloatNonNullableAllScenarios.ToFluentCases(s => s.Name switch
        {
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be finite.")
        });

        public static TheoryData<FluentCase<double>> DoubleCases => F.IsFinite.DoubleNonNullableAllScenarios.ToFluentCases(s => s.Name switch
        {
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be finite.")
        });
    }

    public static class NotFiniteNonNullable
    {
        private static RuleScenario<float>[] FloatAllScenarios =>
        [
            new("PositiveInfinityFloat", float.PositiveInfinity, true),
            new("NaNFloat",              float.NaN,              true),
            new("FiniteFloat",           1.0f,                   false)
        ];

        private static RuleScenario<double>[] DoubleAllScenarios =>
        [
            new("PositiveInfinityDouble", double.PositiveInfinity, true),
            new("NaNDouble",              double.NaN,              true),
            new("FiniteDouble",           1.0,                     false)
        ];

        public static TheoryData<FluentCase<float>> FloatCases => FloatAllScenarios.ToFluentCases(s => s.Name switch
        {
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must not be finite.")
        });

        public static TheoryData<FluentCase<double>> DoubleCases => DoubleAllScenarios.ToFluentCases(s => s.Name switch
        {
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must not be finite.")
        });
    }

    public static class NaNNonNullable
    {
        public static TheoryData<FluentCase<float>> FloatCases => F.IsNaN.FloatNonNullableAllScenarios.ToFluentCases(s => s.Name switch
        {
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be NaN.")
        });

        public static TheoryData<FluentCase<double>> DoubleCases => F.IsNaN.DoubleNonNullableAllScenarios.ToFluentCases(s => s.Name switch
        {
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be NaN.")
        });
    }

    public static class NotNaNNonNullable
    {
        private static RuleScenario<float>[] FloatAllScenarios =>
        [
            new("FiniteFloat",   1.0f,                   true),
            new("InfinityFloat", float.PositiveInfinity, true),
            new("NaNFloat",      float.NaN,              false)
        ];

        private static RuleScenario<double>[] DoubleAllScenarios =>
        [
            new("FiniteDouble",   1.0,                    true),
            new("InfinityDouble", double.PositiveInfinity, true),
            new("NaNDouble",      double.NaN,             false)
        ];

        public static TheoryData<FluentCase<float>> FloatCases => FloatAllScenarios.ToFluentCases(s => s.Name switch
        {
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must not be NaN.")
        });

        public static TheoryData<FluentCase<double>> DoubleCases => DoubleAllScenarios.ToFluentCases(s => s.Name switch
        {
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must not be NaN.")
        });
    }
}
