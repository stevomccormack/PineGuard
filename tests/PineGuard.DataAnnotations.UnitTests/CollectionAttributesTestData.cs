using PineGuard.Testing.UnitTests;

namespace PineGuard.DataAnnotations.UnitTests;

public static class CollectionAttributesTestData
{
    private static readonly int[] Empty = [];
    private static readonly int[] One = [1];
    private static readonly int[] OneTwo = [1, 2];
    private static readonly int[] OneTwoThree = [1, 2, 3];
    private static readonly int[] OneTwoThreeFour = [1, 2, 3, 4];
    private static readonly int[] OneOneTwo = [1, 1, 2];

    public sealed record ValidCase(string Name, object? Value, bool Expected) : ReturnCase<object?, bool>(Name, Value, Expected);

    private static TheoryData<ValidCase> CommonEdgeCases() =>
    [
        new("null", null, true),
        new("not collection (int)", 123, true)
    ];

    public static class EmptyCollection
    {
        public static TheoryData<ValidCase> ValidCases => [new("empty", Empty, true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new("not empty", One, false)];
    }

    public static class NotEmptyCollection
    {
        public static TheoryData<ValidCase> ValidCases => [new("not empty", One, true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new("empty", Empty, false)];
    }

    public static class HasExactCountCollection
    {
        public static TheoryData<ValidCase> ValidCases => [new("exact match", OneTwo, true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases =>
        [
            new("too few", One, false),
            new("too many", OneTwoThree, false)
        ];
    }

    public static class HasMinCountCollection
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("min match", OneTwo, true),
            new("more than min", OneTwoThree, true)
        ];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new("too few", One, false)];
    }

    public static class HasMaxCountCollection
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("max match", OneTwo, true),
            new("less than max", One, true)
        ];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new("too many", OneTwoThree, false)];
    }

    public static class HasCountBetweenCollection
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("min boundary", One, true),
            new("max boundary", OneTwoThree, true),
            new("middle", OneTwo, true)
        ];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases =>
        [
            new("too few", Empty, false),
            new("too many", OneTwoThreeFour, false)
        ];
    }

    public static class HasDistinctItemsCollection
    {
        public static TheoryData<ValidCase> ValidCases => [new("distinct", OneTwoThree, true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new("duplicates", OneOneTwo, false)];
    }

    public static class HasDuplicateItemsCollection
    {
        public static TheoryData<ValidCase> ValidCases => [new("duplicates", OneOneTwo, true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new("distinct", OneTwoThree, false)];
    }
}
