using System.ComponentModel.DataAnnotations;
using PineGuard.Codes;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;

namespace PineGuard.DataAnnotations.UnitTests;

internal sealed class ValidatorModel
{
    [Required]
    public string RequiredName { get; set; } = "ok";

    [Email]
    public string EmailAddress { get; set; } = "ok@example.com";
}

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Class)]
internal sealed class AlwaysFailsAttribute : ValidationAttribute
{
    public override bool IsValid(object? value) => false;

    public override string FormatErrorMessage(string name) => $"{name} always fails.";
}

internal sealed class MultiFailurePropertyModel
{
    [AlwaysFails]
    [Email]
    public string EmailAddress { get; set; } = "not-an-email";
}

[AlwaysFails]
internal sealed class RootFailureModel
{
    public string Name { get; set; } = "value";
}

internal sealed class IndexerModel
{
    [Email]
    public string EmailAddress { get; set; } = "ok@example.com";

    public string this[int index] => index.ToString();
}

internal sealed class WriteOnlyModel
{
    private string _secret = "";

    [Email]
    public string EmailAddress { get; set; } = "ok@example.com";

    public string Secret
    {
        set => _secret = value;
    }
}

public static class DataAnnotationsAttributeValidatorTestData
{
    public static class Validate
    {
        public static TheoryData<Case> Cases =>
        [
            new("every attribute passes", new ValidatorModel(), new ValidateExpected(true, [])),
            new("a required property left empty fails with no code", new ValidatorModel { RequiredName = "" }, new ValidateExpected(false, [new("RequiredName", "", null)])),
            new("an invalid email fails with its Must code", new ValidatorModel { EmailAddress = "not-an-email" }, new ValidateExpected(false, [new("EmailAddress", MustCodes.Email.Address.Invalid, "EmailAddress must be a valid email address.")])),
            new("both attributes on one property report independently", new MultiFailurePropertyModel(), new ValidateExpected(false, [new("EmailAddress", "", "EmailAddress always fails."), new("EmailAddress", MustCodes.Email.Address.Invalid, "EmailAddress must be a valid email address.")])),
            new("a class-level attribute failure has an empty property path", new RootFailureModel(), new ValidateExpected(false, [new("", "", "RootFailureModel always fails.")])),
            new("an indexer property is skipped rather than crashing", new IndexerModel(), new ValidateExpected(true, [])),
            new("a write-only property is skipped rather than crashing", new WriteOnlyModel(), new ValidateExpected(true, []))
        ];

        public static TheoryData<IThrowsCase> InvalidCases =>
        [
            new InvalidCase("null instance", () => _ = DataAnnotationsAttributeValidator.Validate(null!), new ExpectedException(typeof(ArgumentNullException), "instance"))
        ];

        public sealed record FailureExpected(string PropertyPath, string Code, string? Message);

        public sealed record ValidateExpected(bool IsValid, IReadOnlyList<FailureExpected> Failures) : ReturnExpected(IsValid);

        public sealed record Case(string Name, object Value, ValidateExpected Expected)
            : ReturnCase<object, ValidateExpected>(Name, Value, Expected);

        public sealed record InvalidCase(string Name, Action Value, ExpectedException ExpectedException)
            : ThrowsCase<Action>(Name, Value, ExpectedException);
    }
}
