using PineGuard.Testing.UnitTests.MustClauses;
using F = PineGuard.Testing.Fixtures.EnumRulesFixtures;

namespace PineGuard.MustClauses.UnitTests;

#pragma warning disable CS0618
public static class MustEnumClausesTestData
{
    public static class Defined
    {
        public static TheoryData<MustCase<F.SimpleEnum>> ValidCases =>
        [
            new(nameof(F.IsDefined.DefinedOne), F.SimpleEnum.One, new MustExpected(true)),
            new(nameof(F.IsDefined.DefinedTwo), F.SimpleEnum.Two, new MustExpected(true))
        ];
        public static TheoryData<MustCase<F.SimpleEnum>> InvalidCases =>
        [
            new(nameof(F.IsDefined.Undefined), (F.SimpleEnum)999, new MustExpected(false, "value must be a defined enum value."))
        ];
    }

    public static class NotDefined
    {
        public static TheoryData<MustCase<F.SimpleEnum>> ValidCases =>
        [
            new(nameof(F.IsDefined.Undefined), (F.SimpleEnum)999, new MustExpected(true))
        ];
        public static TheoryData<MustCase<F.SimpleEnum>> InvalidCases =>
        [
            new(nameof(F.IsDefined.DefinedOne), F.SimpleEnum.One, new MustExpected(false, "value must not be a defined enum value."))
        ];
    }

    public static class DefinedValue
    {
        public static TheoryData<MustCase<int>> ValidCases =>
        [
            new(nameof(F.IsDefinedValue.ValueOne), 1, new MustExpected(true))
        ];
        public static TheoryData<MustCase<int>> InvalidCases =>
        [
            new(nameof(F.IsDefinedValue.Undefined), 999, new MustExpected(false, "value must be a defined enum backing value."))
        ];
    }

    public static class NotDefinedValue
    {
        public static TheoryData<MustCase<int>> ValidCases =>
        [
            new(nameof(F.IsDefinedValue.Undefined), 999, new MustExpected(true))
        ];
        public static TheoryData<MustCase<int>> InvalidCases =>
        [
            new(nameof(F.IsDefinedValue.ValueOne), 1, new MustExpected(false, "value must not be a defined enum backing value."))
        ];
    }

    public static class DefinedName
    {
        public static TheoryData<MustCase<(string? name, bool ignoreCase)>> ValidCases =>
        [
            new(nameof(F.IsDefinedName.ExactOne), F.IsDefinedName.ExactOne, new MustExpected(true)),
            new(nameof(F.IsDefinedName.LowerIgnoreCase), F.IsDefinedName.LowerIgnoreCase, new MustExpected(true))
        ];
        public static TheoryData<MustCase<(string? name, bool ignoreCase)>> InvalidCases =>
        [
            new(nameof(F.IsDefinedName.Missing), F.IsDefinedName.Missing, new MustExpected(false, "name must be a defined enum name."))
        ];
    }

    public static class NotDefinedName
    {
        public static TheoryData<MustCase<(string? name, bool ignoreCase)>> ValidCases =>
        [
            new(nameof(F.IsDefinedName.Missing), F.IsDefinedName.Missing, new MustExpected(true))
        ];
        public static TheoryData<MustCase<(string? name, bool ignoreCase)>> InvalidCases =>
        [
            new(nameof(F.IsDefinedName.ExactOne), F.IsDefinedName.ExactOne, new MustExpected(false, "name must not be a defined enum name."))
        ];
    }

    public static class FlagsEnumCombination
    {
        public static TheoryData<MustCase<F.FlagsEnum>> ValidCases =>
        [
            new(nameof(F.IsFlagsEnumCombination.FlagsA), F.FlagsEnum.A, new MustExpected(true)),
            new(nameof(F.IsFlagsEnumCombination.FlagsAOrB), F.FlagsEnum.A | F.FlagsEnum.B, new MustExpected(true))
        ];
        public static TheoryData<MustCase<F.FlagsEnum>> InvalidCases =>
        [
            new(nameof(F.IsFlagsEnumCombination.UndefinedBit), (F.FlagsEnum)8, new MustExpected(false, "value must be a valid flags enum combination."))
        ];
    }

    public static class NotFlagsEnumCombination
    {
        public static TheoryData<MustCase<F.FlagsEnum>> ValidCases =>
        [
            new(nameof(F.IsFlagsEnumCombination.UndefinedBit), (F.FlagsEnum)8, new MustExpected(true))
        ];
        public static TheoryData<MustCase<F.FlagsEnum>> InvalidCases =>
        [
            new(nameof(F.IsFlagsEnumCombination.FlagsA), F.FlagsEnum.A, new MustExpected(false, "value must not be a valid flags enum combination."))
        ];
    }

    public static class HasAttribute
    {
        public static TheoryData<MustCase<F.AttributedEnum>> ValidCases =>
        [
            new(nameof(F.HasDescription.WithDescriptionValue), F.AttributedEnum.WithDescription, new MustExpected(true))
        ];
        public static TheoryData<MustCase<F.AttributedEnum>> InvalidCases =>
        [
            new(nameof(F.HasDescription.NoneValue), F.AttributedEnum.None, new MustExpected(false, "value must have the expected attribute."))
        ];
    }

    public static class NotHasAttribute
    {
        public static TheoryData<MustCase<F.AttributedEnum>> ValidCases =>
        [
            new(nameof(F.HasDescription.NoneValue), F.AttributedEnum.None, new MustExpected(true))
        ];
        public static TheoryData<MustCase<F.AttributedEnum>> InvalidCases =>
        [
            new(nameof(F.HasDescription.WithDescriptionValue), F.AttributedEnum.WithDescription, new MustExpected(false, "value must not have the expected attribute."))
        ];
    }

