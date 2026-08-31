using PineGuard.AspNetCore.UnitTests.Samples;
using PineGuard.MustClauses;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;

namespace PineGuard.AspNetCore.UnitTests;

public static class DefaultMustFailureMessageResolverTestData
{
    public static class Resolve
    {
        public static TheoryData<Case> Cases =>
        [
            new("rendered-message-is-published-unchanged", SampleFailures.Email, "Email must be a valid email address."),
            new("indexed-path-message-is-published-unchanged", SampleFailures.LineSku, "Lines[1].Sku must not be null or whitespace."),
            new("root-failure-message-is-published-unchanged", SampleFailures.Root, "The order is not consistent.")
        ];

        public static TheoryData<IThrowsCase> InvalidCases =>
        [
            new FailureThrowsCase("null-failure", null!, new ExpectedException(typeof(ArgumentNullException), "failure"))
        ];

        public sealed record Case(string Name, MustFailure Value, string Expected)
            : ReturnCase<MustFailure, string>(Name, Value, Expected);

        private sealed record FailureThrowsCase(string Name, MustFailure Value, ExpectedException ExpectedException)
            : ThrowsCase<MustFailure>(Name, Value, ExpectedException);
    }
}
