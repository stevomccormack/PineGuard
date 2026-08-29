using PineGuard.Testing.UnitTests.Rules;
using F = PineGuard.Testing.Fixtures.NumberRulesFixtures;

namespace PineGuard.Core.UnitTests.Rules;

public static class NumberRulesTestData
{
    public static class IsPositiveInt
    {
        public static TheoryData<RuleCase<int?>> Cases => F.IsPositive.AllScenarios.ToRuleCases();
    }

    public static class IsNegativeInt
    {
        public static TheoryData<RuleCase<int?>> Cases => F.IsNegative.AllScenarios.ToRuleCases();
    }

    public static class IsZeroInt
    {
        public static TheoryData<RuleCase<int?>> Cases => F.IsZero.AllScenarios.ToRuleCases();
    }

    public static class IsNotZeroInt
    {
        public static TheoryData<RuleCase<int?>> Cases => F.IsNotZero.AllScenarios.ToRuleCases();
    }

    public static class IsZeroOrPositiveInt
    {
        public static TheoryData<RuleCase<int?>> Cases => F.IsZeroOrPositive.AllScenarios.ToRuleCases();
    }

    public static class IsZeroOrNegativeInt
    {
        public static TheoryData<RuleCase<int?>> Cases => F.IsZeroOrNegative.AllScenarios.ToRuleCases();
    }

    public static class IsGreaterThan
    {
        public static TheoryData<RuleCase<(int? value, int min)>> Cases => F.IsGreaterThan.AllScenarios.ToRuleCases();
    }

    public static class IsGreaterThanOrEqual
    {
        public static TheoryData<RuleCase<(int? value, int min)>> Cases => F.IsGreaterThanOrEqual.AllScenarios.ToRuleCases();
    }

    public static class IsLessThan
    {
        public static TheoryData<RuleCase<(int? value, int max)>> Cases => F.IsLessThan.AllScenarios.ToRuleCases();
    }

    public static class IsLessThanOrEqual
    {
        public static TheoryData<RuleCase<(int? value, int max)>> Cases => F.IsLessThanOrEqual.AllScenarios.ToRuleCases();
    }

    public static class IsInRange
    {
        public static TheoryData<RuleCase<(int? value, int min, int max, PineGuard.Common.Inclusion inclusion)>> Cases => F.IsInRange.AllScenarios.ToRuleCases();
    }

    public static class IsPercentage
    {
        public static TheoryData<RuleCase<decimal?>> Cases => F.IsPercentage.AllScenarios.ToRuleCases();
    }

    public static class IsApproximately
    {
        public static TheoryData<RuleCase<(decimal? value, decimal target, decimal? tolerance)>> Cases => F.IsApproximately.AllScenarios.ToRuleCases();
    }

    public static class IsApproximatelyUnsignedUnderflow
    {
        public static TheoryData<RuleCase<(uint? value, uint target, uint? tolerance)>> Cases => F.IsApproximatelyUnsignedUnderflow.AllScenarios.ToRuleCases();
    }

    public static class IsApproximatelySignedOverflowGuard
    {
        public static TheoryData<RuleCase<(int? value, int target, int? tolerance)>> Cases => F.IsApproximatelySignedOverflowGuard.AllScenarios.ToRuleCases();
    }

    public static class IsMultipleOf
    {
        public static TheoryData<RuleCase<(int? value, int factor)>> Cases => F.IsMultipleOf.AllScenarios.ToRuleCases();
    }

    public static class IsEvenInt
    {
        public static TheoryData<RuleCase<int?>> Cases => F.IsEven.IntAllScenarios.ToRuleCases();
    }

    public static class IsEvenLong
    {
        public static TheoryData<RuleCase<long?>> Cases => F.IsEven.LongAllScenarios.ToRuleCases();
    }

    public static class IsOddInt
    {
        public static TheoryData<RuleCase<int?>> Cases => F.IsOdd.IntAllScenarios.ToRuleCases();
    }

    public static class IsOddLong
    {
        public static TheoryData<RuleCase<long?>> Cases => F.IsOdd.LongAllScenarios.ToRuleCases();
    }

    public static class IsFiniteFloat
    {
        public static TheoryData<RuleCase<float?>> Cases => F.IsFinite.FloatAllScenarios.ToRuleCases();
    }

    public static class IsFiniteDouble
    {
        public static TheoryData<RuleCase<double?>> Cases => F.IsFinite.DoubleAllScenarios.ToRuleCases();
    }

    public static class IsNaNFloat
    {
        public static TheoryData<RuleCase<float?>> Cases => F.IsNaN.FloatAllScenarios.ToRuleCases();
    }

    public static class IsNaNDouble
    {
        public static TheoryData<RuleCase<double?>> Cases => F.IsNaN.DoubleAllScenarios.ToRuleCases();
    }
}
