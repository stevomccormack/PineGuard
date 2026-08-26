using PineGuard.MustClauses;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.MustClauses;

public static class MustValidationExceptionTestData
{
    private static readonly MustValidationResult FailureResult = MustValidationResult.Fail(new MustFailure("Email", "must.validation.a", "message a", "value-a"));
    private static readonly InvalidOperationException CustomInnerException = new("inner failure");

    public static class Constructor
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("one-arg uses the result message", static () => new MustValidationException(FailureResult), FailureResult, FailureResult.Message, null),
            new("two-arg uses a custom message", static () => new MustValidationException(FailureResult, "custom message"), FailureResult, "custom message", null),
            new("three-arg uses a custom message and inner exception", static () => new MustValidationException(FailureResult, "custom message", CustomInnerException), FailureResult, "custom message", CustomInnerException)
        ];

        public static TheoryData<IThrowsCase> InvalidCases =>
        [
            new InvalidCase("one-arg null result", static () => _ = new MustValidationException(null!), new ExpectedException(typeof(ArgumentNullException), "result")),
            new InvalidCase("two-arg null result", static () => _ = new MustValidationException(null!, "custom message"), new ExpectedException(typeof(ArgumentNullException), "result")),
            new InvalidCase("three-arg null result", static () => _ = new MustValidationException(null!, "custom message", CustomInnerException), new ExpectedException(typeof(ArgumentNullException), "result"))
        ];

        public sealed record ValidCase(string Name, Func<MustValidationException> Value, MustValidationResult ExpectedResult, string ExpectedMessage, Exception? ExpectedInnerException)
            : ValueCase<Func<MustValidationException>>(Name, Value);

        public sealed record InvalidCase(string Name, Action Value, ExpectedException ExpectedException)
            : ThrowsCase<Action>(Name, Value, ExpectedException);
    }
}
