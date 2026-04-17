using FluentValidation;
using PineGuard.FluentValidation.Common;
using PineGuard.MustClauses;

namespace PineGuard.FluentValidation;

/// <summary>
/// Provides FluentValidation extension methods for <see cref="char"/> property validation.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/fluent/char">Fluent Char Extensions documentation</seealso>
public static class FluentCharExtensions
{
    /// <summary>
    /// Validates that the character is a Unicode letter.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>Delegates to <see cref="MustCharClauses.Letter"/>.</remarks>
    /// <example><code>RuleFor(x => x.Initial).Letter();</code></example>
    /// <seealso cref="MustCharClauses.Letter"/>
    public static IRuleBuilderOptions<TModel, char> Letter<TModel>(this IRuleBuilder<TModel, char> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.Letter(val, paramName: null),
            message);

    /// <summary>
    /// Validates that the character is not a Unicode letter.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>Delegates to <see cref="MustCharClauses.NotLetter"/>.</remarks>
    /// <example><code>RuleFor(x => x.Separator).NotLetter();</code></example>
    /// <seealso cref="MustCharClauses.NotLetter"/>
    public static IRuleBuilderOptions<TModel, char> NotLetter<TModel>(this IRuleBuilder<TModel, char> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.NotLetter(val, paramName: null),
            message);

    /// <summary>
    /// Validates that the character is a decimal digit (0-9).
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>Delegates to <see cref="MustCharClauses.Digit"/>.</remarks>
    /// <example><code>RuleFor(x => x.CheckDigit).Digit();</code></example>
    /// <seealso cref="MustCharClauses.Digit"/>
    public static IRuleBuilderOptions<TModel, char> Digit<TModel>(this IRuleBuilder<TModel, char> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.Digit(val, paramName: null),
            message);

    /// <summary>
    /// Validates that the character is not a decimal digit.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>Delegates to <see cref="MustCharClauses.NotDigit"/>.</remarks>
    /// <example><code>RuleFor(x => x.Prefix).NotDigit();</code></example>
    /// <seealso cref="MustCharClauses.NotDigit"/>
    public static IRuleBuilderOptions<TModel, char> NotDigit<TModel>(this IRuleBuilder<TModel, char> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.NotDigit(val, paramName: null),
            message);

    /// <summary>
    /// Validates that the character is a letter or digit.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>Delegates to <see cref="MustCharClauses.LetterOrDigit"/>.</remarks>
    /// <example><code>RuleFor(x => x.AlphaNum).LetterOrDigit();</code></example>
    /// <seealso cref="MustCharClauses.LetterOrDigit"/>
    public static IRuleBuilderOptions<TModel, char> LetterOrDigit<TModel>(this IRuleBuilder<TModel, char> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.LetterOrDigit(val, paramName: null),
            message);

    /// <summary>
    /// Validates that the character is not a letter or digit.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>Delegates to <see cref="MustCharClauses.NotLetterOrDigit"/>.</remarks>
    /// <example><code>RuleFor(x => x.Delimiter).NotLetterOrDigit();</code></example>
    /// <seealso cref="MustCharClauses.NotLetterOrDigit"/>
    public static IRuleBuilderOptions<TModel, char> NotLetterOrDigit<TModel>(this IRuleBuilder<TModel, char> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.NotLetterOrDigit(val, paramName: null),
            message);

    /// <summary>
    /// Validates that the character is within the ASCII range (0x00-0x7F).
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>Delegates to <see cref="MustCharClauses.Ascii"/>.</remarks>
    /// <example><code>RuleFor(x => x.Code).Ascii();</code></example>
    /// <seealso cref="MustCharClauses.Ascii"/>
    public static IRuleBuilderOptions<TModel, char> Ascii<TModel>(this IRuleBuilder<TModel, char> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.Ascii(val, paramName: null),
            message);

    /// <summary>
    /// Validates that the character is outside the ASCII range.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>Delegates to <see cref="MustCharClauses.NotAscii"/>.</remarks>
    /// <example><code>RuleFor(x => x.Symbol).NotAscii();</code></example>
    /// <seealso cref="MustCharClauses.NotAscii"/>
    public static IRuleBuilderOptions<TModel, char> NotAscii<TModel>(this IRuleBuilder<TModel, char> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.NotAscii(val, paramName: null),
            message);

