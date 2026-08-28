using PineGuard.Common;

namespace PineGuard.MustClauses;

/// <summary>
/// The handle returned by <c>RuleFor</c>/<c>RuleForEach</c>, letting a rule be refined with a
/// condition, a code override, a message override, or a property-path override.
/// </summary>
/// <typeparam name="T">The type the owning <see cref="MustValidator{T}"/> validates.</typeparam>
/// <typeparam name="TProperty">The property (or collection element) type the rule checks.</typeparam>
public sealed class MustPropertyRule<T, TProperty>
{
    private readonly IMustRuleRunner<T> _runner;

    internal MustPropertyRule(IMustRuleRunner<T> runner) => _runner = runner;

    /// <summary>
    /// Gets the property path this rule reports failures under.
    /// </summary>
    public string PropertyPath => _runner.PropertyPath;

    /// <summary>
    /// Runs this rule only when <paramref name="condition"/> returns <see langword="true"/> for the instance.
    /// Multiple conditions (from repeated <see cref="When"/>/<see cref="Unless"/> calls) AND together.
    /// </summary>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="condition"/> is <see langword="null"/>.</exception>
    public MustPropertyRule<T, TProperty> When(Func<T, bool> condition)
    {
        ThrowHelper.ThrowIfNull(condition);

        _runner.AddCondition(condition);
        return this;
    }

    /// <summary>
    /// Runs this rule only when <paramref name="condition"/> returns <see langword="false"/> for the instance.
    /// Multiple conditions (from repeated <see cref="When"/>/<see cref="Unless"/> calls) AND together.
    /// </summary>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="condition"/> is <see langword="null"/>.</exception>
    public MustPropertyRule<T, TProperty> Unless(Func<T, bool> condition)
    {
        ThrowHelper.ThrowIfNull(condition);

        _runner.AddCondition(instance => !condition(instance));
        return this;
    }

    /// <summary>
    /// Overrides the <see cref="MustFailure.Code"/> on every failure this rule emits.
    /// </summary>
    /// <exception cref="ArgumentException">Thrown when <paramref name="code"/> is <see langword="null"/> or empty.</exception>
    public MustPropertyRule<T, TProperty> WithCode(string code)
    {
        ThrowHelper.ThrowIfNullOrWhiteSpace(code);

        _runner.SetCodeOverride(code);
        return this;
    }

    /// <summary>
    /// Overrides the message rendered on every failure this rule emits. May contain <c>{paramName}</c>.
    /// </summary>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="messageTemplate"/> is <see langword="null"/>.</exception>
    public MustPropertyRule<T, TProperty> WithMessage(string messageTemplate)
    {
        ThrowHelper.ThrowIfNull(messageTemplate);

        _runner.SetMessageOverride(messageTemplate);
        return this;
    }

    /// <summary>
    /// Overrides the expression-derived <see cref="PropertyPath"/> (e.g. for a root rule registered via
    /// <c>RuleFor(x =&gt; x, …)</c>, whose expression path is otherwise empty).
    /// </summary>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="propertyPath"/> is <see langword="null"/>.</exception>
    public MustPropertyRule<T, TProperty> WithPropertyPath(string propertyPath)
    {
        ThrowHelper.ThrowIfNull(propertyPath);

        _runner.SetPropertyPathOverride(propertyPath);
        return this;
    }
}
