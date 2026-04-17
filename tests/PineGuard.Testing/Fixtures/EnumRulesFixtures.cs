using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using PineGuard.Testing.UnitTests.Rules;

namespace PineGuard.Testing.Fixtures;

public static class EnumRulesFixtures
{
    public enum SimpleEnum { One = 1, Two = 2, Three = 3 }

    [Flags]
    public enum FlagsEnum { None = 0, A = 1, B = 2, C = 4 }

    public enum AttributedEnum
    {
        None = 0,
        [Description("desc")] WithDescription = 1,
        [Display(Name = "display")] WithDisplay = 2,
        [EnumMember(Value = "member")] WithEnumMember = 3,
#pragma warning disable CS0618
        [Obsolete("Deprecated for testing purposes.")] Obsolete = 4
#pragma warning restore CS0618
    }

    public static class IsDefined
    {
        public static readonly SimpleEnum? DefinedOne = SimpleEnum.One;
        public static readonly SimpleEnum? DefinedTwo = SimpleEnum.Two;
        public static readonly SimpleEnum? DefinedThree = SimpleEnum.Three;
        public static readonly SimpleEnum? NullValue = null;
        public static readonly SimpleEnum? Undefined = (SimpleEnum)999;

        public static RuleScenario<SimpleEnum?>[] ValidScenarios =>
        [
            new(nameof(DefinedOne), DefinedOne, true),
            new(nameof(DefinedTwo), DefinedTwo, true),
            new(nameof(DefinedThree), DefinedThree, true)
        ];

        public static RuleScenario<SimpleEnum?>[] InvalidScenarios =>
        [
            new(nameof(NullValue), NullValue, false),
            new(nameof(Undefined), Undefined, false)
        ];

