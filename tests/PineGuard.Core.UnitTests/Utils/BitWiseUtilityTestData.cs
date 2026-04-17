using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Utils;

public static class BitWiseUtilityTestData
{
    public static class TryParseNonNegativeMaskByte
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("hex", "0xFF", true, 255),
            new("hex underscores", "0x_FF", true, 255),
            new("binary", "0b1111_0000", true, 240),
            new("decimal", "10", true, 10),
            new("trim", " 0b0001 ", true, 1)];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("null", null, false, 0),
            new("empty", "", false, 0),
            new("space", " ", false, 0),
            new("hex empty", "0x", false, 0),
            new("invalid hex", "0xGG", false, 0),
            new("overflow hex", "0x100", false, 0),
            new("binary empty", "0b", false, 0),
            new("binary invalid", "0b102", false, 0),
            new("overflow binary", "0b1_0000_0000", false, 0),
            new("invalid decimal", "abc", false, 0),
            new("negative decimal", "-1", false, 0)];

        public sealed record ValidCase(string Name, string? Value, bool Expected, byte ExpectedOutValue)
            : TryCase<string?, byte>(Name, Value, Expected, ExpectedOutValue);
    }

    public static class TryParseNonNegativeMaskUInt16
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("decimal underscores", "1_000", true, 1000),
            new("hex max", "0xFFFF", true, ushort.MaxValue)];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("overflow decimal", "65536", false, 0),
            new("overflow binary", "0b1_0000_0000_0000_0000", false, 0)];

        public sealed record ValidCase(string Name, string? Value, bool Expected, ushort ExpectedOutValue)
            : TryCase<string?, ushort>(Name, Value, Expected, ExpectedOutValue);
    }
}
