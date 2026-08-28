namespace PineGuard.Codes;

// Serves: MustOwaspClauses.cs
public static partial class MustCodes
{
    /// <summary>The <c>owasp</c> domain: injection and redirection attack surfaces found in untrusted input.</summary>
    public static class Owasp
    {
        /// <summary>The code prefix for this node (<c>"owasp"</c>).</summary>
        public const string Prefix = "owasp";

        /// <summary>The input taken as a whole, across every attack surface below.</summary>
        public static class Payload
        {
            /// <summary>The code prefix for this node (<c>"owasp.payload"</c>).</summary>
            public const string Prefix = Owasp.Prefix + ".payload";

            /// <summary><c>owasp.payload.unsafe</c></summary>
            public const string Unsafe = Prefix + ".unsafe";
        }

        /// <summary>Cross-site scripting vectors carried by the input.</summary>
        public static class Xss
        {
            /// <summary>The code prefix for this node (<c>"owasp.xss"</c>).</summary>
            public const string Prefix = Owasp.Prefix + ".xss";

            /// <summary><c>owasp.xss.unsafe</c></summary>
            public const string Unsafe = Prefix + ".unsafe";
        }

        /// <summary>SQL injection vectors carried by the input.</summary>
        public static class SqlInjection
        {
            /// <summary>The code prefix for this node (<c>"owasp.sql-injection"</c>).</summary>
            public const string Prefix = Owasp.Prefix + ".sql-injection";

            /// <summary><c>owasp.sql-injection.unsafe</c></summary>
            public const string Unsafe = Prefix + ".unsafe";
        }

        /// <summary>Directory-escape sequences carried by the input.</summary>
        public static class PathTraversal
        {
            /// <summary>The code prefix for this node (<c>"owasp.path-traversal"</c>).</summary>
            public const string Prefix = Owasp.Prefix + ".path-traversal";

            /// <summary><c>owasp.path-traversal.unsafe</c></summary>
            public const string Unsafe = Prefix + ".unsafe";
        }

        /// <summary>Shell command injection vectors carried by the input.</summary>
        public static class CommandInjection
        {
            /// <summary>The code prefix for this node (<c>"owasp.command-injection"</c>).</summary>
            public const string Prefix = Owasp.Prefix + ".command-injection";

            /// <summary><c>owasp.command-injection.unsafe</c></summary>
            public const string Unsafe = Prefix + ".unsafe";
        }

        /// <summary>Carriage-return and line-feed sequences that split headers or log lines.</summary>
        public static class Crlf
        {
            /// <summary>The code prefix for this node (<c>"owasp.crlf"</c>).</summary>
            public const string Prefix = Owasp.Prefix + ".crlf";

            /// <summary><c>owasp.crlf.unsafe</c></summary>
            public const string Unsafe = Prefix + ".unsafe";
        }

        /// <summary>LDAP search-filter metacharacters carried by the input.</summary>
        public static class LdapFilter
        {
            /// <summary>The code prefix for this node (<c>"owasp.ldap-filter"</c>).</summary>
            public const string Prefix = Owasp.Prefix + ".ldap-filter";

            /// <summary><c>owasp.ldap-filter.unsafe</c></summary>
            public const string Unsafe = Prefix + ".unsafe";
        }

        /// <summary>Redirect targets that hand control to an attacker-chosen host.</summary>
        public static class OpenRedirect
        {
            /// <summary>The code prefix for this node (<c>"owasp.open-redirect"</c>).</summary>
            public const string Prefix = Owasp.Prefix + ".open-redirect";

            /// <summary><c>owasp.open-redirect.unsafe</c></summary>
            public const string Unsafe = Prefix + ".unsafe";
        }

        /// <summary>URL schemes that let a server-side fetch reach an unintended target.</summary>
        public static class SsrfScheme
        {
            /// <summary>The code prefix for this node (<c>"owasp.ssrf-scheme"</c>).</summary>
            public const string Prefix = Owasp.Prefix + ".ssrf-scheme";

            /// <summary><c>owasp.ssrf-scheme.unsafe</c></summary>
            public const string Unsafe = Prefix + ".unsafe";
        }
    }
}
