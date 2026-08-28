using System.Runtime.CompilerServices;
using PineGuard.Codes;
using PineGuard.Rules;

namespace PineGuard.MustClauses;

/// <summary>
/// Provides <see cref="IMustClause"/> extension methods that validate enum values,
/// delegating to <see cref="EnumRules"/> for core validation logic.
/// </summary>
/// <seealso cref="EnumRules"/>
/// <seealso href="https://pineguard.ai/docs/must/enum">Enum Must Clauses documentation</seealso>
public static class MustEnumClauses
{
    /// <summary>
    /// Validates that the specified value must be a defined enum value.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must be a defined enum value."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/enum">Enum Must Clauses documentation</seealso>
    public static MustResult<TEnum> Defined<TEnum>(this IMustClause _,
        TEnum value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where TEnum : struct, Enum
    {
        const string messageTemplate = "{paramName} must be a defined enum value.";

        var ok = EnumRules.IsDefined<TEnum>(value);
        return MustResult<TEnum>.FromBool(ok, MustCodes.Enum.Value.NotDefined, messageTemplate, paramName, value, result: value);
    }

    /// <summary>
    /// Validates that the specified value must be a defined enum backing value.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must be a defined enum backing value."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/enum">Enum Must Clauses documentation</seealso>
    public static MustResult<int> DefinedValue<TEnum>(this IMustClause _,
        int value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where TEnum : struct, Enum
    {
        const string messageTemplate = "{paramName} must be a defined enum backing value.";

        var ok = EnumRules.IsDefinedValue<TEnum>(value);
        return MustResult<int>.FromBool(ok, MustCodes.Enum.BackingValue.NotDefined, messageTemplate, paramName, value, result: value);
    }

    /// <summary>
    /// Validates that the specified value must be a defined enum name.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="name">The name to validate.</param>
    /// <param name="ignoreCase">If <see langword="true"/>, the comparison is case-insensitive.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must be a defined enum name."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/enum">Enum Must Clauses documentation</seealso>
    public static MustResult<string> DefinedName<TEnum>(this IMustClause _,
        string? name,
        bool ignoreCase = true,
        [CallerArgumentExpression(nameof(name))] string? paramName = null)
        where TEnum : struct, Enum
    {
        const string messageTemplate = "{paramName} must be a defined enum name.";

        var ok = EnumRules.IsDefinedName<TEnum>(name, ignoreCase);
        return MustResult<string>.FromBool(ok, MustCodes.Enum.Name.NotDefined, messageTemplate, paramName, name, result: name!);
    }

    /// <summary>
    /// Validates that the specified value must be a valid flags enum combination.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must be a valid flags enum combination."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/enum">Enum Must Clauses documentation</seealso>
    public static MustResult<TEnum> FlagsEnumCombination<TEnum>(this IMustClause _,
        TEnum value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where TEnum : struct, Enum
    {
        const string messageTemplate = "{paramName} must be a valid flags enum combination.";

        var ok = EnumRules.IsFlagsEnumCombination<TEnum>(value);
        return MustResult<TEnum>.FromBool(ok, MustCodes.Enum.Flags.NotDefined, messageTemplate, paramName, value, result: value);
    }

    /// <summary>
    /// Validates that the specified value must have the expected attribute.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must have the expected attribute."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/enum">Enum Must Clauses documentation</seealso>
    public static MustResult<TEnum> HasAttribute<TEnum, TAttribute>(this IMustClause _,
        TEnum value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where TEnum : struct, Enum
        where TAttribute : Attribute
    {
        const string messageTemplate = "{paramName} must have the expected attribute.";

        var ok = EnumRules.HasAttribute<TEnum, TAttribute>(value);
        return MustResult<TEnum>.FromBool(ok, MustCodes.Enum.Attribute.Missing, messageTemplate, paramName, value, result: value);
    }

    /// <summary>
    /// Validates that the specified value must have the expected flag.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="flag">The flag to check for.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must have the expected flag."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/enum">Enum Must Clauses documentation</seealso>
    public static MustResult<TEnum> HasFlag<TEnum>(this IMustClause _,
        TEnum value,
        TEnum flag,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where TEnum : struct, Enum
    {
        const string messageTemplate = "{paramName} must have the expected flag.";

        var ok = EnumRules.HasFlag(value, flag);
        return MustResult<TEnum>.FromBool(ok, MustCodes.Enum.Flags.NotSet, messageTemplate, paramName, value, result: value);
    }

    /// <summary>
    /// Validates that the specified value must not have the expected flag.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="flag">The flag to check for.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must not have the expected flag."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/enum">Enum Must Clauses documentation</seealso>
    public static MustResult<TEnum> NotHasFlag<TEnum>(this IMustClause _,
        TEnum value,
        TEnum flag,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where TEnum : struct, Enum
    {
        const string messageTemplate = "{paramName} must not have the expected flag.";

        var ok = !EnumRules.HasFlag(value, flag);
        return MustResult<TEnum>.FromBool(ok, MustCodes.Enum.Flags.Set, messageTemplate, paramName, value, result: value);
    }

    /// <summary>
    /// Validates that the specified value must have a description.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must have a description."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/enum">Enum Must Clauses documentation</seealso>
    public static MustResult<TEnum> HasDescription<TEnum>(this IMustClause _,
        TEnum value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where TEnum : struct, Enum
    {
        const string messageTemplate = "{paramName} must have a description.";

        var ok = EnumRules.HasDescription<TEnum>(value);
        return MustResult<TEnum>.FromBool(ok, MustCodes.Enum.Description.Missing, messageTemplate, paramName, value, result: value);
    }

    /// <summary>
    /// Validates that the specified value must not have a description.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must not have a description."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/enum">Enum Must Clauses documentation</seealso>
    public static MustResult<TEnum> NotHasDescription<TEnum>(this IMustClause _,
        TEnum value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where TEnum : struct, Enum
    {
        const string messageTemplate = "{paramName} must not have a description.";

        var ok = !EnumRules.HasDescription<TEnum>(value);
        return MustResult<TEnum>.FromBool(ok, MustCodes.Enum.Description.Present, messageTemplate, paramName, value, result: value);
    }

    /// <summary>
    /// Validates that the specified value must have a display attribute.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must have a display attribute."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/enum">Enum Must Clauses documentation</seealso>
    public static MustResult<TEnum> HasDisplay<TEnum>(this IMustClause _,
        TEnum value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where TEnum : struct, Enum
    {
        const string messageTemplate = "{paramName} must have a display attribute.";

        var ok = EnumRules.HasDisplay<TEnum>(value);
        return MustResult<TEnum>.FromBool(ok, MustCodes.Enum.Display.Missing, messageTemplate, paramName, value, result: value);
    }

    /// <summary>
    /// Validates that the specified value must not have a display attribute.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must not have a display attribute."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/enum">Enum Must Clauses documentation</seealso>
    public static MustResult<TEnum> NotHasDisplay<TEnum>(this IMustClause _,
        TEnum value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where TEnum : struct, Enum
    {
        const string messageTemplate = "{paramName} must not have a display attribute.";

        var ok = !EnumRules.HasDisplay<TEnum>(value);
        return MustResult<TEnum>.FromBool(ok, MustCodes.Enum.Display.Present, messageTemplate, paramName, value, result: value);
    }

    /// <summary>
    /// Validates that the specified value must have an enum member attribute.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must have an enum member attribute."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/enum">Enum Must Clauses documentation</seealso>
    public static MustResult<TEnum> HasEnumMember<TEnum>(this IMustClause _,
        TEnum value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where TEnum : struct, Enum
    {
        const string messageTemplate = "{paramName} must have an enum member attribute.";

        var ok = EnumRules.HasEnumMember<TEnum>(value);
        return MustResult<TEnum>.FromBool(ok, MustCodes.Enum.MemberAttribute.Missing, messageTemplate, paramName, value, result: value);
    }

    /// <summary>
    /// Validates that the specified value must not have an enum member attribute.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must not have an enum member attribute."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/enum">Enum Must Clauses documentation</seealso>
    public static MustResult<TEnum> NotHasEnumMember<TEnum>(this IMustClause _,
        TEnum value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where TEnum : struct, Enum
    {
        const string messageTemplate = "{paramName} must not have an enum member attribute.";

        var ok = !EnumRules.HasEnumMember<TEnum>(value);
        return MustResult<TEnum>.FromBool(ok, MustCodes.Enum.MemberAttribute.Present, messageTemplate, paramName, value, result: value);
    }

    /// <summary>
    /// Validates that the specified value must be obsolete.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must be obsolete."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/enum">Enum Must Clauses documentation</seealso>
    public static MustResult<TEnum> Obsolete<TEnum>(this IMustClause _,
        TEnum value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where TEnum : struct, Enum
    {
        const string messageTemplate = "{paramName} must be obsolete.";

        var ok = EnumRules.IsObsolete<TEnum>(value);
        return MustResult<TEnum>.FromBool(ok, MustCodes.Enum.Obsolescence.Missing, messageTemplate, paramName, value, result: value);
    }

    /// <summary>
    /// Validates that the specified value must not be obsolete.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must not be obsolete."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/enum">Enum Must Clauses documentation</seealso>
    public static MustResult<TEnum> NotObsolete<TEnum>(this IMustClause _,
        TEnum value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where TEnum : struct, Enum
    {
        const string messageTemplate = "{paramName} must not be obsolete.";

        var ok = !EnumRules.IsObsolete<TEnum>(value);
        return MustResult<TEnum>.FromBool(ok, MustCodes.Enum.Obsolescence.Present, messageTemplate, paramName, value, result: value);
    }

    /// <summary>
    /// Validates that the specified value must not be a defined enum value.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must not be a defined enum value."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/enum">Enum Must Clauses documentation</seealso>
    public static MustResult<TEnum> NotDefined<TEnum>(this IMustClause _,
        TEnum value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where TEnum : struct, Enum
    {
        const string messageTemplate = "{paramName} must not be a defined enum value.";

        var ok = !EnumRules.IsDefined<TEnum>(value);
        return MustResult<TEnum>.FromBool(ok, MustCodes.Enum.Value.Defined, messageTemplate, paramName, value, result: value);
    }

    /// <summary>
    /// Validates that the specified value must not be a defined enum backing value.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must not be a defined enum backing value."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/enum">Enum Must Clauses documentation</seealso>
    public static MustResult<int> NotDefinedValue<TEnum>(this IMustClause _,
        int value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where TEnum : struct, Enum
    {
        const string messageTemplate = "{paramName} must not be a defined enum backing value.";

        var ok = !EnumRules.IsDefinedValue<TEnum>(value);
        return MustResult<int>.FromBool(ok, MustCodes.Enum.BackingValue.Defined, messageTemplate, paramName, value, result: value);
    }

    /// <summary>
    /// Validates that the specified value must not be a defined enum name.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="name">The name to validate.</param>
    /// <param name="ignoreCase">If <see langword="true"/>, the comparison is case-insensitive.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must not be a defined enum name."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/enum">Enum Must Clauses documentation</seealso>
    public static MustResult<string> NotDefinedName<TEnum>(this IMustClause _,
        string? name,
        bool ignoreCase = true,
        [CallerArgumentExpression(nameof(name))] string? paramName = null)
        where TEnum : struct, Enum
    {
        const string messageTemplate = "{paramName} must not be a defined enum name.";

        var ok = !EnumRules.IsDefinedName<TEnum>(name, ignoreCase);
        return MustResult<string>.FromBool(ok, MustCodes.Enum.Name.Defined, messageTemplate, paramName, name, result: name!);
    }

    /// <summary>
    /// Validates that the specified value must not be a valid flags enum combination.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must not be a valid flags enum combination."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/enum">Enum Must Clauses documentation</seealso>
    public static MustResult<TEnum> NotFlagsEnumCombination<TEnum>(this IMustClause _,
        TEnum value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where TEnum : struct, Enum
    {
        const string messageTemplate = "{paramName} must not be a valid flags enum combination.";

        var ok = !EnumRules.IsFlagsEnumCombination<TEnum>(value);
        return MustResult<TEnum>.FromBool(ok, MustCodes.Enum.Flags.Defined, messageTemplate, paramName, value, result: value);
    }

    /// <summary>
    /// Validates that the specified value must not have the expected attribute.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must not have the expected attribute."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/enum">Enum Must Clauses documentation</seealso>
    public static MustResult<TEnum> NotHasAttribute<TEnum, TAttribute>(this IMustClause _,
        TEnum value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where TEnum : struct, Enum
        where TAttribute : Attribute
    {
        const string messageTemplate = "{paramName} must not have the expected attribute.";

        var ok = !EnumRules.HasAttribute<TEnum, TAttribute>(value);
        return MustResult<TEnum>.FromBool(ok, MustCodes.Enum.Attribute.Present, messageTemplate, paramName, value, result: value);
    }
}
