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

    [Theory]
    [MemberData(nameof(FluentIdentifierExtensionsTestData.Slug.Cases), MemberType = typeof(FluentIdentifierExtensionsTestData.Slug))]
    public void Slug_BehavesAsExpected(FluentCase<string?> tc)
    {
        var result = new SlugValidator().Validate(new Model { Value = tc.Value });
        AssertResult(tc, result);
    }
}
