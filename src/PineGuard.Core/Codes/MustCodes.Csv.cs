namespace PineGuard.Codes;

// Serves: MustCsvClauses.cs
public static partial class MustCodes
{
    /// <summary>The <c>csv</c> domain: delimited-line well-formedness, header agreement, and row conformance to a schema.</summary>
    public static class Csv
    {
        /// <summary>The code prefix for this node (<c>"csv"</c>).</summary>
        public const string Prefix = "csv";

        /// <summary>Well-formedness of a single delimited line, whatever it carries.</summary>
        public static class Line
        {
            /// <summary>The code prefix for this node (<c>"csv.line"</c>).</summary>
            public const string Prefix = Csv.Prefix + ".line";

            /// <summary><c>csv.line.invalid</c></summary>
            public const string Invalid = Prefix + ".invalid";
        }

        /// <summary>The header line and the column names it declares.</summary>
        public static class Header
        {
            /// <summary>The code prefix for this node (<c>"csv.header"</c>).</summary>
            public const string Prefix = Csv.Prefix + ".header";

            /// <summary><c>csv.header.invalid</c></summary>
            public const string Invalid = Prefix + ".invalid";
        }

        /// <summary>A data row and its conformance to the declared columns and column types.</summary>
        public static class Row
        {
            /// <summary>The code prefix for this node (<c>"csv.row"</c>).</summary>
            public const string Prefix = Csv.Prefix + ".row";

            /// <summary><c>csv.row.invalid</c></summary>
            public const string Invalid = Prefix + ".invalid";
        }
    }
}
