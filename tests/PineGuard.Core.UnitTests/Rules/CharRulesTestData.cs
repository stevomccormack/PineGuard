using PineGuard.Rules;

namespace PineGuard.Core.UnitTests.Rules;

public static class CharRulesTestData
{
    public static class ClassificationMethods
    {
        public static TheoryData<Case> ValidCases => new()
        {
            new("Uppercase letter", 'A', true, false, true, true, true, false, false, true, false),
            new("Lowercase letter", 'a', true, false, true, true, true, false, false, false, true),
            new("Digit", '0', false, true, true, true, true, false, false, false, false),
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            new("ASCII min", CharRules.AsciiMinValue, false, false, false, true, false, false, true, false, false),
            new("ASCII max", CharRules.AsciiMaxValue, false, false, false, true, false, false, true, false, false),
            new(
                "Printable ASCII min",
                CharRules.PrintableAsciiMinValue,
                false,
                false,
                false,
                true,
                true,
                true,
                false,
                false,
                false),
            new(
                "Printable ASCII max",
                CharRules.PrintableAsciiMaxValue,
                false,
                false,
                false,
                true,
                true,
                false,
                false,
                false,
                false),
            new("Unit separator", '\u001F', false, false, false, true, false, false, true, false, false),

            // C1 control character: not ASCII, but still control.
            new("C1 control", '\u0080', false, false, false, false, false, false, true, false, false),
        };

        #region Cases

        public sealed record Case(
            string Name,
            char Value,
            bool ExpectedIsLetter,
            bool ExpectedIsDigit,
            bool ExpectedIsLetterOrDigit,
            bool ExpectedIsAscii,
            bool ExpectedIsPrintableAscii,
            bool ExpectedIsWhitespace,
            bool ExpectedIsControl,
            bool ExpectedIsUppercase,
            bool ExpectedIsLowercase)
        {
            public override string ToString() => Name;
        }

        #endregion
    }

    public static class IsHexDigit
    {
        public static TheoryData<Case> ValidCases => new()
        {
            new("Numeric", '9', true),
            new("Lowercase f", 'f', true),
            new("Lowercase a", 'a', true),
            new("Uppercase A", 'A', true),
            new("Uppercase B", 'B', true),
            new("Uppercase E", 'E', true),
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            new("Min hex digit", '0', true),
            new("Max hex digit", 'F', true),

            new("Before 0", '/', false),
            new("After 9", ':', false),
            new("Lowercase g", 'g', false),
            new("Uppercase G", 'G', false),

            new("ASCII min", CharRules.AsciiMinValue, false),
            new("ASCII max", CharRules.AsciiMaxValue, false),
        };

        #region Cases

        public sealed record Case(string Name, char Value, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }
}
