using FluentValidation;
using PineGuard.Testing.UnitTests.FluentValidation;
using Xunit.Abstractions;

namespace PineGuard.FluentValidation.UnitTests;

public sealed class FluentStringTimeOnlyExtensionsTests(ITestOutputHelper output) : BaseFluentUnitTest(output)
{
    private sealed record Model { public string? Value { get; init; } }

    private static readonly TimeOnly T1000 = new(10, 0);
    private static readonly TimeOnly T1200 = new(12, 0);
    private static readonly string S1200 = T1200.ToString("HH:mm");
    private static readonly string S0800 = new TimeOnly(8, 0).ToString("HH:mm");
    private static readonly string S0900 = new TimeOnly(9, 0).ToString("HH:mm");

    private sealed class BetweenTimeOnlyValidator : AbstractValidator<Model>
    { public BetweenTimeOnlyValidator() => RuleFor(x => x.Value).BetweenTimeOnly(T1000, T1200); }

    [Theory]
    [MemberData(nameof(FluentStringTimeOnlyExtensionsTestData.BetweenTimeOnly.Cases), MemberType = typeof(FluentStringTimeOnlyExtensionsTestData.BetweenTimeOnly))]
    public void BetweenTimeOnly_BehavesAsExpected(FluentCase<string?> tc)
    { AssertResult(tc, new BetweenTimeOnlyValidator().Validate(new Model { Value = tc.Value })); }

    private sealed class NotBetweenTimeOnlyValidator : AbstractValidator<Model>
    { public NotBetweenTimeOnlyValidator() => RuleFor(x => x.Value).NotBetweenTimeOnly(T1000, T1200); }

    [Theory]
    [MemberData(nameof(FluentStringTimeOnlyExtensionsTestData.NotBetweenTimeOnly.Cases), MemberType = typeof(FluentStringTimeOnlyExtensionsTestData.NotBetweenTimeOnly))]
    public void NotBetweenTimeOnly_BehavesAsExpected(FluentCase<string?> tc)
    { AssertResult(tc, new NotBetweenTimeOnlyValidator().Validate(new Model { Value = tc.Value })); }

    private sealed class WithinTimeOnlyValidator : AbstractValidator<Model>
    { public WithinTimeOnlyValidator() => RuleFor(x => x.Value).WithinTimeOnly(S1200, TimeSpan.FromMinutes(30)); }

    [Theory]
    [MemberData(nameof(FluentStringTimeOnlyExtensionsTestData.WithinTimeOnly.Cases), MemberType = typeof(FluentStringTimeOnlyExtensionsTestData.WithinTimeOnly))]
    public void WithinTimeOnly_BehavesAsExpected(FluentCase<string?> tc)
    { AssertResult(tc, new WithinTimeOnlyValidator().Validate(new Model { Value = tc.Value })); }

    private sealed class NotWithinTimeOnlyValidator : AbstractValidator<Model>
    { public NotWithinTimeOnlyValidator() => RuleFor(x => x.Value).NotWithinTimeOnly(S1200, TimeSpan.FromMinutes(30)); }

    [Theory]
    [MemberData(nameof(FluentStringTimeOnlyExtensionsTestData.NotWithinTimeOnly.Cases), MemberType = typeof(FluentStringTimeOnlyExtensionsTestData.NotWithinTimeOnly))]
    public void NotWithinTimeOnly_BehavesAsExpected(FluentCase<string?> tc)
    { AssertResult(tc, new NotWithinTimeOnlyValidator().Validate(new Model { Value = tc.Value })); }

    private sealed class BeforeTimeOnlyValidator : AbstractValidator<Model>
    { public BeforeTimeOnlyValidator() => RuleFor(x => x.Value).BeforeTimeOnly(T1200); }

    [Theory]
    [MemberData(nameof(FluentStringTimeOnlyExtensionsTestData.BeforeTimeOnly.Cases), MemberType = typeof(FluentStringTimeOnlyExtensionsTestData.BeforeTimeOnly))]
    public void BeforeTimeOnly_BehavesAsExpected(FluentCase<string?> tc)
    { AssertResult(tc, new BeforeTimeOnlyValidator().Validate(new Model { Value = tc.Value })); }

    private sealed class NotBeforeTimeOnlyValidator : AbstractValidator<Model>
    { public NotBeforeTimeOnlyValidator() => RuleFor(x => x.Value).NotBeforeTimeOnly(T1200); }

