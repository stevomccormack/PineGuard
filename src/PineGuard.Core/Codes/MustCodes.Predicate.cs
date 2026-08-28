namespace PineGuard.Codes;

// Serves: MustPredicateClauses.cs
public static partial class MustCodes
{
    /// <summary>The <c>predicate</c> domain: satisfaction of a caller-supplied predicate.</summary>
    public static class Predicate
    {
        /// <summary>The code prefix for this node (<c>"predicate"</c>).</summary>
        public const string Prefix = "predicate";

        /// <summary>The truth value the predicate returned for the input.</summary>
        public static class Result
        {
            /// <summary>The code prefix for this node (<c>"predicate.result"</c>).</summary>
            public const string Prefix = Predicate.Prefix + ".result";

            /// <summary><c>predicate.result.false</c></summary>
            public const string False = Prefix + ".false";

            /// <summary><c>predicate.result.true</c></summary>
            public const string True = Prefix + ".true";
        }

        /// <summary>The predicate delegate the caller supplied, before it is ever invoked.</summary>
        public static class Callback
        {
            /// <summary>The code prefix for this node (<c>"predicate.callback"</c>).</summary>
            public const string Prefix = Predicate.Prefix + ".callback";

            /// <summary><c>predicate.callback.null</c></summary>
            public const string Null = Prefix + ".null";
        }
    }
}
