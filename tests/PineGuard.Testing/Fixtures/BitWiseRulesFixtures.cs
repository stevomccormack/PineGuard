using PineGuard.Testing.UnitTests.Rules;

namespace PineGuard.Testing.Fixtures;

public static class BitWiseRulesFixtures
{
    public static class IsBitwiseEqualTo
    {
        public static readonly (int? left, int? right, int mask) MaskSubset = (10, 14, 0b0010);
        public static readonly (int? left, int? right, int mask) HexLowByte = (0x12CD, 0x34CD, 0xFF);
        public static readonly (int? left, int? right, int mask) FullMaskMatch = (10, 10, 0b1111);
        public static readonly (int? left, int? right, int mask) ZeroMask = (1, 1, 0);
        public static readonly (int? left, int? right, int mask) MaskedMismatch = (10, 14, 0b0100);
        public static readonly (int? left, int? right, int mask) LeftNull = (null, 1, 0b0010);
        public static readonly (int? left, int? right, int mask) RightNull = (1, null, 0b0010);

        public static RuleScenario<(int? left, int? right, int mask)>[] ValidScenarios =>
        [
            new(nameof(MaskSubset), MaskSubset, true),
            new(nameof(HexLowByte), HexLowByte, true),
            new(nameof(FullMaskMatch), FullMaskMatch, true)
        ];

        public static RuleScenario<(int? left, int? right, int mask)>[] InvalidScenarios =>
        [
            new(nameof(ZeroMask), ZeroMask, false),
            new(nameof(MaskedMismatch), MaskedMismatch, false),
            new(nameof(LeftNull), LeftNull, false),
            new(nameof(RightNull), RightNull, false)
        ];

        public static RuleScenario<(int? left, int? right, int mask)>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class HasAllBits
    {
        public static readonly (int? value, int mask) HasAllBitsSet = (14, 0b0110);
        public static readonly (int? value, int mask) MissingOneBit = (10, 0b0110);
        public static readonly (int? value, int mask) ZeroMask = (14, 0);
        public static readonly (int? value, int mask) ValueNull = (null, 0b0110);

        public static RuleScenario<(int? value, int mask)>[] ValidScenarios =>
        [
            new(nameof(HasAllBitsSet), HasAllBitsSet, true)
        ];

        public static RuleScenario<(int? value, int mask)>[] InvalidScenarios =>
        [
            new(nameof(MissingOneBit), MissingOneBit, false),
            new(nameof(ZeroMask), ZeroMask, false),
            new(nameof(ValueNull), ValueNull, false)
        ];

        public static RuleScenario<(int? value, int mask)>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class HasAnyBits
    {
        public static readonly (int? value, int mask) HasAtLeastOne = (10, 0b0010);
        public static readonly (int? value, int mask) HasNone = (10, 0b0100);
        public static readonly (int? value, int mask) ZeroMask = (10, 0);
        public static readonly (int? value, int mask) ValueNull = (null, 0b0010);

        public static RuleScenario<(int? value, int mask)>[] ValidScenarios =>
        [
            new(nameof(HasAtLeastOne), HasAtLeastOne, true)
        ];

        public static RuleScenario<(int? value, int mask)>[] InvalidScenarios =>
        [
            new(nameof(HasNone), HasNone, false),
            new(nameof(ZeroMask), ZeroMask, false),
            new(nameof(ValueNull), ValueNull, false)
        ];

        public static RuleScenario<(int? value, int mask)>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class HasNoBits
    {
        public static readonly (int? value, int mask) NoBitsSet = (10, 0b0100);
        public static readonly (int? value, int mask) HasSome = (10, 0b0010);
        public static readonly (int? value, int mask) ZeroMask = (10, 0);
        public static readonly (int? value, int mask) ValueNull = (null, 0b0100);

        public static RuleScenario<(int? value, int mask)>[] ValidScenarios =>
        [
            new(nameof(NoBitsSet), NoBitsSet, true)
        ];

        public static RuleScenario<(int? value, int mask)>[] InvalidScenarios =>
        [
            new(nameof(HasSome), HasSome, false),
            new(nameof(ZeroMask), ZeroMask, false),
            new(nameof(ValueNull), ValueNull, false)
        ];

        public static RuleScenario<(int? value, int mask)>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class HasOnlyBits
    {
        public static readonly (int? value, int allowedMask) OnlyAllowed = (3, 0b0111);
        public static readonly (int? value, int allowedMask) DisallowedBit = (8, 0b0111);
        public static readonly (int? value, int allowedMask) ZeroMask = (3, 0);
        public static readonly (int? value, int allowedMask) ValueNull = (null, 0b0111);

        public static RuleScenario<(int? value, int allowedMask)>[] ValidScenarios =>
        [
            new(nameof(OnlyAllowed), OnlyAllowed, true)
        ];

        public static RuleScenario<(int? value, int allowedMask)>[] InvalidScenarios =>
        [
            new(nameof(DisallowedBit), DisallowedBit, false),
            new(nameof(ZeroMask), ZeroMask, false),
            new(nameof(ValueNull), ValueNull, false)
        ];

        public static RuleScenario<(int? value, int allowedMask)>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsPowerOfTwo
    {
        public static readonly int? One = 1;
        public static readonly int? Two = 2;
        public static readonly int? Large = 1024;
        public static readonly int? Zero = 0;
        public static readonly int? Negative = -1;
        public static readonly int? NotPowerOfTwo = 3;
        public static readonly int? ValueNull = null;

        public static RuleScenario<int?>[] ValidScenarios =>
        [
            new(nameof(One), One, true),
            new(nameof(Two), Two, true),
            new(nameof(Large), Large, true)
        ];

        public static RuleScenario<int?>[] InvalidScenarios =>
        [
            new(nameof(Zero), Zero, false),
            new(nameof(Negative), Negative, false),
            new(nameof(NotPowerOfTwo), NotPowerOfTwo, false),
            new(nameof(ValueNull), ValueNull, false)
        ];

        public static RuleScenario<int?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }
}
