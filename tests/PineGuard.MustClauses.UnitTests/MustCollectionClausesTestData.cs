using PineGuard.Codes;
using PineGuard.Common;
using PineGuard.Testing.UnitTests.MustClauses;
using PineGuard.Testing.UnitTests.Rules;
using F = PineGuard.Testing.Fixtures.CollectionRulesFixtures;

namespace PineGuard.MustClauses.UnitTests;

public static class MustCollectionClausesTestData
{
    private static readonly string[] ArrayBacked = ["a", "b", "c"];
    private static readonly string[] ArrayBackedWithDuplicate = ["a", "b", "a"];
    private static readonly string[] ArrayBackedSubset = ["a", "b"];
    private static readonly string[] ArrayBackedDisjoint = ["z"];

    public static class Empty
    {
        public static TheoryData<MustCase<IEnumerable<string>>> ValidCases => F.IsEmpty.ValidScenarios.ToMustCases();

        public static TheoryData<MustCase<IEnumerable<string>>> InvalidCases => F.IsEmpty.InvalidScenarios.ToMustCases(s => s.Name switch
        {
            "Null" => new MustExpected(false, "value must not be null.", "value"),
            _ => new MustExpected(false, "value must be empty.", Code: MustCodes.Collection.Items.NotEmpty)
        });
    }

    public static class NotEmpty
    {
        public static TheoryData<MustCase<IEnumerable<string>>> ValidCases => F.IsNotEmpty.ValidScenarios.ToMustCases();

        public static TheoryData<MustCase<IEnumerable<string>>> InvalidCases => F.IsNotEmpty.InvalidScenarios.ToMustCases(s => s.Name switch
        {
            "Null" => new MustExpected(false, "value must not be null.", "value"),
            _ => new MustExpected(false, "value must not be empty.", Code: MustCodes.Collection.Items.Empty)
        });
    }

    public static class HasExactCount
    {
        public static TheoryData<MustCase<(IEnumerable<string>? value, int count)>> ValidCases => F.HasExactCount.ValidScenarios.ToMustCases();

        public static TheoryData<MustCase<(IEnumerable<string>? value, int count)>> InvalidCases => F.HasExactCount.InvalidScenarios.ToMustCases(s => s.Name switch
        {
            nameof(F.HasExactCount.NullThree) => new MustExpected(false, "value must not be null.", "value"),
            nameof(F.HasExactCount.SingleNeg) => new MustExpected(false, "count requires a non-negative count.", "count"),
            _ => new MustExpected(false, "value must have the expected count.", Code: MustCodes.Collection.Count.Mismatch)
        });
    }

    public static class HasMinCount
    {
        public static TheoryData<MustCase<(IEnumerable<string>? value, int min)>> ValidCases => F.HasMinCount.ValidScenarios.ToMustCases();

        public static TheoryData<MustCase<(IEnumerable<string>? value, int min)>> InvalidCases
        {
            get
            {
                var data = F.HasMinCount.InvalidScenarios.ToMustCases(s => s.Name switch
                {
                    nameof(F.HasMinCount.NullOne) => new MustExpected(false, "value must not be null.", "value"),
                    _ => new MustExpected(false, "value must have at least the minimum count.", Code: MustCodes.Collection.Count.TooFew)
                });
                data.Add(new MustCase<(IEnumerable<string>? value, int min)>("NegativeMin", (["a"], -1), new MustExpected(false, "min requires a non-negative minimum count.", "min")));
                return data;
            }
        }
    }

    public static class HasMaxCount
    {
        public static TheoryData<MustCase<(IEnumerable<string>? value, int max)>> ValidCases => F.HasMaxCount.ValidScenarios.ToMustCases();

        public static TheoryData<MustCase<(IEnumerable<string>? value, int max)>> EdgeCases =>
        [
            new(nameof(ArrayBacked), (ArrayBacked, 3), new MustExpected(true))
        ];

        public static TheoryData<MustCase<(IEnumerable<string>? value, int max)>> InvalidCases
        {
            get
            {
                var data = F.HasMaxCount.InvalidScenarios.ToMustCases(s => s.Name switch
                {
                    nameof(F.HasMaxCount.NullThree) => new MustExpected(false, "value must not be null.", "value"),
                    _ => new MustExpected(false, "value must have at most the maximum count.", Code: MustCodes.Collection.Count.TooMany)
                });
                data.Add(new MustCase<(IEnumerable<string>? value, int max)>("NegativeMax", (["a"], -1), new MustExpected(false, "max requires a non-negative maximum count.", "max")));
                return data;
            }
        }
    }

