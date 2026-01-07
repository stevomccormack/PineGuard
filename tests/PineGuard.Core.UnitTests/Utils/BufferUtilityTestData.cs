using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Utils;

public static class BufferUtilityTestData
{
    public static class IsHexString
    {
        private static Case V(string name, string? input, bool expected) => new(name, input, expected);

        public static TheoryData<Case> ValidCases => new()
        {
            { V("single digit", "0", true) },
            { V("mixed case", "deadBEEF", true) },
            { V("single char", "F", true) },
            { V("trimmed", " 0A1b ", true) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { V("long", new string('a', 64), true) },
            { V("null", null, false) },
            { V("empty", "", false) },
            { V("whitespace", " ", false) },
            { V("0x prefix", "0x1", false) },
            { V("non-hex", "GG", false) },
            { V("separator", "12-34", false) },
            { V("control", "\t\r\n", false) },
            { V("non-ascii", "123\u0080", false) },
        };

        public static TheoryData<ThrowsCase> InvalidCases => [];

        #region Cases

        public sealed record Case(string Name, string? Input, bool Expected)
            : IsCase<string?>(Name, Input, Expected);

        #endregion
    }

    public static class IsBase64String
    {
        private static Case V(string name, string? input, bool expected) => new(name, input, expected);

        public static TheoryData<Case> ValidCases => new()
        {
            { V("M", "TQ==", true) },
            { V("Hello", "SGVsbG8=", true) },
            { V("zero", "AA==", true) },
            { V("trimmed", "  TQ==  ", true) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { V("no padding", "AAAA", true) },
            { V("null", null, false) },
            { V("empty", "", false) },
            { V("whitespace", " ", false) },
            { V("bad padding", "TQ=", false) },
            { V("too much padding", "TQ===", false) },
            { V("embedded space", "T Q==", false) },
            { V("invalid chars", "****", false) },
            { V("length 1", "A", false) },
            { V("length 3", "AAA", false) },
            { V("space", "AA A", false) },
        };

        public static TheoryData<ThrowsCase> InvalidCases => [];

        #region Cases

        public sealed record Case(string Name, string? Input, bool Expected)
            : IsCase<string?>(Name, Input, Expected);

        #endregion
    }
}
