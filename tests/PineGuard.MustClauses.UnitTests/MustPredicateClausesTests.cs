using PineGuard.Testing.UnitTests.MustClauses;
using Xunit.Abstractions;

namespace PineGuard.MustClauses.UnitTests;

public sealed class MustPredicateClausesTests(ITestOutputHelper output) : BaseMustUnitTest(output)
{
    [Theory]
    [MemberData(nameof(MustPredicateClausesTestData.Satisfies.ValidCases), MemberType = typeof(MustPredicateClausesTestData.Satisfies))]
    [MemberData(nameof(MustPredicateClausesTestData.Satisfies.InvalidCases), MemberType = typeof(MustPredicateClausesTestData.Satisfies))]
    public void Satisfies_BehavesAsExpected(MustCase<(string? value, Func<string, bool>? predicate)> tc)
    {
        // Act
        var result = Must.Be.Satisfies(tc.Value.value, tc.Value.predicate!, paramName: "value");

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustPredicateClausesTestData.NotSatisfies.ValidCases), MemberType = typeof(MustPredicateClausesTestData.NotSatisfies))]
    [MemberData(nameof(MustPredicateClausesTestData.NotSatisfies.InvalidCases), MemberType = typeof(MustPredicateClausesTestData.NotSatisfies))]
    public void NotSatisfies_BehavesAsExpected(MustCase<(string? value, Func<string, bool>? predicate)> tc)
    {
        // Act
        var result = Must.Be.NotSatisfies(tc.Value.value, tc.Value.predicate!, paramName: "value");

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustPredicateClausesTestData.SatisfiesAsync.ValidCases), MemberType = typeof(MustPredicateClausesTestData.SatisfiesAsync))]
    [MemberData(nameof(MustPredicateClausesTestData.SatisfiesAsync.InvalidCases), MemberType = typeof(MustPredicateClausesTestData.SatisfiesAsync))]
    public async Task SatisfiesAsync_BehavesAsExpected(MustCase<(string? value, Func<string, CancellationToken, ValueTask<bool>>? predicate)> tc)
    {
        // Act
        var result = await Must.Be.SatisfiesAsync(tc.Value.value, tc.Value.predicate!, paramName: "value");

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustPredicateClausesTestData.NotSatisfiesAsync.ValidCases), MemberType = typeof(MustPredicateClausesTestData.NotSatisfiesAsync))]
    [MemberData(nameof(MustPredicateClausesTestData.NotSatisfiesAsync.InvalidCases), MemberType = typeof(MustPredicateClausesTestData.NotSatisfiesAsync))]
    public async Task NotSatisfiesAsync_BehavesAsExpected(MustCase<(string? value, Func<string, CancellationToken, ValueTask<bool>>? predicate)> tc)
    {
        // Act
        var result = await Must.Be.NotSatisfiesAsync(tc.Value.value, tc.Value.predicate!, paramName: "value");

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustPredicateClausesTestData.AsyncCancellation.Cases), MemberType = typeof(MustPredicateClausesTestData.AsyncCancellation))]
    public async Task SatisfiesAsync_PassesTheTokenToThePredicate(bool _)
    {
        // Arrange
        using var cancellation = new CancellationTokenSource();
        cancellation.Cancel();

        static ValueTask<bool> Observing(string value, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return new ValueTask<bool>(true);
        }

        // Act & Assert
        await Assert.ThrowsAsync<OperationCanceledException>(() => Must.Be.SatisfiesAsync("hello", Observing, cancellation.Token, paramName: "value").AsTask());
        await Assert.ThrowsAsync<OperationCanceledException>(() => Must.Be.NotSatisfiesAsync("hello", Observing, cancellation.Token, paramName: "value").AsTask());
    }
}
