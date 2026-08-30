namespace PineGuard.Codes;

// Serves: MustFilePathClauses.cs, MustFileSignatureClauses.cs
public static partial class MustCodes
{
    /// <summary>The <c>file</c> domain: file name safety, file extension allow-lists and magic-byte signatures.</summary>
    public static class File
    {
        /// <summary>The code prefix for this node (<c>"file"</c>).</summary>
        public const string Prefix = "file";

        /// <summary>The file name itself — the characters and traversal sequences it carries.</summary>
        public static class Name
        {
            /// <summary>The code prefix for this node (<c>"file.name"</c>).</summary>
            public const string Prefix = File.Prefix + ".name";

            /// <summary><c>file.name.unsafe</c></summary>
            public const string Unsafe = Prefix + ".unsafe";
        }

        /// <summary>The extension the path carries, against the allowed set.</summary>
        public static class Extension
        {
            /// <summary>The code prefix for this node (<c>"file.extension"</c>).</summary>
            public const string Prefix = File.Prefix + ".extension";

            /// <summary><c>file.extension.mismatch</c></summary>
            public const string Mismatch = Prefix + ".mismatch";
        }

        /// <summary>The magic bytes the file leads with, against the extension it claims.</summary>
        public static class Signature
        {
            /// <summary>The code prefix for this node (<c>"file.signature"</c>).</summary>
            public const string Prefix = File.Prefix + ".signature";

            /// <summary><c>file.signature.mismatch</c></summary>
            public const string Mismatch = Prefix + ".mismatch";

            /// <summary><c>file.signature.unknown</c></summary>
            public const string Unknown = Prefix + ".unknown";
        }
    }
}
