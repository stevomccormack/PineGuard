#pragma warning disable CS0618

using PineGuard.Testing.UnitTests.FluentValidation;
using F = PineGuard.Testing.Fixtures.EnumRulesFixtures;

namespace PineGuard.FluentValidation.UnitTests;

public static class FluentEnumExtensionsTestData
{
    public static class Defined
    {
        public static TheoryData<FluentCase<F.SimpleEnum?>> Cases => F.IsDefined.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsDefined.NullValue) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be a defined enum value.")
        });
    }

    public static class NotDefined
    {
        public static TheoryData<FluentCase<F.SimpleEnum?>> Cases => F.IsDefined.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsDefined.NullValue) => new FluentExpected(true),
            _ when !s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must not be a defined enum value.")
        });
    }

    public static class DefinedValue
    {
        public static TheoryData<FluentCase<int?>> Cases => F.IsDefinedValue.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsDefinedValue.NullValue) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be a defined enum backing value.")
        });
    }

    public static class NotDefinedValue
    {
        public static TheoryData<FluentCase<int?>> Cases => F.IsDefinedValue.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsDefinedValue.NullValue) => new FluentExpected(true),
            _ when !s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must not be a defined enum backing value.")
        });
    }

    public static class DefinedName
    {
        public static TheoryData<FluentCase<(string? name, bool ignoreCase)>> Cases => F.IsDefinedName.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsDefinedName.NullValue) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be a defined enum name.")
        });
    }

    public static class NotDefinedName
    {
        public static TheoryData<FluentCase<(string? name, bool ignoreCase)>> Cases => F.IsDefinedName.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsDefinedName.NullValue) => new FluentExpected(true),
            _ when !s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must not be a defined enum name.")
        });
    }

    public static class FlagsEnumCombination
    {
        public static TheoryData<FluentCase<F.FlagsEnum?>> Cases => F.IsFlagsEnumCombination.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsFlagsEnumCombination.NullValue) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be a valid flags enum combination.")
        });
    }

    public static class NotFlagsEnumCombination
    {
        public static TheoryData<FluentCase<F.FlagsEnum?>> Cases => F.IsFlagsEnumCombination.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsFlagsEnumCombination.NullValue) => new FluentExpected(true),
            _ when !s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must not be a valid flags enum combination.")
        });
    }

    public static class HasAttribute
    {
        public static TheoryData<FluentCase<F.AttributedEnum?>> Cases => F.HasDescription.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.HasDescription.NullValue) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must have the expected attribute.")
        });
    }

    public static class NotHasAttribute
    {
        public static TheoryData<FluentCase<F.AttributedEnum?>> Cases => F.HasDescription.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.HasDescription.NullValue) => new FluentExpected(true),
            _ when !s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must not have the expected attribute.")
        });
    }

    public static class HasFlag
    {
        public static TheoryData<FluentCase<(F.FlagsEnum? value, F.FlagsEnum flag)>> Cases => F.HasFlag.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.HasFlag.NullValue) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must have the expected flag.")
        });
    }

    public static class NotHasFlag
    {
        public static TheoryData<FluentCase<(F.FlagsEnum? value, F.FlagsEnum flag)>> Cases => F.HasFlag.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.HasFlag.NullValue) => new FluentExpected(true),
            _ when !s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must not have the expected flag.")
        });
    }

    public static class HasDescription
    {
        public static TheoryData<FluentCase<F.AttributedEnum?>> Cases => F.HasDescription.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.HasDescription.NullValue) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must have a description.")
        });
    }

    public static class NotHasDescription
    {
        public static TheoryData<FluentCase<F.AttributedEnum?>> Cases => F.HasDescription.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.HasDescription.NullValue) => new FluentExpected(true),
            _ when !s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must not have a description.")
        });
    }

    public static class HasDisplay
    {
        public static TheoryData<FluentCase<F.AttributedEnum?>> Cases => F.HasDisplay.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.HasDisplay.NullValue) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must have a display attribute.")
        });
    }

    public static class NotHasDisplay
    {
        public static TheoryData<FluentCase<F.AttributedEnum?>> Cases => F.HasDisplay.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.HasDisplay.NullValue) => new FluentExpected(true),
            _ when !s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must not have a display attribute.")
        });
    }

    public static class HasEnumMember
    {
        public static TheoryData<FluentCase<F.AttributedEnum?>> Cases => F.HasEnumMember.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.HasEnumMember.NullValue) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must have an enum member attribute.")
        });
    }

    public static class NotHasEnumMember
    {
        public static TheoryData<FluentCase<F.AttributedEnum?>> Cases => F.HasEnumMember.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.HasEnumMember.NullValue) => new FluentExpected(true),
            _ when !s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must not have an enum member attribute.")
        });
    }

    public static class Obsolete
    {
        public static TheoryData<FluentCase<F.AttributedEnum?>> Cases => F.IsObsolete.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsObsolete.NullValue) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be obsolete.")
        });
    }

    public static class NotObsolete
    {
        public static TheoryData<FluentCase<F.AttributedEnum?>> Cases => F.IsObsolete.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsObsolete.NullValue) => new FluentExpected(true),
            _ when !s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must not be obsolete.")
        });
    }

    // ── Non-nullable overloads ─────────────────────────────────────────────

    public static class DefinedNonNullable
    {
        public static TheoryData<FluentCase<F.SimpleEnum>> Cases => F.IsDefinedNonNullable.AllScenarios.ToFluentCases(s => s.Name switch
        {
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be a defined enum value.")
        });
    }

    public static class NotDefinedNonNullable
    {
        public static TheoryData<FluentCase<F.SimpleEnum>> Cases => F.IsDefinedNonNullable.AllScenarios.ToFluentCases(s => s.Name switch
        {
            _ when !s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must not be a defined enum value.")
        });
    }

    public static class DefinedValueNonNullable
    {
        public static TheoryData<FluentCase<int>> Cases => F.IsDefinedValueNonNullable.AllScenarios.ToFluentCases(s => s.Name switch
        {
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be a defined enum backing value.")
        });
    }

    public static class NotDefinedValueNonNullable
    {
        public static TheoryData<FluentCase<int>> Cases => F.IsDefinedValueNonNullable.AllScenarios.ToFluentCases(s => s.Name switch
        {
            _ when !s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must not be a defined enum backing value.")
        });
    }

    public static class FlagsEnumCombinationNonNullable
    {
        public static TheoryData<FluentCase<F.FlagsEnum>> Cases => F.IsFlagsEnumCombinationNonNullable.AllScenarios.ToFluentCases(s => s.Name switch
        {
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be a valid flags enum combination.")
        });
    }

    public static class NotFlagsEnumCombinationNonNullable
    {
        public static TheoryData<FluentCase<F.FlagsEnum>> Cases => F.IsFlagsEnumCombinationNonNullable.AllScenarios.ToFluentCases(s => s.Name switch
        {
            _ when !s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must not be a valid flags enum combination.")
        });
    }

    public static class HasAttributeNonNullable
    {
        public static TheoryData<FluentCase<F.AttributedEnum>> Cases => F.HasDescriptionNonNullable.AllScenarios.ToFluentCases(s => s.Name switch
        {
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must have the expected attribute.")
        });
    }

    public static class NotHasAttributeNonNullable
    {
        public static TheoryData<FluentCase<F.AttributedEnum>> Cases => F.HasDescriptionNonNullable.AllScenarios.ToFluentCases(s => s.Name switch
        {
            _ when !s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must not have the expected attribute.")
        });
    }

    public static class HasFlagNonNullable
    {
        public static TheoryData<FluentCase<(F.FlagsEnum value, F.FlagsEnum flag)>> Cases => F.HasFlagNonNullable.AllScenarios.ToFluentCases(s => s.Name switch
        {
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must have the expected flag.")
        });
    }

    public static class NotHasFlagNonNullable
    {
        public static TheoryData<FluentCase<(F.FlagsEnum value, F.FlagsEnum flag)>> Cases => F.HasFlagNonNullable.AllScenarios.ToFluentCases(s => s.Name switch
        {
            _ when !s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must not have the expected flag.")
        });
    }

    public static class HasDescriptionNonNullable
    {
        public static TheoryData<FluentCase<F.AttributedEnum>> Cases => F.HasDescriptionNonNullable.AllScenarios.ToFluentCases(s => s.Name switch
        {
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must have a description.")
        });
    }

    public static class NotHasDescriptionNonNullable
    {
        public static TheoryData<FluentCase<F.AttributedEnum>> Cases => F.HasDescriptionNonNullable.AllScenarios.ToFluentCases(s => s.Name switch
        {
            _ when !s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must not have a description.")
        });
    }

    public static class HasDisplayNonNullable
    {
        public static TheoryData<FluentCase<F.AttributedEnum>> Cases => F.HasDisplayNonNullable.AllScenarios.ToFluentCases(s => s.Name switch
        {
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must have a display attribute.")
        });
    }

    public static class NotHasDisplayNonNullable
    {
        public static TheoryData<FluentCase<F.AttributedEnum>> Cases => F.HasDisplayNonNullable.AllScenarios.ToFluentCases(s => s.Name switch
        {
            _ when !s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must not have a display attribute.")
        });
    }

    public static class HasEnumMemberNonNullable
    {
        public static TheoryData<FluentCase<F.AttributedEnum>> Cases => F.HasEnumMemberNonNullable.AllScenarios.ToFluentCases(s => s.Name switch
        {
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must have an enum member attribute.")
        });
    }

    public static class NotHasEnumMemberNonNullable
    {
        public static TheoryData<FluentCase<F.AttributedEnum>> Cases => F.HasEnumMemberNonNullable.AllScenarios.ToFluentCases(s => s.Name switch
        {
            _ when !s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must not have an enum member attribute.")
        });
    }

    public static class ObsoleteNonNullable
    {
        public static TheoryData<FluentCase<F.AttributedEnum>> Cases => F.IsObsoleteNonNullable.AllScenarios.ToFluentCases(s => s.Name switch
        {
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be obsolete.")
        });
    }

    public static class NotObsoleteNonNullable
    {
        public static TheoryData<FluentCase<F.AttributedEnum>> Cases => F.IsObsoleteNonNullable.AllScenarios.ToFluentCases(s => s.Name switch
        {
            _ when !s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must not be obsolete.")
        });
    }
}

#pragma warning restore CS0618
