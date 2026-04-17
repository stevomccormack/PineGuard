using FluentValidation;
using PineGuard.Testing.UnitTests.FluentValidation;
using Xunit.Abstractions;

namespace PineGuard.FluentValidation.UnitTests;

public sealed class FluentOwaspExtensionsTests(ITestOutputHelper output) : BaseFluentUnitTest(output)
{
    private sealed record Model { public string? Value { get; init; } }

    private sealed class OwaspSafeValidator : AbstractValidator<Model>
    {
        public OwaspSafeValidator() => RuleFor(x => x.Value).OwaspSafe();
    }

    [Theory]
    [MemberData(nameof(FluentOwaspExtensionsTestData.OwaspSafe.Cases), MemberType = typeof(FluentOwaspExtensionsTestData.OwaspSafe))]
    public void OwaspSafe_BehavesAsExpected(FluentCase<string?> tc)
    {
        var result = new OwaspSafeValidator().Validate(new Model { Value = tc.Value });
        AssertResult(tc, result);
    }

    private sealed class XssSafeValidator : AbstractValidator<Model>
    {
        public XssSafeValidator() => RuleFor(x => x.Value).XssSafe();
    }

    [Theory]
    [MemberData(nameof(FluentOwaspExtensionsTestData.XssSafe.Cases), MemberType = typeof(FluentOwaspExtensionsTestData.XssSafe))]
    public void XssSafe_BehavesAsExpected(FluentCase<string?> tc)
    {
        var result = new XssSafeValidator().Validate(new Model { Value = tc.Value });
        AssertResult(tc, result);
    }

    private sealed class SqlInjectionSafeValidator : AbstractValidator<Model>
    {
        public SqlInjectionSafeValidator() => RuleFor(x => x.Value).SqlInjectionSafe();
    }

    [Theory]
    [MemberData(nameof(FluentOwaspExtensionsTestData.SqlInjectionSafe.Cases), MemberType = typeof(FluentOwaspExtensionsTestData.SqlInjectionSafe))]
    public void SqlInjectionSafe_BehavesAsExpected(FluentCase<string?> tc)
    {
        var result = new SqlInjectionSafeValidator().Validate(new Model { Value = tc.Value });
        AssertResult(tc, result);
    }

    private sealed class PathTraversalSafeValidator : AbstractValidator<Model>
    {
        public PathTraversalSafeValidator() => RuleFor(x => x.Value).PathTraversalSafe();
    }

    [Theory]
    [MemberData(nameof(FluentOwaspExtensionsTestData.PathTraversalSafe.Cases), MemberType = typeof(FluentOwaspExtensionsTestData.PathTraversalSafe))]
    public void PathTraversalSafe_BehavesAsExpected(FluentCase<string?> tc)
    {
        var result = new PathTraversalSafeValidator().Validate(new Model { Value = tc.Value });
        AssertResult(tc, result);
    }

    private sealed class CommandInjectionSafeValidator : AbstractValidator<Model>
    {
        public CommandInjectionSafeValidator() => RuleFor(x => x.Value).CommandInjectionSafe();
    }

    [Theory]
    [MemberData(nameof(FluentOwaspExtensionsTestData.CommandInjectionSafe.Cases), MemberType = typeof(FluentOwaspExtensionsTestData.CommandInjectionSafe))]
    public void CommandInjectionSafe_BehavesAsExpected(FluentCase<string?> tc)
    {
        var result = new CommandInjectionSafeValidator().Validate(new Model { Value = tc.Value });
        AssertResult(tc, result);
    }

    private sealed class CrLfSafeValidator : AbstractValidator<Model>
    {
        public CrLfSafeValidator() => RuleFor(x => x.Value).CrLfSafe();
    }

    [Theory]
    [MemberData(nameof(FluentOwaspExtensionsTestData.CrLfSafe.Cases), MemberType = typeof(FluentOwaspExtensionsTestData.CrLfSafe))]
    public void CrLfSafe_BehavesAsExpected(FluentCase<string?> tc)
    {
        var result = new CrLfSafeValidator().Validate(new Model { Value = tc.Value });
        AssertResult(tc, result);
    }

    private sealed class LdapFilterSafeValidator : AbstractValidator<Model>
    {
        public LdapFilterSafeValidator() => RuleFor(x => x.Value).LdapFilterSafe();
    }

    [Theory]
    [MemberData(nameof(FluentOwaspExtensionsTestData.LdapFilterSafe.Cases), MemberType = typeof(FluentOwaspExtensionsTestData.LdapFilterSafe))]
    public void LdapFilterSafe_BehavesAsExpected(FluentCase<string?> tc)
    {
        var result = new LdapFilterSafeValidator().Validate(new Model { Value = tc.Value });
        AssertResult(tc, result);
    }

    private sealed class OpenRedirectSafeValidator : AbstractValidator<Model>
    {
        public OpenRedirectSafeValidator() => RuleFor(x => x.Value).OpenRedirectSafe();
    }

    [Theory]
    [MemberData(nameof(FluentOwaspExtensionsTestData.OpenRedirectSafe.Cases), MemberType = typeof(FluentOwaspExtensionsTestData.OpenRedirectSafe))]
    public void OpenRedirectSafe_BehavesAsExpected(FluentCase<string?> tc)
    {
        var result = new OpenRedirectSafeValidator().Validate(new Model { Value = tc.Value });
        AssertResult(tc, result);
    }
    private sealed class SsrfSchemeSafeValidator : AbstractValidator<Model>
    {
        public SsrfSchemeSafeValidator() => RuleFor(x => x.Value).SsrfSchemeSafe();
    }

    [Theory]
    [MemberData(nameof(FluentOwaspExtensionsTestData.SsrfSchemeSafe.Cases), MemberType = typeof(FluentOwaspExtensionsTestData.SsrfSchemeSafe))]
    public void SsrfSchemeSafe_BehavesAsExpected(FluentCase<string?> tc)
    {
        var result = new SsrfSchemeSafeValidator().Validate(new Model { Value = tc.Value });
        AssertResult(tc, result);
    }
}
