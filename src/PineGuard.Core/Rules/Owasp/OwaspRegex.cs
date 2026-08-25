using System.Text.RegularExpressions;

#pragma warning disable CS8795 // Partial method must have an implementation part (source generator provides it)

namespace PineGuard.Rules.Owasp;

/// <summary>
/// Provides compiled regular expressions for OWASP security validation categories.
/// </summary>
/// <remarks>
/// Each nested class corresponds to an OWASP attack category and exposes both the raw pattern
/// string constant and a source-generated <see cref="Regex"/> accessor for efficient matching.
/// </remarks>
/// <seealso href="https://pineguard.ai/docs/rules/owasp">OWASP Rules documentation</seealso>
/// <seealso href="https://owasp.org/www-project-top-ten/">OWASP Top Ten</seealso>
public static partial class OwaspRegex
{
    /// <summary>
    /// Provides regex patterns for cross-site scripting (XSS) detection.
    /// </summary>
    public static partial class Xss
    {
        /// <summary>
        /// A pattern that matches strings containing no angle brackets.
        /// </summary>
        public const string NoAngleBracketsPattern = "^[^<>]*$";

        /// <summary>
        /// A pattern that matches HTML tags.
        /// </summary>
        public const string HtmlTagPattern = @"<\s*/?\s*[a-zA-Z][^>]*>";

        /// <summary>
        /// A pattern that matches HTML entity-encoded angle brackets.
        /// </summary>
        public const string HtmlEntityEncodedAngleBracketPattern = "(?:&#0*60;|&#x0*3c;|&lt;|&#0*62;|&#x0*3e;|&gt;)";

        /// <summary>
        /// A pattern that matches URL-percent-encoded angle brackets (<c>%3C</c>, <c>%3E</c>).
        /// </summary>
        public const string PercentEncodedAngleBracketPattern = "(?:%3c|%3e)";

        /// <summary>
        /// A pattern that matches <c>javascript:</c> or <c>data:</c> protocol prefixes.
        /// </summary>
        public const string ScriptProtocolPattern = @"\b(?:javascript|data)\s*:";

        /// <summary>
        /// A pattern that matches HTML event handler attributes (e.g., <c>onclick=</c>).
        /// </summary>
        public const string HtmlEventHandlerAttributePattern = @"\bon[a-z]+\s*=";

        /// <summary>
        /// Gets a compiled regex that matches strings containing no angle brackets.
        /// </summary>
        /// <returns>A <see cref="Regex"/> compiled from <see cref="NoAngleBracketsPattern"/>.</returns>
#if NET8_0_OR_GREATER
        [GeneratedRegex(NoAngleBracketsPattern, RegexOptions.CultureInvariant, matchTimeoutMilliseconds: 250)]
        public static partial Regex NoAngleBracketsRegex();
#else
        public static Regex NoAngleBracketsRegex() => CompiledNoAngleBracketsRegex;
        private static readonly Regex CompiledNoAngleBracketsRegex = new(NoAngleBracketsPattern, RegexOptions.CultureInvariant | RegexOptions.Compiled, TimeSpan.FromMilliseconds(250));
#endif

        /// <summary>
        /// Gets a compiled regex that matches HTML tags.
        /// </summary>
        /// <returns>A <see cref="Regex"/> compiled from <see cref="HtmlTagPattern"/>.</returns>
#if NET8_0_OR_GREATER
        [GeneratedRegex(HtmlTagPattern, RegexOptions.CultureInvariant, matchTimeoutMilliseconds: 250)]
        public static partial Regex HtmlTagRegex();
#else
        public static Regex HtmlTagRegex() => CompiledHtmlTagRegex;
        private static readonly Regex CompiledHtmlTagRegex = new(HtmlTagPattern, RegexOptions.CultureInvariant | RegexOptions.Compiled, TimeSpan.FromMilliseconds(250));
#endif

