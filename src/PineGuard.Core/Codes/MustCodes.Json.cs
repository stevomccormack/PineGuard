namespace PineGuard.Codes;

// Serves: MustJsonClauses.cs
public static partial class MustCodes
{
    /// <summary>The <c>json</c> domain: JSON document well-formedness, root kind, and JSON content negotiation.</summary>
    public static class Json
    {
        /// <summary>The code prefix for this node (<c>"json"</c>).</summary>
        public const string Prefix = "json";

        /// <summary>Well-formedness of the payload as a whole.</summary>
        public static class Document
        {
            /// <summary>The code prefix for this node (<c>"json.document"</c>).</summary>
            public const string Prefix = Json.Prefix + ".document";

            /// <summary><c>json.document.invalid</c></summary>
            public const string Invalid = Prefix + ".invalid";
        }

        /// <summary>The kind of the top-level JSON value.</summary>
        public static class Root
        {
            /// <summary>The code prefix for this node (<c>"json.root"</c>).</summary>
            public const string Prefix = Json.Prefix + ".root";

            /// <summary><c>json.root.not-object</c></summary>
            public const string NotObject = Prefix + ".not-object";

            /// <summary><c>json.root.not-array</c></summary>
            public const string NotArray = Prefix + ".not-array";
        }

        /// <summary>The negotiated media type carried by the HTTP headers.</summary>
        public static class ContentType
        {
            /// <summary>The code prefix for this node (<c>"json.content-type"</c>).</summary>
            public const string Prefix = Json.Prefix + ".content-type";

            /// <summary><c>json.content-type.mismatch</c></summary>
            public const string Mismatch = Prefix + ".mismatch";
        }
    }
}
