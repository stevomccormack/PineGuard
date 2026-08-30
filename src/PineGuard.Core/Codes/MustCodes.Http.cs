namespace PineGuard.Codes;

// Serves: MustHttpClauses.cs, MustHttpSecurityHeaderClauses.cs
public static partial class MustCodes
{
    /// <summary>The <c>http</c> domain: header names, header values, status codes, content types, and the security response headers.</summary>
    public static class Http
    {
        /// <summary>The code prefix for this node (<c>"http"</c>).</summary>
        public const string Prefix = "http";

        /// <summary>The syntax of a header field name as an RFC 7230 token.</summary>
        public static class HeaderName
        {
            /// <summary>The code prefix for this node (<c>"http.header-name"</c>).</summary>
            public const string Prefix = Http.Prefix + ".header-name";

            /// <summary><c>http.header-name.malformed</c></summary>
            public const string Malformed = Prefix + ".malformed";

            /// <summary><c>http.header-name.well-formed</c></summary>
            public const string WellFormed = Prefix + ".well-formed";
        }

        /// <summary>The value carried by a header field: its syntax, its presence, its multiplicity, and what it equals.</summary>
        public static class HeaderValue
        {
            /// <summary>The code prefix for this node (<c>"http.header-value"</c>).</summary>
            public const string Prefix = Http.Prefix + ".header-value";

            /// <summary><c>http.header-value.malformed</c></summary>
            public const string Malformed = Prefix + ".malformed";

            /// <summary><c>http.header-value.well-formed</c></summary>
            public const string WellFormed = Prefix + ".well-formed";

            /// <summary><c>http.header-value.missing</c></summary>
            public const string Missing = Prefix + ".missing";

            /// <summary><c>http.header-value.present</c></summary>
            public const string Present = Prefix + ".present";

            /// <summary><c>http.header-value.mismatch</c></summary>
            public const string Mismatch = Prefix + ".mismatch";

            /// <summary><c>http.header-value.match</c></summary>
            public const string Match = Prefix + ".match";

            /// <summary><c>http.header-value.not-single</c></summary>
            public const string NotSingle = Prefix + ".not-single";

            /// <summary><c>http.header-value.single</c></summary>
            public const string Single = Prefix + ".single";
        }

        /// <summary>The response status code and the class (1xx–5xx) it belongs to.</summary>
        public static class Status
        {
            /// <summary>The code prefix for this node (<c>"http.status"</c>).</summary>
            public const string Prefix = Http.Prefix + ".status";

            /// <summary><c>http.status.out-of-range</c></summary>
            public const string OutOfRange = Prefix + ".out-of-range";

            /// <summary><c>http.status.in-range</c></summary>
            public const string InRange = Prefix + ".in-range";

            /// <summary><c>http.status.not-informational</c></summary>
            public const string NotInformational = Prefix + ".not-informational";

            /// <summary><c>http.status.informational</c></summary>
            public const string Informational = Prefix + ".informational";

            /// <summary><c>http.status.not-success</c></summary>
            public const string NotSuccess = Prefix + ".not-success";

            /// <summary><c>http.status.success</c></summary>
            public const string Success = Prefix + ".success";

            /// <summary><c>http.status.not-redirect</c></summary>
            public const string NotRedirect = Prefix + ".not-redirect";

            /// <summary><c>http.status.redirect</c></summary>
            public const string Redirect = Prefix + ".redirect";

            /// <summary><c>http.status.not-client-error</c></summary>
            public const string NotClientError = Prefix + ".not-client-error";

            /// <summary><c>http.status.client-error</c></summary>
            public const string ClientError = Prefix + ".client-error";

            /// <summary><c>http.status.not-server-error</c></summary>
            public const string NotServerError = Prefix + ".not-server-error";

            /// <summary><c>http.status.server-error</c></summary>
            public const string ServerError = Prefix + ".server-error";
        }

        /// <summary>Membership of a named header in a header collection.</summary>
        public static class Header
        {
            /// <summary>The code prefix for this node (<c>"http.header"</c>).</summary>
            public const string Prefix = Http.Prefix + ".header";

            /// <summary><c>http.header.missing</c></summary>
            public const string Missing = Prefix + ".missing";

            /// <summary><c>http.header.present</c></summary>
            public const string Present = Prefix + ".present";
        }

        /// <summary>The media type advertised by the <c>Content-Type</c> header, against an allow-list.</summary>
        public static class ContentType
        {
            /// <summary>The code prefix for this node (<c>"http.content-type"</c>).</summary>
            public const string Prefix = Http.Prefix + ".content-type";

            /// <summary><c>http.content-type.not-allowed</c></summary>
            public const string NotAllowed = Prefix + ".not-allowed";

            /// <summary><c>http.content-type.allowed</c></summary>
            public const string Allowed = Prefix + ".allowed";
        }

        /// <summary>The <c>type/subtype</c> form of a media type, independent of any header that carries it.</summary>
        public static class MediaType
        {
            /// <summary>The code prefix for this node (<c>"http.media-type"</c>).</summary>
            public const string Prefix = Http.Prefix + ".media-type";

