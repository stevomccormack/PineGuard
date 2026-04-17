using PineGuard.Testing.UnitTests.Rules;
using F = PineGuard.Testing.Fixtures.OwaspRulesFixtures;

namespace PineGuard.Core.UnitTests.Rules.Owasp;

public static class OwaspRulesTestData
{
    public static class OwaspRegexXss
    {
        public static TheoryData<RuleCase<string>> HtmlTagRegexCases =>
        [
            new("script tag",    "<script>alert(1)</script>",   new RuleExpected(true)),
            new("simple tag",    "<b>hi</b>",                   new RuleExpected(true)),
            new("img onerror",   "<img src=x onerror=alert(1)>",new RuleExpected(true)),
            new("not a tag",     "a < 1 > b",                   new RuleExpected(false))
        ];

        public static TheoryData<RuleCase<string>> HtmlEntityEncodedAngleBracketRegexCases =>
        [
            new("&lt;",     "&lt;",    new RuleExpected(true)),
            new("&gt;",     "&gt;",    new RuleExpected(true)),
            new("&#60;",    "&#60;",   new RuleExpected(true)),
            new("&#062;",   "&#062;",  new RuleExpected(true)),
            new("&#x3c;",   "&#x3c;",  new RuleExpected(true)),
            new("&#x003E;", "&#x003E;",new RuleExpected(true)),
            new("plain",    "hello",   new RuleExpected(false))
        ];

        public static TheoryData<RuleCase<string>> ScriptProtocolRegexCases =>
        [
            new("javascript",       "javascript:alert(1)",                                         new RuleExpected(true)),
            new("mixed case",       "JaVaScRiPt:alert(1)",                                         new RuleExpected(true)),
            new("data",             "data:text/html;base64,PHNjcmlwdD5hbGVydCgxKTwvc2NyaXB0Pg==",  new RuleExpected(true)),
            new("https",            "https://example.com",                                         new RuleExpected(false))
        ];

        public static TheoryData<RuleCase<string>> HtmlEventHandlerAttributeRegexCases =>
        [
            new("onload",       "onload=",          new RuleExpected(true)),
            new("onclick",      "onclick =",         new RuleExpected(true)),
            new("onerror",      "ONERROR=alert(1)", new RuleExpected(true)),
            new("non-on attr",  "role=",            new RuleExpected(false))
        ];
    }

    public static class IsOwaspSafe
    {
        public static TheoryData<RuleCase<string?>> Cases => F.IsOwaspSafe.AllScenarios.ToRuleCases();
    }

    public static class IsXssSafe
    {
        public static TheoryData<RuleCase<string?>> Cases => F.IsXssSafe.AllScenarios.ToRuleCases();
    }

    public static class IsSqlInjectionSafe
    {
        public static TheoryData<RuleCase<string?>> Cases => F.IsSqlInjectionSafe.AllScenarios.ToRuleCases();
    }

    public static class IsPathTraversalSafe
    {
        public static TheoryData<RuleCase<string?>> Cases => F.IsPathTraversalSafe.AllScenarios.ToRuleCases();
    }

    public static class IsCommandInjectionSafe
    {
        public static TheoryData<RuleCase<string?>> Cases => F.IsCommandInjectionSafe.AllScenarios.ToRuleCases();
    }

    public static class IsCrLfSafe
    {
        public static TheoryData<RuleCase<string?>> Cases => F.IsCrLfSafe.AllScenarios.ToRuleCases();
    }

    public static class IsLdapFilterSafe
    {
        public static TheoryData<RuleCase<string?>> Cases => F.IsLdapFilterSafe.AllScenarios.ToRuleCases();
    }

    public static class IsOpenRedirectSafe
    {
        public static TheoryData<RuleCase<string?>> Cases => F.IsOpenRedirectSafe.AllScenarios.ToRuleCases();
    }
    public static class IsSsrfSchemeSafe
    {
        public static TheoryData<RuleCase<string?>> Cases => F.IsSsrfSchemeSafe.AllScenarios.ToRuleCases();
    }
}
