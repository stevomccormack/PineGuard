using PineGuard.Common;
using PineGuard.Testing.UnitTests.Rules;
using F = PineGuard.Testing.Fixtures.StringRulesFixtures;

namespace PineGuard.Core.UnitTests.Rules.StringRules;

public static class StringRulesNumbersTestData
{
    public static class IsPositive
    {
        public static TheoryData<RuleCase<string?>> Cases => F.NumbersIsPositive.AllScenarios.ToRuleCases();
    }

    public static class IsNegative
    {
        public static TheoryData<RuleCase<string?>> Cases => F.NumbersIsNegative.AllScenarios.ToRuleCases();
    }

    public static class IsZero
    {
        public static TheoryData<RuleCase<string?>> Cases => F.NumbersIsZero.AllScenarios.ToRuleCases();
    }

    public static class IsNotZero
    {
        public static TheoryData<RuleCase<string?>> Cases => F.NumbersIsNotZero.AllScenarios.ToRuleCases();
    }

    public static class IsZeroOrPositive
    {
        public static TheoryData<RuleCase<string?>> Cases => F.NumbersIsZeroOrPositive.AllScenarios.ToRuleCases();
    }

    public static class IsZeroOrNegative
    {
        public static TheoryData<RuleCase<string?>> Cases => F.NumbersIsZeroOrNegative.AllScenarios.ToRuleCases();
    }

    public static class IsGreaterThan
    {
        public static TheoryData<RuleCase<(string? text, decimal min)>> Cases => F.NumbersIsGreaterThan.AllScenarios.ToRuleCases();
    }

    public static class IsGreaterThanOrEqual
    {
        public static TheoryData<RuleCase<(string? text, decimal min)>> Cases => F.NumbersIsGreaterThanOrEqual.AllScenarios.ToRuleCases();
    }

    public static class IsLessThan
    {
        public static TheoryData<RuleCase<(string? text, decimal max)>> Cases => F.NumbersIsLessThan.AllScenarios.ToRuleCases();
    }

    public static class IsLessThanOrEqual
    {
        public static TheoryData<RuleCase<(string? text, decimal max)>> Cases => F.NumbersIsLessThanOrEqual.AllScenarios.ToRuleCases();
    }

    public static class IsInRange
    {
        public static TheoryData<RuleCase<(string? text, decimal min, decimal max, Inclusion inclusion)>> Cases => F.NumbersIsInRange.AllScenarios.ToRuleCases();
    }

    public static class IsApproximately
    {
        public static TheoryData<RuleCase<(string? text, decimal target, decimal? tolerance)>> Cases => F.NumbersIsApproximately.AllScenarios.ToRuleCases();
    }

    public static class IsMultipleOf
    {
        public static TheoryData<RuleCase<(string? text, decimal factor)>> Cases => F.NumbersIsMultipleOf.AllScenarios.ToRuleCases();
    }

    public static class IsEven
    {
        public static TheoryData<RuleCase<string?>> Cases => F.NumbersIsEven.AllScenarios.ToRuleCases();
    }

    public static class IsOdd
    {
        public static TheoryData<RuleCase<string?>> Cases => F.NumbersIsOdd.AllScenarios.ToRuleCases();
    }

    public static class IsFinite
    {
        public static TheoryData<RuleCase<string?>> Cases => F.NumbersIsFinite.AllScenarios.ToRuleCases();
    }

    public static class IsNaN
    {
        public static TheoryData<RuleCase<string?>> Cases => F.NumbersIsNaN.AllScenarios.ToRuleCases();
    }
}
