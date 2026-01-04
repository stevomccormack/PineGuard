using System.Text.RegularExpressions;

namespace PineGuard.Rules.Owasp;

public static partial class OwaspRegex
{
    public static partial class Xss
    {
        public const string NoAngleBracketsPattern = "^[^<>]*$";
        public const string HtmlTagPattern = "<\\s*/?\\s*[a-zA-Z][^>]*>";
        public const string HtmlEntityEncodedAngleBracketPattern = "(?:&#0*60;|&#x0*3c;|&lt;|&#0*62;|&#x0*3e;|&gt;)";
        public const string ScriptProtocolPattern = "\\b(?:javascript|data)\\s*:";
        public const string HtmlEventHandlerAttributePattern = "\\bon[a-z]+\\s*=";

        [GeneratedRegex(NoAngleBracketsPattern, RegexOptions.CultureInvariant)]
        public static partial Regex NoAngleBracketsRegex();

        [GeneratedRegex(HtmlTagPattern, RegexOptions.CultureInvariant)]
        public static partial Regex HtmlTagRegex();

        [GeneratedRegex(HtmlEntityEncodedAngleBracketPattern, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)]
        public static partial Regex HtmlEntityEncodedAngleBracketRegex();

        [GeneratedRegex(ScriptProtocolPattern, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)]
        public static partial Regex ScriptProtocolRegex();

        [GeneratedRegex(HtmlEventHandlerAttributePattern, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)]
        public static partial Regex HtmlEventHandlerAttributeRegex();
    }

    public static partial class SqlInjection
    {
        public const string SqlKeywordPattern = "\\b(select|insert|update|delete|drop|alter|create|truncate|exec(?:ute)?|merge|union|grant|revoke)\\b";
        public const string SqlCommentPattern = "(--|/\\*|\\*/|#)";
        public const string SqlBooleanPattern = "\\b(or|and)\\b\\s+\\w+\\s*(=|!=|<>|<|>|<=|>=)";
        public const string SqlStatementTerminatorPattern = ";";
        public const string SqlQuotePattern = "['\"]";
        public const string SqlUnionSelectPattern = "\\bunion\\b\\s+\\bselect\\b";

        [GeneratedRegex(SqlKeywordPattern, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)]
        public static partial Regex SqlKeywordRegex();

        [GeneratedRegex(SqlCommentPattern, RegexOptions.CultureInvariant)]
        public static partial Regex SqlCommentRegex();

        [GeneratedRegex(SqlBooleanPattern, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)]
        public static partial Regex SqlBooleanRegex();

        [GeneratedRegex(SqlStatementTerminatorPattern, RegexOptions.CultureInvariant)]
        public static partial Regex SqlStatementTerminatorRegex();

        [GeneratedRegex(SqlQuotePattern, RegexOptions.CultureInvariant)]
        public static partial Regex SqlQuoteRegex();

        [GeneratedRegex(SqlUnionSelectPattern, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)]
        public static partial Regex SqlUnionSelectRegex();
    }

    public static partial class PathTraversal
    {
        public const string DotDotSegmentPattern = "(\\.\\.(?:/|\\\\)|%2e%2e(?:%2f|%5c)|%2e%2e/)";
        public const string AbsoluteUnixPathPattern = "^/";
        public const string WindowsDriveAbsolutePathPattern = "^[a-zA-Z]:\\\\?";
        public const string UncPathPattern = "^\\\\\\\\";

        [GeneratedRegex(DotDotSegmentPattern, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)]
        public static partial Regex DotDotSegmentRegex();

        [GeneratedRegex(AbsoluteUnixPathPattern, RegexOptions.CultureInvariant)]
        public static partial Regex AbsoluteUnixPathRegex();

        [GeneratedRegex(WindowsDriveAbsolutePathPattern, RegexOptions.CultureInvariant)]
        public static partial Regex WindowsDriveAbsolutePathRegex();

        [GeneratedRegex(UncPathPattern, RegexOptions.CultureInvariant)]
        public static partial Regex UncPathRegex();
    }

    public static partial class CommandInjection
    {
        public const string ShellMetacharactersPattern = "[;&|`$><]";
        public const string NewlinePattern = "\\r|\\n|%0d|%0a";
        public const string CommandChainingPattern = "(&&|\\|\\||;)";

        [GeneratedRegex(ShellMetacharactersPattern, RegexOptions.CultureInvariant)]
        public static partial Regex ShellMetacharactersRegex();

        [GeneratedRegex(NewlinePattern, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)]
        public static partial Regex NewlineRegex();

        [GeneratedRegex(CommandChainingPattern, RegexOptions.CultureInvariant)]
        public static partial Regex CommandChainingRegex();
    }

    public static partial class HeaderInjection
    {
        public const string CrLfPattern = "\\r|\\n|%0d|%0a";

        [GeneratedRegex(CrLfPattern, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)]
        public static partial Regex CrLfRegex();
    }

    public static partial class LdapInjection
    {
        public const string LdapFilterSpecialCharsPattern = "[\\*\\(\\)\\\\\\x00]";

        [GeneratedRegex(LdapFilterSpecialCharsPattern, RegexOptions.CultureInvariant)]
        public static partial Regex LdapFilterSpecialCharsRegex();
    }

    public static partial class OpenRedirect
    {
        public const string AbsoluteOrProtocolRelativeUrlPattern = "^(?:[a-zA-Z][a-zA-Z0-9+\\-.]*:|//)";

        [GeneratedRegex(AbsoluteOrProtocolRelativeUrlPattern, RegexOptions.CultureInvariant)]
        public static partial Regex AbsoluteOrProtocolRelativeUrlRegex();
    }

    public static partial class Ssrf
    {
        public const string DangerousSchemePattern = "\\b(?:file|gopher|ftp|data|javascript)\\s*:";

        [GeneratedRegex(DangerousSchemePattern, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)]
        public static partial Regex DangerousSchemeRegex();
    }
}
