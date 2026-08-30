using FluentValidation;
using PineGuard.Common;
using PineGuard.Testing.UnitTests.FluentValidation;
using Xunit.Abstractions;

namespace PineGuard.FluentValidation.UnitTests;

public sealed class FluentStringGraphemesExtensionsTests(ITestOutputHelper output) : BaseFluentUnitTest(output)
{
    private sealed record Model { public string? Value { get; init; } }

    private sealed class HasExactGraphemeCountValidator : AbstractValidator<Model>
    {
        public HasExactGraphemeCountValidator(int count) => RuleFor(x => x.Value).HasExactGraphemeCount(count);
    }

    private sealed class NotHasExactGraphemeCountValidator : AbstractValidator<Model>
    {
        public NotHasExactGraphemeCountValidator(int count) => RuleFor(x => x.Value).NotHasExactGraphemeCount(count);
    }

    private sealed class HasMinGraphemeCountValidator : AbstractValidator<Model>
    {
        public HasMinGraphemeCountValidator(int min) => RuleFor(x => x.Value).HasMinGraphemeCount(min);
    }

    private sealed class NotHasMinGraphemeCountValidator : AbstractValidator<Model>
    {
        public NotHasMinGraphemeCountValidator(int min) => RuleFor(x => x.Value).NotHasMinGraphemeCount(min);
    }

    private sealed class HasMaxGraphemeCountValidator : AbstractValidator<Model>
    {
        public HasMaxGraphemeCountValidator(int max) => RuleFor(x => x.Value).HasMaxGraphemeCount(max);
    }

    private sealed class NotHasMaxGraphemeCountValidator : AbstractValidator<Model>
    {
        public NotHasMaxGraphemeCountValidator(int max) => RuleFor(x => x.Value).NotHasMaxGraphemeCount(max);
    }

    private sealed class HasGraphemeCountBetweenValidator : AbstractValidator<Model>
    {
        public HasGraphemeCountBetweenValidator(int min, int max, Inclusion inclusion) => RuleFor(x => x.Value).HasGraphemeCountBetween(min, max, inclusion);
    }

    private sealed class NotHasGraphemeCountBetweenValidator : AbstractValidator<Model>
    {
        public NotHasGraphemeCountBetweenValidator(int min, int max, Inclusion inclusion) => RuleFor(x => x.Value).NotHasGraphemeCountBetween(min, max, inclusion);
    }

    // FluentStringGraphemesExtensions.HasExactGraphemeCount
    [Theory]
    [MemberData(nameof(FluentStringGraphemesExtensionsTestData.HasExactGraphemeCount.Cases), MemberType = typeof(FluentStringGraphemesExtensionsTestData.HasExactGraphemeCount))]
    public void HasExactGraphemeCount_BehavesAsExpected(FluentCase<(string? value, int count)> tc)
    {
        // Act
        var result = new HasExactGraphemeCountValidator(tc.Value.count).Validate(new Model { Value = tc.Value.value });

        // Assert
        AssertResult(tc, result);
    }

    // FluentStringGraphemesExtensions.NotHasExactGraphemeCount
    [Theory]
    [MemberData(nameof(FluentStringGraphemesExtensionsTestData.NotHasExactGraphemeCount.Cases), MemberType = typeof(FluentStringGraphemesExtensionsTestData.NotHasExactGraphemeCount))]
    public void NotHasExactGraphemeCount_BehavesAsExpected(FluentCase<(string? value, int count)> tc)
    {
        // Act
        var result = new NotHasExactGraphemeCountValidator(tc.Value.count).Validate(new Model { Value = tc.Value.value });

        // Assert
        AssertResult(tc, result);
    }

    // FluentStringGraphemesExtensions.HasMinGraphemeCount
    [Theory]
    [MemberData(nameof(FluentStringGraphemesExtensionsTestData.HasMinGraphemeCount.Cases), MemberType = typeof(FluentStringGraphemesExtensionsTestData.HasMinGraphemeCount))]
    public void HasMinGraphemeCount_BehavesAsExpected(FluentCase<(string? value, int min)> tc)
    {
        // Act
        var result = new HasMinGraphemeCountValidator(tc.Value.min).Validate(new Model { Value = tc.Value.value });

        // Assert
        AssertResult(tc, result);
    }

    // FluentStringGraphemesExtensions.NotHasMinGraphemeCount
    [Theory]
    [MemberData(nameof(FluentStringGraphemesExtensionsTestData.NotHasMinGraphemeCount.Cases), MemberType = typeof(FluentStringGraphemesExtensionsTestData.NotHasMinGraphemeCount))]
    public void NotHasMinGraphemeCount_BehavesAsExpected(FluentCase<(string? value, int min)> tc)
    {
        // Act
        var result = new NotHasMinGraphemeCountValidator(tc.Value.min).Validate(new Model { Value = tc.Value.value });

        // Assert
        AssertResult(tc, result);
    }

    // FluentStringGraphemesExtensions.HasMaxGraphemeCount
    [Theory]
    [MemberData(nameof(FluentStringGraphemesExtensionsTestData.HasMaxGraphemeCount.Cases), MemberType = typeof(FluentStringGraphemesExtensionsTestData.HasMaxGraphemeCount))]
    public void HasMaxGraphemeCount_BehavesAsExpected(FluentCase<(string? value, int max)> tc)
    {
        // Act
        var result = new HasMaxGraphemeCountValidator(tc.Value.max).Validate(new Model { Value = tc.Value.value });

        // Assert
        AssertResult(tc, result);
    }

    // FluentStringGraphemesExtensions.NotHasMaxGraphemeCount
    [Theory]
    [MemberData(nameof(FluentStringGraphemesExtensionsTestData.NotHasMaxGraphemeCount.Cases), MemberType = typeof(FluentStringGraphemesExtensionsTestData.NotHasMaxGraphemeCount))]
    public void NotHasMaxGraphemeCount_BehavesAsExpected(FluentCase<(string? value, int max)> tc)
    {
        // Act
        var result = new NotHasMaxGraphemeCountValidator(tc.Value.max).Validate(new Model { Value = tc.Value.value });

        // Assert
        AssertResult(tc, result);
    }

    // FluentStringGraphemesExtensions.HasGraphemeCountBetween
    [Theory]
    [MemberData(nameof(FluentStringGraphemesExtensionsTestData.HasGraphemeCountBetween.Cases), MemberType = typeof(FluentStringGraphemesExtensionsTestData.HasGraphemeCountBetween))]
    public void HasGraphemeCountBetween_BehavesAsExpected(FluentCase<(string? value, int min, int max, Inclusion inclusion)> tc)
    {
        // Act
        var result = new HasGraphemeCountBetweenValidator(tc.Value.min, tc.Value.max, tc.Value.inclusion).Validate(new Model { Value = tc.Value.value });

        // Assert
        AssertResult(tc, result);
    }

    // FluentStringGraphemesExtensions.NotHasGraphemeCountBetween
    [Theory]
    [MemberData(nameof(FluentStringGraphemesExtensionsTestData.NotHasGraphemeCountBetween.Cases), MemberType = typeof(FluentStringGraphemesExtensionsTestData.NotHasGraphemeCountBetween))]
    public void NotHasGraphemeCountBetween_BehavesAsExpected(FluentCase<(string? value, int min, int max, Inclusion inclusion)> tc)
    {
        // Act
        var result = new NotHasGraphemeCountBetweenValidator(tc.Value.min, tc.Value.max, tc.Value.inclusion).Validate(new Model { Value = tc.Value.value });

        // Assert
        AssertResult(tc, result);
    }
}
