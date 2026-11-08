using PineGuard.Common;
using PineGuard.Testing.UnitTests.Rules;
using F = PineGuard.Testing.Fixtures.StringRulesFixtures;

namespace PineGuard.Core.UnitTests.Rules.StringRules;

public static class StringRulesNumberTypesTestData
{
    public static class DefaultAllowedDigitSeparators
    {
        public static TheoryData<RuleCase<int>> Cases =>
        [
            new("space", 0, new RuleExpected(true)),
            new("hyphen", 1, new RuleExpected(true))
        ];
    }

    public static class IsDecimal
    {
        public static TheoryData<RuleCase<string?>> Cases => F.IsDecimal.AllScenarios.ToRuleCases();
    }

    public static class IsDecimalWithZeroPlaces
    {
        public static TheoryData<RuleCase<string?>> Cases =>
        [
            new("123 => true", "123", new RuleExpected(true)),
            new("+123 => true", "+123", new RuleExpected(true)),
            new("-0 => true", "-0", new RuleExpected(true)),
            new("1.0 => false", "1.0", new RuleExpected(false))
        ];
    }

    public static class IsDecimalNegativePlaces
    {
        public static TheoryData<RuleCase<(string? value, int decimalPlaces)>> Cases =>
        [
            new("NegativePlaces", ("1.23", -1), new RuleExpected(false))
        ];
    }

    public static class IsExactDecimal
    {
        public static TheoryData<RuleCase<string?>> Cases => F.IsExactDecimal.AllScenarios.ToRuleCases();
    }

    public static class IsExactDecimalWithZeroPlaces
    {
        public static TheoryData<RuleCase<string?>> Cases =>
        [
            new("123 => true", "123", new RuleExpected(true)),
            new("+123 => true", "+123", new RuleExpected(true)),
            new("-0 => true", "-0", new RuleExpected(true)),
            new("1.0 => false", "1.0", new RuleExpected(false))
        ];
    }

    public static class IsExactDecimalNegativePlaces
    {
        public static TheoryData<RuleCase<(string? value, int exactDecimalPlaces)>> Cases =>
        [
            new("NegativePlaces", ("1.23", -1), new RuleExpected(false))
        ];
    }

    public static class IsInt32
    {
        public static TheoryData<RuleCase<string?>> Cases => F.IsInt32.AllScenarios.ToRuleCases();
    }

    public static class IsInt64
    {
        public static TheoryData<RuleCase<string?>> Cases => F.IsInt64.AllScenarios.ToRuleCases();
    }

    public static class IsInRangeInt32
    {
        public static TheoryData<RuleCase<(string text, int min, int max, Inclusion inclusion)>> Cases =>
            F.IsInRangeInt32.AllScenarios.ToRuleCases();
    }

    public static class IsInRangeInt64
    {
        public static TheoryData<RuleCase<(string text, long min, long max, Inclusion inclusion)>> Cases =>
            F.IsInRangeInt64.AllScenarios.ToRuleCases();
    }

}
