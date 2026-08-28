using FluentValidation;
using PineGuard.Codes;
using PineGuard.FluentValidation.Common;
using PineGuard.MustClauses;

namespace PineGuard.FluentValidation;

/// <summary>
/// Provides FluentValidation extension methods for enum value validation.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/fluent/enum">Fluent Enum Extensions documentation</seealso>
public static class FluentEnumExtensions
{
    /// <summary>
    /// Validates that the nullable enum property value is a defined member of the <typeparamref name="TEnum"/> enumeration.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <typeparam name="TEnum">The enum type to validate against.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustEnumClauses.Defined"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Status).Defined();
    /// </code>
    /// </example>
    /// <seealso cref="MustEnumClauses.Defined"/>
    public static IRuleBuilderOptions<TModel, TEnum?> Defined<TModel, TEnum>(this IRuleBuilder<TModel, TEnum?> ruleBuilder, string? message = null)
        where TEnum : struct, Enum =>
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.Defined(val.Value, paramName: null) : MustResult<TEnum>.Ok(default),
            message, MustCodes.Enum.Value.NotDefined);

    /// <summary>
    /// Validates that the non-nullable enum property value is a defined member of the <typeparamref name="TEnum"/> enumeration.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <typeparam name="TEnum">The enum type to validate against.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustEnumClauses.Defined"/>.
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Status).Defined();
    /// </code>
    /// </example>
    /// <seealso cref="MustEnumClauses.Defined"/>
    public static IRuleBuilderOptions<TModel, TEnum> Defined<TModel, TEnum>(this IRuleBuilder<TModel, TEnum> ruleBuilder, string? message = null)
        where TEnum : struct, Enum =>
        ruleBuilder.MustBe(val => Must.Be.Defined(val, paramName: null),
            message, MustCodes.Enum.Value.NotDefined);

    /// <summary>
    /// Validates that the nullable enum property value is not a defined member of the <typeparamref name="TEnum"/> enumeration.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <typeparam name="TEnum">The enum type to validate against.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustEnumClauses.NotDefined"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Status).NotDefined();
    /// </code>
    /// </example>
    /// <seealso cref="MustEnumClauses.NotDefined"/>
    public static IRuleBuilderOptions<TModel, TEnum?> NotDefined<TModel, TEnum>(this IRuleBuilder<TModel, TEnum?> ruleBuilder, string? message = null)
        where TEnum : struct, Enum =>
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.NotDefined(val.Value, paramName: null) : MustResult<TEnum>.Ok(default),
            message, MustCodes.Enum.Value.Defined);

    /// <summary>
    /// Validates that the non-nullable enum property value is not a defined member of the <typeparamref name="TEnum"/> enumeration.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <typeparam name="TEnum">The enum type to validate against.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustEnumClauses.NotDefined"/>.
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Status).NotDefined();
    /// </code>
    /// </example>
    /// <seealso cref="MustEnumClauses.NotDefined"/>
    public static IRuleBuilderOptions<TModel, TEnum> NotDefined<TModel, TEnum>(this IRuleBuilder<TModel, TEnum> ruleBuilder, string? message = null)
        where TEnum : struct, Enum =>
        ruleBuilder.MustBe(val => Must.Be.NotDefined(val, paramName: null),
            message, MustCodes.Enum.Value.Defined);

    /// <summary>
    /// Validates that the nullable <see cref="int"/> property value corresponds to a defined member of <typeparamref name="TEnum"/>.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <typeparam name="TEnum">The enum type to validate the integer value against.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustEnumClauses.DefinedValue"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code><![CDATA[
    /// RuleFor(x => x.StatusCode).DefinedValue<MyModel, MyEnum>();
    /// ]]></code>
    /// </example>
    /// <seealso cref="MustEnumClauses.DefinedValue"/>
    public static IRuleBuilderOptions<TModel, int?> DefinedValue<TModel, TEnum>(this IRuleBuilder<TModel, int?> ruleBuilder, string? message = null)
        where TEnum : struct, Enum =>
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.DefinedValue<TEnum>(val.Value, paramName: null) : MustResult<int>.Ok(0),
            message, MustCodes.Enum.BackingValue.NotDefined);

    /// <summary>
    /// Validates that the non-nullable <see cref="int"/> property value corresponds to a defined member of <typeparamref name="TEnum"/>.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <typeparam name="TEnum">The enum type to validate the integer value against.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustEnumClauses.DefinedValue"/>.
    /// </remarks>
    /// <example>
    /// <code><![CDATA[
    /// RuleFor(x => x.StatusCode).DefinedValue<MyModel, MyEnum>();
    /// ]]></code>
    /// </example>
    /// <seealso cref="MustEnumClauses.DefinedValue"/>
    public static IRuleBuilderOptions<TModel, int> DefinedValue<TModel, TEnum>(this IRuleBuilder<TModel, int> ruleBuilder, string? message = null)
        where TEnum : struct, Enum =>
        ruleBuilder.MustBe(val => Must.Be.DefinedValue<TEnum>(val, paramName: null),
            message, MustCodes.Enum.BackingValue.NotDefined);

    /// <summary>
    /// Validates that the nullable <see cref="int"/> property value does not correspond to a defined member of <typeparamref name="TEnum"/>.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <typeparam name="TEnum">The enum type to validate the integer value against.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustEnumClauses.NotDefinedValue"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code><![CDATA[
    /// RuleFor(x => x.StatusCode).NotDefinedValue<MyModel, MyEnum>();
    /// ]]></code>
    /// </example>
    /// <seealso cref="MustEnumClauses.NotDefinedValue"/>
    public static IRuleBuilderOptions<TModel, int?> NotDefinedValue<TModel, TEnum>(this IRuleBuilder<TModel, int?> ruleBuilder, string? message = null)
        where TEnum : struct, Enum =>
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.NotDefinedValue<TEnum>(val.Value, paramName: null) : MustResult<int>.Ok(0),
            message, MustCodes.Enum.BackingValue.Defined);

    /// <summary>
    /// Validates that the non-nullable <see cref="int"/> property value does not correspond to a defined member of <typeparamref name="TEnum"/>.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <typeparam name="TEnum">The enum type to validate the integer value against.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustEnumClauses.NotDefinedValue"/>.
    /// </remarks>
    /// <example>
    /// <code><![CDATA[
    /// RuleFor(x => x.StatusCode).NotDefinedValue<MyModel, MyEnum>();
    /// ]]></code>
    /// </example>
    /// <seealso cref="MustEnumClauses.NotDefinedValue"/>
    public static IRuleBuilderOptions<TModel, int> NotDefinedValue<TModel, TEnum>(this IRuleBuilder<TModel, int> ruleBuilder, string? message = null)
        where TEnum : struct, Enum =>
        ruleBuilder.MustBe(val => Must.Be.NotDefinedValue<TEnum>(val, paramName: null),
            message, MustCodes.Enum.BackingValue.Defined);

    /// <summary>
    /// Validates that the string property value is a defined name of a <typeparamref name="TEnum"/> member.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <typeparam name="TEnum">The enum type to validate the name against.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="ignoreCase">When <see langword="true"/>, the name comparison is case-insensitive.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustEnumClauses.DefinedName"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code><![CDATA[
    /// RuleFor(x => x.StatusName).DefinedName<MyModel, MyEnum>();
    /// ]]></code>
    /// </example>
    /// <seealso cref="MustEnumClauses.DefinedName"/>
    public static IRuleBuilderOptions<TModel, string?> DefinedName<TModel, TEnum>(this IRuleBuilder<TModel, string?> ruleBuilder,
        bool ignoreCase = true,
        string? message = null)
        where TEnum : struct, Enum =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.DefinedName<TEnum>(val, ignoreCase, paramName: null) : MustResult<string>.Ok(string.Empty),
            message, MustCodes.Enum.Name.NotDefined);

    /// <summary>
    /// Validates that the string property value is not a defined name of a <typeparamref name="TEnum"/> member.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <typeparam name="TEnum">The enum type to validate the name against.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="ignoreCase">When <see langword="true"/>, the name comparison is case-insensitive.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustEnumClauses.NotDefinedName"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code><![CDATA[
    /// RuleFor(x => x.StatusName).NotDefinedName<MyModel, MyEnum>();
    /// ]]></code>
    /// </example>
    /// <seealso cref="MustEnumClauses.NotDefinedName"/>
    public static IRuleBuilderOptions<TModel, string?> NotDefinedName<TModel, TEnum>(this IRuleBuilder<TModel, string?> ruleBuilder,
        bool ignoreCase = true,
        string? message = null)
        where TEnum : struct, Enum =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.NotDefinedName<TEnum>(val, ignoreCase, paramName: null) : MustResult<string>.Ok(string.Empty),
            message, MustCodes.Enum.Name.Defined);

    /// <summary>
    /// Validates that the nullable enum property value is a valid combination of flags for a <c>[Flags]</c> enum.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <typeparam name="TEnum">The flags enum type.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustEnumClauses.FlagsEnumCombination"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Permissions).FlagsEnumCombination();
    /// </code>
    /// </example>
    /// <seealso cref="MustEnumClauses.FlagsEnumCombination"/>
    public static IRuleBuilderOptions<TModel, TEnum?> FlagsEnumCombination<TModel, TEnum>(this IRuleBuilder<TModel, TEnum?> ruleBuilder, string? message = null)
        where TEnum : struct, Enum =>
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.FlagsEnumCombination(val.Value, paramName: null) : MustResult<TEnum>.Ok(default),
            message, MustCodes.Enum.Flags.NotDefined);

    /// <summary>
    /// Validates that the non-nullable enum property value is a valid combination of flags for a <c>[Flags]</c> enum.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <typeparam name="TEnum">The flags enum type.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustEnumClauses.FlagsEnumCombination"/>.
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Permissions).FlagsEnumCombination();
    /// </code>
    /// </example>
    /// <seealso cref="MustEnumClauses.FlagsEnumCombination"/>
    public static IRuleBuilderOptions<TModel, TEnum> FlagsEnumCombination<TModel, TEnum>(this IRuleBuilder<TModel, TEnum> ruleBuilder, string? message = null)
        where TEnum : struct, Enum =>
        ruleBuilder.MustBe(val => Must.Be.FlagsEnumCombination(val, paramName: null),
            message, MustCodes.Enum.Flags.NotDefined);

    /// <summary>
    /// Validates that the nullable enum property value is not a valid combination of flags for a <c>[Flags]</c> enum.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <typeparam name="TEnum">The flags enum type.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustEnumClauses.NotFlagsEnumCombination"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Permissions).NotFlagsEnumCombination();
    /// </code>
    /// </example>
    /// <seealso cref="MustEnumClauses.NotFlagsEnumCombination"/>
    public static IRuleBuilderOptions<TModel, TEnum?> NotFlagsEnumCombination<TModel, TEnum>(this IRuleBuilder<TModel, TEnum?> ruleBuilder, string? message = null)
        where TEnum : struct, Enum =>
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.NotFlagsEnumCombination(val.Value, paramName: null) : MustResult<TEnum>.Ok(default),
            message, MustCodes.Enum.Flags.Defined);

    /// <summary>
    /// Validates that the non-nullable enum property value is not a valid combination of flags for a <c>[Flags]</c> enum.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <typeparam name="TEnum">The flags enum type.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustEnumClauses.NotFlagsEnumCombination"/>.
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Permissions).NotFlagsEnumCombination();
    /// </code>
    /// </example>
    /// <seealso cref="MustEnumClauses.NotFlagsEnumCombination"/>
    public static IRuleBuilderOptions<TModel, TEnum> NotFlagsEnumCombination<TModel, TEnum>(this IRuleBuilder<TModel, TEnum> ruleBuilder, string? message = null)
        where TEnum : struct, Enum =>
        ruleBuilder.MustBe(val => Must.Be.NotFlagsEnumCombination(val, paramName: null),
            message, MustCodes.Enum.Flags.Defined);

    /// <summary>
    /// Validates that the nullable enum property value has the <typeparamref name="TAttribute"/> attribute applied to its member.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <typeparam name="TEnum">The enum type.</typeparam>
    /// <typeparam name="TAttribute">The attribute type to check for.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustEnumClauses.HasAttribute"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code><![CDATA[
    /// RuleFor(x => x.Status).HasAttribute<MyModel, MyEnum, DisplayAttribute>();
    /// ]]></code>
    /// </example>
    /// <seealso cref="MustEnumClauses.HasAttribute"/>
    public static IRuleBuilderOptions<TModel, TEnum?> HasAttribute<TModel, TEnum, TAttribute>(this IRuleBuilder<TModel, TEnum?> ruleBuilder, string? message = null)
        where TEnum : struct, Enum
        where TAttribute : Attribute =>
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.HasAttribute<TEnum, TAttribute>(val.Value, paramName: null) : MustResult<TEnum>.Ok(default),
            message, MustCodes.Enum.Attribute.Missing);

    /// <summary>
    /// Validates that the non-nullable enum property value has the <typeparamref name="TAttribute"/> attribute applied to its member.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <typeparam name="TEnum">The enum type.</typeparam>
    /// <typeparam name="TAttribute">The attribute type to check for.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustEnumClauses.HasAttribute"/>.
    /// </remarks>
    /// <example>
    /// <code><![CDATA[
    /// RuleFor(x => x.Status).HasAttribute<MyModel, MyEnum, DisplayAttribute>();
    /// ]]></code>
    /// </example>
    /// <seealso cref="MustEnumClauses.HasAttribute"/>
    public static IRuleBuilderOptions<TModel, TEnum> HasAttribute<TModel, TEnum, TAttribute>(this IRuleBuilder<TModel, TEnum> ruleBuilder, string? message = null)
        where TEnum : struct, Enum
        where TAttribute : Attribute =>
        ruleBuilder.MustBe(val => Must.Be.HasAttribute<TEnum, TAttribute>(val, paramName: null),
            message, MustCodes.Enum.Attribute.Missing);

    /// <summary>
    /// Validates that the nullable enum property value does not have the <typeparamref name="TAttribute"/> attribute applied to its member.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <typeparam name="TEnum">The enum type.</typeparam>
    /// <typeparam name="TAttribute">The attribute type to check for absence.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustEnumClauses.NotHasAttribute"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code><![CDATA[
    /// RuleFor(x => x.Status).NotHasAttribute<MyModel, MyEnum, ObsoleteAttribute>();
    /// ]]></code>
    /// </example>
    /// <seealso cref="MustEnumClauses.NotHasAttribute"/>
    public static IRuleBuilderOptions<TModel, TEnum?> NotHasAttribute<TModel, TEnum, TAttribute>(this IRuleBuilder<TModel, TEnum?> ruleBuilder, string? message = null)
        where TEnum : struct, Enum
        where TAttribute : Attribute =>
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.NotHasAttribute<TEnum, TAttribute>(val.Value, paramName: null) : MustResult<TEnum>.Ok(default),
            message, MustCodes.Enum.Attribute.Present);

    /// <summary>
    /// Validates that the non-nullable enum property value does not have the <typeparamref name="TAttribute"/> attribute applied to its member.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <typeparam name="TEnum">The enum type.</typeparam>
    /// <typeparam name="TAttribute">The attribute type to check for absence.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustEnumClauses.NotHasAttribute"/>.
    /// </remarks>
    /// <example>
    /// <code><![CDATA[
    /// RuleFor(x => x.Status).NotHasAttribute<MyModel, MyEnum, ObsoleteAttribute>();
    /// ]]></code>
    /// </example>
    /// <seealso cref="MustEnumClauses.NotHasAttribute"/>
    public static IRuleBuilderOptions<TModel, TEnum> NotHasAttribute<TModel, TEnum, TAttribute>(this IRuleBuilder<TModel, TEnum> ruleBuilder, string? message = null)
        where TEnum : struct, Enum
        where TAttribute : Attribute =>
        ruleBuilder.MustBe(val => Must.Be.NotHasAttribute<TEnum, TAttribute>(val, paramName: null),
            message, MustCodes.Enum.Attribute.Present);

    /// <summary>
    /// Validates that the nullable enum property value has the specified flag set.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <typeparam name="TEnum">The flags enum type.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="flag">The flag that must be set.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustEnumClauses.HasFlag"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Permissions).HasFlag(Permission.Read);
    /// </code>
    /// </example>
    /// <seealso cref="MustEnumClauses.HasFlag"/>
    public static IRuleBuilderOptions<TModel, TEnum?> HasFlag<TModel, TEnum>(
        this IRuleBuilder<TModel, TEnum?> ruleBuilder,
        TEnum flag,
        string? message = null)
        where TEnum : struct, Enum =>
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.HasFlag(val.Value, flag, paramName: null) : MustResult<TEnum>.Ok(default),
            message);

    /// <summary>
    /// Validates that the non-nullable enum property value has the specified flag set.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <typeparam name="TEnum">The flags enum type.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="flag">The flag that must be set.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustEnumClauses.HasFlag"/>.
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Permissions).HasFlag(Permission.Write);
    /// </code>
    /// </example>
    /// <seealso cref="MustEnumClauses.HasFlag"/>
    public static IRuleBuilderOptions<TModel, TEnum> HasFlag<TModel, TEnum>(
        this IRuleBuilder<TModel, TEnum> ruleBuilder,
        TEnum flag,
        string? message = null)
        where TEnum : struct, Enum =>
        ruleBuilder.MustBe(val => Must.Be.HasFlag(val, flag, paramName: null),
            message);

    /// <summary>
    /// Validates that the nullable enum property value does not have the specified flag set.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <typeparam name="TEnum">The flags enum type.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="flag">The flag that must not be set.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustEnumClauses.NotHasFlag"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Permissions).NotHasFlag(Permission.Admin);
    /// </code>
    /// </example>
    /// <seealso cref="MustEnumClauses.NotHasFlag"/>
    public static IRuleBuilderOptions<TModel, TEnum?> NotHasFlag<TModel, TEnum>(
        this IRuleBuilder<TModel, TEnum?> ruleBuilder,
        TEnum flag,
        string? message = null)
        where TEnum : struct, Enum =>
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.NotHasFlag(val.Value, flag, paramName: null) : MustResult<TEnum>.Ok(default),
            message);

    /// <summary>
    /// Validates that the non-nullable enum property value does not have the specified flag set.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <typeparam name="TEnum">The flags enum type.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="flag">The flag that must not be set.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustEnumClauses.NotHasFlag"/>.
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Permissions).NotHasFlag(Permission.Admin);
    /// </code>
    /// </example>
    /// <seealso cref="MustEnumClauses.NotHasFlag"/>
    public static IRuleBuilderOptions<TModel, TEnum> NotHasFlag<TModel, TEnum>(
        this IRuleBuilder<TModel, TEnum> ruleBuilder,
        TEnum flag,
        string? message = null)
        where TEnum : struct, Enum =>
        ruleBuilder.MustBe(val => Must.Be.NotHasFlag(val, flag, paramName: null),
            message);

    /// <summary>
    /// Validates that the nullable enum property value has a <see cref="System.ComponentModel.DescriptionAttribute"/> applied.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <typeparam name="TEnum">The enum type.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustEnumClauses.HasDescription"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Status).HasDescription();
    /// </code>
    /// </example>
    /// <seealso cref="MustEnumClauses.HasDescription"/>
    public static IRuleBuilderOptions<TModel, TEnum?> HasDescription<TModel, TEnum>(
        this IRuleBuilder<TModel, TEnum?> ruleBuilder,
        string? message = null)
        where TEnum : struct, Enum =>
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.HasDescription(val.Value, paramName: null) : MustResult<TEnum>.Ok(default),
            message);

    /// <summary>
    /// Validates that the non-nullable enum property value has a <see cref="System.ComponentModel.DescriptionAttribute"/> applied.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <typeparam name="TEnum">The enum type.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustEnumClauses.HasDescription"/>.
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Status).HasDescription();
    /// </code>
    /// </example>
    /// <seealso cref="MustEnumClauses.HasDescription"/>
    public static IRuleBuilderOptions<TModel, TEnum> HasDescription<TModel, TEnum>(
        this IRuleBuilder<TModel, TEnum> ruleBuilder,
        string? message = null)
        where TEnum : struct, Enum =>
        ruleBuilder.MustBe(val => Must.Be.HasDescription(val, paramName: null),
            message);

    /// <summary>
    /// Validates that the nullable enum property value does not have a <see cref="System.ComponentModel.DescriptionAttribute"/> applied.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <typeparam name="TEnum">The enum type.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustEnumClauses.NotHasDescription"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Status).NotHasDescription();
    /// </code>
    /// </example>
    /// <seealso cref="MustEnumClauses.NotHasDescription"/>
    public static IRuleBuilderOptions<TModel, TEnum?> NotHasDescription<TModel, TEnum>(
        this IRuleBuilder<TModel, TEnum?> ruleBuilder,
        string? message = null)
        where TEnum : struct, Enum =>
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.NotHasDescription(val.Value, paramName: null) : MustResult<TEnum>.Ok(default),
            message);

    /// <summary>
    /// Validates that the non-nullable enum property value does not have a <see cref="System.ComponentModel.DescriptionAttribute"/> applied.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <typeparam name="TEnum">The enum type.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustEnumClauses.NotHasDescription"/>.
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Status).NotHasDescription();
    /// </code>
    /// </example>
    /// <seealso cref="MustEnumClauses.NotHasDescription"/>
    public static IRuleBuilderOptions<TModel, TEnum> NotHasDescription<TModel, TEnum>(
        this IRuleBuilder<TModel, TEnum> ruleBuilder,
        string? message = null)
        where TEnum : struct, Enum =>
        ruleBuilder.MustBe(val => Must.Be.NotHasDescription(val, paramName: null),
            message);

    /// <summary>
    /// Validates that the nullable enum property value has a <see cref="System.ComponentModel.DataAnnotations.DisplayAttribute"/> applied.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <typeparam name="TEnum">The enum type.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustEnumClauses.HasDisplay"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Status).HasDisplay();
    /// </code>
    /// </example>
    /// <seealso cref="MustEnumClauses.HasDisplay"/>
    public static IRuleBuilderOptions<TModel, TEnum?> HasDisplay<TModel, TEnum>(
        this IRuleBuilder<TModel, TEnum?> ruleBuilder,
        string? message = null)
        where TEnum : struct, Enum =>
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.HasDisplay(val.Value, paramName: null) : MustResult<TEnum>.Ok(default),
            message);

    /// <summary>
    /// Validates that the non-nullable enum property value has a <see cref="System.ComponentModel.DataAnnotations.DisplayAttribute"/> applied.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <typeparam name="TEnum">The enum type.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustEnumClauses.HasDisplay"/>.
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Status).HasDisplay();
    /// </code>
    /// </example>
    /// <seealso cref="MustEnumClauses.HasDisplay"/>
    public static IRuleBuilderOptions<TModel, TEnum> HasDisplay<TModel, TEnum>(
        this IRuleBuilder<TModel, TEnum> ruleBuilder,
        string? message = null)
        where TEnum : struct, Enum =>
        ruleBuilder.MustBe(val => Must.Be.HasDisplay(val, paramName: null),
            message);

    /// <summary>
    /// Validates that the nullable enum property value does not have a <see cref="System.ComponentModel.DataAnnotations.DisplayAttribute"/> applied.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <typeparam name="TEnum">The enum type.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustEnumClauses.NotHasDisplay"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Status).NotHasDisplay();
    /// </code>
    /// </example>
    /// <seealso cref="MustEnumClauses.NotHasDisplay"/>
    public static IRuleBuilderOptions<TModel, TEnum?> NotHasDisplay<TModel, TEnum>(
        this IRuleBuilder<TModel, TEnum?> ruleBuilder,
        string? message = null)
        where TEnum : struct, Enum =>
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.NotHasDisplay(val.Value, paramName: null) : MustResult<TEnum>.Ok(default),
            message);

    /// <summary>
    /// Validates that the non-nullable enum property value does not have a <see cref="System.ComponentModel.DataAnnotations.DisplayAttribute"/> applied.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <typeparam name="TEnum">The enum type.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustEnumClauses.NotHasDisplay"/>.
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Status).NotHasDisplay();
    /// </code>
    /// </example>
    /// <seealso cref="MustEnumClauses.NotHasDisplay"/>
    public static IRuleBuilderOptions<TModel, TEnum> NotHasDisplay<TModel, TEnum>(
        this IRuleBuilder<TModel, TEnum> ruleBuilder,
        string? message = null)
        where TEnum : struct, Enum =>
        ruleBuilder.MustBe(val => Must.Be.NotHasDisplay(val, paramName: null),
            message);

    /// <summary>
    /// Validates that the nullable enum property value has an <c>[EnumMember]</c> attribute applied.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <typeparam name="TEnum">The enum type.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustEnumClauses.HasEnumMember"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Status).HasEnumMember();
    /// </code>
    /// </example>
    /// <seealso cref="MustEnumClauses.HasEnumMember"/>
    public static IRuleBuilderOptions<TModel, TEnum?> HasEnumMember<TModel, TEnum>(
        this IRuleBuilder<TModel, TEnum?> ruleBuilder,
        string? message = null)
        where TEnum : struct, Enum =>
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.HasEnumMember(val.Value, paramName: null) : MustResult<TEnum>.Ok(default),
            message);

    /// <summary>
    /// Validates that the non-nullable enum property value has an <c>[EnumMember]</c> attribute applied.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <typeparam name="TEnum">The enum type.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustEnumClauses.HasEnumMember"/>.
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Status).HasEnumMember();
    /// </code>
    /// </example>
    /// <seealso cref="MustEnumClauses.HasEnumMember"/>
    public static IRuleBuilderOptions<TModel, TEnum> HasEnumMember<TModel, TEnum>(
        this IRuleBuilder<TModel, TEnum> ruleBuilder,
        string? message = null)
        where TEnum : struct, Enum =>
        ruleBuilder.MustBe(val => Must.Be.HasEnumMember(val, paramName: null),
            message);

    /// <summary>
    /// Validates that the nullable enum property value does not have an <c>[EnumMember]</c> attribute applied.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <typeparam name="TEnum">The enum type.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustEnumClauses.NotHasEnumMember"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Status).NotHasEnumMember();
    /// </code>
    /// </example>
    /// <seealso cref="MustEnumClauses.NotHasEnumMember"/>
    public static IRuleBuilderOptions<TModel, TEnum?> NotHasEnumMember<TModel, TEnum>(
        this IRuleBuilder<TModel, TEnum?> ruleBuilder,
        string? message = null)
        where TEnum : struct, Enum =>
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.NotHasEnumMember(val.Value, paramName: null) : MustResult<TEnum>.Ok(default),
            message);

    /// <summary>
    /// Validates that the non-nullable enum property value does not have an <c>[EnumMember]</c> attribute applied.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <typeparam name="TEnum">The enum type.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustEnumClauses.NotHasEnumMember"/>.
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Status).NotHasEnumMember();
    /// </code>
    /// </example>
    /// <seealso cref="MustEnumClauses.NotHasEnumMember"/>
    public static IRuleBuilderOptions<TModel, TEnum> NotHasEnumMember<TModel, TEnum>(
        this IRuleBuilder<TModel, TEnum> ruleBuilder,
        string? message = null)
        where TEnum : struct, Enum =>
        ruleBuilder.MustBe(val => Must.Be.NotHasEnumMember(val, paramName: null),
            message);

    /// <summary>
    /// Validates that the nullable enum property value has an <see cref="ObsoleteAttribute"/> applied.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <typeparam name="TEnum">The enum type.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustEnumClauses.Obsolete"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.LegacyStatus).Obsolete();
    /// </code>
    /// </example>
    /// <seealso cref="MustEnumClauses.Obsolete"/>
    public static IRuleBuilderOptions<TModel, TEnum?> Obsolete<TModel, TEnum>(
        this IRuleBuilder<TModel, TEnum?> ruleBuilder,
        string? message = null)
        where TEnum : struct, Enum =>
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.Obsolete(val.Value, paramName: null) : MustResult<TEnum>.Ok(default),
            message);

    /// <summary>
    /// Validates that the non-nullable enum property value has an <see cref="ObsoleteAttribute"/> applied.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <typeparam name="TEnum">The enum type.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustEnumClauses.Obsolete"/>.
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.LegacyStatus).Obsolete();
    /// </code>
    /// </example>
    /// <seealso cref="MustEnumClauses.Obsolete"/>
    public static IRuleBuilderOptions<TModel, TEnum> Obsolete<TModel, TEnum>(
        this IRuleBuilder<TModel, TEnum> ruleBuilder,
        string? message = null)
        where TEnum : struct, Enum =>
        ruleBuilder.MustBe(val => Must.Be.Obsolete(val, paramName: null),
            message);

    /// <summary>
    /// Validates that the nullable enum property value does not have an <see cref="ObsoleteAttribute"/> applied.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <typeparam name="TEnum">The enum type.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustEnumClauses.NotObsolete"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Status).NotObsolete();
    /// </code>
    /// </example>
    /// <seealso cref="MustEnumClauses.NotObsolete"/>
    public static IRuleBuilderOptions<TModel, TEnum?> NotObsolete<TModel, TEnum>(
        this IRuleBuilder<TModel, TEnum?> ruleBuilder,
        string? message = null)
        where TEnum : struct, Enum =>
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.NotObsolete(val.Value, paramName: null) : MustResult<TEnum>.Ok(default),
            message);

    /// <summary>
    /// Validates that the non-nullable enum property value does not have an <see cref="ObsoleteAttribute"/> applied.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <typeparam name="TEnum">The enum type.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustEnumClauses.NotObsolete"/>.
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Status).NotObsolete();
    /// </code>
    /// </example>
    /// <seealso cref="MustEnumClauses.NotObsolete"/>
    public static IRuleBuilderOptions<TModel, TEnum> NotObsolete<TModel, TEnum>(
        this IRuleBuilder<TModel, TEnum> ruleBuilder,
        string? message = null)
        where TEnum : struct, Enum =>
        ruleBuilder.MustBe(val => Must.Be.NotObsolete(val, paramName: null),
            message);
}
