using FluentValidation;
using PineGuard.Codes;
using PineGuard.Testing.UnitTests.FluentValidation;
using Xunit.Abstractions;

namespace PineGuard.FluentValidation.UnitTests;

public sealed class TemporalConfigurationTests(ITestOutputHelper output)
    : BaseFluentUnitTest(output)
{
    private sealed record Model(string? Value);

    [Theory]
    [MemberData(nameof(TemporalConfigurationTestData.Configuration.Cases), MemberType = typeof(TemporalConfigurationTestData.Configuration))]
    public void Configuration_IsForwardedWithOptionalNullSkip(TemporalConfigurationTestData.Case tc)
    {
        // Arrange
        var validator = new InlineValidator<Model>();
        validator.RuleFor(model => model.Value).ChronologicalDateOnly("2024-02-01", tc.Value.Inclusion, styles: tc.Value.Styles);

        // Act
        var result = validator.Validate(new Model(tc.Value.Input));

        // Assert
        Assert.Equal(tc.Expected, result.IsValid);
        if (!tc.Expected)
            Assert.Equal(MustCodes.Date.Order.NotChronological, Assert.Single(result.Errors).ErrorCode);
    }
}
