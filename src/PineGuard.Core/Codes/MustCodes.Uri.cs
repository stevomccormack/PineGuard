namespace PineGuard.Codes;

// Serves: MustUriClauses.cs
public static partial class MustCodes
{
    /// <summary>The <c>uri</c> domain: the form a URI string parses to, the scheme it carries, and local file-path shape.</summary>
    public static class Uri
    {
        /// <summary>The code prefix for this node (<c>"uri"</c>).</summary>
        public const string Prefix = "uri";

        /// <summary>The shape the string parses to — absolute, relative, or an http(s) URL.</summary>
        public static class Form
        {
            /// <summary>The code prefix for this node (<c>"uri.form"</c>).</summary>
            public const string Prefix = Uri.Prefix + ".form";

            /// <summary><c>uri.form.not-absolute</c></summary>
            public const string NotAbsolute = Prefix + ".not-absolute";

            /// <summary><c>uri.form.not-relative</c></summary>
            public const string NotRelative = Prefix + ".not-relative";

            /// <summary><c>uri.form.not-url</c></summary>
            public const string NotUrl = Prefix + ".not-url";
        }

        /// <summary>The scheme the URI carries, against a required or a forbidden one.</summary>
        public static class Scheme
        {
            /// <summary>The code prefix for this node (<c>"uri.scheme"</c>).</summary>
            public const string Prefix = Uri.Prefix + ".scheme";

            /// <summary><c>uri.scheme.not-https</c></summary>
            public const string NotHttps = Prefix + ".not-https";

            /// <summary><c>uri.scheme.not-http</c></summary>
            public const string NotHttp = Prefix + ".not-http";

            /// <summary><c>uri.scheme.not-file</c></summary>
            public const string NotFile = Prefix + ".not-file";

            /// <summary><c>uri.scheme.mismatch</c></summary>
            public const string Mismatch = Prefix + ".mismatch";

            /// <summary><c>uri.scheme.match</c></summary>
            public const string Match = Prefix + ".match";
        }

        /// <summary>The local file-system path shape of the string, as opposed to a URI.</summary>
        public static class FilePath
        {
            /// <summary>The code prefix for this node (<c>"uri.file-path"</c>).</summary>
            public const string Prefix = Uri.Prefix + ".file-path";

            /// <summary><c>uri.file-path.invalid</c></summary>
            public const string Invalid = Prefix + ".invalid";

            /// <summary><c>uri.file-path.well-formed</c></summary>
            public const string WellFormed = Prefix + ".well-formed";
        }
    }
}
