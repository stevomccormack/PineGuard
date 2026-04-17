using PineGuard.Testing.UnitTests.Rules;

namespace PineGuard.Testing.Fixtures;

public static class ObjectRulesFixtures
{
    public static class IsEqualTo
    {
        public static readonly (string? value, string? other) EqualStrings = ("abc", "abc");
        public static readonly (string? value, string? other) NotEqualStrings = ("abc", "def");
        public static readonly (string? value, string? other) BothNull = (null, null);

        public static RuleScenario<(string? value, string? other)>[] ValidScenarios =>
        [
            new(nameof(EqualStrings), EqualStrings, true),
            new(nameof(BothNull), BothNull, true)
        ];

        public static RuleScenario<(string? value, string? other)>[] InvalidScenarios =>
        [
            new(nameof(NotEqualStrings), NotEqualStrings, false)
        ];

        public static RuleScenario<(string? value, string? other)>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsOfType
    {
        public static readonly object? StringValue = "abc";
        public static readonly object? Null = null;
        public static readonly object? IntValue = 123;

        public static RuleScenario<object?>[] ValidScenarios =>
        [
            new(nameof(StringValue), StringValue, true)
        ];

        public static RuleScenario<object?>[] InvalidScenarios =>
        [
            new(nameof(Null), Null, false),
            new(nameof(IntValue), IntValue, false)
        ];

        public static RuleScenario<object?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsAssignableToType
    {
        public static readonly object? StringValue = "abc";
        public static readonly object? EmptyString = string.Empty;
        public static readonly object? Null = null;
        public static readonly object? IntValue = 123;
        public static readonly object? ObjectValue = new();

        public static RuleScenario<object?>[] ValidScenarios =>
        [
            new(nameof(StringValue), StringValue, true),
            new(nameof(EmptyString), EmptyString, true)
        ];

        public static RuleScenario<object?>[] InvalidScenarios =>
        [
            new(nameof(Null), Null, false),
            new(nameof(IntValue), IntValue, false),
            new(nameof(ObjectValue), ObjectValue, false)
        ];

        public static RuleScenario<object?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsSameReferenceAs
    {
        private static readonly object ObjectA = new();
        private static readonly object ObjectB = new();

        public static readonly (object? a, object? b) SameReference = (ObjectA, ObjectA);
        public static readonly (object? a, object? b) DifferentReference = (ObjectA, ObjectB);
        public static readonly (string? a, string? b) BothNull = (null, null);

        public static RuleScenario<(object? a, object? b)>[] ValidScenarios =>
        [
            new(nameof(SameReference), SameReference, true),
            new(nameof(BothNull), (null, null), true)
        ];

        public static RuleScenario<(object? a, object? b)>[] InvalidScenarios =>
        [
            new(nameof(DifferentReference), DifferentReference, false)
        ];

        public static RuleScenario<(object? a, object? b)>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }
}
