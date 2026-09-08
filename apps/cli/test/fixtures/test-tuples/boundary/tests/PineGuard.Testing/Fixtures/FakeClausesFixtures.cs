using PineGuard.Testing.UnitTests.Rules;

namespace PineGuard.Testing.Fixtures;

/// <summary>
/// Boundary C ("the fixture is where the wrong name is born"): the scenario
/// tuple's first element is <c>lower</c>, a renamed form of
/// <c>FakeClauses.IsBetween</c>'s real <c>min</c> parameter. It is valid
/// camelCase, so only the exact-name check may fire — and it must fire in a
/// fixture file, not just in <c>*TestData.cs</c>, because root spec §9.3 and
/// fixture.md §11.4 apply §4.3 here verbatim. Resolution goes through the
/// <c>Fixtures</c> suffix (<c>FakeClausesFixtures</c> -> <c>FakeClauses</c>),
/// the mirror of the <c>TestData</c> suffix used in the layer projects.
/// </summary>
public static class FakeClausesFixtures
{
    public static class IsBetween
    {
        public static RuleScenario<(int value, int lower, int max)>[] ValidScenarios =>
        [
            new("inside", (value: 5, lower: 0, max: 10), true),
        ];
    }
}
