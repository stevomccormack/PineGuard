using FluentValidation;
using PineGuard.Testing.UnitTests.FluentValidation;
using Xunit.Abstractions;

namespace PineGuard.FluentValidation.UnitTests;

public sealed class FluentIdentifierExtensionsTests(ITestOutputHelper output) : BaseFluentUnitTest(output)
{
    private sealed record Model { public string? Value { get; init; } }

    private sealed class SlugValidator : AbstractValidator<Model>
    {
        public SlugValidator() => RuleFor(x => x.Value).Slug();
    }

    private sealed class UlidValidator : AbstractValidator<Model>
    {
        public UlidValidator() => RuleFor(x => x.Value).Ulid();
    }

    [Theory]
    [MemberData(nameof(FluentIdentifierExtensionsTestData.Slug.Cases), MemberType = typeof(FluentIdentifierExtensionsTestData.Slug))]
    public void Slug_BehavesAsExpected(FluentCase<string?> tc)
    {
        var result = new SlugValidator().Validate(new Model { Value = tc.Value });
        AssertResult(tc, result);
    }

    // FluentIdentifierExtensions.Ulid
    [Theory]
    [MemberData(nameof(FluentIdentifierExtensionsTestData.Ulid.Cases), MemberType = typeof(FluentIdentifierExtensionsTestData.Ulid))]
    public void Ulid_BehavesAsExpected(FluentCase<string?> tc)
    {
        // Act
        var result = new UlidValidator().Validate(new Model { Value = tc.Value });

        // Assert
        AssertResult(tc, result);
    }
}
