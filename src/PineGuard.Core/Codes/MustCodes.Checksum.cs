namespace PineGuard.Codes;

// Serves: MustChecksumClauses.cs
public static partial class MustCodes
{
    /// <summary>The <c>checksum</c> domain: sequences that carry a check digit over their own contents.</summary>
    public static class Checksum
    {
        /// <summary>The code prefix for this node (<c>"checksum"</c>).</summary>
        public const string Prefix = "checksum";

        /// <summary>The Luhn (mod 10) check digit.</summary>
        public static class Luhn
        {
            /// <summary>The code prefix for this node (<c>"checksum.luhn"</c>).</summary>
            public const string Prefix = Checksum.Prefix + ".luhn";

            /// <summary><c>checksum.luhn.invalid</c></summary>
            public const string Invalid = Prefix + ".invalid";
        }
    }
}
