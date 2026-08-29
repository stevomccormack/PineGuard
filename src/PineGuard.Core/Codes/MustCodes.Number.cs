namespace PineGuard.Codes;

// Serves: MustNumberClauses.cs, MustStringNumbersClauses.cs, MustStringNumberTypesClauses.cs
public static partial class MustCodes
{
    /// <summary>The <c>number</c> domain: numeric values and the numeric strings that encode them.</summary>
    public static class Number
    {
        /// <summary>The code prefix for this node (<c>"number"</c>).</summary>
        public const string Prefix = "number";

        /// <summary>Position on the negative / zero / positive trichotomy.</summary>
        public static class Sign
        {
            /// <summary>The code prefix for this node (<c>"number.sign"</c>).</summary>
            public const string Prefix = Number.Prefix + ".sign";

            /// <summary><c>number.sign.not-positive</c></summary>
            public const string NotPositive = Prefix + ".not-positive";

            /// <summary><c>number.sign.not-negative</c></summary>
            public const string NotNegative = Prefix + ".not-negative";

            /// <summary><c>number.sign.not-zero</c></summary>
            public const string NotZero = Prefix + ".not-zero";

            /// <summary><c>number.sign.zero</c></summary>
            public const string Zero = Prefix + ".zero";

            /// <summary><c>number.sign.negative</c></summary>
            public const string Negative = Prefix + ".negative";

            /// <summary><c>number.sign.positive</c></summary>
            public const string Positive = Prefix + ".positive";
        }

        /// <summary>Position relative to a bound or a pair of bounds, and the validity of the bounds themselves.</summary>
        public static class Range
        {
            /// <summary>The code prefix for this node (<c>"number.range"</c>).</summary>
            public const string Prefix = Number.Prefix + ".range";

            /// <summary><c>number.range.not-greater</c></summary>
            public const string NotGreater = Prefix + ".not-greater";

            /// <summary><c>number.range.below-minimum</c></summary>
            public const string BelowMinimum = Prefix + ".below-minimum";

            /// <summary><c>number.range.not-less</c></summary>
            public const string NotLess = Prefix + ".not-less";

            /// <summary><c>number.range.exceeded</c></summary>
            public const string Exceeded = Prefix + ".exceeded";

            /// <summary><c>number.range.out-of-range</c></summary>
            public const string OutOfRange = Prefix + ".out-of-range";

            /// <summary><c>number.range.in-range</c></summary>
            public const string InRange = Prefix + ".in-range";

            /// <summary><c>number.range.not-percentage</c></summary>
            public const string NotPercentage = Prefix + ".not-percentage";

            /// <summary><c>number.range.invalid</c></summary>
            public const string Invalid = Prefix + ".invalid";
        }

        /// <summary>Closeness to a target value within a tolerance.</summary>
        public static class Proximity
        {
            /// <summary>The code prefix for this node (<c>"number.proximity"</c>).</summary>
            public const string Prefix = Number.Prefix + ".proximity";

            /// <summary><c>number.proximity.not-approximate</c></summary>
            public const string NotApproximate = Prefix + ".not-approximate";

            /// <summary><c>number.proximity.approximate</c></summary>
            public const string Approximate = Prefix + ".approximate";
        }

        /// <summary>Divisibility by a factor.</summary>
        public static class Divisibility
        {
            /// <summary>The code prefix for this node (<c>"number.divisibility"</c>).</summary>
            public const string Prefix = Number.Prefix + ".divisibility";

            /// <summary><c>number.divisibility.not-multiple</c></summary>
            public const string NotMultiple = Prefix + ".not-multiple";

            /// <summary><c>number.divisibility.multiple</c></summary>
            public const string Multiple = Prefix + ".multiple";
        }

        /// <summary>Divisibility by two.</summary>
        public static class Parity
        {
            /// <summary>The code prefix for this node (<c>"number.parity"</c>).</summary>
            public const string Prefix = Number.Prefix + ".parity";

            /// <summary><c>number.parity.odd</c></summary>
            public const string Odd = Prefix + ".odd";

            /// <summary><c>number.parity.even</c></summary>
            public const string Even = Prefix + ".even";
        }

        /// <summary>The IEEE 754 form of a floating-point value: finite, infinite, or not a number.</summary>
        public static class Form
        {
            /// <summary>The code prefix for this node (<c>"number.form"</c>).</summary>
            public const string Prefix = Number.Prefix + ".form";

            /// <summary><c>number.form.not-finite</c></summary>
            public const string NotFinite = Prefix + ".not-finite";

            /// <summary><c>number.form.finite</c></summary>
            public const string Finite = Prefix + ".finite";

            /// <summary><c>number.form.nan</c></summary>
            public const string Nan = Prefix + ".nan";

            /// <summary><c>number.form.not-nan</c></summary>
            public const string NotNan = Prefix + ".not-nan";
        }

        /// <summary>The numeric type a text representation encodes.</summary>
        public static class Format
        {
            /// <summary>The code prefix for this node (<c>"number.format"</c>).</summary>
            public const string Prefix = Number.Prefix + ".format";

            /// <summary><c>number.format.not-decimal</c></summary>
            public const string NotDecimal = Prefix + ".not-decimal";

            /// <summary><c>number.format.not-int32</c></summary>
            public const string NotInt32 = Prefix + ".not-int32";

            /// <summary><c>number.format.not-int64</c></summary>
            public const string NotInt64 = Prefix + ".not-int64";
        }

        /// <summary>The number of decimal places, and the validity of the required place count.</summary>
        public static class Scale
        {
            /// <summary>The code prefix for this node (<c>"number.scale"</c>).</summary>
            public const string Prefix = Number.Prefix + ".scale";

            /// <summary><c>number.scale.mismatch</c></summary>
            public const string Mismatch = Prefix + ".mismatch";

            /// <summary><c>number.scale.negative</c></summary>
            public const string Negative = Prefix + ".negative";
        }

        /// <summary>The tolerance a proximity comparison was configured with.</summary>
        public static class Tolerance
        {
            /// <summary>The code prefix for this node (<c>"number.tolerance"</c>).</summary>
            public const string Prefix = Number.Prefix + ".tolerance";

            /// <summary><c>number.tolerance.null</c></summary>
            public const string Null = Prefix + ".null";
        }

        /// <summary>The factor a divisibility check was configured with.</summary>
        public static class Factor
        {
            /// <summary>The code prefix for this node (<c>"number.factor"</c>).</summary>
            public const string Prefix = Number.Prefix + ".factor";

            /// <summary><c>number.factor.zero</c></summary>
            public const string Zero = Prefix + ".zero";
        }
    }
}
