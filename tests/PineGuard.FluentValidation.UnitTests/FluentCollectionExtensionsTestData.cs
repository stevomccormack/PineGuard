using PineGuard.Common;
using PineGuard.Testing.UnitTests.FluentValidation;
using PineGuard.Testing.UnitTests.Rules;
using F = PineGuard.Testing.Fixtures.CollectionRulesFixtures;

namespace PineGuard.FluentValidation.UnitTests;

public static class FluentCollectionExtensionsTestData
{
    public static class Empty
    {
        public static TheoryData<FluentCase<IEnumerable<string>>> Cases => F.IsEmpty.AllScenarios.ToFluentCases(s => s.Name switch
        {
            "Empty" => new FluentExpected(true),
            "Null" => new FluentExpected(false, "Value must not be null."),
            _ => new FluentExpected(false, "Value must be empty.")
        });
    }

    public static class NotEmpty
    {
        public static TheoryData<FluentCase<IEnumerable<string>>> Cases => F.IsNotEmpty.AllScenarios.ToFluentCases(s => s.Name switch
        {
            _ when s.IsValid => new FluentExpected(true),
            "Null" => new FluentExpected(false, "Value must not be null."),
            _ => new FluentExpected(false, "Value must not be empty.")
        });
    }

    public static class HasExactCount
    {
        public static TheoryData<FluentCase<(IEnumerable<string>? value, int count)>> Cases => F.HasExactCount.AllScenarios.ToFluentCases(s => s.Name switch
        {
            _ when s.IsValid => new FluentExpected(true),
            nameof(F.HasExactCount.NullThree) => new FluentExpected(false, "Value must not be null."),
            nameof(F.HasExactCount.SingleNeg) => new FluentExpected(false, "count requires a non-negative count."),
            _ => new FluentExpected(false, "Value must have the expected count.")
        });
    }

