using System.Diagnostics.CodeAnalysis;

namespace PineGuard.Codes;

// Serves: MustXmlClauses.cs, MustXmlSchemaClauses.cs (PineGuard.Xml)
public static partial class MustCodes
{
    /// <summary>
    /// The <c>xml</c> domain: XML document well-formedness, root element identification, XSD schema
    /// conformance, and XML content negotiation.
    /// </summary>
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

        /// <summary>The identity of the document's root element.</summary>
        public static class Root
        {
            /// <summary>The code prefix for this node (<c>"xml.root"</c>).</summary>
            public const string Prefix = Xml.Prefix + ".root";

            /// <summary><c>xml.root.mismatch</c></summary>
            public const string Mismatch = Prefix + ".mismatch";
        }

        /// <summary>Conformance of the document against a compiled XSD schema set.</summary>
        public static class Schema
        {
            /// <summary>The code prefix for this node (<c>"xml.schema"</c>).</summary>
            public const string Prefix = Xml.Prefix + ".schema";

            /// <summary><c>xml.schema.mismatch</c></summary>
            public const string Mismatch = Prefix + ".mismatch";
        }

        /// <summary>Coverage of the document's root namespace by a compiled XSD schema set.</summary>
        [SuppressMessage("Naming", "CA1716:Identifiers should not match keywords",
            Justification = "Domain identifiers mirror the public code strings; this node addresses an XML namespace, not a .NET namespace declaration.")]
        public static class Namespace
        {
            /// <summary>The code prefix for this node (<c>"xml.namespace"</c>).</summary>
            public const string Prefix = Xml.Prefix + ".namespace";

            /// <summary><c>xml.namespace.unknown</c></summary>
            public const string Unknown = Prefix + ".unknown";
        }
    }
}
