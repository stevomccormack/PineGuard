using FluentValidation.Results;
using PineGuard.MustClauses;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;

namespace PineGuard.FluentValidation.UnitTests;

public static class ValidationResultExtensionTestData
{
    private const string EmailCode = "email.address.invalid";
    private const string SkuCode = "text.content.blank";
    private const string NullCode = "value.state.null";

    private const string EmailMessage = "Email must be a valid email address.";
    private const string SkuMessage = "Lines[1].Sku must not be null or whitespace.";
    private const string RootMessage = "The order must not be null.";

    private const string EmailPath = "Email";
    private const string SkuPath = "Lines[1].Sku";

    private const string AttemptedEmail = "not-an-email";
    private const string Secret = "hunter2";

    private static ValidationFailure EmailError => new(EmailPath, EmailMessage, AttemptedEmail) { ErrorCode = EmailCode };
    private static ValidationFailure SkuError => new(SkuPath, SkuMessage, "   ") { ErrorCode = SkuCode };
    private static ValidationFailure UnpopulatedError => new(null!, null!);

    private static readonly MustFailure EmailFailure = new(EmailPath, EmailCode, EmailMessage, AttemptedEmail);
    private static readonly MustFailure SkuFailure = new(SkuPath, SkuCode, SkuMessage, "   ");
    private static readonly MustFailure RootFailure = new("", NullCode, RootMessage, Secret);

    public static class ToMustValidationResult
    {
        public static TheoryData<ValidationBridgeCase<ValidationResult>> Cases =>
        [
            new("a valid result crosses as a successful one", new ValidationResult(), new ValidationBridgeExpected(true, [])),
            new("one error becomes one failure carrying path, code and message", new ValidationResult([EmailError]), new ValidationBridgeExpected(false, [(EmailPath, EmailCode, EmailMessage)])),
            new("many errors keep their order", new ValidationResult([EmailError, SkuError]), new ValidationBridgeExpected(false, [(EmailPath, EmailCode, EmailMessage), (SkuPath, SkuCode, SkuMessage)]))
        ];

        public static TheoryData<IThrowsCase> InvalidCases =>
        [
            new ActionThrowsCase("null result", () => ValidationResultExtension.ToMustValidationResult(null!), new ExpectedException(typeof(ArgumentNullException), "result"))
        ];

        private sealed record ActionThrowsCase(string Name, Action Value, ExpectedException ExpectedException)
            : ThrowsCase<Action>(Name, Value, ExpectedException);
    }

    public static class ToMustFailure
    {
        public static TheoryData<ValidationBridgeCase<ValidationFailure>> Cases =>
        [
            new("every slot travels, attempted value included", EmailError, new ValidationBridgeExpected(false, [(EmailPath, EmailCode, EmailMessage)], AttemptedEmail)),
            new("unpopulated slots fall back to empty strings", UnpopulatedError, new ValidationBridgeExpected(false, [("", "", "")]))
        ];

        public static TheoryData<IThrowsCase> InvalidCases =>
        [
            new ActionThrowsCase("null failure", () => ValidationResultExtension.ToMustFailure(null!), new ExpectedException(typeof(ArgumentNullException), "failure"))
        ];

        private sealed record ActionThrowsCase(string Name, Action Value, ExpectedException ExpectedException)
            : ThrowsCase<Action>(Name, Value, ExpectedException);
    }

    public static class ToValidationResult
    {
        public static TheoryData<ValidationBridgeCase<MustValidationResult>> Cases =>
        [
            new("a successful result crosses as a valid one", MustValidationResult.Ok(), new ValidationBridgeExpected(true, [])),
            new("one failure becomes one error carrying name, code and message", MustValidationResult.Fail(EmailFailure), new ValidationBridgeExpected(false, [(EmailPath, EmailCode, EmailMessage)])),
            new("many failures keep their order", MustValidationResult.Fail(EmailFailure, SkuFailure), new ValidationBridgeExpected(false, [(EmailPath, EmailCode, EmailMessage), (SkuPath, SkuCode, SkuMessage)]))
        ];

        public static TheoryData<IThrowsCase> InvalidCases =>
        [
            new ActionThrowsCase("null result", () => ValidationResultExtension.ToValidationResult(null!), new ExpectedException(typeof(ArgumentNullException), "result"))
        ];

        private sealed record ActionThrowsCase(string Name, Action Value, ExpectedException ExpectedException)
            : ThrowsCase<Action>(Name, Value, ExpectedException);
    }

    public static class ToValidationFailure
    {
        public static TheoryData<ValidationBridgeCase<MustFailure>> Cases =>
        [
            new("a property failure keeps its path", EmailFailure, new ValidationBridgeExpected(false, [(EmailPath, EmailCode, EmailMessage)])),
            new("a root failure keeps its empty path and leaves the secret behind", RootFailure, new ValidationBridgeExpected(false, [("", NullCode, RootMessage)]))
        ];

        public static TheoryData<IThrowsCase> InvalidCases =>
        [
            new ActionThrowsCase("null failure", () => ValidationResultExtension.ToValidationFailure(null!), new ExpectedException(typeof(ArgumentNullException), "failure"))
        ];

        private sealed record ActionThrowsCase(string Name, Action Value, ExpectedException ExpectedException)
            : ThrowsCase<Action>(Name, Value, ExpectedException);
    }
}
