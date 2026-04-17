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
}