            /// <summary><c>http.media-type.invalid</c></summary>
            public const string Invalid = Prefix + ".invalid";
        }

        /// <summary>The <c>Content-Security-Policy</c> header: its presence and the strength of its directives.</summary>
        public static class ContentSecurityPolicy
        {
            /// <summary>The code prefix for this node (<c>"http.content-security-policy"</c>).</summary>
            public const string Prefix = Http.Prefix + ".content-security-policy";

            /// <summary><c>http.content-security-policy.missing</c></summary>
            public const string Missing = Prefix + ".missing";

            /// <summary><c>http.content-security-policy.present</c></summary>
            public const string Present = Prefix + ".present";

            /// <summary><c>http.content-security-policy.weak</c></summary>
            public const string Weak = Prefix + ".weak";

            /// <summary><c>http.content-security-policy.strong</c></summary>
            public const string Strong = Prefix + ".strong";
        }

        /// <summary>The <c>Strict-Transport-Security</c> header: its presence and the strength of its <c>max-age</c>, <c>includeSubDomains</c>, and <c>preload</c> directives.</summary>
        public static class StrictTransportSecurity
        {
            /// <summary>The code prefix for this node (<c>"http.strict-transport-security"</c>).</summary>
            public const string Prefix = Http.Prefix + ".strict-transport-security";

            /// <summary><c>http.strict-transport-security.missing</c></summary>
            public const string Missing = Prefix + ".missing";

            /// <summary><c>http.strict-transport-security.present</c></summary>
            public const string Present = Prefix + ".present";

            /// <summary><c>http.strict-transport-security.weak</c></summary>
            public const string Weak = Prefix + ".weak";

            /// <summary><c>http.strict-transport-security.strong</c></summary>
            public const string Strong = Prefix + ".strong";
        }

        /// <summary>The <c>X-Content-Type-Options</c> header: its presence and whether its value is the expected one.</summary>
        public static class ContentTypeOptions
        {
            /// <summary>The code prefix for this node (<c>"http.content-type-options"</c>).</summary>
            public const string Prefix = Http.Prefix + ".content-type-options";

            /// <summary><c>http.content-type-options.missing</c></summary>
            public const string Missing = Prefix + ".missing";

            /// <summary><c>http.content-type-options.present</c></summary>
            public const string Present = Prefix + ".present";

            /// <summary><c>http.content-type-options.mismatch</c></summary>
            public const string Mismatch = Prefix + ".mismatch";

            /// <summary><c>http.content-type-options.match</c></summary>
            public const string Match = Prefix + ".match";
        }

        /// <summary>The <c>X-Frame-Options</c> header: its presence and whether its value is the expected one.</summary>
        public static class FrameOptions
        {
            /// <summary>The code prefix for this node (<c>"http.frame-options"</c>).</summary>
            public const string Prefix = Http.Prefix + ".frame-options";

            /// <summary><c>http.frame-options.missing</c></summary>
            public const string Missing = Prefix + ".missing";

            /// <summary><c>http.frame-options.present</c></summary>
            public const string Present = Prefix + ".present";

            /// <summary><c>http.frame-options.mismatch</c></summary>
            public const string Mismatch = Prefix + ".mismatch";

            /// <summary><c>http.frame-options.match</c></summary>
            public const string Match = Prefix + ".match";
        }

        /// <summary>The <c>Referrer-Policy</c> header: its presence and whether its value is the expected one.</summary>
        public static class ReferrerPolicy
        {
            /// <summary>The code prefix for this node (<c>"http.referrer-policy"</c>).</summary>
            public const string Prefix = Http.Prefix + ".referrer-policy";

            /// <summary><c>http.referrer-policy.missing</c></summary>
            public const string Missing = Prefix + ".missing";

            /// <summary><c>http.referrer-policy.present</c></summary>
            public const string Present = Prefix + ".present";

            /// <summary><c>http.referrer-policy.mismatch</c></summary>
            public const string Mismatch = Prefix + ".mismatch";

            /// <summary><c>http.referrer-policy.match</c></summary>
            public const string Match = Prefix + ".match";
        }

        /// <summary>The <c>Permissions-Policy</c> header: its presence, whether its value is the expected one, and whether it carries the required restriction fragments.</summary>
        public static class PermissionsPolicy
        {
            /// <summary>The code prefix for this node (<c>"http.permissions-policy"</c>).</summary>
            public const string Prefix = Http.Prefix + ".permissions-policy";

            /// <summary><c>http.permissions-policy.missing</c></summary>
            public const string Missing = Prefix + ".missing";

            /// <summary><c>http.permissions-policy.present</c></summary>
            public const string Present = Prefix + ".present";

            /// <summary><c>http.permissions-policy.mismatch</c></summary>
            public const string Mismatch = Prefix + ".mismatch";

            /// <summary><c>http.permissions-policy.not-contains</c></summary>
            public const string NotContains = Prefix + ".not-contains";

            /// <summary><c>http.permissions-policy.contains</c></summary>
            public const string Contains = Prefix + ".contains";
        }
    }
}
