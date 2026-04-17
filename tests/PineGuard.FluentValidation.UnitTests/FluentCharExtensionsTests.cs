using FluentValidation;
using PineGuard.Testing.UnitTests.FluentValidation;
using Xunit.Abstractions;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace PineGuard.FluentValidation.UnitTests;

public sealed class FluentCharExtensionsTests(ITestOutputHelper output) : BaseFluentUnitTest(output)
{
    private sealed record Model { public char Value { get; init; } }

    private sealed class LetterValidator : AbstractValidator<Model> { public LetterValidator() => RuleFor(x => x.Value).Letter(); }
    private sealed class NotLetterValidator : AbstractValidator<Model> { public NotLetterValidator() => RuleFor(x => x.Value).NotLetter(); }
    private sealed class DigitValidator : AbstractValidator<Model> { public DigitValidator() => RuleFor(x => x.Value).Digit(); }
    private sealed class NotDigitValidator : AbstractValidator<Model> { public NotDigitValidator() => RuleFor(x => x.Value).NotDigit(); }
    private sealed class LetterOrDigitValidator : AbstractValidator<Model> { public LetterOrDigitValidator() => RuleFor(x => x.Value).LetterOrDigit(); }
    private sealed class NotLetterOrDigitValidator : AbstractValidator<Model> { public NotLetterOrDigitValidator() => RuleFor(x => x.Value).NotLetterOrDigit(); }
    private sealed class AsciiValidator : AbstractValidator<Model> { public AsciiValidator() => RuleFor(x => x.Value).Ascii(); }
    private sealed class NotAsciiValidator : AbstractValidator<Model> { public NotAsciiValidator() => RuleFor(x => x.Value).NotAscii(); }
    private sealed class PrintableAsciiValidator : AbstractValidator<Model> { public PrintableAsciiValidator() => RuleFor(x => x.Value).PrintableAscii(); }
    private sealed class NotPrintableAsciiValidator : AbstractValidator<Model> { public NotPrintableAsciiValidator() => RuleFor(x => x.Value).NotPrintableAscii(); }
    private sealed class NotWhitespaceValidator : AbstractValidator<Model> { public NotWhitespaceValidator() => RuleFor(x => x.Value).NotWhitespace(); }
    private sealed class ControlValidator : AbstractValidator<Model> { public ControlValidator() => RuleFor(x => x.Value).Control(); }
    private sealed class NotControlValidator : AbstractValidator<Model> { public NotControlValidator() => RuleFor(x => x.Value).NotControl(); }
    private sealed class UppercaseValidator : AbstractValidator<Model> { public UppercaseValidator() => RuleFor(x => x.Value).Uppercase(); }
    private sealed class LowercaseValidator : AbstractValidator<Model> { public LowercaseValidator() => RuleFor(x => x.Value).Lowercase(); }
    private sealed class HexDigitValidator : AbstractValidator<Model> { public HexDigitValidator() => RuleFor(x => x.Value).HexDigit(); }
    private sealed class NotHexDigitValidator : AbstractValidator<Model> { public NotHexDigitValidator() => RuleFor(x => x.Value).NotHexDigit(); }

    private static ValidationResult ValidateChar(FluentCase<char?> tc, AbstractValidator<Model> validator) =>
        tc.Value.HasValue ? validator.Validate(new Model { Value = tc.Value.Value }) : new ValidationResult();

    [Theory]
    [MemberData(nameof(FluentCharExtensionsTestData.Letter.Cases), MemberType = typeof(FluentCharExtensionsTestData.Letter))]
    public void Letter_BehavesAsExpected(FluentCase<char?> tc) => AssertResult(tc, ValidateChar(tc, new LetterValidator()));

    [Theory]
    [MemberData(nameof(FluentCharExtensionsTestData.NotLetter.Cases), MemberType = typeof(FluentCharExtensionsTestData.NotLetter))]
    public void NotLetter_BehavesAsExpected(FluentCase<char?> tc) => AssertResult(tc, ValidateChar(tc, new NotLetterValidator()));

    [Theory]
    [MemberData(nameof(FluentCharExtensionsTestData.Digit.Cases), MemberType = typeof(FluentCharExtensionsTestData.Digit))]
    public void Digit_BehavesAsExpected(FluentCase<char?> tc) => AssertResult(tc, ValidateChar(tc, new DigitValidator()));

    [Theory]
    [MemberData(nameof(FluentCharExtensionsTestData.NotDigit.Cases), MemberType = typeof(FluentCharExtensionsTestData.NotDigit))]
    public void NotDigit_BehavesAsExpected(FluentCase<char?> tc) => AssertResult(tc, ValidateChar(tc, new NotDigitValidator()));

