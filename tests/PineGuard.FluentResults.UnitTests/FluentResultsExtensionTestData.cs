using PineGuard.MustClauses;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;

namespace PineGuard.FluentResults.UnitTests;

public static class FluentResultsExtensionTestData
{
    private const string EmailCode = "email.address.invalid";
    private const string PortCode = "network.port.invalid";
    private const string NullCode = "value.state.null";

    private const string EmailTemplate = "{paramName} must be a valid email address.";
    private const string EmailMessage = "email must be a valid email address.";
    private const string PropertyPathMessage = "Order.Email must be a valid email address.";
    private const string PortMessage = "Order.Port must be a valid port number.";
    private const string RootMessage = "The order must not be null.";

    private static readonly MustFailure EmailFailure = new("Order.Email", EmailCode, PropertyPathMessage, "not-an-email");
    private static readonly MustFailure PortFailure = new("Order.Port", PortCode, PortMessage, 70000);

    public static class ToResult
    {
        public static TheoryData<FluentResultsCase<MustResult<string>>> Cases =>
        [
            new("success-carries-the-typed-result", MustResult<string>.Ok("user@example.com", "user@example.com", "email"), new FluentResultsExpected(true, "user@example.com")),
            new("success-with-a-null-result-carries-default", MustResult<string>.Ok(null, null, "email"), new FluentResultsExpected(true)),
            new("failure-carries-code-message-and-property-path", MustResult<string>.Fail(EmailCode, EmailTemplate, "email", "not-an-email"), new FluentResultsExpected(false, null, [(EmailCode, EmailMessage, "email")])),
            new("failure-without-a-param-name-lands-at-the-root", MustResult<string>.Fail(NullCode, RootMessage, null, null), new FluentResultsExpected(false, null, [(NullCode, RootMessage, "")]))
        ];

        public static TheoryData<IThrowsCase> InvalidCases =>
        [
            new ActionThrowsCase("null-result", () => FluentResultsExtension.ToResult((MustResult<string>)null!), new ExpectedException(typeof(ArgumentNullException), "result"))
        ];

        private sealed record ActionThrowsCase(string Name, Action Value, ExpectedException ExpectedException)
            : ThrowsCase<Action>(Name, Value, ExpectedException);
    }

    public static class ToResultFromValidationResult
    {
        public static TheoryData<FluentResultsCase<MustValidationResult>> Cases =>
        [
            new("success-is-an-ok-result", MustValidationResult.Ok(), new FluentResultsExpected(true, null, [])),
            new("one-failure-produces-one-must-error", MustValidationResult.Fail(EmailFailure), new FluentResultsExpected(false, null, [(EmailCode, PropertyPathMessage, "Order.Email")])),
            new("many-failures-keep-their-order", MustValidationResult.Fail(EmailFailure, PortFailure), new FluentResultsExpected(false, null, [(EmailCode, PropertyPathMessage, "Order.Email"), (PortCode, PortMessage, "Order.Port")]))
        ];

        public static TheoryData<IThrowsCase> InvalidCases =>
        [
            new ActionThrowsCase("null-result", () => FluentResultsExtension.ToResult(null!), new ExpectedException(typeof(ArgumentNullException), "result"))
        ];

        private sealed record ActionThrowsCase(string Name, Action Value, ExpectedException ExpectedException)
            : ThrowsCase<Action>(Name, Value, ExpectedException);
    }

    public static class ToResultFromValidationResultWithValue
    {
        public static TheoryData<FluentResultsCase<(MustValidationResult result, string value)>> Cases =>
        [
            new("success-carries-the-value", (MustValidationResult.Ok(), "order-1"), new FluentResultsExpected(true, "order-1")),
            new("failure-carries-every-must-error", (MustValidationResult.Fail(EmailFailure, PortFailure), "order-1"), new FluentResultsExpected(false, null, [(EmailCode, PropertyPathMessage, "Order.Email"), (PortCode, PortMessage, "Order.Port")]))
        ];

        public static TheoryData<IThrowsCase> InvalidCases =>
        [
            new ActionThrowsCase("null-result", () => FluentResultsExtension.ToResult(null!, "order-1"), new ExpectedException(typeof(ArgumentNullException), "result"))
        ];

        private sealed record ActionThrowsCase(string Name, Action Value, ExpectedException ExpectedException)
            : ThrowsCase<Action>(Name, Value, ExpectedException);
    }
}
