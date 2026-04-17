using FluentValidation;
using PineGuard.Common;
using PineGuard.FluentValidation.Common;
using PineGuard.MustClauses;

namespace PineGuard.FluentValidation;

/// <summary>
/// Provides FluentValidation extension methods for string casing style validation.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/fluent/string-casing">Fluent String Casing Extensions documentation</seealso>
public static class FluentStringCasingExtensions
{
    /// <summary>
    /// Validates that the property value matches the specified <see cref="StringCasing"/> style.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="style">The <see cref="StringCasing"/> style the value must conform to.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustStringCasingClauses.CaseStyle"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.FieldName).CaseStyle(StringCasing.CamelCase);
    /// </code>
    /// </example>
    /// <seealso cref="MustStringCasingClauses.CaseStyle"/>
    public static IRuleBuilderOptions<TModel, string?> CaseStyle<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        StringCasing style,
        string? message = null) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.CaseStyle(val, style, paramName: null) : MustResult<string>.Ok(null!),
            message);

    /// <summary>
    /// Validates that the property value does not match the specified <see cref="StringCasing"/> style.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="style">The <see cref="StringCasing"/> style the value must not conform to.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustStringCasingClauses.NotCaseStyle"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.FieldName).NotCaseStyle(StringCasing.PascalCase);
    /// </code>
    /// </example>
    /// <seealso cref="MustStringCasingClauses.NotCaseStyle"/>
    public static IRuleBuilderOptions<TModel, string?> NotCaseStyle<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        StringCasing style,
        string? message = null) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.NotCaseStyle(val, style, paramName: null) : MustResult<string>.Ok(null!),
            message);

    /// <summary>
    /// Validates that the property value is in camelCase format.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustStringCasingClauses.CamelCase"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.PropertyName).CamelCase();
    /// </code>
    /// </example>
    /// <seealso cref="MustStringCasingClauses.CamelCase"/>
    public static IRuleBuilderOptions<TModel, string?> CamelCase<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.CamelCase(val, paramName: null) : MustResult<string>.Ok(null!),
            message);

    /// <summary>
    /// Validates that the property value is not in camelCase format.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustStringCasingClauses.NotCamelCase"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.PropertyName).NotCamelCase();
    /// </code>
    /// </example>
    /// <seealso cref="MustStringCasingClauses.NotCamelCase"/>
    public static IRuleBuilderOptions<TModel, string?> NotCamelCase<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.NotCamelCase(val, paramName: null) : MustResult<string>.Ok(null!),
            message);

    /// <summary>
    /// Validates that the property value is in PascalCase format.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustStringCasingClauses.PascalCase"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.TypeName).PascalCase();
    /// </code>
    /// </example>
    /// <seealso cref="MustStringCasingClauses.PascalCase"/>
    public static IRuleBuilderOptions<TModel, string?> PascalCase<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.PascalCase(val, paramName: null) : MustResult<string>.Ok(null!),
            message);

    /// <summary>
    /// Validates that the property value is not in PascalCase format.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustStringCasingClauses.NotPascalCase"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.TypeName).NotPascalCase();
    /// </code>
    /// </example>
    /// <seealso cref="MustStringCasingClauses.NotPascalCase"/>
    public static IRuleBuilderOptions<TModel, string?> NotPascalCase<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.NotPascalCase(val, paramName: null) : MustResult<string>.Ok(null!),
            message);

    /// <summary>
    /// Validates that the property value is in snake_case format.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustStringCasingClauses.SnakeCase"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.ColumnName).SnakeCase();
    /// </code>
    /// </example>
    /// <seealso cref="MustStringCasingClauses.SnakeCase"/>
    public static IRuleBuilderOptions<TModel, string?> SnakeCase<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.SnakeCase(val, paramName: null) : MustResult<string>.Ok(null!),
            message);

    /// <summary>
    /// Validates that the property value is not in snake_case format.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustStringCasingClauses.NotSnakeCase"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.ColumnName).NotSnakeCase();
    /// </code>
    /// </example>
    /// <seealso cref="MustStringCasingClauses.NotSnakeCase"/>
    public static IRuleBuilderOptions<TModel, string?> NotSnakeCase<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.NotSnakeCase(val, paramName: null) : MustResult<string>.Ok(null!),
            message);

    /// <summary>
    /// Validates that the property value is in UPPER_SNAKE_CASE format.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustStringCasingClauses.UpperSnakeCase"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.ConstantName).UpperSnakeCase();
    /// </code>
    /// </example>
    /// <seealso cref="MustStringCasingClauses.UpperSnakeCase"/>
    public static IRuleBuilderOptions<TModel, string?> UpperSnakeCase<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.UpperSnakeCase(val, paramName: null) : MustResult<string>.Ok(null!),
            message);

    /// <summary>
    /// Validates that the property value is not in UPPER_SNAKE_CASE format.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustStringCasingClauses.NotUpperSnakeCase"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.ConstantName).NotUpperSnakeCase();
    /// </code>
    /// </example>
    /// <seealso cref="MustStringCasingClauses.NotUpperSnakeCase"/>
    public static IRuleBuilderOptions<TModel, string?> NotUpperSnakeCase<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.NotUpperSnakeCase(val, paramName: null) : MustResult<string>.Ok(null!),
            message);

    /// <summary>
    /// Validates that the property value is in kebab-case format.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustStringCasingClauses.KebabCase"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.UrlSlug).KebabCase();
    /// </code>
    /// </example>
    /// <seealso cref="MustStringCasingClauses.KebabCase"/>
    public static IRuleBuilderOptions<TModel, string?> KebabCase<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.KebabCase(val, paramName: null) : MustResult<string>.Ok(null!),
            message);

    /// <summary>
    /// Validates that the property value is not in kebab-case format.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustStringCasingClauses.NotKebabCase"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.UrlSlug).NotKebabCase();
    /// </code>
    /// </example>
    /// <seealso cref="MustStringCasingClauses.NotKebabCase"/>
    public static IRuleBuilderOptions<TModel, string?> NotKebabCase<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.NotKebabCase(val, paramName: null) : MustResult<string>.Ok(null!),
            message);

    /// <summary>
    /// Validates that the property value is in Train-Case format.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustStringCasingClauses.TrainCase"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.HeaderName).TrainCase();
    /// </code>
    /// </example>
    /// <seealso cref="MustStringCasingClauses.TrainCase"/>
    public static IRuleBuilderOptions<TModel, string?> TrainCase<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.TrainCase(val, paramName: null) : MustResult<string>.Ok(null!),
            message);

    /// <summary>
    /// Validates that the property value is not in Train-Case format.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustStringCasingClauses.NotTrainCase"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.HeaderName).NotTrainCase();
    /// </code>
    /// </example>
    /// <seealso cref="MustStringCasingClauses.NotTrainCase"/>
    public static IRuleBuilderOptions<TModel, string?> NotTrainCase<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.NotTrainCase(val, paramName: null) : MustResult<string>.Ok(null!),
            message);

    /// <summary>
    /// Validates that the property value is in <c>dot.case</c> format.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustStringCasingClauses.DotCase"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.NamespacePath).DotCase();
    /// </code>
    /// </example>
    /// <seealso cref="MustStringCasingClauses.DotCase"/>
    public static IRuleBuilderOptions<TModel, string?> DotCase<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.DotCase(val, paramName: null) : MustResult<string>.Ok(null!),
            message);

    /// <summary>
    /// Validates that the property value is not in <c>dot.case</c> format.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustStringCasingClauses.NotDotCase"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.NamespacePath).NotDotCase();
    /// </code>
    /// </example>
    /// <seealso cref="MustStringCasingClauses.NotDotCase"/>
    public static IRuleBuilderOptions<TModel, string?> NotDotCase<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.NotDotCase(val, paramName: null) : MustResult<string>.Ok(null!),
            message);

    /// <summary>
    /// Validates that the property value is in space case format.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustStringCasingClauses.SpaceCase"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.DisplayLabel).SpaceCase();
    /// </code>
    /// </example>
    /// <seealso cref="MustStringCasingClauses.SpaceCase"/>
    public static IRuleBuilderOptions<TModel, string?> SpaceCase<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.SpaceCase(val, paramName: null) : MustResult<string>.Ok(null!),
            message);

    /// <summary>
    /// Validates that the property value is not in space case format.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustStringCasingClauses.NotSpaceCase"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.DisplayLabel).NotSpaceCase();
    /// </code>
    /// </example>
    /// <seealso cref="MustStringCasingClauses.NotSpaceCase"/>
    public static IRuleBuilderOptions<TModel, string?> NotSpaceCase<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.NotSpaceCase(val, paramName: null) : MustResult<string>.Ok(null!),
            message);

    /// <summary>
    /// Validates that the property value is entirely uppercase (culture-invariant).
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustStringCasingClauses.UpperInvariant"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.CountryCode).UpperInvariant();
    /// </code>
    /// </example>
    /// <seealso cref="MustStringCasingClauses.UpperInvariant"/>
    public static IRuleBuilderOptions<TModel, string?> UpperInvariant<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.UpperInvariant(val, paramName: null) : MustResult<string>.Ok(null!),
            message);

    /// <summary>
    /// Validates that the property value is not entirely uppercase (culture-invariant).
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustStringCasingClauses.NotUpperInvariant"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.CountryCode).NotUpperInvariant();
    /// </code>
    /// </example>
    /// <seealso cref="MustStringCasingClauses.NotUpperInvariant"/>
    public static IRuleBuilderOptions<TModel, string?> NotUpperInvariant<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.NotUpperInvariant(val, paramName: null) : MustResult<string>.Ok(null!),
            message);

    /// <summary>
    /// Validates that the property value is entirely lowercase (culture-invariant).
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustStringCasingClauses.LowerInvariant"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.EmailAddress).LowerInvariant();
    /// </code>
    /// </example>
    /// <seealso cref="MustStringCasingClauses.LowerInvariant"/>
    public static IRuleBuilderOptions<TModel, string?> LowerInvariant<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.LowerInvariant(val, paramName: null) : MustResult<string>.Ok(null!),
            message);

    /// <summary>
    /// Validates that the property value is not entirely lowercase (culture-invariant).
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustStringCasingClauses.NotLowerInvariant"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.EmailAddress).NotLowerInvariant();
    /// </code>
    /// </example>
    /// <seealso cref="MustStringCasingClauses.NotLowerInvariant"/>
    public static IRuleBuilderOptions<TModel, string?> NotLowerInvariant<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.NotLowerInvariant(val, paramName: null) : MustResult<string>.Ok(null!),
            message);
}
