using System.Linq.Expressions;

namespace PineGuard.MustClauses;

/// <summary>
/// A <see cref="MustValidator{T}"/> configured with lambdas instead of a subclass — for tests, and
/// for adapters (e.g. Options) that build a validator from a configuration delegate.
/// </summary>
/// <typeparam name="T">The type this validator validates.</typeparam>
/// <example>
/// <code>
/// var validator = new InlineMustValidator&lt;CreateOrder&gt;();
/// validator.RuleFor(x => x.Email, email => Must.Be.Email(email));
/// var result = validator.Validate(order);
/// </code>
/// </example>
public sealed class InlineMustValidator<T> : MustValidator<T>
    where T : notnull
{
    /// <inheritdoc cref="MustValidator{T}.RuleFor{TProperty,TResult}(Expression{Func{T,TProperty}},Func{TProperty,MustResult{TResult}})"/>
    public new MustPropertyRule<T, TProperty> RuleFor<TProperty, TResult>(Expression<Func<T, TProperty>> expression, Func<TProperty, MustResult<TResult>> check) =>
        base.RuleFor(expression, check);

    /// <inheritdoc cref="MustValidator{T}.RuleFor{TProperty,TResult}(Expression{Func{T,TProperty}},Func{T,TProperty,MustResult{TResult}})"/>
    public new MustPropertyRule<T, TProperty> RuleFor<TProperty, TResult>(Expression<Func<T, TProperty>> expression, Func<T, TProperty, MustResult<TResult>> check) =>
        base.RuleFor(expression, check);

    /// <inheritdoc cref="MustValidator{T}.RuleFor{TProperty}(Expression{Func{T,TProperty}},IMustValidator{TProperty})"/>
    public new MustPropertyRule<T, TProperty> RuleFor<TProperty>(Expression<Func<T, TProperty?>> expression, IMustValidator<TProperty> validator)
        where TProperty : notnull =>
        base.RuleFor(expression, validator);

    /// <inheritdoc cref="MustValidator{T}.RuleForEach{TItem,TResult}(Expression{Func{T,IEnumerable{TItem}}},Func{TItem,MustResult{TResult}})"/>
    public new MustPropertyRule<T, TItem> RuleForEach<TItem, TResult>(Expression<Func<T, IEnumerable<TItem>?>> expression, Func<TItem, MustResult<TResult>> check) =>
        base.RuleForEach(expression, check);

    /// <inheritdoc cref="MustValidator{T}.RuleForEach{TItem,TResult}(Expression{Func{T,IEnumerable{TItem}}},Func{T,TItem,MustResult{TResult}})"/>
    public new MustPropertyRule<T, TItem> RuleForEach<TItem, TResult>(Expression<Func<T, IEnumerable<TItem>?>> expression, Func<T, TItem, MustResult<TResult>> check) =>
        base.RuleForEach(expression, check);

    /// <inheritdoc cref="MustValidator{T}.RuleForEach{TItem}(Expression{Func{T,IEnumerable{TItem}}},IMustValidator{TItem})"/>
    public new MustPropertyRule<T, TItem> RuleForEach<TItem>(Expression<Func<T, IEnumerable<TItem>?>> expression, IMustValidator<TItem> validator)
        where TItem : notnull =>
        base.RuleForEach(expression, validator);

    /// <inheritdoc cref="MustValidator{T}.RuleForAsync{TProperty,TResult}(Expression{Func{T,TProperty}},Func{TProperty,CancellationToken,ValueTask{MustResult{TResult}}})"/>
    public new MustPropertyRule<T, TProperty> RuleForAsync<TProperty, TResult>(Expression<Func<T, TProperty>> expression, Func<TProperty, CancellationToken, ValueTask<MustResult<TResult>>> check) =>
        base.RuleForAsync(expression, check);

    /// <inheritdoc cref="MustValidator{T}.RuleForAsync{TProperty,TResult}(Expression{Func{T,TProperty}},Func{T,TProperty,CancellationToken,ValueTask{MustResult{TResult}}})"/>
    public new MustPropertyRule<T, TProperty> RuleForAsync<TProperty, TResult>(Expression<Func<T, TProperty>> expression, Func<T, TProperty, CancellationToken, ValueTask<MustResult<TResult>>> check) =>
        base.RuleForAsync(expression, check);

    /// <inheritdoc cref="MustValidator{T}.RuleForEachAsync{TItem,TResult}(Expression{Func{T,IEnumerable{TItem}}},Func{TItem,CancellationToken,ValueTask{MustResult{TResult}}})"/>
    public new MustPropertyRule<T, TItem> RuleForEachAsync<TItem, TResult>(Expression<Func<T, IEnumerable<TItem>?>> expression, Func<TItem, CancellationToken, ValueTask<MustResult<TResult>>> check) =>
        base.RuleForEachAsync(expression, check);

    /// <inheritdoc cref="MustValidator{T}.RuleForEachAsync{TItem,TResult}(Expression{Func{T,IEnumerable{TItem}}},Func{T,TItem,CancellationToken,ValueTask{MustResult{TResult}}})"/>
    public new MustPropertyRule<T, TItem> RuleForEachAsync<TItem, TResult>(Expression<Func<T, IEnumerable<TItem>?>> expression, Func<T, TItem, CancellationToken, ValueTask<MustResult<TResult>>> check) =>
        base.RuleForEachAsync(expression, check);
}
