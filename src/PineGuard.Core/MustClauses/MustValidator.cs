using System.Linq.Expressions;
using PineGuard.Codes;
using PineGuard.Common;
using PineGuard.Utils;

namespace PineGuard.MustClauses;

/// <summary>
/// The base class you derive from to validate a whole <typeparamref name="T"/>: declare rules per
/// property in the constructor via <see cref="RuleFor{TProperty,TResult}(Expression{Func{T,TProperty}},Func{TProperty,MustResult{TResult}})"/>
/// / <c>RuleForEach</c>, then call <see cref="Validate"/>.
/// </summary>
/// <typeparam name="T">The type this validator validates.</typeparam>
/// <remarks>
/// <para>
/// Rules run in registration order and every failure is collected (<see cref="MustValidationMode.Aggregate"/>);
/// pass <see cref="MustValidationMode.StopOnFirstFailure"/> to
/// <see cref="ValidateAsync(T, MustValidationMode, CancellationToken)"/> to stop at the first failing rule.
/// A rule's member path is derived from its expression (<c>x =&gt; x.Email</c> → <c>"Email"</c>) via
/// <see cref="PropertyPathUtility.FromExpression"/>, so a lambda parameter name (<c>e =&gt; Must.Be.Email(e)</c>)
/// never leaks into a failure message — the check's <see cref="MustResult{T}.MessageTemplate"/> is re-rendered
/// against the property path instead.
/// </para>
/// <para>
/// The validator is immutable after construction — <c>RuleFor</c>/<c>RuleForEach</c> are <see langword="protected"/>
/// and constructor-only by convention — so <see cref="Validate"/> is thread-safe and instances are
/// safe to register as DI singletons.
/// </para>
/// </remarks>
/// <seealso cref="IMustValidator{T}"/>
/// <seealso cref="InlineMustValidator{T}"/>
public abstract class MustValidator<T> : IMustValidator<T>
    where T : notnull
{
    private readonly List<IMustRuleRunner<T>> _runners = [];

    /// <summary>
    /// Gets a value indicating whether any registered rule is asynchronous, in which case
    /// <see cref="Validate"/> is unusable and <see cref="ValidateAsync(T, CancellationToken)"/> must be called instead.
    /// </summary>
    protected bool HasAsyncRules { get; private set; }

    /// <summary>
    /// Registers a rule that checks one property in isolation.
    /// </summary>
    /// <typeparam name="TProperty">The property type.</typeparam>
    /// <typeparam name="TResult">The result type of <paramref name="check"/>.</typeparam>
    /// <param name="expression">A member-access expression identifying the property (e.g. <c>x =&gt; x.Email</c>).</param>
    /// <param name="check">The Must check to run against the property value.</param>
    protected MustPropertyRule<T, TProperty> RuleFor<TProperty, TResult>(Expression<Func<T, TProperty>> expression, Func<TProperty, MustResult<TResult>> check)
    {
        ThrowHelper.ThrowIfNull(expression);
        ThrowHelper.ThrowIfNull(check);

        var runner = new MustPropertyRuleRunner<T, TProperty, TResult>(PropertyPathUtility.FromExpression(expression), expression.Compile(), check);
        _runners.Add(runner);
        return new MustPropertyRule<T, TProperty>(runner);
    }

    /// <summary>
    /// Registers a cross-property rule: the check also receives the whole instance, so it can compare
    /// the property against another property on the same object.
    /// </summary>
    /// <typeparam name="TProperty">The property type.</typeparam>
    /// <typeparam name="TResult">The result type of <paramref name="check"/>.</typeparam>
    /// <param name="expression">A member-access expression identifying the property being attributed.</param>
    /// <param name="check">The Must check, receiving <c>(instance, propertyValue)</c>.</param>
    protected MustPropertyRule<T, TProperty> RuleFor<TProperty, TResult>(Expression<Func<T, TProperty>> expression, Func<T, TProperty, MustResult<TResult>> check)
    {
        ThrowHelper.ThrowIfNull(expression);
        ThrowHelper.ThrowIfNull(check);

        var runner = new MustCrossPropertyRuleRunner<T, TProperty, TResult>(PropertyPathUtility.FromExpression(expression), expression.Compile(), check);
        _runners.Add(runner);
        return new MustPropertyRule<T, TProperty>(runner);
    }

    /// <summary>
    /// Registers a nested-validator rule: <paramref name="validator"/> validates the property, and its
    /// result is re-rooted under the property path. Skipped when the property is <see langword="null"/>
    /// — presence is a separate rule.
    /// </summary>
    /// <typeparam name="TProperty">The property type. The property itself may be nullable; <paramref name="validator"/> is typed for its non-null form.</typeparam>
    /// <param name="expression">A member-access expression identifying the property.</param>
    /// <param name="validator">The validator to run against the property value.</param>
    protected MustPropertyRule<T, TProperty> RuleFor<TProperty>(Expression<Func<T, TProperty?>> expression, IMustValidator<TProperty> validator)
        where TProperty : notnull
    {
        ThrowHelper.ThrowIfNull(expression);
        ThrowHelper.ThrowIfNull(validator);

        var runner = new MustNestedValidatorRuleRunner<T, TProperty>(PropertyPathUtility.FromExpression(expression), expression.Compile(), validator);
        _runners.Add(runner);
        return new MustPropertyRule<T, TProperty>(runner);
    }

    /// <summary>
    /// Registers a rule that checks every element of a collection property in isolation, reporting
    /// failures at <c>Property[i]</c>. Skips a <see langword="null"/> collection; enumerates once.
    /// </summary>
    /// <typeparam name="TItem">The collection element type.</typeparam>
    /// <typeparam name="TResult">The result type of <paramref name="check"/>.</typeparam>
    /// <param name="expression">A member-access expression identifying the collection property.</param>
    /// <param name="check">The Must check to run against each element.</param>
    protected MustPropertyRule<T, TItem> RuleForEach<TItem, TResult>(Expression<Func<T, IEnumerable<TItem>?>> expression, Func<TItem, MustResult<TResult>> check)
    {
        ThrowHelper.ThrowIfNull(expression);
        ThrowHelper.ThrowIfNull(check);

        var runner = new MustCollectionRuleRunner<T, TItem, TResult>(PropertyPathUtility.FromExpression(expression), expression.Compile(), check);
        _runners.Add(runner);
        return new MustPropertyRule<T, TItem>(runner);
    }

    /// <summary>
    /// Registers a cross-property rule that checks every element of a collection property, reporting
    /// failures at <c>Property[i]</c>. Skips a <see langword="null"/> collection; enumerates once.
    /// </summary>
    /// <typeparam name="TItem">The collection element type.</typeparam>
    /// <typeparam name="TResult">The result type of <paramref name="check"/>.</typeparam>
    /// <param name="expression">A member-access expression identifying the collection property.</param>
    /// <param name="check">The Must check, receiving <c>(instance, item)</c>.</param>
    protected MustPropertyRule<T, TItem> RuleForEach<TItem, TResult>(Expression<Func<T, IEnumerable<TItem>?>> expression, Func<T, TItem, MustResult<TResult>> check)
    {
        ThrowHelper.ThrowIfNull(expression);
        ThrowHelper.ThrowIfNull(check);

        var runner = new MustCollectionCrossPropertyRuleRunner<T, TItem, TResult>(PropertyPathUtility.FromExpression(expression), expression.Compile(), check);
        _runners.Add(runner);
        return new MustPropertyRule<T, TItem>(runner);
    }

    /// <summary>
    /// Registers a nested-validator rule that validates every element of a collection property,
    /// re-rooting each element's result under <c>Property[i]</c>. Skips a <see langword="null"/>
    /// collection and any <see langword="null"/> element; enumerates once.
    /// </summary>
    /// <typeparam name="TItem">The collection element type.</typeparam>
    /// <param name="expression">A member-access expression identifying the collection property.</param>
    /// <param name="validator">The validator to run against each element.</param>
    protected MustPropertyRule<T, TItem> RuleForEach<TItem>(Expression<Func<T, IEnumerable<TItem>?>> expression, IMustValidator<TItem> validator)
        where TItem : notnull
    {
        ThrowHelper.ThrowIfNull(expression);
        ThrowHelper.ThrowIfNull(validator);

        var runner = new MustCollectionValidatorRuleRunner<T, TItem>(PropertyPathUtility.FromExpression(expression), expression.Compile(), validator);
        _runners.Add(runner);
        return new MustPropertyRule<T, TItem>(runner);
    }

    /// <summary>
    /// Registers an asynchronous rule that checks one property in isolation.
    /// </summary>
    /// <typeparam name="TProperty">The property type.</typeparam>
    /// <typeparam name="TResult">The result type of <paramref name="check"/>.</typeparam>
    /// <param name="expression">A member-access expression identifying the property (e.g. <c>x =&gt; x.Email</c>).</param>
    /// <param name="check">The asynchronous Must check to run against the property value.</param>
    protected MustPropertyRule<T, TProperty> RuleForAsync<TProperty, TResult>(Expression<Func<T, TProperty>> expression, Func<TProperty, CancellationToken, ValueTask<MustResult<TResult>>> check)
    {
        ThrowHelper.ThrowIfNull(expression);
        ThrowHelper.ThrowIfNull(check);

        return AddAsyncRule<TProperty>(new MustAsyncPropertyRuleRunner<T, TProperty, TResult>(PropertyPathUtility.FromExpression(expression), expression.Compile(), check));
    }

    /// <summary>
    /// Registers an asynchronous cross-property rule: the check also receives the whole instance, so it
    /// can compare the property against another property on the same object.
    /// </summary>
    /// <typeparam name="TProperty">The property type.</typeparam>
    /// <typeparam name="TResult">The result type of <paramref name="check"/>.</typeparam>
    /// <param name="expression">A member-access expression identifying the property being attributed.</param>
    /// <param name="check">The asynchronous Must check, receiving <c>(instance, propertyValue, cancellationToken)</c>.</param>
    protected MustPropertyRule<T, TProperty> RuleForAsync<TProperty, TResult>(Expression<Func<T, TProperty>> expression, Func<T, TProperty, CancellationToken, ValueTask<MustResult<TResult>>> check)
    {
        ThrowHelper.ThrowIfNull(expression);
        ThrowHelper.ThrowIfNull(check);

        return AddAsyncRule<TProperty>(new MustAsyncCrossPropertyRuleRunner<T, TProperty, TResult>(PropertyPathUtility.FromExpression(expression), expression.Compile(), check));
    }

    /// <summary>
    /// Registers an asynchronous rule that checks every element of a collection property in isolation,
    /// reporting failures at <c>Property[i]</c>. Skips a <see langword="null"/> collection; enumerates once.
    /// </summary>
    /// <typeparam name="TItem">The collection element type.</typeparam>
    /// <typeparam name="TResult">The result type of <paramref name="check"/>.</typeparam>
    /// <param name="expression">A member-access expression identifying the collection property.</param>
    /// <param name="check">The asynchronous Must check to run against each element.</param>
    protected MustPropertyRule<T, TItem> RuleForEachAsync<TItem, TResult>(Expression<Func<T, IEnumerable<TItem>?>> expression, Func<TItem, CancellationToken, ValueTask<MustResult<TResult>>> check)
    {
        ThrowHelper.ThrowIfNull(expression);
        ThrowHelper.ThrowIfNull(check);

        return AddAsyncRule<TItem>(new MustAsyncCollectionRuleRunner<T, TItem, TResult>(PropertyPathUtility.FromExpression(expression), expression.Compile(), check));
    }

    /// <summary>
    /// Registers an asynchronous cross-property rule that checks every element of a collection property,
    /// reporting failures at <c>Property[i]</c>. Skips a <see langword="null"/> collection; enumerates once.
    /// </summary>
    /// <typeparam name="TItem">The collection element type.</typeparam>
    /// <typeparam name="TResult">The result type of <paramref name="check"/>.</typeparam>
    /// <param name="expression">A member-access expression identifying the collection property.</param>
    /// <param name="check">The asynchronous Must check, receiving <c>(instance, item, cancellationToken)</c>.</param>
    protected MustPropertyRule<T, TItem> RuleForEachAsync<TItem, TResult>(Expression<Func<T, IEnumerable<TItem>?>> expression, Func<T, TItem, CancellationToken, ValueTask<MustResult<TResult>>> check)
    {
        ThrowHelper.ThrowIfNull(expression);
        ThrowHelper.ThrowIfNull(check);

        return AddAsyncRule<TItem>(new MustAsyncCollectionCrossPropertyRuleRunner<T, TItem, TResult>(PropertyPathUtility.FromExpression(expression), expression.Compile(), check));
    }

    /// <summary>
    /// Validates <paramref name="value"/> against every registered rule.
    /// </summary>
    /// <param name="value">The instance to validate. Never throws when <see langword="null"/>.</param>
    /// <returns>
    /// A single failure at the root (<see cref="MustCodes.Value.State.Null"/>) when <paramref name="value"/> is
    /// <see langword="null"/>; otherwise every failure from every rule, in registration order.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when <see cref="HasAsyncRules"/> is <see langword="true"/> — an async rule has no synchronous
    /// form, so the validator can only answer through <see cref="ValidateAsync(T, CancellationToken)"/>.
    /// </exception>
    public MustValidationResult Validate(T value)
    {
        if (HasAsyncRules)
            throw MustAsyncRuleRunnerBase<T>.AsyncRulesRequireValidateAsync();

        return value is null ? FailNull() : RunRules(value);
    }

    /// <summary>
    /// Validates <paramref name="value"/> against every registered rule, asynchronously, collecting
    /// every failure (<see cref="MustValidationMode.Aggregate"/>).
    /// </summary>
    /// <param name="value">The instance to validate. Never throws when <see langword="null"/>.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    public virtual ValueTask<MustValidationResult> ValidateAsync(T value, CancellationToken cancellationToken = default) =>
        ValidateAsync(value, MustValidationMode.Aggregate, cancellationToken);

    /// <summary>
    /// Validates <paramref name="value"/> against every registered rule, asynchronously, stopping as
    /// <paramref name="mode"/> dictates.
    /// </summary>
    /// <param name="value">The instance to validate. Never throws when <see langword="null"/>.</param>
    /// <param name="mode">Whether to collect every failure or stop at the first rule that fails.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <remarks>
    /// Rules run sequentially in registration order — never in parallel — so the failure order is
    /// deterministic and <see cref="MustValidationMode.StopOnFirstFailure"/> stops at a well-defined rule.
    /// The token is observed before each rule.
    /// </remarks>
    public virtual async ValueTask<MustValidationResult> ValidateAsync(T value, MustValidationMode mode, CancellationToken cancellationToken = default)
    {
        if (value is null)
            return FailNull();

        var failures = new List<MustFailure>();
        foreach (var runner in _runners)
        {
            cancellationToken.ThrowIfCancellationRequested();
            failures.AddRange(await runner.RunAsync(value, cancellationToken).ConfigureAwait(false));

            if (mode == MustValidationMode.StopOnFirstFailure && failures.Count > 0)
                break;
        }

        return failures.Count == 0 ? MustValidationResult.Ok() : MustValidationResult.Fail(failures);
    }

    private MustPropertyRule<T, TProperty> AddAsyncRule<TProperty>(IMustRuleRunner<T> runner)
    {
        HasAsyncRules = true;
        _runners.Add(runner);
        return new MustPropertyRule<T, TProperty>(runner);
    }

    private MustValidationResult RunRules(T value)
    {
        var failures = new List<MustFailure>();
        foreach (var runner in _runners)
            failures.AddRange(runner.Run(value));

        return failures.Count == 0 ? MustValidationResult.Ok() : MustValidationResult.Fail(failures);
    }

    private static MustValidationResult FailNull() =>
        MustValidationResult.Fail(new MustFailure(string.Empty, MustCodes.Value.State.Null, $"{typeof(T).Name} must not be null.", null));

    Type IMustValidator.ValidatedType => typeof(T);

    MustValidationResult IMustValidator.Validate(object? value) => Validate(MustValidatorCast.To<T>(value));

    ValueTask<MustValidationResult> IMustValidator.ValidateAsync(object? value, CancellationToken cancellationToken) =>
        ValidateAsync(MustValidatorCast.To<T>(value), cancellationToken);

    ValueTask<MustValidationResult> IMustValidator.ValidateAsync(object? value, MustValidationMode mode, CancellationToken cancellationToken) =>
        ValidateAsync(MustValidatorCast.To<T>(value), mode, cancellationToken);
}
