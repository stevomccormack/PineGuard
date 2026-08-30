namespace PineGuard.Codes;

// Serves: MustIdentifierClauses.cs
public static partial class MustCodes
{
    /// <summary>The <c>identifier</c> domain: human-authored identifiers such as URL slugs.</summary>
    public static class Identifier
    {
        /// <summary>The code prefix for this node (<c>"identifier"</c>).</summary>
        public const string Prefix = "identifier";

        /// <summary>The URL-safe slug form: lowercase letters, digits, and hyphens.</summary>
        public static class Slug
        {
            /// <summary>The code prefix for this node (<c>"identifier.slug"</c>).</summary>
            public const string Prefix = Identifier.Prefix + ".slug";

            /// <summary><c>identifier.slug.invalid</c></summary>
            public const string Invalid = Prefix + ".invalid";
        }

        /// <summary>The ULID form: 26 Crockford base32 characters, lexicographically sortable.</summary>
        public static class Ulid
        {
            /// <summary>The code prefix for this node (<c>"identifier.ulid"</c>).</summary>
            public const string Prefix = Identifier.Prefix + ".ulid";

            /// <summary><c>identifier.ulid.invalid</c></summary>
            public const string Invalid = Prefix + ".invalid";
        }
    }
}