        /// <summary>
        /// Gets a compiled regex that matches HTML entity-encoded angle brackets.
        /// </summary>
        /// <returns>A <see cref="Regex"/> compiled from <see cref="HtmlEntityEncodedAngleBracketPattern"/>.</returns>
#if NET8_0_OR_GREATER
        [GeneratedRegex(HtmlEntityEncodedAngleBracketPattern, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant, matchTimeoutMilliseconds: 250)]
        public static partial Regex HtmlEntityEncodedAngleBracketRegex();
#else
        public static Regex HtmlEntityEncodedAngleBracketRegex() => CompiledHtmlEntityEncodedAngleBracketRegex;
        private static readonly Regex CompiledHtmlEntityEncodedAngleBracketRegex = new(HtmlEntityEncodedAngleBracketPattern, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled, TimeSpan.FromMilliseconds(250));
#endif

        /// <summary>
        /// Gets a compiled regex that matches URL-percent-encoded angle brackets.
        /// </summary>
        /// <returns>A <see cref="Regex"/> compiled from <see cref="PercentEncodedAngleBracketPattern"/>.</returns>
#if NET8_0_OR_GREATER
        [GeneratedRegex(PercentEncodedAngleBracketPattern, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant, matchTimeoutMilliseconds: 250)]
        public static partial Regex PercentEncodedAngleBracketRegex();
#else
        public static Regex PercentEncodedAngleBracketRegex() => CompiledPercentEncodedAngleBracketRegex;
        private static readonly Regex CompiledPercentEncodedAngleBracketRegex = new(PercentEncodedAngleBracketPattern, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled, TimeSpan.FromMilliseconds(250));
#endif

        /// <summary>
        /// Gets a compiled regex that matches script protocol prefixes.
        /// </summary>
        /// <returns>A <see cref="Regex"/> compiled from <see cref="ScriptProtocolPattern"/>.</returns>
#if NET8_0_OR_GREATER
        [GeneratedRegex(ScriptProtocolPattern, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant, matchTimeoutMilliseconds: 250)]
        public static partial Regex ScriptProtocolRegex();
#else
        public static Regex ScriptProtocolRegex() => CompiledScriptProtocolRegex;
        private static readonly Regex CompiledScriptProtocolRegex = new(ScriptProtocolPattern, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled, TimeSpan.FromMilliseconds(250));
#endif

        /// <summary>
        /// Gets a compiled regex that matches HTML event handler attributes.
        /// </summary>
        /// <returns>A <see cref="Regex"/> compiled from <see cref="HtmlEventHandlerAttributePattern"/>.</returns>
#if NET8_0_OR_GREATER
        [GeneratedRegex(HtmlEventHandlerAttributePattern, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant, matchTimeoutMilliseconds: 250)]
        public static partial Regex HtmlEventHandlerAttributeRegex();
#else
        public static Regex HtmlEventHandlerAttributeRegex() => CompiledHtmlEventHandlerAttributeRegex;
        private static readonly Regex CompiledHtmlEventHandlerAttributeRegex = new(HtmlEventHandlerAttributePattern, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled, TimeSpan.FromMilliseconds(250));
#endif
    }

    /// <summary>
    /// Provides regex patterns for SQL injection detection.
    /// </summary>
    public static partial class SqlInjection
    {
        /// <summary>
        /// A pattern that matches SQL keywords (SELECT, INSERT, UPDATE, DELETE, DROP, etc.).
        /// </summary>
        public const string SqlKeywordPattern = @"\b(select|insert|update|delete|drop|alter|create|truncate|exec(?:ute)?|merge|union|grant|revoke)\b";

        /// <summary>
        /// A pattern that matches SQL comment sequences (<c>--</c>, <c>/*</c>, <c>*/</c>, <c>#</c>).
        /// </summary>
        public const string SqlCommentPattern = @"(--|/\*|\*/|#)";

        /// <summary>
        /// A pattern that matches SQL boolean-based injection patterns (e.g., <c>OR 1=1</c>).
        /// </summary>
        public const string SqlBooleanPattern = @"\b(or|and)\b\s+\w+\s*(=|!=|<>|<|>|<=|>=)";

        /// <summary>
        /// A pattern that matches the SQL statement terminator (<c>;</c>).
        /// </summary>
        public const string SqlStatementTerminatorPattern = ";";

        /// <summary>
        /// A pattern that matches single or double quote characters used in SQL injection.
        /// </summary>
        public const string SqlQuotePattern = "['\"]";

        /// <summary>
        /// A pattern that matches <c>UNION SELECT</c> SQL injection patterns.
        /// </summary>
        public const string SqlUnionSelectPattern = @"\bunion\b\s+\bselect\b";

