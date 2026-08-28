using PineGuard.Codes;
using PineGuard.Testing.UnitTests.FluentValidation;
using F = PineGuard.Testing.Fixtures.OwaspRulesFixtures;

namespace PineGuard.FluentValidation.UnitTests;

public static class FluentOwaspExtensionsTestData
{
    public static class OwaspSafe
    {
        public static TheoryData<FluentCase<string?>> Cases => F.IsOwaspSafe.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsOwaspSafe.Null) => new FluentExpected(false, "Value must not be null."),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be OWASP safe.", Code: MustCodes.Owasp.Payload.Unsafe)
        });
    }

    public static class XssSafe
    {
        public static TheoryData<FluentCase<string?>> Cases => F.IsXssSafe.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsXssSafe.Null) => new FluentExpected(false, "Value must not be null."),
            nameof(F.IsXssSafe.Space) => new FluentExpected(false, "Value must be XSS safe."),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be XSS safe.")
        });
    }

    public static class SqlInjectionSafe
    {
        public static TheoryData<FluentCase<string?>> Cases => F.IsSqlInjectionSafe.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsSqlInjectionSafe.Null) => new FluentExpected(false, "Value must not be null."),
            nameof(F.IsSqlInjectionSafe.Space) => new FluentExpected(false, "Value must be SQL-injection safe."),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be SQL-injection safe.")
        });
    }

    public static class PathTraversalSafe
    {
        public static TheoryData<FluentCase<string?>> Cases => F.IsPathTraversalSafe.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsPathTraversalSafe.Null) => new FluentExpected(false, "Value must not be null."),
            nameof(F.IsPathTraversalSafe.Space) => new FluentExpected(false, "Value must be path-traversal safe."),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be path-traversal safe.")
        });
    }

    public static class CommandInjectionSafe
    {
        public static TheoryData<FluentCase<string?>> Cases => F.IsCommandInjectionSafe.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsCommandInjectionSafe.Null) => new FluentExpected(false, "Value must not be null."),
            nameof(F.IsCommandInjectionSafe.Space) => new FluentExpected(false, "Value must be command-injection safe."),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be command-injection safe.")
        });
    }

    public static class CrLfSafe
    {
        public static TheoryData<FluentCase<string?>> Cases => F.IsCrLfSafe.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsCrLfSafe.Null) => new FluentExpected(false, "Value must not be null."),
            nameof(F.IsCrLfSafe.Space) => new FluentExpected(false, "Value must be CRLF safe."),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be CRLF safe.")
        });
    }

    public static class LdapFilterSafe
    {
        public static TheoryData<FluentCase<string?>> Cases => F.IsLdapFilterSafe.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsLdapFilterSafe.Null) => new FluentExpected(false, "Value must not be null."),
            nameof(F.IsLdapFilterSafe.Space) => new FluentExpected(false, "Value must be LDAP-filter safe."),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be LDAP-filter safe.")
        });
    }

    public static class OpenRedirectSafe
    {
        public static TheoryData<FluentCase<string?>> Cases => F.IsOpenRedirectSafe.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsOpenRedirectSafe.Null) => new FluentExpected(false, "Value must not be null."),
            nameof(F.IsOpenRedirectSafe.Space) => new FluentExpected(false, "Value must be open-redirect safe."),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be open-redirect safe.")
        });
    }
    public static class SsrfSchemeSafe
    {
        public static TheoryData<FluentCase<string?>> Cases => F.IsSsrfSchemeSafe.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsSsrfSchemeSafe.Null) => new FluentExpected(false, "Value must not be null."),
            nameof(F.IsSsrfSchemeSafe.Space) => new FluentExpected(false, "Value must be SSRF-scheme safe."),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be SSRF-scheme safe.")
        });
    }
}