    public static class HasCountBetween
    {
        public static TheoryData<MustCase<(IEnumerable<string>? value, int min, int max, Inclusion inclusion)>> ValidCases => F.HasCountBetween.ValidScenarios.ToMustCases();

        public static TheoryData<MustCase<(IEnumerable<string>? value, int min, int max, Inclusion inclusion)>> EdgeCases =>
        [
            new(nameof(ArrayBacked), (ArrayBacked, 2, 4, Inclusion.Inclusive), new MustExpected(true))
        ];

        public static TheoryData<MustCase<(IEnumerable<string>? value, int min, int max, Inclusion inclusion)>> InvalidCases
        {
            get
            {
                var data = F.HasCountBetween.InvalidScenarios.ToMustCases(s => s.Name switch
                {
                    nameof(F.HasCountBetween.NullTwoFourInclusive) => new MustExpected(false, "value must not be null.", "value"),
                    _ => new MustExpected(false, "value must have a count within the expected range.", Code: MustCodes.Collection.Count.OutOfRange)
                });
                data.Add(new MustCase<(IEnumerable<string>? value, int min, int max, Inclusion inclusion)>("NegativeMin", (["a"], -1, 3, Inclusion.Inclusive), new MustExpected(false, "min requires a non-negative minimum count.", "min")));
                data.Add(new MustCase<(IEnumerable<string>? value, int min, int max, Inclusion inclusion)>("NegativeMax", (["a"], 0, -1, Inclusion.Inclusive), new MustExpected(false, "max requires a non-negative maximum count.", "max")));
                data.Add(new MustCase<(IEnumerable<string>? value, int min, int max, Inclusion inclusion)>("MinGtMax", (["a"], 4, 2, Inclusion.Inclusive), new MustExpected(false, "min requires a valid count range.", "min")));
                return data;
            }
        }
    }

    public static class HasAny
    {
        private static readonly Func<string, bool> IsA = x => x == "a";

        public static TheoryData<MustCase<(IEnumerable<string>? value, Func<string, bool>? predicate)>> ValidCases =>
        [
            new("Multiple", (["a", "b"], IsA), new MustExpected(true))
        ];

        public static TheoryData<MustCase<(IEnumerable<string>? value, Func<string, bool>? predicate)>> InvalidCases =>
        [
            new("Null", (null, IsA), new MustExpected(false, "value must not be null.", "value")),
            new("Empty", ([], IsA), new MustExpected(false, "value must contain an item that matches the predicate.")),
            new("No match", (["b", "c"], IsA), new MustExpected(false, "value must contain an item that matches the predicate.", Code: MustCodes.Collection.Items.NoMatch)),
            new("Null predicate", (["a"], null), new MustExpected(false, "predicate must not be null.", "predicate"))
        ];
    }

    public static class NotHasAny
    {
        private static readonly Func<string, bool> IsA = x => x == "a";

        public static TheoryData<MustCase<(IEnumerable<string>? value, Func<string, bool>? predicate)>> ValidCases =>
        [
            new("No match", (["b", "c"], IsA), new MustExpected(true)),
            new("Empty", ([], IsA), new MustExpected(true))
        ];

        public static TheoryData<MustCase<(IEnumerable<string>? value, Func<string, bool>? predicate)>> InvalidCases =>
        [
            new("Null", (null, IsA), new MustExpected(false, "value must not be null.", "value")),
            new("Has match", (["a", "b"], IsA), new MustExpected(false, "value must not contain an item that matches the predicate.", Code: MustCodes.Collection.Items.Match)),
            new("Null predicate", (["a"], null), new MustExpected(false, "predicate must not be null.", "predicate"))
        ];
    }

    public static class HasAll
    {
        private static readonly Func<string, bool> IsA = x => x == "a";

        public static TheoryData<MustCase<(IEnumerable<string>? value, Func<string, bool>? predicate)>> ValidCases =>
        [
            new("All a", (["a", "a"], IsA), new MustExpected(true)),
            new("Empty", ([], IsA), new MustExpected(true))
        ];

