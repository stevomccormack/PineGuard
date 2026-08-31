using FluentValidation;
using PineGuard.Codes;
using PineGuard.FluentValidation.Common;
using PineGuard.MustClauses;

namespace PineGuard.FluentValidation;

/// <summary>
/// Provides FluentValidation extension methods for version string property validation.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/fluent/version">Fluent Version Extensions documentation</seealso>
public static class FluentVersionExtensions
{
    /// <summary>
    /// Validates that the property value is a Semantic Versioning 2.0.0 version.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustVersionClauses.SemVer"/>, so all three numeric components are required and a
    /// leading <c>v</c> is rejected as a packaging convention rather than part of the specification. If the value
    /// is <see langword="null"/>, validation passes (null values should be handled by a separate
    /// <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.PackageVersion).SemVer();
    /// </code>
    /// </example>
    /// <seealso cref="MustVersionClauses.SemVer"/>
    public static IRuleBuilderOptions<TModel, string?> SemVer<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.SemVer(val, paramName: null) : MustResult<string>.Ok(null!),
            message, MustCodes.Version.Semver.Invalid);
}
