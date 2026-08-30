using FluentValidation;
using PineGuard.Testing.UnitTests.FluentValidation;
using Xunit.Abstractions;

namespace PineGuard.FluentValidation.UnitTests;

public sealed class FluentChecksumExtensionsTests(ITestOutputHelper output) : BaseFluentUnitTest(output)
{
    private sealed record Model { public string? Value { get; init; } }

    private sealed class LuhnValidator : AbstractValidator<Model>
    {
        public LuhnValidator() => RuleFor(x => x.Value).Luhn();
    }

    // FluentChecksumExtensions.Luhn
    [Theory]
    [MemberData(nameof(FluentChecksumExtensionsTestData.Luhn.Cases), MemberType = typeof(FluentChecksumExtensionsTestData.Luhn))]
    public void Luhn_BehavesAsExpected(FluentCase<string?> tc)
    {
        // Act
        var result = new LuhnValidator().Validate(new Model { Value = tc.Value });

        // Assert
        AssertResult(tc, result);
    }
}