        /// <summary>
        /// Gets a compiled regex that matches SQL keywords.
        /// </summary>
        /// <returns>A <see cref="Regex"/> compiled from <see cref="SqlKeywordPattern"/>.</returns>
#if NET8_0_OR_GREATER
        [GeneratedRegex(SqlKeywordPattern, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant, matchTimeoutMilliseconds: 250)]
        public static partial Regex SqlKeywordRegex();
#else
        public static Regex SqlKeywordRegex() => CompiledSqlKeywordRegex;
        private static readonly Regex CompiledSqlKeywordRegex = new(SqlKeywordPattern, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled, TimeSpan.FromMilliseconds(250));
#endif

        /// <summary>
        /// Gets a compiled regex that matches SQL comment sequences.
        /// </summary>
        /// <returns>A <see cref="Regex"/> compiled from <see cref="SqlCommentPattern"/>.</returns>
#if NET8_0_OR_GREATER
        [GeneratedRegex(SqlCommentPattern, RegexOptions.CultureInvariant, matchTimeoutMilliseconds: 250)]
        public static partial Regex SqlCommentRegex();
#else
        public static Regex SqlCommentRegex() => CompiledSqlCommentRegex;
        private static readonly Regex CompiledSqlCommentRegex = new(SqlCommentPattern, RegexOptions.CultureInvariant | RegexOptions.Compiled, TimeSpan.FromMilliseconds(250));
#endif

        /// <summary>
        /// Gets a compiled regex that matches SQL boolean-based injection patterns.
        /// </summary>
        /// <returns>A <see cref="Regex"/> compiled from <see cref="SqlBooleanPattern"/>.</returns>
#if NET8_0_OR_GREATER
        [GeneratedRegex(SqlBooleanPattern, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant, matchTimeoutMilliseconds: 250)]
        public static partial Regex SqlBooleanRegex();
#else
        public static Regex SqlBooleanRegex() => CompiledSqlBooleanRegex;
        private static readonly Regex CompiledSqlBooleanRegex = new(SqlBooleanPattern, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled, TimeSpan.FromMilliseconds(250));
#endif

        /// <summary>
        /// Gets a compiled regex that matches the SQL statement terminator.
        /// </summary>
        /// <returns>A <see cref="Regex"/> compiled from <see cref="SqlStatementTerminatorPattern"/>.</returns>
#if NET8_0_OR_GREATER
        [GeneratedRegex(SqlStatementTerminatorPattern, RegexOptions.CultureInvariant, matchTimeoutMilliseconds: 250)]
        public static partial Regex SqlStatementTerminatorRegex();
#else
        public static Regex SqlStatementTerminatorRegex() => CompiledSqlStatementTerminatorRegex;
        private static readonly Regex CompiledSqlStatementTerminatorRegex = new(SqlStatementTerminatorPattern, RegexOptions.CultureInvariant | RegexOptions.Compiled, TimeSpan.FromMilliseconds(250));
#endif

        /// <summary>
        /// Gets a compiled regex that matches quote characters used in SQL injection.
        /// </summary>
        /// <returns>A <see cref="Regex"/> compiled from <see cref="SqlQuotePattern"/>.</returns>
#if NET8_0_OR_GREATER
        [GeneratedRegex(SqlQuotePattern, RegexOptions.CultureInvariant, matchTimeoutMilliseconds: 250)]
        public static partial Regex SqlQuoteRegex();
#else
        public static Regex SqlQuoteRegex() => CompiledSqlQuoteRegex;
        private static readonly Regex CompiledSqlQuoteRegex = new(SqlQuotePattern, RegexOptions.CultureInvariant | RegexOptions.Compiled, TimeSpan.FromMilliseconds(250));
#endif

        /// <summary>
        /// Gets a compiled regex that matches <c>UNION SELECT</c> patterns.
        /// </summary>
        /// <returns>A <see cref="Regex"/> compiled from <see cref="SqlUnionSelectPattern"/>.</returns>
#if NET8_0_OR_GREATER
        [GeneratedRegex(SqlUnionSelectPattern, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant, matchTimeoutMilliseconds: 250)]
        public static partial Regex SqlUnionSelectRegex();
#else
        public static Regex SqlUnionSelectRegex() => CompiledSqlUnionSelectRegex;
        private static readonly Regex CompiledSqlUnionSelectRegex = new(SqlUnionSelectPattern, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled, TimeSpan.FromMilliseconds(250));
#endif
    }

