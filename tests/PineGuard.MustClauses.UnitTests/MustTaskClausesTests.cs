using PineGuard.Testing.UnitTests.MustClauses;
using Xunit.Abstractions;

namespace PineGuard.MustClauses.UnitTests;

public sealed class MustTaskClausesTests(ITestOutputHelper output) : BaseMustUnitTest(output)
{
    [Theory]
    [MemberData(nameof(MustTaskClausesTestData.Completed.ValidCases), MemberType = typeof(MustTaskClausesTestData.Completed))]
    [MemberData(nameof(MustTaskClausesTestData.Completed.InvalidCases), MemberType = typeof(MustTaskClausesTestData.Completed))]
    public void Completed_BehavesAsExpected(MustCase<Func<Task?>> tc)
    {
        var task = tc.Value();
        var result = Must.Be.Completed(task);
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustTaskClausesTestData.NotCompleted.ValidCases), MemberType = typeof(MustTaskClausesTestData.NotCompleted))]
    [MemberData(nameof(MustTaskClausesTestData.NotCompleted.InvalidCases), MemberType = typeof(MustTaskClausesTestData.NotCompleted))]
    public void NotCompleted_BehavesAsExpected(MustCase<Func<Task?>> tc)
    {
        var task = tc.Value();
        var result = Must.Be.NotCompleted(task);
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustTaskClausesTestData.Canceled.ValidCases), MemberType = typeof(MustTaskClausesTestData.Canceled))]
    [MemberData(nameof(MustTaskClausesTestData.Canceled.InvalidCases), MemberType = typeof(MustTaskClausesTestData.Canceled))]
    public void Canceled_BehavesAsExpected(MustCase<Func<Task?>> tc)
    {
        var task = tc.Value();
        var result = Must.Be.Canceled(task);
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustTaskClausesTestData.NotCanceled.ValidCases), MemberType = typeof(MustTaskClausesTestData.NotCanceled))]
    [MemberData(nameof(MustTaskClausesTestData.NotCanceled.InvalidCases), MemberType = typeof(MustTaskClausesTestData.NotCanceled))]
    public void NotCanceled_BehavesAsExpected(MustCase<Func<Task?>> tc)
    {
        var task = tc.Value();
        var result = Must.Be.NotCanceled(task);
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustTaskClausesTestData.Faulted.ValidCases), MemberType = typeof(MustTaskClausesTestData.Faulted))]
    [MemberData(nameof(MustTaskClausesTestData.Faulted.InvalidCases), MemberType = typeof(MustTaskClausesTestData.Faulted))]
    public void Faulted_BehavesAsExpected(MustCase<Func<Task?>> tc)
    {
        var task = tc.Value();
        var result = Must.Be.Faulted(task);
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustTaskClausesTestData.NotFaulted.ValidCases), MemberType = typeof(MustTaskClausesTestData.NotFaulted))]
    [MemberData(nameof(MustTaskClausesTestData.NotFaulted.InvalidCases), MemberType = typeof(MustTaskClausesTestData.NotFaulted))]
    public void NotFaulted_BehavesAsExpected(MustCase<Func<Task?>> tc)
    {
        var task = tc.Value();
        var result = Must.Be.NotFaulted(task);
        AssertResult(tc, result);
    }
}
