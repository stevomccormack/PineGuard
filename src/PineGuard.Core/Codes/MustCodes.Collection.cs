using System.Diagnostics.CodeAnalysis;

namespace PineGuard.Codes;

// Serves: MustCollectionClauses.cs
public static partial class MustCodes
{
    /// <summary>The <c>collection</c> domain: the items a sequence holds, how many it holds, and which positions it addresses.</summary>
    [SuppressMessage("Naming", "CA1711:Identifiers should not have incorrect suffix",
        Justification = "Domain identifiers mirror the public code strings; the domain of these codes is 'collection'.")]
    public static class Collection
    {
        /// <summary>The code prefix for this node (<c>"collection"</c>).</summary>
        public const string Prefix = "collection";

        /// <summary>The items the sequence holds: presence, membership, predicate matching, distinctness, and set relation.</summary>
        public static class Items
        {
            /// <summary>The code prefix for this node (<c>"collection.items"</c>).</summary>
            public const string Prefix = Collection.Prefix + ".items";

            /// <summary><c>collection.items.not-empty</c></summary>
            public const string NotEmpty = Prefix + ".not-empty";

            /// <summary><c>collection.items.empty</c></summary>
            public const string Empty = Prefix + ".empty";

            /// <summary><c>collection.items.no-match</c></summary>
            public const string NoMatch = Prefix + ".no-match";

            /// <summary><c>collection.items.match</c></summary>
            public const string Match = Prefix + ".match";

            /// <summary><c>collection.items.not-all-match</c></summary>
            public const string NotAllMatch = Prefix + ".not-all-match";

            /// <summary><c>collection.items.all-match</c></summary>
            public const string AllMatch = Prefix + ".all-match";

            /// <summary><c>collection.items.duplicate</c></summary>
            public const string Duplicate = Prefix + ".duplicate";

            /// <summary><c>collection.items.distinct</c></summary>
            public const string Distinct = Prefix + ".distinct";

            /// <summary><c>collection.items.contains-null</c></summary>
            public const string ContainsNull = Prefix + ".contains-null";

            /// <summary><c>collection.items.missing</c></summary>
            public const string Missing = Prefix + ".missing";

            /// <summary><c>collection.items.present</c></summary>
            public const string Present = Prefix + ".present";

            /// <summary><c>collection.items.not-subset</c></summary>
            public const string NotSubset = Prefix + ".not-subset";

            /// <summary><c>collection.items.subset</c></summary>
            public const string Subset = Prefix + ".subset";
        }

        /// <summary>How many items the sequence holds, against an exact count, a bound, or a range.</summary>
        public static class Count
        {
            /// <summary>The code prefix for this node (<c>"collection.count"</c>).</summary>
            public const string Prefix = Collection.Prefix + ".count";

            /// <summary><c>collection.count.mismatch</c></summary>
            public const string Mismatch = Prefix + ".mismatch";

            /// <summary><c>collection.count.match</c></summary>
            public const string Match = Prefix + ".match";

            /// <summary><c>collection.count.too-few</c></summary>
            public const string TooFew = Prefix + ".too-few";

            /// <summary><c>collection.count.too-many</c></summary>
            public const string TooMany = Prefix + ".too-many";

            /// <summary><c>collection.count.out-of-range</c></summary>
            public const string OutOfRange = Prefix + ".out-of-range";

            /// <summary><c>collection.count.in-range</c></summary>
            public const string InRange = Prefix + ".in-range";
        }

        /// <summary>The positions the sequence addresses, against a zero-based index.</summary>
        public static class Index
        {
            /// <summary>The code prefix for this node (<c>"collection.index"</c>).</summary>
            public const string Prefix = Collection.Prefix + ".index";

            /// <summary><c>collection.index.out-of-range</c></summary>
            public const string OutOfRange = Prefix + ".out-of-range";

            /// <summary><c>collection.index.in-range</c></summary>
            public const string InRange = Prefix + ".in-range";
        }
    }
}
