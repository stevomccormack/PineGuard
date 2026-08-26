namespace PineGuard.MustClauses;

/// <summary>
/// One registered <c>RuleFor</c> or <c>RuleForEach</c> rule, run against a validated instance to
/// produce zero or more <see cref="MustFailure"/>s.
/// </summary>
/// <typeparam name="T">The type the owning <see cref="MustValidator{T}"/> validates.</typeparam>
internal interface IMustRuleRunner<T>
{
    /// <summary>
    /// The expression-derived (or <see cref="SetPropertyPathOverride"/>-overridden) property path this rule reports failures under.
    /// </summary>
    string PropertyPath { get; }

    /// <summary>
    /// Registers a condition that must pass (alongside every other registered condition) before this rule runs.
    /// </summary>
    void AddCondition(Func<T, bool> condition);

    /// <summary>
    /// Overrides the <see cref="MustFailure.Code"/> emitted by every failure this rule produces.
    /// </summary>
    void SetCodeOverride(string code);

    /// <summary>
    /// Overrides the message template rendered into every failure this rule produces.
    /// </summary>
    void SetMessageOverride(string messageTemplate);

    /// <summary>
    /// Overrides <see cref="PropertyPath"/>.
    /// </summary>
    void SetPropertyPathOverride(string propertyPath);

    /// <summary>
    /// Runs this rule against <paramref name="instance"/>.
    /// </summary>
    IEnumerable<MustFailure> Run(T instance);

    /// <summary>
    /// Runs this rule against <paramref name="instance"/> asynchronously.
    /// </summary>
    ValueTask<IEnumerable<MustFailure>> RunAsync(T instance, CancellationToken cancellationToken);
}
