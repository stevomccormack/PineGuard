using PineGuard.Testing.UnitTests.MustClauses;

namespace PineGuard.MustClauses.UnitTests;

public static class MustBitWiseClausesTestData
{
    public static class BitwiseEqualTo
    {
        public static TheoryData<MustCase<(int input, int other, string? mask)>> ValidCases =>
        [
            new("equal no mask",         (5, 5, null), new MustExpected(true)),
            new("equal with mask 1",     (5, 5, "1"),  new MustExpected(true)),
            new("equal with mask 1 5-7", (5, 7, "1"),  new MustExpected(true))
        ];

        public static TheoryData<MustCase<(int input, int other, string? mask)>> InvalidCases =>
        [
            new("not equal no mask",   (5, 6, null),      new MustExpected(false, "value must be bitwise equal to the expected value.")),
            new("not equal with mask", (5, 7, "2"),        new MustExpected(false, "value must be bitwise equal to the expected value.")),
            new("invalid mask alpha",  (5, 5, "invalid"),  new MustExpected(false, "mask must be a valid bitwise mask.", "mask")),
            new("invalid mask xyz",    (5, 5, "xyz"),      new MustExpected(false, "mask must be a valid bitwise mask.", "mask")),
            new("zero mask",           (5, 5, "0"),        new MustExpected(false, "mask must be a valid bitwise mask.", "mask"))
        ];
    }

    public static class NotBitwiseEqualTo
    {
        public static TheoryData<MustCase<(int input, int other, string? mask)>> ValidCases =>
        [
            new("not equal no mask",   (5, 6, null), new MustExpected(true)),
            new("not equal with mask", (5, 6, "2"),  new MustExpected(true))
        ];

        public static TheoryData<MustCase<(int input, int other, string? mask)>> InvalidCases =>
        [
            new("equal no mask",       (5, 5, null),      new MustExpected(false, "value must not be bitwise equal to the expected value.")),
            new("equal with mask",     (5, 7, "1"),        new MustExpected(false, "value must not be bitwise equal to the expected value.")),
            new("invalid mask alpha",  (5, 5, "invalid"),  new MustExpected(false, "mask must be a valid bitwise mask.", "mask")),
            new("zero mask 5-5",       (5, 5, "0"),        new MustExpected(false, "mask must be a valid bitwise mask.", "mask")),
            new("invalid mask xyz",    (5, 6, "xyz"),      new MustExpected(false, "mask must be a valid bitwise mask.", "mask")),
            new("zero mask 5-6",       (5, 6, "0"),        new MustExpected(false, "mask must be a valid bitwise mask.", "mask"))
        ];
    }

    public static class HasAllBits
    {
        public static TheoryData<MustCase<(int input, string? mask)>> ValidCases =>
        [
            new("has all bits", (7, "3"), new MustExpected(true))
        ];

        public static TheoryData<MustCase<(int input, string? mask)>> InvalidCases =>
        [
            new("missing bit",     (5, "3"),   new MustExpected(false, "value must contain all required bits.")),
            new("null mask",       (5, null),  new MustExpected(false, "mask must be a valid bitwise mask.", "mask")),
            new("empty mask",      (5, ""),    new MustExpected(false, "mask must be a valid bitwise mask.", "mask")),
            new("whitespace mask", (5, "   "), new MustExpected(false, "mask must be a valid bitwise mask.", "mask")),
            new("invalid mask",    (5, "xyz"), new MustExpected(false, "mask must be a valid bitwise mask.", "mask")),
            new("zero mask",       (5, "0"),   new MustExpected(false, "mask must be a valid bitwise mask.", "mask"))
        ];
    }

    public static class HasAnyBits
    {
        public static TheoryData<MustCase<(int input, string? mask)>> ValidCases =>
        [
            new("has any bits", (5, "3"), new MustExpected(true))
        ];

