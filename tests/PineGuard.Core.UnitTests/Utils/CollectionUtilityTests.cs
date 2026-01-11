using PineGuard.Testing.UnitTests;
using PineGuard.Utils;

namespace PineGuard.Core.UnitTests.Utils;

public sealed class CollectionUtilityTests : BaseUnitTest
{
    private sealed class ReadOnlyListOnly<T>(params T[] items) : IReadOnlyList<T>
    {
        public int Count => items.Length;

        public T this[int index] => items[index];

        public IEnumerator<T> GetEnumerator() => ((IEnumerable<T>)items).GetEnumerator();

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => items.GetEnumerator();
    }

    [Fact]
    public void TryGetCount_ReturnsFalse_ForNull()
    {
        var result = CollectionUtility.TryGetCount<string>(null, out var count);

        Assert.False(result);
        Assert.Equal(0, count);
    }

    [Fact]
    public void TryGetCount_ReturnsTrue_ForICollection()
    {
        IEnumerable<int> values = new List<int> { 1, 2, 3 };

        var result = CollectionUtility.TryGetCount(values, out var count);

        Assert.True(result);
        Assert.Equal(3, count);
    }

    [Fact]
    public void TryGetCount_ReturnsTrue_ForIReadOnlyCollection()
    {
        IReadOnlyCollection<int> readOnly = Array.AsReadOnly([1, 2, 3, 4]);
        IEnumerable<int> values = readOnly;

        var result = CollectionUtility.TryGetCount(values, out var count);

        Assert.True(result);
        Assert.Equal(4, count);
    }

    [Fact]
    public void TryGetCount_ReturnsTrue_ForIReadOnlyCollectionOnly()
    {
        IEnumerable<int> values = new ReadOnlyListOnly<int>(1, 2, 3);

        var result = CollectionUtility.TryGetCount(values, out var count);

        Assert.True(result);
        Assert.Equal(3, count);
    }

    [Fact]
    public void TryGetCount_ReturnsFalse_ForNonCountableEnumerable()
    {
        static IEnumerable<int> Iterator()
        {
            yield return 10;
            yield return 20;
        }

        var result = CollectionUtility.TryGetCount(Iterator(), out var count);

        Assert.False(result);
        Assert.Equal(0, count);
    }

    [Fact]
    public void TryGet_ReturnsFalse_ForNullOrNegativeIndex()
    {
        Assert.False(CollectionUtility.TryGet<string>(null, 0, out _));
        Assert.False(CollectionUtility.TryGet(["a"], -1, out _));
    }

    [Fact]
    public void TryGet_Works_ForIListPath()
    {
        IList<string> list = ["a", "b", "c"];

        Assert.True(CollectionUtility.TryGet(list, 1, out var item));
        Assert.Equal("b", item);

        Assert.False(CollectionUtility.TryGet(list, 3, out _));
    }

    [Fact]
    public void TryGet_Works_ForIReadOnlyListPath()
    {
        IReadOnlyList<string> roList = Array.AsReadOnly(["a", "b"]);

        Assert.True(CollectionUtility.TryGet(roList, 0, out var item));
        Assert.Equal("a", item);

        Assert.False(CollectionUtility.TryGet(roList, 2, out _));
    }

    [Fact]
    public void TryGet_Works_ForIReadOnlyListOnlyPath()
    {
        IEnumerable<string> values = new ReadOnlyListOnly<string>("a", "b");

        Assert.True(CollectionUtility.TryGet(values, 0, out var item));
        Assert.Equal("a", item);

        Assert.False(CollectionUtility.TryGet(values, 2, out var outOfRange));
        Assert.Null(outOfRange);
    }

    [Fact]
    public void TryGet_Works_ForEnumeratorPath()
    {
        static IEnumerable<string> Iterator()
        {
            yield return "x";
            yield return "y";
            yield return "z";
        }

        Assert.True(CollectionUtility.TryGet(Iterator(), 2, out var item));
        Assert.Equal("z", item);

        Assert.False(CollectionUtility.TryGet(Iterator(), 3, out _));
    }

    [Fact]
    public void TryGet_UsesCountShortCircuit_ForCountableNonList()
    {
        // Queue<T> is countable but not indexable.
        IEnumerable<string> values = new Queue<string>(["a", "b"]);

        Assert.False(CollectionUtility.TryGet(values, 2, out var outOfRange));
        Assert.Null(outOfRange);

        Assert.True(CollectionUtility.TryGet(values, 1, out var item));
        Assert.Equal("b", item);
    }

    [Fact]
    public void TryGet_ReturnsFalse_ForICollectionNonList_WhenIndexOutOfRange()
    {
        // HashSet<T> is countable but not indexable.
        IEnumerable<string> values = new HashSet<string>(StringComparer.Ordinal) { "a", "b" };

        Assert.False(CollectionUtility.TryGet(values, 2, out var outOfRange));
        Assert.Null(outOfRange);
    }

    [Fact]
    public void TryGetIndex_ReturnsFalse_ForNull()
    {
        var result = CollectionUtility.TryGetIndex<string>(null, "a", out var index);

        Assert.False(result);
        Assert.Equal(-1, index);
    }

    [Fact]
    public void TryGetIndex_Works_ForIListPath()
    {
        IList<int> list = [10, 20, 30];

        Assert.True(CollectionUtility.TryGetIndex(list, 20, out var index));
        Assert.Equal(1, index);

        Assert.False(CollectionUtility.TryGetIndex(list, 99, out index));
        Assert.Equal(-1, index);
    }

    [Fact]
    public void TryGetIndex_Works_ForEnumeratorPath()
    {
        static IEnumerable<int> Iterator()
        {
            yield return 5;
            yield return 6;
        }

        Assert.True(CollectionUtility.TryGetIndex(Iterator(), 6, out var index));
        Assert.Equal(1, index);

        Assert.False(CollectionUtility.TryGetIndex(Iterator(), 7, out index));
        Assert.Equal(-1, index);
    }
}
