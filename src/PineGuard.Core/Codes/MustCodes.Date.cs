using System.Diagnostics.CodeAnalysis;

namespace PineGuard.Codes;

// Serves: MustDateTimeClauses.cs, MustDateOnlyClauses.cs, MustDateTimeOffsetClauses.cs, MustSqlDateTimeClauses.cs, MustStringDateOnlyClauses.cs, MustStringDateTimeOffsetClauses.cs
public static partial class MustCodes
{
    /// <summary>
    /// The <c>date</c> domain: instant and calendar checks over <see cref="DateTime"/>, <c>DateOnly</c>,
    /// <see cref="DateTimeOffset"/> and their string representations.
    /// </summary>
    [SuppressMessage("Naming", "CA1716:Identifiers should not match keywords",
        Justification = "Domain identifiers mirror the public code strings; the domain of these codes is 'date'.")]
    public static class Date
    {
        /// <summary>The code prefix for this node (<c>"date"</c>).</summary>
        public const string Prefix = "date";

        /// <summary>Parsing of a textual date or date/time into a date value.</summary>
        public static class Format
        {
            /// <summary>The code prefix for this node (<c>"date.format"</c>).</summary>
            public const string Prefix = Date.Prefix + ".format";

            /// <summary><c>date.format.invalid</c></summary>
            public const string Invalid = Prefix + ".invalid";
        }

        /// <summary>Position relative to the current instant.</summary>
        public static class Relative
        {
            /// <summary>The code prefix for this node (<c>"date.relative"</c>).</summary>
            public const string Prefix = Date.Prefix + ".relative";

            /// <summary><c>date.relative.not-past</c></summary>
            public const string NotPast = Prefix + ".not-past";

            /// <summary><c>date.relative.past</c></summary>
            public const string Past = Prefix + ".past";

            /// <summary><c>date.relative.not-future</c></summary>
            public const string NotFuture = Prefix + ".not-future";

            /// <summary><c>date.relative.future</c></summary>
            public const string Future = Prefix + ".future";
        }

        /// <summary>Elapsed whole years between a date of birth and today.</summary>
        public static class Age
        {
            /// <summary>The code prefix for this node (<c>"date.age"</c>).</summary>
            public const string Prefix = Date.Prefix + ".age";

            /// <summary><c>date.age.below-minimum</c></summary>
            public const string BelowMinimum = Prefix + ".below-minimum";
        }

        /// <summary>Chronological ordering against another date, or of a start and end pair.</summary>
        public static class Order
        {
            /// <summary>The code prefix for this node (<c>"date.order"</c>).</summary>
            public const string Prefix = Date.Prefix + ".order";

            /// <summary><c>date.order.not-before</c></summary>
            public const string NotBefore = Prefix + ".not-before";

            /// <summary><c>date.order.before</c></summary>
            public const string Before = Prefix + ".before";

            /// <summary><c>date.order.not-after</c></summary>
            public const string NotAfter = Prefix + ".not-after";

            /// <summary><c>date.order.after</c></summary>
            public const string After = Prefix + ".after";

            /// <summary><c>date.order.not-chronological</c></summary>
            public const string NotChronological = Prefix + ".not-chronological";

            /// <summary><c>date.order.chronological</c></summary>
            public const string Chronological = Prefix + ".chronological";
        }

        /// <summary>Equality against another date or date/time, at an optional precision.</summary>
        public static class Equality
        {
            /// <summary>The code prefix for this node (<c>"date.equality"</c>).</summary>
            public const string Prefix = Date.Prefix + ".equality";

            /// <summary><c>date.equality.not-equal</c></summary>
            public const string NotEqual = Prefix + ".not-equal";

            /// <summary><c>date.equality.equal</c></summary>
            public const string Equal = Prefix + ".equal";

            /// <summary><c>date.equality.not-same-day</c></summary>
            public const string NotSameDay = Prefix + ".not-same-day";

            /// <summary><c>date.equality.same-day</c></summary>
            public const string SameDay = Prefix + ".same-day";
        }

        /// <summary>Containment within an explicit minimum/maximum pair.</summary>
        public static class Range
        {
            /// <summary>The code prefix for this node (<c>"date.range"</c>).</summary>
            public const string Prefix = Date.Prefix + ".range";

