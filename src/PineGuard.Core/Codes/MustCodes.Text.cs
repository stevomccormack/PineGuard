namespace PineGuard.Codes;

// Serves: MustStringClauses.cs, MustStringCasingClauses.cs
public static partial class MustCodes
{
    /// <summary>The <c>text</c> domain: string content, length, casing, character set, and pattern checks.</summary>
    public static class Text
    {
        /// <summary>The code prefix for this node (<c>"text"</c>).</summary>
        public const string Prefix = "text";

        /// <summary>Presence of usable content: null, empty, whitespace-only, and the substrings the text carries.</summary>
        public static class Content
        {
            /// <summary>The code prefix for this node (<c>"text.content"</c>).</summary>
            public const string Prefix = Text.Prefix + ".content";

            /// <summary><c>text.content.null-or-empty</c></summary>
            public const string NullOrEmpty = Prefix + ".null-or-empty";

            /// <summary><c>text.content.not-null-or-empty</c></summary>
            public const string NotNullOrEmpty = Prefix + ".not-null-or-empty";

            /// <summary><c>text.content.blank</c></summary>
            public const string Blank = Prefix + ".blank";

            /// <summary><c>text.content.not-blank</c></summary>
            public const string NotBlank = Prefix + ".not-blank";

            /// <summary><c>text.content.empty</c></summary>
            public const string Empty = Prefix + ".empty";

            /// <summary><c>text.content.not-empty</c></summary>
            public const string NotEmpty = Prefix + ".not-empty";

            /// <summary><c>text.content.whitespace</c></summary>
            public const string Whitespace = Prefix + ".whitespace";

            /// <summary><c>text.content.not-contains</c></summary>
            public const string NotContains = Prefix + ".not-contains";

            /// <summary><c>text.content.contains</c></summary>
            public const string Contains = Prefix + ".contains";

            /// <summary><c>text.content.not-starts-with</c></summary>
            public const string NotStartsWith = Prefix + ".not-starts-with";

            /// <summary><c>text.content.starts-with</c></summary>
            public const string StartsWith = Prefix + ".starts-with";

            /// <summary><c>text.content.not-ends-with</c></summary>
            public const string NotEndsWith = Prefix + ".not-ends-with";

            /// <summary><c>text.content.ends-with</c></summary>
            public const string EndsWith = Prefix + ".ends-with";
        }

        /// <summary>Character count against an exact length, a range, or a bound.</summary>
        public static class Length
        {
            /// <summary>The code prefix for this node (<c>"text.length"</c>).</summary>
            public const string Prefix = Text.Prefix + ".length";

            /// <summary><c>text.length.mismatch</c></summary>
            public const string Mismatch = Prefix + ".mismatch";

            /// <summary><c>text.length.out-of-range</c></summary>
            public const string OutOfRange = Prefix + ".out-of-range";

            /// <summary><c>text.length.too-short</c></summary>
            public const string TooShort = Prefix + ".too-short";

            /// <summary><c>text.length.too-long</c></summary>
            public const string TooLong = Prefix + ".too-long";
        }

        /// <summary>Letter case and the naming-convention case styles.</summary>
        public static class Casing
        {
            /// <summary>The code prefix for this node (<c>"text.casing"</c>).</summary>
            public const string Prefix = Text.Prefix + ".casing";

            /// <summary><c>text.casing.not-upper</c></summary>
            public const string NotUpper = Prefix + ".not-upper";

            /// <summary><c>text.casing.upper</c></summary>
            public const string Upper = Prefix + ".upper";

            /// <summary><c>text.casing.not-lower</c></summary>
            public const string NotLower = Prefix + ".not-lower";

            /// <summary><c>text.casing.lower</c></summary>
            public const string Lower = Prefix + ".lower";

            /// <summary><c>text.casing.mismatch</c></summary>
            public const string Mismatch = Prefix + ".mismatch";

            /// <summary><c>text.casing.match</c></summary>
            public const string Match = Prefix + ".match";

            /// <summary><c>text.casing.not-camel</c></summary>
            public const string NotCamel = Prefix + ".not-camel";

            /// <summary><c>text.casing.camel</c></summary>
            public const string Camel = Prefix + ".camel";

            /// <summary><c>text.casing.not-pascal</c></summary>
            public const string NotPascal = Prefix + ".not-pascal";

            /// <summary><c>text.casing.pascal</c></summary>
            public const string Pascal = Prefix + ".pascal";

            /// <summary><c>text.casing.not-snake</c></summary>
            public const string NotSnake = Prefix + ".not-snake";

            /// <summary><c>text.casing.snake</c></summary>
            public const string Snake = Prefix + ".snake";

            /// <summary><c>text.casing.not-upper-snake</c></summary>
            public const string NotUpperSnake = Prefix + ".not-upper-snake";

            /// <summary><c>text.casing.upper-snake</c></summary>
            public const string UpperSnake = Prefix + ".upper-snake";

            /// <summary><c>text.casing.not-kebab</c></summary>
            public const string NotKebab = Prefix + ".not-kebab";

            /// <summary><c>text.casing.kebab</c></summary>
            public const string Kebab = Prefix + ".kebab";

            /// <summary><c>text.casing.not-train</c></summary>
            public const string NotTrain = Prefix + ".not-train";

            /// <summary><c>text.casing.train</c></summary>
            public const string Train = Prefix + ".train";

            /// <summary><c>text.casing.not-dot</c></summary>
            public const string NotDot = Prefix + ".not-dot";

