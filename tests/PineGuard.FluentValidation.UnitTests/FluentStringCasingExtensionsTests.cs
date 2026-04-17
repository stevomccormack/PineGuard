using FluentValidation;
using PineGuard.Common;
using PineGuard.Testing.UnitTests.FluentValidation;
using Xunit.Abstractions;

namespace PineGuard.FluentValidation.UnitTests;

public sealed class FluentStringCasingExtensionsTests(ITestOutputHelper output) : BaseFluentUnitTest(output)
{
    private sealed record Model { public string? Value { get; init; } }

    private sealed class CaseStyleValidator : AbstractValidator<Model>
    {
        public CaseStyleValidator(StringCasing style) => RuleFor(x => x.Value).CaseStyle(style);
    }

    [Theory]
    [MemberData(nameof(FluentStringCasingExtensionsTestData.CaseStyle.Cases), MemberType = typeof(FluentStringCasingExtensionsTestData.CaseStyle))]
    public void CaseStyle_BehavesAsExpected(FluentCase<(string? value, StringCasing style)> tc)
    {
        var validator = new CaseStyleValidator(tc.Value.style);
        var result = validator.Validate(new Model { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    private sealed class NotCaseStyleValidator : AbstractValidator<Model>
    {
        public NotCaseStyleValidator(StringCasing style) => RuleFor(x => x.Value).NotCaseStyle(style);
    }

    [Theory]
    [MemberData(nameof(FluentStringCasingExtensionsTestData.NotCaseStyle.Cases), MemberType = typeof(FluentStringCasingExtensionsTestData.NotCaseStyle))]
    public void NotCaseStyle_BehavesAsExpected(FluentCase<(string? value, StringCasing style)> tc)
    {
        var validator = new NotCaseStyleValidator(tc.Value.style);
        var result = validator.Validate(new Model { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    private sealed class CamelCaseValidator : AbstractValidator<Model>
    {
        public CamelCaseValidator() => RuleFor(x => x.Value).CamelCase();
    }

    [Theory]
    [MemberData(nameof(FluentStringCasingExtensionsTestData.CamelCase.Cases), MemberType = typeof(FluentStringCasingExtensionsTestData.CamelCase))]
    public void CamelCase_BehavesAsExpected(FluentCase<string> tc)
    {
        var result = new CamelCaseValidator().Validate(new Model { Value = tc.Value });
        AssertResult(tc, result);
    }

    private sealed class NotCamelCaseValidator : AbstractValidator<Model>
    {
        public NotCamelCaseValidator() => RuleFor(x => x.Value).NotCamelCase();
    }

    [Theory]
    [MemberData(nameof(FluentStringCasingExtensionsTestData.NotCamelCase.Cases), MemberType = typeof(FluentStringCasingExtensionsTestData.NotCamelCase))]
    public void NotCamelCase_BehavesAsExpected(FluentCase<string> tc)
    {
        var result = new NotCamelCaseValidator().Validate(new Model { Value = tc.Value });
        AssertResult(tc, result);
    }

    private sealed class PascalCaseValidator : AbstractValidator<Model>
    {
        public PascalCaseValidator() => RuleFor(x => x.Value).PascalCase();
    }

    [Theory]
    [MemberData(nameof(FluentStringCasingExtensionsTestData.PascalCase.Cases), MemberType = typeof(FluentStringCasingExtensionsTestData.PascalCase))]
    public void PascalCase_BehavesAsExpected(FluentCase<string> tc)
    {
        var result = new PascalCaseValidator().Validate(new Model { Value = tc.Value });
        AssertResult(tc, result);
    }

    private sealed class NotPascalCaseValidator : AbstractValidator<Model>
    {
        public NotPascalCaseValidator() => RuleFor(x => x.Value).NotPascalCase();
    }

    [Theory]
    [MemberData(nameof(FluentStringCasingExtensionsTestData.NotPascalCase.Cases), MemberType = typeof(FluentStringCasingExtensionsTestData.NotPascalCase))]
    public void NotPascalCase_BehavesAsExpected(FluentCase<string> tc)
    {
        var result = new NotPascalCaseValidator().Validate(new Model { Value = tc.Value });
        AssertResult(tc, result);
    }

    private sealed class SnakeCaseValidator : AbstractValidator<Model>
    {
        public SnakeCaseValidator() => RuleFor(x => x.Value).SnakeCase();
    }

    [Theory]
    [MemberData(nameof(FluentStringCasingExtensionsTestData.SnakeCase.Cases), MemberType = typeof(FluentStringCasingExtensionsTestData.SnakeCase))]
    public void SnakeCase_BehavesAsExpected(FluentCase<string> tc)
    {
        var result = new SnakeCaseValidator().Validate(new Model { Value = tc.Value });
        AssertResult(tc, result);
    }

    private sealed class NotSnakeCaseValidator : AbstractValidator<Model>
    {
        public NotSnakeCaseValidator() => RuleFor(x => x.Value).NotSnakeCase();
    }

    [Theory]
    [MemberData(nameof(FluentStringCasingExtensionsTestData.NotSnakeCase.Cases), MemberType = typeof(FluentStringCasingExtensionsTestData.NotSnakeCase))]
    public void NotSnakeCase_BehavesAsExpected(FluentCase<string> tc)
    {
        var result = new NotSnakeCaseValidator().Validate(new Model { Value = tc.Value });
        AssertResult(tc, result);
    }

    private sealed class UpperSnakeCaseValidator : AbstractValidator<Model>
    {
        public UpperSnakeCaseValidator() => RuleFor(x => x.Value).UpperSnakeCase();
    }

    [Theory]
    [MemberData(nameof(FluentStringCasingExtensionsTestData.UpperSnakeCase.Cases), MemberType = typeof(FluentStringCasingExtensionsTestData.UpperSnakeCase))]
    public void UpperSnakeCase_BehavesAsExpected(FluentCase<string> tc)
    {
        var result = new UpperSnakeCaseValidator().Validate(new Model { Value = tc.Value });
        AssertResult(tc, result);
    }

    private sealed class NotUpperSnakeCaseValidator : AbstractValidator<Model>
    {
        public NotUpperSnakeCaseValidator() => RuleFor(x => x.Value).NotUpperSnakeCase();
    }

    [Theory]
    [MemberData(nameof(FluentStringCasingExtensionsTestData.NotUpperSnakeCase.Cases), MemberType = typeof(FluentStringCasingExtensionsTestData.NotUpperSnakeCase))]
    public void NotUpperSnakeCase_BehavesAsExpected(FluentCase<string> tc)
    {
        var result = new NotUpperSnakeCaseValidator().Validate(new Model { Value = tc.Value });
        AssertResult(tc, result);
    }

    private sealed class KebabCaseValidator : AbstractValidator<Model>
    {
        public KebabCaseValidator() => RuleFor(x => x.Value).KebabCase();
    }

    [Theory]
    [MemberData(nameof(FluentStringCasingExtensionsTestData.KebabCase.Cases), MemberType = typeof(FluentStringCasingExtensionsTestData.KebabCase))]
    public void KebabCase_BehavesAsExpected(FluentCase<string> tc)
    {
        var result = new KebabCaseValidator().Validate(new Model { Value = tc.Value });
        AssertResult(tc, result);
    }

    private sealed class NotKebabCaseValidator : AbstractValidator<Model>
    {
        public NotKebabCaseValidator() => RuleFor(x => x.Value).NotKebabCase();
    }

    [Theory]
    [MemberData(nameof(FluentStringCasingExtensionsTestData.NotKebabCase.Cases), MemberType = typeof(FluentStringCasingExtensionsTestData.NotKebabCase))]
    public void NotKebabCase_BehavesAsExpected(FluentCase<string> tc)
    {
        var result = new NotKebabCaseValidator().Validate(new Model { Value = tc.Value });
        AssertResult(tc, result);
    }

    private sealed class TrainCaseValidator : AbstractValidator<Model>
    {
        public TrainCaseValidator() => RuleFor(x => x.Value).TrainCase();
    }

    [Theory]
    [MemberData(nameof(FluentStringCasingExtensionsTestData.TrainCase.Cases), MemberType = typeof(FluentStringCasingExtensionsTestData.TrainCase))]
    public void TrainCase_BehavesAsExpected(FluentCase<string> tc)
    {
        var result = new TrainCaseValidator().Validate(new Model { Value = tc.Value });
        AssertResult(tc, result);
    }

    private sealed class NotTrainCaseValidator : AbstractValidator<Model>
    {
        public NotTrainCaseValidator() => RuleFor(x => x.Value).NotTrainCase();
    }

    [Theory]
    [MemberData(nameof(FluentStringCasingExtensionsTestData.NotTrainCase.Cases), MemberType = typeof(FluentStringCasingExtensionsTestData.NotTrainCase))]
    public void NotTrainCase_BehavesAsExpected(FluentCase<string> tc)
    {
        var result = new NotTrainCaseValidator().Validate(new Model { Value = tc.Value });
        AssertResult(tc, result);
    }

    private sealed class DotCaseValidator : AbstractValidator<Model>
    {
        public DotCaseValidator() => RuleFor(x => x.Value).DotCase();
    }

    [Theory]
    [MemberData(nameof(FluentStringCasingExtensionsTestData.DotCase.Cases), MemberType = typeof(FluentStringCasingExtensionsTestData.DotCase))]
    public void DotCase_BehavesAsExpected(FluentCase<string> tc)
    {
        var result = new DotCaseValidator().Validate(new Model { Value = tc.Value });
        AssertResult(tc, result);
    }

    private sealed class NotDotCaseValidator : AbstractValidator<Model>
    {
        public NotDotCaseValidator() => RuleFor(x => x.Value).NotDotCase();
    }

    [Theory]
    [MemberData(nameof(FluentStringCasingExtensionsTestData.NotDotCase.Cases), MemberType = typeof(FluentStringCasingExtensionsTestData.NotDotCase))]
    public void NotDotCase_BehavesAsExpected(FluentCase<string> tc)
    {
        var result = new NotDotCaseValidator().Validate(new Model { Value = tc.Value });
        AssertResult(tc, result);
    }

    private sealed class SpaceCaseValidator : AbstractValidator<Model>
    {
        public SpaceCaseValidator() => RuleFor(x => x.Value).SpaceCase();
    }

    [Theory]
    [MemberData(nameof(FluentStringCasingExtensionsTestData.SpaceCase.Cases), MemberType = typeof(FluentStringCasingExtensionsTestData.SpaceCase))]
    public void SpaceCase_BehavesAsExpected(FluentCase<string> tc)
    {
        var result = new SpaceCaseValidator().Validate(new Model { Value = tc.Value });
        AssertResult(tc, result);
    }

    private sealed class NotSpaceCaseValidator : AbstractValidator<Model>
    {
        public NotSpaceCaseValidator() => RuleFor(x => x.Value).NotSpaceCase();
    }

    [Theory]
    [MemberData(nameof(FluentStringCasingExtensionsTestData.NotSpaceCase.Cases), MemberType = typeof(FluentStringCasingExtensionsTestData.NotSpaceCase))]
    public void NotSpaceCase_BehavesAsExpected(FluentCase<string> tc)
    {
        var result = new NotSpaceCaseValidator().Validate(new Model { Value = tc.Value });
        AssertResult(tc, result);
    }

    private sealed class UpperInvariantValidator : AbstractValidator<Model>
    {
        public UpperInvariantValidator() => RuleFor(x => x.Value).UpperInvariant();
    }

    [Theory]
    [MemberData(nameof(FluentStringCasingExtensionsTestData.UpperInvariant.Cases), MemberType = typeof(FluentStringCasingExtensionsTestData.UpperInvariant))]
    public void UpperInvariant_BehavesAsExpected(FluentCase<string?> tc)
    {
        var result = new UpperInvariantValidator().Validate(new Model { Value = tc.Value });
        AssertResult(tc, result);
    }

    private sealed class NotUpperInvariantValidator : AbstractValidator<Model>
    {
        public NotUpperInvariantValidator() => RuleFor(x => x.Value).NotUpperInvariant();
    }

    [Theory]
    [MemberData(nameof(FluentStringCasingExtensionsTestData.NotUpperInvariant.Cases), MemberType = typeof(FluentStringCasingExtensionsTestData.NotUpperInvariant))]
    public void NotUpperInvariant_BehavesAsExpected(FluentCase<string?> tc)
    {
        var result = new NotUpperInvariantValidator().Validate(new Model { Value = tc.Value });
        AssertResult(tc, result);
    }

    private sealed class LowerInvariantValidator : AbstractValidator<Model>
    {
        public LowerInvariantValidator() => RuleFor(x => x.Value).LowerInvariant();
    }

    [Theory]
    [MemberData(nameof(FluentStringCasingExtensionsTestData.LowerInvariant.Cases), MemberType = typeof(FluentStringCasingExtensionsTestData.LowerInvariant))]
    public void LowerInvariant_BehavesAsExpected(FluentCase<string?> tc)
    {
        var result = new LowerInvariantValidator().Validate(new Model { Value = tc.Value });
        AssertResult(tc, result);
    }

    private sealed class NotLowerInvariantValidator : AbstractValidator<Model>
    {
        public NotLowerInvariantValidator() => RuleFor(x => x.Value).NotLowerInvariant();
    }

    [Theory]
    [MemberData(nameof(FluentStringCasingExtensionsTestData.NotLowerInvariant.Cases), MemberType = typeof(FluentStringCasingExtensionsTestData.NotLowerInvariant))]
    public void NotLowerInvariant_BehavesAsExpected(FluentCase<string?> tc)
    {
        var result = new NotLowerInvariantValidator().Validate(new Model { Value = tc.Value });
        AssertResult(tc, result);
    }
}
