using FluentValidation;
using PineGuard.Common;
using PineGuard.Testing.UnitTests.FluentValidation;
using Xunit.Abstractions;

namespace PineGuard.FluentValidation.UnitTests;

public sealed class FluentCronExtensionsTests(ITestOutputHelper output) : BaseFluentUnitTest(output)
{
    private sealed record Model { public string? Value { get; init; } }

    private sealed class CronExpressionValidator : AbstractValidator<Model>
    {
        public CronExpressionValidator(CronFormat format) => RuleFor(x => x.Value).CronExpression(format);
    }

    // FluentCronExtensions.CronExpression
    [Theory]
    [MemberData(nameof(FluentCronExtensionsTestData.CronExpression.Cases), MemberType = typeof(FluentCronExtensionsTestData.CronExpression))]
    public void CronExpression_BehavesAsExpected(FluentCase<(string? value, CronFormat format)> tc)
    {
        // Act
        var result = new CronExpressionValidator(tc.Value.format).Validate(new Model { Value = tc.Value.value });

        // Assert
        AssertResult(tc, result);
    }
}
