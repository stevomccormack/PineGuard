using PineGuard.Codes;
using PineGuard.Testing.UnitTests.FluentValidation;
using F = PineGuard.Testing.Fixtures.CharRulesFixtures;

namespace PineGuard.FluentValidation.UnitTests;

public static class FluentCharExtensionsTestData
{
    public static class Letter
    {
        public static TheoryData<FluentCase<char?>> Cases => F.IsLetter.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsLetter.Null) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be a letter.", Code: MustCodes.Character.Charset.NotLetter)
        });
    }

    public static class NotLetter
    {
        public static TheoryData<FluentCase<char?>> Cases => F.IsLetter.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsLetter.Null) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(false, "Value must not be a letter."),
            _ => new FluentExpected(true)
        });
    }

    public static class Digit
    {
        public static TheoryData<FluentCase<char?>> Cases => F.IsDigit.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsDigit.Null) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be a digit.")
        });
    }

    public static class NotDigit
    {
        public static TheoryData<FluentCase<char?>> Cases => F.IsDigit.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsDigit.Null) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(false, "Value must not be a digit."),
            _ => new FluentExpected(true)
        });
    }

    public static class LetterOrDigit
    {
        public static TheoryData<FluentCase<char?>> Cases => F.IsLetterOrDigit.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsLetterOrDigit.Null) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be a letter or digit.")
        });
    }

    public static class NotLetterOrDigit
    {
        public static TheoryData<FluentCase<char?>> Cases => F.IsLetterOrDigit.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsLetterOrDigit.Null) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(false, "Value must not be a letter or digit."),
            _ => new FluentExpected(true)
        });
    }

    public static class Ascii
    {
        public static TheoryData<FluentCase<char?>> Cases => F.IsAscii.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsAscii.Null) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be an ASCII character.")
        });
    }

    public static class NotAscii
    {
        public static TheoryData<FluentCase<char?>> Cases => F.IsAscii.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsAscii.Null) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(false, "Value must not be an ASCII character."),
            _ => new FluentExpected(true)
        });
    }

    public static class PrintableAscii
    {
        public static TheoryData<FluentCase<char?>> Cases => F.IsPrintableAscii.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsPrintableAscii.Null) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be a printable ASCII character.")
        });
    }

    public static class NotPrintableAscii
    {
        public static TheoryData<FluentCase<char?>> Cases => F.IsPrintableAscii.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsPrintableAscii.Null) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(false, "Value must not be a printable ASCII character."),
            _ => new FluentExpected(true)
        });
    }

    public static class NotWhitespace
    {
        public static TheoryData<FluentCase<char?>> Cases => F.IsWhitespace.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsWhitespace.Null) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(false, "Value must not be whitespace."),
            _ => new FluentExpected(true)
        });
    }

    public static class Control
    {
        public static TheoryData<FluentCase<char?>> Cases => F.IsControl.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsControl.Null) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be a control character.")
        });
    }

    public static class NotControl
    {
        public static TheoryData<FluentCase<char?>> Cases => F.IsControl.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsControl.Null) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(false, "Value must not be a control character."),
            _ => new FluentExpected(true)
        });
    }

    public static class Uppercase
    {
        public static TheoryData<FluentCase<char?>> Cases => F.IsUppercase.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsUppercase.Null) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be an uppercase letter.")
        });
    }

    public static class Lowercase
    {
        public static TheoryData<FluentCase<char?>> Cases => F.IsLowercase.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsLowercase.Null) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be a lowercase letter.")
        });
    }

    public static class HexDigit
    {
        public static TheoryData<FluentCase<char?>> Cases => F.IsHexDigit.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsHexDigit.Null) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be a hexadecimal digit.")
        });
    }

    public static class NotHexDigit
    {
        public static TheoryData<FluentCase<char?>> Cases => F.IsHexDigit.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsHexDigit.Null) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(false, "Value must not be a hexadecimal digit."),
            _ => new FluentExpected(true)
        });
    }
}