    [Theory]
    [MemberData(nameof(FluentStringTimeOnlyExtensionsTestData.NotBeforeTimeOnly.Cases), MemberType = typeof(FluentStringTimeOnlyExtensionsTestData.NotBeforeTimeOnly))]
    public void NotBeforeTimeOnly_BehavesAsExpected(FluentCase<string?> tc)
    { AssertResult(tc, new NotBeforeTimeOnlyValidator().Validate(new Model { Value = tc.Value })); }

    private sealed class OnOrBeforeTimeOnlyValidator : AbstractValidator<Model>
    { public OnOrBeforeTimeOnlyValidator() => RuleFor(x => x.Value).OnOrBeforeTimeOnly(T1200); }

    [Theory]
    [MemberData(nameof(FluentStringTimeOnlyExtensionsTestData.OnOrBeforeTimeOnly.Cases), MemberType = typeof(FluentStringTimeOnlyExtensionsTestData.OnOrBeforeTimeOnly))]
    public void OnOrBeforeTimeOnly_BehavesAsExpected(FluentCase<string?> tc)
    { AssertResult(tc, new OnOrBeforeTimeOnlyValidator().Validate(new Model { Value = tc.Value })); }

    private sealed class NotOnOrBeforeTimeOnlyValidator : AbstractValidator<Model>
    { public NotOnOrBeforeTimeOnlyValidator() => RuleFor(x => x.Value).NotOnOrBeforeTimeOnly(T1200); }

    [Theory]
    [MemberData(nameof(FluentStringTimeOnlyExtensionsTestData.NotOnOrBeforeTimeOnly.Cases), MemberType = typeof(FluentStringTimeOnlyExtensionsTestData.NotOnOrBeforeTimeOnly))]
    public void NotOnOrBeforeTimeOnly_BehavesAsExpected(FluentCase<string?> tc)
    { AssertResult(tc, new NotOnOrBeforeTimeOnlyValidator().Validate(new Model { Value = tc.Value })); }

    private sealed class AfterTimeOnlyValidator : AbstractValidator<Model>
    { public AfterTimeOnlyValidator() => RuleFor(x => x.Value).AfterTimeOnly(T1200); }

    [Theory]
    [MemberData(nameof(FluentStringTimeOnlyExtensionsTestData.AfterTimeOnly.Cases), MemberType = typeof(FluentStringTimeOnlyExtensionsTestData.AfterTimeOnly))]
    public void AfterTimeOnly_BehavesAsExpected(FluentCase<string?> tc)
    { AssertResult(tc, new AfterTimeOnlyValidator().Validate(new Model { Value = tc.Value })); }

    private sealed class NotAfterTimeOnlyValidator : AbstractValidator<Model>
    { public NotAfterTimeOnlyValidator() => RuleFor(x => x.Value).NotAfterTimeOnly(T1200); }

    [Theory]
    [MemberData(nameof(FluentStringTimeOnlyExtensionsTestData.NotAfterTimeOnly.Cases), MemberType = typeof(FluentStringTimeOnlyExtensionsTestData.NotAfterTimeOnly))]
    public void NotAfterTimeOnly_BehavesAsExpected(FluentCase<string?> tc)
    { AssertResult(tc, new NotAfterTimeOnlyValidator().Validate(new Model { Value = tc.Value })); }

    private sealed class OnOrAfterTimeOnlyValidator : AbstractValidator<Model>
    { public OnOrAfterTimeOnlyValidator() => RuleFor(x => x.Value).OnOrAfterTimeOnly(T1200); }

    [Theory]
    [MemberData(nameof(FluentStringTimeOnlyExtensionsTestData.OnOrAfterTimeOnly.Cases), MemberType = typeof(FluentStringTimeOnlyExtensionsTestData.OnOrAfterTimeOnly))]
    public void OnOrAfterTimeOnly_BehavesAsExpected(FluentCase<string?> tc)
    { AssertResult(tc, new OnOrAfterTimeOnlyValidator().Validate(new Model { Value = tc.Value })); }

    private sealed class NotOnOrAfterTimeOnlyValidator : AbstractValidator<Model>
    { public NotOnOrAfterTimeOnlyValidator() => RuleFor(x => x.Value).NotOnOrAfterTimeOnly(T1200); }

    [Theory]
    [MemberData(nameof(FluentStringTimeOnlyExtensionsTestData.NotOnOrAfterTimeOnly.Cases), MemberType = typeof(FluentStringTimeOnlyExtensionsTestData.NotOnOrAfterTimeOnly))]
    public void NotOnOrAfterTimeOnly_BehavesAsExpected(FluentCase<string?> tc)
    { AssertResult(tc, new NotOnOrAfterTimeOnlyValidator().Validate(new Model { Value = tc.Value })); }

