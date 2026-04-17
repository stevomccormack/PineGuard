using PineGuard.Testing.UnitTests.GuardClauses;
using PineGuard.Testing.UnitTests.Rules;
using F = PineGuard.Testing.Fixtures.CharRulesFixtures;

namespace PineGuard.GuardClauses.UnitTests;

public static class GuardCharClausesTestData
{
    public static class NotLetter
    {
        public static TheoryData<GuardCase<char?>> ValidCases => F.IsLetter.AllValid.ToGuardCases();
        public static TheoryData<GuardCase<char?>> InvalidCases => F.IsLetter.AllInvalid.Except(nameof(F.IsLetter.Null)).ToGuardCases("value");
    }

    public static class NotDigit
    {
        public static TheoryData<GuardCase<char?>> ValidCases => F.IsDigit.AllValid.ToGuardCases();
        public static TheoryData<GuardCase<char?>> InvalidCases => F.IsDigit.AllInvalid.Except(nameof(F.IsDigit.Null)).ToGuardCases("value");
    }

    public static class Digit
    {
        public static TheoryData<GuardCase<char?>> ValidCases => F.IsDigit.AllInvalid.Except(nameof(F.IsDigit.Null)).ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<char?>> InvalidCases => F.IsDigit.AllValid.ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    public static class NotLetterOrDigit
    {
        public static TheoryData<GuardCase<char?>> ValidCases => F.IsLetterOrDigit.AllValid.ToGuardCases();
        public static TheoryData<GuardCase<char?>> InvalidCases => F.IsLetterOrDigit.AllInvalid.Except(nameof(F.IsLetterOrDigit.Null)).ToGuardCases("value");
    }

    public static class LetterOrDigit
    {
        public static TheoryData<GuardCase<char?>> ValidCases => F.IsLetterOrDigit.AllInvalid.Except(nameof(F.IsLetterOrDigit.Null)).ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<char?>> InvalidCases => F.IsLetterOrDigit.AllValid.ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    public static class NotAscii
    {
        public static TheoryData<GuardCase<char?>> ValidCases => F.IsAscii.AllValid.ToGuardCases();
        public static TheoryData<GuardCase<char?>> InvalidCases => F.IsAscii.AllInvalid.Except(nameof(F.IsAscii.Null)).ToGuardCases("value");
    }

    public static class Ascii
    {
        public static TheoryData<GuardCase<char?>> ValidCases => F.IsAscii.AllInvalid.Except(nameof(F.IsAscii.Null)).ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<char?>> InvalidCases => F.IsAscii.AllValid.ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    public static class NotPrintableAscii
    {
        public static TheoryData<GuardCase<char?>> ValidCases => F.IsPrintableAscii.AllValid.ToGuardCases();
        public static TheoryData<GuardCase<char?>> InvalidCases => F.IsPrintableAscii.AllInvalid.Except(nameof(F.IsPrintableAscii.Null)).ToGuardCases("value");
    }

    public static class PrintableAscii
    {
        public static TheoryData<GuardCase<char?>> ValidCases => F.IsPrintableAscii.AllInvalid.Except(nameof(F.IsPrintableAscii.Null)).ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<char?>> InvalidCases => F.IsPrintableAscii.AllValid.ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    public static class Whitespace
    {
        public static TheoryData<GuardCase<char?>> ValidCases => F.IsWhitespace.AllInvalid.Except(nameof(F.IsWhitespace.Null)).ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<char?>> InvalidCases => F.IsWhitespace.AllValid.ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    public static class NotControl
    {
        public static TheoryData<GuardCase<char?>> ValidCases => F.IsControl.AllValid.ToGuardCases();
        public static TheoryData<GuardCase<char?>> InvalidCases => F.IsControl.AllInvalid.Except(nameof(F.IsControl.Null)).ToGuardCases("value");
    }

    public static class Control
    {
        public static TheoryData<GuardCase<char?>> ValidCases => F.IsControl.AllInvalid.Except(nameof(F.IsControl.Null)).ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<char?>> InvalidCases => F.IsControl.AllValid.ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    public static class Letter
    {
        public static TheoryData<GuardCase<char?>> ValidCases => F.IsLetter.AllInvalid.Except(nameof(F.IsLetter.Null)).ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<char?>> InvalidCases => F.IsLetter.AllValid.ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    public static class HexDigit
    {
        public static TheoryData<GuardCase<char?>> ValidCases => F.IsHexDigit.AllInvalid.Except(nameof(F.IsHexDigit.Null)).ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<char?>> InvalidCases => F.IsHexDigit.AllValid.ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    public static class NotHexDigit
    {
        public static TheoryData<GuardCase<char?>> ValidCases => F.IsHexDigit.AllValid.ToGuardCases();
        public static TheoryData<GuardCase<char?>> InvalidCases => F.IsHexDigit.AllInvalid.Except(nameof(F.IsHexDigit.Null)).ToGuardCases("value");
    }

    public static class Lowercase
    {
        public static TheoryData<GuardCase<char?>> ValidCases => F.IsUppercase.AllValid.ToGuardCases();
        public static TheoryData<GuardCase<char?>> InvalidCases => F.IsUppercase.AllInvalid.Except(nameof(F.IsUppercase.Null)).ToGuardCases("value");
    }

    public static class Uppercase
    {
        public static TheoryData<GuardCase<char?>> ValidCases => F.IsLowercase.AllValid.ToGuardCases();
        public static TheoryData<GuardCase<char?>> InvalidCases => F.IsLowercase.AllInvalid.Except(nameof(F.IsLowercase.Null)).ToGuardCases("value");
    }
}
