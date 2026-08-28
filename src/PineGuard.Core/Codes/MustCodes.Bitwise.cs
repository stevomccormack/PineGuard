namespace PineGuard.Codes;

// Serves: MustBitWiseClauses.cs
public static partial class MustCodes
{
    /// <summary>The <c>bitwise</c> domain: masked equality, bit membership against a mask, and the power-of-two shape of an integer.</summary>
    public static class Bitwise
    {
        /// <summary>The code prefix for this node (<c>"bitwise"</c>).</summary>
        public const string Prefix = "bitwise";

        /// <summary>The bitmask argument itself — unparsable, blank, or all-zero.</summary>
        public static class Mask
        {
            /// <summary>The code prefix for this node (<c>"bitwise.mask"</c>).</summary>
            public const string Prefix = Bitwise.Prefix + ".mask";

            /// <summary><c>bitwise.mask.invalid</c></summary>
            public const string Invalid = Prefix + ".invalid";
        }

        /// <summary>Equality of the value and the compared value, seen through the mask.</summary>
        public static class Equality
        {
            /// <summary>The code prefix for this node (<c>"bitwise.equality"</c>).</summary>
            public const string Prefix = Bitwise.Prefix + ".equality";

            /// <summary><c>bitwise.equality.not-equal</c></summary>
            public const string NotEqual = Prefix + ".not-equal";

            /// <summary><c>bitwise.equality.equal</c></summary>
            public const string Equal = Prefix + ".equal";
        }

        /// <summary>Which of the mask's bits the value has set: all of them, any of them, none of them, or bits outside the mask.</summary>
        public static class Bits
        {
            /// <summary>The code prefix for this node (<c>"bitwise.bits"</c>).</summary>
            public const string Prefix = Bitwise.Prefix + ".bits";

            /// <summary><c>bitwise.bits.not-all-set</c></summary>
            public const string NotAllSet = Prefix + ".not-all-set";

            /// <summary><c>bitwise.bits.all-set</c></summary>
            public const string AllSet = Prefix + ".all-set";

            /// <summary><c>bitwise.bits.none-set</c></summary>
            public const string NoneSet = Prefix + ".none-set";

            /// <summary><c>bitwise.bits.any-set</c></summary>
            public const string AnySet = Prefix + ".any-set";

            /// <summary><c>bitwise.bits.not-subset</c></summary>
            public const string NotSubset = Prefix + ".not-subset";

            /// <summary><c>bitwise.bits.subset</c></summary>
            public const string Subset = Prefix + ".subset";
        }

        /// <summary>The numeric shape of the value on its own, with no mask involved.</summary>
        public static class Value
        {
            /// <summary>The code prefix for this node (<c>"bitwise.value"</c>).</summary>
            public const string Prefix = Bitwise.Prefix + ".value";

            /// <summary><c>bitwise.value.not-power-of-two</c></summary>
            public const string NotPowerOfTwo = Prefix + ".not-power-of-two";

            /// <summary><c>bitwise.value.power-of-two</c></summary>
            public const string PowerOfTwo = Prefix + ".power-of-two";
        }
    }
}
