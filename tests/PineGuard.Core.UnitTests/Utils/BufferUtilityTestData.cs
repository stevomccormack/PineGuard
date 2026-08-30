using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Utils;

public static class BufferUtilityTestData
{
    public static class IsHexString
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("single digit", "0", true),
            new("mixed case", "deadBEEF", true),
            new("single char", "F", true),
            new("trimmed", " 0A1b ", true)
        ];

        public static TheoryData<ValidCase> EdgeCases =>
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

        public sealed record ValidCase(string Name, string? Value, bool Expected)
            : IsCase<string?>(Name, Value, Expected);
    }

    public static class IsBase64String
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("M", "TQ==", true),
            new("Hello", "SGVsbG8=", true),
            new("zero", "AA==", true),
            new("trimmed", "  TQ==  ", true)
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("no padding", "AAAA", true),
            new("null", null, false),
            new("empty", "", false),
            new("whitespace", " ", false),
            new("bad padding", "TQ=", false),
            new("too much padding", "TQ===", false),
            new("embedded space", "T Q==", true),
            new("invalid chars", "****", false),
            new("length 1", "A", false),
            new("length 3", "AAA", false),
            new("space", "AA A", false)
        ];

        public sealed record ValidCase(string Name, string? Value, bool Expected)
            : IsCase<string?>(Name, Value, Expected);
    }

    public static class IsBase64UrlString
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("unpadded", "SGVsbG8", true),
            new("padded", "SGVsbG8=", true),
            new("url-safe alphabet", "-_-_", true),
            new("trimmed", "  SGVsbG8  ", true)
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("two chars unpadded", "QQ", true),
            new("two chars padded", "AA==", true),
            new("null", null, false),
            new("empty", "", false),
            new("whitespace", " ", false),
            new("base64 plus", "SGVsbG8+", false),
            new("base64 slash", "SGVsbG8/", false),
            new("embedded space", "SG Vsb", false),
            new("padding in middle", "A=BC", false),
            new("length 1", "A", false),
            new("length 5", "AAAAA", false),
            new("bad padding", "QQ=", false),
            new("too much padding", "====", false),
            new("only padding", "==", false)
        ];

        public sealed record ValidCase(string Name, string? Value, bool Expected)
            : IsCase<string?>(Name, Value, Expected);
    }

    public static class TryDecodeUtf8
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("ascii", [0x48, 0x69], true, "Hi"),
            new("two-byte sequence", [0xC3, 0xA9], true, "é"),
            new("three-byte sequence", [0xE2, 0x82, 0xAC], true, "€"),
            new("four-byte sequence", [0xF0, 0x9F, 0x98, 0x80], true, "\U0001F600")
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("byte-order mark retained", [0xEF, 0xBB, 0xBF], true, "﻿"),
            new("null byte", [0x00], true, "\0"),
            new("null", null, false, null),
            new("empty", [], false, null),
            new("overlong encoding", [0xC0, 0x80], false, null),
            new("surrogate half", [0xED, 0xA0, 0x80], false, null),
            new("truncated sequence", [0xE2, 0x82], false, null),
            new("lone continuation", [0x80], false, null)
        ];

        public sealed record ValidCase(string Name, byte[]? Value, bool Expected, string? ExpectedOutValue)
            : TryCase<byte[]?, string?>(Name, Value, Expected, ExpectedOutValue);
    }
}
