using FluentValidation;
using PineGuard.Testing.UnitTests.FluentValidation;
using Xunit.Abstractions;

namespace PineGuard.FluentValidation.UnitTests;

public sealed class FluentTokenExtensionsTests(ITestOutputHelper output) : BaseFluentUnitTest(output)
{
    private sealed record Model { public string? Value { get; init; } }

    private sealed class JwtValidator : AbstractValidator<Model>
    {
        public JwtValidator() => RuleFor(x => x.Value).Jwt();
    }

    // FluentTokenExtensions.Jwt
    [Theory]
    [MemberData(nameof(FluentTokenExtensionsTestData.Jwt.Cases), MemberType = typeof(FluentTokenExtensionsTestData.Jwt))]
    public void Jwt_BehavesAsExpected(FluentCase<string?> tc)
    {
        // Act
        var result = new JwtValidator().Validate(new Model { Value = tc.Value });

        // Assert
        AssertResult(tc, result);
    }
}
