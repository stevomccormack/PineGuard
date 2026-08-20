using PineGuard.Testing.UnitTests.GuardClauses;
using Xunit.Abstractions;

namespace PineGuard.GuardClauses.UnitTests;

public sealed class GuardTaskClausesTests(ITestOutputHelper output) : BaseGuardUnitTest(output)
{
    [Theory]
    [MemberData(nameof(GuardTaskClausesTestData.Completed.ValidCases), MemberType = typeof(GuardTaskClausesTestData.Completed))]
    [MemberData(nameof(GuardTaskClausesTestData.Completed.InvalidCases), MemberType = typeof(GuardTaskClausesTestData.Completed))]
    public void Completed_BehavesAsExpected(GuardCase<Func<Task>> tc)
    {
        var task = tc.Value();
        var result = AssertResult(tc, () => Guard.Against.Completed(task));
        AssertCustomMessage(tc, () => Guard.Against.Completed(task, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(task, result);
    }

    [Theory]
    [MemberData(nameof(GuardTaskClausesTestData.NotCompleted.ValidCases), MemberType = typeof(GuardTaskClausesTestData.NotCompleted))]
    [MemberData(nameof(GuardTaskClausesTestData.NotCompleted.InvalidCases), MemberType = typeof(GuardTaskClausesTestData.NotCompleted))]
    public void NotCompleted_BehavesAsExpected(GuardCase<Func<Task>> tc)
    {
        var task = tc.Value();
        var result = AssertResult(tc, () => Guard.Against.NotCompleted(task));
        AssertCustomMessage(tc, () => Guard.Against.NotCompleted(task, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(task, result);
    }

    [Theory]
    [MemberData(nameof(GuardTaskClausesTestData.Canceled.ValidCases), MemberType = typeof(GuardTaskClausesTestData.Canceled))]
    [MemberData(nameof(GuardTaskClausesTestData.Canceled.InvalidCases), MemberType = typeof(GuardTaskClausesTestData.Canceled))]
    public void Canceled_BehavesAsExpected(GuardCase<Func<Task>> tc)
    {
        var task = tc.Value();
        var result = AssertResult(tc, () => Guard.Against.Canceled(task));
        AssertCustomMessage(tc, () => Guard.Against.Canceled(task, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(task, result);
    }

    [Theory]
    [MemberData(nameof(GuardTaskClausesTestData.NotCanceled.ValidCases), MemberType = typeof(GuardTaskClausesTestData.NotCanceled))]
    [MemberData(nameof(GuardTaskClausesTestData.NotCanceled.InvalidCases), MemberType = typeof(GuardTaskClausesTestData.NotCanceled))]
    public void NotCanceled_BehavesAsExpected(GuardCase<Func<Task>> tc)
    {
        var task = tc.Value();
        var result = AssertResult(tc, () => Guard.Against.NotCanceled(task));
        AssertCustomMessage(tc, () => Guard.Against.NotCanceled(task, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(task, result);
    }

    [Theory]
    [MemberData(nameof(GuardTaskClausesTestData.Faulted.ValidCases), MemberType = typeof(GuardTaskClausesTestData.Faulted))]
    [MemberData(nameof(GuardTaskClausesTestData.Faulted.InvalidCases), MemberType = typeof(GuardTaskClausesTestData.Faulted))]
    public void Faulted_BehavesAsExpected(GuardCase<Func<Task>> tc)
    {
        var task = tc.Value();
        var result = AssertResult(tc, () => Guard.Against.Faulted(task));
        AssertCustomMessage(tc, () => Guard.Against.Faulted(task, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(task, result);
    }

    [Theory]
    [MemberData(nameof(GuardTaskClausesTestData.NotFaulted.ValidCases), MemberType = typeof(GuardTaskClausesTestData.NotFaulted))]
    [MemberData(nameof(GuardTaskClausesTestData.NotFaulted.InvalidCases), MemberType = typeof(GuardTaskClausesTestData.NotFaulted))]
    public void NotFaulted_BehavesAsExpected(GuardCase<Func<Task>> tc)
    {
        var task = tc.Value();
        var result = AssertResult(tc, () => Guard.Against.NotFaulted(task));
        AssertCustomMessage(tc, () => Guard.Against.NotFaulted(task, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(task, result);
    }
}
