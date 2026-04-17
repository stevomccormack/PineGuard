using System.Reflection;
using PineGuard.Testing.Fixtures;

namespace PineGuard.Testing.UnitTests.Fixtures;

public sealed class FixtureSmokeTests
{
    public static class InitializeOps
    {
        [Theory]
        [MemberData(nameof(FixtureSmokeTestData.InitializeOps.ValidCases), MemberType = typeof(FixtureSmokeTestData.InitializeOps))]
        public static void ShouldInitializeWithoutErrors(FixtureSmokeTestData.InitializeOps.Case testCase)
        {
            var type = typeof(BoolRulesFixtures).Assembly.GetType(testCase.Name)!;
            var exception = Record.Exception(() =>
            {
                foreach (var field in type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly))
                    _ = field.GetValue(null);

                foreach (var prop in type.GetProperties(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly))
                    _ = prop.GetValue(null);
            });

            Assert.Null(exception);
        }
    }

    public static class DictionaryPredicateOps
    {
        [Theory]
        [MemberData(nameof(FixtureSmokeTestData.DictionaryPredicateOps.ValidCases), MemberType = typeof(FixtureSmokeTestData.DictionaryPredicateOps))]
        public static void ShouldExercisePredicateBranches(FixtureSmokeTestData.DictionaryPredicateOps.Case testCase)
        {
            _ = testCase;

            // DictionaryRulesFixtures.HasAnyItem: (k, v) => k == "a" && v == 1
            var dictPredicate = DictionaryRulesFixtures.HasAnyItem.MatchingItem.predicate;
            Assert.True(dictPredicate("a", 1));   // true && true
            Assert.False(dictPredicate("x", 1));  // false (short-circuit)

            // ReadOnlyDictionaryRulesFixtures.HasAnyItem: (k, v) => k == "a" && v == 1
            var roDictPredicate = ReadOnlyDictionaryRulesFixtures.HasAnyItem.MatchingItem.predicate;
            Assert.True(roDictPredicate("a", 1));
            Assert.False(roDictPredicate("x", 1));
        }
    }
}
