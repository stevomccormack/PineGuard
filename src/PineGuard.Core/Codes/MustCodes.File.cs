namespace PineGuard.Codes;

// Serves: MustFilePathClauses.cs
public static partial class MustCodes
{
    /// <summary>The <c>file</c> domain: file name safety and file extension allow-lists.</summary>
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
    }
}
