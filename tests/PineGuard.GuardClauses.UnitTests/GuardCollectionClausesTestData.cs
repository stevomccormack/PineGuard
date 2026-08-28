using PineGuard.Common;
using PineGuard.Testing.UnitTests.GuardClauses;
using F = PineGuard.Testing.Fixtures.CollectionRulesFixtures;

namespace PineGuard.GuardClauses.UnitTests;

public static class GuardCollectionClausesTestData
{
    // Guard.Against.Empty — calls Must.Be.NotEmpty — throws when empty or null
    public static class Empty
    {
        public static TheoryData<GuardCase<IEnumerable<string>>> ValidCases => F.IsNotEmpty.ValidScenarios.ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<IEnumerable<string>>> InvalidCases => F.IsNotEmpty.InvalidScenarios.ToGuardCases("value");
    }

    // Guard.Against.NotEmpty — calls Must.Be.Empty — throws when not empty
    public static class NotEmpty
    {
        public static TheoryData<GuardCase<IEnumerable<string>>> ValidCases => F.IsEmpty.ValidScenarios.ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<IEnumerable<string>>> InvalidCases => F.IsEmpty.InvalidScenarios.ToGuardCases("value");
    }

    // Guard.Against.NotHasExactCount — calls Must.Be.HasExactCount — throws when does NOT have exact count
    // Must.Be.HasExactCount checks its own preconditions: a null value attributes to "value", but a
    // negative count is its own precondition and attributes to "count" (see MustCollectionClauses.HasExactCount).
    public static class NotHasExactCount
    {
        public static TheoryData<GuardCase<(IEnumerable<string>? value, int count)>> ValidCases => F.HasExactCount.ValidScenarios.ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<(IEnumerable<string>? value, int count)>> InvalidCases => F.HasExactCount.InvalidScenarios.ToGuardCases(s => s.Inputs.value == null
            ? new GuardExpected(false, typeof(ArgumentNullException), "value")
            : s.Inputs.count < 0
                ? new GuardExpected(false, typeof(ArgumentException), "count")
                : new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    // Guard.Against.HasExactCount — calls Must.Be.NotHasExactCount — throws when HAS exact count
    // Null value and negative count cause pre-condition throws; only non-null, non-negative-count cases pass.
    // The negative-count precondition attributes to "count", not "value" (see MustCollectionClauses.NotHasExactCount).
    public static class HasExactCount
    {
        public static TheoryData<GuardCase<(IEnumerable<string>? value, int count)>> ValidCases =>
        [
            new(nameof(F.HasExactCount.EmptyThree),   F.HasExactCount.EmptyThree,   new GuardExpected(true)),
            new(nameof(F.HasExactCount.MultipleTwo),  F.HasExactCount.MultipleTwo,  new GuardExpected(true))
        ];
        public static TheoryData<GuardCase<(IEnumerable<string>? value, int count)>> InvalidCases =>
        [
            .. F.HasExactCount.ValidScenarios.ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value")),
            new(nameof(F.HasExactCount.NullThree), F.HasExactCount.NullThree, new GuardExpected(false, typeof(ArgumentNullException), "value")),
            new(nameof(F.HasExactCount.SingleNeg), F.HasExactCount.SingleNeg, new GuardExpected(false, typeof(ArgumentException), "count"))
        ];
    }

    // Guard.Against.NotHasMinCount — calls Must.Be.HasMinCount — throws when does NOT have min count
    public static class NotHasMinCount
    {
        public static TheoryData<GuardCase<(IEnumerable<string>? value, int min)>> ValidCases => F.HasMinCount.ValidScenarios.ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<(IEnumerable<string>? value, int min)>> InvalidCases => F.HasMinCount.InvalidScenarios.ToGuardCases(s => s.Inputs.value == null ? new GuardExpected(false, typeof(ArgumentNullException), "value") : new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    // Guard.Against.HasMinCount — calls Must.Be.NotHasMinCount — throws when HAS min count
    // Null value causes pre-condition throw; only non-null cases that fail NotHasMinCount truly pass
    public static class HasMinCount
    {
        public static TheoryData<GuardCase<(IEnumerable<string>? value, int min)>> ValidCases =>
        [
            new(nameof(F.HasMinCount.EmptyOne),    F.HasMinCount.EmptyOne,    new GuardExpected(true)),
            new(nameof(F.HasMinCount.MultipleFour), F.HasMinCount.MultipleFour, new GuardExpected(true))
        ];
        public static TheoryData<GuardCase<(IEnumerable<string>? value, int min)>> InvalidCases =>
        [
            .. F.HasMinCount.ValidScenarios.ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value")),
            new(nameof(F.HasMinCount.NullOne), F.HasMinCount.NullOne, new GuardExpected(false, typeof(ArgumentNullException), "value"))
        ];
    }

    // Guard.Against.NotHasMaxCount — calls Must.Be.HasMaxCount — throws when does NOT have max count
    public static class NotHasMaxCount
    {
        public static TheoryData<GuardCase<(IEnumerable<string>? value, int max)>> ValidCases => F.HasMaxCount.ValidScenarios.ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<(IEnumerable<string>? value, int max)>> InvalidCases => F.HasMaxCount.InvalidScenarios.ToGuardCases(s => s.Inputs.value == null ? new GuardExpected(false, typeof(ArgumentNullException), "value") : new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    // Guard.Against.HasMaxCount — calls Must.Be.NotHasMaxCount — throws when HAS max count
    // Null value causes pre-condition throw; only non-null cases that fail NotHasMaxCount truly pass
    public static class HasMaxCount
    {
        public static TheoryData<GuardCase<(IEnumerable<string>? value, int max)>> ValidCases =>
        [
            new(nameof(F.HasMaxCount.MultipleTwo), F.HasMaxCount.MultipleTwo, new GuardExpected(true))
        ];
        public static TheoryData<GuardCase<(IEnumerable<string>? value, int max)>> InvalidCases =>
        [
            .. F.HasMaxCount.ValidScenarios.ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value")),
            new(nameof(F.HasMaxCount.NullThree), F.HasMaxCount.NullThree, new GuardExpected(false, typeof(ArgumentNullException), "value"))
        ];
    }

    // Guard.Against.NotHasCountBetween — calls Must.Be.HasCountBetween — throws when count is NOT in range
    public static class NotHasCountBetween
    {
        public static TheoryData<GuardCase<(IEnumerable<string>? value, int min, int max, Inclusion inclusion)>> ValidCases => F.HasCountBetween.ValidScenarios.ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<(IEnumerable<string>? value, int min, int max, Inclusion inclusion)>> InvalidCases => F.HasCountBetween.InvalidScenarios.ToGuardCases(s => s.Inputs.value == null ? new GuardExpected(false, typeof(ArgumentNullException), "value") : new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    // Guard.Against.HasCountBetween — calls Must.Be.NotHasCountBetween — throws when count IS in range
    // Null value causes pre-condition throw; only non-null cases that fail NotHasCountBetween truly pass
    public static class HasCountBetween
    {
        public static TheoryData<GuardCase<(IEnumerable<string>? value, int min, int max, Inclusion inclusion)>> ValidCases =>
        [
            new(nameof(F.HasCountBetween.MultipleFourSixInclusive), F.HasCountBetween.MultipleFourSixInclusive, new GuardExpected(true))
        ];
        public static TheoryData<GuardCase<(IEnumerable<string>? value, int min, int max, Inclusion inclusion)>> InvalidCases =>
        [
            .. F.HasCountBetween.ValidScenarios.ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value")),
            new(nameof(F.HasCountBetween.NullTwoFourInclusive), F.HasCountBetween.NullTwoFourInclusive, new GuardExpected(false, typeof(ArgumentNullException), "value"))
        ];
    }

    // Guard.Against.DuplicateItems — calls Must.Be.HasDistinctItems — throws when has duplicates
    public static class DuplicateItems
    {
        public static TheoryData<GuardCase<IEnumerable<string>>> ValidCases => F.HasDistinctItems.ValidScenarios.ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<IEnumerable<string>>> InvalidCases => F.HasDistinctItems.InvalidScenarios.ToGuardCases("value");
    }

    // Guard.Against.DistinctItems — calls Must.Be.HasDuplicateItems — throws when all distinct
    public static class DistinctItems
    {
        public static TheoryData<GuardCase<IEnumerable<string>>> ValidCases => F.HasDuplicateItems.ValidScenarios.ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<IEnumerable<string>>> InvalidCases => F.HasDuplicateItems.InvalidScenarios.ToGuardCases("value");
    }

    // Guard.Against.ContainsNullItems — calls Must.Be.NotContainsNullItems — throws when has null items
    // Guard passes when collection has NO null items; throws for null collection or collection with null items
    public static class ContainsNullItems
    {
        public static TheoryData<GuardCase<IEnumerable<string?>>> ValidCases =>
        [
            new("NoNull", ["a", "b"], new GuardExpected(true))
        ];
        public static TheoryData<GuardCase<IEnumerable<string?>>> InvalidCases =>
        [
            new("NullCollection", null!, new GuardExpected(false, typeof(ArgumentNullException), "value")),
            new("WithNull", ["a", null], new GuardExpected(false, typeof(ArgumentException), "value"))
        ];
    }

    // Guard.Against.NotContains — calls Must.Be.Contains — throws when does NOT contain item
    public static class NotContains
    {
        public static TheoryData<GuardCase<(IEnumerable<string>? value, string item)>> ValidCases => F.Contains.ValidScenarios.ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<(IEnumerable<string>? value, string item)>> InvalidCases => F.Contains.InvalidScenarios.ToGuardCases(s => s.Inputs.value == null ? new GuardExpected(false, typeof(ArgumentNullException), "value") : new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    // Guard.Against.Contains — calls Must.Be.NotContains — throws when DOES contain item
    // Null value causes pre-condition throw; only non-null cases that fail NotContains truly pass
    public static class Contains
    {
        public static TheoryData<GuardCase<(IEnumerable<string>? value, string item)>> ValidCases =>
        [
            new(nameof(F.Contains.EmptyA),   F.Contains.EmptyA,   new GuardExpected(true)),
            new(nameof(F.Contains.MultipleZ), F.Contains.MultipleZ, new GuardExpected(true))
        ];
        public static TheoryData<GuardCase<(IEnumerable<string>? value, string item)>> InvalidCases =>
        [
            .. F.Contains.ValidScenarios.ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value")),
            new(nameof(F.Contains.NullA), F.Contains.NullA, new GuardExpected(false, typeof(ArgumentNullException), "value"))
        ];
    }

    // Guard.Against.NotSubsetOf — calls Must.Be.SubsetOf — throws when NOT a subset
    // Must.Be.SubsetOf checks "other" for null as its own precondition, distinct from a null "value"
    // (see MustCollectionClauses.SubsetOf).
    public static class NotSubsetOf
    {
        public static TheoryData<GuardCase<(IEnumerable<string>? value, IEnumerable<string>? other)>> ValidCases => F.IsSubsetOf.ValidScenarios.ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<(IEnumerable<string>? value, IEnumerable<string>? other)>> InvalidCases => F.IsSubsetOf.InvalidScenarios.ToGuardCases(s => s.Inputs.value == null
            ? new GuardExpected(false, typeof(ArgumentNullException), "value")
            : s.Inputs.other == null
                ? new GuardExpected(false, typeof(ArgumentNullException), "other")
                : new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    // Guard.Against.SubsetOf — calls Must.Be.NotSubsetOf — throws when IS a subset
    // Null value/other cause pre-condition throws; only cases that truly fail NotSubsetOf pass.
    // A null "other" attributes to "other", not "value" (see MustCollectionClauses.NotSubsetOf).
    public static class SubsetOf
    {
        public static TheoryData<GuardCase<(IEnumerable<string>? value, IEnumerable<string>? other)>> ValidCases =>
        [
            new(nameof(F.IsSubsetOf.ZMultiple), F.IsSubsetOf.ZMultiple, new GuardExpected(true))
        ];
        public static TheoryData<GuardCase<(IEnumerable<string>? value, IEnumerable<string>? other)>> InvalidCases =>
        [
            .. F.IsSubsetOf.ValidScenarios.ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value")),
            new(nameof(F.IsSubsetOf.NullMultiple), F.IsSubsetOf.NullMultiple, new GuardExpected(false, typeof(ArgumentNullException), "value")),
            new(nameof(F.IsSubsetOf.MultipleNull), F.IsSubsetOf.MultipleNull, new GuardExpected(false, typeof(ArgumentNullException), "other"))
        ];
    }

    // Guard.Against.NotHasIndex — calls Must.Be.HasIndex — throws when does NOT have index
    // Must.Be.HasIndex checks its own preconditions: a null value attributes to "value", but a
    // negative index is its own precondition and attributes to "index" (see MustCollectionClauses.HasIndex).
    public static class NotHasIndex
    {
        public static TheoryData<GuardCase<(IEnumerable<string>? value, int index)>> ValidCases => F.HasIndex.ValidScenarios.ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<(IEnumerable<string>? value, int index)>> InvalidCases => F.HasIndex.InvalidScenarios.ToGuardCases(s => s.Inputs.value == null
            ? new GuardExpected(false, typeof(ArgumentNullException), "value")
            : s.Inputs.index < 0
                ? new GuardExpected(false, typeof(ArgumentException), "index")
                : new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    // Guard.Against.HasIndex — calls Must.Be.NotHasIndex — throws when HAS index
    // Null value and negative index cause pre-condition throws; only valid non-throwing pass cases remain.
    // The negative-index precondition attributes to "index", not "value" (see MustCollectionClauses.NotHasIndex).
    public static class HasIndex
    {
        public static TheoryData<GuardCase<(IEnumerable<string>? value, int index)>> ValidCases =>
        [
            new(nameof(F.HasIndex.EmptyZero),    F.HasIndex.EmptyZero,    new GuardExpected(true)),
            new(nameof(F.HasIndex.MultipleThree), F.HasIndex.MultipleThree, new GuardExpected(true))
        ];
        public static TheoryData<GuardCase<(IEnumerable<string>? value, int index)>> InvalidCases =>
        [
            .. F.HasIndex.ValidScenarios.ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value")),
            new(nameof(F.HasIndex.NullZero),   F.HasIndex.NullZero,   new GuardExpected(false, typeof(ArgumentNullException), "value")),
            new(nameof(F.HasIndex.MultipleNeg), F.HasIndex.MultipleNeg, new GuardExpected(false, typeof(ArgumentException), "index"))
        ];
    }

    // Guard.Against.NotHasAny — calls Must.Be.HasAny — throws when predicate matches NO items
    public static class NotHasAny
    {
        public static TheoryData<GuardCase<IEnumerable<string>?>> ValidCases =>
        [
            new("has-a", ["a", "b"], new GuardExpected(true))
        ];
        public static TheoryData<GuardCase<IEnumerable<string>?>> InvalidCases =>
        [
            new("no-a", ["b", "c"], new GuardExpected(false, typeof(ArgumentException), "value")),
            new("empty", [], new GuardExpected(false, typeof(ArgumentException), "value")),
            new("null", null, new GuardExpected(false, typeof(ArgumentNullException), "value"))
        ];

        public static Func<string, bool> Predicate { get; } = x => x == "a";
    }

    // Guard.Against.HasAny — calls Must.Be.NotHasAny — throws when predicate matches ANY items
    public static class HasAny
    {
        public static TheoryData<GuardCase<IEnumerable<string>?>> ValidCases =>
        [
            new("no-a", ["b", "c"], new GuardExpected(true)),
            new("empty", [], new GuardExpected(true))
        ];
        public static TheoryData<GuardCase<IEnumerable<string>?>> InvalidCases =>
        [
            new("has-a", ["a", "b"], new GuardExpected(false, typeof(ArgumentException), "value")),
            new("null", null, new GuardExpected(false, typeof(ArgumentNullException), "value"))
        ];

        public static Func<string, bool> Predicate { get; } = x => x == "a";
    }

    // Guard.Against.NotHasAll — calls Must.Be.HasAll — throws when predicate does NOT match all items
    // Empty collection: HasAll vacuously true → guard passes (ValidCase)
    public static class NotHasAll
    {
        public static TheoryData<GuardCase<IEnumerable<string>?>> ValidCases =>
        [
            new("all-a", ["a", "a"], new GuardExpected(true)),
            new("empty", [], new GuardExpected(true))
        ];
        public static TheoryData<GuardCase<IEnumerable<string>?>> InvalidCases =>
        [
            new("not-all-a", ["a", "b"], new GuardExpected(false, typeof(ArgumentException), "value")),
            new("null", null, new GuardExpected(false, typeof(ArgumentNullException), "value"))
        ];

        public static Func<string, bool> Predicate { get; } = x => x == "a";
    }

    // Guard.Against.HasAll — calls Must.Be.NotHasAll — throws when predicate matches ALL items
    // Empty collection: HasAll vacuously true → NotHasAll false → guard throws (InvalidCase)
    public static class HasAll
    {
        public static TheoryData<GuardCase<IEnumerable<string>?>> ValidCases =>
        [
            new("not-all-a", ["a", "b"], new GuardExpected(true))
        ];
        public static TheoryData<GuardCase<IEnumerable<string>?>> InvalidCases =>
        [
            new("all-a", ["a", "a"], new GuardExpected(false, typeof(ArgumentException), "value")),
            new("empty", [], new GuardExpected(false, typeof(ArgumentException), "value")),
            new("null", null, new GuardExpected(false, typeof(ArgumentNullException), "value"))
        ];

        public static Func<string, bool> Predicate { get; } = x => x == "a";
    }
}
