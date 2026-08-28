namespace PineGuard.Codes;

// Serves: MustDateTimeRangeClauses.cs, MustDateOnlyRangeClauses.cs, MustDateTimeOffsetRangeClauses.cs, MustTimeOnlyRangeClauses.cs
public static partial class MustCodes
{
    /// <summary>The <c>range</c> domain: the start/end ordering of a range, its overlap with another range, and its containment of a point in time.</summary>
    public static class Range
    {
        /// <summary>The code prefix for this node (<c>"range"</c>).</summary>
        public const string Prefix = "range";

        /// <summary>The ordering of the range's own start and end.</summary>
        public static class Order
        {
            /// <summary>The code prefix for this node (<c>"range.order"</c>).</summary>
            public const string Prefix = Range.Prefix + ".order";

            /// <summary><c>range.order.not-chronological</c></summary>
            public const string NotChronological = Prefix + ".not-chronological";
        }

        /// <summary>The intersection of the range with another range.</summary>
        public static class Overlap
        {
            /// <summary>The code prefix for this node (<c>"range.overlap"</c>).</summary>
            public const string Prefix = Range.Prefix + ".overlap";

            /// <summary><c>range.overlap.missing</c></summary>
            public const string Missing = Prefix + ".missing";

            /// <summary><c>range.overlap.present</c></summary>
            public const string Present = Prefix + ".present";
        }

        /// <summary>The span between the range's endpoints, against which a single point is tested.</summary>
        public static class Bounds
        {
            /// <summary>The code prefix for this node (<c>"range.bounds"</c>).</summary>
            public const string Prefix = Range.Prefix + ".bounds";

            /// <summary><c>range.bounds.not-contains</c></summary>
            public const string NotContains = Prefix + ".not-contains";

            /// <summary><c>range.bounds.contains</c></summary>
            public const string Contains = Prefix + ".contains";
        }
    }
}