        public static TheoryData<MustCase<(IEnumerable<string>? value, Func<string, bool>? predicate)>> InvalidCases =>
        [
            new("Null", (null, IsA), new MustExpected(false, "value must not be null.", "value")),
            new("Partial", (["a", "b"], IsA), new MustExpected(false, "value must have all items match the predicate.", Code: MustCodes.Collection.Items.NotAllMatch)),
            new("Null predicate", (["a"], null), new MustExpected(false, "predicate must not be null.", "predicate"))
        ];
    }

    public static class NotHasAll
    {
        private static readonly Func<string, bool> IsA = x => x == "a";

        public static TheoryData<MustCase<(IEnumerable<string>? value, Func<string, bool>? predicate)>> ValidCases =>
        [
            new("Not all a", (["a", "b"], IsA), new MustExpected(true))
        ];

        public static TheoryData<MustCase<(IEnumerable<string>? value, Func<string, bool>? predicate)>> InvalidCases =>
        [
            new("Null", (null, IsA), new MustExpected(false, "value must not be null.", "value")),
            new("All match", (["a", "a"], IsA), new MustExpected(false, "value must not have all items match the predicate.", Code: MustCodes.Collection.Items.AllMatch)),
            new("Empty", ([], IsA), new MustExpected(false, "value must not have all items match the predicate.", Code: MustCodes.Collection.Items.AllMatch)),
            new("Null predicate", (["a"], null), new MustExpected(false, "predicate must not be null.", "predicate"))
        ];
    }

    public static class HasDistinctItems
    {
        public static TheoryData<MustCase<IEnumerable<string>>> ValidCases => F.HasDistinctItems.ValidScenarios.ToMustCases();

        public static TheoryData<MustCase<IEnumerable<string>>> EdgeCases =>
        [
            new(nameof(ArrayBacked), ArrayBacked, new MustExpected(true))
        ];

        public static TheoryData<MustCase<IEnumerable<string>>> InvalidCases => F.HasDistinctItems.InvalidScenarios.ToMustCases(s => s.Name switch
        {
            "Null" => new MustExpected(false, "value must not be null.", "value"),
            _ => new MustExpected(false, "value must have distinct items.", Code: MustCodes.Collection.Items.Duplicate)
        });
    }

    public static class HasDuplicateItems
    {
        public static TheoryData<MustCase<IEnumerable<string>>> ValidCases => F.HasDuplicateItems.ValidScenarios.ToMustCases();

        public static TheoryData<MustCase<IEnumerable<string>>> EdgeCases =>
        [
            new(nameof(ArrayBackedWithDuplicate), ArrayBackedWithDuplicate, new MustExpected(true))
        ];

        public static TheoryData<MustCase<IEnumerable<string>>> InvalidCases => F.HasDuplicateItems.InvalidScenarios.ToMustCases(s => s.Name switch
        {
            "Null" => new MustExpected(false, "value must not be null.", "value"),
            _ => new MustExpected(false, "value must have duplicate items.", Code: MustCodes.Collection.Items.Distinct)
        });
    }

    public static class NotContainsNullItems
    {
        public static TheoryData<MustCase<IEnumerable<string?>>> ValidCases => F.ContainsNullItems.InvalidScenarios.Except("NullCollection").ToMustCases(_ => new MustExpected(true));

        public static TheoryData<MustCase<IEnumerable<string?>>> EdgeCases =>
        [
            new(nameof(ArrayBacked), ArrayBacked, new MustExpected(true))
        ];

        public static TheoryData<MustCase<IEnumerable<string?>>> InvalidCases
        {
            get
            {
                var data = F.ContainsNullItems.ValidScenarios.ToMustCases(_ => new MustExpected(false, "value must not contain any null items.", Code: MustCodes.Collection.Items.ContainsNull));
                data.Add(new MustCase<IEnumerable<string?>>("NullCollection", null!, new MustExpected(false, "value must not be null.", "value")));
                return data;
            }
        }
    }

    public static class Contains
    {
        public static TheoryData<MustCase<(IEnumerable<string>? value, string item)>> ValidCases => F.Contains.ValidScenarios.ToMustCases();

        public static TheoryData<MustCase<(IEnumerable<string>? value, string item)>> InvalidCases => F.Contains.InvalidScenarios.ToMustCases(s => s.Name switch
        {
            nameof(F.Contains.NullA) => new MustExpected(false, "value must not be null.", "value"),
            _ => new MustExpected(false, "value must contain the specified item.", Code: MustCodes.Collection.Items.Missing)
        });
    }

