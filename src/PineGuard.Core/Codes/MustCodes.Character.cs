namespace PineGuard.Codes;

// Serves: MustCharClauses.cs
public static partial class MustCodes
{
    /// <summary>The <c>character</c> domain: repertoire, Unicode category, and letter case of a single character.</summary>
    public static class Character
    {
        /// <summary>The code prefix for this node (<c>"character"</c>).</summary>
        public const string Prefix = "character";

        /// <summary>The character repertoire the value belongs to (letters, digits, ASCII, hexadecimal).</summary>
        public static class Charset
        {
            /// <summary>The code prefix for this node (<c>"character.charset"</c>).</summary>
            public const string Prefix = Character.Prefix + ".charset";

            /// <summary><c>character.charset.not-letter</c></summary>
            public const string NotLetter = Prefix + ".not-letter";

            /// <summary><c>character.charset.letter</c></summary>
            public const string Letter = Prefix + ".letter";

            /// <summary><c>character.charset.not-digit</c></summary>
            public const string NotDigit = Prefix + ".not-digit";

            /// <summary><c>character.charset.digit</c></summary>
            public const string Digit = Prefix + ".digit";

            /// <summary><c>character.charset.not-letter-or-digit</c></summary>
            public const string NotLetterOrDigit = Prefix + ".not-letter-or-digit";

            /// <summary><c>character.charset.letter-or-digit</c></summary>
            public const string LetterOrDigit = Prefix + ".letter-or-digit";

            /// <summary><c>character.charset.not-ascii</c></summary>
            public const string NotAscii = Prefix + ".not-ascii";

            /// <summary><c>character.charset.ascii</c></summary>
            public const string Ascii = Prefix + ".ascii";

            /// <summary><c>character.charset.not-printable-ascii</c></summary>
            public const string NotPrintableAscii = Prefix + ".not-printable-ascii";

            /// <summary><c>character.charset.printable-ascii</c></summary>
            public const string PrintableAscii = Prefix + ".printable-ascii";

            /// <summary><c>character.charset.not-hex-digit</c></summary>
            public const string NotHexDigit = Prefix + ".not-hex-digit";

            /// <summary><c>character.charset.hex-digit</c></summary>
            public const string HexDigit = Prefix + ".hex-digit";
        }

        /// <summary>The Unicode general category of the value.</summary>
        public static class Category
        {
            /// <summary>The code prefix for this node (<c>"character.category"</c>).</summary>
            public const string Prefix = Character.Prefix + ".category";

            /// <summary><c>character.category.whitespace</c></summary>
            public const string Whitespace = Prefix + ".whitespace";

            /// <summary><c>character.category.not-control</c></summary>
            public const string NotControl = Prefix + ".not-control";

            /// <summary><c>character.category.control</c></summary>
            public const string Control = Prefix + ".control";
        }

        /// <summary>The letter case of the value.</summary>
        public static class Casing
        {
            /// <summary>The code prefix for this node (<c>"character.casing"</c>).</summary>
            public const string Prefix = Character.Prefix + ".casing";

            /// <summary><c>character.casing.not-upper</c></summary>
            public const string NotUpper = Prefix + ".not-upper";

            /// <summary><c>character.casing.not-lower</c></summary>
            public const string NotLower = Prefix + ".not-lower";
        }
    }
}
