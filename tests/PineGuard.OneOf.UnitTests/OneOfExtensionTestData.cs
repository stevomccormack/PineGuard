using PineGuard.MustClauses;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;

namespace PineGuard.OneOf.UnitTests;

public static class OneOfExtensionTestData
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

    public static class ToOneOf
    {
        public static TheoryData<OneOfCase<MustResult<string>>> Cases =>
        [
            new("success-carries-the-typed-result", MustResult<string>.Ok("user@example.com", "user@example.com", "email"), new OneOfExpected(true, "user@example.com")),
            new("success-with-a-null-result-carries-default", MustResult<string>.Ok(null, null, "email"), new OneOfExpected(true)),
            new("failure-carries-code-message-and-property-path", MustResult<string>.Fail(EmailCode, EmailTemplate, "email", "not-an-email"), new OneOfExpected(false, null, [(EmailCode, EmailMessage, "email")])),
            new("failure-without-a-param-name-lands-at-the-root", MustResult<string>.Fail(NullCode, RootMessage, null, null), new OneOfExpected(false, null, [(NullCode, RootMessage, "")]))
        ];

        public static TheoryData<IThrowsCase> InvalidCases =>
        [
            new ActionThrowsCase("null-result", () => OneOfExtension.ToOneOf((MustResult<string>)null!), new ExpectedException(typeof(ArgumentNullException), "result"))
        ];

        private sealed record ActionThrowsCase(string Name, Action Value, ExpectedException ExpectedException)
            : ThrowsCase<Action>(Name, Value, ExpectedException);
    }

    public static class ToOneOfWithValue
    {
        public static TheoryData<OneOfCase<(MustValidationResult result, string value)>> Cases =>
        [
            new("success-carries-the-value", (MustValidationResult.Ok(), "order-1"), new OneOfExpected(true, "order-1")),
            new("one-failure-crosses-whole", (MustValidationResult.Fail(EmailFailure), "order-1"), new OneOfExpected(false, null, [(EmailCode, PropertyPathMessage, "Order.Email")])),
            new("many-failures-keep-their-order", (MustValidationResult.Fail(EmailFailure, PortFailure), "order-1"), new OneOfExpected(false, null, [(EmailCode, PropertyPathMessage, "Order.Email"), (PortCode, PortMessage, "Order.Port")]))
        ];

        public static TheoryData<IThrowsCase> InvalidCases =>
        [
            new ActionThrowsCase("null-result", () => OneOfExtension.ToOneOf(null!, "order-1"), new ExpectedException(typeof(ArgumentNullException), "result"))
        ];

        private sealed record ActionThrowsCase(string Name, Action Value, ExpectedException ExpectedException)
            : ThrowsCase<Action>(Name, Value, ExpectedException);
    }
}
