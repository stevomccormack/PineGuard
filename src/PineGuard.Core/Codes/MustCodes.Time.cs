namespace PineGuard.Codes;

// Serves: MustTimeOnlyClauses.cs, MustTimeSpanClauses.cs, MustStringTimeOnlyClauses.cs, MustStringTimeSpanClauses.cs
public static partial class MustCodes
{
    /// <summary>The <c>time</c> domain: time-of-day and duration parsing, ordering, equality, ranges, proximity, and overlap.</summary>
    public static class Time
    {
        /// <summary>The code prefix for this node (<c>"time"</c>).</summary>
        public const string Prefix = "time";

        /// <summary>Parsing of a textual time or duration into a time value.</summary>
        public static class Format
        {
            /// <summary>The code prefix for this node (<c>"time.format"</c>).</summary>
            public const string Prefix = Time.Prefix + ".format";

            /// <summary><c>time.format.invalid</c></summary>
            public const string Invalid = Prefix + ".invalid";
        }

        /// <summary>Chronological ordering against another time, or of a start and end pair.</summary>
        public static class Order
        {
            /// <summary>The code prefix for this node (<c>"time.order"</c>).</summary>
            public const string Prefix = Time.Prefix + ".order";

            /// <summary><c>time.order.not-before</c></summary>
            public const string NotBefore = Prefix + ".not-before";

            /// <summary><c>time.order.before</c></summary>
            public const string Before = Prefix + ".before";

            /// <summary><c>time.order.not-after</c></summary>
            public const string NotAfter = Prefix + ".not-after";

            /// <summary><c>time.order.after</c></summary>
            public const string After = Prefix + ".after";

            /// <summary><c>time.order.not-chronological</c></summary>
            public const string NotChronological = Prefix + ".not-chronological";

            /// <summary><c>time.order.chronological</c></summary>
            public const string Chronological = Prefix + ".chronological";
        }

        /// <summary>Equality against another time, compared at the requested precision.</summary>
        public static class Equality
        {
            /// <summary>The code prefix for this node (<c>"time.equality"</c>).</summary>
            public const string Prefix = Time.Prefix + ".equality";

            /// <summary><c>time.equality.not-equal</c></summary>
            public const string NotEqual = Prefix + ".not-equal";

            /// <summary><c>time.equality.equal</c></summary>
            public const string Equal = Prefix + ".equal";
        }

        /// <summary>Membership of a time-of-day range bounded by a minimum and a maximum.</summary>
        public static class Range
        {
            /// <summary>The code prefix for this node (<c>"time.range"</c>).</summary>
            public const string Prefix = Time.Prefix + ".range";

            /// <summary><c>time.range.out-of-range</c></summary>
            public const string OutOfRange = Prefix + ".out-of-range";

            /// <summary><c>time.range.in-range</c></summary>
            public const string InRange = Prefix + ".in-range";
        }

        /// <summary>Length of an elapsed duration, against a range or a threshold.</summary>
        public static class Duration
        {
            /// <summary>The code prefix for this node (<c>"time.duration"</c>).</summary>
            public const string Prefix = Time.Prefix + ".duration";

            /// <summary><c>time.duration.out-of-range</c></summary>
            public const string OutOfRange = Prefix + ".out-of-range";

            /// <summary><c>time.duration.in-range</c></summary>
            public const string InRange = Prefix + ".in-range";

            /// <summary><c>time.duration.not-greater</c></summary>
            public const string NotGreater = Prefix + ".not-greater";

            /// <summary><c>time.duration.not-less</c></summary>
            public const string NotLess = Prefix + ".not-less";
        }

        /// <summary>Nearness to a reference time, measured against a tolerance window.</summary>
        public static class Proximity
        {
            /// <summary>The code prefix for this node (<c>"time.proximity"</c>).</summary>
            public const string Prefix = Time.Prefix + ".proximity";

            /// <summary><c>time.proximity.not-within</c></summary>
            public const string NotWithin = Prefix + ".not-within";

            /// <summary><c>time.proximity.within</c></summary>
            public const string Within = Prefix + ".within";
        }

        /// <summary>Intersection of one start-and-end time interval with another.</summary>
        public static class Overlap
        {
            /// <summary>The code prefix for this node (<c>"time.overlap"</c>).</summary>
            public const string Prefix = Time.Prefix + ".overlap";

            /// <summary><c>time.overlap.missing</c></summary>
            public const string Missing = Prefix + ".missing";

            /// <summary><c>time.overlap.present</c></summary>
            public const string Present = Prefix + ".present";
        }
    }
}