    /// <summary>
    /// Provides regex patterns for path traversal attack detection.
    /// </summary>
    public static partial class PathTraversal
    {
        /// <summary>
        /// A pattern that matches a dot-dot path traversal segment bounded by a path separator (or the start/end
        /// of the string). Each dot of the pair, and each bounding separator, is matched independently as either
        /// its literal form or a single- or double-percent-encoded form (e.g. <c>../</c>, <c>..\</c>, <c>%2e./</c>,
        /// <c>.%2e/</c>, <c>%2e%2e/</c>, <c>..%2f</c>, <c>..%252f</c>, a bare trailing <c>..</c>, or a trailing
        /// <c>%2e%2e</c>) so that mixed-encoding and double-encoding evasions of the traversal token are still
        /// recognized before any URL-decoding happens downstream.
        /// </summary>
        public const string DotDotSegmentPattern = @"(?:^|/|\\|%2f|%5c|%252f|%255c)(?:\.|%2e|%252e){2}(?:/|\\|%2f|%5c|%252f|%255c|$)";

        /// <summary>
        /// A pattern that matches absolute Unix paths starting with <c>/</c>.
        /// </summary>
        public const string AbsoluteUnixPathPattern = "^/";

        /// <summary>
        /// A pattern that matches Windows drive-letter absolute paths (e.g., <c>C:\</c>).
        /// </summary>
        public const string WindowsDriveAbsolutePathPattern = @"^[a-zA-Z]:\\?";

        /// <summary>
        /// A pattern that matches UNC paths starting with <c>\\</c>.
        /// </summary>
        public const string UncPathPattern = @"^\\\\";

        /// <summary>
        /// Gets a compiled regex that matches dot-dot path traversal segments.
        /// </summary>
        /// <returns>A <see cref="Regex"/> compiled from <see cref="DotDotSegmentPattern"/>.</returns>
#if NET8_0_OR_GREATER
        [GeneratedRegex(DotDotSegmentPattern, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant, matchTimeoutMilliseconds: 250)]
        public static partial Regex DotDotSegmentRegex();
#else
        public static Regex DotDotSegmentRegex() => CompiledDotDotSegmentRegex;
        private static readonly Regex CompiledDotDotSegmentRegex = new(DotDotSegmentPattern, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled, TimeSpan.FromMilliseconds(250));
#endif

        /// <summary>
        /// Gets a compiled regex that matches absolute Unix paths.
        /// </summary>
        /// <returns>A <see cref="Regex"/> compiled from <see cref="AbsoluteUnixPathPattern"/>.</returns>
#if NET8_0_OR_GREATER
        [GeneratedRegex(AbsoluteUnixPathPattern, RegexOptions.CultureInvariant, matchTimeoutMilliseconds: 250)]
        public static partial Regex AbsoluteUnixPathRegex();
#else
        public static Regex AbsoluteUnixPathRegex() => CompiledAbsoluteUnixPathRegex;
        private static readonly Regex CompiledAbsoluteUnixPathRegex = new(AbsoluteUnixPathPattern, RegexOptions.CultureInvariant | RegexOptions.Compiled, TimeSpan.FromMilliseconds(250));
#endif

        /// <summary>
        /// Gets a compiled regex that matches Windows drive-letter absolute paths.
        /// </summary>
        /// <returns>A <see cref="Regex"/> compiled from <see cref="WindowsDriveAbsolutePathPattern"/>.</returns>
#if NET8_0_OR_GREATER
        [GeneratedRegex(WindowsDriveAbsolutePathPattern, RegexOptions.CultureInvariant, matchTimeoutMilliseconds: 250)]
        public static partial Regex WindowsDriveAbsolutePathRegex();
#else
        public static Regex WindowsDriveAbsolutePathRegex() => CompiledWindowsDriveAbsolutePathRegex;
        private static readonly Regex CompiledWindowsDriveAbsolutePathRegex = new(WindowsDriveAbsolutePathPattern, RegexOptions.CultureInvariant | RegexOptions.Compiled, TimeSpan.FromMilliseconds(250));
#endif

