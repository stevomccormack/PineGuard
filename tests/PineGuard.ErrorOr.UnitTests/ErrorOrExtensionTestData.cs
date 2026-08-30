using PineGuard.MustClauses;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;

namespace PineGuard.ErrorOr.UnitTests;

public static class ErrorOrExtensionTestData
{
    private const string EmailCode = "email.address.invalid";
    private const string PortCode = "network.port.invalid";
    private const string NullCode = "value.state.null";

    private const string EmailTemplate = "{paramName} must be a valid email address.";
    private const string EmailMessage = "email must be a valid email address.";
    private const string RootMessage = "The order must not be null.";
    private const string PortMessage = "Port must be a valid port number.";

    private static readonly MustFailure EmailFailure = new("Order.Email", EmailCode, EmailMessage, "not-an-email");
    private static readonly MustFailure PortFailure = new("Order.Port", PortCode, PortMessage, 70000);
    private static readonly MustFailure RootFailure = new("", NullCode, RootMessage, null);

    public static class ToErrorOr
    {
        public static TheoryData<ErrorOrCase<MustResult<string>>> Cases =>
        [
            new("success-carries-the-typed-result", MustResult<string>.Ok("user@example.com", "user@example.com", "email"), new ErrorOrExpected(true, "user@example.com")),
            new("success-with-a-null-result-carries-default", MustResult<string>.Ok(null, null, "email"), new ErrorOrExpected(true)),
            new("failure-carries-code-description-and-property-path", MustResult<string>.Fail(EmailCode, EmailTemplate, "email", "not-an-email"), new ErrorOrExpected(false, null, [(EmailCode, EmailMessage, "email")])),
            new("failure-without-a-param-name-lands-at-the-root", MustResult<string>.Fail(NullCode, RootMessage, null, null), new ErrorOrExpected(false, null, [(NullCode, RootMessage, "")]))
        ];

        public static TheoryData<IThrowsCase> InvalidCases =>
        [
            new ActionThrowsCase("null-result", () => ErrorOrExtension.ToErrorOr((MustResult<string>)null!), new ExpectedException(typeof(ArgumentNullException), "result"))
        ];

        private sealed record ActionThrowsCase(string Name, Action Value, ExpectedException ExpectedException)
            : ThrowsCase<Action>(Name, Value, ExpectedException);
    }

    public static class ToError
    {
        public static TheoryData<ErrorOrCase<MustFailure>> Cases =>
        [
            new("failure-with-a-property-path", EmailFailure, new ErrorOrExpected(false, null, [(EmailCode, EmailMessage, "Order.Email")])),
            new("failure-at-the-root", RootFailure, new ErrorOrExpected(false, null, [(NullCode, RootMessage, "")]))
        ];

        public static TheoryData<IThrowsCase> InvalidCases =>
        [
            new ActionThrowsCase("null-failure", () => ErrorOrExtension.ToError(null!), new ExpectedException(typeof(ArgumentNullException), "failure"))
        ];

        private sealed record ActionThrowsCase(string Name, Action Value, ExpectedException ExpectedException)
            : ThrowsCase<Action>(Name, Value, ExpectedException);
    }

    public static class ToErrors
    {
        public static TheoryData<ErrorOrCase<MustValidationResult>> Cases =>
        [
            new("success-produces-an-empty-list", MustValidationResult.Ok(), new ErrorOrExpected(true, null, [])),
            new("one-failure-produces-one-error", MustValidationResult.Fail(EmailFailure), new ErrorOrExpected(false, null, [(EmailCode, EmailMessage, "Order.Email")])),
            new("many-failures-keep-their-order", MustValidationResult.Fail(EmailFailure, PortFailure), new ErrorOrExpected(false, null, [(EmailCode, EmailMessage, "Order.Email"), (PortCode, PortMessage, "Order.Port")]))
        ];

        public static TheoryData<IThrowsCase> InvalidCases =>
        [
            new ActionThrowsCase("null-result", () => ErrorOrExtension.ToErrors(null!), new ExpectedException(typeof(ArgumentNullException), "result"))
        ];

        private sealed record ActionThrowsCase(string Name, Action Value, ExpectedException ExpectedException)
            : ThrowsCase<Action>(Name, Value, ExpectedException);
    }

    public static class ToErrorOrWithValue
    {
        public static TheoryData<ErrorOrCase<(MustValidationResult result, string value)>> Cases =>
        [
            new("success-carries-the-value", (MustValidationResult.Ok(), "order-1"), new ErrorOrExpected(true, "order-1")),
            new("failure-carries-every-error", (MustValidationResult.Fail(EmailFailure, PortFailure), "order-1"), new ErrorOrExpected(false, null, [(EmailCode, EmailMessage, "Order.Email"), (PortCode, PortMessage, "Order.Port")]))
        ];

        public static TheoryData<IThrowsCase> InvalidCases =>
        [
            new ActionThrowsCase("null-result", () => ErrorOrExtension.ToErrorOr(null!, "order-1"), new ExpectedException(typeof(ArgumentNullException), "result"))
        ];

        private sealed record ActionThrowsCase(string Name, Action Value, ExpectedException ExpectedException)
            : ThrowsCase<Action>(Name, Value, ExpectedException);
    }
}
