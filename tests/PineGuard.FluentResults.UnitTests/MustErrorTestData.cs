using PineGuard.MustClauses;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;

namespace PineGuard.FluentResults.UnitTests;

public static class MustErrorTestData
{
    private const string EmailCode = "email.address.invalid";
    private const string NullCode = "value.state.null";

    private const string EmailTemplate = "{paramName} must be a valid email address.";
    private const string EmailMessage = "email must be a valid email address.";
    private const string PropertyPathMessage = "Order.Email must be a valid email address.";
    private const string RootMessage = "The order must not be null.";

    private static readonly MustFailure EmailFailure = new("Order.Email", EmailCode, PropertyPathMessage, "not-an-email");
    private static readonly MustFailure RootFailure = new("", NullCode, RootMessage, null);

    public static class Constructor
    {
        public static TheoryData<FluentResultsCase<(string code, string propertyPath, string message)>> Cases =>
        [
            new("carries-code-property-path-and-message", (EmailCode, "Order.Email", PropertyPathMessage), new FluentResultsExpected(false, null, [(EmailCode, PropertyPathMessage, "Order.Email")])),
            new("an-empty-property-path-is-the-root", (NullCode, "", RootMessage), new FluentResultsExpected(false, null, [(NullCode, RootMessage, "")]))
        ];

        public static TheoryData<IThrowsCase> InvalidCases =>
        [
            new ActionThrowsCase("null-code", () => _ = new MustError(null!, "Order.Email", PropertyPathMessage), new ExpectedException(typeof(ArgumentNullException), "code")),
            new ActionThrowsCase("null-property-path", () => _ = new MustError(EmailCode, null!, PropertyPathMessage), new ExpectedException(typeof(ArgumentNullException), "propertyPath")),
            new ActionThrowsCase("null-message", () => _ = new MustError(EmailCode, "Order.Email", null!), new ExpectedException(typeof(ArgumentNullException), "message"))
        ];

        private sealed record ActionThrowsCase(string Name, Action Value, ExpectedException ExpectedException)
            : ThrowsCase<Action>(Name, Value, ExpectedException);
    }

    public static class FromResult
    {
        public static TheoryData<FluentResultsCase<IMustResult>> Cases =>
        [
            new("failure-carries-code-message-and-param-name", MustResult<string>.Fail(EmailCode, EmailTemplate, "email", "not-an-email"), new FluentResultsExpected(false, null, [(EmailCode, EmailMessage, "email")])),
            new("failure-without-a-param-name-lands-at-the-root", MustResult<string>.Fail(NullCode, RootMessage, null, null), new FluentResultsExpected(false, null, [(NullCode, RootMessage, "")]))
        ];

        public static TheoryData<IThrowsCase> InvalidCases =>
        [
            new ActionThrowsCase("null-result", () => MustError.From((IMustResult)null!), new ExpectedException(typeof(ArgumentNullException), "result")),
            new ActionThrowsCase("successful-result", () => MustError.From(MustResult<string>.Ok("user@example.com")), new ExpectedException(typeof(ArgumentException), "result", "must represent a failure"))
        ];

        private sealed record ActionThrowsCase(string Name, Action Value, ExpectedException ExpectedException)
            : ThrowsCase<Action>(Name, Value, ExpectedException);
    }

    public static class FromFailure
    {
        public static TheoryData<FluentResultsCase<MustFailure>> Cases =>
        [
            new("failure-with-a-property-path", EmailFailure, new FluentResultsExpected(false, null, [(EmailCode, PropertyPathMessage, "Order.Email")])),
            new("failure-at-the-root", RootFailure, new FluentResultsExpected(false, null, [(NullCode, RootMessage, "")]))
        ];

        public static TheoryData<IThrowsCase> InvalidCases =>
        [
            new ActionThrowsCase("null-failure", () => MustError.From((MustFailure)null!), new ExpectedException(typeof(ArgumentNullException), "failure"))
        ];

        private sealed record ActionThrowsCase(string Name, Action Value, ExpectedException ExpectedException)
            : ThrowsCase<Action>(Name, Value, ExpectedException);
    }
}