    public static class NotHasExactCount
    {
        public static TheoryData<FluentCase<(IEnumerable<string>? value, int count)>> Cases => F.HasExactCount.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.HasExactCount.MultipleThree) => new FluentExpected(false, "Value must not have the expected count."),
            nameof(F.HasExactCount.NullThree) => new FluentExpected(false, "Value must not be null."),
            nameof(F.HasExactCount.SingleNeg) => new FluentExpected(false, "count requires a non-negative count."),
            _ => new FluentExpected(true)
        });
    }

    public static class HasMinCount
    {
        public static TheoryData<FluentCase<(IEnumerable<string>? value, int min)>> Cases => F.HasMinCount.AllScenarios.ToFluentCases(s => s.Name switch
        {
            _ when s.IsValid => new FluentExpected(true),
            nameof(F.HasMinCount.NullOne) => new FluentExpected(false, "Value must not be null."),
            _ => new FluentExpected(false, "Value must have at least the minimum count.")
        });
    }

    public static class NotHasMinCount
    {
        public static TheoryData<FluentCase<(IEnumerable<string>? value, int min)>> Cases => F.HasMinCount.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.HasMinCount.MultipleTwo) => new FluentExpected(false, "Value must not have at least the minimum count."),
            nameof(F.HasMinCount.MultipleThree) => new FluentExpected(false, "Value must not have at least the minimum count."),
            nameof(F.HasMinCount.NullOne) => new FluentExpected(false, "Value must not be null."),
            _ => new FluentExpected(true)
        });
    }

    public static class HasMaxCount
    {
        public static TheoryData<FluentCase<(IEnumerable<string>? value, int max)>> Cases => F.HasMaxCount.AllScenarios.ToFluentCases(s => s.Name switch
        {
            _ when s.IsValid => new FluentExpected(true),
            nameof(F.HasMaxCount.NullThree) => new FluentExpected(false, "Value must not be null."),
            _ => new FluentExpected(false, "Value must have at most the maximum count.")
        });
    }

    public static class NotHasMaxCount
    {
        public static TheoryData<FluentCase<(IEnumerable<string>? value, int max)>> Cases => F.HasMaxCount.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.HasMaxCount.MultipleThree) => new FluentExpected(false, "Value must not have at most the maximum count."),
            nameof(F.HasMaxCount.MultipleFive) => new FluentExpected(false, "Value must not have at most the maximum count."),
            nameof(F.HasMaxCount.NullThree) => new FluentExpected(false, "Value must not be null."),
            _ => new FluentExpected(true)
        });
    }

    public static class HasCountBetween
    {
        public static TheoryData<FluentCase<(IEnumerable<string>? value, int min, int max, Inclusion inclusion)>> Cases => F.HasCountBetween.AllScenarios.ToFluentCases(s => s.Name switch
        {
            _ when s.IsValid => new FluentExpected(true),
            nameof(F.HasCountBetween.NullTwoFourInclusive) => new FluentExpected(false, "Value must not be null."),
            _ => new FluentExpected(false, "Value must have a count within the expected range.")
        });
    }

    public static class NotHasCountBetween
    {
        public static TheoryData<FluentCase<(IEnumerable<string>? value, int min, int max, Inclusion inclusion)>> Cases => F.HasCountBetween.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.HasCountBetween.MultipleTwoFourInclusive) => new FluentExpected(false, "Value must not have a count within the expected range."),
            nameof(F.HasCountBetween.MultipleThreeThreeInclusive) => new FluentExpected(false, "Value must not have a count within the expected range."),
            nameof(F.HasCountBetween.NullTwoFourInclusive) => new FluentExpected(false, "Value must not be null."),
            _ => new FluentExpected(true)
        });
    }

    public static class HasDistinctItems
    {
        public static TheoryData<FluentCase<IEnumerable<string>>> Cases => F.HasDistinctItems.AllScenarios.ToFluentCases(s => s.Name switch
        {
            _ when s.IsValid => new FluentExpected(true),
            "Null" => new FluentExpected(false, "Value must not be null."),
            _ => new FluentExpected(false, "Value must have distinct items.")
        });
    }

    public static class HasDuplicateItems
    {
        public static TheoryData<FluentCase<IEnumerable<string>>> Cases => F.HasDuplicateItems.AllScenarios.ToFluentCases(s => s.Name switch
        {
            _ when s.IsValid => new FluentExpected(true),
            "Null" => new FluentExpected(false, "Value must not be null."),
            _ => new FluentExpected(false, "Value must have duplicate items.")
        });
    }

    public static class NotContainsNullItems
    {
        public static TheoryData<FluentCase<IEnumerable<string?>>> Cases => F.ContainsNullItems.AllScenarios.ToFluentCases(s => s.Name switch
        {
            "WithNull" => new FluentExpected(false, "Value must not contain any null items."),
            "NullCollection" => new FluentExpected(false, "Value must not be null."),
            _ => new FluentExpected(true)
        });
    }

    public static class Contains
    {
        public static TheoryData<FluentCase<(IEnumerable<string>? value, string item)>> Cases => F.Contains.AllScenarios.ToFluentCases(s => s.Name switch
        {
            _ when s.IsValid => new FluentExpected(true),
            nameof(F.Contains.NullA) => new FluentExpected(false, "Value must not be null."),
            _ => new FluentExpected(false, "Value must contain the specified item.")
        });
    }

    public static class NotContains
    {
        public static TheoryData<FluentCase<(IEnumerable<string>? value, string item)>> Cases => F.Contains.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.Contains.MultipleA) => new FluentExpected(false, "Value must not contain the specified item."),
            nameof(F.Contains.NullA) => new FluentExpected(false, "Value must not be null."),
            _ => new FluentExpected(true)
        });
    }

    public static class SubsetOf
    {
        public static TheoryData<FluentCase<(IEnumerable<string>? value, IEnumerable<string>? other)>> Cases => F.IsSubsetOf.AllScenarios.ToFluentCases(s => s.Name switch
        {
            _ when s.IsValid => new FluentExpected(true),
            nameof(F.IsSubsetOf.NullMultiple) => new FluentExpected(false, "Value must not be null."),
            nameof(F.IsSubsetOf.MultipleNull) => new FluentExpected(false, "other must not be null."),
            _ => new FluentExpected(false, "Value must be a subset of the other collection.")
        });
    }

    public static class NotSubsetOf
    {
        public static TheoryData<FluentCase<(IEnumerable<string>? value, IEnumerable<string>? other)>> Cases => F.IsSubsetOf.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsSubsetOf.SingleMultiple) => new FluentExpected(false, "Value must not be a subset of the other collection."),
            nameof(F.IsSubsetOf.NullMultiple) => new FluentExpected(false, "Value must not be null."),
            nameof(F.IsSubsetOf.MultipleNull) => new FluentExpected(false, "other must not be null."),
            _ => new FluentExpected(true)
        });
    }

    public static class HasIndex
    {
        public static TheoryData<FluentCase<(IEnumerable<string>? value, int index)>> Cases => F.HasIndex.AllScenarios.ToFluentCases(s => s.Name switch
        {
            _ when s.IsValid => new FluentExpected(true),
            nameof(F.HasIndex.NullZero) => new FluentExpected(false, "Value must not be null."),
            nameof(F.HasIndex.MultipleNeg) => new FluentExpected(false, "index requires a non-negative index."),
            _ => new FluentExpected(false, "Value must have an item at the specified index.")
        });
    }

    public static class NotHasIndex
    {
        public static TheoryData<FluentCase<(IEnumerable<string>? value, int index)>> Cases => F.HasIndex.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.HasIndex.MultipleZero) => new FluentExpected(false, "Value must not have an item at the specified index."),
            nameof(F.HasIndex.MultipleTwo) => new FluentExpected(false, "Value must not have an item at the specified index."),
            nameof(F.HasIndex.NullZero) => new FluentExpected(false, "Value must not be null."),
            nameof(F.HasIndex.MultipleNeg) => new FluentExpected(false, "index requires a non-negative index."),
            _ => new FluentExpected(true)
        });
    }

    public static class HasAny
    {
        private static readonly RuleScenario<(IEnumerable<string>? value, Func<string, bool> predicate)>[] Scenarios =
        [
            new("MatchingItem", (["a", "b"], s => s == "a"), true),
            new("NoMatch", (["x", "y"], s => s == "a"), false),
            new("NullValue", (null!, s => s == "a"), false)
        ];

        public static TheoryData<FluentCase<(IEnumerable<string>? value, Func<string, bool> predicate)>> Cases => Scenarios.ToFluentCases(s => s.Name switch
        {
            "MatchingItem" => new FluentExpected(true),
            "NullValue" => new FluentExpected(false, "Value must not be null."),
            _ => new FluentExpected(false, "Value must contain an item that matches the predicate.")
        });
    }

    public static class NotHasAny
    {
        private static readonly RuleScenario<(IEnumerable<string>? value, Func<string, bool> predicate)>[] Scenarios =
        [
            new("NoMatch", (["x", "y"], s => s == "a"), true),
            new("MatchingItem", (["a", "b"], s => s == "a"), false),
            new("NullValue", (null!, s => s == "a"), false)
        ];

        public static TheoryData<FluentCase<(IEnumerable<string>? value, Func<string, bool> predicate)>> Cases => Scenarios.ToFluentCases(s => s.Name switch
        {
            "NoMatch" => new FluentExpected(true),
            "NullValue" => new FluentExpected(false, "Value must not be null."),
            _ => new FluentExpected(false, "Value must not contain an item that matches the predicate.")
        });
    }

    public static class HasAll
    {
        private static readonly RuleScenario<(IEnumerable<string>? value, Func<string, bool> predicate)>[] Scenarios =
        [
            new("AllMatch", (["a", "a"], s => s == "a"), true),
            new("PartialMatch", (["a", "b"], s => s == "a"), false),
            new("NullValue", (null!, s => s == "a"), false)
        ];

        public static TheoryData<FluentCase<(IEnumerable<string>? value, Func<string, bool> predicate)>> Cases => Scenarios.ToFluentCases(s => s.Name switch
        {
            "AllMatch" => new FluentExpected(true),
            "NullValue" => new FluentExpected(false, "Value must not be null."),
            _ => new FluentExpected(false, "Value must have all items match the predicate.")
        });
    }

    public static class NotHasAll
    {
        private static readonly RuleScenario<(IEnumerable<string>? value, Func<string, bool> predicate)>[] Scenarios =
        [
            new("PartialMatch", (["a", "b"], s => s == "a"), true),
            new("AllMatch", (["a", "a"], s => s == "a"), false),
            new("NullValue", (null!, s => s == "a"), false)
        ];

        public static TheoryData<FluentCase<(IEnumerable<string>? value, Func<string, bool> predicate)>> Cases => Scenarios.ToFluentCases(s => s.Name switch
        {
            "PartialMatch" => new FluentExpected(true),
            "NullValue" => new FluentExpected(false, "Value must not be null."),
            _ => new FluentExpected(false, "Value must not have all items match the predicate.")
        });
    }
}