    [Theory]
    [MemberData(nameof(FluentCharExtensionsTestData.LetterOrDigit.Cases), MemberType = typeof(FluentCharExtensionsTestData.LetterOrDigit))]
    public void LetterOrDigit_BehavesAsExpected(FluentCase<char?> tc) => AssertResult(tc, ValidateChar(tc, new LetterOrDigitValidator()));

    [Theory]
    [MemberData(nameof(FluentCharExtensionsTestData.NotLetterOrDigit.Cases), MemberType = typeof(FluentCharExtensionsTestData.NotLetterOrDigit))]
    public void NotLetterOrDigit_BehavesAsExpected(FluentCase<char?> tc) => AssertResult(tc, ValidateChar(tc, new NotLetterOrDigitValidator()));

    [Theory]
    [MemberData(nameof(FluentCharExtensionsTestData.Ascii.Cases), MemberType = typeof(FluentCharExtensionsTestData.Ascii))]
    public void Ascii_BehavesAsExpected(FluentCase<char?> tc) => AssertResult(tc, ValidateChar(tc, new AsciiValidator()));

    [Theory]
    [MemberData(nameof(FluentCharExtensionsTestData.NotAscii.Cases), MemberType = typeof(FluentCharExtensionsTestData.NotAscii))]
    public void NotAscii_BehavesAsExpected(FluentCase<char?> tc) => AssertResult(tc, ValidateChar(tc, new NotAsciiValidator()));

    [Theory]
    [MemberData(nameof(FluentCharExtensionsTestData.PrintableAscii.Cases), MemberType = typeof(FluentCharExtensionsTestData.PrintableAscii))]
    public void PrintableAscii_BehavesAsExpected(FluentCase<char?> tc) => AssertResult(tc, ValidateChar(tc, new PrintableAsciiValidator()));

    [Theory]
    [MemberData(nameof(FluentCharExtensionsTestData.NotPrintableAscii.Cases), MemberType = typeof(FluentCharExtensionsTestData.NotPrintableAscii))]
    public void NotPrintableAscii_BehavesAsExpected(FluentCase<char?> tc) => AssertResult(tc, ValidateChar(tc, new NotPrintableAsciiValidator()));

    [Theory]
    [MemberData(nameof(FluentCharExtensionsTestData.NotWhitespace.Cases), MemberType = typeof(FluentCharExtensionsTestData.NotWhitespace))]
    public void NotWhitespace_BehavesAsExpected(FluentCase<char?> tc) => AssertResult(tc, ValidateChar(tc, new NotWhitespaceValidator()));

    [Theory]
    [MemberData(nameof(FluentCharExtensionsTestData.Control.Cases), MemberType = typeof(FluentCharExtensionsTestData.Control))]
    public void Control_BehavesAsExpected(FluentCase<char?> tc) => AssertResult(tc, ValidateChar(tc, new ControlValidator()));

    [Theory]
    [MemberData(nameof(FluentCharExtensionsTestData.NotControl.Cases), MemberType = typeof(FluentCharExtensionsTestData.NotControl))]
    public void NotControl_BehavesAsExpected(FluentCase<char?> tc) => AssertResult(tc, ValidateChar(tc, new NotControlValidator()));

    [Theory]
    [MemberData(nameof(FluentCharExtensionsTestData.Uppercase.Cases), MemberType = typeof(FluentCharExtensionsTestData.Uppercase))]
    public void Uppercase_BehavesAsExpected(FluentCase<char?> tc) => AssertResult(tc, ValidateChar(tc, new UppercaseValidator()));

    [Theory]
    [MemberData(nameof(FluentCharExtensionsTestData.Lowercase.Cases), MemberType = typeof(FluentCharExtensionsTestData.Lowercase))]
    public void Lowercase_BehavesAsExpected(FluentCase<char?> tc) => AssertResult(tc, ValidateChar(tc, new LowercaseValidator()));

    [Theory]
    [MemberData(nameof(FluentCharExtensionsTestData.HexDigit.Cases), MemberType = typeof(FluentCharExtensionsTestData.HexDigit))]
    public void HexDigit_BehavesAsExpected(FluentCase<char?> tc) => AssertResult(tc, ValidateChar(tc, new HexDigitValidator()));

    [Theory]
    [MemberData(nameof(FluentCharExtensionsTestData.NotHexDigit.Cases), MemberType = typeof(FluentCharExtensionsTestData.NotHexDigit))]
    public void NotHexDigit_BehavesAsExpected(FluentCase<char?> tc) => AssertResult(tc, ValidateChar(tc, new NotHexDigitValidator()));
}
