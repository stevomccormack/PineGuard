namespace PineGuard.Codes;

// Serves: MustBufferClauses.cs
public static partial class MustCodes
{
    /// <summary>The <c>encoding</c> domain: binary payloads carried as text (Base64, hexadecimal).</summary>
    public static class Encoding
    {
        /// <summary>The code prefix for this node (<c>"encoding"</c>).</summary>
        public const string Prefix = "encoding";

        /// <summary>The Base64 form of the value.</summary>
        public static class Base64
        {
            /// <summary>The code prefix for this node (<c>"encoding.base64"</c>).</summary>
            public const string Prefix = Encoding.Prefix + ".base64";

            /// <summary><c>encoding.base64.invalid</c></summary>
            public const string Invalid = Prefix + ".invalid";

            /// <summary><c>encoding.base64.well-formed</c></summary>
            public const string WellFormed = Prefix + ".well-formed";
        }

        /// <summary>The Base64Url form of the value: RFC 4648 §5's URL- and filename-safe alphabet.</summary>
        public static class Base64url
        {
            /// <summary>The code prefix for this node (<c>"encoding.base64url"</c>).</summary>
            public const string Prefix = Encoding.Prefix + ".base64url";

            /// <summary><c>encoding.base64url.invalid</c></summary>
            public const string Invalid = Prefix + ".invalid";
        }

        /// <summary>The hexadecimal form of the value.</summary>
        public static class Hex
        {
            /// <summary>The code prefix for this node (<c>"encoding.hex"</c>).</summary>
            public const string Prefix = Encoding.Prefix + ".hex";

            /// <summary><c>encoding.hex.invalid</c></summary>
            public const string Invalid = Prefix + ".invalid";

            /// <summary><c>encoding.hex.well-formed</c></summary>
            public const string WellFormed = Prefix + ".well-formed";
        }
    }
}
