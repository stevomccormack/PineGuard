using PineGuard.Codes;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;
using PineGuard.Testing.UnitTests.DataAnnotations;
using F = PineGuard.Testing.Fixtures.OwaspRulesFixtures;

namespace PineGuard.DataAnnotations.UnitTests;

public static class OwaspAttributesTestData
{
    public static class OwaspSafeTypeMismatch
    {
        private sealed record ActionThrowsCase(string Name, Action Value, ExpectedException ExpectedException)
            : ThrowsCase<Action>(Name, Value, ExpectedException);

        public static TheoryData<IThrowsCase> Cases =>
        [
            new ActionThrowsCase("int-value", () => new OwaspSafeAttribute().GetValidationResult(42, new System.ComponentModel.DataAnnotations.ValidationContext(new object()) { MemberName = "Value" }), new ExpectedException(typeof(InvalidOperationException)))
        ];
    }

    public static class OwaspSafe
    {
        public static TheoryData<DataAnnotationCase> Cases => F.IsOwaspSafe.AllScenarios.ToDataAnnotationCases(s => s.Name switch
        {
            nameof(F.IsOwaspSafe.Null) => new DataAnnotationExpected(true),
            _ when s.IsValid => new DataAnnotationExpected(true),
            _ => new DataAnnotationExpected(false, Code: MustCodes.Owasp.Payload.Unsafe)
        });
    }

    public static class XssSafe
    {
        public static TheoryData<DataAnnotationCase> Cases => F.IsXssSafe.AllScenarios.ToDataAnnotationCases(s => s.Name switch
        {
            nameof(F.IsXssSafe.Null) => new DataAnnotationExpected(true),
            _ when s.IsValid => new DataAnnotationExpected(true),
            _ => new DataAnnotationExpected(false)
        });
    }

    public static class SqlInjectionSafe
    {
        public static TheoryData<DataAnnotationCase> Cases => F.IsSqlInjectionSafe.AllScenarios.ToDataAnnotationCases(s => s.Name switch
        {
            nameof(F.IsSqlInjectionSafe.Null) => new DataAnnotationExpected(true),
            _ when s.IsValid => new DataAnnotationExpected(true),
            _ => new DataAnnotationExpected(false)
        });
    }

    public static class PathTraversalSafe
    {
        public static TheoryData<DataAnnotationCase> Cases => F.IsPathTraversalSafe.AllScenarios.ToDataAnnotationCases(s => s.Name switch
        {
            nameof(F.IsPathTraversalSafe.Null) => new DataAnnotationExpected(true),
            _ when s.IsValid => new DataAnnotationExpected(true),
            _ => new DataAnnotationExpected(false)
        });
    }

    public static class CommandInjectionSafe
    {
        public static TheoryData<DataAnnotationCase> Cases => F.IsCommandInjectionSafe.AllScenarios.ToDataAnnotationCases(s => s.Name switch
        {
            nameof(F.IsCommandInjectionSafe.Null) => new DataAnnotationExpected(true),
            _ when s.IsValid => new DataAnnotationExpected(true),
            _ => new DataAnnotationExpected(false)
        });
    }

    public static class CrLfSafe
    {
        public static TheoryData<DataAnnotationCase> Cases => F.IsCrLfSafe.AllScenarios.ToDataAnnotationCases(s => s.Name switch
        {
            nameof(F.IsCrLfSafe.Null) => new DataAnnotationExpected(true),
            _ when s.IsValid => new DataAnnotationExpected(true),
            _ => new DataAnnotationExpected(false)
        });
    }

    public static class LdapFilterSafe
    {
        public static TheoryData<DataAnnotationCase> Cases => F.IsLdapFilterSafe.AllScenarios.ToDataAnnotationCases(s => s.Name switch
        {
            nameof(F.IsLdapFilterSafe.Null) => new DataAnnotationExpected(true),
            _ when s.IsValid => new DataAnnotationExpected(true),
            _ => new DataAnnotationExpected(false)
        });
    }

    public static class OpenRedirectSafe
    {
        public static TheoryData<DataAnnotationCase> Cases => F.IsOpenRedirectSafe.AllScenarios.ToDataAnnotationCases(s => s.Name switch
        {
            nameof(F.IsOpenRedirectSafe.Null) => new DataAnnotationExpected(true),
            _ when s.IsValid => new DataAnnotationExpected(true),
            _ => new DataAnnotationExpected(false)
        });
    }

    public static class SsrfSchemeSafe
    {
        public static TheoryData<DataAnnotationCase> Cases => F.IsSsrfSchemeSafe.AllScenarios.ToDataAnnotationCases(s => s.Name switch
        {
            nameof(F.IsSsrfSchemeSafe.Null) => new DataAnnotationExpected(true),
            _ when s.IsValid => new DataAnnotationExpected(true),
            _ => new DataAnnotationExpected(false)
        });
    }
}
