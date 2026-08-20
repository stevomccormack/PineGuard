using PineGuard.Testing.UnitTests.Rules;
using F = PineGuard.Testing.Fixtures.EnumRulesFixtures;

namespace PineGuard.Core.UnitTests.Rules;

public static class EnumRulesTestData
{
    // Flags enums covering every underlying integral type, so the bit-pattern reinterpretation in
    // EnumRules.ToUInt64 is exercised for both the signed (sign-extending) and unsigned arms.

    [Flags]
    public enum SByteFlags : sbyte { None = 0, A = 1, B = 2 }

    [Flags]
    public enum Int16Flags : short { None = 0, A = 1, B = 2 }

    [Flags]
    public enum Int32Flags { None = 0, A = 1, B = 2 }

    [Flags]
    public enum Int64Flags : long { None = 0, A = 1, B = 2 }

    [Flags]
    public enum ByteFlags : byte { None = 0, A = 1, B = 2 }

    [Flags]
    public enum UInt16Flags : ushort { None = 0, A = 1, B = 2 }

    [Flags]
    public enum UInt32Flags : uint { None = 0, A = 1, B = 2 }

    [Flags]
    public enum UInt64Flags : ulong { None = 0, A = 1, B = 2 }

    public static class IsFlagsEnumCombinationUnderlyingTypes
    {
        public static TheoryData<TypeCode> Cases =>
        [
            TypeCode.SByte,
            TypeCode.Int16,
            TypeCode.Int32,
            TypeCode.Int64,
            TypeCode.Byte,
            TypeCode.UInt16,
            TypeCode.UInt32,
            TypeCode.UInt64
        ];
    }

    public static class IsDefined
    {
        public static TheoryData<RuleCase<F.SimpleEnum?>> Cases => F.IsDefined.AllScenarios.ToRuleCases();
    }

    public static class IsDefinedValue
    {
        public static TheoryData<RuleCase<int?>> Cases => F.IsDefinedValue.AllScenarios.ToRuleCases();
    }

    public static class IsDefinedValueByteBacked
    {
        public static TheoryData<RuleCase<int?>> Cases => F.IsDefinedValueByteBacked.AllScenarios.ToRuleCases();
    }

    public static class IsDefinedName
    {
        public static TheoryData<RuleCase<(string? name, bool ignoreCase)>> Cases => F.IsDefinedName.AllScenarios.ToRuleCases();
    }

    public static class IsFlagsEnumCombination
    {
        public static TheoryData<RuleCase<F.FlagsEnum?>> Cases => F.IsFlagsEnumCombination.AllScenarios.ToRuleCases();
    }

    public static class IsFlagsEnumCombinationNonFlags
    {
        public static TheoryData<RuleCase<F.SimpleEnum?>> Cases => F.IsFlagsEnumCombinationNonFlags.AllScenarios.ToRuleCases();
    }

    public static class IsFlagsEnumCombinationNegativeMember
    {
        public static TheoryData<RuleCase<F.SignedFlagsEnum?>> Cases => F.IsFlagsEnumCombinationNegativeMember.AllScenarios.ToRuleCases();
    }

    public static class HasFlag
    {
        public static TheoryData<RuleCase<(F.FlagsEnum? value, F.FlagsEnum flag)>> Cases => F.HasFlag.AllScenarios.ToRuleCases();
    }

    public static class HasDescription
    {
        public static TheoryData<RuleCase<F.AttributedEnum?>> Cases => F.HasDescription.AllScenarios.ToRuleCases();
    }

    public static class HasDisplay
    {
        public static TheoryData<RuleCase<F.AttributedEnum?>> Cases => F.HasDisplay.AllScenarios.ToRuleCases();
    }

    public static class HasEnumMember
    {
        public static TheoryData<RuleCase<F.AttributedEnum?>> Cases => F.HasEnumMember.AllScenarios.ToRuleCases();
    }

    public static class IsObsolete
    {
        public static TheoryData<RuleCase<F.AttributedEnum?>> Cases => F.IsObsolete.AllScenarios.ToRuleCases();
    }

    public static class IsFlagsEnum
    {
        public static TheoryData<bool> Cases => [true, false];
    }
}
