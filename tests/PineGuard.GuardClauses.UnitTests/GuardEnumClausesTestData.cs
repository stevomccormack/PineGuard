using PineGuard.Testing.UnitTests.GuardClauses;
using F = PineGuard.Testing.Fixtures.EnumRulesFixtures;

namespace PineGuard.GuardClauses.UnitTests;

#pragma warning disable CS0618
public static class GuardEnumClausesTestData
{
    public static class NotDefined
    {
        public static TheoryData<GuardCase<F.SimpleEnum>> ValidCases =>
        [
            new(nameof(F.IsDefined.DefinedOne), F.SimpleEnum.One, new GuardExpected(true)),
            new(nameof(F.IsDefined.DefinedTwo), F.SimpleEnum.Two, new GuardExpected(true))
        ];
        public static TheoryData<GuardCase<F.SimpleEnum>> InvalidCases =>
        [
            new(nameof(F.IsDefined.Undefined), (F.SimpleEnum)999, new GuardExpected(false, typeof(ArgumentException), "value"))
        ];
    }

    public static class Defined
    {
        public static TheoryData<GuardCase<F.SimpleEnum>> ValidCases =>
        [
            new(nameof(F.IsDefined.Undefined), (F.SimpleEnum)999, new GuardExpected(true))
        ];
        public static TheoryData<GuardCase<F.SimpleEnum>> InvalidCases =>
        [
            new(nameof(F.IsDefined.DefinedOne), F.SimpleEnum.One, new GuardExpected(false, typeof(ArgumentException), "value"))
        ];
    }

    public static class NotDefinedValue
    {
        public static TheoryData<GuardCase<int>> ValidCases =>
        [
            new(nameof(F.IsDefinedValue.ValueOne), 1, new GuardExpected(true))
        ];
        public static TheoryData<GuardCase<int>> InvalidCases =>
        [
            new(nameof(F.IsDefinedValue.Undefined), 999, new GuardExpected(false, typeof(ArgumentException), "value"))
        ];
    }

    public static class DefinedValue
    {
        public static TheoryData<GuardCase<int>> ValidCases =>
        [
            new(nameof(F.IsDefinedValue.Undefined), 999, new GuardExpected(true))
        ];
        public static TheoryData<GuardCase<int>> InvalidCases =>
        [
            new(nameof(F.IsDefinedValue.ValueOne), 1, new GuardExpected(false, typeof(ArgumentException), "value"))
        ];
    }

    public static class NotDefinedName
    {
        public static TheoryData<GuardCase<(string? name, bool ignoreCase)>> ValidCases =>
        [
            new(nameof(F.IsDefinedName.ExactOne), F.IsDefinedName.ExactOne, new GuardExpected(true)),
            new(nameof(F.IsDefinedName.LowerIgnoreCase), F.IsDefinedName.LowerIgnoreCase, new GuardExpected(true))
        ];
        public static TheoryData<GuardCase<(string? name, bool ignoreCase)>> InvalidCases =>
        [
            new(nameof(F.IsDefinedName.NullValue), F.IsDefinedName.NullValue, new GuardExpected(false, typeof(ArgumentNullException), "name")),
            new(nameof(F.IsDefinedName.Missing), F.IsDefinedName.Missing, new GuardExpected(false, typeof(ArgumentException), "name"))
        ];
    }

    public static class DefinedName
    {
        public static TheoryData<GuardCase<(string? name, bool ignoreCase)>> ValidCases =>
        [
            new(nameof(F.IsDefinedName.Missing), F.IsDefinedName.Missing, new GuardExpected(true))
        ];
        public static TheoryData<GuardCase<(string? name, bool ignoreCase)>> InvalidCases =>
        [
            new(nameof(F.IsDefinedName.ExactOne), F.IsDefinedName.ExactOne, new GuardExpected(false, typeof(ArgumentException), "name"))
        ];
    }

