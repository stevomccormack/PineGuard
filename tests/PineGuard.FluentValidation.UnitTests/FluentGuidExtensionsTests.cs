using FluentValidation;
using PineGuard.Testing.UnitTests.FluentValidation;
using Xunit.Abstractions;

namespace PineGuard.FluentValidation.UnitTests;

public sealed class FluentGuidExtensionsTests(ITestOutputHelper output) : BaseFluentUnitTest(output)
{
    private sealed record GuidModel { public Guid Value { get; init; } }
    private sealed record NullableGuidModel { public Guid? Value { get; init; } }

    private sealed class NotEmptyValidator : AbstractValidator<GuidModel>
    {
        public NotEmptyValidator() => RuleFor(x => x.Value).NotEmpty();
    }

    private sealed class NotEmptyNullableValidator : AbstractValidator<NullableGuidModel>
    {
        public NotEmptyNullableValidator() => RuleFor(x => x.Value).NotEmpty();
    }

    private sealed class HasGuidVersionValidator : AbstractValidator<GuidModel>
    {
        public HasGuidVersionValidator(int version) => RuleFor(x => x.Value).HasGuidVersion(version);
    }

    private sealed class HasGuidVersionNullableValidator : AbstractValidator<NullableGuidModel>
    {
        public HasGuidVersionNullableValidator(int version) => RuleFor(x => x.Value).HasGuidVersion(version);
    }

    [Theory]
    [MemberData(nameof(FluentGuidExtensionsTestData.NotEmpty.Cases), MemberType = typeof(FluentGuidExtensionsTestData.NotEmpty))]
    public void NotEmpty_BehavesAsExpected(FluentCase<Guid> tc)
    {
        var result = new NotEmptyValidator().Validate(new GuidModel { Value = tc.Value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentGuidExtensionsTestData.NotEmptyNullable.Cases), MemberType = typeof(FluentGuidExtensionsTestData.NotEmptyNullable))]
    public void NotEmptyNullable_BehavesAsExpected(FluentCase<Guid?> tc)
    {
        var result = new NotEmptyNullableValidator().Validate(new NullableGuidModel { Value = tc.Value });
        AssertResult(tc, result);
    }

    // FluentGuidExtensions.HasGuidVersion
    [Theory]
    [MemberData(nameof(FluentGuidExtensionsTestData.HasGuidVersion.Cases), MemberType = typeof(FluentGuidExtensionsTestData.HasGuidVersion))]
    public void HasGuidVersion_BehavesAsExpected(FluentCase<(Guid value, int version)> tc)
    {
        // Act
        var result = new HasGuidVersionValidator(tc.Value.version).Validate(new GuidModel { Value = tc.Value.value });

        // Assert
        AssertResult(tc, result);
    }

    // FluentGuidExtensions.HasGuidVersion (nullable)
    [Theory]
    [MemberData(nameof(FluentGuidExtensionsTestData.HasGuidVersionNullable.Cases), MemberType = typeof(FluentGuidExtensionsTestData.HasGuidVersionNullable))]
    public void HasGuidVersionNullable_BehavesAsExpected(FluentCase<(Guid? value, int version)> tc)
    {
        // Act
        var result = new HasGuidVersionNullableValidator(tc.Value.version).Validate(new NullableGuidModel { Value = tc.Value.value });

        // Assert
        AssertResult(tc, result);
    }
}
