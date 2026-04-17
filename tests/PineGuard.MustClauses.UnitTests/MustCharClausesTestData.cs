using PineGuard.Testing.UnitTests.MustClauses;
using PineGuard.Testing.UnitTests.Rules;
using F = PineGuard.Testing.Fixtures.CharRulesFixtures;

namespace PineGuard.MustClauses.UnitTests;

public static class MustCharClausesTestData
{
    public static class Letter
    {
        public static TheoryData<MustCase<char?>> ValidCases => F.IsLetter.ValidScenarios.ToMustCases();
        public static TheoryData<MustCase<char?>> InvalidCases => F.IsLetter.AllInvalid.Except(nameof(F.IsLetter.Null)).ToMustCases(_ => new MustExpected(false, "value must be a letter."));
    }

    public static class NotLetter
    {
        public static TheoryData<MustCase<char?>> ValidCases => F.IsLetter.AllInvalid.Except(nameof(F.IsLetter.Null)).ToMustCases(_ => new MustExpected(true));
        public static TheoryData<MustCase<char?>> InvalidCases => F.IsLetter.AllValid.ToMustCases(_ => new MustExpected(false, "value must not be a letter."));
    }

    public static class Digit
    {
        public static TheoryData<MustCase<char?>> ValidCases => F.IsDigit.ValidScenarios.ToMustCases();
        public static TheoryData<MustCase<char?>> InvalidCases => F.IsDigit.AllInvalid.Except(nameof(F.IsDigit.Null)).ToMustCases(_ => new MustExpected(false, "value must be a digit."));
    }

    public static class NotDigit
    {
        public static TheoryData<MustCase<char?>> ValidCases => F.IsDigit.AllInvalid.Except(nameof(F.IsDigit.Null)).ToMustCases(_ => new MustExpected(true));
        public static TheoryData<MustCase<char?>> InvalidCases => F.IsDigit.AllValid.ToMustCases(_ => new MustExpected(false, "value must not be a digit."));
    }

    public static class LetterOrDigit
    {
        public static TheoryData<MustCase<char?>> ValidCases => F.IsLetterOrDigit.ValidScenarios.ToMustCases();
        public static TheoryData<MustCase<char?>> InvalidCases => F.IsLetterOrDigit.AllInvalid.Except(nameof(F.IsLetterOrDigit.Null)).ToMustCases(_ => new MustExpected(false, "value must be a letter or digit."));
    }

    public static class NotLetterOrDigit
    {
        public static TheoryData<MustCase<char?>> ValidCases => F.IsLetterOrDigit.AllInvalid.Except(nameof(F.IsLetterOrDigit.Null)).ToMustCases(_ => new MustExpected(true));
        public static TheoryData<MustCase<char?>> InvalidCases => F.IsLetterOrDigit.AllValid.ToMustCases(_ => new MustExpected(false, "value must not be a letter or digit."));
    }

    public static class Ascii
    {
        public static TheoryData<MustCase<char?>> ValidCases => F.IsAscii.AllValid.ToMustCases();
        public static TheoryData<MustCase<char?>> InvalidCases => F.IsAscii.AllInvalid.Except(nameof(F.IsAscii.Null)).ToMustCases(_ => new MustExpected(false, "value must be an ASCII character."));
    }

    public static class NotAscii
    {
        public static TheoryData<MustCase<char?>> ValidCases => F.IsAscii.AllInvalid.Except(nameof(F.IsAscii.Null)).ToMustCases(_ => new MustExpected(true));
        public static TheoryData<MustCase<char?>> InvalidCases => F.IsAscii.AllValid.ToMustCases(_ => new MustExpected(false, "value must not be an ASCII character."));
    }

    public static class PrintableAscii
    {
        public static TheoryData<MustCase<char?>> ValidCases => F.IsPrintableAscii.AllValid.ToMustCases();
        public static TheoryData<MustCase<char?>> InvalidCases => F.IsPrintableAscii.AllInvalid.Except(nameof(F.IsPrintableAscii.Null)).ToMustCases(_ => new MustExpected(false, "value must be a printable ASCII character."));
    }

    public static class NotPrintableAscii
    {
        public static TheoryData<MustCase<char?>> ValidCases => F.IsPrintableAscii.AllInvalid.Except(nameof(F.IsPrintableAscii.Null)).ToMustCases(_ => new MustExpected(true));
        public static TheoryData<MustCase<char?>> InvalidCases => F.IsPrintableAscii.AllValid.ToMustCases(_ => new MustExpected(false, "value must not be a printable ASCII character."));
    }

    public static class NotWhitespace
    {
        public static TheoryData<MustCase<char?>> ValidCases => F.IsWhitespace.AllInvalid.Except(nameof(F.IsWhitespace.Null)).ToMustCases(_ => new MustExpected(true));
        public static TheoryData<MustCase<char?>> InvalidCases => F.IsWhitespace.AllValid.ToMustCases(_ => new MustExpected(false, "value must not be whitespace."));
    }

    public static class Control
    {
        public static TheoryData<MustCase<char?>> ValidCases => F.IsControl.AllValid.ToMustCases();
        public static TheoryData<MustCase<char?>> InvalidCases => F.IsControl.AllInvalid.Except(nameof(F.IsControl.Null)).ToMustCases(_ => new MustExpected(false, "value must be a control character."));
    }

    public static class NotControl
    {
        public static TheoryData<MustCase<char?>> ValidCases => F.IsControl.AllInvalid.Except(nameof(F.IsControl.Null)).ToMustCases(_ => new MustExpected(true));
        public static TheoryData<MustCase<char?>> InvalidCases => F.IsControl.AllValid.ToMustCases(_ => new MustExpected(false, "value must not be a control character."));
    }

    public static class Uppercase
    {
        public static TheoryData<MustCase<char?>> ValidCases => F.IsUppercase.ValidScenarios.ToMustCases();
        public static TheoryData<MustCase<char?>> InvalidCases => F.IsUppercase.AllInvalid.Except(nameof(F.IsUppercase.Null)).ToMustCases(_ => new MustExpected(false, "value must be an uppercase letter."));
    }

    public static class Lowercase
    {
        public static TheoryData<MustCase<char?>> ValidCases => F.IsLowercase.ValidScenarios.ToMustCases();
        public static TheoryData<MustCase<char?>> InvalidCases => F.IsLowercase.AllInvalid.Except(nameof(F.IsLowercase.Null)).ToMustCases(_ => new MustExpected(false, "value must be a lowercase letter."));
    }

    public static class HexDigit
    {
        public static TheoryData<MustCase<char?>> ValidCases => F.IsHexDigit.AllValid.ToMustCases();
        public static TheoryData<MustCase<char?>> InvalidCases => F.IsHexDigit.AllInvalid.Except(nameof(F.IsHexDigit.Null)).ToMustCases(_ => new MustExpected(false, "value must be a hexadecimal digit."));
    }

    public static class NotHexDigit
    {
        public static TheoryData<MustCase<char?>> ValidCases => F.IsHexDigit.AllInvalid.Except(nameof(F.IsHexDigit.Null)).ToMustCases(_ => new MustExpected(true));
        public static TheoryData<MustCase<char?>> InvalidCases => F.IsHexDigit.AllValid.ToMustCases(_ => new MustExpected(false, "value must not be a hexadecimal digit."));
    }
}
