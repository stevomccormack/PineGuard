using System.Diagnostics.CodeAnalysis;

namespace PineGuard.Codes;

// Serves: MustBoolClauses.cs, MustStringBoolClauses.cs
public static partial class MustCodes
{
    /// <summary>The <c>boolean</c> domain: truth-value checks on booleans and on their string representations.</summary>
    [SuppressMessage("Naming", "CA1716:Identifiers should not match keywords",
        Justification = "Domain identifiers mirror the public code strings; the domain of these codes is 'boolean'.")]
    public static class Boolean
    {
        /// <summary>The code prefix for this node (<c>"boolean"</c>).</summary>
        public const string Prefix = "boolean";

        /// <summary>The truth value the input carries, whatever type it arrived as.</summary>
        public static class Value
        {
            /// <summary>The code prefix for this node (<c>"boolean.value"</c>).</summary>
            public const string Prefix = Boolean.Prefix + ".value";

            /// <summary><c>boolean.value.false</c></summary>
            public const string False = Prefix + ".false";

            /// <summary><c>boolean.value.true</c></summary>
            public const string True = Prefix + ".true";
        }
    }
}