    public static class HasFlag
    {
        public static TheoryData<MustCase<(F.FlagsEnum value, F.FlagsEnum flag)>> ValidCases =>
        [
            new(nameof(F.HasFlag.ContainsB), (F.FlagsEnum.A | F.FlagsEnum.B, F.FlagsEnum.B), new MustExpected(true))
        ];
        public static TheoryData<MustCase<(F.FlagsEnum value, F.FlagsEnum flag)>> InvalidCases =>
        [
            new(nameof(F.HasFlag.MissingFlag), (F.FlagsEnum.A, F.FlagsEnum.B), new MustExpected(false, "value must have the expected flag."))
        ];
    }

    public static class NotHasFlag
    {
        public static TheoryData<MustCase<(F.FlagsEnum value, F.FlagsEnum flag)>> ValidCases =>
        [
            new(nameof(F.HasFlag.MissingFlag), (F.FlagsEnum.A, F.FlagsEnum.B), new MustExpected(true))
        ];
        public static TheoryData<MustCase<(F.FlagsEnum value, F.FlagsEnum flag)>> InvalidCases =>
        [
            new(nameof(F.HasFlag.ContainsB), (F.FlagsEnum.A | F.FlagsEnum.B, F.FlagsEnum.B), new MustExpected(false, "value must not have the expected flag."))
        ];
    }

    public static class HasDescription
    {
        public static TheoryData<MustCase<F.AttributedEnum>> ValidCases =>
        [
            new(nameof(F.HasDescription.WithDescriptionValue), F.AttributedEnum.WithDescription, new MustExpected(true))
        ];
        public static TheoryData<MustCase<F.AttributedEnum>> InvalidCases =>
        [
            new(nameof(F.HasDescription.NoneValue), F.AttributedEnum.None, new MustExpected(false, "value must have a description."))
        ];
    }

    public static class NotHasDescription
    {
        public static TheoryData<MustCase<F.AttributedEnum>> ValidCases =>
        [
            new(nameof(F.HasDescription.NoneValue), F.AttributedEnum.None, new MustExpected(true))
        ];
        public static TheoryData<MustCase<F.AttributedEnum>> InvalidCases =>
        [
            new(nameof(F.HasDescription.WithDescriptionValue), F.AttributedEnum.WithDescription, new MustExpected(false, "value must not have a description."))
        ];
    }

    public static class HasDisplay
    {
        public static TheoryData<MustCase<F.AttributedEnum>> ValidCases =>
        [
            new(nameof(F.HasDisplay.WithDisplayValue), F.AttributedEnum.WithDisplay, new MustExpected(true))
        ];
        public static TheoryData<MustCase<F.AttributedEnum>> InvalidCases =>
        [
            new(nameof(F.HasDisplay.NoneValue), F.AttributedEnum.None, new MustExpected(false, "value must have a display attribute."))
        ];
    }

    public static class NotHasDisplay
    {
        public static TheoryData<MustCase<F.AttributedEnum>> ValidCases =>
        [
            new(nameof(F.HasDisplay.NoneValue), F.AttributedEnum.None, new MustExpected(true))
        ];
        public static TheoryData<MustCase<F.AttributedEnum>> InvalidCases =>
        [
            new(nameof(F.HasDisplay.WithDisplayValue), F.AttributedEnum.WithDisplay, new MustExpected(false, "value must not have a display attribute."))
        ];
    }

    public static class HasEnumMember
    {
        public static TheoryData<MustCase<F.AttributedEnum>> ValidCases =>
        [
            new(nameof(F.HasEnumMember.WithEnumMemberValue), F.AttributedEnum.WithEnumMember, new MustExpected(true))
        ];
        public static TheoryData<MustCase<F.AttributedEnum>> InvalidCases =>
        [
            new(nameof(F.HasEnumMember.NoneValue), F.AttributedEnum.None, new MustExpected(false, "value must have an enum member attribute."))
        ];
    }

    public static class NotHasEnumMember
    {
        public static TheoryData<MustCase<F.AttributedEnum>> ValidCases =>
        [
            new(nameof(F.HasEnumMember.NoneValue), F.AttributedEnum.None, new MustExpected(true))
        ];
        public static TheoryData<MustCase<F.AttributedEnum>> InvalidCases =>
        [
            new(nameof(F.HasEnumMember.WithEnumMemberValue), F.AttributedEnum.WithEnumMember, new MustExpected(false, "value must not have an enum member attribute."))
        ];
    }

    public static class Obsolete
    {
        public static TheoryData<MustCase<F.AttributedEnum>> ValidCases =>
        [
#pragma warning disable CS0618
            new(nameof(F.IsObsolete.ObsoleteValue), F.AttributedEnum.Obsolete, new MustExpected(true)),
#pragma warning restore CS0618
        ];
        public static TheoryData<MustCase<F.AttributedEnum>> InvalidCases =>
        [
            new(nameof(F.IsObsolete.NoneValue), F.AttributedEnum.None, new MustExpected(false, "value must be obsolete."))
        ];
    }

    public static class NotObsolete
    {
        public static TheoryData<MustCase<F.AttributedEnum>> ValidCases =>
        [
            new(nameof(F.IsObsolete.NoneValue), F.AttributedEnum.None, new MustExpected(true))
        ];
        public static TheoryData<MustCase<F.AttributedEnum>> InvalidCases =>
        [
#pragma warning disable CS0618
            new(nameof(F.IsObsolete.ObsoleteValue), F.AttributedEnum.Obsolete, new MustExpected(false, "value must not be obsolete.")),
#pragma warning restore CS0618
        ];
    }
}
#pragma warning restore CS0618
