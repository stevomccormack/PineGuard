using PineGuard.Testing.UnitTests.Rules;

namespace PineGuard.Testing.Fixtures;

public static class DefaultEqualityRulesFixtures
{
    public static class IsDefaultInt32
    {
        public static readonly int Default = 0;
        public static readonly int Positive = 1;
        public static readonly int Negative = -1;

        public static RuleScenario<int>[] ValidScenarios =>
        [
            new(nameof(Default), Default, true)
        ];

        public static RuleScenario<int>[] InvalidScenarios =>
        [
            new(nameof(Positive), Positive, false),
            new(nameof(Negative), Negative, false)
        ];

        public static RuleScenario<int>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsDefaultNullableInt32
    {
        public static readonly int? Null = null;
        public static readonly int? Zero = 0;
        public static readonly int? One = 1;

        public static RuleScenario<int?>[] ValidScenarios =>
        [
            new(nameof(Null), Null, true)
        ];

        public static RuleScenario<int?>[] InvalidScenarios =>
        [
            new(nameof(Zero), Zero, false),
            new(nameof(One),  One,  false)
        ];

        public static RuleScenario<int?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsDefaultString
    {
        public static readonly string? Null = null;
        public static readonly string? Empty = string.Empty;
        public static readonly string? Whitespace = " ";
        public static readonly string? Text = "abc";

        public static RuleScenario<string?>[] ValidScenarios =>
        [
            new(nameof(Null), Null, true)
        ];

        public static RuleScenario<string?>[] InvalidScenarios =>
        [
            new(nameof(Empty),      Empty,      false),
            new(nameof(Whitespace), Whitespace, false),
            new(nameof(Text),       Text,       false)
        ];

        public static RuleScenario<string?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsNullOrDefaultInt32
    {
        public static readonly int Default = 0;
        public static readonly int NonDefault = 1;

        public static RuleScenario<int>[] ValidScenarios =>
        [
            new(nameof(Default), Default, true)
        ];

        public static RuleScenario<int>[] InvalidScenarios =>
        [
            new(nameof(NonDefault), NonDefault, false)
        ];

        public static RuleScenario<int>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsNullOrDefaultNullableInt32
    {
        public static readonly int? Null = null;
        public static readonly int? Zero = 0;
        public static readonly int? One = 1;

        public static RuleScenario<int?>[] ValidScenarios =>
        [
            new(nameof(Null), Null, true),
            new(nameof(Zero), Zero, true)
        ];

        public static RuleScenario<int?>[] InvalidScenarios =>
        [
            new(nameof(One), One, false)
        ];

        public static RuleScenario<int?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsNullOrDefaultString
    {
        public static readonly string? Null = null;
        public static readonly string? Empty = string.Empty;
        public static readonly string? Text = "abc";

        public static RuleScenario<string?>[] ValidScenarios =>
        [
            new(nameof(Null), Null, true)
        ];

        public static RuleScenario<string?>[] InvalidScenarios =>
        [
            new(nameof(Empty), Empty, false),
            new(nameof(Text),  Text,  false)
        ];

        public static RuleScenario<string?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }
}
