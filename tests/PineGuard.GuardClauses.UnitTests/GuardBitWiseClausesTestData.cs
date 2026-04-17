using PineGuard.Testing.UnitTests.GuardClauses;

namespace PineGuard.GuardClauses.UnitTests;

public static class GuardBitWiseClausesTestData
{
    public static class BitwiseEqualTo
    {
        public static TheoryData<GuardCase<(int value, int other, string mask)>> ValidCases =>
        [
            new("not-equal", (5, 2, "7"), new GuardExpected(true)),
            new("not-equal-with-mask", (5, 2, "1"), new GuardExpected(true))
        ];
        public static TheoryData<GuardCase<(int value, int other, string mask)>> InvalidCases =>
        [
            new("equal", (5, 7, "5"), new GuardExpected(false, typeof(ArgumentException), "value"))
        ];
    }

    public static class NotBitwiseEqualTo
    {
        public static TheoryData<GuardCase<(int value, int other, string mask)>> ValidCases =>
        [
            new("equal", (5, 7, "5"), new GuardExpected(true))
        ];
        public static TheoryData<GuardCase<(int value, int other, string mask)>> InvalidCases =>
        [
            new("not-equal", (5, 2, "7"), new GuardExpected(false, typeof(ArgumentException), "value"))
        ];
    }

    public static class NotHasAllBits
    {
        public static TheoryData<GuardCase<(int value, string mask)>> ValidCases =>
        [
            new("has-all", (7, "5"), new GuardExpected(true))
        ];
        public static TheoryData<GuardCase<(int value, string mask)>> InvalidCases =>
        [
            new("missing-bits", (5, "7"), new GuardExpected(false, typeof(ArgumentException), "value"))
        ];
    }

    public static class NotHasAnyBits
    {
        public static TheoryData<GuardCase<(int value, string mask)>> ValidCases =>
        [
            new("has-any", (5, "4"), new GuardExpected(true))
        ];
        public static TheoryData<GuardCase<(int value, string mask)>> InvalidCases =>
        [
            new("no-shared-bits", (5, "2"), new GuardExpected(false, typeof(ArgumentException), "value"))
        ];
    }

    public static class NotHasNoBits
    {
        public static TheoryData<GuardCase<(int value, string mask)>> ValidCases =>
        [
            new("no-shared", (5, "2"), new GuardExpected(true))
        ];
        public static TheoryData<GuardCase<(int value, string mask)>> InvalidCases =>
        [
            new("shared-bits", (5, "4"), new GuardExpected(false, typeof(ArgumentException), "value"))
        ];
    }

    public static class NotHasOnlyBits
    {
        public static TheoryData<GuardCase<(int value, string mask)>> ValidCases =>
        [
            new("subset", (5, "7"), new GuardExpected(true))
        ];
        public static TheoryData<GuardCase<(int value, string mask)>> InvalidCases =>
        [
            new("not-subset", (7, "5"), new GuardExpected(false, typeof(ArgumentException), "value"))
        ];
    }

    public static class NotPowerOfTwo
    {
        public static TheoryData<GuardCase<int>> ValidCases =>
        [
            new("pow2-4", 4, new GuardExpected(true)),
            new("pow2-1", 1, new GuardExpected(true))
        ];
        public static TheoryData<GuardCase<int>> InvalidCases =>
        [
            new("not-pow2", 3, new GuardExpected(false, typeof(ArgumentException), "value")),
            new("zero", 0, new GuardExpected(false, typeof(ArgumentException), "value"))
        ];
    }

    public static class PowerOfTwo
    {
        public static TheoryData<GuardCase<int>> ValidCases =>
        [
            new("not-pow2", 3, new GuardExpected(true)),
            new("zero", 0, new GuardExpected(true))
        ];
        public static TheoryData<GuardCase<int>> InvalidCases =>
        [
            new("pow2", 4, new GuardExpected(false, typeof(ArgumentException), "value"))
        ];
    }

    public static class HasAllBits
    {
        public static TheoryData<GuardCase<(int value, string mask)>> ValidCases =>
        [
            new("missing-bits", (5, "7"), new GuardExpected(true))
        ];
        public static TheoryData<GuardCase<(int value, string mask)>> InvalidCases =>
        [
            new("has-all", (7, "5"), new GuardExpected(false, typeof(ArgumentException), "value"))
        ];
    }

    public static class HasAnyBits
    {
        public static TheoryData<GuardCase<(int value, string mask)>> ValidCases =>
        [
            new("no-shared-bits", (5, "2"), new GuardExpected(true))
        ];
        public static TheoryData<GuardCase<(int value, string mask)>> InvalidCases =>
        [
            new("has-any", (5, "4"), new GuardExpected(false, typeof(ArgumentException), "value"))
        ];
    }

    public static class HasNoBits
    {
        public static TheoryData<GuardCase<(int value, string mask)>> ValidCases =>
        [
            new("shared-bits", (5, "4"), new GuardExpected(true))
        ];
        public static TheoryData<GuardCase<(int value, string mask)>> InvalidCases =>
        [
            new("no-shared", (5, "2"), new GuardExpected(false, typeof(ArgumentException), "value"))
        ];
    }

    public static class HasOnlyBits
    {
        public static TheoryData<GuardCase<(int value, string mask)>> ValidCases =>
        [
            new("not-subset", (7, "5"), new GuardExpected(true))
        ];
        public static TheoryData<GuardCase<(int value, string mask)>> InvalidCases =>
        [
            new("subset", (5, "7"), new GuardExpected(false, typeof(ArgumentException), "value"))
        ];
    }
}