        public static TheoryData<MustCase<(int input, string? mask)>> InvalidCases =>
        [
            new("no bits",         (4, "3"),   new MustExpected(false, "value must contain at least one required bit.")),
            new("null mask",       (5, null),  new MustExpected(false, "mask must be a valid bitwise mask.", "mask")),
            new("empty mask",      (5, ""),    new MustExpected(false, "mask must be a valid bitwise mask.", "mask")),
            new("whitespace mask", (5, "   "), new MustExpected(false, "mask must be a valid bitwise mask.", "mask")),
            new("invalid mask",    (5, "xyz"), new MustExpected(false, "mask must be a valid bitwise mask.", "mask")),
            new("zero mask",       (5, "0"),   new MustExpected(false, "mask must be a valid bitwise mask.", "mask"))
        ];
    }

    public static class HasNoBits
    {
        public static TheoryData<MustCase<(int input, string? mask)>> ValidCases =>
        [
            new("no bits", (4, "3"), new MustExpected(true))
        ];

        public static TheoryData<MustCase<(int input, string? mask)>> InvalidCases =>
        [
            new("has bits",            (5, "3"),   new MustExpected(false, "value must contain none of the forbidden bits.")),
            new("invalid mask val 4",  (4, "xyz"), new MustExpected(false, "mask must be a valid bitwise mask.", "mask")),
            new("null mask",           (5, null),  new MustExpected(false, "mask must be a valid bitwise mask.", "mask")),
            new("empty mask",          (5, ""),    new MustExpected(false, "mask must be a valid bitwise mask.", "mask")),
            new("whitespace mask",     (5, "   "), new MustExpected(false, "mask must be a valid bitwise mask.", "mask")),
            new("invalid mask val 5",  (5, "xyz"), new MustExpected(false, "mask must be a valid bitwise mask.", "mask")),
            new("zero mask",           (5, "0"),   new MustExpected(false, "mask must be a valid bitwise mask.", "mask"))
        ];
    }

    public static class HasOnlyBits
    {
        public static TheoryData<MustCase<(int input, string? allowedMask)>> ValidCases =>
        [
            new("only bits", (3, "7"), new MustExpected(true))
        ];

        public static TheoryData<MustCase<(int input, string? allowedMask)>> InvalidCases =>
        [
            new("extra bits",      (7, "3"),   new MustExpected(false, "value must contain only allowed bits.")),
            new("null mask",       (3, null),  new MustExpected(false, "allowedMask must be a valid bitwise mask.", "allowedMask")),
            new("empty mask",      (3, ""),    new MustExpected(false, "allowedMask must be a valid bitwise mask.", "allowedMask")),
            new("whitespace mask", (3, "   "), new MustExpected(false, "allowedMask must be a valid bitwise mask.", "allowedMask")),
            new("invalid mask",    (3, "xyz"), new MustExpected(false, "allowedMask must be a valid bitwise mask.", "allowedMask")),
            new("zero mask",       (3, "0"),   new MustExpected(false, "allowedMask must be a valid bitwise mask.", "allowedMask"))
        ];
    }

    public static class NotHasAllBits
    {
        public static TheoryData<MustCase<(int input, string? mask)>> ValidCases =>
        [
            new("missing bit", (5, "3"), new MustExpected(true))
        ];

        public static TheoryData<MustCase<(int input, string? mask)>> InvalidCases =>
        [
            new("has all bits",    (7, "3"),   new MustExpected(false, "value must not contain all required bits.")),
            new("null mask",       (5, null),  new MustExpected(false, "mask must be a valid bitwise mask.", "mask")),
            new("empty mask",      (5, ""),    new MustExpected(false, "mask must be a valid bitwise mask.", "mask")),
            new("whitespace mask", (5, "   "), new MustExpected(false, "mask must be a valid bitwise mask.", "mask")),
            new("invalid mask",    (5, "xyz"), new MustExpected(false, "mask must be a valid bitwise mask.", "mask")),
            new("zero mask",       (5, "0"),   new MustExpected(false, "mask must be a valid bitwise mask.", "mask"))
        ];
    }

