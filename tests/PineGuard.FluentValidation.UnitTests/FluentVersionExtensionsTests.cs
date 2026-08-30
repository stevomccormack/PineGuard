using FluentValidation;
using PineGuard.Testing.UnitTests.FluentValidation;
using Xunit.Abstractions;

namespace PineGuard.FluentValidation.UnitTests;

public sealed class FluentVersionExtensionsTests(ITestOutputHelper output) : BaseFluentUnitTest(output)
{
    private sealed record Model { public string? Value { get; init; } }

    private sealed class SemVerValidator : AbstractValidator<Model>
    {
        public SemVerValidator() => RuleFor(x => x.Value).SemVer();
    }

    // FluentVersionExtensions.SemVer
    [Theory]
    [MemberData(nameof(FluentVersionExtensionsTestData.SemVer.Cases), MemberType = typeof(FluentVersionExtensionsTestData.SemVer))]
    public void SemVer_BehavesAsExpected(FluentCase<string?> tc)
    {
        // Act
        var result = new SemVerValidator().Validate(new Model { Value = tc.Value });

        // Assert
        AssertResult(tc, result);
    }
}
