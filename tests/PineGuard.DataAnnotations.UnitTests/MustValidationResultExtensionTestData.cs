using PineGuard.MustClauses;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;

namespace PineGuard.DataAnnotations.UnitTests;

public static class MustValidationResultExtensionTestData
{
    public const string EmailPath = "Email";
    public const string NamePath = "Name";
    public const string EmailMessage = "Email must be a valid email address.";
    public const string NameMessage = "Name must not be null or whitespace.";
    public const string RootMessage = "The order must have at least one line.";
    public const string AttemptedValue = "a value that must never be rendered";

    private const string EmailCode = "email.address.invalid";
    private const string NameCode = "text.content.blank";
    private const string RootCode = "object.state.invalid";

    private static MustFailure EmailFailure => new(EmailPath, EmailCode, EmailMessage, AttemptedValue);
    private static MustFailure NameFailure => new(NamePath, NameCode, NameMessage, null);
    private static MustFailure RootFailure => new("", RootCode, RootMessage, null);

    public sealed record ValidationResultsExpected(bool IsValid, IReadOnlyList<(string errorMessage, string[] memberNames)> Results)
        : ReturnExpected(IsValid);

    public static class ToValidationResults
    {
        public static TheoryData<Case> Cases =>
        [
            new("a successful result produces no validation results", MustValidationResult.Ok(), new ValidationResultsExpected(true, [])),
            new("one failure produces one validation result named after its property path", MustValidationResult.Fail(EmailFailure), new ValidationResultsExpected(false, [(EmailMessage, [EmailPath])])),
            new("every failure is carried across in order", MustValidationResult.Fail(EmailFailure, NameFailure), new ValidationResultsExpected(false, [(EmailMessage, [EmailPath]), (NameMessage, [NamePath])])),
            new("a root failure produces a validation result with no member names", MustValidationResult.Fail(RootFailure), new ValidationResultsExpected(false, [(RootMessage, [])]))
        ];

        public static TheoryData<IThrowsCase> InvalidCases =>
        [
            new InvalidCase("null result", () => _ = ((MustValidationResult)null!).ToValidationResults(), new ExpectedException(typeof(ArgumentNullException), "result"))
        ];

        public sealed record Case(string Name, MustValidationResult Value, ValidationResultsExpected Expected)
            : ReturnCase<MustValidationResult, ValidationResultsExpected>(Name, Value, Expected);

        public sealed record InvalidCase(string Name, Action Value, ExpectedException ExpectedException)
            : ThrowsCase<Action>(Name, Value, ExpectedException);
    }

    public static class ToValidationResult
    {
        public static TheoryData<Case> Cases =>
        [
            new("a property failure names its property path", EmailFailure, new ValidationResultsExpected(false, [(EmailMessage, [EmailPath])])),
            new("a root failure names no member", RootFailure, new ValidationResultsExpected(false, [(RootMessage, [])]))
        ];

        public static TheoryData<IThrowsCase> InvalidCases =>
        [
            new InvalidCase("null failure", () => _ = ((MustFailure)null!).ToValidationResult(), new ExpectedException(typeof(ArgumentNullException), "failure"))
        ];

        public sealed record Case(string Name, MustFailure Value, ValidationResultsExpected Expected)
            : ReturnCase<MustFailure, ValidationResultsExpected>(Name, Value, Expected);

        public sealed record InvalidCase(string Name, Action Value, ExpectedException ExpectedException)
            : ThrowsCase<Action>(Name, Value, ExpectedException);
    }
}
