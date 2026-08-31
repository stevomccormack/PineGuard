namespace PineGuard.Codes;

// Serves: MustCronClauses.cs
public static partial class MustCodes
{
    /// <summary>The <c>cron</c> domain: schedules written in cron syntax.</summary>
    public static class Cron
    {
        /// <summary>The code prefix for this node (<c>"cron"</c>).</summary>
        public const string Prefix = "cron";

        /// <summary>The whitespace-separated field list a scheduler reads to decide when to fire.</summary>
        public static class Expression
        {
            /// <summary>The code prefix for this node (<c>"cron.expression"</c>).</summary>
            public const string Prefix = Cron.Prefix + ".expression";

            /// <summary><c>cron.expression.invalid</c></summary>
            public const string Invalid = Prefix + ".invalid";
        }
    }
}