            /// <summary><c>date.range.out-of-range</c></summary>
            public const string OutOfRange = Prefix + ".out-of-range";

            /// <summary><c>date.range.in-range</c></summary>
            public const string InRange = Prefix + ".in-range";
        }

        /// <summary>Nearness to a reference date, measured against a tolerance window.</summary>
        public static class Proximity
        {
            /// <summary>The code prefix for this node (<c>"date.proximity"</c>).</summary>
            public const string Prefix = Date.Prefix + ".proximity";

            /// <summary><c>date.proximity.not-within</c></summary>
            public const string NotWithin = Prefix + ".not-within";

            /// <summary><c>date.proximity.within</c></summary>
            public const string Within = Prefix + ".within";

            /// <summary><c>date.proximity.not-within-calendar-months</c></summary>
            public const string NotWithinCalendarMonths = Prefix + ".not-within-calendar-months";

            /// <summary><c>date.proximity.within-calendar-months</c></summary>
            public const string WithinCalendarMonths = Prefix + ".within-calendar-months";
        }

        /// <summary>Intersection of one start-and-end date interval with another.</summary>
        public static class Overlap
        {
            /// <summary>The code prefix for this node (<c>"date.overlap"</c>).</summary>
            public const string Prefix = Date.Prefix + ".overlap";

            /// <summary><c>date.overlap.missing</c></summary>
            public const string Missing = Prefix + ".missing";

            /// <summary><c>date.overlap.present</c></summary>
            public const string Present = Prefix + ".present";
        }

        /// <summary>Calendar placement of the date itself: day of week, day of month.</summary>
        public static class Calendar
        {
            /// <summary>The code prefix for this node (<c>"date.calendar"</c>).</summary>
            public const string Prefix = Date.Prefix + ".calendar";

            /// <summary><c>date.calendar.not-weekday</c></summary>
            public const string NotWeekday = Prefix + ".not-weekday";

            /// <summary><c>date.calendar.not-weekend</c></summary>
            public const string NotWeekend = Prefix + ".not-weekend";

            /// <summary><c>date.calendar.not-first-day-of-month</c></summary>
            public const string NotFirstDayOfMonth = Prefix + ".not-first-day-of-month";

            /// <summary><c>date.calendar.first-day-of-month</c></summary>
            public const string FirstDayOfMonth = Prefix + ".first-day-of-month";

            /// <summary><c>date.calendar.not-last-day-of-month</c></summary>
            public const string NotLastDayOfMonth = Prefix + ".not-last-day-of-month";

            /// <summary><c>date.calendar.last-day-of-month</c></summary>
            public const string LastDayOfMonth = Prefix + ".last-day-of-month";
        }

        /// <summary>The <see cref="DateTimeKind"/> carried by the value.</summary>
        public static class Kind
        {
            /// <summary>The code prefix for this node (<c>"date.kind"</c>).</summary>
            public const string Prefix = Date.Prefix + ".kind";

            /// <summary><c>date.kind.not-utc</c></summary>
            public const string NotUtc = Prefix + ".not-utc";

            /// <summary><c>date.kind.utc</c></summary>
            public const string Utc = Prefix + ".utc";

            /// <summary><c>date.kind.not-local</c></summary>
            public const string NotLocal = Prefix + ".not-local";

            /// <summary><c>date.kind.local</c></summary>
            public const string Local = Prefix + ".local";

            /// <summary><c>date.kind.not-unspecified</c></summary>
            public const string NotUnspecified = Prefix + ".not-unspecified";

            /// <summary><c>date.kind.unspecified</c></summary>
            public const string Unspecified = Prefix + ".unspecified";
        }

        /// <summary>Representability in a SQL Server <c>datetime</c> column.</summary>
        public static class Sql
        {
            /// <summary>The code prefix for this node (<c>"date.sql"</c>).</summary>
            public const string Prefix = Date.Prefix + ".sql";

            /// <summary><c>date.sql.out-of-range</c></summary>
            public const string OutOfRange = Prefix + ".out-of-range";
        }
    }
}
