using FluentValidation;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;

namespace PineGuard.FluentValidation.UnitTests;

public static class FluentMustValidatorTestData
{
    public const string EmailCode = "email.address.invalid";
    public const string NameCode = "text.content.blank";

    public const string EmailMessage = "Email must be a valid email address.";
    public const string NameMessage = "Name must not be null or whitespace.";

    private const string ValidEmail = "ada@example.com";
    private const string InvalidEmail = "not-an-email";

    public sealed record Customer(string? Email, string? Name);

    public sealed class CustomerValidator : AbstractValidator<Customer>
    {
        public CustomerValidator()
        {
            RuleFor(x => x.Email).Must(email => email == ValidEmail).WithMessage(EmailMessage).WithErrorCode(EmailCode);
            RuleFor(x => x.Name).Must(name => !string.IsNullOrWhiteSpace(name)).WithMessage(NameMessage).WithErrorCode(NameCode);
        }
    }

    public sealed class AsyncCustomerValidator : AbstractValidator<Customer>
    {
        public AsyncCustomerValidator()
        {
            RuleFor(x => x.Email).MustAsync((email, _) => Task.FromResult(email == ValidEmail)).WithMessage(EmailMessage).WithErrorCode(EmailCode);
            RuleFor(x => x.Name).MustAsync((name, _) => Task.FromResult(!string.IsNullOrWhiteSpace(name))).WithMessage(NameMessage).WithErrorCode(NameCode);
        }
    }

    public static class Validate
    {
        public static TheoryData<ValidationBridgeCase<Customer>> Cases =>
        [
            new("a customer passing every rule succeeds", new Customer(ValidEmail, "Ada"), new ValidationBridgeExpected(true, [])),
            new("one broken rule becomes one failure", new Customer(InvalidEmail, "Ada"), new ValidationBridgeExpected(false, [("Email", EmailCode, EmailMessage)])),
            new("every broken rule is aggregated in registration order", new Customer(InvalidEmail, "   "), new ValidationBridgeExpected(false, [("Email", EmailCode, EmailMessage), ("Name", NameCode, NameMessage)]))
        ];
    }

    public static class ValidateAsync
    {
        public static TheoryData<ValidationBridgeCase<Customer>> Cases =>
        [
            new("a customer passing every async rule succeeds", new Customer(ValidEmail, "Ada"), new ValidationBridgeExpected(true, [])),
            new("one broken async rule becomes one failure", new Customer(InvalidEmail, "Ada"), new ValidationBridgeExpected(false, [("Email", EmailCode, EmailMessage)])),
            new("every broken async rule is aggregated in registration order", new Customer(InvalidEmail, "   "), new ValidationBridgeExpected(false, [("Email", EmailCode, EmailMessage), ("Name", NameCode, NameMessage)]))
        ];
    }

    public static class Validator
    {
        public static TheoryData<bool> Cases => [true];
    }

    public static class Constructor
    {
        public static TheoryData<IThrowsCase> InvalidCases =>
        [
            new ActionThrowsCase("null validator", () => _ = new FluentMustValidator<Customer>(null!), new ExpectedException(typeof(ArgumentNullException), "validator"))
        ];

        private sealed record ActionThrowsCase(string Name, Action Value, ExpectedException ExpectedException)
            : ThrowsCase<Action>(Name, Value, ExpectedException);
    }
}
