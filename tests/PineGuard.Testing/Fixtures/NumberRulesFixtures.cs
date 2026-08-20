using PineGuard.Common;
using PineGuard.Testing.UnitTests.Rules;

namespace PineGuard.Testing.Fixtures;

public static class NumberRulesFixtures
{
    public static class IsPositive
    {
        public static readonly int? Positive = 1;
        public static readonly int? Zero = 0;
        public static readonly int? Negative = -1;
        public static readonly int? Null = null;

        public static RuleScenario<int?>[] ValidScenarios =>
        [
            new(nameof(Positive), Positive, true)
        ];

        public static RuleScenario<int?>[] InvalidScenarios =>
        [
            new(nameof(Zero),     Zero,     false),
            new(nameof(Negative), Negative, false),
            new(nameof(Null),     Null,     false)
        ];

        public static RuleScenario<int?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsNegative
    {
        public static readonly int? Negative = -1;
        public static readonly int? Zero = 0;
        public static readonly int? Positive = 1;
        public static readonly int? Null = null;

        public static RuleScenario<int?>[] ValidScenarios =>
        [
            new(nameof(Negative), Negative, true)
        ];

        public static RuleScenario<int?>[] InvalidScenarios =>
        [
            new(nameof(Zero),     Zero,     false),
            new(nameof(Positive), Positive, false),
            new(nameof(Null),     Null,     false)
        ];

        public static RuleScenario<int?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsZero
    {
        public static readonly int? Zero = 0;
        public static readonly int? NonZero = 1;
        public static readonly int? Null = null;

        public static RuleScenario<int?>[] ValidScenarios =>
        [
            new(nameof(Zero),    Zero,    true)
        ];

        public static RuleScenario<int?>[] InvalidScenarios =>
        [
            new(nameof(NonZero), NonZero, false),
            new(nameof(Null),    Null,    false)
        ];

        public static RuleScenario<int?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsNotZero
    {
        public static readonly int? Positive = 1;
        public static readonly int? Negative = -1;
        public static readonly int? Zero = 0;
        public static readonly int? Null = null;

        public static RuleScenario<int?>[] ValidScenarios =>
        [
            new(nameof(Positive), Positive, true),
            new(nameof(Negative), Negative, true)
        ];

        public static RuleScenario<int?>[] InvalidScenarios =>
        [
            new(nameof(Zero), Zero, false),
            new(nameof(Null), Null, false)
        ];

        public static RuleScenario<int?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsZeroOrPositive
    {
        public static readonly int? Zero = 0;
        public static readonly int? Positive = 1;
        public static readonly int? Negative = -1;
        public static readonly int? Null = null;

        public static RuleScenario<int?>[] ValidScenarios =>
        [
            new(nameof(Zero),     Zero,     true),
            new(nameof(Positive), Positive, true)
        ];

        public static RuleScenario<int?>[] InvalidScenarios =>
        [
            new(nameof(Negative), Negative, false),
            new(nameof(Null),     Null,     false)
        ];

        public static RuleScenario<int?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsZeroOrNegative
    {
        public static readonly int? Zero = 0;
        public static readonly int? Negative = -1;
        public static readonly int? Positive = 1;
        public static readonly int? Null = null;

        public static RuleScenario<int?>[] ValidScenarios =>
        [
            new(nameof(Zero),     Zero,     true),
            new(nameof(Negative), Negative, true)
        ];

        public static RuleScenario<int?>[] InvalidScenarios =>
        [
            new(nameof(Positive), Positive, false),
            new(nameof(Null),     Null,     false)
        ];

        public static RuleScenario<int?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsGreaterThan
    {
        public static readonly (int? value, int min) Greater = (2, 1);
        public static readonly (int? value, int min) Equal = (1, 1);
        public static readonly (int? value, int min) Null = (null, 1);

        public static RuleScenario<(int? value, int min)>[] ValidScenarios =>
        [
            new(nameof(Greater), Greater, true)
        ];

        public static RuleScenario<(int? value, int min)>[] InvalidScenarios =>
        [
            new(nameof(Equal), Equal, false),
            new(nameof(Null),  Null,  false)
        ];

        public static RuleScenario<(int? value, int min)>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsGreaterThanOrEqual
    {
        public static readonly (int? value, int min) Greater = (2, 1);
        public static readonly (int? value, int min) Equal = (1, 1);
        public static readonly (int? value, int min) Less = (0, 1);
        public static readonly (int? value, int min) Null = (null, 1);

        public static RuleScenario<(int? value, int min)>[] ValidScenarios =>
        [
            new(nameof(Greater), Greater, true),
            new(nameof(Equal),   Equal,   true)
        ];

        public static RuleScenario<(int? value, int min)>[] InvalidScenarios =>
        [
            new(nameof(Less), Less, false),
            new(nameof(Null), Null, false)
        ];

        public static RuleScenario<(int? value, int min)>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsLessThan
    {
        public static readonly (int? value, int max) Less = (0, 1);
        public static readonly (int? value, int max) Equal = (1, 1);
        public static readonly (int? value, int max) Null = (null, 1);

        public static RuleScenario<(int? value, int max)>[] ValidScenarios =>
        [
            new(nameof(Less), Less, true)
        ];

        public static RuleScenario<(int? value, int max)>[] InvalidScenarios =>
        [
            new(nameof(Equal), Equal, false),
            new(nameof(Null),  Null,  false)
        ];

        public static RuleScenario<(int? value, int max)>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsLessThanOrEqual
    {
        public static readonly (int? value, int max) Less = (0, 1);
        public static readonly (int? value, int max) Equal = (1, 1);
        public static readonly (int? value, int max) Greater = (2, 1);
        public static readonly (int? value, int max) Null = (null, 1);

        public static RuleScenario<(int? value, int max)>[] ValidScenarios =>
        [
            new(nameof(Less),  Less,  true),
            new(nameof(Equal), Equal, true)
        ];

        public static RuleScenario<(int? value, int max)>[] InvalidScenarios =>
        [
            new(nameof(Greater), Greater, false),
            new(nameof(Null),    Null,    false)
        ];

        public static RuleScenario<(int? value, int max)>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsInRange
    {
        public static readonly (int? value, int min, int max, Inclusion inclusion) BetweenInclusive = (5, 1, 10, Inclusion.Inclusive);
        public static readonly (int? value, int min, int max, Inclusion inclusion) AtMinInclusive = (1, 1, 10, Inclusion.Inclusive);
        public static readonly (int? value, int min, int max, Inclusion inclusion) AtMinExclusive = (1, 1, 10, Inclusion.Exclusive);
        public static readonly (int? value, int min, int max, Inclusion inclusion) NullValue = (null, 1, 10, Inclusion.Inclusive);

        public static RuleScenario<(int? value, int min, int max, Inclusion inclusion)>[] ValidScenarios =>
        [
            new(nameof(BetweenInclusive), BetweenInclusive, true),
            new(nameof(AtMinInclusive),   AtMinInclusive,   true)
        ];

        public static RuleScenario<(int? value, int min, int max, Inclusion inclusion)>[] InvalidScenarios =>
        [
            new(nameof(AtMinExclusive), AtMinExclusive, false),
            new(nameof(NullValue),      NullValue,      false)
        ];

        public static RuleScenario<(int? value, int min, int max, Inclusion inclusion)>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsApproximately
    {
        public static readonly (decimal? value, decimal target, decimal? tolerance) WithinTolerance = (10.0m, 10.1m, 0.2m);
        public static readonly (decimal? value, decimal target, decimal? tolerance) OutsideTolerance = (10.0m, 10.3m, 0.2m);
        public static readonly (decimal? value, decimal target, decimal? tolerance) NegativeTolerance = (10.0m, 10.0m, -0.1m);
        public static readonly (decimal? value, decimal target, decimal? tolerance) NullValue = (null, 10.0m, 0.2m);
        public static readonly (decimal? value, decimal target, decimal? tolerance) NullTolerance = (10.0m, 10.0m, null);

        public static RuleScenario<(decimal? value, decimal target, decimal? tolerance)>[] ValidScenarios =>
        [
            new(nameof(WithinTolerance), WithinTolerance, true)
        ];

        public static RuleScenario<(decimal? value, decimal target, decimal? tolerance)>[] InvalidScenarios =>
        [
            new(nameof(OutsideTolerance),  OutsideTolerance,  false),
            new(nameof(NegativeTolerance), NegativeTolerance, false),
            new(nameof(NullValue),         NullValue,         false),
            new(nameof(NullTolerance),     NullTolerance,     false)
        ];

        public static RuleScenario<(decimal? value, decimal target, decimal? tolerance)>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsApproximatelyUnsignedUnderflow
    {
        public static readonly (uint? value, uint target, uint? tolerance) BelowTargetWithinTolerance = (3u, 5u, 2u);
        public static readonly (uint? value, uint target, uint? tolerance) BelowTargetOutsideTolerance = (3u, 10u, 2u);

        public static RuleScenario<(uint? value, uint target, uint? tolerance)>[] ValidScenarios =>
        [
            new(nameof(BelowTargetWithinTolerance), BelowTargetWithinTolerance, true)
        ];

        public static RuleScenario<(uint? value, uint target, uint? tolerance)>[] InvalidScenarios =>
        [
            new(nameof(BelowTargetOutsideTolerance), BelowTargetOutsideTolerance, false)
        ];

        public static RuleScenario<(uint? value, uint target, uint? tolerance)>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsApproximatelySignedOverflowGuard
    {
        public static readonly (int? value, int target, int? tolerance) ExtremeRange = (int.MaxValue, int.MinValue, 10);
        public static readonly (int? value, int target, int? tolerance) MinValueVsZero = (int.MinValue, 0, 5);

        public static RuleScenario<(int? value, int target, int? tolerance)>[] InvalidScenarios =>
        [
            new(nameof(ExtremeRange), ExtremeRange, false),
            new(nameof(MinValueVsZero), MinValueVsZero, false)
        ];

        public static RuleScenario<(int? value, int target, int? tolerance)>[] AllScenarios => [.. InvalidScenarios];
    }

    public static class IsMultipleOf
    {
        public static readonly (int? value, int factor) Multiple = (4, 2);
        public static readonly (int? value, int factor) NotMultiple = (5, 2);
        public static readonly (int? value, int factor) ZeroFactor = (4, 0);
        public static readonly (int? value, int factor) Null = (null, 2);
        public static readonly (int? value, int factor) MinValueByNegativeOne = (int.MinValue, -1);

        public static RuleScenario<(int? value, int factor)>[] ValidScenarios =>
        [
            new(nameof(Multiple), Multiple, true),
            new(nameof(MinValueByNegativeOne), MinValueByNegativeOne, true)
        ];

        public static RuleScenario<(int? value, int factor)>[] InvalidScenarios =>
        [
            new(nameof(NotMultiple), NotMultiple, false),
            new(nameof(ZeroFactor),  ZeroFactor,  false),
            new(nameof(Null),        Null,        false)
        ];

        public static RuleScenario<(int? value, int factor)>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsEven
    {
        public static readonly int EvenInt = 4;
        public static readonly int OddInt = 5;
        public static readonly long EvenLong = 4L;
        public static readonly long OddLong = 5L;

        public static RuleScenario<int?>[] IntValidScenarios =>
        [
            new(nameof(EvenInt), EvenInt, true)
        ];

        public static RuleScenario<int?>[] IntInvalidScenarios =>
        [
            new(nameof(OddInt), OddInt,  false),
            new("NullInt",      null,    false)
        ];

        public static RuleScenario<int?>[] IntAllScenarios => [.. IntValidScenarios, .. IntInvalidScenarios];

        public static RuleScenario<long?>[] LongValidScenarios =>
        [
            new(nameof(EvenLong), EvenLong, true)
        ];

        public static RuleScenario<long?>[] LongInvalidScenarios =>
        [
            new(nameof(OddLong), OddLong, false),
            new("NullLong",      null,    false)
        ];

        public static RuleScenario<long?>[] LongAllScenarios => [.. LongValidScenarios, .. LongInvalidScenarios];

        public static RuleScenario<int>[] IntNonNullableAllScenarios => [new(nameof(EvenInt), EvenInt, true), new(nameof(OddInt), OddInt, false)];
        public static RuleScenario<long>[] LongNonNullableAllScenarios => [new(nameof(EvenLong), EvenLong, true), new(nameof(OddLong), OddLong, false)];
    }

    public static class IsOdd
    {
        public static readonly int OddInt = 5;
        public static readonly int EvenInt = 4;
        public static readonly long OddLong = 5L;
        public static readonly long EvenLong = 4L;

        public static RuleScenario<int?>[] IntValidScenarios =>
        [
            new(nameof(OddInt), OddInt, true)
        ];

        public static RuleScenario<int?>[] IntInvalidScenarios =>
        [
            new(nameof(EvenInt), EvenInt, false),
            new("NullInt",       null,    false)
        ];

        public static RuleScenario<int?>[] IntAllScenarios => [.. IntValidScenarios, .. IntInvalidScenarios];

        public static RuleScenario<long?>[] LongValidScenarios =>
        [
            new(nameof(OddLong), OddLong, true)
        ];

        public static RuleScenario<long?>[] LongInvalidScenarios =>
        [
            new(nameof(EvenLong), EvenLong, false),
            new("NullLong",       null,     false)
        ];

        public static RuleScenario<long?>[] LongAllScenarios => [.. LongValidScenarios, .. LongInvalidScenarios];

        public static RuleScenario<int>[] IntNonNullableAllScenarios => [new(nameof(OddInt), OddInt, true), new(nameof(EvenInt), EvenInt, false)];
        public static RuleScenario<long>[] LongNonNullableAllScenarios => [new(nameof(OddLong), OddLong, true), new(nameof(EvenLong), EvenLong, false)];
    }

    public static class IsFinite
    {
        public static readonly float FiniteFloat = 1.0f;
        public static readonly float PositiveInfinityFloat = float.PositiveInfinity;
        public static readonly double FiniteDouble = 1.0;
        public static readonly double PositiveInfinityDouble = double.PositiveInfinity;

        public static RuleScenario<float?>[] FloatValidScenarios =>
        [
            new(nameof(FiniteFloat), FiniteFloat, true)
        ];

        public static RuleScenario<float?>[] FloatInvalidScenarios =>
        [
            new(nameof(PositiveInfinityFloat), PositiveInfinityFloat, false),
            new("NullFloat",                   null,                  false)
        ];

        public static RuleScenario<float?>[] FloatAllScenarios => [.. FloatValidScenarios, .. FloatInvalidScenarios];

        public static RuleScenario<double?>[] DoubleValidScenarios =>
        [
            new(nameof(FiniteDouble), FiniteDouble, true)
        ];

        public static RuleScenario<double?>[] DoubleInvalidScenarios =>
        [
            new(nameof(PositiveInfinityDouble), PositiveInfinityDouble, false),
            new("NullDouble",                   null,                   false)
        ];

        public static RuleScenario<double?>[] DoubleAllScenarios => [.. DoubleValidScenarios, .. DoubleInvalidScenarios];

        public static RuleScenario<float>[] FloatNonNullableAllScenarios => [new(nameof(FiniteFloat), FiniteFloat, true), new(nameof(PositiveInfinityFloat), PositiveInfinityFloat, false)];
        public static RuleScenario<double>[] DoubleNonNullableAllScenarios => [new(nameof(FiniteDouble), FiniteDouble, true), new(nameof(PositiveInfinityDouble), PositiveInfinityDouble, false)];
    }

    public static class IsNaN
    {
        public static readonly float NaNFloat = float.NaN;
        public static readonly float FiniteFloat = 1.0f;
        public static readonly double NaNDouble = double.NaN;
        public static readonly double FiniteDouble = 1.0;

        public static RuleScenario<float?>[] FloatValidScenarios =>
        [
            new(nameof(NaNFloat), NaNFloat, true)
        ];

        public static RuleScenario<float?>[] FloatInvalidScenarios =>
        [
            new(nameof(FiniteFloat), FiniteFloat, false),
            new("NullFloat",         null,        false)
        ];

        public static RuleScenario<float?>[] FloatAllScenarios => [.. FloatValidScenarios, .. FloatInvalidScenarios];

        public static RuleScenario<double?>[] DoubleValidScenarios =>
        [
            new(nameof(NaNDouble), NaNDouble, true)
        ];

        public static RuleScenario<double?>[] DoubleInvalidScenarios =>
        [
            new(nameof(FiniteDouble), FiniteDouble, false),
            new("NullDouble",         null,         false)
        ];

        public static RuleScenario<double?>[] DoubleAllScenarios => [.. DoubleValidScenarios, .. DoubleInvalidScenarios];

        public static RuleScenario<float>[] FloatNonNullableAllScenarios => [new(nameof(NaNFloat), NaNFloat, true), new(nameof(FiniteFloat), FiniteFloat, false)];
        public static RuleScenario<double>[] DoubleNonNullableAllScenarios => [new(nameof(NaNDouble), NaNDouble, true), new(nameof(FiniteDouble), FiniteDouble, false)];
    }
}
