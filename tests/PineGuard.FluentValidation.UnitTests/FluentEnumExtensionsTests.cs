#pragma warning disable CS0618

using System.ComponentModel;
using FluentValidation;
using PineGuard.Testing.Fixtures;
using PineGuard.Testing.UnitTests.FluentValidation;
using Xunit.Abstractions;

namespace PineGuard.FluentValidation.UnitTests;

public sealed class FluentEnumExtensionsTests(ITestOutputHelper output) : BaseFluentUnitTest(output)
{
    private sealed record SimpleEnumModel { public EnumRulesFixtures.SimpleEnum? Value { get; init; } }
    private sealed record IntModel { public int? Value { get; init; } }
    private sealed record StringModel { public string? Value { get; init; } }
    private sealed record FlagsEnumModel { public EnumRulesFixtures.FlagsEnum? Value { get; init; } }
    private sealed record AttributedEnumModel { public EnumRulesFixtures.AttributedEnum? Value { get; init; } }

    private sealed class DefinedValidator : AbstractValidator<SimpleEnumModel>
    {
        public DefinedValidator() => RuleFor(x => x.Value).Defined();
    }

    private sealed class NotDefinedValidator : AbstractValidator<SimpleEnumModel>
    {
        public NotDefinedValidator() => RuleFor(x => x.Value).NotDefined();
    }

    private sealed class DefinedValueValidator : AbstractValidator<IntModel>
    {
        public DefinedValueValidator() => RuleFor(x => x.Value).DefinedValue<IntModel, EnumRulesFixtures.SimpleEnum>();
    }

    private sealed class NotDefinedValueValidator : AbstractValidator<IntModel>
    {
        public NotDefinedValueValidator() => RuleFor(x => x.Value).NotDefinedValue<IntModel, EnumRulesFixtures.SimpleEnum>();
    }

    private sealed class DefinedNameValidator : AbstractValidator<StringModel>
    {
        public DefinedNameValidator(bool ignoreCase) => RuleFor(x => x.Value).DefinedName<StringModel, EnumRulesFixtures.SimpleEnum>(ignoreCase);
    }

    private sealed class NotDefinedNameValidator : AbstractValidator<StringModel>
    {
        public NotDefinedNameValidator(bool ignoreCase) => RuleFor(x => x.Value).NotDefinedName<StringModel, EnumRulesFixtures.SimpleEnum>(ignoreCase);
    }

    private sealed class FlagsEnumCombinationValidator : AbstractValidator<FlagsEnumModel>
    {
        public FlagsEnumCombinationValidator() => RuleFor(x => x.Value).FlagsEnumCombination();
    }

    private sealed class NotFlagsEnumCombinationValidator : AbstractValidator<FlagsEnumModel>
    {
        public NotFlagsEnumCombinationValidator() => RuleFor(x => x.Value).NotFlagsEnumCombination();
    }

    private sealed class HasAttributeValidator : AbstractValidator<AttributedEnumModel>
    {
        public HasAttributeValidator() => RuleFor(x => x.Value).HasAttribute<AttributedEnumModel, EnumRulesFixtures.AttributedEnum, DescriptionAttribute>();
    }

    private sealed class NotHasAttributeValidator : AbstractValidator<AttributedEnumModel>
    {
        public NotHasAttributeValidator() => RuleFor(x => x.Value).NotHasAttribute<AttributedEnumModel, EnumRulesFixtures.AttributedEnum, DescriptionAttribute>();
    }

    private sealed class HasFlagValidator : AbstractValidator<FlagsEnumModel>
    {
        public HasFlagValidator(EnumRulesFixtures.FlagsEnum flag) => RuleFor(x => x.Value).HasFlag(flag);
    }

    private sealed class NotHasFlagValidator : AbstractValidator<FlagsEnumModel>
    {
        public NotHasFlagValidator(EnumRulesFixtures.FlagsEnum flag) => RuleFor(x => x.Value).NotHasFlag(flag);
    }

    private sealed class HasDescriptionValidator : AbstractValidator<AttributedEnumModel>
    {
        public HasDescriptionValidator() => RuleFor(x => x.Value).HasDescription();
    }

    private sealed class NotHasDescriptionValidator : AbstractValidator<AttributedEnumModel>
    {
        public NotHasDescriptionValidator() => RuleFor(x => x.Value).NotHasDescription();
    }

    private sealed class HasDisplayValidator : AbstractValidator<AttributedEnumModel>
    {
        public HasDisplayValidator() => RuleFor(x => x.Value).HasDisplay();
    }