        /// <summary>
        /// Gets a compiled regex that matches UNC paths.
        /// </summary>
        /// <returns>A <see cref="Regex"/> compiled from <see cref="UncPathPattern"/>.</returns>
#if NET8_0_OR_GREATER
        [GeneratedRegex(UncPathPattern, RegexOptions.CultureInvariant, matchTimeoutMilliseconds: 250)]
        public static partial Regex UncPathRegex();
#else
        public static Regex UncPathRegex() => CompiledUncPathRegex;
        private static readonly Regex CompiledUncPathRegex = new(UncPathPattern, RegexOptions.CultureInvariant | RegexOptions.Compiled, TimeSpan.FromMilliseconds(250));
#endif
    }

    /// <summary>
    /// Provides regex patterns for command injection detection.
    /// </summary>
    public static partial class CommandInjection
    {
        /// <summary>
        /// A pattern that matches shell metacharacters (<c>; &amp; | ` $ &gt; &lt;</c>).
        /// </summary>
        public const string ShellMetacharactersPattern = "[;&|`$><]";

        /// <summary>
        /// A pattern that matches newline characters and their URL-encoded equivalents.
        /// </summary>
        public const string NewlinePattern = @"\r|\n|%0d|%0a";

        /// <summary>
        /// A pattern that matches command chaining operators (<c>&amp;&amp;</c>, <c>||</c>, <c>;</c>).
        /// </summary>
        public const string CommandChainingPattern = @"(&&|\|\||;)";

        /// <summary>
        /// Gets a compiled regex that matches shell metacharacters.
        /// </summary>
        /// <returns>A <see cref="Regex"/> compiled from <see cref="ShellMetacharactersPattern"/>.</returns>
#if NET8_0_OR_GREATER
        [GeneratedRegex(ShellMetacharactersPattern, RegexOptions.CultureInvariant, matchTimeoutMilliseconds: 250)]
        public static partial Regex ShellMetacharactersRegex();
#else
        public static Regex ShellMetacharactersRegex() => CompiledShellMetacharactersRegex;
        private static readonly Regex CompiledShellMetacharactersRegex = new(ShellMetacharactersPattern, RegexOptions.CultureInvariant | RegexOptions.Compiled, TimeSpan.FromMilliseconds(250));
#endif

        /// <summary>
        /// Gets a compiled regex that matches newline characters.
        /// </summary>
        /// <returns>A <see cref="Regex"/> compiled from <see cref="NewlinePattern"/>.</returns>
#if NET8_0_OR_GREATER
        [GeneratedRegex(NewlinePattern, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant, matchTimeoutMilliseconds: 250)]
        public static partial Regex NewlineRegex();
#else
        public static Regex NewlineRegex() => CompiledNewlineRegex;
        private static readonly Regex CompiledNewlineRegex = new(NewlinePattern, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled, TimeSpan.FromMilliseconds(250));
#endif

        /// <summary>
        /// Gets a compiled regex that matches command chaining operators.
        /// </summary>
        /// <returns>A <see cref="Regex"/> compiled from <see cref="CommandChainingPattern"/>.</returns>
#if NET8_0_OR_GREATER
        [GeneratedRegex(CommandChainingPattern, RegexOptions.CultureInvariant, matchTimeoutMilliseconds: 250)]
        public static partial Regex CommandChainingRegex();
#else
        public static Regex CommandChainingRegex() => CompiledCommandChainingRegex;
        private static readonly Regex CompiledCommandChainingRegex = new(CommandChainingPattern, RegexOptions.CultureInvariant | RegexOptions.Compiled, TimeSpan.FromMilliseconds(250));
#endif
    }

    /// <summary>
    /// Provides regex patterns for CRLF (HTTP header) injection detection.
    /// </summary>
    public static partial class HeaderInjection
    {
        /// <summary>
        /// A pattern that matches CR/LF characters and their URL-encoded equivalents.
        /// </summary>
        public const string CrLfPattern = @"\r|\n|%0d|%0a";

