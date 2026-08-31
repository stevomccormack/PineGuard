using PineGuard.MustClauses;

namespace PineGuard.MediatR.UnitTests;

/// <summary>
/// Asserts the one outcome both test classes share: the pipeline either answers with a response, or
/// throws a <see cref="MustValidationException"/> carrying the merged failures.
/// </summary>
public static class MustValidationAssert
{
    public static async Task ResponseAsync(MustValidationExpected expected, Func<Task<Guid>> send)
    {
        ArgumentNullException.ThrowIfNull(expected);
        ArgumentNullException.ThrowIfNull(send);

        if (expected.ExceptionType is null)
        {
            Assert.Equal(expected.Response, await send());
            return;
        }

        var ex = await Assert.ThrowsAsync(expected.ExceptionType, send);
        Assert.Equal(expected.FailurePaths ?? [], FailurePaths(ex));
    }

    private static IReadOnlyList<string> FailurePaths(Exception ex) =>
        [.. ((MustValidationException)ex).Result.Failures.Select(failure => failure.PropertyPath)];
}
