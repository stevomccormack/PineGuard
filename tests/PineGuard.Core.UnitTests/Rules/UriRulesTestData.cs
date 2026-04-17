using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;
using PineGuard.Testing.UnitTests.Rules;
using F = PineGuard.Testing.Fixtures.UriRulesFixtures;

namespace PineGuard.Core.UnitTests.Rules;

#pragma warning disable CS0618

public static class UriRulesTestData
{
    public static class IsAbsoluteUri
    {
        public static TheoryData<RuleCase<string?>> Cases => F.IsAbsoluteUri.AllScenarios.ToRuleCases();
    }

    public static class IsRelativeUri
    {
        public static TheoryData<RuleCase<string?>> Cases => F.IsRelativeUri.AllScenarios.ToRuleCases();
    }

    public static class IsUrl
    {
        public static TheoryData<RuleCase<string?>> Cases => F.IsUrl.AllScenarios.ToRuleCases();
    }

    public static class IsHttpsUrl
    {
        public static TheoryData<RuleCase<string?>> Cases => F.IsHttpsUrl.AllScenarios.ToRuleCases();
    }

    public static class IsHttpUrl
    {
        public static TheoryData<RuleCase<string?>> Cases => F.IsHttpUrl.AllScenarios.ToRuleCases();
    }

    public static class IsFileUri
    {
        public static TheoryData<RuleCase<string?>> Cases => F.IsFileUri.AllScenarios.ToRuleCases();
    }

    public static class IsFilePath
    {
        public static TheoryData<RuleCase<string?>> Cases => F.IsFilePath.AllScenarios.ToRuleCases();
    }

    public static class HasScheme
    {
        public static TheoryData<RuleCase<(string? value, string scheme)>> Cases => F.HasScheme.AllScenarios.ToRuleCases();

        public static TheoryData<InvalidCase> InvalidCases =>
        [
            new("null scheme", ("https://example.com", null!), new ExpectedException(typeof(ArgumentNullException), "scheme"))
        ];

        public sealed record InvalidCase(string Name, (string? Value, string Scheme) Input, ExpectedException ExpectedException)
            : ThrowsCase<(string? Value, string Scheme)>(Name, Input, ExpectedException);
    }
}
