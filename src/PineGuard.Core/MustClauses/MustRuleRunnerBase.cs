namespace PineGuard.MustClauses;

/// <summary>
/// Shared condition and override machinery for every <see cref="IMustRuleRunner{T}"/> implementation.
/// </summary>
/// <typeparam name="T">The type the owning <see cref="MustValidator{T}"/> validates.</typeparam>
internal abstract class MustRuleRunnerBase<T>(string propertyPath) : IMustRuleRunner<T>
{
    private readonly List<Func<T, bool>> _conditions = [];
    private string? _codeOverride;
    private string? _messageOverride;

    public string PropertyPath { get; private set; } = propertyPath;

    public void AddCondition(Func<T, bool> condition) => _conditions.Add(condition);

    public void SetCodeOverride(string code) => _codeOverride = code;

    public void SetMessageOverride(string messageTemplate) => _messageOverride = messageTemplate;

    public void SetPropertyPathOverride(string propertyPath) => PropertyPath = propertyPath;

    public abstract IEnumerable<MustFailure> Run(T instance);

    public abstract ValueTask<IEnumerable<MustFailure>> RunAsync(T instance, CancellationToken cancellationToken);

    /// <summary>
    /// Evaluates every registered condition against <paramref name="instance"/>, AND-ing them together.
    /// </summary>
    protected bool ConditionsPass(T instance)
    {
        foreach (var condition in _conditions)
            if (!condition(instance))
                return false;

        return true;
    }

    /// <summary>
    /// Builds a <see cref="MustFailure"/> from a failed <see cref="IMustResult"/>, applying any
    /// <see cref="SetCodeOverride"/>/<see cref="SetMessageOverride"/> override.
    /// </summary>
    protected MustFailure BuildFailure(IMustResult result, string propertyPath)
    {
        var failure = MustFailure.From(result, propertyPath);

        if (_codeOverride is not null)
            failure = failure with { Code = _codeOverride };

        if (_messageOverride is not null)
            failure = failure with { Message = MustMessage.Format(_messageOverride, propertyPath) };

        return failure;
    }
}
