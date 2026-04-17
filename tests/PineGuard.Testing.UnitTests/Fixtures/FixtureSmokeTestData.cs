using PineGuard.Testing.Fixtures;

namespace PineGuard.Testing.UnitTests.Fixtures;

public static class FixtureSmokeTestData
{
    public static class InitializeOps
    {
        public sealed record Case(string Name) : BaseCase(Name);

        public static TheoryData<Case> ValidCases
        {
            get
            {
                var data = new TheoryData<Case>();
                foreach (var type in GetFixtureTypes())
                    data.Add(new Case(type.FullName!));
                return data;
            }
        }

        internal static IEnumerable<Type> GetFixtureTypes()
        {
            var assembly = typeof(BoolRulesFixtures).Assembly;
            return assembly.GetTypes()
                .Where(IsFixtureStaticClass)
                .OrderBy(t => t.FullName);
        }

        private static bool IsFixtureStaticClass(Type t)
        {
            if (!t.IsAbstract || !t.IsSealed) return false;
            var root = t;
            while (root.DeclaringType is not null) root = root.DeclaringType;
            return root.Namespace?.StartsWith("PineGuard.Testing.Fixtures", StringComparison.Ordinal) == true;
        }
    }

    public static class DictionaryPredicateOps
    {
        public sealed record Case(string Name) : BaseCase(Name);

        public static TheoryData<Case> ValidCases =>
        [
            new("exercise all dictionary predicate branches")
        ];
    }
}
