using PineGuard.Testing.UnitTests.Rules;

namespace PineGuard.Testing.Fixtures;

/// <summary>
/// A conforming fixture file. Root spec §9.3 and fixture.md §11.4 apply §4.3's
/// camelCase + exact-parameter-name rule to fixtures too — both the
/// <c>public static readonly</c> tuple field and the
/// <c>RuleScenario&lt;(…)&gt;</c> array must carry
/// <c>FakeClauses.IsBetween</c>'s real parameter names (value, min, max).
/// Zero findings expected.
/// </summary>
public static class FakeClausesFixtures
{
    public static class IsBetween
    {
        public static readonly (int value, int min, int max) Inside = (5, 0, 10);

        public static RuleScenario<(int value, int min, int max)>[] ValidScenarios =>
        [
            new(nameof(Inside), Inside, true),
        ];
    }
}
