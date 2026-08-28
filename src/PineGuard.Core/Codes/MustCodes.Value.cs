namespace PineGuard.Codes;

// Serves: MustNullClauses.cs, MustDefaultEqualityClauses.cs, MustObjectClauses.cs
public static partial class MustCodes
{
    /// <summary>The <c>value</c> domain: nullability, default-equality, and generic object identity/equality/type checks.</summary>
    public static class Value
    {
        /// <summary>The code prefix for this node (<c>"value"</c>).</summary>
        public const string Prefix = "value";

        /// <summary>Nullability and default-equality state.</summary>
        public static class State
        {
            /// <summary>The code prefix for this node (<c>"value.state"</c>).</summary>
            public const string Prefix = Value.Prefix + ".state";

            /// <summary><c>value.state.null</c></summary>
            public const string Null = Prefix + ".null";

            /// <summary><c>value.state.not-null</c></summary>
            public const string NotNull = Prefix + ".not-null";

            /// <summary><c>value.state.default</c></summary>
            public const string Default = Prefix + ".default";

            /// <summary><c>value.state.not-default</c></summary>
            public const string NotDefault = Prefix + ".not-default";

            /// <summary><c>value.state.null-or-default</c></summary>
            public const string NullOrDefault = Prefix + ".null-or-default";

            /// <summary><c>value.state.not-null-or-default</c></summary>
            public const string NotNullOrDefault = Prefix + ".not-null-or-default";
        }

        /// <summary>Value equality against another value.</summary>
        public static class Equality
        {
            /// <summary>The code prefix for this node (<c>"value.equality"</c>).</summary>
            public const string Prefix = Value.Prefix + ".equality";

            /// <summary><c>value.equality.not-equal</c></summary>
            public const string NotEqual = Prefix + ".not-equal";

            /// <summary><c>value.equality.equal</c></summary>
            public const string Equal = Prefix + ".equal";
        }

        /// <summary>Runtime type identity, assignability, and reference identity.</summary>
        public static class Identity
        {
            /// <summary>The code prefix for this node (<c>"value.identity"</c>).</summary>
            public const string Prefix = Value.Prefix + ".identity";

            /// <summary><c>value.identity.wrong-type</c></summary>
            public const string WrongType = Prefix + ".wrong-type";

            /// <summary><c>value.identity.same-type</c></summary>
            public const string SameType = Prefix + ".same-type";

            /// <summary><c>value.identity.not-assignable</c></summary>
            public const string NotAssignable = Prefix + ".not-assignable";

            /// <summary><c>value.identity.assignable</c></summary>
            public const string Assignable = Prefix + ".assignable";

            /// <summary><c>value.identity.not-same-reference</c></summary>
            public const string NotSameReference = Prefix + ".not-same-reference";

            /// <summary><c>value.identity.same-reference</c></summary>
            public const string SameReference = Prefix + ".same-reference";
        }

        /// <summary>
        /// Reserved for adapters that map an argument exception PineGuard did not itself throw
        /// (e.g. a framework binding failure) onto the Must code vocabulary. No clause emits this directly.
        /// </summary>
        public static class Argument
        {
            /// <summary>The code prefix for this node (<c>"value.argument"</c>).</summary>
            public const string Prefix = Value.Prefix + ".argument";

            /// <summary><c>value.argument.invalid</c></summary>
            public const string Invalid = Prefix + ".invalid";
        }
    }
}