    public static class NotContains
    {
        public static TheoryData<MustCase<(IEnumerable<string>? value, string item)>> ValidCases =>
        [
            new("Not contains", (["b", "c"], "a"), new MustExpected(true)),
            new("Empty", ([], "a"), new MustExpected(true))
        ];

        public static TheoryData<MustCase<(IEnumerable<string>? value, string item)>> InvalidCases =>
        [
            new(nameof(F.Contains.NullA), (null, "a"), new MustExpected(false, "value must not be null.", "value")),
            new("Contains a", (["a", "b"], "a"), new MustExpected(false, "value must not contain the specified item.", Code: MustCodes.Collection.Items.Present))
        ];
    }

    public static class SubsetOf
    {
        public static TheoryData<MustCase<(IEnumerable<string>? value, IEnumerable<string>? other)>> ValidCases => F.IsSubsetOf.ValidScenarios.ToMustCases();

        public static TheoryData<MustCase<(IEnumerable<string>? value, IEnumerable<string>? other)>> EdgeCases =>
        [
            new(nameof(ArrayBackedSubset), (ArrayBackedSubset, ArrayBacked), new MustExpected(true))
        ];

        public static TheoryData<MustCase<(IEnumerable<string>? value, IEnumerable<string>? other)>> InvalidCases => F.IsSubsetOf.InvalidScenarios.ToMustCases(s => s.Name switch
        {
            nameof(F.IsSubsetOf.NullMultiple) => new MustExpected(false, "value must not be null.", "value"),
            nameof(F.IsSubsetOf.MultipleNull) => new MustExpected(false, "other must not be null.", "other"),
            _ => new MustExpected(false, "value must be a subset of the other collection.", Code: MustCodes.Collection.Items.NotSubset)
        });
    }

    public static class NotSubsetOf
    {
        public static TheoryData<MustCase<(IEnumerable<string>? value, IEnumerable<string>? other)>> ValidCases =>
        [
            new("Not subset", (["z"], ["a", "b"]), new MustExpected(true))
        ];

        public static TheoryData<MustCase<(IEnumerable<string>? value, IEnumerable<string>? other)>> EdgeCases =>
        [
            new(nameof(ArrayBackedDisjoint), (ArrayBackedDisjoint, ArrayBacked), new MustExpected(true))
        ];

        public static TheoryData<MustCase<(IEnumerable<string>? value, IEnumerable<string>? other)>> InvalidCases =>
        [
            new(nameof(F.IsSubsetOf.NullMultiple), (null, ["a"]), new MustExpected(false, "value must not be null.", "value")),
            new(nameof(F.IsSubsetOf.MultipleNull), (["a"], null), new MustExpected(false, "other must not be null.", "other")),
            new("Is subset", (["a"], ["a", "b"]), new MustExpected(false, "value must not be a subset of the other collection.", Code: MustCodes.Collection.Items.Subset))
        ];
    }

    public static class HasIndex
    {
        public static TheoryData<MustCase<(IEnumerable<string>? value, int index)>> ValidCases => F.HasIndex.ValidScenarios.ToMustCases();

        public static TheoryData<MustCase<(IEnumerable<string>? value, int index)>> InvalidCases => F.HasIndex.InvalidScenarios.ToMustCases(s => s.Name switch
        {
            nameof(F.HasIndex.NullZero) => new MustExpected(false, "value must not be null.", "value"),
            nameof(F.HasIndex.MultipleNeg) => new MustExpected(false, "index requires a non-negative index.", "index"),
            _ => new MustExpected(false, "value must have an item at the specified index.", Code: MustCodes.Collection.Index.OutOfRange)
        });
    }

    public static class NotHasIndex
    {
        public static TheoryData<MustCase<(IEnumerable<string>? value, int index)>> ValidCases =>
        [
            new("No index", (["a", "b"], 5), new MustExpected(true)),
            new("Empty", ([], 0), new MustExpected(true))
        ];

        public static TheoryData<MustCase<(IEnumerable<string>? value, int index)>> InvalidCases =>
        [
            new(nameof(F.HasIndex.NullZero), (null, 0), new MustExpected(false, "value must not be null.", "value")),
            new(nameof(F.HasIndex.MultipleNeg), (["a"], -1), new MustExpected(false, "index requires a non-negative index.", "index")),
            new("Has index", (["a", "b"], 0), new MustExpected(false, "value must not have an item at the specified index.", Code: MustCodes.Collection.Index.InRange))
        ];
    }

