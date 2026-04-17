using FluentValidation;
using PineGuard.Testing.UnitTests.FluentValidation;
using Xunit.Abstractions;

namespace PineGuard.FluentValidation.UnitTests;

public sealed class FluentPredicateExtensionsTests(ITestOutputHelper output) : BaseFluentUnitTest(output)
{
    private sealed record Model { public int Value { get; init; } }

    private sealed class SatisfiesValidator : AbstractValidator<Model>
    {
        public SatisfiesValidator() => RuleFor(x => x.Value).Satisfies(x => x > 0);
    }

    private sealed class NotSatisfiesValidator : AbstractValidator<Model>
    {
        public NotSatisfiesValidator() => RuleFor(x => x.Value).NotSatisfies(x => x > 0);
    }

    [Theory]
    [MemberData(nameof(FluentPredicateExtensionsTestData.Satisfies.Cases), MemberType = typeof(FluentPredicateExtensionsTestData.Satisfies))]
    public void Satisfies_BehavesAsExpected(FluentCase<int> tc)
    {
        var result = new SatisfiesValidator().Validate(new Model { Value = tc.Value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentPredicateExtensionsTestData.NotSatisfies.Cases), MemberType = typeof(FluentPredicateExtensionsTestData.NotSatisfies))]
    public void NotSatisfies_BehavesAsExpected(FluentCase<int> tc)
    {
        var result = new NotSatisfiesValidator().Validate(new Model { Value = tc.Value });
        AssertResult(tc, result);
    }
}
