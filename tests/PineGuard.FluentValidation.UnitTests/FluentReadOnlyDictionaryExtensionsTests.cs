using FluentValidation;
using FluentValidation.Results;
using PineGuard.Testing.UnitTests.FluentValidation;
using Xunit.Abstractions;

namespace PineGuard.FluentValidation.UnitTests;

public sealed class FluentReadOnlyDictionaryExtensionsTests(ITestOutputHelper output) : BaseFluentUnitTest(output)
{
    private sealed record Model { public IReadOnlyDictionary<string, int>? Dict { get; init; } }

    private sealed class EmptyValidator : AbstractValidator<Model>
    {
        public EmptyValidator() => RuleFor(x => x.Dict).Empty();
    }

    private sealed class NotEmptyValidator : AbstractValidator<Model>
    {
        public NotEmptyValidator() => RuleFor(x => x.Dict).NotEmpty();
    }

    [Theory]
    [MemberData(nameof(FluentReadOnlyDictionaryExtensionsTestData.Empty.Cases), MemberType = typeof(FluentReadOnlyDictionaryExtensionsTestData.Empty))]
    public void Empty_BehavesAsExpected(FluentCase<IReadOnlyDictionary<string, int>?> tc)
    {
        var result = new EmptyValidator().Validate(new Model { Dict = tc.Value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentReadOnlyDictionaryExtensionsTestData.NotEmpty.Cases), MemberType = typeof(FluentReadOnlyDictionaryExtensionsTestData.NotEmpty))]
    public void NotEmpty_BehavesAsExpected(FluentCase<IReadOnlyDictionary<string, int>?> tc)
    {
        var result = new NotEmptyValidator().Validate(new Model { Dict = tc.Value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentReadOnlyDictionaryExtensionsTestData.HasKey.Cases), MemberType = typeof(FluentReadOnlyDictionaryExtensionsTestData.HasKey))]
    public void HasKey_BehavesAsExpected(FluentCase<(IReadOnlyDictionary<string, int>? dictionary, string key)> tc)
    {
        var validator = new InlineValidator<Model>();
        validator.RuleFor(x => x.Dict).HasKey(tc.Value.key);
        var result = validator.Validate(new Model { Dict = tc.Value.dictionary });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentReadOnlyDictionaryExtensionsTestData.NotHasKey.Cases), MemberType = typeof(FluentReadOnlyDictionaryExtensionsTestData.NotHasKey))]
    public void NotHasKey_BehavesAsExpected(FluentCase<(IReadOnlyDictionary<string, int>? dictionary, string key)> tc)
    {
        var validator = new InlineValidator<Model>();
        validator.RuleFor(x => x.Dict).NotHasKey(tc.Value.key);
        var result = validator.Validate(new Model { Dict = tc.Value.dictionary });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentReadOnlyDictionaryExtensionsTestData.HasValue.Cases), MemberType = typeof(FluentReadOnlyDictionaryExtensionsTestData.HasValue))]
    public void HasValue_BehavesAsExpected(FluentCase<(IReadOnlyDictionary<string, int>? dictionary, int value)> tc)
    {
        var validator = new InlineValidator<Model>();
        validator.RuleFor(x => x.Dict).HasValue(tc.Value.value);
        var result = validator.Validate(new Model { Dict = tc.Value.dictionary });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentReadOnlyDictionaryExtensionsTestData.NotHasValue.Cases), MemberType = typeof(FluentReadOnlyDictionaryExtensionsTestData.NotHasValue))]
    public void NotHasValue_BehavesAsExpected(FluentCase<(IReadOnlyDictionary<string, int>? dictionary, int value)> tc)
    {
        var validator = new InlineValidator<Model>();
        validator.RuleFor(x => x.Dict).NotHasValue(tc.Value.value);
        var result = validator.Validate(new Model { Dict = tc.Value.dictionary });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentReadOnlyDictionaryExtensionsTestData.HasKeyValue.Cases), MemberType = typeof(FluentReadOnlyDictionaryExtensionsTestData.HasKeyValue))]
    public void HasKeyValue_BehavesAsExpected(FluentCase<(IReadOnlyDictionary<string, int>? dictionary, string key, int value)> tc)
    {
        var validator = new InlineValidator<Model>();
        validator.RuleFor(x => x.Dict).HasKeyValue(tc.Value.key, tc.Value.value);
        var result = validator.Validate(new Model { Dict = tc.Value.dictionary });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentReadOnlyDictionaryExtensionsTestData.NotHasKeyValue.Cases), MemberType = typeof(FluentReadOnlyDictionaryExtensionsTestData.NotHasKeyValue))]
    public void NotHasKeyValue_BehavesAsExpected(FluentCase<(IReadOnlyDictionary<string, int>? dictionary, string key, int value)> tc)
    {
        var validator = new InlineValidator<Model>();
        validator.RuleFor(x => x.Dict).NotHasKeyValue(tc.Value.key, tc.Value.value);
        var result = validator.Validate(new Model { Dict = tc.Value.dictionary });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentReadOnlyDictionaryExtensionsTestData.HasAnyKey.Cases), MemberType = typeof(FluentReadOnlyDictionaryExtensionsTestData.HasAnyKey))]
    public void HasAnyKey_BehavesAsExpected(FluentCase<(IReadOnlyDictionary<string, int>? dictionary, Func<string, bool> predicate)> tc)
    {
        var validator = new InlineValidator<Model>();
        validator.RuleFor(x => x.Dict).HasAnyKey(tc.Value.predicate);
        var result = validator.Validate(new Model { Dict = tc.Value.dictionary });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentReadOnlyDictionaryExtensionsTestData.NotHasAnyKey.Cases), MemberType = typeof(FluentReadOnlyDictionaryExtensionsTestData.NotHasAnyKey))]
    public void NotHasAnyKey_BehavesAsExpected(FluentCase<(IReadOnlyDictionary<string, int>? dictionary, Func<string, bool> predicate)> tc)
    {
        var validator = new InlineValidator<Model>();
        validator.RuleFor(x => x.Dict).NotHasAnyKey(tc.Value.predicate);
        var result = validator.Validate(new Model { Dict = tc.Value.dictionary });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentReadOnlyDictionaryExtensionsTestData.HasAnyValue.Cases), MemberType = typeof(FluentReadOnlyDictionaryExtensionsTestData.HasAnyValue))]
    public void HasAnyValue_BehavesAsExpected(FluentCase<(IReadOnlyDictionary<string, int>? dictionary, Func<int, bool> predicate)> tc)
    {
        var validator = new InlineValidator<Model>();
        validator.RuleFor(x => x.Dict).HasAnyValue(tc.Value.predicate);
        var result = validator.Validate(new Model { Dict = tc.Value.dictionary });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentReadOnlyDictionaryExtensionsTestData.NotHasAnyValue.Cases), MemberType = typeof(FluentReadOnlyDictionaryExtensionsTestData.NotHasAnyValue))]
    public void NotHasAnyValue_BehavesAsExpected(FluentCase<(IReadOnlyDictionary<string, int>? dictionary, Func<int, bool> predicate)> tc)
    {
        var validator = new InlineValidator<Model>();
        validator.RuleFor(x => x.Dict).NotHasAnyValue(tc.Value.predicate);
        var result = validator.Validate(new Model { Dict = tc.Value.dictionary });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentReadOnlyDictionaryExtensionsTestData.HasAnyItem.Cases), MemberType = typeof(FluentReadOnlyDictionaryExtensionsTestData.HasAnyItem))]
    public void HasAnyItem_BehavesAsExpected(FluentCase<(IReadOnlyDictionary<string, int>? dictionary, Func<string, int, bool> predicate)> tc)
    {
        var validator = new InlineValidator<Model>();
        validator.RuleFor(x => x.Dict).HasAnyItem(tc.Value.predicate);
        var result = validator.Validate(new Model { Dict = tc.Value.dictionary });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentReadOnlyDictionaryExtensionsTestData.NotHasAnyItem.Cases), MemberType = typeof(FluentReadOnlyDictionaryExtensionsTestData.NotHasAnyItem))]
    public void NotHasAnyItem_BehavesAsExpected(FluentCase<(IReadOnlyDictionary<string, int>? dictionary, Func<string, int, bool> predicate)> tc)
    {
        var validator = new InlineValidator<Model>();
        validator.RuleFor(x => x.Dict).NotHasAnyItem(tc.Value.predicate);
        var result = validator.Validate(new Model { Dict = tc.Value.dictionary });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentReadOnlyDictionaryExtensionsTestData.OverloadResolution.Cases), MemberType = typeof(FluentReadOnlyDictionaryExtensionsTestData.OverloadResolution))]
    public void OverloadResolution_BehavesAsExpected(FluentCase<Func<ValidationResult>> tc)
    {
        var result = tc.Value();
        AssertResult(tc, result);
    }
}