    public static class NotHasExactCount
    {
        public static TheoryData<MustCase<(IEnumerable<string>? value, int count)>> ValidCases =>
        [
            new("Wrong count", (["a", "b"], 3), new MustExpected(true)),
            new("Empty", ([], 3), new MustExpected(true))
        ];

        public static TheoryData<MustCase<(IEnumerable<string>? value, int count)>> InvalidCases =>
        [
            new(nameof(F.HasExactCount.NullThree), (null, 3), new MustExpected(false, "value must not be null.", "value")),
            new(nameof(F.HasExactCount.SingleNeg), (["a"], -1), new MustExpected(false, "count requires a non-negative count.", "count")),
            new("Exact count", (["a", "b", "c"], 3), new MustExpected(false, "value must not have the expected count.", Code: MustCodes.Collection.Count.Match))
        ];
    }

    public static class NotHasMinCount
    {
        public static TheoryData<MustCase<(IEnumerable<string>? value, int min)>> ValidCases =>
        [
            new("Below min", (["a"], 3), new MustExpected(true)),
            new("Empty", ([], 1), new MustExpected(true))
        ];

        public static TheoryData<MustCase<(IEnumerable<string>? value, int min)>> InvalidCases =>
        [
            new(nameof(F.HasMinCount.NullOne), (null, 1), new MustExpected(false, "value must not be null.", "value")),
            new("At min", (["a", "b"], 2), new MustExpected(false, "value must not have at least the minimum count.", Code: MustCodes.Collection.Count.TooMany)),
            new("NegativeMin", (["a"], -1), new MustExpected(false, "min requires a non-negative minimum count.", "min"))
        ];
    }

    public static class NotHasMaxCount
    {
        public static TheoryData<MustCase<(IEnumerable<string>? value, int max)>> ValidCases =>
        [
            new("Above max", (["a", "b", "c"], 2), new MustExpected(true))
        ];

        public static TheoryData<MustCase<(IEnumerable<string>? value, int max)>> EdgeCases =>
        [
            new(nameof(ArrayBacked), (ArrayBacked, 2), new MustExpected(true))
        ];

        public static TheoryData<MustCase<(IEnumerable<string>? value, int max)>> InvalidCases =>
        [
            new(nameof(F.HasMaxCount.NullThree), (null, 3), new MustExpected(false, "value must not be null.", "value")),
            new("At max", (["a", "b"], 3), new MustExpected(false, "value must not have at most the maximum count.", Code: MustCodes.Collection.Count.TooFew)),
            new("NegativeMax", (["a"], -1), new MustExpected(false, "max requires a non-negative maximum count.", "max"))
        ];
    }

    public static class NotHasCountBetween
    {
        public static TheoryData<MustCase<(IEnumerable<string>? value, int min, int max, Inclusion inclusion)>> ValidCases =>
        [
            new("Out of range", (["a", "b", "c"], 4, 6, Inclusion.Inclusive), new MustExpected(true))
        ];

        public static TheoryData<MustCase<(IEnumerable<string>? value, int min, int max, Inclusion inclusion)>> EdgeCases =>
        [
            new(nameof(ArrayBacked), (ArrayBacked, 4, 6, Inclusion.Inclusive), new MustExpected(true))
        ];

        public static TheoryData<MustCase<(IEnumerable<string>? value, int min, int max, Inclusion inclusion)>> InvalidCases =>
        [
            new(nameof(F.HasCountBetween.NullTwoFourInclusive), (null, 2, 4, Inclusion.Inclusive), new MustExpected(false, "value must not be null.", "value")),
            new("In range", (["a", "b", "c"], 2, 4, Inclusion.Inclusive), new MustExpected(false, "value must not have a count within the expected range.", Code: MustCodes.Collection.Count.InRange)),
            new("NegativeMin", (["a"], -1, 3, Inclusion.Inclusive), new MustExpected(false, "min requires a non-negative minimum count.", "min")),
            new("NegativeMax", (["a"], 0, -1, Inclusion.Inclusive), new MustExpected(false, "max requires a non-negative maximum count.", "max")),
            new("MinGtMax", (["a"], 4, 2, Inclusion.Inclusive), new MustExpected(false, "min requires a valid count range.", "min"))
        ];
    }
}