            /// <summary><c>text.casing.dot</c></summary>
            public const string Dot = Prefix + ".dot";

            /// <summary><c>text.casing.not-space</c></summary>
            public const string NotSpace = Prefix + ".not-space";

            /// <summary><c>text.casing.space</c></summary>
            public const string Space = Prefix + ".space";

            /// <summary><c>text.casing.not-upper-invariant</c></summary>
            public const string NotUpperInvariant = Prefix + ".not-upper-invariant";

            /// <summary><c>text.casing.upper-invariant</c></summary>
            public const string UpperInvariant = Prefix + ".upper-invariant";

            /// <summary><c>text.casing.not-lower-invariant</c></summary>
            public const string NotLowerInvariant = Prefix + ".not-lower-invariant";

            /// <summary><c>text.casing.lower-invariant</c></summary>
            public const string LowerInvariant = Prefix + ".lower-invariant";
        }

        /// <summary>The characters the text is composed of: classes, allowed sets, and disallowed sets.</summary>
        public static class Charset
        {
            /// <summary>The code prefix for this node (<c>"text.charset"</c>).</summary>
            public const string Prefix = Text.Prefix + ".charset";

            /// <summary><c>text.charset.not-alpha</c></summary>
            public const string NotAlpha = Prefix + ".not-alpha";

            /// <summary><c>text.charset.alpha</c></summary>
            public const string Alpha = Prefix + ".alpha";

            /// <summary><c>text.charset.not-numeric</c></summary>
            public const string NotNumeric = Prefix + ".not-numeric";

            /// <summary><c>text.charset.numeric</c></summary>
            public const string Numeric = Prefix + ".numeric";

            /// <summary><c>text.charset.not-alphanumeric</c></summary>
            public const string NotAlphanumeric = Prefix + ".not-alphanumeric";

            /// <summary><c>text.charset.alphanumeric</c></summary>
            public const string Alphanumeric = Prefix + ".alphanumeric";

            /// <summary><c>text.charset.not-digits</c></summary>
            public const string NotDigits = Prefix + ".not-digits";

            /// <summary><c>text.charset.digits</c></summary>
            public const string Digits = Prefix + ".digits";

            /// <summary><c>text.charset.not-ascii</c></summary>
            public const string NotAscii = Prefix + ".not-ascii";

            /// <summary><c>text.charset.ascii</c></summary>
            public const string Ascii = Prefix + ".ascii";

            /// <summary><c>text.charset.not-printable</c></summary>
            public const string NotPrintable = Prefix + ".not-printable";

            /// <summary><c>text.charset.printable</c></summary>
            public const string Printable = Prefix + ".printable";

            /// <summary><c>text.charset.not-contains-whitespace</c></summary>
            public const string NotContainsWhitespace = Prefix + ".not-contains-whitespace";

            /// <summary><c>text.charset.contains-whitespace</c></summary>
            public const string ContainsWhitespace = Prefix + ".contains-whitespace";

            /// <summary><c>text.charset.not-contains-control</c></summary>
            public const string NotContainsControl = Prefix + ".not-contains-control";

            /// <summary><c>text.charset.contains-control</c></summary>
            public const string ContainsControl = Prefix + ".contains-control";

            /// <summary><c>text.charset.not-subset</c></summary>
            public const string NotSubset = Prefix + ".not-subset";

            /// <summary><c>text.charset.subset</c></summary>
            public const string Subset = Prefix + ".subset";

            /// <summary><c>text.charset.not-contains-disallowed</c></summary>
            public const string NotContainsDisallowed = Prefix + ".not-contains-disallowed";

            /// <summary><c>text.charset.contains-disallowed</c></summary>
            public const string ContainsDisallowed = Prefix + ".contains-disallowed";

            /// <summary><c>text.charset.not-contains-any</c></summary>
            public const string NotContainsAny = Prefix + ".not-contains-any";
        }

        /// <summary>Regular-expression matching.</summary>
        public static class Pattern
        {
            /// <summary>The code prefix for this node (<c>"text.pattern"</c>).</summary>
            public const string Prefix = Text.Prefix + ".pattern";

            /// <summary><c>text.pattern.no-match</c></summary>
            public const string NoMatch = Prefix + ".no-match";

            /// <summary><c>text.pattern.match</c></summary>
            public const string Match = Prefix + ".match";
        }

        /// <summary>The leading Unicode byte-order mark (<c>U+FEFF</c>) the text carries.</summary>
        public static class Bom
        {
            /// <summary>The code prefix for this node (<c>"text.bom"</c>).</summary>
            public const string Prefix = Text.Prefix + ".bom";

            /// <summary><c>text.bom.missing</c></summary>
            public const string Missing = Prefix + ".missing";

            /// <summary><c>text.bom.present</c></summary>
            public const string Present = Prefix + ".present";
        }

        /// <summary>Unicode integrity: surrogate pairing and normalization form.</summary>
        public static class Unicode
        {
            /// <summary>The code prefix for this node (<c>"text.unicode"</c>).</summary>
            public const string Prefix = Text.Prefix + ".unicode";

            /// <summary><c>text.unicode.malformed</c></summary>
            public const string Malformed = Prefix + ".malformed";

            /// <summary><c>text.unicode.well-formed</c></summary>
            public const string WellFormed = Prefix + ".well-formed";

            /// <summary><c>text.unicode.not-normalized</c></summary>
            public const string NotNormalized = Prefix + ".not-normalized";

            /// <summary><c>text.unicode.normalized</c></summary>
            public const string Normalized = Prefix + ".normalized";
        }
    }
}
