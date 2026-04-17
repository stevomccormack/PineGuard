using FluentValidation;
using PineGuard.Testing.UnitTests.FluentValidation;
using Xunit.Abstractions;

namespace PineGuard.FluentValidation.UnitTests;

public sealed class FluentDictionaryExtensionsTests(ITestOutputHelper output) : BaseFluentUnitTest(output)
{
    private sealed record Model { public IDictionary<string, int>? Dict { get; init; } }

    private sealed class EmptyValidator : AbstractValidator<Model>
    {
        public EmptyValidator() => RuleFor(x => x.Dict).Empty();
    }

    private sealed class NotEmptyValidator : AbstractValidator<Model>
    {
        public NotEmptyValidator() => RuleFor(x => x.Dict).NotEmpty();
    }

    [Theory]
    [MemberData(nameof(FluentDictionaryExtensionsTestData.Empty.Cases), MemberType = typeof(FluentDictionaryExtensionsTestData.Empty))]
    public void Empty_BehavesAsExpected(FluentCase<IDictionary<string, int>?> tc)
    {
        var result = new EmptyValidator().Validate(new Model { Dict = tc.Value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDictionaryExtensionsTestData.NotEmpty.Cases), MemberType = typeof(FluentDictionaryExtensionsTestData.NotEmpty))]
    public void NotEmpty_BehavesAsExpected(FluentCase<IDictionary<string, int>?> tc)
    {
        var result = new NotEmptyValidator().Validate(new Model { Dict = tc.Value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDictionaryExtensionsTestData.HasKey.Cases), MemberType = typeof(FluentDictionaryExtensionsTestData.HasKey))]
    public void HasKey_BehavesAsExpected(FluentCase<(IDictionary<string, int>? dictionary, string key)> tc)
    {
        var validator = new InlineValidator<Model>();
        validator.RuleFor(x => x.Dict).HasKey(tc.Value.key);
        var result = validator.Validate(new Model { Dict = tc.Value.dictionary });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDictionaryExtensionsTestData.NotHasKey.Cases), MemberType = typeof(FluentDictionaryExtensionsTestData.NotHasKey))]
    public void NotHasKey_BehavesAsExpected(FluentCase<(IDictionary<string, int>? dictionary, string key)> tc)
    {
        var validator = new InlineValidator<Model>();
        validator.RuleFor(x => x.Dict).NotHasKey(tc.Value.key);
        var result = validator.Validate(new Model { Dict = tc.Value.dictionary });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDictionaryExtensionsTestData.HasValue.Cases), MemberType = typeof(FluentDictionaryExtensionsTestData.HasValue))]
    public void HasValue_BehavesAsExpected(FluentCase<(IDictionary<string, int>? dictionary, int value)> tc)
    {
        var validator = new InlineValidator<Model>();
        validator.RuleFor(x => x.Dict).HasValue(tc.Value.value);
        var result = validator.Validate(new Model { Dict = tc.Value.dictionary });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDictionaryExtensionsTestData.NotHasValue.Cases), MemberType = typeof(FluentDictionaryExtensionsTestData.NotHasValue))]
    public void NotHasValue_BehavesAsExpected(FluentCase<(IDictionary<string, int>? dictionary, int value)> tc)
    {
        var validator = new InlineValidator<Model>();
        validator.RuleFor(x => x.Dict).NotHasValue(tc.Value.value);
        var result = validator.Validate(new Model { Dict = tc.Value.dictionary });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDictionaryExtensionsTestData.HasKeyValue.Cases), MemberType = typeof(FluentDictionaryExtensionsTestData.HasKeyValue))]
    public void HasKeyValue_BehavesAsExpected(FluentCase<(IDictionary<string, int>? dictionary, string key, int value)> tc)
    {
        var validator = new InlineValidator<Model>();
        validator.RuleFor(x => x.Dict).HasKeyValue(tc.Value.key, tc.Value.value);
        var result = validator.Validate(new Model { Dict = tc.Value.dictionary });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDictionaryExtensionsTestData.NotHasKeyValue.Cases), MemberType = typeof(FluentDictionaryExtensionsTestData.NotHasKeyValue))]
    public void NotHasKeyValue_BehavesAsExpected(FluentCase<(IDictionary<string, int>? dictionary, string key, int value)> tc)
    {
        var validator = new InlineValidator<Model>();
        validator.RuleFor(x => x.Dict).NotHasKeyValue(tc.Value.key, tc.Value.value);
        var result = validator.Validate(new Model { Dict = tc.Value.dictionary });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDictionaryExtensionsTestData.HasAnyKey.Cases), MemberType = typeof(FluentDictionaryExtensionsTestData.HasAnyKey))]
    public void HasAnyKey_BehavesAsExpected(FluentCase<(IDictionary<string, int>? dictionary, Func<string, bool> predicate)> tc)
    {
        var validator = new InlineValidator<Model>();
        validator.RuleFor(x => x.Dict).HasAnyKey(tc.Value.predicate);
        var result = validator.Validate(new Model { Dict = tc.Value.dictionary });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDictionaryExtensionsTestData.NotHasAnyKey.Cases), MemberType = typeof(FluentDictionaryExtensionsTestData.NotHasAnyKey))]
    public void NotHasAnyKey_BehavesAsExpected(FluentCase<(IDictionary<string, int>? dictionary, Func<string, bool> predicate)> tc)
    {
        var validator = new InlineValidator<Model>();
        validator.RuleFor(x => x.Dict).NotHasAnyKey(tc.Value.predicate);
        var result = validator.Validate(new Model { Dict = tc.Value.dictionary });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDictionaryExtensionsTestData.HasAnyValue.Cases), MemberType = typeof(FluentDictionaryExtensionsTestData.HasAnyValue))]
    public void HasAnyValue_BehavesAsExpected(FluentCase<(IDictionary<string, int>? dictionary, Func<int, bool> predicate)> tc)
    {
        var validator = new InlineValidator<Model>();
        validator.RuleFor(x => x.Dict).HasAnyValue(tc.Value.predicate);
        var result = validator.Validate(new Model { Dict = tc.Value.dictionary });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDictionaryExtensionsTestData.NotHasAnyValue.Cases), MemberType = typeof(FluentDictionaryExtensionsTestData.NotHasAnyValue))]
    public void NotHasAnyValue_BehavesAsExpected(FluentCase<(IDictionary<string, int>? dictionary, Func<int, bool> predicate)> tc)
    {
        var validator = new InlineValidator<Model>();
        validator.RuleFor(x => x.Dict).NotHasAnyValue(tc.Value.predicate);
        var result = validator.Validate(new Model { Dict = tc.Value.dictionary });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDictionaryExtensionsTestData.HasAnyItem.Cases), MemberType = typeof(FluentDictionaryExtensionsTestData.HasAnyItem))]
    public void HasAnyItem_BehavesAsExpected(FluentCase<(IDictionary<string, int>? dictionary, Func<string, int, bool> predicate)> tc)
    {
        var validator = new InlineValidator<Model>();
        validator.RuleFor(x => x.Dict).HasAnyItem(tc.Value.predicate);
        var result = validator.Validate(new Model { Dict = tc.Value.dictionary });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDictionaryExtensionsTestData.NotHasAnyItem.Cases), MemberType = typeof(FluentDictionaryExtensionsTestData.NotHasAnyItem))]
    public void NotHasAnyItem_BehavesAsExpected(FluentCase<(IDictionary<string, int>? dictionary, Func<string, int, bool> predicate)> tc)
    {
        var validator = new InlineValidator<Model>();
        validator.RuleFor(x => x.Dict).NotHasAnyItem(tc.Value.predicate);
        var result = validator.Validate(new Model { Dict = tc.Value.dictionary });
        AssertResult(tc, result);
    }

    [Fact]
    public void Empty_IRuleBuilderOptions_IsCallable()
    {
        var validator = new InlineValidator<Model>();
        var opts = validator.RuleFor(x => x.Dict).NotEmpty();
        opts.Empty();
        var result = validator.Validate(new Model { Dict = new Dictionary<string, int> { { "a", 1 } } });
        Assert.False(result.IsValid);
    }

    [Fact]
    public void Empty_IRuleBuilder_IsCallable()
    {
        var validator = new InlineValidator<Model>();
        IRuleBuilder<Model, IDictionary<string, int>?> rb = validator.RuleFor(x => x.Dict);
        rb.Empty();
        var result = validator.Validate(new Model { Dict = new Dictionary<string, int> { { "a", 1 } } });
        Assert.False(result.IsValid);
    }

    [Fact]
    public void NotEmpty_IRuleBuilderOptions_IsCallable()
    {
        var validator = new InlineValidator<Model>();
        var opts = validator.RuleFor(x => x.Dict).Empty();
        opts.NotEmpty();
        var result = validator.Validate(new Model { Dict = new Dictionary<string, int>() });
        Assert.False(result.IsValid);
    }

    [Fact]
    public void NotEmpty_IRuleBuilder_IsCallable()
    {
        var validator = new InlineValidator<Model>();
        IRuleBuilder<Model, IDictionary<string, int>?> rb = validator.RuleFor(x => x.Dict);
        rb.NotEmpty();
        var result = validator.Validate(new Model { Dict = new Dictionary<string, int>() });
        Assert.False(result.IsValid);
    }
}
