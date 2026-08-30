namespace PineGuard.MustClauses;

/// <summary>
/// Runs a single-argument <c>RuleForAsync(x =&gt; x.Property, (value, ct) =&gt; Must.Be.XAsync(value, …, ct))</c> rule.
/// </summary>
internal sealed class MustAsyncPropertyRuleRunner<T, TProperty, TResult>(
    string propertyPath,
    Func<T, TProperty> accessor,
    Func<TProperty, CancellationToken, ValueTask<MustResult<TResult>>> check)
    : MustAsyncRuleRunnerBase<T>(propertyPath)
{
    public override async ValueTask<IEnumerable<MustFailure>> RunAsync(T instance, CancellationToken cancellationToken)
    {
        if (!ConditionsPass(instance))
            return [];

        var result = await check(accessor(instance), cancellationToken).ConfigureAwait(false);
        return result.Failed ? [BuildFailure(result, PropertyPath)] : [];
    }
}
