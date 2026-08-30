using FluentValidation;
using PineGuard.Codes;
using PineGuard.FluentValidation.Common;
using PineGuard.MustClauses;

namespace PineGuard.FluentValidation;

/// <summary>
/// Provides FluentValidation extension methods for the magic bytes a file leads with.
/// </summary>
/// <remarks>
/// These extensions validate the leading bytes of a file; reading those bytes from disk, a stream
/// or an upload is the caller's job.
/// </remarks>
/// <seealso href="https://pineguard.ai/docs/fluent/file-signature">Fluent File Signature Extensions documentation</seealso>
public static class FluentFileSignatureExtensions
{
    /// <summary>
    /// Validates that the property value matches the file signature registered for the declared extension.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="extension">
    /// The extension the file claims to have, with or without a leading dot and in any casing.
    /// It must be one of the extensions PineGuard registers a signature for; an unregistered extension
    /// fails the rule with a message naming <paramref name="extension"/> rather than the property.
    /// </param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustFileSignatureClauses.FileSignature"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.UploadedHeader).FileSignature(".png");
    /// </code>
    /// </example>
    /// <seealso cref="MustFileSignatureClauses.FileSignature"/>
    public static IRuleBuilderOptions<TModel, byte[]?> FileSignature<TModel>(this IRuleBuilder<TModel, byte[]?> ruleBuilder,
        string extension,
        string? message = null) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.FileSignature(val, extension, paramName: null) : MustResult<byte[]>.Ok(null!),
            message, MustCodes.File.Signature.Mismatch);

    /// <summary>
    /// Validates that the property value matches one of the registered file signatures.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustFileSignatureClauses.KnownFileSignature"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule). A passing rule proves
    /// only that the bytes lead with a known signature — never that the rest of the file is well-formed or safe.
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.UploadedHeader).KnownFileSignature();
    /// </code>
    /// </example>
    /// <seealso cref="MustFileSignatureClauses.KnownFileSignature"/>
    public static IRuleBuilderOptions<TModel, byte[]?> KnownFileSignature<TModel>(this IRuleBuilder<TModel, byte[]?> ruleBuilder,
        string? message = null) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.KnownFileSignature(val, paramName: null) : MustResult<byte[]>.Ok(null!),
            message, MustCodes.File.Signature.Unknown);
}
