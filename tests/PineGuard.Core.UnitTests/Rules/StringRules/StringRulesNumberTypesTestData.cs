using PineGuard.Common;
using PineGuard.Testing.UnitTests;
using PineGuard.Testing.UnitTests.Rules;
using F = PineGuard.Testing.Fixtures.StringRulesFixtures;

namespace PineGuard.Core.UnitTests.Rules.StringRules;

public static class StringRulesNumberTypesTestData
{
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

    public static class IsInt32InRange
    {
        public static TheoryData<RuleCase<(string text, int min, int max, Inclusion inclusion)>> Cases =>
            F.IsInt32InRange.AllScenarios.ToRuleCases();
    }

    public static class IsInt64InRange
    {
        public static TheoryData<RuleCase<(string text, long min, long max, Inclusion inclusion)>> Cases =>
            F.IsInt64InRange.AllScenarios.ToRuleCases();
    }

    public static class SignedIntegerRegex
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("positive with plus", "+123", true),
            new("negative", "-456", true),
            new("plain integer", "789", true),
            new("zero", "0", true),
            new("max int", "2147483647", true),
            new("min int", "-2147483648", true)
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("empty", "", false),
            new("letters", "abc", false),
            new("decimal", "12.34", false),
            new("space", " ", false),
            new("plus only", "+", false),
            new("minus only", "-", false)
        ];

        public sealed record ValidCase(string Name, string Value, bool Expected)
            : IsCase<string>(Name, Value, Expected);
    }
}
