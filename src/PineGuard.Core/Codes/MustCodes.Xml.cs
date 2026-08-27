namespace PineGuard.Codes;

// Serves: MustXmlClauses.cs
public static partial class MustCodes
{
    /// <summary>The <c>xml</c> domain: XML document well-formedness and XML content negotiation.</summary>
    public static class Xml
    {
        /// <summary>The code prefix for this node (<c>"xml"</c>).</summary>
        public const string Prefix = "xml";

        /// <summary>Well-formedness of the payload as a whole.</summary>
        public static class Document
        {
            /// <summary>The code prefix for this node (<c>"xml.document"</c>).</summary>
            public const string Prefix = Xml.Prefix + ".document";

            /// <summary><c>xml.document.invalid</c></summary>
            public const string Invalid = Prefix + ".invalid";
        }

        /// <summary>The negotiated media type carried by the HTTP headers.</summary>
        public static class ContentType
        {
            /// <summary>The code prefix for this node (<c>"xml.content-type"</c>).</summary>
            public const string Prefix = Xml.Prefix + ".content-type";

            /// <summary><c>xml.content-type.mismatch</c></summary>
            public const string Mismatch = Prefix + ".mismatch";
        }
    }
}
