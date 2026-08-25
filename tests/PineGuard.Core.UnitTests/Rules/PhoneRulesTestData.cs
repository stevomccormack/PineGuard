using PineGuard.Testing.UnitTests;
using PineGuard.Testing.UnitTests.Rules;
using F = PineGuard.Testing.Fixtures.PhoneRulesFixtures;

namespace PineGuard.Core.UnitTests.Rules;

public static class PhoneRulesTestData
{
    public static class DefaultAllowedNonDigitCharacters
    {
        public static TheoryData<Case> Cases =>
        [
            new("index 0", 0, '+'),
            new("index 1", 1, '('),
            new("index 2", 2, ')'),
            new("index 3", 3, '-'),
            new("index 4", 4, '.'),
            new("index 5", 5, '/')
        ];

        public sealed record Case(string Name, int Value, char Expected)
            : ReturnCase<int, char>(Name, Value, Expected);
    }

    public static class IsPhoneNumber
    {
        public static TheoryData<RuleCase<string?>> Cases => F.IsPhoneNumber.AllScenarios.ToRuleCases();
    }
}
