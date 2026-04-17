using System.Collections;
using FluentValidation;
using PineGuard.Testing.UnitTests.FluentValidation;
using Xunit.Abstractions;

namespace PineGuard.FluentValidation.UnitTests;

public sealed class FluentObjectExtensionsTests(ITestOutputHelper output) : BaseFluentUnitTest(output)
{
    private sealed record StringModel { public string? Value { get; init; } }
    private sealed record ObjectModel { public object? Value { get; init; } }

    private sealed class EqualToValidator : AbstractValidator<StringModel>
    {
        public EqualToValidator(string? other) => RuleFor(x => x.Value).EqualTo(other);
    }

    private sealed class NotEqualToValidator : AbstractValidator<StringModel>
    {
        public NotEqualToValidator(string? other) => RuleFor(x => x.Value).NotEqualTo(other);
    }

    private sealed class OfTypeValidator : AbstractValidator<ObjectModel>
    {
        public OfTypeValidator() => RuleFor(x => x.Value).OfType<ObjectModel, string>();
    }

    private sealed class NotOfTypeValidator : AbstractValidator<ObjectModel>
    {
        public NotOfTypeValidator() => RuleFor(x => x.Value).NotOfType<ObjectModel, string>();
    }

    private sealed class AssignableToTypeValidator : AbstractValidator<ObjectModel>
    {
        public AssignableToTypeValidator() => RuleFor(x => x.Value).AssignableToType<ObjectModel, IEnumerable>();
    }

    private sealed class NotAssignableToTypeValidator : AbstractValidator<ObjectModel>
    {
        public NotAssignableToTypeValidator() => RuleFor(x => x.Value).NotAssignableToType<ObjectModel, IEnumerable>();
    }

    private sealed class SameReferenceAsValidator : AbstractValidator<ObjectModel>
    {
        public SameReferenceAsValidator(object? other) => RuleFor(x => x.Value).SameReferenceAs(other);
    }

    private sealed class NotSameReferenceAsValidator : AbstractValidator<ObjectModel>
    {
        public NotSameReferenceAsValidator(object? other) => RuleFor(x => x.Value).NotSameReferenceAs(other);
    }

    [Theory]
    [MemberData(nameof(FluentObjectExtensionsTestData.EqualTo.Cases), MemberType = typeof(FluentObjectExtensionsTestData.EqualTo))]
    public void EqualTo_BehavesAsExpected(FluentCase<(string? value, string? other)> tc)
    {
        var result = new EqualToValidator(tc.Value.other).Validate(new StringModel { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentObjectExtensionsTestData.NotEqualTo.Cases), MemberType = typeof(FluentObjectExtensionsTestData.NotEqualTo))]
    public void NotEqualTo_BehavesAsExpected(FluentCase<(string? value, string? other)> tc)
    {
        var result = new NotEqualToValidator(tc.Value.other).Validate(new StringModel { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentObjectExtensionsTestData.OfType.Cases), MemberType = typeof(FluentObjectExtensionsTestData.OfType))]
    public void OfType_BehavesAsExpected(FluentCase<object?> tc)
    {
        var result = new OfTypeValidator().Validate(new ObjectModel { Value = tc.Value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentObjectExtensionsTestData.NotOfType.Cases), MemberType = typeof(FluentObjectExtensionsTestData.NotOfType))]
    public void NotOfType_BehavesAsExpected(FluentCase<object?> tc)
    {
        var result = new NotOfTypeValidator().Validate(new ObjectModel { Value = tc.Value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentObjectExtensionsTestData.AssignableToType.Cases), MemberType = typeof(FluentObjectExtensionsTestData.AssignableToType))]
    public void AssignableToType_BehavesAsExpected(FluentCase<object?> tc)
    {
        var result = new AssignableToTypeValidator().Validate(new ObjectModel { Value = tc.Value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentObjectExtensionsTestData.NotAssignableToType.Cases), MemberType = typeof(FluentObjectExtensionsTestData.NotAssignableToType))]
    public void NotAssignableToType_BehavesAsExpected(FluentCase<object?> tc)
    {
        var result = new NotAssignableToTypeValidator().Validate(new ObjectModel { Value = tc.Value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentObjectExtensionsTestData.SameReferenceAs.Cases), MemberType = typeof(FluentObjectExtensionsTestData.SameReferenceAs))]
    public void SameReferenceAs_BehavesAsExpected(FluentCase<(object? a, object? b)> tc)
    {
        var result = new SameReferenceAsValidator(tc.Value.b).Validate(new ObjectModel { Value = tc.Value.a });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentObjectExtensionsTestData.NotSameReferenceAs.Cases), MemberType = typeof(FluentObjectExtensionsTestData.NotSameReferenceAs))]
    public void NotSameReferenceAs_BehavesAsExpected(FluentCase<(object? a, object? b)> tc)
    {
        var result = new NotSameReferenceAsValidator(tc.Value.b).Validate(new ObjectModel { Value = tc.Value.a });
        AssertResult(tc, result);
    }
}
