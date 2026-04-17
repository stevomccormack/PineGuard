using PineGuard.Testing.UnitTests.Rules;

namespace PineGuard.Testing.Fixtures;

public static class GuidRulesFixtures
{
    public static class IsEmpty
    {
        public static readonly Guid? Null = null;
        public static readonly Guid? Empty = Guid.Empty;
        public static readonly Guid? NonEmpty = Guid.Parse("11111111-1111-1111-1111-111111111111");

        public static RuleScenario<Guid?>[] ValidScenarios =>
        [
            new(nameof(Null),  Null,  true),
            new(nameof(Empty), Empty, true)
        ];

        public static RuleScenario<Guid?>[] InvalidScenarios =>
        [
            new(nameof(NonEmpty), NonEmpty, false)
        ];

        public static RuleScenario<Guid?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsNotEmpty
    {
        public static readonly Guid? Null = null;
        public static readonly Guid? Empty = Guid.Empty;
        public static readonly Guid? NonEmpty = Guid.Parse("11111111-1111-1111-1111-111111111111");

        public static RuleScenario<Guid?>[] ValidScenarios =>
        [
            new(nameof(NonEmpty), NonEmpty, true)
        ];

        public static RuleScenario<Guid?>[] InvalidScenarios =>
        [
            new(nameof(Null),  Null,  false),
            new(nameof(Empty), Empty, false)
        ];

        public static RuleScenario<Guid?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class NotEmpty
    {
        public static readonly Guid NonEmpty = Guid.Parse("11111111-1111-1111-1111-111111111111");
        public static readonly Guid Empty = Guid.Empty;

        public static RuleScenario<Guid>[] ValidScenarios =>
        [
            new(nameof(NonEmpty), NonEmpty, true)
        ];

        public static RuleScenario<Guid>[] InvalidScenarios =>
        [
            new(nameof(Empty), Empty, false)
        ];

        public static RuleScenario<Guid>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }
}
