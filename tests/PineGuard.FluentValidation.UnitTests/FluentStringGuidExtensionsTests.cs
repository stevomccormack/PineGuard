using FluentValidation;
using PineGuard.Testing.UnitTests.FluentValidation;
using Xunit.Abstractions;

namespace PineGuard.FluentValidation.UnitTests;

public sealed class FluentStringGuidExtensionsTests(ITestOutputHelper output) : BaseFluentUnitTest(output)
{
    private sealed record Model { public string? Value { get; init; } }

    private sealed class GuidValidator : AbstractValidator<Model>
    {
        public GuidValidator() => RuleFor(x => x.Value).Guid();
    }

    private sealed class NotEmptyGuidValidator : AbstractValidator<Model>
    {
        public NotEmptyGuidValidator() => RuleFor(x => x.Value).NotEmptyGuid();
    }

    [Theory]
    [MemberData(nameof(FluentStringGuidExtensionsTestData.Guid.Cases), MemberType = typeof(FluentStringGuidExtensionsTestData.Guid))]
    public void Guid_BehavesAsExpected(FluentCase<string?> tc)
    {
        AssertResult(tc, new GuidValidator().Validate(new Model { Value = tc.Value }));
    }

    [Theory]
    [MemberData(nameof(FluentStringGuidExtensionsTestData.NotEmptyGuid.Cases), MemberType = typeof(FluentStringGuidExtensionsTestData.NotEmptyGuid))]
    public void NotEmptyGuid_BehavesAsExpected(FluentCase<string?> tc)
    {
        AssertResult(tc, new NotEmptyGuidValidator().Validate(new Model { Value = tc.Value }));
    }
}