        /// <summary>
        /// Gets a compiled regex that matches CR/LF injection patterns.
        /// </summary>
        /// <returns>A <see cref="Regex"/> compiled from <see cref="CrLfPattern"/>.</returns>
#if NET8_0_OR_GREATER
        [GeneratedRegex(CrLfPattern, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant, matchTimeoutMilliseconds: 250)]
        public static partial Regex CrLfRegex();
#else
        public static Regex CrLfRegex() => CompiledCrLfRegex;
        private static readonly Regex CompiledCrLfRegex = new(CrLfPattern, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled, TimeSpan.FromMilliseconds(250));
#endif
    }

    /// <summary>
    /// Provides regex patterns for LDAP filter injection detection.
    /// </summary>
    public static partial class LdapInjection
    {
        /// <summary>
        /// A pattern that matches LDAP filter special characters (<c>* ( ) \ \0</c>).
        /// </summary>
        public const string LdapFilterSpecialCharsPattern = @"[\*\(\)\\\x00]";

        /// <summary>
        /// Gets a compiled regex that matches LDAP filter special characters.
        /// </summary>
        /// <returns>A <see cref="Regex"/> compiled from <see cref="LdapFilterSpecialCharsPattern"/>.</returns>
#if NET8_0_OR_GREATER
        [GeneratedRegex(LdapFilterSpecialCharsPattern, RegexOptions.CultureInvariant, matchTimeoutMilliseconds: 250)]
        public static partial Regex LdapFilterSpecialCharsRegex();
#else
        public static Regex LdapFilterSpecialCharsRegex() => CompiledLdapFilterSpecialCharsRegex;
        private static readonly Regex CompiledLdapFilterSpecialCharsRegex = new(LdapFilterSpecialCharsPattern, RegexOptions.CultureInvariant | RegexOptions.Compiled, TimeSpan.FromMilliseconds(250));
#endif
    }

    /// <summary>
    /// Provides regex patterns for open redirect detection.
    /// </summary>
    public static partial class OpenRedirect
    {
        /// <summary>
        /// A pattern that matches absolute URLs or protocol-relative URLs (<c>//</c>).
        /// </summary>
        public const string AbsoluteOrProtocolRelativeUrlPattern = "^(?:[a-zA-Z][a-zA-Z0-9+\\-.]*:|//)";

        /// <summary>
        /// Gets a compiled regex that matches absolute or protocol-relative URLs.
        /// </summary>
        /// <returns>A <see cref="Regex"/> compiled from <see cref="AbsoluteOrProtocolRelativeUrlPattern"/>.</returns>
#if NET8_0_OR_GREATER
        [GeneratedRegex(AbsoluteOrProtocolRelativeUrlPattern, RegexOptions.CultureInvariant, matchTimeoutMilliseconds: 250)]
        public static partial Regex AbsoluteOrProtocolRelativeUrlRegex();
#else
        public static Regex AbsoluteOrProtocolRelativeUrlRegex() => CompiledAbsoluteOrProtocolRelativeUrlRegex;
        private static readonly Regex CompiledAbsoluteOrProtocolRelativeUrlRegex = new(AbsoluteOrProtocolRelativeUrlPattern, RegexOptions.CultureInvariant | RegexOptions.Compiled, TimeSpan.FromMilliseconds(250));
#endif
    }

    /// <summary>
    /// Provides regex patterns for server-side request forgery (SSRF) detection via dangerous URI schemes.
    /// </summary>
    public static partial class Ssrf
    {
        /// <summary>
        /// A pattern that matches dangerous URI schemes (<c>file:</c>, <c>gopher:</c>, <c>ftp:</c>, <c>data:</c>, <c>javascript:</c>).
        /// </summary>
        public const string DangerousSchemePattern = @"\b(?:file|gopher|ftp|data|javascript)\s*:";

        /// <summary>
        /// Gets a compiled regex that matches dangerous URI scheme patterns.
        /// </summary>
        /// <returns>A <see cref="Regex"/> compiled from <see cref="DangerousSchemePattern"/>.</returns>
#if NET8_0_OR_GREATER
        [GeneratedRegex(DangerousSchemePattern, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant, matchTimeoutMilliseconds: 250)]
        public static partial Regex DangerousSchemeRegex();
#else
        public static Regex DangerousSchemeRegex() => CompiledDangerousSchemeRegex;
        private static readonly Regex CompiledDangerousSchemeRegex = new(DangerousSchemePattern, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled, TimeSpan.FromMilliseconds(250));
#endif
    }
}
