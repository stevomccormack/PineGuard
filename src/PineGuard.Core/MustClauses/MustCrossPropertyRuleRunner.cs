namespace PineGuard.MustClauses;

/// <summary>
/// Runs a two-argument <c>RuleFor(x =&gt; x.Property, (instance, value) =&gt; Must.Be.X(value, instance.Other))</c>
/// cross-property rule.
/// </summary>
internal sealed class MustCrossPropertyRuleRunner<T, TProperty, TResult>(
    string propertyPath,
    Func<T, TProperty> accessor,
    Func<T, TProperty, MustResult<TResult>> check)
    : MustRuleRunnerBase<T>(propertyPath)
{
    public override IEnumerable<MustFailure> Run(T instance)
    {
        if (!ConditionsPass(instance))
            return [];

        var result = check(instance, accessor(instance));
        return result.Failed ? [BuildFailure(result, PropertyPath)] : [];
    }

    public override ValueTask<IEnumerable<MustFailure>> RunAsync(T instance, CancellationToken cancellationToken) =>
        new(Run(instance));
}
