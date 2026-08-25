using System.Numerics;
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
            new("bare zero", "0", true, 0),
            new("binary zero", "0b0", true, 0),
            new("underscore after leading zero is not a prefix split", "0_1", true, 1),
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
            new("negative decimal", "-1", false, 0),
            new("underscore splits hex prefix", "0_xFF", false, 0),
            new("underscore splits binary prefix", "0_b0001", false, 0),
            new("double underscore splits hex prefix", "0__xFF", false, 0),
            new("underscore splits uppercase hex prefix", "0_XFF", false, 0),
            new("underscore splits uppercase binary prefix", "0_B0001", false, 0),
            new("triple underscore splits binary prefix", "0___b0001", false, 0),
            new("leading underscore", "_0xFF", false, 0),
            new("trailing underscore", "0xFF_", false, 0)];

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

    public static class TryParseNonNegativeMaskBigInteger
    {
        // BigInteger has no fixed bit width, so Unsafe.SizeOf<BigInteger>() (managed struct layout: a sign
        // int plus an array reference) must never be used as a significant-bit cap for the binary literal
        // path. These cases exceed that struct-layout-derived cap (128 bits on x64 / 64 bits on x86) yet are
        // valid masks that the equivalent hex literal already accepted before and after the fix.
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("binary literal with 129 significant bits parses like the equivalent hex literal", "0b1" + new string('0', 129), true, BigInteger.Pow(2, 129)),
            new("binary literal with 200 significant bits parses like the equivalent hex literal", "0b1" + new string('0', 200), true, BigInteger.Pow(2, 200)),
            new("hex literal with 129 significant bits parses (parity reference for the binary case above)", "0x" + BigInteger.Pow(2, 129).ToString("x"), true, BigInteger.Pow(2, 129))];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("binary invalid digit still rejected regardless of length", "0b1" + new string('0', 129) + "2", false, BigInteger.Zero)];

        public sealed record ValidCase(string Name, string? Value, bool Expected, BigInteger ExpectedOutValue)
            : TryCase<string?, BigInteger>(Name, Value, Expected, ExpectedOutValue);
    }
}
