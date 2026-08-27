using System.Diagnostics.CodeAnalysis;

namespace PineGuard.Codes;

// Serves: MustEmailClauses.cs
public static partial class MustCodes
{
    /// <summary>The <c>email</c> domain: email address validity and sub-address alias presence.</summary>
    public static class Email
    {
        /// <summary>The code prefix for this node (<c>"email"</c>).</summary>
        public const string Prefix = "email";

        /// <summary>The address as a whole, against the pragmatic and the strict grammars.</summary>
        public static class Address
        {
            /// <summary>The code prefix for this node (<c>"email.address"</c>).</summary>
            public const string Prefix = Email.Prefix + ".address";

            /// <summary><c>email.address.invalid</c></summary>
            public const string Invalid = Prefix + ".invalid";

            /// <summary><c>email.address.not-strict</c></summary>
            public const string NotStrict = Prefix + ".not-strict";
        }

        /// <summary>The sub-address alias — the <c>+tag</c> segment of the local part.</summary>
        [SuppressMessage("Naming", "CA1716:Identifiers should not match keywords",
            Justification = "Domain identifiers mirror the public code strings; this node addresses the email alias segment.")]
        public static class Alias
        {
            /// <summary>The code prefix for this node (<c>"email.alias"</c>).</summary>
            public const string Prefix = Email.Prefix + ".alias";

            /// <summary><c>email.alias.missing</c></summary>
            public const string Missing = Prefix + ".missing";

            /// <summary><c>email.alias.present</c></summary>
            public const string Present = Prefix + ".present";
        }
    }
}