    /// <summary>
    /// Validates that the character is a printable ASCII character (0x20-0x7E).
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>Delegates to <see cref="MustCharClauses.PrintableAscii"/>.</remarks>
    /// <example><code>RuleFor(x => x.DisplayChar).PrintableAscii();</code></example>
    /// <seealso cref="MustCharClauses.PrintableAscii"/>
    public static IRuleBuilderOptions<TModel, char> PrintableAscii<TModel>(this IRuleBuilder<TModel, char> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.PrintableAscii(val, paramName: null),
            message);

    /// <summary>
    /// Validates that the character is not a printable ASCII character.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>Delegates to <see cref="MustCharClauses.NotPrintableAscii"/>.</remarks>
    /// <example><code>RuleFor(x => x.ControlChar).NotPrintableAscii();</code></example>
    /// <seealso cref="MustCharClauses.NotPrintableAscii"/>
    public static IRuleBuilderOptions<TModel, char> NotPrintableAscii<TModel>(this IRuleBuilder<TModel, char> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.NotPrintableAscii(val, paramName: null),
            message);

    /// <summary>
    /// Validates that the character is not whitespace.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>Delegates to <see cref="MustCharClauses.NotWhitespace"/>.</remarks>
    /// <example><code>RuleFor(x => x.Token).NotWhitespace();</code></example>
    /// <seealso cref="MustCharClauses.NotWhitespace"/>
    public static IRuleBuilderOptions<TModel, char> NotWhitespace<TModel>(this IRuleBuilder<TModel, char> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.NotWhitespace(val, paramName: null),
            message);

    /// <summary>
    /// Validates that the character is a Unicode control character.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>Delegates to <see cref="MustCharClauses.Control"/>.</remarks>
    /// <example><code>RuleFor(x => x.Terminator).Control();</code></example>
    /// <seealso cref="MustCharClauses.Control"/>
    public static IRuleBuilderOptions<TModel, char> Control<TModel>(this IRuleBuilder<TModel, char> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.Control(val, paramName: null),
            message);

    /// <summary>
    /// Validates that the character is not a Unicode control character.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>Delegates to <see cref="MustCharClauses.NotControl"/>.</remarks>
    /// <example><code>RuleFor(x => x.DisplayChar).NotControl();</code></example>
    /// <seealso cref="MustCharClauses.NotControl"/>
    public static IRuleBuilderOptions<TModel, char> NotControl<TModel>(this IRuleBuilder<TModel, char> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.NotControl(val, paramName: null),
            message);

    /// <summary>
    /// Validates that the character is an uppercase letter.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>Delegates to <see cref="MustCharClauses.Uppercase"/>.</remarks>
    /// <example><code>RuleFor(x => x.Grade).Uppercase();</code></example>
    /// <seealso cref="MustCharClauses.Uppercase"/>
    public static IRuleBuilderOptions<TModel, char> Uppercase<TModel>(this IRuleBuilder<TModel, char> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.Uppercase(val, paramName: null),
            message);

    /// <summary>
    /// Validates that the character is a lowercase letter.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>Delegates to <see cref="MustCharClauses.Lowercase"/>.</remarks>
    /// <example><code>RuleFor(x => x.Suffix).Lowercase();</code></example>
    /// <seealso cref="MustCharClauses.Lowercase"/>
    public static IRuleBuilderOptions<TModel, char> Lowercase<TModel>(this IRuleBuilder<TModel, char> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.Lowercase(val, paramName: null),
            message);

    /// <summary>
    /// Validates that the character is a hexadecimal digit (0-9, A-F, a-f).
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>Delegates to <see cref="MustCharClauses.HexDigit"/>.</remarks>
    /// <example><code>RuleFor(x => x.HexChar).HexDigit();</code></example>
    /// <seealso cref="MustCharClauses.HexDigit"/>
    public static IRuleBuilderOptions<TModel, char> HexDigit<TModel>(this IRuleBuilder<TModel, char> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.HexDigit(val, paramName: null),
            message);

    /// <summary>
    /// Validates that the character is not a hexadecimal digit.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>Delegates to <see cref="MustCharClauses.NotHexDigit"/>.</remarks>
    /// <example><code>RuleFor(x => x.Separator).NotHexDigit();</code></example>
    /// <seealso cref="MustCharClauses.NotHexDigit"/>
    public static IRuleBuilderOptions<TModel, char> NotHexDigit<TModel>(this IRuleBuilder<TModel, char> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.NotHexDigit(val, paramName: null),
            message);
}