    private sealed class SameTimeOnlyValidator : AbstractValidator<Model>
    { public SameTimeOnlyValidator() => RuleFor(x => x.Value).SameTimeOnly(T1200); }

    [Theory]
    [MemberData(nameof(FluentStringTimeOnlyExtensionsTestData.SameTimeOnly.Cases), MemberType = typeof(FluentStringTimeOnlyExtensionsTestData.SameTimeOnly))]
    public void SameTimeOnly_BehavesAsExpected(FluentCase<string?> tc)
    { AssertResult(tc, new SameTimeOnlyValidator().Validate(new Model { Value = tc.Value })); }

    private sealed class NotSameTimeOnlyValidator : AbstractValidator<Model>
    { public NotSameTimeOnlyValidator() => RuleFor(x => x.Value).NotSameTimeOnly(T1200); }

    [Theory]
    [MemberData(nameof(FluentStringTimeOnlyExtensionsTestData.NotSameTimeOnly.Cases), MemberType = typeof(FluentStringTimeOnlyExtensionsTestData.NotSameTimeOnly))]
    public void NotSameTimeOnly_BehavesAsExpected(FluentCase<string?> tc)
    { AssertResult(tc, new NotSameTimeOnlyValidator().Validate(new Model { Value = tc.Value })); }

    private sealed class ChronologicalTimeOnlyValidator : AbstractValidator<Model>
    { public ChronologicalTimeOnlyValidator() => RuleFor(x => x.Value).ChronologicalTimeOnly(S1200).WithName("Value"); }

    [Theory]
    [MemberData(nameof(FluentStringTimeOnlyExtensionsTestData.ChronologicalTimeOnly.Cases), MemberType = typeof(FluentStringTimeOnlyExtensionsTestData.ChronologicalTimeOnly))]
    public void ChronologicalTimeOnly_BehavesAsExpected(FluentCase<string?> tc)
    { AssertResult(tc, new ChronologicalTimeOnlyValidator().Validate(new Model { Value = tc.Value })); }

    private sealed class NotChronologicalTimeOnlyValidator : AbstractValidator<Model>
    { public NotChronologicalTimeOnlyValidator() => RuleFor(x => x.Value).NotChronologicalTimeOnly(S1200).WithName("Value"); }

    [Theory]
    [MemberData(nameof(FluentStringTimeOnlyExtensionsTestData.NotChronologicalTimeOnly.Cases), MemberType = typeof(FluentStringTimeOnlyExtensionsTestData.NotChronologicalTimeOnly))]
    public void NotChronologicalTimeOnly_BehavesAsExpected(FluentCase<string?> tc)
    { AssertResult(tc, new NotChronologicalTimeOnlyValidator().Validate(new Model { Value = tc.Value })); }

    private sealed class OverlappingTimeOnlyValidator : AbstractValidator<Model>
    { public OverlappingTimeOnlyValidator() => RuleFor(x => x.Value).OverlappingTimeOnly(S0900, S0800, S0900).WithName("Value"); }

    [Theory]
    [MemberData(nameof(FluentStringTimeOnlyExtensionsTestData.OverlappingTimeOnly.Cases), MemberType = typeof(FluentStringTimeOnlyExtensionsTestData.OverlappingTimeOnly))]
    public void OverlappingTimeOnly_BehavesAsExpected(FluentCase<string?> tc)
    { AssertResult(tc, new OverlappingTimeOnlyValidator().Validate(new Model { Value = tc.Value })); }

    private sealed class NotOverlappingTimeOnlyValidator : AbstractValidator<Model>
    { public NotOverlappingTimeOnlyValidator() => RuleFor(x => x.Value).NotOverlappingTimeOnly(S0900, S0800, S0900).WithName("Value"); }

    [Theory]
    [MemberData(nameof(FluentStringTimeOnlyExtensionsTestData.NotOverlappingTimeOnly.Cases), MemberType = typeof(FluentStringTimeOnlyExtensionsTestData.NotOverlappingTimeOnly))]
    public void NotOverlappingTimeOnly_BehavesAsExpected(FluentCase<string?> tc)
    { AssertResult(tc, new NotOverlappingTimeOnlyValidator().Validate(new Model { Value = tc.Value })); }
}