    private sealed class NotHasDisplayValidator : AbstractValidator<AttributedEnumModel>
    {
        public NotHasDisplayValidator() => RuleFor(x => x.Value).NotHasDisplay();
    }

    private sealed class HasEnumMemberValidator : AbstractValidator<AttributedEnumModel>
    {
        public HasEnumMemberValidator() => RuleFor(x => x.Value).HasEnumMember();
    }

    private sealed class NotHasEnumMemberValidator : AbstractValidator<AttributedEnumModel>
    {
        public NotHasEnumMemberValidator() => RuleFor(x => x.Value).NotHasEnumMember();
    }

    private sealed class ObsoleteValidator : AbstractValidator<AttributedEnumModel>
    {
        public ObsoleteValidator() => RuleFor(x => x.Value).Obsolete();
    }

    private sealed class NotObsoleteValidator : AbstractValidator<AttributedEnumModel>
    {
        public NotObsoleteValidator() => RuleFor(x => x.Value).NotObsolete();
    }

    [Theory]
    [MemberData(nameof(FluentEnumExtensionsTestData.Defined.Cases), MemberType = typeof(FluentEnumExtensionsTestData.Defined))]
    public void Defined_BehavesAsExpected(FluentCase<EnumRulesFixtures.SimpleEnum?> tc)
    {
        var result = new DefinedValidator().Validate(new SimpleEnumModel { Value = tc.Value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentEnumExtensionsTestData.NotDefined.Cases), MemberType = typeof(FluentEnumExtensionsTestData.NotDefined))]
    public void NotDefined_BehavesAsExpected(FluentCase<EnumRulesFixtures.SimpleEnum?> tc)
    {
        var result = new NotDefinedValidator().Validate(new SimpleEnumModel { Value = tc.Value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentEnumExtensionsTestData.DefinedValue.Cases), MemberType = typeof(FluentEnumExtensionsTestData.DefinedValue))]
    public void DefinedValue_BehavesAsExpected(FluentCase<int?> tc)
    {
        var result = new DefinedValueValidator().Validate(new IntModel { Value = tc.Value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentEnumExtensionsTestData.NotDefinedValue.Cases), MemberType = typeof(FluentEnumExtensionsTestData.NotDefinedValue))]
    public void NotDefinedValue_BehavesAsExpected(FluentCase<int?> tc)
    {
        var result = new NotDefinedValueValidator().Validate(new IntModel { Value = tc.Value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentEnumExtensionsTestData.DefinedName.Cases), MemberType = typeof(FluentEnumExtensionsTestData.DefinedName))]
    public void DefinedName_BehavesAsExpected(FluentCase<(string? name, bool ignoreCase)> tc)
    {
        var result = new DefinedNameValidator(tc.Value.ignoreCase).Validate(new StringModel { Value = tc.Value.name });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentEnumExtensionsTestData.NotDefinedName.Cases), MemberType = typeof(FluentEnumExtensionsTestData.NotDefinedName))]
    public void NotDefinedName_BehavesAsExpected(FluentCase<(string? name, bool ignoreCase)> tc)
    {
        var result = new NotDefinedNameValidator(tc.Value.ignoreCase).Validate(new StringModel { Value = tc.Value.name });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentEnumExtensionsTestData.FlagsEnumCombination.Cases), MemberType = typeof(FluentEnumExtensionsTestData.FlagsEnumCombination))]
    public void FlagsEnumCombination_BehavesAsExpected(FluentCase<EnumRulesFixtures.FlagsEnum?> tc)
    {
        var result = new FlagsEnumCombinationValidator().Validate(new FlagsEnumModel { Value = tc.Value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentEnumExtensionsTestData.NotFlagsEnumCombination.Cases), MemberType = typeof(FluentEnumExtensionsTestData.NotFlagsEnumCombination))]
    public void NotFlagsEnumCombination_BehavesAsExpected(FluentCase<EnumRulesFixtures.FlagsEnum?> tc)
    {
        var result = new NotFlagsEnumCombinationValidator().Validate(new FlagsEnumModel { Value = tc.Value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentEnumExtensionsTestData.HasAttribute.Cases), MemberType = typeof(FluentEnumExtensionsTestData.HasAttribute))]
    public void HasAttribute_BehavesAsExpected(FluentCase<EnumRulesFixtures.AttributedEnum?> tc)
    {
        var result = new HasAttributeValidator().Validate(new AttributedEnumModel { Value = tc.Value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentEnumExtensionsTestData.NotHasAttribute.Cases), MemberType = typeof(FluentEnumExtensionsTestData.NotHasAttribute))]
    public void NotHasAttribute_BehavesAsExpected(FluentCase<EnumRulesFixtures.AttributedEnum?> tc)
    {
        var result = new NotHasAttributeValidator().Validate(new AttributedEnumModel { Value = tc.Value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentEnumExtensionsTestData.HasFlag.Cases), MemberType = typeof(FluentEnumExtensionsTestData.HasFlag))]
    public void HasFlag_BehavesAsExpected(FluentCase<(EnumRulesFixtures.FlagsEnum? value, EnumRulesFixtures.FlagsEnum flag)> tc)
    {
        var result = new HasFlagValidator(tc.Value.flag).Validate(new FlagsEnumModel { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentEnumExtensionsTestData.NotHasFlag.Cases), MemberType = typeof(FluentEnumExtensionsTestData.NotHasFlag))]
    public void NotHasFlag_BehavesAsExpected(FluentCase<(EnumRulesFixtures.FlagsEnum? value, EnumRulesFixtures.FlagsEnum flag)> tc)
    {
        var result = new NotHasFlagValidator(tc.Value.flag).Validate(new FlagsEnumModel { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentEnumExtensionsTestData.HasDescription.Cases), MemberType = typeof(FluentEnumExtensionsTestData.HasDescription))]
    public void HasDescription_BehavesAsExpected(FluentCase<EnumRulesFixtures.AttributedEnum?> tc)
    {
        var result = new HasDescriptionValidator().Validate(new AttributedEnumModel { Value = tc.Value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentEnumExtensionsTestData.NotHasDescription.Cases), MemberType = typeof(FluentEnumExtensionsTestData.NotHasDescription))]
    public void NotHasDescription_BehavesAsExpected(FluentCase<EnumRulesFixtures.AttributedEnum?> tc)
    {
        var result = new NotHasDescriptionValidator().Validate(new AttributedEnumModel { Value = tc.Value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentEnumExtensionsTestData.HasDisplay.Cases), MemberType = typeof(FluentEnumExtensionsTestData.HasDisplay))]
    public void HasDisplay_BehavesAsExpected(FluentCase<EnumRulesFixtures.AttributedEnum?> tc)
    {
        var result = new HasDisplayValidator().Validate(new AttributedEnumModel { Value = tc.Value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentEnumExtensionsTestData.NotHasDisplay.Cases), MemberType = typeof(FluentEnumExtensionsTestData.NotHasDisplay))]
    public void NotHasDisplay_BehavesAsExpected(FluentCase<EnumRulesFixtures.AttributedEnum?> tc)
    {
        var result = new NotHasDisplayValidator().Validate(new AttributedEnumModel { Value = tc.Value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentEnumExtensionsTestData.HasEnumMember.Cases), MemberType = typeof(FluentEnumExtensionsTestData.HasEnumMember))]
    public void HasEnumMember_BehavesAsExpected(FluentCase<EnumRulesFixtures.AttributedEnum?> tc)
    {
        var result = new HasEnumMemberValidator().Validate(new AttributedEnumModel { Value = tc.Value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentEnumExtensionsTestData.NotHasEnumMember.Cases), MemberType = typeof(FluentEnumExtensionsTestData.NotHasEnumMember))]
    public void NotHasEnumMember_BehavesAsExpected(FluentCase<EnumRulesFixtures.AttributedEnum?> tc)
    {
        var result = new NotHasEnumMemberValidator().Validate(new AttributedEnumModel { Value = tc.Value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentEnumExtensionsTestData.Obsolete.Cases), MemberType = typeof(FluentEnumExtensionsTestData.Obsolete))]
    public void Obsolete_BehavesAsExpected(FluentCase<EnumRulesFixtures.AttributedEnum?> tc)
    {
        var result = new ObsoleteValidator().Validate(new AttributedEnumModel { Value = tc.Value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentEnumExtensionsTestData.NotObsolete.Cases), MemberType = typeof(FluentEnumExtensionsTestData.NotObsolete))]
    public void NotObsolete_BehavesAsExpected(FluentCase<EnumRulesFixtures.AttributedEnum?> tc)
    {
        var result = new NotObsoleteValidator().Validate(new AttributedEnumModel { Value = tc.Value });
        AssertResult(tc, result);
    }

    // ── Non-nullable overloads ─────────────────────────────────────────────

    private sealed record NonNullableSimpleEnumModel { public EnumRulesFixtures.SimpleEnum Value { get; init; } }
    private sealed record NonNullableIntModel { public int Value { get; init; } }
    private sealed record NonNullableFlagsEnumModel { public EnumRulesFixtures.FlagsEnum Value { get; init; } }
    private sealed record NonNullableAttributedEnumModel { public EnumRulesFixtures.AttributedEnum Value { get; init; } }

    private sealed class DefinedNonNullableValidator : AbstractValidator<NonNullableSimpleEnumModel>
    {
        public DefinedNonNullableValidator() => RuleFor(x => x.Value).Defined();
    }

    private sealed class NotDefinedNonNullableValidator : AbstractValidator<NonNullableSimpleEnumModel>
    {
        public NotDefinedNonNullableValidator() => RuleFor(x => x.Value).NotDefined();
    }

    private sealed class DefinedValueNonNullableValidator : AbstractValidator<NonNullableIntModel>
    {
        public DefinedValueNonNullableValidator() => RuleFor(x => x.Value).DefinedValue<NonNullableIntModel, EnumRulesFixtures.SimpleEnum>();
    }

    private sealed class NotDefinedValueNonNullableValidator : AbstractValidator<NonNullableIntModel>
    {
        public NotDefinedValueNonNullableValidator() => RuleFor(x => x.Value).NotDefinedValue<NonNullableIntModel, EnumRulesFixtures.SimpleEnum>();
    }

    private sealed class FlagsEnumCombinationNonNullableValidator : AbstractValidator<NonNullableFlagsEnumModel>
    {
        public FlagsEnumCombinationNonNullableValidator() => RuleFor(x => x.Value).FlagsEnumCombination();
    }

    private sealed class NotFlagsEnumCombinationNonNullableValidator : AbstractValidator<NonNullableFlagsEnumModel>
    {
        public NotFlagsEnumCombinationNonNullableValidator() => RuleFor(x => x.Value).NotFlagsEnumCombination();
    }

    private sealed class HasAttributeNonNullableValidator : AbstractValidator<NonNullableAttributedEnumModel>
    {
        public HasAttributeNonNullableValidator() => RuleFor(x => x.Value).HasAttribute<NonNullableAttributedEnumModel, EnumRulesFixtures.AttributedEnum, DescriptionAttribute>();
    }

    private sealed class NotHasAttributeNonNullableValidator : AbstractValidator<NonNullableAttributedEnumModel>
    {
        public NotHasAttributeNonNullableValidator() => RuleFor(x => x.Value).NotHasAttribute<NonNullableAttributedEnumModel, EnumRulesFixtures.AttributedEnum, DescriptionAttribute>();
    }

    private sealed class HasFlagNonNullableValidator : AbstractValidator<NonNullableFlagsEnumModel>
    {
        public HasFlagNonNullableValidator(EnumRulesFixtures.FlagsEnum flag) => RuleFor(x => x.Value).HasFlag(flag);
    }

    private sealed class NotHasFlagNonNullableValidator : AbstractValidator<NonNullableFlagsEnumModel>
    {
        public NotHasFlagNonNullableValidator(EnumRulesFixtures.FlagsEnum flag) => RuleFor(x => x.Value).NotHasFlag(flag);
    }

    private sealed class HasDescriptionNonNullableValidator : AbstractValidator<NonNullableAttributedEnumModel>
    {
        public HasDescriptionNonNullableValidator() => RuleFor(x => x.Value).HasDescription();
    }

    private sealed class NotHasDescriptionNonNullableValidator : AbstractValidator<NonNullableAttributedEnumModel>
    {
        public NotHasDescriptionNonNullableValidator() => RuleFor(x => x.Value).NotHasDescription();
    }

    private sealed class HasDisplayNonNullableValidator : AbstractValidator<NonNullableAttributedEnumModel>
    {
        public HasDisplayNonNullableValidator() => RuleFor(x => x.Value).HasDisplay();
    }

    private sealed class NotHasDisplayNonNullableValidator : AbstractValidator<NonNullableAttributedEnumModel>
    {
        public NotHasDisplayNonNullableValidator() => RuleFor(x => x.Value).NotHasDisplay();
    }

    private sealed class HasEnumMemberNonNullableValidator : AbstractValidator<NonNullableAttributedEnumModel>
    {
        public HasEnumMemberNonNullableValidator() => RuleFor(x => x.Value).HasEnumMember();
    }

    private sealed class NotHasEnumMemberNonNullableValidator : AbstractValidator<NonNullableAttributedEnumModel>
    {
        public NotHasEnumMemberNonNullableValidator() => RuleFor(x => x.Value).NotHasEnumMember();
    }

    private sealed class ObsoleteNonNullableValidator : AbstractValidator<NonNullableAttributedEnumModel>
    {
        public ObsoleteNonNullableValidator() => RuleFor(x => x.Value).Obsolete();
    }

    private sealed class NotObsoleteNonNullableValidator : AbstractValidator<NonNullableAttributedEnumModel>
    {
        public NotObsoleteNonNullableValidator() => RuleFor(x => x.Value).NotObsolete();
    }

    [Theory]
    [MemberData(nameof(FluentEnumExtensionsTestData.DefinedNonNullable.Cases), MemberType = typeof(FluentEnumExtensionsTestData.DefinedNonNullable))]
    public void DefinedNonNullable_BehavesAsExpected(FluentCase<EnumRulesFixtures.SimpleEnum> tc)
    {
        var result = new DefinedNonNullableValidator().Validate(new NonNullableSimpleEnumModel { Value = tc.Value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentEnumExtensionsTestData.NotDefinedNonNullable.Cases), MemberType = typeof(FluentEnumExtensionsTestData.NotDefinedNonNullable))]
    public void NotDefinedNonNullable_BehavesAsExpected(FluentCase<EnumRulesFixtures.SimpleEnum> tc)
    {
        var result = new NotDefinedNonNullableValidator().Validate(new NonNullableSimpleEnumModel { Value = tc.Value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentEnumExtensionsTestData.DefinedValueNonNullable.Cases), MemberType = typeof(FluentEnumExtensionsTestData.DefinedValueNonNullable))]
    public void DefinedValueNonNullable_BehavesAsExpected(FluentCase<int> tc)
    {
        var result = new DefinedValueNonNullableValidator().Validate(new NonNullableIntModel { Value = tc.Value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentEnumExtensionsTestData.NotDefinedValueNonNullable.Cases), MemberType = typeof(FluentEnumExtensionsTestData.NotDefinedValueNonNullable))]
    public void NotDefinedValueNonNullable_BehavesAsExpected(FluentCase<int> tc)
    {
        var result = new NotDefinedValueNonNullableValidator().Validate(new NonNullableIntModel { Value = tc.Value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentEnumExtensionsTestData.FlagsEnumCombinationNonNullable.Cases), MemberType = typeof(FluentEnumExtensionsTestData.FlagsEnumCombinationNonNullable))]
    public void FlagsEnumCombinationNonNullable_BehavesAsExpected(FluentCase<EnumRulesFixtures.FlagsEnum> tc)
    {
        var result = new FlagsEnumCombinationNonNullableValidator().Validate(new NonNullableFlagsEnumModel { Value = tc.Value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentEnumExtensionsTestData.NotFlagsEnumCombinationNonNullable.Cases), MemberType = typeof(FluentEnumExtensionsTestData.NotFlagsEnumCombinationNonNullable))]
    public void NotFlagsEnumCombinationNonNullable_BehavesAsExpected(FluentCase<EnumRulesFixtures.FlagsEnum> tc)
    {
        var result = new NotFlagsEnumCombinationNonNullableValidator().Validate(new NonNullableFlagsEnumModel { Value = tc.Value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentEnumExtensionsTestData.HasAttributeNonNullable.Cases), MemberType = typeof(FluentEnumExtensionsTestData.HasAttributeNonNullable))]
    public void HasAttributeNonNullable_BehavesAsExpected(FluentCase<EnumRulesFixtures.AttributedEnum> tc)
    {
        var result = new HasAttributeNonNullableValidator().Validate(new NonNullableAttributedEnumModel { Value = tc.Value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentEnumExtensionsTestData.NotHasAttributeNonNullable.Cases), MemberType = typeof(FluentEnumExtensionsTestData.NotHasAttributeNonNullable))]
    public void NotHasAttributeNonNullable_BehavesAsExpected(FluentCase<EnumRulesFixtures.AttributedEnum> tc)
    {
        var result = new NotHasAttributeNonNullableValidator().Validate(new NonNullableAttributedEnumModel { Value = tc.Value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentEnumExtensionsTestData.HasFlagNonNullable.Cases), MemberType = typeof(FluentEnumExtensionsTestData.HasFlagNonNullable))]
    public void HasFlagNonNullable_BehavesAsExpected(FluentCase<(EnumRulesFixtures.FlagsEnum value, EnumRulesFixtures.FlagsEnum flag)> tc)
    {
        var result = new HasFlagNonNullableValidator(tc.Value.flag).Validate(new NonNullableFlagsEnumModel { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentEnumExtensionsTestData.NotHasFlagNonNullable.Cases), MemberType = typeof(FluentEnumExtensionsTestData.NotHasFlagNonNullable))]
    public void NotHasFlagNonNullable_BehavesAsExpected(FluentCase<(EnumRulesFixtures.FlagsEnum value, EnumRulesFixtures.FlagsEnum flag)> tc)
    {
        var result = new NotHasFlagNonNullableValidator(tc.Value.flag).Validate(new NonNullableFlagsEnumModel { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentEnumExtensionsTestData.HasDescriptionNonNullable.Cases), MemberType = typeof(FluentEnumExtensionsTestData.HasDescriptionNonNullable))]
    public void HasDescriptionNonNullable_BehavesAsExpected(FluentCase<EnumRulesFixtures.AttributedEnum> tc)
    {
        var result = new HasDescriptionNonNullableValidator().Validate(new NonNullableAttributedEnumModel { Value = tc.Value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentEnumExtensionsTestData.NotHasDescriptionNonNullable.Cases), MemberType = typeof(FluentEnumExtensionsTestData.NotHasDescriptionNonNullable))]
    public void NotHasDescriptionNonNullable_BehavesAsExpected(FluentCase<EnumRulesFixtures.AttributedEnum> tc)
    {
        var result = new NotHasDescriptionNonNullableValidator().Validate(new NonNullableAttributedEnumModel { Value = tc.Value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentEnumExtensionsTestData.HasDisplayNonNullable.Cases), MemberType = typeof(FluentEnumExtensionsTestData.HasDisplayNonNullable))]
    public void HasDisplayNonNullable_BehavesAsExpected(FluentCase<EnumRulesFixtures.AttributedEnum> tc)
    {
        var result = new HasDisplayNonNullableValidator().Validate(new NonNullableAttributedEnumModel { Value = tc.Value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentEnumExtensionsTestData.NotHasDisplayNonNullable.Cases), MemberType = typeof(FluentEnumExtensionsTestData.NotHasDisplayNonNullable))]
    public void NotHasDisplayNonNullable_BehavesAsExpected(FluentCase<EnumRulesFixtures.AttributedEnum> tc)
    {
        var result = new NotHasDisplayNonNullableValidator().Validate(new NonNullableAttributedEnumModel { Value = tc.Value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentEnumExtensionsTestData.HasEnumMemberNonNullable.Cases), MemberType = typeof(FluentEnumExtensionsTestData.HasEnumMemberNonNullable))]
    public void HasEnumMemberNonNullable_BehavesAsExpected(FluentCase<EnumRulesFixtures.AttributedEnum> tc)
    {
        var result = new HasEnumMemberNonNullableValidator().Validate(new NonNullableAttributedEnumModel { Value = tc.Value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentEnumExtensionsTestData.NotHasEnumMemberNonNullable.Cases), MemberType = typeof(FluentEnumExtensionsTestData.NotHasEnumMemberNonNullable))]
    public void NotHasEnumMemberNonNullable_BehavesAsExpected(FluentCase<EnumRulesFixtures.AttributedEnum> tc)
    {
        var result = new NotHasEnumMemberNonNullableValidator().Validate(new NonNullableAttributedEnumModel { Value = tc.Value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentEnumExtensionsTestData.ObsoleteNonNullable.Cases), MemberType = typeof(FluentEnumExtensionsTestData.ObsoleteNonNullable))]
    public void ObsoleteNonNullable_BehavesAsExpected(FluentCase<EnumRulesFixtures.AttributedEnum> tc)
    {
        var result = new ObsoleteNonNullableValidator().Validate(new NonNullableAttributedEnumModel { Value = tc.Value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentEnumExtensionsTestData.NotObsoleteNonNullable.Cases), MemberType = typeof(FluentEnumExtensionsTestData.NotObsoleteNonNullable))]
    public void NotObsoleteNonNullable_BehavesAsExpected(FluentCase<EnumRulesFixtures.AttributedEnum> tc)
    {
        var result = new NotObsoleteNonNullableValidator().Validate(new NonNullableAttributedEnumModel { Value = tc.Value });
        AssertResult(tc, result);
    }
}

#pragma warning restore CS0618
