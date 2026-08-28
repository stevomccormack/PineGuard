namespace PineGuard.MustClauses;

/// <summary>
/// Runs a single-argument <c>RuleFor(x =&gt; x.Property, value =&gt; Must.Be.X(value))</c> rule.
/// </summary>
internal sealed class MustPropertyRuleRunner<T, TProperty, TResult>(
    string propertyPath,
    Func<T, TProperty> accessor,
    Func<TProperty, MustResult<TResult>> check)
    : MustRuleRunnerBase<T>(propertyPath)
{
    public override IEnumerable<MustFailure> Run(T instance)
    {
        if (!ConditionsPass(instance))
            return [];

        var result = check(accessor(instance));
        return result.Failed ? [BuildFailure(result, PropertyPath)] : [];
    }

    public override ValueTask<IEnumerable<MustFailure>> RunAsync(T instance, CancellationToken cancellationToken) =>
        new(Run(instance));
}
