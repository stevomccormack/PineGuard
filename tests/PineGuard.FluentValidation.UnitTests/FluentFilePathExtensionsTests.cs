using FluentValidation;
using PineGuard.Testing.UnitTests.FluentValidation;
using Xunit.Abstractions;

namespace PineGuard.FluentValidation.UnitTests;

public sealed class FluentFilePathExtensionsTests(ITestOutputHelper output) : BaseFluentUnitTest(output)
{
    private sealed record Model { public string? Value { get; init; } }

    private sealed class SafeFileNameValidator : AbstractValidator<Model>
    {
        public SafeFileNameValidator() => RuleFor(x => x.Value).SafeFileName();
    }

    private sealed class HasFileExtensionValidator : AbstractValidator<Model>
    {
        public HasFileExtensionValidator(string[]? allowed) => RuleFor(x => x.Value).HasFileExtension(allowed);
    }

    [Theory]
    [MemberData(nameof(FluentFilePathExtensionsTestData.SafeFileName.Cases), MemberType = typeof(FluentFilePathExtensionsTestData.SafeFileName))]
    public void SafeFileName_BehavesAsExpected(FluentCase<string?> tc)
    {
        var result = new SafeFileNameValidator().Validate(new Model { Value = tc.Value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentFilePathExtensionsTestData.HasFileExtension.Cases), MemberType = typeof(FluentFilePathExtensionsTestData.HasFileExtension))]
    public void HasFileExtension_BehavesAsExpected(FluentCase<(string? path, string[]? allowed)> tc)
    {
        var result = new HasFileExtensionValidator(tc.Value.allowed).Validate(new Model { Value = tc.Value.path });
        AssertResult(tc, result);
    }
}
