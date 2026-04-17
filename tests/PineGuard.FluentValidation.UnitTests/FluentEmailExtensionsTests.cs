using FluentValidation;
using PineGuard.Testing.UnitTests.FluentValidation;
using Xunit.Abstractions;

namespace PineGuard.FluentValidation.UnitTests;

public sealed class FluentEmailExtensionsTests(ITestOutputHelper output) : BaseFluentUnitTest(output)
{
    private sealed record Model { public string? Value { get; init; } }

    private sealed class EmailValidator : AbstractValidator<Model>
    {
        public EmailValidator() => RuleFor(x => x.Value).Email();
    }

    [Theory]
    [MemberData(nameof(FluentEmailExtensionsTestData.Email.Cases), MemberType = typeof(FluentEmailExtensionsTestData.Email))]
    public void Email_BehavesAsExpected(FluentCase<string?> tc)
    {
        var result = new EmailValidator().Validate(new Model { Value = tc.Value });
        AssertResult(tc, result);
    }

    private sealed class StrictEmailValidator : AbstractValidator<Model>
    {
        public StrictEmailValidator() => RuleFor(x => x.Value).StrictEmail();
    }

    [Theory]
    [MemberData(nameof(FluentEmailExtensionsTestData.StrictEmail.Cases), MemberType = typeof(FluentEmailExtensionsTestData.StrictEmail))]
    public void StrictEmail_BehavesAsExpected(FluentCase<string?> tc)
    {
        var result = new StrictEmailValidator().Validate(new Model { Value = tc.Value });
        AssertResult(tc, result);
    }

    private sealed class HasEmailAliasValidator : AbstractValidator<Model>
    {
        public HasEmailAliasValidator() => RuleFor(x => x.Value).HasEmailAlias();
    }

    [Theory]
    [MemberData(nameof(FluentEmailExtensionsTestData.HasEmailAlias.Cases), MemberType = typeof(FluentEmailExtensionsTestData.HasEmailAlias))]
    public void HasEmailAlias_BehavesAsExpected(FluentCase<string?> tc)
    {
        var result = new HasEmailAliasValidator().Validate(new Model { Value = tc.Value });
        AssertResult(tc, result);
    }

    private sealed class NotHasEmailAliasValidator : AbstractValidator<Model>
    {
        public NotHasEmailAliasValidator() => RuleFor(x => x.Value).NotHasEmailAlias();
    }

    [Theory]
    [MemberData(nameof(FluentEmailExtensionsTestData.NotHasEmailAlias.Cases), MemberType = typeof(FluentEmailExtensionsTestData.NotHasEmailAlias))]
    public void NotHasEmailAlias_BehavesAsExpected(FluentCase<string?> tc)
    {
        var result = new NotHasEmailAliasValidator().Validate(new Model { Value = tc.Value });
        AssertResult(tc, result);
    }
}
