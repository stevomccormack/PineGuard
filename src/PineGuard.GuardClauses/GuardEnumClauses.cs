using System.Runtime.CompilerServices;
using PineGuard.MustClauses;

namespace PineGuard.GuardClauses;

/// <summary>
/// Guard clauses for <see langword="enum"/> values.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/guard/enum">Guard Enum Clauses documentation</seealso>
public static class GuardEnumClauses
{
    /// <summary>
    /// Throws if <paramref name="value"/> is not defined in <typeparamref name="TEnum"/>.
    /// </summary>
    /// <typeparam name="TEnum">The enum type.</typeparam>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The enum value to guard.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustEnumClauses.Defined{TEnum}"/>.
    /// </param>
    /// <param name="exceptionCreator">
    /// An optional factory to create a custom exception. If <see langword="null"/>,
    /// throws <see cref="ArgumentException"/> via <see cref="GuardFailure.Throw"/>.
    /// </param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated value of <paramref name="value"/> if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="value"/> is not a defined enum member and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustEnumClauses.Defined{TEnum}"/>:
    /// <c>Guard.Against.NotDefined</c> passes when the value is a defined enum member.
    /// </remarks>
    /// <example>
    /// <code>
    /// Guard.Against.NotDefined(status);
    /// </code>
    /// </example>
    /// <seealso cref="MustEnumClauses.Defined{TEnum}"/>
    public static TEnum NotDefined<TEnum>(this IGuardClause _,
        TEnum value,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where TEnum : struct, Enum
    {
        var result = Must.Be.Defined(value, paramName);
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> does not correspond to a defined numeric value in <typeparamref name="TEnum"/>.
    /// </summary>
    /// <typeparam name="TEnum">The enum type.</typeparam>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The integer value to guard.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustEnumClauses.DefinedValue{TEnum}"/>.
    /// </param>
    /// <param name="exceptionCreator">
    /// An optional factory to create a custom exception. If <see langword="null"/>,
    /// throws <see cref="ArgumentException"/> via <see cref="GuardFailure.Throw"/>.
    /// </param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated value of <paramref name="value"/> if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="value"/> is not a defined enum value and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustEnumClauses.DefinedValue{TEnum}"/>:
    /// <c>Guard.Against.NotDefinedValue</c> passes when the integer maps to a defined enum member.
    /// </remarks>
    /// <example>
    /// <code><![CDATA[
    /// Guard.Against.NotDefinedValue<MyEnum>(statusCode);
    /// ]]></code>
    /// </example>
    /// <seealso cref="MustEnumClauses.DefinedValue{TEnum}"/>
    public static int NotDefinedValue<TEnum>(this IGuardClause _,
        int value,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where TEnum : struct, Enum
    {
        var result = Must.Be.DefinedValue<TEnum>(value, paramName);
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result;
    }

    /// <summary>
    /// Throws if <paramref name="name"/> does not match a defined member name in <typeparamref name="TEnum"/>.
    /// </summary>
    /// <typeparam name="TEnum">The enum type.</typeparam>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="name">The enum member name string to guard.</param>
    /// <param name="ignoreCase">Whether to ignore case when matching names. Defaults to <see langword="true"/>.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustEnumClauses.DefinedName{TEnum}"/>.
    /// </param>
    /// <param name="exceptionCreator">
    /// An optional factory to create a custom exception. If <see langword="null"/>,
    /// throws <see cref="ArgumentException"/> via <see cref="GuardFailure.Throw"/>.
    /// </param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated value of <paramref name="name"/> if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="name"/> is not a defined enum member name and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustEnumClauses.DefinedName{TEnum}"/>:
    /// <c>Guard.Against.NotDefinedName</c> passes when the name matches a defined member.
    /// </remarks>
    /// <example>
    /// <code><![CDATA[
    /// Guard.Against.NotDefinedName<MyEnum>(rawName);
    /// ]]></code>
    /// </example>
    /// <seealso cref="MustEnumClauses.DefinedName{TEnum}"/>
    public static string NotDefinedName<TEnum>(this IGuardClause _,
        string? name,
        bool ignoreCase = true,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(name))] string? paramName = null)
        where TEnum : struct, Enum
    {
        var result = Must.Be.DefinedName<TEnum>(name, ignoreCase, paramName);
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result!;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> is not a valid flags combination for <typeparamref name="TEnum"/>.
    /// </summary>
    /// <typeparam name="TEnum">The flags enum type.</typeparam>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The enum value to guard.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustEnumClauses.FlagsEnumCombination{TEnum}"/>.
    /// </param>
    /// <param name="exceptionCreator">
    /// An optional factory to create a custom exception. If <see langword="null"/>,
    /// throws <see cref="ArgumentException"/> via <see cref="GuardFailure.Throw"/>.
    /// </param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated value of <paramref name="value"/> if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="value"/> is not a valid flags combination and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustEnumClauses.FlagsEnumCombination{TEnum}"/>:
    /// <c>Guard.Against.NotFlagsEnumCombination</c> passes when all set bits represent defined members.
    /// </remarks>
    /// <example>
    /// <code>
    /// Guard.Against.NotFlagsEnumCombination(permissions);
    /// </code>
    /// </example>
    /// <seealso cref="MustEnumClauses.FlagsEnumCombination{TEnum}"/>
    public static TEnum NotFlagsEnumCombination<TEnum>(this IGuardClause _,
        TEnum value,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where TEnum : struct, Enum
    {
        var result = Must.Be.FlagsEnumCombination(value, paramName);
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> does not have a <typeparamref name="TAttribute"/> applied.
    /// </summary>
    /// <typeparam name="TEnum">The enum type.</typeparam>
    /// <typeparam name="TAttribute">The attribute type to look for.</typeparam>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The enum value to guard.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustEnumClauses.HasAttribute{TEnum,TAttribute}"/>.
    /// </param>
    /// <param name="exceptionCreator">
    /// An optional factory to create a custom exception. If <see langword="null"/>,
    /// throws <see cref="ArgumentException"/> via <see cref="GuardFailure.Throw"/>.
    /// </param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated value of <paramref name="value"/> if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when the attribute is absent and no <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustEnumClauses.HasAttribute{TEnum,TAttribute}"/>:
    /// <c>Guard.Against.NotHasAttribute</c> passes when the member has the attribute.
    /// </remarks>
    /// <example>
    /// <code><![CDATA[
    /// Guard.Against.NotHasAttribute<MyEnum, DescriptionAttribute>(value);
    /// ]]></code>
    /// </example>
    /// <seealso cref="MustEnumClauses.HasAttribute{TEnum,TAttribute}"/>
    public static TEnum NotHasAttribute<TEnum, TAttribute>(this IGuardClause _,
        TEnum value,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where TEnum : struct, Enum
        where TAttribute : Attribute
    {
        var result = Must.Be.HasAttribute<TEnum, TAttribute>(value, paramName);
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return value;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> has a <typeparamref name="TAttribute"/> applied.
    /// </summary>
    /// <typeparam name="TEnum">The enum type.</typeparam>
    /// <typeparam name="TAttribute">The attribute type that must not be present.</typeparam>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The enum value to guard.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustEnumClauses.NotHasAttribute{TEnum,TAttribute}"/>.
    /// </param>
    /// <param name="exceptionCreator">
    /// An optional factory to create a custom exception. If <see langword="null"/>,
    /// throws <see cref="ArgumentException"/> via <see cref="GuardFailure.Throw"/>.
    /// </param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated value of <paramref name="value"/> if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when the attribute is present and no <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustEnumClauses.NotHasAttribute{TEnum,TAttribute}"/>:
    /// <c>Guard.Against.HasAttribute</c> passes when the attribute is absent.
    /// </remarks>
    /// <example>
    /// <code><![CDATA[
    /// Guard.Against.HasAttribute<MyEnum, ObsoleteAttribute>(value);
    /// ]]></code>
    /// </example>
    /// <seealso cref="MustEnumClauses.NotHasAttribute{TEnum,TAttribute}"/>
    public static TEnum HasAttribute<TEnum, TAttribute>(this IGuardClause _,
        TEnum value,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where TEnum : struct, Enum
        where TAttribute : Attribute
    {
        var result = Must.Be.NotHasAttribute<TEnum, TAttribute>(value, paramName);
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> does not have <paramref name="flag"/> set.
    /// </summary>
    /// <typeparam name="TEnum">The flags enum type.</typeparam>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The enum value to guard.</param>
    /// <param name="flag">The flag that must be set.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustEnumClauses.HasFlag{TEnum}"/>.
    /// </param>
    /// <param name="exceptionCreator">
    /// An optional factory to create a custom exception. If <see langword="null"/>,
    /// throws <see cref="ArgumentException"/> via <see cref="GuardFailure.Throw"/>.
    /// </param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated value of <paramref name="value"/> if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when the flag is not set and no <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustEnumClauses.HasFlag{TEnum}"/>:
    /// <c>Guard.Against.NotHasFlag</c> passes when the flag is set.
    /// </remarks>
    /// <example>
    /// <code>
    /// Guard.Against.NotHasFlag(permissions, Permission.Read);
    /// </code>
    /// </example>
    /// <seealso cref="MustEnumClauses.HasFlag{TEnum}"/>
    public static TEnum NotHasFlag<TEnum>(this IGuardClause _,
        TEnum value,
        TEnum flag,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where TEnum : struct, Enum
    {
        var result = Must.Be.HasFlag(value, flag, paramName);
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return value;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> has <paramref name="flag"/> set.
    /// </summary>
    /// <typeparam name="TEnum">The flags enum type.</typeparam>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The enum value to guard.</param>
    /// <param name="flag">The flag that must not be set.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustEnumClauses.NotHasFlag{TEnum}"/>.
    /// </param>
    /// <param name="exceptionCreator">
    /// An optional factory to create a custom exception. If <see langword="null"/>,
    /// throws <see cref="ArgumentException"/> via <see cref="GuardFailure.Throw"/>.
    /// </param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated value of <paramref name="value"/> if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when the flag is set and no <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustEnumClauses.NotHasFlag{TEnum}"/>:
    /// <c>Guard.Against.HasFlag</c> passes when the flag is not set.
    /// </remarks>
    /// <example>
    /// <code>
    /// Guard.Against.HasFlag(permissions, Permission.Admin);
    /// </code>
    /// </example>
    /// <seealso cref="MustEnumClauses.NotHasFlag{TEnum}"/>
    public static TEnum HasFlag<TEnum>(this IGuardClause _,
        TEnum value,
        TEnum flag,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where TEnum : struct, Enum
    {
        var result = Must.Be.NotHasFlag(value, flag, paramName);
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return value;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> does not have a <see cref="System.ComponentModel.DescriptionAttribute"/> applied.
    /// </summary>
    /// <typeparam name="TEnum">The enum type.</typeparam>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The enum value to guard.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustEnumClauses.HasDescription{TEnum}"/>.
    /// </param>
    /// <param name="exceptionCreator">
    /// An optional factory to create a custom exception. If <see langword="null"/>,
    /// throws <see cref="ArgumentException"/> via <see cref="GuardFailure.Throw"/>.
    /// </param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated value of <paramref name="value"/> if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when the description attribute is absent and no <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustEnumClauses.HasDescription{TEnum}"/>:
    /// <c>Guard.Against.NotHasDescription</c> passes when the member has a <c>[Description]</c> attribute.
    /// </remarks>
    /// <example>
    /// <code>
    /// Guard.Against.NotHasDescription(status);
    /// </code>
    /// </example>
    /// <seealso cref="MustEnumClauses.HasDescription{TEnum}"/>
    public static TEnum NotHasDescription<TEnum>(this IGuardClause _,
        TEnum value,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where TEnum : struct, Enum
    {
        var result = Must.Be.HasDescription(value, paramName);
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return value;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> has a <see cref="System.ComponentModel.DescriptionAttribute"/> applied.
    /// </summary>
    /// <typeparam name="TEnum">The enum type.</typeparam>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The enum value to guard.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustEnumClauses.NotHasDescription{TEnum}"/>.
    /// </param>
    /// <param name="exceptionCreator">
    /// An optional factory to create a custom exception. If <see langword="null"/>,
    /// throws <see cref="ArgumentException"/> via <see cref="GuardFailure.Throw"/>.
    /// </param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated value of <paramref name="value"/> if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when the description attribute is present and no <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustEnumClauses.NotHasDescription{TEnum}"/>:
    /// <c>Guard.Against.HasDescription</c> passes when the member has no <c>[Description]</c>.
    /// </remarks>
    /// <example>
    /// <code>
    /// Guard.Against.HasDescription(value);
    /// </code>
    /// </example>
    /// <seealso cref="MustEnumClauses.NotHasDescription{TEnum}"/>
    public static TEnum HasDescription<TEnum>(this IGuardClause _,
        TEnum value,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where TEnum : struct, Enum
    {
        var result = Must.Be.NotHasDescription(value, paramName);
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return value;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> does not have a <see cref="System.ComponentModel.DataAnnotations.DisplayAttribute"/> applied.
    /// </summary>
    /// <typeparam name="TEnum">The enum type.</typeparam>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The enum value to guard.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustEnumClauses.HasDisplay{TEnum}"/>.
    /// </param>
    /// <param name="exceptionCreator">
    /// An optional factory to create a custom exception. If <see langword="null"/>,
    /// throws <see cref="ArgumentException"/> via <see cref="GuardFailure.Throw"/>.
    /// </param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated value of <paramref name="value"/> if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when the display attribute is absent and no <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustEnumClauses.HasDisplay{TEnum}"/>:
    /// <c>Guard.Against.NotHasDisplay</c> passes when the member has a <c>[Display]</c> attribute.
    /// </remarks>
    /// <example>
    /// <code>
    /// Guard.Against.NotHasDisplay(status);
    /// </code>
    /// </example>
    /// <seealso cref="MustEnumClauses.HasDisplay{TEnum}"/>
    public static TEnum NotHasDisplay<TEnum>(this IGuardClause _,
        TEnum value,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where TEnum : struct, Enum
    {
        var result = Must.Be.HasDisplay(value, paramName);
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return value;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> has a <see cref="System.ComponentModel.DataAnnotations.DisplayAttribute"/> applied.
    /// </summary>
    /// <typeparam name="TEnum">The enum type.</typeparam>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The enum value to guard.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustEnumClauses.NotHasDisplay{TEnum}"/>.
    /// </param>
    /// <param name="exceptionCreator">
    /// An optional factory to create a custom exception. If <see langword="null"/>,
    /// throws <see cref="ArgumentException"/> via <see cref="GuardFailure.Throw"/>.
    /// </param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated value of <paramref name="value"/> if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when the display attribute is present and no <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustEnumClauses.NotHasDisplay{TEnum}"/>:
    /// <c>Guard.Against.HasDisplay</c> passes when the member has no <c>[Display]</c>.
    /// </remarks>
    /// <example>
    /// <code>
    /// Guard.Against.HasDisplay(value);
    /// </code>
    /// </example>
    /// <seealso cref="MustEnumClauses.NotHasDisplay{TEnum}"/>
    public static TEnum HasDisplay<TEnum>(this IGuardClause _,
        TEnum value,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where TEnum : struct, Enum
    {
        var result = Must.Be.NotHasDisplay(value, paramName);
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return value;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> does not have an <c>[EnumMember]</c> attribute applied.
    /// </summary>
    /// <typeparam name="TEnum">The enum type.</typeparam>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The enum value to guard.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustEnumClauses.HasEnumMember{TEnum}"/>.
    /// </param>
    /// <param name="exceptionCreator">
    /// An optional factory to create a custom exception. If <see langword="null"/>,
    /// throws <see cref="ArgumentException"/> via <see cref="GuardFailure.Throw"/>.
    /// </param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated value of <paramref name="value"/> if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when the <c>[EnumMember]</c> attribute is absent and no <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustEnumClauses.HasEnumMember{TEnum}"/>:
    /// <c>Guard.Against.NotHasEnumMember</c> passes when the member has an <c>[EnumMember]</c> attribute.
    /// </remarks>
    /// <example>
    /// <code>
    /// Guard.Against.NotHasEnumMember(status);
    /// </code>
    /// </example>
    /// <seealso cref="MustEnumClauses.HasEnumMember{TEnum}"/>
    public static TEnum NotHasEnumMember<TEnum>(this IGuardClause _,
        TEnum value,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where TEnum : struct, Enum
    {
        var result = Must.Be.HasEnumMember(value, paramName);
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return value;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> has an <c>[EnumMember]</c> attribute applied.
    /// </summary>
    /// <typeparam name="TEnum">The enum type.</typeparam>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The enum value to guard.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustEnumClauses.NotHasEnumMember{TEnum}"/>.
    /// </param>
    /// <param name="exceptionCreator">
    /// An optional factory to create a custom exception. If <see langword="null"/>,
    /// throws <see cref="ArgumentException"/> via <see cref="GuardFailure.Throw"/>.
    /// </param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated value of <paramref name="value"/> if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when the <c>[EnumMember]</c> attribute is present and no <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustEnumClauses.NotHasEnumMember{TEnum}"/>:
    /// <c>Guard.Against.HasEnumMember</c> passes when the attribute is absent.
    /// </remarks>
    /// <example>
    /// <code>
    /// Guard.Against.HasEnumMember(value);
    /// </code>
    /// </example>
    /// <seealso cref="MustEnumClauses.NotHasEnumMember{TEnum}"/>
    public static TEnum HasEnumMember<TEnum>(this IGuardClause _,
        TEnum value,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where TEnum : struct, Enum
    {
        var result = Must.Be.NotHasEnumMember(value, paramName);
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return value;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> is not marked with <see cref="ObsoleteAttribute"/>.
    /// </summary>
    /// <typeparam name="TEnum">The enum type.</typeparam>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The enum value to guard.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustEnumClauses.NotObsolete{TEnum}"/>.
    /// </param>
    /// <param name="exceptionCreator">
    /// An optional factory to create a custom exception. If <see langword="null"/>,
    /// throws <see cref="ArgumentException"/> via <see cref="GuardFailure.Throw"/>.
    /// </param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated value of <paramref name="value"/> if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when the member is not obsolete and no <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustEnumClauses.NotObsolete{TEnum}"/>:
    /// <c>Guard.Against.Obsolete</c> passes when the member is not obsolete.
    /// </remarks>
    /// <example>
    /// <code>
    /// Guard.Against.Obsolete(status);
    /// </code>
    /// </example>
    /// <seealso cref="MustEnumClauses.NotObsolete{TEnum}"/>
    public static TEnum Obsolete<TEnum>(this IGuardClause _,
        TEnum value,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where TEnum : struct, Enum
    {
        var result = Must.Be.NotObsolete(value, paramName);
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return value;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> is marked with <see cref="ObsoleteAttribute"/>.
    /// </summary>
    /// <typeparam name="TEnum">The enum type.</typeparam>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The enum value to guard.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustEnumClauses.Obsolete{TEnum}"/>.
    /// </param>
    /// <param name="exceptionCreator">
    /// An optional factory to create a custom exception. If <see langword="null"/>,
    /// throws <see cref="ArgumentException"/> via <see cref="GuardFailure.Throw"/>.
    /// </param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated value of <paramref name="value"/> if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when the member is obsolete and no <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustEnumClauses.Obsolete{TEnum}"/>:
    /// <c>Guard.Against.NotObsolete</c> passes when the member is obsolete (has <see cref="ObsoleteAttribute"/>).
    /// </remarks>
    /// <example>
    /// <code>
    /// Guard.Against.NotObsolete(value);
    /// </code>
    /// </example>
    /// <seealso cref="MustEnumClauses.Obsolete{TEnum}"/>
    public static TEnum NotObsolete<TEnum>(this IGuardClause _,
        TEnum value,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where TEnum : struct, Enum
    {
        var result = Must.Be.Obsolete(value, paramName);
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return value;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> is a defined enum member.
    /// </summary>
    /// <typeparam name="TEnum">The enum type.</typeparam>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The enum value to guard.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustEnumClauses.NotDefined{TEnum}"/>.
    /// </param>
    /// <param name="exceptionCreator">
    /// An optional factory to create a custom exception. If <see langword="null"/>,
    /// throws <see cref="ArgumentException"/> via <see cref="GuardFailure.Throw"/>.
    /// </param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated value of <paramref name="value"/> if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="value"/> is a defined member and no <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustEnumClauses.NotDefined{TEnum}"/>:
    /// <c>Guard.Against.Defined</c> passes when the value is not defined.
    /// </remarks>
    /// <example>
    /// <code>
    /// Guard.Against.Defined(value);
    /// </code>
    /// </example>
    /// <seealso cref="MustEnumClauses.NotDefined{TEnum}"/>
    public static TEnum Defined<TEnum>(this IGuardClause _,
        TEnum value,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where TEnum : struct, Enum
    {
        var result = Must.Be.NotDefined(value, paramName);
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> corresponds to a defined numeric value in <typeparamref name="TEnum"/>.
    /// </summary>
    /// <typeparam name="TEnum">The enum type.</typeparam>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The integer value to guard.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustEnumClauses.NotDefinedValue{TEnum}"/>.
    /// </param>
    /// <param name="exceptionCreator">
    /// An optional factory to create a custom exception. If <see langword="null"/>,
    /// throws <see cref="ArgumentException"/> via <see cref="GuardFailure.Throw"/>.
    /// </param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated value of <paramref name="value"/> if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="value"/> maps to a defined enum value and no <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustEnumClauses.NotDefinedValue{TEnum}"/>:
    /// <c>Guard.Against.DefinedValue</c> passes when the integer is not a defined value.
    /// </remarks>
    /// <example>
    /// <code><![CDATA[
    /// Guard.Against.DefinedValue<MyEnum>(rawInt);
    /// ]]></code>
    /// </example>
    /// <seealso cref="MustEnumClauses.NotDefinedValue{TEnum}"/>
    public static int DefinedValue<TEnum>(this IGuardClause _,
        int value,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where TEnum : struct, Enum
    {
        var result = Must.Be.NotDefinedValue<TEnum>(value, paramName);
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result;
    }

    /// <summary>
    /// Throws if <paramref name="name"/> matches a defined member name in <typeparamref name="TEnum"/>.
    /// </summary>
    /// <typeparam name="TEnum">The enum type.</typeparam>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="name">The enum member name string to guard.</param>
    /// <param name="ignoreCase">Whether to ignore case when matching names. Defaults to <see langword="true"/>.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustEnumClauses.NotDefinedName{TEnum}"/>.
    /// </param>
    /// <param name="exceptionCreator">
    /// An optional factory to create a custom exception. If <see langword="null"/>,
    /// throws <see cref="ArgumentException"/> via <see cref="GuardFailure.Throw"/>.
    /// </param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated value of <paramref name="name"/> if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="name"/> is a defined member name and no <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustEnumClauses.NotDefinedName{TEnum}"/>:
    /// <c>Guard.Against.DefinedName</c> passes when the name is not a defined member.
    /// </remarks>
    /// <example>
    /// <code><![CDATA[
    /// Guard.Against.DefinedName<MyEnum>(rawName);
    /// ]]></code>
    /// </example>
    /// <seealso cref="MustEnumClauses.NotDefinedName{TEnum}"/>
    public static string DefinedName<TEnum>(this IGuardClause _,
        string? name,
        bool ignoreCase = true,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(name))] string? paramName = null)
        where TEnum : struct, Enum
    {
        var result = Must.Be.NotDefinedName<TEnum>(name, ignoreCase, paramName);
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result!;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> is a valid flags combination for <typeparamref name="TEnum"/>.
    /// </summary>
    /// <typeparam name="TEnum">The flags enum type.</typeparam>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The enum value to guard.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustEnumClauses.NotFlagsEnumCombination{TEnum}"/>.
    /// </param>
    /// <param name="exceptionCreator">
    /// An optional factory to create a custom exception. If <see langword="null"/>,
    /// throws <see cref="ArgumentException"/> via <see cref="GuardFailure.Throw"/>.
    /// </param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated value of <paramref name="value"/> if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="value"/> is a valid combination and no <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustEnumClauses.NotFlagsEnumCombination{TEnum}"/>:
    /// <c>Guard.Against.FlagsEnumCombination</c> passes when the value is not a valid combination.
    /// </remarks>
    /// <example>
    /// <code>
    /// Guard.Against.FlagsEnumCombination(value);
    /// </code>
    /// </example>
    /// <seealso cref="MustEnumClauses.NotFlagsEnumCombination{TEnum}"/>
    public static TEnum FlagsEnumCombination<TEnum>(this IGuardClause _,
        TEnum value,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where TEnum : struct, Enum
    {
        var result = Must.Be.NotFlagsEnumCombination(value, paramName);
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result;
    }
}