        public static RuleScenario<SimpleEnum?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsDefinedNonNullable
    {
        public static readonly SimpleEnum DefinedOne = SimpleEnum.One;
        public static readonly SimpleEnum DefinedTwo = SimpleEnum.Two;
        public static readonly SimpleEnum Undefined = (SimpleEnum)999;

        public static RuleScenario<SimpleEnum>[] ValidScenarios => [new(nameof(DefinedOne), DefinedOne, true), new(nameof(DefinedTwo), DefinedTwo, true)];
        public static RuleScenario<SimpleEnum>[] InvalidScenarios => [new(nameof(Undefined), Undefined, false)];
        public static RuleScenario<SimpleEnum>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsDefinedValue
    {
        public static readonly int? ValueOne = 1;
        public static readonly int? ValueTwo = 2;
        public static readonly int? NullValue = null;
        public static readonly int? Zero = 0;
        public static readonly int? Undefined = 999;

        public static RuleScenario<int?>[] ValidScenarios =>
        [
            new(nameof(ValueOne), ValueOne, true),
            new(nameof(ValueTwo), ValueTwo, true)
        ];

        public static RuleScenario<int?>[] InvalidScenarios =>
        [
            new(nameof(NullValue), NullValue, false),
            new(nameof(Zero), Zero, false),
            new(nameof(Undefined), Undefined, false)
        ];

        public static RuleScenario<int?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsDefinedValueNonNullable
    {
        public static readonly int ValueOne = 1;
        public static readonly int ValueTwo = 2;
        public static readonly int Zero = 0;
        public static readonly int Undefined = 999;

        public static RuleScenario<int>[] ValidScenarios => [new(nameof(ValueOne), ValueOne, true), new(nameof(ValueTwo), ValueTwo, true)];
        public static RuleScenario<int>[] InvalidScenarios => [new(nameof(Zero), Zero, false), new(nameof(Undefined), Undefined, false)];
        public static RuleScenario<int>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsDefinedName
    {
        public static readonly (string? name, bool ignoreCase) ExactOne = ("One", true);
        public static readonly (string? name, bool ignoreCase) TrimmedLower = (" one ", true);
        public static readonly (string? name, bool ignoreCase) LowerIgnoreCase = ("one", true);
        public static readonly (string? name, bool ignoreCase) TwoCaseSensitive = ("Two", false);
        public static readonly (string? name, bool ignoreCase) NullValue = (null, true);
        public static readonly (string? name, bool ignoreCase) Whitespace = (" ", true);
        public static readonly (string? name, bool ignoreCase) LowerCaseSensitive = ("one", false);
        public static readonly (string? name, bool ignoreCase) Missing = ("Missing", true);

        public static RuleScenario<(string? name, bool ignoreCase)>[] ValidScenarios =>
        [
            new(nameof(ExactOne), ExactOne, true),
            new(nameof(TrimmedLower), TrimmedLower, true),
            new(nameof(LowerIgnoreCase), LowerIgnoreCase, true),
            new(nameof(TwoCaseSensitive), TwoCaseSensitive, true)
        ];

        public static RuleScenario<(string? name, bool ignoreCase)>[] InvalidScenarios =>
        [
            new(nameof(NullValue), NullValue, false),
            new(nameof(Whitespace), Whitespace, false),
            new(nameof(LowerCaseSensitive), LowerCaseSensitive, false),
            new(nameof(Missing), Missing, false)
        ];

        public static RuleScenario<(string? name, bool ignoreCase)>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsFlagsEnumCombination
    {
        public static readonly FlagsEnum? FlagsNone = FlagsEnum.None;
        public static readonly FlagsEnum? FlagsA = FlagsEnum.A;
        public static readonly FlagsEnum? FlagsAOrB = FlagsEnum.A | FlagsEnum.B;
        public static readonly FlagsEnum? FlagsC = FlagsEnum.C;
        public static readonly FlagsEnum? NullValue = null;
        public static readonly FlagsEnum? UndefinedBit = (FlagsEnum)8;
        public static readonly FlagsEnum? UndefinedMix = (FlagsEnum)9;

        public static RuleScenario<FlagsEnum?>[] ValidScenarios =>
        [
            new(nameof(FlagsNone), FlagsNone, true),
            new(nameof(FlagsA), FlagsA, true),
            new(nameof(FlagsAOrB), FlagsAOrB, true),
            new(nameof(FlagsC), FlagsC, true)
        ];

        public static RuleScenario<FlagsEnum?>[] InvalidScenarios =>
        [
            new(nameof(NullValue), NullValue, false),
            new(nameof(UndefinedBit), UndefinedBit, false),
            new(nameof(UndefinedMix), UndefinedMix, false)
        ];

        public static RuleScenario<FlagsEnum?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsFlagsEnumCombinationNonNullable
    {
        public static readonly FlagsEnum FlagsA = FlagsEnum.A;
        public static readonly FlagsEnum FlagsAOrB = FlagsEnum.A | FlagsEnum.B;
        public static readonly FlagsEnum UndefinedBit = (FlagsEnum)8;

        public static RuleScenario<FlagsEnum>[] ValidScenarios => [new(nameof(FlagsA), FlagsA, true), new(nameof(FlagsAOrB), FlagsAOrB, true)];
        public static RuleScenario<FlagsEnum>[] InvalidScenarios => [new(nameof(UndefinedBit), UndefinedBit, false)];
        public static RuleScenario<FlagsEnum>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsFlagsEnumCombinationNonFlags
    {
        public static readonly SimpleEnum? DefinedOne = SimpleEnum.One;
        public static readonly SimpleEnum? Undefined = (SimpleEnum)999;

        public static RuleScenario<SimpleEnum?>[] ValidScenarios =>
        [
            new(nameof(DefinedOne), DefinedOne, true)
        ];

        public static RuleScenario<SimpleEnum?>[] InvalidScenarios =>
        [
            new(nameof(Undefined), Undefined, false)
        ];

        public static RuleScenario<SimpleEnum?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class HasFlag
    {
        public static readonly (FlagsEnum? value, FlagsEnum flag) ContainsB = (FlagsEnum.A | FlagsEnum.B, FlagsEnum.B);
        public static readonly (FlagsEnum? value, FlagsEnum flag) NullValue = (null, FlagsEnum.A);
        public static readonly (FlagsEnum? value, FlagsEnum flag) MissingFlag = (FlagsEnum.A, FlagsEnum.B);

        public static RuleScenario<(FlagsEnum? value, FlagsEnum flag)>[] ValidScenarios =>
        [
            new(nameof(ContainsB), ContainsB, true)
        ];

        public static RuleScenario<(FlagsEnum? value, FlagsEnum flag)>[] InvalidScenarios =>
        [
            new(nameof(NullValue), NullValue, false),
            new(nameof(MissingFlag), MissingFlag, false)
        ];

        public static RuleScenario<(FlagsEnum? value, FlagsEnum flag)>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class HasFlagNonNullable
    {
        public static readonly (FlagsEnum value, FlagsEnum flag) ContainsB = (FlagsEnum.A | FlagsEnum.B, FlagsEnum.B);
        public static readonly (FlagsEnum value, FlagsEnum flag) MissingFlag = (FlagsEnum.A, FlagsEnum.B);

        public static RuleScenario<(FlagsEnum value, FlagsEnum flag)>[] ValidScenarios => [new(nameof(ContainsB), ContainsB, true)];
        public static RuleScenario<(FlagsEnum value, FlagsEnum flag)>[] InvalidScenarios => [new(nameof(MissingFlag), MissingFlag, false)];
        public static RuleScenario<(FlagsEnum value, FlagsEnum flag)>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class HasDescription
    {
        public static readonly AttributedEnum? WithDescriptionValue = AttributedEnum.WithDescription;
        public static readonly AttributedEnum? NullValue = null;
        public static readonly AttributedEnum? NoneValue = AttributedEnum.None;
        public static readonly AttributedEnum? Undefined = (AttributedEnum)999;

        public static RuleScenario<AttributedEnum?>[] ValidScenarios =>
        [
            new(nameof(WithDescriptionValue), WithDescriptionValue, true)
        ];

        public static RuleScenario<AttributedEnum?>[] InvalidScenarios =>
        [
            new(nameof(NullValue), NullValue, false),
            new(nameof(NoneValue), NoneValue, false),
            new(nameof(Undefined), Undefined, false)
        ];

        public static RuleScenario<AttributedEnum?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class HasDescriptionNonNullable
    {
        public static readonly AttributedEnum WithDescriptionValue = AttributedEnum.WithDescription;
        public static readonly AttributedEnum NoneValue = AttributedEnum.None;
        public static readonly AttributedEnum Undefined = (AttributedEnum)999;

        public static RuleScenario<AttributedEnum>[] ValidScenarios => [new(nameof(WithDescriptionValue), WithDescriptionValue, true)];
        public static RuleScenario<AttributedEnum>[] InvalidScenarios => [new(nameof(NoneValue), NoneValue, false), new(nameof(Undefined), Undefined, false)];
        public static RuleScenario<AttributedEnum>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class HasDisplay
    {
        public static readonly AttributedEnum? WithDisplayValue = AttributedEnum.WithDisplay;
        public static readonly AttributedEnum? NullValue = null;
        public static readonly AttributedEnum? NoneValue = AttributedEnum.None;
        public static readonly AttributedEnum? Undefined = (AttributedEnum)999;

        public static RuleScenario<AttributedEnum?>[] ValidScenarios =>
        [
            new(nameof(WithDisplayValue), WithDisplayValue, true)
        ];

        public static RuleScenario<AttributedEnum?>[] InvalidScenarios =>
        [
            new(nameof(NullValue), NullValue, false),
            new(nameof(NoneValue), NoneValue, false),
            new(nameof(Undefined), Undefined, false)
        ];

        public static RuleScenario<AttributedEnum?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class HasDisplayNonNullable
    {
        public static readonly AttributedEnum WithDisplayValue = AttributedEnum.WithDisplay;
        public static readonly AttributedEnum NoneValue = AttributedEnum.None;
        public static readonly AttributedEnum Undefined = (AttributedEnum)999;

        public static RuleScenario<AttributedEnum>[] ValidScenarios => [new(nameof(WithDisplayValue), WithDisplayValue, true)];
        public static RuleScenario<AttributedEnum>[] InvalidScenarios => [new(nameof(NoneValue), NoneValue, false), new(nameof(Undefined), Undefined, false)];
        public static RuleScenario<AttributedEnum>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class HasEnumMember
    {
        public static readonly AttributedEnum? WithEnumMemberValue = AttributedEnum.WithEnumMember;
        public static readonly AttributedEnum? NullValue = null;
        public static readonly AttributedEnum? NoneValue = AttributedEnum.None;
        public static readonly AttributedEnum? Undefined = (AttributedEnum)999;

        public static RuleScenario<AttributedEnum?>[] ValidScenarios =>
        [
            new(nameof(WithEnumMemberValue), WithEnumMemberValue, true)
        ];

        public static RuleScenario<AttributedEnum?>[] InvalidScenarios =>
        [
            new(nameof(NullValue), NullValue, false),
            new(nameof(NoneValue), NoneValue, false),
            new(nameof(Undefined), Undefined, false)
        ];

        public static RuleScenario<AttributedEnum?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class HasEnumMemberNonNullable
    {
        public static readonly AttributedEnum WithEnumMemberValue = AttributedEnum.WithEnumMember;
        public static readonly AttributedEnum NoneValue = AttributedEnum.None;
        public static readonly AttributedEnum Undefined = (AttributedEnum)999;

        public static RuleScenario<AttributedEnum>[] ValidScenarios => [new(nameof(WithEnumMemberValue), WithEnumMemberValue, true)];
        public static RuleScenario<AttributedEnum>[] InvalidScenarios => [new(nameof(NoneValue), NoneValue, false), new(nameof(Undefined), Undefined, false)];
        public static RuleScenario<AttributedEnum>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsObsolete
    {
#pragma warning disable CS0618
        public static readonly AttributedEnum? ObsoleteValue = AttributedEnum.Obsolete;
#pragma warning restore CS0618
        public static readonly AttributedEnum? NullValue = null;
        public static readonly AttributedEnum? NoneValue = AttributedEnum.None;
        public static readonly AttributedEnum? Undefined = (AttributedEnum)999;

        public static RuleScenario<AttributedEnum?>[] ValidScenarios =>
        [
#pragma warning disable CS0618
            new(nameof(ObsoleteValue), ObsoleteValue, true),
#pragma warning restore CS0618
        ];

        public static RuleScenario<AttributedEnum?>[] InvalidScenarios =>
        [
            new(nameof(NullValue), NullValue, false),
            new(nameof(NoneValue), NoneValue, false),
            new(nameof(Undefined), Undefined, false)
        ];

        public static RuleScenario<AttributedEnum?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsObsoleteNonNullable
    {
#pragma warning disable CS0618
        public static readonly AttributedEnum ObsoleteValue = AttributedEnum.Obsolete;
#pragma warning restore CS0618
        public static readonly AttributedEnum NoneValue = AttributedEnum.None;
        public static readonly AttributedEnum Undefined = (AttributedEnum)999;

#pragma warning disable CS0618
        public static RuleScenario<AttributedEnum>[] ValidScenarios => [new(nameof(ObsoleteValue), ObsoleteValue, true)];
#pragma warning restore CS0618
        public static RuleScenario<AttributedEnum>[] InvalidScenarios => [new(nameof(NoneValue), NoneValue, false), new(nameof(Undefined), Undefined, false)];
        public static RuleScenario<AttributedEnum>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }
}
