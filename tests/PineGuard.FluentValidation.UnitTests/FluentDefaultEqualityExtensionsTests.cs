using FluentValidation;
using PineGuard.Testing.UnitTests.FluentValidation;
using Xunit.Abstractions;

namespace PineGuard.FluentValidation.UnitTests;

public sealed class FluentDefaultEqualityExtensionsTests(ITestOutputHelper output) : BaseFluentUnitTest(output)
{
    private sealed record IntModel { public int Value { get; init; } }
    private sealed record StringModel { public string? Value { get; init; } }

    private sealed class DefaultIntValidator : AbstractValidator<IntModel> { public DefaultIntValidator() => RuleFor(x => x.Value).Default(); }
    private sealed class NotDefaultIntValidator : AbstractValidator<IntModel> { public NotDefaultIntValidator() => RuleFor(x => x.Value).NotDefault(); }
    private sealed class NullOrDefaultStringValidator : AbstractValidator<StringModel> { public NullOrDefaultStringValidator() => RuleFor(x => x.Value).NullOrDefault(); }
    private sealed class NotNullOrDefaultStringValidator : AbstractValidator<StringModel> { public NotNullOrDefaultStringValidator() => RuleFor(x => x.Value).NotNullOrDefault(); }

    [Theory]
    [MemberData(nameof(FluentDefaultEqualityExtensionsTestData.DefaultInt32.Cases), MemberType = typeof(FluentDefaultEqualityExtensionsTestData.DefaultInt32))]
    public void DefaultInt32_BehavesAsExpected(FluentCase<int> tc)
    {
        var result = new DefaultIntValidator().Validate(new IntModel { Value = tc.Value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDefaultEqualityExtensionsTestData.NotDefaultInt32.Cases), MemberType = typeof(FluentDefaultEqualityExtensionsTestData.NotDefaultInt32))]
    public void NotDefaultInt32_BehavesAsExpected(FluentCase<int> tc)
    {
        var result = new NotDefaultIntValidator().Validate(new IntModel { Value = tc.Value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDefaultEqualityExtensionsTestData.NullOrDefaultString.Cases), MemberType = typeof(FluentDefaultEqualityExtensionsTestData.NullOrDefaultString))]
    public void NullOrDefaultString_BehavesAsExpected(FluentCase<string?> tc)
    {
        var result = new NullOrDefaultStringValidator().Validate(new StringModel { Value = tc.Value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDefaultEqualityExtensionsTestData.NotNullOrDefaultString.Cases), MemberType = typeof(FluentDefaultEqualityExtensionsTestData.NotNullOrDefaultString))]
    public void NotNullOrDefaultString_BehavesAsExpected(FluentCase<string?> tc)
    {
        var result = new NotNullOrDefaultStringValidator().Validate(new StringModel { Value = tc.Value });
        AssertResult(tc, result);
    }
}
