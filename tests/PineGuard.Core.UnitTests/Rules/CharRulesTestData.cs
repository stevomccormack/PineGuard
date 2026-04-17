using PineGuard.Rules;
using PineGuard.Testing.UnitTests;
using PineGuard.Testing.UnitTests.Rules;
using F = PineGuard.Testing.Fixtures.CharRulesFixtures;

namespace PineGuard.Core.UnitTests.Rules;

public static class CharRulesTestData
{
    public static class Constants
    {
        public static TheoryData<ValidCase> Cases =>
        [
            new("AsciiMinValue", (char)0x00, CharRules.AsciiMinValue),
            new("AsciiMaxValue", (char)0x7F, CharRules.AsciiMaxValue),
            new("PrintableAsciiMinValue", (char)0x20, CharRules.PrintableAsciiMinValue),
            new("PrintableAsciiMaxValue", (char)0x7E, CharRules.PrintableAsciiMaxValue)
        ];

        public sealed record ValidCase(string Name, char Expected, char Actual) : ValueCase<char>(Name, Expected);
    }

    public static class IsLetter
    {
        public static TheoryData<RuleCase<char?>> Cases => F.IsLetter.AllScenarios.ToRuleCases();
    }

    public static class IsDigit
    {
        public static TheoryData<RuleCase<char?>> Cases => F.IsDigit.AllScenarios.ToRuleCases();
    }

    public static class IsLetterOrDigit
    {
        public static TheoryData<RuleCase<char?>> Cases => F.IsLetterOrDigit.AllScenarios.ToRuleCases();
    }

    public static class IsAscii
    {
        public static TheoryData<RuleCase<char?>> Cases => F.IsAscii.AllScenarios.ToRuleCases();
    }

    public static class IsPrintableAscii
    {
        public static TheoryData<RuleCase<char?>> Cases => F.IsPrintableAscii.AllScenarios.ToRuleCases();
    }

    public static class IsWhitespace
    {
        public static TheoryData<RuleCase<char?>> Cases => F.IsWhitespace.AllScenarios.ToRuleCases();
    }

    public static class IsControl
    {
        public static TheoryData<RuleCase<char?>> Cases => F.IsControl.AllScenarios.ToRuleCases();
    }

    public static class IsUppercase
    {
        public static TheoryData<RuleCase<char?>> Cases => F.IsUppercase.AllScenarios.ToRuleCases();
    }

    public static class IsLowercase
    {
        public static TheoryData<RuleCase<char?>> Cases => F.IsLowercase.AllScenarios.ToRuleCases();
    }

    public static class IsHexDigit
    {
        public static TheoryData<RuleCase<char?>> Cases => F.IsHexDigit.AllScenarios.ToRuleCases();
    }
}
