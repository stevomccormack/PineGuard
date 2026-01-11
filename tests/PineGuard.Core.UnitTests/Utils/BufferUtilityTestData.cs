using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Utils;

public static class BufferUtilityTestData
{
    public static class IsHexString
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("single digit", "0", true),
            new("mixed case", "deadBEEF", true),
            new("single char", "F", true),
            new("trimmed", " 0A1b ", true)
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("long", new string('a', 64), true),
            new("null", null, false),
            new("empty", "", false),
            new("whitespace", " ", false),
            new("0x prefix", "0x1", false),
            new("non-hex", "GG", false),
            new("separator", "12-34", false),
            new("control", "\t\r\n", false),
            new("non-ascii", "123\u0080", false)
        ];

        public static TheoryData<IThrowsCase> InvalidCases => [];

        #region Case Records

        public sealed record Case(string Name, string? Value, bool ExpectedReturn)
            : IsCase<string?>(Name, Value, ExpectedReturn);

        #endregion
    }

    public static class IsBase64String
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("M", "TQ==", true),
            new("Hello", "SGVsbG8=", true),
            new("zero", "AA==", true),
            new("trimmed", "  TQ==  ", true)
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("no padding", "AAAA", true),
            new("null", null, false),
            new("empty", "", false),
            new("whitespace", " ", false),
            new("bad padding", "TQ=", false),
            new("too much padding", "TQ===", false),
            new("embedded space", "T Q==", false),
            new("invalid chars", "****", false),
            new("length 1", "A", false),
            new("length 3", "AAA", false),
            new("space", "AA A", false)
        ];

        public static TheoryData<IThrowsCase> InvalidCases => [];

        #region Case Records

        public sealed record Case(string Name, string? Value, bool ExpectedReturn)
            : IsCase<string?>(Name, Value, ExpectedReturn);

        #endregion
    }
}
