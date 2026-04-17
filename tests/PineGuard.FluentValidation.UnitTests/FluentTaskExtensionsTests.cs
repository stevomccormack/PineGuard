using FluentValidation;
using FluentValidation.Results;
using PineGuard.Testing.UnitTests.FluentValidation;
using Xunit.Abstractions;

namespace PineGuard.FluentValidation.UnitTests;

public sealed class FluentTaskExtensionsTests(ITestOutputHelper output) : BaseFluentUnitTest(output)
{
    private sealed record Model { public Task? Task { get; init; } }

    [Theory]
    [MemberData(nameof(FluentTaskExtensionsTestData.Completed.ValidCases), MemberType = typeof(FluentTaskExtensionsTestData.Completed))]
    [MemberData(nameof(FluentTaskExtensionsTestData.Completed.InvalidCases), MemberType = typeof(FluentTaskExtensionsTestData.Completed))]
    public void Completed_BehavesAsExpected(FluentCase<Func<Task?>> tc) =>
        AssertResult(tc, Validate(tc, rb => rb.Completed()));

    [Theory]
    [MemberData(nameof(FluentTaskExtensionsTestData.NotCompleted.ValidCases), MemberType = typeof(FluentTaskExtensionsTestData.NotCompleted))]
    [MemberData(nameof(FluentTaskExtensionsTestData.NotCompleted.InvalidCases), MemberType = typeof(FluentTaskExtensionsTestData.NotCompleted))]
    public void NotCompleted_BehavesAsExpected(FluentCase<Func<Task?>> tc) =>
        AssertResult(tc, Validate(tc, rb => rb.NotCompleted()));

    [Theory]
    [MemberData(nameof(FluentTaskExtensionsTestData.Canceled.ValidCases), MemberType = typeof(FluentTaskExtensionsTestData.Canceled))]
    [MemberData(nameof(FluentTaskExtensionsTestData.Canceled.InvalidCases), MemberType = typeof(FluentTaskExtensionsTestData.Canceled))]
    public void Canceled_BehavesAsExpected(FluentCase<Func<Task?>> tc) =>
        AssertResult(tc, Validate(tc, rb => rb.Canceled()));

    [Theory]
    [MemberData(nameof(FluentTaskExtensionsTestData.NotCanceled.ValidCases), MemberType = typeof(FluentTaskExtensionsTestData.NotCanceled))]
    [MemberData(nameof(FluentTaskExtensionsTestData.NotCanceled.InvalidCases), MemberType = typeof(FluentTaskExtensionsTestData.NotCanceled))]
    public void NotCanceled_BehavesAsExpected(FluentCase<Func<Task?>> tc) =>
        AssertResult(tc, Validate(tc, rb => rb.NotCanceled()));

    [Theory]
    [MemberData(nameof(FluentTaskExtensionsTestData.Faulted.ValidCases), MemberType = typeof(FluentTaskExtensionsTestData.Faulted))]
    [MemberData(nameof(FluentTaskExtensionsTestData.Faulted.InvalidCases), MemberType = typeof(FluentTaskExtensionsTestData.Faulted))]
    public void Faulted_BehavesAsExpected(FluentCase<Func<Task?>> tc) =>
        AssertResult(tc, Validate(tc, rb => rb.Faulted()));

    [Theory]
    [MemberData(nameof(FluentTaskExtensionsTestData.NotFaulted.ValidCases), MemberType = typeof(FluentTaskExtensionsTestData.NotFaulted))]
    [MemberData(nameof(FluentTaskExtensionsTestData.NotFaulted.InvalidCases), MemberType = typeof(FluentTaskExtensionsTestData.NotFaulted))]
    public void NotFaulted_BehavesAsExpected(FluentCase<Func<Task?>> tc) =>
        AssertResult(tc, Validate(tc, rb => rb.NotFaulted()));

    private static ValidationResult Validate(
        FluentCase<Func<Task?>> tc,
        Func<IRuleBuilder<Model, Task?>, IRuleBuilderOptions<Model, Task?>> configure)
    {
        var validator = new InlineValidator<Model>();
        configure(validator.RuleFor(x => x.Task));
        var task = tc.Value.Invoke();
        if (task is { IsFaulted: true, Exception: not null })
            task.Exception.Handle(_ => true);
        return validator.Validate(new Model { Task = task });
    }
}
