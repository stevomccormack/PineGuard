namespace PineGuard.Codes;

// Serves: MustGuidClauses.cs, MustStringGuidClauses.cs
public static partial class MustCodes
{
    /// <summary>The <c>guid</c> domain: GUID string format validity and emptiness.</summary>
    public static class Guid
    {
        /// <summary>The code prefix for this node (<c>"guid"</c>).</summary>
        public const string Prefix = "guid";

        /// <summary>Whether a string parses as a valid GUID.</summary>
        public static class Format
        {
            /// <summary>The code prefix for this node (<c>"guid.format"</c>).</summary>
            public const string Prefix = Guid.Prefix + ".format";

            /// <summary><c>guid.format.invalid</c></summary>
            public const string Invalid = Prefix + ".invalid";
        }

        /// <summary>Whether the GUID is the empty GUID (<see cref="System.Guid.Empty"/>).</summary>
        public static class Emptiness
        {
            /// <summary>The code prefix for this node (<c>"guid.emptiness"</c>).</summary>
            public const string Prefix = Guid.Prefix + ".emptiness";

            /// <summary><c>guid.emptiness.empty</c></summary>
            public const string Empty = Prefix + ".empty";
        }
    }
}
