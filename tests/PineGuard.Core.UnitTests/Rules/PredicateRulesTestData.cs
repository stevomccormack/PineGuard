using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;
using PineGuard.Testing.UnitTests.Rules;
using F = PineGuard.Testing.Fixtures.PredicateRulesFixtures;

namespace PineGuard.Core.UnitTests.Rules;

public static class PredicateRulesTestData
{
    public static class Satisfies
    {
        public static TheoryData<RuleCase<(string? value, Func<string, bool> predicate)>> Cases => F.Satisfies.AllScenarios.ToRuleCases();

        public static TheoryData<InvalidCase> InvalidCases =>
        [
            new("null predicate with value", ("hello", null!), new ExpectedException(typeof(ArgumentNullException))),
            new("null predicate with null value", (null, null!), new ExpectedException(typeof(ArgumentNullException)))
        ];

        public sealed record InvalidCase(string Name, (string? Value, Func<string, bool>? Predicate) Value, ExpectedException ExpectedException)
            : ThrowsCase<(string? Value, Func<string, bool>? Predicate)>(Name, Value, ExpectedException);
    }
}