    public static class NotFlagsEnumCombination
    {
        public static TheoryData<GuardCase<F.FlagsEnum>> ValidCases =>
        [
            new(nameof(F.IsFlagsEnumCombination.FlagsA), F.FlagsEnum.A, new GuardExpected(true)),
            new(nameof(F.IsFlagsEnumCombination.FlagsAOrB), F.FlagsEnum.A | F.FlagsEnum.B, new GuardExpected(true))
        ];
        public static TheoryData<GuardCase<F.FlagsEnum>> InvalidCases =>
        [
            new(nameof(F.IsFlagsEnumCombination.UndefinedBit), (F.FlagsEnum)8, new GuardExpected(false, typeof(ArgumentException), "value"))
        ];
    }

    public static class FlagsEnumCombination
    {
        public static TheoryData<GuardCase<F.FlagsEnum>> ValidCases =>
        [
            new(nameof(F.IsFlagsEnumCombination.UndefinedBit), (F.FlagsEnum)8, new GuardExpected(true))
        ];
        public static TheoryData<GuardCase<F.FlagsEnum>> InvalidCases =>
        [
            new(nameof(F.IsFlagsEnumCombination.FlagsA), F.FlagsEnum.A, new GuardExpected(false, typeof(ArgumentException), "value"))
        ];
    }

    public static class NotHasAttribute
    {
        public static TheoryData<GuardCase<F.AttributedEnum>> ValidCases =>
        [
            new(nameof(F.HasDescription.WithDescriptionValue), F.AttributedEnum.WithDescription, new GuardExpected(true))
        ];
        public static TheoryData<GuardCase<F.AttributedEnum>> InvalidCases =>
        [
            new(nameof(F.HasDescription.NoneValue), F.AttributedEnum.None, new GuardExpected(false, typeof(ArgumentException), "value"))
        ];
    }

    public static class HasAttribute
    {
        public static TheoryData<GuardCase<F.AttributedEnum>> ValidCases =>
        [
            new(nameof(F.HasDescription.NoneValue), F.AttributedEnum.None, new GuardExpected(true))
        ];
        public static TheoryData<GuardCase<F.AttributedEnum>> InvalidCases =>
        [
            new(nameof(F.HasDescription.WithDescriptionValue), F.AttributedEnum.WithDescription, new GuardExpected(false, typeof(ArgumentException), "value"))
        ];
    }

    public static class NotHasFlag
    {
        public static TheoryData<GuardCase<(F.FlagsEnum value, F.FlagsEnum flag)>> ValidCases =>
        [
            new(nameof(F.HasFlag.ContainsB), (F.FlagsEnum.A | F.FlagsEnum.B, F.FlagsEnum.B), new GuardExpected(true))
        ];
        public static TheoryData<GuardCase<(F.FlagsEnum value, F.FlagsEnum flag)>> InvalidCases =>
        [
            new(nameof(F.HasFlag.MissingFlag), (F.FlagsEnum.A, F.FlagsEnum.B), new GuardExpected(false, typeof(ArgumentException), "value"))
        ];
    }

    public static class HasFlag
    {
        public static TheoryData<GuardCase<(F.FlagsEnum value, F.FlagsEnum flag)>> ValidCases =>
        [
            new(nameof(F.HasFlag.MissingFlag), (F.FlagsEnum.A, F.FlagsEnum.B), new GuardExpected(true))
        ];
        public static TheoryData<GuardCase<(F.FlagsEnum value, F.FlagsEnum flag)>> InvalidCases =>
        [
            new(nameof(F.HasFlag.ContainsB), (F.FlagsEnum.A | F.FlagsEnum.B, F.FlagsEnum.B), new GuardExpected(false, typeof(ArgumentException), "value"))
        ];
    }

    public static class NotHasDescription
    {
        public static TheoryData<GuardCase<F.AttributedEnum>> ValidCases =>
        [
            new(nameof(F.HasDescription.WithDescriptionValue), F.AttributedEnum.WithDescription, new GuardExpected(true))
        ];
        public static TheoryData<GuardCase<F.AttributedEnum>> InvalidCases =>
        [
            new(nameof(F.HasDescription.NoneValue), F.AttributedEnum.None, new GuardExpected(false, typeof(ArgumentException), "value"))
        ];
    }

