using System.Diagnostics.CodeAnalysis;

namespace PineGuard.Codes;

// Serves: MustEnumClauses.cs
public static partial class MustCodes
{
    /// <summary>The <c>enum</c> domain: definedness of a member, the flag bits it carries, and the metadata declared on it.</summary>
    [SuppressMessage("Naming", "CA1716:Identifiers should not match keywords",
        Justification = "Domain identifiers mirror the public code strings; the domain of these codes is 'enum'.")]
    [SuppressMessage("Naming", "CA1711:Identifiers should not have incorrect suffix",
        Justification = "Domain identifiers mirror the public code strings; this is the 'enum' domain, not a System.Enum subtype.")]
    public static class Enum
    {
        /// <summary>The code prefix for this node (<c>"enum"</c>).</summary>
        public const string Prefix = "enum";

        /// <summary>Whether the enum value corresponds to a declared member.</summary>
        public static class Value
        {
            /// <summary>The code prefix for this node (<c>"enum.value"</c>).</summary>
            public const string Prefix = Enum.Prefix + ".value";

            /// <summary><c>enum.value.not-defined</c></summary>
            public const string NotDefined = Prefix + ".not-defined";

            /// <summary><c>enum.value.defined</c></summary>
            public const string Defined = Prefix + ".defined";
        }

        /// <summary>Whether the numeric backing value maps onto a declared member.</summary>
        public static class BackingValue
        {
            /// <summary>The code prefix for this node (<c>"enum.backing-value"</c>).</summary>
            public const string Prefix = Enum.Prefix + ".backing-value";

            /// <summary><c>enum.backing-value.not-defined</c></summary>
            public const string NotDefined = Prefix + ".not-defined";

            /// <summary><c>enum.backing-value.defined</c></summary>
            public const string Defined = Prefix + ".defined";
        }

        /// <summary>Whether the text matches the name of a declared member.</summary>
        public static class Name
        {
            /// <summary>The code prefix for this node (<c>"enum.name"</c>).</summary>
            public const string Prefix = Enum.Prefix + ".name";

            /// <summary><c>enum.name.not-defined</c></summary>
            public const string NotDefined = Prefix + ".not-defined";

            /// <summary><c>enum.name.defined</c></summary>
            public const string Defined = Prefix + ".defined";
        }

        /// <summary>The flag bits the value carries: whether every set bit is declared, and whether one asked-for flag is set.</summary>
        public static class Flags
        {
            /// <summary>The code prefix for this node (<c>"enum.flags"</c>).</summary>
            public const string Prefix = Enum.Prefix + ".flags";

            /// <summary><c>enum.flags.not-defined</c></summary>
            public const string NotDefined = Prefix + ".not-defined";

            /// <summary><c>enum.flags.defined</c></summary>
            public const string Defined = Prefix + ".defined";

            /// <summary><c>enum.flags.not-set</c></summary>
            public const string NotSet = Prefix + ".not-set";

            /// <summary><c>enum.flags.set</c></summary>
            public const string Set = Prefix + ".set";
        }

        /// <summary>The caller-supplied attribute type on the declared member.</summary>
        [SuppressMessage("Naming", "CA1711:Identifiers should not have incorrect suffix",
            Justification = "Domain identifiers mirror the public code strings; this node addresses a caller-supplied attribute type, not an Attribute subtype itself.")]
        public static class Attribute
        {
            /// <summary>The code prefix for this node (<c>"enum.attribute"</c>).</summary>
            public const string Prefix = Enum.Prefix + ".attribute";

            /// <summary><c>enum.attribute.missing</c></summary>
            public const string Missing = Prefix + ".missing";

            /// <summary><c>enum.attribute.present</c></summary>
            public const string Present = Prefix + ".present";
        }

        /// <summary>The <c>[Description]</c> attribute on the declared member.</summary>
        public static class Description
        {
            /// <summary>The code prefix for this node (<c>"enum.description"</c>).</summary>
            public const string Prefix = Enum.Prefix + ".description";

            /// <summary><c>enum.description.missing</c></summary>
            public const string Missing = Prefix + ".missing";

            /// <summary><c>enum.description.present</c></summary>
            public const string Present = Prefix + ".present";
        }

        /// <summary>The <c>[Display]</c> attribute on the declared member.</summary>
        public static class Display
        {
            /// <summary>The code prefix for this node (<c>"enum.display"</c>).</summary>
            public const string Prefix = Enum.Prefix + ".display";

            /// <summary><c>enum.display.missing</c></summary>
            public const string Missing = Prefix + ".missing";

            /// <summary><c>enum.display.present</c></summary>
            public const string Present = Prefix + ".present";
        }

        /// <summary>The <c>[EnumMember]</c> serialization attribute on the declared member.</summary>
        [SuppressMessage("Naming", "CA1711:Identifiers should not have incorrect suffix",
            Justification = "Domain identifiers mirror the public code strings; this node addresses the [EnumMember] attribute, not an Attribute subtype itself.")]
        public static class MemberAttribute
        {
            /// <summary>The code prefix for this node (<c>"enum.member-attribute"</c>).</summary>
            public const string Prefix = Enum.Prefix + ".member-attribute";

            /// <summary><c>enum.member-attribute.missing</c></summary>
            public const string Missing = Prefix + ".missing";

            /// <summary><c>enum.member-attribute.present</c></summary>
            public const string Present = Prefix + ".present";
        }

        /// <summary>The <c>[Obsolete]</c> marker on the declared member.</summary>
        public static class Obsolescence
        {
            /// <summary>The code prefix for this node (<c>"enum.obsolescence"</c>).</summary>
            public const string Prefix = Enum.Prefix + ".obsolescence";

            /// <summary><c>enum.obsolescence.missing</c></summary>
            public const string Missing = Prefix + ".missing";

            /// <summary><c>enum.obsolescence.present</c></summary>
            public const string Present = Prefix + ".present";
        }
    }
}
