using System.Collections;
using System.Collections.ObjectModel;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Utils;

public static class CollectionUtilityTestData
{
    private sealed class ReadOnlyListOnly<T>(params T[] items) : IReadOnlyList<T>
    {
        public int Count => items.Length;
        public T this[int index] => items[index];
        public IEnumerator<T> GetEnumerator() => ((IEnumerable<T>)items).GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => items.GetEnumerator();
    }

    private sealed class ReadOnlyCollectionOnly<T>(params T[] items) : IReadOnlyCollection<T>
    {
        public int Count => items.Length;
        public IEnumerator<T> GetEnumerator() => ((IEnumerable<T>)items).GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => items.GetEnumerator();
    }

    private static IEnumerable<int> Iterator()
    {
        yield return 10;
        yield return 20;
    }

    public static class TryGetCount
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("List", new List<int> { 1, 2, 3 }, true, 3),
            new("Array", [1, 2, 3, 4], true, 4),
            new("ReadOnlyCollection", new ReadOnlyCollection<int>([1, 2, 3, 4]), true, 4),
            new("Custom ReadOnlyList", new ReadOnlyListOnly<int>(1, 2, 3), true, 3)
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("Null", null, false, 0),
            new("Iterator", Iterator(), false, 0)
        ];

        public sealed record ValidCase(string Name, IEnumerable<int>? Value, bool Expected, int ExpectedOutValue)
            : TryCase<IEnumerable<int>?, int>(Name, Value, Expected, ExpectedOutValue);
    }

    public static class TryGet
    {
        private static IEnumerable<string> StringIterator()
        {
            yield return "x";
            yield return "y";
            yield return "z";
        }

        public static TheoryData<ValidCase> ValidCases =>
        [
            new("List valid", (new List<string> { "a", "b", "c" }, 1), true, "b"),
            new("ReadOnlyList valid", (new ReadOnlyCollection<string>(["a", "b"]), 0), true, "a"),
            new("Custom ReadOnlyList valid", (new ReadOnlyListOnly<string>("a", "b"), 0), true, "a"),
            new("Iterator valid", (StringIterator(), 2), true, "z"),
            new("Queue valid", (new Queue<string>(["a", "b"]), 1), true, "b")
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("Null", (null, 0), false, null),
            new("Negative index", (["a"], -1), false, null),
            new("List invalid", (new List<string> { "a", "b", "c" }, 3), false, null),
            new("ReadOnlyList invalid", (new ReadOnlyCollection<string>(["a", "b"]), 2), false, null),
            new("Custom ReadOnlyList invalid", (new ReadOnlyListOnly<string>("a", "b"), 2), false, null),
            new("Iterator invalid", (StringIterator(), 3), false, null),
            new("Queue invalid (short circuit)", (new Queue<string>(["a", "b"]), 2), false, null),
            new("HashSet invalid (not indexable)", (new HashSet<string> { "a", "b" }, 2), false, null)
        ];

        public sealed record ValidCase(string Name, (IEnumerable<string>? Collection, int Index) Value, bool Expected, string? ExpectedOutValue)
            : TryCase<(IEnumerable<string>? Collection, int Index), string?>(Name, Value, Expected, ExpectedOutValue);
    }

    public static class TryGetIndex
    {
        private static IEnumerable<int> IntIterator()
        {
            yield return 5;
            yield return 6;
        }

        public static TheoryData<ValidCase> ValidCases =>
        [
            new("List valid", (new List<int> { 10, 20, 30 }, 20), true, 1),
            new("Custom ReadOnlyList valid", (new ReadOnlyListOnly<int>(10, 20, 30), 20), true, 1),
            new("ReadOnlyCollection valid", (new ReadOnlyCollection<int>([10, 20]), 20), true, 1),
            new("HashSet valid (enumerated)", (new HashSet<int> { 42 }, 42), true, 0),
            new("Custom ReadOnlyCollection valid (enumerated)", (new ReadOnlyCollectionOnly<int>(7), 7), true, 0),
            new("Iterator valid", (IntIterator(), 6), true, 1)
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("Null", (null, 10), false, -1),
            new("List not found", (new List<int> { 10, 20, 30 }, 99), false, -1),
            new("Custom ReadOnlyList not found", (new ReadOnlyListOnly<int>(10, 20, 30), 99), false, -1),
            new("Empty ICollection (short circuit)", (new HashSet<int>(), 10), false, -1),
            new("Empty custom ReadOnlyCollection (short circuit)", (new ReadOnlyCollectionOnly<int>(), 10), false, -1),
            new("HashSet not found (enumerated)", (new HashSet<int> { 42 }, 10), false, -1),
            new("Iterator not found", (IntIterator(), 7), false, -1)
        ];

        public sealed record ValidCase(string Name, (IEnumerable<int>? Collection, int Item) Value, bool Expected, int ExpectedOutValue)
            : TryCase<(IEnumerable<int>? Collection, int Item), int>(Name, Value, Expected, ExpectedOutValue);
    }
}
