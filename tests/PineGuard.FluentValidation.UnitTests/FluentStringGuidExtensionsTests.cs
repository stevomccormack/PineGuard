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

    private sealed class HasGuidVersionValidator : AbstractValidator<Model>
    {
        public HasGuidVersionValidator(int version) => RuleFor(x => x.Value).HasGuidVersion(version);
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

    // FluentStringGuidExtensions.HasGuidVersion
    [Theory]
    [MemberData(nameof(FluentStringGuidExtensionsTestData.HasGuidVersion.Cases), MemberType = typeof(FluentStringGuidExtensionsTestData.HasGuidVersion))]
    public void HasGuidVersion_BehavesAsExpected(FluentCase<(string? value, int version)> tc)
    {
        // Act
        var result = new HasGuidVersionValidator(tc.Value.version).Validate(new Model { Value = tc.Value.value });

        // Assert
        AssertResult(tc, result);
    }
}
