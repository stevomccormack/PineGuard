using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using PineGuard.Common;
using PineGuard.MustClauses;

namespace PineGuard.Extensions.Options;

/// <summary>
/// Wires an <see cref="IMustValidator{T}"/> into <see cref="OptionsBuilder{TOptions}"/> as an
/// <see cref="IValidateOptions{TOptions}"/>, so <c>Must.Be.*</c> rules validate configuration the same
/// way they validate everything else in the application.
/// </summary>
/// <remarks>
/// Every overload registers a <em>singleton</em> <see cref="IValidateOptions{TOptions}"/> — the same
/// lifetime <c>ValidateDataAnnotations()</c> uses. This is safe because <see cref="MustValidator{T}"/>
/// instances are immutable once constructed (rules are registered only in the constructor), so a single
/// instance can validate every request for the options type without shared mutable state. Registering a
/// validator with a shorter lifetime (for example <c>scoped</c>) and resolving it from the root service
/// provider — which is what <c>ValidateOnStart()</c> and first access to <c>IOptions&lt;TOptions&gt;</c>
/// both do — throws in development, because ASP.NET's scope validation rejects resolving a scoped
/// service from the root provider; that exception is the framework reporting the same lifetime mismatch
/// this remark describes, not a bug in this package.
/// </remarks>
/// <seealso cref="MustRulesValidateOptions{TOptions}"/>
/// <seealso cref="IMustValidator{T}"/>
/// <seealso cref="InlineMustValidator{T}"/>
public static class OptionsBuilderExtension
{
    /// <summary>
    /// Validates <typeparamref name="TOptions"/> using the <see cref="IMustValidator{T}"/> resolved from
    /// the dependency injection container when validation runs.
    /// </summary>
    /// <typeparam name="TOptions">The options type being validated.</typeparam>
    /// <param name="builder">The options builder to extend.</param>
    /// <returns><paramref name="builder"/>, for further chaining.</returns>
    /// <remarks>
    /// Resolving lazily (rather than at registration time) keeps registration order irrelevant: the
    /// validator, and anything it depends on, only needs to be registered before validation first runs.
    /// A missing <c>IMustValidator&lt;TOptions&gt;</c> registration surfaces as the container's own
    /// <see cref="InvalidOperationException"/> at that point — this method does not wrap it.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="builder"/> is <see langword="null"/>.</exception>
    public static OptionsBuilder<TOptions> ValidateMustRules<TOptions>(this OptionsBuilder<TOptions> builder)
        where TOptions : class
    {
        ThrowHelper.ThrowIfNull(builder);

        builder.Services.AddSingleton<IValidateOptions<TOptions>>(services =>
            new MustRulesValidateOptions<TOptions>(builder.Name, services.GetRequiredService<IMustValidator<TOptions>>()));

        return builder;
    }

    /// <summary>
    /// Validates <typeparamref name="TOptions"/> using the given validator instance.
    /// </summary>
    /// <typeparam name="TOptions">The options type being validated.</typeparam>
    /// <param name="builder">The options builder to extend.</param>
    /// <param name="validator">The validator to use.</param>
    /// <returns><paramref name="builder"/>, for further chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="builder"/> or <paramref name="validator"/> is <see langword="null"/>.</exception>
    public static OptionsBuilder<TOptions> ValidateMustRules<TOptions>(this OptionsBuilder<TOptions> builder, IMustValidator<TOptions> validator)
        where TOptions : class
    {
        ThrowHelper.ThrowIfNull(builder);
        ThrowHelper.ThrowIfNull(validator);

        builder.Services.AddSingleton<IValidateOptions<TOptions>>(new MustRulesValidateOptions<TOptions>(builder.Name, validator));

        return builder;
    }

    /// <summary>
    /// Validates <typeparamref name="TOptions"/> using an <see cref="InlineMustValidator{T}"/> built from
    /// <paramref name="configure"/>, for a small options type that does not warrant a dedicated
    /// <see cref="MustValidator{T}"/> subclass.
    /// </summary>
    /// <typeparam name="TOptions">The options type being validated.</typeparam>
    /// <param name="builder">The options builder to extend.</param>
    /// <param name="configure">Called exactly once, at registration time, to declare the rules.</param>
    /// <returns><paramref name="builder"/>, for further chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="builder"/> or <paramref name="configure"/> is <see langword="null"/>.</exception>
    public static OptionsBuilder<TOptions> ValidateMustRules<TOptions>(this OptionsBuilder<TOptions> builder, Action<InlineMustValidator<TOptions>> configure)
        where TOptions : class
    {
        ThrowHelper.ThrowIfNull(builder);
        ThrowHelper.ThrowIfNull(configure);

        var validator = new InlineMustValidator<TOptions>();
        configure(validator);

        return builder.ValidateMustRules(validator);
    }
}
