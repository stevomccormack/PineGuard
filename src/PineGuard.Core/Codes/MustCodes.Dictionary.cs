using System.Diagnostics.CodeAnalysis;

namespace PineGuard.Codes;

// Serves: MustDictionaryClauses.cs, MustReadOnlyDictionaryClauses.cs
public static partial class MustCodes
{
    /// <summary>The <c>dictionary</c> domain: key presence, value presence, and entry membership of a mutable or read-only dictionary.</summary>
    [SuppressMessage("Naming", "CA1711:Identifiers should not have incorrect suffix",
        Justification = "Domain identifiers mirror the public code strings; the domain of these codes is 'dictionary', not an IDictionary implementation.")]
    public static class Dictionary
    {
        /// <summary>The code prefix for this node (<c>"dictionary"</c>).</summary>
        public const string Prefix = "dictionary";

        /// <summary>The keys the dictionary exposes, looked up by equality or by predicate.</summary>
        public static class Keys
        {
            /// <summary>The code prefix for this node (<c>"dictionary.keys"</c>).</summary>
            public const string Prefix = Dictionary.Prefix + ".keys";

            /// <summary><c>dictionary.keys.missing</c></summary>
            public const string Missing = Prefix + ".missing";

            /// <summary><c>dictionary.keys.present</c></summary>
            public const string Present = Prefix + ".present";

            /// <summary><c>dictionary.keys.no-match</c></summary>
            public const string NoMatch = Prefix + ".no-match";

            /// <summary><c>dictionary.keys.match</c></summary>
            public const string Match = Prefix + ".match";
        }

        /// <summary>The values the dictionary exposes, looked up by equality or by predicate.</summary>
        public static class Values
        {
            /// <summary>The code prefix for this node (<c>"dictionary.values"</c>).</summary>
            public const string Prefix = Dictionary.Prefix + ".values";

            /// <summary><c>dictionary.values.missing</c></summary>
            public const string Missing = Prefix + ".missing";

            /// <summary><c>dictionary.values.present</c></summary>
            public const string Present = Prefix + ".present";

            /// <summary><c>dictionary.values.no-match</c></summary>
            public const string NoMatch = Prefix + ".no-match";

            /// <summary><c>dictionary.values.match</c></summary>
            public const string Match = Prefix + ".match";
        }

        /// <summary>The dictionary's entries as key/value pairs, including whether it holds any at all.</summary>
        public static class Items
        {
            /// <summary>The code prefix for this node (<c>"dictionary.items"</c>).</summary>
            public const string Prefix = Dictionary.Prefix + ".items";

            /// <summary><c>dictionary.items.not-empty</c></summary>
            public const string NotEmpty = Prefix + ".not-empty";

            /// <summary><c>dictionary.items.empty</c></summary>
            public const string Empty = Prefix + ".empty";

            /// <summary><c>dictionary.items.missing</c></summary>
            public const string Missing = Prefix + ".missing";

            /// <summary><c>dictionary.items.present</c></summary>
            public const string Present = Prefix + ".present";

            /// <summary><c>dictionary.items.no-match</c></summary>
            public const string NoMatch = Prefix + ".no-match";

            /// <summary><c>dictionary.items.match</c></summary>
            public const string Match = Prefix + ".match";
        }
    }
}
