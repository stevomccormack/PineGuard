using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;
using F = PineGuard.Testing.Fixtures.BitWiseRulesFixtures;

namespace PineGuard.DataAnnotations.UnitTests;

public static class BitWiseAttributesTestData
{
    public static class BitwiseEqualTo
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("equal", 5, 5, true),
            new("null", null, 5, true)
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("not equal", 6, 5, false)
        ];

        public static TheoryData<IThrowsCase> InvalidCases =>
        [
            new InvalidCase(
                "wrong type",
                "not an int",
                new ExpectedException(typeof(InvalidOperationException), null, "[BitwiseEqualToAttribute] can only be applied to properties of type Int32"))
        ];

        public sealed record ValidCase(string Name, int? Value, int EqualTo, bool Expected)
            : ReturnCase<int?, bool>(Name, Value, Expected);

        public sealed record InvalidCase(string Name, object? Value, ExpectedException ExpectedException)
            : ThrowsCase<object?>(Name, Value, ExpectedException);
    }

    public static class NotBitwiseEqualTo
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("not equal", 6, 5, true),
            new("null", null, 5, true)
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("equal", 5, 5, false)
        ];

        public static TheoryData<IThrowsCase> InvalidCases =>
        [
            new InvalidCase(
                "wrong type",
                "not an int",
                new ExpectedException(typeof(InvalidOperationException), null, "[NotBitwiseEqualToAttribute] can only be applied to properties of type Int32"))
        ];

        public sealed record ValidCase(string Name, int? Value, int EqualTo, bool Expected)
            : ReturnCase<int?, bool>(Name, Value, Expected);

        public sealed record InvalidCase(string Name, object? Value, ExpectedException ExpectedException)
            : ThrowsCase<object?>(Name, Value, ExpectedException);
    }

    public static class HasAllBits
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new(nameof(F.HasAllBits.HasAllBitsSet), F.HasAllBits.HasAllBitsSet.value, F.HasAllBits.HasAllBitsSet.mask, true),
            new(nameof(F.HasAllBits.ValueNull),     F.HasAllBits.ValueNull.value,     F.HasAllBits.ValueNull.mask,     true)
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new(nameof(F.HasAllBits.MissingOneBit), F.HasAllBits.MissingOneBit.value, F.HasAllBits.MissingOneBit.mask, false)
        ];

        public static TheoryData<IThrowsCase> InvalidCases =>
        [
            new InvalidCase(
                "wrong type",
                "not an int",
                new ExpectedException(typeof(InvalidOperationException), null, "[HasAllBitsAttribute] can only be applied to properties of type Int32"))
        ];

        public sealed record ValidCase(string Name, int? Value, int Mask, bool Expected)
            : ReturnCase<int?, bool>(Name, Value, Expected);

        public sealed record InvalidCase(string Name, object? Value, ExpectedException ExpectedException)
            : ThrowsCase<object?>(Name, Value, ExpectedException);
    }

    public static class HasAnyBits
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new(nameof(F.HasAnyBits.HasAtLeastOne), F.HasAnyBits.HasAtLeastOne.value, F.HasAnyBits.HasAtLeastOne.mask, true),
            new(nameof(F.HasAnyBits.ValueNull),     F.HasAnyBits.ValueNull.value,     F.HasAnyBits.ValueNull.mask,     true)
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new(nameof(F.HasAnyBits.HasNone), F.HasAnyBits.HasNone.value, F.HasAnyBits.HasNone.mask, false)
        ];

        public static TheoryData<IThrowsCase> InvalidCases =>
        [
            new InvalidCase(
                "wrong type",
                "not an int",
                new ExpectedException(typeof(InvalidOperationException), null, "[HasAnyBitsAttribute] can only be applied to properties of type Int32"))
        ];

        public sealed record ValidCase(string Name, int? Value, int Mask, bool Expected)
            : ReturnCase<int?, bool>(Name, Value, Expected);

        public sealed record InvalidCase(string Name, object? Value, ExpectedException ExpectedException)
            : ThrowsCase<object?>(Name, Value, ExpectedException);
    }

    public static class HasNoBits
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new(nameof(F.HasNoBits.NoBitsSet), F.HasNoBits.NoBitsSet.value, F.HasNoBits.NoBitsSet.mask, true),
            new(nameof(F.HasNoBits.ValueNull), F.HasNoBits.ValueNull.value, F.HasNoBits.ValueNull.mask, true)
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new(nameof(F.HasNoBits.HasSome), F.HasNoBits.HasSome.value, F.HasNoBits.HasSome.mask, false)
        ];

        public static TheoryData<IThrowsCase> InvalidCases =>
        [
            new InvalidCase(
                "wrong type",
                "not an int",
                new ExpectedException(typeof(InvalidOperationException), null, "[HasNoBitsAttribute] can only be applied to properties of type Int32"))
        ];

        public sealed record ValidCase(string Name, int? Value, int Mask, bool Expected)
            : ReturnCase<int?, bool>(Name, Value, Expected);

        public sealed record InvalidCase(string Name, object? Value, ExpectedException ExpectedException)
            : ThrowsCase<object?>(Name, Value, ExpectedException);
    }

    public static class HasOnlyBits
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new(nameof(F.HasOnlyBits.OnlyAllowed), F.HasOnlyBits.OnlyAllowed.value, F.HasOnlyBits.OnlyAllowed.allowedMask, true),
            new(nameof(F.HasOnlyBits.ValueNull),   F.HasOnlyBits.ValueNull.value,   F.HasOnlyBits.ValueNull.allowedMask,   true)
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new(nameof(F.HasOnlyBits.DisallowedBit), F.HasOnlyBits.DisallowedBit.value, F.HasOnlyBits.DisallowedBit.allowedMask, false)
        ];

        public static TheoryData<IThrowsCase> InvalidCases =>
        [
            new InvalidCase(
                "wrong type",
                "not an int",
                new ExpectedException(typeof(InvalidOperationException), null, "[HasOnlyBitsAttribute] can only be applied to properties of type Int32"))
        ];

        public sealed record ValidCase(string Name, int? Value, int Mask, bool Expected)
            : ReturnCase<int?, bool>(Name, Value, Expected);

        public sealed record InvalidCase(string Name, object? Value, ExpectedException ExpectedException)
            : ThrowsCase<object?>(Name, Value, ExpectedException);
    }

    public static class PowerOfTwo
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new(nameof(F.IsPowerOfTwo.Two),       F.IsPowerOfTwo.Two,       true),
            new(nameof(F.IsPowerOfTwo.ValueNull),  F.IsPowerOfTwo.ValueNull,  true)
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new(nameof(F.IsPowerOfTwo.NotPowerOfTwo), F.IsPowerOfTwo.NotPowerOfTwo, false)
        ];

        public static TheoryData<IThrowsCase> InvalidCases =>
        [
            new InvalidCase(
                "wrong type",
                "not an int",
                new ExpectedException(typeof(InvalidOperationException), null, "[PowerOfTwoAttribute] can only be applied to properties of type Int32"))
        ];

        public sealed record ValidCase(string Name, int? Value, bool Expected)
            : ReturnCase<int?, bool>(Name, Value, Expected);

        public sealed record InvalidCase(string Name, object? Value, ExpectedException ExpectedException)
            : ThrowsCase<object?>(Name, Value, ExpectedException);
    }

    public static class NotPowerOfTwo
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new(nameof(F.IsPowerOfTwo.NotPowerOfTwo), F.IsPowerOfTwo.NotPowerOfTwo, true),
            new(nameof(F.IsPowerOfTwo.ValueNull),     F.IsPowerOfTwo.ValueNull,     true)
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new(nameof(F.IsPowerOfTwo.Two), F.IsPowerOfTwo.Two, false)
        ];

        public static TheoryData<IThrowsCase> InvalidCases =>
        [
            new InvalidCase(
                "wrong type",
                "not an int",
                new ExpectedException(typeof(InvalidOperationException), null, "[NotPowerOfTwoAttribute] can only be applied to properties of type Int32"))
        ];

        public sealed record ValidCase(string Name, int? Value, bool Expected)
            : ReturnCase<int?, bool>(Name, Value, Expected);

        public sealed record InvalidCase(string Name, object? Value, ExpectedException ExpectedException)
            : ThrowsCase<object?>(Name, Value, ExpectedException);
    }
}
