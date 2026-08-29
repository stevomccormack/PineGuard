using Microsoft.Extensions.Options;
using PineGuard.Common;
using PineGuard.MustClauses;

namespace PineGuard.Extensions.Options;

/// <summary>
/// Adapts an <see cref="IMustValidator{T}"/> to <see cref="IValidateOptions{TOptions}"/>, so a
/// <c>Must.Be.*</c> validator can drive <c>IOptions&lt;TOptions&gt;</c> validation and, via
/// <c>ValidateOnStart()</c>, fail host start-up with every violation listed at once.
/// </summary>
/// <typeparam name="TOptions">The options type being validated.</typeparam>
/// <remarks>
/// <para>
/// Register this type through <see cref="OptionsBuilderExtension.ValidateMustRules{TOptions}(OptionsBuilder{TOptions})"/>
/// (or one of its overloads) rather than constructing it directly — the extension methods take care of
/// wiring <see cref="Name"/> to <c>OptionsBuilder&lt;TOptions&gt;.Name</c> and registering the instance as
/// <see cref="IValidateOptions{TOptions}"/> in the container.
/// </para>
/// <para>
/// Never catches or wraps an exception thrown by the underlying <see cref="IMustValidator{T}"/> — a
/// validator that throws (for example, an async-only validator invoked synchronously) is a programmer
/// error and surfaces as-is.
/// </para>
/// </remarks>
/// <seealso cref="IMustValidator{T}"/>
/// <seealso cref="OptionsBuilderExtension"/>
public sealed class MustRulesValidateOptions<TOptions> : IValidateOptions<TOptions>
    where TOptions : class
{
    private readonly IMustValidator<TOptions> _validator;

    /// <summary>
    /// Initializes a new instance of the <see cref="MustRulesValidateOptions{TOptions}"/> class.
    /// </summary>
    /// <param name="name">
    /// The named options instance this validator applies to, or <see langword="null"/> to validate
    /// every named instance of <typeparamref name="TOptions"/>.
    /// </param>
    /// <param name="validator">The validator that supplies the <c>Must.Be.*</c> rules for <typeparamref name="TOptions"/>.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="validator"/> is <see langword="null"/>.</exception>
    public MustRulesValidateOptions(string? name, IMustValidator<TOptions> validator)
    {
        ThrowHelper.ThrowIfNull(validator);

        Name = name;
        _validator = validator;
    }

    /// <summary>
    /// Gets the named options instance this validator applies to, or <see langword="null"/> when it
    /// applies to every named instance of <typeparamref name="TOptions"/>.
    /// </summary>
    public string? Name { get; }

    /// <summary>
    /// Validates <paramref name="options"/> against every rule registered on the underlying
    /// <see cref="IMustValidator{T}"/>.
    /// </summary>
    /// <param name="name">The name of the options instance being validated.</param>
    /// <param name="options">The options instance to validate.</param>
    /// <returns>
    /// <see cref="ValidateOptionsResult.Skip"/> when <see cref="Name"/> is set and does not match
    /// <paramref name="name"/>; otherwise <see cref="ValidateOptionsResult.Success"/> when every rule
    /// passes, or <see cref="ValidateOptionsResult.Fail(IEnumerable{string})"/> listing every failure —
    /// formatted by <see cref="FormatFailure"/> — in the order the validator reported them.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="options"/> is <see langword="null"/>.</exception>
    public ValidateOptionsResult Validate(string? name, TOptions options)
    {
        if (Name is not null && !string.Equals(name, Name, StringComparison.Ordinal))
            return ValidateOptionsResult.Skip;

        ThrowHelper.ThrowIfNull(options);

        var result = _validator.Validate(options);
        return result.Success
            ? ValidateOptionsResult.Success
            : ValidateOptionsResult.Fail(result.Failures.Select(FormatFailure));
    }

    /// <summary>
    /// Renders one <see cref="MustFailure"/> as the line <see cref="ValidateOptionsResult"/> shows in
    /// <see cref="OptionsValidationException.Message"/>.
    /// </summary>
    /// <param name="failure">The failure to render.</param>
    /// <returns>
    /// <c>"{TypeName}.{PropertyPath}: {Message} [{Code}]"</c> when <see cref="MustFailure.PropertyPath"/>
    /// is non-empty; otherwise <c>"{TypeName}: {Message} [{Code}]"</c>, where <c>TypeName</c> is
    /// <c>typeof(TOptions).Name</c>.
    /// </returns>
    internal static string FormatFailure(MustFailure failure)
    {
        var typeName = typeof(TOptions).Name;
        return string.IsNullOrEmpty(failure.PropertyPath)
            ? $"{typeName}: {failure.Message} [{failure.Code}]"
            : $"{typeName}.{failure.PropertyPath}: {failure.Message} [{failure.Code}]";
    }
}
