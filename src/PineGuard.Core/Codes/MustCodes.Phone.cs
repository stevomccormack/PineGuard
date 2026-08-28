namespace PineGuard.Codes;

// Serves: MustPhoneClauses.cs
public static partial class MustCodes
{
    /// <summary>The <c>phone</c> domain: phone number shape, digit count, and permitted separators.</summary>
    public static class Phone
    {
        /// <summary>The code prefix for this node (<c>"phone"</c>).</summary>
        public const string Prefix = "phone";

        /// <summary>The phone number the value carries, whether it arrived raw or normalised.</summary>
        public static class Number
        {
            /// <summary>The code prefix for this node (<c>"phone.number"</c>).</summary>
            public const string Prefix = Phone.Prefix + ".number";

            /// <summary><c>phone.number.invalid</c></summary>
            public const string Invalid = Prefix + ".invalid";
        }
    }
}