    public static class HasDescription
    {
        public static TheoryData<GuardCase<F.AttributedEnum>> ValidCases =>
        [
            new(nameof(F.HasDescription.NoneValue), F.AttributedEnum.None, new GuardExpected(true))
        ];
        public static TheoryData<GuardCase<F.AttributedEnum>> InvalidCases =>
        [
            new(nameof(F.HasDescription.WithDescriptionValue), F.AttributedEnum.WithDescription, new GuardExpected(false, typeof(ArgumentException), "value"))
        ];
    }

    public static class NotHasDisplay
    {
        public static TheoryData<GuardCase<F.AttributedEnum>> ValidCases =>
        [
            new(nameof(F.HasDisplay.WithDisplayValue), F.AttributedEnum.WithDisplay, new GuardExpected(true))
        ];
        public static TheoryData<GuardCase<F.AttributedEnum>> InvalidCases =>
        [
            new(nameof(F.HasDisplay.NoneValue), F.AttributedEnum.None, new GuardExpected(false, typeof(ArgumentException), "value"))
        ];
    }

    public static class HasDisplay
    {
        public static TheoryData<GuardCase<F.AttributedEnum>> ValidCases =>
        [
            new(nameof(F.HasDisplay.NoneValue), F.AttributedEnum.None, new GuardExpected(true))
        ];
        public static TheoryData<GuardCase<F.AttributedEnum>> InvalidCases =>
        [
            new(nameof(F.HasDisplay.WithDisplayValue), F.AttributedEnum.WithDisplay, new GuardExpected(false, typeof(ArgumentException), "value"))
        ];
    }

    public static class NotHasEnumMember
    {
        public static TheoryData<GuardCase<F.AttributedEnum>> ValidCases =>
        [
            new(nameof(F.HasEnumMember.WithEnumMemberValue), F.AttributedEnum.WithEnumMember, new GuardExpected(true))
        ];
        public static TheoryData<GuardCase<F.AttributedEnum>> InvalidCases =>
        [
            new(nameof(F.HasEnumMember.NoneValue), F.AttributedEnum.None, new GuardExpected(false, typeof(ArgumentException), "value"))
        ];
    }

    public static class HasEnumMember
    {
        public static TheoryData<GuardCase<F.AttributedEnum>> ValidCases =>
        [
            new(nameof(F.HasEnumMember.NoneValue), F.AttributedEnum.None, new GuardExpected(true))
        ];
        public static TheoryData<GuardCase<F.AttributedEnum>> InvalidCases =>
        [
            new(nameof(F.HasEnumMember.WithEnumMemberValue), F.AttributedEnum.WithEnumMember, new GuardExpected(false, typeof(ArgumentException), "value"))
        ];
    }

    public static class Obsolete
    {
        public static TheoryData<GuardCase<F.AttributedEnum>> ValidCases =>
        [
            new(nameof(F.IsObsolete.NoneValue), F.AttributedEnum.None, new GuardExpected(true))
        ];
        public static TheoryData<GuardCase<F.AttributedEnum>> InvalidCases =>
        [
#pragma warning disable CS0618
            new(nameof(F.IsObsolete.ObsoleteValue), F.AttributedEnum.Obsolete, new GuardExpected(false, typeof(ArgumentException), "value")),
#pragma warning restore CS0618
        ];
    }

    public static class NotObsolete
    {
        public static TheoryData<GuardCase<F.AttributedEnum>> ValidCases =>
        [
#pragma warning disable CS0618
            new(nameof(F.IsObsolete.ObsoleteValue), F.AttributedEnum.Obsolete, new GuardExpected(true)),
#pragma warning restore CS0618
        ];
        public static TheoryData<GuardCase<F.AttributedEnum>> InvalidCases =>
        [
            new(nameof(F.IsObsolete.NoneValue), F.AttributedEnum.None, new GuardExpected(false, typeof(ArgumentException), "value"))
        ];
    }
}
#pragma warning restore CS0618