    public static class NotHasAnyBits
    {
        public static TheoryData<MustCase<(int input, string? mask)>> ValidCases =>
        [
            new("no bits", (4, "3"), new MustExpected(true))
        ];

        public static TheoryData<MustCase<(int input, string? mask)>> InvalidCases =>
        [
            new("has any bits",    (5, "3"),   new MustExpected(false, "value must not contain any of the specified bits.")),
            new("null mask",       (5, null),  new MustExpected(false, "mask must be a valid bitwise mask.", "mask")),
            new("empty mask",      (5, ""),    new MustExpected(false, "mask must be a valid bitwise mask.", "mask")),
            new("whitespace mask", (5, "   "), new MustExpected(false, "mask must be a valid bitwise mask.", "mask")),
            new("invalid mask",    (5, "xyz"), new MustExpected(false, "mask must be a valid bitwise mask.", "mask")),
            new("zero mask",       (5, "0"),   new MustExpected(false, "mask must be a valid bitwise mask.", "mask"))
        ];
    }

    public static class NotHasNoBits
    {
        public static TheoryData<MustCase<(int input, string? mask)>> ValidCases =>
        [
            new("has bits", (5, "3"), new MustExpected(true))
        ];

        public static TheoryData<MustCase<(int input, string? mask)>> InvalidCases =>
        [
            new("no bits",             (4, "3"),   new MustExpected(false, "value must contain at least one of the forbidden bits.")),
            new("invalid mask val 4",  (4, "xyz"), new MustExpected(false, "mask must be a valid bitwise mask.", "mask")),
            new("null mask",           (5, null),  new MustExpected(false, "mask must be a valid bitwise mask.", "mask")),
            new("empty mask",          (5, ""),    new MustExpected(false, "mask must be a valid bitwise mask.", "mask")),
            new("whitespace mask",     (5, "   "), new MustExpected(false, "mask must be a valid bitwise mask.", "mask")),
            new("invalid mask val 5",  (5, "xyz"), new MustExpected(false, "mask must be a valid bitwise mask.", "mask")),
            new("zero mask",           (5, "0"),   new MustExpected(false, "mask must be a valid bitwise mask.", "mask"))
        ];
    }

    public static class NotHasOnlyBits
    {
        public static TheoryData<MustCase<(int input, string? allowedMask)>> ValidCases =>
        [
            new("extra bits", (7, "3"), new MustExpected(true))
        ];

        public static TheoryData<MustCase<(int input, string? allowedMask)>> InvalidCases =>
        [
            new("only bits",       (3, "7"),   new MustExpected(false, "value must contain bits not allowed by the mask.")),
            new("null mask",       (3, null),  new MustExpected(false, "allowedMask must be a valid bitwise mask.", "allowedMask")),
            new("empty mask",      (3, ""),    new MustExpected(false, "allowedMask must be a valid bitwise mask.", "allowedMask")),
            new("whitespace mask", (3, "   "), new MustExpected(false, "allowedMask must be a valid bitwise mask.", "allowedMask")),
            new("invalid mask",    (3, "xyz"), new MustExpected(false, "allowedMask must be a valid bitwise mask.", "allowedMask")),
            new("zero mask",       (3, "0"),   new MustExpected(false, "allowedMask must be a valid bitwise mask.", "allowedMask"))
        ];
    }

    public static class PowerOfTwo
    {
        public static TheoryData<MustCase<int>> ValidCases =>
        [
            new("power of two", 4, new MustExpected(true))
        ];

        public static TheoryData<MustCase<int>> InvalidCases =>
        [
            new("not power of two", 3, new MustExpected(false, "value must be a power of two.")),
            new("zero",             0, new MustExpected(false, "value must be a power of two."))
        ];
    }

    public static class NotPowerOfTwo
    {
        public static TheoryData<MustCase<int>> ValidCases =>
        [
            new("not power of two", 3, new MustExpected(true))
        ];

        public static TheoryData<MustCase<int>> InvalidCases =>
        [
            new("power of two", 4, new MustExpected(false, "value must not be a power of two."))
        ];
    }
}
